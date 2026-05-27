using System.Collections.Generic;
using BioWare.Tools;
using NUnit.Framework;

namespace BioWare.Tests
{
    [TestFixture]
    public class ModuleGlobMatcherTests
    {
        [Test]
        public void MatchesAnyModuleGlob_NullOrEmptyPatterns_MatchesAll()
        {
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\tar_m01.mod", null), Is.True);
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\tar_m01.mod", new List<string>()), Is.True);
        }

        [Test]
        public void MatchesAnyModuleGlob_StarWildcard_MatchesAnyFilename()
        {
            var patterns = new List<string> { "*" };
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\danm13.rim", patterns), Is.True);
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\other.mod", patterns), Is.True);
        }

        [Test]
        public void MatchesAnyModuleGlob_PrefixPattern_MatchesCaseInsensitive()
        {
            var patterns = new List<string> { "tar_m02*" };
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\tar_m02aa.mod", patterns), Is.True);
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\TAR_M02BB.RIM", patterns), Is.True);
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\danm13.rim", patterns), Is.False);
        }

        [Test]
        public void MatchesAnyModuleGlob_QuestionMark_MatchesSingleCharacter()
        {
            var patterns = new List<string> { "tar_m0?.mod" };
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\tar_m01.mod", patterns), Is.True);
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\tar_m010.mod", patterns), Is.False);
        }

        [Test]
        public void MatchesAnyModuleGlob_MultiplePatterns_MatchesIfAnyPatternMatches()
        {
            var patterns = new List<string> { "dan*", "tar_m02*" };
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\danm13.rim", patterns), Is.True);
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\tar_m02aa.mod", patterns), Is.True);
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\unk_01.mod", patterns), Is.False);
        }

        [Test]
        public void MatchesAnyModuleGlob_EmptyModulePath_ReturnsFalseWhenFiltered()
        {
            var patterns = new List<string> { "tar*" };
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(null, patterns), Is.False);
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(string.Empty, patterns), Is.False);
        }

        [Test]
        public void MatchesAnyModuleGlob_ExactFilenamePattern_Matches()
        {
            var patterns = new List<string> { "tar_m02aa.mod" };
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\tar_m02aa.mod", patterns), Is.True);
            Assert.That(ModuleGlobMatcher.MatchesAnyModuleGlob(@"C:\kotor\modules\other.mod", patterns), Is.False);
        }
    }
}
