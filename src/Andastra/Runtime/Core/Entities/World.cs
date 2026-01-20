using System;
using System.Collections.Generic;
using System.Numerics;
using Andastra.Runtime.Core.Actions;
using Andastra.Runtime.Games.Common;
using Andastra.Runtime.Core.Animation;
using Andastra.Runtime.Core.Combat;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Module;
using Andastra.Runtime.Core.Perception;c
using Andastra.Runtime.Core.Templates;
using Andastra.Runtime.Core.Triggers;

namespace Andastra.Runtime.Core.Entities
{
    /// <summary>
    /// Odyssey Engine world implementation.
    /// </summary>
    /// <remarks>
    /// Odyssey World/Entity Management:
    /// - [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address) world management system
    /// - Located via string references: "ObjectId" @ 0x007bce5c, "ObjectIDList" @ 0x007bfd7c
    /// - "AreaId" @ 0x007bef48 (entity area association), "Area" @ 0x007be340 (area name)
    /// - Object logging format: "OID: %08x, Tag: %s, %s" @ 0x007c76b8 used for debug/error logging
    /// - Original engine maintains entity lists by ObjectId, Tag, and ObjectType
    /// - Entity lookup: GetEntityByTag searches by tag string (case-insensitive), GetEntity by ObjectId (O(1) lookup)
    /// - ObjectId is unique 32-bit identifier assigned sequentially (OBJECT_INVALID = 0x7F000000, OBJECT_SELF = 0x7F000001)
    /// - Entity registration: Entities are registered in world with ObjectId, Tag, and ObjectType indices
    /// - Entity serialization: 0x005226d0 @ 0x005226d0 saves entity state including ObjectId, AreaId, Position, Orientation
    ///   - Function signature: `void 0x005226d0(void *param_1, void *param_2)`
    ///   - param_1: Entity pointer
    ///   - param_2: GFF structure pointer
    ///   - Writes ObjectId (uint32) via 0x00413880 with "ObjectId" field name
    ///   - Writes AreaId (uint32) via 0x00413880 with "AreaId" field name
    ///   - Writes Position (XPosition, YPosition, ZPosition as float) via 0x00413a00
    ///   - Writes Orientation (XOrientation, YOrientation, ZOrientation as float) via 0x00413a00
    ///   - Calls 0x00508200 to save action queue, 0x00505db0 to save effect list
    /// - Entity deserialization: 0x005223a0 @ 0x005223a0 loads entity data from GFF
    ///   - Reads ObjectId (uint32) via 0x00412d40 with "ObjectId" field name (default 0x7f000000)
    ///   - Reads AreaId (uint32) via 0x00412d40 with "AreaId" field name
    ///   - Reads Position and Orientation from GFF structure
    /// - Area management: Entities belong to areas (AreaId field), areas contain entity lists by type
    /// - Module management: "ModuleList" @ 0x007bdd3c, "ModuleName" @ 0x007bde2c, "LASTMODULE" @ 0x007be1d0
    /// - Module events: "CSWSSCRIPTEVENT_EVENTTYPE_ON_MODULE_LOAD" @ 0x007bc91c, "CSWSSCRIPTEVENT_EVENTTYPE_ON_MODULE_START" @ 0x007bc948
    /// - Note: This is a standalone implementation in Core. Engine-specific implementations
    ///   (e.g., OdysseyWorld) should inherit from BaseWorld in Runtime.Games.Common
    /// </remarks>
    public class World : IWorld
    {
        private readonly Dictionary<uint, IEntity> _entitiesById;
        private readonly Dictionary<uint, IArea> _areasById;
        private readonly Dictionary<string, List<IEntity>> _entitiesByTag;
        private readonly Dictionary<ObjectType, List<IEntity>> _entitiesByType;

        // [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): Tag lookup is case-insensitive
        // Located via string references: "GetObjectByTag" function uses case-insensitive tag comparison
        // Original implementation: Tag matching ignores case differences
        private static readonly StringComparer TagComparer = StringComparer.OrdinalIgnoreCase;
        private readonly List<IEntity> _allEntities;

        // Area ID assignment: Areas get sequential IDs starting from 0x7F000010
        // [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): AreaId @ 0x007bef48, areas are objects with ObjectIds
        // Located via string references: "AreaId" @ 0x007bef48 (entity area association)
        // Original implementation: Areas assigned ObjectIds similar to entities
        // Area ObjectId range: 0x7F000010+ (special object ID range for areas)
        private static uint _nextAreaId = 0x7F000010;
        private readonly Dictionary<IArea, uint> _areaIds;

        // Module ID assignment: Modules get a fixed ObjectId of 0x7F000002
        // [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): Module object ID constant
        // Located via string references: "GetModule" NWScript function, module object references
        // Original implementation: Modules are special objects with fixed ObjectId (0x7F000002)
        // Module ObjectId: 0x7F000002 (special object ID for module, between OBJECT_SELF 0x7F000001 and area IDs 0x7F000010+)
        // Common across all engines: Odyssey, Aurora, Eclipse, Infinity all use fixed module object ID
        public const uint ModuleObjectId = 0x7F000002;
        private Runtime.Core.Interfaces.IModule _registeredModule;

        public World(ITimeManager timeManager)
        {
            if (timeManager == null)
            {
                throw new ArgumentNullException("timeManager");
            }

            _entitiesById = new Dictionary<uint, IEntity>();
            _areasById = new Dictionary<uint, IArea>();
            _entitiesByTag = new Dictionary<string, List<IEntity>>(TagComparer);
            _entitiesByType = new Dictionary<ObjectType, List<IEntity>>();
            _allEntities = new List<IEntity>();
            _areaIds = new Dictionary<IArea, uint>();
            TimeManager = timeManager;
            EventBus = new EventBus();
            DelayScheduler = new DelayScheduler();
            CombatSystem = new CombatSystem(this);
            EffectSystem = new EffectSystem(this);
            PerceptionSystem = new PerceptionSystem(this);
            TriggerSystem = new TriggerSystem(this, (entity, scriptEvent, target) => EventBus.FireScriptEvent(entity, scriptEvent, target));
            AIController = new AIControllerSystem(
                this,
                EngineFamily.Unknown,
                (entity, scriptEvent, target) =>
                {
                    EventBus?.FireScriptEvent(entity, scriptEvent, target);
                });
            AnimationSystem = new AnimationSystem(this);
            AppearAnimationFadeSystem = new AppearAnimationFadeSystem(this);
            // ModuleTransitionSystem will be initialized when SaveSystem and ModuleLoader are available
            ModuleTransitionSystem = null;
        }

        public IArea CurrentArea { get; set; }
        public Runtime.Core.Interfaces.IModule CurrentModule { get; set; }

        /// <summary>
        /// Sets the current area.
        /// </summary>
        public void SetCurrentArea(IArea area)
        {
            CurrentArea = area;
            // Register area if not already registered
            if (area != null)
            {
                RegisterArea(area);
            }
        }

        /// <summary>
        /// Registers an area with the world and assigns it an AreaId.
        /// </summary>
        /// <remarks>
        /// [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): Area registration system
        /// Located via string references: "AreaId" @ 0x007bef48
        /// Original implementation: Areas are registered in world with AreaId for entity lookup
        /// Area ObjectId assignment: Sequential uint32 starting from 0x7F000010 (special object ID range)
        /// Areas can be looked up by AreaId to find which area an entity belongs to
        /// </remarks>
        public void RegisterArea(IArea area)
        {
            if (area == null)
            {
                throw new ArgumentNullException("area");
            }

            // If area is already registered, don't re-register
            if (_areaIds.ContainsKey(area))
            {
                return;
            }

            // Assign AreaId to area
            uint areaId = _nextAreaId++;
            _areaIds[area] = areaId;
            _areasById[areaId] = area;
        }

        /// <summary>
        /// Unregisters an area from the world.
        /// </summary>
        /// <remarks>
        /// [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): Area unregistration system
        /// Removes area from lookup tables when area is unloaded
        /// </remarks>
        public void UnregisterArea(IArea area)
        {
            if (area == null)
            {
                return;
            }

            if (_areaIds.TryGetValue(area, out uint areaId))
            {
                _areaIds.Remove(area);
                _areasById.Remove(areaId);
            }
        }

        /// <summary>
        /// Gets an area by its AreaId.
        /// </summary>
        /// <remarks>
        /// GetArea function - Area lookup by AreaId (ObjectId)
        ///
        /// Reverse Engineering Analysis:
        /// - Function name: [TODO: Find function name in binaries via reverse engineering]
        ///   - Possible names: GetArea, GetAreaByObjectId, GetAreaById, or may use GetObject directly
        ///   - Areas are objects with ObjectIds (0x7F000010+), so GetArea may use GetObject mechanism
        /// - K1 executables (Odyssey Engine): [TODO: Find function address via reverse engineering]
        ///   - Use list-project-files in ReVa MCP to identify available K1 program variants
        ///   - Search strategy: Cross-reference "AreaId" string, find functions that lookup by AreaId
        ///   - Alternative: Check if GetObject handles area ObjectIds directly
        /// - TSL executables (Odyssey Engine): [TODO: Find function address via reverse engineering]
        ///   - Use list-project-files in ReVa MCP to identify available TSL program variants
        ///   - Search strategy: Cross-reference "AreaId" @ 0x007bef48, find hash table lookup functions
        ///   - Known: "AreaId" string reference @ 0x007bef48 (data address in one TSL variant)
        ///   - Known: GetObject uses wrapper → lookup pattern (addresses vary by executable variant)
        ///   - Pattern: May follow similar wrapper→lookup pattern for areas
        ///
        /// Located via string references:
        /// - "AreaId" @ 0x007bef48 (TSL executable variant) - Data address for AreaId field in entity/area structures
        /// - "Area" @ 0x007be340 (TSL executable variant) - Area name string reference
        /// - "AreaId" field used in entity serialization and deserialization (addresses vary by executable variant)
        ///
        /// Original Implementation Details:
        /// - Data structure: Areas stored in hash table/dictionary keyed by AreaId (uint32)
        /// - Lookup algorithm: O(1) hash table lookup by AreaId (uint32)
        /// - Return value: Area pointer if found, null/0 if AreaId not found
        /// - AreaId range: 0x7F000010+ (special object ID range for areas, sequential assignment)
        /// - Area ObjectId assignment: Sequential uint32 starting from 0x7F000010
        ///
        /// Related Functions:
        /// - RegisterArea: Assigns AreaId to area when area is loaded/registered
        /// - GetAreaId: Returns AreaId for a given area instance (reverse lookup)
        /// - GetArea NWScript function: Uses this to find area containing an entity
        ///   - NWScript GetArea(object oTarget) implementation:
        ///     1. Gets entity by ObjectId (via GetObject - addresses vary by executable variant)
        ///     2. Reads entity's AreaId field (offset in entity structure, referenced by "AreaId" string)
        ///     3. Calls GetArea(AreaId) to lookup area by AreaId
        ///     4. Returns AreaId as object ID (or OBJECT_INVALID if not found)
        ///   - Note: Function addresses and data offsets vary between executable variants
        ///
        /// Data Structure Analysis:
        /// - Areas are stored in a hash table/dictionary: Dictionary&lt;uint, Area*&gt; or similar structure
        /// - Key: AreaId (uint32) - unique identifier for each area
        /// - Value: Area pointer/object
        /// - Registration: Areas registered via RegisterArea when loaded or set as current area
        /// - Unregistration: Areas removed from lookup table when unloaded via UnregisterArea
        ///
        /// Usage Patterns:
        /// - Called by GetArea NWScript function to find area containing an entity
        /// - Used internally when entity's AreaId is known and area reference is needed
        /// - Used for area transitions and entity area association lookups
        ///
        /// Implementation Notes:
        /// - This implementation uses Dictionary&lt;uint, IArea&gt; for O(1) lookup
        /// - Matches original engine behavior: O(1) hash table lookup
        /// - Returns null if AreaId not found (matches original: returns null/0)
        /// - Thread safety: Not thread-safe (original engine is single-threaded)
        ///
        /// Reverse Engineering Guide (for finding function addresses):
        /// 1. Use ReVa MCP list-project-files to identify available K1 and TSL executable variants in Ghidra project
        ///    - Different variants exist for different platforms (Windows, macOS, Linux) and builds
        ///    - Document which variant(s) you're analyzing
        /// 2. Open identified executable(s) in Ghidra and search for string "AreaId"
        ///    - Find data address(es) for "AreaId" (addresses vary by executable variant)
        ///    - Example: One TSL variant has "AreaId" @ 0x007bef48
        /// 3. Find cross-references to "AreaId" string or data address(es)
        /// 4. Look for functions that:
        ///    a. Take uint32 parameter (AreaId/ObjectId)
        ///    b. Perform hash table/dictionary lookup
        ///    c. Return area pointer or null
        /// 5. Check if GetObject handles area ObjectIds directly
        ///    - Areas have ObjectIds in 0x7F000010+ range
        ///    - GetObject addresses vary by executable variant
        ///    - If GetObject handles all ObjectIds, GetArea may just be GetObject
        /// 6. Alternative: Search for functions called by GetArea NWScript implementation
        ///    - NWScript GetArea calls: GetObject → read AreaId → GetArea(AreaId)
        ///    - Find the function that does the final AreaId → Area lookup
        /// 7. Document findings: Function name, addresses per executable variant, parameters, return type
        /// 8. Add labels, comments, function names, structures in Ghidra project using ReVa MCP
        ///    - Ensure documentation reflects the specific executable variant(s) analyzed
        ///
        /// Cross-Engine Compatibility:
        /// - Common across all BioWare engines: Odyssey, Aurora, Eclipse, Infinity
        /// - All engines use AreaId-based lookup for area retrieval
        /// - AreaId assignment ranges may vary between engines but lookup mechanism is consistent
        ///
        /// See Also:
        /// - RegisterArea: Area registration and AreaId assignment
        /// - GetAreaId: Reverse lookup (area -&gt; AreaId)
        /// - BaseEngineApi.Func_GetArea: NWScript GetArea function implementation
        /// - Entity.AreaId: Entity's area association field
        /// </remarks>
        public IArea GetArea(uint areaId)
        {
            if (_areasById.TryGetValue(areaId, out IArea area))
            {
                return area;
            }
            return null;
        }

        /// <summary>
        /// Gets the AreaId for an area.
        /// </summary>
        /// <remarks>
        /// [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): AreaId lookup
        /// Returns the AreaId assigned to an area, or 0 if area is not registered
        /// </remarks>
        public uint GetAreaId(IArea area)
        {
            if (area == null)
            {
                return 0;
            }

            if (_areaIds.TryGetValue(area, out uint areaId))
            {
                return areaId;
            }
            return 0;
        }

        /// <summary>
        /// Gets all registered areas in the world.
        /// </summary>
        /// <remarks>
        /// [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): Area registration system
        /// Returns all areas that have been registered with the world via RegisterArea.
        /// Used by GetAreaByTag to search through all loaded areas.
        /// </remarks>
        public IEnumerable<IArea> GetAllAreas()
        {
            return _areasById.Values;
        }

        /// <summary>
        /// Sets the current module.
        /// </summary>
        /// <remarks>
        /// [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): Module registration system
        /// Located via string references: "Module" @ 0x007bc4e0, "ModuleName" @ 0x007bde2c, "ModuleLoaded" @ 0x007bdd70
        /// Original implementation: Module is registered when set as current module
        /// Module ObjectId: Fixed value 0x7F000002 (special object ID for module)
        /// Common across all engines: Modules are registered with fixed ObjectId when set as current
        /// </remarks>
        public void SetCurrentModule(Runtime.Core.Interfaces.IModule module)
        {
            CurrentModule = module;
            _registeredModule = module;
        }

        /// <summary>
        /// Gets the ModuleId for a module.
        /// </summary>
        /// <remarks>
        /// [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): Module object ID lookup
        /// Returns the ModuleId assigned to a module, or 0 if module is not registered
        /// Module ObjectId: Fixed value 0x7F000002 (special object ID for module)
        /// Common across all engines: Modules use fixed ObjectId (0x7F000002) for script references
        /// Used by GetModule NWScript function to return the module object ID
        /// </remarks>
        public uint GetModuleId(Runtime.Core.Interfaces.IModule module)
        {
            if (module == null)
            {
                return 0;
            }

            // Module is registered if it's the current module
            if (module == _registeredModule && module == CurrentModule)
            {
                return ModuleObjectId;
            }

            return 0;
        }
        public ITimeManager TimeManager { get; }
        public IEventBus EventBus { get; }
        public IDelayScheduler DelayScheduler { get; }
        public CombatSystem CombatSystem { get; }
        public EffectSystem EffectSystem { get; }
        public PerceptionSystem PerceptionSystem { get; }
        public TriggerSystem TriggerSystem { get; }
        public AIControllerSystem AIController { get; }
        public AnimationSystem AnimationSystem { get; }
        public AppearAnimationFadeSystem AppearAnimationFadeSystem { get; }
        public ModuleTransitionSystem ModuleTransitionSystem { get; }
        /// <summary>
        /// The game data provider for accessing engine-agnostic game data tables.
        /// </summary>
        /// <remarks>
        /// Provides access to game data tables (2DA files) for looking up creature properties,
        /// appearance data, and other game configuration data.
        /// Engine-specific implementations handle the actual table loading and lookup.
        /// This property should be set by engine-specific implementations.
        /// </remarks>
        public Interfaces.IGameDataProvider GameDataProvider { get; set; }

        public IEntity CreateEntity(IEntityTemplate template, Vector3 position, float facing)
        {
            if (template == null)
            {
                throw new ArgumentNullException("template");
            }

            return template.Spawn(this, position, facing);
        }

        public IEntity CreateEntity(ObjectType objectType, Vector3 position, float facing)
        {
            var entity = new Entity(objectType, this);
            entity.Position = position;
            entity.Facing = facing;
            RegisterEntity(entity);
            return entity;
        }

        /// <summary>
        /// Destroys an entity by its ObjectId.
        /// </summary>
        /// <remarks>
        /// [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): DestroyObject NWScript function
        /// Located via string references: "EVENT_DESTROY_OBJECT" @ 0x007bcd48 (destroy object event, case 0xb in 0x004dcfb0)
        /// Original implementation: Unregisters entity from world, fires destroy events, marks as invalid
        /// Destroy sequence: Fire OnDeath script event (for creatures), unregister from world indices, mark entity as invalid
        /// EVENT_DESTROY_OBJECT (case 0xb) is an object event logged by 0x004dcfb0, not a script event
        /// For creatures, OnDeath script event should fire before destruction (CSWSSCRIPTEVENT_EVENTTYPE_ON_DEATH @ 0x007bca54, case 0xa)
        /// </remarks>
        public void DestroyEntity(uint objectId)
        {
            IEntity entity = GetEntity(objectId);
            if (entity != null)
            {
                // Fire OnDeath script event for creatures before destruction
                // [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): OnDeath script fires before entity is destroyed
                // Located via string reference: "CSWSSCRIPTEVENT_EVENTTYPE_ON_DEATH" @ 0x007bca54 (case 0xa in 0x004dcfb0)
                if (EventBus != null && (entity.ObjectType & ObjectType.Creature) != 0)
                {
                    EventBus.FireScriptEvent(entity, ScriptEvent.OnDeath, null);
                }

                UnregisterEntity(entity);

                if (entity is Entity concreteEntity)
                {
                    concreteEntity.Destroy();
                }
            }
        }

        /// <summary>
        /// Gets an entity by its ObjectId.
        /// </summary>
        /// <remarks>
        /// [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): GetObject function
        /// Located via string references: "ObjectId" @ 0x007bce5c, "ObjectIDList" @ 0x007bfd7c
        /// Original implementation: 0x004dc030 @ 0x004dc030 (wrapper that calls 0x004e9de0)
        /// ObjectId lookup: O(1) dictionary lookup by ObjectId (uint32)
        /// Returns null if ObjectId not found (OBJECT_INVALID = 0x7F000000)
        /// </remarks>
        public IEntity GetEntity(uint objectId)
        {
            if (_entitiesById.TryGetValue(objectId, out IEntity entity))
            {
                return entity;
            }
            return null;
        }

        /// <summary>
        /// Gets an entity by its Tag string.
        /// </summary>
        /// <remarks>
        /// [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): GetObjectByTag NWScript function
        /// Located via string references: "GetObjectByTag" function uses case-insensitive tag comparison
        /// Original implementation: Tag lookup is case-insensitive, supports nth parameter for multiple entities with same tag
        /// Tag matching: Uses case-insensitive string comparison (StringComparer.OrdinalIgnoreCase)
        /// nth parameter: Returns nth entity with matching tag (0 = first, 1 = second, etc.)
        /// Returns null if tag not found or nth is out of range
        /// </remarks>
        public IEntity GetEntityByTag(string tag, int nth = 0)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return null;
            }

            if (_entitiesByTag.TryGetValue(tag, out List<IEntity> entities))
            {
                if (nth >= 0 && nth < entities.Count)
                {
                    return entities[nth];
                }
            }
            return null;
        }

        public IEnumerable<IEntity> GetEntitiesInRadius(Vector3 center, float radius, ObjectType typeMask = ObjectType.All)
        {
            float radiusSquared = radius * radius;
            var result = new List<IEntity>();

            foreach (IEntity entity in _allEntities)
            {
                if ((entity.ObjectType & typeMask) == 0)
                {
                    continue;
                }

                Interfaces.Components.ITransformComponent transform = entity.GetComponent<Interfaces.Components.ITransformComponent>();
                if (transform != null)
                {
                    float distSquared = Vector3.DistanceSquared(center, transform.Position);
                    if (distSquared <= radiusSquared)
                    {
                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        public IEnumerable<IEntity> GetEntitiesOfType(ObjectType type)
        {
            if (_entitiesByType.TryGetValue(type, out List<IEntity> entities))
            {
                return entities;
            }
            return new List<IEntity>();
        }

        public IEnumerable<IEntity> GetAllEntities()
        {
            return _allEntities;
        }

        public void RegisterEntity(IEntity entity)
        {
            // [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): Entity registration system
            // Located via string references: "ObjectId" @ 0x007bce5c, "ObjectIDList" @ 0x007bfd7c
            // Original implementation: Entities registered in world with ObjectId, Tag, and ObjectType indices
            // Entity lookup: GetEntity by ObjectId (O(1) lookup), GetEntityByTag searches by tag string (case-insensitive)
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (_entitiesById.ContainsKey(entity.ObjectId))
            {
                return; // Already registered
            }

            _entitiesById[entity.ObjectId] = entity;
            _allEntities.Add(entity);

            // Register by tag
            // Original engine: Entities indexed by tag for GetObjectByTag NWScript function
            if (!string.IsNullOrEmpty(entity.Tag))
            {
                if (!_entitiesByTag.TryGetValue(entity.Tag, out List<IEntity> tagList))
                {
                    tagList = new List<IEntity>();
                    _entitiesByTag[entity.Tag] = tagList;
                }
                tagList.Add(entity);
            }

            // Register by type
            // Original engine: Entities indexed by ObjectType for efficient type-based queries
            if (!_entitiesByType.TryGetValue(entity.ObjectType, out List<IEntity> typeList))
            {
                typeList = new List<IEntity>();
                _entitiesByType[entity.ObjectType] = typeList;
            }
            typeList.Add(entity);

            // Set entity's AreaId based on current area
            // [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): Entity AreaId assignment
            // Located via string references: "AreaId" @ 0x007bef48
            // Original implementation: Entities store AreaId of area they belong to
            // Entity AreaId is set when entity is registered to world
            if (CurrentArea != null)
            {
                uint areaId = GetAreaId(CurrentArea);
                if (areaId != 0)
                {
                    entity.AreaId = areaId;
                }
            }

            // Fire OnSpawn script event
            // [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): CSWSSCRIPTEVENT_EVENTTYPE_ON_SPAWN_IN fires when entity is spawned/created
            // Located via string references: "CSWSSCRIPTEVENT_EVENTTYPE_ON_SPAWN_IN" @ 0x007bc7d0 (0x8), "ScriptSpawn" @ 0x007bee30
            // Original implementation: OnSpawn script fires on entity when it's first created/spawned into the world
            // OnSpawn fires after entity is fully initialized and registered in the world
            if (EventBus != null)
            {
                EventBus.FireScriptEvent(entity, ScriptEvent.OnSpawn, null);
            }
        }

        public void UnregisterEntity(IEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            _entitiesById.Remove(entity.ObjectId);
            _allEntities.Remove(entity);

            // Remove from tag list
            if (!string.IsNullOrEmpty(entity.Tag))
            {
                if (_entitiesByTag.TryGetValue(entity.Tag, out List<IEntity> tagList))
                {
                    tagList.Remove(entity);
                }
            }

            // Remove from type list
            if (_entitiesByType.TryGetValue(entity.ObjectType, out List<IEntity> typeList))
            {
                typeList.Remove(entity);
            }
        }

        /// <summary>
        /// Updates all entities for a single frame.
        /// </summary>
        /// <remarks>
        /// [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): 0x00404cf0 @ 0x00404cf0 (area update function).
        /// Called from main game loop via 0x00638ca0 → 0x0063de50 → 0x0077f790 → 0x00404cf0.
        ///
        /// Execution flow (TSL executable: 0x00404250):
        /// 1. Main loop: while (DAT_00828390 == 0)
        /// 2. PeekMessageA() - Windows message processing
        /// 3. 0x00638ca0() - Game update (calls area update)
        /// 4. glClear() - Clear screen
        /// 5. 0x00461c20()/0x00461c00() - Render
        /// 6. SwapBuffers() - Present frame
        ///
        /// Area update is called every frame to update area state, effects, lighting, etc.
        /// </remarks>
        public void Update(float deltaTime)
        {
            TimeManager.Update(deltaTime);

            while (TimeManager.HasPendingTicks())
            {
                TimeManager.Tick();
                // Fixed update logic would go here
            }

            // Update delay scheduler
            DelayScheduler.Update(deltaTime);

            // Update perception system
            PerceptionSystem.Update(deltaTime);

            // Update trigger system
            TriggerSystem.Update(deltaTime);

            // Update AI controller
            AIController.Update(deltaTime);

            // Update animation system
            AnimationSystem.Update(deltaTime);

            // Update appear animation fade system
            AppearAnimationFadeSystem.Update(deltaTime);

            // Update combat system
            CombatSystem.Update(deltaTime);

            // Update current area (CRITICAL: Must be called every frame)
            // [TODO: Function name] @ (K1: TODO: Find this address, TSL: TODO: Find this address address): 0x00404cf0 @ 0x00404cf0 updates area state
            // Located via call chain: 0x00638ca0 → 0x0063de50 → 0x0077f790 → 0x00404cf0
            // Original implementation: Area update handles area effects, lighting, weather, entity spawning/despawning
            if (CurrentArea != null)
            {
                CurrentArea.Update(deltaTime);
            }

            EventBus.DispatchQueuedEvents();
        }
    }
}

