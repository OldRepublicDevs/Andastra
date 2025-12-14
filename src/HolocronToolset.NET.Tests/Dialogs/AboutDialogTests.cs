using System;
using FluentAssertions;
using HolocronToolset.NET.Config;
using HolocronToolset.NET.Dialogs;
using HolocronToolset.NET.Tests.TestHelpers;
using Xunit;

namespace HolocronToolset.NET.Tests.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_dialogs.py
    // Original: Comprehensive tests for dialogs
    [Collection("Avalonia Test Collection")]
    public class AboutDialogTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public AboutDialogTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestAboutDialogInit()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_dialogs.py:11
            // Original: def test_about_dialog_init(qtbot: QtBot):
            var dialog = new AboutDialog();
            dialog.Show();

            dialog.Should().NotBeNull();
            dialog.Title.Should().Contain("About");
        }
    }
}

