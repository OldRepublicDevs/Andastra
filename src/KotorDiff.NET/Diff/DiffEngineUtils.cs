// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:78-110
// Original: def is_kotor_install_dir(...), def get_module_root(...), etc.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace KotorDiff.NET.Diff
{
    // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:78-80
    // Original: def is_kotor_install_dir(path: Path) -> bool | None: ...
    public static class DiffEngineUtils
    {
        public static bool IsKotorInstallDir(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                return false;
            }
            string chitinKey = Path.Combine(path, "chitin.key");
            return File.Exists(chitinKey);
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:88-93
        // Original: def get_module_root(module_filepath: Path) -> str: ...
        public static string GetModuleRoot(string moduleFilepath)
        {
            string root = Path.GetFileNameWithoutExtension(moduleFilepath).ToLowerInvariant();
            if (root.EndsWith("_s"))
            {
                root = root.Substring(0, root.Length - 2);
            }
            if (root.EndsWith("_dlg"))
            {
                root = root.Substring(0, root.Length - 4);
            }
            return root;
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1035-1057
        // Original: def is_text_content(data: bytes) -> bool: ...
        public static bool IsTextContent(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return true;
            }

            try
            {
                // Try to decode as UTF-8 first
                Encoding.UTF8.GetString(data);
                return true;
            }
            catch (DecoderFallbackException)
            {
                // Try Windows-1252 (common for KOTOR text files)
                try
                {
                    Encoding.GetEncoding(1252).GetString(data);
                    return true;
                }
                catch (DecoderFallbackException)
                {
                    // Check for high ratio of printable ASCII characters
                    const int PRINTABLE_ASCII_MIN = 32;
                    const int PRINTABLE_ASCII_MAX = 126;
                    const double TEXT_THRESHOLD = 0.7;

                    int printableCount = data.Count(b => 
                        (b >= PRINTABLE_ASCII_MIN && b <= PRINTABLE_ASCII_MAX) || 
                        b == 9 || b == 10 || b == 13);
                    return (double)printableCount / data.Length > TEXT_THRESHOLD;
                }
            }
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1060-1068
        // Original: def read_text_lines(filepath: Path) -> list[str]: ...
        public static List<string> ReadTextLines(string filepath)
        {
            try
            {
                return File.ReadAllLines(filepath, Encoding.UTF8).ToList();
            }
            catch (Exception)
            {
                try
                {
                    return File.ReadAllLines(filepath, Encoding.GetEncoding(1252)).ToList();
                }
                catch (Exception)
                {
                    return new List<string>();
                }
            }
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1205-1210
        // Original: def should_skip_rel(_rel: str) -> bool: ...
        public static bool ShouldSkipRel(string rel)
        {
            return false; // Currently unused but kept for future filtering capabilities
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1199-1202
        // Original: def ext_of(path: Path) -> str: ...
        public static string ExtOf(string path)
        {
            string ext = Path.GetExtension(path).ToLowerInvariant();
            return ext.StartsWith(".") ? ext.Substring(1) : ext;
        }
    }
}

