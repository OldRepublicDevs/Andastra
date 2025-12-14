using System;
using System.Numerics;
using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;
using Odyssey.Core.Interfaces.Components;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Action to use/interact with an object (door, placeable, etc.).
    /// Based on NWScript ActionUseObject semantics.
    /// </summary>
    public class ActionUseObject : ActionBase
    {
        private readonly uint _targetObjectId;
        private const float UseDistance = 2.0f;
        private bool _reachedTarget;
        private bool _hasUsed;

        public ActionUseObject(uint targetObjectId)
            : base(ActionType.UseObject)
        {
            _targetObjectId = targetObjectId;
        }

        protected override ActionStatus ExecuteInternal(IEntity actor, float deltaTime)
        {
            ITransformComponent transform = actor.GetComponent<ITransformComponent>();
            if (transform == null)
            {
                return ActionStatus.Failed;
            }

            // Get target entity
            IEntity target = actor.World.GetEntity(_targetObjectId);
            if (target == null || !target.IsValid)
            {
                return ActionStatus.Failed;
            }

            ITransformComponent targetTransform = target.GetComponent<ITransformComponent>();
            if (targetTransform == null)
            {
                return ActionStatus.Failed;
            }

            // Get use point (could be from BWM hooks, but for now use object position)
            Vector3 usePoint = GetUsePoint(target, targetTransform);

            Vector3 toTarget = usePoint - transform.Position;
            toTarget.Y = 0; // Ignore vertical
            float distance = toTarget.Length();

            // Move to use point first
            if (distance > UseDistance)
            {
                if (!_reachedTarget)
                {
                    // Move towards target
                    IStatsComponent stats = actor.GetComponent<IStatsComponent>();
                    float speed = stats != null ? stats.RunSpeed : 5.0f;

                    Vector3 direction = Vector3.Normalize(toTarget);
                    float moveDistance = speed * deltaTime;

                    if (moveDistance > distance - UseDistance)
                    {
                        moveDistance = distance - UseDistance;
                    }

                    transform.Position += direction * moveDistance;
                    transform.Facing = (float)Math.Atan2(direction.X, direction.Z);
                }
                return ActionStatus.InProgress;
            }

            // Reached target - face it and use it
            if (!_reachedTarget)
            {
                _reachedTarget = true;
                Vector3 direction = Vector3.Normalize(toTarget);
                transform.Facing = (float)Math.Atan2(direction.X, direction.Z);
            }

            // Execute use logic
            if (!_hasUsed)
            {
                _hasUsed = true;

                // Fire OnUsed script event
                IScriptHooksComponent scriptHooks = target.GetComponent<IScriptHooksComponent>();
                if (scriptHooks != null)
                {
                    string scriptResRef = scriptHooks.GetScript(ScriptEvent.OnUsed);
                    if (!string.IsNullOrEmpty(scriptResRef))
                    {
                        // Fire script event via world event bus
                        IEventBus eventBus = actor.World.EventBus;
                        if (eventBus != null)
                        {
                            eventBus.FireScriptEvent(target, ScriptEvent.OnUsed, actor);
                        }
                    }
                }

                // Handle door/placeable specific logic
                IDoorComponent door = target.GetComponent<IDoorComponent>();
                if (door != null)
                {
                    if (!door.IsOpen)
                    {
                        // Try to open door
                        if (door.IsLocked)
                        {
                            // TODO: Check if actor can unlock
                            return ActionStatus.Failed;
                        }
                        door.Open();
                    }
                }

                IPlaceableComponent placeable = target.GetComponent<IPlaceableComponent>();
                if (placeable != null)
                {
                    if (!placeable.IsUsable)
                    {
                        return ActionStatus.Failed;
                    }
                    // TODO: Handle placeable use (inventory, etc.)
                }
            }

            return ActionStatus.Complete;
        }

        private Vector3 GetUsePoint(IEntity target, ITransformComponent targetTransform)
        {
            // TODO: Get from BWM hooks (USE1/USE2) if available
            // For now, use object position
            return targetTransform.Position;
        }
    }
}

