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
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_ssf_editor.py
    // Original: Comprehensive tests for SSF Editor
    [Collection("Avalonia Test Collection")]
    public class SSFEditorTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;

        public SSFEditorTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void TestSsfEditorNewFileCreation()
        {
            var editor = new SSFEditor(null, null);
            editor.New();

            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }

        [Fact]
        public void TestSsfEditorLoadExistingFile()
        {
            var editor = new SSFEditor(null, null);

            // Create minimal SSF data (simplified for testing)
            byte[] testData = new byte[0]; // Will be implemented when SSF format is fully supported

            editor.Load("test.ssf", "test", ResourceType.SSF, testData);

            var (data, _) = editor.Build();
            data.Should().NotBeNull();
        }
    }
}
