using System;
using System.IO;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class CompileCommandTests
    {
        private const string MinimalConfig = @"[package]
name = ""testpack""

[target]
name = ""default""
file = ""test.mod""
";

        [Test]
        public void Execute_NoConfigDirectory_ExitsNonZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-compile-nocfg-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = CompileCommand.Execute(new[] { "default" }, false, null, null, logger);

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
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-compile-unknown-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = CompileCommand.Execute(new[] { "missing-target" }, false, null, null, logger);

                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDirectory);
                DeleteDirectorySafe(projectDir);
            }
        }

        [Test]
        public void Execute_NoNssSources_ExitsZero()
        {
            string projectDir = Path.Combine(Path.GetTempPath(), "kotorcli-compile-nonss-" + Guid.NewGuid().ToString("N"));
            string originalDirectory = Directory.GetCurrentDirectory();

            try
            {
                Directory.CreateDirectory(projectDir);
                File.WriteAllText(Path.Combine(projectDir, "kotorcli.cfg"), MinimalConfig);
                Directory.SetCurrentDirectory(projectDir);

                var logger = new StandardLogger();
                int exitCode = CompileCommand.Execute(new[] { "default" }, false, null, null, logger);

                Assert.That(exitCode, Is.EqualTo(0));
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
