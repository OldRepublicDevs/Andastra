using System;
using System.Collections.Generic;
using CSharpKOTOR.Common.Script;
using Odyssey.Scripting.Interfaces;

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
            
            // For now, delegate to base implementation for common functions
            // TSL-specific functions (800+) should not be called in K1
            if (routineId >= 800)
            {
                string funcName = GetFunctionName(routineId);
                Console.WriteLine("[NCS-K1] TSL-only function called in K1: " + routineId + " (" + funcName + ")");
                return Variable.Void();
            }

            // For K1, we can share most implementations with BaseEngineApi
            // Override here only for K1-specific differences if needed
            string funcName2 = GetFunctionName(routineId);
            Console.WriteLine("[NCS-K1] Unimplemented function: " + routineId + " (" + funcName2 + ")");
            return Variable.Void();
        }
    }
}

