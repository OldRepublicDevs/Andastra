using System;
using JetBrains.Annotations;

namespace Andastra.Runtime.Core.Module
{
    /// <summary>
    /// Manages module state flags matching the original engine behavior.
    /// </summary>
    /// <remarks>
    /// Module State Manager (swkotor2.exe: 0x006caab0 @ 0x006caab0):
    /// - Function signature: `undefined4 0x006caab0(char *param_1, int param_2)`
    /// - Parses server command strings like "S.Module.ModuleLoaded" or "S.Module.ModuleRunning"
    /// - Located via string references: "ModuleLoaded" @ 0x007bdd70, "ModuleRunning" @ 0x007bdd58
    /// - "ServerStatus" @ 0x00826e1c, "ModuleLoaded" @ 0x00826e24, "ModuleRunning" @ 0x00826e2c
    /// - Module state stored in DAT_008283d4 structure accessed via 0x00638850
    /// - State value stored at DAT_008283d4 + 4 (uint16)
    /// - Bit flags stored at DAT_008283d4 + offset (uint32 pointer at puVar6)
    /// 
    /// Original Implementation Details:
    /// - State 0 (Idle): Sets `*(undefined2 *)(DAT_008283d4 + 4) = 0`, sets bit flag `*puVar6 | 1`
    /// - State 1 (ModuleLoaded): Sets `*(undefined2 *)(DAT_008283d4 + 4) = 1`, sets bit flag `*puVar6 | 0x11` (0x10 | 0x1)
    /// - State 2 (ModuleRunning): Sets `*(undefined2 *)(DAT_008283d4 + 4) = 2`, sets bit flag `*puVar6 | 0x1`
    /// 
    /// Server Command Parsing:
    /// - Function parses commands starting with 'S.' prefix
    /// - "S.Module.ModuleLoaded" -> Sets state to ModuleLoaded (1)
    /// - "S.Module.ModuleRunning" -> Sets state to ModuleRunning (2)
    /// - "S.Module.Idle" or empty -> Sets state to Idle (0)
    /// 
    /// Bit Flag Behavior:
    /// - Bit 0 (0x1): Always set for all states (indicates module state is valid)
    /// - Bit 4 (0x10): Set only for ModuleLoaded state (indicates module is loaded but not running)
    /// - Bit flags are OR'd with existing flags (not replaced)
    /// </remarks>
    public class ModuleStateManager
    {
        private ModuleState _currentState;
        private uint _bitFlags;

        /// <summary>
        /// Gets the current module state.
        /// </summary>
        public ModuleState CurrentState
        {
            get { return _currentState; }
        }

        /// <summary>
        /// Gets the current bit flags.
        /// </summary>
        public uint BitFlags
        {
            get { return _bitFlags; }
        }

        /// <summary>
        /// Initializes a new instance of the ModuleStateManager.
        /// </summary>
        public ModuleStateManager()
        {
            _currentState = ModuleState.Idle;
            _bitFlags = 0;
        }

        /// <summary>
        /// Sets the module state, matching the original engine behavior at 0x006caab0.
        /// </summary>
        /// <param name="state">The new module state to set.</param>
        /// <remarks>
        /// swkotor2.exe: 0x006caab0 @ 0x006caab0 (server command parser):
        /// - Sets module state value at DAT_008283d4 + 4 (uint16)
        /// - Sets bit flags at DAT_008283d4 + offset (uint32 pointer at puVar6)
        /// - State 0 (Idle): Sets state to 0, sets bit flag | 1
        /// - State 1 (ModuleLoaded): Sets state to 1, sets bit flag | 0x11 (0x10 | 0x1)
        /// - State 2 (ModuleRunning): Sets state to 2, sets bit flag | 0x1
        /// </remarks>
        public void SetModuleState(ModuleState state)
        {
            _currentState = state;

            // Set bit flags according to original engine behavior
            // Bit 0 (0x1): Always set for all states (indicates module state is valid)
            _bitFlags |= 0x1;

            // Bit 4 (0x10): Set only for ModuleLoaded state (indicates module is loaded but not running)
            if (state == ModuleState.ModuleLoaded)
            {
                _bitFlags |= 0x10;
            }
            else
            {
                // Clear bit 4 for other states
                _bitFlags &= ~0x10u;
            }

            // Log state change (matching original engine debug output)
            string stateMessage = GetStateMessage(state);
            Console.WriteLine("[ModuleStateManager] " + stateMessage);
        }

        /// <summary>
        /// Parses a server command string and sets the module state accordingly.
        /// </summary>
        /// <param name="command">The server command string (e.g., "S.Module.ModuleLoaded").</param>
        /// <param name="param2">Optional second parameter (unused in original, kept for signature compatibility).</param>
        /// <returns>True if the command was parsed successfully, false otherwise.</returns>
        /// <remarks>
        /// swkotor2.exe: 0x006caab0 @ 0x006caab0 (server command parser):
        /// - Function signature: `undefined4 0x006caab0(char *param_1, int param_2)`
        /// - Parses server command strings starting with 'S.' prefix
        /// - "S.Module.ModuleLoaded" -> Sets state to ModuleLoaded (1)
        /// - "S.Module.ModuleRunning" -> Sets state to ModuleRunning (2)
        /// - "S.Module.Idle" or empty -> Sets state to Idle (0)
        /// - Returns non-zero on success, zero on failure
        /// </remarks>
        public bool ParseServerCommand([CanBeNull] string command, int param2 = 0)
        {
            if (string.IsNullOrEmpty(command))
            {
                SetModuleState(ModuleState.Idle);
                return true;
            }

            // Parse server commands starting with 'S.'
            if (command.StartsWith("S.", StringComparison.Ordinal))
            {
                string commandBody = command.Substring(2); // Skip "S." prefix

                // Parse "S.Module.ModuleLoaded" -> ModuleLoaded
                if (string.Equals(commandBody, "Module.ModuleLoaded", StringComparison.Ordinal))
                {
                    SetModuleState(ModuleState.ModuleLoaded);
                    return true;
                }

                // Parse "S.Module.ModuleRunning" -> ModuleRunning
                if (string.Equals(commandBody, "Module.ModuleRunning", StringComparison.Ordinal))
                {
                    SetModuleState(ModuleState.ModuleRunning);
                    return true;
                }

                // Parse "S.Module.Idle" -> Idle
                if (string.Equals(commandBody, "Module.Idle", StringComparison.Ordinal))
                {
                    SetModuleState(ModuleState.Idle);
                    return true;
                }
            }

            // Unknown command
            return false;
        }

        /// <summary>
        /// Gets the debug message for a module state, matching original engine output.
        /// </summary>
        /// <param name="state">The module state.</param>
        /// <returns>The debug message string.</returns>
        /// <remarks>
        /// swkotor2.exe: 0x006caab0 @ 0x006caab0 debug output:
        /// - State 0: ":: Server mode: Idle.\n" @ 0x007cbc80
        /// - State 1: ":: Server mode: Module Loaded.\n" @ 0x007cbc68
        /// - State 2: ":: Server mode: Module Running.\n" @ 0x007cbc44
        /// </remarks>
        private string GetStateMessage(ModuleState state)
        {
            switch (state)
            {
                case ModuleState.Idle:
                    return ":: Server mode: Idle.";
                case ModuleState.ModuleLoaded:
                    return ":: Server mode: Module Loaded.";
                case ModuleState.ModuleRunning:
                    return ":: Server mode: Module Running.";
                default:
                    return ":: Server mode: Unknown.";
            }
        }
    }
}
