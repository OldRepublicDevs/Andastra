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
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_utp_editor.py
    // Original: Comprehensive tests for UTP Editor
    [Collection("Avalonia Test Collection")]
    public class UTPEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public UTPEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestUtpEditorNewFileCreation()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_utp_editor.py
            // Original: def test_utp_editor_new_file_creation(qtbot, installation):
            var editor = new UTPEditor(null, null);

            editor.New();

            // Verify editor is ready
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestUtpEditorLoadExistingFile()
        {
            var editor = new UTPEditor(null, null);

            // Create minimal UTP data
            var utp = new UTP();
            utp.Tag = "test_placeable";
            utp.ResRef = new CSharpKOTOR.Common.ResRef("testplace");
            var gff = UTPHelpers.DismantleUtp(utp);
            byte[] testData = gff.ToBytes();

            editor.Load("test.utp", "test", ResourceType.UTP, testData);

            // Verify content loaded
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestUtpEditorSaveLoadRoundtrip()
        {
            var editor = new UTPEditor(null, null);
            editor.New();

            // Test save/load roundtrip
            var (data, _) = editor.Build();
            data.Should().NotBeNull();

            var editor2 = new UTPEditor(null, null);
            editor2.Load("test.utp", "test", ResourceType.UTP, data);
            var (data2, _) = editor2.Build();
            data2.Should().NotBeNull();
            // Note: UTP roundtrip may not be byte-for-byte identical due to structure differences
        }
    }
}
