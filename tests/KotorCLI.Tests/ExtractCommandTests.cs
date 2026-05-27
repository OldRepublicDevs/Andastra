using System;
using System.IO;
using System.Linq;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.BIF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.KEY;
using BioWare.Tools;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ExtractCommandTests
    {
        private static void WriteSampleBifKeyPair(string bifPath, string keyPath, string resref, int resIndex)
        {
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var bif = new BIF();
            bif.SetData(ResRef.FromBlank(), ResourceType.UTC, utcBytes, resIndex);
            File.WriteAllBytes(bifPath, new BIFBinaryWriter(bif).Write());

            var key = new KEY();
            key.AddBif("sample.bif", (int)new FileInfo(bifPath).Length);
            key.AddKeyEntry(resref, ResourceType.UTC, 0, resIndex);
            File.WriteAllBytes(keyPath, KEYAuto.BytesKey(key));
        }

        [Test]
        public void ExecuteExtractBif_WritesExtractedResourceFiles()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string outputDir = Path.Combine(tempDir, "out");

                byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
                var bif = new BIF();
                bif.SetData(new ResRef("creature_a"), ResourceType.UTC, utcBytes);
                File.WriteAllBytes(bifPath, new BIFBinaryWriter(bif).Write());

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(bifPath, outputDir, null, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(Directory.GetFiles(outputDir).Length, Is.GreaterThan(0));
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }

        [Test]
        public void ListBif_WithKey_AppliesKeyResourceNames()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-listbif-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                KEY roundTrip = KEYAuto.ReadKey(keyPath);
                Assert.That(roundTrip.KeyEntries.Count, Is.EqualTo(1));
                Assert.That(roundTrip.KeyEntries[0].ResRef.ToString(), Is.EqualTo("from_key"));

                var listed = ArchiveHelpers.ListBif(bifPath, keyPath).ToList();
                Assert.That(listed.Count, Is.EqualTo(1));
                Assert.That(listed[0].ResRef.ToString(), Is.EqualTo("from_key"));
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }

        [Test]
        public void ExecuteExtractBif_WithKey_WritesNamedOutputFile()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-key-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                string outputDir = Path.Combine(tempDir, "out");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(bifPath, outputDir, null, keyPath, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(Path.Combine(outputDir, "from_key.utc")), Is.True);
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }

        [Test]
        public void KeyBytesKey_RoundTripsEntries()
        {
            var key = new KEY();
            key.AddBif("data.bif", 1024);
            key.AddKeyEntry("test_creature", ResourceType.UTC, 0, 0);

            byte[] bytes = KEYAuto.BytesKey(key);
            KEY roundTrip = KEYAuto.ReadKey(bytes);

            Assert.That(roundTrip.BifEntries.Count, Is.EqualTo(1));
            Assert.That(roundTrip.BifEntries[0].Filename, Is.EqualTo("data.bif"));
            Assert.That(roundTrip.KeyEntries.Count, Is.EqualTo(1));
            Assert.That(roundTrip.KeyEntries[0].ResRef.ToString(), Is.EqualTo("test_creature"));
            Assert.That(roundTrip.KeyEntries[0].ResType, Is.EqualTo(ResourceType.UTC));
        }
    }
}
