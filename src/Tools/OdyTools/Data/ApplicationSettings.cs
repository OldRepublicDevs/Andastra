using System;
using System.Collections.Generic;
using OdyTools.Data;

namespace OdyTools.Data
{
    /// <summary>
    /// Application-level settings for the OdyTools.
    /// Manages environment variables, font settings, and application attributes.
    /// Note: Some Qt-specific attributes (AA_*) are maintained for compatibility but don't apply to Avalonia.
    /// </summary>
    public class ApplicationSettings : Settings
    {
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

        /// <summary>
        /// Miscellaneous settings that can be changed without restarting.
        /// Note: These are Qt-specific and don't directly apply to Avalonia, but are maintained for compatibility.
        /// </summary>
        public static Dictionary<string, MiscSetting> MiscSettings { get; } = new Dictionary<string, MiscSetting>
        {
            // These Qt-specific settings don't have direct Avalonia equivalents, but we maintain the structure
            // for compatibility and potential future use
        };

        /// <summary>
        /// Application attributes that require a restart to take effect.
        /// Note: These are Qt-specific (AA_*) and don't apply to Avalonia, but are maintained for compatibility.
        /// </summary>
        public static Dictionary<string, object> RequiresRestart { get; } = new Dictionary<string, object>
        {
            // Qt Application Attributes that require restart
            // These don't apply to Avalonia but are maintained for structure compatibility
        };

        public ApplicationSettings() : base("Application")
        {
        }
    }

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

