using System;
using System.IO;
using KotorCLI.Commands;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ArchiveCommandHelpersTests
    {
        [Test]
        public void ResolveSiblingKeyPath_PrefersChitinKeyWhenPresent()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-keypath-chitin-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(tempDir);
                string bifPath = Path.Combine(tempDir, "data.bif");
                string chitinPath = Path.Combine(tempDir, "chitin.key");
                string stemKeyPath = Path.Combine(tempDir, "data.key");
                File.WriteAllText(bifPath, "bif");
                File.WriteAllText(chitinPath, "key");
                File.WriteAllText(stemKeyPath, "key");

                string resolved = ArchiveCommandHelpers.ResolveSiblingKeyPath(bifPath);
                Assert.That(resolved, Is.EqualTo(chitinPath));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveSiblingKeyPath_UsesStemKeyWhenChitinMissing()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-keypath-stem-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(tempDir);
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string stemKeyPath = Path.Combine(tempDir, "sample.key");
                File.WriteAllText(bifPath, "bif");
                File.WriteAllText(stemKeyPath, "key");

                string resolved = ArchiveCommandHelpers.ResolveSiblingKeyPath(bifPath);
                Assert.That(resolved, Is.EqualTo(stemKeyPath));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void ResolveSiblingKeyPath_ReturnsNullWhenNoKeyFound()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-keypath-none-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(tempDir);
                string bifPath = Path.Combine(tempDir, "orphan.bif");
                File.WriteAllText(bifPath, "bif");

                string resolved = ArchiveCommandHelpers.ResolveSiblingKeyPath(bifPath);
                Assert.That(resolved, Is.Null);
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
