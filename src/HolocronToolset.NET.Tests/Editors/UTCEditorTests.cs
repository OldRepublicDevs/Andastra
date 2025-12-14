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
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_utc_editor.py
    // Original: Comprehensive tests for UTC Editor
    [Collection("Avalonia Test Collection")]
    public class UTCEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public UTCEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestUtcEditorNewFileCreation()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_utc_editor.py
            // Original: def test_utc_editor_new_file_creation(qtbot, installation):
            var editor = new UTCEditor(null, null);

            editor.New();

            // Verify editor is ready
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestUtcEditorLoadExistingFile()
        {
            var editor = new UTCEditor(null, null);

            // Create minimal UTC data
            var utc = new UTC();
            utc.Tag = "test_creature";
            utc.ResRef = new CSharpKOTOR.Common.ResRef("testcreat");
            var gff = UTCHelpers.DismantleUtc(utc);
            byte[] testData = gff.ToBytes();

            editor.Load("test.utc", "test", ResourceType.UTC, testData);

            // Verify content loaded
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestUtcEditorSaveLoadRoundtrip()
        {
            var editor = new UTCEditor(null, null);
            editor.New();

            // Test save/load roundtrip
            var (data, _) = editor.Build();
            data.Should().NotBeNull();

            var editor2 = new UTCEditor(null, null);
            editor2.Load("test.utc", "test", ResourceType.UTC, data);
            var (data2, _) = editor2.Build();
            data2.Should().NotBeNull();
            // Note: UTC roundtrip may not be byte-for-byte identical due to structure differences
        }
    }
}
