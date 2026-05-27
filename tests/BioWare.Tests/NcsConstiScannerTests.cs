using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.NCS;
using BioWare.Tools;
using NUnit.Framework;

namespace BioWare.Tests
{
    [TestFixture]
    public class NcsConstiScannerTests
    {
        [Test]
        public void ExtractConstiInstructions_CompiledNss_FindsLiteral()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss("void main() { int n = " + targetStrRef + "; }", BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);

            Assert.That(instructions, Has.Some.Matches<NcsConstiScanner.ConstiInstruction>(i => i.Value == targetStrRef));
        }

        [Test]
        public void StrRefReferenceCache_NcsLiteral_IsFound()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss("void main() { int n = " + targetStrRef + "; }", BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(targetStrRef), Is.True);
            }
            finally
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
            }
        }

        [Test]
        public void FindStrRefReferences_OverrideNcs_FindsConstiLiteral()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ncs-find-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss("void main() { int n = " + targetStrRef + "; }", BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_script.ncs"), bytes);

            try
            {
                var installation = new Installation(installRoot);
                List<StrRefSearchResult> results = ReferenceCacheHelpers.FindStrRefReferences(
                    installation,
                    targetStrRef,
                    null,
                    null);

                Assert.That(results, Is.Not.Empty);
                List<ReferenceSearchResult> converted = ReferenceCacheHelpers.ConvertToReferenceSearchResults(
                    results,
                    targetStrRef);
                Assert.That(converted, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath.StartsWith("(NCS bytecode) offset_")));
            }
            finally
            {
                try
                {
                    Directory.Delete(installRoot, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }
    }
}
