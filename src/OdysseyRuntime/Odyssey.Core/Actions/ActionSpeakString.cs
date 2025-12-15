using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Action to speak a string (bark bubble).
    /// </summary>
    /// <remarks>
    /// Speak String Action:
    /// - Based on swkotor2.exe speech/bark system
    /// - Located via string references: "SpeakString" NWScript function, "ActionSpeakString" action type
    /// - Original implementation: Displays text bubble above entity, plays voice if available, lip sync animation
    /// - Talk volume: 0=Normal, 1=Whisper, 2=Shout, 3=Party, 4=Silent
    /// - Speech bubbles displayed above speaking entity for specified duration
    /// - Can play voice over (VO) audio if voice file exists matching dialogue string reference
    /// </remarks>
    public class ActionSpeakString : ActionBase
    {
        private readonly string _text;
        private readonly int _talkVolume;

        /// <summary>
        /// Creates a new speak string action.
        /// </summary>
        /// <param name="text">The text to speak.</param>
        /// <param name="talkVolume">0=Normal, 1=Whisper, 2=Shout, 3=Party, 4=Silent</param>
        public ActionSpeakString(string text, int talkVolume = 0)
            : base(ActionType.SpeakString)
        {
            _text = text ?? string.Empty;
            _talkVolume = talkVolume;
        }

        public string Text { get { return _text; } }
        public int TalkVolume { get { return _talkVolume; } }

        protected override ActionStatus ExecuteInternal(IEntity actor, float deltaTime)
        {
            // The actual UI display would be handled by the game layer
            // This action completes immediately after being processed
            return ActionStatus.Complete;
        }
    }
}

