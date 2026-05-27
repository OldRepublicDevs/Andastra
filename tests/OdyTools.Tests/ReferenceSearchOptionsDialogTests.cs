using BioWare.Tools;
using Avalonia.Headless.NUnit;
using NUnit.Framework;
using OdyTools.Dialogs;

namespace OdyTools.Tests
{
    [TestFixture]
    public class ReferenceSearchOptionsDialogTests
    {
        [Test]
        [AvaloniaTest]
        public void ToSearchOptions_StrRefNcsSection_MapsScanAndMinimum()
        {
            var dialog = new ReferenceSearchOptionsDialog(null, showStrRefNcsOptions: true);
            dialog.SetDefaults(new ReferenceSearchOptions
            {
                IncludeNcsStrRefScan = false,
                NcsStrRefCandidateMinimum = 50
            });

            ReferenceSearchOptions options = dialog.ToSearchOptions();

            Assert.That(options.IncludeNcsStrRefScan, Is.False);
            Assert.That(options.NcsStrRefCandidateMinimum, Is.EqualTo(50));
        }

        [Test]
        [AvaloniaTest]
        public void ToSearchOptions_StrRefNcsSection_BlankMinimum_IsNull()
        {
            var dialog = new ReferenceSearchOptionsDialog(null, showStrRefNcsOptions: true);
            dialog.SetDefaults(new ReferenceSearchOptions
            {
                IncludeNcsStrRefScan = true,
                NcsStrRefCandidateMinimum = null
            });

            ReferenceSearchOptions options = dialog.ToSearchOptions();

            Assert.That(options.IncludeNcsStrRefScan, Is.True);
            Assert.That(options.NcsStrRefCandidateMinimum, Is.Null);
        }

        [Test]
        [AvaloniaTest]
        public void ToSearchOptions_WithoutStrRefNcsSection_LeavesNcsDefaults()
        {
            var dialog = new ReferenceSearchOptionsDialog(null, showStrRefNcsOptions: false);
            dialog.SetDefaults(new ReferenceSearchOptions
            {
                SearchOverride = false,
                IncludeNcsStrRefScan = false
            });

            ReferenceSearchOptions options = dialog.ToSearchOptions();

            Assert.That(options.SearchOverride, Is.False);
            Assert.That(options.IncludeNcsStrRefScan, Is.True);
        }
    }
}
