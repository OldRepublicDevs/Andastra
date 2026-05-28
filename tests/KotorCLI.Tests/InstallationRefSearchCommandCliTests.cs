using System;
using System.Diagnostics;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics.UTC;
using BioWare.Resource.Formats.SSF;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class InstallationRefSearchCommandCliTests
    {
        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

        [Test]
        public void Cli_FindStrRef_InOverride_ExitsZero()
        {
            string installRoot = CreateInstallWithStrRef(88888);
            try
            {
                int exitCode = RunKotorCli(
                    "find-strref 88888 --installation \"" + installRoot + "\" --override-only --no-chitin --no-modules --no-ncs",
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain("88888"));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Cli_FindStrRef_NoMatch_ExitsNonZero()
        {
            string installRoot = CreateInstallWithStrRef(88888);
            try
            {
                int exitCode = RunKotorCli(
                    "find-strref 99998 --installation \"" + installRoot + "\" --override-only --no-chitin --no-modules --no-ncs",
                    out string stdout,
                    out string stderr);

                Assert.That(exitCode, Is.Not.EqualTo(0), stdout + stderr);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Cli_Find2DARef_InOverride_ExitsZero()
        {
            string installRoot = CreateInstallWithAppearanceRow(12);
            try
            {
                int exitCode = RunKotorCli(
                    "find-2da-ref appearance 12 --installation \"" + installRoot + "\" --override-only --no-chitin --no-modules",
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain("appearance"));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Cli_FindFieldValue_InOverride_ExitsZero()
        {
            string installRoot = CreateInstallWithTag("cli_fv_tag");
            try
            {
                int exitCode = RunKotorCli(
                    "find-field-value cli_fv_tag --installation \"" + installRoot + "\" --override-only --no-chitin --no-modules",
                    out string stdout,
                    out string stderr);

                string combined = stdout + stderr;
                Assert.That(exitCode, Is.EqualTo(0), combined);
                Assert.That(combined, Does.Contain("cli_fv_tag"));
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Cli_FindFieldValue_NoMatch_ExitsNonZero()
        {
            string installRoot = CreateInstallWithTag("cli_fv_tag");
            try
            {
                int exitCode = RunKotorCli(
                    "find-field-value missing_fv_value --installation \"" + installRoot + "\" --override-only --no-chitin --no-modules",
                    out string stdout,
                    out string stderr);

                Assert.That(exitCode, Is.Not.EqualTo(0), stdout + stderr);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        [Test]
        public void Cli_Find2DARef_NoMatch_ExitsNonZero()
        {
            string installRoot = CreateInstallWithAppearanceRow(12);
            try
            {
                int exitCode = RunKotorCli(
                    "find-2da-ref appearance 999 --installation \"" + installRoot + "\" --override-only --no-chitin --no-modules",
                    out string stdout,
                    out string stderr);

                Assert.That(exitCode, Is.Not.EqualTo(0), stdout + stderr);
            }
            finally
            {
                DeleteDirectorySafe(installRoot);
            }
        }

        private static int RunKotorCli(string arguments, out string stdout, out string stderr)
        {
            string cliDll = Path.Combine(RepoRoot, "src", "Tools", "KotorCLI", "bin", "Debug", "net9.0", "KotorCLI.dll");
            if (!File.Exists(cliDll))
            {
                var buildPsi = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "build \"" + Path.Combine(RepoRoot, "src", "Tools", "KotorCLI", "KotorCLI.csproj") + "\" --framework net9.0",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = RepoRoot
                };

                using (Process buildProcess = Process.Start(buildPsi))
                {
                    buildProcess.WaitForExit(120000);
                    Assert.That(buildProcess.ExitCode, Is.EqualTo(0), "KotorCLI build failed before integration test.");
                }
            }

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "exec \"" + cliDll + "\" " + arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = RepoRoot
            };

            using (Process process = Process.Start(psi))
            {
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();
                process.WaitForExit(120000);
                return process.ExitCode;
            }
        }

        private static string CreateInstallWithStrRef(int strref)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-strref-cli-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var ssf = new SSF();
            ssf.SetData(SSFSound.BATTLE_CRY_1, strref);
            byte[] bytes = SSFAuto.BytesSsf(ssf);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_set.ssf"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithTag(string tag)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-field-cli-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllBytes(Path.Combine(installRoot, "chitin.key"), new byte[0]);

            var utc = new UTC();
            utc.Tag = tag;
            GFF gff = UTCHelpers.DismantleUtc(utc, BioWareGame.K1);
            byte[] bytes = GFFAuto.BytesGff(gff, ResourceType.UTC);
            File.WriteAllBytes(Path.Combine(overrideDir, "test_npc.utc"), bytes);

            return installRoot;
        }

        private static string CreateInstallWithAppearanceRow(int rowIndex)
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-2da-cli-" + Guid.NewGuid().ToString("N"));
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
