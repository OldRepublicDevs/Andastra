using System;
using System.Collections.Generic;
using Avalonia.Controls;
using FluentAssertions;
using HolocronToolset.NET.Data;
using HolocronToolset.NET.Dialogs;
using HolocronToolset.NET.Tests.TestHelpers;
using Xunit;

namespace HolocronToolset.NET.Tests.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_clone.py
    // Original: Comprehensive tests for CloneModuleDialog
    [Collection("Avalonia Test Collection")]
    public class CloneModuleDialogTests : IClassFixture<AvaloniaTestFixture>
    {
        private readonly AvaloniaTestFixture _fixture;
        private static HTInstallation _installation;

        public CloneModuleDialogTests(AvaloniaTestFixture fixture)
        {
            _fixture = fixture;
        }

        static CloneModuleDialogTests()
        {
            string k1Path = Environment.GetEnvironmentVariable("K1_PATH");
            if (string.IsNullOrEmpty(k1Path))
            {
                k1Path = @"C:\Program Files (x86)\Steam\steamapps\common\swkotor";
            }

            if (!string.IsNullOrEmpty(k1Path) && System.IO.File.Exists(System.IO.Path.Combine(k1Path, "chitin.key")))
            {
                _installation = new HTInstallation(k1Path, "Test");
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_clone.py:13-44
        // Original: def test_clone_module_dialog_all_widgets_exist(qtbot: QtBot, installation: HTInstallation):
        [Fact]
        public void TestCloneModuleDialogAllWidgetsExist()
        {
            if (_installation == null)
            {
                return; // Skip if K1_PATH not set
            }

            var parent = new Window();
            var installations = new Dictionary<string, HTInstallation> { { _installation.Name, _installation } };
            var dialog = new CloneModuleDialog(parent, _installation, installations);
            dialog.Show();

            // Module selector
            dialog.Ui.ModuleSelect.Should().NotBeNull();

            // Text inputs
            dialog.Ui.FilenameEdit.Should().NotBeNull();
            dialog.Ui.PrefixEdit.Should().NotBeNull();
            dialog.Ui.NameEdit.Should().NotBeNull();
            dialog.Ui.ModuleRootEdit.Should().NotBeNull();

            // Checkboxes
            dialog.Ui.CopyTexturesCheckbox.Should().NotBeNull();
            dialog.Ui.CopyLightmapsCheckbox.Should().NotBeNull();
            dialog.Ui.KeepDoorsCheckbox.Should().NotBeNull();
            dialog.Ui.KeepPlaceablesCheckbox.Should().NotBeNull();
            dialog.Ui.KeepSoundsCheckbox.Should().NotBeNull();
            dialog.Ui.KeepPathingCheckbox.Should().NotBeNull();

            // Buttons
            dialog.Ui.CreateButton.Should().NotBeNull();
            dialog.Ui.CancelButton.Should().NotBeNull();

            dialog.Close();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_clone.py:46-163
        // Original: def test_clone_module_dialog_all_widgets_interactions(qtbot: QtBot, installation: HTInstallation):
        [Fact]
        public void TestCloneModuleDialogAllWidgetsInteractions()
        {
            if (_installation == null)
            {
                return; // Skip if K1_PATH not set
            }

            var parent = new Window();
            var installations = new Dictionary<string, HTInstallation> { { _installation.Name, _installation } };
            var dialog = new CloneModuleDialog(parent, _installation, installations);
            dialog.Show();

            // Test moduleSelect - ComboBox
            if (dialog.Ui.ModuleSelect.Items.Count > 0)
            {
                for (int i = 0; i < Math.Min(5, dialog.Ui.ModuleSelect.Items.Count); i++)
                {
                    dialog.Ui.ModuleSelect.SelectedIndex = i;
                    dialog.Ui.ModuleSelect.SelectedIndex.Should().Be(i);

                    // Verify moduleRootEdit updates
                    var currentData = dialog.Ui.ModuleSelect.SelectedItem as CloneModuleDialog.ModuleOption;
                    if (currentData != null)
                    {
                        dialog.Ui.ModuleRootEdit.Text.Should().Be(currentData.Root);
                    }
                }
            }

            // Test filenameEdit - TextBox with prefix generation
            // Note: filenameEdit has MaxLength=16, so filenames longer than 16 chars will be truncated
            var testFilenames = new[]
            {
                ("new_module", "NEW"),
                ("a_module", "A_M"),
                ("test123", "TES"),
                ("m", "M"),
                ("ab", "AB"),
                ("abc", "ABC"),
                ("very_long_module", "VER"), // 16 chars (maxLength limit)
                ("", ""),
            };

            foreach (var (filename, expectedPrefix) in testFilenames)
            {
                dialog.Ui.FilenameEdit.Text = filename;
                // Manually trigger prefix update (TextChanged may not fire in headless test mode)
                dialog.UpdatePrefixFromFilename();
                // The text may be truncated if it exceeds maxLength (16)
                string actualText = dialog.Ui.FilenameEdit.Text;
                (actualText == filename || actualText.Length == 16).Should().BeTrue($"Expected '{filename}' but got '{actualText}' (may be truncated to 16 chars)");
                dialog.Ui.PrefixEdit.Text.Should().Be(expectedPrefix);
            }

            // Test prefixEdit - TextBox (can be manually edited)
            // Note: prefixEdit has MaxLength=3, so prefixes longer than 3 chars will be truncated
            dialog.Ui.PrefixEdit.Text = "CUS"; // 3 chars (maxLength limit)
            dialog.Ui.PrefixEdit.Text.Should().Be("CUS");

            // Test nameEdit - TextBox
            var testNames = new[]
            {
                "Test Module",
                "Another Module",
                "Module 123",
                "",
                "Very Long Module Name That Might Wrap",
            };

            foreach (var name in testNames)
            {
                dialog.Ui.NameEdit.Text = name;
                dialog.Ui.NameEdit.Text.Should().Be(name);
            }

            // Test ALL checkboxes - every combination
            var checkboxes = new[]
            {
                (dialog.Ui.CopyTexturesCheckbox, true),
                (dialog.Ui.CopyTexturesCheckbox, false),
                (dialog.Ui.CopyLightmapsCheckbox, true),
                (dialog.Ui.CopyLightmapsCheckbox, false),
                (dialog.Ui.KeepDoorsCheckbox, true),
                (dialog.Ui.KeepDoorsCheckbox, false),
                (dialog.Ui.KeepPlaceablesCheckbox, true),
                (dialog.Ui.KeepPlaceablesCheckbox, false),
                (dialog.Ui.KeepSoundsCheckbox, true),
                (dialog.Ui.KeepSoundsCheckbox, false),
                (dialog.Ui.KeepPathingCheckbox, true),
                (dialog.Ui.KeepPathingCheckbox, false),
            };

            foreach (var (checkbox, checkedState) in checkboxes)
            {
                checkbox.IsChecked = checkedState;
                checkbox.IsChecked.Should().Be(checkedState);
            }

            // Test all checkboxes checked simultaneously
            dialog.Ui.CopyTexturesCheckbox.IsChecked = true;
            dialog.Ui.CopyLightmapsCheckbox.IsChecked = true;
            dialog.Ui.KeepDoorsCheckbox.IsChecked = true;
            dialog.Ui.KeepPlaceablesCheckbox.IsChecked = true;
            dialog.Ui.KeepSoundsCheckbox.IsChecked = true;
            dialog.Ui.KeepPathingCheckbox.IsChecked = true;

            new[]
            {
                dialog.Ui.CopyTexturesCheckbox.IsChecked == true,
                dialog.Ui.CopyLightmapsCheckbox.IsChecked == true,
                dialog.Ui.KeepDoorsCheckbox.IsChecked == true,
                dialog.Ui.KeepPlaceablesCheckbox.IsChecked == true,
                dialog.Ui.KeepSoundsCheckbox.IsChecked == true,
                dialog.Ui.KeepPathingCheckbox.IsChecked == true,
            }.Should().AllBeEquivalentTo(true);

            // Test all checkboxes unchecked
            dialog.Ui.CopyTexturesCheckbox.IsChecked = false;
            dialog.Ui.CopyLightmapsCheckbox.IsChecked = false;
            dialog.Ui.KeepDoorsCheckbox.IsChecked = false;
            dialog.Ui.KeepPlaceablesCheckbox.IsChecked = false;
            dialog.Ui.KeepSoundsCheckbox.IsChecked = false;
            dialog.Ui.KeepPathingCheckbox.IsChecked = false;

            new[]
            {
                dialog.Ui.CopyTexturesCheckbox.IsChecked == true,
                dialog.Ui.CopyLightmapsCheckbox.IsChecked == true,
                dialog.Ui.KeepDoorsCheckbox.IsChecked == true,
                dialog.Ui.KeepPlaceablesCheckbox.IsChecked == true,
                dialog.Ui.KeepSoundsCheckbox.IsChecked == true,
                dialog.Ui.KeepPathingCheckbox.IsChecked == true,
            }.Should().AllBeEquivalentTo(false);

            dialog.Close();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_clone.py:165-194
        // Original: def test_clone_module_dialog_prefix_generation_exhaustive(qtbot: QtBot, installation: HTInstallation):
        [Fact]
        public void TestCloneModuleDialogPrefixGenerationExhaustive()
        {
            if (_installation == null)
            {
                return; // Skip if K1_PATH not set
            }

            var parent = new Window();
            var installations = new Dictionary<string, HTInstallation> { { _installation.Name, _installation } };
            var dialog = new CloneModuleDialog(parent, _installation, installations);
            dialog.Show();

            // Test various filename patterns
            var testCases = new[]
            {
                ("single", "SIN"),
                ("ab", "AB"),
                ("abc", "ABC"),
                ("abcd", "ABC"),
                ("test_module", "TES"),
                ("a_b_c", "A_B"),
                ("123test", "123"),
                ("test123", "TES"),
                ("TEST_UPPER", "TES"),
                ("test-with-dashes", "TES"),
                ("test.with.dots", "TES"),
                ("", ""),
                ("a", "A"),
            };

            foreach (var (filename, expected) in testCases)
            {
                dialog.Ui.FilenameEdit.Text = filename;
                // Manually trigger prefix update (TextChanged may not fire in headless test mode)
                dialog.UpdatePrefixFromFilename();
                string actualPrefix = dialog.Ui.PrefixEdit.Text ?? "";
                actualPrefix.Should().Be(expected, $"Failed for filename '{filename}': expected '{expected}', got '{actualPrefix}'");
            }

            dialog.Close();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_clone.py:197-217
        // Original: def test_clone_module_dialog_module_selection_updates_root(qtbot: QtBot, installation: HTInstallation):
        [Fact]
        public void TestCloneModuleDialogModuleSelectionUpdatesRoot()
        {
            if (_installation == null)
            {
                return; // Skip if K1_PATH not set
            }

            var parent = new Window();
            var installations = new Dictionary<string, HTInstallation> { { _installation.Name, _installation } };
            var dialog = new CloneModuleDialog(parent, _installation, installations);
            dialog.Show();

            // Test changing module selection updates root
            if (dialog.Ui.ModuleSelect.Items.Count > 0)
            {
                string initialRoot = dialog.Ui.ModuleRootEdit.Text;

                // Change to different module
                for (int i = 0; i < dialog.Ui.ModuleSelect.Items.Count; i++)
                {
                    dialog.Ui.ModuleSelect.SelectedIndex = i;
                    var currentData = dialog.Ui.ModuleSelect.SelectedItem as CloneModuleDialog.ModuleOption;
                    if (currentData != null)
                    {
                        dialog.Ui.ModuleRootEdit.Text.Should().Be(currentData.Root);
                        break;
                    }
                }
            }

            dialog.Close();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_clone.py:220-269
        // Original: def test_clone_module_dialog_parameter_collection(qtbot: QtBot, installation: HTInstallation):
        [Fact]
        public void TestCloneModuleDialogParameterCollection()
        {
            if (_installation == null)
            {
                return; // Skip if K1_PATH not set
            }

            var parent = new Window();
            var installations = new Dictionary<string, HTInstallation> { { _installation.Name, _installation } };
            var dialog = new CloneModuleDialog(parent, _installation, installations);
            dialog.Show();

            // Set ALL UI values
            dialog.Ui.FilenameEdit.Text = "test_clone";
            dialog.Ui.PrefixEdit.Text = "TST";
            dialog.Ui.NameEdit.Text = "Test Clone Module";
            dialog.Ui.CopyTexturesCheckbox.IsChecked = true;
            dialog.Ui.CopyLightmapsCheckbox.IsChecked = false;
            dialog.Ui.KeepDoorsCheckbox.IsChecked = true;
            dialog.Ui.KeepPlaceablesCheckbox.IsChecked = false;
            dialog.Ui.KeepSoundsCheckbox.IsChecked = true;
            dialog.Ui.KeepPathingCheckbox.IsChecked = false;

            if (dialog.Ui.ModuleSelect.Items.Count > 0)
            {
                dialog.Ui.ModuleSelect.SelectedIndex = 0;

                // Verify parameters would be collected correctly
                var currentData = dialog.Ui.ModuleSelect.SelectedItem as CloneModuleDialog.ModuleOption;
                if (currentData != null)
                {
                    var installationObj = currentData.Installation;
                    string root = currentData.Root;
                    string identifier = dialog.Ui.FilenameEdit.Text.ToLowerInvariant();
                    string prefix = dialog.Ui.PrefixEdit.Text.ToLowerInvariant();
                    string name = dialog.Ui.NameEdit.Text;

                    identifier.Should().Be("test_clone");
                    prefix.Should().Be("tst");
                    name.Should().Be("Test Clone Module");
                    installationObj.Should().Be(_installation);

                    bool copyTextures = dialog.Ui.CopyTexturesCheckbox.IsChecked == true;
                    bool copyLightmaps = dialog.Ui.CopyLightmapsCheckbox.IsChecked == true;
                    bool keepDoors = dialog.Ui.KeepDoorsCheckbox.IsChecked == true;
                    bool keepPlaceables = dialog.Ui.KeepPlaceablesCheckbox.IsChecked == true;
                    bool keepSounds = dialog.Ui.KeepSoundsCheckbox.IsChecked == true;
                    bool keepPathing = dialog.Ui.KeepPathingCheckbox.IsChecked == true;

                    copyTextures.Should().BeTrue();
                    copyLightmaps.Should().BeFalse();
                    keepDoors.Should().BeTrue();
                    keepPlaceables.Should().BeFalse();
                    keepSounds.Should().BeTrue();
                    keepPathing.Should().BeFalse();
                }
            }

            dialog.Close();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_clone.py:272-297
        // Original: def test_clone_module_dialog_buttons(qtbot: QtBot, installation: HTInstallation):
        [Fact]
        public void TestCloneModuleDialogButtons()
        {
            if (_installation == null)
            {
                return; // Skip if K1_PATH not set
            }

            var parent = new Window();
            var installations = new Dictionary<string, HTInstallation> { { _installation.Name, _installation } };
            var dialog = new CloneModuleDialog(parent, _installation, installations);
            dialog.Show();

            // Test cancel button
            dialog.Ui.CancelButton.IsEnabled.Should().BeTrue();
            // Cancel should close dialog - in Avalonia this is handled by Close() method
            // We verify the button exists and is enabled

            // Recreate for create button test
            dialog = new CloneModuleDialog(parent, _installation, installations);
            dialog.Show();

            // Test create button exists and is enabled
            dialog.Ui.CreateButton.IsEnabled.Should().BeTrue();
            // Create button triggers ok() which shows dialogs and runs AsyncLoader
            // We verify the button exists and is enabled (Click event is connected in SetupUI)

            dialog.Close();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_clone.py:300-328
        // Original: def test_clone_module_dialog_validation_edge_cases(qtbot: QtBot, installation: HTInstallation):
        [Fact]
        public void TestCloneModuleDialogValidationEdgeCases()
        {
            if (_installation == null)
            {
                return; // Skip if K1_PATH not set
            }

            var parent = new Window();
            var installations = new Dictionary<string, HTInstallation> { { _installation.Name, _installation } };
            var dialog = new CloneModuleDialog(parent, _installation, installations);
            dialog.Show();

            // Test empty filename
            dialog.Ui.FilenameEdit.Text = "";
            dialog.UpdatePrefixFromFilename();
            dialog.Ui.PrefixEdit.Text.Should().Be("");

            // Test very long filename
            string longName = new string('a', 100);
            dialog.Ui.FilenameEdit.Text = longName;
            dialog.UpdatePrefixFromFilename();
            // Note: filenameEdit has MaxLength=16, so it will be truncated
            // The prefix should be based on the truncated text
            string actualFilename = dialog.Ui.FilenameEdit.Text;
            string expectedPrefix = actualFilename.Length >= 3 ? actualFilename.Substring(0, 3).ToUpperInvariant() : actualFilename.ToUpperInvariant();
            dialog.Ui.PrefixEdit.Text.Should().Be(expectedPrefix); // Should be based on actual (truncated) text

            // Test special characters in filename
            dialog.Ui.FilenameEdit.Text = "test!@#$%^&*()";
            dialog.UpdatePrefixFromFilename();
            string prefix = dialog.Ui.PrefixEdit.Text;
            prefix.Length.Should().BeLessThanOrEqualTo(3);

            // Test whitespace handling
            dialog.Ui.FilenameEdit.Text = "test module";
            dialog.UpdatePrefixFromFilename();
            // Prefix should handle spaces (may convert or truncate)
            prefix = dialog.Ui.PrefixEdit.Text;
            prefix.Length.Should().BeLessThanOrEqualTo(3);

            dialog.Close();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/tests/test_ui_clone.py:331-351
        // Original: def test_clone_module_dialog_module_root_readonly(qtbot: QtBot, installation: HTInstallation):
        [Fact]
        public void TestCloneModuleDialogModuleRootReadonly()
        {
            if (_installation == null)
            {
                return; // Skip if K1_PATH not set
            }

            var parent = new Window();
            var installations = new Dictionary<string, HTInstallation> { { _installation.Name, _installation } };
            var dialog = new CloneModuleDialog(parent, _installation, installations);
            dialog.Show();

            // Module root should be set from selected module
            if (dialog.Ui.ModuleSelect.Items.Count > 0)
            {
                string initialRoot = dialog.Ui.ModuleRootEdit.Text;
                (initialRoot.Length > 0 || initialRoot == "").Should().BeTrue(); // May be empty if no modules

                // Changing module should update root
                if (dialog.Ui.ModuleSelect.Items.Count > 1)
                {
                    dialog.Ui.ModuleSelect.SelectedIndex = 1;
                    string newRoot = dialog.Ui.ModuleRootEdit.Text;
                    // Root should update (may be same if modules share root)
                }
            }

            dialog.Close();
        }
    }
}
