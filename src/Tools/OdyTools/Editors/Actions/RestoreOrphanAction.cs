using System;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Editors.DLG;

namespace OdyTools.Editors.Actions
{
    /// <summary>
    /// Action for restoring an orphaned node into the tree at a given position. Undo removes the inserted link.
    /// </summary>
    public class RestoreOrphanAction : IDLGAction
    {
        private readonly DLGStandardItem _targetParent;
        private readonly int _intendedRow;
        private readonly DLGLink _newLink;
        private readonly DLGListWidgetItem _orphanListItem;
        private DLGStandardItem _insertedItem;

        public RestoreOrphanAction(DLGStandardItem targetParent, int intendedRow, DLGLink newLink, DLGListWidgetItem orphanListItem)
        {
            _targetParent = targetParent;
            _intendedRow = intendedRow;
            _newLink = newLink ?? throw new ArgumentNullException(nameof(newLink));
            _orphanListItem = orphanListItem ?? throw new ArgumentNullException(nameof(orphanListItem));
        }

        public void Apply(OdyToolDLG editor)
        {
            if (editor?.Model == null) return;
            _insertedItem = editor.Model.InsertLinkToParentAsItem(_targetParent, _newLink, _intendedRow);
            editor.OrphanedNodesList?.RemoveItem(_orphanListItem);
        }

        public void Undo(OdyToolDLG editor)
        {
            if (editor == null || _insertedItem == null) return;
            editor.RemoveLinkInternal(_insertedItem);
            editor.OrphanedNodesList?.AddItem(_orphanListItem);
        }
    }
}
