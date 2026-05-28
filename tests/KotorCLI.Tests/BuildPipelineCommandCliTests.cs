using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class BuildPipelineCommandCliTests
    {
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
