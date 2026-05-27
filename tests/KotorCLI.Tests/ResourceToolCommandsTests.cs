using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Tools;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ResourceToolCommandsTests
    {
        [Test]
        public void ExecuteTextureConvert_MissingInput_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = ResourceToolCommands.ExecuteTextureConvert(
                Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".tpc"),
                null,
                null,
                logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void ExecuteModelConvert_MissingInput_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = ResourceToolCommands.ExecuteModelConvert(
                Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".mdl"),
                null,
                true,
                null,
                logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class MergeGffCommandsTests
    {
        [Test]
        public void ExecuteMerge_OverlaysSourceFieldsOntoTarget()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-merge-" + Guid.NewGuid().ToString("N"));
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

                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteMerge(targetPath, sourcePath, outputPath, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(outputPath), Is.True);

                GFF merged = new GFFBinaryReader(File.ReadAllBytes(outputPath)).Load();
                Assert.That(merged.Root.GetString("Tag"), Is.EqualTo("source_tag"));
                Assert.That(merged.Root.GetString("Comment"), Is.EqualTo("keep_me"));
                Assert.That(merged.Root.GetString("Description"), Is.EqualTo("from_source"));
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
        public void MergeGffFiles_MissingSource_Throws()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-merge-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string targetPath = Path.Combine(tempDir, "target.utc");

            try
            {
                var targetGff = new GFF(GFFContent.GFF);
                targetGff.Root.SetString("Tag", "x");
                File.WriteAllBytes(targetPath, GFFAuto.BytesGff(targetGff, ResourceType.UTC));

                Assert.Throws<FileNotFoundException>(() =>
                    Utilities.MergeGffFiles(targetPath, Path.Combine(tempDir, "missing.utc"), targetPath));
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
