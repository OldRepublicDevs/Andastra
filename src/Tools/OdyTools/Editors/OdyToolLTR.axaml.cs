using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.LTR;
using OdyTools.Common;
using OdyTools.Data;

namespace OdyTools.Editors
{
    public partial class OdyToolLTR : Editor
    {
        private const int MinEditorWidth = 980;
        private const int MinEditorHeight = 680;
        private const int UndoMaxLevels = 50;

        private sealed class ProbabilityRow : INotifyPropertyChanged
        {
            private float _start;
            private float _middle;
            private float _end;

            public ProbabilityRow(string displayContext, string character, float start, float middle, float end)
            {
                DisplayContext = displayContext;
                Character = character;
                _start = start;
                _middle = middle;
                _end = end;
            }

            public string DisplayContext { get; private set; }
            public string Character { get; private set; }

            public float Start
            {
                get { return _start; }
                set { SetField(ref _start, value); }
            }

            public float Middle
            {
                get { return _middle; }
                set { SetField(ref _middle, value); }
            }

            public float End
            {
                get { return _end; }
                set { SetField(ref _end, value); }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            private void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
            {
                if (EqualityComparer<T>.Default.Equals(field, value))
                {
                    return;
                }

                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private LTR _ltr = new LTR();
        private readonly List<string> _charList = LTR.CharacterSet.Select(c => c.ToString()).ToList();

        private readonly ObservableCollection<ProbabilityRow> _singleRows = new ObservableCollection<ProbabilityRow>();
        private readonly ObservableCollection<ProbabilityRow> _doubleRows = new ObservableCollection<ProbabilityRow>();
        private readonly ObservableCollection<ProbabilityRow> _tripleRows = new ObservableCollection<ProbabilityRow>();
        private readonly ObservableCollection<string> _generatedNames = new ObservableCollection<string>();

        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;
        private bool _suppressModelSync;

        private DataGrid _tableSingles;
        private DataGrid _tableDoubles;
        private DataGrid _tableTriples;
        private TabControl _mainTabControl;
        private ComboBox _doublePrevCombo;
        private ComboBox _triplePrev2Combo;
        private ComboBox _triplePrev1Combo;
        private ComboBox _quickModeCombo;
        private ComboBox _quickCharCombo;
        private NumericUpDown _quickStartSpin;
        private NumericUpDown _quickMiddleSpin;
        private NumericUpDown _quickEndSpin;
        private NumericUpDown _generatorCountSpin;
        private Button _applyQuickButton;
        private Button _normalizeVisibleButton;
        private Button _uniformVisibleButton;
        private Button _generateNamesButton;
        private Button _clearGeneratedButton;
        private Button _findNextButton;
        private TextBox _searchBox;
        private CheckBox _findMatchCase;
        private TextBlock _statusText;
        private TextBlock _contextText;
        private ListBox _generatedNamesList;

        private int _findIndex = -1;

        public OdyToolLTR(Window parent = null, OdyInstallation installation = null)
            : base(parent, Localization.Tr("OdyToolLTR"), "ltr", new[] { ResourceType.LTR }, new[] { ResourceType.LTR }, installation)
        {
            InitializeComponent();
            SetupUI();
            SetupSignals();
            SetupMenuHandlers();
            AddHelpAction();

            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;

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
                SetupProgrammaticFallback();
            }
        }

        private void SetupProgrammaticFallback()
        {
            Content = new TextBlock
            {
                Text = Localization.Tr("OdyToolLTR XAML failed to load."),
                Margin = new Avalonia.Thickness(12)
            };
        }

        private void SetupUI()
        {
            _tableSingles = EditorHelpers.FindControlSafe<DataGrid>(this, "TableSingles");
            _tableDoubles = EditorHelpers.FindControlSafe<DataGrid>(this, "TableDoubles");
            _tableTriples = EditorHelpers.FindControlSafe<DataGrid>(this, "TableTriples");
            _mainTabControl = EditorHelpers.FindControlSafe<TabControl>(this, "MainTabControl");
            _doublePrevCombo = EditorHelpers.FindControlSafe<ComboBox>(this, "DoublePrevCombo");
            _triplePrev2Combo = EditorHelpers.FindControlSafe<ComboBox>(this, "TriplePrev2Combo");
            _triplePrev1Combo = EditorHelpers.FindControlSafe<ComboBox>(this, "TriplePrev1Combo");
            _quickModeCombo = EditorHelpers.FindControlSafe<ComboBox>(this, "QuickModeCombo");
            _quickCharCombo = EditorHelpers.FindControlSafe<ComboBox>(this, "QuickCharCombo");
            _quickStartSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "QuickStartSpin");
            _quickMiddleSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "QuickMiddleSpin");
            _quickEndSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "QuickEndSpin");
            _generatorCountSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "GeneratorCountSpin");
            _applyQuickButton = EditorHelpers.FindControlSafe<Button>(this, "ApplyQuickButton");
            _normalizeVisibleButton = EditorHelpers.FindControlSafe<Button>(this, "NormalizeVisibleButton");
            _uniformVisibleButton = EditorHelpers.FindControlSafe<Button>(this, "UniformVisibleButton");
            _generateNamesButton = EditorHelpers.FindControlSafe<Button>(this, "GenerateNamesButton");
            _clearGeneratedButton = EditorHelpers.FindControlSafe<Button>(this, "ClearGeneratedButton");
            _findNextButton = EditorHelpers.FindControlSafe<Button>(this, "actionFindNextButton");
            _searchBox = EditorHelpers.FindControlSafe<TextBox>(this, "SearchBox");
            _findMatchCase = EditorHelpers.FindControlSafe<CheckBox>(this, "FindMatchCaseCheck");
            _statusText = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
            _contextText = EditorHelpers.FindControlSafe<TextBlock>(this, "ContextText");
            _generatedNamesList = EditorHelpers.FindControlSafe<ListBox>(this, "GeneratedNamesList");

            if (_tableSingles != null) _tableSingles.ItemsSource = _singleRows;
            if (_tableDoubles != null) _tableDoubles.ItemsSource = _doubleRows;
            if (_tableTriples != null) _tableTriples.ItemsSource = _tripleRows;
            if (_generatedNamesList != null) _generatedNamesList.ItemsSource = _generatedNames;

            if (_doublePrevCombo != null) _doublePrevCombo.ItemsSource = _charList;
            if (_triplePrev2Combo != null) _triplePrev2Combo.ItemsSource = _charList;
            if (_triplePrev1Combo != null) _triplePrev1Combo.ItemsSource = _charList;
            if (_quickCharCombo != null) _quickCharCombo.ItemsSource = _charList;
            if (_quickModeCombo != null) _quickModeCombo.ItemsSource = new[] { Localization.Tr("Singles"), Localization.Tr("Doubles"), Localization.Tr("Triples") };

            if (_doublePrevCombo != null) _doublePrevCombo.SelectedIndex = 0;
            if (_triplePrev2Combo != null) _triplePrev2Combo.SelectedIndex = 0;
            if (_triplePrev1Combo != null) _triplePrev1Combo.SelectedIndex = 0;
            if (_quickCharCombo != null) _quickCharCombo.SelectedIndex = 0;
            if (_quickModeCombo != null) _quickModeCombo.SelectedIndex = 0;
            if (_generatorCountSpin != null) _generatorCountSpin.Value = 25;

            ApplyLocalization();
        }

        private void ApplyLocalization()
        {
            Title = Localization.Tr("OdyToolLTR");
            if (Installation != null) RefreshWindowTitle();

            void SetHeader(string name, string key) { var c = EditorHelpers.FindControlSafe<MenuItem>(this, name); if (c != null) c.Header = Localization.Tr(key); }
            SetHeader("actionNew", "New");
            SetHeader("actionOpen", "Open...");
            SetHeader("actionSave", "Save");
            SetHeader("actionSaveAs", "Save As...");
            SetHeader("actionRevert", "Revert");
            SetHeader("actionExit", "Exit");
            SetHeader("actionUndo", "Undo");
            SetHeader("actionRedo", "Redo");
            SetHeader("actionFind", "Find");
            SetHeader("actionFindNext", "Find Next (F3)");
            SetHeader("actionNormalizeVisible", "Normalize Visible Distribution");
            SetHeader("actionUniformVisible", "Set Visible Distribution to Uniform");
            SetHeader("actionGenerateNames", "Generate Name Samples");

            var menu = this.GetVisualDescendants().OfType<Menu>().FirstOrDefault();
            if (menu?.Items != null)
            {
                var items = menu.Items.OfType<MenuItem>().ToList();
                if (items.Count >= 1) items[0].Header = "_" + Localization.Tr("File");
                if (items.Count >= 2) items[1].Header = "_" + Localization.Tr("Edit");
                if (items.Count >= 3) items[2].Header = "_" + Localization.Tr("Tools");
            }

            var findLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "FindLabel");
            if (findLabel != null) findLabel.Text = Localization.Tr("Find:");
            if (_searchBox != null) _searchBox.Watermark = Localization.Tr("Search visible rows...");
            if (_findMatchCase != null) _findMatchCase.Content = Localization.Tr("Match case");
            if (_findNextButton != null) _findNextButton.Content = Localization.Tr("Find Next (F3)");
            if (_contextText != null) _contextText.Text = Localization.Tr("Context: none selected");
            if (_statusText != null) _statusText.Text = Localization.Tr("LTR status");

            var doublesPrevLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "DoublesPrevLabel");
            if (doublesPrevLabel != null) doublesPrevLabel.Text = Localization.Tr("Previous char:");
            var triplesPrevLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "TriplesPrevLabel");
            if (triplesPrevLabel != null) triplesPrevLabel.Text = Localization.Tr("Previous chars:");

            if (_mainTabControl != null)
            {
                var tabs = _mainTabControl.Items?.Cast<TabItem>()?.ToList() ?? new List<TabItem>();
                if (tabs.Count >= 1) tabs[0].Header = Localization.Tr("Singles");
                if (tabs.Count >= 2) tabs[1].Header = Localization.Tr("Doubles");
                if (tabs.Count >= 3) tabs[2].Header = Localization.Tr("Triples");
            }

            var quickEditLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "QuickEditLabel");
            if (quickEditLabel != null) quickEditLabel.Text = Localization.Tr("Quick Edit");
            var quickEditDesc = EditorHelpers.FindControlSafe<TextBlock>(this, "QuickEditDesc");
            if (quickEditDesc != null) quickEditDesc.Text = Localization.Tr("Applies directly to one character in the selected mode/context.");
            var quickModeLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "QuickModeLabel");
            if (quickModeLabel != null) quickModeLabel.Text = Localization.Tr("Mode:");
            var quickCharLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "QuickCharLabel");
            if (quickCharLabel != null) quickCharLabel.Text = Localization.Tr("Char:");
            var quickStartLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "QuickStartLabel");
            if (quickStartLabel != null) quickStartLabel.Text = Localization.Tr("Start:");
            var quickMiddleLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "QuickMiddleLabel");
            if (quickMiddleLabel != null) quickMiddleLabel.Text = Localization.Tr("Middle:");
            var quickEndLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "QuickEndLabel");
            if (quickEndLabel != null) quickEndLabel.Text = Localization.Tr("End:");
            if (_applyQuickButton != null) _applyQuickButton.Content = Localization.Tr("Apply Quick Edit");

            var distToolsLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "DistToolsLabel");
            if (distToolsLabel != null) distToolsLabel.Text = Localization.Tr("Distribution Tools");
            var distToolsDesc = EditorHelpers.FindControlSafe<TextBlock>(this, "DistToolsDesc");
            if (distToolsDesc != null) distToolsDesc.Text = Localization.Tr("Operate on visible rows only (current mode and context).");
            if (_normalizeVisibleButton != null) _normalizeVisibleButton.Content = Localization.Tr("Normalize Visible Distribution");
            if (_uniformVisibleButton != null) _uniformVisibleButton.Content = Localization.Tr("Set Visible to Uniform");

            var nameGenLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "NameGenLabel");
            if (nameGenLabel != null) nameGenLabel.Text = Localization.Tr("Name Generator Preview");
            var generatorCountLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "GeneratorCountLabel");
            if (generatorCountLabel != null) generatorCountLabel.Text = Localization.Tr("Count:");
            if (_generateNamesButton != null) _generateNamesButton.Content = Localization.Tr("Generate Samples");
            if (_clearGeneratedButton != null) _clearGeneratedButton.Content = Localization.Tr("Clear");

            foreach (var dg in new[] { _tableSingles, _tableDoubles, _tableTriples })
            {
                if (dg?.Columns == null) continue;
                foreach (var col in dg.Columns.OfType<DataGridTextColumn>())
                {
                    if (col.Header?.ToString() == "Context") col.Header = Localization.Tr("Context");
                    else if (col.Header?.ToString() == "Char") col.Header = Localization.Tr("Char");
                    else if (col.Header?.ToString() == "Start") col.Header = Localization.Tr("Start");
                    else if (col.Header?.ToString() == "Middle") col.Header = Localization.Tr("Middle");
                    else if (col.Header?.ToString() == "End") col.Header = Localization.Tr("End");
                }
            }
        }

        private void SetupSignals()
        {
            KeyDown += OnWindowKeyDown;
            Opened += (s, e) =>
            {
                UpdateStatusBar();
                if (_tableSingles != null) _tableSingles.Focus();
            };

            if (_doublePrevCombo != null)
            {
                _doublePrevCombo.SelectionChanged += (s, e) => RefreshDoubleRowsFromModel();
            }
            if (_triplePrev2Combo != null)
            {
                _triplePrev2Combo.SelectionChanged += (s, e) => RefreshTripleRowsFromModel();
            }
            if (_triplePrev1Combo != null)
            {
                _triplePrev1Combo.SelectionChanged += (s, e) => RefreshTripleRowsFromModel();
            }
            if (_quickModeCombo != null)
            {
                _quickModeCombo.SelectionChanged += (s, e) => LoadQuickEditFromSelection();
            }

            if (_tableSingles != null)
            {
                _tableSingles.SelectionChanged += (s, e) =>
                {
                    LoadQuickEditFromSelection();
                    UpdateStatusBar();
                };
                _tableSingles.BeginningEdit += (s, e) => PushState();
            }
            if (_tableDoubles != null)
            {
                _tableDoubles.SelectionChanged += (s, e) =>
                {
                    LoadQuickEditFromSelection();
                    UpdateStatusBar();
                };
                _tableDoubles.BeginningEdit += (s, e) => PushState();
            }
            if (_tableTriples != null)
            {
                _tableTriples.SelectionChanged += (s, e) =>
                {
                    LoadQuickEditFromSelection();
                    UpdateStatusBar();
                };
                _tableTriples.BeginningEdit += (s, e) => PushState();
            }

            if (_applyQuickButton != null)
            {
                _applyQuickButton.Click += (s, e) => ApplyQuickEdit();
            }
            if (_normalizeVisibleButton != null)
            {
                _normalizeVisibleButton.Click += (s, e) => NormalizeVisibleDistribution();
            }
            if (_uniformVisibleButton != null)
            {
                _uniformVisibleButton.Click += (s, e) => SetUniformVisibleDistribution();
            }
            if (_generateNamesButton != null)
            {
                _generateNamesButton.Click += (s, e) => GenerateNameSamples();
            }
            if (_clearGeneratedButton != null)
            {
                _clearGeneratedButton.Click += (s, e) => _generatedNames.Clear();
            }
            if (_findNextButton != null)
            {
                _findNextButton.Click += (s, e) => FindNextMatch();
            }
        }

        private void SetupMenuHandlers()
        {
            void Bind(string name, Action handler)
            {
                try
                {
                    var item = EditorHelpers.FindControlSafe<MenuItem>(this, name);
                    if (item != null)
                    {
                        item.Click += (s, e) => handler();
                    }
                }
                catch
                {
                }
            }

            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
            Bind("actionUndo", Undo);
            Bind("actionRedo", Redo);
            Bind("actionFind", ShowFindDialog);
            Bind("actionFindNext", FindNextMatch);
            Bind("actionNormalizeVisible", NormalizeVisibleDistribution);
            Bind("actionUniformVisible", SetUniformVisibleDistribution);
            Bind("actionGenerateNames", GenerateNameSamples);
        }

        private void HookRowEvents(IEnumerable<ProbabilityRow> rows)
        {
            foreach (var row in rows)
            {
                row.PropertyChanged += OnRowPropertyChanged;
            }
        }

        private void UnhookRowEvents(IEnumerable<ProbabilityRow> rows)
        {
            foreach (var row in rows)
            {
                row.PropertyChanged -= OnRowPropertyChanged;
            }
        }

        private void OnRowPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_suppressModelSync || _undoRedoInProgress)
            {
                return;
            }

            if (e.PropertyName != "Start" && e.PropertyName != "Middle" && e.PropertyName != "End")
            {
                return;
            }

            var row = sender as ProbabilityRow;
            if (row == null)
            {
                return;
            }

            ApplyRowToModel(row);
            UpdateStatusBar();
        }

        private void ApplyRowToModel(ProbabilityRow row)
        {
            if (row == null)
            {
                return;
            }

            string mode = GetQuickMode();
            if (mode == "Singles" || _singleRows.Contains(row))
            {
                _ltr.SetSinglesStart(row.Character, Clamp01(row.Start));
                _ltr.SetSinglesMiddle(row.Character, Clamp01(row.Middle));
                _ltr.SetSinglesEnd(row.Character, Clamp01(row.End));
                return;
            }

            if (mode == "Doubles" || _doubleRows.Contains(row))
            {
                string prev = GetSelectedChar(_doublePrevCombo);
                _ltr.SetDoublesStart(prev, row.Character, Clamp01(row.Start));
                _ltr.SetDoublesMiddle(prev, row.Character, Clamp01(row.Middle));
                _ltr.SetDoublesEnd(prev, row.Character, Clamp01(row.End));
                return;
            }

            string prev2 = GetSelectedChar(_triplePrev2Combo);
            string prev1 = GetSelectedChar(_triplePrev1Combo);
            _ltr.SetTriplesStart(prev2, prev1, row.Character, Clamp01(row.Start));
            _ltr.SetTriplesMiddle(prev2, prev1, row.Character, Clamp01(row.Middle));
            _ltr.SetTriplesEnd(prev2, prev1, row.Character, Clamp01(row.End));
        }

        private static float Clamp01(float value)
        {
            if (value < 0f) return 0f;
            if (value > 1f) return 1f;
            return value;
        }

        private void PushState()
        {
            if (_undoRedoInProgress)
            {
                return;
            }

            try
            {
                var data = Build().Item1;
                if (data == null || data.Length == 0)
                {
                    return;
                }
                _undoStack.Add(data);
                if (_undoStack.Count > UndoMaxLevels)
                {
                    _undoStack.RemoveAt(0);
                }
                _redoStack.Clear();
            }
            catch
            {
            }
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
                var previous = _undoStack[_undoStack.Count - 1];
                _undoStack.RemoveAt(_undoStack.Count - 1);
                _redoStack.Add(Build().Item1);
                LoadFromBytes(previous);
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
                var next = _redoStack[_redoStack.Count - 1];
                _redoStack.RemoveAt(_redoStack.Count - 1);
                _undoStack.Add(Build().Item1);
                LoadFromBytes(next);
            }
            finally
            {
                _undoRedoInProgress = false;
            }
        }

        protected override async Task RunOpenAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null)
            {
                return;
            }

            var options = new FilePickerOpenOptions
            {
                Title = Localization.Tr("Open LTR File"),
                AllowMultiple = false,
                FileTypeFilter = new List<FilePickerFileType>
                {
                    new FilePickerFileType(Localization.Tr("LTR Files")) { Patterns = new[] { "*.ltr" } },
                    new FilePickerFileType(Localization.Tr("All Files")) { Patterns = new[] { "*.*" } }
                }
            };

            var files = await storageProvider.OpenFilePickerAsync(options);
            if (files == null || files.Count == 0)
            {
                return;
            }

            var path = files[0].Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            var data = File.ReadAllBytes(path);
            var resref = Path.GetFileNameWithoutExtension(path) ?? "namegen";
            Load(path, resref, ResourceType.LTR, data);
        }

        protected override async Task RunSaveAsAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null)
            {
                return;
            }

            string suggestedName = string.IsNullOrEmpty(_resname) ? "namegen" : _resname;
            var options = new FilePickerSaveOptions
            {
                Title = Localization.Tr("Save LTR As"),
                SuggestedFileName = suggestedName + ".ltr",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType(Localization.Tr("LTR Files")) { Patterns = new[] { "*.ltr" } }
                }
            };

            var file = await storageProvider.SaveFilePickerAsync(options);
            if (file == null)
            {
                return;
            }

            var path = file.Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            _filepath = path;
            RefreshWindowTitle();
            Save();
        }

        public override void Revert()
        {
            if (_revert == null || _revert.Length == 0)
            {
                return;
            }

            _undoStack.Clear();
            _redoStack.Clear();
            LoadFromBytes(_revert);
        }

        private void LoadFromBytes(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                _ltr = new LTR();
            }
            else
            {
                _ltr = LTRAuto.ReadLtr(data);
            }

            RefreshAllRowsFromModel();
            UpdateStatusBar();
        }

        private void RefreshAllRowsFromModel()
        {
            _suppressModelSync = true;
            try
            {
                RefreshSingleRowsFromModel();
                RefreshDoubleRowsFromModel();
                RefreshTripleRowsFromModel();
                LoadQuickEditFromSelection();
            }
            finally
            {
                _suppressModelSync = false;
            }
        }

        private void RefreshSingleRowsFromModel()
        {
            UnhookRowEvents(_singleRows);
            _singleRows.Clear();
            foreach (var c in _charList)
            {
                _singleRows.Add(new ProbabilityRow(
                    Localization.Tr("single"),
                    c,
                    _ltr.GetSinglesStart(c),
                    _ltr.GetSinglesMiddle(c),
                    _ltr.GetSinglesEnd(c)));
            }
            HookRowEvents(_singleRows);
        }

        private void RefreshDoubleRowsFromModel()
        {
            UnhookRowEvents(_doubleRows);
            _doubleRows.Clear();

            string prev = GetSelectedChar(_doublePrevCombo);
            foreach (var c in _charList)
            {
                _doubleRows.Add(new ProbabilityRow(
                    prev + " _",
                    c,
                    _ltr.GetDoublesStart(prev, c),
                    _ltr.GetDoublesMiddle(prev, c),
                    _ltr.GetDoublesEnd(prev, c)));
            }

            HookRowEvents(_doubleRows);
            UpdateStatusBar();
        }

        private void RefreshTripleRowsFromModel()
        {
            UnhookRowEvents(_tripleRows);
            _tripleRows.Clear();

            string prev2 = GetSelectedChar(_triplePrev2Combo);
            string prev1 = GetSelectedChar(_triplePrev1Combo);
            foreach (var c in _charList)
            {
                _tripleRows.Add(new ProbabilityRow(
                    prev2 + prev1 + " _",
                    c,
                    _ltr.GetTriplesStart(prev2, prev1, c),
                    _ltr.GetTriplesMiddle(prev2, prev1, c),
                    _ltr.GetTriplesEnd(prev2, prev1, c)));
            }

            HookRowEvents(_tripleRows);
            UpdateStatusBar();
        }

        private void NormalizeVisibleDistribution()
        {
            var rows = GetVisibleRows().ToList();
            if (rows.Count == 0)
            {
                return;
            }

            PushState();

            _suppressModelSync = true;
            try
            {
                NormalizeColumn(rows, r => r.Start, (r, v) => r.Start = v);
                NormalizeColumn(rows, r => r.Middle, (r, v) => r.Middle = v);
                NormalizeColumn(rows, r => r.End, (r, v) => r.End = v);
            }
            finally
            {
                _suppressModelSync = false;
            }

            CommitVisibleRowsToModel(rows);
            UpdateStatusBar();
        }

        private static void NormalizeColumn(
            IList<ProbabilityRow> rows,
            Func<ProbabilityRow, float> getter,
            Action<ProbabilityRow, float> setter)
        {
            float sum = rows.Sum(getter);
            if (sum <= 0.000001f)
            {
                float uniform = 1f / rows.Count;
                foreach (var row in rows)
                {
                    setter(row, uniform);
                }
                return;
            }

            foreach (var row in rows)
            {
                setter(row, getter(row) / sum);
            }
        }

        private void SetUniformVisibleDistribution()
        {
            var rows = GetVisibleRows().ToList();
            if (rows.Count == 0)
            {
                return;
            }

            PushState();
            float uniform = 1f / rows.Count;
            _suppressModelSync = true;
            try
            {
                foreach (var row in rows)
                {
                    row.Start = uniform;
                    row.Middle = uniform;
                    row.End = uniform;
                }
            }
            finally
            {
                _suppressModelSync = false;
            }

            CommitVisibleRowsToModel(rows);
            UpdateStatusBar();
        }

        private IEnumerable<ProbabilityRow> GetVisibleRows()
        {
            string mode = GetQuickMode();
            if (mode == "Singles")
            {
                return _singleRows;
            }
            if (mode == "Doubles")
            {
                return _doubleRows;
            }
            return _tripleRows;
        }

        private void CommitVisibleRowsToModel(IEnumerable<ProbabilityRow> rows)
        {
            string mode = GetQuickMode();
            if (mode == "Singles")
            {
                foreach (var row in rows)
                {
                    _ltr.SetSinglesStart(row.Character, Clamp01(row.Start));
                    _ltr.SetSinglesMiddle(row.Character, Clamp01(row.Middle));
                    _ltr.SetSinglesEnd(row.Character, Clamp01(row.End));
                }
                return;
            }

            if (mode == "Doubles")
            {
                string prev = GetSelectedChar(_doublePrevCombo);
                foreach (var row in rows)
                {
                    _ltr.SetDoublesStart(prev, row.Character, Clamp01(row.Start));
                    _ltr.SetDoublesMiddle(prev, row.Character, Clamp01(row.Middle));
                    _ltr.SetDoublesEnd(prev, row.Character, Clamp01(row.End));
                }
                return;
            }

            string prev2 = GetSelectedChar(_triplePrev2Combo);
            string prev1 = GetSelectedChar(_triplePrev1Combo);
            foreach (var row in rows)
            {
                _ltr.SetTriplesStart(prev2, prev1, row.Character, Clamp01(row.Start));
                _ltr.SetTriplesMiddle(prev2, prev1, row.Character, Clamp01(row.Middle));
                _ltr.SetTriplesEnd(prev2, prev1, row.Character, Clamp01(row.End));
            }
        }

        private void LoadQuickEditFromSelection()
        {
            ProbabilityRow selected = GetSelectedRow();
            if (selected == null)
            {
                return;
            }

            if (_quickCharCombo != null)
            {
                _quickCharCombo.SelectedItem = selected.Character;
            }
            if (_quickStartSpin != null)
            {
                _quickStartSpin.Value = (decimal)selected.Start;
            }
            if (_quickMiddleSpin != null)
            {
                _quickMiddleSpin.Value = (decimal)selected.Middle;
            }
            if (_quickEndSpin != null)
            {
                _quickEndSpin.Value = (decimal)selected.End;
            }

            if (_contextText != null)
            {
                _contextText.Text = Localization.Trf("Context: {0}", selected.DisplayContext + selected.Character);
            }
        }

        private ProbabilityRow GetSelectedRow()
        {
            string mode = GetQuickMode();
            if (mode == "Singles")
            {
                return _tableSingles != null ? _tableSingles.SelectedItem as ProbabilityRow : null;
            }
            if (mode == "Doubles")
            {
                return _tableDoubles != null ? _tableDoubles.SelectedItem as ProbabilityRow : null;
            }
            return _tableTriples != null ? _tableTriples.SelectedItem as ProbabilityRow : null;
        }

        private string GetQuickMode()
        {
            int idx = _quickModeCombo?.SelectedIndex ?? 0;
            if (idx == 1) return "Doubles";
            if (idx == 2) return "Triples";
            return "Singles";
        }

        private void ApplyQuickEdit()
        {
            string mode = GetQuickMode();
            string ch = GetSelectedChar(_quickCharCombo);
            float start = _quickStartSpin != null && _quickStartSpin.Value.HasValue ? (float)_quickStartSpin.Value.Value : 0f;
            float middle = _quickMiddleSpin != null && _quickMiddleSpin.Value.HasValue ? (float)_quickMiddleSpin.Value.Value : 0f;
            float end = _quickEndSpin != null && _quickEndSpin.Value.HasValue ? (float)_quickEndSpin.Value.Value : 0f;

            PushState();

            start = Clamp01(start);
            middle = Clamp01(middle);
            end = Clamp01(end);

            if (mode == "Singles")
            {
                _ltr.SetSinglesStart(ch, start);
                _ltr.SetSinglesMiddle(ch, middle);
                _ltr.SetSinglesEnd(ch, end);
                RefreshSingleRowsFromModel();
            }
            else if (mode == "Doubles")
            {
                string prev = GetSelectedChar(_doublePrevCombo);
                _ltr.SetDoublesStart(prev, ch, start);
                _ltr.SetDoublesMiddle(prev, ch, middle);
                _ltr.SetDoublesEnd(prev, ch, end);
                RefreshDoubleRowsFromModel();
            }
            else
            {
                string prev2 = GetSelectedChar(_triplePrev2Combo);
                string prev1 = GetSelectedChar(_triplePrev1Combo);
                _ltr.SetTriplesStart(prev2, prev1, ch, start);
                _ltr.SetTriplesMiddle(prev2, prev1, ch, middle);
                _ltr.SetTriplesEnd(prev2, prev1, ch, end);
                RefreshTripleRowsFromModel();
            }
        }

        private void GenerateNameSamples()
        {
            int count = _generatorCountSpin != null && _generatorCountSpin.Value.HasValue
                ? (int)_generatorCountSpin.Value.Value
                : 25;
            if (count < 1) count = 1;
            if (count > 500) count = 500;

            _generatedNames.Clear();
            for (int i = 0; i < count; i++)
            {
                _generatedNames.Add(_ltr.Generate());
            }
        }

        private void ShowFindDialog()
        {
            if (_searchBox == null)
            {
                return;
            }

            _searchBox.Focus();
            _searchBox.SelectAll();
        }

        private void FindNextMatch()
        {
            if (_searchBox == null)
            {
                return;
            }

            string query = _searchBox.Text ?? "";
            if (string.IsNullOrWhiteSpace(query))
            {
                return;
            }

            bool matchCase = _findMatchCase != null && _findMatchCase.IsChecked == true;
            var rows = GetVisibleRows().ToList();
            if (rows.Count == 0)
            {
                return;
            }

            int start = _findIndex + 1;
            for (int i = 0; i < rows.Count; i++)
            {
                int idx = (start + i) % rows.Count;
                var row = rows[idx];
                if (RowMatches(row, query, matchCase))
                {
                    _findIndex = idx;
                    SelectAndScroll(row);
                    return;
                }
            }
        }

        private static bool RowMatches(ProbabilityRow row, string query, bool matchCase)
        {
            if (row == null)
            {
                return false;
            }

            var comparison = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            if ((row.Character ?? "").IndexOf(query, comparison) >= 0) return true;
            if ((row.DisplayContext ?? "").IndexOf(query, comparison) >= 0) return true;
            if (row.Start.ToString("0.0000").IndexOf(query, comparison) >= 0) return true;
            if (row.Middle.ToString("0.0000").IndexOf(query, comparison) >= 0) return true;
            if (row.End.ToString("0.0000").IndexOf(query, comparison) >= 0) return true;
            return false;
        }

        private void SelectAndScroll(ProbabilityRow row)
        {
            string mode = GetQuickMode();
            if (mode == "Singles" && _tableSingles != null)
            {
                _tableSingles.SelectedItem = row;
                _tableSingles.ScrollIntoView(row, null);
                _tableSingles.Focus();
                return;
            }
            if (mode == "Doubles" && _tableDoubles != null)
            {
                _tableDoubles.SelectedItem = row;
                _tableDoubles.ScrollIntoView(row, null);
                _tableDoubles.Focus();
                return;
            }
            if (_tableTriples != null)
            {
                _tableTriples.SelectedItem = row;
                _tableTriples.ScrollIntoView(row, null);
                _tableTriples.Focus();
            }
        }

        private void UpdateStatusBar()
        {
            if (_statusText == null)
            {
                return;
            }

            string mode = GetQuickMode();
            string doublePrev = GetSelectedChar(_doublePrevCombo);
            string triplePrev2 = GetSelectedChar(_triplePrev2Combo);
            string triplePrev1 = GetSelectedChar(_triplePrev1Combo);
            int selectedCount = GetSelectedRow() != null ? 1 : 0;
            _statusText.Text = Localization.Trf("Charset: {0} | Mode: {1} | Doubles Prev: {2} | Triples Prev: {3}{4} | Selected: {5}",
                LTR.NumCharacters,
                mode,
                doublePrev,
                triplePrev2,
                triplePrev1,
                selectedCount);
        }

        private static string GetSelectedChar(ComboBox combo)
        {
            var selected = combo != null && combo.SelectedItem != null ? combo.SelectedItem.ToString() : null;
            if (string.IsNullOrWhiteSpace(selected))
            {
                return LTR.CharacterSet.Substring(0, 1);
            }
            return selected;
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            bool ctrl = (e.KeyModifiers & KeyModifiers.Control) != 0;
            if (ctrl && e.Key == Key.S) { Save(); e.Handled = true; return; }
            if (ctrl && e.Key == Key.O) { _ = RunOpenAsync(); e.Handled = true; return; }
            if (ctrl && e.Key == Key.N) { New(); e.Handled = true; return; }
            if (ctrl && e.Key == Key.Z) { Undo(); e.Handled = true; return; }
            if (ctrl && e.Key == Key.Y) { Redo(); e.Handled = true; return; }
            if (ctrl && e.Key == Key.F) { ShowFindDialog(); e.Handled = true; return; }
            if (e.Key == Key.F3) { FindNextMatch(); e.Handled = true; }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _undoStack.Clear();
            _redoStack.Clear();
            LoadFromBytes(data);
        }

        public override Tuple<byte[], byte[]> Build()
        {
            byte[] data = LTRAuto.BytesLtr(_ltr);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            _ltr = new LTR();
            _generatedNames.Clear();
            RefreshAllRowsFromModel();
            UpdateStatusBar();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }
    }
}
