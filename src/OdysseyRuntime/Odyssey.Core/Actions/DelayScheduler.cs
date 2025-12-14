using System.Collections.Generic;
using Odyssey.Core.Interfaces;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Scheduler for delayed actions using a priority queue.
    /// </summary>
    public class DelayScheduler : IDelayScheduler
    {
        private readonly List<DelayedAction> _delayedActions;
        private float _currentTime;

        public DelayScheduler()
        {
            _delayedActions = new List<DelayedAction>();
        }

        public int PendingCount { get { return _delayedActions.Count; } }

        public void ScheduleDelay(float delaySeconds, IAction action, IEntity target)
        {
            if (action == null || target == null)
            {
                return;
            }

            float executeTime = _currentTime + delaySeconds;
            
            var delayed = new DelayedAction
            {
                ExecuteTime = executeTime,
                Action = action,
                Target = target
            };

            // Insert in sorted order (ascending by execute time)
            int index = 0;
            while (index < _delayedActions.Count && _delayedActions[index].ExecuteTime <= executeTime)
            {
                index++;
            }
            _delayedActions.Insert(index, delayed);
        }

        public void Update(float deltaTime)
        {
            _currentTime += deltaTime;

            // Process all actions that are due
            while (_delayedActions.Count > 0 && _delayedActions[0].ExecuteTime <= _currentTime)
            {
                var delayed = _delayedActions[0];
                _delayedActions.RemoveAt(0);

                if (delayed.Target.IsValid)
                {
                    var actionQueue = delayed.Target.GetComponent<IActionQueue>();
                    if (actionQueue != null)
                    {
                        actionQueue.Add(delayed.Action);
                    }
                    else
                    {
                        // Execute immediately if no action queue
                        delayed.Action.Owner = delayed.Target;
                        delayed.Action.Update(delayed.Target, 0);
                        delayed.Action.Dispose();
                    }
                }
                else
                {
                    delayed.Action.Dispose();
                }
            }
        }

        public void ClearForEntity(IEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            _delayedActions.RemoveAll(d =>
            {
                if (d.Target == entity)
                {
                    d.Action.Dispose();
                    return true;
                }
                return false;
            });
        }

        public void ClearAll()
        {
            foreach (var delayed in _delayedActions)
            {
                delayed.Action.Dispose();
            }
            _delayedActions.Clear();
        }

        /// <summary>
        /// Resets the current time (for testing).
        /// </summary>
        public void Reset()
        {
            ClearAll();
            _currentTime = 0;
        }

        private struct DelayedAction
        {
            public float ExecuteTime;
            public IAction Action;
            public IEntity Target;
        }
    }
}

