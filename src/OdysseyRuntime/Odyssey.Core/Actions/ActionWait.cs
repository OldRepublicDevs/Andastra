using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Action to wait for a duration.
    /// </summary>
    public class ActionWait : ActionBase
    {
        private readonly float _duration;

        public ActionWait(float duration)
            : base(ActionType.Wait)
        {
            _duration = duration;
        }

        protected override ActionStatus ExecuteInternal(IEntity actor, float deltaTime)
        {
            if (ElapsedTime >= _duration)
            {
                return ActionStatus.Complete;
            }
            return ActionStatus.InProgress;
        }
    }
}

