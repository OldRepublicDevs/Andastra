using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Formats.SSF;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/ssf.py:21
    // Original: class SSFEditor(Editor):
    public class SSFEditor : Editor
    {
        // UI controls - will be populated from XAML or created programmatically
        private NumericUpDown _battlecry1StrrefSpin;
        private NumericUpDown _battlecry2StrrefSpin;
        private NumericUpDown _battlecry3StrrefSpin;
        private NumericUpDown _battlecry4StrrefSpin;
        private NumericUpDown _battlecry5StrrefSpin;
        private NumericUpDown _battlecry6StrrefSpin;
        private NumericUpDown _select1StrrefSpin;
        private NumericUpDown _select2StrrefSpin;
        private NumericUpDown _select3StrrefSpin;
        private NumericUpDown _attack1StrrefSpin;
        private NumericUpDown _attack2StrrefSpin;
        private NumericUpDown _attack3StrrefSpin;
        private NumericUpDown _pain1StrrefSpin;
        private NumericUpDown _pain2StrrefSpin;
        private NumericUpDown _lowHpStrrefSpin;
        private NumericUpDown _deadStrrefSpin;
        private NumericUpDown _criticalStrrefSpin;
        private NumericUpDown _immuneStrrefSpin;
        private NumericUpDown _layMineStrrefSpin;
        private NumericUpDown _disarmMineStrrefSpin;
        private NumericUpDown _beginStealthStrrefSpin;
        private NumericUpDown _beginSearchStrrefSpin;
        private NumericUpDown _beginUnlockStrrefSpin;
        private NumericUpDown _unlockSuccessStrrefSpin;
        private NumericUpDown _unlockFailedStrrefSpin;
        private NumericUpDown _partySeparatedStrrefSpin;
        private NumericUpDown _rejoinPartyStrrefSpin;
        private NumericUpDown _poisonedStrrefSpin;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/ssf.py:22-61
        // Original: def __init__(self, parent, installation):
        public SSFEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Soundset Editor", "soundset", new[] { ResourceType.SSF }, new[] { ResourceType.SSF }, installation)
        {
            InitializeComponent();
            SetupSignals();
            New();
            MinWidth = 577;
            MinHeight = 437;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            // Initialize controls - will be done via XAML or programmatically
            // For now, create basic structure
            var panel = new StackPanel();
            Content = panel;
        }

        private void SetupSignals()
        {
            // Connect value changed events for all spin boxes
            // This will be implemented when UI controls are created
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/ssf.py:106-157
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            SSF ssf = SSF.FromBytes(data);

            if (_battlecry1StrrefSpin != null) _battlecry1StrrefSpin.Value = ssf.Get(SSFSound.BATTLE_CRY_1) ?? 0;
            if (_battlecry2StrrefSpin != null) _battlecry2StrrefSpin.Value = ssf.Get(SSFSound.BATTLE_CRY_2) ?? 0;
            if (_battlecry3StrrefSpin != null) _battlecry3StrrefSpin.Value = ssf.Get(SSFSound.BATTLE_CRY_3) ?? 0;
            if (_battlecry4StrrefSpin != null) _battlecry4StrrefSpin.Value = ssf.Get(SSFSound.BATTLE_CRY_4) ?? 0;
            if (_battlecry5StrrefSpin != null) _battlecry5StrrefSpin.Value = ssf.Get(SSFSound.BATTLE_CRY_5) ?? 0;
            if (_battlecry6StrrefSpin != null) _battlecry6StrrefSpin.Value = ssf.Get(SSFSound.BATTLE_CRY_6) ?? 0;
            if (_select1StrrefSpin != null) _select1StrrefSpin.Value = ssf.Get(SSFSound.SELECT_1) ?? 0;
            if (_select2StrrefSpin != null) _select2StrrefSpin.Value = ssf.Get(SSFSound.SELECT_2) ?? 0;
            if (_select3StrrefSpin != null) _select3StrrefSpin.Value = ssf.Get(SSFSound.SELECT_3) ?? 0;
            if (_attack1StrrefSpin != null) _attack1StrrefSpin.Value = ssf.Get(SSFSound.ATTACK_GRUNT_1) ?? 0;
            if (_attack2StrrefSpin != null) _attack2StrrefSpin.Value = ssf.Get(SSFSound.ATTACK_GRUNT_2) ?? 0;
            if (_attack3StrrefSpin != null) _attack3StrrefSpin.Value = ssf.Get(SSFSound.ATTACK_GRUNT_3) ?? 0;
            if (_pain1StrrefSpin != null) _pain1StrrefSpin.Value = ssf.Get(SSFSound.PAIN_GRUNT_1) ?? 0;
            if (_pain2StrrefSpin != null) _pain2StrrefSpin.Value = ssf.Get(SSFSound.PAIN_GRUNT_2) ?? 0;
            if (_lowHpStrrefSpin != null) _lowHpStrrefSpin.Value = ssf.Get(SSFSound.LOW_HEALTH) ?? 0;
            if (_deadStrrefSpin != null) _deadStrrefSpin.Value = ssf.Get(SSFSound.DEAD) ?? 0;
            if (_criticalStrrefSpin != null) _criticalStrrefSpin.Value = ssf.Get(SSFSound.CRITICAL_HIT) ?? 0;
            if (_immuneStrrefSpin != null) _immuneStrrefSpin.Value = ssf.Get(SSFSound.TARGET_IMMUNE) ?? 0;
            if (_layMineStrrefSpin != null) _layMineStrrefSpin.Value = ssf.Get(SSFSound.LAY_MINE) ?? 0;
            if (_disarmMineStrrefSpin != null) _disarmMineStrrefSpin.Value = ssf.Get(SSFSound.DISARM_MINE) ?? 0;
            if (_beginStealthStrrefSpin != null) _beginStealthStrrefSpin.Value = ssf.Get(SSFSound.BEGIN_STEALTH) ?? 0;
            if (_beginSearchStrrefSpin != null) _beginSearchStrrefSpin.Value = ssf.Get(SSFSound.BEGIN_SEARCH) ?? 0;
            if (_beginUnlockStrrefSpin != null) _beginUnlockStrrefSpin.Value = ssf.Get(SSFSound.BEGIN_UNLOCK) ?? 0;
            if (_unlockFailedStrrefSpin != null) _unlockFailedStrrefSpin.Value = ssf.Get(SSFSound.UNLOCK_FAILED) ?? 0;
            if (_unlockSuccessStrrefSpin != null) _unlockSuccessStrrefSpin.Value = ssf.Get(SSFSound.UNLOCK_SUCCESS) ?? 0;
            if (_partySeparatedStrrefSpin != null) _partySeparatedStrrefSpin.Value = ssf.Get(SSFSound.SEPARATED_FROM_PARTY) ?? 0;
            if (_rejoinPartyStrrefSpin != null) _rejoinPartyStrrefSpin.Value = ssf.Get(SSFSound.REJOINED_PARTY) ?? 0;
            if (_poisonedStrrefSpin != null) _poisonedStrrefSpin.Value = ssf.Get(SSFSound.POISONED) ?? 0;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/ssf.py:159-210
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            SSF ssf = new SSF();

            if (_battlecry1StrrefSpin != null) ssf.SetData(SSFSound.BATTLE_CRY_1, (int)_battlecry1StrrefSpin.Value);
            if (_battlecry2StrrefSpin != null) ssf.SetData(SSFSound.BATTLE_CRY_2, (int)_battlecry2StrrefSpin.Value);
            if (_battlecry3StrrefSpin != null) ssf.SetData(SSFSound.BATTLE_CRY_3, (int)_battlecry3StrrefSpin.Value);
            if (_battlecry4StrrefSpin != null) ssf.SetData(SSFSound.BATTLE_CRY_4, (int)_battlecry4StrrefSpin.Value);
            if (_battlecry5StrrefSpin != null) ssf.SetData(SSFSound.BATTLE_CRY_5, (int)_battlecry5StrrefSpin.Value);
            if (_battlecry6StrrefSpin != null) ssf.SetData(SSFSound.BATTLE_CRY_6, (int)_battlecry6StrrefSpin.Value);
            if (_select1StrrefSpin != null) ssf.SetData(SSFSound.SELECT_1, (int)_select1StrrefSpin.Value);
            if (_select2StrrefSpin != null) ssf.SetData(SSFSound.SELECT_2, (int)_select2StrrefSpin.Value);
            if (_select3StrrefSpin != null) ssf.SetData(SSFSound.SELECT_3, (int)_select3StrrefSpin.Value);
            if (_attack1StrrefSpin != null) ssf.SetData(SSFSound.ATTACK_GRUNT_1, (int)_attack1StrrefSpin.Value);
            if (_attack2StrrefSpin != null) ssf.SetData(SSFSound.ATTACK_GRUNT_2, (int)_attack2StrrefSpin.Value);
            if (_attack3StrrefSpin != null) ssf.SetData(SSFSound.ATTACK_GRUNT_3, (int)_attack3StrrefSpin.Value);
            if (_pain1StrrefSpin != null) ssf.SetData(SSFSound.PAIN_GRUNT_1, (int)_pain1StrrefSpin.Value);
            if (_pain2StrrefSpin != null) ssf.SetData(SSFSound.PAIN_GRUNT_2, (int)_pain2StrrefSpin.Value);
            if (_lowHpStrrefSpin != null) ssf.SetData(SSFSound.LOW_HEALTH, (int)_lowHpStrrefSpin.Value);
            if (_deadStrrefSpin != null) ssf.SetData(SSFSound.DEAD, (int)_deadStrrefSpin.Value);
            if (_criticalStrrefSpin != null) ssf.SetData(SSFSound.CRITICAL_HIT, (int)_criticalStrrefSpin.Value);
            if (_immuneStrrefSpin != null) ssf.SetData(SSFSound.TARGET_IMMUNE, (int)_immuneStrrefSpin.Value);
            if (_layMineStrrefSpin != null) ssf.SetData(SSFSound.LAY_MINE, (int)_layMineStrrefSpin.Value);
            if (_disarmMineStrrefSpin != null) ssf.SetData(SSFSound.DISARM_MINE, (int)_disarmMineStrrefSpin.Value);
            if (_beginStealthStrrefSpin != null) ssf.SetData(SSFSound.BEGIN_STEALTH, (int)_beginStealthStrrefSpin.Value);
            if (_beginSearchStrrefSpin != null) ssf.SetData(SSFSound.BEGIN_SEARCH, (int)_beginSearchStrrefSpin.Value);
            if (_beginUnlockStrrefSpin != null) ssf.SetData(SSFSound.BEGIN_UNLOCK, (int)_beginUnlockStrrefSpin.Value);
            if (_unlockFailedStrrefSpin != null) ssf.SetData(SSFSound.UNLOCK_FAILED, (int)_unlockFailedStrrefSpin.Value);
            if (_unlockSuccessStrrefSpin != null) ssf.SetData(SSFSound.UNLOCK_SUCCESS, (int)_unlockSuccessStrrefSpin.Value);
            if (_partySeparatedStrrefSpin != null) ssf.SetData(SSFSound.SEPARATED_FROM_PARTY, (int)_partySeparatedStrrefSpin.Value);
            if (_rejoinPartyStrrefSpin != null) ssf.SetData(SSFSound.REJOINED_PARTY, (int)_rejoinPartyStrrefSpin.Value);
            if (_poisonedStrrefSpin != null) ssf.SetData(SSFSound.POISONED, (int)_poisonedStrrefSpin.Value);

            byte[] data = ssf.ToBytes();
            return Tuple.Create(data, new byte[0]);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/ssf.py:212-242
        // Original: def new(self):
        public override void New()
        {
            base.New();
            // Reset all spin boxes to 0
            if (_battlecry1StrrefSpin != null) _battlecry1StrrefSpin.Value = 0;
            if (_battlecry2StrrefSpin != null) _battlecry2StrrefSpin.Value = 0;
            if (_battlecry3StrrefSpin != null) _battlecry3StrrefSpin.Value = 0;
            if (_battlecry4StrrefSpin != null) _battlecry4StrrefSpin.Value = 0;
            if (_battlecry5StrrefSpin != null) _battlecry5StrrefSpin.Value = 0;
            if (_battlecry6StrrefSpin != null) _battlecry6StrrefSpin.Value = 0;
            if (_select1StrrefSpin != null) _select1StrrefSpin.Value = 0;
            if (_select2StrrefSpin != null) _select2StrrefSpin.Value = 0;
            if (_select3StrrefSpin != null) _select3StrrefSpin.Value = 0;
            if (_attack1StrrefSpin != null) _attack1StrrefSpin.Value = 0;
            if (_attack2StrrefSpin != null) _attack2StrrefSpin.Value = 0;
            if (_attack3StrrefSpin != null) _attack3StrrefSpin.Value = 0;
            if (_pain1StrrefSpin != null) _pain1StrrefSpin.Value = 0;
            if (_pain2StrrefSpin != null) _pain2StrrefSpin.Value = 0;
            if (_lowHpStrrefSpin != null) _lowHpStrrefSpin.Value = 0;
            if (_deadStrrefSpin != null) _deadStrrefSpin.Value = 0;
            if (_criticalStrrefSpin != null) _criticalStrrefSpin.Value = 0;
            if (_immuneStrrefSpin != null) _immuneStrrefSpin.Value = 0;
            if (_layMineStrrefSpin != null) _layMineStrrefSpin.Value = 0;
            if (_disarmMineStrrefSpin != null) _disarmMineStrrefSpin.Value = 0;
            if (_beginSearchStrrefSpin != null) _beginSearchStrrefSpin.Value = 0;
            if (_beginUnlockStrrefSpin != null) _beginUnlockStrrefSpin.Value = 0;
            if (_beginStealthStrrefSpin != null) _beginStealthStrrefSpin.Value = 0;
            if (_unlockSuccessStrrefSpin != null) _unlockSuccessStrrefSpin.Value = 0;
            if (_unlockFailedStrrefSpin != null) _unlockFailedStrrefSpin.Value = 0;
            if (_partySeparatedStrrefSpin != null) _partySeparatedStrrefSpin.Value = 0;
            if (_rejoinPartyStrrefSpin != null) _rejoinPartyStrrefSpin.Value = 0;
            if (_poisonedStrrefSpin != null) _poisonedStrrefSpin.Value = 0;
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
