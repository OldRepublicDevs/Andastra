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

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1070-1100
        // Original: def compare_text_content(...): ...
        public static bool CompareTextContent(byte[] data1, byte[] data2, string where)
        {

            string text1;
            string text2;

            try
            {
                text1 = Encoding.UTF8.GetString(data1);
                text2 = Encoding.UTF8.GetString(data2);
            }
            catch (Exception)
            {
                try
                {
                    text1 = Encoding.GetEncoding(1252).GetString(data1);
                    text2 = Encoding.GetEncoding(1252).GetString(data2);
                }
                catch (Exception)
                {
                    // Last resort - treat as binary
                    return data1.SequenceEqual(data2);
                }
            }

            if (text1 == text2)
            {
                return true;
            }

            // Simple line-by-line diff for now
            var lines1 = text1.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var lines2 = text2.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            bool hasDiff = false;
            int maxLines = Math.Max(lines1.Length, lines2.Length);

            for (int i = 0; i < maxLines; i++)
            {
                string line1 = i < lines1.Length ? lines1[i] : "";
                string line2 = i < lines2.Length ? lines2[i] : "";

                if (line1 != line2)
                {
                    hasDiff = true;
                    break; // Found difference, no need to continue
                }
            }

            return !hasDiff;
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1102-1108
        // Original: def generate_hash(data: bytes) -> str: ...
        public static string CalculateSha256(byte[] data)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(data);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1050-1068
        // Original: def _determine_destination_for_source(...): ...
        public static string DetermineDestinationForSource(string sourceFilePath)
        {
            if (string.IsNullOrEmpty(sourceFilePath))
            {
                return "Override";
            }

            string lowerPath = sourceFilePath.ToLowerInvariant();
            if (lowerPath.Contains("override"))
            {
                return "Override";
            }
            if (lowerPath.Contains("modules"))
            {
                // Extract module name if it's a resource inside a module
                // e.g., "modules/tar_m01aa.mod/some_file.utc" -> "modules/tar_m01aa.mod"
                // This needs to be more robust. For now, return "modules"
                return "modules";
            }
            if (lowerPath.Contains("lips"))
            {
                return "Lips";
            }
            if (lowerPath.Contains("streamwaves") || lowerPath.Contains("streamvoice"))
            {
                return "StreamWaves";
            }
            // Default to Override for loose files in the game root or unknown locations
            return "Override";
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1110-1118
        // Original: def is_capsule_file(filename: str) -> bool: ...
        public static bool IsCapsuleFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }
            string ext = Path.GetExtension(filename).ToLowerInvariant();
            return ext == ".erf" || ext == ".mod" || ext == ".rim";
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1906-1928
        // Original: def should_use_composite_for_file(...): ...
        public static bool ShouldUseCompositeForFile(string filePath, string otherFilePath)
        {
            // Check if this file is a .rim file (not in rims folder)
            if (!IsCapsuleFile(Path.GetFileName(filePath)))
            {
                return false;
            }
            string parentName = Path.GetDirectoryName(filePath);
            if (parentName != null && Path.GetFileName(parentName).ToLowerInvariant() == "rims")
            {
                return false;
            }
            if (Path.GetExtension(filePath).ToLowerInvariant() != ".rim")
            {
                return false;
            }

            // Check if the other file is a .mod file (not in rims folder)
            if (!IsCapsuleFile(Path.GetFileName(otherFilePath)))
            {
                return false;
            }
            string otherParentName = Path.GetDirectoryName(otherFilePath);
            if (otherParentName != null && Path.GetFileName(otherParentName).ToLowerInvariant() == "rims")
            {
                return false;
            }
            return Path.GetExtension(otherFilePath).ToLowerInvariant() == ".mod";
        }
    }
}

