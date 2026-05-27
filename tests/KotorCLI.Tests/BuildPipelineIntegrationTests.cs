using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class BuildPipelineIntegrationTests
    {
        private const string PipelineConfig = @"[package]
name = ""testpack""

  [package.sources]
  include = ""src/**/*.json""

[target]
name = ""default""
file = ""test.mod""
";

        [Test]
        public void Convert_Pack_Install_Pipeline_ProducesInstalledMod()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pipeline-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-pipeline-game-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string resref = "plc_creature";

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(fakeInstallDir, "chitin.key"), "fake-key");
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), PipelineConfig);

                string srcDir = Path.Combine(projectDir, "src", "blueprints", "creatures");
                Directory.CreateDirectory(srcDir);
                string jsonPath = Path.Combine(srcDir, resref + ".utc.json");
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "pipeline-test");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                Directory.SetCurrentDirectory(projectDir);
                var logger = new StandardLogger();

                int convertExit = ConvertCommand.Execute(new[] { "default" }, false, logger);
                Assert.That(convertExit, Is.EqualTo(0));

                string cacheUtc = Path.Combine(projectDir, ".kotorcli", "cache", "default", resref + ".utc");
                Assert.That(File.Exists(cacheUtc), Is.True, "Convert should stage binary GFF in cache");

                int packExit = PackCommand.Execute(new[] { "default" }, false, true, true, logger);
                Assert.That(packExit, Is.EqualTo(0));

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                int installExit = InstallCommand.Execute(new[] { "default" }, fakeInstallDir, false, false, logger);
                Assert.That(installExit, Is.EqualTo(0));

                string installedModPath = Path.Combine(fakeInstallDir, "modules", "test.mod");
                Assert.That(File.Exists(installedModPath), Is.True);

                ERF installedMod = ERFAuto.ReadErf(installedModPath, ResourceType.MOD);
                Assert.That(installedMod.Get(resref, ResourceType.UTC), Is.Not.Null);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
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
