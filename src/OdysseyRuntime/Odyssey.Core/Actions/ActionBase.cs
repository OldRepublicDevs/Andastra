using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Base class for all actions.
    /// </summary>
    public abstract class ActionBase : IAction
    {
        protected float ElapsedTime;

        protected ActionBase(ActionType type)
        {
            Type = type;
            GroupId = -1;
        }

        public ActionType Type { get; }
        public int GroupId { get; set; }
        public IEntity Owner { get; set; }

        public ActionStatus Update(IEntity actor, float deltaTime)
        {
            ElapsedTime += deltaTime;
            return ExecuteInternal(actor, deltaTime);
        }

        protected abstract ActionStatus ExecuteInternal(IEntity actor, float deltaTime);

        public virtual void Dispose()
        {
            // Override in derived classes if cleanup is needed
        }
    }
}

