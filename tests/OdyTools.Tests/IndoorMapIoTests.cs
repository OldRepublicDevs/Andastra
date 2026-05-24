using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource.Formats.ERF;
using NUnit.Framework;
using IndoorMapIo = BioWare.Tools.IndoorMapIo;

namespace OdyTools.Tests
{
    [TestFixture]
    public class IndoorMapIoTests
    {
        [Test]
        public void EmbedIndoorJson_RoundtripsOnErf()
        {
            byte[] payload = System.Text.Encoding.UTF8.GetBytes("{\"module_id\":\"test01\"}");
            var mod = new ERF(ERFType.MOD);
            IndoorMapIo.EmbedIndoorJson(mod, payload);

            byte[] extracted = IndoorMapIo.TryExtractFromErf(mod);
            Assert.That(extracted, Is.Not.Null);
            Assert.That(System.Text.Encoding.UTF8.GetString(extracted), Is.EqualTo("{\"module_id\":\"test01\"}"));
        }

        [Test]
        public void TryExtractEmbeddedIndoorJsonFromModuleFiles_ReadsModOnDisk()
        {
            byte[] payload = System.Text.Encoding.UTF8.GetBytes("{\"module_id\":\"diskmod\"}");
            var mod = new ERF(ERFType.MOD);
            IndoorMapIo.EmbedIndoorJson(mod, payload);

            string tempPath = Path.Combine(Path.GetTempPath(), "indoormap_io_" + Guid.NewGuid().ToString("N") + ".mod");
            try
            {
                ERFAuto.WriteErf(mod, tempPath, ResourceType.MOD);
                byte[] extracted = IndoorMapIo.TryExtractEmbeddedIndoorJsonFromModuleFiles(new[] { tempPath });
                Assert.That(extracted, Is.Not.Null);
                Assert.That(System.Text.Encoding.UTF8.GetString(extracted), Is.EqualTo("{\"module_id\":\"diskmod\"}"));
            }
            finally
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
        }

        [Test]
        public void EmbedIndoorJson_NullModThrows()
        {
            Assert.Throws<ArgumentNullException>(() => IndoorMapIo.EmbedIndoorJson(null, new byte[] { 1 }));
        }
    }
}
