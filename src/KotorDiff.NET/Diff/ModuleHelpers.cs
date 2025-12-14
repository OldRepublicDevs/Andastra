// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:88-111, 1988-2007, 2010-2012
// Original: Various module helper functions
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpKOTOR.Installation;

namespace KotorDiff.NET.Diff
{
    // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:88-93
    // Original: def get_module_root(module_filepath: Path) -> str: ...
    public static class ModuleHelpers
    {
        /// <summary>
        /// Extract the module root name, following Installation.py logic.
        /// </summary>
        public static string GetModuleRoot(string moduleFilepath)
        {
            string filename = Path.GetFileName(moduleFilepath);
            string root = Path.GetFileNameWithoutExtension(filename).ToLowerInvariant();
            root = root.EndsWith("_s") ? root.Substring(0, root.Length - 2) : root;
            root = root.EndsWith("_dlg") ? root.Substring(0, root.Length - 4) : root;
            return root;
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1988-2007
        // Original: def group_module_files(files: set[str]) -> dict[str, list[str]]: ...
        /// <summary>
        /// Group module files by their root name.
        /// </summary>
        public static Dictionary<string, List<string>> GroupModuleFiles(HashSet<string> files)
        {
            var moduleGroups = new Dictionary<string, List<string>>();

            foreach (string filePath in files)
            {
                string filename = Path.GetFileName(filePath);
                if (IsCapsuleFile(filename))
                {
                    string root = GetModuleRoot(filePath);
                    if (!moduleGroups.ContainsKey(root))
                    {
                        moduleGroups[root] = new List<string>();
                    }
                    moduleGroups[root].Add(filePath);
                }
            }

            return moduleGroups;
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:2010-2012
        // Original: def is_modules_directory(dir_path: Path) -> bool: ...
        /// <summary>
        /// Check if a directory is a modules directory.
        /// </summary>
        public static bool IsModulesDirectory(DirectoryInfo dirPath)
        {
            string name = dirPath.Name.ToLowerInvariant();
            return name == "modules" || name == "module" || name == "mods";
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:78-80
        // Original: def is_capsule_file(filename: str) -> bool: ...
        /// <summary>
        /// Check if a filename is a capsule file (ERF, RIM, MOD, SAV).
        /// </summary>
        public static bool IsCapsuleFile(string filename)
        {
            if (string.IsNullOrEmpty(filename))
            {
                return false;
            }

            string ext = Path.GetExtension(filename).ToLowerInvariant().TrimStart('.');
            return ext == "erf" || ext == "mod" || ext == "rim" || ext == "sav";
        }

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1906-1927
        // Original: def should_use_composite_for_file(...): ...
        /// <summary>
        /// Determine if composite module loading should be used for a specific file.
        /// Only use composite loading for .rim files when comparing against .mod files.
        /// </summary>
        public static bool ShouldUseCompositeForFile(string filePath, string otherFilePath)
        {
            var file = new FileInfo(filePath);
            var otherFile = new FileInfo(otherFilePath);

            // Check if this file is a .rim file (not in rims folder)
            if (!IsCapsuleFile(file.Name))
            {
                return false;
            }
            if (file.Directory != null && file.Directory.Name.ToLowerInvariant() == "rims")
            {
                return false;
            }
            if (Path.GetExtension(file.Name).ToLowerInvariant() != ".rim")
            {
                return false;
            }

            // Check if the other file is a .mod file (not in rims folder)
            if (!IsCapsuleFile(otherFile.Name))
            {
                return false;
            }
            if (otherFile.Directory != null && otherFile.Directory.Name.ToLowerInvariant() == "rims")
            {
                return false;
            }
            return Path.GetExtension(otherFile.Name).ToLowerInvariant() == ".mod";
        }
    }
}

