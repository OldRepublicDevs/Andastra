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
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_ltr_editor.py
    // Original: Comprehensive tests for LTR Editor
    [Collection("Avalonia Test Collection")]
    public class LTREditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public LTREditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestLtrEditorNewFileCreation()
        {
            var editor = new LTREditor(null, null);
            editor.New();

            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }

        [Fact]
        public void TestLtrEditorLoadExistingFile()
        {
            var editor = new LTREditor(null, null);

            // Create minimal LTR data (simplified for testing)
            byte[] testData = new byte[0]; // Will be implemented when LTR format is fully supported

            editor.Load("test.ltr", "test", ResourceType.LTR, testData);

            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }
    }
}
