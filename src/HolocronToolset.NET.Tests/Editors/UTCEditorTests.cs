using System;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using FluentAssertions;
using HolocronToolset.NET.Data;
using HolocronToolset.NET.Editors;
using HolocronToolset.NET.Tests.TestHelpers;
using Xunit;
using GFFAuto = CSharpKOTOR.Formats.GFF.GFFAuto;

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

        // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_utc_editor.py:2413-2433
        // Original: def test_utc_editor_load_real_file(qtbot, installation: HTInstallation, test_files_dir):
        [Fact]
        public void TestUtcEditorLoadExistingFile()
        {
            // Get test files directory
            string testFilesDir = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "..", "..", "..", "..", "vendor", "PyKotor", "Tools", "HolocronToolset", "tests", "test_files");

            // Try to find a UTC file
            string utcFile = System.IO.Path.Combine(testFilesDir, "p_hk47.utc");
            if (!System.IO.File.Exists(utcFile))
            {
                // Try alternative location
                testFilesDir = System.IO.Path.Combine(
                    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                    "..", "..", "..", "..", "..", "vendor", "PyKotor", "Tools", "HolocronToolset", "tests", "test_files");
                utcFile = System.IO.Path.Combine(testFilesDir, "p_hk47.utc");
            }

            if (!System.IO.File.Exists(utcFile))
            {
                // Skip if no UTC files available for testing (matching Python pytest.skip behavior)
                return;
            }

            // Get installation if available
            string k1Path = Environment.GetEnvironmentVariable("K1_PATH");
            if (string.IsNullOrEmpty(k1Path))
            {
                k1Path = @"C:\Program Files (x86)\Steam\steamapps\common\swkotor";
            }

            HTInstallation installation = null;
            if (System.IO.Directory.Exists(k1Path) && System.IO.File.Exists(System.IO.Path.Combine(k1Path, "chitin.key")))
            {
                installation = new HTInstallation(k1Path, "Test Installation", tsl: false);
            }

            var editor = new UTCEditor(null, installation);

            byte[] originalData = System.IO.File.ReadAllBytes(utcFile);
            editor.Load(utcFile, "p_hk47", ResourceType.UTC, originalData);

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
        public void TestUtcEditorSaveLoadRoundtrip()
        {
            // Matching PyKotor implementation at Tools/HolocronToolset/tests/gui/editors/test_utc_editor.py
            // Original: def test_utc_editor_save_load_roundtrip(qtbot, installation, test_files_dir):
            // This test requires actual UTC test files - skipping for now
            var editor = new UTCEditor(null, null);
            editor.Should().NotBeNull();
        }
    }
}
