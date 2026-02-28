using System;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Editors.DLG;

namespace OdyTools.Editors.Actions
{
    /// <summary>
    /// Action for removing a stunt from the DLG. Undo re-inserts it at the original index.
    /// </summary>
    public class RemoveStuntAction : IDLGAction
    {
        private readonly DLGStunt _stunt;
        private readonly int _index;

        public RemoveStuntAction(OdyToolDLG editor, DLGStunt stunt)
        {
            if (editor == null) throw new ArgumentNullException(nameof(editor));
            _stunt = stunt ?? throw new ArgumentNullException(nameof(stunt));
            _index = editor.CoreDlg?.Stunts?.IndexOf(_stunt) ?? -1;
        }

        public void Apply(OdyToolDLG editor)
        {
            if (editor?.CoreDlg == null) return;
            editor.CoreDlg.Stunts.Remove(_stunt);
            editor.RefreshStuntList();
            editor.OnNodeUpdate();
        }

        public void Undo(OdyToolDLG editor)
        {
            if (editor?.CoreDlg == null) return;
            if (_index >= 0 && _index <= editor.CoreDlg.Stunts.Count)
            {
                editor.CoreDlg.Stunts.Insert(_index, _stunt);
            }
            else
            {
                editor.CoreDlg.Stunts.Add(_stunt);
            }
            editor.RefreshStuntList();
            editor.OnNodeUpdate();
        }
    }
}
