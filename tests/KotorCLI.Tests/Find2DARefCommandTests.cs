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
    public class Find2DARefCommandTests
    {
        [Test]
        public void Execute_2DARowInOverrideUtc_ExitsZero()
        {
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var logger = new StandardLogger();
                int exitCode = Find2DARefCommand.Execute("appearance", 9, installRoot, logger);
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
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var logger = new StandardLogger();
                int exitCode = Find2DARefCommand.Execute("appearance", 999, installRoot, logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NegativeRow_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = Find2DARefCommand.Execute("appearance", -1, Path.GetTempPath(), logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void Execute_NoOverride_SkipsOverrideUtc_ExitsNonZero()
        {
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var logger = new StandardLogger();
                int exitCode = Find2DARefCommand.Execute(
                    "appearance",
                    9,
                    installRoot,
                    overrideOnly: false,
                    noOverride: true,
                    noChitin: true,
                    noModules: true,
                    logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_OverrideOnly_FindsOverrideUtc_ExitsZero()
        {
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var logger = new StandardLogger();
                int exitCode = Find2DARefCommand.Execute(
                    "appearance",
                    9,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_OverrideOnly_FindsOverrideUtc()
        {
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var logger = new StandardLogger();
                int exitCode = Find2DARefCommand.Execute(
                    "appearance",
                    9,
                    installRoot,
                    overrideOnly: true,
                    noOverride: false,
                    noChitin: true,
                    noModules: true,
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Execute_NoOverride_SkipsOverrideUtc()
        {
            string installRoot = CreateInstallWithAppearanceRow(9);
            try
            {
                var logger = new StandardLogger();
                int exitCode = Find2DARefCommand.Execute(
                    "appearance",
                    9,
                    installRoot,
                    overrideOnly: false,
                    noOverride: true,
                    noChitin: true,
                    noModules: true,
                    logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static string CreateInstallWithAppearanceRow(int rowIndex)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-2da-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var utc = new UTC();
            utc.AppearanceId = rowIndex;
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
