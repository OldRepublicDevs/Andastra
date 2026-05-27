using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.RIM;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ValidationCommandsTests
    {
        [Test]
        public void ExecuteCheckTxi_MissingTexture_ExitsNonZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-txi-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(installRoot);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);

            try
            {
                var logger = new StandardLogger();
                int exitCode = ValidationCommands.ExecuteCheckTxi(
                    installRoot,
                    new[] { "nonexistent_texture" },
                    logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                try
                {
                    Directory.Delete(installRoot, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }

        [Test]
        public void ExecuteCheckTxi_FoundInOverride_ExitsZero()
        {
            string installRoot = Path.Combine(Path.GetTempPath(), "kotorcli-txi-" + Guid.NewGuid().ToString("N"));
            string overrideDir = Path.Combine(installRoot, "Override");
            Directory.CreateDirectory(overrideDir);
            File.WriteAllBytes(Path.Combine(installRoot, "SWKOTOR.EXE"), new byte[0]);
            File.WriteAllText(Path.Combine(overrideDir, "test_tex.txi"), "blending additive");

            try
            {
                var logger = new StandardLogger();
                int exitCode = ValidationCommands.ExecuteCheckTxi(
                    installRoot,
                    new[] { "test_tex" },
                    logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                try
                {
                    Directory.Delete(installRoot, true);
                }
                catch
                {
                    // Best-effort cleanup.
                }
            }
        }
    }

    [TestFixture]
    public class CatCommandTests
    {
        [Test]
        public void Execute_ReadsResourceFromRim()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-cat-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string rimPath = Path.Combine(tempDir, "test.rim");

            try
            {
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "cat-test");
                byte[] utcBytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

                var rim = new RIM();
                rim.SetData("sample", ResourceType.UTC, utcBytes);
                RIMAuto.WriteRim(rim, rimPath, ResourceType.RIM);

                var logger = new StandardLogger();
                int exitCode = CatCommand.Execute(rimPath, "sample", "utc", logger);
                Assert.That(exitCode, Is.EqualTo(0));

                // Verify binary output by re-reading rim resource directly
                var verifyRim = new RIMBinaryReader(rimPath).Load();
                bool found = false;
                foreach (RIMResource resource in verifyRim)
                {
                    if (string.Equals(resource.ResRef.ToString(), "sample", StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;
                        break;
                    }
                }

                Assert.That(found, Is.True);
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
        public void Execute_MissingResource_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-cat-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string rimPath = Path.Combine(tempDir, "empty.rim");

            try
            {
                RIMAuto.WriteRim(new RIM(), rimPath, ResourceType.RIM);
                var logger = new StandardLogger();
                int exitCode = CatCommand.Execute(rimPath, "missing", null, logger);
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
