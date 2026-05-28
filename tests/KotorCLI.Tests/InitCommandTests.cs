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
    public class InitCommandTests
    {
        [Test]
        public void Execute_DefaultMode_CreatesConfigFromDirectoryName()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-init-" + Guid.NewGuid().ToString("N"));
            string packageName = Path.GetFileName(projectDir);

            try
            {
                var logger = new StandardLogger();
                int exitCode = InitCommand.Execute(projectDir, null, true, "none", logger);

                string configPath = Path.Combine(projectDir, "kotorcli.cfg");
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(configPath), Is.True);

                string configText = File.ReadAllText(configPath);
                Assert.That(configText, Does.Contain("name = \"" + packageName + "\""));
                Assert.That(configText, Does.Contain("file = \"" + packageName + ".mod\""));
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_DefaultMode_CreatesSourceTreeAndGitignore()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-init-tree-" + Guid.NewGuid().ToString("N"));

            try
            {
                var logger = new StandardLogger();
                int exitCode = InitCommand.Execute(projectDir, null, true, "none", logger);

                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(Directory.Exists(Path.Combine(projectDir, "src", "scripts")), Is.True);
                Assert.That(Directory.Exists(Path.Combine(projectDir, "src", "blueprints", "creatures")), Is.True);
                Assert.That(File.Exists(Path.Combine(projectDir, ".gitignore")), Is.True);
                Assert.That(File.ReadAllText(Path.Combine(projectDir, ".gitignore")), Does.Contain(".kotorcli/cache/"));
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_DefaultModeWithInitFile_UnpacksUtcIntoSourceTree()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-init-unpack-" + Guid.NewGuid().ToString("N"));
            string modPath = Path.Combine(Path.GetTempPath(), "kotorcli-init-mod-" + Guid.NewGuid().ToString("N") + ".mod");
            const string resref = "init_creature";

            try
            {
                WriteModWithUtc(modPath, resref);

                var logger = new StandardLogger();
                int exitCode = InitCommand.Execute(projectDir, modPath, true, "none", logger);

                string unpackedJson = Path.Combine(projectDir, "src", "blueprints", "creatures", resref + ".utc.json");
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(unpackedJson), Is.True);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
                DeleteFileSafe(modPath);
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

        private static void DeleteFileSafe(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch
            {
                // Best-effort cleanup.
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
