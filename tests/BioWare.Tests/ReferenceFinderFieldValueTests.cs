using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Tools;
using NUnit.Framework;

namespace BioWare.Tests
{
    [TestFixture]
    public class ReferenceFinderFieldValueTests
    {
        [Test]
        public void FindFieldValueInGffBytes_TagField_Matches()
        {
            var utc = new UTC();
            utc.Tag = "find_me_tag";

            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            var fieldNames = new HashSet<string> { "Tag" };
            List<string> paths = ReferenceFinder.FindFieldValueInGffBytes(bytes, "find_me_tag", null, fieldNames);

            Assert.That(paths, Has.Some.EqualTo("Tag"));
        }

        [Test]
        public void FindFieldValueReferences_OverrideUtc_FindsTag()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "field-value-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var utc = new UTC();
            utc.Tag = "find_me_tag";
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            try
            {
                var installation = new Installation(installRoot);
                var fieldNames = new HashSet<string> { "Tag" };
                List<ReferenceSearchResult> results = ReferenceFinder.FindFieldValueReferences(
                    installation,
                    "find_me_tag",
                    fieldNames,
                    new ReferenceSearchOptions { SearchOverride = true, SearchChitin = false, SearchModules = false });

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "Tag" && r.MatchedValue == "find_me_tag"));
            }
            finally
            {
                try
                {
                    Directory.Delete(installRoot, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }
    }
}
