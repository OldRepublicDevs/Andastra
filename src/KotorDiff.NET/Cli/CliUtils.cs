// Matching PyKotor implementation at vendor/PyKotor/Tools/KotorDiff/src/kotordiff/cli_utils.py:13-75
// Original: def normalize_path_arg(path_str: str | None) -> str | None: ...
using System;
using System.IO;

namespace KotorDiff.NET.Cli
{
    // Matching PyKotor implementation at vendor/PyKotor/Tools/KotorDiff/src/kotordiff/cli_utils.py:13-50
    // Original: def normalize_path_arg(path_str: str | None) -> str | None: ...
    public static class CliUtils
    {
        public static string NormalizePathArg(string pathStr)
        {
            if (string.IsNullOrEmpty(pathStr))
            {
                return null;
            }

            pathStr = pathStr.Trim();

            if (string.IsNullOrEmpty(pathStr))
            {
                return null;
            }

            // Handle Windows PowerShell quote escaping issues
            if (pathStr.Contains("\"") && pathStr.Contains(" "))
            {
                int quoteSpaceIdx = pathStr.IndexOf("\" ");
                if (quoteSpaceIdx > 0)
                {
                    pathStr = pathStr.Substring(0, quoteSpaceIdx);
                }
            }

            // Strip quotes if present
            if ((pathStr.StartsWith("\"") && pathStr.EndsWith("\"")) ||
                (pathStr.StartsWith("'") && pathStr.EndsWith("'")))
            {
                pathStr = pathStr.Substring(1, pathStr.Length - 2);
            }

            // Remove any remaining quotes
            pathStr = pathStr.Replace("\"", "").Replace("'", "");

            // Strip trailing backslashes
            pathStr = pathStr.TrimEnd('\\', '/');
            pathStr = pathStr.Trim();

            return string.IsNullOrEmpty(pathStr) ? null : pathStr;
        }

        // Matching PyKotor implementation at vendor/PyKotor/Tools/KotorDiff/src/kotordiff/cli_utils.py:52-54
        // Original: def is_kotor_install_dir(path: Path) -> bool | None: ...
        public static bool IsKotorInstallDir(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                return false;
            }

            string chitinKey = Path.Combine(path, "chitin.key");
            return File.Exists(chitinKey);
        }
    }
}

