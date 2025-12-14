using Odyssey.Core.Actions;

namespace Odyssey.Core.Interfaces.Components
{
    /// <summary>
    /// Component for entities that have an action queue.
    /// </summary>
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
    }
}

