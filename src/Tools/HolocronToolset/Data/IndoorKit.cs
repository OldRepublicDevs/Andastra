using System.Collections.Generic;
using System.Numerics;
using Andastra.Parsing;
using Andastra.Parsing.Formats.BWM;
using Andastra.Parsing.Formats.GFF;
using Andastra.Parsing.Formats.MDL;
using Andastra.Parsing.Resource.Generics;

namespace HolocronToolset.Data
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit.py:24
    // Original: class Kit:
    public class Kit
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit.py:25-35
        // Original: def __init__(self, name: str):
        public Kit(string name)
        {
            Name = name;
            Components = new List<KitComponent>();
            Doors = new List<KitDoor>();
            Textures = new Dictionary<string, byte[]>();
            Lightmaps = new Dictionary<string, byte[]>();
            Txis = new Dictionary<string, byte[]>();
            Always = new Dictionary<string, byte[]>();
            SidePadding = new Dictionary<int, Dictionary<int, MDLMDXTuple>>();
            TopPadding = new Dictionary<int, Dictionary<int, MDLMDXTuple>>();
            Skyboxes = new Dictionary<string, MDLMDXTuple>();
        }

        public string Name { get; set; }
        public List<KitComponent> Components { get; set; }
        public List<KitDoor> Doors { get; set; }
        public Dictionary<string, byte[]> Textures { get; set; }
        public Dictionary<string, byte[]> Lightmaps { get; set; }
        public Dictionary<string, byte[]> Txis { get; set; }
        public Dictionary<string, byte[]> Always { get; set; }
        public Dictionary<int, Dictionary<int, MDLMDXTuple>> SidePadding { get; set; }
        public Dictionary<int, Dictionary<int, MDLMDXTuple>> TopPadding { get; set; }
        public Dictionary<string, MDLMDXTuple> Skyboxes { get; set; }
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit.py:38
    // Original: class KitComponent:
    public class KitComponent
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit.py:39-47
        // Original: def __init__(self, kit: Kit, name: str, image: QImage, bwm: BWM, mdl: bytes, mdx: bytes):
        public KitComponent(Kit kit, string name, object image, BWM bwm, byte[] mdl, byte[] mdx)
        {
            Kit = kit;
            Name = name;
            Image = image;
            Bwm = bwm;
            Mdl = mdl;
            Mdx = mdx;
            Hooks = new List<KitComponentHook>();
        }

        public Kit Kit { get; set; }
        public string Name { get; set; }
        public object Image { get; set; }
        public BWM Bwm { get; set; }
        public byte[] Mdl { get; set; }
        public byte[] Mdx { get; set; }
        public List<KitComponentHook> Hooks { get; set; }
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit.py:50
    // Original: class KitComponentHook:
    public class KitComponentHook
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit.py:51-55
        // Original: def __init__(self, position: Vector3, rotation: float, edge: int, door: KitDoor):
        public KitComponentHook(Vector3 position, float rotation, int edge, KitDoor door)
        {
            Position = position;
            Rotation = rotation;
            Edge = edge;
            Door = door;
        }

        public Vector3 Position { get; set; }
        public float Rotation { get; set; }
        public int Edge { get; set; }
        public KitDoor Door { get; set; }
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit.py:58
    // Original: class KitDoor:
    public class KitDoor
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit.py:59-63
        // Original: def __init__(self, utdK1: UTD, utdK2: UTD, width: float, height: float):
        public KitDoor(UTD utdK1, UTD utdK2, float width, float height)
        {
            UtdK1 = utdK1;
            UtdK2 = utdK2;
            Width = width;
            Height = height;
        }

        public UTD UtdK1 { get; set; }
        public UTD UtdK2 { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit.py:66
    // Original: class MDLMDXTuple(NamedTuple):
    public class MDLMDXTuple
    {
        public MDLMDXTuple(byte[] mdl, byte[] mdx)
        {
            Mdl = mdl;
            Mdx = mdx;
        }

        public byte[] Mdl { get; set; }
        public byte[] Mdx { get; set; }
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit/module_converter.py:33-57
    // Original: class ModuleKit(Kit):
    public class ModuleKit : Kit
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit/module_converter.py:43-57
        // Original: def __init__(self, name: str, module_root: str, installation: HTInstallation):
        public ModuleKit(string name, string moduleRoot, HTInstallation installation) : base(name)
        {
            ModuleRoot = moduleRoot;
            _installation = installation;
            _loaded = false;
            IsModuleKit = true;
            SourceModule = moduleRoot;
        }

        public string ModuleRoot { get; set; }
        public bool IsModuleKit { get; set; }
        public string SourceModule { get; set; }
        private HTInstallation _installation;
        private bool _loaded;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit/module_converter.py:59-74
        // Original: def ensure_loaded(self) -> bool:
        public bool EnsureLoaded()
        {
            if (_loaded)
            {
                return Components.Count > 0;
            }

            _loaded = true;
            try
            {
                // TODO: Implement _load_module_components() when Module class is available
                // For now, return false to indicate components weren't loaded
                return false;
            }
            catch
            {
                return false;
            }
        }
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit/module_converter.py:419-503
    // Original: class ModuleKitManager:
    public class ModuleKitManager
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit/module_converter.py:426-429
        // Original: def __init__(self, installation: HTInstallation):
        public ModuleKitManager(HTInstallation installation)
        {
            _installation = installation;
            _cache = new Dictionary<string, ModuleKit>();
            _moduleNames = null;
        }

        private HTInstallation _installation;
        private Dictionary<string, ModuleKit> _cache;
        private Dictionary<string, string> _moduleNames;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit/module_converter.py:431-441
        // Original: def get_module_names(self) -> dict[str, str | None]:
        public Dictionary<string, string> GetModuleNames()
        {
            if (_moduleNames == null)
            {
                _moduleNames = _installation.ModuleNames();
            }
            return _moduleNames;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit/module_converter.py:443-458
        // Original: def get_module_roots(self) -> list[str]:
        public List<string> GetModuleRoots()
        {
            var seenRoots = new HashSet<string>();
            var roots = new List<string>();

            var moduleNames = GetModuleNames();
            foreach (var moduleFilename in moduleNames.Keys)
            {
                string root = Andastra.Parsing.Installation.Installation.GetModuleRoot(moduleFilename);
                if (!seenRoots.Contains(root))
                {
                    seenRoots.Add(root);
                    roots.Add(root);
                }
            }

            roots.Sort();
            return roots;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit/module_converter.py:460-479
        // Original: def get_module_display_name(self, module_root: str) -> str:
        public string GetModuleDisplayName(string moduleRoot)
        {
            var moduleNames = GetModuleNames();

            // Try to find the display name from various extensions
            string[] extensions = { ".rim", ".mod", "_s.rim" };
            foreach (var ext in extensions)
            {
                string filename = moduleRoot + ext;
                if (moduleNames.ContainsKey(filename))
                {
                    string areaName = moduleNames[filename];
                    if (!string.IsNullOrEmpty(areaName) && areaName != "<Unknown Area>")
                    {
                        return $"{moduleRoot.ToUpperInvariant()} - {areaName}";
                    }
                }
            }

            return moduleRoot.ToUpperInvariant();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit/module_converter.py:481-498
        // Original: def get_module_kit(self, module_root: str) -> ModuleKit:
        public ModuleKit GetModuleKit(string moduleRoot)
        {
            if (!_cache.ContainsKey(moduleRoot))
            {
                string displayName = GetModuleDisplayName(moduleRoot);
                var kit = new ModuleKit(displayName, moduleRoot, _installation);
                _cache[moduleRoot] = kit;
            }

            return _cache[moduleRoot];
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/indoorkit/module_converter.py:500-503
        // Original: def clear_cache(self):
        public void ClearCache()
        {
            _cache.Clear();
            _moduleNames = null;
        }
    }
}

