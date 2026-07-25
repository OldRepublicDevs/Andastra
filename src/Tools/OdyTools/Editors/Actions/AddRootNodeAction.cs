using System;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Editors.DLG;

namespace OdyTools.Editors.Actions
{
    // Action for adding a root node to the DLG dialog graph
    // Tracks the created link and item for undo/redo
    public class AddRootNodeAction : IDLGAction
    {
        private DLGLink _link;
        private DLGStandardItem _item;
        private int _index;

        public AddRootNodeAction()
        {
            // Constructor doesn't need parameters - the link and item are created during Apply
        }

        public void Apply(OdyToolDLG editor)
        {
            if (editor == null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            // Use the model to add the root node (this performs the actual operation)
            _item = editor.Model.AddRootNodeCore();
            if (_item == null || _item.Link == null)
            {
                throw new InvalidOperationException("Failed to create root node");
            }

            _link = _item.Link;
            _index = editor.CoreDlg.Starters.IndexOf(_link);

            // Update tree view
            editor.UpdateTreeView();
        }

        public void Undo(OdyToolDLG editor)
        {
            if (editor == null)
            {
                throw new ArgumentNullException(nameof(editor));
            }

            if (_link == null)
            {
                return; // Nothing to undo
            }

            // Remove link from CoreDlg and model
            editor.CoreDlg.Starters.Remove(_link);
            editor.Model.RemoveStarter(_link);

            // Update tree view
            editor.UpdateTreeView();
        }

        /// <summary>
        /// Gets the created item (for use by the editor to select it).
        /// </summary>
        public DLGStandardItem Item => _item;
    }
}
