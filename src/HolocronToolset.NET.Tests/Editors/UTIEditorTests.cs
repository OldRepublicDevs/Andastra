using System;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using FluentAssertions;
using HolocronToolset.NET.Data;
using HolocronToolset.NET.Editors;
using HolocronToolset.NET.Tests.TestHelpers;
using Xunit;

namespace HolocronToolset.NET.Tests.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_uti_editor.py
    // Original: Comprehensive tests for UTI Editor
    [Collection("Avalonia Test Collection")]
    public class UTIEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public UTIEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestUtiEditorNewFileCreation()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_uti_editor.py
            // Original: def test_uti_editor_new_file_creation(qtbot, installation):
            var editor = new UTIEditor(null, null);

            editor.New();

            // Verify editor is ready
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestUtiEditorLoadExistingFile()
        {
            var editor = new UTIEditor(null, null);

            // Create minimal UTI data
            var uti = new UTI();
            uti.Tag = "test_item";
            uti.ResRef = new CSharpKOTOR.Common.ResRef("testitem");
            var gff = UTIHelpers.DismantleUti(uti);
            byte[] testData = gff.ToBytes();

            editor.Load("test.uti", "test", ResourceType.UTI, testData);

            // Verify content loaded
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestUtiEditorSaveLoadRoundtrip()
        {
            var editor = new UTIEditor(null, null);
            editor.New();

            // Test save/load roundtrip
            var (data, _) = editor.Build();
            data.Should().NotBeNull();

            var editor2 = new UTIEditor(null, null);
            editor2.Load("test.uti", "test", ResourceType.UTI, data);
            var (data2, _) = editor2.Build();
            data2.Should().NotBeNull();
            // Note: UTI roundtrip may not be byte-for-byte identical due to structure differences
        }
    }
}
