using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using BioWare;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.RIM;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Common;
using OdyTools.Data;
using ERFResource = BioWare.Resource.Formats.ERF.ERFResource;
using RIMResource = BioWare.Resource.Formats.RIM.RIMResource;

namespace OdyTools.Editors
{
    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:97
    // Original: class OdyToolERF(Editor):
    public partial class OdyToolERF : Editor
    {
        private const int MinEditorWidth = 400;
        private const int MinEditorHeight = 369;
        private const int UndoMaxLevels = 30;

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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:98-146
        // Original: def __init__(self, parent, installation):
        public OdyToolERF(Window parent = null, OdyInstallation installation = null)
            : base(parent, "ERF Editor", "none",
                new[] { ResourceType.RIM, ResourceType.ERF, ResourceType.MOD, ResourceType.SAV, ResourceType.BIF },
                new[] { ResourceType.RIM, ResourceType.ERF, ResourceType.MOD, ResourceType.SAV, ResourceType.BIF },
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
            _loadButton = new Button { Content = "Load" };
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
            var ctx = new ContextMenu();
            var extractItem = new MenuItem { Header = "Extract" };
            extractItem.Click += (s, e) => ExtractSelected();
            var openItem = new MenuItem { Header = "Open" };
            openItem.Click += (s, e) => OpenSelected();
            var removeItem = new MenuItem { Header = "Remove" };
            removeItem.Click += (s, e) => RemoveSelected();
            var findItem = new MenuItem { Header = "Find..." };
            findItem.Click += (s, e) => ShowFindDialog();
            ctx.Items.Add(extractItem);
            ctx.Items.Add(openItem);
            ctx.Items.Add(removeItem);
            ctx.Items.Add(new Separator());
            ctx.Items.Add(findItem);
            _tableView.ContextMenu = ctx;
            mainPanel.Children.Add(_tableView);
            _statusText = new TextBlock { Text = "0 resources", Margin = new Avalonia.Thickness(4, 2) };
            mainPanel.Children.Add(_statusText);
            Content = mainPanel;
        }

        private void SetupUI()
        {
            _tableView = EditorHelpers.FindControlSafe<DataGrid>(this, "tableView") ?? this.FindControl<DataGrid>("tableView");
            _extractButton = EditorHelpers.FindControlSafe<Button>(this, "extractButton") ?? this.FindControl<Button>("extractButton");
            _loadButton = EditorHelpers.FindControlSafe<Button>(this, "loadButton") ?? this.FindControl<Button>("loadButton");
            _unloadButton = EditorHelpers.FindControlSafe<Button>(this, "unloadButton") ?? this.FindControl<Button>("unloadButton");
            _openButton = EditorHelpers.FindControlSafe<Button>(this, "openButton") ?? this.FindControl<Button>("openButton");
            _refreshButton = EditorHelpers.FindControlSafe<Button>(this, "refreshButton") ?? this.FindControl<Button>("refreshButton");
            _filterEdit = EditorHelpers.FindControlSafe<TextBox>(this, "filterEdit");
            _statusText = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
            if (_tableView != null)
                _tableView.ItemsSource = _filteredResources.View;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:175-187
        // Original: def _setup_signals(self):
        private void SetupSignals()
        {
            if (_extractButton != null)
            {
                _extractButton.Click += (s, e) => ExtractSelected();
            }

            if (_loadButton != null)
            {
                _loadButton.Click += (s, e) => SelectFilesToAdd();
            }

            if (_unloadButton != null)
            {
                _unloadButton.Click += (s, e) => RemoveSelected();
            }

            if (_openButton != null)
            {
                _openButton.Click += (s, e) => OpenSelected();
            }

            if (_refreshButton != null)
            {
                _refreshButton.Click += (s, e) => Refresh();
            }

            if (_tableView != null)
            {
                _tableView.SelectionChanged += (s, e) => { OnSelectionChanged(); UpdateStatusBar(); };
                _tableView.DoubleTapped += (s, e) => OpenSelected();
            }
            if (_filterEdit != null)
                _filterEdit.TextChanged += (s, e) => DoFilter(_filterEdit?.Text ?? "");
            Opened += (s, e) => { UpdateStatusBar(); _tableView?.Focus(); };
            KeyDown += OnWindowKeyDown;
        }

        private void SetupMenuHandlers()
        {
            void Bind(string name, Action handler)
            {
                try
                {
                    var item = EditorHelpers.FindControlSafe<MenuItem>(this, name) ?? this.FindControl<MenuItem>(name);
                    if (item != null) item.Click += (s, e) => handler();
                }
                catch { }
            }
            Bind("actionNew", () => New());
            Bind("actionOpen", () => { });
            Bind("actionSave", () => Save());
            Bind("actionSaveAs", () => _ = RunSaveAsAsync());
            Bind("actionRevert", () => Revert());
            Bind("actionExit", () => Close());
            Bind("actionUndo", () => Undo());
            Bind("actionRedo", () => Redo());
            Bind("actionFind", () => ShowFindDialog());
            Bind("actionFindNext", () => FindNextMatch());
            Bind("actionExtract", () => ExtractSelected());
            Bind("actionOpenResource", () => OpenSelected());
            Bind("actionRemove", () => RemoveSelected());
            Bind("ctxExtract", () => ExtractSelected());
            Bind("ctxOpen", () => OpenSelected());
            Bind("ctxRemove", () => RemoveSelected());
            Bind("ctxFind", () => ShowFindDialog());
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

        private void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            try
            {
                _undoStack.Clear();
                _redoStack.Clear();
                LoadFromBytes(_revert);
                _hasChanges = false;
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Revert failed: {ex}");
            }
        }

        private async Task RunSaveAsAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            string suggestedName = string.IsNullOrEmpty(_resname) ? "archive" : _resname;
            var ext = _restype != null ? _restype.Extension : "erf";
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + "." + ext,
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("ERF") { Patterns = new[] { "*.erf" } },
                    new FilePickerFileType("MOD") { Patterns = new[] { "*.mod" } },
                    new FilePickerFileType("RIM") { Patterns = new[] { "*.rim" } },
                    new FilePickerFileType("SAV") { Patterns = new[] { "*.sav" } }
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
            else _restype = ResourceType.ERF;
            RefreshWindowTitle();
            Save();
            UpdateStatusBar();
        }

        private void UpdateStatusBar()
        {
            try
            {
                string statusText = $"{_sourceResources?.Count ?? 0} resources";
                int sel = _tableView?.SelectedItems?.Count ?? 0;
                if (sel > 0) statusText += $" | {sel} selected";
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
            if (string.IsNullOrEmpty(_findText) || _tableView == null) return;
            DoFilter(_findText);
            var list = _sourceResources.Where(r =>
                (_findMatchCase ? r.ResRef : r.ResRef?.ToLowerInvariant()).Contains(_findMatchCase ? _findText : _findText.ToLowerInvariant())
                || (_findMatchCase ? r.Type : r.Type?.ToLowerInvariant()).Contains(_findMatchCase ? _findText : _findText.ToLowerInvariant())).ToList();
            if (list.Count == 0) return;
            var current = _tableView.SelectedItem as ERFResourceViewModel;
            int idx = current != null ? list.IndexOf(current) : -1;
            int next = (idx + 1) % list.Count;
            var sel = list[next];
            _tableView.SelectedItem = sel;
            _tableView.ScrollIntoView(sel, null);
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
                        RimResource = resource
                    });
                    offset += resource.Data.Length;
                }
            }
            else if (restype == ResourceType.ERF || restype == ResourceType.MOD || restype == ResourceType.SAV)
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
                        RimResource = null
                    });
                    offset += resource.Data.Length;
                }
            }
            if (_refreshButton != null)
                _refreshButton.IsEnabled = _sourceResources.Count > 0;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:199-255
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            if (_hasChanges && !PromptConfirm())
                return;
            _hasChanges = false;
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:257-289
        // Original: def build(self) -> tuple[bytes, bytes]:
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
                    {
                        rim.SetData(viewModel.RimResource.ResRef.ToString(), viewModel.RimResource.ResType, viewModel.RimResource.Data);
                    }
                }
                byte[] data = RIMAuto.BytesRim(rim);
                return Tuple.Create(data, new byte[0]);
            }
            else if (restype == ResourceType.ERF || restype == ResourceType.MOD || restype == ResourceType.SAV)
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
                    {
                        erf.SetData(viewModel.ErfResource.ResRef.ToString(), viewModel.ErfResource.ResType, viewModel.ErfResource.Data);
                    }
                }
                byte[] data = ERFAuto.BytesErf(erf, restype);
                return Tuple.Create(data, new byte[0]);
            }
            else
            {
                throw new InvalidOperationException($"Invalid restype for OdyToolERF: {restype}");
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:291-299
        // Original: def new(self):
        public override void New()
        {
            if (_hasChanges && !PromptConfirm())
                return;
            _hasChanges = false;
            _undoStack.Clear();
            _redoStack.Clear();
            base.New();
            _restype = ResourceType.ERF;
            _sourceResources.Clear();
            if (_refreshButton != null)
                _refreshButton.IsEnabled = false;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:301-328
        // Original: def save(self):
        public override void Save()
        {
            _hasChanges = false;
            if (string.IsNullOrEmpty(_filepath))
            {
                SaveAs();
                return;
            }

            if (_refreshButton != null)
            {
                _refreshButton.IsEnabled = true;
            }

            var (data, _) = Build();
            _revert = data;
            File.WriteAllBytes(_filepath, data);
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:376-381
        // Original: def extract_selected(self):
        private void ExtractSelected()
        {
            var selected = GetSelectedResources();
            if (selected.Count == 0)
            {
                return;
            }
            // Extract functionality will be implemented when file dialogs are available
            System.Console.WriteLine($"Extracting {selected.Count} resources");
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:467-469
        // Original: def select_files_to_add(self):
        private void SelectFilesToAdd()
        {
            // File selection will be implemented when file dialogs are available
            System.Console.WriteLine("Select files to add");
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:430-437
        // Original: def remove_selected(self):
        private void RemoveSelected()
        {
            var selected = _tableView?.SelectedItems?.Cast<ERFResourceViewModel>().ToList() ?? new List<ERFResourceViewModel>();
            if (selected.Count == 0) return;
            PushState();
            _hasChanges = true;
            foreach (var item in selected)
                _sourceResources.Remove(item);
            UpdateStatusBar();
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:471-500
        // Original: def open_selected(self, *, gff_specialized=None):
        private void OpenSelected()
        {
            var selected = GetSelectedResources();
            if (selected.Count == 0)
            {
                return;
            }
            // Open in editor functionality will be implemented when WindowUtils is available
            System.Console.WriteLine($"Opening {selected.Count} resources");
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:523-538
        // Original: def refresh(self):
        private void Refresh()
        {
            if (_hasChanges && !PromptConfirm())
            {
                return;
            }
            if (string.IsNullOrEmpty(_filepath))
            {
                System.Console.WriteLine("Nothing to refresh - file not loaded");
                return;
            }
            _hasChanges = false;
            byte[] data = File.ReadAllBytes(_filepath);
            Load(_filepath, _resname, _restype, data);
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:540-544
        // Original: def on_selection_changed(self):
        private void OnSelectionChanged()
        {
            bool hasSelection = _tableView?.SelectedItems?.Count > 0;
            if (_extractButton != null)
            {
                _extractButton.IsEnabled = hasSelection;
            }
            if (_openButton != null)
            {
                _openButton.IsEnabled = hasSelection;
            }
            if (_unloadButton != null)
            {
                _unloadButton.IsEnabled = hasSelection;
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:368-374
        // Original: def get_selected_resources(self) -> list[ERFResource]:
        private List<object> GetSelectedResources()
        {
            var selected = _tableView?.SelectedItems?.Cast<ERFResourceViewModel>().ToList() ?? new List<ERFResourceViewModel>();
            var resources = new List<object>();
            foreach (var vm in selected)
            {
                if (vm.ErfResource != null)
                {
                    resources.Add(vm.ErfResource);
                }
                else if (vm.RimResource != null)
                {
                    resources.Add(vm.RimResource);
                }
            }
            return resources;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:189-197
        // Original: def prompt_confirm(self) -> bool:
        private bool PromptConfirm()
        {
            // Confirmation dialog will be implemented when MessageBox is available
            return true;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/erf.py:71-76
        // Original: def human_readable_size(byte_size: float) -> str:
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
    }
}
