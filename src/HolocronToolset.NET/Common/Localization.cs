using System;
using System.Collections.Generic;
using System.Globalization;
using CSharpKOTOR.Common;

namespace HolocronToolset.NET.Common
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/common/localization.py:13
    // Original: class ToolsetLanguage(IntEnum):
    public enum ToolsetLanguage
    {
        English = 0,
        French = 1,
        German = 2,
        Italian = 3,
        Spanish = 4,
        Polish = 5
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/common/localization.py:61-1280
    // Original: Translation dictionaries and translate function
    public static class Localization
    {
        private static ToolsetLanguage _currentLanguage = ToolsetLanguage.English;
        private static readonly Dictionary<ToolsetLanguage, Dictionary<string, string>> Translations =
            new Dictionary<ToolsetLanguage, Dictionary<string, string>>();

        static Localization()
        {
            InitializeTranslations();
        }

        private static void InitializeTranslations()
        {
            // Initialize English translations
            var english = new Dictionary<string, string>
            {
                ["Holocron Toolset"] = "Holocron Toolset",
                ["File"] = "File",
                ["Edit"] = "Edit",
                ["Tools"] = "Tools",
                ["Theme"] = "Theme",
                ["Language"] = "Language",
                ["Help"] = "Help",
                ["New"] = "New",
                ["Open"] = "Open",
                ["Recent Files"] = "Recent Files",
                ["Settings"] = "Settings",
                ["Exit"] = "Exit",
                ["Module Designer"] = "Module Designer",
                ["Indoor Map Builder"] = "Indoor Map Builder",
                ["KotorDiff"] = "KotorDiff",
                ["File Search"] = "File Search",
                ["Clone Module"] = "Clone Module",
                ["About"] = "About",
                ["Check For Updates"] = "Check For Updates",
                ["Core"] = "Core",
                ["Saves"] = "Saves",
                ["Modules"] = "Modules",
                ["Override"] = "Override",
                ["Textures"] = "Textures",
                ["Open Selected"] = "Open Selected",
                ["Extract Selected"] = "Extract Selected",
                ["OK"] = "OK",
                ["Cancel"] = "Cancel",
                ["Yes"] = "Yes",
                ["No"] = "No",
                ["Close"] = "Close",
                ["Save"] = "Save",
                ["Loading..."] = "Loading...",
                ["Error"] = "Error",
                ["Warning"] = "Warning",
                ["Information"] = "Information"
            };
            Translations[ToolsetLanguage.English] = english;

            // Other languages will be added when translations are available
            Translations[ToolsetLanguage.French] = new Dictionary<string, string>(english);
            Translations[ToolsetLanguage.German] = new Dictionary<string, string>(english);
            Translations[ToolsetLanguage.Italian] = new Dictionary<string, string>(english);
            Translations[ToolsetLanguage.Spanish] = new Dictionary<string, string>(english);
            Translations[ToolsetLanguage.Polish] = new Dictionary<string, string>(english);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/common/localization.py
        // Original: def translate(key: str, language: ToolsetLanguage | None = None) -> str:
        public static string Translate(string key, ToolsetLanguage? language = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }

            ToolsetLanguage lang = language ?? _currentLanguage;
            if (Translations.TryGetValue(lang, out var langDict) && langDict.TryGetValue(key, out string value))
            {
                return value;
            }

            // Fallback to English if translation not found
            if (lang != ToolsetLanguage.English && Translations.TryGetValue(ToolsetLanguage.English, out var englishDict) && englishDict.TryGetValue(key, out string englishValue))
            {
                return englishValue;
            }

            // Return key if no translation found
            return key;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/common/localization.py
        // Original: def trf(format_string: str, **kwargs) -> str:
        public static string TranslateFormat(string formatString, params object[] args)
        {
            string translated = Translate(formatString);
            try
            {
                return string.Format(translated, args);
            }
            catch
            {
                return translated;
            }
        }

        public static ToolsetLanguage CurrentLanguage
        {
            get => _currentLanguage;
            set => _currentLanguage = value;
        }

        public static ToolsetLanguage FromPykotorLanguage(Language language)
        {
            switch (language)
            {
                case Language.English: return ToolsetLanguage.English;
                case Language.French: return ToolsetLanguage.French;
                case Language.German: return ToolsetLanguage.German;
                case Language.Italian: return ToolsetLanguage.Italian;
                case Language.Spanish: return ToolsetLanguage.Spanish;
                case Language.Polish: return ToolsetLanguage.Polish;
                default: return ToolsetLanguage.English;
            }
        }
    }
}
