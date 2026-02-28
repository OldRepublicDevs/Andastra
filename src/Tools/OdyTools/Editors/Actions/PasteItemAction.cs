using System;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Editors.DLG;

namespace OdyTools.Editors.Actions
{
    /// <summary>
    /// Action for pasting a link (or deep copy) into the DLG tree. Undo removes the pasted item.
    /// </summary>
    public class PasteItemAction : IDLGAction
    {
        private readonly DLGStandardItem _parentItem;
        private readonly int? _row;
        private readonly DLGLink _copy;
        private readonly bool _asNewBranches;
        private DLGStandardItem _addedItem;

        public PasteItemAction(DLGStandardItem parentItem, int? row, DLGLink copy, bool asNewBranches)
        {
            _parentItem = parentItem;
            _row = row;
            _copy = copy ?? throw new ArgumentNullException(nameof(copy));
            _asNewBranches = asNewBranches;
        }

        public void Apply(OdyToolDLG editor)
        {
            if (editor?.Model == null) return;
            editor.Model.PasteItem(_parentItem, _copy, _row, _asNewBranches);
            int insertedIndex = _row.HasValue && _row.Value >= 0
                ? _row.Value
                : (_parentItem == null
                    ? editor.Model.GetRootItems().Count - 1
                    : _parentItem.Children.Count - 1);
            if (insertedIndex < 0) return;
            _addedItem = _parentItem == null
                ? editor.Model.GetRootItems()[insertedIndex]
                : _parentItem.Children[insertedIndex];
            editor.UpdateTreeView();
        }

        public void Undo(OdyToolDLG editor)
        {
            if (editor == null || _addedItem == null) return;
            editor.RemoveLinkInternal(_addedItem);
        }
    }
}
