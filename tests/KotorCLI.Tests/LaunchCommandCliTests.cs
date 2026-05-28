using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
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
        public void CliAlias_FullLaunch_WithWait_InstallsAndRunsStub(string alias)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.Ignore("Shell stub launch CLI tests require Linux.");
            }

            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-cli-spawn-proj-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-cli-spawn-game-" + Guid.NewGuid().ToString("N"));
            string markerPath = Path.Combine(fakeInstallDir, "cli-launch-ran.marker");

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(fakeInstallDir, "chitin.key"), "fake-key");
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                WriteModWithUtc(Path.Combine(projectDir, "test.mod"), "cli_spawn");

                string gameExe = Path.Combine(fakeInstallDir, "swkotor.exe");
                WriteShellStubScript(gameExe, markerPath, 0);

                int exitCode = RunKotorCli(
                    alias + " default --installDir \"" + fakeInstallDir + "\" --wait",
                    projectDir,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(File.Exists(markerPath), Is.True, combined);
                Assert.That(File.Exists(Path.Combine(fakeInstallDir, "modules", "test.mod")), Is.True, combined);
                Assert.That(combined.ToLowerInvariant(), Does.Contain("launching game"));
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [Test]
        public void CliLaunch_DryRun_WithInstallDir_ResolvesSwkotorExe()
        {
            string installDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-resolve-cli-" + Guid.NewGuid().ToString("N"));
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                Directory.CreateDirectory(installDir);
                File.WriteAllText(Path.Combine(installDir, "chitin.key"), "fake-key");
                string gameExe = Path.Combine(installDir, "swkotor.exe");
                File.WriteAllBytes(gameExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", null);

                int exitCode = RunKotorCli(
                    "launch default --dry-run --installDir \"" + installDir + "\"",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain(Path.GetFullPath(gameExe)));
                Assert.That(combined.ToLowerInvariant(), Does.Contain("dry-run"));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(installDir);
            }
        }

        [Test]
        public void CliLaunch_DryRun_InstallDirWithoutExe_ExitsNonZero()
        {
            string installDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-resolve-cli-noexe-" + Guid.NewGuid().ToString("N"));
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                Directory.CreateDirectory(installDir);
                File.WriteAllText(Path.Combine(installDir, "chitin.key"), "fake-key");

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", null);

                int exitCode = RunKotorCli(
                    "launch default --dry-run --installDir \"" + installDir + "\"",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(1), combined);
                Assert.That(combined.ToLowerInvariant(), Does.Contain("could not resolve"));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(installDir);
            }
        }

        [Test]
        public void CliLaunch_DryRun_GameBinOverridesInstallDir()
        {
            string installDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-resolve-cli-priority-" + Guid.NewGuid().ToString("N"));
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                Directory.CreateDirectory(installDir);
                File.WriteAllText(Path.Combine(installDir, "chitin.key"), "fake-key");
                string customExe = Path.Combine(installDir, "custom.exe");
                File.WriteAllBytes(customExe, new byte[] { 0x4D, 0x5A });
                File.WriteAllBytes(Path.Combine(installDir, "swkotor.exe"), new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", null);

                int exitCode = RunKotorCli(
                    "launch default --dry-run --gameBin \"" + customExe + "\" --installDir \"" + installDir + "\"",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain(Path.GetFullPath(customExe)));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(installDir);
            }
        }

        [Test]
        public void CliLaunch_DryRun_KotorPathEnv_ResolvesSwkotorExe()
        {
            string installDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-env-kotor-" + Guid.NewGuid().ToString("N"));
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                Directory.CreateDirectory(installDir);
                File.WriteAllText(Path.Combine(installDir, "chitin.key"), "fake-key");
                string gameExe = Path.Combine(installDir, "swkotor.exe");
                File.WriteAllBytes(gameExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", installDir);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", null);

                int exitCode = RunKotorCli(
                    "launch default --dry-run",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain(Path.GetFullPath(gameExe)));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(installDir);
            }
        }

        [Test]
        public void CliLaunch_DryRun_K1PathEnv_ResolvesSwkotorExe()
        {
            string installDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-env-k1-" + Guid.NewGuid().ToString("N"));
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                Directory.CreateDirectory(installDir);
                File.WriteAllText(Path.Combine(installDir, "chitin.key"), "fake-key");
                string gameExe = Path.Combine(installDir, "swkotor.exe");
                File.WriteAllBytes(gameExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", installDir);
                Environment.SetEnvironmentVariable("K2_PATH", null);

                int exitCode = RunKotorCli(
                    "launch default --dry-run",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain(Path.GetFullPath(gameExe)));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(installDir);
            }
        }

        [Test]
        public void CliLaunch_DryRun_K2PathEnv_ResolvesSwkotor2Exe()
        {
            string installDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-env-k2-" + Guid.NewGuid().ToString("N"));
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                Directory.CreateDirectory(installDir);
                File.WriteAllText(Path.Combine(installDir, "chitin.key"), "fake-key");
                string gameExe = Path.Combine(installDir, "swkotor2.exe");
                File.WriteAllBytes(gameExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", installDir);

                int exitCode = RunKotorCli(
                    "launch default --dry-run",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain(Path.GetFullPath(gameExe)));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(installDir);
            }
        }

        [Test]
        public void CliLaunch_DryRun_InstallDir_PrefersK1OverTsl()
        {
            string installDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-resolve-cli-k1pref-" + Guid.NewGuid().ToString("N"));
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                Directory.CreateDirectory(installDir);
                File.WriteAllText(Path.Combine(installDir, "chitin.key"), "fake-key");
                string k1Exe = Path.Combine(installDir, "swkotor.exe");
                string tslExe = Path.Combine(installDir, "swkotor2.exe");
                File.WriteAllBytes(k1Exe, new byte[] { 0x4D, 0x5A });
                File.WriteAllBytes(tslExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", null);

                int exitCode = RunKotorCli(
                    "launch default --dry-run --installDir \"" + installDir + "\"",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain(Path.GetFullPath(k1Exe)));
                Assert.That(combined, Does.Not.Contain(Path.GetFullPath(tslExe)));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(installDir);
            }
        }

        [Test]
        public void CliLaunch_DryRun_InvalidGameBin_FallsBackToInstallDir()
        {
            string installDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-fallback-cli-" + Guid.NewGuid().ToString("N"));
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                Directory.CreateDirectory(installDir);
                string gameExe = Path.Combine(installDir, "swkotor.exe");
                File.WriteAllBytes(gameExe, new byte[] { 0x4D, 0x5A });
                string missingGameBin = Path.Combine(installDir, "missing-game.exe");

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", null);

                int exitCode = RunKotorCli(
                    "launch default --dry-run --gameBin \"" + missingGameBin + "\" --installDir \"" + installDir + "\"",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain(Path.GetFullPath(gameExe)));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(installDir);
            }
        }

        [Test]
        public void CliLaunch_DryRun_InstallDirOnlyTsl_ResolvesSwkotor2()
        {
            string installDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-tsl-cli-" + Guid.NewGuid().ToString("N"));
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                Directory.CreateDirectory(installDir);
                string gameExe = Path.Combine(installDir, "swkotor2.exe");
                File.WriteAllBytes(gameExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", null);

                int exitCode = RunKotorCli(
                    "launch default --dry-run --installDir \"" + installDir + "\"",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain(Path.GetFullPath(gameExe)));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(installDir);
            }
        }

        [Test]
        public void CliLaunch_DryRun_KotorPathWithoutChitin_ExitsNonZero()
        {
            string installDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-nochitin-cli-" + Guid.NewGuid().ToString("N"));
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                Directory.CreateDirectory(installDir);
                File.WriteAllBytes(Path.Combine(installDir, "swkotor.exe"), new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", installDir);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", null);

                int exitCode = RunKotorCli(
                    "launch default --dry-run",
                    RepoRoot,
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(1), combined);
                Assert.That(combined.ToLowerInvariant(), Does.Contain("could not resolve"));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(installDir);
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

        private static void WriteShellStubScript(string scriptPath, string markerPath, int exitCode)
        {
            string escapedMarker = markerPath.Replace("\"", "\\\"");
            File.WriteAllText(
                scriptPath,
                "#!/bin/sh\n" +
                "touch \"" + escapedMarker + "\"\n" +
                "exit " + exitCode + "\n");
            MakeExecutable(scriptPath);
        }

        private static void MakeExecutable(string path)
        {
            var chmod = new ProcessStartInfo
            {
                FileName = "/bin/chmod",
                Arguments = "+x \"" + path + "\"",
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (Process process = Process.Start(chmod))
            {
                process.WaitForExit();
                Assert.That(process.ExitCode, Is.EqualTo(0), "chmod failed for " + path);
            }
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
