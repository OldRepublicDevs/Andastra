using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class LaunchCommandTests
    {
        private const string MinimalConfig = @"[package]
name = ""testpack""

[target]
name = ""default""
file = ""test.mod""
";

        [Test]
        public void Execute_InstallOnly_NoConfigDirectory_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-installonly-nocfg-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "default" }, null, null, false, true, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_InstallOnly_WithPackedModAndFakeInstallDir_CopiesToModules()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-installonly-happy-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-installonly-game-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(fakeInstallDir, "chitin.key"), "fake-key");
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);

                string modPath = Path.Combine(projectDir, "test.mod");
                WriteModWithUtc(modPath, "launch_inst_cre");

                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "default" }, null, fakeInstallDir, false, true, logger);

                string installedModPath = Path.Combine(fakeInstallDir, "modules", "test.mod");
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(installedModPath), Is.True);

                ERF installedMod = ERFAuto.ReadErf(installedModPath, ResourceType.MOD);
                Assert.That(installedMod.Get("launch_inst_cre", ResourceType.UTC), Is.Not.Null);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [Test]
        public void Execute_InstallOnly_DoesNotRequireGameBinary()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-installonly-nobin-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-installonly-nobin-game-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(fakeInstallDir, "chitin.key"), "fake-key");
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);

                string modPath = Path.Combine(projectDir, "test.mod");
                WriteModWithUtc(modPath, "launch_nobin_cr");

                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "default" }, null, fakeInstallDir, false, true, logger);

                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(Path.Combine(fakeInstallDir, "modules", "test.mod")), Is.True);
                Assert.That(File.Exists(Path.Combine(fakeInstallDir, "swkotor.exe")), Is.False);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [Test]
        public void Execute_DryRun_WithGameBin_PrintsResolvedPath()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, fakeExe, null, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_DryRun_WithInstallDir_ResolvesSwkotorExe()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-install-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, null, tempDir, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_DryRun_InvalidGameBin_FallsBackToInstallDir()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-fallback-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });
                string missingGameBin = Path.Combine(tempDir, "missing-game.exe");

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, missingGameBin, tempDir, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_DryRun_WithInstallDir_ResolvesTslExe()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-tsl-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string fakeExe = Path.Combine(tempDir, "swkotor2.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, null, tempDir, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_DryRun_NoResolvableBinary_ExitsNonZero()
        {
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");

            try
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, null, null, true, logger);
                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
            }
        }

        [Test]
        public void ResolveGameBinary_ExistingGameBin_ReturnsFullPath()
        {
            string tempDir = CreateTempLaunchDir();
            try
            {
                string relativeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(relativeExe, new byte[] { 0x4D, 0x5A });

                var logger = new StandardLogger();
                string resolved = LaunchCommand.ResolveGameBinary(relativeExe, null, logger);

                Assert.That(resolved, Is.EqualTo(Path.GetFullPath(relativeExe)));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveGameBinary_KotorPathEnvWithChitin_ResolvesSwkotorExe()
        {
            string tempDir = CreateTempLaunchDir();
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");

            try
            {
                File.WriteAllBytes(Path.Combine(tempDir, "chitin.key"), new byte[0]);
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", tempDir);
                Environment.SetEnvironmentVariable("K1_PATH", null);

                var logger = new StandardLogger();
                string resolved = LaunchCommand.ResolveGameBinary(null, null, logger);

                Assert.That(resolved, Is.EqualTo(Path.GetFullPath(fakeExe)));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveGameBinary_K1PathEnvWhenKotorPathUnset_ResolvesSwkotorExe()
        {
            string tempDir = CreateTempLaunchDir();
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");

            try
            {
                File.WriteAllBytes(Path.Combine(tempDir, "chitin.key"), new byte[0]);
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", tempDir);

                var logger = new StandardLogger();
                string resolved = LaunchCommand.ResolveGameBinary(null, null, logger);

                Assert.That(resolved, Is.EqualTo(Path.GetFullPath(fakeExe)));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveGameBinary_KotorPathWithoutChitin_ReturnsNull()
        {
            string tempDir = CreateTempLaunchDir();
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");

            try
            {
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", tempDir);
                Environment.SetEnvironmentVariable("K1_PATH", null);

                var logger = new StandardLogger();
                string resolved = LaunchCommand.ResolveGameBinary(null, null, logger);

                Assert.That(resolved, Is.Null);
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveGameBinary_InvalidInstallDir_ReturnsNull()
        {
            string tempDir = CreateTempLaunchDir();
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");

            try
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);

                string missingInstallDir = Path.Combine(tempDir, "missing-install");
                var logger = new StandardLogger();
                string resolved = LaunchCommand.ResolveGameBinary(null, missingInstallDir, logger);

                Assert.That(resolved, Is.Null);
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveGameBinary_InstallDirWithoutExe_ReturnsNull()
        {
            string tempDir = CreateTempLaunchDir();
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");

            try
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);

                var logger = new StandardLogger();
                string resolved = LaunchCommand.ResolveGameBinary(null, tempDir, logger);

                Assert.That(resolved, Is.Null);
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveGameBinary_GameBinTakesPriorityOverInstallDir()
        {
            string tempDir = CreateTempLaunchDir();

            try
            {
                string gameBinExe = Path.Combine(tempDir, "custom.exe");
                string installExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(gameBinExe, new byte[] { 0x4D, 0x5A });
                File.WriteAllBytes(installExe, new byte[] { 0x4D, 0x5A });

                var logger = new StandardLogger();
                string resolved = LaunchCommand.ResolveGameBinary(gameBinExe, tempDir, logger);

                Assert.That(resolved, Is.EqualTo(Path.GetFullPath(gameBinExe)));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveGameBinary_WithInstallDir_PrefersK1OverTsl()
        {
            string tempDir = CreateTempLaunchDir();

            try
            {
                File.WriteAllBytes(Path.Combine(tempDir, "swkotor.exe"), new byte[] { 0x4D, 0x5A });
                File.WriteAllBytes(Path.Combine(tempDir, "swkotor2.exe"), new byte[] { 0x4D, 0x5A });

                var logger = new StandardLogger();
                string resolved = LaunchCommand.ResolveGameBinary(null, tempDir, logger);

                Assert.That(resolved, Is.EqualTo(Path.GetFullPath(Path.Combine(tempDir, "swkotor.exe"))));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveGameBinary_InvalidGameBin_FallsBackToInstallDir()
        {
            string tempDir = CreateTempLaunchDir();

            try
            {
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });
                string missingGameBin = Path.Combine(tempDir, "missing-game.exe");

                var logger = new StandardLogger();
                string resolved = LaunchCommand.ResolveGameBinary(missingGameBin, tempDir, logger);

                Assert.That(resolved, Is.EqualTo(fakeExe));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveGameBinary_K2PathEnvWithChitin_ResolvesSwkotor2Exe()
        {
            string tempDir = CreateTempLaunchDir();
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                File.WriteAllBytes(Path.Combine(tempDir, "chitin.key"), new byte[0]);
                string fakeExe = Path.Combine(tempDir, "swkotor2.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", tempDir);

                var logger = new StandardLogger();
                string resolved = LaunchCommand.ResolveGameBinary(null, null, logger);

                Assert.That(resolved, Is.EqualTo(Path.GetFullPath(fakeExe)));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_DryRun_WithK2PathEnv_ResolvesSwkotor2Exe()
        {
            string tempDir = CreateTempLaunchDir();
            string priorKotorPath = Environment.GetEnvironmentVariable("KOTOR_PATH");
            string priorK1Path = Environment.GetEnvironmentVariable("K1_PATH");
            string priorK2Path = Environment.GetEnvironmentVariable("K2_PATH");

            try
            {
                File.WriteAllBytes(Path.Combine(tempDir, "chitin.key"), new byte[0]);
                string fakeExe = Path.Combine(tempDir, "swkotor2.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                Environment.SetEnvironmentVariable("KOTOR_PATH", null);
                Environment.SetEnvironmentVariable("K1_PATH", null);
                Environment.SetEnvironmentVariable("K2_PATH", tempDir);

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, null, null, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                Environment.SetEnvironmentVariable("KOTOR_PATH", priorKotorPath);
                Environment.SetEnvironmentVariable("K1_PATH", priorK1Path);
                Environment.SetEnvironmentVariable("K2_PATH", priorK2Path);
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void TryStartGameProcess_WithShellStub_Wait_ReturnsExitCode()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.Ignore("Shell stub launch tests require Linux.");
            }

            string markerPath = Path.Combine(Path.GetTempPath(), "kotorcli-launch-marker-" + Guid.NewGuid().ToString("N"));
            string scriptPath = Path.Combine(Path.GetTempPath(), "kotorcli-launch-stub-" + Guid.NewGuid().ToString("N") + ".sh");
            WriteShellStubScript(scriptPath, markerPath, 42);

            try
            {
                var logger = new StandardLogger();
                bool started = LaunchCommand.TryStartGameProcess(
                    scriptPath,
                    Path.GetDirectoryName(scriptPath),
                    true,
                    logger,
                    out int processExitCode);

                Assert.That(started, Is.True);
                Assert.That(processExitCode, Is.EqualTo(42));
                Assert.That(File.Exists(markerPath), Is.True);
            }
            finally
            {
                DeleteFileSafe(markerPath);
                DeleteFileSafe(scriptPath);
            }
        }

        [Test]
        public void Execute_FullLaunch_WithWait_InstallsAndRunsStub()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Assert.Ignore("Shell stub launch tests require Linux.");
            }

            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-full-proj-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-full-game-" + Guid.NewGuid().ToString("N"));
            string markerPath = Path.Combine(fakeInstallDir, "launch-ran.marker");
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(fakeInstallDir, "chitin.key"), "fake-key");
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                WriteModWithUtc(Path.Combine(projectDir, "test.mod"), "launch_spawn");

                string gameExe = Path.Combine(fakeInstallDir, "swkotor.exe");
                WriteShellStubScript(gameExe, markerPath, 0);

                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "default" }, null, fakeInstallDir, false, false, true, logger);

                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(markerPath), Is.True);
                Assert.That(File.Exists(Path.Combine(fakeInstallDir, "modules", "test.mod")), Is.True);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [Test]
        public void Execute_WithoutDryRun_StillExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, fakeExe, null, false, logger);
                Assert.That(exitCode, Is.EqualTo(1));
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

        private static string WriteShellStubScript(string scriptPath, string markerPath, int exitCode)
        {
            string escapedMarker = markerPath.Replace("\"", "\\\"");
            File.WriteAllText(
                scriptPath,
                "#!/bin/sh\n" +
                "touch \"" + escapedMarker + "\"\n" +
                "exit " + exitCode + "\n");
            MakeExecutable(scriptPath);
            return scriptPath;
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

        private static string CreateTempLaunchDir()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-resolve-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            return tempDir;
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
