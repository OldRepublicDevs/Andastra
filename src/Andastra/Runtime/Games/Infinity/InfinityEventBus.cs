using System;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Games.Common;

namespace Andastra.Runtime.Games.Infinity
{
    /// <summary>
    /// Infinity Engine event bus implementation.
    /// </summary>
    /// <remarks>
    /// Infinity Event Bus System:
    /// - Based on Infinity Engine (Baldur's Gate, Icewind Dale, Planescape: Torment) event systems
    /// - Infinity engine uses event dispatching patterns similar to other BioWare engines
    /// - Event system supports script event hooks for Infinity Engine games
    /// - Common event types: OnHeartbeat, OnPerception, OnDamaged, OnDeath, OnDialogue, etc.
    /// - Event routing: Events are queued and dispatched at frame boundaries
    /// - Script execution: FireScriptEvent triggers script execution on entities with matching event hooks
    /// - Inheritance: Inherits from BaseEventBus (Runtime.Games.Common) with Infinity-specific event handling
    /// - TODO: Reverse engineer specific function addresses from Infinity Engine executables using Ghidra MCP
    /// - NOTE: Infinity Engine support is planned for future implementation
    /// </remarks>
    [PublicAPI]
    public class InfinityEventBus : BaseEventBus
    {
        /// <summary>
        /// Initializes a new instance of the InfinityEventBus class.
        /// </summary>
        public InfinityEventBus()
        {
        }

        /// <summary>
        /// Fires a script event on an entity.
        /// </summary>
        /// <param name="entity">The entity to fire the event on.</param>
        /// <param name="eventType">The script event type.</param>
        /// <param name="triggerer">The triggering entity (optional).</param>
        /// <remarks>
        /// Based on Infinity Engine: Script event firing implementation
        /// Infinity engine uses script event system similar to other BioWare engines
        /// Events are queued and processed at frame boundaries to prevent re-entrancy
        /// Script events trigger script execution on entities with matching event hooks
        /// TODO: Reverse engineer specific function addresses from Infinity Engine executables using Ghidra MCP
        /// NOTE: Infinity Engine support is planned for future implementation
        /// </remarks>
        public override void FireScriptEvent(IEntity entity, ScriptEvent eventType, IEntity triggerer = null)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            // Create script event args and queue for processing
            // Based on Infinity Engine: Events are queued and processed at frame boundary
            var evt = new ScriptEventArgs(entity, eventType, triggerer);
            ((IEventBus)this).QueueEvent(evt);
        }

        /// <summary>
        /// Event args for script events.
        /// </summary>
        /// <remarks>
        /// Infinity-specific script event arguments structure.
        /// Based on Infinity Engine event data structure.
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

