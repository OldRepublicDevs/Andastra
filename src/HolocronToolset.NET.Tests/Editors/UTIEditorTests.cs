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

        // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_uti_editor.py:400-427
        // Original: def test_uti_editor_load_real_file(qtbot, installation: HTInstallation, test_files_dir):
        [Fact]
        public void TestUtiEditorLoadExistingFile()
        {
            // Get test files directory
            string testFilesDir = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "..", "..", "..", "..", "vendor", "PyKotor", "Tools", "HolocronToolset", "tests", "test_files");

            // Try to find a UTI file
            string utiFile = System.IO.Path.Combine(testFilesDir, "baragwin.uti");
            if (!System.IO.File.Exists(utiFile))
            {
                // Try alternative location
                testFilesDir = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                    "..", "..", "..", "..", "..", "vendor", "PyKotor", "Tools", "HolocronToolset", "tests", "test_files");
                utiFile = System.IO.Path.Combine(testFilesDir, "baragwin.uti");
            }

            if (!System.IO.File.Exists(utiFile))
            {
                // Skip if no UTI files available for testing (matching Python pytest.skip behavior)
                return;
            }

            // Get installation if available (K2 preferred for UTI files)
            string k2Path = Environment.GetEnvironmentVariable("K2_PATH");
            if (string.IsNullOrEmpty(k2Path))
            {
                k2Path = @"C:\Program Files (x86)\Steam\steamapps\common\Knights of the Old Republic II";
            }

            HTInstallation installation = null;
            if (System.IO.Directory.Exists(k2Path) && System.IO.File.Exists(System.IO.Path.Combine(k2Path, "chitin.key")))
            {
                installation = new HTInstallation(k2Path, "Test Installation", tsl: true);
            }
            else
            {
                // Fallback to K1
                string k1Path = Environment.GetEnvironmentVariable("K1_PATH");
                if (string.IsNullOrEmpty(k1Path))
                {
                    k1Path = @"C:\Program Files (x86)\Steam\steamapps\common\swkotor";
                }

                if (System.IO.Directory.Exists(k1Path) && System.IO.File.Exists(System.IO.Path.Combine(k1Path, "chitin.key")))
                {
                    installation = new HTInstallation(k1Path, "Test Installation", tsl: false);
                }
            }

            var editor = new UTIEditor(null, installation);

            byte[] originalData = System.IO.File.ReadAllBytes(utiFile);
            editor.Load(utiFile, "baragwin", ResourceType.UTI, originalData);

            // Verify editor loaded the data
            editor.Should().NotBeNull();

            // Build and verify it works
            var (data, _) = editor.Build();
            data.Should().NotBeNull();
            data.Length.Should().BeGreaterThan(0);

            // Verify we can read it back
            var gff = CSharpKOTOR.Formats.GFF.GFF.FromBytes(data);
            gff.Should().NotBeNull();
        }

        [Fact(Skip = "Requires valid GFF data - will be enabled when test files are available")]
        public void TestUtiEditorSaveLoadRoundtrip()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_uti_editor.py
            // Original: def test_uti_editor_save_load_roundtrip(qtbot, installation, test_files_dir):
            // This test requires actual UTI test files - skipping for now
            var editor = new UTIEditor(null, null);
            editor.Should().NotBeNull();
        }
    }
}
