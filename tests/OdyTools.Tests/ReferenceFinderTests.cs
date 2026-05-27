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

namespace OdyTools.Tests
{
    [TestFixture]
    public class ReferenceFinderTests
    {
        [Test]
        public void FindScriptResRefInGffBytes_FindsUtcScriptField()
        {
            var utc = new UTC();
            utc.OnHeartbeat = new ResRef("k_test_hb");

            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            List<string> paths = ReferenceFinder.FindScriptResRefInGffBytes(bytes, "k_test_hb");

            Assert.That(paths, Is.Not.Empty);
            Assert.That(paths, Has.Some.EqualTo("ScriptHeartbeat"));
        }

        [Test]
        public void FindScriptResRefInGffBytes_EmptyNeedleReturnsEmpty()
        {
            var utc = new UTC();
            utc.OnHeartbeat = new ResRef("k_test_hb");
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            Assert.That(ReferenceFinder.FindScriptResRefInGffBytes(bytes, ""), Is.Empty);
            Assert.That(ReferenceFinder.FindScriptResRefInGffBytes(bytes, "   "), Is.Empty);
        }

        [Test]
        public void FindScriptResRefInGffBytes_NoMatchReturnsEmpty()
        {
            var utc = new UTC();
            utc.OnHeartbeat = new ResRef("k_other");
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            Assert.That(ReferenceFinder.FindScriptResRefInGffBytes(bytes, "k_test_hb"), Is.Empty);
        }

        [Test]
        public void FindScriptReferences_OverrideUtc_ReturnsFieldPath()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-find-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.OnHeartbeat = new ResRef("k_test_hb");
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            try
            {
                var installation = new Installation(installRoot);
                var options = new ReferenceSearchOptions
                {
                    SearchChitin = false,
                    SearchModules = false,
                    SearchOverride = true
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindScriptReferences(
                    installation,
                    "k_test_hb",
                    options);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "ScriptHeartbeat"));
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

        [Test]
        public void FindScriptReferences_EmptyNeedleReturnsEmpty()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-find-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            try
            {
                var installation = new Installation(installRoot);
                Assert.That(ReferenceFinder.FindScriptReferences(installation, ""), Is.Empty);
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

        [Test]
        public void FindTagInGffBytes_FindsUtcTagField()
        {
            var utc = new UTC();
            utc.Tag = "test_creature_tag";

            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            List<string> paths = ReferenceFinder.FindTagInGffBytes(bytes, "test_creature_tag");

            Assert.That(paths, Is.Not.Empty);
            Assert.That(paths, Has.Some.EqualTo("Tag"));
        }

        [Test]
        public void FindTagInGffBytes_PartialMatch_FindsSubstring()
        {
            var utc = new UTC();
            utc.Tag = "test_creature_tag";

            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            var options = new ReferenceSearchOptions { PartialMatch = true };
            List<string> paths = ReferenceFinder.FindTagInGffBytes(bytes, "creature", options);

            Assert.That(paths, Is.Not.Empty);
        }

        [Test]
        public void FindTemplateResRefInGffBytes_FindsUtcTemplateResRef()
        {
            var utc = new UTC();
            utc.ResRef = new ResRef("p_carth");

            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            List<string> paths = ReferenceFinder.FindTemplateResRefInGffBytes(bytes, "p_carth");

            Assert.That(paths, Is.Not.Empty);
            Assert.That(paths, Has.Some.EqualTo("TemplateResRef"));
        }

        [Test]
        public void FindTagReferences_OverrideUtc_ReturnsFieldPath()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-tag-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = "unique_tag_ref";
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            try
            {
                var installation = new Installation(installRoot);
                var options = new ReferenceSearchOptions
                {
                    SearchChitin = false,
                    SearchModules = false,
                    SearchOverride = true
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindTagReferences(
                    installation,
                    "unique_tag_ref",
                    options);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "Tag"));
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
