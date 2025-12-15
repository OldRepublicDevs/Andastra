using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Action to play an animation.
    /// </summary>
    /// <remarks>
    /// Play Animation Action:
    /// - Based on swkotor2.exe animation system
    /// - Located via string references: ActionPlayAnimation NWScript function implementation
    /// - Original implementation: Plays animation on entity, supports speed and duration parameters
    /// - Animation IDs reference animation indices in MDL animation arrays
    /// - Speed parameter controls playback rate (1.0 = normal, 2.0 = double speed, 0.5 = half speed)
    /// - Duration parameter controls how long animation plays (0 = play once, >0 = loop for duration)
    /// - Action completes when animation finishes or duration expires
    /// </remarks>
    public class ActionPlayAnimation : ActionBase
    {
        private readonly int _animation;
        private readonly float _duration;
        private readonly float _speed;
        private bool _started;

        public ActionPlayAnimation(int animation, float speed = 1.0f, float duration = 0f)
            : base(ActionType.PlayAnimation)
        {
            _animation = animation;
            _speed = speed;
            _duration = duration;
        }

        public int Animation { get { return _animation; } }
        public float Speed { get { return _speed; } }
        public float Duration { get { return _duration; } }

        protected override ActionStatus ExecuteInternal(IEntity actor, float deltaTime)
        {
            if (!_started)
            {
                _started = true;
                // Animation system would be notified here
            }

            // If duration is 0, play once and complete immediately
            if (_duration <= 0)
            {
                return ActionStatus.Complete;
            }

            if (ElapsedTime >= _duration)
            {
                return ActionStatus.Complete;
            }

            return ActionStatus.InProgress;
        }
    }
}

