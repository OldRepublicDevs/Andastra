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

        private const string ScriptPipelineConfig = @"[package]
name = ""testpack""

  [package.sources]
  include = ""src/**/*.nss""

[target]
name = ""default""
file = ""test.mod""
";

        private const string MinimalNssSource = @"void main()
{
}
";

        private const string MixedPipelineConfig = @"[package]
name = ""testpack""

  [package.sources]
  include = [""src/**/*.json"", ""src/**/*.nss""]

[target]
name = ""default""
file = ""test.mod""
";

        private const string PackUnpackPipelineConfig = @"[package]
name = ""testpack""

  [package.sources]
  include = ""src/**/*.json""

  [package.rules]
  ""*.utc"" = ""src/blueprints/creatures""

[target]
name = ""default""
file = ""test.mod""
";

        [Test]
        public void Pack_Unpack_RemoveDeleted_RemovesStaleJsonNotInArchive()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-unpack-rm-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string resref = "rm_creature";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), PackUnpackPipelineConfig);

                string creaturesDir = Path.Combine(projectDir, "src", "blueprints", "creatures");
                Directory.CreateDirectory(creaturesDir);
                string jsonPath = Path.Combine(creaturesDir, resref + ".utc.json");
                GFF gff = UTCHelpers.DismantleUtc(new UTC(), BioWareGame.K1);
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                Directory.SetCurrentDirectory(projectDir);
                var logger = new StandardLogger();

                int packExit = PackCommand.Execute(new[] { "default" }, false, false, false, logger);
                Assert.That(packExit, Is.EqualTo(0));

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                string stalePath = Path.Combine(creaturesDir, "stale.utc.json");
                File.WriteAllText(stalePath, "{}");

                int unpackExit = UnpackCommand.Execute("default", modPath, true, logger);
                Assert.That(unpackExit, Is.EqualTo(0));
                Assert.That(File.Exists(stalePath), Is.False);
                Assert.That(File.Exists(jsonPath), Is.True);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Pack_Unpack_Roundtrip_WritesJsonUnderRulesPath()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-unpack-rt-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string resref = "rt_creature";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), PackUnpackPipelineConfig);

                string creaturesDir = Path.Combine(projectDir, "src", "blueprints", "creatures");
                Directory.CreateDirectory(creaturesDir);
                string jsonPath = Path.Combine(creaturesDir, resref + ".utc.json");
                GFF gff = UTCHelpers.DismantleUtc(new UTC(), BioWareGame.K1);
                gff.Root.SetString("Label", "roundtrip-test");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                Directory.SetCurrentDirectory(projectDir);
                var logger = new StandardLogger();

                int packExit = PackCommand.Execute(new[] { "default" }, false, false, false, logger);
                Assert.That(packExit, Is.EqualTo(0));

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                ERF packedMod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(packedMod.Get(resref, ResourceType.UTC), Is.Not.Null);

                File.Delete(jsonPath);
                Assert.That(File.Exists(jsonPath), Is.False);

                int unpackExit = UnpackCommand.Execute("default", modPath, false, logger);
                Assert.That(unpackExit, Is.EqualTo(0));
                Assert.That(File.Exists(jsonPath), Is.True);

                GFF restored = GFFAuto.ReadGff(jsonPath, fileFormat: ResourceType.GFF_JSON);
                Assert.That(restored.Root.GetString("Label"), Is.EqualTo("roundtrip-test"));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Mixed_Pack_Install_Pipeline_ProducesModWithUtcAndNcs()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-mixed-pipeline-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-mixed-game-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string creatureResref = "mx_creature";
            const string scriptResref = "mx_main";

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(fakeInstallDir, "chitin.key"), "fake-key");
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MixedPipelineConfig);

                string creatureDir = Path.Combine(projectDir, "src", "blueprints", "creatures");
                Directory.CreateDirectory(creatureDir);
                string jsonPath = Path.Combine(creatureDir, creatureResref + ".utc.json");
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "mixed-pipeline");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                string scriptDir = Path.Combine(projectDir, "src", "scripts");
                Directory.CreateDirectory(scriptDir);
                File.WriteAllText(Path.Combine(scriptDir, scriptResref + ".nss"), MinimalNssSource);

                Directory.SetCurrentDirectory(projectDir);
                var logger = new StandardLogger();

                int packExit = PackCommand.Execute(new[] { "default" }, false, false, false, logger);
                Assert.That(packExit, Is.EqualTo(0));

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                ERF packedMod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(packedMod.Get(creatureResref, ResourceType.UTC), Is.Not.Null);
                Assert.That(packedMod.Get(scriptResref, ResourceType.NCS), Is.Not.Null);

                int installExit = InstallCommand.Execute(new[] { "default" }, fakeInstallDir, false, false, logger);
                Assert.That(installExit, Is.EqualTo(0));

                string installedModPath = Path.Combine(fakeInstallDir, "modules", "test.mod");
                Assert.That(File.Exists(installedModPath), Is.True);

                ERF installedMod = ERFAuto.ReadErf(installedModPath, ResourceType.MOD);
                Assert.That(installedMod.Get(creatureResref, ResourceType.UTC), Is.Not.Null);
                Assert.That(installedMod.Get(scriptResref, ResourceType.NCS), Is.Not.Null);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [Test]
        public void Compile_Pack_ProducesModWithCompiledScript()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-compile-pack-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string scriptResref = "mod_main";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), ScriptPipelineConfig);

                string scriptDir = Path.Combine(projectDir, "src", "scripts");
                Directory.CreateDirectory(scriptDir);
                File.WriteAllText(Path.Combine(scriptDir, scriptResref + ".nss"), MinimalNssSource);

                Directory.SetCurrentDirectory(projectDir);
                var logger = new StandardLogger();

                int compileExit = CompileCommand.Execute(new[] { "default" }, false, null, null, logger);
                Assert.That(compileExit, Is.EqualTo(0));

                string cacheNcs = Path.Combine(projectDir, ".kotorcli", "cache", "default", scriptResref + ".ncs");
                Assert.That(File.Exists(cacheNcs), Is.True, "Compile should write NCS into cache");

                int packExit = PackCommand.Execute(new[] { "default" }, false, true, true, logger);
                Assert.That(packExit, Is.EqualTo(0));

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                ERF mod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(mod.Get(scriptResref, ResourceType.NCS), Is.Not.Null);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Pack_WithInlineCompile_ProducesModFromNssSource()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-compile-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string scriptResref = "pk_main";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), ScriptPipelineConfig);

                string scriptDir = Path.Combine(projectDir, "src", "scripts");
                Directory.CreateDirectory(scriptDir);
                File.WriteAllText(Path.Combine(scriptDir, scriptResref + ".nss"), MinimalNssSource);

                Directory.SetCurrentDirectory(projectDir);
                var logger = new StandardLogger();

                int packExit = PackCommand.Execute(new[] { "default" }, false, true, false, logger);
                Assert.That(packExit, Is.EqualTo(0));

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                ERF mod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(mod.Get(scriptResref, ResourceType.NCS), Is.Not.Null);
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

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

        [Test]
        public void Pack_WithInlineConvert_ProducesModFromJsonSource()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-orchestrate-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string resref = "pk_creature";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), PipelineConfig);

                string srcDir = Path.Combine(projectDir, "src", "blueprints", "creatures");
                Directory.CreateDirectory(srcDir);
                string jsonPath = Path.Combine(srcDir, resref + ".utc.json");
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "pack-orchestrate");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                Directory.SetCurrentDirectory(projectDir);
                var logger = new StandardLogger();

                int packExit = PackCommand.Execute(new[] { "default" }, false, false, false, logger);
                Assert.That(packExit, Is.EqualTo(0));

                string modPath = Path.Combine(projectDir, "test.mod");
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
