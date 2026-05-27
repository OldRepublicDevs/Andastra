using System;
using System.IO;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class InstallCommandTests
    {
        private const string MinimalConfig = @"[package]
name = ""testpack""

[target]
name = ""default""
file = ""test.mod""
";

        [Test]
        public void Execute_NoConfigDirectory_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-nocfg-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = InstallCommand.Execute(new[] { "default" }, null, false, false, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_UnknownTarget_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-unknown-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = InstallCommand.Execute(new[] { "missing-target" }, null, false, false, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_InstallDirMissingChitin_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-nochitin-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-fake-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = InstallCommand.Execute(new[] { "default" }, fakeInstallDir, false, false, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
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
