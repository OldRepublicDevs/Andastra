using System;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Dialogue;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Scripting.VM;
using Andastra.Runtime.Scripting.Interfaces;
using Andastra.Runtime.Scripting.EngineApi;
using Andastra.Parsing.Installation;
using Andastra.Parsing.Resource;

namespace Andastra.Runtime.Games.Odyssey
{
    /// <summary>
    /// Odyssey Engine script executor implementation.
    /// </summary>
    /// <remarks>
    /// Odyssey Script Executor:
    /// - Inherits from BaseScriptExecutor (Runtime.Games.Common) with Odyssey-specific resource loading
    /// - Based on swkotor2.exe script execution system with comprehensive event type mappings
    /// - Located via string references: Extensive script event type definitions
    /// - Script hook fields: Complete mapping of script event handlers
    /// - NCS file format: Compiled NWScript bytecode with "NCS " signature, "V1.0" version string
    /// - Script loading: Loads NCS files from installation via ResourceLookup (ResourceType.NCS)
    /// - Execution context: Creates ExecutionContext with owner (OBJECT_SELF), world, engine API, globals
    /// - OBJECT_SELF: Set to owner entity ObjectId (0x7F000001 = OBJECT_SELF constant)
    /// - OBJECT_INVALID: 0x7F000000 (invalid object reference constant)
    /// - Triggerer: Optional triggering entity (for event-driven scripts like OnEnter, OnClick, etc.)
    /// - Return value: Script return value (0 = FALSE, non-zero = TRUE) for condition scripts
    /// - Error handling: Returns 0 (FALSE) on script load failure or execution error
    /// - Script execution: FUN_004dcfb0 @ 0x004dcfb0 dispatches script events and executes scripts
    ///   - Function signature: `int FUN_004dcfb0(void *param_1, int param_2, void *param_3, int param_4)`
    ///   - param_1: Entity pointer (owner of script)
    ///   - param_2: Script event type (CSWSSCRIPTEVENT_EVENTTYPE_* constant)
    ///   - param_3: Triggerer entity pointer (optional, can be null)
    ///   - param_4: Unknown flag
    ///   - Loads script ResRef from entity's script hook field based on event type
    ///   - Executes script with owner as OBJECT_SELF, triggerer as OBJECT_TRIGGERER
    ///   - Returns script return value (0 = FALSE, non-zero = TRUE)
    /// - Based on NCS VM execution in vendor/PyKotor/wiki/NCS-File-Format.md
    /// - Event types: Comprehensive mapping from 0x0 (ON_HEARTBEAT) to 0x26 (ON_DESTROYPLAYERCREATURE)
    /// - Inheritance: Extends BaseScriptExecutor with Odyssey-specific Installation resource loading
    /// </remarks>
    public class OdysseyScriptExecutor : BaseScriptExecutor
    {
        private readonly Installation _installation;
        private readonly IGameServicesContext _servicesContext;

        public OdysseyScriptExecutor([NotNull] IWorld world, [NotNull] IEngineApi engineApi, [NotNull] IScriptGlobals globals, [NotNull] Installation installation, [CanBeNull] IGameServicesContext servicesContext = null)
            : base(world, engineApi, globals)
        {
            _installation = installation ?? throw new ArgumentNullException(nameof(installation));
            _servicesContext = servicesContext;
        }

        /// <summary>
        /// Executes a script with Odyssey-specific resource loading.
        /// </summary>
        /// <remarks>
        /// Based on swkotor2.exe script execution with Installation resource loading.
        /// Includes GameServicesContext for enhanced script context.
        /// </remarks>
        public override int ExecuteScript(IEntity caller, string scriptResRef, IEntity triggerer = null)
        {
            if (string.IsNullOrEmpty(scriptResRef))
            {
                return 0; // FALSE
            }

            try
            {
                // Load NCS bytecode using Odyssey resource system
                byte[] bytecode = LoadNcsBytecode(scriptResRef);
                if (bytecode == null || bytecode.Length == 0)
                {
                    Console.WriteLine("[OdysseyScriptExecutor] Script not found: " + scriptResRef);
                    return 0; // FALSE
                }

                // Create execution context with Odyssey-specific enhancements
                var context = CreateExecutionContext(caller, triggerer);

                // Set additional context (GameServicesContext) if available
                // Odyssey-specific: Enhanced script context for game services
                if (_servicesContext != null)
                {
                    context.AdditionalContext = _servicesContext;
                }

                // Execute script via VM
                // Based on swkotor2.exe: Script execution with instruction budget tracking
                // Located via string references: Script execution budget limits per frame
                // Original implementation: Tracks instruction count per entity for budget enforcement
                int returnValue = _vm.Execute(bytecode, context);

                // Accumulate instruction count to owner entity's action queue component
                // This allows the game loop to enforce per-frame script budget limits
                TrackScriptExecution(caller, _vm.InstructionsExecuted);

                return returnValue;
            }
            catch (Exception ex)
            {
                HandleScriptError(scriptResRef, caller, ex);
                return 0; // FALSE on error
            }
        }

        /// <summary>
        /// Loads NCS bytecode using Odyssey Installation resource system.
        /// </summary>
        /// <remarks>
        /// Odyssey-specific: Uses Installation.ResourceLookup for NCS files.
        /// Based on swkotor2.exe resource loading patterns.
        /// </remarks>
        protected override byte[] LoadNcsBytecode(string scriptResRef)
        {
            var resource = _installation.Resources.LookupResource(scriptResRef, ResourceType.NCS);
            return resource?.Data;
        }
    }
}

