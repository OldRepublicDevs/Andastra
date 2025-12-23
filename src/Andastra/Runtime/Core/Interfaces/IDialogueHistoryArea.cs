using System.Collections.Generic;

namespace Andastra.Runtime.Core.Interfaces
{
    /// <summary>
    /// Represents a single entry in the dialogue history.
    /// </summary>
    public class DialogueHistoryEntry
    {
        /// <summary>
        /// The name of the speaker.
        /// </summary>
        public string SpeakerName { get; }

        /// <summary>
        /// The message text.
        /// </summary>
        public string MessageText { get; }

        /// <summary>
        /// The timestamp when the dialogue occurred.
        /// </summary>
        public float Timestamp { get; }

        /// <summary>
        /// Initializes a new instance of the DialogueHistoryEntry class.
        /// </summary>
        public DialogueHistoryEntry(string speakerName, string messageText, float timestamp)
        {
            SpeakerName = speakerName;
            MessageText = messageText;
            Timestamp = timestamp;
        }
    }

    /// <summary>
    /// Represents an area that supports dialogue history tracking.
    /// </summary>
    /// <remarks>
    /// Eclipse Engine Specific (daorigins.exe, DragonAge2.exe):
    /// - Dialogue history tracks conversations that occur in the area
    /// - Used for rendering dialogue history UI in graphics backends
    /// - Each entry contains speaker name, message text, and timestamp
    /// </remarks>
    public interface IDialogueHistoryArea
    {
        /// <summary>
        /// Adds a dialogue history entry.
        /// </summary>
        /// <param name="speakerName">The name of the speaker.</param>
        /// <param name="messageText">The message text.</param>
        /// <param name="timestamp">The timestamp when the dialogue occurred.</param>
        void AddDialogueHistoryEntry(string speakerName, string messageText, float timestamp);

        /// <summary>
        /// Gets the dialogue history entries.
        /// </summary>
        /// <returns>A read-only list of dialogue history entries.</returns>
        IReadOnlyList<DialogueHistoryEntry> GetDialogueHistory();
    }
}

