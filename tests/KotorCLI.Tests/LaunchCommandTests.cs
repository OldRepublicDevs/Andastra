using System;
using System.IO;
using BioWare.Resource;
using BioWare.Resource.Formats.KEY;
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
