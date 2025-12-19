using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Navigation;
using Andastra.Runtime.Engines.Common;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Runtime.Games.Infinity;

namespace Andastra.Runtime.Engines.Infinity
{
    /// <summary>
    /// Infinity Engine module loader implementation for Baldur's Gate, Icewind Dale, and Planescape: Torment.
    /// </summary>
    /// <remarks>
    /// Module Loading Process:
    /// - Based on Infinity Engine module loading system (Baldur's Gate, Icewind Dale, Planescape: Torment)
    /// - Infinity Engine uses different file formats than Odyssey/Aurora/Eclipse:
    ///   - ARE files (area files) contain area data, creatures, items, triggers
    ///   - WED files (world editor data) contain area geometry and walkmesh
    ///   - GAM files contain game state and party information
    ///   - BIF files (BioWare Infinity Format) contain game resources
    /// - Module structure: Infinity Engine uses a simpler module system than Odyssey
    ///   - Modules are typically single areas or collections of areas
    ///   - No IFO files - module information is embedded in ARE files
    /// - Cross-engine: Similar module loading patterns but different file formats
    ///   - Odyssey: IFO/LYT/VIS/GIT/ARE files
    ///   - Aurora: IFO/ARE files with hak files
    ///   - Eclipse: UnrealScript packages and .rim files
    ///   - Infinity: ARE/WED/GAM files with BIF archives
    /// - Inheritance: BaseEngineModule (Runtime.Games.Common) implements common module loading/unloading
    ///   - Infinity: InfinityModuleLoader : BaseEngineModule (Runtime.Games.Infinity) - Infinity-specific module file formats (ARE, WED, GAM)
    /// - Module loading order: ARE (area data) -> WED (area geometry/walkmesh) -> GAM (game state) -> BIF (resources)
    /// - Module state: Tracks current module name, current area, navigation mesh
    /// - TODO: Reverse engineer specific function addresses from Infinity Engine executables using Ghidra MCP
    ///   - Baldur's Gate: BaldurGate.exe module loading functions
    ///   - Icewind Dale: IcewindDale.exe module loading functions
    ///   - Planescape: Torment: PlanescapeTorment.exe module loading functions
    /// </remarks>
    public class InfinityModuleLoader : BaseEngineModule
    {
        public InfinityModuleLoader(IWorld world, IGameResourceProvider resourceProvider)
            : base(world, resourceProvider)
        {
        }

        public override async Task LoadModuleAsync(string moduleName, [CanBeNull] Action<float> progressCallback = null)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                throw new ArgumentException("Module name cannot be null or empty", nameof(moduleName));
            }

            progressCallback?.Invoke(0.0f);

            // Infinity Engine module loading process:
            // Based on Infinity Engine (Baldur's Gate, Icewind Dale, Planescape: Torment) module loading system
            // Infinity Engine uses different file formats than Odyssey/Aurora/Eclipse:
            // - ARE files (area files) contain area data, creatures, items, triggers
            // - WED files (world editor data) contain area geometry and walkmesh
            // - GAM files contain game state and party information
            // - BIF files (BioWare Infinity Format) contain game resources
            // Module structure: Infinity Engine uses a simpler module system than Odyssey
            // - Modules are typically single areas or collections of areas
            // - No IFO files - module information is embedded in ARE files

            try
            {
                // Step 1: Load ARE file (area data) - contains creatures, items, triggers, doors
                // Infinity Engine ARE files use binary format (different from Odyssey/Aurora GFF-based ARE)
                // Based on Infinity Engine (Baldur's Gate, Icewind Dale, Planescape: Torment) ARE file format
                byte[] areData = null;
                if (_resourceProvider != null)
                {
                    try
                    {
                        areData = await _resourceProvider.LoadResourceAsync(moduleName + ".are", ResourceType.ARE);
                    }
                    catch
                    {
                        // ARE file may not exist for some modules
                        areData = null;
                    }
                }

                if (areData == null || areData.Length == 0)
                {
                    throw new InvalidOperationException($"ARE file not found for module '{moduleName}'");
                }

                // Parse ARE file structure
                // Infinity Engine ARE files contain:
                // - Area header (name, dimensions, flags, etc.)
                // - Creature list (creatures in area)
                // - Item list (items in area)
                // - Trigger list (triggers in area)
                // - Door list (doors in area)
                // - Container list (containers in area)
                // - Script hooks (OnEnter, OnExit, OnHeartbeat, etc.)
                InfinityAreaData areaData = ParseAreaFile(areData, moduleName);
                progressCallback?.Invoke(0.2f);

                // Step 2: Load WED file (world editor data) - contains area geometry and walkmesh
                byte[] wedData = null;
                if (_resourceProvider != null)
                {
                    try
                    {
                        // WED files may use a different resource type or extension
                        // Try loading as binary data first
                        wedData = await _resourceProvider.LoadResourceAsync(moduleName + ".wed", ResourceType.Unknown);
                    }
                    catch
                    {
                        // WED file may not exist for some modules
                        wedData = null;
                    }
                }

                // Parse WED file structure (if available)
                // Infinity Engine WED files contain:
                // - Overlay list (background/foreground overlays)
                // - Door list (door polygons and animations)
                // - Animation list (area animations)
                // - Walkmesh data (navigation mesh vertices and faces)
                InfinityWedData wedFileData = null;
                if (wedData != null && wedData.Length > 0)
                {
                    wedFileData = ParseWedFile(wedData);
                }
                progressCallback?.Invoke(0.4f);

                // Step 3: Load GAM file (game state) - contains party information and global variables
                byte[] gamData = null;
                if (_resourceProvider != null)
                {
                    try
                    {
                        gamData = await _resourceProvider.LoadResourceAsync(moduleName + ".gam", ResourceType.GAM);
                    }
                    catch
                    {
                        // GAM file is optional (only exists for savegames)
                        gamData = null;
                    }
                }

                // Parse GAM file structure (if available)
                // Infinity Engine GAM files contain:
                // - Party members (character data)
                // - Global variables
                // - Journal entries
                // - Game time
                InfinityGamData gamFileData = null;
                if (gamData != null && gamData.Length > 0)
                {
                    gamFileData = ParseGamFile(gamData);
                }
                progressCallback?.Invoke(0.6f);

                // Step 4: Load resources from BIF files as needed
                // BIF files are already supported via existing BIF parser
                // Resources are loaded on-demand when needed (textures, models, sounds, etc.)
                // Based on Infinity Engine: BIF files contain game resources indexed by KEY files
                progressCallback?.Invoke(0.8f);

                // Step 5: Create area entity and add to world
                // Create InfinityArea entity from ARE/WED data
                InfinityArea area = CreateAreaFromData(areaData, wedFileData, moduleName);
                
                // Set up navigation mesh from WED walkmesh data
                if (wedFileData != null && wedFileData.Walkmesh != null)
                {
                    area.NavigationMesh = CreateNavigationMesh(wedFileData.Walkmesh);
                }

                // Add area to world
                _world.SetCurrentArea(area);

                // Spawn creatures, items, triggers from ARE file
                SpawnAreaEntities(areaData, area);

                // Set up area properties (name, tag, script hooks, etc.)
                area.Tag = areaData.Tag;
                area.Name = areaData.Name;

                // Store module name
            _currentModuleName = moduleName;

            progressCallback?.Invoke(1.0f);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[InfinityModuleLoader] Error loading module '{moduleName}': {ex.Message}");
                throw;
            }
        }

        public override bool HasModule(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                return false;
            }

            // Infinity Engine module existence check
            // Infinity Engine modules are typically identified by ARE file names
            // Module structure: {moduleName}.are file must exist
            // TODO: PLACEHOLDER - Infinity Engine resource provider integration not yet implemented
            // Full implementation should:
            // 1. Check if ARE file exists: {moduleName}.are
            // 2. Optionally check if WED file exists: {moduleName}.wed
            // 3. Return true if ARE file exists (required for module loading)

            try
            {
                // Check if ARE file exists via resource provider
                // Infinity Engine modules require ARE file to exist
                if (_resourceProvider != null)
                {
                    // Check if ARE file exists
                    try
                    {
                        byte[] areData = _resourceProvider.LoadResourceAsync(moduleName + ".are", ResourceType.ARE).Result;
                        return areData != null && areData.Length > 0;
                    }
                    catch
                    {
                        return false;
                    }
                }
                return false;
            }
            catch
            {
            return false;
            }
        }

        protected override void OnUnloadModule()
        {
            // Infinity Engine module unloading
            // Based on Infinity Engine (Baldur's Gate, Icewind Dale, Planescape: Torment) module unloading system
            // Full implementation should:
            // 1. Remove area entities from world
            // 2. Dispose navigation mesh
            // 3. Clear cached resources (ARE, WED, GAM data)
            // 4. Reset module state

            try
            {
                // Clear current area if set
                if (_world.CurrentArea != null)
                {
                    // Remove area from world
                    // TODO: PLACEHOLDER - Area removal not yet implemented
                    // Full implementation should remove area entity from world
                }

                // Clear module state
                _currentModuleName = null;

                // TODO: PLACEHOLDER - Resource cleanup not yet implemented
                // Full implementation should:
                // 1. Dispose navigation mesh
                // 2. Clear cached ARE/WED/GAM data
                // 3. Release BIF file resources
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[InfinityModuleLoader] Error unloading module: {ex.Message}");
            }
        }

        /// <summary>
        /// Parses Infinity Engine ARE file data.
        /// Based on Infinity Engine ARE file format (Baldur's Gate, Icewind Dale, Planescape: Torment).
        /// </summary>
        private InfinityAreaData ParseAreaFile(byte[] data, string moduleName)
        {
            // Infinity Engine ARE file format:
            // - Binary format (not GFF-based like Odyssey/Aurora)
            // - Header contains area name, dimensions, flags
            // - Lists of creatures, items, triggers, doors, containers
            // - Script hooks (OnEnter, OnExit, OnHeartbeat, etc.)
            
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                var areaData = new InfinityAreaData();
                
                // Read ARE file header
                // Infinity Engine ARE header structure (simplified - full format is more complex):
                // - Signature: "AREA V1.0" or similar
                // - Area name (string)
                // - Dimensions (width, height)
                // - Flags (interior, underground, natural, etc.)
                
                // Read signature (first 8 bytes typically contain version info)
                string signature = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd('\0');
                
                // Read area name (typically 32 bytes)
                areaData.Name = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(32)).TrimEnd('\0');
                if (string.IsNullOrEmpty(areaData.Name))
                {
                    areaData.Name = moduleName;
                }
                
                // Read area tag (typically 32 bytes)
                areaData.Tag = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(32)).TrimEnd('\0');
                if (string.IsNullOrEmpty(areaData.Tag))
                {
                    areaData.Tag = moduleName;
                }
                
                // Read dimensions (width, height in tiles)
                areaData.Width = reader.ReadInt32();
                areaData.Height = reader.ReadInt32();
                
                // Read flags (interior, underground, natural, etc.)
                areaData.Flags = reader.ReadUInt32();
                
                // Read creature count and list
                int creatureCount = reader.ReadInt32();
                areaData.Creatures = new List<InfinityCreatureData>();
                for (int i = 0; i < creatureCount; i++)
                {
                    var creature = new InfinityCreatureData();
                    creature.ResRef = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd('\0');
                    creature.X = reader.ReadSingle();
                    creature.Y = reader.ReadSingle();
                    creature.Z = reader.ReadSingle();
                    creature.Facing = reader.ReadSingle();
                    areaData.Creatures.Add(creature);
                }
                
                // Read item count and list
                int itemCount = reader.ReadInt32();
                areaData.Items = new List<InfinityItemData>();
                for (int i = 0; i < itemCount; i++)
                {
                    var item = new InfinityItemData();
                    item.ResRef = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd('\0');
                    item.X = reader.ReadSingle();
                    item.Y = reader.ReadSingle();
                    item.Z = reader.ReadSingle();
                    areaData.Items.Add(item);
                }
                
                // Read trigger count and list
                int triggerCount = reader.ReadInt32();
                areaData.Triggers = new List<InfinityTriggerData>();
                for (int i = 0; i < triggerCount; i++)
                {
                    var trigger = new InfinityTriggerData();
                    trigger.Name = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(32)).TrimEnd('\0');
                    trigger.X = reader.ReadSingle();
                    trigger.Y = reader.ReadSingle();
                    trigger.Z = reader.ReadSingle();
                    trigger.Width = reader.ReadSingle();
                    trigger.Height = reader.ReadSingle();
                    areaData.Triggers.Add(trigger);
                }
                
                // Read door count and list
                int doorCount = reader.ReadInt32();
                areaData.Doors = new List<InfinityDoorData>();
                for (int i = 0; i < doorCount; i++)
                {
                    var door = new InfinityDoorData();
                    door.ResRef = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd('\0');
                    door.X = reader.ReadSingle();
                    door.Y = reader.ReadSingle();
                    door.Z = reader.ReadSingle();
                    door.Facing = reader.ReadSingle();
                    areaData.Doors.Add(door);
                }
                
                // Read script hooks (OnEnter, OnExit, OnHeartbeat, etc.)
                areaData.OnEnterScript = ReadString(reader, 16);
                areaData.OnExitScript = ReadString(reader, 16);
                areaData.OnHeartbeatScript = ReadString(reader, 16);
                
                return areaData;
            }
        }

        /// <summary>
        /// Parses Infinity Engine WED file data.
        /// Based on Infinity Engine WED file format (Baldur's Gate, Icewind Dale, Planescape: Torment).
        /// </summary>
        private InfinityWedData ParseWedFile(byte[] data)
        {
            // Infinity Engine WED file format:
            // - Binary format containing area geometry and walkmesh
            // - Overlay list (background/foreground overlays)
            // - Door list (door polygons and animations)
            // - Animation list (area animations)
            // - Walkmesh data (navigation mesh vertices and faces)
            
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                var wedData = new InfinityWedData();
                
                // Read WED file header
                // Infinity Engine WED header structure:
                // - Signature: "WED V1.0" or similar
                // - Overlay count
                // - Door count
                // - Animation count
                // - Walkmesh data offset
                
                string signature = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd('\0');
                
                // Read overlay count and list
                int overlayCount = reader.ReadInt32();
                wedData.Overlays = new List<InfinityOverlayData>();
                for (int i = 0; i < overlayCount; i++)
                {
                    var overlay = new InfinityOverlayData();
                    overlay.ResRef = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd('\0');
                    overlay.X = reader.ReadInt32();
                    overlay.Y = reader.ReadInt32();
                    wedData.Overlays.Add(overlay);
                }
                
                // Read walkmesh data
                // Infinity Engine walkmesh structure:
                // - Vertex count and list
                // - Face count and list (triangles)
                int vertexCount = reader.ReadInt32();
                wedData.Walkmesh = new InfinityWalkmeshData();
                wedData.Walkmesh.Vertices = new List<Vector3>();
                for (int i = 0; i < vertexCount; i++)
                {
                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    float z = reader.ReadSingle();
                    wedData.Walkmesh.Vertices.Add(new Vector3(x, y, z));
                }
                
                int faceCount = reader.ReadInt32();
                wedData.Walkmesh.Faces = new List<InfinityWalkmeshFace>();
                for (int i = 0; i < faceCount; i++)
                {
                    var face = new InfinityWalkmeshFace();
                    face.Vertex0 = reader.ReadInt32();
                    face.Vertex1 = reader.ReadInt32();
                    face.Vertex2 = reader.ReadInt32();
                    wedData.Walkmesh.Faces.Add(face);
                }
                
                return wedData;
            }
        }

        /// <summary>
        /// Parses Infinity Engine GAM file data.
        /// Based on Infinity Engine GAM file format (Baldur's Gate, Icewind Dale, Planescape: Torment).
        /// </summary>
        private InfinityGamData ParseGamFile(byte[] data)
        {
            // Infinity Engine GAM file format:
            // - Binary format containing game state
            // - Party members (character data)
            // - Global variables
            // - Journal entries
            // - Game time
            
            using (var reader = new BinaryReader(new MemoryStream(data)))
            {
                var gamData = new InfinityGamData();
                
                // Read GAM file header
                // Infinity Engine GAM header structure:
                // - Signature: "GAME V1.0" or similar
                // - Party member count
                // - Global variable count
                // - Journal entry count
                
                string signature = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd('\0');
                
                // Read party member count and list
                int partyCount = reader.ReadInt32();
                gamData.PartyMembers = new List<InfinityPartyMemberData>();
                for (int i = 0; i < partyCount; i++)
                {
                    var member = new InfinityPartyMemberData();
                    member.Name = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(32)).TrimEnd('\0');
                    member.ResRef = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(8)).TrimEnd('\0');
                    gamData.PartyMembers.Add(member);
                }
                
                // Read global variable count and list
                int globalVarCount = reader.ReadInt32();
                gamData.GlobalVariables = new Dictionary<string, int>();
                for (int i = 0; i < globalVarCount; i++)
                {
                    string name = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(32)).TrimEnd('\0');
                    int value = reader.ReadInt32();
                    gamData.GlobalVariables[name] = value;
                }
                
                // Read journal entry count and list
                int journalCount = reader.ReadInt32();
                gamData.JournalEntries = new List<InfinityJournalEntry>();
                for (int i = 0; i < journalCount; i++)
                {
                    var entry = new InfinityJournalEntry();
                    entry.Text = System.Text.Encoding.ASCII.GetString(reader.ReadBytes(256)).TrimEnd('\0');
                    entry.Completed = reader.ReadBoolean();
                    gamData.JournalEntries.Add(entry);
                }
                
                return gamData;
            }
        }

        /// <summary>
        /// Creates an InfinityArea from parsed ARE/WED data.
        /// </summary>
        private IArea CreateAreaFromData(InfinityAreaData areaData, InfinityWedData wedData, string moduleName)
        {
            // Create area using base area class
            // Infinity Engine areas are simpler than Odyssey/Aurora - single area per module
            var area = new InfinityArea(moduleName);
            area.Tag = areaData.Tag;
            area.Name = areaData.Name;
            return area;
        }

        /// <summary>
        /// Creates a navigation mesh from WED walkmesh data.
        /// </summary>
        private INavigationMesh CreateNavigationMesh(InfinityWalkmeshData walkmesh)
        {
            // Create navigation mesh from walkmesh vertices and faces
            // Based on Infinity Engine walkmesh structure
            // Convert walkmesh triangles to navigation mesh structure
            
            if (walkmesh == null || walkmesh.Vertices == null || walkmesh.Faces == null)
            {
                return null;
            }
            
            // Create basic navigation mesh from walkmesh
            // Full implementation would use proper navigation mesh builder
            // For now, create a simple mesh from the walkmesh data
            var navMesh = new InfinityNavigationMesh();
            navMesh.InitializeFromWalkmesh(walkmesh.Vertices, walkmesh.Faces);
            return navMesh;
        }

        /// <summary>
        /// Spawns entities (creatures, items, triggers, doors) from ARE file data.
        /// </summary>
        private void SpawnAreaEntities(InfinityAreaData areaData, IArea area)
        {
            // Spawn creatures
            if (areaData.Creatures != null)
            {
                foreach (var creatureData in areaData.Creatures)
                {
                    // Create creature entity from ResRef
                    // Full implementation would use entity factory to create creatures
                    // For now, just log the creature data
                    Console.WriteLine($"[InfinityModuleLoader] Spawning creature: {creatureData.ResRef} at ({creatureData.X}, {creatureData.Y}, {creatureData.Z})");
                }
            }
            
            // Spawn items
            if (areaData.Items != null)
            {
                foreach (var itemData in areaData.Items)
                {
                    // Create item entity from ResRef
                    Console.WriteLine($"[InfinityModuleLoader] Spawning item: {itemData.ResRef} at ({itemData.X}, {itemData.Y}, {itemData.Z})");
                }
            }
            
            // Spawn triggers
            if (areaData.Triggers != null)
            {
                foreach (var triggerData in areaData.Triggers)
                {
                    // Create trigger entity
                    Console.WriteLine($"[InfinityModuleLoader] Spawning trigger: {triggerData.Name} at ({triggerData.X}, {triggerData.Y}, {triggerData.Z})");
                }
            }
            
            // Spawn doors
            if (areaData.Doors != null)
            {
                foreach (var doorData in areaData.Doors)
                {
                    // Create door entity from ResRef
                    Console.WriteLine($"[InfinityModuleLoader] Spawning door: {doorData.ResRef} at ({doorData.X}, {doorData.Y}, {doorData.Z})");
                }
            }
        }

        /// <summary>
        /// Reads a null-terminated string from binary reader.
        /// </summary>
        private string ReadString(BinaryReader reader, int maxLength)
        {
            byte[] bytes = reader.ReadBytes(maxLength);
            int nullIndex = Array.IndexOf(bytes, (byte)0);
            if (nullIndex >= 0)
            {
                return System.Text.Encoding.ASCII.GetString(bytes, 0, nullIndex);
            }
            return System.Text.Encoding.ASCII.GetString(bytes).TrimEnd('\0');
        }
    }

    // Infinity Engine data structures
    internal class InfinityAreaData
    {
        public string Name;
        public string Tag;
        public int Width;
        public int Height;
        public uint Flags;
        public List<InfinityCreatureData> Creatures;
        public List<InfinityItemData> Items;
        public List<InfinityTriggerData> Triggers;
        public List<InfinityDoorData> Doors;
        public string OnEnterScript;
        public string OnExitScript;
        public string OnHeartbeatScript;
    }

    internal class InfinityCreatureData
    {
        public string ResRef;
        public float X;
        public float Y;
        public float Z;
        public float Facing;
    }

    internal class InfinityItemData
    {
        public string ResRef;
        public float X;
        public float Y;
        public float Z;
    }

    internal class InfinityTriggerData
    {
        public string Name;
        public float X;
        public float Y;
        public float Z;
        public float Width;
        public float Height;
    }

    internal class InfinityDoorData
    {
        public string ResRef;
        public float X;
        public float Y;
        public float Z;
        public float Facing;
    }

    internal class InfinityWedData
    {
        public List<InfinityOverlayData> Overlays;
        public InfinityWalkmeshData Walkmesh;
    }

    internal class InfinityOverlayData
    {
        public string ResRef;
        public int X;
        public int Y;
    }

    internal class InfinityWalkmeshData
    {
        public List<Vector3> Vertices;
        public List<InfinityWalkmeshFace> Faces;
    }

    internal class InfinityWalkmeshFace
    {
        public int Vertex0;
        public int Vertex1;
        public int Vertex2;
    }

    internal class InfinityGamData
    {
        public List<InfinityPartyMemberData> PartyMembers;
        public Dictionary<string, int> GlobalVariables;
        public List<InfinityJournalEntry> JournalEntries;
    }

    internal class InfinityPartyMemberData
    {
        public string Name;
        public string ResRef;
    }

    internal class InfinityJournalEntry
    {
        public string Text;
        public bool Completed;
    }

    /// <summary>
    /// Infinity Engine area implementation.
    /// Based on Infinity Engine area structure (Baldur's Gate, Icewind Dale, Planescape: Torment).
    /// </summary>
    internal class InfinityArea : BaseArea
    {
        private readonly string _resRef;
        private string _tag;
        private string _name;
        private INavigationMesh _navigationMesh;
        private readonly List<IEntity> _creatures;
        private readonly List<IEntity> _placeables;
        private readonly List<IEntity> _doors;
        private readonly List<IEntity> _triggers;
        private readonly List<IEntity> _waypoints;
        private readonly List<IEntity> _sounds;

        public InfinityArea(string moduleName)
        {
            _resRef = moduleName;
            _tag = moduleName;
            _name = moduleName;
            _creatures = new List<IEntity>();
            _placeables = new List<IEntity>();
            _doors = new List<IEntity>();
            _triggers = new List<IEntity>();
            _waypoints = new List<IEntity>();
            _sounds = new List<IEntity>();
        }

        public override string ResRef => _resRef;
        public override string DisplayName => _name;
        public override string Tag
        {
            get => _tag;
            set => _tag = value ?? _resRef;
        }
        public string Name
        {
            get => _name;
            set => _name = value ?? _resRef;
        }
        public override INavigationMesh NavigationMesh
        {
            get => _navigationMesh;
            set => _navigationMesh = value;
        }
        public override IEnumerable<IEntity> Creatures => _creatures;
        public override IEnumerable<IEntity> Placeables => _placeables;
        public override IEnumerable<IEntity> Doors => _doors;
        public override IEnumerable<IEntity> Triggers => _triggers;
        public override IEnumerable<IEntity> Waypoints => _waypoints;
        public override IEnumerable<IEntity> Sounds => _sounds;
        public override bool IsUnescapable { get; set; }
        public override bool StealthXPEnabled { get; set; }

        public override IEntity GetObjectByTag(string tag, int nth = 0)
        {
            // Search all entity lists for matching tag
            int count = 0;
            foreach (var entity in _creatures)
            {
                if (entity.Tag == tag && count++ == nth) return entity;
            }
            foreach (var entity in _placeables)
            {
                if (entity.Tag == tag && count++ == nth) return entity;
            }
            foreach (var entity in _doors)
            {
                if (entity.Tag == tag && count++ == nth) return entity;
            }
            return null;
        }

        public override bool IsPointWalkable(Vector3 point)
        {
            if (_navigationMesh == null) return false;
            return _navigationMesh.IsWalkable(_navigationMesh.FindFaceAt(point));
        }

        public override bool ProjectToWalkmesh(Vector3 point, out Vector3 result, out float height)
        {
            result = point;
            height = point.Y;
            if (_navigationMesh == null) return false;
            int faceIndex = _navigationMesh.FindFaceAt(point);
            if (faceIndex < 0) return false;
            result = _navigationMesh.GetFaceCenter(faceIndex);
            height = result.Y;
            return true;
        }

        protected override void LoadAreaProperties(byte[] gffData)
        {
            // Infinity Engine areas don't use GFF format
            // Properties are loaded from ARE file directly
        }

        protected override byte[] SaveAreaProperties()
        {
            // Infinity Engine areas don't use GFF format
            return new byte[0];
        }

        protected override void LoadEntities(byte[] gitData)
        {
            // Infinity Engine entities are loaded from ARE file
            // This is handled by InfinityModuleLoader.SpawnAreaEntities
        }

        protected override void LoadAreaGeometry(byte[] areData)
        {
            // Infinity Engine area geometry is loaded from WED file
            // This is handled by InfinityModuleLoader
        }

        protected override void InitializeAreaEffects()
        {
            // Initialize area-specific effects (lighting, fog, etc.)
        }

        protected override void RemoveEntityFromArea(IEntity entity)
        {
            if (entity == null) return;
            _creatures.Remove(entity);
            _placeables.Remove(entity);
            _doors.Remove(entity);
            _triggers.Remove(entity);
            _waypoints.Remove(entity);
            _sounds.Remove(entity);
            entity.AreaId = 0;
        }

        protected override void AddEntityToArea(IEntity entity)
        {
            if (entity == null) return;
            // Entity type determines which list it goes into
            // This is simplified - full implementation would check entity type
            _creatures.Add(entity);
            // AreaId is set by the entity's area assignment
            // Infinity Engine areas use ResRef as identifier
        }
    }
}

