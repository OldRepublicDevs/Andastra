namespace Andastra.Runtime.Core.Module
{
    /// <summary>
    /// Represents the current state of a module.
    /// </summary>
    /// <remarks>
    /// Module State Enum (swkotor2.exe: 0x006caab0 @ 0x006caab0):
    /// - Located via string references: "ModuleLoaded" @ 0x007bdd70, "ModuleRunning" @ 0x007bdd58
    /// - ":: Server mode: Idle.\n" @ 0x007cbc80 (state 0)
    /// - ":: Server mode: Module Loaded.\n" @ 0x007cbc68 (state 1)
    /// - ":: Server mode: Module Running.\n" @ 0x007cbc44 (state 2)
    /// - Original implementation: Module state stored in DAT_008283d4 structure at offset +4 (uint16)
    /// - State transitions: Idle -> ModuleLoaded -> ModuleRunning -> Idle
    /// - State 0 (Idle): No module loaded, game is idle
    /// - State 1 (ModuleLoaded): Module resources loaded but not running (before OnModuleStart)
    /// - State 2 (ModuleRunning): Module loaded and running (after OnModuleStart, gameplay active)
    /// </remarks>
    public enum ModuleState : ushort
    {
        /// <summary>
        /// Idle state - no module is loaded.
        /// </summary>
        /// <remarks>
        /// swkotor2.exe: 0x006caab0 @ 0x006caab0 line 181: ":: Server mode: Idle.\n"
        /// Sets `*(undefined2 *)(DAT_008283d4 + 4) = 0`, sets bit flag `*puVar6 | 1`
        /// </remarks>
        Idle = 0,

        /// <summary>
        /// Module loaded state - module resources are loaded but not running.
        /// </summary>
        /// <remarks>
        /// swkotor2.exe: 0x006caab0 @ 0x006caab0 line 190: ":: Server mode: Module Loaded.\n"
        /// Sets `*(undefined2 *)(DAT_008283d4 + 4) = 1`, sets bit flag `*puVar6 | 0x11` (0x10 | 0x1)
        /// Module is loaded but OnModuleStart has not been called yet
        /// </remarks>
        ModuleLoaded = 1,

        /// <summary>
        /// Module running state - module is loaded and running.
        /// </summary>
        /// <remarks>
        /// swkotor2.exe: 0x006caab0 @ 0x006caab0 line 202: ":: Server mode: Module Running.\n"
        /// Sets `*(undefined2 *)(DAT_008283d4 + 4) = 2`, sets bit flag `*puVar6 | 0x1`
        /// Module is fully loaded, OnModuleStart has been called, gameplay is active
        /// </remarks>
        ModuleRunning = 2
    }
}
