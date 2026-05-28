using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource.Formats.NCS;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ScriptToolCommandsTests
    {
        [Test]
        public void ExecuteDisassemble_ValidNcs_WritesOutput()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-disasm-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string ncsPath = Path.Combine(tempDir, "test.ncs");
            string outputPath = Path.Combine(tempDir, "test.ncsdis");

            try
            {
                NCS ncs = NCSAuto.CompileNss("void main() { }", BioWareGame.K2);
                Assert.That(ncs, Is.Not.Null);
                NCSAuto.WriteNcs(ncs, ncsPath);

                var logger = new StandardLogger();
                int exitCode = ScriptToolCommands.ExecuteDisassemble(ncsPath, outputPath, logger);

                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(outputPath), Is.True);
                Assert.That(new FileInfo(outputPath).Length, Is.GreaterThan(0));
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
        public void ExecuteDisassemble_MissingInput_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = ScriptToolCommands.ExecuteDisassemble(
                Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".ncs"),
                null,
                logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }

        [Test]
        public void ExecuteAssemble_ValidNss_WritesNcs()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-assemble-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            string nssPath = Path.Combine(tempDir, "test.nss");
            string ncsPath = Path.Combine(tempDir, "test.ncs");

            try
            {
                File.WriteAllText(nssPath, "void main() { }");

                var logger = new StandardLogger();
                int exitCode = ScriptToolCommands.ExecuteAssemble(nssPath, ncsPath, null, false, "k2", logger);

                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(ncsPath), Is.True);
                Assert.That(new FileInfo(ncsPath).Length, Is.GreaterThan(0));
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
