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
