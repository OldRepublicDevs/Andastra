using System;
using System.IO;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ConfigCommandTests
    {
        [Test]
        public void Execute_LocalSet_WritesUserConfigFile()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-config-set-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string configKey = "gamePath";
            const string configValue = "/opt/kotor";

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = ConfigCommand.Execute(configKey, configValue, false, true, false, false, false, false, logger);

                string userConfigPath = Path.Combine(projectDir, ".kotorcli", "user.cfg");
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(userConfigPath), Is.True);
                Assert.That(File.ReadAllText(userConfigPath), Does.Contain(configKey));
                Assert.That(File.ReadAllText(userConfigPath), Does.Contain(configValue));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_LocalUnset_RemovesKey()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-config-unset-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string configKey = "editor";

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                Assert.That(ConfigCommand.Execute(configKey, "vim", false, true, false, false, false, false, logger), Is.EqualTo(0));

                string userConfigPath = Path.Combine(projectDir, ".kotorcli", "user.cfg");
                Assert.That(File.ReadAllText(userConfigPath), Does.Contain(configKey));

                int unsetExit = ConfigCommand.Execute(configKey, null, false, true, false, false, true, false, logger);
                Assert.That(unsetExit, Is.EqualTo(0));
                Assert.That(File.ReadAllText(userConfigPath), Does.Not.Contain(configKey + " ="));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_LocalListEmpty_ExitsZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-config-list-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = ConfigCommand.Execute(null, null, false, true, false, false, false, true, logger);

                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_NoOperation_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-config-noop-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = ConfigCommand.Execute(null, null, false, true, false, false, false, false, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
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
