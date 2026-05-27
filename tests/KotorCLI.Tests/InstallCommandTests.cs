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
    public class InstallCommandTests
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
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-nocfg-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = InstallCommand.Execute(new[] { "default" }, null, false, false, logger);

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
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-unknown-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = InstallCommand.Execute(new[] { "missing-target" }, null, false, false, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_InstallDirMissingChitin_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-nochitin-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-fake-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = InstallCommand.Execute(new[] { "default" }, fakeInstallDir, false, false, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [Test]
        public void Execute_WithPackedModAndFakeInstallDir_CopiesToModules()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-happy-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-game-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(fakeInstallDir, "chitin.key"), "fake-key");
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);

                string modPath = Path.Combine(projectDir, "test.mod");
                WriteModWithUtc(modPath, "install_creature");

                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = InstallCommand.Execute(new[] { "default" }, fakeInstallDir, false, false, logger);

                string installedModPath = Path.Combine(fakeInstallDir, "modules", "test.mod");
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(installedModPath), Is.True);

                ERF installedMod = ERFAuto.ReadErf(installedModPath, ResourceType.MOD);
                Assert.That(installedMod.Get("install_creature", ResourceType.UTC), Is.Not.Null);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        private static void WriteModWithUtc(string modPath, string resref)
        {
            GFF gff = UTCHelpers.DismantleUtc(new UTC(), BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            var mod = new ERF(ERFType.MOD);
            mod.SetData(resref, ResourceType.UTC, bytes);
            ERFAuto.WriteErf(mod, modPath, ResourceType.MOD);
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
