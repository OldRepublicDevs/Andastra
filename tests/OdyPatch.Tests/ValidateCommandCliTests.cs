using System;
using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace OdyPatch.Tests
{
    [TestFixture]
    public class ValidateCommandCliTests
    {
        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

        [Test]
        public void Cli_Validate_MinimalMod_ExitsZero()
        {
            string gameDir = Path.Combine(RepoRoot, "tests", "fixtures", "odypatch-fake-game");
            string tslpatchdata = Path.Combine(RepoRoot, "tests", "fixtures", "odypatch-minimal-mod", "tslpatchdata");

            Assert.That(Directory.Exists(gameDir), Is.True, "Fixture game dir missing: " + gameDir);
            Assert.That(Directory.Exists(tslpatchdata), Is.True, "Fixture tslpatchdata missing: " + tslpatchdata);

            int exitCode = RunOdyPatch(
                "--validate --game-dir \"" + gameDir + "\" --tslpatchdata \"" + tslpatchdata + "\"",
                out string stdout,
                out string stderr);

            string combined = stdout + stderr;
            Assert.That(exitCode, Is.EqualTo(0), combined);
            Assert.That(combined, Does.Contain("Validation completed successfully"));
        }

        private static int RunOdyPatch(string arguments, out string stdout, out string stderr)
        {
            string odypatchDll = Path.Combine(RepoRoot, "src", "Tools", "OdyPatch", "bin", "Release", "net9.0", "OdyPatch.dll");
            if (!File.Exists(odypatchDll))
            {
                odypatchDll = Path.Combine(RepoRoot, "src", "Tools", "OdyPatch", "bin", "Debug", "net9.0", "OdyPatch.dll");
            }

            if (!File.Exists(odypatchDll))
            {
                var buildPsi = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "build \"" + Path.Combine(RepoRoot, "src", "Tools", "OdyPatch", "OdyPatch.csproj") + "\" --framework net9.0 -c Release",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = RepoRoot
                };

                using (Process buildProcess = Process.Start(buildPsi))
                {
                    buildProcess.WaitForExit(180000);
                    Assert.That(buildProcess.ExitCode, Is.EqualTo(0), "OdyPatch build failed before integration test.");
                }

                odypatchDll = Path.Combine(RepoRoot, "src", "Tools", "OdyPatch", "bin", "Release", "net9.0", "OdyPatch.dll");
            }

            Assert.That(File.Exists(odypatchDll), Is.True, "OdyPatch.dll not found after build: " + odypatchDll);

            var psi = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "exec \"" + odypatchDll + "\" " + arguments,
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
                process.WaitForExit(180000);
                return process.ExitCode;
            }
        }
    }
}
