using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Input;
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

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:152
        // Original: self.keys_down: set[int] = set()
        private HashSet<Key> _keysDown = new HashSet<Key>();

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:113
        // Original: self._copy: DLGLink | None = None
        private DLGLink _copy;

        // UI Controls - Animations
        // Matching PyKotor implementation at Tools/HolocronToolset/src/ui/editors/dlg.ui:966-992
        // Original: QListWidget animsList, QPushButton addAnimButton, removeAnimButton, editAnimButton
        private ListBox _animsList;
        private Button _addAnimButton;
        private Button _removeAnimButton;
        private Button _editAnimButton;

        // Search functionality
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:122-124, 451-465
        // Original: self.search_results: list[DLGStandardItem] = [], self.current_search_text: str = "", self.current_result_index: int = 0
        private List<DLGLink> _searchResults = new List<DLGLink>();
        private string _currentSearchText = "";
        private int _currentResultIndex = 0;

        // Search UI Controls
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:451-465
        // Original: self.find_bar: QWidget, self.find_input: QLineEdit, self.find_button: QPushButton, self.back_button: QPushButton, self.results_label: QLabel
        private Panel _findBar;
        private TextBox _findInput;
        private Button _findButton;
        private Button _backButton;
        private TextBlock _resultsLabel;

        // UI Controls - Link widgets
        // Matching PyKotor implementation at Tools/HolocronToolset/src/ui/editors/dlg.ui
        // Original: QComboBox condition1ResrefEdit, condition2ResrefEdit, QSpinBox logicSpin, QTreeView dialogTree
        private ComboBox _condition1ResrefEdit;
        private ComboBox _condition2ResrefEdit;
        private NumericUpDown _logicSpin;
        private TreeView _dialogTree;
        
        // Flag to track if node is loaded into UI (prevents updates during loading)
        private bool _nodeLoadedIntoUi = false;

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

            // Initialize dialog tree view
            // Matching PyKotor implementation at Tools/HolocronToolset/src/ui/editors/dlg.ui
            _dialogTree = new TreeView();
            _dialogTree.SelectionChanged += (s, e) => OnSelectionChanged();
            panel.Children.Add(_dialogTree);

            // Initialize link condition widgets
            // Matching PyKotor implementation at Tools/HolocronToolset/src/ui/editors/dlg.ui
            _condition1ResrefEdit = new ComboBox { IsEditable = true };
            _condition1ResrefEdit.LostFocus += (s, e) => OnNodeUpdate();
            _condition2ResrefEdit = new ComboBox { IsEditable = true };
            _condition2ResrefEdit.LostFocus += (s, e) => OnNodeUpdate();
            _logicSpin = new NumericUpDown { Minimum = 0, Maximum = 1, Value = 0 };
            _logicSpin.ValueChanged += (s, e) => OnNodeUpdate();

            var linkPanel = new StackPanel();
            linkPanel.Children.Add(new TextBlock { Text = "Condition 1 ResRef:" });
            linkPanel.Children.Add(_condition1ResrefEdit);
            linkPanel.Children.Add(new TextBlock { Text = "Condition 2 ResRef:" });
            linkPanel.Children.Add(_condition2ResrefEdit);
            linkPanel.Children.Add(new TextBlock { Text = "Logic:" });
            linkPanel.Children.Add(_logicSpin);
            panel.Children.Add(linkPanel);

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
        
        // Expose link widgets for testing
        // Matching PyKotor implementation: editor.ui.condition1ResrefEdit, etc.
        public ComboBox Condition1ResrefEdit => _condition1ResrefEdit;
        public ComboBox Condition2ResrefEdit => _condition2ResrefEdit;
        public NumericUpDown LogicSpin => _logicSpin;
        public TreeView DialogTree => _dialogTree;

        /// <summary>
        /// Handles selection changes in the dialog tree.
        /// Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:2364-2454
        /// Original: def on_selection_changed(self, selection: QItemSelection):
        /// </summary>
        private void OnSelectionChanged()
        {
            _nodeLoadedIntoUi = false;
            
            if (_dialogTree?.SelectedItem == null)
            {
                // Clear UI when nothing is selected
                if (_condition1ResrefEdit != null)
                {
                    _condition1ResrefEdit.Text = string.Empty;
                }
                if (_condition2ResrefEdit != null)
                {
                    _condition2ResrefEdit.Text = string.Empty;
                }
                if (_logicSpin != null)
                {
                    _logicSpin.Value = 0;
                }
                _nodeLoadedIntoUi = true;
                return;
            }

            // Get selected item from tree
            var selectedItem = _dialogTree.SelectedItem;
            if (selectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
            {
                LoadLinkIntoUI(dlgItem);
            }
            else if (selectedItem is DLGStandardItem dlgItemDirect)
            {
                LoadLinkIntoUI(dlgItemDirect);
            }
            
            _nodeLoadedIntoUi = true;
        }

        /// <summary>
        /// Loads link properties into UI controls.
        /// Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:2364-2454
        /// </summary>
        private void LoadLinkIntoUI(DLGStandardItem item)
        {
            if (item?.Link == null)
            {
                return;
            }

            var link = item.Link;
            
            // Load condition1
            if (_condition1ResrefEdit != null)
            {
                _condition1ResrefEdit.Text = link.Active1?.ToString() ?? string.Empty;
            }
            
            // Load condition2
            if (_condition2ResrefEdit != null)
            {
                _condition2ResrefEdit.Text = link.Active2?.ToString() ?? string.Empty;
            }
            
            // Load logic (0 = AND/false, 1 = OR/true)
            if (_logicSpin != null)
            {
                _logicSpin.Value = link.Logic ? 1 : 0;
            }
        }

        /// <summary>
        /// Updates node properties based on UI selections.
        /// Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:2456-2491
        /// Original: def on_node_update(self, *args, **kwargs):
        /// </summary>
        public void OnNodeUpdate()
        {
            if (!_nodeLoadedIntoUi)
            {
                return;
            }

            if (_dialogTree?.SelectedItem == null)
            {
                return;
            }

            // Get selected item from tree
            DLGStandardItem item = null;
            var selectedItem = _dialogTree.SelectedItem;
            if (selectedItem is TreeViewItem treeItem && treeItem.Tag is DLGStandardItem dlgItem)
            {
                item = dlgItem;
            }
            else if (selectedItem is DLGStandardItem dlgItemDirect)
            {
                item = dlgItemDirect;
            }

            if (item?.Link == null)
            {
                return;
            }

            var link = item.Link;

            // Update condition1
            if (_condition1ResrefEdit != null)
            {
                string text = _condition1ResrefEdit.Text ?? string.Empty;
                link.Active1 = string.IsNullOrEmpty(text) ? ResRef.FromBlank() : new ResRef(text);
            }

            // Update condition2
            if (_condition2ResrefEdit != null)
            {
                string text = _condition2ResrefEdit.Text ?? string.Empty;
                link.Active2 = string.IsNullOrEmpty(text) ? ResRef.FromBlank() : new ResRef(text);
            }

            // Update logic (0 = AND/false, 1 = OR/true)
            if (_logicSpin != null)
            {
                link.Logic = _logicSpin.Value.HasValue && _logicSpin.Value.Value != 0;
            }
        }
    }

    /// <summary>
    /// Represents a standard item in the DLG tree model.
    /// Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/model.py:52-100
    /// Original: class DLGStandardItem(QStandardItem):
    /// </summary>
    public class DLGStandardItem
    {
        private readonly WeakReference<DLGLink> _linkRef;
        private readonly List<DLGStandardItem> _children = new List<DLGStandardItem>();
        private DLGStandardItem _parent;

        /// <summary>
        /// Gets the link associated with this item, or null if the reference is no longer valid.
        /// Matching PyKotor implementation: property link(self) -> DLGLink | None
        /// </summary>
        public DLGLink Link
        {
            get
            {
                if (_linkRef != null && _linkRef.TryGetTarget(out DLGLink link))
                {
                    return link;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the number of child items.
        /// </summary>
        public int RowCount => _children.Count;

        /// <summary>
        /// Gets the parent item, or null if this is a root item.
        /// </summary>
        public DLGStandardItem Parent => _parent;

        /// <summary>
        /// Gets all child items.
        /// </summary>
        public IReadOnlyList<DLGStandardItem> Children => _children;

        /// <summary>
        /// Initializes a new instance of DLGStandardItem with the specified link.
        /// </summary>
        public DLGStandardItem(DLGLink link)
        {
            if (link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }
            _linkRef = new WeakReference<DLGLink>(link);
        }

        /// <summary>
        /// Adds a child item to this item.
        /// </summary>
        public void AddChild(DLGStandardItem child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }
            if (child._parent != null)
            {
                child._parent._children.Remove(child);
            }
            child._parent = this;
            _children.Add(child);
        }

        /// <summary>
        /// Gets the index of this item in its parent's children list.
        /// </summary>
        public int GetIndex()
        {
            if (_parent == null)
            {
                return -1;
            }
            return _parent._children.IndexOf(this);
        }
    }

    // Simple model class for tests (matching Python DLGStandardItemModel)
    public class DLGModel
    {
        private List<DLGStandardItem> _rootItems = new List<DLGStandardItem>();
        private DLGEditor _editor;

        public DLGModel()
        {
        }

        public DLGModel(DLGEditor editor)
        {
            _editor = editor;
        }

        public int RowCount => _rootItems.Count;

        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value >= -1 && value < _rootItems.Count)
                {
                    _selectedIndex = value;
                }
            }
        }

        public void ResetModel()
        {
            _rootItems.Clear();
            _selectedIndex = -1;
        }

        public void AddStarter(DLGLink link)
        {
            if (link == null)
            {
                return;
            }
            var item = new DLGStandardItem(link);
            _rootItems.Add(item);
            
            // Also add to CoreDlg.Starters if editor is available
            if (_editor != null && _editor.CoreDlg != null)
            {
                if (!_editor.CoreDlg.Starters.Contains(link))
                {
                    _editor.CoreDlg.Starters.Add(link);
                }
            }
        }

        /// <summary>
        /// Adds a root node to the dialog graph.
        /// Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/model.py:846-856
        /// Original: def add_root_node(self):
        /// </summary>
        public DLGStandardItem AddRootNode()
        {
            var newEntry = new DLGEntry();
            newEntry.PlotIndex = -1;
            var newLink = new DLGLink(newEntry);
            newLink.Node.ListIndex = GetNewNodeListIndex(newLink.Node);
            
            var newItem = new DLGStandardItem(newLink);
            _rootItems.Add(newItem);
            
            // Add to CoreDlg.Starters
            if (_editor != null && _editor.CoreDlg != null)
            {
                _editor.CoreDlg.Starters.Add(newLink);
            }
            
            UpdateItemDisplayText(newItem);
            return newItem;
        }

        /// <summary>
        /// Adds a child node to the specified parent item.
        /// Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/model.py:858-877
        /// Original: def add_child_to_item(self, parent_item: DLGStandardItem, link: DLGLink | None = None) -> DLGStandardItem:
        /// </summary>
        public DLGStandardItem AddChildToItem(DLGStandardItem parentItem, DLGLink link = null)
        {
            if (parentItem == null)
            {
                throw new ArgumentNullException(nameof(parentItem));
            }
            
            if (parentItem.Link == null)
            {
                throw new InvalidOperationException("Parent item must have a valid link");
            }

            if (link == null)
            {
                // Create new node - if parent is Reply, create Entry; if parent is Entry, create Reply
                DLGNode newNode;
                if (parentItem.Link.Node is DLGReply)
                {
                    newNode = new DLGEntry();
                }
                else
                {
                    newNode = new DLGReply();
                }
                newNode.PlotIndex = -1;
                newNode.ListIndex = GetNewNodeListIndex(newNode);
                link = new DLGLink(newNode);
            }

            // Link the nodes
            if (parentItem.Link.Node != null)
            {
                link.ListIndex = parentItem.Link.Node.Links.Count;
                parentItem.Link.Node.Links.Add(link);
            }

            var newItem = new DLGStandardItem(link);
            parentItem.AddChild(newItem);
            
            UpdateItemDisplayText(newItem);
            UpdateItemDisplayText(parentItem);
            
            return newItem;
        }

        /// <summary>
        /// Gets the item at the specified row and column.
        /// Matching PyKotor implementation: def item(self, row: int, column: int = 0) -> DLGStandardItem | None:
        /// </summary>
        public DLGStandardItem Item(int row, int column = 0)
        {
            if (row < 0 || row >= _rootItems.Count || column != 0)
            {
                return null;
            }
            return _rootItems[row];
        }

        /// <summary>
        /// Gets a new list index for a node.
        /// </summary>
        private int GetNewNodeListIndex(DLGNode node)
        {
            if (_editor?.CoreDlg == null)
            {
                return 0;
            }

            if (node is DLGEntry)
            {
                int maxIndex = -1;
                foreach (var entry in _editor.CoreDlg.AllEntries())
                {
                    if (entry.ListIndex > maxIndex)
                    {
                        maxIndex = entry.ListIndex;
                    }
                }
                return maxIndex + 1;
            }
            else if (node is DLGReply)
            {
                int maxIndex = -1;
                foreach (var reply in _editor.CoreDlg.AllReplies())
                {
                    if (reply.ListIndex > maxIndex)
                    {
                        maxIndex = reply.ListIndex;
                    }
                }
                return maxIndex + 1;
            }
            return 0;
        }

        /// <summary>
        /// Updates the display text for an item.
        /// </summary>
        private void UpdateItemDisplayText(DLGStandardItem item)
        {
            // This would update the display text in the tree view
            // For now, it's a placeholder
        }

        /// <summary>
        /// Inserts a starter link at the specified index.
        /// </summary>
        public void InsertStarter(int index, DLGLink link)
        {
            if (link == null)
            {
                return;
            }
            var item = new DLGStandardItem(link);
            if (index < 0 || index > _rootItems.Count)
            {
                _rootItems.Add(item);
            }
            else
            {
                _rootItems.Insert(index, item);
            }
        }

        /// <summary>
        /// Gets the starter link at the specified index.
        /// </summary>
        public DLGLink GetStarterAt(int index)
        {
            if (index < 0 || index >= _rootItems.Count)
            {
                return null;
            }
            return _rootItems[index].Link;
        }

        // Matching PyKotor implementation
        // Original: def remove_starter(self, link: DLGLink): ...
        /// <summary>
        /// Removes a starter link from the model.
        /// </summary>
        public void RemoveStarter(DLGLink link)
        {
            for (int i = _rootItems.Count - 1; i >= 0; i--)
            {
                if (_rootItems[i].Link == link)
                {
                    _rootItems.RemoveAt(i);
                    break;
                }
            }
        }

        // Matching PyKotor implementation
        // Original: def move_starter(self, old_index: int, new_index: int): ...
        /// <summary>
        /// Moves a starter link from one index to another.
        /// </summary>
        public void MoveStarter(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= _rootItems.Count || newIndex < 0 || newIndex >= _rootItems.Count)
            {
                return;
            }

            var item = _rootItems[oldIndex];
            _rootItems.RemoveAt(oldIndex);
            _rootItems.Insert(newIndex, item);
        }
    }
}
