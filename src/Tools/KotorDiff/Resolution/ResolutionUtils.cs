// Utility methods for Resolution project to avoid circular dependency with Diff
using System;
using System.IO;

namespace KotorDiff.Resolution
{
    /// <summary>
    /// Utility methods for resolution operations.
    /// These are duplicates of methods from DiffEngineUtils to avoid circular dependency.
    /// </summary>
    internal static class ResolutionUtils
    {
        /// <summary>
        /// Check if a path is a KOTOR installation directory.
        /// </summary>
        public static bool IsKotorInstallDir(string path)
        {
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                return false;
            }
            string chitinKey = Path.Combine(path, "chitin.key");
            return File.Exists(chitinKey);
        }

        /// <summary>
        /// Get the module root name from a module filepath.
        /// </summary>
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
    }
}

