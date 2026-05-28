using System;
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
    public class StrRefReferenceCacheIOTests
    {
        [Test]
        public void SaveLoad_RoundTripsSsfReference()
        {
            const int targetStrRef = 515151;
            byte[] bytes = CreateSsfBytesWithStrRef(targetStrRef);
            string filepath = Path.Combine(Path.GetTempPath(), "strref-io-" + Guid.NewGuid().ToString("N") + ".ssf");
            string cacheFile = Path.Combine(Path.GetTempPath(), "strref-io-cache-" + Guid.NewGuid().ToString("N") + ".json");

            try
            {
                File.WriteAllBytes(filepath, bytes);
                var resource = new FileResource("test_set", ResourceType.SSF, bytes.Length, 0, filepath);
                var original = new StrRefReferenceCache(BioWareGame.K1);
                original.ScanResource(resource, bytes);

                StrRefReferenceCacheIO.Save(cacheFile, original);
                StrRefReferenceCache restored = StrRefReferenceCacheIO.Load(cacheFile, BioWareGame.K1, validateGame: true);

                Assert.That(restored.GetReferences(targetStrRef), Is.Not.Empty);
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
            const int targetStrRef = 616161;
            byte[] bytes = CreateSsfBytesWithStrRef(targetStrRef);
            string filepath = Path.Combine(Path.GetTempPath(), "strref-io-mismatch-" + Guid.NewGuid().ToString("N") + ".ssf");
            string cacheFile = Path.Combine(Path.GetTempPath(), "strref-io-mismatch-cache-" + Guid.NewGuid().ToString("N") + ".json");

            try
            {
                File.WriteAllBytes(filepath, bytes);
                var resource = new FileResource("test_set", ResourceType.SSF, bytes.Length, 0, filepath);
                var original = new StrRefReferenceCache(BioWareGame.K1);
                original.ScanResource(resource, bytes);
                StrRefReferenceCacheIO.Save(cacheFile, original);

                Assert.Throws<InvalidDataException>(() =>
                    StrRefReferenceCacheIO.Load(cacheFile, BioWareGame.TSL, validateGame: true));
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

        private static byte[] CreateSsfBytesWithStrRef(int strRef)
        {
            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, strRef);
            return SSFAuto.BytesSsf(ssf);
        }
    }
}
