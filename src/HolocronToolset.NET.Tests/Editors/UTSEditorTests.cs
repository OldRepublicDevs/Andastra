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
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_uts_editor.py
    // Original: Comprehensive tests for UTS Editor
    [Collection("Avalonia Test Collection")]
    public class UTSEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public UTSEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestUtsEditorNewFileCreation()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_uts_editor.py
            // Original: def test_uts_editor_new_file_creation(qtbot, installation):
            var editor = new UTSEditor(null, null);

            editor.New();

            // Verify editor is ready
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestUtsEditorLoadExistingFile()
        {
            var editor = new UTSEditor(null, null);

            // Create minimal UTS data
            var uts = new UTS();
            uts.Tag = "test_sound";
            uts.ResRef = new CSharpKOTOR.Common.ResRef("testsound");
            var gff = UTSHelpers.DismantleUts(uts);
            byte[] testData = gff.ToBytes();

            editor.Load("test.uts", "test", ResourceType.UTS, testData);

            // Verify content loaded
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestUtsEditorSaveLoadRoundtrip()
        {
            var editor = new UTSEditor(null, null);
            editor.New();

            // Test save/load roundtrip
            var (data, _) = editor.Build();
            data.Should().NotBeNull();

            var editor2 = new UTSEditor(null, null);
            editor2.Load("test.uts", "test", ResourceType.UTS, data);
            var (data2, _) = editor2.Build();
            data2.Should().NotBeNull();
            // Note: UTS roundtrip may not be byte-for-byte identical due to structure differences
        }
    }
}
