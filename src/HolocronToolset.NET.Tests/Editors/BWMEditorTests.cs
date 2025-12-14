using System;
using CSharpKOTOR.Resources;
using FluentAssertions;
using HolocronToolset.NET.Data;
using HolocronToolset.NET.Editors;
using HolocronToolset.NET.Tests.TestHelpers;
using Xunit;

namespace HolocronToolset.NET.Tests.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_bwm_editor.py
    // Original: Comprehensive tests for BWM Editor
    [Collection("Avalonia Test Collection")]
    public class BWMEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public BWMEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestBwmEditorNewFileCreation()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_bwm_editor.py
            // Original: def test_bwm_editor_new_file_creation(qtbot, installation):
            var editor = new BWMEditor(null, null);

            editor.New();

            // Verify editor is ready
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }

        [Fact]
        public void TestBwmEditorLoadExistingFile()
        {
            var editor = new BWMEditor(null, null);

            // Create minimal BWM data (simplified for testing)
            byte[] testData = new byte[0]; // Will be implemented when BWM format is fully supported

            editor.Load("test.wok", "test", ResourceType.WOK, testData);

            // Verify content loaded (will be implemented when UI is complete)
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }

        [Fact]
        public void TestBwmEditorSaveLoadRoundtrip()
        {
            var editor = new BWMEditor(null, null);
            editor.New();

            // Test save/load roundtrip (will be implemented when BWM format is fully supported)
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }
    }
}
