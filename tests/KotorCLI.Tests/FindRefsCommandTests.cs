using System;
using System.Collections.Generic;
using System.IO;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Tools;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class FindRefsCommandTests
    {
        [Test]
        public void Execute_ScriptReference_InOverride_ExitsZero()
        {
            string installRoot = CreateInstallWithScriptReference("k_cli_ref");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindRefsCommand.Execute(
                    "k_cli_ref",
                    installRoot,
                    "script",
                    overrideOnly: true,
                    noChitin: true,
                    noModules: true,
                    caseSensitive: false,
                    partialMatch: false,
                    logger);

                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_ConversationReference_InOverride_ExitsZero()
        {
            string installRoot = CreateInstallWithConversationReference("dlg_cli_ref");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindRefsCommand.Execute(
                    "dlg_cli_ref",
                    installRoot,
                    "conversation",
                    overrideOnly: true,
                    noChitin: true,
                    noModules: true,
                    caseSensitive: false,
                    partialMatch: false,
                    logger);

                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NoMatch_ExitsNonZero()
        {
            string installRoot = CreateInstallWithScriptReference("k_cli_ref");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindRefsCommand.Execute(
                    "missing_script",
                    installRoot,
                    "script",
                    overrideOnly: true,
                    noChitin: true,
                    noModules: true,
                    caseSensitive: false,
                    partialMatch: false,
                    logger);

                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_EmptyNeedle_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = FindRefsCommand.Execute(
                "   ",
                Path.GetTempPath(),
                "script",
                overrideOnly: true,
                noChitin: true,
                noModules: true,
                caseSensitive: false,
                partialMatch: false,
                logger);

            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void Execute_UnsupportedType_ExitsNonZero()
        {
            string installRoot = CreateInstallWithScriptReference("k_cli_ref");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindRefsCommand.Execute(
                    "k_cli_ref",
                    installRoot,
                    "unknown",
                    overrideOnly: true,
                    noChitin: true,
                    noModules: true,
                    caseSensitive: false,
                    partialMatch: false,
                    logger);

                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_TemplateReference_InOverride_ExitsZero()
        {
            string installRoot = CreateInstallWithTemplateReference("p_test_tpl");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindRefsCommand.Execute(
                    "p_test_tpl",
                    installRoot,
                    "template",
                    overrideOnly: true,
                    noChitin: true,
                    noModules: true,
                    caseSensitive: false,
                    partialMatch: false,
                    logger);

                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NcsScriptReference_InOverride_ExitsZero()
        {
            string installRoot = CreateInstallWithNcsScriptReference("k_ncs_cli");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindRefsCommand.Execute(
                    "k_ncs_cli",
                    installRoot,
                    "script",
                    overrideOnly: true,
                    noChitin: true,
                    noModules: true,
                    caseSensitive: false,
                    partialMatch: false,
                    logger);

                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_TagReference_InOverride_ExitsZero()
        {
            string installRoot = CreateInstallWithTagReference("unique_cli_tag");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindRefsCommand.Execute(
                    "unique_cli_tag",
                    installRoot,
                    "tag",
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    caseSensitive: false,
                    partialMatch: false,
                    logger);

                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_PartialTagMatch_InOverride_ExitsZero()
        {
            string installRoot = CreateInstallWithTagReference("unique_cli_tag");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindRefsCommand.Execute(
                    "cli_tag",
                    installRoot,
                    "tag",
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    caseSensitive: false,
                    partialMatch: true,
                    logger);

                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NoOverride_SkipsOverrideHit()
        {
            string installRoot = CreateInstallWithTagReference("only_in_override");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindRefsCommand.Execute(
                    "only_in_override",
                    installRoot,
                    "tag",
                    overrideOnly: false,
                    noOverride: true,
                    noChitin: true,
                    noModules: true,
                    caseSensitive: false,
                    partialMatch: false,
                    logger);

                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static string CreateInstallWithTemplateReference(string templateResRef)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-findrefs-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.ResRef = new ResRef(templateResRef);
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithNcsScriptReference(string scriptResRef)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-findrefs-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            byte[] ncsBytes = System.Text.Encoding.ASCII.GetBytes("abc " + scriptResRef + " xyz");
            File.WriteAllBytes(Path.Combine(overrideDir, "embedded.ncs"), ncsBytes);

            return installRoot;
        }

        private static string CreateInstallWithScriptReference(string scriptResRef)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-findrefs-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.OnHeartbeat = new ResRef(scriptResRef);
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithConversationReference(string conversationResRef)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-findrefs-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Conversation = new ResRef(conversationResRef);
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithTagReference(string tag)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-findrefs-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            var utc = new UTC();
            utc.Tag = tag;
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
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
