using Avalonia.Controls;
using Avalonia.Headless.NUnit;
using NUnit.Framework;
using OdyTools.Blender;
using OdyTools.Dialogs;
using OdyTools.Editors;
using OdyTools.Widgets.Settings;
using OdyTools.Widgets;
using OdyTools.Widgets.Edit;
using OdyTools.Windows;

namespace OdyTools.Tests
{
    public class SharedWidgetStartupTests
    {
        [Test]
        [AvaloniaTest]
        public void ResourceList_BindsXamlControlsBeforeShown()
        {
            var widget = new ResourceList();

            Assert.That(widget.Ui, Is.Not.Null);
            Assert.That(widget.Ui.SectionCombo, Is.Not.Null);
            Assert.That(widget.Ui.SearchEdit, Is.Not.Null);
            Assert.That(widget.Ui.ReloadButton, Is.Not.Null);
            Assert.That(widget.Ui.RefreshButton, Is.Not.Null);
            Assert.That(widget.Ui.ResourceTree, Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void SetBindWidget_BindsXamlControlsBeforeShown()
        {
            var widget = new SetBindWidget();

            Assert.That(EditorHelpers.FindControlSafe<ComboBox>(widget, "mouseCombo"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(widget, "setKeysEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "setButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "clearButton"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void CommandPalette_BindsSearchListAndStatusBeforeShown()
        {
            var palette = new CommandPalette();

            Assert.That(EditorHelpers.FindControlSafe<TextBox>(palette, "searchEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<ListBox>(palette, "commandList"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBlock>(palette, "statusLabel"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void CommonEmbeddedWidgets_BindXamlControlsBeforeShown()
        {
            var locStringEdit = new LocalizedStringEdit();
            var breadcrumbs = new BreadcrumbsWidget();
            var terminal = new TerminalWidget();

            Assert.That(EditorHelpers.FindControlSafe<TextBox>(locStringEdit, "locstringText"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(locStringEdit, "editButton"), Is.Not.Null);
            Assert.That(locStringEdit.CanOpenEditorWithoutInstallationForTest(), Is.True);
            Assert.That(EditorHelpers.FindControlSafe<StackPanel>(breadcrumbs, "layout"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(terminal, "terminalOutput"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void ModuleDesignerWindow_BindsMainXamlControlsBeforeShown()
        {
            var window = new ModuleDesignerWindow();

            Assert.That(window.Ui, Is.Not.Null);
            Assert.That(window.Ui.ModuleTree, Is.Not.Null);
            Assert.That(window.Ui.PropertiesTable, Is.Not.Null);
            Assert.That(window.Ui.OpenInBlenderButton, Is.Not.Null);
            Assert.That(window.Ui.BlenderStatusLabel, Is.Not.Null);
            Assert.That(window.Ui.OpenInBlenderButton.IsEnabled, Is.False);
            Assert.That(window.Ui.BlenderStatusLabel.Text, Does.Contain("Open a module"));
        }

        [Test]
        [AvaloniaTest]
        public void ModuleDesignerWindow_OpenInBlenderUsesCurrentModulePath()
        {
            var modulePath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "test_module.mod");
            var launched = false;
            string launchedModulePath = null;

            var window = new ModuleDesignerWindow();
            window.SetModulePathForTests(modulePath);
            window.SetBlenderServicesForTests(
                _ =>
                {
                    var info = new BlenderInfo
                    {
                        Executable = "/usr/bin/blender",
                        Version = (4, 2, 0),
                        IsValid = true,
                        HasKotorblender = true
                    };
                    info.UpdateVersionString();
                    return info;
                },
                (info, port, installationPath, launchedPath, blendFile, background) =>
                {
                    launched = true;
                    launchedModulePath = launchedPath;
                    return System.Diagnostics.Process.GetCurrentProcess();
                });

            Assert.That(window.Ui.OpenInBlenderButton.IsEnabled, Is.True);
            Assert.That(window.TryLaunchBlenderForCurrentModule(), Is.True);
            Assert.That(launched, Is.True);
            Assert.That(launchedModulePath, Is.EqualTo(modulePath));
            Assert.That(window.Ui.BlenderStatusLabel.Text, Does.Contain("Launched Blender"));
        }

        [Test]
        [AvaloniaTest]
        public void ModuleDesignerWindow_OpenInBlenderReportsMissingAddon()
        {
            var window = new ModuleDesignerWindow();
            window.SetModulePathForTests(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "test_module.mod"));
            window.SetBlenderServicesForTests(
                _ =>
                {
                    var info = new BlenderInfo
                    {
                        Executable = "/usr/bin/blender",
                        Version = (4, 2, 0),
                        IsValid = true,
                        HasKotorblender = false,
                        Error = "Blender 4.2.0 found but kotorblender add-on is not installed."
                    };
                    info.UpdateVersionString();
                    return info;
                },
                (info, port, installationPath, launchedPath, blendFile, background) =>
                {
                    Assert.Fail("Blender should not launch without kotorblender.");
                    return null;
                });

            Assert.That(window.TryLaunchBlenderForCurrentModule(), Is.False);
            Assert.That(window.Ui.BlenderStatusLabel.Text, Does.Contain("kotorblender"));
        }

        [Test]
        [AvaloniaTest]
        public void MediaPlayerWidget_BindsPlaybackControlsBeforeShown()
        {
            var widget = new MediaPlayerWidget();

            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "playPauseButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "stopButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Slider>(widget, "timeSlider"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBlock>(widget, "timeLabel"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "muteButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Slider>(widget, "volumeSlider"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "speedButton"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void GitSettingsWidget_RegistersBindAndColourControlsBeforeShown()
        {
            var widget = new GITSettingsWidget();

            Assert.That(EditorHelpers.FindControlSafe<SetBindWidget>(widget, "moveCameraBindEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<SetBindWidget>(widget, "toggleLockInstancesBindEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<ColorEdit>(widget, "undefinedMaterialColourEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<ColorEdit>(widget, "nonWalkGrassMaterialColourEdit"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void ModuleDesignerSettingsWidget_BuildsCustomizationSurfaceBeforeShown()
        {
            var widget = new ModuleDesignerSettingsWidget();

            Assert.That(widget.HasCustomizationSurfaceForTest, Is.True);
            Assert.That(widget.RegisteredBindCountForTest, Is.EqualTo(55));
            Assert.That(widget.RegisteredColourCountForTest, Is.EqualTo(20));
            Assert.That(EditorHelpers.FindControlSafe<SetBindWidget>(widget, "speedBoostCamera3dBindEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<SetBindWidget>(widget, "duplicateObject2dBindEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<ColorEdit>(widget, "undefinedMaterialColourEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<ColorEdit>(widget, "nonWalkGrassMaterialColourEdit"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void ColorEdit_BindsEditableControlsBeforeShown()
        {
            var widget = new ColorEdit();

            Assert.That(EditorHelpers.FindControlSafe<Border>(widget, "colorLabel"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<NumericUpDown>(widget, "colorSpin"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "editButton"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void InstallationsWidget_BindsEditableControlsBeforeShown()
        {
            var widget = new InstallationsWidget();

            Assert.That(EditorHelpers.FindControlSafe<ListBox>(widget, "pathList"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "addPathButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "removePathButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Border>(widget, "pathFrame"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(widget, "pathNameEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(widget, "pathDirEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(widget, "pathTslCheckbox"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void ApplicationSettingsWidget_BindsEditableControlsBeforeShown()
        {
            var widget = new ApplicationSettingsWidget();

            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "resetAttributesButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBlock>(widget, "currentFontLabel"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "fontButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<DataGrid>(widget, "tableWidget"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "addButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "editButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "removeButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<StackPanel>(widget, "verticalLayout_misc"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<StackPanel>(widget, "verticalLayout_3"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void DlgSettingsWidget_BindsEditableControlsBeforeShown()
        {
            var widget = new DLGSettingsWidget();

            Assert.That(EditorHelpers.FindControlSafe<TextBox>(widget, "tlkPathEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(widget, "femaleTlkPathEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(widget, "override2DADirEdit"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void EditorSpecificSettingsDialogs_BindControlsBeforeShown()
        {
            var dlgDialog = new DLGSettingsDialog();
            var utcDialog = new UTCSettingsDialog();

            Assert.That(EditorHelpers.FindControlSafe<ComboBox>(dlgDialog, "installationCombo"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(dlgDialog, "tlkPathEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(dlgDialog, "femaleTlkPathEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(dlgDialog, "override2DADirEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(dlgDialog, "tlkBrowseBtn"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(dlgDialog, "okButton"), Is.Not.Null);

            Assert.That(EditorHelpers.FindControlSafe<ComboBox>(utcDialog, "installationCombo"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(utcDialog, "saveUnusedFieldsCheck"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(utcDialog, "alwaysSaveK2FieldsCheck"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(utcDialog, "okButton"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void MiscSettingsWidget_BindsEditableControlsBeforeShown()
        {
            var widget = new MiscSettingsWidget();

            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(widget, "useBetaChannel"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(widget, "attemptKeepOldGFFFields"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(widget, "saveRimCheck"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(widget, "mergeRimCheck"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<ComboBox>(widget, "moduleSortOptionComboBox"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(widget, "greyRimCheck"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(widget, "showPreviewUTCCheck"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(widget, "showPreviewUTPCheck"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(widget, "showPreviewUTDCheck"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(widget, "tempDirEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<ComboBox>(widget, "gffEditorCombo"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(widget, "ncsToolEdit"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<TextBox>(widget, "nssCompEdit"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void Preview3DWidget_BindsEditableControlsBeforeShown()
        {
            var widget = new Preview3DWidget();

            Assert.That(EditorHelpers.FindControlSafe<CheckBox>(widget, "utcShowByDefault"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<NumericUpDown>(widget, "backgroundColour"), Is.Not.Null);
        }

        [Test]
        [AvaloniaTest]
        public void EnvVarsWidget_BindsEditableControlsBeforeShown()
        {
            var widget = new EnvVarsWidget();

            Assert.That(EditorHelpers.FindControlSafe<DataGrid>(widget, "tableWidget"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "addButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "editButton"), Is.Not.Null);
            Assert.That(EditorHelpers.FindControlSafe<Button>(widget, "removeButton"), Is.Not.Null);
        }
    }
}
