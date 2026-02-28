using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OdyTools.Data;

namespace OdyTools.Dialogs
{
    public partial class UTCSettingsDialog : EditorInstallationSettingsDialogBase
    {
        private CheckBox _saveUnusedFieldsCheck;
        private CheckBox _alwaysSaveK2FieldsCheck;

        private OdyToolUTCSettings _settings;

        public UTCSettingsDialog()
        {
            _settings = new OdyToolUTCSettings();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            var installationCombo = this.FindControl<ComboBox>("installationCombo");
            _saveUnusedFieldsCheck = this.FindControl<CheckBox>("saveUnusedFieldsCheck");
            _alwaysSaveK2FieldsCheck = this.FindControl<CheckBox>("alwaysSaveK2FieldsCheck");
            var okButton = this.FindControl<Button>("okButton");
            var cancelButton = this.FindControl<Button>("cancelButton");

            InitializeInstallationSection(installationCombo, okButton, cancelButton);
            LoadValues();
        }

        private void LoadValues()
        {
            bool useInstallation = _settings.UseInstallation(true);
            string selectedName = _settings.SelectedInstallationName("");
            if (!useInstallation)
            {
                InstallationCombo.SelectedIndex = 0;
            }
            else
            {
                int idx = InstallationNames.IndexOf(selectedName);
                if (idx >= 0)
                    InstallationCombo.SelectedIndex = idx + 1;
                else if (InstallationNames.Count > 0)
                    InstallationCombo.SelectedIndex = 1;
                else
                    InstallationCombo.SelectedIndex = 0;
            }

            if (_saveUnusedFieldsCheck != null) _saveUnusedFieldsCheck.IsChecked = _settings.SaveUnusedFields;
            if (_alwaysSaveK2FieldsCheck != null) _alwaysSaveK2FieldsCheck.IsChecked = _settings.AlwaysSaveK2Fields;
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

            if (_saveUnusedFieldsCheck != null) _settings.SaveUnusedFields = _saveUnusedFieldsCheck.IsChecked == true;
            if (_alwaysSaveK2FieldsCheck != null) _settings.AlwaysSaveK2Fields = _alwaysSaveK2FieldsCheck.IsChecked == true;
        }
    }
}
