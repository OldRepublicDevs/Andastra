using System;
using System.IO;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class UtilityCommandsTests
    {
        [Test]
        public void ExecuteGrep_FindsMatchingLine()
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "kotorcli-grep-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile, "alpha\nbeta needle\ngamma\n");

            try
            {
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteGrep(tempFile, "needle", false, true, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [Test]
        public void ExecuteGrep_NoMatch_ExitsNonZero()
        {
            string tempFile = Path.Combine(Path.GetTempPath(), "kotorcli-grep-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile, "alpha\nbeta\n");

            try
            {
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteGrep(tempFile, "missing", false, false, logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            }
        }

        [Test]
        public void ExecuteGrep_MissingFile_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = UtilityCommands.ExecuteGrep(
                Path.Combine(Path.GetTempPath(), "missing-" + Guid.NewGuid().ToString("N") + ".txt"),
                "x",
                false,
                false,
                logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }
    }
}
