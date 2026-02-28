using System;
using BioWare.Common;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Editors.DLG;

namespace OdyTools.Editors.Actions
{
    /// <summary>
    /// Action for editing a stunt (participant and stunt model). Undo restores previous values.
    /// </summary>
    public class EditStuntAction : IDLGAction
    {
        private readonly DLGStunt _stunt;
        private readonly string _oldParticipant;
        private readonly string _oldStuntModelStr;
        private readonly string _newParticipant;
        private readonly string _newStuntModelStr;

        public EditStuntAction(DLGStunt stunt, string oldParticipant, string oldStuntModelStr, string newParticipant, string newStuntModelStr)
        {
            _stunt = stunt ?? throw new ArgumentNullException(nameof(stunt));
            _oldParticipant = oldParticipant ?? "";
            _oldStuntModelStr = oldStuntModelStr ?? "";
            _newParticipant = newParticipant ?? "";
            _newStuntModelStr = newStuntModelStr ?? "";
        }

        public void Apply(OdyToolDLG editor)
        {
            if (_stunt == null) return;
            _stunt.Participant = _newParticipant;
            _stunt.StuntModel = string.IsNullOrEmpty(_newStuntModelStr) ? ResRef.FromBlank() : new ResRef(_newStuntModelStr);
            editor?.RefreshStuntList();
            editor?.OnNodeUpdate();
        }

        public void Undo(OdyToolDLG editor)
        {
            if (_stunt == null) return;
            _stunt.Participant = _oldParticipant;
            _stunt.StuntModel = string.IsNullOrEmpty(_oldStuntModelStr) ? ResRef.FromBlank() : new ResRef(_oldStuntModelStr);
            editor?.RefreshStuntList();
            editor?.OnNodeUpdate();
        }
    }
}
