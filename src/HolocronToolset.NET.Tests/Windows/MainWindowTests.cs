using System;
using FluentAssertions;
using HolocronToolset.NET.Tests.TestHelpers;
using HolocronToolset.NET.Windows;
using Xunit;

namespace HolocronToolset.NET.Tests.Windows
{
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_main.py
    // Original: Comprehensive tests for MainWindow
    [Collection("Avalonia Test Collection")]
    public class MainWindowTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public MainWindowTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestMainWindowInit()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_main.py:23
            // Original: def test_main_window_init(qtbot: QtBot):
            var window = new MainWindow();
            window.Show();

            window.Should().NotBeNull();
            window.Title.Should().Contain("Holocron");
            window.UpdateManager.Should().NotBeNull();
        }

        [Fact]
        public void TestMainWindowInitialization()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/windows/test_main_window.py:37
            // Original: def test_main_window_initialization(qtbot):
            var window = new MainWindow();
            window.Show();

            // Verify initialization
            window.UpdateManager.Should().NotBeNull();
        }
    }
}

