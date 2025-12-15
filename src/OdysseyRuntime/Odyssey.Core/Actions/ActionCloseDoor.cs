using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;
using Odyssey.Core.Interfaces.Components;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Action to close a door.
    /// </summary>
    /// <remarks>
    /// Close Door Action:
    /// - Based on swkotor2.exe door closing system
    /// - Located via string references: "OnClosed" @ 0x007be1c8 (door script event)
    /// - Original implementation: Closes door, fires OnClosed script event
    /// - Door state: IsOpen flag set to false
    /// - Script events: OnClosed (door closed), OnLocked (door locked)
    /// - Based on NWScript ActionCloseDoor semantics
    /// </remarks>
    public class ActionCloseDoor : ActionBase
    {
        private readonly uint _doorObjectId;
        private bool _approached;
        private const float InteractRange = 2.0f;

        public ActionCloseDoor(uint doorObjectId)
            : base(ActionType.CloseDoor)
        {
            _doorObjectId = doorObjectId;
        }

        protected override ActionStatus ExecuteInternal(IEntity actor, float deltaTime)
        {
            ITransformComponent transform = actor.GetComponent<ITransformComponent>();
            if (transform == null)
            {
                return ActionStatus.Failed;
            }

            // Get door entity
            IEntity door = actor.World.GetEntity(_doorObjectId);
            if (door == null || !door.IsValid)
            {
                return ActionStatus.Failed;
            }

            ITransformComponent doorTransform = door.GetComponent<ITransformComponent>();
            if (doorTransform == null)
            {
                return ActionStatus.Failed;
            }

            Vector3 toTarget = doorTransform.Position - transform.Position;
            toTarget.Y = 0;
            float distance = toTarget.Length();

            // Move towards door if not in range
            if (distance > InteractRange && !_approached)
            {
                IStatsComponent stats = actor.GetComponent<IStatsComponent>();
                float speed = stats != null ? stats.WalkSpeed : 2.5f;

                Vector3 direction = Vector3.Normalize(toTarget);
                float moveDistance = speed * deltaTime;
                float targetDistance = distance - InteractRange;

                if (moveDistance > targetDistance)
                {
                    moveDistance = targetDistance;
                }

                transform.Position += direction * moveDistance;
                // Y-up system: Atan2(Y, X) for 2D plane facing
                transform.Facing = (float)System.Math.Atan2(direction.Y, direction.X);

                return ActionStatus.InProgress;
            }

            _approached = true;

            // Close the door
            IDoorComponent doorState = door.GetComponent<IDoorComponent>();
            if (doorState != null)
            {
                doorState.IsOpen = false;

                // Fire closed event
                IEventBus eventBus = actor.World.EventBus;
                if (eventBus != null)
                {
                    eventBus.Publish(new DoorClosedEvent { Actor = actor, Door = door });
                }
            }

            return ActionStatus.Complete;
        }
    }

    /// <summary>
    /// Event fired when a door is closed.
    /// </summary>
    public class DoorClosedEvent : IGameEvent
    {
        public IEntity Actor { get; set; }
        public IEntity Door { get; set; }
        public IEntity Entity { get { return Door; } }
    }
}

