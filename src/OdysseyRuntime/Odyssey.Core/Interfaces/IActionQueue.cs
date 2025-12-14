using System.Collections.Generic;

namespace Odyssey.Core.Interfaces
{
    /// <summary>
    /// Action queue for an entity.
    /// </summary>
    public interface IActionQueue : IComponent
    {
        /// <summary>
        /// Adds an action to the end of the queue.
        /// </summary>
        void Add(IAction action);
        
        /// <summary>
        /// Adds an action to the front of the queue.
        /// </summary>
        void AddFront(IAction action);
        
        /// <summary>
        /// Clears all actions from the queue.
        /// </summary>
        void Clear();
        
        /// <summary>
        /// Clears all actions with the specified group ID.
        /// </summary>
        void ClearByGroupId(int groupId);
        
        /// <summary>
        /// The currently executing action.
        /// </summary>
        IAction Current { get; }
        
        /// <summary>
        /// Whether there are any actions in the queue.
        /// </summary>
        bool HasActions { get; }
        
        /// <summary>
        /// The number of actions in the queue.
        /// </summary>
        int Count { get; }
        
        /// <summary>
        /// Processes the current action.
        /// Returns the number of script instructions executed.
        /// </summary>
        int Process(float deltaTime);
        
        /// <summary>
        /// Gets all actions in the queue.
        /// </summary>
        IEnumerable<IAction> GetAllActions();
    }
}

