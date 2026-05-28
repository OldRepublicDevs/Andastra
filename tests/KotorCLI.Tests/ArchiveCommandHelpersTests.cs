using System;
using System.IO;
using System.Text;
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

        [Test]
        public void MatchesFilter_EmptyPattern_ReturnsTrue()
        {
            Assert.That(ArchiveCommandHelpers.MatchesFilter("anything.utc", null), Is.True);
            Assert.That(ArchiveCommandHelpers.MatchesFilter("anything.utc", string.Empty), Is.True);
        }

        [Test]
        public void MatchesFilter_Wildcard_IsCaseInsensitiveByDefault()
        {
            Assert.That(ArchiveCommandHelpers.MatchesFilter("Creature.UTC", "creature.*"), Is.True);
            Assert.That(ArchiveCommandHelpers.MatchesFilter("other.utc", "creature.*"), Is.False);
        }

        [Test]
        public void MatchesFilter_CaseSensitive_SubstringRequiresExactCase()
        {
            Assert.That(ArchiveCommandHelpers.MatchesFilter("Needle", "needle", true), Is.False);
            Assert.That(ArchiveCommandHelpers.MatchesFilter("Needle", "Need", true), Is.True);
        }

        [Test]
        public void ContentMatches_FindsUtf8Substring()
        {
            byte[] data = Encoding.UTF8.GetBytes("prefix archive-test suffix");
            Assert.That(ArchiveCommandHelpers.ContentMatches(data, "archive-test", false), Is.True);
        }

        [Test]
        public void ContentMatches_NullOrEmptyData_ReturnsFalse()
        {
            Assert.That(ArchiveCommandHelpers.ContentMatches(null, "needle", false), Is.False);
            Assert.That(ArchiveCommandHelpers.ContentMatches(new byte[0], "needle", false), Is.False);
        }

        [Test]
        public void ContentMatches_IsCaseInsensitiveByDefault()
        {
            byte[] data = Encoding.UTF8.GetBytes("Archive-Test");
            Assert.That(ArchiveCommandHelpers.ContentMatches(data, "archive-test", false), Is.True);
            Assert.That(ArchiveCommandHelpers.ContentMatches(data, "archive-test", true), Is.False);
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
