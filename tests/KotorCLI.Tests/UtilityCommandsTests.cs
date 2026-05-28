using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.BIF;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.TLK;
using BioWare.Resource.Formats.TwoDA;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class UtilityCommandsTests
    {
        [Test]
        public void ExecuteGrep_FindsMatchingLine()
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "kotorcli-grep-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile, "alpha\nbeta needle\ngamma\n");

            try
            {
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteGrep(tempFile, "needle", false, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [Test]
        public void ExecuteGrep_NoMatch_ExitsNonZero()
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "kotorcli-grep-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile, "alpha\nbeta\n");

            try
            {
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteGrep(tempFile, "missing", false, false, logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [Test]
        public void ExecuteGrep_MissingFile_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = UtilityCommands.ExecuteGrep(
                Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".txt"),
                "x",
                false,
                false,
                logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void ExecuteDiff_IdenticalFiles_ExitsZero()
        {
            string tempFile1 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-a-" + Guid.NewGuid().ToString("N") + ".txt");
            string tempFile2 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-b-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile1, "same content\n");
            File.WriteAllText(tempFile2, "same content\n");

            try
            {
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteDiff(tempFile1, tempFile2, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                if (File.Exists(tempFile1))
                {
                    File.Delete(tempFile1);
                }

                if (File.Exists(tempFile2))
                {
                    File.Delete(tempFile2);
                }
            }
        }

        [Test]
        public void ExecuteDiff_DifferentFiles_ExitsNonZero()
        {
            string tempFile1 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-a-" + Guid.NewGuid().ToString("N") + ".txt");
            string tempFile2 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-b-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile1, "alpha\n");
            File.WriteAllText(tempFile2, "beta\n");

            try
            {
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteDiff(tempFile1, tempFile2, null, logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                if (File.Exists(tempFile1))
                {
                    File.Delete(tempFile1);
                }

                if (File.Exists(tempFile2))
                {
                    File.Delete(tempFile2);
                }
            }
        }

        [Test]
        public void ExecuteStats_ValidUtc_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-stats-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string utcPath = WriteSampleUtc(tempDir);
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteStats(utcPath, logger);
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
        public void ExecuteValidate_ValidUtc_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-validate-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string utcPath = WriteSampleUtc(tempDir);
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteValidate(utcPath, false, logger);
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
        public void ExecuteValidate_MissingFile_ExitsNonZero()
        {
            string missingPath = Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".utc");
            var logger = new StandardLogger();
            int exitCode = UtilityCommands.ExecuteValidate(missingPath, false, logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void ExecuteStats_ValidTwoDA_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-stats-2da-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string twoDaPath = WriteSampleTwoDA(tempDir);
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteStats(twoDaPath, logger);
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
        public void ExecuteValidate_ValidTwoDA_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-validate-2da-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string twoDaPath = WriteSampleTwoDA(tempDir);
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteValidate(twoDaPath, false, logger);
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
        public void ExecuteStats_ValidErf_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-stats-erf-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = WriteSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteStats(erfPath, logger);
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
        public void ExecuteValidate_ValidErf_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-validate-erf-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string erfPath = WriteSampleErf(tempDir);
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteValidate(erfPath, false, logger);
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
        public void ExecuteStats_ValidBif_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-stats-bif-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = WriteSampleBif(tempDir);
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteStats(bifPath, logger);
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
        public void ExecuteValidate_ValidBif_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-validate-bif-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = WriteSampleBif(tempDir);
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteValidate(bifPath, false, logger);
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
        public void ExecuteStats_ValidTlk_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-stats-tlk-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string tlkPath = WriteSampleTlk(tempDir);
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteStats(tlkPath, logger);
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
        public void ExecuteValidate_ValidTlk_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-validate-tlk-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string tlkPath = WriteSampleTlk(tempDir);
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteValidate(tlkPath, false, logger);
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

        private static string WriteSampleUtc(string tempDir)
        {
            string utcPath = Path.Combine(tempDir, "sample.utc");
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            File.WriteAllBytes(utcPath, utcBytes);
            return utcPath;
        }

        private static string WriteSampleTwoDA(string tempDir)
        {
            string twoDaPath = Path.Combine(tempDir, "sample.2da");
            var twoDA = new TwoDA(new System.Collections.Generic.List<string> { "label", "value" });
            twoDA.AddRow();
            File.WriteAllBytes(twoDaPath, TwoDAAuto.BytesTwoDA(twoDA));
            return twoDaPath;
        }

        private static string WriteSampleErf(string tempDir)
        {
            string erfPath = Path.Combine(tempDir, "sample.erf");
            var gff = new GFF(GFFContent.GFF);
            gff.Root.SetString("Label", "stats-validate-test");
            byte[] utcBytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            var erf = new ERF(ERFType.ERF);
            erf.SetData("sample_npc", ResourceType.UTC, utcBytes);
            erf.SetData("other_res", ResourceType.GFF, GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.GFF));
            ERFAuto.WriteErf(erf, erfPath, ResourceType.ERF);
            return erfPath;
        }

        private static string WriteSampleBif(string tempDir)
        {
            string bifPath = Path.Combine(tempDir, "sample.bif");
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var bif = new BIF();
            bif.SetData(new ResRef("creature_a"), ResourceType.UTC, utcBytes);
            File.WriteAllBytes(bifPath, new BIFBinaryWriter(bif).Write());
            return bifPath;
        }

        private static string WriteSampleTlk(string tempDir)
        {
            string tlkPath = Path.Combine(tempDir, "sample.tlk");
            var tlk = new TLK(Language.English);
            tlk.Add("stats-validate-tlk-entry", string.Empty);
            File.WriteAllBytes(tlkPath, TLKAuto.BytesTlk(tlk, ResourceType.TLK));
            return tlkPath;
        }
    }
}
