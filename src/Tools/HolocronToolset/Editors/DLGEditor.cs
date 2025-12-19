using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Andastra.Parsing.Common;
using Andastra.Parsing.Formats.GFF;
using Andastra.Parsing.Resource.Generics.DLG;
using Andastra.Parsing.Resource;
using HolocronToolset.Data;
using HolocronToolset.Editors.Actions;
using GFFAuto = Andastra.Parsing.Formats.GFF.GFFAuto;

namespace HolocronToolset.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:88
    // Original: class DLGEditor(Editor):
    public class DLGEditor : Editor
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:116
        // Original: self.core_dlg: DLG = DLG()
        private DLG _coreDlg;
        private DLGModel _model;
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:117
        // Original: self.undo_stack: QUndoStack = QUndoStack()
        private DLGActionHistory _actionHistory;

        // UI Controls - Animations
        // Matching PyKotor implementation at Tools/HolocronToolset/src/ui/editors/dlg.ui:966-992
        // Original: QListWidget animsList, QPushButton addAnimButton, removeAnimButton, editAnimButton
        private ListBox _animsList;
        private Button _addAnimButton;
        private Button _removeAnimButton;
        private Button _editAnimButton;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:101-177
        // Original: def __init__(self, parent: QWidget | None = None, installation: HTInstallation | None = None):
        public DLGEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Dialog Editor", "dialog",
                new[] { ResourceType.DLG },
                new[] { ResourceType.DLG },
                installation)
        {
            _coreDlg = new DLG();
            _model = new DLGModel();
            _actionHistory = new DLGActionHistory(this);
            InitializeComponent();
            SetupUI();
            New();
        }

        private void InitializeComponent()
        {
            if (!TryLoadXaml())
            {
                SetupUI();
            }
        }

        private void SetupUI()
        {
            var panel = new StackPanel();
            Content = panel;

            // Initialize animation UI controls
            // Matching PyKotor implementation at Tools/HolocronToolset/src/ui/editors/dlg.ui:966-992
            _animsList = new ListBox();
            _addAnimButton = new Button { Content = "Add Animation" };
            _removeAnimButton = new Button { Content = "Remove Animation" };
            _editAnimButton = new Button { Content = "Edit Animation" };

            // Add animation controls to UI (basic layout for now)
            var animPanel = new StackPanel();
            animPanel.Children.Add(_animsList);
            var buttonPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            buttonPanel.Children.Add(_addAnimButton);
            buttonPanel.Children.Add(_removeAnimButton);
            buttonPanel.Children.Add(_editAnimButton);
            animPanel.Children.Add(buttonPanel);
            panel.Children.Add(animPanel);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:1135-1171
        // Original: def load(self, filepath: os.PathLike | str, resref: str, restype: ResourceType, data: bytes | bytearray):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            _coreDlg = DLGHelper.ReadDlg(data);
            LoadDLG(_coreDlg);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:1193-1227
        // Original: def _load_dlg(self, dlg: DLG):
        private void LoadDLG(DLG dlg)
        {
            _coreDlg = dlg;
            _model.ResetModel();
            foreach (DLGLink start in dlg.Starters)
            {
                _model.AddStarter(start);
            }
            // Clear undo/redo history when loading a dialog
            _actionHistory.Clear();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:1229-1254
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            Game gameToUse = _installation?.Game ?? Game.K2;
            byte[] data = DLGHelper.BytesDlg(_coreDlg, gameToUse, ResourceType.DLG);
            return Tuple.Create(data, new byte[0]);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:1256-1260
        // Original: def new(self):
        public override void New()
        {
            base.New();
            _coreDlg = new DLG();
            _model.ResetModel();
            // Clear undo/redo history when creating new dialog
            _actionHistory.Clear();
        }

        public override void SaveAs()
        {
            Save();
        }

        // Properties for tests
        public DLG CoreDlg => _coreDlg;
        public DLGModel Model => _model;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/node_editor.py:1170-1184
        // Original: def undo(self): / def redo(self):
        // Undo/redo functionality for DLG editor
        // Based on QUndoStack pattern from PyKotor implementation

        /// <summary>
        /// Gets whether undo is available.
        /// </summary>
        public bool CanUndo => _actionHistory.CanUndo;

        /// <summary>
        /// Gets whether redo is available.
        /// </summary>
        public bool CanRedo => _actionHistory.CanRedo;

        /// <summary>
        /// Undoes the last action.
        /// </summary>
        public void Undo()
        {
            _actionHistory.Undo();
        }

        /// <summary>
        /// Redoes the last undone action.
        /// </summary>
        public void Redo()
        {
            _actionHistory.Redo();
        }

        /// <summary>
        /// Adds a starter link to the dialog and records it in the action history for undo/redo.
        /// </summary>
        /// <param name="link">The link to add.</param>
        public void AddStarter(DLGLink link)
        {
            if (link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }

            int index = _coreDlg.Starters.Count;
            var action = new AddStarterAction(link, index);
            _actionHistory.Apply(action);
        }

        /// <summary>
        /// Removes a starter link from the dialog and records it in the action history for undo/redo.
        /// </summary>
        /// <param name="link">The link to remove.</param>
        public void RemoveStarter(DLGLink link)
        {
            if (link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }

            int index = _coreDlg.Starters.IndexOf(link);
            if (index < 0)
            {
                return; // Link not found, nothing to remove
            }

            var action = new RemoveStarterAction(link, index);
            _actionHistory.Apply(action);
        }

        /// <summary>
        /// Moves the selected item down in the starter list and records it in the action history for undo/redo.
        /// </summary>
        public void MoveItemDown()
        {
            int selectedIndex = _model.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= _coreDlg.Starters.Count - 1)
            {
                return; // No selection or already at bottom
            }

            int newIndex = selectedIndex + 1;
            DLGLink link = _coreDlg.Starters[selectedIndex];
            var action = new MoveStarterAction(link, selectedIndex, newIndex);
            _actionHistory.Apply(action);

            // Update selected index to track the moved item
            _model.SelectedIndex = newIndex;
        }

        /// <summary>
        /// Moves the selected item up in the starter list and records it in the action history for undo/redo.
        /// </summary>
        public void MoveItemUp()
        {
            int selectedIndex = _model.SelectedIndex;
            if (selectedIndex <= 0 || selectedIndex >= _coreDlg.Starters.Count)
            {
                return; // No selection or already at top
            }

            int newIndex = selectedIndex - 1;
            DLGLink link = _coreDlg.Starters[selectedIndex];
            var action = new MoveStarterAction(link, selectedIndex, newIndex);
            _actionHistory.Apply(action);

            // Update selected index to track the moved item
            _model.SelectedIndex = newIndex;
        }

        // Matching PyKotor implementation: Expose UI controls for testing
        // Original: editor.ui.animsList, editor.ui.addAnimButton, etc.
        public ListBox AnimsList => _animsList;
        public Button AddAnimButton => _addAnimButton;
        public Button RemoveAnimButton => _removeAnimButton;
        public Button EditAnimButton => _editAnimButton;
    }

    // Simple model class for tests (matching Python DLGStandardItemModel)
    public class DLGModel
    {
        private List<DLGLink> _starters = new List<DLGLink>();

        public int RowCount => _starters.Count;

        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value >= -1 && value < _starters.Count)
                {
                    _selectedIndex = value;
                }
            }
        }

        public void ResetModel()
        {
            _starters.Clear();
            _selectedIndex = -1;
        }

        public void AddStarter(DLGLink link)
        {
            _starters.Add(link);
        }

        /// <summary>
        /// Inserts a starter link at the specified index.
        /// </summary>
        public void InsertStarter(int index, DLGLink link)
        {
            if (index < 0 || index > _starters.Count)
            {
                _starters.Add(link);
            }
            else
            {
                _starters.Insert(index, link);
            }
        }

        /// <summary>
        /// Gets the starter link at the specified index.
        /// </summary>
        public DLGLink GetStarterAt(int index)
        {
            if (index < 0 || index >= _starters.Count)
            {
                return null;
            }
            return _starters[index];
        }

        // Matching PyKotor implementation
        // Original: def remove_starter(self, link: DLGLink): ...
        /// <summary>
        /// Removes a starter link from the model.
        /// </summary>
        public void RemoveStarter(DLGLink link)
        {
            _starters.Remove(link);
        }

        // Matching PyKotor implementation
        // Original: def move_starter(self, old_index: int, new_index: int): ...
        /// <summary>
        /// Moves a starter link from one index to another.
        /// </summary>
        public void MoveStarter(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= _starters.Count || newIndex < 0 || newIndex >= _starters.Count)
            {
                return;
            }

            var link = _starters[oldIndex];
            _starters.RemoveAt(oldIndex);
            _starters.Insert(newIndex, link);
        }
    }
}
