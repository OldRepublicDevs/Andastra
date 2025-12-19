using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Games.Infinity.Components;
using Andastra.Runtime.Games.Common;
using Andastra.Runtime.Games.Common.Components;

namespace Andastra.Runtime.Games.Infinity
{
    /// <summary>
    /// Infinity Engine (Mass Effect, Mass Effect 2) specific entity implementation.
    /// </summary>
    /// <remarks>
    /// Infinity Entity Implementation:
    /// - Based on MassEffect.exe (Mass Effect) and MassEffect2.exe (Mass Effect 2) entity systems
    /// - Implements ObjectId, Tag, ObjectType structure
    /// - Streamlined component-based architecture for modular functionality
    /// - Script hooks for events and behaviors
    ///
    /// Based on reverse engineering of:
    /// - MassEffect.exe: Entity creation and management
    /// - MassEffect2.exe: Enhanced entity system
    /// - ObjectId: Entity identification system (exact addresses to be determined via further reverse engineering)
    /// - Tag: Entity tag system for script lookups (exact addresses to be determined via further reverse engineering)
    /// - Entity structure: ObjectId (uint32), Tag (string), ObjectType (enum)
    /// - Component system: Streamlined transform, stats, inventory, script hooks, etc.
    ///
    /// Entity lifecycle:
    /// - Created from template files or script instantiation
    /// - Assigned sequential ObjectId for uniqueness
    /// - Components attached based on ObjectType
    /// - Registered with area and world systems
    /// - Updated each frame, destroyed when no longer needed
    ///
    /// Infinity-specific details:
    /// - Streamlined entity system compared to Odyssey/Aurora/Eclipse
    /// - Different property calculations and upgrade mechanics
    /// - Uses different character system (to be reverse engineered)
    /// - Different save/load format than other engines
    /// - Note: Infinity engine reverse engineering is in progress, exact function addresses to be added
    /// </remarks>
    [PublicAPI]
    public class InfinityEntity : BaseEntity
    {
        private uint _objectId;
        private string _tag;
        private readonly ObjectType _objectType;
        private IWorld _world;
        private bool _isValid = true;
        private uint _areaId;

        /// <summary>
        /// Creates a new Infinity entity.
        /// </summary>
        /// <param name="objectId">Unique object identifier.</param>
        /// <param name="objectType">The type of object this entity represents.</param>
        /// <param name="tag">Tag string for script lookups.</param>
        /// <remarks>
        /// Based on entity creation in MassEffect.exe and MassEffect2.exe.
        /// ObjectId must be unique within the game session.
        /// ObjectType determines available components and behaviors.
        /// </remarks>
        public InfinityEntity(uint objectId, ObjectType objectType, string tag = null)
        {
            _objectId = objectId;
            _objectType = objectType;
            _tag = tag ?? string.Empty;

            Initialize();
        }

        /// <summary>
        /// Unique object ID for this entity.
        /// </summary>
        /// <remarks>
        /// Based on ObjectId field in Infinity entity structure.
        /// Exact addresses to be determined via further reverse engineering of MassEffect.exe and MassEffect2.exe.
        /// Assigned sequentially and must be unique across all entities.
        /// Used for script references and save game serialization.
        /// </remarks>
        public override uint ObjectId => _objectId;

        /// <summary>
        /// Tag string for script lookups.
        /// </summary>
        /// <remarks>
        /// Script-accessible identifier for GetObjectByTag functions.
        /// Exact addresses to be determined via further reverse engineering of MassEffect.exe and MassEffect2.exe.
        /// Can be changed at runtime for dynamic lookups.
        /// </remarks>
        public override string Tag
        {
            get => _tag;
            set => _tag = value ?? string.Empty;
        }

        /// <summary>
        /// The type of this object.
        /// </summary>
        /// <remarks>
        /// Determines available components and behaviors.
        /// Cannot be changed after entity creation.
        /// </remarks>
        public override ObjectType ObjectType => _objectType;

        /// <summary>
        /// Whether this entity is valid and not destroyed.
        /// </summary>
        /// <remarks>
        /// Entity validity prevents use-after-free issues.
        /// Becomes invalid when Destroy() is called.
        /// </remarks>
        public override bool IsValid => _isValid;

        /// <summary>
        /// The world this entity belongs to.
        /// </summary>
        /// <remarks>
        /// Reference to containing world for cross-entity operations.
        /// Set when entity is added to an area.
        /// </remarks>
        public override IWorld World
        {
            get => _world;
            set => _world = value;
        }

        /// <summary>
        /// Gets or sets the area ID this entity belongs to.
        /// </summary>
        /// <remarks>
        /// AreaId identifies which area the entity is located in.
        /// Set when entity is registered to an area in the world.
        /// </remarks>
        public override uint AreaId
        {
            get => _areaId;
            set => _areaId = value;
        }

        /// <summary>
        /// Initializes the entity after creation.
        /// </summary>
        /// <remarks>
        /// Attaches default components based on ObjectType.
        /// Registers with necessary systems.
        /// Called automatically in constructor.
        /// </remarks>
        protected override void Initialize()
        {
            // Attach components based on object type
            switch (_objectType)
            {
                case ObjectType.Creature:
                    AttachCreatureComponents();
                    break;
                case ObjectType.Door:
                    AttachDoorComponents();
                    break;
                case ObjectType.Placeable:
                    AttachPlaceableComponents();
                    break;
                case ObjectType.Trigger:
                    AttachTriggerComponents();
                    break;
                case ObjectType.Waypoint:
                    AttachWaypointComponents();
                    break;
                case ObjectType.Sound:
                    AttachSoundComponents();
                    break;
            }

            // All entities get transform and script hooks
            AttachCommonComponents();
        }

        /// <summary>
        /// Attaches components common to all entity types.
        /// </summary>
        /// <remarks>
        /// Common components attached to all entities:
        /// - TransformComponent: Position, orientation, scale for all entities
        /// - ScriptHooksComponent: Script event hooks and local variables for all entities
        ///
        /// Based on MassEffect.exe and MassEffect2.exe: All entities have transform and script hooks capability.
        /// Transform data is loaded from level files and templates.
        /// </remarks>
        private void AttachCommonComponents()
        {
            // Attach transform component for all entities
            // Based on MassEffect.exe and MassEffect2.exe: All entities have transform data (position, orientation, scale)
            if (!HasComponent<ITransformComponent>())
            {
                var transformComponent = new InfinityTransformComponent();
                AddComponent<ITransformComponent>(transformComponent);
            }

            // TODO: Attach script hooks component
            // TODO: Attach any other common components
        }

        /// <summary>
        /// Attaches components specific to creatures.
        /// </summary>
        /// <remarks>
        /// Creatures have stats, inventory, combat capabilities, etc.
        /// Based on creature component structure in MassEffect.exe and MassEffect2.exe.
        /// Uses different character system (to be reverse engineered).
        /// </remarks>
        private void AttachCreatureComponents()
        {
            // TODO: Attach creature-specific components
            // StatsComponent, InventoryComponent, CombatComponent, etc.
            // Infinity-specific: Different character system
        }

        /// <summary>
        /// Attaches components specific to doors.
        /// </summary>
        /// <remarks>
        /// Doors have open/close state, lock state, transition logic.
        /// Based on door component structure in MassEffect.exe and MassEffect2.exe.
        /// - Note: Infinity engines may not have traditional door systems like Odyssey/Aurora
        /// - If doors are supported, they would use Infinity-specific file formats and systems
        /// - Original implementation: Needs reverse engineering from MassEffect.exe and MassEffect2.exe
        /// - Door component attached during entity creation if door support exists
        /// </remarks>
        private void AttachDoorComponents()
        {
            // Attach door component if not already present
            // Based on Infinity engine: Door component attachment (if doors are supported)
            // Note: Infinity engines may not support traditional doors, but component exists for compatibility
            if (!HasComponent<IDoorComponent>())
            {
                var doorComponent = new InfinityDoorComponent();
                doorComponent.Owner = this;
                AddComponent<IDoorComponent>(doorComponent);
            }
        }

        /// <summary>
        /// Attaches components specific to placeables.
        /// </summary>
        /// <remarks>
        /// Placeables have interaction state, inventory, use logic.
        /// Based on placeable component structure in MassEffect.exe and MassEffect2.exe.
        /// </remarks>
        private void AttachPlaceableComponents()
        {
            // TODO: Attach placeable-specific components
            // PlaceableComponent with use/interaction state
        }

        /// <summary>
        /// Attaches components specific to triggers.
        /// </summary>
        /// <remarks>
        /// Triggers have enter/exit detection, script firing.
        /// Based on trigger component structure in MassEffect.exe and MassEffect2.exe.
        /// </remarks>
        private void AttachTriggerComponents()
        {
            // Attach trigger component if not already present
            // Based on MassEffect.exe and MassEffect2.exe: Trigger component is attached during entity creation
            // ComponentInitializer also handles this, but we ensure it's attached here for consistency
            // - Infinity engine uses different trigger system than Odyssey/Aurora/Eclipse
            // - Note: Infinity engine trigger system is not fully reverse engineered yet
            // - Component provides: Geometry, IsEnabled, TriggerType, LinkedTo, LinkedToModule, IsTrap, TrapActive, TrapDetected, TrapDisarmed, TrapDetectDC, TrapDisarmDC, FireOnce, HasFired, ContainsPoint, ContainsEntity
            if (!HasComponent<ITriggerComponent>())
            {
                var triggerComponent = new Components.InfinityTriggerComponent();
                triggerComponent.Owner = this;
                AddComponent<ITriggerComponent>(triggerComponent);
            }
        }

        /// <summary>
        /// Attaches components specific to waypoints.
        /// </summary>
        /// <remarks>
        /// Waypoints have position data, pathfinding integration.
        /// Based on waypoint component structure in MassEffect.exe and MassEffect2.exe.
        /// </remarks>
        private void AttachWaypointComponents()
        {
            if (!HasComponent<IWaypointComponent>())
            {
                var waypointComponent = new InfinityWaypointComponent();
                AddComponent<IWaypointComponent>(waypointComponent);
            }
        }

        /// <summary>
        /// Attaches components specific to sounds.
        /// </summary>
        /// <remarks>
        /// Sounds have audio playback, spatial positioning.
        /// Based on sound component structure in MassEffect.exe and MassEffect2.exe.
        /// </remarks>
        private void AttachSoundComponents()
        {
            // TODO: Attach sound-specific components
            // SoundComponent with audio playback capabilities
        }

        /// <summary>
        /// Updates the entity each frame.
        /// </summary>
        /// <remarks>
        /// Updates all attached components.
        /// Processes any pending script events.
        /// Handles component interactions.
        /// 
        /// Based on MassEffect.exe and MassEffect2.exe: Entity update loop processes components in dependency order.
        /// Component update order:
        /// 1. TransformComponent (position, orientation updates)
        /// 2. ActionQueueComponent (action execution, may modify transform)
        /// 3. StatsComponent (HP regeneration, stat updates)
        /// 4. PerceptionComponent (perception checks, uses transform position)
        /// 5. Other components (in arbitrary order)
        /// 
        /// Component interactions:
        /// - Transform changes trigger perception updates
        /// - HP changes trigger death state updates
        /// - Action queue execution may modify transform
        /// - Inventory changes affect encumbrance and movement speed
        /// </remarks>
        public override void Update(float deltaTime)
        {
            if (!IsValid)
                return;

            // Update components in dependency order
            // 1. TransformComponent first (position/orientation)
            var transformComponent = GetComponent<ITransformComponent>();
            if (transformComponent is IUpdatableComponent updatableTransform)
            {
                updatableTransform.Update(deltaTime);
            }

            // 2. ActionQueueComponent (may modify transform through movement actions)
            var actionQueueComponent = GetComponent<IActionQueueComponent>();
            if (actionQueueComponent != null)
            {
                actionQueueComponent.Update(this, deltaTime);
            }

            // 3. StatsComponent (HP regeneration, stat updates)
            var statsComponent = GetComponent<IStatsComponent>();
            if (statsComponent is IUpdatableComponent updatableStats)
            {
                updatableStats.Update(deltaTime);
            }

            // 4. PerceptionComponent (uses transform position)
            var perceptionComponent = GetComponent<IPerceptionComponent>();
            if (perceptionComponent is IUpdatableComponent updatablePerception)
            {
                updatablePerception.Update(deltaTime);
            }

            // 5. Other components (in arbitrary order)
            foreach (var component in GetAllComponents())
            {
                // Skip already-updated components
                if (component == transformComponent || 
                    component == actionQueueComponent || 
                    component == statsComponent || 
                    component == perceptionComponent)
                {
                    continue;
                }

                if (component is IUpdatableComponent updatable)
                {
                    updatable.Update(deltaTime);
                }
            }

            // Handle component interactions after all components are updated
            HandleComponentInteractions(deltaTime);

            // Process script events and hooks
            // Script events are processed by the game loop, not here
            // But we could fire heartbeat events here if needed
        }

        /// <summary>
        /// Destroys the entity and cleans up resources.
        /// </summary>
        /// <remarks>
        /// Removes from world and area systems.
        /// Cleans up all components and resources.
        /// Marks entity as invalid.
        /// </remarks>
        public override void Destroy()
        {
            if (!IsValid)
                return;

            _isValid = false;

            // Remove from world/area
            if (_world != null)
            {
                // TODO: Remove from world's entity collections
            }

            // Clean up components
            foreach (var component in GetAllComponents())
            {
                if (component is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            // Clear component references
            var componentTypes = GetAllComponents().Select(c => c.GetType()).ToArray();
            foreach (var componentType in componentTypes)
            {
                // Remove component by type - this is a bit hacky but works
                var method = GetType().GetMethod("RemoveComponent")?.MakeGenericMethod(componentType);
                method?.Invoke(this, null);
            }
        }

        /// <summary>
        /// Serializes entity data for save games.
        /// </summary>
        /// <remarks>
        /// Based on Infinity entity serialization functions in MassEffect.exe and MassEffect2.exe.
        /// Serializes ObjectId, Tag, components, and custom data.
        /// Uses Infinity-specific binary save format (similar to Eclipse engine).
        ///
        /// Binary format structure:
        /// - Basic entity properties (ObjectId: uint32, Tag: string, ObjectType: int32, AreaId: uint32, IsValid: int32)
        /// - Transform component (if present: flag int32, then Position: 3 floats, Facing: float, Scale: 3 floats, ParentObjectId: uint32)
        /// - Stats component (if present: flag int32, then HP/FP/abilities/saves: various types)
        /// - Inventory component (if present: flag int32, then item count and items)
        /// - Script hooks component (if present: flag int32, then script ResRefs and local variables)
        /// - Door component (if present: flag int32, then door state data)
        /// - Placeable component (if present: flag int32, then placeable state data)
        /// - Custom data dictionary (count int32, then key-value pairs)
        ///
        /// Serialized data includes:
        /// - Basic entity properties (ObjectId, Tag, ObjectType, AreaId)
        /// - Transform component (position, facing, scale)
        /// - Stats component (HP, abilities, skills, saves)
        /// - Door component (open/locked state, HP, transitions)
        /// - Placeable component (open/locked state, HP, useability)
        /// - Inventory component (equipped items and inventory bag)
        /// - Script hooks component (script ResRefs and local variables)
        /// - Custom data dictionary (arbitrary key-value pairs)
        /// </remarks>
        public override byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            using (var writer = new BinaryWriter(stream, Encoding.UTF8))
            {
                // Serialize basic entity properties
                writer.Write(_objectId);
                WriteString(writer, _tag ?? "");
                writer.Write((int)_objectType);
                writer.Write(_areaId);
                writer.Write(_isValid ? 1 : 0);

                // Serialize Transform component
                var transformComponent = GetComponent<ITransformComponent>();
                writer.Write(transformComponent != null ? 1 : 0);
                if (transformComponent != null)
                {
                    writer.Write(transformComponent.Position.X);
                    writer.Write(transformComponent.Position.Y);
                    writer.Write(transformComponent.Position.Z);
                    writer.Write(transformComponent.Facing);
                    writer.Write(transformComponent.Scale.X);
                    writer.Write(transformComponent.Scale.Y);
                    writer.Write(transformComponent.Scale.Z);
                    writer.Write(transformComponent.Parent != null ? transformComponent.Parent.ObjectId : 0u);
                }

                // Serialize Stats component
                var statsComponent = GetComponent<IStatsComponent>();
                writer.Write(statsComponent != null ? 1 : 0);
                if (statsComponent != null)
                {
                    writer.Write(statsComponent.CurrentHP);
                    writer.Write(statsComponent.MaxHP);
                    writer.Write(statsComponent.CurrentFP);
                    writer.Write(statsComponent.MaxFP);
                    writer.Write(statsComponent.IsDead ? 1 : 0);
                    writer.Write(statsComponent.BaseAttackBonus);
                    writer.Write(statsComponent.ArmorClass);
                    writer.Write(statsComponent.FortitudeSave);
                    writer.Write(statsComponent.ReflexSave);
                    writer.Write(statsComponent.WillSave);
                    writer.Write(statsComponent.WalkSpeed);
                    writer.Write(statsComponent.RunSpeed);
                    writer.Write(statsComponent.Level);

                    // Serialize ability scores
                    writer.Write(statsComponent.GetAbility(Ability.Strength));
                    writer.Write(statsComponent.GetAbility(Ability.Dexterity));
                    writer.Write(statsComponent.GetAbility(Ability.Constitution));
                    writer.Write(statsComponent.GetAbility(Ability.Intelligence));
                    writer.Write(statsComponent.GetAbility(Ability.Wisdom));
                    writer.Write(statsComponent.GetAbility(Ability.Charisma));

                    // Serialize ability modifiers
                    writer.Write(statsComponent.GetAbilityModifier(Ability.Strength));
                    writer.Write(statsComponent.GetAbilityModifier(Ability.Dexterity));
                    writer.Write(statsComponent.GetAbilityModifier(Ability.Constitution));
                    writer.Write(statsComponent.GetAbilityModifier(Ability.Intelligence));
                    writer.Write(statsComponent.GetAbilityModifier(Ability.Wisdom));
                    writer.Write(statsComponent.GetAbilityModifier(Ability.Charisma));

                    // Serialize known spells (simplified - serialize spell IDs)
                    // Note: In a full implementation, we would iterate through all possible spell IDs
                    // For now, we serialize an empty count as a placeholder
                    writer.Write(0); // Known spell count
                }

                // Serialize Inventory component
                var inventoryComponent = GetComponent<IInventoryComponent>();
                writer.Write(inventoryComponent != null ? 1 : 0);
                if (inventoryComponent != null)
                {
                    // Collect all items from all slots
                    var allItems = new List<(int slot, IEntity item)>();
                    for (int slot = 0; slot < 256; slot++) // Reasonable upper bound
                    {
                        var item = inventoryComponent.GetItemInSlot(slot);
                        if (item != null)
                        {
                            allItems.Add((slot, item));
                        }
                    }

                    writer.Write(allItems.Count);
                    foreach (var (slot, item) in allItems)
                    {
                        writer.Write(slot);
                        writer.Write(item.ObjectId);
                        WriteString(writer, item.Tag ?? "");
                        writer.Write((int)item.ObjectType);
                    }
                }

                // Serialize ScriptHooks component
                var scriptHooksComponent = GetComponent<IScriptHooksComponent>();
                writer.Write(scriptHooksComponent != null ? 1 : 0);
                if (scriptHooksComponent != null)
                {
                    // Serialize script ResRefs for all event types
                    int scriptCount = 0;
                    var scripts = new List<(int eventType, string resRef)>();
                    foreach (ScriptEvent eventType in Enum.GetValues(typeof(ScriptEvent)))
                    {
                        string scriptResRef = scriptHooksComponent.GetScript(eventType);
                        if (!string.IsNullOrEmpty(scriptResRef))
                        {
                            scripts.Add(((int)eventType, scriptResRef));
                            scriptCount++;
                        }
                    }
                    writer.Write(scriptCount);
                    foreach (var (eventType, resRef) in scripts)
                    {
                        writer.Write(eventType);
                        WriteString(writer, resRef);
                    }

                    // Serialize local variables using reflection to access private dictionaries
                    // Based on MassEffect.exe and MassEffect2.exe: Local variables are stored in ScriptHooksComponent
                    // and serialized to binary format
                    Type componentType = scriptHooksComponent.GetType();
                    FieldInfo localIntsField = componentType.GetField("_localInts", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo localFloatsField = componentType.GetField("_localFloats", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo localStringsField = componentType.GetField("_localStrings", BindingFlags.NonPublic | BindingFlags.Instance);

                    int localIntCount = 0;
                    int localFloatCount = 0;
                    int localStringCount = 0;
                    var localInts = new List<(string name, int value)>();
                    var localFloats = new List<(string name, float value)>();
                    var localStrings = new List<(string name, string value)>();

                    if (localIntsField != null)
                    {
                        var localIntsDict = localIntsField.GetValue(scriptHooksComponent) as Dictionary<string, int>;
                        if (localIntsDict != null && localIntsDict.Count > 0)
                        {
                            localIntCount = localIntsDict.Count;
                            foreach (var kvp in localIntsDict)
                            {
                                localInts.Add((kvp.Key, kvp.Value));
                            }
                        }
                    }

                    if (localFloatsField != null)
                    {
                        var localFloatsDict = localFloatsField.GetValue(scriptHooksComponent) as Dictionary<string, float>;
                        if (localFloatsDict != null && localFloatsDict.Count > 0)
                        {
                            localFloatCount = localFloatsDict.Count;
                            foreach (var kvp in localFloatsDict)
                            {
                                localFloats.Add((kvp.Key, kvp.Value));
                            }
                        }
                    }

                    if (localStringsField != null)
                    {
                        var localStringsDict = localStringsField.GetValue(scriptHooksComponent) as Dictionary<string, string>;
                        if (localStringsDict != null && localStringsDict.Count > 0)
                        {
                            localStringCount = localStringsDict.Count;
                            foreach (var kvp in localStringsDict)
                            {
                                localStrings.Add((kvp.Key, kvp.Value ?? ""));
                            }
                        }
                    }

                    writer.Write(localIntCount);
                    foreach (var (name, value) in localInts)
                    {
                        WriteString(writer, name);
                        writer.Write(value);
                    }

                    writer.Write(localFloatCount);
                    foreach (var (name, value) in localFloats)
                    {
                        WriteString(writer, name);
                        writer.Write(value);
                    }

                    writer.Write(localStringCount);
                    foreach (var (name, value) in localStrings)
                    {
                        WriteString(writer, name);
                        WriteString(writer, value);
                    }
                }

                // Serialize Door component
                var doorComponent = GetComponent<IDoorComponent>();
                writer.Write(doorComponent != null ? 1 : 0);
                if (doorComponent != null)
                {
                    writer.Write(doorComponent.IsOpen ? 1 : 0);
                    writer.Write(doorComponent.IsLocked ? 1 : 0);
                    writer.Write(doorComponent.LockableByScript ? 1 : 0);
                    writer.Write(doorComponent.LockDC);
                    writer.Write(doorComponent.IsBashed ? 1 : 0);
                    writer.Write(doorComponent.HitPoints);
                    writer.Write(doorComponent.MaxHitPoints);
                    writer.Write(doorComponent.Hardness);
                    WriteString(writer, doorComponent.KeyTag ?? "");
                    writer.Write(doorComponent.KeyRequired ? 1 : 0);
                    writer.Write(doorComponent.OpenState);
                    WriteString(writer, doorComponent.LinkedTo ?? "");
                    WriteString(writer, doorComponent.LinkedToModule ?? "");
                }

                // Serialize Placeable component
                var placeableComponent = GetComponent<IPlaceableComponent>();
                writer.Write(placeableComponent != null ? 1 : 0);
                if (placeableComponent != null)
                {
                    writer.Write(placeableComponent.IsUseable ? 1 : 0);
                    writer.Write(placeableComponent.HasInventory ? 1 : 0);
                    writer.Write(placeableComponent.IsStatic ? 1 : 0);
                    writer.Write(placeableComponent.IsOpen ? 1 : 0);
                    writer.Write(placeableComponent.IsLocked ? 1 : 0);
                    writer.Write(placeableComponent.LockDC);
                    WriteString(writer, placeableComponent.KeyTag ?? "");
                    writer.Write(placeableComponent.HitPoints);
                    writer.Write(placeableComponent.MaxHitPoints);
                    writer.Write(placeableComponent.Hardness);
                    writer.Write(placeableComponent.AnimationState);
                }

                // Serialize custom data dictionary using reflection to access private _data field
                // BaseEntity stores custom data in _data dictionary for script variables and temporary state
                Type baseEntityType = typeof(BaseEntity);
                FieldInfo dataField = baseEntityType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);

                int customDataCount = 0;
                var customDataEntries = new List<(string key, object value, int type)>();

                if (dataField != null)
                {
                    var data = dataField.GetValue(this) as Dictionary<string, object>;
                    if (data != null && data.Count > 0)
                    {
                        customDataCount = data.Count;
                        foreach (var kvp in data)
                        {
                            int valueType = 0; // 0=null, 1=int, 2=float, 3=string, 4=bool, 5=uint
                            if (kvp.Value == null)
                            {
                                valueType = 0;
                            }
                            else
                            {
                                Type valueTypeObj = kvp.Value.GetType();
                                if (valueTypeObj == typeof(int))
                                {
                                    valueType = 1;
                                }
                                else if (valueTypeObj == typeof(float))
                                {
                                    valueType = 2;
                                }
                                else if (valueTypeObj == typeof(string))
                                {
                                    valueType = 3;
                                }
                                else if (valueTypeObj == typeof(bool))
                                {
                                    valueType = 4;
                                }
                                else if (valueTypeObj == typeof(uint))
                                {
                                    valueType = 5;
                                }
                                else
                                {
                                    valueType = 3; // Default to string for unknown types
                                }
                            }
                            customDataEntries.Add((kvp.Key, kvp.Value, valueType));
                        }
                    }
                }

                writer.Write(customDataCount);
                foreach (var (key, value, type) in customDataEntries)
                {
                    WriteString(writer, key);
                    writer.Write(type);
                    switch (type)
                    {
                        case 0: // null
                            break;
                        case 1: // int
                            writer.Write((int)value);
                            break;
                        case 2: // float
                            writer.Write((float)value);
                            break;
                        case 3: // string
                            WriteString(writer, value?.ToString() ?? "");
                            break;
                        case 4: // bool
                            writer.Write((bool)value ? 1 : 0);
                            break;
                        case 5: // uint
                            writer.Write((uint)value);
                            break;
                    }
                }

                return stream.ToArray();
            }
        }

        /// <summary>
        /// Helper method to write a string to BinaryWriter.
        /// </summary>
        /// <remarks>
        /// Writes string length as int32, then UTF-8 encoded bytes.
        /// Based on Eclipse save serializer string writing pattern.
        /// </remarks>
        private static void WriteString(BinaryWriter writer, string value)
        {
            if (value == null)
            {
                writer.Write(0);
                return;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(value);
            writer.Write(bytes.Length);
            if (bytes.Length > 0)
            {
                writer.Write(bytes);
            }
        }

        /// <summary>
        /// Deserializes entity data from save games.
        /// </summary>
        /// <remarks>
        /// Based on Infinity entity deserialization functions in MassEffect.exe and MassEffect2.exe.
        /// Restores ObjectId, Tag, components, and custom data.
        /// Recreates component attachments and state.
        ///
        /// Binary format structure (matches Serialize):
        /// - Basic entity properties (ObjectId: uint32, Tag: string, ObjectType: int32, AreaId: uint32, IsValid: int32)
        /// - Transform component (if present: flag int32, then Position: 3 floats, Facing: float, Scale: 3 floats, ParentObjectId: uint32)
        /// - Stats component (if present: flag int32, then HP/FP/abilities/saves: various types)
        /// - Inventory component (if present: flag int32, then item count and items)
        /// - Script hooks component (if present: flag int32, then script ResRefs and local variables)
        /// - Door component (if present: flag int32, then door state data)
        /// - Placeable component (if present: flag int32, then placeable state data)
        /// - Custom data dictionary (count int32, then key-value pairs)
        /// </remarks>
        public override void Deserialize(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                throw new ArgumentException("Entity deserialization data cannot be null or empty", nameof(data));
            }

            using (var stream = new MemoryStream(data))
            using (var reader = new BinaryReader(stream, Encoding.UTF8))
            {
                // Deserialize basic entity properties
                _objectId = reader.ReadUInt32();
                _tag = ReadString(reader);
                // ObjectType is read-only, so we verify it matches
                int objectTypeValue = reader.ReadInt32();
                if (objectTypeValue != (int)_objectType)
                {
                    throw new InvalidDataException($"Deserialized ObjectType {objectTypeValue} does not match entity ObjectType {(int)_objectType}");
                }
                _areaId = reader.ReadUInt32();
                _isValid = reader.ReadInt32() != 0;

                // Deserialize Transform component
                bool hasTransform = reader.ReadInt32() != 0;
                if (hasTransform)
                {
                    var transformComponent = GetComponent<ITransformComponent>();
                    if (transformComponent == null)
                    {
                        transformComponent = new InfinityTransformComponent();
                        AddComponent<ITransformComponent>(transformComponent);
                    }

                    float x = reader.ReadSingle();
                    float y = reader.ReadSingle();
                    float z = reader.ReadSingle();
                    float facing = reader.ReadSingle();
                    float scaleX = reader.ReadSingle();
                    float scaleY = reader.ReadSingle();
                    float scaleZ = reader.ReadSingle();
                    uint parentObjectId = reader.ReadUInt32();

                    transformComponent.Position = new System.Numerics.Vector3(x, y, z);
                    transformComponent.Facing = facing;
                    transformComponent.Scale = new System.Numerics.Vector3(scaleX, scaleY, scaleZ);
                    // Parent will be resolved later when all entities are loaded
                    // Store parentObjectId in custom data for later resolution
                    if (parentObjectId != 0)
                    {
                        Type baseEntityTypeForParent = typeof(BaseEntity);
                        FieldInfo dataFieldForParent = baseEntityTypeForParent.GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (dataFieldForParent != null)
                        {
                            var data = dataFieldForParent.GetValue(this) as Dictionary<string, object>;
                            if (data == null)
                            {
                                data = new Dictionary<string, object>();
                                dataFieldForParent.SetValue(this, data);
                            }
                            data["_ParentObjectId"] = parentObjectId;
                        }
                    }
                }

                // Deserialize Stats component
                bool hasStats = reader.ReadInt32() != 0;
                if (hasStats)
                {
                    // Stats component should already exist for creatures, but we ensure it's present
                    var statsComponent = GetComponent<IStatsComponent>();
                    if (statsComponent == null)
                    {
                        // Create stats component - we'll need to check what the Infinity stats component type is
                        // For now, we'll use reflection or a factory method
                        // TODO: Create InfinityStatsComponent if it exists
                        throw new InvalidOperationException("Stats component not found and cannot be created automatically");
                    }

                    statsComponent.CurrentHP = reader.ReadInt32();
                    statsComponent.MaxHP = reader.ReadInt32();
                    statsComponent.CurrentFP = reader.ReadInt32();
                    statsComponent.MaxFP = reader.ReadInt32();
                    // IsDead is computed from CurrentHP, so we don't deserialize it directly
                    int isDead = reader.ReadInt32();
                    int baseAttackBonus = reader.ReadInt32();
                    int armorClass = reader.ReadInt32();
                    int fortitudeSave = reader.ReadInt32();
                    int reflexSave = reader.ReadInt32();
                    int willSave = reader.ReadInt32();
                    float walkSpeed = reader.ReadSingle();
                    float runSpeed = reader.ReadSingle();
                    int level = reader.ReadInt32();

                    // Deserialize ability scores
                    int str = reader.ReadInt32();
                    int dex = reader.ReadInt32();
                    int con = reader.ReadInt32();
                    int intel = reader.ReadInt32();
                    int wis = reader.ReadInt32();
                    int cha = reader.ReadInt32();

                    statsComponent.SetAbility(Ability.Strength, str);
                    statsComponent.SetAbility(Ability.Dexterity, dex);
                    statsComponent.SetAbility(Ability.Constitution, con);
                    statsComponent.SetAbility(Ability.Intelligence, intel);
                    statsComponent.SetAbility(Ability.Wisdom, wis);
                    statsComponent.SetAbility(Ability.Charisma, cha);

                    // Deserialize ability modifiers (read but don't set - they're computed)
                    reader.ReadInt32(); // STR modifier
                    reader.ReadInt32(); // DEX modifier
                    reader.ReadInt32(); // CON modifier
                    reader.ReadInt32(); // INT modifier
                    reader.ReadInt32(); // WIS modifier
                    reader.ReadInt32(); // CHA modifier

                    // Deserialize known spells count
                    int knownSpellCount = reader.ReadInt32();
                    for (int i = 0; i < knownSpellCount; i++)
                    {
                        // In a full implementation, we would read spell IDs and restore them
                        // For now, we skip the spell data
                    }
                }

                // Deserialize Inventory component
                bool hasInventory = reader.ReadInt32() != 0;
                if (hasInventory)
                {
                    var inventoryComponent = GetComponent<IInventoryComponent>();
                    if (inventoryComponent == null)
                    {
                        // Inventory component should exist for creatures, but we ensure it's present
                        // TODO: Create InfinityInventoryComponent if it exists
                        throw new InvalidOperationException("Inventory component not found and cannot be created automatically");
                    }

                    int itemCount = reader.ReadInt32();
                    // Store item references for later resolution when all entities are loaded
                    var itemReferences = new List<(int slot, uint objectId, string tag, int objectType)>();
                    for (int i = 0; i < itemCount; i++)
                    {
                        int slot = reader.ReadInt32();
                        uint itemObjectId = reader.ReadUInt32();
                        string itemTag = ReadString(reader);
                        int itemObjectType = reader.ReadInt32();
                        itemReferences.Add((slot, itemObjectId, itemTag, itemObjectType));
                    }
                    // Store item references in custom data for later resolution
                    Type baseEntityTypeForItems = typeof(BaseEntity);
                    FieldInfo dataFieldForItems = baseEntityTypeForItems.GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (dataFieldForItems != null)
                    {
                        var data = dataFieldForItems.GetValue(this) as Dictionary<string, object>;
                        if (data == null)
                        {
                            data = new Dictionary<string, object>();
                            dataFieldForItems.SetValue(this, data);
                        }
                        data["_ItemReferences"] = itemReferences;
                    }
                }

                // Deserialize ScriptHooks component
                bool hasScriptHooks = reader.ReadInt32() != 0;
                if (hasScriptHooks)
                {
                    var scriptHooksComponent = GetComponent<IScriptHooksComponent>();
                    if (scriptHooksComponent == null)
                    {
                        // Script hooks component should exist for all entities
                        // TODO: Create InfinityScriptHooksComponent if it exists
                        throw new InvalidOperationException("ScriptHooks component not found and cannot be created automatically");
                    }

                    // Deserialize script ResRefs
                    int scriptCount = reader.ReadInt32();
                    for (int i = 0; i < scriptCount; i++)
                    {
                        int eventType = reader.ReadInt32();
                        string resRef = ReadString(reader);
                        scriptHooksComponent.SetScript((ScriptEvent)eventType, resRef);
                    }

                    // Deserialize local variables using reflection
                    Type componentType = scriptHooksComponent.GetType();
                    FieldInfo localIntsField = componentType.GetField("_localInts", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo localFloatsField = componentType.GetField("_localFloats", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo localStringsField = componentType.GetField("_localStrings", BindingFlags.NonPublic | BindingFlags.Instance);

                    // Deserialize local ints
                    int localIntCount = reader.ReadInt32();
                    if (localIntsField != null)
                    {
                        var localInts = localIntsField.GetValue(scriptHooksComponent) as Dictionary<string, int>;
                        if (localInts == null)
                        {
                            localInts = new Dictionary<string, int>();
                            localIntsField.SetValue(scriptHooksComponent, localInts);
                        }
                        localInts.Clear();
                        for (int i = 0; i < localIntCount; i++)
                        {
                            string name = ReadString(reader);
                            int value = reader.ReadInt32();
                            localInts[name] = value;
                        }
                    }
                    else
                    {
                        // Skip ints if field not found
                        for (int i = 0; i < localIntCount; i++)
                        {
                            ReadString(reader);
                            reader.ReadInt32();
                        }
                    }

                    // Deserialize local floats
                    int localFloatCount = reader.ReadInt32();
                    if (localFloatsField != null)
                    {
                        var localFloats = localFloatsField.GetValue(scriptHooksComponent) as Dictionary<string, float>;
                        if (localFloats == null)
                        {
                            localFloats = new Dictionary<string, float>();
                            localFloatsField.SetValue(scriptHooksComponent, localFloats);
                        }
                        localFloats.Clear();
                        for (int i = 0; i < localFloatCount; i++)
                        {
                            string name = ReadString(reader);
                            float value = reader.ReadSingle();
                            localFloats[name] = value;
                        }
                    }
                    else
                    {
                        // Skip floats if field not found
                        for (int i = 0; i < localFloatCount; i++)
                        {
                            ReadString(reader);
                            reader.ReadSingle();
                        }
                    }

                    // Deserialize local strings
                    int localStringCount = reader.ReadInt32();
                    if (localStringsField != null)
                    {
                        var localStrings = localStringsField.GetValue(scriptHooksComponent) as Dictionary<string, string>;
                        if (localStrings == null)
                        {
                            localStrings = new Dictionary<string, string>();
                            localStringsField.SetValue(scriptHooksComponent, localStrings);
                        }
                        localStrings.Clear();
                        for (int i = 0; i < localStringCount; i++)
                        {
                            string name = ReadString(reader);
                            string value = ReadString(reader);
                            localStrings[name] = value;
                        }
                    }
                    else
                    {
                        // Skip strings if field not found
                        for (int i = 0; i < localStringCount; i++)
                        {
                            ReadString(reader);
                            ReadString(reader);
                        }
                    }
                }

                // Deserialize Door component
                bool hasDoor = reader.ReadInt32() != 0;
                if (hasDoor)
                {
                    var doorComponent = GetComponent<IDoorComponent>();
                    if (doorComponent == null)
                    {
                        doorComponent = new InfinityDoorComponent();
                        doorComponent.Owner = this;
                        AddComponent<IDoorComponent>(doorComponent);
                    }

                    doorComponent.IsOpen = reader.ReadInt32() != 0;
                    doorComponent.IsLocked = reader.ReadInt32() != 0;
                    doorComponent.LockableByScript = reader.ReadInt32() != 0;
                    doorComponent.LockDC = reader.ReadInt32();
                    doorComponent.IsBashed = reader.ReadInt32() != 0;
                    doorComponent.HitPoints = reader.ReadInt32();
                    doorComponent.MaxHitPoints = reader.ReadInt32();
                    doorComponent.Hardness = reader.ReadInt32();
                    doorComponent.KeyTag = ReadString(reader);
                    doorComponent.KeyRequired = reader.ReadInt32() != 0;
                    doorComponent.OpenState = reader.ReadInt32();
                    doorComponent.LinkedTo = ReadString(reader);
                    doorComponent.LinkedToModule = ReadString(reader);
                }

                // Deserialize Placeable component
                bool hasPlaceable = reader.ReadInt32() != 0;
                if (hasPlaceable)
                {
                    var placeableComponent = GetComponent<IPlaceableComponent>();
                    if (placeableComponent == null)
                    {
                        // TODO: Create InfinityPlaceableComponent if it exists
                        throw new InvalidOperationException("Placeable component not found and cannot be created automatically");
                    }

                    placeableComponent.IsUseable = reader.ReadInt32() != 0;
                    placeableComponent.HasInventory = reader.ReadInt32() != 0;
                    placeableComponent.IsStatic = reader.ReadInt32() != 0;
                    placeableComponent.IsOpen = reader.ReadInt32() != 0;
                    placeableComponent.IsLocked = reader.ReadInt32() != 0;
                    placeableComponent.LockDC = reader.ReadInt32();
                    placeableComponent.KeyTag = ReadString(reader);
                    placeableComponent.HitPoints = reader.ReadInt32();
                    placeableComponent.MaxHitPoints = reader.ReadInt32();
                    placeableComponent.Hardness = reader.ReadInt32();
                    placeableComponent.AnimationState = reader.ReadInt32();
                }

                // Deserialize custom data dictionary
                int customDataCount = reader.ReadInt32();
                Type baseEntityType = typeof(BaseEntity);
                FieldInfo dataField = baseEntityType.GetField("_data", BindingFlags.NonPublic | BindingFlags.Instance);

                if (dataField != null)
                {
                    var data = dataField.GetValue(this) as Dictionary<string, object>;
                    if (data == null)
                    {
                        data = new Dictionary<string, object>();
                        dataField.SetValue(this, data);
                    }
                    data.Clear();

                    for (int i = 0; i < customDataCount; i++)
                    {
                        string key = ReadString(reader);
                        int type = reader.ReadInt32();
                        object value = null;

                        switch (type)
                        {
                            case 0: // null
                                value = null;
                                break;
                            case 1: // int
                                value = reader.ReadInt32();
                                break;
                            case 2: // float
                                value = reader.ReadSingle();
                                break;
                            case 3: // string
                                value = ReadString(reader);
                                break;
                            case 4: // bool
                                value = reader.ReadInt32() != 0;
                                break;
                            case 5: // uint
                                value = reader.ReadUInt32();
                                break;
                        }

                        data[key] = value;
                    }
                }
                else
                {
                    // Skip custom data if field not found
                    for (int i = 0; i < customDataCount; i++)
                    {
                        ReadString(reader);
                        int type = reader.ReadInt32();
                        switch (type)
                        {
                            case 0: // null
                                break;
                            case 1: // int
                                reader.ReadInt32();
                                break;
                            case 2: // float
                                reader.ReadSingle();
                                break;
                            case 3: // string
                                ReadString(reader);
                                break;
                            case 4: // bool
                                reader.ReadInt32();
                                break;
                            case 5: // uint
                                reader.ReadUInt32();
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Helper method to read a string from BinaryReader.
        /// </summary>
        /// <remarks>
        /// Reads string length as int32, then UTF-8 encoded bytes.
        /// Based on Eclipse save serializer string reading pattern.
        /// </remarks>
        private static string ReadString(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            if (length == 0)
            {
                return "";
            }

            byte[] bytes = reader.ReadBytes(length);
            return Encoding.UTF8.GetString(bytes);
        }
    }

    /// <summary>
    /// Interface for components that need per-frame updates.
    /// </summary>
    internal interface IUpdatableComponent
    {
        void Update(float deltaTime);
    }
}

