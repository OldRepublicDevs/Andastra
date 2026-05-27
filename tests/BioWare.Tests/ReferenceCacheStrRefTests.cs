using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.SSF;
using BioWare.Tools;
using NUnit.Framework;

namespace BioWare.Tests
{
    [TestFixture]
    public class ReferenceCacheStrRefTests
    {
        [Test]
        public void ConvertToReferenceSearchResults_SsfLocation_IncludesFieldPath()
        {
            var resource = new FileResource("test", ResourceType.SSF, 0, 0, "Override/test.ssf");
            var strrefResult = new StrRefSearchResult(
                resource,
                new List<object> { new SSFRefLocation(SSFSound.BATTLE_CRY_1) });

            List<ReferenceSearchResult> converted = ReferenceCacheHelpers.ConvertToReferenceSearchResults(
                new[] { strrefResult },
                12345);

            Assert.That(converted, Has.Count.EqualTo(1));
            Assert.That(converted[0].FieldPath, Is.EqualTo("Sound BATTLE_CRY_1"));
            Assert.That(converted[0].MatchedValue, Is.EqualTo("12345"));
        }

        [Test]
        public void StrRefReferenceCache_SsfSound_IsFound()
        {
            const int targetStrRef = 424242;
            byte[] bytes = CreateSsfBytesWithStrRef(targetStrRef);

            string filepath = Path.Combine(Path.GetTempPath(), "strref-cache-" + Guid.NewGuid().ToString("N") + ".ssf");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_set", ResourceType.SSF, bytes.Length, 0, filepath);
                var cache = new StrRefReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.GetReferences(targetStrRef), Is.Not.Empty);
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
        public void FindStrRefReferences_OverrideSsf_FindsSoundSlot()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "strref-find-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            const int targetStrRef = 424242;
            byte[] bytes = CreateSsfBytesWithStrRef(targetStrRef);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_set.ssf"), bytes);

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
                    r => r.FieldPath == "Sound BATTLE_CRY_1" && r.MatchedValue == targetStrRef.ToString()));
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
        public void FindStrRefReferences_NoOverride_SkipsOverrideSsf()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "strref-scope-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            const int targetStrRef = 424242;
            byte[] bytes = CreateSsfBytesWithStrRef(targetStrRef);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_set.ssf"), bytes);

            try
            {
                var installation = new Installation(installRoot);
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = false,
                    SearchChitin = false,
                    SearchModules = false
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

        private static byte[] CreateSsfBytesWithStrRef(int strref)
        {
            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, strref);
            return SSFAuto.BytesSsf(ssf);
        }
    }
}
