using System.Collections.Generic;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Games.Common;

namespace Andastra.Runtime.Games.Odyssey
{
    /// <summary>
    /// Odyssey Engine event dispatcher implementation.
    /// </summary>
    /// <remarks>
    /// Odyssey Event Dispatcher Implementation:
    /// - Based on DispatchEvent @ 0x004dcfb0 in swkotor2.exe
    /// - Maps event IDs to string names for debugging
    /// - Routes events to appropriate handlers based on type
    ///
    /// Based on reverse engineering of:
    /// - swkotor.exe: Event dispatching functions
    /// - swkotor2.exe: DispatchEvent @ 0x004dcfb0 with comprehensive event mapping
    /// - Event types: AREA_TRANSITION (0x1a), REMOVE_FROM_AREA (4), etc.
    /// - Script events: ON_HEARTBEAT (0), ON_PERCEPTION (1), etc.
    ///
    /// Event system features:
    /// - Immediate event dispatching
    /// - Queued event processing for script safety
    /// - Event logging and debugging support
    /// - Script hook integration
    /// </remarks>
    [PublicAPI]
    public class OdysseyEventDispatcher : BaseEventDispatcher
    {
        private readonly Queue<PendingEvent> _eventQueue = new Queue<PendingEvent>();

        private struct PendingEvent
        {
            public IEntity SourceEntity;
            public IEntity TargetEntity;
            public int EventType;
            public int EventSubtype;
        }

        /// <summary>
        /// Dispatches an event immediately.
        /// </summary>
        /// <remarks>
        /// Based on DispatchEvent @ 0x004dcfb0 in swkotor2.exe.
        /// Maps event IDs and routes to appropriate handlers.
        /// May queue events for later processing if needed.
        /// </remarks>
        public override void DispatchEvent(IEntity sourceEntity, IEntity targetEntity, int eventType, int eventSubtype)
        {
            // Log event for debugging
            var eventName = GetEventName(eventType);
            var subtypeName = GetEventSubtypeName(eventSubtype);

            // TODO: Add logging
            // System.Diagnostics.Debug.WriteLine($"Dispatching event: {eventName} ({eventType}) -> {subtypeName} ({eventSubtype})");

            // Route to appropriate handler based on event type
            switch (eventType)
            {
                case 0x1a: // EVENT_AREA_TRANSITION
                    HandleAreaTransition(targetEntity, null); // TODO: Extract target area from event data
                    break;

                case 4: // EVENT_REMOVE_FROM_AREA
                    HandleAreaTransition(targetEntity, null); // Area removal
                    break;

                case 6: // EVENT_CLOSE_OBJECT
                case 7: // EVENT_OPEN_OBJECT
                case 0xc: // EVENT_UNLOCK_OBJECT
                case 0xd: // EVENT_LOCK_OBJECT
                    HandleObjectEvent(targetEntity, eventType);
                    break;

                case 4: // EVENT_ON_DAMAGED (same ID as REMOVE_FROM_AREA, context matters)
                case 10: // EVENT_DESTROY_OBJECT
                case 0xf: // EVENT_ON_MELEE_ATTACKED
                    if (targetEntity != null)
                        HandleCombatEvent(targetEntity, eventType);
                    break;

                case 10: // EVENT_SIGNAL_EVENT (script events)
                    if (targetEntity != null)
                        HandleScriptEvent(targetEntity, eventType, eventSubtype);
                    break;

                default:
                    // Unknown event type
                    break;
            }
        }

        /// <summary>
        /// Gets the string name for an event type.
        /// </summary>
        /// <remarks>
        /// Based on event name mapping in DispatchEvent @ 0x004dcfb0.
        /// Returns descriptive names for known events.
        /// </remarks>
        protected override string GetEventName(int eventType)
        {
            switch (eventType)
            {
                case 1: return "EVENT_TIMED_EVENT";
                case 2: return "EVENT_ENTERED_TRIGGER";
                case 3: return "EVENT_LEFT_TRIGGER";
                case 4: return "EVENT_REMOVE_FROM_AREA"; // or EVENT_ON_DAMAGED in different context
                case 5: return "EVENT_APPLY_EFFECT";
                case 6: return "EVENT_CLOSE_OBJECT";
                case 7: return "EVENT_OPEN_OBJECT";
                case 8: return "EVENT_SPELL_IMPACT";
                case 9: return "EVENT_PLAY_ANIMATION";
                case 10: return "EVENT_SIGNAL_EVENT"; // or EVENT_DESTROY_OBJECT in different context
                case 0xb: return "EVENT_DESTROY_OBJECT";
                case 0xc: return "EVENT_UNLOCK_OBJECT";
                case 0xd: return "EVENT_LOCK_OBJECT";
                case 0xe: return "EVENT_REMOVE_EFFECT";
                case 0xf: return "EVENT_ON_MELEE_ATTACKED";
                case 0x10: return "EVENT_DECREMENT_STACKSIZE";
                case 0x11: return "EVENT_SPAWN_BODY_BAG";
                case 0x12: return "EVENT_FORCED_ACTION";
                case 0x13: return "EVENT_ITEM_ON_HIT_SPELL_IMPACT";
                case 0x14: return "EVENT_BROADCAST_AOO";
                case 0x15: return "EVENT_BROADCAST_SAFE_PROJECTILE";
                case 0x16: return "EVENT_FEEDBACK_MESSAGE";
                case 0x17: return "EVENT_ABILITY_EFFECT_APPLIED";
                case 0x18: return "EVENT_SUMMON_CREATURE";
                case 0x19: return "EVENT_ACQUIRE_ITEM";
                case 0x1a: return "EVENT_AREA_TRANSITION";
                case 0x1b: return "EVENT_CONTROLLER_RUMBLE";
                default: return $"Event({eventType})";
            }
        }

        /// <summary>
        /// Gets the string name for an event subtype.
        /// </summary>
        /// <remarks>
        /// Based on script event subtype mapping in DispatchEvent.
        /// Used for SIGNAL_EVENT subtypes.
        /// </remarks>
        protected override string GetEventSubtypeName(int eventSubtype)
        {
            switch (eventSubtype)
            {
                case 0: return "CSWSSCRIPTEVENT_EVENTTYPE_ON_HEARTBEAT";
                case 1: return "CSWSSCRIPTEVENT_EVENTTYPE_ON_PERCEPTION";
                case 2: return "CSWSSCRIPTEVENT_EVENTTYPE_ON_SPELLCASTAT";
                case 4: return "CSWSSCRIPTEVENT_EVENTTYPE_ON_DAMAGED";
                case 5: return "CSWSSCRIPTEVENT_EVENTTYPE_ON_DISTURBED";
                case 7: return "CSWSSCRIPTEVENT_EVENTTYPE_ON_DIALOGUE";
                case 8: return "CSWSSCRIPTEVENT_EVENTTYPE_ON_SPAWN_IN";
                case 9: return "CSWSSCRIPTEVENT_EVENTTYPE_ON_RESTED";
                default: return $"EventType({eventSubtype})";
            }
        }

        /// <summary>
        /// Handles area transition events.
        /// </summary>
        /// <remarks>
        /// Based on EVENT_AREA_TRANSITION (0x1a) and EVENT_REMOVE_FROM_AREA (4).
        /// Manages entity movement between areas.
        /// Updates area membership and triggers transition effects.
        /// </remarks>
        protected override void HandleAreaTransition(IEntity entity, string targetArea)
        {
            // TODO: Implement area transition logic
            // Update entity's area membership
            // Trigger transition scripts and effects
            // Handle area loading/unloading if needed
        }

        /// <summary>
        /// Handles object manipulation events.
        /// </summary>
        /// <remarks>
        /// Based on object events: OPEN_OBJECT (7), CLOSE_OBJECT (6), LOCK_OBJECT (0xd), UNLOCK_OBJECT (0xc).
        /// Updates object state and triggers associated scripts.
        /// Handles visual/audio feedback for state changes.
        /// </remarks>
        protected override void HandleObjectEvent(IEntity entity, int eventType)
        {
            // TODO: Implement object event handling
            // Update door/placeable state based on event type
            // Trigger associated scripts and effects
            // Update visual representation
        }

        /// <summary>
        /// Handles combat-related events.
        /// </summary>
        /// <remarks>
        /// Based on combat events: ON_DAMAGED (4), ON_DEATH (10), ON_ATTACKED (0xf).
        /// Triggers combat scripts and AI behaviors.
        /// Updates combat state and effects.
        /// </remarks>
        protected override void HandleCombatEvent(IEntity entity, int eventType)
        {
            // TODO: Implement combat event handling
            // Update combat state
            // Trigger AI behaviors and scripts
            // Apply damage/effects
        }

        /// <summary>
        /// Handles script hook events.
        /// </summary>
        /// <remarks>
        /// Based on SIGNAL_EVENT (10) with subtypes for different script hooks.
        /// Executes entity-specific scripts based on event type.
        /// Handles heartbeat, perception, dialogue, etc.
        /// </remarks>
        protected override void HandleScriptEvent(IEntity entity, int eventType, int eventSubtype)
        {
            // TODO: Implement script event handling
            // Execute appropriate script based on eventSubtype
            // Pass event parameters to script execution
            // Handle script return values and effects
        }

        /// <summary>
        /// Queues an event for later processing.
        /// </summary>
        /// <remarks>
        /// Events are queued to prevent recursive dispatching.
        /// Ensures proper execution order and script safety.
        /// </remarks>
        public override void QueueEvent(IEntity sourceEntity, IEntity targetEntity, int eventType, int eventSubtype)
        {
            _eventQueue.Enqueue(new PendingEvent
            {
                SourceEntity = sourceEntity,
                TargetEntity = targetEntity,
                EventType = eventType,
                EventSubtype = eventSubtype
            });
        }

        /// <summary>
        /// Processes queued events.
        /// </summary>
        /// <remarks>
        /// Called during script execution phase.
        /// Processes all queued events in order.
        /// Clears queue after processing.
        /// </remarks>
        public override void ProcessQueuedEvents()
        {
            while (_eventQueue.Count > 0)
            {
                var pendingEvent = _eventQueue.Dequeue();
                DispatchEvent(pendingEvent.SourceEntity, pendingEvent.TargetEntity,
                            pendingEvent.EventType, pendingEvent.EventSubtype);
            }
        }
    }
}
