using BioWare.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using OdyTools.Data;
using OdyTools.Widgets;
using GFFAuto = BioWare.Resource.Formats.GFF.GFFAuto;
using JRLHelper = BioWare.Resource.Formats.GFF.Generics.JRLHelpers;

namespace OdyTools.Editors
{
    public partial class OdyToolJRL : Editor
    {
        private const int MinEditorWidth = 480;
        private const int MinEditorHeight = 400;
        private const int UndoMaxLevels = 30;

        private JRL _jrl;
        private List<JournalTreeItem> _model;
        private GFF _originalGff;

        private Avalonia.Controls.TextBlock _statusText;
        private TextBlock _questCountLabel;
        private TreeView _journalTree;
        private TextBox _filterEdit;
        private Button _filterClearButton;
        private Button _addQuestButton;
        private Button _addEntryButton;
        private Button _removeButton;
        private StackPanel _detailNoSelection;
        private StackPanel _detailQuest;
        private StackPanel _detailEntry;
        private LocalizedStringEdit _questNameEdit;
        private NumericUpDown _questPlanetId;
        private NumericUpDown _questPlotIndex;
        private ComboBox _questPriority;
        private TextBox _questTag;
        private TextBox _questComment;
        private TextBlock _questEntryCount;
        private LocalizedStringEdit _entryTextEdit;
        private NumericUpDown _entryId;
        private CheckBox _entryEnd;
        private NumericUpDown _entryXpPct;
        private JournalTreeItem _selectedItem;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;
        private bool _loadingSelection;
        private string _findText = "";
        private bool _findMatchCase;
        private JournalTreeItem _lastFoundItem;
        private JRLEditorSettings _settings;

        internal bool HasStructuredEditorSurface =>
            _journalTree != null &&
            _addQuestButton != null &&
            _addEntryButton != null &&
            _removeButton != null &&
            _questNameEdit != null &&
            _questPlanetId != null &&
            _questPlotIndex != null &&
            _questPriority != null &&
            _questTag != null &&
            _questComment != null &&
            _entryTextEdit != null &&
            _entryId != null &&
            _entryEnd != null &&
            _entryXpPct != null;

        internal NumericUpDown QuestPlotIndexControlForTest => _questPlotIndex;
        internal NumericUpDown QuestPlanetIdControlForTest => _questPlanetId;
        internal ComboBox QuestPriorityControlForTest => _questPriority;
        internal TextBox QuestTagControlForTest => _questTag;
        internal TextBox QuestCommentControlForTest => _questComment;
        internal LocalizedStringEdit QuestNameEditForTest => _questNameEdit;
        internal LocalizedStringEdit EntryTextEditForTest => _entryTextEdit;
        internal NumericUpDown EntryIdControlForTest => _entryId;
        internal CheckBox EntryEndControlForTest => _entryEnd;
        internal NumericUpDown EntryXpPctControlForTest => _entryXpPct;
        internal Button AddQuestButtonForTest => _addQuestButton;
        internal Button AddEntryButtonForTest => _addEntryButton;
        internal Button RemoveButtonForTest => _removeButton;
        internal ContextMenu JournalTreeContextMenuForTest => _journalTree?.ContextMenu;
        internal JRLEditorSettings SettingsForTest => _settings;
        internal int SelectedQuestEntryCountForTest => (_selectedItem?.Data as JRLQuest)?.Entries.Count ?? 0;
        internal int EntryCountForTest(int questIndex) =>
            _model != null &&
            questIndex >= 0 &&
            questIndex < _model.Count &&
            _model[questIndex].Data is JRLQuest quest
                ? quest.Entries.Count
                : 0;
        internal void RunAddQuestToolbarActionForTest() => AddQuest();
        internal void RunAddEntryToolbarActionForTest() => AddEntry();
        internal void RunRemoveToolbarActionForTest() => RemoveSelected();
        internal void RunDuplicateSelectedForTest() => DuplicateSelected();
        internal void RunMoveSelectedForTest(int direction) => MoveSelected(direction);
        internal void RunSortSelectedQuestEntriesForTest(bool ascending) => SortSelectedQuestEntries(ascending);
        internal string[] FindMatchTextsForTest(string text)
        {
            if (text == null)
            {
                text = "";
            }
            string normalized = _findMatchCase ? text : text.ToLowerInvariant();
            bool Match(string value) => value != null && (_findMatchCase ? value : value.ToLowerInvariant()).Contains(normalized);
            return FindMatches(Match).Select(item => item.Text).ToArray();
        }
        internal string[] VisibleTreeTextsForTest(string filterText)
        {
            if (_filterEdit != null)
            {
                _filterEdit.Text = filterText ?? "";
            }
            ApplyJournalFilter();
            var visible = new List<string>();
            if (_journalTree?.ItemsSource == null)
            {
                return visible.ToArray();
            }

            foreach (var item in _journalTree.ItemsSource)
            {
                var treeItem = item as JournalTreeItem;
                if (treeItem == null)
                {
                    continue;
                }
                visible.Add(treeItem.Text);
                visible.AddRange(treeItem.Children.Select(child => child.Text));
            }

            return visible.ToArray();
        }

        public OdyToolJRL() : this(null, null) { }
        public OdyToolJRL(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolJRL", "journal",
                new[] { ResourceType.JRL },
                new[] { ResourceType.JRL },
                installation)
        {
            _jrl = new JRL();
            _model = new List<JournalTreeItem>();
            _settings = new JRLEditorSettings();
            InitializeComponent();
            SetupUI();
            SetupSignals();
            SetupMenuHandlers();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            Width = 480;
            Height = 400;
            New();
        }

        private void InitializeComponent()
        {
            try
            {
                AvaloniaXamlLoader.Load(this);
            }
            catch
            {
                SetupProgrammaticUI();
                return;
            }
            SetupUI();
        }

        private Menu BuildMenu()
        {
            var menu = new Menu();
            var fileMenu = new MenuItem { Header = "_File" };
            fileMenu.Items.Add(new MenuItem { Header = "_New", Name = "actionNew" });
            fileMenu.Items.Add(new MenuItem { Header = "_Open", Name = "actionOpen" });
            fileMenu.Items.Add(new MenuItem { Header = "_Save", Name = "actionSave" });
            fileMenu.Items.Add(new MenuItem { Header = "Save _As", Name = "actionSaveAs" });
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(new MenuItem { Header = "_Revert", Name = "actionRevert" });
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(new MenuItem { Header = "E_xit", Name = "actionExit" });
            menu.Items.Add(fileMenu);
            var editMenu = new MenuItem { Header = "_Edit" };
            editMenu.Items.Add(new MenuItem { Header = "_Undo", Name = "actionUndo" });
            editMenu.Items.Add(new MenuItem { Header = "_Redo", Name = "actionRedo" });
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(new MenuItem { Header = "_Duplicate", Name = "actionDuplicate" });
            editMenu.Items.Add(new MenuItem { Header = "Move _Up", Name = "actionMoveUp" });
            editMenu.Items.Add(new MenuItem { Header = "Move _Down", Name = "actionMoveDown" });
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(new MenuItem { Header = "Sort Entries by ID (_Ascending)", Name = "actionSortEntriesAscending" });
            editMenu.Items.Add(new MenuItem { Header = "Sort Entries by ID (D_escending)", Name = "actionSortEntriesDescending" });
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(new MenuItem { Header = "Find...", Name = "actionFind" });
            editMenu.Items.Add(new MenuItem { Header = "Find _Next", Name = "actionFindNext" });
            menu.Items.Add(editMenu);
            return menu;
        }

        private void SetupProgrammaticUI()
        {
            var dock = new DockPanel();
            var menu = BuildMenu();
            dock.Children.Add(menu);
            DockPanel.SetDock(menu, Dock.Top);

            var toolbar = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8,
                Margin = new Avalonia.Thickness(8)
            };
            toolbar.Children.Add(new Button { Name = "addQuestButton", Content = "+ Quest" });
            toolbar.Children.Add(new Button { Name = "addEntryButton", Content = "+ Entry", IsEnabled = false });
            toolbar.Children.Add(new Button { Name = "removeButton", Content = "Remove", IsEnabled = false });
            toolbar.Children.Add(new Button { Name = "findButton", Content = "Find..." });
            toolbar.Children.Add(new TextBlock { Name = "questCountLabel", VerticalAlignment = VerticalAlignment.Center });
            dock.Children.Add(toolbar);
            DockPanel.SetDock(toolbar, Dock.Top);

            var mainGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,380"),
                Margin = new Avalonia.Thickness(8, 0, 8, 8)
            };

            var leftPanel = new DockPanel { Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var filterGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,Auto"),
                Margin = new Avalonia.Thickness(0, 0, 0, 8)
            };
            filterGrid.Children.Add(new TextBox { Name = "filterEdit", Watermark = "Filter quests and entries" });
            var clearButton = new Button { Name = "filterClearButton", Content = "Clear", Margin = new Avalonia.Thickness(8, 0, 0, 0) };
            Grid.SetColumn(clearButton, 1);
            filterGrid.Children.Add(clearButton);
            leftPanel.Children.Add(filterGrid);
            DockPanel.SetDock(filterGrid, Dock.Top);
            leftPanel.Children.Add(new TreeView { Name = "journalTree" });
            mainGrid.Children.Add(leftPanel);

            var detailPanel = new StackPanel { Name = "detailPanel", Spacing = 8, Margin = new Avalonia.Thickness(8, 0, 0, 0) };
            detailPanel.Children.Add(new StackPanel { Name = "detailNoSelection" });

            var questPanel = new StackPanel { Name = "detailQuest", IsVisible = false, Spacing = 6 };
            questPanel.Children.Add(new LocalizedStringEdit { Name = "questNameEdit" });
            questPanel.Children.Add(new NumericUpDown { Name = "questPlanetId", Minimum = 0, Maximum = int.MaxValue });
            questPanel.Children.Add(new NumericUpDown { Name = "questPlotIndex", Minimum = 0, Maximum = int.MaxValue });
            questPanel.Children.Add(new ComboBox { Name = "questPriority" });
            questPanel.Children.Add(new TextBox { Name = "questTag" });
            questPanel.Children.Add(new TextBox { Name = "questComment" });
            questPanel.Children.Add(new TextBlock { Name = "questEntryCount" });
            detailPanel.Children.Add(questPanel);

            var entryPanel = new StackPanel { Name = "detailEntry", IsVisible = false, Spacing = 6 };
            entryPanel.Children.Add(new LocalizedStringEdit { Name = "entryTextEdit" });
            entryPanel.Children.Add(new NumericUpDown { Name = "entryId", Minimum = 0, Maximum = int.MaxValue });
            entryPanel.Children.Add(new CheckBox { Name = "entryEnd" });
            entryPanel.Children.Add(new NumericUpDown { Name = "entryXpPct", Minimum = 0, Maximum = 100, Increment = 0.1m });
            detailPanel.Children.Add(entryPanel);

            Grid.SetColumn(detailPanel, 1);
            mainGrid.Children.Add(detailPanel);
            dock.Children.Add(mainGrid);

            _statusText = new TextBlock { Name = "statusText", Text = "Journal", Margin = new Avalonia.Thickness(4, 2) };
            dock.Children.Add(_statusText);
            DockPanel.SetDock(_statusText, Dock.Bottom);
            Content = dock;
        }

        private void SetupUI()
        {
            void BindCommit(NumericUpDown spin, Action commit)
            {
                if (spin != null)
                {
                    spin.ValueChanged += (s, e) => commit();
                    spin.LostFocus += (s, e) => commit();
                }
            }

            _statusText = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
            _journalTree = EditorHelpers.FindControlSafe<TreeView>(this, "journalTree");
            _filterEdit = EditorHelpers.FindControlSafe<TextBox>(this, "filterEdit");
            _filterClearButton = EditorHelpers.FindControlSafe<Button>(this, "filterClearButton");
            _addQuestButton = EditorHelpers.FindControlSafe<Button>(this, "addQuestButton");
            _addEntryButton = EditorHelpers.FindControlSafe<Button>(this, "addEntryButton");
            _removeButton = EditorHelpers.FindControlSafe<Button>(this, "removeButton");
            _questCountLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "questCountLabel");
            _detailNoSelection = EditorHelpers.FindControlSafe<StackPanel>(this, "detailNoSelection");
            _detailQuest = EditorHelpers.FindControlSafe<StackPanel>(this, "detailQuest");
            _detailEntry = EditorHelpers.FindControlSafe<StackPanel>(this, "detailEntry");
            _questNameEdit = EditorHelpers.FindControlSafe<LocalizedStringEdit>(this, "questNameEdit");
            _questPlanetId = EditorHelpers.FindControlSafe<NumericUpDown>(this, "questPlanetId");
            _questPlotIndex = EditorHelpers.FindControlSafe<NumericUpDown>(this, "questPlotIndex");
            _questPriority = EditorHelpers.FindControlSafe<ComboBox>(this, "questPriority");
            _questTag = EditorHelpers.FindControlSafe<TextBox>(this, "questTag");
            _questComment = EditorHelpers.FindControlSafe<TextBox>(this, "questComment");
            _questEntryCount = EditorHelpers.FindControlSafe<TextBlock>(this, "questEntryCount");
            _entryTextEdit = EditorHelpers.FindControlSafe<LocalizedStringEdit>(this, "entryTextEdit");
            _entryId = EditorHelpers.FindControlSafe<NumericUpDown>(this, "entryId");
            _entryEnd = EditorHelpers.FindControlSafe<CheckBox>(this, "entryEnd");
            _entryXpPct = EditorHelpers.FindControlSafe<NumericUpDown>(this, "entryXpPct");

            if (_questPriority != null)
            {
                _questPriority.ItemsSource = Enum.GetValues(typeof(JRLQuestPriority)).Cast<object>().ToList();
                _questPriority.SelectedIndex = (int)JRLQuestPriority.Lowest;
            }
            if (_questNameEdit != null) _questNameEdit.SetInstallation(_installation);
            if (_entryTextEdit != null) _entryTextEdit.SetInstallation(_installation);

            if (_journalTree != null)
            {
                _journalTree.ItemsSource = _model;
                _journalTree.SelectionChanged += OnTreeSelectionChanged;
                SetupJournalTreeContextMenu();
            }
            if (_filterEdit != null)
            {
                _filterEdit.TextChanged += (s, e) => ApplyJournalFilter();
            }
            EditorHelpers.BindClick(_filterClearButton, () =>
            {
                if (_filterEdit != null)
                {
                    _filterEdit.Text = "";
                }
                ApplyJournalFilter();
            });
            EditorHelpers.BindClick(_addQuestButton, AddQuest);
            EditorHelpers.BindClick(_addEntryButton, AddEntry);
            EditorHelpers.BindClick(_removeButton, RemoveSelected);
            var findBtn = EditorHelpers.FindControlSafe<Button>(this, "findButton");
            EditorHelpers.BindClick(findBtn, ShowFindDialog);

            void CommitAndPush()
            {
                if (_loadingSelection)
                {
                    return;
                }

                SaveCurrentSelectionToModel();
                PushState();
                UpdateStatusBar();
            }
            BindCommit(_questPlanetId, CommitAndPush);
            BindCommit(_questPlotIndex, CommitAndPush);
            if (_questPriority != null) _questPriority.SelectionChanged += (s, e) => CommitAndPush();
            EditorHelpers.BindLostFocus(_questTag, CommitAndPush);
            EditorHelpers.BindLostFocus(_questComment, CommitAndPush);
            BindCommit(_entryId, CommitAndPush);
            if (_entryEnd != null) _entryEnd.IsCheckedChanged += (s, e) => CommitAndPush();
            BindCommit(_entryXpPct, CommitAndPush);
            // LocalizedStringEdit commits when dialog closes; we also flush on selection change and Build()

            RefreshDetailVisibility();
            UpdateQuestCountLabel();
        }

        private void SetupJournalTreeContextMenu()
        {
            if (_journalTree == null)
            {
                return;
            }

            var menu = new ContextMenu();
            menu.Items.Add(CreateContextMenuItem("ctxAddQuest", "Add Quest", AddQuest));
            menu.Items.Add(CreateContextMenuItem("ctxAddEntry", "Add Entry", AddEntry));
            menu.Items.Add(CreateContextMenuItem("ctxDuplicate", "Duplicate", DuplicateSelected));
            menu.Items.Add(new Separator());
            menu.Items.Add(CreateContextMenuItem("ctxMoveUp", "Move Up", () => MoveSelected(-1)));
            menu.Items.Add(CreateContextMenuItem("ctxMoveDown", "Move Down", () => MoveSelected(1)));
            menu.Items.Add(new Separator());
            menu.Items.Add(CreateContextMenuItem("ctxSortEntriesAscending", "Sort Entries by ID Ascending", () => SortSelectedQuestEntries(true)));
            menu.Items.Add(CreateContextMenuItem("ctxSortEntriesDescending", "Sort Entries by ID Descending", () => SortSelectedQuestEntries(false)));
            menu.Items.Add(new Separator());
            menu.Items.Add(CreateContextMenuItem("ctxRemove", "Remove", RemoveSelected));
            menu.Opened += (s, e) => UpdateJournalTreeContextMenuState(menu);
            _journalTree.ContextMenu = menu;
        }

        private static MenuItem CreateContextMenuItem(string name, string header, Action handler)
        {
            var item = new MenuItem { Name = name, Header = header };
            item.Click += (s, e) => handler();
            return item;
        }

        private void UpdateJournalTreeContextMenuState(ContextMenu menu)
        {
            bool hasSelection = _selectedItem != null;
            bool isQuest = _selectedItem?.Data is JRLQuest;
            bool isEntry = _selectedItem?.Data is JRLQuestEntry;
            bool canMoveUp = CanMoveSelected(-1);
            bool canMoveDown = CanMoveSelected(1);

            foreach (var menuItem in menu.Items.OfType<MenuItem>())
            {
                switch (menuItem.Name)
                {
                    case "ctxAddEntry":
                    case "ctxSortEntriesAscending":
                    case "ctxSortEntriesDescending":
                        menuItem.IsEnabled = isQuest || isEntry;
                        break;
                    case "ctxDuplicate":
                    case "ctxRemove":
                        menuItem.IsEnabled = hasSelection;
                        break;
                    case "ctxMoveUp":
                        menuItem.IsEnabled = canMoveUp;
                        break;
                    case "ctxMoveDown":
                        menuItem.IsEnabled = canMoveDown;
                        break;
                }
            }
        }

        private bool CanMoveSelected(int direction)
        {
            if (_selectedItem == null || direction == 0)
            {
                return false;
            }

            if (_selectedItem.Data is JRLQuest)
            {
                int index = _model.IndexOf(_selectedItem);
                int newIndex = index + direction;
                return index >= 0 && newIndex >= 0 && newIndex < _model.Count;
            }

            if (_selectedItem.Data is JRLQuestEntry)
            {
                var parent = FindQuestItemForEntry(_selectedItem);
                int index = parent?.Children.IndexOf(_selectedItem) ?? -1;
                int newIndex = index + direction;
                return parent != null && index >= 0 && newIndex >= 0 && newIndex < parent.Children.Count;
            }

            return false;
        }

        private void OnTreeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveCurrentSelectionToModel();
            _selectedItem = ResolveSourceTreeItem(_journalTree?.SelectedItem as JournalTreeItem);
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
        }

        private void SaveCurrentSelectionToModel()
        {
            if (_selectedItem == null) return;
            if (_selectedItem.Data is JRLQuest quest)
            {
                if (_questNameEdit != null) quest.Name = _questNameEdit.GetLocString();
                if (_questPlanetId != null && _questPlanetId.Value.HasValue) quest.PlanetId = (int)_questPlanetId.Value.Value;
                if (_questPlotIndex != null && _questPlotIndex.Value.HasValue) quest.PlotIndex = (int)_questPlotIndex.Value.Value;
                if (_questPriority != null && _questPriority.SelectedItem is JRLQuestPriority p) quest.Priority = p;
                if (_questTag != null) quest.Tag = _questTag.Text ?? "";
                if (_questComment != null) quest.Comment = _questComment.Text ?? "";
                RefreshQuestItem(_selectedItem);
            }
            else if (_selectedItem.Data is JRLQuestEntry entry)
            {
                if (_entryTextEdit != null) entry.Text = _entryTextEdit.GetLocString();
                if (_entryId != null && _entryId.Value.HasValue) entry.EntryId = (int)_entryId.Value.Value;
                if (_entryEnd != null) entry.End = _entryEnd.IsChecked == true;
                if (_entryXpPct != null && _entryXpPct.Value.HasValue) entry.XpPercentage = (float)_entryXpPct.Value.Value;
                RefreshEntryItem(_selectedItem);
            }
        }

        private void LoadSelectionIntoDetail()
        {
            if (_selectedItem == null) return;
            _loadingSelection = true;
            try
            {
                if (_selectedItem.Data is JRLQuest quest)
                {
                    if (_questNameEdit != null) _questNameEdit.SetLocString(quest.Name);
                    if (_questPlanetId != null) _questPlanetId.Value = quest.PlanetId;
                    if (_questPlotIndex != null) _questPlotIndex.Value = quest.PlotIndex;
                    if (_questPriority != null) _questPriority.SelectedItem = quest.Priority;
                    if (_questTag != null) _questTag.Text = quest.Tag;
                    if (_questComment != null) _questComment.Text = quest.Comment;
                    if (_questEntryCount != null) _questEntryCount.Text = quest.Entries.Count.ToString();
                }
                else if (_selectedItem.Data is JRLQuestEntry entry)
                {
                    if (_entryTextEdit != null) _entryTextEdit.SetLocString(entry.Text);
                    if (_entryId != null) _entryId.Value = entry.EntryId;
                    if (_entryEnd != null) _entryEnd.IsChecked = entry.End;
                    if (_entryXpPct != null) _entryXpPct.Value = (decimal)entry.XpPercentage;
                }
            }
            finally
            {
                _loadingSelection = false;
            }
        }

        private void RefreshDetailVisibility()
        {
            bool isQuest = _selectedItem?.Data is JRLQuest;
            bool isEntry = _selectedItem?.Data is JRLQuestEntry;
            if (_detailNoSelection != null) _detailNoSelection.IsVisible = !isQuest && !isEntry;
            if (_detailQuest != null) _detailQuest.IsVisible = isQuest;
            if (_detailEntry != null) _detailEntry.IsVisible = isEntry;
        }

        private void UpdateButtonStates()
        {
            bool hasSelection = _selectedItem != null;
            bool isQuest = _selectedItem?.Data is JRLQuest;
            if (_addEntryButton != null) _addEntryButton.IsEnabled = isQuest;
            if (_removeButton != null) _removeButton.IsEnabled = hasSelection;
        }

        private void UpdateQuestCountLabel()
        {
            if (_questCountLabel != null)
                _questCountLabel.Text = _model == null ? "0 quests" : $"{_model.Count} quest(s)";
        }

        private void AddQuest()
        {
            var quest = new JRLQuest { Name = LocalizedString.FromEnglish("New Quest") };
            _jrl.Quests.Add(quest);
            var item = new JournalTreeItem { Data = quest };
            RefreshQuestItem(item);
            _model.Add(item);
            RebindTree();
            _selectedItem = item;
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
            MarkDocumentDirty();
            UpdateStatusBar();
            UpdateQuestCountLabel();
        }

        private void AddEntry()
        {
            var questItem = _selectedItem?.Data is JRLQuest ? _selectedItem : FindQuestItemForEntry(_selectedItem);
            if (questItem == null) return;
            var quest = (JRLQuest)questItem.Data;
            var entry = new JRLQuestEntry { Text = LocalizedString.FromEnglish("New Entry") };
            quest.Entries.Add(entry);
            var entryItem = new JournalTreeItem { Data = entry };
            RefreshEntryItem(entryItem);
            questItem.Children.Add(entryItem);
            RebindTree();
            _selectedItem = entryItem;
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
            MarkDocumentDirty();
            UpdateStatusBar();
            if (_questEntryCount != null && _selectedItem == questItem) _questEntryCount.Text = quest.Entries.Count.ToString();
        }

        private void RemoveSelected()
        {
            if (_selectedItem == null) return;
            if (_selectedItem.Data is JRLQuest quest)
            {
                int idx = _model.IndexOf(_selectedItem);
                if (idx >= 0) { _model.RemoveAt(idx); _jrl.Quests.RemoveAt(idx); }
            }
            else if (_selectedItem.Data is JRLQuestEntry entry)
            {
                foreach (var q in _model)
                {
                    if (q.Data is JRLQuest qq && qq.Entries.Contains(entry))
                    {
                        int eidx = qq.Entries.IndexOf(entry);
                        if (eidx >= 0) { qq.Entries.RemoveAt(eidx); q.Children.RemoveAt(eidx); }
                        break;
                    }
                }
            }
            _selectedItem = null;
            RebindTree();
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
            MarkDocumentDirty();
            UpdateStatusBar();
            UpdateQuestCountLabel();
        }

        private void DuplicateSelected()
        {
            if (_selectedItem == null)
            {
                return;
            }

            if (_selectedItem.Data is JRLQuest quest)
            {
                int index = _model.IndexOf(_selectedItem);
                if (index < 0)
                {
                    return;
                }

                JRLQuest copy = CopyQuest(quest);
                copy.Tag = (copy.Tag ?? string.Empty) + "_copy";
                var copyItem = CreateQuestItem(copy);
                int insertIndex = index + 1;
                _jrl.Quests.Insert(insertIndex, copy);
                _model.Insert(insertIndex, copyItem);
                _selectedItem = copyItem;
            }
            else if (_selectedItem.Data is JRLQuestEntry entry)
            {
                var parent = FindQuestItemForEntry(_selectedItem);
                if (parent == null || !(parent.Data is JRLQuest parentQuest))
                {
                    return;
                }

                int index = parent.Children.IndexOf(_selectedItem);
                if (index < 0)
                {
                    return;
                }

                JRLQuestEntry copy = CopyEntry(entry);
                copy.EntryId++;
                var copyItem = new JournalTreeItem { Data = copy };
                RefreshEntryItem(copyItem);
                int insertIndex = index + 1;
                parentQuest.Entries.Insert(insertIndex, copy);
                parent.Children.Insert(insertIndex, copyItem);
                _selectedItem = copyItem;
            }

            RebindTree();
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
            MarkDocumentDirty();
            UpdateStatusBar();
            UpdateQuestCountLabel();
        }

        private void MoveSelected(int direction)
        {
            if (_selectedItem == null || direction == 0)
            {
                return;
            }

            if (_selectedItem.Data is JRLQuest)
            {
                int index = _model.IndexOf(_selectedItem);
                int newIndex = index + direction;
                if (index < 0 || newIndex < 0 || newIndex >= _model.Count)
                {
                    return;
                }

                var quest = _jrl.Quests[index];
                _jrl.Quests.RemoveAt(index);
                _jrl.Quests.Insert(newIndex, quest);
                _model.RemoveAt(index);
                _model.Insert(newIndex, _selectedItem);
            }
            else if (_selectedItem.Data is JRLQuestEntry entry)
            {
                var parent = FindQuestItemForEntry(_selectedItem);
                if (parent == null || !(parent.Data is JRLQuest parentQuest))
                {
                    return;
                }

                int index = parent.Children.IndexOf(_selectedItem);
                int newIndex = index + direction;
                if (index < 0 || newIndex < 0 || newIndex >= parent.Children.Count)
                {
                    return;
                }

                parentQuest.Entries.Remove(entry);
                parentQuest.Entries.Insert(newIndex, entry);
                parent.Children.RemoveAt(index);
                parent.Children.Insert(newIndex, _selectedItem);
            }

            RebindTree();
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
            MarkDocumentDirty();
            UpdateStatusBar();
        }

        private void SortSelectedQuestEntries(bool ascending)
        {
            var questItem = _selectedItem?.Data is JRLQuest ? _selectedItem : FindQuestItemForEntry(_selectedItem);
            if (questItem == null || !(questItem.Data is JRLQuest quest))
            {
                return;
            }

            var sortedEntries = ascending
                ? quest.Entries.OrderBy(entry => entry.EntryId).ToList()
                : quest.Entries.OrderByDescending(entry => entry.EntryId).ToList();
            quest.Entries.Clear();
            quest.Entries.AddRange(sortedEntries);
            questItem.Children.Clear();
            foreach (var entry in sortedEntries)
            {
                var entryItem = new JournalTreeItem { Data = entry };
                RefreshEntryItem(entryItem);
                questItem.Children.Add(entryItem);
            }

            _selectedItem = questItem;
            RebindTree();
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
            MarkDocumentDirty();
            UpdateStatusBar();
        }

        private JournalTreeItem FindQuestItemForEntry(JournalTreeItem entryItem)
        {
            entryItem = ResolveSourceTreeItem(entryItem);
            if (entryItem == null)
            {
                return null;
            }

            foreach (var questItem in _model)
            {
                if (questItem.Children.Contains(entryItem))
                {
                    return questItem;
                }
            }

            return null;
        }

        private JournalTreeItem CreateQuestItem(JRLQuest quest)
        {
            var questItem = new JournalTreeItem { Data = quest };
            RefreshQuestItem(questItem);
            foreach (var entry in quest.Entries)
            {
                var entryItem = new JournalTreeItem { Data = entry };
                RefreshEntryItem(entryItem);
                questItem.Children.Add(entryItem);
            }
            return questItem;
        }

        private static JRLQuest CopyQuest(JRLQuest quest)
        {
            var copy = new JRLQuest
            {
                Name = CopyLocalizedString(quest.Name),
                PlanetId = quest.PlanetId,
                PlotIndex = quest.PlotIndex,
                Priority = quest.Priority,
                Tag = quest.Tag ?? string.Empty,
                Comment = quest.Comment ?? string.Empty
            };
            foreach (var entry in quest.Entries)
            {
                copy.Entries.Add(CopyEntry(entry));
            }
            return copy;
        }

        private static JRLQuestEntry CopyEntry(JRLQuestEntry entry)
        {
            return new JRLQuestEntry
            {
                Text = CopyLocalizedString(entry.Text),
                EntryId = entry.EntryId,
                End = entry.End,
                XpPercentage = entry.XpPercentage
            };
        }

        private static LocalizedString CopyLocalizedString(LocalizedString value)
        {
            if (value == null)
            {
                return LocalizedString.FromInvalid();
            }

            var copy = new LocalizedString(value.StringRef);
            foreach (var item in value)
            {
                copy.SetData(item.Item1, item.Item2, item.Item3);
            }
            return copy;
        }

        private void RebindTree()
        {
            if (_journalTree == null) return;
            ApplyJournalFilter();
        }

        private void ApplyJournalFilter()
        {
            if (_journalTree == null)
            {
                return;
            }

            string filterText = (_filterEdit?.Text ?? "").Trim();
            _journalTree.ItemsSource = null;
            _journalTree.ItemsSource = string.IsNullOrEmpty(filterText) ? _model : BuildFilteredModel(filterText);
        }

        private List<JournalTreeItem> BuildFilteredModel(string filterText)
        {
            var visible = new List<JournalTreeItem>();
            if (_model == null)
            {
                return visible;
            }

            string text = (filterText ?? "").Trim().ToLowerInvariant();
            if (text.Length == 0)
            {
                visible.AddRange(_model);
                return visible;
            }

            string mode = _settings == null ? JRLEditorSettings.FilterModeSmart : _settings.FilterMode;
            foreach (var questItem in _model)
            {
                var quest = questItem.Data as JRLQuest;
                bool questMatches = QuestMatchesFilter(questItem, quest, text);
                var matchingEntries = new List<JournalTreeItem>();
                bool entryMatches = false;

                foreach (var entryItem in questItem.Children)
                {
                    bool ematch = EntryMatchesFilter(entryItem, entryItem.Data as JRLQuestEntry, text);
                    if (ematch)
                    {
                        entryMatches = true;
                    }

                    if (mode == JRLEditorSettings.FilterModeQuestOnly)
                    {
                        if (questMatches)
                        {
                            matchingEntries.Add(entryItem);
                        }
                    }
                    else if (mode == JRLEditorSettings.FilterModeAllLevels)
                    {
                        if (questMatches || ematch)
                        {
                            matchingEntries.Add(entryItem);
                        }
                    }
                    else if (questMatches || ematch)
                    {
                        matchingEntries.Add(entryItem);
                    }
                }

                bool showQuest = mode == JRLEditorSettings.FilterModeQuestOnly
                    ? questMatches
                    : questMatches || entryMatches;
                if (!showQuest)
                {
                    continue;
                }

                var visibleQuest = new JournalTreeItem
                {
                    Data = questItem.Data,
                    Text = questItem.Text,
                    SourceItem = questItem
                };
                visibleQuest.Children.AddRange(matchingEntries.Select(entry => new JournalTreeItem
                {
                    Data = entry.Data,
                    Text = entry.Text,
                    SourceItem = entry
                }));
                visible.Add(visibleQuest);
            }

            return visible;
        }

        private bool QuestMatchesFilter(JournalTreeItem questItem, JRLQuest quest, string text)
        {
            if (questItem != null && MatchesFilterText(questItem.Text, text))
            {
                return true;
            }

            return quest != null && MatchesFilterText(quest.Tag, text);
        }

        private static bool EntryMatchesFilter(JournalTreeItem entryItem, JRLQuestEntry entry, string text)
        {
            if (entryItem != null && MatchesFilterText(entryItem.Text, text))
            {
                return true;
            }

            return entry != null && MatchesFilterText(entry.EntryId.ToString(), text);
        }

        private static bool MatchesFilterText(string value, string text)
        {
            return !string.IsNullOrEmpty(value) &&
                   value.ToLowerInvariant().Contains(text);
        }

        private static JournalTreeItem ResolveSourceTreeItem(JournalTreeItem item)
        {
            return item == null ? null : (item.SourceItem ?? item);
        }

        private void SetupSignals()
        {
            Opened += (s, e) => { UpdateStatusBar(); Focus(); };
            KeyDown += OnWindowKeyDown;
        }

        private void SetupMenuHandlers()
        {
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
            EditorHelpers.BindMenuClicks(this, new (string menuItemName, Action handler)[]
            {
                ("actionUndo", Undo),
                ("actionRedo", Redo),
                ("actionDuplicate", DuplicateSelected),
                ("actionMoveUp", () => MoveSelected(-1)),
                ("actionMoveDown", () => MoveSelected(1)),
                ("actionSortEntriesAscending", () => SortSelectedQuestEntries(true)),
                ("actionSortEntriesDescending", () => SortSelectedQuestEntries(false)),
                ("actionFind", ShowFindDialog),
                ("actionFindNext", FindNextMatch),
            });
        }

        protected override async Task ShowSettingsDialogAsync()
        {
            var dialog = new JRLSettingsDialog(_settings)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            bool? accepted = await dialog.ShowDialog<bool?>(this);
            if (accepted == true)
            {
                _lastFoundItem = null;
                ApplyJournalFilter();
            }
        }

        private void PushState()
        {
            if (_undoRedoInProgress) return;
            try
            {
                var (data, _) = Build();
                if (data == null || data.Length == 0) return;
                _undoStack.Add(data);
                if (_undoStack.Count > UndoMaxLevels) _undoStack.RemoveAt(0);
                _redoStack.Clear();
                MarkDocumentDirty();
            }
            catch { }
        }

        private void Undo()
        {
            if (_undoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                byte[] data = _undoStack[_undoStack.Count - 1];
                _undoStack.RemoveAt(_undoStack.Count - 1);
                _redoStack.Add(Build().Item1);
                LoadFromBytes(data);
                UpdateStatusBar();
            }
            finally { _undoRedoInProgress = false; }
        }

        private void Redo()
        {
            if (_redoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                byte[] data = _redoStack[_redoStack.Count - 1];
                _redoStack.RemoveAt(_redoStack.Count - 1);
                _undoStack.Add(Build().Item1);
                LoadFromBytes(data);
                UpdateStatusBar();
            }
            finally { _undoRedoInProgress = false; }
        }

        private void LoadFromBytes(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                _jrl = new JRL();
                _originalGff = null;
                _model.Clear();
            }
            else
            {
                try
                {
                    _originalGff = GFF.FromBytes(data);
                    _jrl = JRLHelper.ReadJrl(data);
                    _model.Clear();
                    foreach (JRLQuest quest in _jrl.Quests)
                    {
                        var questItem = new JournalTreeItem { Data = quest };
                        RefreshQuestItem(questItem);
                        _model.Add(questItem);
                        foreach (JRLQuestEntry entry in quest.Entries)
                        {
                            var entryItem = new JournalTreeItem { Data = entry };
                            RefreshEntryItem(entryItem);
                            questItem.Children.Add(entryItem);
                        }
                    }
                }
                catch
                {
                    _jrl = new JRL();
                    _originalGff = null;
                    _model.Clear();
                }
            }
            _lastFoundItem = null;
            _selectedItem = null;
            _undoRedoInProgress = true;
            try
            {
                RebindTree();
                UpdateQuestCountLabel();
                UpdateStatusBar();
            }
            finally { _undoRedoInProgress = false; }
        }

        public override void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            try
            {
                _undoStack.Clear();
                _redoStack.Clear();
                LoadFromBytes(_revert);
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Revert failed: {ex}");
            }
        }

        protected override async Task RunSaveAsAsync()
        {
            await base.RunSaveAsAsync();
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            try
            {
                int count = _model?.Count ?? 0;
                string text = count == 0 ? "Journal" : $"{count} quest(s)";
                var c = _statusText ?? EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
                if (c != null) c.Text = text;
            }
            catch { }
        }

        private void ShowFindDialog()
        {
            var dialog = new Window
            {
                Title = "Find",
                Width = 400,
                Height = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var findBox = new TextBox { Watermark = "Find what:", Text = _findText, Margin = new Avalonia.Thickness(8) };
            var matchCase = new CheckBox { Content = "Match case", IsChecked = _findMatchCase, Margin = new Avalonia.Thickness(8) };
            var findNext = new Button { Content = "Find Next", Margin = new Avalonia.Thickness(8) };
            var closeBtn = new Button { Content = "Close", Margin = new Avalonia.Thickness(8) };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(10) };
            panel.Children.Add(findBox);
            panel.Children.Add(matchCase);
            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal };
            btnPanel.Children.Add(findNext);
            btnPanel.Children.Add(closeBtn);
            panel.Children.Add(btnPanel);
            dialog.Content = panel;
            findNext.Click += (s, e) => { _findText = findBox.Text ?? ""; _findMatchCase = matchCase.IsChecked == true; FindNextMatch(); };
            closeBtn.Click += (s, e) => dialog.Close();
            dialog.Opened += (s, e) => findBox.Focus();
            dialog.ShowDialog(this);
        }

        private void FindNextMatch()
        {
            if (string.IsNullOrEmpty(_findText) || _model == null) return;
            string t = _findMatchCase ? _findText : _findText.ToLowerInvariant();
            bool Match(string value) => value != null && (_findMatchCase ? value : value.ToLowerInvariant()).Contains(t);
            var flat = FindMatches(Match);
            int start = _lastFoundItem == null ? 0 : flat.IndexOf(_lastFoundItem) + 1;
            if (flat.Count > 0)
            {
                int idx = start >= 0 && start < flat.Count ? start : 0;
                _lastFoundItem = flat[idx];
                SelectAndReveal(flat[idx]);
                return;
            }
            _lastFoundItem = null;
        }

        private List<JournalTreeItem> FindMatches(Func<string, bool> match)
        {
            var matches = new List<JournalTreeItem>();
            if (_model == null || match == null)
            {
                return matches;
            }

            string mode = _settings == null ? JRLEditorSettings.FilterModeSmart : _settings.FilterMode;
            foreach (var questItem in _model)
            {
                bool questMatches = match(questItem.Text);
                bool entryMatches = questItem.Children.Any(entry => match(entry.Text));

                if (mode == JRLEditorSettings.FilterModeQuestOnly)
                {
                    if (questMatches)
                    {
                        matches.Add(questItem);
                    }
                    continue;
                }

                if (mode == JRLEditorSettings.FilterModeSmart)
                {
                    if (questMatches || entryMatches)
                    {
                        matches.Add(questItem);
                    }
                    continue;
                }

                if (questMatches)
                {
                    matches.Add(questItem);
                }
                foreach (var entryItem in questItem.Children)
                {
                    if (match(entryItem.Text))
                    {
                        matches.Add(entryItem);
                    }
                }
            }

            return matches;
        }

        private void SelectAndReveal(JournalTreeItem item)
        {
            if (_journalTree == null) return;
            _journalTree.SelectedItem = FindVisibleTreeItem(item) ?? item;
            Dispatcher.UIThread.Post(() =>
            {
                var container = _journalTree.ContainerFromItem(_journalTree.SelectedItem);
                container?.BringIntoView();
            }, DispatcherPriority.Loaded);
        }

        private JournalTreeItem FindVisibleTreeItem(JournalTreeItem sourceItem)
        {
            if (sourceItem == null || _journalTree?.ItemsSource == null)
            {
                return null;
            }

            foreach (var root in _journalTree.ItemsSource)
            {
                var rootItem = root as JournalTreeItem;
                if (rootItem == null)
                {
                    continue;
                }
                if (ResolveSourceTreeItem(rootItem) == sourceItem)
                {
                    return rootItem;
                }
                foreach (var child in rootItem.Children)
                {
                    if (ResolveSourceTreeItem(child) == sourceItem)
                    {
                        return child;
                    }
                }
            }

            return null;
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (e.KeyModifiers & KeyModifiers.Control) != 0) { Save(); e.Handled = true; return; }
            if (e.Key == Key.Z && (e.KeyModifiers & KeyModifiers.Control) != 0) { Undo(); e.Handled = true; return; }
            if (e.Key == Key.Y && (e.KeyModifiers & KeyModifiers.Control) != 0) { Redo(); e.Handled = true; return; }
            if (e.Key == Key.F && (e.KeyModifiers & KeyModifiers.Control) != 0) { ShowFindDialog(); e.Handled = true; return; }
            if (e.Key == Key.Delete) { RemoveSelected(); e.Handled = true; return; }
            if (e.Key == Key.F3) { FindNextMatch(); e.Handled = true; }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _undoStack.Clear();
            _redoStack.Clear();
            try { LoadFromBytes(data); }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load JRL: {ex}");
                New();
            }
        }

        public override Tuple<byte[], byte[]> Build()
        {
            SaveCurrentSelectionToModel();
            var gff = JRLHelper.DismantleJrl(_jrl);

            // Preserve unmodified fields from original GFF that aren't yet supported by JRL object model.
            // Supported journal fields stay authoritative from the structured editor model.
            if (_originalGff != null)
            {
                var originalRoot = _originalGff.Root;
                var newRoot = gff.Root;
                MergeOriginalJrlCategoryMetadata(originalRoot, newRoot);

                // List of fields that JRLHelper.DismantleJrl explicitly sets
                // Categories is handled above by merging unsupported metadata into the edited list.
                var fieldsSetByDismantle = new HashSet<string>
                {
                    "Categories"
                };

                // Copy all fields from original that aren't explicitly set by DismantleJrl
                foreach (var (label, fieldType, value) in originalRoot)
                {
                    if (!fieldsSetByDismantle.Contains(label) && !newRoot.Exists(label))
                    {
                        CopyGffField(originalRoot, newRoot, label, fieldType);
                    }
                }
            }

            byte[] data = GFFAuto.BytesGff(gff, ResourceType.JRL);
            return Tuple.Create(data, new byte[0]);
        }

        private static void MergeOriginalJrlCategoryMetadata(GFFStruct originalRoot, GFFStruct newRoot)
        {
            if (originalRoot == null || newRoot == null)
            {
                return;
            }

            if (!originalRoot.TryGetList("Categories", out var originalCategories) ||
                !newRoot.TryGetList("Categories", out var newCategories))
            {
                return;
            }

            int categoryCount = Math.Min(originalCategories.Count, newCategories.Count);
            for (int i = 0; i < categoryCount; i++)
            {
                var originalCategory = originalCategories.At(i);
                var newCategory = newCategories.At(i);
                if (originalCategory == null || newCategory == null)
                {
                    continue;
                }

                newCategory.StructId = originalCategory.StructId;
                CopyUnsupportedFields(originalCategory, newCategory, new HashSet<string>
                {
                    "Comment",
                    "Name",
                    "PlanetID",
                    "PlotIndex",
                    "Priority",
                    "Tag",
                    "EntryList"
                });

                MergeOriginalJrlEntryMetadata(originalCategory, newCategory);
            }
        }

        private static void MergeOriginalJrlEntryMetadata(GFFStruct originalCategory, GFFStruct newCategory)
        {
            if (originalCategory == null || newCategory == null)
            {
                return;
            }

            if (!originalCategory.TryGetList("EntryList", out var originalEntries) ||
                !newCategory.TryGetList("EntryList", out var newEntries))
            {
                return;
            }

            int entryCount = Math.Min(originalEntries.Count, newEntries.Count);
            for (int i = 0; i < entryCount; i++)
            {
                var originalEntry = originalEntries.At(i);
                var newEntry = newEntries.At(i);
                if (originalEntry == null || newEntry == null)
                {
                    continue;
                }

                newEntry.StructId = originalEntry.StructId;
                CopyUnsupportedFields(originalEntry, newEntry, new HashSet<string>
                {
                    "End",
                    "ID",
                    "Text",
                    "XP_Percentage"
                });
            }
        }

        private static void CopyUnsupportedFields(GFFStruct source, GFFStruct destination, HashSet<string> supportedLabels)
        {
            if (source == null || destination == null)
            {
                return;
            }

            foreach (var (label, fieldType, _) in source)
            {
                if (supportedLabels.Contains(label) || destination.Exists(label))
                {
                    continue;
                }

                CopyGffField(source, destination, label, fieldType);
            }
        }

        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            _jrl = new JRL();
            _model.Clear();
            _originalGff = null; // Clear original GFF when creating new file
            _lastFoundItem = null;
            _selectedItem = null;
            RebindTree();
            UpdateQuestCountLabel();
            UpdateStatusBar();
            UpdateButtonStates();
        }

        // Helper method to copy a GFF field from one struct to another, preserving type
        private static void CopyGffField(GFFStruct source, GFFStruct destination, string label, GFFFieldType fieldType)
        {
            switch (fieldType)
            {
                case GFFFieldType.UInt8:
                    destination.SetUInt8(label, source.GetUInt8(label));
                    break;
                case GFFFieldType.Int8:
                    destination.SetInt8(label, source.GetInt8(label));
                    break;
                case GFFFieldType.UInt16:
                    destination.SetUInt16(label, source.GetUInt16(label));
                    break;
                case GFFFieldType.Int16:
                    destination.SetInt16(label, source.GetInt16(label));
                    break;
                case GFFFieldType.UInt32:
                    destination.SetUInt32(label, source.GetUInt32(label));
                    break;
                case GFFFieldType.Int32:
                    destination.SetInt32(label, source.GetInt32(label));
                    break;
                case GFFFieldType.UInt64:
                    destination.SetUInt64(label, source.GetUInt64(label));
                    break;
                case GFFFieldType.Int64:
                    destination.SetInt64(label, source.GetInt64(label));
                    break;
                case GFFFieldType.Single:
                    destination.SetSingle(label, source.GetSingle(label));
                    break;
                case GFFFieldType.Double:
                    destination.SetDouble(label, source.GetDouble(label));
                    break;
                case GFFFieldType.String:
                    destination.SetString(label, source.GetString(label));
                    break;
                case GFFFieldType.ResRef:
                    destination.SetResRef(label, source.GetResRef(label));
                    break;
                case GFFFieldType.LocalizedString:
                    destination.SetLocString(label, source.GetLocString(label));
                    break;
                case GFFFieldType.Binary:
                    destination.SetBinary(label, source.GetBinary(label));
                    break;
                case GFFFieldType.Vector3:
                    destination.SetVector3(label, source.GetVector3(label));
                    break;
                case GFFFieldType.Vector4:
                    destination.SetVector4(label, source.GetVector4(label));
                    break;
                case GFFFieldType.Struct:
                    destination.SetStruct(label, source.GetStruct(label));
                    break;
                case GFFFieldType.List:
                    destination.SetList(label, source.GetList(label));
                    break;
            }
        }

        private void RefreshEntryItem(JournalTreeItem entryItem)
        {
            if (entryItem.Data is JRLQuestEntry entry)
            {
                string text;
                if (_installation == null)
                {
                    text = $"[{entry.EntryId}] {entry.Text}";
                }
                else
                {
                    text = $"[{entry.EntryId}] {_installation.String(entry.Text)}";
                }
                entryItem.Text = text;
            }
        }

        private void RefreshQuestItem(JournalTreeItem questItem)
        {
            if (questItem.Data is JRLQuest quest)
            {
                string text;
                if (_installation == null)
                {
                    text = quest.Name?.ToString() ?? "[Unnamed]";
                }
                else
                {
                    text = _installation.String(quest.Name, "[Unnamed]");
                }
                questItem.Text = text;
            }
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        // Property to access model for tests
        public int ModelRowCount => _model.Count;

        internal void AddQuestForTest()
        {
            var quest = new JRLQuest { Name = LocalizedString.FromEnglish("New Quest") };
            _jrl.Quests.Add(quest);
            var item = new JournalTreeItem { Data = quest };
            RefreshQuestItem(item);
            _model.Add(item);
            RebindTree();
            _selectedItem = item;
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
            UpdateQuestCountLabel();
        }

        internal void AddQuestForTest(
            LocalizedString name,
            int planetId,
            int plotIndex,
            JRLQuestPriority priority,
            string tag,
            string comment)
        {
            var quest = new JRLQuest
            {
                Name = name ?? LocalizedString.FromInvalid(),
                PlanetId = planetId,
                PlotIndex = plotIndex,
                Priority = priority,
                Tag = tag ?? string.Empty,
                Comment = comment ?? string.Empty
            };
            _jrl.Quests.Add(quest);
            var item = new JournalTreeItem { Data = quest };
            RefreshQuestItem(item);
            _model.Add(item);
            RebindTree();
            _selectedItem = item;
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
            UpdateQuestCountLabel();
        }

        internal void AddEntryForTest()
        {
            var questItem = _selectedItem?.Data is JRLQuest ? _selectedItem : null;
            if (questItem == null)
            {
                return;
            }

            var quest = (JRLQuest)questItem.Data;
            var entry = new JRLQuestEntry { Text = LocalizedString.FromEnglish("New Entry") };
            quest.Entries.Add(entry);
            var entryItem = new JournalTreeItem { Data = entry };
            RefreshEntryItem(entryItem);
            questItem.Children.Add(entryItem);
            RebindTree();
            _selectedItem = entryItem;
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
            UpdateStatusBar();
        }

        internal void AddEntryForTest(
            int questIndex,
            LocalizedString text,
            int entryId,
            bool end,
            float xpPercentage)
        {
            if (_model == null || questIndex < 0 || questIndex >= _model.Count)
            {
                return;
            }

            var questItem = _model[questIndex];
            if (!(questItem.Data is JRLQuest quest))
            {
                return;
            }

            var entry = new JRLQuestEntry
            {
                Text = text ?? LocalizedString.FromInvalid(),
                EntryId = entryId,
                End = end,
                XpPercentage = xpPercentage
            };
            quest.Entries.Add(entry);
            var entryItem = new JournalTreeItem { Data = entry };
            RefreshEntryItem(entryItem);
            questItem.Children.Add(entryItem);
            RebindTree();
            _selectedItem = entryItem;
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
            UpdateStatusBar();
        }

        internal void SelectQuestForTest(int index)
        {
            if (_model == null || index < 0 || index >= _model.Count)
            {
                return;
            }

            _selectedItem = _model[index];
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
        }

        internal void SelectEntryForTest(int questIndex, int entryIndex)
        {
            if (_model == null || questIndex < 0 || questIndex >= _model.Count)
            {
                return;
            }

            var questItem = _model[questIndex];
            if (entryIndex < 0 || entryIndex >= questItem.Children.Count)
            {
                return;
            }

            var entryItem = questItem.Children[entryIndex];
            _selectedItem = entryItem;
            LoadSelectionIntoDetail();
            RefreshDetailVisibility();
            UpdateButtonStates();
        }
    }

    // Simple tree item class to hold quest/entry data (similar to QStandardItem)
    internal class JournalTreeItem
    {
        public string Text { get; set; } = string.Empty;
        public object Data { get; set; }
        public List<JournalTreeItem> Children { get; set; } = new List<JournalTreeItem>();
        public JournalTreeItem SourceItem { get; set; }
    }

    internal sealed class JRLEditorSettings : Settings
    {
        public const string FilterModeSmart = "smart";
        public const string FilterModeQuestOnly = "quest_only";
        public const string FilterModeAllLevels = "all_levels";

        public JRLEditorSettings() : base("JRLEditor")
        {
        }

        public string FilterMode
        {
            get
            {
                string value = GetValue("jrl_editor/filter_mode", FilterModeSmart);
                return IsValidFilterMode(value) ? value : FilterModeSmart;
            }
            set
            {
                SetValue("jrl_editor/filter_mode", IsValidFilterMode(value) ? value : FilterModeSmart);
            }
        }

        public bool JumpAutoOpen
        {
            get { return GetValue("jrl_editor/jump_auto_open", true); }
            set { SetValue("jrl_editor/jump_auto_open", value); }
        }

        private static bool IsValidFilterMode(string value)
        {
            return value == FilterModeSmart ||
                   value == FilterModeQuestOnly ||
                   value == FilterModeAllLevels;
        }
    }

    internal sealed class JRLSettingsDialog : Window
    {
        private readonly JRLEditorSettings _settings;
        private readonly ComboBox _filterCombo;
        private readonly CheckBox _jumpAutoOpen;
        private readonly JRLFilterModeOption[] _filterOptions;

        public JRLSettingsDialog(JRLEditorSettings settings)
        {
            _settings = settings ?? new JRLEditorSettings();
            Title = "Journal Editor Settings";
            Width = 460;
            Height = 230;
            MinWidth = 420;
            MinHeight = 220;

            _filterOptions = new[]
            {
                new JRLFilterModeOption(JRLEditorSettings.FilterModeSmart, "Smart - show quest if name/tag or any entry matches"),
                new JRLFilterModeOption(JRLEditorSettings.FilterModeQuestOnly, "Quest only - match quest name or tag only"),
                new JRLFilterModeOption(JRLEditorSettings.FilterModeAllLevels, "All levels - show any quest or entry that matches")
            };

            _filterCombo = new ComboBox
            {
                ItemsSource = _filterOptions,
                Margin = new Avalonia.Thickness(0, 0, 0, 8)
            };
            SelectFilterMode(_settings.FilterMode);

            _jumpAutoOpen = new CheckBox
            {
                Content = "Auto-open editor when exactly one result is found",
                IsChecked = _settings.JumpAutoOpen,
                Margin = new Avalonia.Thickness(0, 0, 0, 12)
            };

            var okButton = new Button { Content = "OK", MinWidth = 80, Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var cancelButton = new Button { Content = "Cancel", MinWidth = 80 };
            okButton.Click += (s, e) => Accept();
            cancelButton.Click += (s, e) => Close(false);

            var buttons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            buttons.Children.Add(okButton);
            buttons.Children.Add(cancelButton);

            var form = new StackPanel { Spacing = 4 };
            form.Children.Add(new TextBlock { Text = "Filter mode:" });
            form.Children.Add(_filterCombo);
            form.Children.Add(new TextBlock { Text = "Jump to scripts/dialogs:" });
            form.Children.Add(_jumpAutoOpen);
            form.Children.Add(buttons);

            Content = new Border
            {
                Padding = new Avalonia.Thickness(16),
                Child = form
            };
        }

        private void SelectFilterMode(string filterMode)
        {
            _filterCombo.SelectedItem = _filterOptions.FirstOrDefault(option => option.Value == filterMode) ?? _filterOptions[0];
        }

        private void Accept()
        {
            if (_filterCombo.SelectedItem is JRLFilterModeOption option)
            {
                _settings.FilterMode = option.Value;
            }
            _settings.JumpAutoOpen = _jumpAutoOpen.IsChecked == true;
            Close(true);
        }

        private sealed class JRLFilterModeOption
        {
            public string Value { get; }
            private readonly string _label;

            public JRLFilterModeOption(string value, string label)
            {
                Value = value;
                _label = label;
            }

            public override string ToString()
            {
                return _label;
            }
        }
    }
}
