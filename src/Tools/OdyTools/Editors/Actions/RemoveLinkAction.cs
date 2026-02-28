using System;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Editors.DLG;

namespace OdyTools.Editors.Actions
{
    /// <summary>
    /// Action for removing a link from the DLG (either a starter or a child link).
    /// Tracks the item, link, parent and index for undo/redo.
    /// </summary>
    public class RemoveLinkAction : IDLGAction
    {
        private readonly DLGStandardItem _item;
        private readonly DLGLink _link;
        private readonly DLGStandardItem _parent;
        private readonly int _index;
        private readonly bool _isStarter;

        public RemoveLinkAction(OdyToolDLG editor, DLGStandardItem item)
        {
            if (editor == null) throw new ArgumentNullException(nameof(editor));
            _item = item ?? throw new ArgumentNullException(nameof(item));
            _link = item.Link ?? throw new ArgumentNullException(nameof(item));
            _parent = item.Parent;
            _isStarter = (_parent == null);
            _index = _isStarter
                ? (editor.CoreDlg?.Starters?.IndexOf(_link) ?? -1)
                : (_parent?.Link?.Node?.Links?.IndexOf(_link) ?? -1);
        }

        public void Apply(OdyToolDLG editor)
        {
            if (editor == null) throw new ArgumentNullException(nameof(editor));
            editor.RemoveLinkInternal(_item);
        }

        public void Undo(OdyToolDLG editor)
        {
            if (editor == null) throw new ArgumentNullException(nameof(editor));
            if (_isStarter)
            {
                if (_index >= 0 && _index <= editor.CoreDlg.Starters.Count)
                {
                    editor.CoreDlg.Starters.Insert(_index, _link);
                    editor.Model.InsertStarter(_index, _link);
                }
                else
                {
                    editor.CoreDlg.Starters.Add(_link);
                    editor.Model.AddStarter(_link);
                }
            }
            else if (_parent != null)
            {
                int insertAt = _index >= 0 ? _index : 0;
                editor.Model.InsertLinkToParentAsItem(_parent, _link, insertAt);
                return;
            }
            editor.UpdateTreeView();
        }

        public DLGStandardItem Item => _item;
        public DLGLink Link => _link;
        public bool IsStarter => _isStarter;
    }
}
