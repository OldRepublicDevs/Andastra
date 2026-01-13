using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;

namespace Andastra.Runtime.Core.Actions
{
    /// <summary>
    /// Action to play an animation.
    /// </summary>
    /// <remarks>
    /// Play Animation Action:
    /// - CSWSObject::AIActionPlayAnimation @ (K1: 0x0057d080, TSL: TODO: Find this address) animation system
    /// - Located via string references: "Animation" @ 0x00746060 (K1), "Animation" @ 0x007bf604 (TSL)
    /// - "EVENT_PLAY_ANIMATION" @ 0x00744b3c (K1), "EVENT_PLAY_ANIMATION" @ 0x007bcd74 (TSL)
    /// - "PlayAnim" @ 0x007c346c, "AnimLoop" @ 0x007c4c70 (animation loop flag)
    /// - "CurrentAnim" @ 0x007c38d4, "NextAnim" @ 0x007c38c8 (animation state tracking)
    /// - Animation timing: "frameStart" @ 0x007ba698, "frameEnd" @ 0x007ba668 (animation frame timing)
    /// - ActionPlayAnimation NWScript function (routine 40) queues animation action to entity action queue
    /// - Called from RunActions when action type is 6 (PlayAnimation) in action node structure
    /// - Original implementation: Plays animation on entity, supports speed and duration parameters
    /// - Animation IDs reference animation indices in MDL animation arrays (must be >= 10000 for NWScript)
    /// - Speed parameter controls playback rate (1.0 = normal, 2.0 = double speed, 0.5 = half speed)
    /// - Duration parameter controls how long animation plays (0 = play once, >0 = loop for duration)
    /// - Action node structure: param_1+0x38 = animation ID, param_1+0x3c = speed, param_1+0x40 = duration
    /// - Function reads animation length from client object, calculates duration based on speed
    /// - Plays animation via vtable function (field31_0x7c) with animation ID and speed
    /// - Returns action status: 0 = in progress, 1 = complete, 2 = failed, 3 = aborted
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
            // CSWSObject::AIActionPlayAnimation @ (K1: 0x0057d080, TSL: TODO: Find this address): ActionPlayAnimation implementation
            // Located via string references: "Animation" @ 0x00746060 (K1), "Animation" @ 0x007bf604 (TSL)
            // Called from RunActions when action type is 6 (PlayAnimation) in action node structure
            // Original implementation: Plays animation on entity's animation component via client object
            // Animation ID references animation index in MDL animation array (must be >= 10000 for NWScript)
            // Speed parameter controls playback rate (1.0 = normal speed, stored in field44_0xd8)
            // Duration parameter: 0 = play once, >0 = loop for specified duration
            // Function calculates animation duration: animationLength / speed, or duration * 1000 if duration > 0
            // Plays animation via vtable function (field31_0x7c) with animation ID (uVar3 & 0xffff)
            // For fire-and-forget animations (duration = 0), checks AnimationFireAndForget flag
            // Special handling for player creature: adjusts duration for lockpick/mine animations
            // Action node structure offsets: +0x38 = animation ID, +0x3c = speed, +0x40 = duration, +0x44 = first run flag, +0x48 = complete flag, +0x4c = duration override
            if (!_started)
            {
                _started = true;

                // Get animation component and play animation
                var animationComponent = actor.GetComponent<Interfaces.Components.IAnimationComponent>();
                if (animationComponent != null)
                {
                    // Duration > 0 means loop for that duration, duration = 0 means play once
                    bool loop = _duration > 0;
                    animationComponent.PlayAnimation(_animation, _speed, loop);
                }
            }

            // If duration is 0, play once and wait for animation to complete
            // Original engine: Action completes when animation finishes (checked by animation system)
            if (_duration <= 0)
            {
                var animationComponent = actor.GetComponent<Interfaces.Components.IAnimationComponent>();
                if (animationComponent != null && animationComponent.AnimationComplete)
                {
                    return ActionStatus.Complete;
                }
                return ActionStatus.InProgress;
            }

            // If duration > 0, loop animation for specified duration
            // Original engine: Animation loops until duration expires
            if (ElapsedTime >= _duration)
            {
                // Stop looping animation
                var animationComponent = actor.GetComponent<Interfaces.Components.IAnimationComponent>();
                if (animationComponent != null)
                {
                    animationComponent.StopAnimation();
                }
                return ActionStatus.Complete;
            }

            return ActionStatus.InProgress;
        }
    }
}

