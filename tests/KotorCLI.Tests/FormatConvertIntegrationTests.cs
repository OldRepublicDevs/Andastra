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
    public class FormatConvertIntegrationTests
    {
        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

        private static int RunKotorCli(string arguments, out string stdout, out string stderr)
        {
            string cliDll = Path.Combine(RepoRoot, "src", "Tools", "KotorCLI", "bin", "Debug", "net9.0", "KotorCLI.dll");
            if (!File.Exists(cliDll))
            {
                var buildPsi = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "build \"" + Path.Combine(RepoRoot, "src", "Tools", "KotorCLI", "KotorCLI.csproj") + "\" --framework net9.0",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = RepoRoot
                };

                using (var buildProcess = Process.Start(buildPsi))
                {
                    buildProcess.WaitForExit(120000);
                    Assert.That(buildProcess.ExitCode, Is.EqualTo(0), "KotorCLI build failed before integration test.");
                }
            }

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "exec \"" + cliDll + "\" " + arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = RepoRoot
            };

            using (var process = Process.Start(psi))
            {
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();
                process.WaitForExit(120000);
                return process.ExitCode;
            }
        }

        [Test]
        public void Gff2Json_MinimalGff_WritesJsonFile()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-gff2json-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string gffPath = Path.Combine(tempDir, "sample.gff");
            string jsonPath = Path.Combine(tempDir, "sample.json");

            try
            {
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "test");
                File.WriteAllBytes(gffPath, GFFAuto.BytesGff(gff, ResourceType.GFF));

                int exitCode = RunKotorCli("gff2json \"" + gffPath + "\" --output \"" + jsonPath + "\"", out _, out string stderr);

                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(jsonPath), Is.True);
                Assert.That(new FileInfo(jsonPath).Length, Is.GreaterThan(0));
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
        public void Json2Gff_MissingInput_ExitsNonZero()
        {
            int exitCode = RunKotorCli("json2gff \"" + Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".json") + "\"", out _, out _);
            Assert.That(exitCode, Is.Not.EqualTo(0));
        }
    }
}
