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
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_gff_editor.py
    // Original: Comprehensive tests for GFF Editor
    [Collection("Avalonia Test Collection")]
    public class GFFEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public GFFEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestGffEditorNewFileCreation()
        {
            var editor = new GFFEditor(null, null);
            editor.New();

            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }

        [Fact]
        public void TestGffEditorLoadExistingFile()
        {
            var editor = new GFFEditor(null, null);

            // Create minimal GFF data (simplified for testing)
            byte[] testData = new byte[0]; // Will be implemented when GFF format is fully supported

            editor.Load("test.gff", "test", ResourceType.ARE, testData);

            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }
    }
}
