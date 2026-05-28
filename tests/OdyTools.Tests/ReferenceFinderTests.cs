using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.RIM;
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
                Assert.That(ReferenceFinder.FindScriptReferences(installation, "   "), Is.Empty);
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
        public void FindTagInGffBytes_CaseSensitive_RequiresExactCase()
        {
            var utc = new UTC();
            utc.Tag = "TestTag";

            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            var insensitive = new ReferenceSearchOptions { CaseSensitive = false };
            Assert.That(ReferenceFinder.FindTagInGffBytes(bytes, "testtag", insensitive), Is.Not.Empty);

            var sensitive = new ReferenceSearchOptions { CaseSensitive = true };
            Assert.That(ReferenceFinder.FindTagInGffBytes(bytes, "testtag", sensitive), Is.Empty);
            Assert.That(ReferenceFinder.FindTagInGffBytes(bytes, "TestTag", sensitive), Is.Not.Empty);
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
        public void FindScriptReferences_ModuleMod_ReturnsFieldPath()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-mod-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            WriteModuleWithScriptReference(Path.Combine(modulesDir, "test_mod.mod"), "k_mod_ref");

            try
            {
                var installation = new Installation(installRoot);
                var options = new ReferenceSearchOptions
                {
                    SearchChitin = false,
                    SearchModules = true,
                    SearchOverride = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindScriptReferences(
                    installation,
                    "k_mod_ref",
                    options);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "ScriptHeartbeat" && r.MatchedValue == "k_mod_ref"));
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
        public void FindScriptReferences_NoModules_SkipsModuleUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-nomod-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            WriteModuleWithScriptReference(Path.Combine(modulesDir, "test_mod.mod"), "k_mod_ref");

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
                    "k_mod_ref",
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
        public void FindScriptReferences_ModuleGlob_FiltersNonMatchingModule()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-modglob-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            WriteModuleWithScriptReference(Path.Combine(modulesDir, "tar_m01.mod"), "k_tar_only");
            WriteModuleWithScriptReference(Path.Combine(modulesDir, "danm13.rim"), "k_dan_only");

            try
            {
                var installation = new Installation(installRoot);
                var tarOptions = new ReferenceSearchOptions
                {
                    SearchChitin = false,
                    SearchModules = true,
                    SearchOverride = false,
                    ModuleGlobFilters = new List<string> { "tar_m01*" }
                };

                List<ReferenceSearchResult> tarResults = ReferenceFinder.FindScriptReferences(
                    installation,
                    "k_tar_only",
                    tarOptions);
                Assert.That(tarResults, Is.Not.Empty);

                List<ReferenceSearchResult> danMissResults = ReferenceFinder.FindScriptReferences(
                    installation,
                    "k_dan_only",
                    tarOptions);
                Assert.That(danMissResults, Is.Empty);

                var danOptions = new ReferenceSearchOptions
                {
                    SearchChitin = false,
                    SearchModules = true,
                    SearchOverride = false,
                    ModuleGlobFilters = new List<string> { "danm13*" }
                };

                List<ReferenceSearchResult> danHitResults = ReferenceFinder.FindScriptReferences(
                    installation,
                    "k_dan_only",
                    danOptions);
                Assert.That(danHitResults, Is.Not.Empty);
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
        public void FindTagReferences_ModuleMod_ReturnsFieldPath()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-modtag-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = "mod_tag_ref";
            WriteModuleWithUtc(Path.Combine(modulesDir, "test_mod.mod"), utc);

            try
            {
                var installation = new Installation(installRoot);
                var options = new ReferenceSearchOptions
                {
                    SearchChitin = false,
                    SearchModules = true,
                    SearchOverride = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindTagReferences(
                    installation,
                    "mod_tag_ref",
                    options);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "Tag" && r.MatchedValue == "mod_tag_ref"));
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
        public void FindTemplateResRefReferences_ModuleMod_ReturnsFieldPath()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-modtpl-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.ResRef = new ResRef("p_mod_tpl");
            WriteModuleWithUtc(Path.Combine(modulesDir, "test_mod.mod"), utc);

            try
            {
                var installation = new Installation(installRoot);
                var options = new ReferenceSearchOptions
                {
                    SearchChitin = false,
                    SearchModules = true,
                    SearchOverride = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindTemplateResRefReferences(
                    installation,
                    "p_mod_tpl",
                    options);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "TemplateResRef" && r.MatchedValue == "p_mod_tpl"));
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
        public void FindConversationResRefReferences_ModuleMod_ReturnsFieldPath()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-modconv-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Conversation = new ResRef("mod_dlg_ref");
            WriteModuleWithUtc(Path.Combine(modulesDir, "test_mod.mod"), utc);

            try
            {
                var installation = new Installation(installRoot);
                var options = new ReferenceSearchOptions
                {
                    SearchChitin = false,
                    SearchModules = true,
                    SearchOverride = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindConversationResRefReferences(
                    installation,
                    "mod_dlg_ref",
                    options);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "Conversation" && r.MatchedValue == "mod_dlg_ref"));
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
        public void FindTagReferences_NoModules_SkipsModuleUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-notagmod-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = "mod_tag_ref";
            WriteModuleWithUtc(Path.Combine(modulesDir, "test_mod.mod"), utc);

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
                    "mod_tag_ref",
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
        public void FindTemplateResRefReferences_NoModules_SkipsModuleUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-notplmod-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.ResRef = new ResRef("p_mod_tpl");
            WriteModuleWithUtc(Path.Combine(modulesDir, "test_mod.mod"), utc);

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
                    "p_mod_tpl",
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
        public void FindConversationResRefReferences_NoModules_SkipsModuleUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-noconvmod-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Conversation = new ResRef("mod_dlg_ref");
            WriteModuleWithUtc(Path.Combine(modulesDir, "test_mod.mod"), utc);

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
                    "mod_dlg_ref",
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
        public void FindFieldValueReferences_ModuleMod_FindsTag()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-fldmod-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = "mod_fld_tag";
            WriteModuleWithUtc(Path.Combine(modulesDir, "test_mod.mod"), utc);

            try
            {
                var installation = new Installation(installRoot);
                var fieldNames = new HashSet<string> { "Tag" };
                var options = new ReferenceSearchOptions
                {
                    SearchChitin = false,
                    SearchModules = true,
                    SearchOverride = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindFieldValueReferences(
                    installation,
                    "mod_fld_tag",
                    fieldNames,
                    options);

                Assert.That(results, Is.Not.Empty);
                Assert.That(results, Has.Some.Matches<ReferenceSearchResult>(
                    r => r.FieldPath == "Tag" && r.MatchedValue == "mod_fld_tag"));
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
        public void FindFieldValueReferences_NoModules_SkipsModuleUtc()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-nofldmod-" + Guid.NewGuid().ToString("N"));
            string modulesDir = Path.Combine(installRoot, "modules");
            Directory.CreateDirectory(modulesDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = "mod_fld_tag";
            WriteModuleWithUtc(Path.Combine(modulesDir, "test_mod.mod"), utc);

            try
            {
                var installation = new Installation(installRoot);
                var fieldNames = new HashSet<string> { "Tag" };
                var options = new ReferenceSearchOptions
                {
                    SearchChitin = false,
                    SearchModules = false,
                    SearchOverride = false
                };

                List<ReferenceSearchResult> results = ReferenceFinder.FindFieldValueReferences(
                    installation,
                    "mod_fld_tag",
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
        public void FindFieldValueReferences_EmptyNeedleReturnsEmpty()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "ref-fld-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            try
            {
                var installation = new Installation(installRoot);
                var fieldNames = new HashSet<string> { "Tag" };
                Assert.That(ReferenceFinder.FindFieldValueReferences(installation, "", fieldNames), Is.Empty);
                Assert.That(ReferenceFinder.FindFieldValueReferences(installation, "   ", fieldNames), Is.Empty);
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
        public void FindFieldValueReferences_NullInstallation_ThrowsArgumentNullException()
        {
            var fieldNames = new HashSet<string> { "Tag" };
            Assert.Throws<ArgumentNullException>(() =>
                ReferenceFinder.FindFieldValueReferences(null, "find_me_tag", fieldNames));
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
        public void FindScriptReferences_NullInstallation_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReferenceFinder.FindScriptReferences(null, "k_test_hb"));
        }

        [Test]
        public void FindTemplateResRefReferences_NullInstallation_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReferenceFinder.FindTemplateResRefReferences(null, "p_unique_tpl"));
        }

        [Test]
        public void FindConversationResRefReferences_NullInstallation_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ReferenceFinder.FindConversationResRefReferences(null, "test_dlg_ref"));
        }

        [Test]
        public void FindScriptResRefInNcsBytes_CaseSensitive_RequiresExactCase()
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes("abc k_Test_Hb xyz");

            var insensitive = new ReferenceSearchOptions { CaseSensitive = false };
            Assert.That(ReferenceFinder.FindScriptResRefInNcsBytes(data, "k_test_hb", insensitive), Is.Not.Empty);

            var sensitive = new ReferenceSearchOptions { CaseSensitive = true };
            Assert.That(ReferenceFinder.FindScriptResRefInNcsBytes(data, "k_test_hb", sensitive), Is.Empty);
            Assert.That(ReferenceFinder.FindScriptResRefInNcsBytes(data, "k_Test_Hb", sensitive), Is.Not.Empty);
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

        private static void WriteModuleWithScriptReference(string modulePath, string scriptResRef)
        {
            var utc = new UTC();
            utc.OnHeartbeat = new ResRef(scriptResRef);
            WriteModuleWithUtc(modulePath, utc);
        }

        private static void WriteModuleWithUtc(string modulePath, UTC utc)
        {
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            string ext = Path.GetExtension(modulePath);
            if (ext.Equals(".rim", StringComparison.OrdinalIgnoreCase))
            {
                var rim = new RIM();
                rim.SetData("test_npc", ResourceType.UTC, bytes);
                RIMAuto.WriteRim(rim, modulePath, ResourceType.RIM);
            }
            else
            {
                var mod = new ERF(ERFType.MOD);
                mod.SetData("test_npc", ResourceType.UTC, bytes);
                ERFAuto.WriteErf(mod, modulePath, ResourceType.MOD);
            }
        }
    }
}
