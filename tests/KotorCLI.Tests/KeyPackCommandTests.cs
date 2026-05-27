using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.BIF;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.KEY;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class KeyPackCommandTests
    {
        private static void WriteSampleBif(string bifPath)
        {
            byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
            var bif = new BIF();
            bif.SetData(new ResRef("creature_a"), ResourceType.UTC, utcBytes);
            File.WriteAllBytes(bifPath, new BIFBinaryWriter(bif).Write());
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

        [Test]
        public void Execute_KeyPackFromBifDirectory_ProducesKeyUsableByListArchive()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-keypack-" + Guid.NewGuid().ToString("N"));
            string bifPath = Path.Combine(tempDir, "sample.bif");
            string keyPath = Path.Combine(tempDir, "sample.key");
            Directory.CreateDirectory(tempDir);

            try
            {
                WriteSampleBif(bifPath);

                var logger = new StandardLogger();
                int exitCode = KeyPackCommand.Execute(tempDir, keyPath, null, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(keyPath), Is.True);
                Assert.That(new FileInfo(keyPath).Length, Is.GreaterThan(KEY.HeaderSize));

                int listExitCode = ListArchiveCommand.Execute(bifPath, false, "*.utc", logger);
                Assert.That(listExitCode, Is.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_MissingInputDirectory_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-keypack-missing-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string missingDir = Path.Combine(tempDir, "missing-in");
                string keyPath = Path.Combine(tempDir, "out.key");
                var logger = new StandardLogger();
                int exitCode = KeyPackCommand.Execute(missingDir, keyPath, null, null, logger);
                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }

        [Test]
        public void Execute_WithFilter_ExcludesNonMatchingBifFiles()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-keypack-filter-" + Guid.NewGuid().ToString("N"));
            string includedBif = Path.Combine(tempDir, "keep.bif");
            string excludedBif = Path.Combine(tempDir, "skip.bif");
            string keyPath = Path.Combine(tempDir, "keep.key");
            Directory.CreateDirectory(tempDir);

            try
            {
                WriteSampleBif(includedBif);
                WriteSampleBif(excludedBif);

                var logger = new StandardLogger();
                int exitCode = KeyPackCommand.Execute(tempDir, keyPath, null, "keep*", logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(keyPath), Is.True);

                int listExitCode = ListArchiveCommand.Execute(includedBif, false, null, logger);
                Assert.That(listExitCode, Is.EqualTo(0));

                Assert.That(File.Exists(Path.Combine(tempDir, "skip.key")), Is.False);
            }
            finally
            {
                DeleteDirectorySafe(tempDir);
            }
        }
    }
}
