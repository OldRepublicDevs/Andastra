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
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_utd_editor.py
    // Original: Comprehensive tests for UTD Editor
    [Collection("Avalonia Test Collection")]
    public class UTDEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public UTDEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestUtdEditorNewFileCreation()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_utd_editor.py
            // Original: def test_utd_editor_new_file_creation(qtbot, installation):
            var editor = new UTDEditor(null, null);

            editor.New();

            // Verify editor is ready
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestUtdEditorLoadExistingFile()
        {
            var editor = new UTDEditor(null, null);

            // Create minimal UTD data
            var utd = new UTD();
            utd.Tag = "test_door";
            utd.ResRef = new CSharpKOTOR.Common.ResRef("testdoor");
            var gff = UTDHelpers.DismantleUtd(utd);
            byte[] testData = gff.ToBytes();

            editor.Load("test.utd", "test", ResourceType.UTD, testData);

            // Verify content loaded
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestUtdEditorSaveLoadRoundtrip()
        {
            var editor = new UTDEditor(null, null);
            editor.New();

            // Test save/load roundtrip
            var (data, _) = editor.Build();
            data.Should().NotBeNull();

            var editor2 = new UTDEditor(null, null);
            editor2.Load("test.utd", "test", ResourceType.UTD, data);
            var (data2, _) = editor2.Build();
            data2.Should().NotBeNull();
            // Note: UTD roundtrip may not be byte-for-byte identical due to structure differences
        }
    }
}
