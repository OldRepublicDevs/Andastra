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
    public class UnpackCommandTests
    {
        private const string MinimalConfig = @"[package]
name = ""testpack""

  [package.rules]
  ""*.utc"" = ""src/blueprints/creatures""
  ""*"" = ""src""

[target]
name = ""default""
file = ""test.mod""
";

        [Test]
        public void Execute_RemoveDeleted_RemovesStaleSourcesNotInArchive()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-unpack-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);

                string modPath = Path.Combine(projectDir, "test.mod");
                WriteModWithUtc(modPath, "creature_a");

                string creaturesDir = Path.Combine(projectDir, "src", "blueprints", "creatures");
                Directory.CreateDirectory(creaturesDir);
                string stalePath = Path.Combine(creaturesDir, "stale.utc.json");
                File.WriteAllText(stalePath, "{}");

                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = UnpackCommand.Execute("default", modPath, true, logger);

                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(stalePath), Is.False);
                Assert.That(File.Exists(Path.Combine(creaturesDir, "creature_a.utc.json")), Is.True);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_WithoutRemoveDeleted_KeepsStaleSources()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-unpack-keep-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);

                string modPath = Path.Combine(projectDir, "test.mod");
                WriteModWithUtc(modPath, "creature_b");

                string creaturesDir = Path.Combine(projectDir, "src", "blueprints", "creatures");
                Directory.CreateDirectory(creaturesDir);
                string stalePath = Path.Combine(creaturesDir, "stale.utc.json");
                File.WriteAllText(stalePath, "{}");

                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = UnpackCommand.Execute("default", modPath, false, logger);

                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(stalePath), Is.True);
                Assert.That(File.Exists(Path.Combine(creaturesDir, "creature_b.utc.json")), Is.True);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
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
