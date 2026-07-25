using BioWare.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Utils;
using OdyTools.Widgets;

namespace OdyTools.Editors
{
    public partial class OdyToolIFO : Editor
    {
        private const int MinEditorWidth = 480;
        private const int MinEditorHeight = 520;
        private const int UndoMaxLevels = 30;
        private static readonly string[] ScriptNames =
        {
            "on_heartbeat", "on_load", "on_start", "on_enter", "on_leave",
            "on_activate_item", "on_acquire_item", "on_user_defined", "on_unacquire_item",
            "on_player_death", "on_player_dying", "on_player_levelup", "on_player_respawn",
            "on_player_rest", "start_movie"
        };

        private IFO _ifo;
        private TextBlock _statusText;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;
        private string _findText = "";
        private bool _findMatchCase;
        private int _findCursorIndex = -1;
        private string _lastFindFieldKey = "";

        // Public property to access IFO for testing (matching Python's self.ifo)
        public IFO Ifo => _ifo;

        // Public properties for testing: use generated name references (TagEdit, VoIdEdit, etc. from AXAML).
        // ScriptFields is not a named control in XAML.
        public Dictionary<string, ComboBox> ScriptFields => _scriptFields;
        internal LocalizedStringEdit NameEditForTest => _nameEdit;
        internal LocalizedStringEdit DescEditForTest => _descEdit;
        internal TextBox TagEditForTest => _tagEdit;
        internal Button TagGenerateButtonForTest => _tagGenerateButton;
        internal TextBox VoIdEditForTest => _voIdEdit;
        internal TextBox HakEditForTest => _hakEdit;
        internal TextBox EntryResrefEditForTest => _entryResrefEdit;
        internal NumericUpDown EntryXSpinForTest => _entryXSpin;
        internal NumericUpDown EntryYSpinForTest => _entryYSpin;
        internal NumericUpDown EntryZSpinForTest => _entryZSpin;
        internal NumericUpDown EntryDirSpinForTest => _entryDirSpin;
        internal NumericUpDown DawnHourSpinForTest => _dawnHourSpin;
        internal NumericUpDown DuskHourSpinForTest => _duskHourSpin;
        internal NumericUpDown TimeScaleSpinForTest => _timeScaleSpin;
        internal NumericUpDown StartMonthSpinForTest => _startMonthSpin;
        internal NumericUpDown StartDaySpinForTest => _startDaySpin;
        internal NumericUpDown StartHourSpinForTest => _startHourSpin;
        internal NumericUpDown StartYearSpinForTest => _startYearSpin;
        internal NumericUpDown XpScaleSpinForTest => _xpScaleSpin;
        internal string LastFindFieldKeyForTest => _lastFindFieldKey;

        // UI Controls - Basic Info
        private LocalizedStringEdit _nameEdit;
        private Button _nameEditBtn;
        private TextBox _tagEdit;
        private Button _tagGenerateButton;
        private TextBox _voIdEdit;
        private TextBox _hakEdit;
        private LocalizedStringEdit _descEdit;
        private Button _descEditBtn;

        // UI Controls - Entry Point
        private TextBox _entryResrefEdit;
        private NumericUpDown _entryXSpin;
        private NumericUpDown _entryYSpin;
        private NumericUpDown _entryZSpin;
        private NumericUpDown _entryDirSpin;

        // UI Controls - Time Settings
        private NumericUpDown _dawnHourSpin;
        private NumericUpDown _duskHourSpin;
        private NumericUpDown _timeScaleSpin;
        private NumericUpDown _startMonthSpin;
        private NumericUpDown _startDaySpin;
        private NumericUpDown _startHourSpin;
        private NumericUpDown _startYearSpin;
        private NumericUpDown _xpScaleSpin;

        // UI Controls - Scripts (editable combos with prefilled script resnames)
        private Dictionary<string, ComboBox> _scriptFields;

        internal bool HasStructuredEditorSurface =>
            _nameEdit != null &&
            _tagEdit != null &&
            _voIdEdit != null &&
            _hakEdit != null &&
            _descEdit != null &&
            _entryResrefEdit != null &&
            _entryXSpin != null &&
            _entryYSpin != null &&
            _entryZSpin != null &&
            _entryDirSpin != null &&
            _dawnHourSpin != null &&
            _duskHourSpin != null &&
            _timeScaleSpin != null &&
            _startMonthSpin != null &&
            _startDaySpin != null &&
            _startHourSpin != null &&
            _startYearSpin != null &&
            _xpScaleSpin != null &&
            _scriptFields != null &&
            ScriptNames.All(_scriptFields.ContainsKey);

        public OdyToolIFO() : this(null, null) { }
        public OdyToolIFO(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolIFO", "ifo",
                new[] { ResourceType.IFO, ResourceType.IFO_XML },
                new[] { ResourceType.IFO, ResourceType.IFO_XML },
                installation)
        {
            InitializeComponent();
            SetupUI();
            SetupMenuHandlers();
            AddHelpAction(); // Auto-detects "GFF-IFO.md" for IFO
            New();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
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

        private void SetupProgrammaticUI()
        {
            var scrollViewer = new ScrollViewer();
            var mainPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Basic Info Group
            var basicGroup = new Expander { Header = "Basic Information", IsExpanded = true };
            var basicPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Module Name
            var nameLabel = new TextBlock { Text = "Module Name:" };
            _nameEdit = new LocalizedStringEdit();
            basicPanel.Children.Add(nameLabel);
            basicPanel.Children.Add(_nameEdit);

            // Tag
            var tagLabel = new TextBlock { Text = "Tag:" };
            _tagEdit = new TextBox();
            _tagEdit.TextChanged += (s, e) => OnValueChanged();
            _tagGenerateButton = new Button { Content = "Generate" };
            _tagGenerateButton.Click += (s, e) => GenerateTag();
            basicPanel.Children.Add(tagLabel);
            basicPanel.Children.Add(_tagEdit);
            basicPanel.Children.Add(_tagGenerateButton);

            AttachReferenceSearchMenus();

            // VO ID
            var voIdLabel = new TextBlock { Text = "VO ID:" };
            _voIdEdit = new TextBox();
            _voIdEdit.TextChanged += (s, e) => OnValueChanged();
            basicPanel.Children.Add(voIdLabel);
            basicPanel.Children.Add(_voIdEdit);

            // Hak
            var hakLabel = new TextBlock { Text = "Hak:" };
            _hakEdit = new TextBox();
            _hakEdit.TextChanged += (s, e) => OnValueChanged();
            basicPanel.Children.Add(hakLabel);
            basicPanel.Children.Add(_hakEdit);

            // Description
            var descLabel = new TextBlock { Text = "Description:" };
            _descEdit = new LocalizedStringEdit();
            basicPanel.Children.Add(descLabel);
            basicPanel.Children.Add(_descEdit);

            basicGroup.Content = basicPanel;
            mainPanel.Children.Add(basicGroup);

            // Entry Point Group
            var entryGroup = new Expander { Header = "Entry Point", IsExpanded = true };
            var entryPanel = new StackPanel { Orientation = Orientation.Vertical };

            var entryResrefLabel = new TextBlock { Text = "Area ResRef:" };
            _entryResrefEdit = new TextBox();
            _entryResrefEdit.TextChanged += (s, e) => OnValueChanged();
            entryPanel.Children.Add(entryResrefLabel);
            entryPanel.Children.Add(_entryResrefEdit);

            var entryXLabel = new TextBlock { Text = "Entry X:" };
            _entryXSpin = new NumericUpDown { Minimum = -99999, Maximum = 99999, Increment = 0.1m };
            _entryXSpin.ValueChanged += (s, e) => OnValueChanged();
            entryPanel.Children.Add(entryXLabel);
            entryPanel.Children.Add(_entryXSpin);

            var entryYLabel = new TextBlock { Text = "Entry Y:" };
            _entryYSpin = new NumericUpDown { Minimum = -99999, Maximum = 99999, Increment = 0.1m };
            _entryYSpin.ValueChanged += (s, e) => OnValueChanged();
            entryPanel.Children.Add(entryYLabel);
            entryPanel.Children.Add(_entryYSpin);

            var entryZLabel = new TextBlock { Text = "Entry Z:" };
            _entryZSpin = new NumericUpDown { Minimum = -99999, Maximum = 99999, Increment = 0.1m };
            _entryZSpin.ValueChanged += (s, e) => OnValueChanged();
            entryPanel.Children.Add(entryZLabel);
            entryPanel.Children.Add(_entryZSpin);

            var entryDirLabel = new TextBlock { Text = "Entry Direction:" };
            _entryDirSpin = new NumericUpDown { Minimum = 0, Maximum = 360, Increment = 1 };
            _entryDirSpin.ValueChanged += (s, e) => OnValueChanged();
            entryPanel.Children.Add(entryDirLabel);
            entryPanel.Children.Add(_entryDirSpin);

            entryGroup.Content = entryPanel;
            mainPanel.Children.Add(entryGroup);

            // Time Settings Group
            var timeGroup = new Expander { Header = "Time Settings", IsExpanded = true };
            var timePanel = new StackPanel { Orientation = Orientation.Vertical };

            var dawnHourLabel = new TextBlock { Text = "Dawn Hour:" };
            _dawnHourSpin = new NumericUpDown { Minimum = 0, Maximum = 23 };
            _dawnHourSpin.ValueChanged += (s, e) => OnValueChanged();
            timePanel.Children.Add(dawnHourLabel);
            timePanel.Children.Add(_dawnHourSpin);

            var duskHourLabel = new TextBlock { Text = "Dusk Hour:" };
            _duskHourSpin = new NumericUpDown { Minimum = 0, Maximum = 23 };
            _duskHourSpin.ValueChanged += (s, e) => OnValueChanged();
            timePanel.Children.Add(duskHourLabel);
            timePanel.Children.Add(_duskHourSpin);

            var timeScaleLabel = new TextBlock { Text = "Time Scale:" };
            _timeScaleSpin = new NumericUpDown { Minimum = 0, Maximum = 100 };
            _timeScaleSpin.ValueChanged += (s, e) => OnValueChanged();
            timePanel.Children.Add(timeScaleLabel);
            timePanel.Children.Add(_timeScaleSpin);

            var startMonthLabel = new TextBlock { Text = "Start Month:" };
            _startMonthSpin = new NumericUpDown { Minimum = 1, Maximum = 12 };
            _startMonthSpin.ValueChanged += (s, e) => OnValueChanged();
            timePanel.Children.Add(startMonthLabel);
            timePanel.Children.Add(_startMonthSpin);

            var startDayLabel = new TextBlock { Text = "Start Day:" };
            _startDaySpin = new NumericUpDown { Minimum = 1, Maximum = 31 };
            _startDaySpin.ValueChanged += (s, e) => OnValueChanged();
            timePanel.Children.Add(startDayLabel);
            timePanel.Children.Add(_startDaySpin);

            var startHourLabel = new TextBlock { Text = "Start Hour:" };
            _startHourSpin = new NumericUpDown { Minimum = 0, Maximum = 23 };
            _startHourSpin.ValueChanged += (s, e) => OnValueChanged();
            timePanel.Children.Add(startHourLabel);
            timePanel.Children.Add(_startHourSpin);

            var startYearLabel = new TextBlock { Text = "Start Year:" };
            _startYearSpin = new NumericUpDown { Minimum = 0, Maximum = 9999 };
            _startYearSpin.ValueChanged += (s, e) => OnValueChanged();
            timePanel.Children.Add(startYearLabel);
            timePanel.Children.Add(_startYearSpin);

            var xpScaleLabel = new TextBlock { Text = "XP Scale:" };
            _xpScaleSpin = new NumericUpDown { Minimum = 0, Maximum = 100 };
            _xpScaleSpin.ValueChanged += (s, e) => OnValueChanged();
            timePanel.Children.Add(xpScaleLabel);
            timePanel.Children.Add(_xpScaleSpin);

            timeGroup.Content = timePanel;
            mainPanel.Children.Add(timeGroup);

            // Scripts Group
            var scriptGroup = new Expander { Header = "Scripts", IsExpanded = true };
            var scriptPanel = new StackPanel { Orientation = Orientation.Vertical };
            _scriptFields = new Dictionary<string, ComboBox>();

            AddProgrammaticScriptFields(scriptPanel);

            scriptGroup.Content = scriptPanel;
            mainPanel.Children.Add(scriptGroup);

            scrollViewer.Content = mainPanel;
            var dock = new DockPanel();
            var menu = BuildMenu();
            dock.Children.Add(menu);
            DockPanel.SetDock(menu, Dock.Top);
            dock.Children.Add(scrollViewer);
            _statusText = new TextBlock { Text = "Module Info", Margin = new Avalonia.Thickness(4, 2) };
            dock.Children.Add(_statusText);
            DockPanel.SetDock(_statusText, Dock.Bottom);
            Content = dock;
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
            editMenu.Items.Add(new MenuItem { Header = "_Find...", Name = "actionFind" });
            editMenu.Items.Add(new MenuItem { Header = "Find _Next", Name = "actionFindNext" });
            menu.Items.Add(editMenu);
            return menu;
        }

        private void AttachReferenceSearchMenus()
        {
            if (_tagEdit == null)
            {
                return;
            }

            ReferenceSearchHelper.AttachTagFindReferencesMenu(_tagEdit, this, _installation);
        }

        private void AttachCommitHandlers()
        {
            void OnCommit(object s, EventArgs e) { if (!_undoRedoInProgress) PushState(); }
            EditorHelpers.BindLostFocus(_tagEdit, OnCommit);
            EditorHelpers.BindLostFocus(_voIdEdit, OnCommit);
            EditorHelpers.BindLostFocus(_hakEdit, OnCommit);
            EditorHelpers.BindLostFocus(_entryResrefEdit, OnCommit);
            EditorHelpers.BindLostFocus(_entryXSpin, OnCommit);
            EditorHelpers.BindLostFocus(_entryYSpin, OnCommit);
            EditorHelpers.BindLostFocus(_entryZSpin, OnCommit);
            EditorHelpers.BindLostFocus(_entryDirSpin, OnCommit);
            EditorHelpers.BindLostFocus(_dawnHourSpin, OnCommit);
            EditorHelpers.BindLostFocus(_duskHourSpin, OnCommit);
            EditorHelpers.BindLostFocus(_timeScaleSpin, OnCommit);
            EditorHelpers.BindLostFocus(_startMonthSpin, OnCommit);
            EditorHelpers.BindLostFocus(_startDaySpin, OnCommit);
            EditorHelpers.BindLostFocus(_startHourSpin, OnCommit);
            EditorHelpers.BindLostFocus(_startYearSpin, OnCommit);
            EditorHelpers.BindLostFocus(_xpScaleSpin, OnCommit);
            if (_scriptFields != null)
                foreach (var kv in _scriptFields)
                    EditorHelpers.BindLostFocus(kv.Value, OnCommit);
        }

        private void SetupScriptComboBoxContextMenu(ComboBox comboBox, string scriptTypeName)
        {
            if (comboBox == null) return;

            var contextMenu = new ContextMenu();
            var openInEditorItem = new MenuItem { Header = "Open in OdyToolNSS", IsEnabled = false };
            openInEditorItem.Click += (sender, e) => OpenScriptInEditor(comboBox, scriptTypeName);
            contextMenu.Items.Add(openInEditorItem);

            var findReferencesItem = new MenuItem { Header = "Find References", IsEnabled = false };
            findReferencesItem.Click += (sender, e) => ScriptReferenceHelper.FindAndShowScriptReferences(this, comboBox, _installation);
            contextMenu.Items.Add(findReferencesItem);

            void UpdateOpenEnabled(object s, EventArgs e)
            {
                string text = comboBox.SelectedItem?.ToString() ?? comboBox.Text ?? string.Empty;
                bool hasScript = !string.IsNullOrWhiteSpace(text);
                openInEditorItem.IsEnabled = hasScript;
                findReferencesItem.IsEnabled = hasScript && _installation != null;
            }
            comboBox.SelectionChanged += UpdateOpenEnabled;
            contextMenu.Opened += (s, e) => UpdateOpenEnabled(s, e);
            comboBox.ContextMenu = contextMenu;
        }

        private void ConfigureScriptField(ComboBox scriptCombo, string scriptName)
        {
            if (scriptCombo == null)
            {
                return;
            }

            scriptCombo.IsEditable = true;
            scriptCombo.GetObservable(ComboBox.TextProperty).Subscribe(_ => OnValueChanged());
            SetupScriptComboBoxContextMenu(scriptCombo, scriptName);
            _scriptFields[scriptName] = scriptCombo;
        }

        private static string FormatScriptLabel(string scriptName)
        {
            return scriptName.Replace("_", " ").ToUpperInvariant() + ":";
        }

        private void AddProgrammaticScriptFields(Panel scriptPanel)
        {
            foreach (string scriptName in ScriptNames)
            {
                var label = new TextBlock { Text = FormatScriptLabel(scriptName) };
                var edit = new ComboBox();
                ConfigureScriptField(edit, scriptName);
                scriptPanel.Children.Add(label);
                scriptPanel.Children.Add(edit);
            }
        }

        private void AddXamlScriptFields(Panel scriptPanel)
        {
            foreach (string scriptName in ScriptNames)
            {
                var label = new TextBlock { Text = FormatScriptLabel(scriptName), Margin = new Avalonia.Thickness(0, 4, 12, 4) };
                var edit = new ComboBox { Margin = new Avalonia.Thickness(0, 0, 0, 8) };
                ConfigureScriptField(edit, scriptName);

                var row = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*") };
                row.Children.Add(label);
                Grid.SetColumn(label, 0);
                row.Children.Add(edit);
                Grid.SetColumn(edit, 1);
                scriptPanel.Children.Add(row);
            }
        }

        private void OpenScriptInEditor(ComboBox comboBox, string scriptTypeName)
        {
            if (comboBox == null || _installation == null) return;
            string scriptName = comboBox.Text?.Trim();
            if (string.IsNullOrEmpty(scriptName)) return;

            try
            {
                var resourceResult = _installation.Resource(scriptName, ResourceType.NSS, null);
                var resourceType = ResourceType.NSS;
                if (resourceResult == null)
                {
                    resourceResult = _installation.Resource(scriptName, ResourceType.NCS, null);
                    resourceType = ResourceType.NCS;
                }
                if (resourceResult == null)
                {
                    System.Console.WriteLine($"Script '{scriptName}' not found in installation.");
                    return;
                }
                byte[] data = resourceResult.Data;
                if (data == null && !string.IsNullOrEmpty(resourceResult.FilePath) && System.IO.File.Exists(resourceResult.FilePath))
                    data = System.IO.File.ReadAllBytes(resourceResult.FilePath);
                if (data == null)
                {
                    System.Console.WriteLine($"No data for script '{scriptName}'.");
                    return;
                }
                WindowUtils.OpenResourceEditor(resourceResult.FilePath, scriptName, resourceType, data, _installation, this);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"OpenScriptInEditor failed: {ex.Message}");
            }
        }

        private void PopulateScriptComboBoxes()
        {
            if (_installation == null || _scriptFields == null) return;
            try
            {
                var relevantResources = _installation.GetRelevantResources(ResourceType.NCS, FilepathPublic);
                var resnames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (relevantResources != null)
                {
                    foreach (var res in relevantResources)
                    {
                        if (res != null && !string.IsNullOrEmpty(res.ResName))
                            resnames.Add(res.ResName.ToLowerInvariant());
                    }
                }
                var sorted = resnames.OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToList();
                foreach (var kv in _scriptFields)
                {
                    if (kv.Value == null) continue;
                    kv.Value.Items.Clear();
                    foreach (string r in sorted)
                        kv.Value.Items.Add(r);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to populate script combo boxes: {ex.Message}");
            }
        }

        private void SetupMenuHandlers()
        {
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
            EditorHelpers.BindMenuClicks(this, new (string menuItemName, Action handler)[]
            {
                ("actionUndo", Undo),
                ("actionRedo", Redo),
                ("actionFind", ShowFindDialog),
                ("actionFindNext", () => FindNextMatch()),
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
                _ifo = new IFO();
            else
            {
                try
                {
                    var gff = GFFAuto.ReadGff(data, fileFormat: _restype ?? ResourceType.IFO);
                    _ifo = IFOHelpers.ConstructIfo(gff);
                }
                catch
                {
                    _ifo = new IFO();
                }
            }
            _undoRedoInProgress = true;
            try
            {
                UpdateUIFromIFO();
                PopulateScriptComboBoxes();
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
                string text = _ifo != null ? "Module Info" : "No module";
                if (_ifo != null && !string.IsNullOrEmpty(_ifo.Tag)) text += " | " + _ifo.Tag;
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

        private bool FindNextMatch()
        {
            if (string.IsNullOrEmpty(_findText))
            {
                _lastFindFieldKey = "";
                return false;
            }

            var fields = BuildFindFields();
            if (fields.Count == 0)
            {
                _lastFindFieldKey = "";
                return false;
            }

            string t = _findMatchCase ? _findText : _findText.ToLowerInvariant();
            bool Match(string value) => value != null && (_findMatchCase ? value : value.ToLowerInvariant()).Contains(t);

            int start = Math.Max(-1, Math.Min(_findCursorIndex, fields.Count - 1));
            for (int offset = 1; offset <= fields.Count; offset++)
            {
                int index = (start + offset) % fields.Count;
                var field = fields[index];
                if (!Match(field.value))
                {
                    continue;
                }

                _findCursorIndex = index;
                _lastFindFieldKey = field.key;
                field.control?.Focus();
                return true;
            }

            _lastFindFieldKey = "";
            return false;
        }

        private List<(string key, Control control, string value)> BuildFindFields()
        {
            var fields = new List<(string key, Control control, string value)>();
            if (_tagEdit != null) fields.Add(("tag", _tagEdit, _tagEdit.Text));
            if (_voIdEdit != null) fields.Add(("vo_id", _voIdEdit, _voIdEdit.Text));
            if (_hakEdit != null) fields.Add(("hak", _hakEdit, _hakEdit.Text));
            if (_entryResrefEdit != null) fields.Add(("entry_resref", _entryResrefEdit, _entryResrefEdit.Text));
            if (_scriptFields != null)
            {
                foreach (var kv in _scriptFields)
                {
                    if (kv.Value != null) fields.Add((kv.Key, kv.Value, kv.Value.Text));
                }
            }

            return fields;
        }

        internal void SetFindQueryForTest(string text, bool matchCase = false)
        {
            if (!string.Equals(_findText, text ?? "", StringComparison.Ordinal) || _findMatchCase != matchCase)
            {
                _findCursorIndex = -1;
                _lastFindFieldKey = "";
            }

            _findText = text ?? "";
            _findMatchCase = matchCase;
        }

        internal bool FindNextForTest()
        {
            return FindNextMatch();
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (e.KeyModifiers & KeyModifiers.Control) != 0) { Save(); e.Handled = true; return; }
            if (e.Key == Key.Z && (e.KeyModifiers & KeyModifiers.Control) != 0) { Undo(); e.Handled = true; return; }
            if (e.Key == Key.Y && (e.KeyModifiers & KeyModifiers.Control) != 0) { Redo(); e.Handled = true; return; }
            if (e.Key == Key.F && (e.KeyModifiers & KeyModifiers.Control) != 0) { ShowFindDialog(); e.Handled = true; return; }
            if (e.Key == Key.F3) { FindNextMatch(); e.Handled = true; }
        }

        private void SetupUI()
        {
            // Try to find controls from XAML if available (only if not already set by programmatic UI)
            if (_nameEdit == null) _nameEdit = EditorHelpers.FindControlSafe<LocalizedStringEdit>(this, "NameEdit");
            if (_nameEditBtn == null) _nameEditBtn = EditorHelpers.FindControlSafe<Button>(this, "NameEditBtn");
            if (_tagEdit == null) _tagEdit = EditorHelpers.FindControlSafe<TextBox>(this, "TagEdit");
            if (_tagGenerateButton == null) _tagGenerateButton = EditorHelpers.FindControlSafe<Button>(this, "TagGenerateButton");
            if (_voIdEdit == null) _voIdEdit = EditorHelpers.FindControlSafe<TextBox>(this, "VoIdEdit");
            if (_hakEdit == null) _hakEdit = EditorHelpers.FindControlSafe<TextBox>(this, "HakEdit");
            if (_descEdit == null) _descEdit = EditorHelpers.FindControlSafe<LocalizedStringEdit>(this, "DescEdit");
            if (_descEditBtn == null) _descEditBtn = EditorHelpers.FindControlSafe<Button>(this, "DescEditBtn");
            if (_entryResrefEdit == null) _entryResrefEdit = EditorHelpers.FindControlSafe<TextBox>(this, "EntryResrefEdit");
            if (_entryXSpin == null) _entryXSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "EntryXSpin");
            if (_entryYSpin == null) _entryYSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "EntryYSpin");
            if (_entryZSpin == null) _entryZSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "EntryZSpin");
            if (_entryDirSpin == null) _entryDirSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "EntryDirSpin");
            if (_dawnHourSpin == null) _dawnHourSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "DawnHourSpin");
            if (_duskHourSpin == null) _duskHourSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "DuskHourSpin");
            if (_timeScaleSpin == null) _timeScaleSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "TimeScaleSpin");
            if (_startMonthSpin == null) _startMonthSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "StartMonthSpin");
            if (_startDaySpin == null) _startDaySpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "StartDaySpin");
            if (_startHourSpin == null) _startHourSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "StartHourSpin");
            if (_startYearSpin == null) _startYearSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "StartYearSpin");
            if (_xpScaleSpin == null) _xpScaleSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "XpScaleSpin");
            if (_statusText == null) _statusText = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
            // When XAML provides scriptPanel, build script fields and add to it
            var scriptPanel = EditorHelpers.FindControlSafe<StackPanel>(this, "scriptPanel");
            if (scriptPanel != null && (_scriptFields == null || _scriptFields.Count == 0))
            {
                _scriptFields = new Dictionary<string, ComboBox>();
                AddXamlScriptFields(scriptPanel);
            }
            var findBtn = EditorHelpers.FindControlSafe<Button>(this, "findButton");
            if (findBtn != null) findBtn.Click += (s, e) => ShowFindDialog();
            if (_tagGenerateButton != null) _tagGenerateButton.Click += (s, e) => GenerateTag();
            AttachCommitHandlers();
            AttachReferenceSearchMenus();
            SetupLocalizedStringEditors();
            Opened += (s, e) => { UpdateStatusBar(); _tagEdit?.Focus(); };
            KeyDown += OnWindowKeyDown;
        }

        private void SetupLocalizedStringEditors()
        {
            _nameEdit?.SetInstallation(_installation);
            _descEdit?.SetInstallation(_installation);
        }

        private void EditName()
        {
            if (_ifo == null || _installation == null)
            {
                return;
            }
            var dialog = new LocalizedStringDialog(this, _installation, _ifo.ModName);
            if (dialog.ShowDialog())
            {
                _nameEdit?.SetLocString(dialog.LocString);
                UpdateUIFromIFO();
            }
        }

        private void GenerateTag()
        {
            if (_ifo == null || _tagEdit == null)
            {
                return;
            }

            string source = GetModuleNameTextForTag();
            if (string.IsNullOrWhiteSpace(source))
            {
                source = _resname;
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                return;
            }

            string tag = SanitizeGeneratedTag(source);
            if (string.IsNullOrEmpty(tag))
            {
                return;
            }

            _tagEdit.Text = tag;
            OnValueChanged();
            MarkDocumentDirty();
        }

        private string GetModuleNameTextForTag()
        {
            var locstring = _nameEdit?.GetLocString();
            if (locstring == null)
            {
                return "";
            }

            string name = locstring.Get(Language.English, Gender.Male) ?? locstring.Get(Language.English, Gender.Male, true);
            if (string.IsNullOrWhiteSpace(name) && locstring.StringRef != -1 && _installation != null)
            {
                name = _installation.String(locstring) ?? "";
            }

            return name ?? "";
        }

        private static string SanitizeGeneratedTag(string source)
        {
            var chars = new List<char>();
            foreach (char c in source)
            {
                if (chars.Count >= 32)
                {
                    break;
                }

                chars.Add(char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : '_');
            }

            return new string(chars.ToArray()).Trim('_');
        }

        private void EditDescription()
        {
            if (_ifo == null || _installation == null)
            {
                return;
            }
            var dialog = new LocalizedStringDialog(this, _installation, _ifo.Description);
            if (dialog.ShowDialog())
            {
                _descEdit?.SetLocString(dialog.LocString);
                UpdateUIFromIFO();
            }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _undoStack.Clear();
            _redoStack.Clear();
            try { LoadFromBytes(data); }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load IFO: {ex}");
                _ifo = new IFO();
                LoadFromBytes(null);
            }
        }

        public override Tuple<byte[], byte[]> Build()
        {
            if (_ifo == null)
            {
                return Tuple.Create(new byte[0], new byte[0]);
            }

            ReadUIIntoIfo();
            var gff = IFOHelpers.DismantleIfo(_ifo);
            ResourceType outputType = _restype == ResourceType.IFO_XML ? ResourceType.IFO_XML : ResourceType.IFO;
            byte[] data = GFFAuto.BytesGff(gff, outputType);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            _ifo = new IFO();
            UpdateUIFromIFO();
            PopulateScriptComboBoxes();
        }

        private void UpdateUIFromIFO()
        {
            if (_ifo == null)
            {
                return;
            }

            // Basic Info
            _nameEdit?.SetLocString(_ifo.ModName);
            if (_tagEdit != null) _tagEdit.Text = _ifo.Tag ?? "";
            if (_voIdEdit != null) _voIdEdit.Text = _ifo.VoId ?? "";
            if (_hakEdit != null) _hakEdit.Text = _ifo.Hak ?? "";
            _descEdit?.SetLocString(_ifo.Description);

            // Entry Point
            if (_entryResrefEdit != null) _entryResrefEdit.Text = _ifo.ResRef.ToString();
            if (_entryXSpin != null) _entryXSpin.Value = (decimal)_ifo.EntryX;
            if (_entryYSpin != null) _entryYSpin.Value = (decimal)_ifo.EntryY;
            if (_entryZSpin != null) _entryZSpin.Value = (decimal)_ifo.EntryZ;
            if (_entryDirSpin != null) _entryDirSpin.Value = (decimal)RadiansToDegrees(_ifo.EntryDirection);

            // Time Settings
            if (_dawnHourSpin != null) _dawnHourSpin.Value = _ifo.DawnHour;
            if (_duskHourSpin != null) _duskHourSpin.Value = _ifo.DuskHour;
            if (_timeScaleSpin != null) _timeScaleSpin.Value = _ifo.TimeScale;
            if (_startMonthSpin != null) _startMonthSpin.Value = _ifo.StartMonth;
            if (_startDaySpin != null) _startDaySpin.Value = _ifo.StartDay;
            if (_startHourSpin != null) _startHourSpin.Value = _ifo.StartHour;
            if (_startYearSpin != null) _startYearSpin.Value = _ifo.StartYear;
            if (_xpScaleSpin != null) _xpScaleSpin.Value = _ifo.XpScale;

            // Scripts
            if (_scriptFields != null)
            {
                if (_scriptFields.ContainsKey("on_heartbeat") && _scriptFields["on_heartbeat"] != null)
                    _scriptFields["on_heartbeat"].Text = _ifo.OnHeartbeat.ToString();
                if (_scriptFields.ContainsKey("on_load") && _scriptFields["on_load"] != null)
                    _scriptFields["on_load"].Text = _ifo.OnLoad.ToString();
                if (_scriptFields.ContainsKey("on_start") && _scriptFields["on_start"] != null)
                    _scriptFields["on_start"].Text = _ifo.OnStart.ToString();
                if (_scriptFields.ContainsKey("on_enter") && _scriptFields["on_enter"] != null)
                    _scriptFields["on_enter"].Text = _ifo.OnClientEnter.ToString();
                if (_scriptFields.ContainsKey("on_leave") && _scriptFields["on_leave"] != null)
                    _scriptFields["on_leave"].Text = _ifo.OnClientLeave.ToString();
                if (_scriptFields.ContainsKey("on_activate_item") && _scriptFields["on_activate_item"] != null)
                    _scriptFields["on_activate_item"].Text = _ifo.OnActivateItem.ToString();
                if (_scriptFields.ContainsKey("on_acquire_item") && _scriptFields["on_acquire_item"] != null)
                    _scriptFields["on_acquire_item"].Text = _ifo.OnAcquireItem.ToString();
                if (_scriptFields.ContainsKey("on_user_defined") && _scriptFields["on_user_defined"] != null)
                    _scriptFields["on_user_defined"].Text = _ifo.OnUserDefined.ToString();
                if (_scriptFields.ContainsKey("on_unacquire_item") && _scriptFields["on_unacquire_item"] != null)
                    _scriptFields["on_unacquire_item"].Text = _ifo.OnUnacquireItem.ToString();
                if (_scriptFields.ContainsKey("on_player_death") && _scriptFields["on_player_death"] != null)
                    _scriptFields["on_player_death"].Text = _ifo.OnPlayerDeath.ToString();
                if (_scriptFields.ContainsKey("on_player_dying") && _scriptFields["on_player_dying"] != null)
                    _scriptFields["on_player_dying"].Text = _ifo.OnPlayerDying.ToString();
                if (_scriptFields.ContainsKey("on_player_levelup") && _scriptFields["on_player_levelup"] != null)
                    _scriptFields["on_player_levelup"].Text = _ifo.OnPlayerLevelUp.ToString();
                if (_scriptFields.ContainsKey("on_player_respawn") && _scriptFields["on_player_respawn"] != null)
                    _scriptFields["on_player_respawn"].Text = _ifo.OnPlayerRespawn.ToString();
                if (_scriptFields.ContainsKey("on_player_rest") && _scriptFields["on_player_rest"] != null)
                    _scriptFields["on_player_rest"].Text = _ifo.OnPlayerRest.ToString();
                if (_scriptFields.ContainsKey("start_movie") && _scriptFields["start_movie"] != null)
                    _scriptFields["start_movie"].Text = _ifo.StartMovie.ToString();
            }
        }

        public void OnValueChanged()
        {
            if (_ifo == null)
            {
                return;
            }

            ReadUIIntoIfo();
        }

        private void ReadUIIntoIfo()
        {
            if (_ifo == null)
            {
                return;
            }

            // Basic Info
            _ifo.ModName = _nameEdit?.GetLocString() ?? _ifo.ModName ?? LocalizedString.FromInvalid();
            _ifo.Name = _ifo.ModName; // Alias
            if (_tagEdit != null) _ifo.Tag = _tagEdit.Text ?? "";
            if (_voIdEdit != null) _ifo.VoId = _voIdEdit.Text ?? "";
            if (_hakEdit != null) _ifo.Hak = _hakEdit.Text ?? "";
            _ifo.Description = _descEdit?.GetLocString() ?? _ifo.Description ?? LocalizedString.FromInvalid();

            // Entry Point
            if (_entryResrefEdit != null)
            {
                try
                {
                    _ifo.ResRef = ResRefFromText(_entryResrefEdit.Text);
                    _ifo.EntryArea = _ifo.ResRef; // Alias
                }
                catch
                {
                    // Skip invalid ResRef values
                }
            }
            if (_entryXSpin != null && _entryXSpin.Value.HasValue)
                _ifo.EntryX = (float)_entryXSpin.Value.Value;
            if (_entryYSpin != null && _entryYSpin.Value.HasValue)
                _ifo.EntryY = (float)_entryYSpin.Value.Value;
            if (_entryZSpin != null && _entryZSpin.Value.HasValue)
                _ifo.EntryZ = (float)_entryZSpin.Value.Value;
            if (_entryDirSpin != null && _entryDirSpin.Value.HasValue)
            {
                _ifo.EntryDirection = (float)DegreesToRadians((double)_entryDirSpin.Value.Value);
                // Update direction components from angle
                _ifo.EntryDirectionX = (float)System.Math.Cos(_ifo.EntryDirection);
                _ifo.EntryDirectionY = (float)System.Math.Sin(_ifo.EntryDirection);
            }

            // Time Settings
            if (_dawnHourSpin != null && _dawnHourSpin.Value.HasValue)
                _ifo.DawnHour = (int)_dawnHourSpin.Value.Value;
            if (_duskHourSpin != null && _duskHourSpin.Value.HasValue)
                _ifo.DuskHour = (int)_duskHourSpin.Value.Value;
            if (_timeScaleSpin != null && _timeScaleSpin.Value.HasValue)
                _ifo.TimeScale = (int)_timeScaleSpin.Value.Value;
            if (_startMonthSpin != null && _startMonthSpin.Value.HasValue)
                _ifo.StartMonth = (int)_startMonthSpin.Value.Value;
            if (_startDaySpin != null && _startDaySpin.Value.HasValue)
                _ifo.StartDay = (int)_startDaySpin.Value.Value;
            if (_startHourSpin != null && _startHourSpin.Value.HasValue)
                _ifo.StartHour = (int)_startHourSpin.Value.Value;
            if (_startYearSpin != null && _startYearSpin.Value.HasValue)
                _ifo.StartYear = (int)_startYearSpin.Value.Value;
            if (_xpScaleSpin != null && _xpScaleSpin.Value.HasValue)
                _ifo.XpScale = (int)_xpScaleSpin.Value.Value;

            // Scripts
            if (_scriptFields != null)
            {
                if (_scriptFields.ContainsKey("on_heartbeat") && _scriptFields["on_heartbeat"] != null)
                {
                    _ifo.OnHeartbeat = ResRefFromText(_scriptFields["on_heartbeat"].Text);
                }
                if (_scriptFields.ContainsKey("on_load") && _scriptFields["on_load"] != null)
                {
                    _ifo.OnLoad = ResRefFromText(_scriptFields["on_load"].Text);
                }
                if (_scriptFields.ContainsKey("on_start") && _scriptFields["on_start"] != null)
                {
                    _ifo.OnStart = ResRefFromText(_scriptFields["on_start"].Text);
                }
                if (_scriptFields.ContainsKey("on_enter") && _scriptFields["on_enter"] != null)
                {
                    _ifo.OnClientEnter = ResRefFromText(_scriptFields["on_enter"].Text);
                }
                if (_scriptFields.ContainsKey("on_leave") && _scriptFields["on_leave"] != null)
                {
                    _ifo.OnClientLeave = ResRefFromText(_scriptFields["on_leave"].Text);
                }
                if (_scriptFields.ContainsKey("on_activate_item") && _scriptFields["on_activate_item"] != null)
                {
                    _ifo.OnActivateItem = ResRefFromText(_scriptFields["on_activate_item"].Text);
                }
                if (_scriptFields.ContainsKey("on_acquire_item") && _scriptFields["on_acquire_item"] != null)
                {
                    _ifo.OnAcquireItem = ResRefFromText(_scriptFields["on_acquire_item"].Text);
                }
                if (_scriptFields.ContainsKey("on_user_defined") && _scriptFields["on_user_defined"] != null)
                {
                    _ifo.OnUserDefined = ResRefFromText(_scriptFields["on_user_defined"].Text);
                }
                if (_scriptFields.ContainsKey("on_unacquire_item") && _scriptFields["on_unacquire_item"] != null)
                {
                    _ifo.OnUnacquireItem = ResRefFromText(_scriptFields["on_unacquire_item"].Text);
                }
                if (_scriptFields.ContainsKey("on_player_death") && _scriptFields["on_player_death"] != null)
                {
                    _ifo.OnPlayerDeath = ResRefFromText(_scriptFields["on_player_death"].Text);
                }
                if (_scriptFields.ContainsKey("on_player_dying") && _scriptFields["on_player_dying"] != null)
                {
                    _ifo.OnPlayerDying = ResRefFromText(_scriptFields["on_player_dying"].Text);
                }
                if (_scriptFields.ContainsKey("on_player_levelup") && _scriptFields["on_player_levelup"] != null)
                {
                    _ifo.OnPlayerLevelUp = ResRefFromText(_scriptFields["on_player_levelup"].Text);
                }
                if (_scriptFields.ContainsKey("on_player_respawn") && _scriptFields["on_player_respawn"] != null)
                {
                    _ifo.OnPlayerRespawn = ResRefFromText(_scriptFields["on_player_respawn"].Text);
                }
                if (_scriptFields.ContainsKey("on_player_rest") && _scriptFields["on_player_rest"] != null)
                {
                    _ifo.OnPlayerRest = ResRefFromText(_scriptFields["on_player_rest"].Text);
                }
                if (_scriptFields.ContainsKey("start_movie") && _scriptFields["start_movie"] != null)
                {
                    _ifo.StartMovie = ResRefFromText(_scriptFields["start_movie"].Text);
                }
            }
        }

        private static ResRef ResRefFromText(string text)
        {
            string value = (text ?? string.Empty).Trim();
            return !string.IsNullOrEmpty(value) ? new ResRef(value) : ResRef.FromBlank();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        private static double RadiansToDegrees(double radians)
        {
            double degrees = radians * 180.0 / System.Math.PI;
            if (degrees < 0)
            {
                degrees += 360.0;
            }

            return degrees % 360.0;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * System.Math.PI / 180.0;
        }
    }
}
