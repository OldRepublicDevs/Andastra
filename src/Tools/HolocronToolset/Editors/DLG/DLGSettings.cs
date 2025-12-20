using System;
using HolocronToolset.Data;

namespace HolocronToolset.Editors.DLG
{
    /// <summary>
    /// Settings management for DLG editor.
    /// Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/settings.py
    /// </summary>
    public class DLGSettings
    {
        private readonly GlobalSettings _settings;
        private const string SettingsName = "DLGEditor";

        public DLGSettings()
        {
            _settings = GlobalSettings.Instance;
        }

        /// <summary>
        /// Get a setting value with a default.
        /// Matching PyKotor: def get(self, key: str, default: Any) -> Any
        /// </summary>
        public T Get<T>(string key, T defaultValue)
        {
            try
            {
                string fullKey = $"{SettingsName}.{key}";
                object value = _settings.GetValue(fullKey, defaultValue);
                if (value is string strValue)
                {
                    // Handle boolean string conversion
                    if (typeof(T) == typeof(bool))
                    {
                        if (strValue == "true")
                        {
                            return (T)(object)true;
                        }
                        if (strValue == "false")
                        {
                            return (T)(object)false;
                        }
                    }
                }
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Set a setting value.
        /// Matching PyKotor: def set(self, key: str, value: Any)
        /// </summary>
        public void Set(string key, object value)
        {
            string fullKey = $"{SettingsName}.{key}";
            _settings.SetValue(fullKey, value);
        }

        /// <summary>
        /// Get TSL widget preference.
        /// Matching PyKotor: def tsl_widget_preference(self, default: str) -> str
        /// </summary>
        public string TslWidgetPreference(string defaultValue = "")
        {
            return Get("tsl_widget_preference", defaultValue);
        }

        /// <summary>
        /// Set TSL widget preference.
        /// Matching PyKotor: def set_tsl_widget_preference(self, value: str)
        /// </summary>
        public void SetTslWidgetPreference(string value)
        {
            Set("tsl_widget_preference", value);
        }

        /// <summary>
        /// Get show verbose hover hints setting.
        /// Matching PyKotor: def show_verbose_hover_hints(self, default: bool) -> bool
        /// </summary>
        public bool ShowVerboseHoverHints(bool defaultValue = false)
        {
            return Get("show_verbose_hover_hints", defaultValue);
        }

        /// <summary>
        /// Set show verbose hover hints setting.
        /// Matching PyKotor: def set_show_verbose_hover_hints(self, value: bool)
        /// </summary>
        public void SetShowVerboseHoverHints(bool value)
        {
            Set("show_verbose_hover_hints", value);
        }
    }
}

