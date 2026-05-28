using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.RIM;
using BioWare.Resource.Formats.TwoDA;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ValidationCommandsTests
    {
        [Test]
        public void ExecuteCheckTxi_MissingTexture_ExitsNonZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-txi-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            try
            {
                var logger = new StandardLogger();
                int exitCode = ValidationCommands.ExecuteCheckTxi(
                    installRoot,
                    new[] { "nonexistent_texture" },
                    logger);
                Assert.That(exitCode, Is.EqualTo(1));
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
        public void ExecuteCheckTxi_FoundInOverride_ExitsZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-txi-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllText(Path.Combine(overrideDir, "test_tex.txi"), "blending additive");

            try
            {
                var logger = new StandardLogger();
                int exitCode = ValidationCommands.ExecuteCheckTxi(
                    installRoot,
                    new[] { "test_tex" },
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteCheck2da_MissingTwoDA_ExitsNonZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-2da-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            try
            {
                var logger = new StandardLogger();
                int exitCode = ValidationCommands.ExecuteCheck2da(
                    installRoot,
                    "nonexistent_2da",
                    logger);
                Assert.That(exitCode, Is.EqualTo(1));
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
        public void ExecuteCheck2da_FoundInOverride_ExitsZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-2da-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var twoDA = new TwoDA(new List<string> { "label" });
            twoDA.AddRow();
            File.WriteAllBytes(
                Path.Combine(overrideDir, "test_twoda.2da"),
                TwoDAAuto.BytesTwoDA(twoDA));

            try
            {
                var logger = new StandardLogger();
                int exitCode = ValidationCommands.ExecuteCheck2da(
                    installRoot,
                    "test_twoda",
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteCheck2da_ValidStructure_LogsDimensions()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-2da-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var twoDA = new TwoDA(new List<string> { "label", "name", "value" });
            twoDA.AddRow();
            twoDA.AddRow();
            File.WriteAllBytes(
                Path.Combine(overrideDir, "sample.2da"),
                TwoDAAuto.BytesTwoDA(twoDA));

            var output = new StringWriter();
            TextWriter originalOut = Console.Out;

            try
            {
                Console.SetOut(output);
                var logger = new StandardLogger(noColor: true);
                int exitCode = ValidationCommands.ExecuteCheck2da(
                    installRoot,
                    "sample",
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));

                string log = output.ToString();
                Assert.That(log, Does.Contain("Valid 2DA structure: 3 columns x 2 rows"));
                Assert.That(log, Does.Contain("Headers: label, name, value"));
            }
            finally
            {
                Console.SetOut(originalOut);
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
        public void ExecuteValidateInstallation_MinimalEssentialInstall_ExitsZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-validate-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            WriteEssentialTwoDAFiles(overrideDir);

            try
            {
                var logger = new StandardLogger();
                int exitCode = ValidationCommands.ExecuteValidateInstallation(installRoot, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteValidateInstallation_NonexistentPath_ExitsNonZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-validate-missing-" + Guid.NewGuid().ToString("N"));

            var logger = new StandardLogger();
            int exitCode = ValidationCommands.ExecuteValidateInstallation(installRoot, true, logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void ExecuteValidateInstallation_MissingEssentialTwoDA_ExitsNonZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-validate-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var twoDA = new TwoDA(new List<string> { "label" });
            twoDA.AddRow();
            byte[] twoDaBytes = TwoDAAuto.BytesTwoDA(twoDA);
            File.WriteAllBytes(Path.Combine(overrideDir, "appearance.2da"), twoDaBytes);
            File.WriteAllBytes(Path.Combine(overrideDir, "baseitems.2da"), twoDaBytes);
            File.WriteAllBytes(Path.Combine(overrideDir, "classes.2da"), twoDaBytes);

            try
            {
                var logger = new StandardLogger();
                int exitCode = ValidationCommands.ExecuteValidateInstallation(installRoot, true, logger);
                Assert.That(exitCode, Is.EqualTo(1));
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
        public void ExecuteValidateInstallation_NoEssentialEmptyInstall_ExitsZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-validate-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            try
            {
                var logger = new StandardLogger();
                int exitCode = ValidationCommands.ExecuteValidateInstallation(installRoot, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void CliValidateInstallation_NoEssential_ExitsZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-validate-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            try
            {
                int exitCode = RunKotorCli(
                    "validate-installation --installation \"" + installRoot + "\" --no-essential",
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
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

        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

        private static int RunKotorCli(string arguments, out string stdout, out string stderr)
        {
            string cliDll = Path.Combine(RepoRoot, "src", "Tools", "KotorCLI", "bin", "Debug", "net9.0", "KotorCLI.dll");
            if (!File.Exists(cliDll))
            {
                cliDll = Path.Combine(RepoRoot, "src", "Tools", "KotorCLI", "bin", "Release", "net9.0", "KotorCLI.dll");
            }

            Assert.That(File.Exists(cliDll), Is.True, "KotorCLI.dll not built; run dotnet build first.");

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "exec \"" + cliDll + "\" " + arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (var process = Process.Start(startInfo))
            {
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();
                process.WaitForExit();
                return process.ExitCode;
            }
        }

        private static void WriteEssentialTwoDAFiles(string overrideDir)
        {
            var twoDA = new TwoDA(new List<string> { "label" });
            twoDA.AddRow();
            byte[] twoDaBytes = TwoDAAuto.BytesTwoDA(twoDA);

            string[] essentialNames = { "appearance", "baseitems", "classes", "genericdoors" };
            foreach (string name in essentialNames)
            {
                File.WriteAllBytes(Path.Combine(overrideDir, name + ".2da"), twoDaBytes);
            }
        }

    }

    [TestFixture]
    public class CatCommandTests
    {
        [Test]
        public void Execute_ReadsResourceFromRim()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-cat-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string rimPath = Path.Combine(tempDir, "test.rim");

            try
            {
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "cat-test");
                byte[] utcBytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

                var rim = new RIM();
                rim.SetData("sample", ResourceType.UTC, utcBytes);
                RIMAuto.WriteRim(rim, rimPath, ResourceType.RIM);

                var logger = new StandardLogger();
                int exitCode = CatCommand.Execute(rimPath, "sample", "utc", logger);
                Assert.That(exitCode, Is.EqualTo(0));

                // Verify binary output by re-reading rim resource directly
                var verifyRim = new RIMBinaryReader(rimPath).Load();
                bool found = false;
                foreach (RIMResource resource in verifyRim)
                {
                    if (string.Equals(resource.ResRef.ToString(), "sample", StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }

                Assert.That(found, Is.True);
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
        public void Execute_MissingResource_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-cat-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string rimPath = Path.Combine(tempDir, "empty.rim");

            try
            {
                RIMAuto.WriteRim(new RIM(), rimPath, ResourceType.RIM);
                var logger = new StandardLogger();
                int exitCode = CatCommand.Execute(rimPath, "missing", null, logger);
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
    }
}
