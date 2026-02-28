using System;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Editors.DLG;

namespace OdyTools.Editors.Actions
{
    /// <summary>
    /// Action for adding a stunt to the DLG. Undo removes the stunt.
    /// </summary>
    public class AddStuntAction : IDLGAction
    {
        private readonly DLGStunt _stunt;

        public AddStuntAction(DLGStunt stunt)
        {
            _stunt = stunt ?? throw new ArgumentNullException(nameof(stunt));
        }

        public void Apply(OdyToolDLG editor)
        {
            if (editor?.CoreDlg == null) return;
            if (!editor.CoreDlg.Stunts.Contains(_stunt))
            {
                editor.CoreDlg.Stunts.Add(_stunt);
            }
            editor.RefreshStuntList();
            editor.OnNodeUpdate();
        }

        public void Undo(OdyToolDLG editor)
        {
            if (editor?.CoreDlg == null) return;
            editor.CoreDlg.Stunts.Remove(_stunt);
            editor.RefreshStuntList();
            editor.OnNodeUpdate();
        }
    }
}
