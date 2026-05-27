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
    public class ExtractCommandTests
    {
        [Test]
        public void ExecuteExtractBif_WritesExtractedResourceFiles()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-extract-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string bifPath = Path.Combine(tempDir, "sample.bif");
                string outputDir = Path.Combine(tempDir, "out");

                byte[] utcBytes = GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.UTC);
                var bif = new BIF();
                bif.SetData(new ResRef("creature_a"), ResourceType.UTC, utcBytes);
                File.WriteAllBytes(bifPath, new BIFBinaryWriter(bif).Write());

                var logger = new StandardLogger();
                int exitCode = ExtractCommand.Execute(bifPath, outputDir, null, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(Directory.GetFiles(outputDir).Length, Is.GreaterThan(0));
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
