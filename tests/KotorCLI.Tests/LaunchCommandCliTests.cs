using System;
using System.Diagnostics;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class LaunchCommandCliTests
    {
        private const string MinimalConfig = @"[package]
name = ""testpack""

[target]
name = ""default""
file = ""test.mod""
";

        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

        [TestCase("launch")]
        [TestCase("serve")]
        [TestCase("play")]
        [TestCase("test")]
        public void CliAlias_InstallOnly_WithFakeInstallDir_InstallsMod(string alias)
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-cli-installonly-proj-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-cli-installonly-game-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(fakeInstallDir, "chitin.key"), "fake-key");
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                WriteModWithUtc(Path.Combine(projectDir, "test.mod"), "cli_creature");

                int exitCode = RunKotorCli(
                    alias + " default --install-only --installDir \"" + fakeInstallDir + "\"",
                    projectDir,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                string installedModPath = Path.Combine(fakeInstallDir, "modules", "test.mod");
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(File.Exists(installedModPath), Is.True, combined);
                Assert.That(combined.ToLowerInvariant(), Does.Contain("install-only"));
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [TestCase("launch")]
        [TestCase("serve")]
        [TestCase("play")]
        [TestCase("test")]
        public void CliAlias_DryRun_WithGameBin_ExitsZero(string alias)
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                int exitCode = RunKotorCli(
                    alias + " test_mod --dry-run --gameBin \"" + fakeExe + "\"",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain(fakeExe));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        private static void WriteModWithUtc(string modPath, string resref)
        {
            GFF gff = UTCHelpers.DismantleUtc(new UTC(), BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            var mod = new ERF(ERFType.MOD);
            mod.SetData(resref, ResourceType.UTC, bytes);
            ERFAuto.WriteErf(mod, modPath, ResourceType.MOD);
        }

        private static int RunKotorCli(string arguments, out string stdout, out string stderr)
        {
            return RunKotorCli(arguments, RepoRoot, out stdout, out stderr);
        }

        private static int RunKotorCli(string arguments, string workingDirectory, out string stdout, out string stderr)
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
                WorkingDirectory = workingDirectory
            };

            using (var process = Process.Start(psi))
            {
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();
                process.WaitForExit(120000);
                return process.ExitCode;
            }
        }

        private static void DeleteDirectorySafe(string path)
        {
            try
            {
                Directory.Delete(path, true);
            }
            catch
            {
                // Best-effort cleanup.
            }
        }
    }
}
