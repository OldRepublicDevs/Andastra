using System;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Scripting.Interfaces;

namespace Andastra.Runtime.Games.Common.Dialogue
{
    /// <summary>
    /// Abstract base class for dialogue managers across all engines.
    /// </summary>
    /// <remarks>
    /// Base Dialogue Manager:
    /// - Common functionality shared across all BioWare engines (Odyssey, Aurora, Eclipse, Infinity)
    /// - Base classes MUST only contain functionality that is identical across ALL engines
    /// - Engine-specific details MUST be in subclasses (OdysseyDialogueManager, AuroraDialogueManager, EclipseDialogueManager)
    /// - Common: Conversation state management, pause/resume, abort, update loop
    /// </remarks>
    public abstract class BaseDialogueManager
    {
        protected readonly INcsVm _vm;
        protected readonly IWorld _world;

        protected BaseDialogueManager(INcsVm vm, IWorld world)
        {
            if (vm == null)
            {
                throw new ArgumentNullException(nameof(vm));
            }
            if (world == null)
            {
                throw new ArgumentNullException(nameof(world));
            }

            _vm = vm;
            _world = world;
        }

        /// <summary>
        /// The current active dialogue state.
        /// </summary>
        [CanBeNull]
        public abstract BaseDialogueState CurrentState { get; }

        /// <summary>
        /// Whether a conversation is currently active.
        /// </summary>
        public bool IsConversationActive
        {
            get
            {
                var state = CurrentState;
                return state != null && state.IsActive;
            }
        }

        /// <summary>
        /// Starts a conversation with the specified dialogue.
        /// </summary>
        /// <param name="dialogueResRef">ResRef of the dialogue file</param>
        /// <param name="owner">The owner object (NPC, placeable, etc.)</param>
        /// <param name="pc">The player character</param>
        /// <returns>True if conversation started successfully</returns>
        public abstract bool StartConversation(string dialogueResRef, IEntity owner, IEntity pc);

        /// <summary>
        /// Advances the conversation by selecting a reply.
        /// </summary>
        /// <param name="replyIndex">Index of the reply in AvailableReplies</param>
        public abstract void SelectReply(int replyIndex);

        /// <summary>
        /// Skips the current node if skippable.
        /// </summary>
        public abstract void SkipNode();

        /// <summary>
        /// Pauses the current conversation.
        /// </summary>
        public virtual void PauseConversation()
        {
            var state = CurrentState;
            if (state != null && state.IsActive)
            {
                state.IsPaused = true;
            }
        }

        /// <summary>
        /// Resumes a paused conversation.
        /// </summary>
        public virtual void ResumeConversation()
        {
            var state = CurrentState;
            if (state != null && state.IsPaused)
            {
                state.IsPaused = false;
            }
        }

        /// <summary>
        /// Aborts the current conversation.
        /// </summary>
        public abstract void AbortConversation();

        /// <summary>
        /// Updates the dialogue system (call each frame).
        /// </summary>
        public abstract void Update(float deltaTime);
    }
}

