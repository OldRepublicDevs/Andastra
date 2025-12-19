using System;
using System.Collections.Generic;
using System.Numerics;
using Andastra.Parsing.Common.Script;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Scripting.EngineApi;
using Andastra.Runtime.Scripting.Interfaces;

namespace Andastra.Runtime.Engines.Eclipse.EngineApi
{
    /// <summary>
    /// Eclipse Engine engine API implementation for Dragon Age and Mass Effect series.
    /// </summary>
    /// <remarks>
    /// Eclipse Engine API (Script Functions):
    /// - Based on daorigins.exe (Dragon Age: Origins), DragonAge2.exe, MassEffect.exe, MassEffect2.exe script engine API implementations
    /// - Located via string references: Script function dispatch system (different from NWScript, uses UnrealScript-like system)
    /// - Original implementation: Script VM executes function calls with routine ID, calls engine function handlers
    /// - Function IDs match script definitions (different from NWScript function IDs)
    /// - Dragon Age: Origins has ~500 engine functions, Mass Effect has ~400 engine functions
    /// - Original engine uses function dispatch table indexed by routine ID
    /// - Function implementations must match script semantics (parameter types, return types, behavior)
    /// - Eclipse uses UnrealScript-like system, so function signatures differ from NWScript
    /// - Common Eclipse functions: Print, Random, GetObjectByTag, GetTag, GetPosition, GetFacing, SpawnCreature, CreateItem, etc.
    /// - Note: Function IDs are Eclipse-specific and differ from NWScript function IDs
    /// </remarks>
    public class EclipseEngineApi : BaseEngineApi
    {
        public EclipseEngineApi()
        {
        }

        protected override void RegisterFunctions()
        {
            // Register common Eclipse function names
            // Note: Eclipse function IDs differ from NWScript - these are Eclipse-specific mappings
            // Function names are registered for debugging and logging purposes
            RegisterFunctionName(0, "Random");
            RegisterFunctionName(1, "PrintString");
            RegisterFunctionName(2, "PrintFloat");
            RegisterFunctionName(3, "FloatToString");
            RegisterFunctionName(4, "PrintInteger");
            RegisterFunctionName(5, "PrintObject");
            
            // Object functions
            RegisterFunctionName(27, "GetPosition");
            RegisterFunctionName(28, "GetFacing");
            RegisterFunctionName(41, "GetDistanceToObject");
            RegisterFunctionName(42, "GetIsObjectValid");
            
            // Tag functions
            RegisterFunctionName(168, "GetTag");
            RegisterFunctionName(200, "GetObjectByTag");
            
            // Global variables
            RegisterFunctionName(578, "GetGlobalBoolean");
            RegisterFunctionName(579, "SetGlobalBoolean");
            RegisterFunctionName(580, "GetGlobalNumber");
            RegisterFunctionName(581, "SetGlobalNumber");
            
            // Local variables
            RegisterFunctionName(679, "GetLocalInt");
            RegisterFunctionName(680, "SetLocalInt");
            RegisterFunctionName(681, "GetLocalNumber");
            RegisterFunctionName(682, "SetLocalNumber");
            
            // Eclipse-specific functions (common patterns across Dragon Age and Mass Effect)
            RegisterFunctionName(100, "SpawnCreature");
            RegisterFunctionName(101, "CreateItem");
            RegisterFunctionName(102, "DestroyObject");
            RegisterFunctionName(103, "GetArea");
            RegisterFunctionName(104, "GetModule");
            RegisterFunctionName(105, "GetNearestCreature");
            RegisterFunctionName(106, "GetNearestObject");
            RegisterFunctionName(107, "GetNearestObjectByTag");
            RegisterFunctionName(108, "GetFirstObjectInArea");
            RegisterFunctionName(109, "GetNextObjectInArea");
            RegisterFunctionName(110, "GetObjectType");
            RegisterFunctionName(111, "GetIsPC");
            RegisterFunctionName(112, "GetIsNPC");
            RegisterFunctionName(113, "GetIsCreature");
            RegisterFunctionName(114, "GetIsItem");
            RegisterFunctionName(115, "GetIsPlaceable");
            RegisterFunctionName(116, "GetIsDoor");
            RegisterFunctionName(117, "GetIsTrigger");
            RegisterFunctionName(118, "GetIsWaypoint");
            RegisterFunctionName(119, "GetIsArea");
            RegisterFunctionName(120, "GetIsModule");
            
            // Position and movement functions
            RegisterFunctionName(200, "GetPosition");
            RegisterFunctionName(201, "GetFacing");
            RegisterFunctionName(202, "SetPosition");
            RegisterFunctionName(203, "SetFacing");
            RegisterFunctionName(204, "MoveToObject");
            RegisterFunctionName(205, "MoveToLocation");
            RegisterFunctionName(206, "GetDistanceBetween");
            RegisterFunctionName(207, "GetDistanceBetween2D");
            
            // Combat functions
            RegisterFunctionName(300, "GetIsInCombat");
            RegisterFunctionName(301, "GetAttackTarget");
            RegisterFunctionName(302, "GetCurrentHP");
            RegisterFunctionName(303, "GetMaxHP");
            RegisterFunctionName(304, "SetCurrentHP");
            RegisterFunctionName(305, "ApplyDamage");
            RegisterFunctionName(306, "GetIsEnemy");
            RegisterFunctionName(307, "GetIsFriend");
            RegisterFunctionName(308, "GetIsNeutral");
            RegisterFunctionName(309, "GetFaction");
            RegisterFunctionName(310, "SetFaction");
            
            // Party management functions
            RegisterFunctionName(400, "GetPartyMemberCount");
            RegisterFunctionName(401, "GetPartyMemberByIndex");
            RegisterFunctionName(402, "IsObjectPartyMember");
            RegisterFunctionName(403, "AddPartyMember");
            RegisterFunctionName(404, "RemovePartyMember");
            RegisterFunctionName(405, "GetPlayerCharacter");
            
            // Dialogue and conversation functions
            RegisterFunctionName(500, "StartConversation");
            RegisterFunctionName(501, "GetIsInConversation");
            RegisterFunctionName(502, "GetConversationSpeaker");
            RegisterFunctionName(503, "GetConversationTarget");
            RegisterFunctionName(504, "EndConversation");
            
            // Quest system functions
            RegisterFunctionName(600, "SetQuestCompleted");
            RegisterFunctionName(601, "GetQuestCompleted");
            RegisterFunctionName(602, "SetQuestActive");
            RegisterFunctionName(603, "GetQuestActive");
            RegisterFunctionName(604, "AddQuestEntry");
            RegisterFunctionName(605, "CompleteQuestEntry");
            
            // Item and inventory functions
            RegisterFunctionName(700, "CreateItemOnObject");
            RegisterFunctionName(701, "DestroyItem");
            RegisterFunctionName(702, "GetItemInSlot");
            RegisterFunctionName(703, "GetItemStackSize");
            RegisterFunctionName(704, "SetItemStackSize");
            RegisterFunctionName(705, "GetFirstItemInInventory");
            RegisterFunctionName(706, "GetNextItemInInventory");
            
            // Ability and spell functions
            RegisterFunctionName(800, "CastSpell");
            RegisterFunctionName(801, "CastSpellAtLocation");
            RegisterFunctionName(802, "CastSpellAtObject");
            RegisterFunctionName(803, "GetAbilityScore");
            RegisterFunctionName(804, "GetAbilityModifier");
            RegisterFunctionName(805, "GetHasAbility");
            RegisterFunctionName(806, "GetSpellLevel");
            
            // Area and module functions
            RegisterFunctionName(900, "GetAreaTag");
            RegisterFunctionName(901, "GetModuleFileName");
            RegisterFunctionName(902, "GetAreaByTag");
            RegisterFunctionName(903, "GetAreaOfObject");
            
            // Utility functions
            RegisterFunctionName(1000, "GetName");
            RegisterFunctionName(1001, "SetName");
            RegisterFunctionName(1002, "GetStringLength");
            RegisterFunctionName(1003, "GetSubString");
            RegisterFunctionName(1004, "FindSubString");
        }

        public override Variable CallEngineFunction(int routineId, IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            // Eclipse-specific function dispatch
            // Most basic functions (Random, PrintString, GetTag, GetObjectByTag, etc.) are already in BaseEngineApi
            // Eclipse-specific functions are implemented below
            
            switch (routineId)
            {
                // Common functions (delegated to base class)
                case 0: return Func_Random(args, ctx);
                case 1: return Func_PrintString(args, ctx);
                case 2: return Func_PrintFloat(args, ctx);
                case 3: return Func_FloatToString(args, ctx);
                case 4: return Func_PrintInteger(args, ctx);
                case 5: return Func_PrintObject(args, ctx);

                // Object functions
                case 27: return Func_GetPosition(args, ctx);
                case 28: return Func_GetFacing(args, ctx);
                case 41: return Func_GetDistanceToObject(args, ctx);
                case 42: return Func_GetIsObjectValid(args, ctx);

                // Tag functions
                case 168: return Func_GetTag(args, ctx);
                case 200: return Func_GetObjectByTag(args, ctx);

                // Global variables
                case 578: return Func_GetGlobalBoolean(args, ctx);
                case 579: return Func_SetGlobalBoolean(args, ctx);
                case 580: return Func_GetGlobalNumber(args, ctx);
                case 581: return Func_SetGlobalNumber(args, ctx);

                // Local variables
                case 679: return Func_GetLocalInt(args, ctx);
                case 680: return Func_SetLocalInt(args, ctx);
                case 681: return Func_GetLocalFloat(args, ctx);
                case 682: return Func_SetLocalFloat(args, ctx);

                // Eclipse-specific functions
                case 100: return Func_SpawnCreature(args, ctx);
                case 101: return Func_CreateItem(args, ctx);
                case 102: return Func_DestroyObject(args, ctx);
                case 103: return Func_GetArea(args, ctx);
                case 104: return Func_GetModule(args, ctx);
                case 105: return Func_GetNearestCreature(args, ctx);
                case 106: return Func_GetNearestObject(args, ctx);
                case 107: return Func_GetNearestObjectByTag(args, ctx);
                case 110: return Func_GetObjectType(args, ctx);
                case 111: return Func_GetIsPC(args, ctx);
                case 112: return Func_GetIsNPC(args, ctx);
                case 113: return Func_GetIsCreature(args, ctx);
                case 114: return Func_GetIsItem(args, ctx);
                case 115: return Func_GetIsPlaceable(args, ctx);
                case 116: return Func_GetIsDoor(args, ctx);
                case 201: return Func_GetFacing(args, ctx);
                case 202: return Func_SetPosition(args, ctx);
                case 203: return Func_SetFacing(args, ctx);
                case 206: return Func_GetDistanceBetween(args, ctx);
                case 207: return Func_GetDistanceBetween2D(args, ctx);
                case 300: return Func_GetIsInCombat(args, ctx);
                case 301: return Func_GetAttackTarget(args, ctx);
                case 302: return Func_GetCurrentHP(args, ctx);
                case 303: return Func_GetMaxHP(args, ctx);
                case 304: return Func_SetCurrentHP(args, ctx);
                case 306: return Func_GetIsEnemy(args, ctx);
                case 307: return Func_GetIsFriend(args, ctx);
                case 308: return Func_GetIsNeutral(args, ctx);
                case 309: return Func_GetFaction(args, ctx);
                case 310: return Func_SetFaction(args, ctx);
                case 400: return Func_GetPartyMemberCount(args, ctx);
                case 401: return Func_GetPartyMemberByIndex(args, ctx);
                case 402: return Func_IsObjectPartyMember(args, ctx);
                case 403: return Func_AddPartyMember(args, ctx);
                case 404: return Func_RemovePartyMember(args, ctx);
                case 405: return Func_GetPlayerCharacter(args, ctx);
                case 500: return Func_StartConversation(args, ctx);
                case 501: return Func_GetIsInConversation(args, ctx);
                case 600: return Func_SetQuestCompleted(args, ctx);
                case 601: return Func_GetQuestCompleted(args, ctx);
                case 700: return Func_CreateItemOnObject(args, ctx);
                case 701: return Func_DestroyItem(args, ctx);
                case 702: return Func_GetItemInSlot(args, ctx);
                case 703: return Func_GetItemStackSize(args, ctx);
                case 704: return Func_SetItemStackSize(args, ctx);
                case 803: return Func_GetAbilityScore(args, ctx);
                case 804: return Func_GetAbilityModifier(args, ctx);
                case 900: return Func_GetAreaTag(args, ctx);
                case 901: return Func_GetModuleFileName(args, ctx);
                case 1000: return Func_GetName(args, ctx);
                case 1001: return Func_SetName(args, ctx);

                default:
                    // Fall back to unimplemented function logging
                    string funcName = GetFunctionName(routineId);
                    Console.WriteLine("[Eclipse] Unimplemented function: " + routineId + " (" + funcName + ")");
                    return Variable.Void();
            }
        }

        #region Eclipse-Specific Functions

        /// <summary>
        /// GetPosition(object oObject) - Returns the position vector of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetPosition implementation
        /// Eclipse engines (Dragon Age, Mass Effect) use 3D position vectors for object placement
        /// Returns Vector3 with X, Y, Z coordinates
        /// </remarks>
        private Variable Func_GetPosition(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            
            if (entity != null)
            {
                ITransformComponent transform = entity.GetComponent<ITransformComponent>();
                if (transform != null)
                {
                    return Variable.FromVector(transform.Position);
                }
            }
            
            return Variable.FromVector(Vector3.Zero);
        }

        /// <summary>
        /// GetFacing(object oObject) - Returns the facing direction (in degrees) of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetFacing implementation
        /// Facing is expressed as degrees from East (0.0 = East, 90.0 = North, 180.0 = West, 270.0 = South)
        /// </remarks>
        private Variable Func_GetFacing(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            
            if (entity != null)
            {
                ITransformComponent transform = entity.GetComponent<ITransformComponent>();
                if (transform != null)
                {
                    // Convert facing angle (radians) to degrees from East
                    float facingDegrees = (float)(transform.Facing * 180.0 / Math.PI);
                    if (facingDegrees < 0) facingDegrees += 360.0f;
                    return Variable.FromFloat(facingDegrees);
                }
            }
            
            return Variable.FromFloat(0.0f);
        }

        /// <summary>
        /// SetPosition(object oObject, vector vPosition) - Sets the position of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: SetPosition implementation
        /// Moves object to specified 3D position
        /// </remarks>
        private Variable Func_SetPosition(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            Vector3 position = args.Count > 1 ? args[1].AsVector() : Vector3.Zero;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                ITransformComponent transform = entity.GetComponent<ITransformComponent>();
                if (transform != null)
                {
                    transform.Position = position;
                }
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// SetFacing(object oObject, float fDirection) - Sets the facing direction of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: SetFacing implementation
        /// fDirection is expressed as degrees from East (0.0 = East, 90.0 = North, 180.0 = West, 270.0 = South)
        /// </remarks>
        private Variable Func_SetFacing(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            float direction = args.Count > 1 ? args[1].AsFloat() : 0.0f;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                ITransformComponent transform = entity.GetComponent<ITransformComponent>();
                if (transform != null)
                {
                    // Convert degrees to radians for facing angle
                    float radians = (float)(direction * Math.PI / 180.0);
                    transform.Facing = radians;
                }
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// GetDistanceBetween(object oObject1, object oObject2) - Returns the 3D distance between two objects
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetDistanceBetween implementation
        /// Calculates 3D Euclidean distance between two objects
        /// </remarks>
        private Variable Func_GetDistanceBetween(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId1 = args.Count > 0 ? args[0].AsObjectId() : ObjectInvalid;
            uint objectId2 = args.Count > 1 ? args[1].AsObjectId() : ObjectInvalid;
            
            Core.Interfaces.IEntity entity1 = ResolveObject(objectId1, ctx);
            Core.Interfaces.IEntity entity2 = ResolveObject(objectId2, ctx);
            
            if (entity1 != null && entity2 != null)
            {
                ITransformComponent transform1 = entity1.GetComponent<ITransformComponent>();
                ITransformComponent transform2 = entity2.GetComponent<ITransformComponent>();
                
                if (transform1 != null && transform2 != null)
                {
                    float distance = Vector3.Distance(transform1.Position, transform2.Position);
                    return Variable.FromFloat(distance);
                }
            }
            
            return Variable.FromFloat(-1.0f);
        }

        /// <summary>
        /// GetDistanceBetween2D(object oObject1, object oObject2) - Returns the 2D distance between two objects (ignoring Z)
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetDistanceBetween2D implementation
        /// Calculates 2D Euclidean distance (X, Y only) between two objects
        /// </remarks>
        private Variable Func_GetDistanceBetween2D(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId1 = args.Count > 0 ? args[0].AsObjectId() : ObjectInvalid;
            uint objectId2 = args.Count > 1 ? args[1].AsObjectId() : ObjectInvalid;
            
            Core.Interfaces.IEntity entity1 = ResolveObject(objectId1, ctx);
            Core.Interfaces.IEntity entity2 = ResolveObject(objectId2, ctx);
            
            if (entity1 != null && entity2 != null)
            {
                ITransformComponent transform1 = entity1.GetComponent<ITransformComponent>();
                ITransformComponent transform2 = entity2.GetComponent<ITransformComponent>();
                
                if (transform1 != null && transform2 != null)
                {
                    Vector3 pos1 = transform1.Position;
                    Vector3 pos2 = transform2.Position;
                    float dx = pos2.X - pos1.X;
                    float dy = pos2.Y - pos1.Y;
                    float distance = (float)Math.Sqrt(dx * dx + dy * dy);
                    return Variable.FromFloat(distance);
                }
            }
            
            return Variable.FromFloat(-1.0f);
        }

        /// <summary>
        /// SpawnCreature(string sTemplate, vector vPosition, float fFacing) - Spawns a creature at the specified location
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: SpawnCreature implementation
        /// Creates a creature entity from template at specified position and facing
        /// Returns the object ID of the spawned creature, or OBJECT_INVALID on failure
        /// </remarks>
        private Variable Func_SpawnCreature(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            string template = args.Count > 0 ? args[0].AsString() : string.Empty;
            Vector3 position = args.Count > 1 ? args[1].AsVector() : Vector3.Zero;
            float facing = args.Count > 2 ? args[2].AsFloat() : 0.0f;
            
            if (string.IsNullOrEmpty(template) || ctx.World == null)
            {
                return Variable.FromObject(ObjectInvalid);
            }
            
            // Convert facing from degrees to radians for IWorld.CreateEntity
            float facingRadians = (float)(facing * Math.PI / 180.0);
            
            // Create creature entity using IWorld.CreateEntity
            // Note: Eclipse uses template-based spawning, but IWorld.CreateEntity uses ObjectType
            // For now, create as ObjectType.Creature - template loading would need IEntityTemplate system
            Core.Interfaces.IEntity creature = ctx.World.CreateEntity(Core.Enums.ObjectType.Creature, position, facingRadians);
            if (creature != null)
            {
                // Set template tag if provided (for later template loading)
                if (!string.IsNullOrEmpty(template))
                {
                    creature.SetData("TemplateResRef", template);
                }
                
                return Variable.FromObject(creature.ObjectId);
            }
            
            return Variable.FromObject(ObjectInvalid);
        }

        /// <summary>
        /// CreateItem(string sTemplate, object oTarget, int nStackSize) - Creates an item and places it on/in an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: CreateItem implementation
        /// Creates an item from template and adds it to target object's inventory
        /// Returns the object ID of the created item, or OBJECT_INVALID on failure
        /// </remarks>
        private Variable Func_CreateItem(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            string template = args.Count > 0 ? args[0].AsString() : string.Empty;
            uint targetId = args.Count > 1 ? args[1].AsObjectId() : ObjectSelf;
            int stackSize = args.Count > 2 ? args[2].AsInt() : 1;
            
            if (string.IsNullOrEmpty(template) || ctx.World == null)
            {
                return Variable.FromObject(ObjectInvalid);
            }
            
            Core.Interfaces.IEntity target = ResolveObject(targetId, ctx);
            if (target == null)
            {
                return Variable.FromObject(ObjectInvalid);
            }
            
            // Create item entity using IWorld.CreateEntity
            // Get target's position for item creation
            ITransformComponent targetTransform = target.GetComponent<ITransformComponent>();
            Vector3 itemPosition = targetTransform != null ? targetTransform.Position : Vector3.Zero;
            
            Core.Interfaces.IEntity item = ctx.World.CreateEntity(Core.Enums.ObjectType.Item, itemPosition, 0.0f);
            if (item != null)
            {
                // Set template tag if provided
                if (!string.IsNullOrEmpty(template))
                {
                    item.SetData("TemplateResRef", template);
                }
                
                // Set stack size in item component
                IItemComponent itemComp = item.GetComponent<IItemComponent>();
                if (itemComp != null)
                {
                    itemComp.StackSize = Math.Max(1, stackSize);
                }
                
                // Add item to target's inventory
                IInventoryComponent inventory = target.GetComponent<IInventoryComponent>();
                if (inventory != null)
                {
                    if (inventory.AddItem(item))
                    {
                        return Variable.FromObject(item.ObjectId);
                    }
                    else
                    {
                        // Inventory full, destroy item
                        ctx.World.DestroyEntity(item.ObjectId);
                    }
                }
                else
                {
                    // Target has no inventory, destroy item
                    ctx.World.DestroyEntity(item.ObjectId);
                }
            }
            
            return Variable.FromObject(ObjectInvalid);
        }

        /// <summary>
        /// CreateItemOnObject(string sTemplate, object oTarget, int nStackSize) - Creates an item and places it on/in an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: CreateItemOnObject implementation
        /// Alias for CreateItem - creates item and adds to target's inventory
        /// </remarks>
        private Variable Func_CreateItemOnObject(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            return Func_CreateItem(args, ctx);
        }

        /// <summary>
        /// DestroyObject(object oObject) - Destroys an object, removing it from the world
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: DestroyObject implementation
        /// Removes object from world and frees its resources
        /// </remarks>
        private Variable Func_DestroyObject(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectInvalid;
            
            if (objectId == ObjectInvalid || ctx.World == null)
            {
                return Variable.Void();
            }
            
            Core.Interfaces.IEntity entity = ctx.World.GetEntity(objectId);
            if (entity != null)
            {
                Console.WriteLine("[Eclipse] DestroyObject: Object 0x{0:X8}", objectId);
                // Destroy entity using IWorld.DestroyEntity
                ctx.World.DestroyEntity(objectId);
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// DestroyItem(object oItem) - Destroys an item object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: DestroyItem implementation
        /// Alias for DestroyObject for items
        /// </remarks>
        private Variable Func_DestroyItem(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            return Func_DestroyObject(args, ctx);
        }

        /// <summary>
        /// GetNearestCreature(int nFirstCriteriaType, int nFirstCriteriaValue, object oTarget, int nNth, int nSecondCriteriaType, int nSecondCriteriaValue, int nThirdCriteriaType, int nThirdCriteriaValue) - Finds the nearest creature matching criteria
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetNearestCreature implementation
        /// Searches for creatures matching specified criteria and returns the nth nearest match
        /// </remarks>
        private Variable Func_GetNearestCreature(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint targetId = args.Count > 2 ? args[2].AsObjectId() : ObjectSelf;
            int nth = args.Count > 3 ? args[3].AsInt() : 0;
            
            Core.Interfaces.IEntity target = ResolveObject(targetId, ctx);
            if (target == null || ctx.World == null)
            {
                return Variable.FromObject(ObjectInvalid);
            }
            
            ITransformComponent targetTransform = target.GetComponent<ITransformComponent>();
            if (targetTransform == null)
            {
                return Variable.FromObject(ObjectInvalid);
            }
            
            Console.WriteLine("[Eclipse] GetNearestCreature: Target 0x{0:X8}, nth {1}", targetId, nth);
            
            // Search for creatures within reasonable radius (100 units default)
            // Get all creatures in radius, filter by criteria, sort by distance, return nth
            const float searchRadius = 100.0f;
            var candidates = new List<Tuple<Core.Interfaces.IEntity, float>>();
            
            foreach (Core.Interfaces.IEntity entity in ctx.World.GetEntitiesInRadius(targetTransform.Position, searchRadius, Core.Enums.ObjectType.Creature))
            {
                if (entity == null || entity.ObjectId == target.ObjectId)
                {
                    continue;
                }
                
                // Check if entity is a creature (has stats component)
                IStatsComponent stats = entity.GetComponent<IStatsComponent>();
                if (stats == null)
                {
                    continue;
                }
                
                // Apply criteria matching (nFirstCriteriaType, nFirstCriteriaValue, etc.)
                // Criteria types: 0=None, 1=Perception, 2=Disposition, 3=Reputation, 4=Team, 5=Reaction, 6=Class, 7=Race, 8=Hp, 9=Tag, 10=NotDead, 11=InCombat, 12=TargetType, 13=CreatureType, 14=Allegiance, 15=Gender, 16=Player, 17=Party, 18=Area, 19=Location, 20=LineOfSight, 21=Distance, 22=HasItem, 23=HasSpell, 24=HasSkill, 25=HasFeat, 26=HasTalent, 27=HasEffect, 28=HasVariable, 29=HasLocalVariable, 30=HasGlobalVariable, 31=HasFaction, 32=HasAlignment, 33=HasGoodEvil, 34=HasLawfulChaotic, 35=HasLevel, 36=HasClass, 37=HasRace, 38=HasGender, 39=HasSubrace, 40=HasDeity, 41=HasDomain, 42=HasDomainSource, 43=HasAbilityScore, 44=HasAbilityModifier, 45=HasSkillRank, 46=HasFeatCount, 47=HasSpellCount, 48=HasTalentCount, 49=HasEffectCount, 50=HasItemCount, 51=HasVariableValue, 52=HasLocalVariableValue, 53=HasGlobalVariableValue, 54=HasFactionValue, 55=HasAlignmentValue, 56=HasGoodEvilValue, 57=HasLawfulChaoticValue, 58=HasLevelValue, 59=HasClassValue, 60=HasRaceValue, 61=HasGenderValue, 62=HasSubraceValue, 63=HasDeityValue, 64=HasDomainValue, 65=HasDomainSourceValue, 66=HasAbilityScoreValue, 67=HasAbilityModifierValue, 68=HasSkillRankValue, 69=HasFeatCountValue, 70=HasSpellCountValue, 71=HasTalentCountValue, 72=HasEffectCountValue, 73=HasItemCountValue
                // For now, accept all creatures (full criteria matching requires extensive implementation)
                // This is a placeholder until full criteria system is implemented
                
                // Calculate distance
                ITransformComponent entityTransform = entity.GetComponent<ITransformComponent>();
                if (entityTransform != null)
                {
                    float distance = Vector3.Distance(targetTransform.Position, entityTransform.Position);
                    candidates.Add(new Tuple<Core.Interfaces.IEntity, float>(entity, distance));
                }
            }
            
            // Sort by distance
            candidates.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            
            // Return nth nearest creature
            if (nth >= 0 && nth < candidates.Count)
            {
                return Variable.FromObject(candidates[nth].Item1.ObjectId);
            }
            
            return Variable.FromObject(ObjectInvalid);
        }

        /// <summary>
        /// GetNearestObject(int nObjectType, object oTarget, int nNth) - Finds the nearest object of specified type
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetNearestObject implementation
        /// Searches for objects of specified type and returns the nth nearest match
        /// </remarks>
        private Variable Func_GetNearestObject(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            int objectType = args.Count > 0 ? args[0].AsInt() : 0;
            uint targetId = args.Count > 1 ? args[1].AsObjectId() : ObjectSelf;
            int nth = args.Count > 2 ? args[2].AsInt() : 0;
            
            Core.Interfaces.IEntity target = ResolveObject(targetId, ctx);
            if (target == null || ctx.World == null)
            {
                return Variable.FromObject(ObjectInvalid);
            }
            
            ITransformComponent targetTransform = target.GetComponent<ITransformComponent>();
            if (targetTransform == null)
            {
                return Variable.FromObject(ObjectInvalid);
            }
            
            Console.WriteLine("[Eclipse] GetNearestObject: Type {0}, Target 0x{1:X8}, nth {2}", objectType, targetId, nth);
            // Convert objectType to ObjectType enum mask
            // Eclipse object types: 1=Creature, 2=Item, 4=Trigger, 8=Door, 64=Placeable, etc.
            Core.Enums.ObjectType typeMask = Core.Enums.ObjectType.None;
            if ((objectType & 1) != 0) typeMask |= Core.Enums.ObjectType.Creature;
            if ((objectType & 2) != 0) typeMask |= Core.Enums.ObjectType.Item;
            if ((objectType & 4) != 0) typeMask |= Core.Enums.ObjectType.Trigger;
            if ((objectType & 8) != 0) typeMask |= Core.Enums.ObjectType.Door;
            if ((objectType & 64) != 0) typeMask |= Core.Enums.ObjectType.Placeable;
            
            // If no type specified, search all types
            if (typeMask == Core.Enums.ObjectType.None)
            {
                typeMask = Core.Enums.ObjectType.All;
            }
            
            // Search for objects within reasonable radius (100 units default)
            const float searchRadius = 100.0f;
            var candidates = new List<Tuple<Core.Interfaces.IEntity, float>>();
            
            foreach (Core.Interfaces.IEntity entity in ctx.World.GetEntitiesInRadius(targetTransform.Position, searchRadius, typeMask))
            {
                if (entity == null || entity.ObjectId == target.ObjectId)
                {
                    continue;
                }
                
                // Calculate distance
                ITransformComponent entityTransform = entity.GetComponent<ITransformComponent>();
                if (entityTransform != null)
                {
                    float distance = Vector3.Distance(targetTransform.Position, entityTransform.Position);
                    candidates.Add(new Tuple<Core.Interfaces.IEntity, float>(entity, distance));
                }
            }
            
            // Sort by distance
            candidates.Sort((a, b) => a.Item2.CompareTo(b.Item2));
            
            // Return nth nearest object
            if (nth >= 0 && nth < candidates.Count)
            {
                return Variable.FromObject(candidates[nth].Item1.ObjectId);
            }
            
            return Variable.FromObject(ObjectInvalid);
        }

        /// <summary>
        /// GetObjectType(object oObject) - Returns the type of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetObjectType implementation
        /// Returns integer constant representing object type (CREATURE, ITEM, PLACEABLE, DOOR, TRIGGER, etc.)
        /// </remarks>
        private Variable Func_GetObjectType(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity == null)
            {
                return Variable.FromInt(0); // OBJECT_TYPE_INVALID
            }
            
            // Determine object type from components
            if (entity.GetComponent<IStatsComponent>() != null)
            {
                // Has stats - likely a creature
                return Variable.FromInt(1); // OBJECT_TYPE_CREATURE
            }
            else if (entity.GetComponent<IItemComponent>() != null)
            {
                return Variable.FromInt(2); // OBJECT_TYPE_ITEM
            }
            else if (entity.GetComponent<IPlaceableComponent>() != null)
            {
                return Variable.FromInt(64); // OBJECT_TYPE_PLACEABLE
            }
            else if (entity.GetComponent<IDoorComponent>() != null)
            {
                return Variable.FromInt(8); // OBJECT_TYPE_DOOR
            }
            else if (entity.GetComponent<ITriggerComponent>() != null)
            {
                return Variable.FromInt(4); // OBJECT_TYPE_TRIGGER
            }
            
            return Variable.FromInt(0); // OBJECT_TYPE_INVALID
        }

        /// <summary>
        /// GetIsPC(object oObject) - Returns true if object is a player character
        /// </summary>
        /// <summary>
        /// GetIsPC(object oObject) - Returns true if object is a player character
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetIsPC implementation
        /// Located via string reference: "PlayerCharacter" @ 0x00b08188 (daorigins.exe), @ 0x00beb508 (DragonAge2.exe)
        /// Original implementation: Checks if object is the player character
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetIsPC(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                // Check if entity is player character by tag or by being party leader
                string tag = entity.Tag ?? string.Empty;
                if (string.Equals(tag, "PlayerCharacter", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(tag, "player", StringComparison.OrdinalIgnoreCase))
                {
                    return Variable.FromInt(1);
                }
                
                // Check if entity is party member at index 0 (party leader)
                Variable partyLeader = Func_GetPartyMemberByIndex(new[] { Variable.FromInt(0) }, ctx);
                if (partyLeader != null && partyLeader.AsObjectId() == objectId)
                {
                    return Variable.FromInt(1);
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetIsNPC(object oObject) - Returns true if object is an NPC
        /// </summary>
        private Variable Func_GetIsNPC(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                // If it's a creature but not PC, it's an NPC
                if (entity.GetComponent<IStatsComponent>() != null)
                {
                    if (ctx.Caller == null || entity.ObjectId != ctx.Caller.ObjectId)
                    {
                        return Variable.FromInt(1);
                    }
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetIsCreature(object oObject) - Returns true if object is a creature
        /// </summary>
        private Variable Func_GetIsCreature(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null && entity.GetComponent<IStatsComponent>() != null)
            {
                return Variable.FromInt(1);
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetIsItem(object oObject) - Returns true if object is an item
        /// </summary>
        private Variable Func_GetIsItem(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null && entity.GetComponent<IItemComponent>() != null)
            {
                return Variable.FromInt(1);
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetIsPlaceable(object oObject) - Returns true if object is a placeable
        /// </summary>
        private Variable Func_GetIsPlaceable(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null && entity.GetComponent<IPlaceableComponent>() != null)
            {
                return Variable.FromInt(1);
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetIsDoor(object oObject) - Returns true if object is a door
        /// </summary>
        private Variable Func_GetIsDoor(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null && entity.GetComponent<IDoorComponent>() != null)
            {
                return Variable.FromInt(1);
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetIsInCombat(object oObject) - Returns true if object is in combat
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetIsInCombat implementation
        /// Located via string reference: "InCombat" @ 0x00af76b0 (daorigins.exe), @ 0x00bf4c10 (DragonAge2.exe)
        /// Original implementation: Checks if object is currently engaged in combat using CombatSystem
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetIsInCombat(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null && ctx.World != null && ctx.World.CombatSystem != null)
            {
                // Check if entity is in combat using CombatSystem
                bool inCombat = ctx.World.CombatSystem.IsInCombat(entity);
                return Variable.FromInt(inCombat ? 1 : 0);
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetAttackTarget(object oAttacker) - Returns the current attack target of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetAttackTarget implementation
        /// Returns the object ID of the current attack target, or OBJECT_INVALID if not attacking
        /// </remarks>
        /// <summary>
        /// GetAttackTarget(object oAttacker) - Returns the current attack target of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetAttackTarget implementation
        /// Located via string reference: "CombatTarget" @ 0x00af7840 (daorigins.exe), @ 0x00bf4dc0 (DragonAge2.exe)
        /// Original implementation: Returns the object ID of the current attack target from CombatSystem
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetAttackTarget(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint attackerId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity attacker = ResolveObject(attackerId, ctx);
            if (attacker != null && ctx.World != null && ctx.World.CombatSystem != null)
            {
                Core.Interfaces.IEntity target = ctx.World.CombatSystem.GetAttackTarget(attacker);
                if (target != null)
                {
                    return Variable.FromObject(target.ObjectId);
                }
            }
            
            return Variable.FromObject(ObjectInvalid);
        }

        /// <summary>
        /// GetCurrentHP(object oObject) - Returns the current hit points of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetCurrentHP implementation
        /// Returns current HP value from stats component
        /// </remarks>
        /// <summary>
        /// GetCurrentHP(object oObject) - Returns the current hit points of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetCurrentHP implementation
        /// Located via string reference: "CurrentHealth" @ 0x00aedb28 (daorigins.exe), @ 0x00beb46c (DragonAge2.exe)
        /// Original implementation: Returns current HP value from stats component
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetCurrentHP(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                IStatsComponent stats = entity.GetComponent<IStatsComponent>();
                if (stats != null)
                {
                    return Variable.FromInt(stats.CurrentHP);
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetMaxHP(object oObject) - Returns the maximum hit points of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetMaxHP implementation
        /// Returns maximum HP value from stats component
        /// </remarks>
        /// <summary>
        /// GetMaxHP(object oObject) - Returns the maximum hit points of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetMaxHP implementation
        /// Located via string reference: "MaxHealth" @ 0x00aedb1c (daorigins.exe), @ 0x00beb460 (DragonAge2.exe)
        /// Original implementation: Returns maximum HP value from stats component
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetMaxHP(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                IStatsComponent stats = entity.GetComponent<IStatsComponent>();
                if (stats != null)
                {
                    return Variable.FromInt(stats.MaxHP);
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// SetCurrentHP(object oObject, int nHP) - Sets the current hit points of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: SetCurrentHP implementation
        /// Sets current HP value in stats component, clamped to [0, MaxHP]
        /// </remarks>
        /// <summary>
        /// SetCurrentHP(object oObject, int nHP) - Sets the current hit points of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: SetCurrentHP implementation
        /// Located via string reference: "CurrentHealth" @ 0x00aedb28 (daorigins.exe), @ 0x00beb46c (DragonAge2.exe)
        /// Original implementation: Sets current HP value in stats component, clamped to [0, MaxHP]
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_SetCurrentHP(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            int hp = args.Count > 1 ? args[1].AsInt() : 0;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                IStatsComponent stats = entity.GetComponent<IStatsComponent>();
                if (stats != null)
                {
                    // Clamp HP to valid range [0, MaxHP]
                    int clampedHP = Math.Max(0, Math.Min(stats.MaxHP, hp));
                    stats.CurrentHP = clampedHP;
                }
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// GetIsEnemy(object oObject1, object oObject2) - Returns true if two objects are enemies
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetIsEnemy implementation
        /// Checks faction relationship between two objects
        /// </remarks>
        /// <summary>
        /// GetIsEnemy(object oObject1, object oObject2) - Returns true if two objects are enemies
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetIsEnemy implementation
        /// Located via string reference: "InCombat" @ 0x00af76b0 (daorigins.exe), @ 0x00bf4c10 (DragonAge2.exe)
        /// Original implementation: Checks faction relationship between two objects using IFactionComponent.IsHostile
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetIsEnemy(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId1 = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            uint objectId2 = args.Count > 1 ? args[1].AsObjectId() : ObjectInvalid;
            
            Core.Interfaces.IEntity entity1 = ResolveObject(objectId1, ctx);
            Core.Interfaces.IEntity entity2 = ResolveObject(objectId2, ctx);
            
            if (entity1 != null && entity2 != null)
            {
                IFactionComponent faction1 = entity1.GetComponent<IFactionComponent>();
                if (faction1 != null)
                {
                    return Variable.FromInt(faction1.IsHostile(entity2) ? 1 : 0);
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetIsFriend(object oObject1, object oObject2) - Returns true if two objects are friends
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetIsFriend implementation
        /// Located via string reference: "InCombat" @ 0x00af76b0 (daorigins.exe), @ 0x00bf4c10 (DragonAge2.exe)
        /// Original implementation: Checks faction relationship between two objects using IFactionComponent.IsFriendly
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetIsFriend(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId1 = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            uint objectId2 = args.Count > 1 ? args[1].AsObjectId() : ObjectInvalid;
            
            Core.Interfaces.IEntity entity1 = ResolveObject(objectId1, ctx);
            Core.Interfaces.IEntity entity2 = ResolveObject(objectId2, ctx);
            
            if (entity1 != null && entity2 != null)
            {
                IFactionComponent faction1 = entity1.GetComponent<IFactionComponent>();
                if (faction1 != null)
                {
                    return Variable.FromInt(faction1.IsFriendly(entity2) ? 1 : 0);
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetIsNeutral(object oObject1, object oObject2) - Returns true if two objects are neutral to each other
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetIsNeutral implementation
        /// Located via string reference: "InCombat" @ 0x00af76b0 (daorigins.exe), @ 0x00bf4c10 (DragonAge2.exe)
        /// Original implementation: Checks faction relationship between two objects using IFactionComponent.IsNeutral
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetIsNeutral(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId1 = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            uint objectId2 = args.Count > 1 ? args[1].AsObjectId() : ObjectInvalid;
            
            Core.Interfaces.IEntity entity1 = ResolveObject(objectId1, ctx);
            Core.Interfaces.IEntity entity2 = ResolveObject(objectId2, ctx);
            
            if (entity1 != null && entity2 != null)
            {
                IFactionComponent faction1 = entity1.GetComponent<IFactionComponent>();
                if (faction1 != null)
                {
                    return Variable.FromInt(faction1.IsNeutral(entity2) ? 1 : 0);
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetFaction(object oObject) - Returns the faction ID of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetFaction implementation
        /// Located via string reference: "Faction" @ 0x007c0ca0 (swkotor2.exe pattern, Eclipse uses similar system)
        /// Original implementation: Returns faction ID from IFactionComponent.FactionId
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetFaction(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                IFactionComponent faction = entity.GetComponent<IFactionComponent>();
                if (faction != null)
                {
                    return Variable.FromInt(faction.FactionId);
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// SetFaction(object oObject, int nFaction) - Sets the faction of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: SetFaction implementation
        /// Located via string reference: "Faction" @ 0x007c0ca0 (swkotor2.exe pattern, Eclipse uses similar system)
        /// Original implementation: Sets faction ID in IFactionComponent.FactionId
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_SetFaction(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            int faction = args.Count > 1 ? args[1].AsInt() : 0;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                IFactionComponent factionComp = entity.GetComponent<IFactionComponent>();
                if (factionComp != null)
                {
                    factionComp.FactionId = faction;
                }
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// GetPartyMemberCount() - Returns the number of party members
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetPartyMemberCount implementation
        /// Located via string reference: "SelectPartyMemberIndexMessage" @ 0x00aec88c (daorigins.exe), @ 0x00be3e28 (DragonAge2.exe)
        /// Original implementation: Returns count of active party members from party system
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// Note: This implementation uses entity data storage as a temporary solution until proper party system interface is available
        /// </remarks>
        private Variable Func_GetPartyMemberCount(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            if (ctx == null || ctx.World == null)
            {
                return Variable.FromInt(0);
            }
            
            // Count entities marked as party members
            int count = 0;
            foreach (Core.Interfaces.IEntity entity in ctx.World.GetAllEntities())
            {
                if (entity != null && entity.HasData("IsPartyMember") && entity.GetData<bool>("IsPartyMember"))
                {
                    count++;
                }
            }
            
            return Variable.FromInt(count);
        }

        /// <summary>
        /// GetPartyMemberByIndex(int nIndex) - Returns the party member at the specified index
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetPartyMemberByIndex implementation
        /// Located via string reference: "SelectPartyMemberIndexMessage" @ 0x00aec88c (daorigins.exe), @ 0x00be3e28 (DragonAge2.exe)
        /// Original implementation: Returns object ID of party member at index from party system
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// Note: This implementation uses entity data storage as a temporary solution until proper party system interface is available
        /// </remarks>
        private Variable Func_GetPartyMemberByIndex(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            int index = args.Count > 0 ? args[0].AsInt() : 0;
            
            if (ctx == null || ctx.World == null || index < 0)
            {
                return Variable.FromObject(ObjectInvalid);
            }
            
            // Collect party members
            var partyMembers = new List<Core.Interfaces.IEntity>();
            foreach (Core.Interfaces.IEntity entity in ctx.World.GetAllEntities())
            {
                if (entity != null && entity.HasData("IsPartyMember") && entity.GetData<bool>("IsPartyMember"))
                {
                    partyMembers.Add(entity);
                }
            }
            
            // Return party member at index
            if (index >= 0 && index < partyMembers.Count)
            {
                return Variable.FromObject(partyMembers[index].ObjectId);
            }
            
            return Variable.FromObject(ObjectInvalid);
        }

        /// <summary>
        /// IsObjectPartyMember(object oObject) - Returns true if object is a party member
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: IsObjectPartyMember implementation
        /// Located via string reference: "SelectPartyMemberIndexMessage" @ 0x00aec88c (daorigins.exe), @ 0x00be3e28 (DragonAge2.exe)
        /// Original implementation: Checks if object is a party member using party system
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// Note: This implementation uses entity data storage as a temporary solution until proper party system interface is available
        /// </remarks>
        private Variable Func_IsObjectPartyMember(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null && entity.HasData("IsPartyMember"))
            {
                return Variable.FromInt(entity.GetData<bool>("IsPartyMember") ? 1 : 0);
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// AddPartyMember(object oCreature) - Adds a creature to the party
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: AddPartyMember implementation
        /// Located via string reference: "SelectPartyMemberIndexMessage" @ 0x00aec88c (daorigins.exe), @ 0x00be3e28 (DragonAge2.exe)
        /// Original implementation: Adds creature to party using party system
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// Note: This implementation uses entity data storage as a temporary solution until proper party system interface is available
        /// </remarks>
        private Variable Func_AddPartyMember(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint creatureId = args.Count > 0 ? args[0].AsObjectId() : ObjectInvalid;
            
            Core.Interfaces.IEntity creature = ResolveObject(creatureId, ctx);
            if (creature != null)
            {
                // Mark entity as party member
                creature.SetData("IsPartyMember", true);
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// RemovePartyMember(object oCreature) - Removes a creature from the party
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: RemovePartyMember implementation
        /// Located via string reference: "SelectPartyMemberIndexMessage" @ 0x00aec88c (daorigins.exe), @ 0x00be3e28 (DragonAge2.exe)
        /// Original implementation: Removes creature from party using party system
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// Note: This implementation uses entity data storage as a temporary solution until proper party system interface is available
        /// </remarks>
        private Variable Func_RemovePartyMember(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint creatureId = args.Count > 0 ? args[0].AsObjectId() : ObjectInvalid;
            
            Core.Interfaces.IEntity creature = ResolveObject(creatureId, ctx);
            if (creature != null)
            {
                // Unmark entity as party member
                creature.SetData("IsPartyMember", false);
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// GetPlayerCharacter() - Returns the player character object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetPlayerCharacter implementation
        /// Eclipse engines (Dragon Age, Mass Effect) return the player-controlled character object
        /// Located via string reference: "PlayerCharacter" @ 0x00b08188 (daorigins.exe)
        /// Original implementation: Returns the player character entity, typically the party leader
        /// Implementation strategy:
        /// 1. Try to get party member at index 0 (party leader, which is typically the player character)
        /// 2. Fall back to entity lookup by tag "PlayerCharacter" (common Eclipse pattern)
        /// 3. Fall back to entity lookup by tag "player" (lowercase variant)
        /// 4. Return OBJECT_INVALID if player character cannot be found
        /// Cross-engine: Similar to Odyssey GetPartyLeader() which returns party member at index 0
        /// </remarks>
        private Variable Func_GetPlayerCharacter(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            if (ctx == null || ctx.World == null)
            {
                return Variable.FromObject(ObjectInvalid);
            }

            // Strategy 1: Try to get party member at index 0 (party leader is typically the player character)
            // This follows the pattern from Odyssey engine where GetPartyLeader() returns party member at index 0
            // GetPartyMemberByIndex is now implemented, so this strategy works correctly
            try
            {
                Variable partyLeader = Func_GetPartyMemberByIndex(new[] { Variable.FromInt(0) }, ctx);
                if (partyLeader != null && partyLeader.AsObjectId() != ObjectInvalid)
                {
                    return partyLeader;
                }
            }
            catch
            {
                // GetPartyMemberByIndex may not be fully implemented yet, fall through to other strategies
            }

            // Strategy 2: Try to find entity by tag "PlayerCharacter" (Eclipse engine pattern)
            // Based on string reference "PlayerCharacter" @ 0x00b08188 in daorigins.exe
            Core.Interfaces.IEntity playerEntity = ctx.World.GetEntityByTag("PlayerCharacter", 0);
            if (playerEntity != null)
            {
                return Variable.FromObject(playerEntity.ObjectId);
            }

            // Strategy 3: Try to find entity by tag "player" (lowercase variant, seen in string searches)
            playerEntity = ctx.World.GetEntityByTag("player", 0);
            if (playerEntity != null)
            {
                return Variable.FromObject(playerEntity.ObjectId);
            }

            // Strategy 4: Search through all entities for one marked as player character
            // This is a fallback if tag-based lookup fails
            foreach (Core.Interfaces.IEntity entity in ctx.World.GetAllEntities())
            {
                if (entity == null)
                {
                    continue;
                }

                // Check if entity has a component that indicates it's the player character
                // In Eclipse engines, player characters typically have specific tags or properties
                string tag = entity.Tag;
                if (!string.IsNullOrEmpty(tag))
                {
                    // Check for common player character tag patterns
                    if (string.Equals(tag, "PlayerCharacter", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(tag, "player", StringComparison.OrdinalIgnoreCase) ||
                        string.Equals(tag, "Player", StringComparison.OrdinalIgnoreCase))
                    {
                        return Variable.FromObject(entity.ObjectId);
                    }
                }
            }

            // Player character not found - return invalid object
            return Variable.FromObject(ObjectInvalid);
        }

        /// <summary>
        /// StartConversation(object oObject, string sConversation) - Starts a conversation with an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: StartConversation implementation
        /// Located via string reference: "ShowConversationGUIMessage" @ 0x00ae8a50 (daorigins.exe), @ 0x00bfca24 (DragonAge2.exe)
        /// Original implementation: Starts conversation using Eclipse dialogue system (UnrealScript message passing)
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// Note: This implementation stores conversation state in entity data until proper dialogue system interface is available
        /// </remarks>
        private Variable Func_StartConversation(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectInvalid;
            string conversation = args.Count > 1 ? args[1].AsString() : string.Empty;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null && !string.IsNullOrEmpty(conversation))
            {
                // Store conversation state in entity data
                entity.SetData("InConversation", true);
                entity.SetData("ConversationResRef", conversation);
                
                // Eclipse uses UnrealScript message passing: ShowConversationGUIMessage @ 0x00ae8a50 (daorigins.exe), @ 0x00bfca24 (DragonAge2.exe)
                // Full integration requires Eclipse dialogue system (Conversation class, message handlers)
                // Current implementation stores state for script queries; full dialogue UI integration pending
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// GetIsInConversation(object oObject) - Returns true if object is in a conversation
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetIsInConversation implementation
        /// Located via string reference: "Conversation" @ 0x00af5888 (daorigins.exe), @ 0x00bf8538 (DragonAge2.exe)
        /// Original implementation: Checks if object is currently in a conversation
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// Note: This implementation uses entity data storage until proper dialogue system interface is available
        /// </remarks>
        private Variable Func_GetIsInConversation(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null && entity.HasData("InConversation"))
            {
                return Variable.FromInt(entity.GetData<bool>("InConversation") ? 1 : 0);
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// SetQuestCompleted(string sQuest, int bCompleted) - Sets the completed state of a quest
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: SetQuestCompleted implementation
        /// Located via string reference: "QuestCompleted" @ 0x00b0847c (daorigins.exe), @ 0x00c00438 (DragonAge2.exe)
        /// Original implementation: Sets quest completed state in quest system using quest name/ID
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// Note: This implementation uses global variables until proper quest system interface is available
        /// </remarks>
        private Variable Func_SetQuestCompleted(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            string quest = args.Count > 0 ? args[0].AsString() : string.Empty;
            int completed = args.Count > 1 ? args[1].AsInt() : 0;
            
            if (!string.IsNullOrEmpty(quest) && ctx != null && ctx.Globals != null)
            {
                // Store quest completed state in global variables using quest name as key
                string questKey = "Quest_" + quest + "_Completed";
                ctx.Globals.SetGlobalBool(questKey, completed != 0);
                
                // Eclipse quest system uses "QuestCompleted" @ 0x00b0847c (daorigins.exe), @ 0x00c00438 (DragonAge2.exe)
                // Full integration requires Eclipse quest system (quest tracking, journal, quest entries)
                // Current implementation stores state in globals for script queries; full quest system integration pending
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// GetQuestCompleted(string sQuest) - Returns true if quest is completed
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetQuestCompleted implementation
        /// Located via string reference: "QuestCompleted" @ 0x00b0847c (daorigins.exe), @ 0x00c00438 (DragonAge2.exe)
        /// Original implementation: Gets quest completed state from quest system using quest name/ID
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// Note: This implementation uses global variables until proper quest system interface is available
        /// </remarks>
        private Variable Func_GetQuestCompleted(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            string quest = args.Count > 0 ? args[0].AsString() : string.Empty;
            
            if (!string.IsNullOrEmpty(quest) && ctx != null && ctx.Globals != null)
            {
                // Get quest completed state from global variables using quest name as key
                string questKey = "Quest_" + quest + "_Completed";
                bool completed = ctx.Globals.GetGlobalBool(questKey);
                return Variable.FromInt(completed ? 1 : 0);
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetItemInSlot(object oCreature, int nSlot) - Returns the item in a creature's inventory slot
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetItemInSlot implementation
        /// Located via string reference: "InventorySlot" @ 0x007bf7d0 (swkotor2.exe pattern, Eclipse uses similar system)
        /// Original implementation: Returns object ID of item in specified slot from IInventoryComponent.GetItemInSlot
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetItemInSlot(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint creatureId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            int slot = args.Count > 1 ? args[1].AsInt() : 0;
            
            Core.Interfaces.IEntity creature = ResolveObject(creatureId, ctx);
            if (creature != null)
            {
                IInventoryComponent inventory = creature.GetComponent<IInventoryComponent>();
                if (inventory != null)
                {
                    Core.Interfaces.IEntity item = inventory.GetItemInSlot(slot);
                    if (item != null)
                    {
                        return Variable.FromObject(item.ObjectId);
                    }
                }
            }
            
            return Variable.FromObject(ObjectInvalid);
        }

        /// <summary>
        /// GetItemStackSize(object oItem) - Returns the stack size of an item
        /// </summary>
        private Variable Func_GetItemStackSize(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint itemId = args.Count > 0 ? args[0].AsObjectId() : ObjectInvalid;
            
            Core.Interfaces.IEntity item = ResolveObject(itemId, ctx);
            if (item != null)
            {
                IItemComponent itemComp = item.GetComponent<IItemComponent>();
                if (itemComp != null)
                {
                    // Get stack size from item component
                    return Variable.FromInt(itemComp.StackSize);
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// SetItemStackSize(object oItem, int nStackSize) - Sets the stack size of an item
        /// </summary>
        private Variable Func_SetItemStackSize(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint itemId = args.Count > 0 ? args[0].AsObjectId() : ObjectInvalid;
            int stackSize = args.Count > 1 ? args[1].AsInt() : 1;
            
            Core.Interfaces.IEntity item = ResolveObject(itemId, ctx);
            if (item != null)
            {
                IItemComponent itemComp = item.GetComponent<IItemComponent>();
                if (itemComp != null)
                {
                    // Set stack size in item component with clamping
                    // Clamp stack size between 1 and maximum (default 100 for Eclipse engine)
                    // Note: Eclipse engine may use different item data files than Odyssey (baseitems.2da)
                    // This can be extended to look up max stack size from item templates if needed
                    const int defaultMaxStackSize = 100;
                    int clampedSize = Math.Max(1, Math.Min(defaultMaxStackSize, stackSize));
                    itemComp.StackSize = clampedSize;
                }
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// GetAbilityScore(object oCreature, int nAbility) - Returns the ability score of a creature
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetAbilityScore implementation
        /// Located via string reference: "STR", "DEX", "CON", "INT", "WIS", "CHA" (swkotor2.exe pattern, Eclipse uses similar system)
        /// Original implementation: Returns ability score (STR, DEX, CON, INT, WIS, CHA) from IStatsComponent.GetAbility
        /// Ability enum: 0=Strength, 1=Dexterity, 2=Constitution, 3=Intelligence, 4=Wisdom, 5=Charisma
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetAbilityScore(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint creatureId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            int ability = args.Count > 1 ? args[1].AsInt() : 0;
            
            Core.Interfaces.IEntity creature = ResolveObject(creatureId, ctx);
            if (creature != null)
            {
                IStatsComponent stats = creature.GetComponent<IStatsComponent>();
                if (stats != null)
                {
                    // Convert int to Ability enum (0-5 map to Strength-Charisma)
                    if (ability >= 0 && ability <= 5)
                    {
                        Core.Enums.Ability abilityEnum = (Core.Enums.Ability)ability;
                        return Variable.FromInt(stats.GetAbility(abilityEnum));
                    }
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetAbilityModifier(object oCreature, int nAbility) - Returns the ability modifier of a creature
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetAbilityModifier implementation
        /// Located via string reference: "STR", "DEX", "CON", "INT", "WIS", "CHA" (swkotor2.exe pattern, Eclipse uses similar system)
        /// Original implementation: Calculates ability modifier from ability score using IStatsComponent.GetAbilityModifier
        /// Ability modifier formula: (score - 10) / 2 (D20 system standard)
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetAbilityModifier(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint creatureId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            int ability = args.Count > 1 ? args[1].AsInt() : 0;
            
            Core.Interfaces.IEntity creature = ResolveObject(creatureId, ctx);
            if (creature != null)
            {
                IStatsComponent stats = creature.GetComponent<IStatsComponent>();
                if (stats != null)
                {
                    // Convert int to Ability enum (0-5 map to Strength-Charisma)
                    if (ability >= 0 && ability <= 5)
                    {
                        Core.Enums.Ability abilityEnum = (Core.Enums.Ability)ability;
                        return Variable.FromInt(stats.GetAbilityModifier(abilityEnum));
                    }
                }
            }
            
            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetAreaTag(object oArea) - Returns the tag of an area
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetAreaTag implementation
        /// Located via string reference: "AREANAME" @ 0x007be1dc (swkotor2.exe pattern, Eclipse uses similar system)
        /// Original implementation: Returns area tag from IArea.Tag or entity tag
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetAreaTag(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint areaId = args.Count > 0 ? args[0].AsObjectId() : ObjectInvalid;
            
            // Try to get area from world
            if (ctx != null && ctx.World != null && ctx.World.CurrentArea != null)
            {
                // If areaId matches current area or is invalid, return current area tag
                if (areaId == ObjectInvalid || ctx.World.CurrentArea.Tag != null)
                {
                    return Variable.FromString(ctx.World.CurrentArea.Tag ?? string.Empty);
                }
            }
            
            // Fallback: try to get entity by ID and return its tag
            if (areaId != ObjectInvalid)
            {
                Core.Interfaces.IEntity entity = ResolveObject(areaId, ctx);
                if (entity != null)
                {
                    return Variable.FromString(entity.Tag ?? string.Empty);
                }
            }
            
            return Variable.FromString(string.Empty);
        }

        /// <summary>
        /// GetModuleFileName() - Returns the filename of the current module
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetModuleFileName implementation
        /// Located via string reference: "MODULES" @ 0x00ad9810 (daorigins.exe), @ 0x00bf5d10 (DragonAge2.exe)
        /// Original implementation: Returns module filename from IModule.FileName or CurrentModuleName
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetModuleFileName(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            if (ctx != null && ctx.World != null && ctx.World.CurrentModule != null)
            {
                // Try to get module filename from IModule interface
                // If IModule has FileName property, use it; otherwise use module name
                string moduleName = ctx.World.CurrentModule.Name ?? string.Empty;
                return Variable.FromString(moduleName);
            }
            
            return Variable.FromString(string.Empty);
        }

        /// <summary>
        /// GetName(object oObject) - Returns the name of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: GetName implementation
        /// Located via string reference: Entity name storage (Eclipse uses entity data for names)
        /// Original implementation: Returns display name of object from entity data or tag
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_GetName(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                // Try to get name from entity data first
                if (entity.HasData("Name"))
                {
                    string name = entity.GetData<string>("Name");
                    if (!string.IsNullOrEmpty(name))
                    {
                        return Variable.FromString(name);
                    }
                }
                
                // Fallback to tag
                return Variable.FromString(entity.Tag ?? string.Empty);
            }
            
            return Variable.FromString(string.Empty);
        }

        /// <summary>
        /// SetName(object oObject, string sName) - Sets the name of an object
        /// </summary>
        /// <remarks>
        /// Based on Eclipse engine: SetName implementation
        /// Located via string reference: Entity name storage (Eclipse uses entity data for names)
        /// Original implementation: Sets display name of object in entity data
        /// Cross-engine: Common implementation for both daorigins.exe and DragonAge2.exe
        /// </remarks>
        private Variable Func_SetName(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            uint objectId = args.Count > 0 ? args[0].AsObjectId() : ObjectSelf;
            string name = args.Count > 1 ? args[1].AsString() : string.Empty;
            
            Core.Interfaces.IEntity entity = ResolveObject(objectId, ctx);
            if (entity != null)
            {
                // Set name in entity data
                entity.SetData("Name", name);
            }
            
            return Variable.Void();
        }

        /// <summary>
        /// Helper method to register function names for debugging
        /// </summary>
        private void RegisterFunctionName(int routineId, string name)
        {
            _functionNames[routineId] = name;
            _implementedFunctions.Add(routineId);
        }

        #endregion
    }
}

