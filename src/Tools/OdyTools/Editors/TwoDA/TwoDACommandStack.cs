using System;
using System.Collections.Generic;

namespace OdyTools.Editors.TwoDACommands
{
    /// <summary>
    /// Manages a stack of undoable 2DA commands, providing granular undo/redo semantics
    /// that match PyKotor's command-based implementation (vs. snapshot-based).
    /// </summary>
    public class TwoDACommandStack
    {
        private readonly Stack<I2DACommand> _undoStack = new Stack<I2DACommand>();
        private readonly Stack<I2DACommand> _redoStack = new Stack<I2DACommand>();
        private readonly int _maxUndoLevels;

        /// <summary>Event raised when a command is executed via this stack (not on direct redo/undo).</summary>
        public event EventHandler<I2DACommand> CommandExecuted;

        /// <summary>Event raised after an undo operation completes.</summary>
        public event EventHandler Undone;

        /// <summary>Event raised after a redo operation completes.</summary>
        public event EventHandler Redone;

        /// <summary>Gets whether undo is currently available.</summary>
        public bool CanUndo => _undoStack.Count > 0;

        /// <summary>Gets whether redo is currently available.</summary>
        public bool CanRedo => _redoStack.Count > 0;

        /// <summary>Gets the count of commands in the undo stack.</summary>
        public int UndoCount => _undoStack.Count;

        /// <summary>Gets the count of commands in the redo stack.</summary>
        public int RedoCount => _redoStack.Count;

        public TwoDACommandStack(int maxUndoLevels = 100)
        {
            _maxUndoLevels = Math.Max(1, maxUndoLevels);
        }

        /// <summary>
        /// Executes a command and pushes it onto the undo stack, clearing the redo stack.
        /// This is the primary method for applying changes with undo support.
        /// </summary>
        public void Execute(I2DACommand command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            command.Execute();
            _undoStack.Push(command);

            // Clear redo stack when a new command is executed (standard undo behavior)
            _redoStack.Clear();

            // Trim undo stack if it exceeds max levels
            if (_undoStack.Count > _maxUndoLevels)
            {
                var temp = new Stack<I2DACommand>(_maxUndoLevels);
                for (int i = 0; i < _maxUndoLevels; i++)
                {
                    temp.Push(_undoStack.Pop());
                }
                _undoStack.Clear();
                while (temp.Count > 0)
                {
                    _undoStack.Push(temp.Pop());
                }
            }

            CommandExecuted?.Invoke(this, command);
        }

        /// <summary>Undoes the most recent command if available.</summary>
        public void Undo()
        {
            if (!CanUndo) return;

            var command = _undoStack.Pop();
            command.Undo();
            _redoStack.Push(command);

            Undone?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Redoes the most recent undone command if available.</summary>
        public void Redo()
        {
            if (!CanRedo) return;

            var command = _redoStack.Pop();
            command.Execute();
            _undoStack.Push(command);

            Redone?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Clears both undo and redo stacks (use when loading a new document).</summary>
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }

        /// <summary>Gets the description of the command that would be undone, or null if none available.</summary>
        public string GetUndoDescription()
        {
            return CanUndo ? _undoStack.Peek().Description : null;
        }

        /// <summary>Gets the description of the command that would be redone, or null if none available.</summary>
        public string GetRedoDescription()
        {
            return CanRedo ? _redoStack.Peek().Description : null;
        }
    }
}
