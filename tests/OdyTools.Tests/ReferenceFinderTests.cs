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
        public void FindConversationResRefInGffBytes_FindsUtcConversationResRef()
        {
            var utc = new UTC();
            utc.Conversation = new ResRef("test_dlg");

            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            List<string> paths = ReferenceFinder.FindConversationResRefInGffBytes(bytes, "test_dlg");

            Assert.That(paths, Is.Not.Empty);
            Assert.That(paths, Has.Some.EqualTo("Conversation"));
        }

        [Test]
        public void FindConversationResRefReferences_OverrideUtc_ReturnsFieldPath()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-conv-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Conversation = new ResRef("test_dlg_ref");
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

                List<ReferenceSearchResult> results = ReferenceFinder.FindConversationResRefReferences(
                    installation,
                    "test_dlg_ref",
                    options);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "Conversation" && r.MatchedValue == "test_dlg_ref"));
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
        public void FindScriptReferences_OverrideNcs_ReturnsOffsetPath()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-ncs-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            byte[] ncsBytes = System.Text.Encoding.ASCII.GetBytes("prefix k_ncs_ref suffix");
            File.WriteAllBytes(Path.Combine(overrideDir, "test_script.ncs"), ncsBytes);

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
                    "k_ncs_ref",
                    options);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath != null && r.FieldPath.StartsWith("offset_") && r.MatchedValue == "k_ncs_ref"));
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
        public void FindTemplateResRefReferences_OverrideUtc_ReturnsFieldPath()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-templ-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.ResRef = new ResRef("p_unique_tpl");
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

                List<ReferenceSearchResult> results = ReferenceFinder.FindTemplateResRefReferences(
                    installation,
                    "p_unique_tpl",
                    options);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "TemplateResRef" && r.MatchedValue == "p_unique_tpl"));
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

        [Test]
        public void FindScriptReferences_NoOverride_SkipsOverrideUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-find-noovr-" + Guid.NewGuid().ToString("N"));
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
                    SearchOverride = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindScriptReferences(
                    installation,
                    "k_test_hb",
                    options);

                Assert.That(results, Is.Empty);
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
        public void FindTagReferences_NoOverride_SkipsOverrideUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-tag-noovr-" + Guid.NewGuid().ToString("N"));
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
                    SearchOverride = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindTagReferences(
                    installation,
                    "unique_tag_ref",
                    options);

                Assert.That(results, Is.Empty);
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
        public void FindTemplateResRefReferences_NoOverride_SkipsOverrideUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-tpl-noovr-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.ResRef = new ResRef("p_unique_tpl");
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
                    SearchOverride = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindTemplateResRefReferences(
                    installation,
                    "p_unique_tpl",
                    options);

                Assert.That(results, Is.Empty);
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
        public void FindConversationResRefReferences_NoOverride_SkipsOverrideUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-conv-noovr-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Conversation = new ResRef("test_dlg_ref");
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
                    SearchOverride = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindConversationResRefReferences(
                    installation,
                    "test_dlg_ref",
                    options);

                Assert.That(results, Is.Empty);
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
        public void FindTagReferences_PartialMatch_OverrideUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-tag-partial-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = "test_creature_tag";
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
                    SearchOverride = true,
                    PartialMatch = true
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindTagReferences(
                    installation,
                    "creature",
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

        [Test]
        public void FindFieldValueReferences_OverrideUtc_FindsTag()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-field-value-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = "find_me_tag";
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            try
            {
                var installation = new Installation(installRoot);
                var fieldNames = new HashSet<string> { "Tag" };
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = true,
                    SearchChitin = false,
                    SearchModules = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindFieldValueReferences(
                    installation,
                    "find_me_tag",
                    fieldNames,
                    options);

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

        [Test]
        public void FindFieldValueReferences_NoOverride_SkipsOverrideUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-fld-noovr-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = "find_me_tag";
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            try
            {
                var installation = new Installation(installRoot);
                var fieldNames = new HashSet<string> { "Tag" };
                var options = new ReferenceSearchOptions
                {
                    SearchOverride = false,
                    SearchChitin = false,
                    SearchModules = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindFieldValueReferences(
                    installation,
                    "find_me_tag",
                    fieldNames,
                    options);

                Assert.That(results, Is.Empty);
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
        public void FindTagReferences_EmptyNeedleReturnsEmpty()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-tag-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            try
            {
                var installation = new Installation(installRoot);
                Assert.That(ReferenceFinder.FindTagReferences(installation, ""), Is.Empty);
                Assert.That(ReferenceFinder.FindTagReferences(installation, "   "), Is.Empty);
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
        public void FindTemplateResRefReferences_EmptyNeedleReturnsEmpty()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-templ-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            try
            {
                var installation = new Installation(installRoot);
                Assert.That(ReferenceFinder.FindTemplateResRefReferences(installation, ""), Is.Empty);
                Assert.That(ReferenceFinder.FindTemplateResRefReferences(installation, "   "), Is.Empty);
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
        public void FindConversationResRefReferences_EmptyNeedleReturnsEmpty()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-conv-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            try
            {
                var installation = new Installation(installRoot);
                Assert.That(ReferenceFinder.FindConversationResRefReferences(installation, ""), Is.Empty);
                Assert.That(ReferenceFinder.FindConversationResRefReferences(installation, "   "), Is.Empty);
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
        public void FindTagReferences_NullInstallation_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReferenceFinder.FindTagReferences(null, "tag_needle"));
        }

        [Test]
        public void FindScriptResRefInNcsBytes_FindsEmbeddedResRef()
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes("abc k_test_hb xyz");
            List<string> paths = ReferenceFinder.FindScriptResRefInNcsBytes(data, "k_test_hb");

            Assert.That(paths, Is.Not.Empty);
            Assert.That(paths, Has.Some.StartsWith("offset_"));
        }

        [Test]
        public void FindScriptResRefInNcsBytes_NoMatchReturnsEmpty()
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes("abc def");
            Assert.That(ReferenceFinder.FindScriptResRefInNcsBytes(data, "missing"), Is.Empty);
        }
    }
}
