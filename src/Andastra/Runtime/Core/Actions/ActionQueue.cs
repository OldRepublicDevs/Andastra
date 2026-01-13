using System.Collections.Generic;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;

namespace Andastra.Runtime.Core.Actions
{
    /// <summary>
    /// FIFO action queue for entity actions.
    /// </summary>
    /// <remarks>
    /// Action Queue System:
    /// Legacy action queue class. New code should use engine-specific action queue classes:
    /// - BaseActionQueue (Andastra.Game.Games.Common.Actions) - Single implementation for all engines
    ///   - Engine-specific action queue classes (OdysseyActionQueue, AuroraActionQueue) have been merged into BaseActionQueue
    /// - Common: Runtime.Games.Common.Actions.BaseActionQueue
    /// 
    /// This class is kept for backward compatibility. Core cannot depend on Games.Common due to circular dependency,
    /// so this is a standalone implementation that matches BaseActionQueue functionality.
    /// </remarks>
    public class ActionQueue : IActionQueue
    {
        private IEntity _owner;
        private readonly LinkedList<IAction> _queue;
        private IAction _current;
        private int _lastInstructionCount;
        private int _accumulatedInstructionCount;

        public ActionQueue()
        {
            _queue = new LinkedList<IAction>();
        }

        public ActionQueue(IEntity owner) : this()
        {
            _owner = owner;
        }

        // IComponent implementation
        public IEntity Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        public void OnAttach()
        {
            // Initialize if needed
        }

        public void OnDetach()
        {
            Clear();
        }

        public IAction Current { get { return _current; } }
        public bool HasActions { get { return _current != null || _queue.Count > 0; } }
        public int Count { get { return _queue.Count + (_current != null ? 1 : 0); } }

        public void Add(IAction action)
        {
            if (action == null)
            {
                return;
            }

            action.Owner = _owner;
            _queue.AddLast(action);
        }

        public void AddFront(IAction action)
        {
            if (action == null)
            {
                return;
            }

            action.Owner = _owner;

            if (_current != null)
            {
                _queue.AddFirst(_current);
            }
            _current = action;
        }

        public void Clear()
        {
            if (_current != null)
            {
                _current.Dispose();
                _current = null;
            }

            foreach (IAction action in _queue)
            {
                action.Dispose();
            }
            _queue.Clear();
        }

        public void ClearByGroupId(int groupId)
        {
            if (_current != null && _current.GroupId == groupId)
            {
                _current.Dispose();
                _current = null;
            }

            LinkedListNode<IAction> node = _queue.First;
            while (node != null)
            {
                LinkedListNode<IAction> next = node.Next;
                if (node.Value.GroupId == groupId)
                {
                    node.Value.Dispose();
                    _queue.Remove(node);
                }
                node = next;
            }
        }

        public int Process(float deltaTime)
        {
            int instructionsExecuted = 0;

            // Get next action if we don't have one
            if (_current == null && _queue.Count > 0)
            {
                _current = _queue.First.Value;
                _queue.RemoveFirst();
            }

            if (_current == null)
            {
                // Store accumulated instruction count from this frame
                _lastInstructionCount = _accumulatedInstructionCount;
                return instructionsExecuted;
            }

            // Execute current action
            ActionStatus status = _current.Update(_owner, deltaTime);

            if (status != ActionStatus.InProgress)
            {
                // Action complete or failed - dispose and move to next
                _current.Dispose();
                _current = null;
            }

            // Store accumulated instruction count from this frame
            _lastInstructionCount = _accumulatedInstructionCount;
            return instructionsExecuted;
        }

        /// <summary>
        /// Adds instruction count from script execution to the accumulator.
        /// </summary>
        public void AddInstructionCount(int count)
        {
            if (count > 0)
            {
                _accumulatedInstructionCount += count;
            }
        }

        /// <summary>
        /// Gets the instruction count from the last Process() call.
        /// </summary>
        public int GetLastInstructionCount()
        {
            return _lastInstructionCount;
        }

        /// <summary>
        /// Resets the instruction count accumulator for the current frame.
        /// </summary>
        public void ResetInstructionCount()
        {
            _accumulatedInstructionCount = 0;
            _lastInstructionCount = 0;
        }

        public IEnumerable<IAction> GetAllActions()
        {
            if (_current != null)
            {
                yield return _current;
            }

            foreach (IAction action in _queue)
            {
                yield return action;
            }
        }

        public void Update(IEntity entity, float deltaTime)
        {
            Process(deltaTime);
        }
    }
}
