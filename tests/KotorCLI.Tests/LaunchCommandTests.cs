using System;
using System.IO;
using BioWare.Resource;
using BioWare.Resource.Formats.KEY;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class LaunchCommandTests
    {
        [Test]
        public void Execute_DryRun_WithGameBin_PrintsResolvedPath()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, fakeExe, null, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }

        [Test]
        public void Execute_WithoutDryRun_StillExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-launch-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string fakeExe = Path.Combine(tempDir, "swkotor.exe");
                File.WriteAllBytes(fakeExe, new byte[] { 0x4D, 0x5A });

                var logger = new StandardLogger();
                int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, fakeExe, null, false, logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                try
                {
                    Directory.Delete(tempDir, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }
    }
}
