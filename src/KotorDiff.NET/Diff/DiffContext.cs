// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:997-1028
// Original: @dataclass class DiffContext: ...
using System.IO;

namespace KotorDiff.NET.Diff
{
    // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:997-1028
    // Original: @dataclass class DiffContext: ...
    public class DiffContext
    {
        public FileInfo File1Rel { get; set; }
        public FileInfo File2Rel { get; set; }
        public string Ext { get; set; }
        public string ResName { get; set; }

        // Resolution order location types (for resolution-aware diffing)
        public string File1LocationType { get; set; } // Location type in vanilla/older install (Override, Modules (.mod), etc.)
        public string File2LocationType { get; set; } // Location type in modded/newer install
        public FileInfo File1Filepath { get; set; } // Full filepath in base installation (for StrRef reference finding)
        public FileInfo File2Filepath { get; set; } // Full filepath in target installation (for module name extraction)
        // TODO: Add Installation objects when porting
        // public Installation File1Installation { get; set; } // Base installation object
        // public Installation File2Installation { get; set; } // Target installation object

        // Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/engine.py:1014-1027
        // Original: @property def where(self) -> str: ...
        public string Where
        {
            get
            {
                if (!string.IsNullOrEmpty(ResName))
                {
                    // For resources inside containers (capsules/BIFs)
                    return $"{File2Rel}/{ResName}.{Ext}";
                }
                // For loose files, just return the full path from modded/target
                return File2Rel?.FullName ?? "";
            }
        }

        public DiffContext(FileInfo file1Rel, FileInfo file2Rel, string ext, string resName = null)
        {
            File1Rel = file1Rel;
            File2Rel = file2Rel;
            Ext = ext;
            ResName = resName;
        }
    }
}

