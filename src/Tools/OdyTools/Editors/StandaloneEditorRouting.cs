using System;
using System.Linq;
using System.Reflection;
using System.IO;
using BioWare.Common;

namespace OdyTools.Editors
{
    internal static class StandaloneEditorRouting
    {
        private static readonly string[] PlainTextFallbackExtensions = { "cfg", "log", "2da_bak" };
        private static readonly EditorLaunchInfo[] EditorLaunchInfos =
        {
            new EditorLaunchInfo("2da", "2DA Table"),
            new EditorLaunchInfo("are", "Area"),
            new EditorLaunchInfo("bwm", "Walkmesh"),
            new EditorLaunchInfo("dlg", "Dialog"),
            new EditorLaunchInfo("erf", "ERF/MOD/RIM/SAV"),
            new EditorLaunchInfo("fac", "Faction"),
            new EditorLaunchInfo("gff", "GFF"),
            new EditorLaunchInfo("git", "GIT"),
            new EditorLaunchInfo("gui", "GUI"),
            new EditorLaunchInfo("ifo", "Module Info"),
            new EditorLaunchInfo("jrl", "Journal"),
            new EditorLaunchInfo("lip", "LIP Sync"),
            new EditorLaunchInfo("ltr", "LTR"),
            new EditorLaunchInfo("lyt", "Layout"),
            new EditorLaunchInfo("mdl", "Model"),
            new EditorLaunchInfo("nss", "Script"),
            new EditorLaunchInfo("pth", "Path"),
            new EditorLaunchInfo("savegame", "Save Game"),
            new EditorLaunchInfo("ssf", "Sound Set"),
            new EditorLaunchInfo("tlk", "Talk Table"),
            new EditorLaunchInfo("tpc", "Texture"),
            new EditorLaunchInfo("txt", "Text"),
            new EditorLaunchInfo("utc", "Creature"),
            new EditorLaunchInfo("utd", "Door"),
            new EditorLaunchInfo("ute", "Encounter"),
            new EditorLaunchInfo("uti", "Item"),
            new EditorLaunchInfo("utm", "Store"),
            new EditorLaunchInfo("utp", "Placeable"),
            new EditorLaunchInfo("uts", "Sound"),
            new EditorLaunchInfo("utt", "Trigger"),
            new EditorLaunchInfo("utw", "Waypoint"),
            new EditorLaunchInfo("wav", "Audio"),
            new EditorLaunchInfo("module-designer", "Module Designer"),
            new EditorLaunchInfo("indoor-builder", "Indoor Builder")
        };

        public static EditorLaunchInfo[] KnownEditors()
        {
            return (EditorLaunchInfo[])EditorLaunchInfos.Clone();
        }

        public static string NormalizeEditorKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            var normalized = key.Trim().ToLowerInvariant();
            if (normalized.EndsWith(".exe"))
            {
                normalized = normalized.Substring(0, normalized.Length - 4);
            }
            if (normalized.StartsWith("odytools."))
            {
                normalized = normalized.Substring("odytools.".Length);
            }
            if (normalized.StartsWith("odytool"))
            {
                normalized = normalized.Substring("odytool".Length);
            }
            if (normalized.EndsWith(".standalone"))
            {
                normalized = normalized.Substring(0, normalized.Length - ".standalone".Length);
            }
            if (normalized.EndsWith("-editor"))
            {
                normalized = normalized.Substring(0, normalized.Length - "-editor".Length);
            }
            if (normalized.EndsWith("_editor"))
            {
                normalized = normalized.Substring(0, normalized.Length - "_editor".Length);
            }
            if (normalized.EndsWith("editor") && normalized.Length > "editor".Length)
            {
                normalized = normalized.Substring(0, normalized.Length - "editor".Length);
            }

            normalized = normalized.Trim().Replace("_", "-").Replace(" ", "-");

            if (normalized == "twoda") return "2da";
            if (normalized == "save" || normalized == "sav" || normalized == "save-game" || normalized == "savegame") return "savegame";
            if (normalized == "module-designer" || normalized == "moduledesigner") return "module-designer";
            if (normalized == "indoor-builder" || normalized == "indoorbuilder") return "indoor-builder";
            if (normalized == "2da-table") return "2da";
            if (normalized == "area") return "are";
            if (normalized == "walkmesh" || normalized == "walkmesh-painter") return "bwm";
            if (normalized == "dialog" || normalized == "dialogue") return "dlg";
            if (normalized == "erf-mod-rim-sav" || normalized == "erf-mod-rim-sav-bif-hak") return "erf";
            if (normalized == "faction") return "fac";
            if (normalized == "module-info") return "ifo";
            if (normalized == "journal") return "jrl";
            if (normalized == "lip-sync") return "lip";
            if (normalized == "layout") return "lyt";
            if (normalized == "model" || normalized == "model-viewer") return "mdl";
            if (normalized == "script") return "nss";
            if (normalized == "path") return "pth";
            if (normalized == "sound-set" || normalized == "soundset") return "ssf";
            if (normalized == "talk-table" || normalized == "talktable") return "tlk";
            if (normalized == "texture" || normalized == "texture-viewer") return "tpc";
            if (normalized == "text") return "txt";
            if (normalized == "creature") return "utc";
            if (normalized == "door") return "utd";
            if (normalized == "encounter") return "ute";
            if (normalized == "item") return "uti";
            if (normalized == "store") return "utm";
            if (normalized == "placeable") return "utp";
            if (normalized == "sound") return "uts";
            if (normalized == "trigger") return "utt";
            if (normalized == "waypoint") return "utw";
            if (normalized == "audio" || normalized == "audio-player") return "wav";

            return normalized;
        }

        public static string GetEditorKeyFromPath(string path)
        {
            if (IsSaveGameFolder(path) || IsSaveGameSavPath(path))
            {
                return "savegame";
            }

            var restype = GetResourceTypeFromPath(path);
            if (restype != null && !restype.IsInvalid)
            {
                return GetEditorKey(restype);
            }

            if (IsExtension(path, "bwm"))
            {
                return "bwm";
            }

            return IsPlainTextFallbackPath(path) ? "txt" : null;
        }

        private static bool IsSaveGameFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
            {
                return false;
            }

            return Directory.EnumerateFiles(path)
                .Any(file => string.Equals(Path.GetFileName(file), "SAVEGAME.sav", StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsSaveGameSavPath(string path)
        {
            return !string.IsNullOrWhiteSpace(path)
                && File.Exists(path)
                && string.Equals(Path.GetFileName(path), "SAVEGAME.sav", StringComparison.OrdinalIgnoreCase);
        }

        public static ResourceType GetResourceTypeFromPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var fileName = Path.GetFileName(path);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return null;
            }

            foreach (var candidate in AllKnownResourceTypes()
                         .Where(r => r != null && !r.IsInvalid && !string.IsNullOrWhiteSpace(r.Extension))
                         .OrderByDescending(r => r.Extension.Length))
            {
                if (fileName.EndsWith("." + candidate.Extension, StringComparison.OrdinalIgnoreCase))
                {
                    return candidate;
                }
            }

            return null;
        }

        private static ResourceType[] AllKnownResourceTypes()
        {
            return typeof(ResourceType)
                .GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(field => field.FieldType == typeof(ResourceType))
                .Select(field => (ResourceType)field.GetValue(null))
                .ToArray();
        }

        private static bool IsPlainTextFallbackPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            var extension = GetPathExtension(path);
            if (string.IsNullOrWhiteSpace(extension))
            {
                return false;
            }

            foreach (var fallbackExtension in PlainTextFallbackExtensions)
            {
                if (extension == fallbackExtension)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsExtension(string path, string extension)
        {
            return GetPathExtension(path) == extension;
        }

        private static string GetPathExtension(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }

            return Path.GetExtension(path).TrimStart('.').ToLowerInvariant();
        }

        public static string GetEditorKey(ResourceType restype)
        {
            if (restype == null || restype.IsInvalid)
            {
                return null;
            }

            var targetType = restype.TargetType();
            if (targetType == ResourceType.TwoDA) return "2da";
            if (restype == ResourceType.TwoDA_CSV || restype == ResourceType.TwoDA_JSON) return "2da";
            if (targetType == ResourceType.ARE) return "are";
            if (restype.Category == "Audio") return "wav";
            if (restype.Category == "Walkmeshes" || targetType.Category == "Walkmeshes") return "bwm";
            if (targetType == ResourceType.DLG) return "dlg";
            if (restype == ResourceType.CNV || restype == ResourceType.DLG_TWINE_HTML || restype == ResourceType.DLG_TWINE_JSON) return "dlg";
            if (restype == ResourceType.ERF || restype == ResourceType.MOD || restype == ResourceType.RIM || restype == ResourceType.SAV || restype == ResourceType.BIF || restype == ResourceType.HAK) return "erf";
            if (targetType == ResourceType.FAC) return "fac";
            if (targetType == ResourceType.GUI) return "gui";
            if (targetType == ResourceType.GIT) return "git";
            if (targetType == ResourceType.IFO) return "ifo";
            if (targetType == ResourceType.JRL) return "jrl";
            if (targetType == ResourceType.LIP) return "lip";
            if (targetType == ResourceType.LTR) return "ltr";
            if (targetType == ResourceType.LYT) return "lyt";
            if (restype == ResourceType.MDL || restype == ResourceType.MDX) return "mdl";
            if (restype == ResourceType.MDL_ASCII) return "mdl";
            if (targetType == ResourceType.NSS || targetType == ResourceType.NCS) return "nss";
            if (targetType == ResourceType.PTH) return "pth";
            if (targetType == ResourceType.SSF) return "ssf";
            if (targetType == ResourceType.TLK) return "tlk";
            if (restype == ResourceType.TLK_XML || restype == ResourceType.TLK_JSON) return "tlk";
            if (restype == ResourceType.PLT) return "tpc";
            if ((targetType.Category == "Images" || targetType.Category == "Textures") && targetType != ResourceType.TXI) return "tpc";
            if (targetType == ResourceType.UTC || targetType == ResourceType.BTC || targetType == ResourceType.BIC) return "utc";
            if (targetType == ResourceType.UTD || targetType == ResourceType.BTD) return "utd";
            if (targetType == ResourceType.UTE || targetType == ResourceType.BTE) return "ute";
            if (targetType == ResourceType.UTI || targetType == ResourceType.BTI) return "uti";
            if (targetType == ResourceType.UTM || targetType == ResourceType.BTM) return "utm";
            if (targetType == ResourceType.UTP || targetType == ResourceType.BTP) return "utp";
            if (targetType == ResourceType.UTS) return "uts";
            if (targetType == ResourceType.UTT || targetType == ResourceType.BTT) return "utt";
            if (targetType == ResourceType.UTW) return "utw";
            if (targetType.Contents == "gff") return "gff";
            if (restype.Contents == "plaintext") return "txt";
            return null;
        }

        public static string GetStandaloneExecutableName(ResourceType restype)
        {
            var key = GetEditorKey(restype);
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            if (key == "2da")
            {
                return "OdyTool2DA";
            }

            return "OdyTool" + key.ToUpperInvariant() + ".Standalone";
        }

        public static string GetStandaloneExecutableNameForKey(string key)
        {
            key = NormalizeEditorKey(key);
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            if (key == "2da")
            {
                return "OdyTool2DA";
            }

            if (key == "savegame")
            {
                return "OdyToolSAV.Standalone";
            }

            if (key == "module-designer" || key == "indoor-builder")
            {
                return "OdyTools.Standalone";
            }

            return "OdyTool" + key.ToUpperInvariant() + ".Standalone";
        }
    }

    internal sealed class EditorLaunchInfo
    {
        public EditorLaunchInfo(string key, string label)
        {
            Key = key;
            Label = label;
        }

        public string Key { get; }
        public string Label { get; }

        public override string ToString()
        {
            return Label;
        }
    }
}
