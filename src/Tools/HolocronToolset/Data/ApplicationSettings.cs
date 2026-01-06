using System;
using System.Collections.Generic;
using HolocronToolset.Data;

namespace HolocronToolset.Data
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/application.py:307
    // Original: class ApplicationSettings(Settings):
    /// <summary>
    /// Application-level settings for the Holocron Toolset.
    /// Manages environment variables, font settings, and application attributes.
    /// Note: Some Qt-specific attributes (AA_*) are maintained for compatibility but don't apply to Avalonia.
    /// </summary>
    public class ApplicationSettings : Settings
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/application.py:311-313
        // Original: app_env_variables: SettingsProperty[dict[str, str]] = Settings.addSetting("EnvironmentVariables", {...})
        /// <summary>
        /// Environment variables to be set before application initialization.
        /// Default includes Windows-specific Qt multimedia settings if on Windows.
        /// </summary>
        public Dictionary<string, string> AppEnvVariables
        {
            get
            {
                var defaultVars = new Dictionary<string, string>();
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    string existingValue = Environment.GetEnvironmentVariable("QT_MULTIMEDIA_PREFERRED_PLUGINS");
                    defaultVars["QT_MULTIMEDIA_PREFERRED_PLUGINS"] = existingValue ?? "windowsmediafoundation";
                }
                return GetValue("EnvironmentVariables", defaultVars);
            }
            set => SetValue("EnvironmentVariables", value);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/application.py:315-322
        // Original: MISC_SETTINGS: ClassVar[dict[str, MiscSetting]] = {...}
        /// <summary>
        /// Miscellaneous settings that can be changed without restarting.
        /// Note: These are Qt-specific and don't directly apply to Avalonia, but are maintained for compatibility.
        /// </summary>
        public static Dictionary<string, MiscSetting> MiscSettings { get; } = new Dictionary<string, MiscSetting>
        {
            // These Qt-specific settings don't have direct Avalonia equivalents, but we maintain the structure
            // for compatibility and potential future use
        };

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/application.py:324-342
        // Original: REQUIRES_RESTART: ClassVar[dict[str, Qt.ApplicationAttribute | None]] = {...}
        /// <summary>
        /// Application attributes that require a restart to take effect.
        /// Note: These are Qt-specific (AA_*) and don't apply to Avalonia, but are maintained for compatibility.
        /// </summary>
        public static Dictionary<string, object> RequiresRestart { get; } = new Dictionary<string, object>
        {
            // Qt Application Attributes that require restart
            // These don't apply to Avalonia but are maintained for structure compatibility
        };

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/application.py:308-309
        // Original: def __init__(self): super().__init__("Application")
        public ApplicationSettings() : base("Application")
        {
        }
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/application.py:295-305
    // Original: class MiscSetting:
    /// <summary>
    /// Represents a miscellaneous setting with getter, setter, and type information.
    /// Used for settings that can be changed without restarting the application.
    /// </summary>
    public class MiscSetting
    {
        public Func<object> Getter { get; }
        public Action<object> Setter { get; }
        public Type SettingType { get; }

        public MiscSetting(Func<object> getter, Action<object> setter, Type settingType)
        {
            Getter = getter;
            Setter = setter;
            SettingType = settingType;
        }
    }
}

