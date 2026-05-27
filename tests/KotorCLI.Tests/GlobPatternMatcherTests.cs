using System;
using System.IO;
using System.Linq;
using KotorCLI;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class GlobPatternMatcherTests
    {
        [Test]
        public void FindFilesMatchingPattern_RecursiveGlob_FindsNestedJson()
        {
            string root = Path.Combine(Path.GetTempPath(), "kotorcli-glob-rec-" + Guid.NewGuid().ToString("N"));

            try
            {
                string nestedDir = Path.Combine(root, "src", "nested");
                Directory.CreateDirectory(nestedDir);
                string jsonPath = Path.Combine(nestedDir, "creature.utc.json");
                File.WriteAllText(jsonPath, "{}");
                File.WriteAllText(Path.Combine(root, "src", "readme.txt"), "skip");

                var matches = GlobPatternMatcher.FindFilesMatchingPattern(root, "src/**/*.json");

                Assert.That(matches.Count, Is.EqualTo(1));
                Assert.That(matches[0], Is.EqualTo(jsonPath));
            }
            finally
            {
                DeleteDirectorySafe(root);
            }
        }

        [Test]
        public void FindFilesMatchingPattern_ExactRelativePath_ReturnsExistingFile()
        {
            string root = Path.Combine(Path.GetTempPath(), "kotorcli-glob-exact-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(root);
                string jsonPath = Path.Combine(root, "sample.utc.json");
                File.WriteAllText(jsonPath, "{}");

                var matches = GlobPatternMatcher.FindFilesMatchingPattern(root, "sample.utc.json");

                Assert.That(matches.Count, Is.EqualTo(1));
                Assert.That(matches[0], Is.EqualTo(jsonPath));
            }
            finally
            {
                DeleteDirectorySafe(root);
            }
        }

        [Test]
        public void FindFilesMatchingPattern_ShallowWildcard_MatchesRootJsonOnly()
        {
            string root = Path.Combine(Path.GetTempPath(), "kotorcli-glob-shallow-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(root);
                string rootJson = Path.Combine(root, "root.utc.json");
                File.WriteAllText(rootJson, "{}");

                string nestedDir = Path.Combine(root, "nested");
                Directory.CreateDirectory(nestedDir);
                File.WriteAllText(Path.Combine(nestedDir, "nested.utc.json"), "{}");

                var matches = GlobPatternMatcher.FindFilesMatchingPattern(root, "*.json");

                Assert.That(matches.Count, Is.EqualTo(1));
                Assert.That(matches.Single(), Is.EqualTo(rootJson));
            }
            finally
            {
                DeleteDirectorySafe(root);
            }
        }

        [Test]
        public void MatchPattern_ExtensionWildcard_IsCaseInsensitive()
        {
            Assert.That(GlobPatternMatcher.MatchPattern("creature.UTC.json", "*.json"), Is.True);
            Assert.That(GlobPatternMatcher.MatchPattern("creature.utc.txt", "*.json"), Is.False);
        }

        [Test]
        public void MatchPattern_PathSegments_MatchesNestedRelativePath()
        {
            Assert.That(GlobPatternMatcher.MatchPattern("src/nested/creature.utc.json", "src/**/*.json"), Is.True);
            Assert.That(GlobPatternMatcher.MatchPattern("other/nested/creature.utc.json", "src/**/*.json"), Is.False);
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
