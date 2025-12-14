using System.Numerics;
using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;
using Odyssey.Core.Interfaces.Components;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Action to instantly teleport to a location.
    /// </summary>
    public class ActionJumpToLocation : ActionBase
    {
        private readonly Vector3 _location;
        private readonly float _facing;

        public ActionJumpToLocation(Vector3 location, float facing = 0f)
            : base(ActionType.JumpToLocation)
        {
            _location = location;
            _facing = facing;
        }

        protected override ActionStatus ExecuteInternal(IEntity actor, float deltaTime)
        {
            var transform = actor.GetComponent<ITransformComponent>();
            if (transform == null)
            {
                return ActionStatus.Failed;
            }

            transform.Position = _location;
            transform.Facing = _facing;

            return ActionStatus.Complete;
        }
    }
}

