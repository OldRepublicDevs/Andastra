using Andastra.Runtime.Core.Actions;

namespace Andastra.Runtime.Core.Interfaces.Components
{
    /// <summary>
    /// Component for entities that have an action queue.
    /// </summary>
    /// <remarks>
    /// Action Queue Component Interface:
    /// - TODO: lookup data from daorigins.exe/dragonage2.exe/masseffect.exe/masseffect2.exe/swkotor.exe/swkotor2.exe and split into subclass'd inheritence structures appropriately. parent class(es) should contain common code.
    /// - TODO: this should NOT specify swkotor2.exe unless it specifies the other exes as well!!!
    /// - Based on swkotor2.exe action system
    /// - Located via string references: "ActionList" @ 0x007bebdc, "ActionId" @ 0x007bebd0
    /// - Original implementation: Entities maintain action queue with current action and pending actions
    /// - Actions processed sequentially: Current action executes until complete, then next action dequeued
    /// - Update processes current action, returns number of script instructions executed
    /// - Action types: Move, Attack, UseObject, SpeakString, PlayAnimation, etc.
    /// </remarks>
    public interface IActionQueueComponent : IComponent
    {
        /// <summary>
        /// Gets the current action being executed.
        /// </summary>
        IAction CurrentAction { get; }

        /// <summary>
        /// Gets the number of queued actions.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Adds an action to the queue.
        /// </summary>
        void Add(IAction action);

        /// <summary>
        /// Clears all queued actions.
        /// </summary>
        void Clear();

        /// <summary>
        /// Updates the action queue.
        /// </summary>
        void Update(IEntity entity, float deltaTime);

        /// <summary>
        /// Gets the number of script instructions executed during the last update.
        /// </summary>
        /// <returns>The instruction count from the last update, or 0 if no scripts were executed.</returns>
        /// <remarks>
        /// Instruction Count Tracking:
        /// - Based on swkotor2.exe script budget system
        /// - Located via string references: Script execution budget limits per frame
        /// - Original implementation: Tracks instruction count to enforce per-frame script budget limits
        /// - Used by game loop to prevent script lockups (MaxScriptBudget constant)
        /// - Instruction count accumulates from all script executions (ExecuteScript, heartbeats, etc.)
        /// - Reset each frame before processing action queues
        /// </remarks>
        int GetLastInstructionCount();

        /// <summary>
        /// Resets the instruction count accumulator for the current frame.
        /// </summary>
        /// <remarks>
        /// Called by game loop at the start of each frame to reset instruction tracking.
        /// </remarks>
        void ResetInstructionCount();

        /// <summary>
        /// Adds instruction count from script execution to the accumulator.
        /// </summary>
        /// <param name="count">The number of instructions executed.</param>
        /// <remarks>
        /// Instruction Count Accumulation:
        /// - Called when scripts execute (ExecuteScript, heartbeats, etc.) to track instruction usage
        /// - Accumulates instruction count per frame for budget enforcement
        /// </remarks>
        void AddInstructionCount(int count);
    }
}

