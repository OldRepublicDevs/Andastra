using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using BioWare;
using BioWare.Common;
using BioWare.Resource.Formats.TLK;
using BioWare.Resource;
using OdyTools.Common;
using OdyTools.Data;
using OdyTools.Dialogs;

namespace OdyTools.Editors
{
    public partial class OdyToolTLK : Editor
    {
        private const int MinEditorWidth = 400;
        private const int MinEditorHeight = 300;
        private const int UndoMaxLevels = 30;
        private static readonly (Language language, string menuItemName)[] LanguageMenuItems =
        {
            (Language.English, "actionLangEnglish"),
            (Language.French, "actionLangFrench"),
            (Language.German, "actionLangGerman"),
            (Language.Italian, "actionLangItalian"),
            (Language.Spanish, "actionLangSpanish"),
            (Language.Polish, "actionLangPolish"),
            (Language.ChineseSimplified, "actionLangChineseSimplified"),
            (Language.ChineseTraditional, "actionLangChineseTraditional"),
            (Language.Japanese, "actionLangJapanese"),
            (Language.Korean, "actionLangKorean"),
        };

        private ObservableCollection<TLKEntryViewModel> _sourceEntries;
        private CollectionViewSource _filteredEntries;
        private Language _language;
        private TextBox _textEdit;
        private TextBox _soundEdit;
        private TextBox _searchEdit;
        private Button _searchButton;
        private NumericUpDown _jumpSpinbox;
        private Button _jumpButton;
        private DataGrid _talkTable;
        private Control _searchBox;
        private Control _jumpBox;
        private TLKEntryViewModel _selectedEntry;
        private TextBlock _statusText;

        private readonly List<List<(string text, string sound)>> _undoStack = new List<List<(string, string)>>();
        private readonly List<List<(string text, string sound)>> _redoStack = new List<List<(string, string)>>();
        private bool _undoRedoInProgress;
        private string _findText = "";
        private string _replaceText = "";
        private bool _findMatchCase;
        private int _lastFindIndex = -1;

        public OdyToolTLK() : this(null, null) { }
        public OdyToolTLK(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolTLK", "none",
                new[] { ResourceType.TLK, ResourceType.TLK_XML, ResourceType.TLK_JSON },
                new[] { ResourceType.TLK, ResourceType.TLK_XML, ResourceType.TLK_JSON },
                installation)
        {
            _sourceEntries = new ObservableCollection<TLKEntryViewModel>();
            _filteredEntries = new CollectionViewSource { Source = _sourceEntries };
            _language = Language.English;

            InitializeComponent();
            SetupUI();
            SetupSignals();
            SetupMenuHandlers();
            KeyDown += OnWindowKeyDown;
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            New();
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;
            }
            catch
            {
                // XAML not available - will use programmatic UI
            }

            if (!xamlLoaded)
            {
                SetupProgrammaticUI();
            }
        }

        private void EnsureDataGridColumns()
        {
            if (_talkTable == null) return;
            if (_talkTable.Columns.Count > 0) return; // Already have columns
            _talkTable.Columns.Add(new DataGridTextColumn
            {
                Header = "#",
                Binding = new Binding("StringRef"),
                IsReadOnly = true,
                Width = new DataGridLength(60, DataGridLengthUnitType.Pixel)
            });
            _talkTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Text",
                Binding = new Binding("Text"),
                IsReadOnly = false
            });
            _talkTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Sound",
                Binding = new Binding("Sound"),
                IsReadOnly = false,
                Width = new DataGridLength(120, DataGridLengthUnitType.Pixel)
            });
        }

        private void SetupProgrammaticUI()
        {
            var mainPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Search box
            var searchBoxPanel = new StackPanel { Orientation = Orientation.Horizontal, IsVisible = false };
            _searchBox = searchBoxPanel;
            _searchEdit = new TextBox { Watermark = "Search..." };
            _searchButton = new Button { Content = "Search" };
            searchBoxPanel.Children.Add(_searchEdit);
            searchBoxPanel.Children.Add(_searchButton);
            mainPanel.Children.Add(searchBoxPanel);

            // Jump box
            var jumpBoxPanel = new StackPanel { Orientation = Orientation.Horizontal, IsVisible = false };
            _jumpBox = jumpBoxPanel;
            _jumpSpinbox = new NumericUpDown { Minimum = 0, Maximum = 0 };
            _jumpButton = new Button { Content = "Go" };
            jumpBoxPanel.Children.Add(_jumpSpinbox);
            jumpBoxPanel.Children.Add(_jumpButton);
            mainPanel.Children.Add(jumpBoxPanel);

            // Table
            _talkTable = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserReorderColumns = false,
                CanUserResizeColumns = true,
                SelectionMode = DataGridSelectionMode.Single
            };
            _talkTable.Columns.Add(new DataGridTextColumn
            {
                Header = "#",
                Binding = new Binding("StringRef"),
                IsReadOnly = true,
                Width = new DataGridLength(60, DataGridLengthUnitType.Pixel)
            });
            _talkTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Text",
                Binding = new Binding("Text"),
                IsReadOnly = false
            });
            _talkTable.Columns.Add(new DataGridTextColumn
            {
                Header = "Sound",
                Binding = new Binding("Sound"),
                IsReadOnly = false,
                Width = new DataGridLength(120, DataGridLengthUnitType.Pixel)
            });
            mainPanel.Children.Add(_talkTable);

            // Bottom panel
            var bottomPanel = new StackPanel { Orientation = Orientation.Vertical };
            _textEdit = new TextBox { AcceptsReturn = true, Watermark = "Text" };
            _soundEdit = new TextBox { MaxLength = 16, Watermark = "Sound ResRef" };
            bottomPanel.Children.Add(new TextBlock { Text = "Text:" });
            bottomPanel.Children.Add(_textEdit);
            bottomPanel.Children.Add(new TextBlock { Text = "Sound:" });
            bottomPanel.Children.Add(_soundEdit);
            mainPanel.Children.Add(bottomPanel);

            Content = mainPanel;
        }

        private void SetupUI()
        {
            // If controls are already initialized (e.g., by SetupProgrammaticUI), skip control finding
            if (_talkTable != null && _textEdit != null && _soundEdit != null && _searchEdit != null && _searchButton != null && _jumpSpinbox != null && _jumpButton != null)
            {
                if (_talkTable.ItemsSource == null)
                {
                    _talkTable.ItemsSource = _filteredEntries.View;
                }
                return;
            }

            // Use try-catch to handle cases where XAML controls might not be available (e.g., in tests)
            try
            {
                // Try to find controls from XAML if available
                _talkTable = this.FindControl<DataGrid>("talkTable");
                _textEdit = this.FindControl<TextBox>("textEdit");
                _soundEdit = this.FindControl<TextBox>("soundEdit");
                _searchEdit = this.FindControl<TextBox>("searchEdit");
                _searchButton = this.FindControl<Button>("searchButton");
                _jumpSpinbox = this.FindControl<NumericUpDown>("jumpSpinbox");
                _jumpButton = this.FindControl<Button>("jumpButton");
                var searchBoxBorder = this.FindControl<Border>("searchBox");
                if (searchBoxBorder != null)
                {
                    _searchBox = searchBoxBorder;
                }
                var jumpBoxBorder = this.FindControl<Border>("jumpBox");
                if (jumpBoxBorder != null)
                {
                    _jumpBox = jumpBoxBorder;
                }
            }
            catch
            {
                // XAML controls not available - controls should already be initialized by SetupProgrammaticUI
                // If not, ensure minimal setup
                if (_talkTable == null)
                {
                    _talkTable = new DataGrid
                    {
                        AutoGenerateColumns = false,
                        CanUserReorderColumns = false,
                        CanUserResizeColumns = true,
                        SelectionMode = DataGridSelectionMode.Single
                    };
                }
                if (_textEdit == null)
                {
                    _textEdit = new TextBox { AcceptsReturn = true };
                }
                if (_soundEdit == null)
                {
                    _soundEdit = new TextBox { MaxLength = 16 };
                }
                if (_searchEdit == null)
                {
                    _searchEdit = new TextBox { Watermark = "Search..." };
                }
                if (_searchButton == null)
                {
                    _searchButton = new Button { Content = "Search" };
                }
                if (_jumpSpinbox == null)
                {
                    _jumpSpinbox = new NumericUpDown { Minimum = 0, Maximum = 0 };
                }
                if (_jumpButton == null)
                {
                    _jumpButton = new Button { Content = "Go" };
                }
                if (_searchBox == null)
                {
                    _searchBox = new StackPanel { Orientation = Orientation.Horizontal };
                }
                if (_jumpBox == null)
                {
                    _jumpBox = new StackPanel { Orientation = Orientation.Horizontal };
                }
            }

            if (_talkTable != null)
            {
                if (_talkTable.ItemsSource == null)
                {
                    _talkTable.ItemsSource = _filteredEntries.View;
                }
                EnsureDataGridColumns();
            }
            else if (_talkTable == null && Content is StackPanel panel)
            {
                // Ensure table is set up if created programmatically
                foreach (var child in panel.Children)
                {
                    if (child is DataGrid dg)
                    {
                        _talkTable = dg;
                        if (_talkTable.ItemsSource == null)
                        {
                            _talkTable.ItemsSource = _filteredEntries.View;
                        }
                        break;
                    }
                }
            }
        }

        private void SetupSignals()
        {
            if (_jumpButton != null)
            {
                _jumpButton.Click += (s, e) => OnJumpSpinboxGoto();
            }

            if (_jumpSpinbox != null)
            {
                _jumpSpinbox.ValueChanged += (s, e) => OnJumpSpinboxGoto();
                _jumpSpinbox.KeyDown += (s, e) =>
                {
                    if (e.Key == Key.Enter || e.Key == Key.Return)
                    {
                        OnJumpSpinboxGoto();
                        e.Handled = true;
                    }
                };
            }

            if (_searchButton != null)
            {
                _searchButton.Click += (s, e) => DoFilter(_searchEdit?.Text ?? "");
            }

            if (_searchEdit != null)
            {
                _searchEdit.KeyDown += (s, e) =>
                {
                    if (e.Key == Key.Enter || e.Key == Key.Return)
                    {
                        DoFilter(_searchEdit?.Text ?? "");
                        e.Handled = true;
                    }
                };
            }

            if (_talkTable != null)
            {
                _talkTable.SelectionChanged += (s, e) => { SelectionChanged(); UpdateStatusBar(); };
            }

            if (_textEdit != null)
            {
                _textEdit.TextChanged += (s, e) => UpdateEntry();
                EditorHelpers.BindLostFocus(_textEdit, CommitEntryEdits);
            }

            if (_soundEdit != null)
            {
                _soundEdit.TextChanged += (s, e) => UpdateEntry();
                EditorHelpers.BindLostFocus(_soundEdit, CommitEntryEdits);
            }

            Opened += (s, e) =>
            {
                UpdateStatusBar();
                _talkTable?.Focus();
            };
        }

        private void SetupMenuHandlers()
        {
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
            EditorHelpers.BindMenuClicks(this, new (string menuItemName, Action handler)[]
            {
                ("actionUndo", Undo),
                ("actionRedo", Redo),
                ("actionInsert", Insert),
                ("actionDeleteEntry", RemoveSelectedEntry),
                ("actionFind", ShowFindDialog),
                ("actionReplace", ShowReplaceDialog),
                ("actionFindNext", () => FindNextMatch()),
                ("actionGoTo", ShowGoToEntryDialog),
                ("ctxInsert", Insert),
                ("ctxDeleteEntry", RemoveSelectedEntry),
                ("ctxFind", ShowFindDialog),
                ("ctxGoTo", ShowGoToEntryDialog),
                ("ctxFindReferences", FindLocalizedStringReferences),
            });

            BindLanguageMenuHandlers();

            RefreshLanguageMenuCheckmarks();
        }

        /// <summary>
        /// Wires TLK file language menu actions to language-switch handlers.
        /// </summary>
        private void BindLanguageMenuHandlers()
        {
            EditorHelpers.BindMenuClick(this, "actionLangAutoDetect", () =>
            {
                if (_revert != null && _revert.Length > 0)
                {
                    Revert();
                }
            });

            foreach (var item in LanguageMenuItems)
            {
                Language selectedLanguage = item.language;
                EditorHelpers.BindMenuClick(this, item.menuItemName, () => ChangeLanguage(selectedLanguage));
            }
        }

        private void CommitEntryEdits()
        {
            if (_selectedEntry == null) return;
            string text = _textEdit?.Text ?? "";
            string sound = _soundEdit?.Text ?? "";
            if (text == _selectedEntry.Text && sound == _selectedEntry.Sound) return;
            PushState();
            UpdateEntry();
        }

        private void PushState()
        {
            if (_undoRedoInProgress) return;
            var snapshot = CaptureCurrentEntriesSnapshot();
            _redoStack.Clear();
            _undoStack.Add(snapshot);
            if (_undoStack.Count > UndoMaxLevels) _undoStack.RemoveAt(0);
            MarkDocumentDirty();
        }

        private void ApplyState(List<(string text, string sound)> snapshot)
        {
            _sourceEntries.Clear();
            for (int i = 0; i < snapshot.Count; i++)
                _sourceEntries.Add(new TLKEntryViewModel(i, snapshot[i].text, snapshot[i].sound));
            UpdateJumpSpinboxMaximum();
            UpdateStatusBar();
        }

        public void Undo()
        {
            if (_undoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                var snapshot = _undoStack[_undoStack.Count - 1];
                _undoStack.RemoveAt(_undoStack.Count - 1);
                var current = CaptureCurrentEntriesSnapshot();
                _redoStack.Add(current);
                ApplyState(snapshot);
            }
            finally { _undoRedoInProgress = false; }
        }

        public void Redo()
        {
            if (_redoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                var snapshot = _redoStack[_redoStack.Count - 1];
                _redoStack.RemoveAt(_redoStack.Count - 1);
                var current = CaptureCurrentEntriesSnapshot();
                _undoStack.Add(current);
                ApplyState(snapshot);
            }
            finally { _undoRedoInProgress = false; }
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            var mod = e.KeyModifiers;
            bool ctrl = (mod & KeyModifiers.Control) != 0;
            if (ctrl)
            {
                if (e.Key == Key.S) { Save(); e.Handled = true; }
                else if (e.Key == Key.Z) { Undo(); e.Handled = true; }
                else if (e.Key == Key.Y) { Redo(); e.Handled = true; }
                else if (e.Key == Key.F) { ShowFindDialog(); e.Handled = true; }
                else if (e.Key == Key.H) { ShowReplaceDialog(); e.Handled = true; }
                else if (e.Key == Key.G) { ShowGoToEntryDialog(); e.Handled = true; }
                else if (e.Key == Key.I) { Insert(); e.Handled = true; }
            }
            else if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                RemoveSelectedEntry();
                e.Handled = true;
            }
            else if (e.Key == Key.F3)
            {
                FindNextMatch();
                e.Handled = true;
            }
        }

        private void UpdateStatusBar()
        {
            try
            {
                if (_statusText == null)
                    _statusText = this.FindControl<TextBlock>("statusText");
                if (_statusText != null)
                {
                    int n = _sourceEntries?.Count ?? 0;
                    string text = n == 1 ? "1 entry" : $"{n} entries";
                    if (_selectedEntry != null)
                    {
                        int idx = _sourceEntries.IndexOf(_selectedEntry);
                        if (idx >= 0) text += $" | Entry {idx} selected";
                    }
                    _statusText.Text = text;
                }
            }
            catch { }
        }

        public override void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            try
            {
                base.Revert(); // Calls Load which does TLK loading
                RefreshLanguageMenuCheckmarks();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Revert failed: {ex}");
            }
        }

        /// <summary>Opens a TLK file from disk (File → Open). Used by standalone and when opening from toolset.</summary>
        protected override async Task RunOpenAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            var options = new FilePickerOpenOptions
            {
                Title = "Open TLK",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("TLK (binary)") { Patterns = new[] { "*.tlk" } },
                    new FilePickerFileType("TLK XML") { Patterns = new[] { "*.tlk.xml", "*.xml" } },
                    new FilePickerFileType("TLK JSON") { Patterns = new[] { "*.tlk.json", "*.json" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
                }
            };
            var files = await storageProvider.OpenFilePickerAsync(options);
            if (files == null || files.Count == 0) return;
            string path = files[0].Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path) || !System.IO.File.Exists(path)) return;
            try
            {
                byte[] data = System.IO.File.ReadAllBytes(path);
                string resname = System.IO.Path.GetFileNameWithoutExtension(path);
                string ext = System.IO.Path.GetExtension(path).TrimStart('.').ToLowerInvariant();
                if (path.EndsWith(".tlk.xml", StringComparison.OrdinalIgnoreCase)) ext = "tlk.xml";
                else if (path.EndsWith(".tlk.json", StringComparison.OrdinalIgnoreCase)) ext = "tlk.json";
                else if (ext == "xml" && path.ToLowerInvariant().Contains(".tlk.")) ext = "tlk.xml";
                else if (ext == "json" && path.ToLowerInvariant().Contains(".tlk.")) ext = "tlk.json";
                ResourceType restype = ResourceType.FromExtension(ext) ?? ResourceType.TLK;
                Load(path, resname, restype, data);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Open failed: {ex}");
            }
        }

        protected override async Task RunSaveAsAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            string suggestedName = string.IsNullOrEmpty(_resname) ? "dialog" : _resname;
            var options = new FilePickerSaveOptions
            {
                Title = "Save TLK As",
                SuggestedFileName = suggestedName + ".tlk",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("TLK (binary)") { Patterns = new[] { "*.tlk" } },
                    new FilePickerFileType("TLK XML") { Patterns = new[] { "*.tlk.xml", "*.xml" } },
                    new FilePickerFileType("TLK JSON") { Patterns = new[] { "*.tlk.json", "*.json" } }
                }
            };
            var file = await storageProvider.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            if (path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) _restype = ResourceType.TLK_XML;
            else if (path.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) _restype = ResourceType.TLK_JSON;
            else _restype = ResourceType.TLK;
            RefreshWindowTitle();
            Save();
            UpdateStatusBar();
        }

        private void OnJumpSpinboxGoto()
        {
            if (_jumpSpinbox == null || _talkTable == null)
            {
                return;
            }

            int sourceRow = (int)(_jumpSpinbox.Value ?? 0);
            if (sourceRow < 0 || sourceRow >= _sourceEntries.Count)
            {
                return;
            }

            var entry = _sourceEntries[sourceRow];
            if (entry != null)
            {
                _talkTable.SelectedItem = entry;
                _talkTable.ScrollIntoView(entry, null);
            }
        }

        public void ChangeLanguage(Language language)
        {
            _language = language;

            // Only reload if we have revert data (file was loaded)
            if (_revert == null || _revert.Length == 0)
            {
                RefreshLanguageMenuCheckmarks();
                return;
            }

            var tlk = TLKAuto.ReadTlk(_revert);
            tlk.Language = language;
            LoadTLK(tlk);
            RefreshLanguageMenuCheckmarks();
        }

        private void RefreshLanguageMenuCheckmarks()
        {
            try
            {
                bool hasRevert = _revert != null && _revert.Length > 0;
                Language? fileLanguage = null;
                if (hasRevert)
                {
                    try
                    {
                        var tlk = TLKAuto.ReadTlk(_revert);
                        fileLanguage = tlk.Language;
                    }
                    catch { }
                }
                bool isAutoDetect = hasRevert && fileLanguage.HasValue && _language == fileLanguage.Value;
                var autoItem = this.FindControl<MenuItem>("actionLangAutoDetect");
                if (autoItem != null) autoItem.IsChecked = isAutoDetect;
                foreach (var item in LanguageMenuItems)
                {
                    var menuItem = this.FindControl<MenuItem>(item.menuItemName);
                    if (menuItem != null) menuItem.IsChecked = _language == item.language;
                }
            }
            catch { }
        }

        private void FindLocalizedStringReferences()
        {
            if (_installation == null || _selectedEntry == null) return;
            int strref = _selectedEntry.StringRef;
            try
            {
                var install = _installation;
                if (install?.Installation == null) return;
                var results = BioWare.Tools.ReferenceCacheHelpers.FindStrRefReferences(
                    install.Installation, strref, null, s => System.Console.WriteLine(s));
                var fileResources = results?.Select(r => r.Resource).Distinct().ToList() ?? new List<BioWare.Extract.FileResource>();
                var dialog = new Dialogs.FileResultsDialog(this as Window, fileResources, install);
                dialog.SearchResultsSelected += (resource) => { /* Could open resource in appropriate editor */ };
                dialog.Show();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Find references failed: {ex}");
            }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            if (data == null || data.Length == 0)
            {
                _sourceEntries.Clear();
                RefreshLanguageMenuCheckmarks();
                return;
            }

            try
            {
                var tlk = TLKAuto.ReadTlk(data);
                _language = tlk.Language;
                LoadTLK(tlk);
                RefreshLanguageMenuCheckmarks();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error loading TLK: {ex}");
            }
        }

        private void LoadTLK(TLK tlk)
        {
            _sourceEntries.Clear();
            _undoStack.Clear();
            _redoStack.Clear();
            _language = tlk.Language;

            // Load entries in batches for performance
            int batchSize = 200;
            for (int i = 0; i < tlk.Entries.Count; i++)
            {
                var entry = tlk.Entries[i];
                _sourceEntries.Add(new TLKEntryViewModel(i, entry.Text, entry.Voiceover.ToString()));

                // Yield to UI thread periodically
                if (i % batchSize == 0 && i > 0)
                {
                    Thread.Sleep(1);
                }
            }

            if (_jumpSpinbox != null)
                _jumpSpinbox.Maximum = GetLastEntryIndex();
            UpdateStatusBar();
        }

        public override void New()
        {
            base.New();
            _sourceEntries.Clear();
            _undoStack.Clear();
            _redoStack.Clear();
            _language = Language.English;
            if (_textEdit != null) _textEdit.IsEnabled = false;
            if (_soundEdit != null) _soundEdit.IsEnabled = false;
            if (_jumpSpinbox != null) _jumpSpinbox.Maximum = 0;
            RefreshLanguageMenuCheckmarks();
            UpdateStatusBar();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var tlk = new TLK(_language);

            foreach (var entry in _sourceEntries)
            {
                tlk.Entries.Add(entry.ToTLKEntry());
            }

            ResourceType tlkType = _restype ?? ResourceType.TLK;
            byte[] data = TLKAuto.BytesTlk(tlk, tlkType);
            return Tuple.Create(data, new byte[0]);
        }

        public void Insert()
        {
            PushState();
            int newIndex = _sourceEntries.Count;
            _sourceEntries.Add(new TLKEntryViewModel(newIndex, "", ""));
            UpdateJumpSpinboxMaximum();
            UpdateStatusBar();
        }

        public void RemoveSelectedEntry()
        {
            if (_selectedEntry == null) return;
            int idx = _sourceEntries.IndexOf(_selectedEntry);
            if (idx < 0) return;
            PushState();
            _sourceEntries.RemoveAt(idx);
            var list = _sourceEntries.Select(e => (e.Text ?? "", e.Sound ?? "")).ToList();
            _sourceEntries.Clear();
            for (int i = 0; i < list.Count; i++)
                _sourceEntries.Add(new TLKEntryViewModel(i, list[i].Item1, list[i].Item2));
            UpdateJumpSpinboxMaximum();
            _selectedEntry = null;
            if (_textEdit != null) _textEdit.IsEnabled = false;
            if (_soundEdit != null) _soundEdit.IsEnabled = false;
            UpdateStatusBar();
        }

        private List<(string text, string sound)> CaptureCurrentEntriesSnapshot()
        {
            var snapshot = new List<(string text, string sound)>();
            foreach (var entry in _sourceEntries)
            {
                snapshot.Add((entry.Text ?? "", entry.Sound ?? ""));
            }

            return snapshot;
        }

        private int GetLastEntryIndex()
        {
            return _sourceEntries.Count > 0 ? _sourceEntries.Count - 1 : 0;
        }

        private void UpdateJumpSpinboxMaximum()
        {
            if (_jumpSpinbox != null)
            {
                _jumpSpinbox.Maximum = GetLastEntryIndex();
            }
        }

        public void DoFilter(string text)
        {
            if (_filteredEntries.View == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                _filteredEntries.View.Filter = null;
            }
            else
            {
                string filterText = text.ToLowerInvariant();
                _filteredEntries.View.Filter = item =>
                {
                    if (item is TLKEntryViewModel entry)
                    {
                        return entry.Text.ToLowerInvariant().Contains(filterText) ||
                               entry.Sound.ToLowerInvariant().Contains(filterText);
                    }
                    return false;
                };
            }

            _filteredEntries.View.Refresh();
        }

        public void ToggleFilterBox()
        {
            if (_searchBox != null)
            {
                _searchBox.IsVisible = !_searchBox.IsVisible;
                if (_searchBox.IsVisible && _searchEdit != null)
                {
                    _searchEdit.Focus();
                    _searchEdit.SelectAll();
                }
            }
        }

        public void ToggleGotoBox()
        {
            if (_jumpBox != null)
            {
                _jumpBox.IsVisible = !_jumpBox.IsVisible;
                if (_jumpBox.IsVisible && _jumpSpinbox != null)
                {
                    _jumpSpinbox.Focus();
                }
            }
        }

        private void SelectionChanged()
        {
            if (_talkTable == null)
            {
                return;
            }

            _selectedEntry = _talkTable.SelectedItem as TLKEntryViewModel;

            if (_selectedEntry == null)
            {
                if (_textEdit != null)
                {
                    _textEdit.IsEnabled = false;
                }
                if (_soundEdit != null)
                {
                    _soundEdit.IsEnabled = false;
                }
                return;
            }

            if (_textEdit != null)
            {
                _textEdit.IsEnabled = true;
                _textEdit.Text = _selectedEntry.Text;
            }
            if (_soundEdit != null)
            {
                _soundEdit.IsEnabled = true;
                _soundEdit.Text = _selectedEntry.Sound;
            }
        }

        private void UpdateEntry()
        {
            if (_selectedEntry == null)
            {
                return;
            }

            if (_textEdit != null)
            {
                _selectedEntry.Text = _textEdit.Text;
            }
            if (_soundEdit != null)
            {
                _selectedEntry.Sound = _soundEdit.Text;
            }
        }

        private void ShowFindDialog()
        {
            var dialog = new Window
            {
                Title = "Find",
                Width = 400,
                Height = 160,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var findLabel = new TextBlock { Text = "Find what:" };
            var findBox = new TextBox { Text = _findText, Watermark = "Search text or sound" };
            var matchCaseCb = new CheckBox { Content = "Match case", IsChecked = _findMatchCase };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var findNextBtn = new Button { Content = "Find Next", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var closeBtn = new Button { Content = "Close" };
            panel.Children.Add(findLabel);
            panel.Children.Add(findBox);
            panel.Children.Add(matchCaseCb);
            buttons.Children.Add(findNextBtn);
            buttons.Children.Add(closeBtn);
            panel.Children.Add(buttons);
            dialog.Content = panel;
            findNextBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                if (FindNextMatch()) dialog.Close();
            };
            closeBtn.Click += (s, e) => dialog.Close();
            findBox.Focus();
            _ = dialog.ShowDialog(this as Window);
        }

        private bool FindNextMatch()
        {
            if (string.IsNullOrEmpty(_findText)) return false;
            var comp = _findMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int start = _lastFindIndex + 1;
            for (int i = start; i < _sourceEntries.Count; i++)
            {
                var e = _sourceEntries[i];
                if ((e.Text ?? "").IndexOf(_findText, comp) >= 0 || (e.Sound ?? "").IndexOf(_findText, comp) >= 0)
                {
                    _lastFindIndex = i;
                    _talkTable.SelectedItem = e;
                    _talkTable.ScrollIntoView(e, null);
                    SelectionChanged();
                    UpdateStatusBar();
                    return true;
                }
            }
            for (int i = 0; i < start; i++)
            {
                var e = _sourceEntries[i];
                if ((e.Text ?? "").IndexOf(_findText, comp) >= 0 || (e.Sound ?? "").IndexOf(_findText, comp) >= 0)
                {
                    _lastFindIndex = i;
                    _talkTable.SelectedItem = e;
                    _talkTable.ScrollIntoView(e, null);
                    SelectionChanged();
                    UpdateStatusBar();
                    return true;
                }
            }
            _lastFindIndex = -1;
            return false;
        }

        private void ShowReplaceDialog()
        {
            var dialog = new Window
            {
                Title = "Replace",
                Width = 400,
                Height = 220,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var findLabel = new TextBlock { Text = "Find what:" };
            var findBox = new TextBox { Text = _findText, Watermark = "Search text or sound" };
            var replaceLabel = new TextBlock { Text = "Replace with:" };
            var replaceBox = new TextBox { Text = _replaceText, Watermark = "Replacement" };
            var matchCaseCb = new CheckBox { Content = "Match case", IsChecked = _findMatchCase };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var findNextBtn = new Button { Content = "Find Next", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var replaceOneBtn = new Button { Content = "Replace", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var replaceAllBtn = new Button { Content = "Replace All", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var closeBtn = new Button { Content = "Close" };
            panel.Children.Add(findLabel);
            panel.Children.Add(findBox);
            panel.Children.Add(replaceLabel);
            panel.Children.Add(replaceBox);
            panel.Children.Add(matchCaseCb);
            buttons.Children.Add(findNextBtn);
            buttons.Children.Add(replaceOneBtn);
            buttons.Children.Add(replaceAllBtn);
            buttons.Children.Add(closeBtn);
            panel.Children.Add(buttons);
            dialog.Content = panel;
            findNextBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                FindNextMatch();
            };
            replaceOneBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                ReplaceOne();
            };
            replaceAllBtn.Click += (s, e) =>
            {
                _findText = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                if (string.IsNullOrEmpty(_findText)) { dialog.Close(); return; }
                PushState();
                ReplaceAll();
                dialog.Close();
            };
            closeBtn.Click += (s, e) => dialog.Close();
            findBox.Focus();
            _ = dialog.ShowDialog(this as Window);
        }

        private void ReplaceOne()
        {
            if (string.IsNullOrEmpty(_findText) || _lastFindIndex < 0 || _lastFindIndex >= _sourceEntries.Count) return;
            var entry = _sourceEntries[_lastFindIndex];
            var comp = _findMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            string text = entry.Text ?? "";
            string sound = entry.Sound ?? "";
            bool changed = false;
            if (text.IndexOf(_findText, comp) >= 0)
            {
                PushState();
                entry.Text = ReplaceAllInString(text, _findText, _replaceText ?? "", _findMatchCase);
                changed = true;
            }
            else if (sound.IndexOf(_findText, comp) >= 0)
            {
                if (!changed) PushState();
                entry.Sound = ReplaceAllInString(sound, _findText, _replaceText ?? "", _findMatchCase);
                changed = true;
            }
            if (changed) UpdateStatusBar();
            FindNextMatch();
        }

        private void ReplaceAll()
        {
            var comp = _findMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            foreach (var entry in _sourceEntries)
            {
                string text = entry.Text ?? "";
                string sound = entry.Sound ?? "";
                if (text.IndexOf(_findText, comp) >= 0)
                    entry.Text = ReplaceAllInString(text, _findText, _replaceText ?? "", _findMatchCase);
                if (sound.IndexOf(_findText, comp) >= 0)
                    entry.Sound = ReplaceAllInString(sound, _findText, _replaceText ?? "", _findMatchCase);
            }
            _lastFindIndex = -1;
            UpdateStatusBar();
        }

        private static string ReplaceAllInString(string text, string find, string replace, bool matchCase)
        {
            if (string.IsNullOrEmpty(find)) return text;
            var comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int pos = 0;
            var sb = new System.Text.StringBuilder();
            while (pos < text.Length)
            {
                int idx = text.IndexOf(find, pos, comparison);
                if (idx < 0) break;
                sb.Append(text, pos, idx - pos);
                sb.Append(replace);
                pos = idx + find.Length;
            }
            if (pos == 0) return text;
            sb.Append(text, pos, text.Length - pos);
            return sb.ToString();
        }

        private void ShowGoToEntryDialog()
        {
            var dialog = new Window
            {
                Title = "Go to Entry",
                Width = 320,
                Height = 120,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var label = new TextBlock { Text = "Entry index:" };
            int maxIdx = GetLastEntryIndex();
            var spin = new NumericUpDown { Minimum = 0, Maximum = maxIdx, Value = 0 };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var goBtn = new Button { Content = "Go", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var cancelBtn = new Button { Content = "Cancel" };
            goBtn.Click += (s, e) =>
            {
                int idx = (int)(spin.Value ?? 0);
                if (idx >= 0 && idx < _sourceEntries.Count)
                {
                    var entry = _sourceEntries[idx];
                    _talkTable.SelectedItem = entry;
                    _talkTable.ScrollIntoView(entry, null);
                    SelectionChanged();
                    UpdateStatusBar();
                }
                dialog.Close();
            };
            cancelBtn.Click += (s, e) => dialog.Close();
            buttons.Children.Add(goBtn);
            buttons.Children.Add(cancelBtn);
            panel.Children.Add(label);
            panel.Children.Add(spin);
            panel.Children.Add(buttons);
            dialog.Content = panel;
            spin.Focus();
            _ = dialog.ShowDialog(this as Window);
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        public Language Language => _language;
    }
}
