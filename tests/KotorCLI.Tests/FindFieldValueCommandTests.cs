using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class FindFieldValueCommandTests
    {
        [Test]
        public void Execute_TagInOverrideUtc_ExitsZero()
        {
            string installRoot = CreateInstallWithTag("cli_find_tag");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindFieldValueCommand.Execute("cli_find_tag", installRoot, false, false, logger);
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
            string installRoot = CreateInstallWithTag("cli_find_tag");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindFieldValueCommand.Execute("missing_value", installRoot, false, false, logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_EmptyValue_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = FindFieldValueCommand.Execute("  ", Path.GetTempPath(), false, false, logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void Execute_OverrideOnly_FindsOverrideTag()
        {
            string installRoot = CreateInstallWithTag("cli_find_tag");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindFieldValueCommand.Execute(
                    "cli_find_tag",
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    partial: false,
                    caseSensitive: false,
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NoOverride_SkipsOverrideTag()
        {
            string installRoot = CreateInstallWithTag("cli_find_tag");
            try
            {
                var logger = new StandardLogger();
                int exitCode = FindFieldValueCommand.Execute(
                    "cli_find_tag",
                    installRoot,
                    overrideOnly: false,
                    noOverride: true,
                    noChitin: true,
                    noModules: true,
                    partial: false,
                    caseSensitive: false,
                    logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static string CreateInstallWithTag(string tag)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-field-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

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
