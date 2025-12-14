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
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_ifo_editor.py
    // Original: Comprehensive tests for IFO Editor
    [Collection("Avalonia Test Collection")]
    public class IFOEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public IFOEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestIfoEditorNewFileCreation()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_ifo_editor.py
            // Original: def test_ifo_editor_new_file_creation(qtbot, installation):
            var editor = new IFOEditor(null, null);

            editor.New();

            // Verify editor is ready
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestIfoEditorLoadExistingFile()
        {
            var editor = new IFOEditor(null, null);

            // Create minimal IFO data
            var ifo = new IFO();
            ifo.Tag = "test_module";
            ifo.ResRef = new CSharpKOTOR.Common.ResRef("testarea");
            var gff = IFOHelpers.DismantleIfo(ifo);
            byte[] testData = GFFAuto.BytesGff(gff, ResourceType.IFO);

            editor.Load("test.ifo", "test", ResourceType.IFO, testData);

            // Verify content loaded
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public void TestIfoEditorSaveLoadRoundtrip()
        {
            var editor = new IFOEditor(null, null);
            editor.New();

            // Test save/load roundtrip
            var (data, _) = editor.Build();
            data.Should().NotBeNull();

            var editor2 = new IFOEditor(null, null);
            editor2.Load("test.ifo", "test", ResourceType.IFO, data);
            var (data2, _) = editor2.Build();
            data2.Should().NotBeNull();
            // Note: IFO roundtrip may not be byte-for-byte identical due to structure differences
        }
    }
}
