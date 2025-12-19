using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Games.Common.Components;

namespace Andastra.Runtime.Games.Infinity.Components
{
    /// <summary>
    /// Infinity engine-specific trigger component implementation.
    /// </summary>
    /// <remarks>
    /// Infinity Trigger Component:
    /// - Inherits from BaseTriggerComponent for common trigger functionality
    /// - Infinity-specific: Trigger class, field names, transition system
    /// - Based on MassEffect.exe and MassEffect2.exe trigger system
    /// - Original implementation: Trigger class in MassEffect.exe and MassEffect2.exe
    /// - Infinity engine uses different trigger system than Odyssey/Aurora/Eclipse
    /// - Triggers have enter/exit detection, script firing, transitions, traps
    /// - Script events: OnEnter, OnExit, OnHeartbeat, OnClick, OnDisarm, OnTrapTriggered (fired via EventBus)
    /// - Trigger types: 0=generic, 1=transition, 2=trap (same as other engines)
    /// - Transition triggers: LinkedTo, LinkedToModule, LinkedToFlags for area/module transitions
    /// - Trap triggers: Can be detected/disarmed with DCs, fire OnTrapTriggered script
    /// - Note: Infinity engine trigger system is not fully reverse engineered yet
    /// </remarks>
    public class InfinityTriggerComponent : BaseTriggerComponent
    {
        /// <summary>
        /// Linked flags for transitions (Infinity-specific).
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: LinkedToFlags field in Trigger class
        /// </remarks>
        public int LinkedToFlags { get; set; }

        /// <summary>
        /// Trigger type (0 = generic, 1 = transition, 2 = trap) (Infinity-specific).
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: Type field in Trigger class
        /// </remarks>
        public int Type { get; set; }

        /// <summary>
        /// Trap detect DC (Infinity-specific).
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: TrapDetectDC field in Trigger class
        /// </remarks>
        private int _trapDetectDC;

        /// <summary>
        /// Trap disarm DC (Infinity-specific).
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: TrapDisarmDC field in Trigger class
        /// </remarks>
        private int _trapDisarmDC;

        /// <summary>
        /// Whether trap is one-shot (Infinity-specific).
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: TrapOneShot field in Trigger class
        /// </remarks>
        private bool _trapOneShot;

        /// <summary>
        /// Whether trigger is a trap (Infinity-specific).
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: Type == 2 indicates trap trigger
        /// </remarks>
        private bool _isTrap;

        #region ITriggerComponent Abstract Property Implementations

        /// <summary>
        /// Type of trigger (0=generic, 1=transition, 2=trap).
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: Type field in Trigger class
        /// </remarks>
        public override int TriggerType
        {
            get { return Type; }
            set
            {
                Type = value;
                _isTrap = (Type == 2);
            }
        }

        /// <summary>
        /// Whether this is a trap trigger.
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: Type == 2 indicates trap trigger
        /// </remarks>
        public override bool IsTrap
        {
            get { return _isTrap; }
            set
            {
                _isTrap = value;
                if (_isTrap)
                {
                    Type = 2;
                }
                else if (Type == 2)
                {
                    Type = 0;
                }
            }
        }

        /// <summary>
        /// DC to detect the trap.
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: TrapDetectDC field in Trigger class
        /// </remarks>
        public override int TrapDetectDC
        {
            get { return _trapDetectDC; }
            set { _trapDetectDC = value; }
        }

        /// <summary>
        /// DC to disarm the trap.
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: TrapDisarmDC field in Trigger class
        /// </remarks>
        public override int TrapDisarmDC
        {
            get { return _trapDisarmDC; }
            set { _trapDisarmDC = value; }
        }

        /// <summary>
        /// Whether the trigger fires only once.
        /// </summary>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: TrapOneShot field in Trigger class
        /// </remarks>
        public override bool FireOnce
        {
            get { return _trapOneShot; }
            set { _trapOneShot = value; }
        }

        #endregion
    }
}

