using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.TwoDA;
using CSharpKOTOR.Installation;
using CSharpKOTOR.Resources;
using JetBrains.Annotations;
using ResourceResult = CSharpKOTOR.Installation.ResourceResult;

namespace HolocronToolset.NET.Data
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:48
    // Original: class HTInstallation(Installation):
    public class HTInstallation
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:49-91
        // Original: TwoDA_PORTRAITS: str = TwoDARegistry.PORTRAITS
        public const string TwoDAPortraits = "portraits.2da";
        public const string TwoDAAppearances = "appearance.2da";
        public const string TwoDASubraces = "subraces.2da";
        public const string TwoDASpeeds = "speeds.2da";
        public const string TwoDASoundsets = "soundset.2da";
        public const string TwoDAFactions = "factions.2da";
        public const string TwoDAGenders = "genders.2da";
        public const string TwoDAPerceptions = "perceptions.2da";
        public const string TwoDAClasses = "classes.2da";
        public const string TwoDAFeats = "feat.2da";
        public const string TwoDAPowers = "spells.2da";
        public const string TwoDABaseitems = "baseitems.2da";
        public const string TwoDAPlaceables = "placeables.2da";
        public const string TwoDADoors = "doortypes.2da";
        public const string TwoDACursors = "cursors.2da";
        public const string TwoDATraps = "traps.2da";
        public const string TwoDARaces = "racialtypes.2da";
        public const string TwoDASkills = "skills.2da";
        public const string TwoDAUpgrades = "upcrystals.2da";
        public const string TwoDAEncDifficulties = "encdifficulty.2da";
        public const string TwoDAItemProperties = "itemprops.2da";
        public const string TwoDAIprpParamtable = "iprp_paramtable.2da";
        public const string TwoDAIprpCosttable = "iprp_costtable.2da";
        public const string TwoDAIprpAbilities = "iprp_abilities.2da";
        public const string TwoDAIprpAligngrp = "iprp_aligngrp.2da";
        public const string TwoDAIprpCombatdam = "iprp_combatdam.2da";
        public const string TwoDAIprpDamagetype = "iprp_damagetype.2da";
        public const string TwoDAIprpProtection = "iprp_protection.2da";
        public const string TwoDAIprpAcmodtype = "iprp_acmodtype.2da";
        public const string TwoDAIprpImmunity = "iprp_immunity.2da";
        public const string TwoDAIprpSaveelement = "iprp_saveelement.2da";
        public const string TwoDAIprpSavingthrow = "iprp_savingthrow.2da";
        public const string TwoDAIprpOnhit = "iprp_onhit.2da";
        public const string TwoDAIprpAmmotype = "iprp_ammotype.2da";
        public const string TwoDAIprpMonsterhit = "iprp_monsterhit.2da";
        public const string TwoDAIprpWalk = "iprp_walk.2da";
        public const string TwoDAEmotions = "emotions.2da";
        public const string TwoDAExpressions = "expressions.2da";
        public const string TwoDAVideoEffects = "videoeffects.2da";
        public const string TwoDADialogAnims = "dialoganimations.2da";
        public const string TwoDAPlanets = "planetary.2da";
        public const string TwoDAPlot = "plot.2da";
        public const string TwoDACameras = "cameras.2da";

        private readonly Installation _installation;
        private readonly Dictionary<string, TwoDA> _cache2da = new Dictionary<string, TwoDA>();
        private bool? _tsl;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:93-120
        // Original: def __init__(self, path: str | os.PathLike, name: str, *, tsl: bool | None = None, ...):
        public HTInstallation(string path, string name, bool? tsl = null)
        {
            _installation = new Installation(path);
            Name = name;
            _tsl = tsl;
        }

        public string Name { get; set; }
        public Installation Installation => _installation;
        public Game Game => _installation.Game;
        public string Path => _installation.Path;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def resource(self, resname: str, restype: ResourceType, ...) -> ResourceResult | None:
        [CanBeNull]
        public ResourceResult Resource(string resname, ResourceType restype, SearchLocation[] searchOrder = null)
        {
            return _installation.Resource(resname, restype, searchOrder);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:444-469
        // Original: def ht_get_cache_2da(self, resname: str) -> TwoDA | None:
        [CanBeNull]
        public TwoDA HtGetCache2DA(string resname)
        {
            resname = resname.ToLowerInvariant();
            if (!_cache2da.ContainsKey(resname))
            {
                ResourceResult result = _installation.Resource(
                    resname,
                    ResourceType.TwoDA,
                    new[] { SearchLocation.OVERRIDE, SearchLocation.CHITIN });
                if (result == null)
                {
                    return null;
                }
                var reader = new TwoDABinaryReader(result.Data);
                _cache2da[resname] = reader.Load();
            }
            return _cache2da[resname];
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:673-677
        // Original: @property def tsl(self) -> bool:
        public bool Tsl
        {
            get
            {
                if (!_tsl.HasValue)
                {
                    _tsl = Game == Game.TSL;
                }
                return _tsl.Value;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:549-550
        // Original: def htClearCache2DA(self):
        public void HtClearCache2DA()
        {
            _cache2da.Clear();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:146-151
        // Original: def clear_all_caches(self):
        public void ClearAllCaches()
        {
            _cache2da.Clear();
            _installation.ClearCache();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def module_path(self) -> Path:
        public string ModulePath()
        {
            return Installation.GetModulesPath(Path);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def override_path(self) -> Path:
        public string OverridePath()
        {
            return Installation.GetOverridePath(Path);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def save_locations(self) -> list[Path]:
        public List<string> SaveLocations()
        {
            var locations = new List<string>();
            // Get save locations from installation
            // This will be implemented when save location detection is available
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (Tsl)
            {
                locations.Add(System.IO.Path.Combine(documentsPath, "Knights of the Old Republic II", "saves"));
            }
            else
            {
                locations.Add(System.IO.Path.Combine(documentsPath, "Knights of the Old Republic", "saves"));
            }
            return locations.Where(loc => Directory.Exists(loc)).ToList();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def core_resources(self) -> list[FileResource]:
        public List<FileResource> CoreResources()
        {
            var resources = new List<FileResource>();
            // Get core resources from chitin
            // This will be implemented when resource enumeration is available
            return resources;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def modules_list(self) -> list[str]:
        public List<string> ModulesList()
        {
            var modules = new List<string>();
            string modulesPath = ModulePath();
            if (!Directory.Exists(modulesPath))
            {
                return modules;
            }

            // Get module files
            var moduleFiles = Directory.GetFiles(modulesPath, "*.rim")
                .Concat(Directory.GetFiles(modulesPath, "*.mod"))
                .Concat(Directory.GetFiles(modulesPath, "*.erf"))
                .Select(f => System.IO.Path.GetFileName(f))
                .ToList();

            modules.AddRange(moduleFiles);
            return modules;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def module_names(self) -> dict[str, str | None]:
        public Dictionary<string, string> ModuleNames()
        {
            var moduleNames = new Dictionary<string, string>();
            string modulesPath = ModulePath();
            if (!Directory.Exists(modulesPath))
            {
                return moduleNames;
            }

            // Get module files
            var moduleFiles = Directory.GetFiles(modulesPath, "*.rim")
                .Concat(Directory.GetFiles(modulesPath, "*.mod"))
                .Concat(Directory.GetFiles(modulesPath, "*.erf"))
                .Select(f => System.IO.Path.GetFileName(f))
                .ToList();

            foreach (var moduleFile in moduleFiles)
            {
                // Try to get area name from module
                string areaName = GetModuleAreaName(moduleFile);
                moduleNames[moduleFile] = areaName;
            }

            return moduleNames;
        }

        private string GetModuleAreaName(string moduleFile)
        {
            // Try to get area name from module.ifo in the module
            // This will be implemented when module reading is available
            return "<Unknown Area>";
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def override_list(self) -> list[str]:
        public List<string> OverrideList()
        {
            var overrideDirs = new List<string>();
            string overridePath = OverridePath();
            if (!Directory.Exists(overridePath))
            {
                return overrideDirs;
            }

            // Get all subdirectories in override
            var dirs = Directory.GetDirectories(overridePath, "*", SearchOption.AllDirectories)
                .Select(d => System.IO.Path.GetRelativePath(overridePath, d))
                .ToList();
            overrideDirs.Add("."); // Root override
            overrideDirs.AddRange(dirs);
            return overrideDirs;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def texturepacks_list(self) -> list[str]:
        public List<string> TexturepacksList()
        {
            var texturePacks = new List<string>();
            string texturePacksPath = Installation.GetTexturePacksPath(Path);
            if (!Directory.Exists(texturePacksPath))
            {
                return texturePacks;
            }

            // Get texture pack files
            var packFiles = Directory.GetFiles(texturePacksPath, "*.erf")
                .Select(f => System.IO.Path.GetFileName(f))
                .ToList();
            texturePacks.AddRange(packFiles);
            return texturePacks;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def module_resources(self, module_name: str) -> list[FileResource]:
        public List<FileResource> ModuleResources(string moduleName)
        {
            var resources = new List<FileResource>();
            // Get resources from module
            // This will be implemented when module resource enumeration is available
            return resources;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def override_resources(self, subfolder: str | None = None) -> list[FileResource]:
        public List<FileResource> OverrideResources(string subfolder = null)
        {
            var resources = new List<FileResource>();
            // Get resources from override directory
            // This will be implemented when override resource enumeration is available
            return resources;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def texturepack_resources(self, texturepack_name: str) -> list[FileResource]:
        public List<FileResource> TexturepackResources(string texturepackName)
        {
            var resources = new List<FileResource>();
            // Get resources from texture pack
            // This will be implemented when texture pack resource enumeration is available
            return resources;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py
        // Original: def string(self, locstring: LocalizedString) -> str | None:
        [CanBeNull]
        public string String(LocalizedString locstring)
        {
            if (locstring == null)
            {
                return null;
            }
            try
            {
                string tlkPath = System.IO.Path.Combine(Path, "dialog.tlk");
                if (!File.Exists(tlkPath))
                {
                    return null;
                }
                var talkTable = new CSharpKOTOR.Extract.TalkTable(tlkPath);
                string result = talkTable.GetString(locstring.StringRef);
                return string.IsNullOrEmpty(result) ? null : result;
            }
            catch
            {
                return null;
            }
        }
    }
}
