using System;

namespace OdyTools.Editors.DLG
{
    // Action interface for DLG editor undo/redo system
    // Based on command pattern for undoable operations
    // C# 7.3 compatible implementation
    public interface IDLGAction
    {
        // Apply the action to the editor
        void Apply(OdyToolDLG editor);

        // Undo the action, restoring previous state
        void Undo(OdyToolDLG editor);
    }
}


