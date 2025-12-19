using System.Collections.Generic;
using System.Numerics;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;

namespace Andastra.Runtime.Games.Common.Components
{
    /// <summary>
    /// Base implementation of trigger component functionality shared across all BioWare engines.
    /// </summary>
    /// <remarks>
    /// Base Trigger Component Implementation:
    /// - Common trigger properties and methods across all engines
    /// - Handles base trigger state, geometry, enter/exit detection, script firing
    /// - Provides base for engine-specific trigger component implementations
    /// - Cross-engine analysis: All engines share common trigger structure patterns
    /// - Common functionality: Geometry, IsEnabled, TriggerType, LinkedTo, LinkedToModule, IsTrap, TrapActive, TrapDetected, TrapDisarmed, TrapDetectDC, TrapDisarmDC, FireOnce, HasFired, ContainsPoint, ContainsEntity
    /// - Engine-specific: File format details, event handling, field names, transition systems, trap systems
    ///
    /// Based on reverse engineering of:
    /// - swkotor.exe: Trigger system with UTT template loading
    /// - swkotor2.exe: Enhanced trigger system (FUN_004e5920 @ 0x004e5920)
    ///   - Located via string references: "Trigger" @ 0x007bc51c, "TriggerList" @ 0x007bd254
    ///   - "EVENT_ENTERED_TRIGGER" @ 0x007bce08, "EVENT_LEFT_TRIGGER" @ 0x007bcdf4
    ///   - "OnTrapTriggered" @ 0x007c1a34, "CSWSSCRIPTEVENT_EVENTTYPE_ON_MINE_TRIGGERED" @ 0x007bc7ac
    ///   - Transition fields: "LinkedTo" @ 0x007bd798, "LinkedToModule" @ 0x007bd7bc, "LinkedToFlags" @ 0x007bd788
    ///   - "TransitionDestination" @ 0x007bd7a4
    /// - nwmain.exe: Aurora trigger system using CNWSTrigger class
    ///   - CNWSTrigger::LoadTrigger @ 0x1404c8a00, CNWSTrigger::SaveTrigger @ 0x1404c9b40
    ///   - LoadTriggers @ 0x140362b20, SaveTriggers @ 0x1403680a0
    ///   - Located via string reference: "Trigger List" @ 0x140ddb800
    /// - daorigins.exe: Eclipse engine trigger system (CCTrigger class)
    ///   - TriggerList @ 0x00af5040, CTrigger @ 0x00b0d4a0
    ///   - COMMAND_GETTRIGGER* and COMMAND_SETTRIGGER* functions
    /// - DragonAge2.exe: Enhanced Eclipse engine (similar to daorigins.exe)
    /// - MassEffect.exe/MassEffect2.exe: Infinity engine (different trigger system, not fully reverse engineered)
    ///
    /// Common structure across engines:
    /// - Geometry (IList&lt;Vector3&gt;): Polygon vertices defining trigger volume
    /// - IsEnabled (bool): Whether trigger is currently enabled
    /// - TriggerType (int): Type of trigger (0=generic, 1=transition, 2=trap)
    /// - LinkedTo (string): Linked waypoint/door tag for transitions
    /// - LinkedToModule (string): Linked module for transitions
    /// - IsTrap (bool): Whether trigger is a trap
    /// - TrapActive (bool): Whether trap is active
    /// - TrapDetected (bool): Whether trap has been detected
    /// - TrapDisarmed (bool): Whether trap has been disarmed
    /// - TrapDetectDC (int): DC to detect trap
    /// - TrapDisarmDC (int): DC to disarm trap
    /// - FireOnce (bool): Whether trigger fires only once
    /// - HasFired (bool): Whether trigger has already fired (for FireOnce triggers)
    ///
    /// Common trigger operations across engines:
    /// - ContainsPoint(Vector3): Tests if point is inside trigger volume
    /// - ContainsEntity(IEntity): Tests if entity is inside trigger volume
    /// - Point-in-polygon test: Uses ray casting algorithm (common across all engines)
    /// </remarks>
    public abstract class BaseTriggerComponent : ITriggerComponent
    {
        /// <summary>
        /// The entity this component is attached to.
        /// </summary>
        public IEntity Owner { get; set; }

        /// <summary>
        /// Trigger polygon vertices (common across all engines).
        /// </summary>
        /// <remarks>
        /// Geometry Property:
        /// - Common across all engines: Polygon vertices defining trigger volume
        /// - Based on swkotor2.exe: Geometry field in UTT template (FUN_004e5920 @ 0x004e5920)
        /// - Based on nwmain.exe: Geometry field in CNWSTrigger (LoadTrigger @ 0x1404c8a00)
        /// - Engine-specific: Field names and storage may differ, but concept is common
        /// </remarks>
        protected List<Vector3> _vertices;

        /// <summary>
        /// Set of entity IDs currently inside this trigger (common across all engines).
        /// </summary>
        protected HashSet<uint> _enteredBy;

        /// <summary>
        /// Initializes a new instance of the BaseTriggerComponent class.
        /// </summary>
        protected BaseTriggerComponent()
        {
            _vertices = new List<Vector3>();
            _enteredBy = new HashSet<uint>();
            IsEnabled = true;
            LinkedTo = string.Empty;
            LinkedToModule = string.Empty;
            TrapActive = true;
            TrapDetected = false;
            TrapDisarmed = false;
            HasFired = false;
        }

        #region IComponent Implementation

        /// <summary>
        /// Called when component is attached to an entity.
        /// </summary>
        public virtual void OnAttach()
        {
            // Common initialization - engine-specific subclasses may override
        }

        /// <summary>
        /// Called when component is detached from an entity.
        /// </summary>
        public virtual void OnDetach()
        {
            // Common cleanup - engine-specific subclasses may override
            _enteredBy.Clear();
        }

        #endregion

        #region ITriggerComponent Implementation

        /// <summary>
        /// The geometry vertices defining the trigger volume.
        /// </summary>
        /// <remarks>
        /// Common implementation across all engines:
        /// - Returns internal vertex list
        /// - Setting replaces the entire list
        /// </remarks>
        public virtual IList<Vector3> Geometry
        {
            get { return _vertices; }
            set { _vertices = value != null ? new List<Vector3>(value) : new List<Vector3>(); }
        }

        /// <summary>
        /// Whether the trigger is currently enabled.
        /// </summary>
        /// <remarks>
        /// Common across all engines: Controls whether trigger can fire events
        /// </remarks>
        public virtual bool IsEnabled { get; set; }

        /// <summary>
        /// Type of trigger (0=generic, 1=transition, 2=trap).
        /// </summary>
        /// <remarks>
        /// Common across all engines: Trigger type classification
        /// - 0 = Generic: Fires OnEnter/OnExit scripts
        /// - 1 = Transition: Links to other areas/modules
        /// - 2 = Trap: Can be detected/disarmed, fires OnTrapTriggered
        /// </remarks>
        public abstract int TriggerType { get; set; }

        /// <summary>
        /// For transition triggers, the destination tag.
        /// </summary>
        /// <remarks>
        /// Common across all engines: Waypoint/door tag for area transitions
        /// </remarks>
        public virtual string LinkedTo { get; set; }

        /// <summary>
        /// For transition triggers, the destination module.
        /// </summary>
        /// <remarks>
        /// Common across all engines: Module ResRef for module transitions
        /// </remarks>
        public virtual string LinkedToModule { get; set; }

        /// <summary>
        /// Whether this is a trap trigger.
        /// </summary>
        /// <remarks>
        /// Common across all engines: Indicates if trigger is a trap
        /// </remarks>
        public abstract bool IsTrap { get; set; }

        /// <summary>
        /// Whether the trap is active.
        /// </summary>
        /// <remarks>
        /// Common across all engines: Controls whether trap can trigger
        /// </remarks>
        public virtual bool TrapActive { get; set; }

        /// <summary>
        /// Whether the trap has been detected.
        /// </summary>
        /// <remarks>
        /// Common across all engines: Indicates if trap has been detected by player
        /// </remarks>
        public virtual bool TrapDetected { get; set; }

        /// <summary>
        /// Whether the trap has been disarmed.
        /// </summary>
        /// <remarks>
        /// Common across all engines: Indicates if trap has been disarmed
        /// </remarks>
        public virtual bool TrapDisarmed { get; set; }

        /// <summary>
        /// DC to detect the trap.
        /// </summary>
        /// <remarks>
        /// Common across all engines: Difficulty class for trap detection
        /// </remarks>
        public abstract int TrapDetectDC { get; set; }

        /// <summary>
        /// DC to disarm the trap.
        /// </summary>
        /// <remarks>
        /// Common across all engines: Difficulty class for trap disarming
        /// </remarks>
        public abstract int TrapDisarmDC { get; set; }

        /// <summary>
        /// Whether the trigger fires only once.
        /// </summary>
        /// <remarks>
        /// Common across all engines: Controls if trigger fires only once
        /// </remarks>
        public abstract bool FireOnce { get; set; }

        /// <summary>
        /// Whether the trigger has already been fired (for FireOnce triggers).
        /// </summary>
        /// <remarks>
        /// Common across all engines: Tracks if FireOnce trigger has fired
        /// </remarks>
        public virtual bool HasFired { get; set; }

        /// <summary>
        /// Tests if a point is inside the trigger volume.
        /// </summary>
        /// <remarks>
        /// Common implementation across all engines:
        /// - Uses ray casting algorithm for point-in-polygon test (2D projection)
        /// - Projects to X/Z plane (Y is height)
        /// - Returns true if point is inside polygon
        /// </remarks>
        public virtual bool ContainsPoint(Vector3 point)
        {
            if (_vertices.Count < 3)
            {
                return false;
            }

            // Ray casting algorithm for point-in-polygon test (2D projection)
            // Common algorithm used across all BioWare engines
            int crossings = 0;
            for (int i = 0; i < _vertices.Count; i++)
            {
                int j = (i + 1) % _vertices.Count;
                Vector3 v1 = _vertices[i];
                Vector3 v2 = _vertices[j];

                if ((v1.Z <= point.Z && v2.Z > point.Z) ||
                    (v2.Z <= point.Z && v1.Z > point.Z))
                {
                    float x = v1.X + (point.Z - v1.Z) / (v2.Z - v1.Z) * (v2.X - v1.X);
                    if (point.X < x)
                    {
                        crossings++;
                    }
                }
            }

            return (crossings % 2) == 1;
        }

        /// <summary>
        /// Tests if an entity is inside the trigger volume.
        /// </summary>
        /// <remarks>
        /// Common implementation across all engines:
        /// - Gets entity transform component
        /// - Tests if entity position is inside trigger volume
        /// </remarks>
        public virtual bool ContainsEntity(IEntity entity)
        {
            if (entity == null)
            {
                return false;
            }

            ITransformComponent transform = entity.GetComponent<ITransformComponent>();
            if (transform == null)
            {
                return false;
            }

            return ContainsPoint(transform.Position);
        }

        #endregion
    }
}

