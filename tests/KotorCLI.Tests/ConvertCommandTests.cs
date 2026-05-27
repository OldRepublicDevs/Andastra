using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ConvertCommandTests
    {
        private const string MinimalConfig = @"[package]
name = ""testpack""

  [package.sources]
  include = ""*.json""

[target]
name = ""default""
file = ""test.mod""
";

        [Test]
        public void Execute_NoConfigDirectory_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-convert-nocfg-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = ConvertCommand.Execute(new[] { "default" }, false, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_UnknownTarget_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-convert-unknown-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = ConvertCommand.Execute(new[] { "missing-target" }, false, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_ConvertsJsonGff_ToBinaryAlongsideSource()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-convert-json-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);

                string jsonPath = Path.Combine(projectDir, "sample.utc.json");
                string binaryPath = Path.Combine(projectDir, "sample.utc");
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "convert-test");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = ConvertCommand.Execute(new[] { "default" }, false, logger);

                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(binaryPath), Is.True);

                GFF roundTrip = GFFAuto.ReadGff(binaryPath, fileFormat: ResourceType.GFF);
                Assert.That(roundTrip.Root.GetString("Label"), Is.EqualTo("convert-test"));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_ConvertsJsonUnderSrcRecursiveGlob()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-convert-srcglob-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();
            const string srcGlobConfig = @"[package]
name = ""testpack""

  [package.sources]
  include = ""src/**/*.json""

[target]
name = ""default""
file = ""test.mod""
";

            try
            {
                Directory.CreateDirectory(projectDir);
                string srcNestedDir = Path.Combine(projectDir, "src", "nested");
                Directory.CreateDirectory(srcNestedDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), srcGlobConfig);

                string jsonPath = Path.Combine(srcNestedDir, "sample.utc.json");
                string binaryPath = Path.Combine(srcNestedDir, "sample.utc");
                var gff = new GFF(GFFContent.GFF);
                gff.Root.SetString("Label", "src-glob-test");
                GFFAuto.WriteGff(gff, jsonPath, ResourceType.GFF_JSON);

                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = ConvertCommand.Execute(new[] { "default" }, false, logger);

                Assert.That(exitCode, Is.EqualTo(0));
                Assert.That(File.Exists(binaryPath), Is.True);

                GFF roundTrip = GFFAuto.ReadGff(binaryPath, fileFormat: ResourceType.GFF);
                Assert.That(roundTrip.Root.GetString("Label"), Is.EqualTo("src-glob-test"));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
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
