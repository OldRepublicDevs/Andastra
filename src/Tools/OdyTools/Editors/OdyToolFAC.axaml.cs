using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BioWare.Common;
using BioWare.Resource.Formats.GFF.Generics;
using OdyTools.Data;

namespace OdyTools.Editors
{
    // Matching HolocronToolset implementation at vendor/src/toolset/gui/editors/fac.py
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/fac.py
    public partial class OdyToolFAC : Editor
    {
        private const int MinEditorWidth = 720;
        private const int MinEditorHeight = 520;
        private const int UndoMaxLevels = 30;

        private FAC _fac;
        private TextBlock _statusText;
        private ListBox _factionList;
        private ListBox _reputationList;
        private TextBox _factionNameEdit;
        private NumericUpDown _parentIdSpin;
        private CheckBox _globalEffectCheck;
        private NumericUpDown _reputationValueSpin;
        private Button _addFactionButton;
        private Button _removeFactionButton;
        private Button _addReputationButton;
        private Button _removeReputationButton;
        private bool _uiSyncInProgress;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;

        public FAC Fac => _fac;

        public OdyToolFAC() : this(null, null) { }

        public OdyToolFAC(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolFAC", "faction",
                new[] { ResourceType.FAC },
                new[] { ResourceType.FAC },
                installation)
        {
            _fac = new FAC();
            InitializeComponent();
            SetupUI();
            SetupSignals();
            SetupMenuHandlers();
            AddHelpAction();
            New();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetupUI()
        {
            _statusText = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
            _factionList = EditorHelpers.FindControlSafe<ListBox>(this, "factionList");
            _reputationList = EditorHelpers.FindControlSafe<ListBox>(this, "reputationList");
            _factionNameEdit = EditorHelpers.FindControlSafe<TextBox>(this, "factionNameEdit");
            _parentIdSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "parentIdSpin");
            _globalEffectCheck = EditorHelpers.FindControlSafe<CheckBox>(this, "globalEffectCheck");
            _reputationValueSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "reputationValueSpin");
            _addFactionButton = EditorHelpers.FindControlSafe<Button>(this, "addFactionButton");
            _removeFactionButton = EditorHelpers.FindControlSafe<Button>(this, "removeFactionButton");
            _addReputationButton = EditorHelpers.FindControlSafe<Button>(this, "addReputationButton");
            _removeReputationButton = EditorHelpers.FindControlSafe<Button>(this, "removeReputationButton");
        }

        private void SetupMenuHandlers()
        {
            EditorHelpers.BindMenuClicks(this, new (string menuItemName, Action handler)[]
            {
                ("actionUndo", Undo),
                ("actionRedo", Redo),
            });
        }

        private void LoadFromBytes(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                _fac = new FAC();
            }
            else
            {
                _fac = FACHelpers.ReadFac(data);
            }
            RefreshLists();
            ClearSelectionDetails();
            UpdateStatus();
        }

        private void Undo()
        {
            if (_undoStack.Count == 0)
            {
                return;
            }
            _undoRedoInProgress = true;
            try
            {
                byte[] data = _undoStack[_undoStack.Count - 1];
                _undoStack.RemoveAt(_undoStack.Count - 1);
                _redoStack.Add(Build().Item1);
                LoadFromBytes(data);
            }
            finally
            {
                _undoRedoInProgress = false;
            }
        }

        private void Redo()
        {
            if (_redoStack.Count == 0)
            {
                return;
            }
            _undoRedoInProgress = true;
            try
            {
                byte[] data = _redoStack[_redoStack.Count - 1];
                _redoStack.RemoveAt(_redoStack.Count - 1);
                _undoStack.Add(Build().Item1);
                LoadFromBytes(data);
            }
            finally
            {
                _undoRedoInProgress = false;
            }
        }

        private void SetupSignals()
        {
            if (_factionList != null)
            {
                _factionList.SelectionChanged += OnFactionSelectionChanged;
            }
            if (_reputationList != null)
            {
                _reputationList.SelectionChanged += OnReputationSelectionChanged;
            }
            if (_factionNameEdit != null)
            {
                _factionNameEdit.LostFocus += (s, e) => ApplyFactionEdits();
            }
            if (_parentIdSpin != null)
            {
                _parentIdSpin.ValueChanged += (s, e) => ApplyFactionEdits();
            }
            if (_globalEffectCheck != null)
            {
                _globalEffectCheck.Click += (s, e) => ApplyFactionEdits();
            }
            if (_reputationValueSpin != null)
            {
                _reputationValueSpin.ValueChanged += (s, e) => ApplyReputationEdits();
            }
            if (_addFactionButton != null)
            {
                _addFactionButton.Click += OnAddFaction;
            }
            if (_removeFactionButton != null)
            {
                _removeFactionButton.Click += OnRemoveFaction;
            }
            if (_addReputationButton != null)
            {
                _addReputationButton.Click += OnAddReputation;
            }
            if (_removeReputationButton != null)
            {
                _removeReputationButton.Click += OnRemoveReputation;
            }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _fac = FACHelpers.ReadFac(data);
            RefreshLists();
            ClearSelectionDetails();
            UpdateStatus();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            ApplyFactionEdits();
            ApplyReputationEdits();
            FAC built = CloneFac(_fac);
            byte[] data = FACHelpers.BytesFac(built, ResourceType.FAC);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            _fac = new FAC();
            RefreshLists();
            ClearSelectionDetails();
            UpdateStatus();
        }

        private void PushUndo()
        {
            if (_undoRedoInProgress || _fac == null)
            {
                return;
            }
            _undoStack.Add(FACHelpers.BytesFac(CloneFac(_fac), ResourceType.FAC));
            if (_undoStack.Count > UndoMaxLevels)
            {
                _undoStack.RemoveAt(0);
            }
            _redoStack.Clear();
        }

        private FAC CloneFac(FAC source)
        {
            var clone = new FAC();
            foreach (FACFaction faction in source.Factions)
            {
                clone.Factions.Add(new FACFaction
                {
                    Name = faction.Name,
                    ParentId = faction.ParentId,
                    IsGlobal = faction.IsGlobal
                });
            }
            foreach (FACReputation rep in source.Reputations)
            {
                clone.Reputations.Add(new FACReputation
                {
                    FactionId1 = rep.FactionId1,
                    FactionId2 = rep.FactionId2,
                    Reputation = rep.Reputation
                });
            }
            return clone;
        }

        private void RefreshLists()
        {
            if (_factionList != null)
            {
                _factionList.ItemsSource = null;
                var factionItems = new List<string>();
                for (int i = 0; i < _fac.Factions.Count; i++)
                {
                    factionItems.Add(FormatFactionItem(i, _fac.Factions[i]));
                }
                _factionList.ItemsSource = factionItems;
            }
            if (_reputationList != null)
            {
                _reputationList.ItemsSource = null;
                var repItems = new List<string>();
                foreach (FACReputation rep in _fac.Reputations)
                {
                    repItems.Add(FormatReputationItem(rep));
                }
                _reputationList.ItemsSource = repItems;
            }
        }

        private string FormatFactionItem(int index, FACFaction faction)
        {
            string text = string.Format("[{0}] {1}", index, faction.Name);
            if (faction.IsGlobal)
            {
                text += " (Global)";
            }
            return text;
        }

        private string FormatReputationItem(FACReputation rep)
        {
            string faction1 = GetFactionName(rep.FactionId1);
            string faction2 = GetFactionName(rep.FactionId2);
            string relText;
            if (rep.Reputation <= 10)
            {
                relText = "Hostile";
            }
            else if (rep.Reputation <= 89)
            {
                relText = "Neutral";
            }
            else
            {
                relText = "Friendly";
            }
            return string.Format("{0} -> {1}: {2} ({3})", faction2, faction1, rep.Reputation, relText);
        }

        private string GetFactionName(int factionId)
        {
            if (factionId >= 0 && factionId < _fac.Factions.Count)
            {
                return _fac.Factions[factionId].Name;
            }
            return "Faction" + factionId;
        }

        private void ClearSelectionDetails()
        {
            _uiSyncInProgress = true;
            if (_factionNameEdit != null)
            {
                _factionNameEdit.Text = string.Empty;
            }
            if (_parentIdSpin != null)
            {
                _parentIdSpin.Value = 0;
            }
            if (_globalEffectCheck != null)
            {
                _globalEffectCheck.IsChecked = false;
            }
            if (_reputationValueSpin != null)
            {
                _reputationValueSpin.Value = 100;
            }
            _uiSyncInProgress = false;
        }

        private void OnFactionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_factionList == null || _factionList.SelectedIndex < 0)
            {
                return;
            }
            int index = _factionList.SelectedIndex;
            if (index >= _fac.Factions.Count)
            {
                return;
            }
            FACFaction faction = _fac.Factions[index];
            _uiSyncInProgress = true;
            if (_factionNameEdit != null)
            {
                _factionNameEdit.Text = faction.Name;
            }
            if (_globalEffectCheck != null)
            {
                _globalEffectCheck.IsChecked = faction.IsGlobal;
            }
            if (_parentIdSpin != null)
            {
                int parentDisplay = faction.ParentId == unchecked((int)0xFFFFFFFF) ? 0 : faction.ParentId;
                _parentIdSpin.Value = parentDisplay;
            }
            _uiSyncInProgress = false;
        }

        private void OnReputationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_reputationList == null || _reputationList.SelectedIndex < 0)
            {
                return;
            }
            int index = _reputationList.SelectedIndex;
            if (index >= _fac.Reputations.Count)
            {
                return;
            }
            FACReputation rep = _fac.Reputations[index];
            _uiSyncInProgress = true;
            if (_reputationValueSpin != null)
            {
                _reputationValueSpin.Value = rep.Reputation;
            }
            _uiSyncInProgress = false;
        }

        private void ApplyFactionEdits()
        {
            if (_uiSyncInProgress || _factionList == null || _factionList.SelectedIndex < 0)
            {
                return;
            }
            int index = _factionList.SelectedIndex;
            if (index < 0 || index >= _fac.Factions.Count)
            {
                return;
            }
            PushUndo();
            FACFaction faction = _fac.Factions[index];
            if (_factionNameEdit != null)
            {
                faction.Name = _factionNameEdit.Text ?? string.Empty;
            }
            if (_globalEffectCheck != null)
            {
                faction.IsGlobal = _globalEffectCheck.IsChecked == true;
            }
            if (_parentIdSpin != null)
            {
                decimal parentVal = _parentIdSpin.Value ?? 0;
                faction.ParentId = parentVal == 0 ? unchecked((int)0xFFFFFFFF) : (int)parentVal;
            }
            RefreshLists();
            if (_factionList != null)
            {
                _factionList.SelectedIndex = index;
            }
        }

        private void ApplyReputationEdits()
        {
            if (_uiSyncInProgress || _reputationList == null || _reputationList.SelectedIndex < 0)
            {
                return;
            }
            int index = _reputationList.SelectedIndex;
            if (index < 0 || index >= _fac.Reputations.Count)
            {
                return;
            }
            PushUndo();
            FACReputation rep = _fac.Reputations[index];
            if (_reputationValueSpin != null)
            {
                rep.Reputation = (int)(_reputationValueSpin.Value ?? 100);
            }
            RefreshLists();
            if (_reputationList != null)
            {
                _reputationList.SelectedIndex = index;
            }
        }

        private void OnAddFaction(object sender, RoutedEventArgs e)
        {
            PushUndo();
            _fac.Factions.Add(new FACFaction { Name = "New Faction", IsGlobal = true });
            RefreshLists();
            if (_factionList != null)
            {
                _factionList.SelectedIndex = _fac.Factions.Count - 1;
            }
            UpdateStatus();
        }

        private void OnRemoveFaction(object sender, RoutedEventArgs e)
        {
            if (_factionList == null || _factionList.SelectedIndex < 0)
            {
                return;
            }
            PushUndo();
            int index = _factionList.SelectedIndex;
            _fac.Factions.RemoveAt(index);
            _fac.Reputations.RemoveAll(r => r.FactionId1 == index || r.FactionId2 == index);
            RefreshLists();
            ClearSelectionDetails();
            UpdateStatus();
        }

        private void OnAddReputation(object sender, RoutedEventArgs e)
        {
            PushUndo();
            int id1 = _fac.Factions.Count > 0 ? 0 : 0;
            int id2 = _fac.Factions.Count > 1 ? 1 : id1;
            _fac.Reputations.Add(new FACReputation
            {
                FactionId1 = id1,
                FactionId2 = id2,
                Reputation = 100
            });
            RefreshLists();
            if (_reputationList != null)
            {
                _reputationList.SelectedIndex = _fac.Reputations.Count - 1;
            }
            UpdateStatus();
        }

        private void OnRemoveReputation(object sender, RoutedEventArgs e)
        {
            if (_reputationList == null || _reputationList.SelectedIndex < 0)
            {
                return;
            }
            PushUndo();
            _fac.Reputations.RemoveAt(_reputationList.SelectedIndex);
            RefreshLists();
            ClearSelectionDetails();
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if (_statusText != null)
            {
                _statusText.Text = string.Format("Factions: {0}, Reputations: {1}", _fac.Factions.Count, _fac.Reputations.Count);
            }
        }
    }
}
