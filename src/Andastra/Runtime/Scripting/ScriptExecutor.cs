using System;
using System.Threading;
using Andastra.Parsing;
using Andastra.Parsing.Resource;
using Andastra.Runtime.Content.Interfaces;
using Andastra.Runtime.Core.Entities;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Scripting.Interfaces;
using Andastra.Runtime.Scripting.VM;

namespace Andastra.Runtime.Scripting
{
    /// <summary>
    /// Executes NWScript scripts for entities when events fire.
    /// </summary>
    /// <remarks>
    /// Script Executor:
    /// - Based on swkotor2.exe: DispatchScriptEvent @ 0x004dd730
    /// - Located via string references: "CSWSSCRIPTEVENT_EVENTTYPE_ON_HEARTBEAT" @ 0x007bcb90, "ACTION" @ 0x007cd138
    /// - Script loading: Loads NCS bytecode from resource provider (IGameResourceProvider or Installation)
    /// - Script execution: Executes NCS bytecode via NCS VM with entity as caller (OBJECT_SELF)
    /// - Event-driven: Subscribes to script events (OnSpawn, OnHeartbeat, etc.) and executes matching scripts
    /// - Script hooks: IScriptHooksComponent stores script ResRefs mapped to event types
    /// - Execution context: Creates ExecutionContext with entity as caller, triggerer as parameter
    /// - Heartbeat timing: OnHeartbeat fires every 6 seconds (heartbeat interval)
    /// - Original implementation:
    ///   - swkotor2.exe: DispatchScriptEvent @ 0x004dd730 - Dispatches script events to registered handlers, creates event data structure, iterates through registered script handlers, calls FUN_004db870 to match event types, queues matching handlers
    ///   - swkotor2.exe: LoadScriptHooks @ 0x0050c510 - Loads script hook references from GFF templates (ScriptHeartbeat, ScriptOnNotice, ScriptSpellAt, ScriptAttacked, ScriptDamaged, ScriptDisturbed, ScriptEndRound, ScriptDialogue, ScriptSpawn, ScriptRested, ScriptDeath, ScriptUserDefine, ScriptOnBlocked, ScriptEndDialogue)
    ///   - swkotor2.exe: LogScriptEvent @ 0x004dcfb0 - Logs script events for debugging, maps event types to string names
    ///   - nwmain.exe: ExecuteCommandExecuteScript @ 0x14051d5c0 - Executes script command in NCS VM, thunk that calls FUN_140c10370, part of CNWSVirtualMachineCommands class
    ///   - nwmain.exe: CScriptEvent @ 0x1404c6490 - Script event constructor, initializes event data structure
    /// - Cross-engine: Similar functions in swkotor.exe (TODO: STUB), nwmain.exe (found), daorigins.exe (TODO: STUB)
    /// - Inheritance: Base class ScriptExecutor (Runtime.Games.Common), Odyssey override: OdysseyScriptExecutor (Runtime.Games.Odyssey), Aurora override: AuroraScriptExecutor (Runtime.Games.Aurora)
    /// - Based on NCS VM execution model in vendor/PyKotor/wiki/NCS-File-Format.md
    /// </remarks>
    public class ScriptExecutor
    {
        private readonly IWorld _world;
        private readonly IEngineApi _engineApi;
        private readonly IScriptGlobals _globals;
        private readonly object _resourceProvider;
        private readonly NcsVm _vm;
        private float _heartbeatTimer;
        private const float HeartbeatInterval = 6.0f; // 6 seconds between heartbeats

        public ScriptExecutor(IWorld world, IEngineApi engineApi, IScriptGlobals globals, object resourceProvider)
        {
            _world = world ?? throw new ArgumentNullException("world");
            _engineApi = engineApi ?? throw new ArgumentNullException("engineApi");
            _globals = globals ?? throw new ArgumentNullException("globals");
            _resourceProvider = resourceProvider ?? throw new ArgumentNullException("resourceProvider");
            _vm = new NcsVm();
            _heartbeatTimer = 0.0f;

            // Subscribe to script events
            _world.EventBus.Subscribe<ScriptEventArgs>(OnScriptEvent);
        }

        /// <summary>
        /// Updates heartbeat timer and fires heartbeat events.
        /// </summary>
        public void Update(float deltaTime)
        {
            // Based on swkotor2.exe: Heartbeat system implementation
            // Located via string references: "CSWSSCRIPTEVENT_EVENTTYPE_ON_HEARTBEAT" @ 0x007bcb90, "ScriptHeartbeat" (loaded via LoadScriptHooks @ 0x0050c510)
            // Original implementation: OnHeartbeat fires every 6 seconds for all entities with heartbeat scripts
            // Heartbeat timing: Uses game simulation time to track 6-second intervals
            // Event dispatch: DispatchScriptEvent @ 0x004dd730 handles event routing to script handlers
            _heartbeatTimer += deltaTime;

            if (_heartbeatTimer >= HeartbeatInterval)
            {
                _heartbeatTimer = 0.0f;

                // Fire heartbeat for all entities
                foreach (IEntity entity in _world.GetEntitiesInRadius(System.Numerics.Vector3.Zero, float.MaxValue, Andastra.Runtime.Core.Enums.ObjectType.All))
                {
                    IScriptHooksComponent hooks = entity.GetComponent<IScriptHooksComponent>();
                    if (hooks != null && !string.IsNullOrEmpty(hooks.GetScript(ScriptEvent.OnHeartbeat)))
                    {
                        _world.EventBus.FireScriptEvent(entity, ScriptEvent.OnHeartbeat);
                    }
                }
            }
        }

        private void OnScriptEvent(ScriptEventArgs evt)
        {
            if (evt.Entity == null)
            {
                return;
            }

            IScriptHooksComponent hooks = evt.Entity.GetComponent<IScriptHooksComponent>();
            if (hooks == null)
            {
                return;
            }

            string scriptResRef = hooks.GetScript(evt.EventType);
            if (string.IsNullOrEmpty(scriptResRef))
            {
                return;
            }

            // Execute the script
            ExecuteScript(evt.Entity, scriptResRef, evt.Triggerer);
        }

        /// <summary>
        /// Executes a script on an entity.
        /// </summary>
        public void ExecuteScript(IEntity caller, string scriptResRef, IEntity triggerer = null)
        {
            if (caller == null || string.IsNullOrEmpty(scriptResRef))
            {
                return;
            }

            try
            {
                // Create execution context
                var ctx = new Andastra.Runtime.Scripting.VM.ExecutionContext(caller, _world, _engineApi, _globals);
                ctx.SetTriggerer(triggerer);
                ctx.ResourceProvider = _resourceProvider;

                // Execute the script
                _vm.ExecuteScript(scriptResRef, ctx);
            }
            catch (Exception ex)
            {
                // Log script execution errors but don't crash
                // Original engine: Script errors are logged but don't stop game execution
                System.Diagnostics.Debug.WriteLine($"Script execution error: {scriptResRef} on entity {caller.ObjectId}: {ex.Message}");
            }
        }
    }

}

