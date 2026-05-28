using System;
using System.Diagnostics;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.RIM;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class GrepDiffCatCommandCliTests
    {
        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

        [Test]
        public void CliGrep_FindsMatchingLine_ExitsZero()
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "kotorcli-grep-cli-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile, "alpha\nbeta needle\ngamma\n");

            try
            {
                int exitCode = RunKotorCli(
                    "grep \"" + tempFile + "\" needle --line-numbers",
                    out string stdout,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(stdout + stderr, Does.Contain("needle"));
            }
            finally
            {
                DeleteFileSafe(tempFile);
            }
        }

        [Test]
        public void CliGrep_NoMatch_ExitsNonZero()
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "kotorcli-grep-cli-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile, "alpha\nbeta\n");

            try
            {
                int exitCode = RunKotorCli(
                    "grep \"" + tempFile + "\" missing",
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(1), stderr);
            }
            finally
            {
                DeleteFileSafe(tempFile);
            }
        }

        [Test]
        public void CliGrep_MissingFile_ExitsNonZero()
        {
            string missingFile = Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".txt");

            int exitCode = RunKotorCli(
                "grep \"" + missingFile + "\" x",
                out _,
                out string stderr);
            Assert.That(exitCode, Is.EqualTo(1), stderr);
        }

        [Test]
        public void CliDiff_IdenticalFiles_ExitsZero()
        {
            string tempFile1 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-cli-a-" + Guid.NewGuid().ToString("N") + ".txt");
            string tempFile2 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-cli-b-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile1, "same content\n");
            File.WriteAllText(tempFile2, "same content\n");

            try
            {
                int exitCode = RunKotorCli(
                    "diff \"" + tempFile1 + "\" \"" + tempFile2 + "\"",
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
            }
            finally
            {
                DeleteFileSafe(tempFile1);
                DeleteFileSafe(tempFile2);
            }
        }

        [Test]
        public void CliDiff_DifferentFiles_ExitsNonZero()
        {
            string tempFile1 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-cli-a-" + Guid.NewGuid().ToString("N") + ".txt");
            string tempFile2 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-cli-b-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile1, "alpha\n");
            File.WriteAllText(tempFile2, "beta\n");

            try
            {
                int exitCode = RunKotorCli(
                    "diff \"" + tempFile1 + "\" \"" + tempFile2 + "\"",
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(1), stderr);
            }
            finally
            {
                DeleteFileSafe(tempFile1);
                DeleteFileSafe(tempFile2);
            }
        }

        [Test]
        public void CliCat_ReadsResourceFromRim_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-cat-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string rimPath = Path.Combine(tempDir, "test.rim");

            try
            {
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "cat-cli-test");
                byte[] utcBytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

                var rim = new RIM();
                rim.SetData("sample", ResourceType.UTC, utcBytes);
                RIMAuto.WriteRim(rim, rimPath, ResourceType.RIM);

                int exitCode = RunKotorCli(
                    "cat \"" + rimPath + "\" sample --type utc",
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

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

        private static void DeleteFileSafe(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch
            {
                // Best-effort cleanup.
            }
        }

        private static void DeleteDirectorySafe(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch
            {
                // Best-effort cleanup.
            }
        }
    }
}
