using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.BIF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.KEY;
using BioWare.Resource.Formats.RIM;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ArchiveCommandsTests
    {
        private static string CreateSampleRim(string tempDir)
        {
            string rimPath = Path.Combine(tempDir, "test.rim");
            var gff = new GFF(GFFContent.GFF);
            gff.Root.SetString("Label", "archive-test");
            byte[] utcBytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            var rim = new RIM();
            rim.SetData("sample_npc", ResourceType.UTC, utcBytes);
            rim.SetData("other_res", ResourceType.GFF, GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.GFF));
            RIMAuto.WriteRim(rim, rimPath, ResourceType.RIM);
            return rimPath;
        }

        private static string CreateSampleMod(string tempDir)
        {
            string modPath = Path.Combine(tempDir, "test.mod");
            var gff = new GFF(GFFContent.GFF);
            gff.Root.SetString("Label", "archive-test");
            byte[] utcBytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            var mod = new ERF(ERFType.MOD);
            mod.SetData("sample_npc", ResourceType.UTC, utcBytes);
            mod.SetData("other_res", ResourceType.GFF, GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.GFF));
            ERFAuto.WriteErf(mod, modPath, ResourceType.MOD);
            return modPath;
        }

        private static string CreateSampleErf(string tempDir)
        {
            string erfPath = Path.Combine(tempDir, "test.erf");
            var gff = new GFF(GFFContent.GFF);
            gff.Root.SetString("Label", "archive-test");
            byte[] utcBytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            var erf = new ERF(ERFType.ERF);
            erf.SetData("sample_npc", ResourceType.UTC, utcBytes);
            erf.SetData("other_res", ResourceType.GFF, GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.GFF));
            ERFAuto.WriteErf(erf, erfPath, ResourceType.ERF);
            return erfPath;
        }

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

        private static void WriteStandaloneBifWithEmbeddedName(string bifPath, string resref)
        {
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var bif = new BIF();
            bif.SetData(new ResRef(resref), ResourceType.UTC, utcBytes);
            File.WriteAllBytes(bifPath, new BIFBinaryWriter(bif).Write());
        }

        [Test]
        public void ExecuteListArchive_BifWithSiblingKey_ListsNamedResource()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-bif-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(bifPath, false, "from_key*", logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_BifWithSiblingKey_ListsAllResourcesWithoutFilter()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-bif-nofilter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(bifPath, false, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_BifWithSiblingKey_FilterNoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-bif-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(bifPath, false, "missing_*", logger);
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

        [Test]
        public void ExecuteListArchive_BifWithSiblingKey_VerboseMode_ListsNamedResource()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-bif-verbose-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(bifPath, true, "from_key*", logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_BifWithoutSiblingKey_ListsEmbeddedResourceNames()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-bif-nokey-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "standalone.bif");
                WriteStandaloneBifWithEmbeddedName(bifPath, "creature_a");
                Assert.That(File.Exists(Path.Combine(tempDir, "standalone.key")), Is.False);
                Assert.That(File.Exists(Path.Combine(tempDir, "chitin.key")), Is.False);

                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(bifPath, false, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_BifWithoutSiblingKey_FilterMatchesExtensionPattern()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-bif-nokey-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "standalone.bif");
                WriteStandaloneBifWithEmbeddedName(bifPath, "creature_a");

                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(bifPath, false, "*.utc", logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_BifWithoutSiblingKey_FilterNoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-bif-nokey-nomatch-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "standalone.bif");
                WriteStandaloneBifWithEmbeddedName(bifPath, "creature_a");

                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(bifPath, false, "missing_*", logger);
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

        [Test]
        public void ExecuteListArchive_KeyFile_ListsNamedEntry()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-key-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(keyPath, false, "from_key*", logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_KeyFile_MatchesNamedEntry()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-key-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(keyPath, "from_key*", false, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_KeyFile_FilterNoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-key-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(keyPath, false, "missing_*", logger);
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

        [Test]
        public void ExecuteSearchArchive_KeyFile_NoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-key-nomatch-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(keyPath, "missing_*", false, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_KeyFile_CaseSensitiveName_RejectsCaseMismatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-key-case-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(keyPath, "From_key*", true, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_KeyFile_CaseSensitiveName_MatchesExactCase()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-key-case-match-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(keyPath, "from_key*", true, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_KeyFile_VerboseMode_ListsNamedEntry()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-key-verbose-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(keyPath, true, "from_key*", logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_ListsRimResources()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(rimPath, false, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_VerboseMode_ListsRimResources()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-verbose-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(rimPath, true, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_MissingFile_ExitsNonZero()
        {
            string missingPath = Path.Combine(Path.GetTempPath(), "kotorcli-list-missing-" + Guid.NewGuid().ToString("N") + ".rim");
            var logger = new StandardLogger();
            int exitCode = ListArchiveCommand.Execute(missingPath, false, null, logger);
            Assert.That(exitCode, Is.Not.EqualTo(0));
        }

        [Test]
        public void ExecuteListArchive_FilterNoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(rimPath, false, "does_not_exist_*", logger);
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

        [Test]
        public void ExecuteListArchive_UnsupportedExtension_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-unsupported-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string txtPath = Path.Combine(tempDir, "not_an_archive.txt");
                File.WriteAllText(txtPath, "not an archive");
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(txtPath, false, null, logger);
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

        [Test]
        public void ExecuteSearchArchive_UnsupportedExtension_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-unsupported-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string txtPath = Path.Combine(tempDir, "not_an_archive.txt");
                File.WriteAllText(txtPath, "not an archive");
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(txtPath, "sample_*", false, false, logger);
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

        [Test]
        public void ExecuteListArchive_EmptyFilePath_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = ListArchiveCommand.Execute(string.Empty, false, null, logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void ExecuteListArchive_FilterMatchesResource_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(rimPath, false, "sample_*", logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_ListsModResources()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-mod-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(modPath, false, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_ModFilterMatchesResource_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-mod-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(modPath, false, "sample_*", logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_ModMatchesWildcard()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-mod-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(modPath, "sample_*", false, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_ModContentMode_MatchesStringInResourceData()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-mod-content-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(modPath, "archive-test", false, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_ListsErfResources()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-erf-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(erfPath, false, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_ErfFilterMatchesResource_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-erf-filter-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(erfPath, false, "sample_*", logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_ErfMatchesWildcard()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-erf-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(erfPath, "sample_*", false, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_ErfContentMode_MatchesStringInResourceData()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-erf-content-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(erfPath, "archive-test", false, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_ErfVerboseMode_ListsResources()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-erf-verbose-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(erfPath, true, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_ErfFilterNoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-erf-filter-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(erfPath, false, "does_not_exist_*", logger);
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

        [Test]
        public void ExecuteSearchArchive_ErfNoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-erf-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(erfPath, "missing_*", false, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_ErfCaseSensitiveName_RejectsCaseMismatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-erf-case-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(erfPath, "Sample_*", true, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_ErfCaseSensitiveName_MatchesExactCase()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-erf-case-name-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(erfPath, "sample_*", true, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_ErfCaseSensitiveContent_RejectsCaseMismatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-erf-case-content-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(erfPath, "Archive-Test", true, true, logger);
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

        [Test]
        public void ExecuteSearchArchive_ErfCaseSensitiveContent_MatchesExactCase()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-erf-case-content-match-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(erfPath, "archive-test", true, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_ErfContentModeDisabled_SkipsPayloadWhenNameDoesNotMatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-erf-nocontent-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = CreateSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(erfPath, "archive-test", false, false, logger);
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

        [Test]
        public void ExecuteListArchive_ModVerboseMode_ListsResources()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-mod-verbose-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(modPath, true, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_ModFilterNoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-mod-filter-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(modPath, false, "does_not_exist_*", logger);
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

        [Test]
        public void ExecuteSearchArchive_ModNoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-mod-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(modPath, "missing_*", false, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_ModCaseSensitiveName_RejectsCaseMismatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-mod-case-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(modPath, "Sample_*", true, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_ModCaseSensitiveName_MatchesExactCase()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-mod-case-name-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(modPath, "sample_*", true, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_ModCaseSensitiveContent_RejectsCaseMismatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-mod-case-content-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(modPath, "Archive-Test", true, true, logger);
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

        [Test]
        public void ExecuteSearchArchive_ModCaseSensitiveContent_MatchesExactCase()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-mod-case-content-match-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(modPath, "archive-test", true, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_ModContentModeDisabled_SkipsPayloadWhenNameDoesNotMatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-mod-nocontent-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string modPath = CreateSampleMod(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(modPath, "archive-test", false, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_MatchesWildcard()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, "sample_*", false, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_BifWithSiblingKey_MatchesNamedResource()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-bif-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(bifPath, "from_key*", false, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_BifWithSiblingKey_NoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-bif-nomatch-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(bifPath, "missing_*", false, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_BifWithSiblingKey_CaseSensitiveName_RejectsCaseMismatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-bif-case-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(bifPath, "From_key*", true, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_BifWithSiblingKey_CaseSensitiveName_MatchesExactCase()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-bif-case-match-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string keyPath = Path.Combine(tempDir, "sample.key");
                WriteSampleBifKeyPair(bifPath, keyPath, "from_key", 0);

                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(bifPath, "from_key*", true, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_ContentMode_MatchesStringInResourceData()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-content-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, "archive-test", false, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_ContentModeDisabled_SkipsPayloadWhenNameDoesNotMatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-nocontent-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, "archive-test", false, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_NoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, "missing_*", false, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_EmptyFilePath_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = SearchArchiveCommand.Execute(string.Empty, "sample_*", false, false, logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void ExecuteSearchArchive_MissingFile_ExitsNonZero()
        {
            string missingPath = Path.Combine(Path.GetTempPath(), "kotorcli-missing-" + Guid.NewGuid().ToString("N") + ".rim");
            var logger = new StandardLogger();
            int exitCode = SearchArchiveCommand.Execute(missingPath, "*", false, false, logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void ExecuteSearchArchive_EmptyPattern_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, string.Empty, false, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_CaseSensitiveName_RejectsCaseMismatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-case-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, "Sample_*", true, false, logger);
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

        [Test]
        public void ExecuteSearchArchive_CaseSensitiveContent_RejectsCaseMismatch()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-case-content-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, "Archive-Test", true, true, logger);
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

        [Test]
        public void ExecuteSearchArchive_CaseSensitiveName_MatchesExactCase()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-case-name-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, "sample_*", true, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_CaseSensitiveContent_MatchesExactCase()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-case-content-ok-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, "archive-test", true, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteLaunch_Stub_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, null, null, logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }
    }
}
