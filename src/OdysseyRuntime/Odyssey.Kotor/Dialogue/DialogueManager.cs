using System;
using System.Collections.Generic;
using Odyssey.Core.Interfaces;

namespace Odyssey.Kotor.Dialogue
{
    /// <summary>
    /// Manages dialogue state and flow.
    /// TODO: Integrate with CSharpKOTOR DLG parsing
    /// TODO: Execute script conditionals and actions
    /// TODO: Handle dialogue animations
    /// </summary>
    public class DialogueManager
    {
        private DialogueState _state;
        private string _currentDialogueResRef;
        private IEntity _owner;
        private IEntity _player;

        // TODO: TLK string lookup
        // TODO: Camera cuts
        // TODO: Voice audio playback

        public event Action<DialogueNode> OnNodeChanged;
        public event Action OnDialogueEnded;

        public DialogueManager()
        {
            _state = DialogueState.None;
        }

        /// <summary>
        /// Gets the current dialogue state.
        /// </summary>
        public DialogueState State { get { return _state; } }

        /// <summary>
        /// Gets whether a dialogue is currently active.
        /// </summary>
        public bool IsActive { get { return _state != DialogueState.None; } }

        /// <summary>
        /// Gets the current dialogue node.
        /// </summary>
        public DialogueNode CurrentNode { get; private set; }

        /// <summary>
        /// Gets the available player replies.
        /// </summary>
        public IReadOnlyList<DialogueReply> Replies { get; private set; }

        /// <summary>
        /// Start a dialogue with an NPC.
        /// </summary>
        public void StartDialogue(string dialogueResRef, IEntity owner, IEntity player)
        {
            Console.WriteLine("[DialogueManager] Starting dialogue: " + dialogueResRef);

            _currentDialogueResRef = dialogueResRef;
            _owner = owner;
            _player = player;
            _state = DialogueState.Loading;

            // TODO: Load DLG file using CSharpKOTOR
            // TODO: Execute OnConversation script

            // For now, create placeholder dialogue
            LoadPlaceholderDialogue();
        }

        private void LoadPlaceholderDialogue()
        {
            // Create a simple test dialogue
            CurrentNode = new DialogueNode
            {
                Index = 0,
                Speaker = _owner != null ? _owner.Tag : "NPC",
                Text = "Greetings, traveler. What brings you here?",
                VoiceResRef = "",
                CameraAngle = 0
            };

            Replies = new List<DialogueReply>
            {
                new DialogueReply
                {
                    Index = 0,
                    Text = "I'm looking for information.",
                    NextNodeIndex = 1
                },
                new DialogueReply
                {
                    Index = 1,
                    Text = "Just passing through.",
                    NextNodeIndex = 2
                },
                new DialogueReply
                {
                    Index = 2,
                    Text = "[End conversation]",
                    NextNodeIndex = -1
                }
            };

            _state = DialogueState.WaitingForReply;

            if (OnNodeChanged != null)
            {
                OnNodeChanged(CurrentNode);
            }

            Console.WriteLine("[DialogueManager] FIXME: Using placeholder dialogue");
        }

        /// <summary>
        /// Select a reply option.
        /// </summary>
        public void SelectReply(int replyIndex)
        {
            if (_state != DialogueState.WaitingForReply)
            {
                return;
            }

            if (replyIndex < 0 || Replies == null || replyIndex >= Replies.Count)
            {
                return;
            }

            var reply = Replies[replyIndex];
            Console.WriteLine("[DialogueManager] Selected reply: " + reply.Text);

            // TODO: Execute reply script (ActionTaken)

            if (reply.NextNodeIndex < 0)
            {
                EndDialogue();
            }
            else
            {
                AdvanceToNode(reply.NextNodeIndex);
            }
        }

        private void AdvanceToNode(int nodeIndex)
        {
            // TODO: Load actual node from DLG

            // Placeholder response
            CurrentNode = new DialogueNode
            {
                Index = nodeIndex,
                Speaker = _owner != null ? _owner.Tag : "NPC",
                Text = "I see. Is there anything else you need?",
                VoiceResRef = "",
                CameraAngle = 0
            };

            Replies = new List<DialogueReply>
            {
                new DialogueReply
                {
                    Index = 0,
                    Text = "No, that's all.",
                    NextNodeIndex = -1
                }
            };

            _state = DialogueState.WaitingForReply;

            if (OnNodeChanged != null)
            {
                OnNodeChanged(CurrentNode);
            }
        }

        /// <summary>
        /// End the current dialogue.
        /// </summary>
        public void EndDialogue()
        {
            Console.WriteLine("[DialogueManager] Ending dialogue: " + _currentDialogueResRef);

            // TODO: Execute OnConversationEnd script

            _state = DialogueState.None;
            _currentDialogueResRef = null;
            CurrentNode = null;
            Replies = null;

            if (OnDialogueEnded != null)
            {
                OnDialogueEnded();
            }
        }

        /// <summary>
        /// Abort the dialogue immediately.
        /// </summary>
        public void AbortDialogue()
        {
            if (_state == DialogueState.None)
            {
                return;
            }

            Console.WriteLine("[DialogueManager] Aborting dialogue");
            _state = DialogueState.None;
            _currentDialogueResRef = null;
            CurrentNode = null;
            Replies = null;

            if (OnDialogueEnded != null)
            {
                OnDialogueEnded();
            }
        }
    }

    /// <summary>
    /// Dialogue state machine states.
    /// </summary>
    public enum DialogueState
    {
        None,
        Loading,
        Playing,
        WaitingForReply,
        Ending
    }

    /// <summary>
    /// A single node in a dialogue tree.
    /// </summary>
    public class DialogueNode
    {
        public int Index { get; set; }
        public string Speaker { get; set; }
        public string Text { get; set; }
        public string VoiceResRef { get; set; }
        public int CameraAngle { get; set; }
        // TODO: Script (Condition, ActionTaken)
        // TODO: Animation
        // TODO: Delay
    }

    /// <summary>
    /// A player reply option.
    /// </summary>
    public class DialogueReply
    {
        public int Index { get; set; }
        public string Text { get; set; }
        public int NextNodeIndex { get; set; }
        public bool IsAvailable { get; set; } = true;
        // TODO: Condition script
    }
}

