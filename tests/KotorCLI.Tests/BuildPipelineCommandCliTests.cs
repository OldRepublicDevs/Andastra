using System;
using System.Diagnostics;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class BuildPipelineCommandCliTests
    {
        private const string UnpackPipelineConfig = @"[package]
name = ""testpack""

  [package.rules]
  ""*.utc"" = ""src/blueprints/creatures""
  ""*"" = ""src""

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

        private const string PackPipelineConfig = @"[package]
name = ""testpack""

[target]
name = ""default""
file = ""test.mod""
";

        private const string ConvertPipelineConfig = @"[package]
name = ""testpack""

  [package.sources]
  include = ""*.json""

[target]
name = ""default""
file = ""test.mod""
";

        private const string CompilePipelineConfig = @"[package]
name = ""testpack""

[target]
name = ""default""
file = ""test.mod""
";

        private const string MixedPipelineConfig = @"[package]
name = ""testpack""

  [package.sources]
  include = [""src/**/*.json"", ""src/**/*.nss""]

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

        private const string InlineConvertPipelineConfig = @"[package]
name = ""testpack""

  [package.sources]
  include = ""src/**/*.json""

[target]
name = ""default""
file = ""test.mod""
";

        private const string MinimalNssSource = @"void main()
{
}
";

        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

        [Test]
        public void CliInit_DefaultMode_CreatesConfig()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-init-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(projectDir);
            string packageName = Path.GetFileName(projectDir);

            try
            {
                int exitCode = RunKotorCli(
                    "init . . --default --vcs none",
                    projectDir,
                    out _,
                    out string stderr);

                string configPath = Path.Combine(projectDir, "kotorcli.cfg");
                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(configPath), Is.True);
                Assert.That(File.ReadAllText(configPath), Does.Contain("name = \"" + packageName + "\""));
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliConfig_LocalSet_WritesUserConfig()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-config-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(projectDir);
            const string configKey = "gamePath";
            const string configValue = "/opt/kotor";

            try
            {
                Assert.That(
                    RunKotorCli("init . . --default --vcs none", projectDir, out _, out string initErr),
                    Is.EqualTo(0),
                    initErr);

                int exitCode = RunKotorCli(
                    "config " + configKey + " \"" + configValue + "\" --local",
                    projectDir,
                    out _,
                    out string stderr);

                string userConfigPath = Path.Combine(projectDir, ".kotorcli", "user.cfg");
                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(userConfigPath), Is.True);
                Assert.That(File.ReadAllText(userConfigPath), Does.Contain(configKey));
                Assert.That(File.ReadAllText(userConfigPath), Does.Contain(configValue));
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliConfig_LocalUnset_RemovesKey()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-config-cli-unset-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(projectDir);
            const string configKey = "editor";
            const string configValue = "vim";

            try
            {
                Assert.That(
                    RunKotorCli("init . . --default --vcs none", projectDir, out _, out string initErr),
                    Is.EqualTo(0),
                    initErr);

                Assert.That(
                    RunKotorCli("config " + configKey + " \"" + configValue + "\" --local", projectDir, out _, out string setErr),
                    Is.EqualTo(0),
                    setErr);

                string userConfigPath = Path.Combine(projectDir, ".kotorcli", "user.cfg");
                Assert.That(File.ReadAllText(userConfigPath), Does.Contain(configKey));

                int unsetExit = RunKotorCli(
                    "config " + configKey + " _ --local --unset",
                    projectDir,
                    out _,
                    out string unsetErr);
                Assert.That(unsetExit, Is.EqualTo(0), unsetErr);
                Assert.That(File.ReadAllText(userConfigPath), Does.Not.Contain(configKey + " ="));
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliConfig_LocalListEmpty_ExitsZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-config-cli-list-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(projectDir);

            try
            {
                Assert.That(
                    RunKotorCli("init . . --default --vcs none", projectDir, out _, out string initErr),
                    Is.EqualTo(0),
                    initErr);

                int exitCode = RunKotorCli(
                    "config _ _ --local --list",
                    projectDir,
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliList_AfterInit_ExitsZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(projectDir);

            try
            {
                Assert.That(
                    RunKotorCli("init . . --default --vcs none", projectDir, out _, out string initErr),
                    Is.EqualTo(0),
                    initErr);

                int exitCode = RunKotorCli("list", projectDir, out _, out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliList_NoPackage_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-cli-empty-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(projectDir);

            try
            {
                int exitCode = RunKotorCli("list", projectDir, out _, out string stderr);
                Assert.That(exitCode, Is.EqualTo(1), stderr);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [TestCase("convert default")]
        [TestCase("compile default")]
        [TestCase("pack default")]
        [TestCase("install default")]
        [TestCase("unpack default test.mod")]
        public void CliPipeline_NoConfigDirectory_ExitsNonZero(string command)
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-nocfg-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(projectDir);

            try
            {
                int exitCode = RunKotorCli(command, projectDir, out _, out string stderr);
                Assert.That(exitCode, Is.EqualTo(1), stderr);
                Assert.That(stderr.ToLowerInvariant(), Does.Contain("kotorcli"));
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliUnpack_FromMod_WritesCreatureJson()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-unpack-cli-" + Guid.NewGuid().ToString("N"));
            const string resref = "cli_creature";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), UnpackPipelineConfig);

                string modPath = Path.Combine(projectDir, "test.mod");
                WriteModWithUtc(modPath, resref);

                int exitCode = RunKotorCli(
                    "unpack default \"" + modPath + "\" --removeDeleted",
                    projectDir,
                    out _,
                    out string stderr);

                string jsonPath = Path.Combine(projectDir, "src", "blueprints", "creatures", resref + ".utc.json");
                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(jsonPath), Is.True);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliPack_Unpack_RemoveDeleted_RemovesStaleJson()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-unpack-rm-cli-" + Guid.NewGuid().ToString("N"));
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

                Assert.That(
                    RunKotorCli("pack default", projectDir, out _, out string packErr),
                    Is.EqualTo(0),
                    packErr);

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                string stalePath = Path.Combine(creaturesDir, "stale.utc.json");
                File.WriteAllText(stalePath, "{}");

                Assert.That(
                    RunKotorCli("unpack default \"" + modPath + "\" --removeDeleted", projectDir, out _, out string unpackErr),
                    Is.EqualTo(0),
                    unpackErr);
                Assert.That(File.Exists(stalePath), Is.False);
                Assert.That(File.Exists(jsonPath), Is.True);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliPack_Unpack_Roundtrip_RestoresJsonUnderRules()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-unpack-rt-cli-" + Guid.NewGuid().ToString("N"));
            const string resref = "rt_creature";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), PackUnpackPipelineConfig);

                string creaturesDir = Path.Combine(projectDir, "src", "blueprints", "creatures");
                Directory.CreateDirectory(creaturesDir);
                string jsonPath = Path.Combine(creaturesDir, resref + ".utc.json");
                GFF gff = UTCHelpers.DismantleUtc(new UTC(), BioWareGame.K1);
                gff.Root.SetString("Label", "roundtrip-cli");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                Assert.That(
                    RunKotorCli("pack default", projectDir, out _, out string packErr),
                    Is.EqualTo(0),
                    packErr);

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                ERF packedMod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(packedMod.Get(resref, ResourceType.UTC), Is.Not.Null);

                File.Delete(jsonPath);
                Assert.That(File.Exists(jsonPath), Is.False);

                Assert.That(
                    RunKotorCli("unpack default \"" + modPath + "\"", projectDir, out _, out string unpackErr),
                    Is.EqualTo(0),
                    unpackErr);
                Assert.That(File.Exists(jsonPath), Is.True);

                GFF restored = GFFAuto.ReadGff(jsonPath, fileFormat: ResourceType.GFF_JSON);
                Assert.That(restored.Root.GetString("Label"), Is.EqualTo("roundtrip-cli"));
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliPack_WithCache_WritesMod()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-cli-" + Guid.NewGuid().ToString("N"));
            const string resref = "pack_cli_cre";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), PackPipelineConfig);

                string cacheDir = Path.Combine(projectDir, ".kotorcli", "cache", "default");
                Directory.CreateDirectory(cacheDir);
                string cacheUtcPath = Path.Combine(cacheDir, resref + ".utc");
                GFF gff = UTCHelpers.DismantleUtc(new UTC(), BioWareGame.K1);
                GFFAuto.WriteGff(gff, cacheUtcPath, ResourceType.UTC);

                int exitCode = RunKotorCli(
                    "pack default --noConvert --noCompile",
                    projectDir,
                    out _,
                    out string stderr);

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(modPath), Is.True);

                ERF mod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(mod.Get(resref, ResourceType.UTC), Is.Not.Null);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliPack_UnknownTarget_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-cli-unknown-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), PackPipelineConfig);

                int exitCode = RunKotorCli(
                    "pack missing-target --noConvert --noCompile",
                    projectDir,
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(1), stderr);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliConvert_JsonGff_WritesBinary()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-convert-cli-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), ConvertPipelineConfig);

                string jsonPath = Path.Combine(projectDir, "sample.utc.json");
                string binaryPath = Path.Combine(projectDir, "sample.utc");
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "convert-cli");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                int exitCode = RunKotorCli("convert default", projectDir, out _, out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(binaryPath), Is.True);

                GFF roundTrip = GFFAuto.ReadGff(binaryPath, fileFormat: ResourceType.GFF);
                Assert.That(roundTrip.Root.GetString("Label"), Is.EqualTo("convert-cli"));
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliConvert_UnknownTarget_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-convert-cli-unknown-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), ConvertPipelineConfig);

                int exitCode = RunKotorCli("convert missing-target", projectDir, out _, out string stderr);
                Assert.That(exitCode, Is.EqualTo(1), stderr);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliCompile_NoNssSources_ExitsZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-compile-cli-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), CompilePipelineConfig);

                int exitCode = RunKotorCli("compile default", projectDir, out _, out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliCompile_UnknownTarget_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-compile-cli-unknown-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), CompilePipelineConfig);

                int exitCode = RunKotorCli("compile missing-target", projectDir, out _, out string stderr);
                Assert.That(exitCode, Is.EqualTo(1), stderr);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliInstall_UnknownTarget_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-cli-unknown-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), PackPipelineConfig);

                int exitCode = RunKotorCli("install missing-target", projectDir, out _, out string stderr);
                Assert.That(exitCode, Is.EqualTo(1), stderr);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliInstall_WithPackedMod_CopiesToModules()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-cli-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-cli-game-" + Guid.NewGuid().ToString("N"));
            const string resref = "install_cre";

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(fakeInstallDir, "chitin.key"), "fake-key");
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), PackPipelineConfig);

                WriteModWithUtc(Path.Combine(projectDir, "test.mod"), resref);

                int exitCode = RunKotorCli(
                    "install default --installDir \"" + fakeInstallDir + "\" --noPack",
                    projectDir,
                    out _,
                    out string stderr);

                string installedModPath = Path.Combine(fakeInstallDir, "modules", "test.mod");
                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(installedModPath), Is.True);

                ERF installedMod = ERFAuto.ReadErf(installedModPath, ResourceType.MOD);
                Assert.That(installedMod.Get(resref, ResourceType.UTC), Is.Not.Null);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [Test]
        public void CliPack_WithInlineConvert_FromJson_WritesMod()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-inline-conv-" + Guid.NewGuid().ToString("N"));
            const string resref = "pk_creature";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), InlineConvertPipelineConfig);

                string srcDir = Path.Combine(projectDir, "src", "blueprints", "creatures");
                Directory.CreateDirectory(srcDir);
                string jsonPath = Path.Combine(srcDir, resref + ".utc.json");
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "pack-inline-convert");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                int exitCode = RunKotorCli("pack default", projectDir, out _, out string stderr);

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(modPath), Is.True);

                ERF mod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(mod.Get(resref, ResourceType.UTC), Is.Not.Null);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliPack_WithInlineCompile_FromNss_WritesMod()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-pack-inline-comp-" + Guid.NewGuid().ToString("N"));
            const string scriptResref = "pk_main";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), ScriptPipelineConfig);

                string scriptDir = Path.Combine(projectDir, "src", "scripts");
                Directory.CreateDirectory(scriptDir);
                File.WriteAllText(Path.Combine(scriptDir, scriptResref + ".nss"), MinimalNssSource);

                int exitCode = RunKotorCli("pack default", projectDir, out _, out string stderr);

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(modPath), Is.True);

                ERF mod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(mod.Get(scriptResref, ResourceType.NCS), Is.Not.Null);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void CliConvert_Pack_Install_FullChain_ProducesInstalledMod()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-conv-pack-inst-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-conv-pack-inst-game-" + Guid.NewGuid().ToString("N"));
            const string resref = "plc_creature";

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(fakeInstallDir, "chitin.key"), "fake-key");
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), InlineConvertPipelineConfig);

                string srcDir = Path.Combine(projectDir, "src", "blueprints", "creatures");
                Directory.CreateDirectory(srcDir);
                string jsonPath = Path.Combine(srcDir, resref + ".utc.json");
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "pipeline-cli-chain");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                Assert.That(
                    RunKotorCli("convert default", projectDir, out _, out string convertErr),
                    Is.EqualTo(0),
                    convertErr);

                string cacheUtc = Path.Combine(projectDir, ".kotorcli", "cache", "default", resref + ".utc");
                Assert.That(File.Exists(cacheUtc), Is.True, "convert should stage binary GFF in cache");

                Assert.That(
                    RunKotorCli("pack default --noConvert --noCompile", projectDir, out _, out string packErr),
                    Is.EqualTo(0),
                    packErr);

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                int installExit = RunKotorCli(
                    "install default --installDir \"" + fakeInstallDir + "\" --noPack",
                    projectDir,
                    out _,
                    out string installErr);

                string installedModPath = Path.Combine(fakeInstallDir, "modules", "test.mod");
                Assert.That(installExit, Is.EqualTo(0), installErr);
                Assert.That(File.Exists(installedModPath), Is.True);

                ERF installedMod = ERFAuto.ReadErf(installedModPath, ResourceType.MOD);
                Assert.That(installedMod.Get(resref, ResourceType.UTC), Is.Not.Null);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [Test]
        public void CliInstall_InstallDirMissingChitin_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-cli-nochitin-proj-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-install-cli-nochitin-game-" + Guid.NewGuid().ToString("N"));

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.CreateDirectory(fakeInstallDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), PackPipelineConfig);
                WriteModWithUtc(Path.Combine(projectDir, "test.mod"), "install_cre");

                int exitCode = RunKotorCli(
                    "install default --installDir \"" + fakeInstallDir + "\" --noPack",
                    projectDir,
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(1), stderr);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [Test]
        public void CliMixed_PackThenInstall_ProducesModWithUtcAndNcs()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-mixed-cli-" + Guid.NewGuid().ToString("N"));
            string fakeInstallDir = Path.Combine(Path.GetTempPath(), "kotorcli-mixed-cli-game-" + Guid.NewGuid().ToString("N"));
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
                gff.Root.SetString("Label", "mixed-cli");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                string scriptDir = Path.Combine(projectDir, "src", "scripts");
                Directory.CreateDirectory(scriptDir);
                File.WriteAllText(Path.Combine(scriptDir, scriptResref + ".nss"), MinimalNssSource);

                Assert.That(
                    RunKotorCli("pack default", projectDir, out _, out string packErr),
                    Is.EqualTo(0),
                    packErr);

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                ERF packedMod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(packedMod.Get(creatureResref, ResourceType.UTC), Is.Not.Null);
                Assert.That(packedMod.Get(scriptResref, ResourceType.NCS), Is.Not.Null);

                Assert.That(
                    RunKotorCli(
                        "install default --installDir \"" + fakeInstallDir + "\" --noPack",
                        projectDir,
                        out _,
                        out string installErr),
                    Is.EqualTo(0),
                    installErr);

                string installedModPath = Path.Combine(fakeInstallDir, "modules", "test.mod");
                Assert.That(File.Exists(installedModPath), Is.True);

                ERF installedMod = ERFAuto.ReadErf(installedModPath, ResourceType.MOD);
                Assert.That(installedMod.Get(creatureResref, ResourceType.UTC), Is.Not.Null);
                Assert.That(installedMod.Get(scriptResref, ResourceType.NCS), Is.Not.Null);
            }
            finally
            {
                DeleteDirectorySafe(projectDir);
                DeleteDirectorySafe(fakeInstallDir);
            }
        }

        [Test]
        public void CliCompile_ThenPack_ProducesModWithNcs()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-compile-pack-cli-" + Guid.NewGuid().ToString("N"));
            const string scriptResref = "mod_main";

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), ScriptPipelineConfig);

                string scriptDir = Path.Combine(projectDir, "src", "scripts");
                Directory.CreateDirectory(scriptDir);
                File.WriteAllText(Path.Combine(scriptDir, scriptResref + ".nss"), MinimalNssSource);

                Assert.That(
                    RunKotorCli("compile default", projectDir, out _, out string compileErr),
                    Is.EqualTo(0),
                    compileErr);

                string cacheNcs = Path.Combine(projectDir, ".kotorcli", "cache", "default", scriptResref + ".ncs");
                Assert.That(File.Exists(cacheNcs), Is.True, "Compile should write NCS into cache");

                Assert.That(
                    RunKotorCli("pack default --noConvert --noCompile", projectDir, out _, out string packErr),
                    Is.EqualTo(0),
                    packErr);

                string modPath = Path.Combine(projectDir, "test.mod");
                Assert.That(File.Exists(modPath), Is.True);

                ERF mod = ERFAuto.ReadErf(modPath, ResourceType.MOD);
                Assert.That(mod.Get(scriptResref, ResourceType.NCS), Is.Not.Null);
            }
            finally
            {
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

        private static int RunKotorCli(string arguments, string workingDirectory, out string stdout, out string stderr)
        {
            string cliDll = Path.Combine(RepoRoot, "src", "Tools", "KotorCLI", "bin", "Debug", "net9.0", "KotorCLI.dll");
            if (!File.Exists(cliDll))
            {
                cliDll = Path.Combine(RepoRoot, "src", "Tools", "KotorCLI", "bin", "Release", "net9.0", "KotorCLI.dll");
            }

            Assert.That(File.Exists(cliDll), Is.True, "KotorCLI.dll not built; run dotnet build first.");

            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "exec \"" + cliDll + "\" " + arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory,
            };

            using (var process = Process.Start(startInfo))
            {
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();
                process.WaitForExit(120000);
                return process.ExitCode;
            }
        }

        private static void DeleteDirectorySafe(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }
            }
            catch
            {
                // Best-effort cleanup.
            }
        }
    }
}
