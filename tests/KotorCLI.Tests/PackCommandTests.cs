using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class PackCommandTests
    {
        private const string MinimalConfig = @"[package]
name = ""testpack""

[target]
name = ""default""
file = ""test.mod""
";

        [Test]
        public void Execute_NoConfigDirectory_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-nocfg-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = PackCommand.Execute(new[] { "default" }, false, true, true, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_UnknownTarget_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-unknown-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = PackCommand.Execute(new[] { "missing-target" }, false, true, true, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_MissingCache_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-nocache-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = PackCommand.Execute(new[] { "default" }, false, true, true, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_WithPopulatedCache_WritesModArchive()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-happy-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string resref = "pack_creature";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);

                string cacheDir = Path.Combine(projectDir, ".kotorcli", "cache", "default");
                Directory.CreateDirectory(cacheDir);
                string cacheUtcPath = Path.Combine(cacheDir, resref + ".utc");
                GFF gff = UTCHelpers.DismantleUtc(new UTC(), BioWareGame.K1);
                GFFAuto.WriteGff(gff, cacheUtcPath, ResourceType.UTC);

                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = PackCommand.Execute(new[] { "default" }, false, true, true, logger);

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(modPath), Is.True);

                ERF mod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(mod.Get(resref, ResourceType.UTC), Is.Not.Null);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
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
