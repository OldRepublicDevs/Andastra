using System;
using System.IO;
using BioWare.Common;
using BioWare.Extract.Capsule;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.RIM;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class CreateArchiveCommandTests
    {
        [Test]
        public void Execute_CreateRimFromDirectory_ProducesReadableArchive()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-create-" + Guid.NewGuid().ToString("N"));
            string inputDir = Path.Combine(tempDir, "in");
            string rimPath = Path.Combine(tempDir, "packed.rim");
            Directory.CreateDirectory(inputDir);

            try
            {
                byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
                File.WriteAllBytes(Path.Combine(inputDir, "merchant.utc"), utcBytes);

                var logger = new StandardLogger();
                int exitCode = CreateArchiveCommand.Execute(inputDir, rimPath, "rim", null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(rimPath), Is.True);

                var capsule = new LazyCapsule(rimPath);
                bool found = false;
                foreach (BioWare.Extract.FileResource resource in capsule.GetResources())
                {
                    if (string.Equals(resource.ResName, "merchant", StringComparison.OrdinalIgnoreCase) &&
                        resource.ResType == ResourceType.UTC)
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
        public void Execute_MissingInputDirectory_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-create-missing-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string missingDir = Path.Combine(tempDir, "missing-in");
                string outputPath = Path.Combine(tempDir, "packed.rim");
                var logger = new StandardLogger();
                int exitCode = CreateArchiveCommand.Execute(missingDir, outputPath, "rim", null, logger);
                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_CreateModFromDirectory_ProducesReadableArchive()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-create-mod-" + Guid.NewGuid().ToString("N"));
            string inputDir = Path.Combine(tempDir, "in");
            string modPath = Path.Combine(tempDir, "packed.mod");
            Directory.CreateDirectory(inputDir);

            try
            {
                byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
                File.WriteAllBytes(Path.Combine(inputDir, "merchant.utc"), utcBytes);

                var logger = new StandardLogger();
                int exitCode = CreateArchiveCommand.Execute(inputDir, modPath, "mod", null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(modPath), Is.True);

                var capsule = new LazyCapsule(modPath);
                bool found = false;
                foreach (BioWare.Extract.FileResource resource in capsule.GetResources())
                {
                    if (string.Equals(resource.ResName, "merchant", StringComparison.OrdinalIgnoreCase) &&
                        resource.ResType == ResourceType.UTC)
                    {
                        found = true;
                        break;
                    }
                }

                Assert.That(found, Is.True);
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_UnsupportedArchiveType_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-create-badtype-" + Guid.NewGuid().ToString("N"));
            string inputDir = Path.Combine(tempDir, "in");
            Directory.CreateDirectory(inputDir);

            try
            {
                File.WriteAllBytes(Path.Combine(inputDir, "sample.txt"), new byte[] { 1, 2, 3 });
                string outputPath = Path.Combine(tempDir, "packed.xyz");

                var logger = new StandardLogger();
                int exitCode = CreateArchiveCommand.Execute(inputDir, outputPath, "xyz", null, logger);
                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_CreateRimWithFilter_IncludesMatchingFilesOnly()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-create-filter-" + Guid.NewGuid().ToString("N"));
            string inputDir = Path.Combine(tempDir, "in");
            string rimPath = Path.Combine(tempDir, "packed.rim");
            Directory.CreateDirectory(inputDir);

            try
            {
                byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
                File.WriteAllBytes(Path.Combine(inputDir, "merchant.utc"), utcBytes);
                File.WriteAllBytes(Path.Combine(inputDir, "vendor.utc"), utcBytes);

                var logger = new StandardLogger();
                int exitCode = CreateArchiveCommand.Execute(inputDir, rimPath, "rim", "merchant*", logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(rimPath), Is.True);

                var capsule = new LazyCapsule(rimPath);
                int matchCount = 0;
                foreach (BioWare.Extract.FileResource resource in capsule.GetResources())
                {
                    if (string.Equals(resource.ResName, "merchant", StringComparison.OrdinalIgnoreCase))
                    {
                        matchCount++;
                    }

                    Assert.That(resource.ResName, Is.Not.EqualTo("vendor"));
                }

                Assert.That(matchCount, Is.EqualTo(1));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_CreateRimWithFilterNoMatch_ProducesEmptyArchive()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-create-filter-empty-" + Guid.NewGuid().ToString("N"));
            string inputDir = Path.Combine(tempDir, "in");
            string rimPath = Path.Combine(tempDir, "packed.rim");
            Directory.CreateDirectory(inputDir);

            try
            {
                byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
                File.WriteAllBytes(Path.Combine(inputDir, "merchant.utc"), utcBytes);

                var logger = new StandardLogger();
                int exitCode = CreateArchiveCommand.Execute(inputDir, rimPath, "rim", "missing_*", logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(rimPath), Is.True);

                var capsule = new LazyCapsule(rimPath);
                Assert.That(capsule.GetResources().Count, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
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
