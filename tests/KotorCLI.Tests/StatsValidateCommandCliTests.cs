using System;
using System.Diagnostics;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class StatsValidateCommandCliTests
    {
        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

        [Test]
        public void CliStats_ValidUtc_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-stats-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string utcPath = WriteSampleUtc(tempDir);
                int exitCode = RunKotorCli(
                    "stats \"" + utcPath + "\"",
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void CliStats_MissingFile_ExitsNonZero()
        {
            string missingPath = Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".utc");

            int exitCode = RunKotorCli(
                "stats \"" + missingPath + "\"",
                out _,
                out string stderr);
            Assert.That(exitCode, Is.EqualTo(1), stderr);
        }

        [Test]
        public void CliValidate_ValidUtc_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-validate-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string utcPath = WriteSampleUtc(tempDir);
                int exitCode = RunKotorCli(
                    "validate \"" + utcPath + "\"",
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void CliValidate_MissingFile_ExitsNonZero()
        {
            string missingPath = Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".utc");

            int exitCode = RunKotorCli(
                "validate \"" + missingPath + "\"",
                out _,
                out string stderr);
            Assert.That(exitCode, Is.EqualTo(1), stderr);
        }

        private static string WriteSampleUtc(string tempDir)
        {
            string utcPath = Path.Combine(tempDir, "sample.utc");
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            File.WriteAllBytes(utcPath, utcBytes);
            return utcPath;
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
