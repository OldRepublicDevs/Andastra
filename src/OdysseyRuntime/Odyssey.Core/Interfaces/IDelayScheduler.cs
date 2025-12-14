namespace Odyssey.Core.Interfaces
{
    /// <summary>
    /// Scheduler for delayed actions (DelayCommand).
    /// </summary>
    public interface IDelayScheduler
    {
        /// <summary>
        /// Schedules an action to execute after a delay.
        /// </summary>
        void ScheduleDelay(float delaySeconds, IAction action, IEntity target);
        
        /// <summary>
        /// Updates the scheduler and fires any due actions.
        /// </summary>
        void Update(float deltaTime);
        
        /// <summary>
        /// Clears all delayed actions for a specific entity.
        /// </summary>
        void ClearForEntity(IEntity entity);
        
        /// <summary>
        /// Clears all delayed actions.
        /// </summary>
        void ClearAll();
        
        /// <summary>
        /// The number of pending delayed actions.
        /// </summary>
        int PendingCount { get; }
    }
}

