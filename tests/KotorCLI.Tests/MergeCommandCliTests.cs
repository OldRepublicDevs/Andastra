using System;
using System.Diagnostics;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class MergeCommandCliTests
    {
        private static string RepoRoot =>
            Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..", "..", "..", ".."));

        [Test]
        public void CliMerge_OverlaysSourceFields_ExitsZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-merge-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string targetPath = Path.Combine(tempDir, "target.utc");
            string sourcePath = Path.Combine(tempDir, "source.utc");
            string outputPath = Path.Combine(tempDir, "merged.utc");

            try
            {
                var targetGff = new GFF(GFFContent.GFF);
                targetGff.Root.SetString("Tag", "target_tag");
                targetGff.Root.SetString("Comment", "keep_me");
                File.WriteAllBytes(targetPath, GFFAuto.BytesGff(targetGff, ResourceType.UTC));

                var sourceGff = new GFF(GFFContent.GFF);
                sourceGff.Root.SetString("Tag", "source_tag");
                sourceGff.Root.SetString("Description", "from_source");
                File.WriteAllBytes(sourcePath, GFFAuto.BytesGff(sourceGff, ResourceType.UTC));

                int exitCode = RunKotorCli(
                    "merge \"" + targetPath + "\" \"" + sourcePath + "\" --output \"" + outputPath + "\"",
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(0), stderr);
                Assert.That(File.Exists(outputPath), Is.True);

                GFF merged = new GFFBinaryReader(File.ReadAllBytes(outputPath)).Load();
                Assert.That(merged.Root.GetString("Tag"), Is.EqualTo("source_tag"));
                Assert.That(merged.Root.GetString("Comment"), Is.EqualTo("keep_me"));
                Assert.That(merged.Root.GetString("Description"), Is.EqualTo("from_source"));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void CliMerge_MissingSource_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-merge-cli-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string targetPath = Path.Combine(tempDir, "target.utc");
            string missingSource = Path.Combine(tempDir, "missing.utc");
            string outputPath = Path.Combine(tempDir, "merged.utc");

            try
            {
                var targetGff = new GFF(GFFContent.GFF);
                targetGff.Root.SetString("Tag", "x");
                File.WriteAllBytes(targetPath, GFFAuto.BytesGff(targetGff, ResourceType.UTC));

                int exitCode = RunKotorCli(
                    "merge \"" + targetPath + "\" \"" + missingSource + "\" --output \"" + outputPath + "\"",
                    out _,
                    out string stderr);
                Assert.That(exitCode, Is.EqualTo(1), stderr);
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        private static int RunKotorCli(string arguments, out string stdout, out string stderr)
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
            };

            using (var process = Process.Start(startInfo))
            {
                stdout = process.StandardOutput.ReadToEnd();
                stderr = process.StandardError.ReadToEnd();
                process.WaitForExit();
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
