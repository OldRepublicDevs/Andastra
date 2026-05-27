using System;
using System.IO;
using System.Linq;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.BIF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.KEY;
using BioWare.Resource.Formats.RIM;
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

        private static void WriteSampleBifKeyPairWithTwoResources(
            string bifPath,
            string keyPath,
            string firstResref,
            string secondResref)
        {
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var bif = new BIF();
            bif.SetData(ResRef.FromBlank(), ResourceType.UTC, utcBytes, 0);
            bif.SetData(ResRef.FromBlank(), ResourceType.UTC, utcBytes, 1);
            File.WriteAllBytes(bifPath, new BIFBinaryWriter(bif).Write());

            var key = new KEY();
            key.AddBif("sample.bif", (int)new FileInfo(bifPath).Length);
            key.AddKeyEntry(firstResref, ResourceType.UTC, 0, 0);
            key.AddKeyEntry(secondResref, ResourceType.UTC, 0, 1);
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
        public void ExecuteExtractErf_WritesExtractedResourceFiles()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-erf-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErfWithTwoResources(tempDir);
                string outputDir = Path.Combine(tempDir, "out");

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(erfPath, outputDir, null, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(Directory.GetFiles(outputDir).Length, Is.EqualTo(2));
                Assert.That(File.Exists(Path.Combine(outputDir, "creature_a.utc")), Is.True);
                Assert.That(File.Exists(Path.Combine(outputDir, "creature_b.utc")), Is.True);
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

        [Test]
        public void ExecuteExtract_MissingInputFile_ExitsNonZero()
        {
            string missingPath = Path.Combine(Path.GetTempPath(), "kotorcli-missing-" + Guid.NewGuid().ToString("N") + ".rim");
            var logger = new StandardLogger();
            int exitCode = ExtractCommand.Execute(missingPath, null, null, null, logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void ExecuteExtract_UnsupportedExtension_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-unsupported-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string txtPath = Path.Combine(tempDir, "not_an_archive.txt");
                File.WriteAllText(txtPath, "not an archive");
                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(txtPath, null, null, null, logger);
                Assert.That(exitCode, Is.EqualTo(1));
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

        private static string CreateSampleModWithTwoResources(string tempDir)
        {
            string modPath = Path.Combine(tempDir, "sample.mod");
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var mod = new ERF(ERFType.MOD);
            mod.SetData("creature_a", ResourceType.UTC, utcBytes);
            mod.SetData("creature_b", ResourceType.UTC, utcBytes);
            ERFAuto.WriteErf(mod, modPath, ResourceType.MOD);
            return modPath;
        }

        private static string CreateSampleModWithOneResource(string tempDir)
        {
            string modPath = Path.Combine(tempDir, "sample.mod");
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var mod = new ERF(ERFType.MOD);
            mod.SetData("creature_a", ResourceType.UTC, utcBytes);
            ERFAuto.WriteErf(mod, modPath, ResourceType.MOD);
            return modPath;
        }

        private static string CreateSampleErfWithTwoResources(string tempDir)
        {
            string erfPath = Path.Combine(tempDir, "sample.erf");
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var erf = new ERF(ERFType.ERF);
            erf.SetData("creature_a", ResourceType.UTC, utcBytes);
            erf.SetData("creature_b", ResourceType.UTC, utcBytes);
            ERFAuto.WriteErf(erf, erfPath, ResourceType.ERF);
            return erfPath;
        }

        private static string CreateSampleErfWithOneResource(string tempDir)
        {
            string erfPath = Path.Combine(tempDir, "sample.erf");
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var erf = new ERF(ERFType.ERF);
            erf.SetData("creature_a", ResourceType.UTC, utcBytes);
            ERFAuto.WriteErf(erf, erfPath, ResourceType.ERF);
            return erfPath;
        }

        private static string CreateSampleRimWithTwoResources(string tempDir)
        {
            string rimPath = Path.Combine(tempDir, "sample.rim");
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var rim = new RIM();
            rim.SetData("creature_a", ResourceType.UTC, utcBytes);
            rim.SetData("creature_b", ResourceType.UTC, utcBytes);
            RIMAuto.WriteRim(rim, rimPath, ResourceType.RIM);
            return rimPath;
        }

        private static string CreateSampleRimWithOneResource(string tempDir)
        {
            string rimPath = Path.Combine(tempDir, "sample.rim");
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var rim = new RIM();
            rim.SetData("creature_a", ResourceType.UTC, utcBytes);
            RIMAuto.WriteRim(rim, rimPath, ResourceType.RIM);
            return rimPath;
        }

        [Test]
        public void ExecuteExtract_WithFilter_ExtractsMatchingResourceOnly()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRimWithTwoResources(tempDir);
                string outputDir = Path.Combine(tempDir, "out");

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(rimPath, outputDir, "creature_a*", null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(Path.Combine(outputDir, "creature_a.utc")), Is.True);
                Assert.That(File.Exists(Path.Combine(outputDir, "creature_b.utc")), Is.False);
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
        public void ExecuteExtract_WithFilterNoMatch_WritesNoFiles()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-filter-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRimWithOneResource(tempDir);
                string outputDir = Path.Combine(tempDir, "out");

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(rimPath, outputDir, "missing_*", null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(Directory.Exists(outputDir), Is.True);
                Assert.That(Directory.GetFiles(outputDir).Length, Is.EqualTo(0));
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
        public void ExecuteExtractBif_WithKeyAndFilter_ExtractsMatchingResourceOnly()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-bif-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                string outputDir = Path.Combine(tempDir, "out");
                WriteSampleBifKeyPairWithTwoResources(bifPath, keyPath, "creature_a", "creature_b");

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(bifPath, outputDir, "creature_a*", keyPath, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(Path.Combine(outputDir, "creature_a.utc")), Is.True);
                Assert.That(File.Exists(Path.Combine(outputDir, "creature_b.utc")), Is.False);
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
        public void ExecuteExtractBif_WithKeyAndFilterNoMatch_WritesNoFiles()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-bif-filter-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                string outputDir = Path.Combine(tempDir, "out");
                WriteSampleBifKeyPair(bifPath, keyPath, "creature_a", 0);

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(bifPath, outputDir, "missing_*", keyPath, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(Directory.Exists(outputDir), Is.True);
                Assert.That(Directory.GetFiles(outputDir).Length, Is.EqualTo(0));
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
        public void ExecuteExtractMod_WithFilter_ExtractsMatchingResourceOnly()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-mod-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleModWithTwoResources(tempDir);
                string outputDir = Path.Combine(tempDir, "out");

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(modPath, outputDir, "creature_a*", null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(Path.Combine(outputDir, "creature_a.utc")), Is.True);
                Assert.That(File.Exists(Path.Combine(outputDir, "creature_b.utc")), Is.False);
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
        public void ExecuteExtractMod_WithFilterNoMatch_WritesNoFiles()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-mod-filter-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleModWithOneResource(tempDir);
                string outputDir = Path.Combine(tempDir, "out");

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(modPath, outputDir, "missing_*", null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(Directory.Exists(outputDir), Is.True);
                Assert.That(Directory.GetFiles(outputDir).Length, Is.EqualTo(0));
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
        public void ExecuteExtractErf_WithFilter_ExtractsMatchingResourceOnly()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-erf-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErfWithTwoResources(tempDir);
                string outputDir = Path.Combine(tempDir, "out");

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(erfPath, outputDir, "creature_a*", null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(Path.Combine(outputDir, "creature_a.utc")), Is.True);
                Assert.That(File.Exists(Path.Combine(outputDir, "creature_b.utc")), Is.False);
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
        public void ExecuteExtractErf_WithFilterNoMatch_WritesNoFiles()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-erf-filter-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErfWithOneResource(tempDir);
                string outputDir = Path.Combine(tempDir, "out");

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(erfPath, outputDir, "missing_*", null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(Directory.Exists(outputDir), Is.True);
                Assert.That(Directory.GetFiles(outputDir).Length, Is.EqualTo(0));
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
        public void ExecuteExtractKey_WritesNamedOutputFile()
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
                int exitCode = ExtractCommand.Execute(keyPath, outputDir, null, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(Path.Combine(outputDir, "sample", "from_key.utc")), Is.True);
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
        public void ExecuteExtractKey_WithFilter_ExtractsMatchingResourceOnly()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-key-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                string outputDir = Path.Combine(tempDir, "out");
                WriteSampleBifKeyPairWithTwoResources(bifPath, keyPath, "creature_a", "creature_b");

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(keyPath, outputDir, "creature_a*", null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(Path.Combine(outputDir, "sample", "creature_a.utc")), Is.True);
                Assert.That(File.Exists(Path.Combine(outputDir, "sample", "creature_b.utc")), Is.False);
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
        public void ExecuteExtractKey_WithFilterNoMatch_WritesNoFiles()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-key-filter-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                string outputDir = Path.Combine(tempDir, "out");
                WriteSampleBifKeyPair(bifPath, keyPath, "creature_a", 0);

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(keyPath, outputDir, "missing_*", null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(Directory.Exists(outputDir), Is.True);
                Assert.That(Directory.GetFiles(outputDir).Length, Is.EqualTo(0));
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
    }
}
