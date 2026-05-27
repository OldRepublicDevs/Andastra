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
    }
}
