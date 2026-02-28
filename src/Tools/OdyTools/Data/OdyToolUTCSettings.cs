using System;

namespace OdyTools.Data
{
    public class OdyToolUTCSettings : Settings, IEditorInstallationSettings
    {
        public bool SaveUnusedFields
        {
            get => GetValue("saveUnusedFields", true);
            set => SetValue("saveUnusedFields", value);
        }

        public bool AlwaysSaveK2Fields
        {
            get => GetValue("alwaysSaveK2Fields", false);
            set => SetValue("alwaysSaveK2Fields", value);
        }

        public bool UseInstallation(bool defaultValue = true)
        {
            return GetValue("UseInstallation", defaultValue);
        }

        public void SetUseInstallation(bool value)
        {
            SetValue("UseInstallation", value);
        }

        public string SelectedInstallationName(string defaultValue = "")
        {
            return GetValue("SelectedInstallationName", defaultValue) ?? defaultValue;
        }

        public void SetSelectedInstallationName(string value)
        {
            SetValue("SelectedInstallationName", value ?? "");
        }

        public OdyToolUTCSettings() : base("OdyToolUTC")
        {
        }
    }
}

