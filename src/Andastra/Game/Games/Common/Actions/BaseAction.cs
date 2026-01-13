using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;

namespace Andastra.Game.Games.Common.Actions
{
    /// <summary>
    /// Base class for all actions shared across BioWare engines that use GFF-based action serialization.
    /// </summary>
    /// <remarks>
    /// Base Action Implementation:
    /// Common action system shared across Odyssey (swkotor.exe, swkotor2.exe) and Aurora (nwmain.exe, nwn2main.exe).
    ///
    /// All engine-specific action classes (OdysseyAction, AuroraAction) have been merged into this base class
    /// since they are abstract classes that only call the base constructor with no additional implementation.
    ///
    /// Common structure across engines:
    /// - ActionId (uint32): Action type identifier stored in GFF ActionList
    /// - GroupActionId (int16): Group ID for batching/clearing related actions together
    /// - NumParams (int16): Number of parameters (0-13 max parameters)
    /// - Paramaters array: Type/Value pairs stored in GFF structure
    /// - Parameter types: 1=int, 2=float, 3=object/uint32, 4=string, 5=location/vector
    /// - ActionList GFF field: List of actions stored in entity GFF structures
    /// - Actions processed sequentially: Current action executes until complete, then next action dequeued
    /// - Action status: InProgress (continue), Complete (done), Failed (abort)
    ///
    /// Engine-Specific Details (Documented):
    /// - Odyssey (swkotor.exe, swkotor2.exe):
    ///   - Located via string references: "ActionList" @ 0x007bebdc (swkotor2.exe), "ActionList" @ 0x00745ea0 (swkotor.exe)
    ///   - swkotor2.exe: 0x00508260 @ 0x00508260 (load ActionList from GFF), 0x00505bc0 @ 0x00505bc0 (save ActionList to GFF)
    ///   - swkotor.exe: 0x004cecb0 @ 0x004cecb0 (load ActionList from GFF), 0x004cc7e0 @ 0x004cc7e0 (save ActionList to GFF)
    ///   - Parameters stored as ActionParam1-5 for numeric, ActionParamStrA/B for strings, ActionParam1b-5b for booleans
    ///   - EVENT_FORCED_ACTION @ 0x007bccac (swkotor2.exe), @ 0x00744a74 (swkotor.exe) (forced action event constant)
    /// - Aurora (nwmain.exe, nwn2main.exe):
    ///   - Located via string references: "ActionList" @ 0x140df11e0 (nwmain.exe)
    ///   - CNWSObject::LoadActionQueue @ 0x1404963f0 (nwmain.exe) - loads ActionList from GFF
    ///   - CNWSObject::SaveActionQueue @ 0x140499910 (nwmain.exe) - saves ActionList to GFF
    ///   - Parameter types include 6=byte (Aurora-specific)
    ///   - CNWSObject class structure: Actions stored in CExoLinkedList at offset +0x100
    /// - Eclipse (daorigins.exe, DragonAge2.exe): Uses ActionFramework (different architecture)
    /// - Infinity: May use different system (needs investigation)
    /// </remarks>
    public abstract class BaseAction : IAction
    {
        protected float ElapsedTime;

        protected BaseAction(ActionType type)
        {
            Type = type;
            GroupId = -1;
        }

        /// <summary>
        /// The type of this action.
        /// </summary>
        public ActionType Type { get; }

        /// <summary>
        /// Group ID for clearing related actions.
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// The entity that owns this action.
        /// </summary>
        public IEntity Owner { get; set; }

        /// <summary>
        /// Updates the action and returns its status.
        /// </summary>
        public ActionStatus Update(IEntity actor, float deltaTime)
        {
            ElapsedTime += deltaTime;
            return ExecuteInternal(actor, deltaTime);
        }

        /// <summary>
        /// Executes the action logic. Override in derived classes.
        /// </summary>
        protected abstract ActionStatus ExecuteInternal(IEntity actor, float deltaTime);

        /// <summary>
        /// Called when the action is cancelled or completed.
        /// </summary>
        public virtual void Dispose()
        {
            // Override in derived classes if cleanup is needed
        }
    }
}

