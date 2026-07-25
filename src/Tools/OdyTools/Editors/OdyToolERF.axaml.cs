using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using BioWare;
using BioWare.Resource.Formats.BIF;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.MDL;
using BioWare.Resource.Formats.NCS;
using BioWare.Resource.Formats.RIM;
using BioWare.Resource.Formats.TPC;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Common;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Utils;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using IconType = MsBox.Avalonia.Enums.Icon;
using BIFResource = BioWare.Resource.Formats.BIF.BIFResource;
using ERFResource = BioWare.Resource.Formats.ERF.ERFResource;
using RIMResource = BioWare.Resource.Formats.RIM.RIMResource;

namespace OdyTools.Editors
{
    public partial class OdyToolERF : Editor
    {
        private const int MinEditorWidth = 520;
        private const int MinEditorHeight = 380;
        private const int UndoMaxLevels = 30;
        private static readonly (string menuItemName, string localizationKey)[] MenuLocalizationItems =
        {
            ("actionNew", "New"),
            ("actionOpen", "Open"),
            ("actionSave", "Save"),
            ("actionSaveAs", "Save As"),
            ("actionRevert", "Revert"),
            ("actionExit", "Exit"),
            ("actionUndo", "Undo"),
            ("actionRedo", "Redo"),
            ("actionFind", "Find..."),
            ("actionFindNext", "Find Next"),
            ("actionRemove", "Remove"),
            ("actionExtract", "Extract"),
            ("actionOpenResource", "Open Resource"),
            ("ctxExtract", "Extract to..."),
            ("ctxRename", "Rename"),
            ("ctxOpen", "Open"),
            ("ctxRemove", "Remove"),
            ("ctxFind", "Find..."),
            ("menuLanguage", "Language"),
        };

        private static readonly (string menuItemName, ToolsetLanguage language)[] UiLanguageItems =
        {
            ("actionLangEnglish", ToolsetLanguage.English),
            ("actionLangFrench", ToolsetLanguage.French),
            ("actionLangGerman", ToolsetLanguage.German),
            ("actionLangItalian", ToolsetLanguage.Italian),
            ("actionLangSpanish", ToolsetLanguage.Spanish),
            ("actionLangPolish", ToolsetLanguage.Polish),
        };

        private ObservableCollection<ERFResourceViewModel> _sourceResources;
        private CollectionViewSource _filteredResources;
        private DataGrid _tableView;
        private Button _extractButton;
        private Button _loadButton;
        private Button _unloadButton;
        private Button _openButton;
        private Button _refreshButton;
        private TextBox _filterEdit;
        private TextBlock _statusText;
        private bool _hasChanges;

        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;

        private string _findText = "";
        private bool _findMatchCase;

        public OdyToolERF() : this(null, null) { }
        public OdyToolERF(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolERF", "none",
                new[] { ResourceType.RIM, ResourceType.ERF, ResourceType.MOD, ResourceType.SAV, ResourceType.HAK, ResourceType.BIF },
                new[] { ResourceType.RIM, ResourceType.ERF, ResourceType.MOD, ResourceType.SAV, ResourceType.HAK, ResourceType.BIF },
                installation)
        {
            _sourceResources = new ObservableCollection<ERFResourceViewModel>();
            _filteredResources = new CollectionViewSource { Source = _sourceResources };
            _hasChanges = false;

            InitializeComponent();
            if (_xamlLoaded)
            {
                SetupUI();
            }
            SetupSignals();
            SetupMenuHandlers();
            New();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
        }

        internal int ResourceCountForTest => _sourceResources.Count;
        internal IReadOnlyList<ERFResourceViewModel> ResourceRowsForTest => _sourceResources.ToList();
        internal Button ExtractButtonForTest => _extractButton;
        internal Button OpenButtonForTest => _openButton;
        internal Button RemoveButtonForTest => _unloadButton;
        internal Button RefreshButtonForTest => _refreshButton;

        internal void AddResourceFilePathsForTest(params string[] filePaths)
        {
            AddResourcesFromPaths(filePaths?.ToList());
        }

        internal void SelectResourceIndicesForTest(params int[] indices)
        {
            if (_tableView?.SelectedItems == null || indices == null)
            {
                return;
            }

            _tableView.SelectedItems.Clear();
            _tableView.SelectedItem = null;
            foreach (int index in indices)
            {
                if (index >= 0 && index < _sourceResources.Count)
                {
                    _tableView.SelectedItems.Add(_sourceResources[index]);
                }
            }
            if (indices.Length == 1 && indices[0] >= 0 && indices[0] < _sourceResources.Count)
            {
                _tableView.SelectedItem = _sourceResources[indices[0]];
            }
            OnSelectionChanged();
        }

        internal void RemoveSelectedForTest()
        {
            RemoveSelected();
        }

        internal void UndoForTest()
        {
            Undo();
        }

        internal bool RenameSelectedForTest(string newResRef, out string error)
        {
            var selected = GetSelectedViewModels();
            return TryRenameSelected(selected.Count == 1 ? selected[0] : null, newResRef, out error);
        }

        private bool _xamlLoaded = false;

        private void InitializeComponent()
        {
            try
            {
                AvaloniaXamlLoader.Load(this);
                _xamlLoaded = true;
            }
            catch
            {
                // XAML not available - will use programmatic UI
                _xamlLoaded = false;
            }

            if (!_xamlLoaded)
            {
                SetupProgrammaticUI();
            }
        }

        private void SetupProgrammaticUI()
        {
            var mainPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Button panel
            var buttonPanel = new StackPanel { Orientation = Orientation.Horizontal };
            _extractButton = new Button { Content = "Extract" };
            _loadButton = new Button { Content = "Add" };
            _unloadButton = new Button { Content = "Unload" };
            _openButton = new Button { Content = "Open" };
            _refreshButton = new Button { Content = "Refresh" };
            buttonPanel.Children.Add(_extractButton);
            buttonPanel.Children.Add(_loadButton);
            buttonPanel.Children.Add(_unloadButton);
            buttonPanel.Children.Add(_openButton);
            buttonPanel.Children.Add(_refreshButton);
            mainPanel.Children.Add(buttonPanel);

            // Table
            _tableView = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserReorderColumns = false,
                CanUserResizeColumns = true,
                SelectionMode = DataGridSelectionMode.Extended
            };
            _tableView.Columns.Add(new DataGridTextColumn
            {
                Header = "ResRef",
                Binding = new Binding("ResRef"),
                IsReadOnly = true
            });
            _tableView.Columns.Add(new DataGridTextColumn
            {
                Header = "Type",
                Binding = new Binding("Type"),
                IsReadOnly = true
            });
            _tableView.Columns.Add(new DataGridTextColumn
            {
                Header = "Size",
                Binding = new Binding("Size"),
                IsReadOnly = true
            });
            _tableView.Columns.Add(new DataGridTextColumn
            {
                Header = "Offset",
                Binding = new Binding("Offset"),
                IsReadOnly = true
            });
            _tableView.ItemsSource = _filteredResources.View;
            var ctx = new ContextMenu();
            MenuItem CreateContextItem(string header, Action onClick)
            {
                var item = new MenuItem { Header = header };
                item.Click += (s, e) => onClick();
                return item;
            }

            var extractItem = CreateContextItem("Extract to...", () => _ = RunExtractAsync());
            _ctxRename = CreateContextItem("Rename", () => _ = RenameSelectedAsync());
            var openItem = CreateContextItem("Open", OpenSelected);
            var removeItem = CreateContextItem("Remove", RemoveSelected);
            var findItem = CreateContextItem("Find...", ShowFindDialog);
            ctx.Items.Add(extractItem);
            ctx.Items.Add(_ctxRename);
            ctx.Items.Add(openItem);
            ctx.Items.Add(removeItem);
            ctx.Items.Add(new Separator());
            ctx.Items.Add(findItem);
            _tableView.ContextMenu = ctx;
            mainPanel.Children.Add(_tableView);
            _statusText = new TextBlock { Text = Localization.Trf("{0} resources", 0), Margin = new Avalonia.Thickness(4, 2) };
            mainPanel.Children.Add(_statusText);
            Content = mainPanel;
            UpdateActionButtonStates();
            RefreshLocalizedStrings();
        }

        private void SetupUI()
        {
            _tableView = EditorHelpers.FindControlSafe<DataGrid>(this, "tableView");
            _extractButton = EditorHelpers.FindControlSafe<Button>(this, "extractButton");
            _loadButton = EditorHelpers.FindControlSafe<Button>(this, "loadButton");
            _unloadButton = EditorHelpers.FindControlSafe<Button>(this, "unloadButton");
            _openButton = EditorHelpers.FindControlSafe<Button>(this, "openButton");
            _refreshButton = EditorHelpers.FindControlSafe<Button>(this, "refreshButton");
            _filterEdit = EditorHelpers.FindControlSafe<TextBox>(this, "filterEdit");
            _statusText = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
            if (_tableView != null)
                _tableView.ItemsSource = _filteredResources.View;
        }

        private void RefreshLocalizedStrings()
        {
            try
            {
                Title = Localization.Tr("OdyToolERF");
                var statusControl = _statusText ?? EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
                if (statusControl != null)
                    statusControl.Text = Localization.Trf("{0} resources", _sourceResources?.Count ?? 0) + (_tableView?.SelectedItems?.Count > 0 ? " " + Localization.Trf("| {0} selected", _tableView.SelectedItems.Count) : "");
                if (_filterEdit != null)
                    _filterEdit.Watermark = Localization.Tr("Filter by ResRef or Type…");
                if (_extractButton != null) { _extractButton.Content = Localization.Tr("Extract"); ToolTip.SetTip(_extractButton, Localization.Tr("Extract selected resources")); }
                if (_openButton != null) { _openButton.Content = Localization.Tr("Open"); ToolTip.SetTip(_openButton, Localization.Tr("Open selected resource")); }
                if (_loadButton != null) { _loadButton.Content = Localization.Tr("Add"); ToolTip.SetTip(_loadButton, Localization.Tr("Add resources to archive")); }
                if (_unloadButton != null) { _unloadButton.Content = Localization.Tr("Remove"); ToolTip.SetTip(_unloadButton, Localization.Tr("Remove selected resources")); }
                if (_refreshButton != null) { _refreshButton.Content = Localization.Tr("Reload"); ToolTip.SetTip(_refreshButton, Localization.Tr("Reload from disk")); }
                EditorHelpers.SetLocalizedMenuHeaders(this, MenuLocalizationItems);
                EditorHelpers.SetLocalizedParentMenuHeader(this, "actionNew", "File");
                EditorHelpers.SetLocalizedParentMenuHeader(this, "actionUndo", "Edit");
                EditorHelpers.SetLocalizedParentMenuHeader(this, "actionExtract", "Tools");
                if (_tableView?.Columns != null && _tableView.Columns.Count >= 4)
                {
                    _tableView.Columns[0].Header = Localization.Tr("ResRef");
                    _tableView.Columns[1].Header = Localization.Tr("Type");
                    _tableView.Columns[2].Header = Localization.Tr("Size");
                    _tableView.Columns[3].Header = Localization.Tr("Offset");
                }
            }
            catch { }
        }

        private void SetupSignals()
        {
            EditorHelpers.BindClick(_extractButton, () => _ = RunExtractAsync());
            EditorHelpers.BindClick(_loadButton, () => _ = RunAddFilesAsync());
            EditorHelpers.BindClick(_unloadButton, RemoveSelected);
            EditorHelpers.BindClick(_openButton, OpenSelected);
            EditorHelpers.BindClick(_refreshButton, () => _ = RunRefreshAsync());

            if (_tableView != null)
            {
                _tableView.SelectionChanged += (s, e) => { OnSelectionChanged(); UpdateStatusBar(); };
                _tableView.DoubleTapped += (s, e) => OpenSelected();
                _ctxRename = EditorHelpers.FindControlSafe<MenuItem>(this, "ctxRename");
                SetupTableViewDragDrop();
            }
            if (_filterEdit != null)
                _filterEdit.TextChanged += (s, e) => DoFilter(_filterEdit?.Text ?? "");
            Opened += (s, e) => { UpdateStatusBar(); _tableView?.Focus(); };
            KeyDown += OnWindowKeyDown;
        }

        protected override bool UseStandardFileMenuWiring => false;

        private void SetupMenuHandlers()
        {
            var menuHandlers = new (string menuItemName, Action handler)[]
            {
                ("actionNew", () => _ = RunNewAsync()),
                ("actionOpen", () => _ = RunOpenAsync()),
                ("actionSave", Save),
                ("actionSaveAs", () => _ = RunSaveAsAsync()),
                ("actionRevert", () => _ = RunRevertAsync()),
                ("actionExit", Close),
                ("actionUndo", Undo),
                ("actionRedo", Redo),
                ("actionFind", ShowFindDialog),
                ("actionFindNext", FindNextMatch),
                ("actionExtract", () => _ = RunExtractAsync()),
                ("actionOpenResource", OpenSelected),
                ("actionRemove", RemoveSelected),
                ("ctxExtract", () => _ = RunExtractAsync()),
                ("ctxRename", () => _ = RenameSelectedAsync()),
                ("ctxOpen", OpenSelected),
                ("ctxRemove", RemoveSelected),
                ("ctxFind", ShowFindDialog),
            };

            foreach (var (menuItemName, handler) in menuHandlers)
            {
                EditorHelpers.BindMenuClick(this, menuItemName, handler);
            }

            foreach (var (menuItemName, language) in UiLanguageItems)
            {
                BindLanguageMenu(menuItemName, language);
            }
        }

        private void BindLanguageMenu(string menuItemName, ToolsetLanguage language)
        {
            EditorHelpers.BindMenuClick(this, menuItemName, () =>
            {
                Localization.SetLanguage(language);
                RefreshLocalizedStrings();
            });
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

        public override void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            try
            {
                _undoStack.Clear();
                _redoStack.Clear();
                LoadFromBytes(_revert);
                _hasChanges = false;
                ClearDirty();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Revert failed: {ex}");
            }
        }

        protected override async Task RunSaveAsAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            string suggestedName = string.IsNullOrEmpty(_resname) ? "archive" : _resname;
            var ext = _restype != null ? _restype.Extension : "erf";
            var options = new FilePickerSaveOptions
            {
                Title = Localization.Tr("Save As"),
                SuggestedFileName = suggestedName + "." + ext,
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("ERF") { Patterns = new[] { "*.erf" } },
                    new FilePickerFileType("MOD") { Patterns = new[] { "*.mod" } },
                    new FilePickerFileType("RIM") { Patterns = new[] { "*.rim" } },
                    new FilePickerFileType("SAV") { Patterns = new[] { "*.sav" } },
                    new FilePickerFileType("HAK") { Patterns = new[] { "*.hak" } }
                }
            };
            var file = await storageProvider.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            string pathExt = System.IO.Path.GetExtension(path).TrimStart('.').ToLowerInvariant();
            if (pathExt == "mod") _restype = ResourceType.MOD;
            else if (pathExt == "rim") _restype = ResourceType.RIM;
            else if (pathExt == "sav") _restype = ResourceType.SAV;
            else if (pathExt == "hak") _restype = ResourceType.HAK;
            else _restype = ResourceType.ERF;
            RefreshWindowTitle();
            Save();
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            try
            {
                int count = _sourceResources?.Count ?? 0;
                int sel = _tableView?.SelectedItems?.Count ?? 0;
                string statusText = Localization.Trf("{0} resources", count);
                if (sel > 0) statusText += " " + Localization.Trf("| {0} selected", sel);
                var statusControl = _statusText ?? EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
                if (statusControl != null) statusControl.Text = statusText;
            }
            catch { }
        }

        public void DoFilter(string text)
        {
            if (_filteredResources?.View == null) return;
            string t = (text ?? "").Trim();
            if (string.IsNullOrEmpty(t))
                _filteredResources.View.Filter = null;
            else
            {
                bool matchCase = _findMatchCase;
                _filteredResources.View.Filter = item =>
                {
                    if (item is ERFResourceViewModel vm)
                        return (matchCase ? vm.ResRef : vm.ResRef?.ToLowerInvariant()).Contains(matchCase ? t : t.ToLowerInvariant())
                            || (matchCase ? vm.Type : vm.Type?.ToLowerInvariant()).Contains(matchCase ? t : t.ToLowerInvariant());
                    return false;
                };
            }
            _filteredResources.View.Refresh();
        }

        private void ShowFindDialog()
        {
            var dialog = new Window
            {
                Title = Localization.Tr("Find"),
                Width = 400,
                Height = 180,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var findBox = new TextBox { Watermark = Localization.Tr("Find what:"), Text = _findText, Margin = new Avalonia.Thickness(8) };
            var matchCase = new CheckBox { Content = Localization.Tr("Match case"), IsChecked = _findMatchCase, Margin = new Avalonia.Thickness(8) };
            var findNext = new Button { Content = Localization.Tr("Find Next"), Margin = new Avalonia.Thickness(8) };
            var closeBtn = new Button { Content = Localization.Tr("Close"), Margin = new Avalonia.Thickness(8) };
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
            if (string.IsNullOrEmpty(_findText) || _tableView == null)
            {
                _ = DialogHelper.ShowWindowAsync(this, Localization.Tr("Find"), Localization.Tr("Enter text to search for."), ButtonEnum.Ok, IconType.Info);
                return;
            }
            DoFilter(_findText);
            var list = _sourceResources.Where(r =>
                (_findMatchCase ? r.ResRef : r.ResRef?.ToLowerInvariant()).Contains(_findMatchCase ? _findText : _findText.ToLowerInvariant())
                || (_findMatchCase ? r.Type : r.Type?.ToLowerInvariant()).Contains(_findMatchCase ? _findText : _findText.ToLowerInvariant())).ToList();
            if (list.Count == 0)
            {
                _ = DialogHelper.ShowWindowAsync(this, Localization.Tr("Find"), Localization.Tr("No matches found."), ButtonEnum.Ok, IconType.Info);
                return;
            }
            var current = _tableView.SelectedItem as ERFResourceViewModel;
            int idx = current != null ? list.IndexOf(current) : -1;
            int next = (idx + 1) % list.Count;
            var sel = list[next];
            _tableView.SelectedItem = sel;
            _tableView.ScrollIntoView(sel, null);
        }

        private void SetupTableViewDragDrop()
        {
            if (_tableView == null) return;
            DragDrop.SetAllowDrop(_tableView, true);
            _tableView.AddHandler(DragDrop.DropEvent, OnTableViewDrop);
            _tableView.AddHandler(DragDrop.DragOverEvent, OnTableViewDragOver);
        }

        private void OnTableViewDragOver(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            var files = e.Data.GetFileNames();
            if (files != null && files.Any())
                e.DragEffects = DragDropEffects.Copy | DragDropEffects.Move;
        }

        private void OnTableViewDrop(object sender, DragEventArgs e)
        {
            if (e.Data == null) return;
            var files = e.Data.GetFileNames();
            if (files == null) return;
            var paths = files.ToList();
            if (paths.Count == 0) return;
            AddResourcesFromPaths(paths);
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (e.KeyModifiers & KeyModifiers.Control) != 0) { Save(); e.Handled = true; return; }
            if (e.Key == Key.Z && (e.KeyModifiers & KeyModifiers.Control) != 0) { Undo(); e.Handled = true; return; }
            if (e.Key == Key.Y && (e.KeyModifiers & KeyModifiers.Control) != 0) { Redo(); e.Handled = true; return; }
            if (e.Key == Key.F && (e.KeyModifiers & KeyModifiers.Control) != 0) { ShowFindDialog(); e.Handled = true; return; }
            if (e.Key == Key.F3) { FindNextMatch(); e.Handled = true; return; }
            if (e.Key == Key.Delete && _tableView?.SelectedItems?.Count > 0) { RemoveSelected(); e.Handled = true; }
        }

        private void LoadFromBytes(byte[] data)
        {
            _sourceResources.Clear();
            ResourceType restype = _restype ?? ResourceType.ERF;
            if (restype == ResourceType.RIM)
            {
                var rim = RIMAuto.ReadRim(data);
                int offset = 0;
                foreach (var resource in rim)
                {
                    _sourceResources.Add(new ERFResourceViewModel
                    {
                        ResRef = resource.ResRef.ToString(),
                        Type = resource.ResType.Extension.ToUpper(),
                        Size = HumanReadableSize(resource.Data.Length),
                        Offset = $"0x{offset:X}",
                        ErfResource = null,
                        RimResource = resource,
                        BifResource = null
                    });
                    offset += resource.Data.Length;
                }
            }
            else if (restype == ResourceType.ERF || restype == ResourceType.MOD || restype == ResourceType.SAV || restype == ResourceType.HAK)
            {
                var erf = ERFAuto.ReadErf(data);
                int offset = 0;
                foreach (var resource in erf)
                {
                    _sourceResources.Add(new ERFResourceViewModel
                    {
                        ResRef = resource.ResRef.ToString(),
                        Type = resource.ResType.Extension.ToUpper(),
                        Size = HumanReadableSize(resource.Data.Length),
                        Offset = $"0x{offset:X}",
                        ErfResource = resource,
                        RimResource = null,
                        BifResource = null
                    });
                    offset += resource.Data.Length;
                }
            }
            else if (restype == ResourceType.BIF)
            {
                var bif = new BIFBinaryReader(data).Load();
                foreach (var resource in bif)
                {
                    string resref = string.IsNullOrWhiteSpace(resource.ResRef.ToString())
                        ? "id_" + resource.ResnameKeyIndex
                        : resource.ResRef.ToString();
                    _sourceResources.Add(new ERFResourceViewModel
                    {
                        ResRef = resref,
                        Type = resource.ResType.Extension.ToUpper(),
                        Size = HumanReadableSize(resource.Data.Length),
                        Offset = $"0x{resource.Offset:X}",
                        ErfResource = null,
                        RimResource = null,
                        BifResource = resource
                    });
                }
            }
            UpdateActionButtonStates();
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            if (_hasChanges)
            {
                _ = ConfirmThenLoadAsync(filepath, resref, restype, data);
                return;
            }
            LoadCore(filepath, resref, restype, data);
        }

        private async Task ConfirmThenLoadAsync(string filepath, string resref, ResourceType restype, byte[] data)
        {
            if (!await PromptConfirmAsync())
                return;
            LoadCore(filepath, resref, restype, data);
        }

        private void LoadCore(string filepath, string resref, ResourceType restype, byte[] data)
        {
            if (restype != ResourceType.RIM && restype != ResourceType.ERF && restype != ResourceType.MOD && restype != ResourceType.SAV && restype != ResourceType.HAK && restype != ResourceType.BIF)
            {
                _ = DialogHelper.ShowWindowAsync(this, Localization.Tr("Unable to load file"), Localization.Tr("The file specified is not a supported archive type."), ButtonEnum.Ok, IconType.Error);
                return;
            }
            _hasChanges = false;
            ClearDirty();
            _undoStack.Clear();
            _redoStack.Clear();
            base.Load(filepath, resref, restype, data);
            try
            {
                LoadFromBytes(data);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load file: {ex}");
                New();
            }
        }

        public override Tuple<byte[], byte[]> Build()
        {
            // If restype is not set (e.g., after New()), default to ERF
            ResourceType restype = _restype ?? ResourceType.ERF;

            if (restype == ResourceType.RIM)
            {
                var rim = new RIM();
                foreach (var viewModel in _sourceResources)
                {
                    if (viewModel.RimResource != null)
                        rim.SetData(viewModel.RimResource.ResRef.ToString(), viewModel.RimResource.ResType, viewModel.RimResource.Data);
                    else if (viewModel.ErfResource != null)
                        rim.SetData(viewModel.ErfResource.ResRef.ToString(), viewModel.ErfResource.ResType, viewModel.ErfResource.Data);
                    else if (viewModel.BifResource != null)
                        rim.SetData(viewModel.BifResource.ResRef.ToString(), viewModel.BifResource.ResType, viewModel.BifResource.Data);
                }
                byte[] data = RIMAuto.BytesRim(rim);
                return Tuple.Create(data, new byte[0]);
            }
            else if (restype == ResourceType.ERF || restype == ResourceType.MOD || restype == ResourceType.SAV || restype == ResourceType.HAK)
            {
                ERFType erfType = ERFTypeExtensions.FromExtension(restype.Extension);
                var erf = new ERF(erfType);
                if (restype == ResourceType.SAV)
                {
                    erf.IsSaveErf = true;
                }
                foreach (var viewModel in _sourceResources)
                {
                    if (viewModel.ErfResource != null)
                        erf.SetData(viewModel.ErfResource.ResRef.ToString(), viewModel.ErfResource.ResType, viewModel.ErfResource.Data);
                    else if (viewModel.RimResource != null)
                        erf.SetData(viewModel.RimResource.ResRef.ToString(), viewModel.RimResource.ResType, viewModel.RimResource.Data);
                    else if (viewModel.BifResource != null)
                        erf.SetData(viewModel.BifResource.ResRef.ToString(), viewModel.BifResource.ResType, viewModel.BifResource.Data);
                }
                byte[] data = ERFAuto.BytesErf(erf, restype);
                return Tuple.Create(data, new byte[0]);
            }
            else if (restype == ResourceType.BIF)
            {
                var bif = new BIF(BIFType.BIF);
                int nextId = 0;
                foreach (var viewModel in _sourceResources)
                {
                    if (viewModel.BifResource != null)
                    {
                        bif.SetData(viewModel.BifResource.ResRef, viewModel.BifResource.ResType, viewModel.BifResource.Data, viewModel.BifResource.ResnameKeyIndex);
                        nextId = Math.Max(nextId, viewModel.BifResource.ResnameKeyIndex + 1);
                    }
                    else if (viewModel.ErfResource != null)
                    {
                        bif.SetData(viewModel.ErfResource.ResRef, viewModel.ErfResource.ResType, viewModel.ErfResource.Data, nextId++);
                    }
                    else if (viewModel.RimResource != null)
                    {
                        bif.SetData(viewModel.RimResource.ResRef, viewModel.RimResource.ResType, viewModel.RimResource.Data, nextId++);
                    }
                }
                byte[] data = new BIFBinaryWriter(bif).Write();
                return Tuple.Create(data, new byte[0]);
            }
            else
            {
                throw new InvalidOperationException($"Invalid restype for OdyToolERF: {restype}");
            }
        }

        public override void New()
        {
            _hasChanges = false;
            ClearDirty();
            _undoStack.Clear();
            _redoStack.Clear();
            base.New();
            _restype = ResourceType.ERF;
            _sourceResources.Clear();
            UpdateActionButtonStates();
            UpdateStatusBar();
        }

        public override void Save()
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                SaveAs();
                return;
            }

            if (_refreshButton != null)
                _refreshButton.IsEnabled = true;

            base.Save();
            _hasChanges = false;
        }

        private async Task RunExtractAsync()
        {
            var selected = GetSelectedViewModels();
            if (selected.Count == 0)
            {
                await DialogHelper.ShowWindowAsync(this, Localization.Tr("Extract"), Localization.Tr("No resources selected. Select one or more resources to extract."), ButtonEnum.Ok, IconType.Info);
                return;
            }

            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            var options = new FolderPickerOpenOptions
            {
                Title = Localization.Tr("Extract to folder"),
                AllowMultiple = false
            };
            var folders = await storageProvider.OpenFolderPickerAsync(options);
            if (folders == null || folders.Count == 0) return;
            string folderPath = folders[0].Path.LocalPath;
            if (string.IsNullOrWhiteSpace(folderPath)) return;

            var extractOptions = await PromptExtractOptionsAsync(selected);
            if (extractOptions == null)
            {
                return;
            }

            int successCount = 0;
            var failed = new List<string>();
            foreach (var vm in selected)
            {
                string resref = vm.ResRef ?? "resource";
                var restype = GetResourceType(vm);
                string ext = restype?.Extension ?? (vm.Type ?? "bin").ToLowerInvariant();
                try
                {
                    var extractedFiles = ExtractResourceFilesForSave(
                        vm,
                        selected,
                        extractOptions.TpcDecompile,
                        extractOptions.TpcExtractTxi,
                        extractOptions.NcsDecompile,
                        extractOptions.MdlExtractTextures,
                        Path.Combine(folderPath, $"{resref}.{ext}"),
                        extractOptions.MdlDecompile);

                    if (extractedFiles.Count == 0)
                    {
                        failed.Add($"{resref}.{ext}");
                        continue;
                    }

                    foreach (var extracted in extractedFiles)
                    {
                        File.WriteAllBytes(extracted.Path, extracted.Data);
                        successCount++;
                    }
                }
                catch (Exception ex)
                {
                    failed.Add($"{resref}.{ext}: {ex.Message}");
                }
            }

            string message = successCount > 0
                ? Localization.Trf("Extracted {0} resource(s) to:\n{1}", successCount, folderPath)
                : Localization.Tr("No files were extracted.");
            if (failed.Count > 0)
                message += "\n\nFailed: " + DialogHelper.BuildTruncatedList(failed, 5);
            var resultIcon = successCount > 0 ? IconType.Info : IconType.Error;
            await DialogHelper.ShowWindowAsync(this, Localization.Tr("Extract"), message, ButtonEnum.Ok, resultIcon);
        }

        private async Task<ArchiveExtractOptions> PromptExtractOptionsAsync(IReadOnlyList<ERFResourceViewModel> selected)
        {
            var options = ArchiveExtractOptions.Default;
            if (!selected.Any(IsConvertibleForExtractOptions))
            {
                return options;
            }

            var dialog = new ExtractOptionsDialog(this)
            {
                TpcDecompile = options.TpcDecompile,
                TpcExtractTxi = options.TpcExtractTxi,
                MdlDecompile = options.MdlDecompile,
                MdlExtractTextures = options.MdlExtractTextures
            };

            await dialog.ShowDialog(this);
            if (dialog.Result != true)
            {
                return null;
            }

            return new ArchiveExtractOptions
            {
                TpcDecompile = dialog.TpcDecompile,
                TpcExtractTxi = dialog.TpcExtractTxi,
                NcsDecompile = options.NcsDecompile,
                MdlExtractTextures = dialog.MdlExtractTextures,
                MdlDecompile = dialog.MdlDecompile
            };
        }

        private static bool IsConvertibleForExtractOptions(ERFResourceViewModel vm)
        {
            var restype = GetResourceType(vm);
            return restype == ResourceType.TPC
                || restype == ResourceType.NCS
                || restype == ResourceType.MDL
                || restype == ResourceType.MDL_ASCII;
        }

        internal static List<(string Path, byte[] Data)> ExtractResourceFilesForSave(
            ERFResourceViewModel vm,
            IReadOnlyList<ERFResourceViewModel> archiveResources,
            bool decompileTpc,
            bool extractTxi,
            bool decompileNcs,
            bool extractMdlTextures,
            string savePath,
            bool decompileMdl = false)
        {
            if (vm == null)
            {
                throw new ArgumentNullException(nameof(vm));
            }

            var data = GetResourceData(vm);
            if (data == null)
            {
                return new List<(string Path, byte[] Data)>();
            }

            var restype = GetResourceType(vm);
            var extension = ResourceFileExtension(restype);
            var results = new List<(string Path, byte[] Data)>();

            if (restype == ResourceType.TPC)
            {
                try
                {
                    var tpc = TPCAuto.ReadTpc(data);
                    results.Add(decompileTpc
                        ? (Path.ChangeExtension(savePath, ".tga"), TPCAuto.BytesTpc(tpc, ResourceType.TGA))
                        : (ChangeResourceExtension(savePath, extension), data));

                    var txiText = ResolveArchiveTxiText(vm.ResRef, tpc, archiveResources);
                    if (extractTxi && !string.IsNullOrWhiteSpace(txiText))
                    {
                        results.Add((Path.ChangeExtension(savePath, ".txi"), Encoding.ASCII.GetBytes(txiText)));
                    }

                    return results;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Failed to decompile archive TPC {vm.ResRef}: {ex.Message}");
                    results.Add((ChangeResourceExtension(savePath, extension), data));
                    return results;
                }
            }

            if (restype == ResourceType.NCS && decompileNcs)
            {
                try
                {
                    var source = ScriptDecompiler.HtDecompileScript(data, null, false);
                    results.Add((Path.ChangeExtension(savePath, ".nss"), Encoding.UTF8.GetBytes(source ?? string.Empty)));
                    return results;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Failed to decompile archive NCS {vm.ResRef}: {ex.Message}");
                    var diagnostic = "// Decompile failed: " + ex.Message + Environment.NewLine;
                    results.Add((Path.ChangeExtension(savePath, ".nss"), Encoding.UTF8.GetBytes(diagnostic)));
                    return results;
                }
            }

            if ((restype == ResourceType.MDL || restype == ResourceType.MDL_ASCII) && (decompileMdl || extractMdlTextures))
            {
                if (decompileMdl)
                {
                    results.Add((ChangeResourceExtension(savePath, ".mdl.ascii"), DecompileArchiveMdl(vm, data, archiveResources)));
                }
                else
                {
                    results.Add((ChangeResourceExtension(savePath, extension), data));
                }

                AddArchiveModelTextures(results, vm, data, archiveResources, savePath);
                return results;
            }

            results.Add((ChangeResourceExtension(savePath, extension), data));
            return results;
        }

        private static void AddArchiveModelTextures(
            List<(string Path, byte[] Data)> results,
            ERFResourceViewModel vm,
            byte[] data,
            IReadOnlyList<ERFResourceViewModel> archiveResources,
            string savePath)
        {
            try
            {
                var restype = GetResourceType(vm);
                var mdl = restype == ResourceType.MDL_ASCII
                    ? MDLAuto.ReadMdl(data, fileFormat: ResourceType.MDL_ASCII)
                    : MDLAuto.ReadMdl(data, sourceExt: ResolveArchiveResourceData(archiveResources, vm.ResRef, ResourceType.MDX), fileFormat: ResourceType.MDL);

                var textureNames = mdl.AllTextures()
                    .Concat(mdl.AllLightmaps())
                    .Where(name => !string.IsNullOrWhiteSpace(name))
                    .Where(name => !string.Equals(name, "NULL", StringComparison.OrdinalIgnoreCase))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(name => name, StringComparer.OrdinalIgnoreCase);

                var directory = Path.GetDirectoryName(savePath);
                foreach (var textureName in textureNames)
                {
                    var texture = ResolveArchiveResource(archiveResources, textureName, ResourceType.TPC)
                        ?? ResolveArchiveResource(archiveResources, textureName, ResourceType.TGA);
                    var textureData = GetResourceData(texture);
                    var textureType = GetResourceType(texture);
                    if (textureData == null || textureData.Length == 0 || textureType == null || textureType.IsInvalid)
                    {
                        continue;
                    }

                    var filename = textureName + "." + textureType.Extension;
                    var texturePath = string.IsNullOrWhiteSpace(directory)
                        ? filename
                        : Path.Combine(directory, filename);
                    results.Add((texturePath, textureData));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to extract archive MDL textures for {vm.ResRef}: {ex.Message}");
            }
        }

        private static byte[] DecompileArchiveMdl(
            ERFResourceViewModel vm,
            byte[] data,
            IReadOnlyList<ERFResourceViewModel> archiveResources)
        {
            var restype = GetResourceType(vm);
            if (restype == ResourceType.MDL_ASCII)
            {
                return data;
            }

            var mdl = MDLAuto.ReadMdl(data, sourceExt: ResolveArchiveResourceData(archiveResources, vm.ResRef, ResourceType.MDX), fileFormat: ResourceType.MDL);
            return MDLAuto.BytesMdl(mdl, ResourceType.MDL_ASCII);
        }

        private static string ResolveArchiveTxiText(string resref, TPC tpc, IReadOnlyList<ERFResourceViewModel> archiveResources)
        {
            if (!string.IsNullOrWhiteSpace(tpc?.Txi))
            {
                return tpc.Txi;
            }

            var txiData = ResolveArchiveResourceData(archiveResources, resref, ResourceType.TXI);
            return txiData == null || txiData.Length == 0
                ? string.Empty
                : Encoding.ASCII.GetString(txiData);
        }

        private static byte[] ResolveArchiveResourceData(IReadOnlyList<ERFResourceViewModel> archiveResources, string resref, ResourceType restype)
        {
            return GetResourceData(ResolveArchiveResource(archiveResources, resref, restype));
        }

        private static ERFResourceViewModel ResolveArchiveResource(IReadOnlyList<ERFResourceViewModel> archiveResources, string resref, ResourceType restype)
        {
            if (archiveResources == null || string.IsNullOrWhiteSpace(resref) || restype == null)
            {
                return null;
            }

            return archiveResources.FirstOrDefault(candidate =>
                string.Equals(candidate?.ResRef, resref, StringComparison.OrdinalIgnoreCase)
                && GetResourceType(candidate) == restype);
        }

        private static string ChangeResourceExtension(string path, string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
            {
                return path;
            }

            if (!extension.StartsWith(".", StringComparison.Ordinal))
            {
                extension = "." + extension;
            }

            return path.EndsWith(extension, StringComparison.OrdinalIgnoreCase)
                ? path
                : Path.ChangeExtension(path, extension);
        }

        private static string ResourceFileExtension(ResourceType resourceType)
        {
            var extension = resourceType?.Extension ?? string.Empty;
            if (string.IsNullOrWhiteSpace(extension) || extension.StartsWith(".", StringComparison.Ordinal))
            {
                return extension;
            }

            return "." + extension;
        }

        private static byte[] GetResourceData(ERFResourceViewModel vm)
        {
            if (vm == null) return null;
            if (vm.ErfResource != null) return vm.ErfResource.Data;
            if (vm.RimResource != null) return vm.RimResource.Data;
            if (vm.BifResource != null) return vm.BifResource.Data;
            return null;
        }

        private static ResourceType GetResourceType(ERFResourceViewModel vm)
        {
            if (vm == null)
            {
                return ResourceType.INVALID;
            }

            if (vm.ErfResource != null) return vm.ErfResource.ResType;
            if (vm.RimResource != null) return vm.RimResource.ResType;
            if (vm.BifResource != null) return vm.BifResource.ResType;
            return ResourceType.FromExtension("." + (vm.Type ?? "bin").ToLowerInvariant());
        }

        private sealed class ArchiveExtractOptions
        {
            public bool TpcDecompile { get; set; }
            public bool TpcExtractTxi { get; set; }
            public bool NcsDecompile { get; set; }
            public bool MdlExtractTextures { get; set; }
            public bool MdlDecompile { get; set; }

            public static ArchiveExtractOptions Default => new ArchiveExtractOptions
            {
                TpcDecompile = true,
                TpcExtractTxi = false,
                NcsDecompile = true,
                MdlExtractTextures = false,
                MdlDecompile = false
            };
        }

        private async Task RunAddFilesAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            var options = new FilePickerOpenOptions
            {
                Title = Localization.Tr("Load files into archive"),
                AllowMultiple = true,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType(Localization.Tr("Resource files")) { Patterns = new[] { "*.*" } }
                }
            };
            var files = await storageProvider.OpenFilePickerAsync(options);
            if (files == null || files.Count == 0) return;
            AddResourcesFromPaths(files.Select(f => f.Path.LocalPath).ToList());
        }

        private void RemoveSelected()
        {
            var selected = GetSelectedViewModels();
            if (selected.Count == 0) return;
            PushState();
            _hasChanges = true;
            MarkDocumentDirty();
            foreach (var item in selected)
                _sourceResources.Remove(item);
            UpdateStatusBar();
        }

        private void OpenSelected()
        {
            var selected = GetSelectedViewModels();
            if (selected.Count == 0)
            {
                _ = DialogHelper.ShowWindowAsync(this, Localization.Tr("Open"), Localization.Tr("No resources selected. Select one or more resources to open."), ButtonEnum.Ok, IconType.Info);
                return;
            }
            if (string.IsNullOrEmpty(_filepath))
            {
                _ = DialogHelper.ShowWindowAsync(this, Localization.Tr("Cannot edit resource"), Localization.Tr("This archive must be saved to disk first. Save the file and try again."), ButtonEnum.Ok, IconType.Error);
                return;
            }
            // Build list of resources to open (capture data now; opening is deferred)
            var toOpen = new List<(string resname, ResourceType restype, byte[] data)>();
            foreach (var vm in selected)
            {
                string resname = vm.ResRef ?? "resource";
                ResourceType restype = ResourceType.FromExtension("." + (vm.Type ?? "bin").ToLowerInvariant()) ?? ResourceType.INVALID;
                byte[] data = GetResourceData(vm);
                if (data == null || restype == null || restype.IsInvalid) continue;
                toOpen.Add((resname, restype, data));
            }
            if (toOpen.Count == 0)
            {
                _ = DialogHelper.ShowWindowAsync(this, Localization.Tr("Open"), Localization.Tr("No valid resources could be opened (missing data or unknown type)."), ButtonEnum.Ok, IconType.Warning);
                return;
            }
            string filepath = _filepath;
            var installation = _installation;
            var parentWindow = this as Window;
            // Defer opening to next UI tick to avoid re-entrancy; chain so each resource opens in its own tick (UI stays responsive)
            void OpenNextResource(int index)
            {
                if (index >= toOpen.Count) return;
                var (resname, restype, data) = toOpen[index];
                try
                {
                    WindowUtils.OpenResourceEditor(filepath, resname, restype, data, installation, parentWindow);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error opening resource {resname}: {ex}");
                    _ = DialogHelper.ShowAsync(Localization.Tr("Open failed"), Localization.Trf("Could not open {0}: {1}", resname + "." + (restype?.Extension ?? "?"), ex.Message), ButtonEnum.Ok, IconType.Error);
                }
                if (index + 1 < toOpen.Count)
                    Dispatcher.UIThread.Post(() => OpenNextResource(index + 1), DispatcherPriority.Loaded);
            }

            Dispatcher.UIThread.Post(() => OpenNextResource(0), DispatcherPriority.Loaded);
        }

        private void AddResourcesFromPaths(List<string> filePaths)
        {
            if (filePaths == null || filePaths.Count == 0) return;
            ResourceType restype = _restype ?? ResourceType.ERF;
            bool isRim = restype == ResourceType.RIM;
            int added = 0;
            var errors = new List<string>();
            PushState();
            foreach (string filepath in filePaths)
            {
                try
                {
                    var ident = ResourceIdentifier.FromPath(filepath).Validate();
                    string resname = ident.ResName;
                    ResourceType resType = ident.ResType;
                    if (resType == null || resType.IsInvalid) { errors.Add($"{Path.GetFileName(filepath)}: invalid type"); continue; }
                    byte[] data = File.ReadAllBytes(filepath);
                    var vm = new ERFResourceViewModel
                    {
                        ResRef = resname,
                        Type = resType.Extension.ToUpper().TrimStart('.'),
                        Size = HumanReadableSize(data.Length),
                        Offset = "—",
                        ErfResource = isRim ? null : new ERFResource(new ResRef(resname), resType, data),
                        RimResource = isRim ? new RIMResource(new ResRef(resname), resType, data) : null,
                        BifResource = null
                    };
                    _sourceResources.Add(vm);
                    added++;
                }
                catch (Exception ex)
                {
                    errors.Add($"{Path.GetFileName(filepath)}: {ex.Message}");
                }
            }
            if (added > 0)
            {
                _hasChanges = true;
                MarkDocumentDirty();
                UpdateActionButtonStates();
                UpdateStatusBar();
            }
            else if (_undoStack.Count > 0)
            {
                _undoStack.RemoveAt(_undoStack.Count - 1);
            }
            if (errors.Count > 0)
            {
                string msg = Localization.Trf("Added {0} resource(s).\nFailed to add:\n", added) + DialogHelper.BuildTruncatedList(errors, 10);
                _ = DialogHelper.ShowWindowAsync(this, Localization.Tr("Add resources"), msg, ButtonEnum.Ok, IconType.Warning);
            }
        }

        private void Refresh()
        {
            if (string.IsNullOrEmpty(_filepath)) return;
            _hasChanges = false;
            ClearDirty();
            byte[] data = File.ReadAllBytes(_filepath);
            Load(_filepath, _resname, _restype, data);
        }

        private async Task RunRefreshAsync()
        {
            if (_hasChanges && !await PromptConfirmAsync()) return;
            if (string.IsNullOrEmpty(_filepath))
            {
                await DialogHelper.ShowWindowAsync(this, Localization.Tr("Nothing to refresh"), Localization.Tr("This archive was not loaded from a file, so there is nothing to refresh."), ButtonEnum.Ok, IconType.Info);
                return;
            }
            Refresh();
        }

        private MenuItem _ctxRename;

        private void OnSelectionChanged()
        {
            UpdateActionButtonStates();
        }

        private void UpdateActionButtonStates()
        {
            int count = _tableView?.SelectedItems?.Count ?? 0;
            bool hasSelection = count > 0;
            bool singleSelection = count == 1;
            if (_extractButton != null) _extractButton.IsEnabled = hasSelection;
            if (_openButton != null) _openButton.IsEnabled = hasSelection;
            if (_unloadButton != null) _unloadButton.IsEnabled = hasSelection;
            if (_refreshButton != null) _refreshButton.IsEnabled = CanRefreshArchive();
            if (_ctxRename != null) _ctxRename.IsEnabled = singleSelection;
        }

        private bool CanRefreshArchive()
        {
            return _sourceResources.Count > 0 || !string.IsNullOrEmpty(_filepath);
        }

        private List<ERFResourceViewModel> GetSelectedViewModels()
        {
            return _tableView?.SelectedItems?.Cast<ERFResourceViewModel>().ToList() ?? new List<ERFResourceViewModel>();
        }

        /// <summary>Returns true if user chose to discard changes (proceed); false to cancel.</summary>
        private async Task<bool> PromptConfirmAsync()
        {
            var result = await DialogHelper.ShowWindowAsync(this, Localization.Tr("Changes detected"), Localization.Tr("The action you attempted would discard your changes. Continue?"), ButtonEnum.YesNo, IconType.Question);
            return result == ButtonResult.Yes;
        }

        protected override async Task RunOpenAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            var options = new FilePickerOpenOptions
            {
                Title = Localization.Tr("Open ERF / RIM / MOD / SAV"),
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType(Localization.Tr("ERF")) { Patterns = new[] { "*.erf" } },
                    new FilePickerFileType(Localization.Tr("MOD")) { Patterns = new[] { "*.mod" } },
                    new FilePickerFileType(Localization.Tr("RIM")) { Patterns = new[] { "*.rim" } },
                    new FilePickerFileType(Localization.Tr("SAV")) { Patterns = new[] { "*.sav" } },
                    new FilePickerFileType("HAK") { Patterns = new[] { "*.hak" } },
                    new FilePickerFileType(Localization.Tr("All supported")) { Patterns = new[] { "*.erf", "*.mod", "*.rim", "*.sav", "*.hak" } }
                }
            };
            var files = await storageProvider.OpenFilePickerAsync(options);
            if (files == null || files.Count == 0) return;
            string path = files[0].Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;
            try
            {
                byte[] data = File.ReadAllBytes(path);
                string resname = Path.GetFileNameWithoutExtension(path);
                string pathExt = Path.GetExtension(path).TrimStart('.').ToLowerInvariant();
                ResourceType restype;
                if (pathExt == "mod") restype = ResourceType.MOD;
                else if (pathExt == "rim") restype = ResourceType.RIM;
                else if (pathExt == "sav") restype = ResourceType.SAV;
                else if (pathExt == "hak") restype = ResourceType.HAK;
                else restype = ResourceType.ERF;
                Load(path, resname, restype, data);
            }
            catch (Exception ex)
            {
                await DialogHelper.ShowWindowAsync(this, Localization.Tr("Open failed"), Localization.Trf("Could not open file: {0}", ex.Message), ButtonEnum.Ok, IconType.Error);
            }
        }

        private async Task RunNewAsync()
        {
            if (_hasChanges && !await PromptConfirmAsync()) return;
            New();
        }

        private async Task RunRevertAsync()
        {
            if (_hasChanges && !await PromptConfirmAsync()) return;
            Revert();
        }

        private static string HumanReadableSize(double byteSize)
        {
            string[] units = { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            foreach (string unit in units)
            {
                if (byteSize < 1024)
                {
                    return $"{Math.Round(byteSize, 2)} {unit}";
                }
                byteSize /= 1024;
            }
            return byteSize.ToString();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        private async Task RenameSelectedAsync()
        {
            var selected = GetSelectedViewModels();
            if (selected.Count != 1)
                return;
            var vm = selected[0];
            bool isRim = vm.RimResource != null;
            string currentResRef = vm.ResRef ?? "";
            string archiveName = string.IsNullOrEmpty(_resname) || _restype == null
                ? "ERF/RIM"
                : $"{_resname}.{_restype.Extension}";

            var dialog = new Window
            {
                Title = Localization.Trf("Rename {0} resource", archiveName),
                Width = 380,
                Height = 140,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var label = new TextBlock { Text = Localization.Trf("Enter new ResRef (current: {0}):", currentResRef), Margin = new Avalonia.Thickness(8, 8, 8, 2) };
            var input = new TextBox { Text = currentResRef, Watermark = Localization.Tr("ResRef (max 16 ASCII chars)"), Margin = new Avalonia.Thickness(8, 2), MaxLength = 16 };
            var okBtn = new Button { Content = Localization.Tr("OK"), Margin = new Avalonia.Thickness(8), MinWidth = 70 };
            var cancelBtn = new Button { Content = Localization.Tr("Cancel"), Margin = new Avalonia.Thickness(8), MinWidth = 70 };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(10) };
            panel.Children.Add(label);
            panel.Children.Add(input);
            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal };
            btnPanel.Children.Add(okBtn);
            btnPanel.Children.Add(cancelBtn);
            panel.Children.Add(btnPanel);
            dialog.Content = panel;

            bool accepted = false;
            okBtn.Click += (s, e) =>
            {
                string newResRef = (input.Text ?? "").Trim();
                if (!ResRef.IsValid(newResRef))
                {
                    _ = DialogHelper.ShowWindowAsync(dialog, Localization.Tr("Invalid ResRef"), Localization.Tr("ResRefs must be ASCII, max 16 characters, and not contain <>:\"/\\|?*."), ButtonEnum.Ok, IconType.Warning);
                    return;
                }
                accepted = true;
                dialog.Close();
            };
            cancelBtn.Click += (s, e) => dialog.Close();
            dialog.Opened += (s, e) => input.Focus();
            await dialog.ShowDialog(this);
            if (!accepted) return;

            string newName = (input.Text ?? "").Trim();
            if (!TryRenameSelected(vm, newName, out string error) && !string.IsNullOrEmpty(error))
            {
                await DialogHelper.ShowWindowAsync(this, Localization.Tr("Rename failed"), error, ButtonEnum.Ok, IconType.Warning);
            }
        }

        private bool TryRenameSelected(ERFResourceViewModel vm, string newResRef, out string error)
        {
            error = null;
            if (vm == null)
            {
                error = Localization.Tr("Select exactly one resource to rename.");
                return false;
            }

            string newName = (newResRef ?? "").Trim();
            if (!ResRef.IsValid(newName))
            {
                error = Localization.Tr("ResRefs must be ASCII, max 16 characters, and not contain <>:\"/\\|?*.");
                return false;
            }

            ResourceType type = GetResourceType(vm);
            if (type == null || type.IsInvalid)
            {
                error = Localization.Tr("Cannot rename resource with unknown type.");
                return false;
            }

            if (string.Equals(vm.ResRef, newName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            bool duplicate = _sourceResources.Any(resource =>
                !ReferenceEquals(resource, vm) &&
                string.Equals(resource.ResRef, newName, StringComparison.OrdinalIgnoreCase) &&
                Equals(GetResourceType(resource), type));
            if (duplicate)
            {
                error = Localization.Trf("The archive already contains {0}.{1}.", newName, type.Extension);
                return false;
            }

            PushState();
            _hasChanges = true;
            MarkDocumentDirty();
            SetResourceResRef(vm, newName);
            _filteredResources?.View?.Refresh();
            UpdateStatusBar();
            return true;
        }

        private static void SetResourceResRef(ERFResourceViewModel vm, string newName)
        {
            var newRef = new ResRef(newName);
            if (vm.ErfResource != null)
            {
                vm.ErfResource.ResRef = newRef;
            }
            else if (vm.RimResource != null)
            {
                vm.RimResource.ResRef = newRef;
            }
            else if (vm.BifResource != null)
            {
                vm.BifResource.ResRef = newRef;
            }

            vm.ResRef = newName;
        }
    }

    // ViewModel for ERF resources
    public class ERFResourceViewModel
    {
        public string ResRef { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public string Offset { get; set; }
        public ERFResource ErfResource { get; set; }
        public RIMResource RimResource { get; set; }
        public BIFResource BifResource { get; set; }
    }
}
