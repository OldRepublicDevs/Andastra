using System.Collections.Generic;
using System.IO;
using Avalonia.Headless.NUnit;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Tools;
using NUnit.Framework;
using OdyTools.Dialogs;

namespace OdyTools.Tests
{
    [TestFixture]
    public class FileResultsDialogReferenceSearchTests
    {
        [Test]
        [AvaloniaTest]
        public void FromReferenceSearch_PopulatesFieldPathSuffix()
        {
            string filepath = Path.Combine(Path.GetTempPath(), "Override", "test_npc.utc");
            var resource = new FileResource("test_npc", ResourceType.UTC, 128, 0, filepath);
            var results = new List<ReferenceSearchResult>
            {
                new ReferenceSearchResult
                {
                    Resource = resource,
                    FieldPath = "ScriptHeartbeat",
                    MatchedValue = "k_test_hb"
                }
            };

            FileResultsDialog dialog = FileResultsDialog.FromReferenceSearch(null, results, null);

            Assert.That(dialog.Ui.ResultList.Items.Count, Is.EqualTo(1));
            Assert.That(dialog.Ui.ResultList.Items[0].ToString(), Does.Contain(":: ScriptHeartbeat"));
        }

        [Test]
        [AvaloniaTest]
        public void FromReferenceSearch_SkipsNullResourceResults()
        {
            var results = new List<ReferenceSearchResult>
            {
                new ReferenceSearchResult
                {
                    Resource = null,
                    FieldPath = "Tag",
                    MatchedValue = "orphan"
                }
            };

            FileResultsDialog dialog = FileResultsDialog.FromReferenceSearch(null, results, null);

            Assert.That(dialog.Ui.ResultList.Items.Count, Is.EqualTo(0));
        }

        [Test]
        [AvaloniaTest]
        public void FromReferenceSearch_SortsDisplayTextAlphabetically()
        {
            var zebraResource = new FileResource(
                "zebra_npc",
                ResourceType.UTC,
                64,
                0,
                Path.Combine(Path.GetTempPath(), "Override", "zebra_npc.utc"));
            var alphaResource = new FileResource(
                "alpha_npc",
                ResourceType.UTC,
                64,
                0,
                Path.Combine(Path.GetTempPath(), "Override", "alpha_npc.utc"));

            var results = new List<ReferenceSearchResult>
            {
                new ReferenceSearchResult
                {
                    Resource = zebraResource,
                    FieldPath = "Tag",
                    MatchedValue = "z_tag"
                },
                new ReferenceSearchResult
                {
                    Resource = alphaResource,
                    FieldPath = "Tag",
                    MatchedValue = "a_tag"
                }
            };

            FileResultsDialog dialog = FileResultsDialog.FromReferenceSearch(null, results, null);

            Assert.That(dialog.Ui.ResultList.Items.Count, Is.EqualTo(2));
            string first = dialog.Ui.ResultList.Items[0].ToString();
            string second = dialog.Ui.ResultList.Items[1].ToString();
            Assert.That(string.Compare(first, second, System.StringComparison.OrdinalIgnoreCase), Is.LessThanOrEqualTo(0));
            Assert.That(first, Does.Contain("alpha_npc"));
        }

        [Test]
        [AvaloniaTest]
        public void FromReferenceSearch_EmptyResults_LeavesListEmpty()
        {
            FileResultsDialog dialog = FileResultsDialog.FromReferenceSearch(
                null,
                new List<ReferenceSearchResult>(),
                null);

            Assert.That(dialog.Ui.ResultList.Items.Count, Is.EqualTo(0));
        }

        [Test]
        [AvaloniaTest]
        public void FromReferenceSearch_NullResults_LeavesListEmpty()
        {
            FileResultsDialog dialog = FileResultsDialog.FromReferenceSearch(null, null, null);

            Assert.That(dialog.Ui.ResultList.Items.Count, Is.EqualTo(0));
        }

        [Test]
        [AvaloniaTest]
        public void FromReferenceSearch_NoFieldPath_UsesBaseDisplayOnly()
        {
            string filepath = Path.Combine(Path.GetTempPath(), "Override", "test_npc.utc");
            var resource = new FileResource("test_npc", ResourceType.UTC, 128, 0, filepath);
            var results = new List<ReferenceSearchResult>
            {
                new ReferenceSearchResult
                {
                    Resource = resource,
                    FieldPath = null,
                    MatchedValue = "npc_tag"
                }
            };

            FileResultsDialog dialog = FileResultsDialog.FromReferenceSearch(null, results, null);

            Assert.That(dialog.Ui.ResultList.Items.Count, Is.EqualTo(1));
            string display = dialog.Ui.ResultList.Items[0].ToString();
            Assert.That(display, Does.Contain("Override/test_npc.utc"));
            Assert.That(display, Does.Not.Contain("::"));
        }
    }
}
