using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Engines.Common;
using Andastra.Runtime.Content.Interfaces;

namespace Andastra.Runtime.Engines.Infinity
{
    /// <summary>
    /// Infinity Engine game session implementation for Baldur's Gate, Icewind Dale, and Planescape: Torment.
    /// </summary>
    /// <remarks>
    /// Game Session System:
    /// - Based on Infinity Engine game session management (Baldur's Gate, Icewind Dale, Planescape: Torment)
    /// - Infinity Engine uses a different game session architecture than Odyssey/Aurora/Eclipse:
    ///   - Simpler module system - modules are typically single areas
    ///   - GAM files contain game state (party, global variables, journal)
    ///   - ARE files contain area-specific data (creatures, items, triggers)
    /// - Cross-engine: Similar game session patterns but different implementations
    ///   - Odyssey: FUN_006caab0 @ 0x006caab0 (swkotor2.exe) - server command parser, manages module state flags
    ///   - Aurora: Similar module state management (module loading/unloading, state flags)
    ///   - Eclipse: UnrealScript message passing system for game session management
    ///   - Infinity: Direct ARE/GAM file-based game session management
    /// - Inheritance: BaseEngineGame (Runtime.Games.Common) implements common module state management
    ///   - Infinity: InfinityGameSession : BaseEngineGame (Runtime.Games.Infinity) - Infinity-specific module loading
    /// - Original implementation: Infinity Engine manages game state through GAM files
    ///   - State 0 = Idle (no module loaded)
    ///   - State 1 = ModuleLoaded (area loaded but not active)
    ///   - State 2 = ModuleRunning (area loaded and active)
    /// - Module state: Tracks current module name, current area, player entity
    /// - Coordinates: Module loading, entity management, script execution, combat, AI, triggers
    /// - Game loop integration: Update() called every frame to update all systems (60 Hz fixed timestep)
    /// - Module transitions: Handles loading new areas and positioning party at entry point
    /// - Script execution: Infinity Engine uses different scripting system (Baldur's Gate script format)
    /// - TODO: Reverse engineer specific function addresses from Infinity Engine executables using Ghidra MCP
    ///   - Baldur's Gate: BaldurGate.exe game session functions
    ///   - Icewind Dale: IcewindDale.exe game session functions
    ///   - Planescape: Torment: PlanescapeTorment.exe game session functions
    /// </remarks>
    public class InfinityGameSession : BaseEngineGame
    {
        private readonly InfinityEngine _infinityEngine;
        private readonly InfinityModuleLoader _moduleLoader;

        public InfinityGameSession(InfinityEngine engine)
            : base(engine)
        {
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            _infinityEngine = engine;

            // Initialize module loader
            _moduleLoader = new InfinityModuleLoader(engine.World, engine.ResourceProvider);
        }

        public override async Task LoadModuleAsync(string moduleName, [CanBeNull] Action<float> progressCallback = null)
        {
            if (string.IsNullOrEmpty(moduleName))
            {
                throw new ArgumentException("Module name cannot be null or empty", nameof(moduleName));
            }

            // Load module using Infinity module loader
            await _moduleLoader.LoadModuleAsync(moduleName, progressCallback);

            // Update game session state
            CurrentModuleName = moduleName;

            // Set current area from module loader
            if (_moduleLoader.CurrentArea != null)
            {
                // Area is already set in module loader
            }
        }

        protected override void OnUnloadModule()
        {
            // Unload module using module loader
            if (_moduleLoader != null)
            {
                _moduleLoader.UnloadModule();
            }
        }
    }
}

