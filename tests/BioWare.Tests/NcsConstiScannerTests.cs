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
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + "); }",
                BioWareGame.K1);
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

        [Test]
        public void StrRefReferenceCache_CustomMinimum_IndexesSmallConstiWhenMinZero()
        {
            const int smallLiteral = 50;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + smallLiteral + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1, 0);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(smallLiteral), Is.True);
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
        public void IsPlausibleStrRefCandidate_CustomMinimum_RespectsThreshold()
        {
            Assert.That(NcsConstiScanner.IsPlausibleStrRefCandidate(50, 100), Is.False);
            Assert.That(NcsConstiScanner.IsPlausibleStrRefCandidate(50, 0), Is.True);
        }

        [Test]
        public void StrRefReferenceCache_SmallConsti_IsNotIndexed()
        {
            const int smallLiteral = 5;
            NCS ncs = NCSAuto.CompileNss("void main() { int n = " + smallLiteral + "; }", BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(smallLiteral), Is.False);
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
        public void FindStrRefReferences_SmallConstiSlowPath_StillFindsLiteral()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ncs-find-small-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            const int smallLiteral = 5;
            NCS ncs = NCSAuto.CompileNss("void main() { int n = " + smallLiteral + "; }", BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_script.ncs"), bytes);

            try
            {
                var installation = new Installation(installRoot);
                List<StrRefSearchResult> results = ReferenceCacheHelpers.FindStrRefReferences(
                    installation,
                    smallLiteral,
                    null,
                    null);

                Assert.That(results, Is.Not.Empty);
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

        [Test]
        public void FindStrRefReferences_IncludeNcsStrRefScanFalse_SkipsOverrideNcs()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ncs-find-skip-" + Guid.NewGuid().ToString("N"));
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
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = true,
                    SearchChitin = false,
                    SearchModules = false,
                    IncludeNcsStrRefScan = false
                };

                List<StrRefSearchResult> results = ReferenceCacheHelpers.FindStrRefReferences(
                    installation,
                    targetStrRef,
                    null,
                    null,
                    options);

                Assert.That(results, Is.Empty);
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

        [Test]
        public void FindAllStrRefReferences_IncludeNcsStrRefScanFalse_SkipsOverrideNcs()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ncs-batch-skip-" + Guid.NewGuid().ToString("N"));
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
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = true,
                    SearchChitin = false,
                    SearchModules = false,
                    IncludeNcsStrRefScan = false
                };

                (Dictionary<int, List<StrRefSearchResult>> batchResults, StrRefReferenceCache cache) =
                    ReferenceCacheHelpers.FindAllStrRefReferences(
                        installation,
                        new List<int> { targetStrRef },
                        null,
                        null,
                        options);

                Assert.That(batchResults[targetStrRef], Is.Empty);
                Assert.That(cache.HasReferences(targetStrRef), Is.False);
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

        [Test]
        public void IsPlausibleStrRefCandidate_UsesMinimumThreshold()
        {
            Assert.That(NcsConstiScanner.IsPlausibleStrRefCandidate(99), Is.False);
            Assert.That(NcsConstiScanner.IsPlausibleStrRefCandidate(100), Is.True);
            Assert.That(NcsConstiScanner.IsPlausibleStrRefCandidate(424242), Is.True);
        }

        [Test]
        public void GetConstiUsageContext_StrRefAction_ReturnsStrRefConsumer()
        {
            const int smallStrRef = 50;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + smallStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == smallStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.True);
        }

        [Test]
        public void StrRefReferenceCache_StrRefActionSmallLiteral_IsIndexed()
        {
            const int smallStrRef = 50;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + smallStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-strref-action-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(smallStrRef), Is.True);
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
        public void GetConstiUsageContext_ComparisonLiteral_ReturnsGenericInteger()
        {
            const int bound = 150;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { if (0 < " + bound + ") {} }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = default(NcsConstiScanner.ConstiInstruction);
            bool found = false;
            for (int i = 0; i < instructions.Count; i++)
            {
                if (instructions[i].Value == bound)
                {
                    match = instructions[i];
                    found = true;
                    break;
                }
            }

            Assert.That(found, Is.True, "Expected CONSTI literal " + bound);
            int nextOffset = match.ValueByteOffset + 4;
            byte nextOpcode = bytes[nextOffset];
            byte nextQualifier = bytes[nextOffset + 1];
            Assert.That(
                NcsConstiScanner.GetConstiUsageContext(bytes, match),
                Is.EqualTo(NcsConstiScanner.ConstiUsageContext.GenericInteger),
                "Next opcode 0x" + nextOpcode.ToString("X2") + " qualifier 0x" + nextQualifier.ToString("X2"));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.False);
        }

        [Test]
        public void StrRefReferenceCache_ComparisonLiteral_IsNotIndexed()
        {
            const int bound = 150;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { if (0 < " + bound + ") {} }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-loop-bound-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(bound), Is.False);
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
        public void GetConstiUsageContext_ArithmeticAddStrRefLiteral_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + " + " + offset + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_ArithmeticSubStrRefLiteral_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + " - 1); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticAddStrRefLiteral_IsIndexed()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + offset + " + " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticMulStrRefLiteral_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + " * 1); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticMulStrRefLiteral_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + " * 1); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-mul-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticModStrRefLiteral_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + " % 1000000); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticModStrRefLiteral_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + " % 1000000); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-mod-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticDivStrRefLiteral_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + " / 1); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticDivStrRefLiteral_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + " / 1); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-div-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ChainedArithmeticAddStrRefLiteral_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + " + " + offset + " + 0); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ChainedArithmeticAddStrRefLiteral_IsIndexed()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + targetStrRef + " + " + offset + " + 0); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-chain-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " + " + offset + "; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticLocalStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " + " + offset + "; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-local-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticLocalSubStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " - 1; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticLocalSubStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " - 1; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-local-sub-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticLocalMulStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " * 1; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticLocalMulStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " * 1; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-local-mul-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticLocalModStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " % 1000000; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticLocalModStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " % 1000000; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-local-mod-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticLocalDivStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " / 1; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticLocalDivStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " / 1; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-local-div-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticLocalChainedAddStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " + " + offset + " + 0; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticLocalChainedAddStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " + " + offset + " + 0; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-local-chain-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticMultiHopLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + "; int m = n + 0; ActionSpeakStringByStrRef(m); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticMultiHopLocalStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + "; int m = n + 0; ActionSpeakStringByStrRef(m); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-multihop-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ArithmeticFirstMultiHopLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " + " + offset + "; int m = n; ActionSpeakStringByStrRef(m); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ArithmeticFirstMultiHopLocalStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " + " + offset + "; int m = n; ActionSpeakStringByStrRef(m); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-arith-first-multihop-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_CombinedArithmeticMultiHopLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " + " + offset + "; int m = n + 0; ActionSpeakStringByStrRef(m); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_CombinedArithmeticMultiHopLocalStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            const int offset = 100;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + " + " + offset + "; int m = n + 0; ActionSpeakStringByStrRef(m); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-combined-arith-multihop-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_BarkStringSecondArg_ReturnsStrRefConsumer()
        {
            const int smallStrRef = 50;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { BarkString(OBJECT_SELF, " + smallStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == smallStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_BarkStringSmallLiteral_IsIndexed()
        {
            const int smallStrRef = 50;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { BarkString(OBJECT_SELF, " + smallStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-bark-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(smallStrRef), Is.True);
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
        public void GetConstiUsageContext_SpeakByStrRefVolumeArg_IsNotStrRefConsumer()
        {
            const int smallStrRef = 50;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { ActionSpeakStringByStrRef(" + smallStrRef + ", TALKVOLUME_SHOUT); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction volume = instructions.Find(i => i.Value == 2);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, volume), Is.Not.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, volume, NcsConstiScanner.StrRefCandidateMinimum), Is.False);
        }

        [Test]
        public void GetConstiUsageContext_LocalIntStore_ReturnsStackStored()
        {
            const int largeLiteral = 424242;
            NCS ncs = NCSAuto.CompileNss("void main() { int n = " + largeLiteral + "; }", BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == largeLiteral);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StackStored));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.False);
        }

        [Test]
        public void StrRefReferenceCache_LocalIntStore_IsNotIndexed()
        {
            const int largeLiteral = 424242;
            NCS ncs = NCSAuto.CompileNss("void main() { int n = " + largeLiteral + "; }", BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-local-int-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(largeLiteral), Is.False);
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
        public void FindStrRefReferences_LocalIntStoreSlowPath_StillFindsLiteral()
        {
            const int largeLiteral = 424242;
            NCS ncs = NCSAuto.CompileNss("void main() { int n = " + largeLiteral + "; }", BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string installRoot = Path.Combine(Path.GetTempPath(), "ncs-local-find-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_script.ncs"), bytes);

            try
            {
                var installation = new Installation(installRoot);
                List<StrRefSearchResult> results = ReferenceCacheHelpers.FindStrRefReferences(
                    installation,
                    largeLiteral,
                    null,
                    null);

                Assert.That(results, Is.Not.Empty);
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

        [Test]
        public void GetConstiUsageContext_VariableStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + "; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.True);
        }

        [Test]
        public void StrRefReferenceCache_VariableStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + "; ActionSpeakStringByStrRef(n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-var-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_VariableStrRefBarkSecondArg_ReturnsStrRefConsumer()
        {
            const int smallStrRef = 50;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + smallStrRef + "; BarkString(OBJECT_SELF, n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == smallStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_VariableStrRefBarkSecondArg_IsIndexed()
        {
            const int smallStrRef = 50;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + smallStrRef + "; BarkString(OBJECT_SELF, n); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-var-bark-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(smallStrRef), Is.True);
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
        public void GetConstiUsageContext_VariableIntUnusedStore_RemainsStackStored()
        {
            const int largeLiteral = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + largeLiteral + "; int m = n + 1; }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == largeLiteral);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StackStored));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.False);
        }

        [Test]
        public void GetConstiUsageContext_MultiHopLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + "; int m = n; ActionSpeakStringByStrRef(m); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.True);
        }

        [Test]
        public void StrRefReferenceCache_MultiHopLocalStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + "; int m = n; ActionSpeakStringByStrRef(m); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-multihop-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ThreeHopLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + "; int m = n; int k = m; ActionSpeakStringByStrRef(k); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.True);
        }

        [Test]
        public void StrRefReferenceCache_ThreeHopLocalStrRefViaCptopsp_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() { int n = " + targetStrRef + "; int m = n; int k = m; ActionSpeakStringByStrRef(k); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-threehop-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_GlobalStrRefViaCptopbp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "int g_nStrRef;\nvoid main() {\n    g_nStrRef = " + targetStrRef + ";\n    ActionSpeakStringByStrRef(g_nStrRef);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.True);
        }

        [Test]
        public void StrRefReferenceCache_GlobalStrRefViaCptopbp_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "int g_nStrRef;\nvoid main() {\n    g_nStrRef = " + targetStrRef + ";\n    ActionSpeakStringByStrRef(g_nStrRef);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-global-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_GlobalStrRefCrossSubroutine_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "int g;\nvoid sub1() {\n    ActionSpeakStringByStrRef(g);\n}\nvoid main() {\n    g = " + targetStrRef + ";\n    sub1();\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.True);
        }

        [Test]
        public void StrRefReferenceCache_GlobalStrRefCrossSubroutine_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "int g;\nvoid sub1() {\n    ActionSpeakStringByStrRef(g);\n}\nvoid main() {\n    g = " + targetStrRef + ";\n    sub1();\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-global-cross-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_GlobalBpMultiHopLocalStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "int g;\nvoid main() {\n    g = " + targetStrRef + ";\n    int m = g;\n    ActionSpeakStringByStrRef(m);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.True);
        }

        [Test]
        public void StrRefReferenceCache_GlobalBpMultiHopLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "int g;\nvoid main() {\n    g = " + targetStrRef + ";\n    int m = g;\n    ActionSpeakStringByStrRef(m);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-bp-multihop-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_GlobalBpCrossSubThreeHopLocalStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "int g;\nvoid sub1() {\n    int m = g;\n    int k = m;\n    ActionSpeakStringByStrRef(k);\n}\nvoid main() {\n    g = " + targetStrRef + ";\n    sub1();\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
            Assert.That(NcsConstiScanner.ShouldIndexAsStrRefCandidate(bytes, match, NcsConstiScanner.StrRefCandidateMinimum), Is.True);
        }

        [Test]
        public void GetConstiUsageContext_EarlyReturnLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (0) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_WhileZeroReturnLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    while (0) { return; }\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_DeadReturnLocalStrRef_RemainsStackStored()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (1) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StackStored));
        }

        [Test]
        public void GetConstiUsageContext_VariableConditionZeroReturnLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    int x = 0;\n    if (x) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_VariableConditionOneReturnLocalStrRef_RemainsStackStored()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    int x = 1;\n    if (x) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StackStored));
        }

        [Test]
        public void GetConstiUsageContext_DeadIfBranchLocalStrRef_RemainsStackStored()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (0) { ActionSpeakStringByStrRef(n); }\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StackStored));
        }

        [Test]
        public void StrRefReferenceCache_DeadReturnLocalStrRef_IsNotIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (1) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-deadret-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(targetStrRef), Is.False);
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
        public void StrRefReferenceCache_DeadIfBranchLocalStrRef_IsNotIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (0) { ActionSpeakStringByStrRef(n); }\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-deadif-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(targetStrRef), Is.False);
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
        public void StrRefReferenceCache_DeadForBodyLocalStrRef_IsNotIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    int i;\n    for (i = 0; i < 0; i++) { ActionSpeakStringByStrRef(n); }\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-deadfor-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(targetStrRef), Is.False);
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
        public void StrRefReferenceCache_DeadWhileBodyLocalStrRef_IsNotIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    while (0) { ActionSpeakStringByStrRef(n); }\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-deadwhile-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(targetStrRef), Is.False);
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
        public void StrRefReferenceCache_DoWhileBreakLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    do { break; } while (1);\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-dowhile-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void StrRefReferenceCache_NestedDeadIfReturnLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (0) { if (0) return; }\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-nestedif-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void StrRefReferenceCache_EarlyReturnLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (0) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-earlyret-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void StrRefReferenceCache_WhileBreakLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    while (1) { break; }\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-whilebreak-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void StrRefReferenceCache_ElseBranchLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (0) { ActionSpeakStringByStrRef(1); } else { ActionSpeakStringByStrRef(n); }\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-elsebranch-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void StrRefReferenceCache_IfOneLiveBranchLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (1) { ActionSpeakStringByStrRef(n); }\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-ifone-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void StrRefReferenceCache_VariableConditionZeroReturnLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    int x = 0;\n    if (x) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-varzero-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void StrRefReferenceCache_VariableConditionOneReturnLocalStrRef_IsNotIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    int x = 1;\n    if (x) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-varone-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(targetStrRef), Is.False);
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
        public void GetConstiUsageContext_SubroutineEarlyReturnLocalStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void sub1() {\n    int n = " + targetStrRef + ";\n    if (0) return;\n    ActionSpeakStringByStrRef(n);\n}\nvoid main() { sub1(); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_SubroutineEarlyReturnLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void sub1() {\n    int n = " + targetStrRef + ";\n    if (0) return;\n    ActionSpeakStringByStrRef(n);\n}\nvoid main() { sub1(); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-subret-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_SubroutineDeadReturnLocalStrRef_RemainsStackStored()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void sub1() {\n    int n = " + targetStrRef + ";\n    if (1) return;\n    ActionSpeakStringByStrRef(n);\n}\nvoid main() { sub1(); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StackStored));
        }

        [Test]
        public void StrRefReferenceCache_SubroutineDeadReturnLocalStrRef_IsNotIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void sub1() {\n    int n = " + targetStrRef + ";\n    if (1) return;\n    ActionSpeakStringByStrRef(n);\n}\nvoid main() { sub1(); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-subdeadret-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(targetStrRef), Is.False);
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
        public void GetConstiUsageContext_SubroutineInfiniteLoopLocalStrRef_RemainsStackStored()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void sub1() {\n    int n = " + targetStrRef + ";\n    while (1) { if (0) return; }\n    ActionSpeakStringByStrRef(n);\n}\nvoid main() { sub1(); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StackStored));
        }

        [Test]
        public void StrRefReferenceCache_SubroutineInfiniteLoopLocalStrRef_IsNotIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void sub1() {\n    int n = " + targetStrRef + ";\n    while (1) { if (0) return; }\n    ActionSpeakStringByStrRef(n);\n}\nvoid main() { sub1(); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-subinfl-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(targetStrRef), Is.False);
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
        public void StrRefReferenceCache_WhileZeroReturnLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    while (0) { return; }\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-whilezeroret-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void StrRefReferenceCache_WhileOneDeadIfReturnLocalStrRef_IsNotIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    while (1) { if (0) return; }\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-whileone-dead-" + Guid.NewGuid().ToString("N") + ".ncs");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences(targetStrRef), Is.False);
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
        public void FindStrRefReferences_DeadReturnSlowPath_StillFindsLiteral()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (1) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string installRoot = Path.Combine(Path.GetTempPath(), "ncs-deadret-slow-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);
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

        [Test]
        public void FindStrRefReferences_DeadReturnCachePath_IsEmpty()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (1) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string installRoot = Path.Combine(Path.GetTempPath(), "ncs-deadret-cache-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);
            string ncsPath = Path.Combine(overrideDir, "test_script.ncs");
            File.WriteAllBytes(ncsPath, bytes);

            try
            {
                var installation = new Installation(installRoot);
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, ncsPath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                List<StrRefSearchResult> results = ReferenceCacheHelpers.FindStrRefReferences(
                    installation,
                    targetStrRef,
                    cache,
                    null);

                Assert.That(results, Is.Empty);
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

        [Test]
        public void FindStrRefReferences_EarlyReturnLiveCachePath_FindsConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (0) return;\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string installRoot = Path.Combine(Path.GetTempPath(), "ncs-live-cache-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);
            string ncsPath = Path.Combine(overrideDir, "test_script.ncs");
            File.WriteAllBytes(ncsPath, bytes);

            try
            {
                var installation = new Installation(installRoot);
                var resource = new FileResource("test_script", ResourceType.NCS, bytes.Length, 0, ncsPath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                List<StrRefSearchResult> results = ReferenceCacheHelpers.FindStrRefReferences(
                    installation,
                    targetStrRef,
                    cache,
                    null);

                Assert.That(results, Is.Not.Empty);
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

        [Test]
        public void GetConstiUsageContext_ElseBranchLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (0) { ActionSpeakStringByStrRef(1); } else { ActionSpeakStringByStrRef(n); }\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_WhileBreakLocalStrRefViaCptopsp_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    while (1) { break; }\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_DeadWhileBodyLocalStrRef_RemainsStackStored()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    while (0) { ActionSpeakStringByStrRef(n); }\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StackStored));
        }

        [Test]
        public void GetConstiUsageContext_IfOneLiveBranchLocalStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (1) { ActionSpeakStringByStrRef(n); }\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_DoWhileBreakLocalStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    do { break; } while (1);\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_DeadForBodyLocalStrRef_RemainsStackStored()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    int i;\n    for (i = 0; i < 0; i++) { ActionSpeakStringByStrRef(n); }\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StackStored));
        }

        [Test]
        public void GetConstiUsageContext_NestedDeadIfReturnLocalStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    if (0) { if (0) return; }\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_WhileOneDeadIfReturnLocalStrRef_RemainsStackStored()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void main() {\n    int n = " + targetStrRef + ";\n    while (1) { if (0) return; }\n    ActionSpeakStringByStrRef(n);\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StackStored));
        }

        [Test]
        public void StrRefReferenceCache_GlobalBpCrossSubThreeHopLocalStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "int g;\nvoid sub1() {\n    int m = g;\n    int k = m;\n    ActionSpeakStringByStrRef(k);\n}\nvoid main() {\n    g = " + targetStrRef + ";\n    sub1();\n}",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-bp-crosssub-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_JsrCallLiteralStrRefParameter_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int s) { ActionSpeakStringByStrRef(s); }\nvoid main() { speak(" + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_JsrCallLiteralNonStrRefParameter_ReturnsUnknown()
        {
            const int targetLiteral = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void noop(int x) { }\nvoid main() { noop(" + targetLiteral + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetLiteral);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.Unknown));
        }

        [Test]
        public void StrRefReferenceCache_JsrCallLiteralStrRefParameter_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int s) { ActionSpeakStringByStrRef(s); }\nvoid main() { speak(" + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-jsr-strref-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_JsrCallMultiArgStrRefOnSecondParam_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid main() { speak(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_JsrCallMultiArgStrRefOnFirstParamOnly_ReturnsUnknownForSecondLiteral()
        {
            const int targetLiteral = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(a); }\nvoid main() { speak(0, " + targetLiteral + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetLiteral);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.Unknown));
        }

        [Test]
        public void StrRefReferenceCache_JsrCallMultiArgStrRefOnSecondParam_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid main() { speak(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-jsr-multi-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_NestedJsrRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int s) { ActionSpeakStringByStrRef(s); }\nvoid relay(int s) { speak(s); }\nvoid main() { relay(" + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_NestedJsrRelayToNoop_ReturnsUnknown()
        {
            const int targetLiteral = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void noop(int x) { }\nvoid relay(int s) { noop(s); }\nvoid main() { relay(" + targetLiteral + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetLiteral);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.Unknown));
        }

        [Test]
        public void StrRefReferenceCache_NestedJsrRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int s) { ActionSpeakStringByStrRef(s); }\nvoid relay(int s) { speak(s); }\nvoid main() { relay(" + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-nested-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_NestedJsrMultiArgRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid relay(int a, int s) { speak(a, s); }\nvoid main() { relay(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void GetConstiUsageContext_TwoHopNestedJsrRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int s) { ActionSpeakStringByStrRef(s); }\nvoid mid(int s) { speak(s); }\nvoid relay(int s) { mid(s); }\nvoid main() { relay(" + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_TwoHopNestedJsrRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int s) { ActionSpeakStringByStrRef(s); }\nvoid mid(int s) { speak(s); }\nvoid relay(int s) { mid(s); }\nvoid main() { relay(" + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-two-hop-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_TwoHopNestedJsrMultiArgRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid mid(int a, int s) { speak(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid main() { relay(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_TwoHopNestedJsrMultiArgRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid mid(int a, int s) { speak(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid main() { relay(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-two-hop-multi-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_TwoHopMixedConstCptopspRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid mid(int a, int s) { speak(0, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid main() { relay(99, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_TwoHopMixedConstCptopspRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid mid(int a, int s) { speak(0, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid main() { relay(99, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-two-hop-mixed-const-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ThreeHopNestedJsrMultiArgRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(a, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid main() { relay(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ThreeHopNestedJsrMultiArgRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(a, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid main() { relay(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-three-hop-multi-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_FourHopNestedJsrMultiArgRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(a, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid outer(int a, int s) { relay(a, s); }\nvoid main() { outer(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_FourHopNestedJsrMultiArgRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(a, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid outer(int a, int s) { relay(a, s); }\nvoid main() { outer(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-four-hop-multi-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_FiveHopNestedJsrMultiArgRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(a, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid outer(int a, int s) { relay(a, s); }\nvoid deepest(int a, int s) { outer(a, s); }\nvoid main() { deepest(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_FiveHopNestedJsrMultiArgRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(a, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid outer(int a, int s) { relay(a, s); }\nvoid deepest(int a, int s) { outer(a, s); }\nvoid main() { deepest(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-five-hop-multi-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_SixHopNestedJsrMultiArgRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(a, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid outer(int a, int s) { relay(a, s); }\nvoid deepest(int a, int s) { outer(a, s); }\nvoid root(int a, int s) { deepest(a, s); }\nvoid main() { root(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_SixHopNestedJsrMultiArgRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(a, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid outer(int a, int s) { relay(a, s); }\nvoid deepest(int a, int s) { outer(a, s); }\nvoid root(int a, int s) { deepest(a, s); }\nvoid main() { root(0, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-six-hop-multi-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_SixHopMixedConstCptopspRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(0, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid outer(int a, int s) { relay(a, s); }\nvoid deepest(int a, int s) { outer(a, s); }\nvoid root(int a, int s) { deepest(a, s); }\nvoid main() { root(99, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_SixHopMixedConstCptopspRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(0, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid outer(int a, int s) { relay(a, s); }\nvoid deepest(int a, int s) { outer(a, s); }\nvoid root(int a, int s) { deepest(a, s); }\nvoid main() { root(99, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-six-hop-mixed-const-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_ThreeHopMixedConstCptopspRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(0, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid main() { relay(99, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_ThreeHopMixedConstCptopspRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid inner(int a, int s) { speak(0, s); }\nvoid mid(int a, int s) { inner(a, s); }\nvoid relay(int a, int s) { mid(a, s); }\nvoid main() { relay(99, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-three-hop-mixed-const-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
        public void GetConstiUsageContext_NestedJsrMixedConstCptopspRelayStrRef_ReturnsStrRefConsumer()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid relay(int a, int s) { speak(0, s); }\nvoid main() { relay(99, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            List<NcsConstiScanner.ConstiInstruction> instructions = NcsConstiScanner.ExtractConstiInstructions(bytes);
            NcsConstiScanner.ConstiInstruction match = instructions.Find(i => i.Value == targetStrRef);

            Assert.That(NcsConstiScanner.GetConstiUsageContext(bytes, match), Is.EqualTo(NcsConstiScanner.ConstiUsageContext.StrRefConsumer));
        }

        [Test]
        public void StrRefReferenceCache_NestedJsrMixedConstCptopspRelayStrRef_IsIndexed()
        {
            const int targetStrRef = 424242;
            NCS ncs = NCSAuto.CompileNss(
                "void speak(int a, int s) { ActionSpeakStringByStrRef(s); }\nvoid relay(int a, int s) { speak(0, s); }\nvoid main() { relay(99, " + targetStrRef + "); }",
                BioWareGame.K1);
            byte[] bytes = NCSAuto.BytesNcs(ncs);

            string filepath = Path.Combine(Path.GetTempPath(), "ncs-mixed-const-jsr-" + Guid.NewGuid().ToString("N") + ".ncs");
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
    }
}
