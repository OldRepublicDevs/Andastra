using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.SSF;
using BioWare.Tools;
using KotorDiff.Cache;
using NUnit.Framework;

namespace KotorDiff.Tests
{
    [TestFixture]
    public class DiffCacheIOTests
    {
        [Test]
        public void RestoreStrrefCacheFromCache_RoundTripsScannedReferences()
        {
            const int targetStrRef = 515151;
            byte[] bytes = CreateSsfBytesWithStrRef(targetStrRef);
            string filepath = Path.Combine(Path.GetTempPath(), "kotordiff-strref-" + Guid.NewGuid().ToString("N") + ".ssf");

            try
            {
                File.WriteAllBytes(filepath, bytes);
                var resource = new FileResource("test_set", ResourceType.SSF, bytes.Length, 0, filepath);
                var original = new StrRefReferenceCache(BioWareGame.K1);
                original.ScanResource(resource, bytes);

                var diffCache = new DiffCache
                {
                    StrrefCacheGame = DiffCacheIO.FormatGame(BioWareGame.K1),
                    StrrefCacheData = DiffCacheIO.ConvertToObjectDict(original.ToDict())
                };

                StrRefReferenceCache restored = DiffCacheIO.RestoreStrrefCacheFromCache(diffCache);

                Assert.That(restored, Is.Not.Null);
                Assert.That(restored.GetReferences(targetStrRef), Is.Not.Empty);
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
        public void SaveAndLoadDiffCache_PreservesStrrefCacheMetadata()
        {
            const int targetStrRef = 616161;
            byte[] bytes = CreateSsfBytesWithStrRef(targetStrRef);
            string tempRoot = Path.Combine(Path.GetTempPath(), "kotordiff-cache-" + Guid.NewGuid().ToString("N"));
            string cacheFile = Path.Combine(tempRoot, "diff-cache.yaml");
            Directory.CreateDirectory(tempRoot);

            try
            {
                var original = new StrRefReferenceCache(BioWareGame.K1);
                string ssfPath = Path.Combine(tempRoot, "test.ssf");
                File.WriteAllBytes(ssfPath, bytes);
                var resource = new FileResource("test_set", ResourceType.SSF, bytes.Length, 0, ssfPath);
                original.ScanResource(resource, bytes);

                var diffCache = new DiffCache
                {
                    Version = "1",
                    Mine = tempRoot,
                    Older = tempRoot,
                    Timestamp = DateTime.UtcNow.ToString("o"),
                    Files = new List<CachedFileComparison>()
                };

                DiffCacheIO.SaveDiffCache(diffCache, cacheFile, tempRoot, tempRoot, strrefCache: original, logFunc: _ => { });
                DiffCache loaded;
                string leftDir;
                string rightDir;
                (loaded, leftDir, rightDir) = DiffCacheIO.LoadDiffCache(cacheFile, _ => { });
                StrRefReferenceCache restored = DiffCacheIO.RestoreStrrefCacheFromCache(loaded);

                Assert.That(restored, Is.Not.Null);
                Assert.That(restored.GetReferences(targetStrRef), Is.Not.Empty);
                Assert.That(loaded.StrrefCacheGame, Is.EqualTo("k1"));
            }
            finally
            {
                try
                {
                    Directory.Delete(tempRoot, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }

        [Test]
        public void RestoreTwodaCacheFromCache_RoundTripsScannedReferences()
        {
            const int targetRow = 17;
            byte[] bytes = CreateUtcBytesWithAppearanceRow(targetRow);
            string filepath = Path.Combine(Path.GetTempPath(), "kotordiff-twoda-" + Guid.NewGuid().ToString("N") + ".utc");

            try
            {
                File.WriteAllBytes(filepath, bytes);
                var resource = new FileResource("test_npc", ResourceType.UTC, bytes.Length, 0, filepath);
                var original = new TwoDAMemoryReferenceCache(BioWareGame.K1);
                original.ScanResource(resource, bytes);

                var diffCache = new DiffCache
                {
                    TwodaCacheGame = DiffCacheIO.FormatGame(BioWareGame.K1),
                    TwodaCacheData = DiffCacheIO.ConvertToObjectDict(original.ToDict())
                };

                TwoDAMemoryReferenceCache restored = DiffCacheIO.RestoreTwodaCacheFromCache(diffCache);

                Assert.That(restored, Is.Not.Null);
                Assert.That(restored.HasReferences("appearance.2da", targetRow), Is.True);
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
        public void SaveAndLoadDiffCache_PreservesTwodaCacheMetadata()
        {
            const int targetRow = 21;
            byte[] bytes = CreateUtcBytesWithAppearanceRow(targetRow);
            string tempRoot = Path.Combine(Path.GetTempPath(), "kotordiff-twoda-cache-" + Guid.NewGuid().ToString("N"));
            string cacheFile = Path.Combine(tempRoot, "diff-cache.yaml");
            Directory.CreateDirectory(tempRoot);

            try
            {
                var original = new TwoDAMemoryReferenceCache(BioWareGame.K1);
                string utcPath = Path.Combine(tempRoot, "test.utc");
                File.WriteAllBytes(utcPath, bytes);
                var resource = new FileResource("test_npc", ResourceType.UTC, bytes.Length, 0, utcPath);
                original.ScanResource(resource, bytes);

                var diffCache = new DiffCache
                {
                    Version = "1",
                    Mine = tempRoot,
                    Older = tempRoot,
                    Timestamp = DateTime.UtcNow.ToString("o"),
                    Files = new List<CachedFileComparison>()
                };

                DiffCacheIO.SaveDiffCache(diffCache, cacheFile, tempRoot, tempRoot, twodaCache: original, logFunc: _ => { });
                DiffCache loaded;
                string leftDir;
                string rightDir;
                (loaded, leftDir, rightDir) = DiffCacheIO.LoadDiffCache(cacheFile, _ => { });
                TwoDAMemoryReferenceCache restored = DiffCacheIO.RestoreTwodaCacheFromCache(loaded);

                Assert.That(restored, Is.Not.Null);
                Assert.That(restored.HasReferences("appearance.2da", targetRow), Is.True);
                Assert.That(loaded.TwodaCacheGame, Is.EqualTo("k1"));
            }
            finally
            {
                try
                {
                    Directory.Delete(tempRoot, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }

        private static byte[] CreateUtcBytesWithAppearanceRow(int row)
        {
            var utc = new UTC();
            utc.AppearanceId = row;
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            return GFFAuto.BytesGff(gff, ResourceType.UTC);
        }

        private static byte[] CreateSsfBytesWithStrRef(int strRef)
        {
            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, strRef);
            return SSFAuto.BytesSsf(ssf);
        }
    }
}
