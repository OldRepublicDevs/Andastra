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

        [Test]
        public void ExecuteDiff_IdenticalFiles_ExitsZero()
        {
            string tempFile1 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-a-" + Guid.NewGuid().ToString("N") + ".txt");
            string tempFile2 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-b-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile1, "same content\n");
            File.WriteAllText(tempFile2, "same content\n");

            try
            {
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteDiff(tempFile1, tempFile2, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
            }
            finally
            {
                if (File.Exists(tempFile1))
                {
                    File.Delete(tempFile1);
                }

                if (File.Exists(tempFile2))
                {
                    File.Delete(tempFile2);
                }
            }
        }

        [Test]
        public void ExecuteDiff_DifferentFiles_ExitsNonZero()
        {
            string tempFile1 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-a-" + Guid.NewGuid().ToString("N") + ".txt");
            string tempFile2 = Path.Combine(Path.GetTempPath(), "kotorcli-diff-b-" + Guid.NewGuid().ToString("N") + ".txt");
            File.WriteAllText(tempFile1, "alpha\n");
            File.WriteAllText(tempFile2, "beta\n");

            try
            {
                var logger = new StandardLogger();
                int exitCode = UtilityCommands.ExecuteDiff(tempFile1, tempFile2, null, logger);
                Assert.That(exitCode, Is.EqualTo(1));
            }
            finally
            {
                if (File.Exists(tempFile1))
                {
                    File.Delete(tempFile1);
                }

                if (File.Exists(tempFile2))
                {
                    File.Delete(tempFile2);
                }
            }
        }
    }
}
