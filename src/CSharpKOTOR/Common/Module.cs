using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpKOTOR.Formats.Capsule;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Installation;
using CSharpKOTOR.Resources;
using JetBrains.Annotations;

namespace CSharpKOTOR.Common
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/common/module.py:70-75
    // Original: SEARCH_ORDER: list[SearchLocation] = [SearchLocation.OVERRIDE, SearchLocation.CUSTOM_MODULES, SearchLocation.CHITIN]
    public static class ModuleSearchOrder
    {
        public static readonly SearchLocation[] Order = { SearchLocation.OVERRIDE, SearchLocation.CUSTOM_MODULES, SearchLocation.CHITIN };
    }

    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/common/module.py:78-184
    // Original: class KModuleType(Enum):
    /// <summary>
    /// Module file type enumeration.
    /// KotOR modules are split across multiple archive files. The module system
    /// uses different file extensions to organize resources by type and priority.
    /// </summary>
    public enum KModuleType
    {
        /// <summary>
        /// Main module archive containing core module files.
        /// Contains: IFO (module info), ARE (area data), GIT (dynamic area info)
        /// File naming: &lt;modulename&gt;.rim
        /// </summary>
        MAIN,  // .rim

        /// <summary>
        /// Data module archive containing module resources.
        /// Contains: UTC, UTD, UTE, UTI, UTM, UTP, UTS, UTT, UTW, FAC, LYT, NCS, PTH
        /// File naming: &lt;modulename&gt;_s.rim
        /// Note: In KotOR 2, DLG files are NOT in _s.rim (see K2_DLG)
        /// </summary>
        DATA,  // _s.rim

        /// <summary>
        /// KotOR 2 dialog archive containing dialog files.
        /// Contains: DLG (dialog) files
        /// File naming: &lt;modulename&gt;_dlg.erf
        /// Note: KotOR 1 stores DLG files in _s.rim, KotOR 2 uses separate _dlg.erf
        /// </summary>
        K2_DLG,  // _dlg.erf

        /// <summary>
        /// Community override module archive (single-file format).
        /// Contains: All module resources in a single ERF archive
        /// File naming: &lt;modulename&gt;.mod
        /// Priority: Takes precedence over .rim/_s.rim/_dlg.erf files
        /// </summary>
        MOD  // .mod
    }

    public static class KModuleTypeExtensions
    {
        public static string GetExtension(this KModuleType type)
        {
            switch (type)
            {
                case KModuleType.MAIN:
                    return ".rim";
                case KModuleType.DATA:
                    return "_s.rim";
                case KModuleType.K2_DLG:
                    return "_dlg.erf";
                case KModuleType.MOD:
                    return ".mod";
                default:
                    throw new ArgumentException($"Invalid KModuleType: {type}");
            }
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/common/module.py:136-183
        // Original: def contains(self, restype: ResourceType, *, game: Game | None = None) -> bool:
        public static bool Contains(this KModuleType type, ResourceType restype, Game? game = null)
        {
            if (restype.TargetType() != restype)
            {
                return false;
            }

            if (restype == ResourceType.DLG)
            {
                if (game == null)
                {
                    return type == KModuleType.DATA || type == KModuleType.K2_DLG;
                }
                if (game.Value.IsK1())
                {
                    return type == KModuleType.DATA;
                }
                if (game.Value.IsK2())
                {
                    return type == KModuleType.K2_DLG;
                }
            }

            if (type == KModuleType.MOD)
            {
                return restype != ResourceType.TwoDA;
            }

            if (type == KModuleType.MAIN)
            {
                return restype == ResourceType.ARE || restype == ResourceType.IFO || restype == ResourceType.GIT;
            }

            if (type == KModuleType.DATA)
            {
                return restype == ResourceType.FAC ||
                       restype == ResourceType.LYT ||
                       restype == ResourceType.NCS ||
                       restype == ResourceType.PTH ||
                       restype == ResourceType.UTC ||
                       restype == ResourceType.UTD ||
                       restype == ResourceType.UTE ||
                       restype == ResourceType.UTI ||
                       restype == ResourceType.UTM ||
                       restype == ResourceType.UTP ||
                       restype == ResourceType.UTS ||
                       restype == ResourceType.UTT ||
                       restype == ResourceType.UTW;
            }

            throw new InvalidOperationException($"Invalid KModuleType enum: {type}");
        }
    }

    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/common/module.py:186-213
    // Original: @dataclass(frozen=True) class ModulePieceInfo:
    /// <summary>
    /// Information about a module piece (archive file).
    /// </summary>
    public sealed class ModulePieceInfo
    {
        public string Root { get; }
        public KModuleType ModType { get; }

        public ModulePieceInfo(string root, KModuleType modType)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));
            ModType = modType;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/common/module.py:191-199
        // Original: @classmethod def from_filename(cls, filename: str | ResourceIdentifier) -> Self:
        public static ModulePieceInfo FromFilename(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            string root = Module.NameToRoot(filename);
            string extension = filename.Substring(root.Length);
            KModuleType modType = extension switch
            {
                ".rim" => KModuleType.MAIN,
                "_s.rim" => KModuleType.DATA,
                "_dlg.erf" => KModuleType.K2_DLG,
                ".mod" => KModuleType.MOD,
                _ => throw new ArgumentException($"Unknown module extension: {extension}")
            };

            return new ModulePieceInfo(root, modType);
        }

        public static ModulePieceInfo FromFilename(ResourceIdentifier identifier)
        {
            return FromFilename(identifier.ToString());
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/common/module.py:201-202
        // Original: def filename(self) -> str:
        public string Filename()
        {
            return Root + ModType.GetExtension();
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/common/module.py:204-205
        // Original: def res_ident(self) -> ResourceIdentifier:
        public ResourceIdentifier ResIdent()
        {
            return ResourceIdentifier.FromPath(Filename());
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/common/module.py:207-209
        // Original: def resname(self) -> str:
        public string ResName()
        {
            string filename = Filename();
            int dotIndex = filename.IndexOf('.');
            return dotIndex >= 0 ? filename.Substring(0, dotIndex) : filename;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/common/module.py:211-212
        // Original: def restype(self) -> ResourceType:
        public ResourceType ResType()
        {
            return ResourceType.FromExtension(ModType.GetExtension());
        }
    }
}
