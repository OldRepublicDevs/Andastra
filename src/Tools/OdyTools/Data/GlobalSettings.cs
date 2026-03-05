using BioWare.Common;
using BioWare.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using OdyTools.Data;

namespace OdyTools.Data
{
    public class GlobalSettings : Settings
    {
        public const bool ManagedAutosaveEnabled = true;
        public const int ManagedAutosaveIntervalMinutes = 3;

        private static readonly string[] LegacyAutosaveSettingKeys =
        {
            "AutosaveEnabled",
            "AutosaveIntervalMinutes",
            "OdyToolDLG.autosave_enabled",
            "OdyToolDLG.autosave_interval_minutes"
        };

        private static GlobalSettings _instance;
        private static readonly object _lock = new object();

        public static GlobalSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new GlobalSettings();
                        }
                    }
                }
                return _instance;
            }
        }
        public SettingsProperty<bool> GffSpecializedEditors { get; } = new SettingsProperty<bool>("GffSpecializedEditors", true);

        public bool UseBetaChannel { get; set; } = false;
        public string SelectedTheme { get; set; } = "Light";
        public string SelectedStyle { get; set; } = "";
        public int SelectedLanguage { get; set; } = 0; // 0-5, default uses system language when no preference saved
        public bool JoinRIMsTogether { get; set; } = true;
        public string ExtractPath { get; set; } = "";
        public string NssCompilerPath { get; set; } = "";
        public string NcsDecompilerPath { get; set; } = "";
        public List<string> RecentFiles { get; set; } = new List<string>();
        private bool _firstTime = true;

        public bool AutosaveEnabled
        {
            get => ManagedAutosaveEnabled;
            set => SetValue("AutosaveEnabled", ManagedAutosaveEnabled);
        }

        public int AutosaveIntervalMinutes
        {
            get => ManagedAutosaveIntervalMinutes;
            set => SetValue("AutosaveIntervalMinutes", ManagedAutosaveIntervalMinutes);
        }

        public bool BackupsEnabled
        {
            get => GetValue("BackupsEnabled", true);
            set => SetValue("BackupsEnabled", value);
        }

        public int MaxBackupCount
        {
            get => Math.Max(1, GetValue("MaxBackupCount", 5));
            set => SetValue("MaxBackupCount", Math.Max(1, value));
        }

        public bool CrashRecoveryEnabled
        {
            get => GetValue("CrashRecoveryEnabled", true);
            set => SetValue("CrashRecoveryEnabled", value);
        }

        public int CrashRecoveryIntervalSeconds
        {
            get => Math.Max(5, GetValue("CrashRecoveryIntervalSeconds", 30));
            set => SetValue("CrashRecoveryIntervalSeconds", Math.Max(5, value));
        }

        public Dictionary<string, string> AppEnvVariables
        {
            get => GetValue("EnvironmentVariables", new Dictionary<string, string>());
            set => SetValue("EnvironmentVariables", value);
        }

        public GlobalSettings() : base("Global")
        {
            CleanupLegacyAutosaveSettings();

            // Load settings from base class
            GffSpecializedEditors.GetValue(this);
            ExtractPath = GetValue("ExtractPath", "");
            NssCompilerPath = GetValue("NssCompilerPath", "");
            NcsDecompilerPath = GetValue("NcsDecompilerPath", "");
            RecentFiles = GetValue("RecentFiles", new List<string>());
            UseBetaChannel = GetValue("UseBetaChannel", false);
            SelectedTheme = GetValue("SelectedTheme", "Light");
            SelectedStyle = GetValue("SelectedStyle", "");
            SelectedLanguage = GetValue("SelectedLanguage", OdyTools.Common.Localization.GetSystemLanguageAsInt());
            JoinRIMsTogether = GetValue("JoinRIMsTogether", true);
            _firstTime = GetValue("FirstTime", true);
        }

        private void CleanupLegacyAutosaveSettings()
        {
            RemoveValues(LegacyAutosaveSettingKeys);
        }

        public bool GetGffSpecializedEditors()
        {
            return GffSpecializedEditors.GetValue(this);
        }

        public void SetGffSpecializedEditors(bool value)
        {
            GffSpecializedEditors.SetValue(this, value);
        }

        public Dictionary<string, Dictionary<string, object>> Installations()
        {
            var installations = GetValue("Installations", new Dictionary<string, Dictionary<string, object>>());

            // When no installations exist (first run or empty settings), auto-detect from default paths
            if (installations == null || installations.Count == 0)
            {
                installations = DetectAndPrefillInstallations();
                if (installations != null && installations.Count > 0)
                {
                    SetValue("Installations", installations);
                    _firstTime = false;
                    SetValue("FirstTime", false);
                }
            }

            if (_firstTime)
            {
                _firstTime = false;
                SetValue("FirstTime", false);
            }

            return installations ?? new Dictionary<string, Dictionary<string, object>>();
        }

        /// <summary>
        /// Uses FindKotorPathsFromDefault to detect existing KOTOR installations and returns
        /// them in the installations dictionary format. Returns empty dict if none found.
        /// </summary>
        private Dictionary<string, Dictionary<string, object>> DetectAndPrefillInstallations()
        {
            var result = new Dictionary<string, Dictionary<string, object>>();
            try
            {
                var foundPaths = PathTools.FindKotorPathsFromDefault();
                if (foundPaths == null) return result;

                int k1Index = 0;
                if (foundPaths.TryGetValue(BioWareGame.K1, out var k1Paths) && k1Paths != null)
                {
                    foreach (var path in k1Paths)
                    {
                        string resolvedPath = path?.GetResolvedPath();
                        if (string.IsNullOrEmpty(resolvedPath) || !Directory.Exists(resolvedPath)) continue;

                        string name = k1Index == 0 ? "KotOR" : "KotOR " + (k1Index + 1);
                        result[name] = new Dictionary<string, object>
                        {
                            { "name", name },
                            { "path", resolvedPath },
                            { "tsl", false }
                        };
                        k1Index++;
                    }
                }

                int k2Index = 0;
                if (foundPaths.TryGetValue(BioWareGame.K2, out var k2Paths) && k2Paths != null)
                {
                    foreach (var path in k2Paths)
                    {
                        string resolvedPath = path?.GetResolvedPath();
                        if (string.IsNullOrEmpty(resolvedPath) || !Directory.Exists(resolvedPath)) continue;

                        string name = k2Index == 0 ? "KotOR II: TSL" : "KotOR II: TSL " + (k2Index + 1);
                        result[name] = new Dictionary<string, object>
                        {
                            { "name", name },
                            { "path", resolvedPath },
                            { "tsl", true }
                        };
                        k2Index++;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DetectAndPrefillInstallations failed: {ex.Message}");
            }

            return result;
        }

        public void SetInstallations(Dictionary<string, Dictionary<string, object>> installations)
        {
            SetValue("Installations", installations);
        }

        /// <summary>
        /// Calls PathTools.FindKotorPathsFromDefault(), builds installation entries for any found paths,
        /// and merges them into the current installations (existing names are not overwritten).
        /// </summary>
        /// <returns>Number of newly added installations.</returns>
        public int MergeDetectedInstallationsFromDefault()
        {
            var current = Installations() ?? new Dictionary<string, Dictionary<string, object>>();
            if (current == null)
                current = new Dictionary<string, Dictionary<string, object>>();
            var detected = BuildInstallationsFromFoundPaths(PathTools.FindKotorPathsFromDefault());
            int added = 0;
            foreach (var kv in detected)
            {
                if (!current.ContainsKey(kv.Key))
                {
                    current[kv.Key] = kv.Value;
                    added++;
                }
            }
            SetInstallations(current);
            return added;
        }

        private static Dictionary<string, Dictionary<string, object>> BuildInstallationsFromFoundPaths(
            Dictionary<BioWareGame, List<CaseAwarePath>> foundPaths)
        {
            var result = new Dictionary<string, Dictionary<string, object>>();
            if (foundPaths == null) return result;
            int k1Index = 0;
            if (foundPaths.TryGetValue(BioWareGame.K1, out var k1Paths) && k1Paths != null)
            {
                foreach (var path in k1Paths)
                {
                    string resolvedPath = path?.GetResolvedPath();
                    if (string.IsNullOrEmpty(resolvedPath) || !Directory.Exists(resolvedPath)) continue;
                    string name = k1Index == 0 ? "KotOR" : "KotOR " + (k1Index + 1);
                    result[name] = new Dictionary<string, object>
                    {
                        { "name", name },
                        { "path", resolvedPath },
                        { "tsl", false }
                    };
                    k1Index++;
                }
            }
            int k2Index = 0;
            if (foundPaths.TryGetValue(BioWareGame.K2, out var k2Paths) && k2Paths != null)
            {
                foreach (var path in k2Paths)
                {
                    string resolvedPath = path?.GetResolvedPath();
                    if (string.IsNullOrEmpty(resolvedPath) || !Directory.Exists(resolvedPath)) continue;
                    string name = k2Index == 0 ? "KotOR II: TSL" : "KotOR II: TSL " + (k2Index + 1);
                    result[name] = new Dictionary<string, object>
                    {
                        { "name", name },
                        { "path", resolvedPath },
                        { "tsl", true }
                    };
                    k2Index++;
                }
            }
            return result;
        }

        public bool ShowPreviewUTC
        {
            get => GetValue("showPreviewUTC", false);
            set => SetValue("showPreviewUTC", value);
        }

        public bool ShowPreviewUTD
        {
            get => GetValue("showPreviewUTD", false);
            set => SetValue("showPreviewUTD", value);
        }

        public bool ShowPreviewUTP
        {
            get => GetValue("showPreviewUTP", false);
            set => SetValue("showPreviewUTP", value);
        }

        public string GlobalFont
        {
            get => GetValue("GlobalFont", "");
            set => SetValue("GlobalFont", value);
        }
    }
}
