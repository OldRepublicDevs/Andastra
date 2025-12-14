using System;
using System.Text;
using CSharpKOTOR.Resources;
using FluentAssertions;
using HolocronToolset.NET.Data;
using HolocronToolset.NET.Editors;
using HolocronToolset.NET.Tests.TestHelpers;
using Xunit;

namespace HolocronToolset.NET.Tests.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_2da_editor.py
    // Original: Comprehensive tests for TwoDA Editor
    [Collection("Avalonia Test Collection")]
    public class TwoDAEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public TwoDAEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestTwoDAEditorNewFileCreation()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_2da_editor.py:21
            // Original: def test_twoda_editor_new_file_creation(qtbot, installation):
            var editor = new TwoDAEditor(null, null);

            editor.New();

            // Verify editor is ready
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }

        [Fact]
        public void TestTwoDAEditorLoadExistingFile()
        {
            var editor = new TwoDAEditor(null, null);

            // Create minimal 2DA data (simplified for testing)
            byte[] testData = new byte[0]; // Will be implemented when 2DA format is fully supported

            editor.Load("test.2da", "test", ResourceType.TwoDA, testData);

            // Verify content loaded (will be implemented when UI is complete)
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }

        [Fact]
        public void TestTwoDAEditorSaveLoadRoundtrip()
        {
            var editor = new TwoDAEditor(null, null);
            editor.New();

            // Test save/load roundtrip (will be implemented when 2DA format is fully supported)
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }
    }
}
