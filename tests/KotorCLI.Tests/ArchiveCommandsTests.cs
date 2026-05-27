using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.RIM;
using KotorCLI.Commands;
using KotorCLI.Logging;
using NUnit.Framework;

namespace KotorCLI.Tests
{
    [TestFixture]
    public class ArchiveCommandsTests
    {
        private static string CreateSampleRim(string tempDir)
        {
            string rimPath = Path.Combine(tempDir, "test.rim");
            var gff = new GFF(GFFContent.GFF);
            gff.Root.SetString("Label", "archive-test");
            byte[] utcBytes = GFFAuto.BytesGff(gff, ResourceType.UTC);

            var rim = new RIM();
            rim.SetData("sample_npc", ResourceType.UTC, utcBytes);
            rim.SetData("other_res", ResourceType.GFF, GFFAuto.BytesGff(new GFF(GFFContent.GFF), ResourceType.GFF));
            RIMAuto.WriteRim(rim, rimPath, ResourceType.RIM);
            return rimPath;
        }

        [Test]
        public void ExecuteListArchive_ListsRimResources()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(rimPath, false, null, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteListArchive_FilterNoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-list-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = ListArchiveCommand.Execute(rimPath, false, "does_not_exist_*", logger);
                Assert.That(exitCode, Is.EqualTo(1));
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
        public void ExecuteSearchArchive_MatchesWildcard()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, "sample_*", false, false, logger);
                Assert.That(exitCode, Is.EqualTo(0));
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
        public void ExecuteSearchArchive_NoMatch_ExitsNonZero()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), "kotorcli-search-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string rimPath = CreateSampleRim(tempDir);
                var logger = new StandardLogger();
                int exitCode = SearchArchiveCommand.Execute(rimPath, "missing_*", false, false, logger);
                Assert.That(exitCode, Is.EqualTo(1));
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
        public void ExecuteLaunch_Stub_ExitsNonZero()
        {
            var logger = new StandardLogger();
            int exitCode = LaunchCommand.Execute(new[] { "test_mod" }, null, null, logger);
            Assert.That(exitCode, Is.EqualTo(1));
        }
    }
}
