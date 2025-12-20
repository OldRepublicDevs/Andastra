using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;

namespace Andastra.Runtime.Core.Actions
{
    /// <summary>
    /// Base class for all actions.
    /// </summary>
    /// <remarks>
    /// Action Base:
    /// Legacy base class for actions. New code should use engine-specific base classes:
    /// - Odyssey: Runtime.Games.Odyssey.Actions.OdysseyAction
    /// - Aurora: Runtime.Games.Aurora.Actions.AuroraAction
    /// - Common: Runtime.Games.Common.Actions.BaseAction
    ///
    /// This class is kept for backward compatibility. Core cannot depend on Games, so this is a standalone implementation.
    /// </remarks>
    public abstract class ActionBase : IAction
    {
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
        /// Elapsed time since the action started executing.
        /// </summary>
        protected float ElapsedTime { get; private set; }

        protected ActionBase(ActionType type)
        {
            Type = type;
            ElapsedTime = 0f;
            GroupId = 0;
            Owner = null;
        }

        /// <summary>
        /// Executes the action. Returns the status of the action execution.
        /// </summary>
        protected abstract ActionStatus ExecuteInternal(IEntity actor, float deltaTime);

        /// <summary>
        /// Executes the action. This is the public entry point.
        /// </summary>
        public ActionStatus Execute(IEntity actor, float deltaTime)
        {
            ElapsedTime += deltaTime;
            return ExecuteInternal(actor, deltaTime);
        }

        /// <summary>
        /// Updates the action and returns its status (IAction interface).
        /// </summary>
        public ActionStatus Update(IEntity actor, float deltaTime)
        {
            return Execute(actor, deltaTime);
        }

        /// <summary>
        /// Called when the action is cancelled or completed.
        /// </summary>
        public virtual void Dispose()
        {
            // Default implementation does nothing
        }
    }
}

