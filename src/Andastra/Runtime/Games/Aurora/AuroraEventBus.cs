using System;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Games.Common;

namespace Andastra.Runtime.Games.Aurora
{
    /// <summary>
    /// Aurora Engine event bus implementation.
    /// </summary>
    /// <remarks>
    /// Aurora Event Bus System:
    /// - Based on nwmain.exe (Neverwinter Nights) and nwn2main.exe (Neverwinter Nights 2) event systems
    /// - Aurora engine uses similar event dispatching patterns to Odyssey but with Aurora-specific event types
    /// - Event system supports NWScript event hooks similar to Odyssey's script events
    /// - Common event types: OnHeartbeat, OnPerception, OnDamaged, OnDeath, OnDialogue, etc.
    /// - Event routing: Events are queued and dispatched at frame boundaries
    /// - Script execution: FireScriptEvent triggers NWScript execution on entities with matching event hooks
    /// - Inheritance: Inherits from BaseEventBus (Runtime.Games.Common) with Aurora-specific event handling
    /// - TODO: Reverse engineer specific function addresses from nwmain.exe and nwn2main.exe using Ghidra MCP
    /// </remarks>
    [PublicAPI]
    public class AuroraEventBus : BaseEventBus
    {
        /// <summary>
        /// Initializes a new instance of the AuroraEventBus class.
        /// </summary>
        public AuroraEventBus()
        {
        }

        /// <summary>
        /// Fires a script event on an entity.
        /// </summary>
        /// <param name="entity">The entity to fire the event on.</param>
        /// <param name="eventType">The script event type.</param>
        /// <param name="triggerer">The triggering entity (optional).</param>
        /// <remarks>
        /// Based on nwmain.exe: Aurora script event firing implementation
        /// Aurora engine uses NWScript event system similar to Odyssey's script events
        /// Events are queued and processed at frame boundaries to prevent re-entrancy
        /// Script events trigger NWScript execution on entities with matching event hooks
        /// TODO: Reverse engineer specific function addresses from nwmain.exe using Ghidra MCP
        /// </remarks>
        public override void FireScriptEvent(IEntity entity, ScriptEvent eventType, IEntity triggerer = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Create script event args and queue for processing
            // Based on nwmain.exe: Events are queued and processed at frame boundary
            var evt = new ScriptEventArgs(entity, eventType, triggerer);
            ((IEventBus)this).QueueEvent(evt);
        }

        /// <summary>
        /// Event args for script events.
        /// </summary>
        /// <remarks>
        /// Aurora-specific script event arguments structure.
        /// Based on nwmain.exe event data structure.
        /// </remarks>
        private class ScriptEventArgs : IGameEvent
        {
            public ScriptEventArgs(IEntity entity, ScriptEvent eventType, IEntity triggerer)
            {
                Entity = entity;
                EventType = eventType;
                Triggerer = triggerer;
            }

            public IEntity Entity { get; }
            public ScriptEvent EventType { get; }
            public IEntity Triggerer { get; }
        }
    }
}

