using System;
using System.IO;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class LaunchCommandTests
    {
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
