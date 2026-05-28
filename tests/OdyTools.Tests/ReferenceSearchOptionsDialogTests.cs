using System.Collections.Generic;
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

        [Test]
        [AvaloniaTest]
        public void ToSearchOptions_ScopeToggles_RoundTripFromDefaults()
        {
            var dialog = new ReferenceSearchOptionsDialog(null, showStrRefNcsOptions: false);
            dialog.SetDefaults(new ReferenceSearchOptions
            {
                SearchOverride = false,
                SearchModules = true,
                SearchChitin = false
            });

            ReferenceSearchOptions options = dialog.ToSearchOptions();

            Assert.That(options.SearchOverride, Is.False);
            Assert.That(options.SearchModules, Is.True);
            Assert.That(options.SearchChitin, Is.False);
        }

        [Test]
        [AvaloniaTest]
        public void ToSearchOptions_CaseSensitivePartialMatch_RoundTripFromDefaults()
        {
            var dialog = new ReferenceSearchOptionsDialog(null, showStrRefNcsOptions: false);
            dialog.SetDefaults(new ReferenceSearchOptions
            {
                CaseSensitive = true,
                PartialMatch = true
            });

            ReferenceSearchOptions options = dialog.ToSearchOptions();

            Assert.That(options.CaseSensitive, Is.True);
            Assert.That(options.PartialMatch, Is.True);
        }

        [Test]
        [AvaloniaTest]
        public void ToSearchOptions_ModuleGlobFilters_RoundTripFromDefaults()
        {
            var dialog = new ReferenceSearchOptionsDialog(null, showStrRefNcsOptions: false);
            dialog.SetDefaults(new ReferenceSearchOptions
            {
                ModuleGlobFilters = new List<string> { "tar_m02*", "danm13.rim" }
            });

            ReferenceSearchOptions options = dialog.ToSearchOptions();

            Assert.That(options.ModuleGlobFilters, Is.Not.Null);
            Assert.That(options.ModuleGlobFilters.Count, Is.EqualTo(2));
            Assert.That(options.ModuleGlobFilters[0], Is.EqualTo("tar_m02*"));
            Assert.That(options.ModuleGlobFilters[1], Is.EqualTo("danm13.rim"));
        }

        [Test]
        [AvaloniaTest]
        public void ToSearchOptions_BlankModuleGlobField_LeavesFiltersNull()
        {
            var dialog = new ReferenceSearchOptionsDialog(null, showStrRefNcsOptions: false);
            dialog.SetDefaults(new ReferenceSearchOptions());

            ReferenceSearchOptions options = dialog.ToSearchOptions();

            Assert.That(options.ModuleGlobFilters, Is.Null);
        }

        [Test]
        public void ParseModuleGlobFilters_SplitsCommaAndNewline()
        {
            List<string> patterns = ReferenceSearchOptionsDialog.ParseModuleGlobFilters("tar_m02*,\ndanm13*\r\nother.mod");

            Assert.That(patterns, Is.Not.Null);
            Assert.That(patterns.Count, Is.EqualTo(3));
            Assert.That(patterns[0], Is.EqualTo("tar_m02*"));
            Assert.That(patterns[1], Is.EqualTo("danm13*"));
            Assert.That(patterns[2], Is.EqualTo("other.mod"));
        }

        [Test]
        public void ParseModuleGlobFilters_BlankText_ReturnsNull()
        {
            Assert.That(ReferenceSearchOptionsDialog.ParseModuleGlobFilters(null), Is.Null);
            Assert.That(ReferenceSearchOptionsDialog.ParseModuleGlobFilters("   "), Is.Null);
        }
    }
}
