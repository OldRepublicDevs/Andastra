using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BioWare.Extract;
using OdyTools.Data;
using OdyTools.Editors;
using OdyTools.Editors.DLG;

namespace OdyTools.Dialogs
{
    public partial class DLGSettingsDialog : EditorInstallationSettingsDialogBase
    {
        private TextBox _tlkPathEdit;
        private TextBox _femaleTlkPathEdit;
        private TextBox _override2DADirEdit;
        private TextBox _pathDialogAnimsEdit;
        private TextBox _pathEmotionsEdit;
        private TextBox _pathExpressionsEdit;
        private TextBox _pathVideoEffectsEdit;
        private TextBox _pathPlotEdit;
        private Button _tlkBrowseBtn;
        private Button _femaleTlkBrowseBtn;
        private Button _twoDADirBrowseBtn;

        private DLGSettings _settings;

        public DLGSettingsDialog()
        {
            _settings = new DLGSettings();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            var installationCombo = EditorHelpers.FindControlSafe<ComboBox>(this, "installationCombo");
            _tlkPathEdit = EditorHelpers.FindControlSafe<TextBox>(this, "tlkPathEdit");
            _femaleTlkPathEdit = EditorHelpers.FindControlSafe<TextBox>(this, "femaleTlkPathEdit");
            _override2DADirEdit = EditorHelpers.FindControlSafe<TextBox>(this, "override2DADirEdit");
            _pathDialogAnimsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "pathDialogAnimsEdit");
            _pathEmotionsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "pathEmotionsEdit");
            _pathExpressionsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "pathExpressionsEdit");
            _pathVideoEffectsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "pathVideoEffectsEdit");
            _pathPlotEdit = EditorHelpers.FindControlSafe<TextBox>(this, "pathPlotEdit");
            var okButton = EditorHelpers.FindControlSafe<Button>(this, "okButton");
            var cancelButton = EditorHelpers.FindControlSafe<Button>(this, "cancelButton");
            _tlkBrowseBtn = EditorHelpers.FindControlSafe<Button>(this, "tlkBrowseBtn");
            _femaleTlkBrowseBtn = EditorHelpers.FindControlSafe<Button>(this, "femaleTlkBrowseBtn");
            _twoDADirBrowseBtn = EditorHelpers.FindControlSafe<Button>(this, "twoDADirBrowseBtn");

            InitializeInstallationSection(installationCombo, okButton, cancelButton);
            if (_tlkBrowseBtn != null) _tlkBrowseBtn.Click += (s, e) => _ = BrowseForFileAsync(_tlkPathEdit, "dialog.tlk");
            if (_femaleTlkBrowseBtn != null) _femaleTlkBrowseBtn.Click += (s, e) => _ = BrowseForFileAsync(_femaleTlkPathEdit, "dialogf.tlk");
            if (_twoDADirBrowseBtn != null) _twoDADirBrowseBtn.Click += (s, e) => _ = BrowseForFolderAsync(_override2DADirEdit, "Select folder containing 2DA files");

            LoadValues();
            UpdateManualFieldsPlaceholders();
        }

        protected override void OnInstallationSelectionChanged()
        {
            UpdateManualFieldsPlaceholders();
        }

        private void UpdateManualFieldsPlaceholders()
        {
            string tlkWatermark = "Full path to dialog.tlk";
            string femaleTlkWatermark = "Optional";
            string twoDAWatermark = "Folder containing dialoganimations.2da, emotions.2da, etc.";
            int idx = InstallationCombo?.SelectedIndex ?? 0;
            if (idx >= 1 && idx <= InstallationNames.Count)
            {
                string name = InstallationNames[idx - 1];
                string path = GetInstallationPath(name);
                if (!string.IsNullOrEmpty(path))
                {
                    tlkWatermark = Path.Combine(path, "dialog.tlk");
                    femaleTlkWatermark = Path.Combine(path, "dialogf.tlk");
                    twoDAWatermark = Installation.GetOverridePath(path);
                }
            }
            if (_tlkPathEdit != null) _tlkPathEdit.Watermark = tlkWatermark;
            if (_femaleTlkPathEdit != null) _femaleTlkPathEdit.Watermark = femaleTlkWatermark;
            if (_override2DADirEdit != null) _override2DADirEdit.Watermark = twoDAWatermark;
        }

        private void LoadValues()
        {
            bool useInstallation = _settings.UseInstallation(true);
            string selectedName = _settings.SelectedInstallationName("");
            if (!useInstallation)
            {
                InstallationCombo.SelectedIndex = 0; // "(None)"
            }
            else
            {
                int idx = InstallationNames.IndexOf(selectedName);
                if (idx >= 0)
                    InstallationCombo.SelectedIndex = idx + 1; // +1 because index 0 is NoneItem
                else if (InstallationNames.Count > 0)
                    InstallationCombo.SelectedIndex = 1;
                else
                    InstallationCombo.SelectedIndex = 0;
            }

            if (_tlkPathEdit != null) _tlkPathEdit.Text = _settings.TlkPath("");
            if (_femaleTlkPathEdit != null) _femaleTlkPathEdit.Text = _settings.FemaleTlkPath("");
            if (_override2DADirEdit != null) _override2DADirEdit.Text = _settings.Override2DADirectory("");
            if (_pathDialogAnimsEdit != null) _pathDialogAnimsEdit.Text = _settings.Get2DAOverridePath("dialoganimations.2da", "");
            if (_pathEmotionsEdit != null) _pathEmotionsEdit.Text = _settings.Get2DAOverridePath("emotions.2da", "");
            if (_pathExpressionsEdit != null) _pathExpressionsEdit.Text = _settings.Get2DAOverridePath("expressions.2da", "");
            if (_pathVideoEffectsEdit != null) _pathVideoEffectsEdit.Text = _settings.Get2DAOverridePath("videoeffects.2da", "");
            if (_pathPlotEdit != null) _pathPlotEdit.Text = _settings.Get2DAOverridePath("plot.2da", "");
            UpdateManualFieldsPlaceholders();
        }

        protected override void SaveValues()
        {
            int idx = InstallationCombo.SelectedIndex;
            bool isSpecialEntry = (idx == AutoDetectIndex || idx == AddNewInstallationIndex);
            if (isSpecialEntry)
                idx = 0;
            bool useInstallation = idx != 0;
            _settings.SetUseInstallation(useInstallation);

            if (useInstallation && idx >= 1 && idx <= InstallationNames.Count)
            {
                _settings.SetSelectedInstallationName(InstallationNames[idx - 1]);
            }
            else
            {
                _settings.SetSelectedInstallationName("");
            }

            if (_tlkPathEdit != null) _settings.SetTlkPath(_tlkPathEdit.Text?.Trim() ?? "");
            if (_femaleTlkPathEdit != null) _settings.SetFemaleTlkPath(_femaleTlkPathEdit.Text?.Trim() ?? "");
            if (_override2DADirEdit != null) _settings.SetOverride2DADirectory(_override2DADirEdit.Text?.Trim() ?? "");
            if (_pathDialogAnimsEdit != null) _settings.Set2DAOverridePath("dialoganimations.2da", _pathDialogAnimsEdit.Text?.Trim() ?? "");
            if (_pathEmotionsEdit != null) _settings.Set2DAOverridePath("emotions.2da", _pathEmotionsEdit.Text?.Trim() ?? "");
            if (_pathExpressionsEdit != null) _settings.Set2DAOverridePath("expressions.2da", _pathExpressionsEdit.Text?.Trim() ?? "");
            if (_pathVideoEffectsEdit != null) _settings.Set2DAOverridePath("videoeffects.2da", _pathVideoEffectsEdit.Text?.Trim() ?? "");
            if (_pathPlotEdit != null) _settings.Set2DAOverridePath("plot.2da", _pathPlotEdit.Text?.Trim() ?? "");
        }
    }
}
