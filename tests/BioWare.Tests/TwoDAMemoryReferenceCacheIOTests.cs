using System;
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
    public class TwoDAMemoryReferenceCacheIOTests
    {
        [Test]
        public void SaveLoad_RoundTripsUtcAppearanceReference()
        {
            const int targetRow = 21;
            byte[] bytes = CreateUtcBytesWithAppearanceRow(targetRow);
            string filepath = Path.Combine(Path.GetTempPath(), "twoda-io-" + Guid.NewGuid().ToString("N") + ".utc");
            string cacheFile = Path.Combine(Path.GetTempPath(), "twoda-io-cache-" + Guid.NewGuid().ToString("N") + ".json");

            try
            {
                File.WriteAllBytes(filepath, bytes);
                var resource = new FileResource("test_npc", ResourceType.UTC, bytes.Length, 0, filepath);
                var original = new TwoDAMemoryReferenceCache(BioWareGame.K1);
                original.ScanResource(resource, bytes);

                TwoDAMemoryReferenceCacheIO.Save(cacheFile, original);
                TwoDAMemoryReferenceCache restored = TwoDAMemoryReferenceCacheIO.Load(cacheFile, BioWareGame.K1, validateGame: true);

                Assert.That(restored.HasReferences("appearance.2da", targetRow), Is.True);
            }
            finally
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                if (File.Exists(cacheFile))
                {
                    File.Delete(cacheFile);
                }
            }
        }

        [Test]
        public void Load_GameMismatch_Throws()
        {
            const int targetRow = 22;
            byte[] bytes = CreateUtcBytesWithAppearanceRow(targetRow);
            string filepath = Path.Combine(Path.GetTempPath(), "twoda-io-mismatch-" + Guid.NewGuid().ToString("N") + ".utc");
            string cacheFile = Path.Combine(Path.GetTempPath(), "twoda-io-mismatch-cache-" + Guid.NewGuid().ToString("N") + ".json");

            try
            {
                File.WriteAllBytes(filepath, bytes);
                var resource = new FileResource("test_npc", ResourceType.UTC, bytes.Length, 0, filepath);
                var original = new TwoDAMemoryReferenceCache(BioWareGame.K1);
                original.ScanResource(resource, bytes);
                TwoDAMemoryReferenceCacheIO.Save(cacheFile, original);

                Assert.Throws<InvalidDataException>(() =>
                    TwoDAMemoryReferenceCacheIO.Load(cacheFile, BioWareGame.TSL, validateGame: true));
            }
            finally
            {
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }

                if (File.Exists(cacheFile))
                {
                    File.Delete(cacheFile);
                }
            }
        }

        private static byte[] CreateUtcBytesWithAppearanceRow(int rowIndex)
        {
            var utc = new UTC();
            utc.AppearanceId = rowIndex;
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            return GFFAuto.BytesGff(gff, ResourceType.UTC);
        }
    }
}
