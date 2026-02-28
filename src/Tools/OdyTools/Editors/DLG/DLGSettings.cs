using System;
using System.IO;
using OdyTools.Data;

namespace OdyTools.Editors.DLG
{
    /// <summary>
    /// Settings management for DLG editor.
    /// </summary>
    public class DLGSettings : IEditorInstallationSettings
    {
        private readonly GlobalSettings _settings;
        private const string SettingsName = "OdyToolDLG";

        public DLGSettings()
        {
            _settings = GlobalSettings.Instance;
        }

        /// <summary>
        /// Get a setting value with a default.
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
        /// </summary>
        public void Set(string key, object value)
        {
            string fullKey = $"{SettingsName}.{key}";
            _settings.SetValue(fullKey, value);
        }

        /// <summary>
        /// Get TSL widget preference.
        /// </summary>
        public string TslWidgetPreference(string defaultValue = "")
        {
            return Get("tsl_widget_preference", defaultValue);
        }

        /// <summary>
        /// Set TSL widget preference.
        /// </summary>
        public void SetTslWidgetPreference(string value)
        {
            Set("tsl_widget_preference", value);
        }

        /// <summary>
        /// Get show verbose hover hints setting.
        /// </summary>
        public bool ShowVerboseHoverHints(bool defaultValue = false)
        {
            return Get("show_verbose_hover_hints", defaultValue);
        }

        /// <summary>
        /// Set show verbose hover hints setting.
        /// </summary>
        public void SetShowVerboseHoverHints(bool value)
        {
            Set("show_verbose_hover_hints", value);
        }

        // --- Mode: Use installation vs manual paths ---

        /// <summary>True = use game installation when available; False = always use manual paths.</summary>
        public bool UseInstallation(bool defaultValue = true)
        {
            return Get("UseInstallation", defaultValue);
        }

        public void SetUseInstallation(bool value)
        {
            Set("UseInstallation", value);
        }

        /// <summary>When UseInstallation is true, which installation name to use (from GlobalSettings.Installations keys).</summary>
        public string SelectedInstallationName(string defaultValue = "")
        {
            return Get("SelectedInstallationName", defaultValue);
        }

        public void SetSelectedInstallationName(string value)
        {
            Set("SelectedInstallationName", value ?? "");
        }

        // --- DLG manual paths (used when UseInstallation is false or no installation is available) ---

        public string TlkPath(string defaultValue = "")
        {
            return Get("TlkPath", defaultValue);
        }

        public void SetTlkPath(string value)
        {
            Set("TlkPath", value ?? "");
        }

        public string FemaleTlkPath(string defaultValue = "")
        {
            return Get("FemaleTlkPath", defaultValue);
        }

        public void SetFemaleTlkPath(string value)
        {
            Set("FemaleTlkPath", value ?? "");
        }

        /// <summary>Optional directory containing 2DA files (e.g. dialoganimations.2da). Used when no installation is set.</summary>
        public string Override2DADirectory(string defaultValue = "")
        {
            return Get("Override2DADirectory", defaultValue);
        }

        public void SetOverride2DADirectory(string value)
        {
            Set("Override2DADirectory", value ?? "");
        }

        /// <summary>Optional per-2DA override path. Key is resname e.g. "dialoganimations.2da".</summary>
        public string Get2DAOverridePath(string resname, string defaultValue = "")
        {
            if (string.IsNullOrWhiteSpace(resname)) return defaultValue;
            string key = "2DA_" + resname.Replace(".", "_");
            return Get(key, defaultValue);
        }

        public void Set2DAOverridePath(string resname, string value)
        {
            if (string.IsNullOrWhiteSpace(resname)) return;
            string key = "2DA_" + resname.Replace(".", "_");
            Set(key, value ?? "");
        }

        /// <summary>Resolves the file path for a 2DA when using overrides: individual path if set, else directory + resname.</summary>
        public string Resolve2DAPath(string resname)
        {
            if (string.IsNullOrWhiteSpace(resname)) return null;
            string individual = Get2DAOverridePath(resname, "");
            if (!string.IsNullOrWhiteSpace(individual) && File.Exists(individual))
                return individual;
            string dir = Override2DADirectory("");
            if (string.IsNullOrWhiteSpace(dir)) return null;
            string combined = Path.Combine(dir, resname);
            return File.Exists(combined) ? combined : null;
        }

        /// <summary>Returns folders to use as custom 2DA search locations: 2DA directory plus directory of each per-file override path. Used with Installation search (CHITIN, OVERRIDE, then these).</summary>
        public System.Collections.Generic.List<string> GetCustom2DAFolders()
        {
            var list = new System.Collections.Generic.List<string>();
            string dir = Override2DADirectory("")?.Trim();
            if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                list.Add(dir);
            foreach (string resname in new[] { "dialoganimations.2da", "emotions.2da", "expressions.2da", "videoeffects.2da", "plot.2da" })
            {
                string path = Get2DAOverridePath(resname, "")?.Trim();
                if (string.IsNullOrEmpty(path) || !File.Exists(path)) continue;
                string folder = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(folder) && Directory.Exists(folder) && !list.Exists(f => string.Equals(f, folder, StringComparison.OrdinalIgnoreCase)))
                    list.Add(folder);
            }
            return list;
        }
    }
}

