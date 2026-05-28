using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Tools;
using NUnit.Framework;

namespace BioWare.Tests
{
    [TestFixture]
    public class ReferenceCacheTwoDAMemoryTests
    {
        [Test]
        public void NormalizeTwoDAFilename_AppendsExtension()
        {
            Assert.That(ReferenceCacheHelpers.NormalizeTwoDAFilename("appearance"), Is.EqualTo("appearance.2da"));
            Assert.That(ReferenceCacheHelpers.NormalizeTwoDAFilename("appearance.2da"), Is.EqualTo("appearance.2da"));
        }

        [Test]
        public void TwoDAMemoryReferenceCache_UtcAppearanceRow_IsFound()
        {
            const int targetRow = 17;
            var utc = new UTC();
            utc.AppearanceId = targetRow;

            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            string filepath = Path.Combine(Path.GetTempPath(), "twoda-cache-" + Guid.NewGuid().ToString("N") + ".utc");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_npc", ResourceType.UTC, bytes.Length, 0, filepath);
                var cache = new TwoDAMemoryReferenceCache(BioWareGame.K1);
                cache.ScanResource(resource, bytes);

                Assert.That(cache.HasReferences("appearance.2da", targetRow), Is.True);
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
        public void TwoDAMemoryReferenceCache_ToDictFromDict_RoundTripsUtcReference()
        {
            const int targetRow = 19;
            var utc = new UTC();
            utc.AppearanceId = targetRow;
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            string filepath = Path.Combine(Path.GetTempPath(), "twoda-dict-" + Guid.NewGuid().ToString("N") + ".utc");
            File.WriteAllBytes(filepath, bytes);

            try
            {
                var resource = new FileResource("test_npc", ResourceType.UTC, bytes.Length, 0, filepath);
                var original = new TwoDAMemoryReferenceCache(BioWareGame.K1);
                original.ScanResource(resource, bytes);

                TwoDAMemoryReferenceCache restored = TwoDAMemoryReferenceCache.FromDict(
                    BioWareGame.K1,
                    original.ToDict());

                Assert.That(restored.HasReferences("appearance.2da", targetRow), Is.True);
                Assert.That(restored.Game, Is.EqualTo(BioWareGame.K1));
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
        public void Find2DAMemoryReferences_OverrideUtc_FindsAppearanceField()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "twoda-find-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            const int targetRow = 17;
            var utc = new UTC();
            utc.AppearanceId = targetRow;
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            try
            {
                var installation = new Installation(installRoot);
                List<ReferenceSearchResult> results = ReferenceCacheHelpers.Find2DAMemoryReferences(
                    installation,
                    "appearance",
                    targetRow,
                    null,
                    null);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "Appearance_Type" && r.MatchedValue == "appearance.2da:" + targetRow));
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
