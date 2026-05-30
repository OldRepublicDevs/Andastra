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
    }
}
