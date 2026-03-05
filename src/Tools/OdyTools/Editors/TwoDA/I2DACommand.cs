using System;

namespace OdyTools.Editors.TwoDACommands
{
    /// <summary>
    /// Interface for undoable 2DA table operations. All structural and content modifications
    /// should be implemented as commands to provide granular undo/redo semantics matching PyKotor behavior.
    /// </summary>
    public interface I2DACommand
    {
        /// <summary>Gets a user-readable description of this command for UI display (e.g., "Edit cell", "Insert row").</summary>
        string Description { get; }

        /// <summary>Executes the command, applying its changes to the 2DA table data.</summary>
        void Execute();

        /// <summary>Reverses the command, restoring the table state before Execute was called.</summary>
        void Undo();
    }
}
