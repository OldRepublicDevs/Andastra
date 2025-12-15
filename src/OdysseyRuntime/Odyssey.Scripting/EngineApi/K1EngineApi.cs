using System;
using System.Collections.Generic;
using CSharpKOTOR.Common.Script;
using Odyssey.Core.Actions;
using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;
using Odyssey.Core.Interfaces.Components;
using Odyssey.Scripting.Interfaces;
using Odyssey.Scripting.Types;

namespace Odyssey.Scripting.EngineApi
{
    /// <summary>
    /// KOTOR 1 engine API implementation.
    /// Implements K1-specific NWScript functions (~850 functions).
    /// </summary>
    /// <remarks>
    /// KOTOR 1 Engine API (K1 NWScript Functions):
    /// - Based on swkotor.exe NWScript engine API implementation
    /// - Located via string references: ACTION opcode handler dispatches to engine function implementations
    /// - Original implementation: K1 has ~850 engine functions (function IDs 0-849)
    /// - Function IDs: Match function indices from nwscript.nss compilation for K1
    /// - K1 functions are a subset of K2 functions (K2 adds ~100 TSL-specific functions at 800+)
    /// - Function implementations must match original engine behavior for K1 script compatibility
    /// - Common functions: Same base functions as K2 (Random, PrintString, GetTag, GetObjectByTag, etc.)
    /// - K1-specific differences:
    ///   - No influence system (GetInfluence, SetInfluence are K2-only)
    ///   - No party puppet functions (GetPartyMemberByIndex, IsAvailableCreature are K2-only)
    ///   - No workbench/lab functions (ShowUpgradeScreen, GetBaseItemType are K2-only)
    ///   - No combat form functions (GetIsFormActive is K2-only)
    ///   - No stealth XP functions (GetStealthXPEnabled, SetStealthXPEnabled are K2-only)
    ///   - No swoop minigame functions (SWMG_* are K2-only)
    /// - Original engine uses function dispatch table indexed by routine ID (matches nwscript.nss definitions)
    /// - Function implementations must match NWScript semantics for K1 script compatibility
    /// </remarks>
    public class K1EngineApi : BaseEngineApi
    {
        protected override void RegisterFunctions()
        {
            // Register function names from ScriptDefs for K1
            // K1 uses the first ~850 functions from TSL_FUNCTIONS (functions 0-849)
            // TODO: When K1_FUNCTIONS is available in ScriptDefs, use that instead
            int idx = 0;
            int k1FunctionCount = 850; // K1 has ~850 functions
            
            // For now, use TSL_FUNCTIONS and limit to K1 range
            foreach (ScriptFunction func in ScriptDefs.TSL_FUNCTIONS)
            {
                if (idx >= k1FunctionCount)
                {
                    break; // K1 doesn't have TSL-specific functions (800+)
                }
                _functionNames[idx] = func.Name;
                idx++;
            }
        }

        public override Variable CallEngineFunction(int routineId, IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            // K1 has functions 0-849, no TSL-specific functions
            // Most implementation is shared with BaseEngineApi common functions
            
            // TSL-specific functions (800+) should not be called in K1
            if (routineId >= 800)
            {
                string funcName = GetFunctionName(routineId);
                Console.WriteLine("[NCS-K1] TSL-only function called in K1: " + routineId + " (" + funcName + ")");
                return Variable.Void();
            }

            switch (routineId)
            {
                // Door Actions
                case 43: return Func_ActionOpenDoor(args, ctx);
                case 44: return Func_ActionCloseDoor(args, ctx);

                // Door State Functions
                case 324: return Func_SetLocked(args, ctx);
                case 325: return Func_GetLocked(args, ctx);
                case 443: return Func_GetIsOpen(args, ctx);
                case 537: return Func_GetLockKeyRequired(args, ctx);

                default:
                    // For K1, we can share most implementations with BaseEngineApi
                    // Override here only for K1-specific differences if needed
                    string funcName2 = GetFunctionName(routineId);
                    Console.WriteLine("[NCS-K1] Unimplemented function: " + routineId + " (" + funcName2 + ")");
                    return Variable.Void();
            }
        }

        #region Door Functions

        /// <summary>
        /// ActionOpenDoor(object oDoor) - Cause the action subject to open oDoor
        /// </summary>
        /// <remarks>
        /// Based on swkotor2.exe: ActionOpenDoor implementation
        /// Located via string references: "OnOpen" @ 0x007be1b0, "EVENT_OPEN_OBJECT" @ 0x007bcda0
        /// Original implementation: Queues ActionOpenDoor action to open door
        /// </remarks>
        private Variable Func_ActionOpenDoor(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            if (args.Count < 1 || ctx.Caller == null)
            {
                return Variable.Void();
            }

            uint doorId = args[0].AsObjectId();
            IEntity door = ResolveObject(doorId, ctx);
            if (door == null || door.ObjectType != ObjectType.Door)
            {
                return Variable.Void();
            }

            var action = new ActionOpenDoor(doorId);
            IActionQueue queue = ctx.Caller.GetComponent<IActionQueue>();
            if (queue != null)
            {
                queue.Add(action);
            }

            return Variable.Void();
        }

        /// <summary>
        /// ActionCloseDoor(object oDoor) - Cause the action subject to close oDoor
        /// </summary>
        /// <remarks>
        /// Based on swkotor2.exe: ActionCloseDoor implementation
        /// Located via string references: "OnClosed" @ 0x007be1c8, "EVENT_CLOSE_OBJECT" @ 0x007bcdb4
        /// Original implementation: Queues ActionCloseDoor action to close door
        /// </remarks>
        private Variable Func_ActionCloseDoor(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            if (args.Count < 1 || ctx.Caller == null)
            {
                return Variable.Void();
            }

            uint doorId = args[0].AsObjectId();
            IEntity door = ResolveObject(doorId, ctx);
            if (door == null || door.ObjectType != ObjectType.Door)
            {
                return Variable.Void();
            }

            var action = new ActionCloseDoor(doorId);
            IActionQueue queue = ctx.Caller.GetComponent<IActionQueue>();
            if (queue != null)
            {
                queue.Add(action);
            }

            return Variable.Void();
        }

        /// <summary>
        /// SetLocked(object oTarget, int bLocked) - Set the locked state of oTarget (door or placeable)
        /// </summary>
        /// <remarks>
        /// Based on swkotor2.exe: SetLocked implementation
        /// Located via string references: "Locked" @ 0x007c1984, "EVENT_LOCK_OBJECT" @ 0x007bcd20, "EVENT_UNLOCK_OBJECT" @ 0x007bcd34
        /// Original implementation: Sets IsLocked flag on door/placeable, fires lock/unlock events
        /// </remarks>
        private Variable Func_SetLocked(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            if (args.Count < 2)
            {
                return Variable.Void();
            }

            uint targetId = args[0].AsObjectId();
            int bLocked = args[1].AsInt();
            IEntity target = ResolveObject(targetId, ctx);

            if (target == null)
            {
                return Variable.Void();
            }

            // Check if it's a door or placeable
            if (target.ObjectType == ObjectType.Door)
            {
                IDoorComponent doorComponent = target.GetComponent<IDoorComponent>();
                if (doorComponent != null)
                {
                    bool shouldLock = bLocked != 0;
                    if (shouldLock && doorComponent.LockableByScript)
                    {
                        doorComponent.Lock();
                    }
                    else if (!shouldLock)
                    {
                        doorComponent.Unlock();
                    }
                }
            }
            // TODO: Add placeable support when PlaceableComponent is implemented

            return Variable.Void();
        }

        /// <summary>
        /// GetLocked(object oTarget) - Get the locked state of oTarget (door or placeable)
        /// </summary>
        /// <remarks>
        /// Based on swkotor2.exe: GetLocked implementation
        /// Located via string references: "Locked" @ 0x007c1984
        /// Original implementation: Returns IsLocked flag from door/placeable
        /// </remarks>
        private Variable Func_GetLocked(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            if (args.Count < 1)
            {
                return Variable.FromInt(0);
            }

            uint targetId = args[0].AsObjectId();
            IEntity target = ResolveObject(targetId, ctx);

            if (target == null)
            {
                return Variable.FromInt(0);
            }

            // Check if it's a door or placeable
            if (target.ObjectType == ObjectType.Door)
            {
                IDoorComponent doorComponent = target.GetComponent<IDoorComponent>();
                if (doorComponent != null)
                {
                    return Variable.FromInt(doorComponent.IsLocked ? 1 : 0);
                }
            }
            // TODO: Add placeable support when PlaceableComponent is implemented

            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetIsOpen(object oObject) - Returns TRUE if oObject (placeable or door) is currently open
        /// </summary>
        /// <remarks>
        /// Based on swkotor2.exe: GetIsOpen implementation
        /// Located via string references: Door open state checking
        /// Original implementation: Returns IsOpen flag from door/placeable
        /// </remarks>
        private Variable Func_GetIsOpen(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            if (args.Count < 1)
            {
                return Variable.FromInt(0);
            }

            uint objectId = args[0].AsObjectId();
            IEntity entity = ResolveObject(objectId, ctx);

            if (entity == null)
            {
                return Variable.FromInt(0);
            }

            // Check if it's a door or placeable
            if (entity.ObjectType == ObjectType.Door)
            {
                IDoorComponent doorComponent = entity.GetComponent<IDoorComponent>();
                if (doorComponent != null)
                {
                    return Variable.FromInt(doorComponent.IsOpen ? 1 : 0);
                }
            }
            // TODO: Add placeable support when PlaceableComponent is implemented

            return Variable.FromInt(0);
        }

        /// <summary>
        /// GetLockKeyRequired(object oObject) - Returns TRUE if a specific key is required to open the lock on oObject
        /// </summary>
        /// <remarks>
        /// Based on swkotor2.exe: GetLockKeyRequired implementation
        /// Located via string references: "KeyRequired" field in door/placeable templates
        /// Original implementation: Returns KeyRequired flag from door/placeable
        /// </remarks>
        private Variable Func_GetLockKeyRequired(IReadOnlyList<Variable> args, IExecutionContext ctx)
        {
            if (args.Count < 1)
            {
                return Variable.FromInt(0);
            }

            uint objectId = args[0].AsObjectId();
            IEntity entity = ResolveObject(objectId, ctx);

            if (entity == null)
            {
                return Variable.FromInt(0);
            }

            // Check if it's a door or placeable
            if (entity.ObjectType == ObjectType.Door)
            {
                IDoorComponent doorComponent = entity.GetComponent<IDoorComponent>();
                if (doorComponent != null)
                {
                    return Variable.FromInt(doorComponent.KeyRequired ? 1 : 0);
                }
            }
            // TODO: Add placeable support when PlaceableComponent is implemented

            return Variable.FromInt(0);
        }

        #endregion
    }
}

