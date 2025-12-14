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
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_tlk_editor.py
    // Original: Comprehensive tests for TLK Editor
    [Collection("Avalonia Test Collection")]
    public class TLKEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public TLKEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestTlkEditorNewFileCreation()
        {
            var editor = new TLKEditor(null, null);
            editor.New();

            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }

        [Fact]
        public void TestTlkEditorLoadExistingFile()
        {
            var editor = new TLKEditor(null, null);

            // Create minimal TLK data (simplified for testing)
            byte[] testData = new byte[0]; // Will be implemented when TLK format is fully supported

            editor.Load("test.tlk", "test", ResourceType.TLK, testData);

            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }
    }
}
