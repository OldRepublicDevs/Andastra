using BioWare.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using BioWare;
using BioWare.Extract;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Widgets;
using OdyTools.Widgets.Edit;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Game = BioWare.Common.BioWareGame;
using GFFAuto = BioWare.Resource.Formats.GFF.GFFAuto;
using Window = Avalonia.Controls.Window;
using UTE = BioWare.Resource.Formats.GFF.Generics.UTE;
using Avalonia;
using TextBlock = Avalonia.Controls.TextBlock;
using LocalizedString = BioWare.Common.LocalizedString;
using ResourceType = BioWare.Common.ResourceType;
using Button = Avalonia.Controls.Button;
using ComboBox = Avalonia.Controls.ComboBox;
using NumericUpDown = Avalonia.Controls.NumericUpDown;
using CheckBox = Avalonia.Controls.CheckBox;
using DataGrid = Avalonia.Controls.DataGrid;
using TabControl = Avalonia.Controls.TabControl;
using TabItem = Avalonia.Controls.TabItem;
using OdyTools.Utils;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Editors
{
    public partial class OdyToolUTE : Editor
    {
        // Data model for creature table rows
        // C# uses DataGrid with bindings, so we need a proper data model
        private class CreatureRow : INotifyPropertyChanged
        {
            private bool _singleSpawn;
            private float _cr;
            private int _appearance;
            private string _resRef;

            public event PropertyChangedEventHandler PropertyChanged;
            public Action Changed { get; set; }

            public bool SingleSpawn
            {
                get => _singleSpawn;
                set => SetField(ref _singleSpawn, value, nameof(SingleSpawn));
            }

            public float CR
            {
                get => _cr;
                set => SetField(ref _cr, value, nameof(CR));
            }

            public int Appearance
            {
                get => _appearance;
                set => SetField(ref _appearance, value, nameof(Appearance));
            }

            public string ResRef
            {
                get => _resRef;
                set => SetField(ref _resRef, value, nameof(ResRef));
            }

            private void SetField<T>(ref T field, T value, string propertyName)
            {
                if (EqualityComparer<T>.Default.Equals(field, value))
                {
                    return;
                }

                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                Changed?.Invoke();
            }
        }

        private UTE _ute;
        private List<string> _relevantCreatureResnames;
        private List<string> _relevantScriptResnames;
        private ObservableCollection<CreatureRow> _creatureRows;
        internal static ResourceType EncounterCreatureResourceType => ResourceType.UTC;

        // UI Controls - Basic
        private LocalizedStringEdit _nameEdit;
        private Button _nameEditBtn;
        private TextBox _tagEdit;
        private Button _tagGenerateBtn;
        private TextBox _resrefEdit;
        private Button _resrefGenerateBtn;
        private ComboBox2DA _difficultySelect;
        private ComboBox _spawnSelect;
        private NumericUpDown _minCreatureSpin;
        private NumericUpDown _maxCreatureSpin;

        // UI Controls - Advanced
        private CheckBox _activeCheckbox;
        private CheckBox _playerOnlyCheckbox;
        private ComboBox2DA _factionSelect;
        private CheckBox _respawnsCheckbox;
        private CheckBox _infiniteRespawnCheckbox;
        private NumericUpDown _respawnTimeSpin;
        private NumericUpDown _respawnCountSpin;

        // UI Controls - Creatures
        private DataGrid _creatureTable;
        private Button _addCreatureButton;
        private Button _removeCreatureButton;

        // UI Controls - Scripts
        private ComboBox _onEnterSelect;
        private ComboBox _onExitSelect;
        private ComboBox _onExhaustedEdit;
        private ComboBox _onHeartbeatSelect;
        private ComboBox _onUserDefinedSelect;

        // UI Controls - Comments
        private TextBox _commentsEdit;
        private TabControl _editorSurface;
        private bool _loadingUte;
        private bool _dirtyTrackingBound;
        private bool _clearInitialDirtyOnOpen = true;

        internal bool HasStructuredEditorSurface => _editorSurface != null && HasRequiredEditorControls() && _onEnterSelect != null && _commentsEdit != null;

        public LocalizedStringEdit NameEdit => _nameEdit;
        public TextBox TagEdit => _tagEdit;
        public TextBox ResrefEdit => _resrefEdit;
        public ComboBox2DA DifficultySelect => _difficultySelect;
        public ComboBox SpawnSelect => _spawnSelect;
        public NumericUpDown MinCreatureSpin => _minCreatureSpin;
        public NumericUpDown MaxCreatureSpin => _maxCreatureSpin;
        public CheckBox ActiveCheckbox => _activeCheckbox;
        public CheckBox PlayerOnlyCheckbox => _playerOnlyCheckbox;
        public ComboBox2DA FactionSelect => _factionSelect;
        public CheckBox RespawnsCheckbox => _respawnsCheckbox;
        public NumericUpDown RespawnTimeSpin => _respawnTimeSpin;
        public NumericUpDown RespawnCountSpin => _respawnCountSpin;
        public DataGrid CreatureTable => _creatureTable;
        public Button RemoveCreatureButton => _removeCreatureButton;
        public ComboBox OnEnterSelect => _onEnterSelect;
        public ComboBox OnExitSelect => _onExitSelect;
        public ComboBox OnExhaustedEdit => _onExhaustedEdit;
        public ComboBox OnHeartbeatSelect => _onHeartbeatSelect;
        public ComboBox OnUserDefinedSelect => _onUserDefinedSelect;
        public TextBox CommentsEdit => _commentsEdit;

        public OdyToolUTE() : this(null, null) { }
        public OdyToolUTE(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolUTE", "encounter",
                new[] { ResourceType.UTE, ResourceType.BTE, ResourceType.UTE_XML },
                new[] { ResourceType.UTE, ResourceType.BTE, ResourceType.UTE_XML },
                installation)
        {
            _installation = installation;
            _ute = new UTE();
            _relevantCreatureResnames = new List<string>();
            _relevantScriptResnames = new List<string>();
            _creatureRows = new ObservableCollection<CreatureRow>();

            InitializeComponent();
            SetupSignals();
            BindDirtyTracking();
            SetupMenuHandlers();
            AddHelpAction();
            KeyDown += OnWindowKeyDown;
            if (installation != null)
            {
                SetupInstallation(installation);
            }
            New();
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
            return menu;
        }

        private void SetupMenuHandlers()
        {
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            var mod = e.KeyModifiers;
            bool ctrl = (mod & KeyModifiers.Control) != 0;
            if (ctrl)
            {
                if (e.Key == Key.N) { New(); e.Handled = true; }
                else if (e.Key == Key.O) { _ = RunOpenAsync(); e.Handled = true; }
                else if (e.Key == Key.S) { Save(); e.Handled = true; }
                else if (e.Key == Key.R) { Revert(); e.Handled = true; }
                else if (e.Key == Key.Q) { Close(); e.Handled = true; }
            }
            if (ctrl && (mod & KeyModifiers.Shift) != 0 && e.Key == Key.S)
            {
                _ = RunSaveAsAsync();
                e.Handled = true;
            }
        }

        public override void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            try
            {
                Load(_filepath ?? "", _resname ?? "", _restype ?? ResourceType.UTE, _revert);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Revert failed: {ex}");
            }
        }

        protected override async Task RunOpenAsync()
        {
            if (await ConfirmDiscardUnsavedChangesAsync() == false) return;
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            var options = new FilePickerOpenOptions
            {
                Title = "Open Encounter",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Encounter (UTE/BTE)") { Patterns = new[] { "*.ute", "*.bte" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
                }
            };
            var files = await storageProvider.OpenFilePickerAsync(options);
            if (files == null || files.Count == 0) return;
            var file = files[0];
            string path = file.Path?.LocalPath;
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path)) return;
            try
            {
                byte[] data = File.ReadAllBytes(path);
                string ext = Path.GetExtension(path).TrimStart('.').ToLowerInvariant();
                string resname = Path.GetFileNameWithoutExtension(path);
                var restype = ResourceType.FromExtension(ext) ?? ResourceType.UTE;
                Load(path, resname, restype, data);
            }
            catch (Exception ex)
            {
                await DialogHelper.ShowWindowAsync(this, "Open Failed", $"Could not open file: {ex.Message}", ButtonEnum.Ok, IconType.Error);
            }
        }

        protected override async Task RunSaveAsAsync()
        {
            await base.RunSaveAsAsync();
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;

                // Try to find controls from XAML
                _editorSurface = EditorHelpers.FindControlSafe<TabControl>(this, "editorSurface");
                _nameEdit = EditorHelpers.FindControlSafe<LocalizedStringEdit>(this, "nameEdit");
                _nameEditBtn = EditorHelpers.FindControlSafe<Button>(this, "nameEditBtn");
                _tagEdit = EditorHelpers.FindControlSafe<TextBox>(this, "tagEdit");
                _tagGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "tagGenerateButton");
                _resrefEdit = EditorHelpers.FindControlSafe<TextBox>(this, "resrefEdit");
                _resrefGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "resrefGenerateButton");
                _difficultySelect = EditorHelpers.FindControlSafe<ComboBox2DA>(this, "difficultySelect");
                _spawnSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "spawnSelect");
                _minCreatureSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "minCreatureSpin");
                _maxCreatureSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "maxCreatureSpin");
                _activeCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "activeCheckbox");
                _playerOnlyCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "playerOnlyCheckbox");
                _factionSelect = EditorHelpers.FindControlSafe<ComboBox2DA>(this, "factionSelect");
                _respawnsCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "respawnsCheckbox");
                _infiniteRespawnCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "infiniteRespawnCheckbox");
                _respawnTimeSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "respawnTimeSpin");
                _respawnCountSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "respawnCountSpin");
                _creatureTable = EditorHelpers.FindControlSafe<DataGrid>(this, "creatureTable");
                if (_creatureTable != null)
                {
                    _creatureTable.ItemsSource = _creatureRows;
                }
                _addCreatureButton = EditorHelpers.FindControlSafe<Button>(this, "addCreatureButton");
                _removeCreatureButton = EditorHelpers.FindControlSafe<Button>(this, "removeCreatureButton");
                _onEnterSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "onEnterSelect");
                _onExitSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "onExitSelect");
                _onExhaustedEdit = EditorHelpers.FindControlSafe<ComboBox>(this, "onExhaustedEdit");
                _onHeartbeatSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "onHeartbeatSelect");
                _onUserDefinedSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "onUserDefinedSelect");
                _commentsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "commentsEdit");

                xamlLoaded = HasRequiredEditorControls();
            }
            catch
            {
                // XAML not available or controls not found - will use programmatic UI
                xamlLoaded = false;
            }

            if (!xamlLoaded)
            {
                SetupProgrammaticUI();
                AttachReferenceSearchMenus();
            }
            else
            {
                SetupSignals();
                AttachReferenceSearchMenus();
            }
        }

        private void AttachReferenceSearchMenus()
        {
            if (_tagEdit == null || _resrefEdit == null)
            {
                return;
            }

            ReferenceSearchHelper.AttachTagFindReferencesMenu(_tagEdit, this, _installation);
            FieldValueReferenceHelper.AppendFieldValueFindReferencesMenuItem(
                _tagEdit.ContextMenu,
                _tagEdit,
                this,
                _installation,
                () => "Tag");
            ReferenceSearchHelper.AttachTemplateResRefFindReferencesMenu(_resrefEdit, this, _installation);
            FieldValueReferenceHelper.AppendFieldValueFindReferencesMenuItem(
                _resrefEdit.ContextMenu,
                _resrefEdit,
                this,
                _installation,
                () => "TemplateResRef");
        }

        private bool HasRequiredEditorControls()
        {
            return _tagEdit != null
                && _resrefEdit != null
                && _difficultySelect != null
                && _spawnSelect != null
                && _creatureTable != null;
        }

        private void SetupSignals()
        {
            EditorHelpers.BindClick(_tagGenerateBtn, GenerateTag);
            EditorHelpers.BindClick(_resrefGenerateBtn, GenerateResref);
            EditorHelpers.BindCheckedChanged(_infiniteRespawnCheckbox, SetInfiniteRespawn);
            EditorHelpers.BindSelectionChanged(_spawnSelect, SetContinuous);
            EditorHelpers.BindClick(_addCreatureButton, () => AddCreature());
            EditorHelpers.BindClick(_removeCreatureButton, RemoveSelectedCreature);
            EditorHelpers.BindClick(_nameEditBtn, ChangeName);
        }

        private void BindDirtyTracking()
        {
            if (_dirtyTrackingBound)
            {
                return;
            }

            _dirtyTrackingBound = true;
            if (_tagEdit != null) _tagEdit.TextChanged += (s, e) => MarkDirtyAfterLoad();
            if (_resrefEdit != null) _resrefEdit.TextChanged += (s, e) => MarkDirtyAfterLoad();
            if (_difficultySelect != null) _difficultySelect.SelectionChanged += (s, e) => MarkDirtyAfterLoad();
            if (_spawnSelect != null) _spawnSelect.SelectionChanged += (s, e) => MarkDirtyAfterLoad();
            if (_minCreatureSpin != null) _minCreatureSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_maxCreatureSpin != null) _maxCreatureSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_activeCheckbox != null) _activeCheckbox.IsCheckedChanged += (s, e) => MarkDirtyAfterLoad();
            if (_playerOnlyCheckbox != null) _playerOnlyCheckbox.IsCheckedChanged += (s, e) => MarkDirtyAfterLoad();
            if (_factionSelect != null) _factionSelect.SelectionChanged += (s, e) => MarkDirtyAfterLoad();
            if (_respawnsCheckbox != null) _respawnsCheckbox.IsCheckedChanged += (s, e) => MarkDirtyAfterLoad();
            if (_infiniteRespawnCheckbox != null) _infiniteRespawnCheckbox.IsCheckedChanged += (s, e) => MarkDirtyAfterLoad();
            if (_respawnTimeSpin != null) _respawnTimeSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            if (_respawnCountSpin != null) _respawnCountSpin.ValueChanged += (s, e) => MarkDirtyAfterLoad();
            BindComboDirtyTracking(_onEnterSelect);
            BindComboDirtyTracking(_onExitSelect);
            BindComboDirtyTracking(_onExhaustedEdit);
            BindComboDirtyTracking(_onHeartbeatSelect);
            BindComboDirtyTracking(_onUserDefinedSelect);
            if (_commentsEdit != null) _commentsEdit.TextChanged += (s, e) => MarkDirtyAfterLoad();
        }

        private void BindComboDirtyTracking(ComboBox comboBox)
        {
            if (comboBox == null)
            {
                return;
            }

            comboBox.SelectionChanged += (s, e) => MarkDirtyAfterLoad();
            comboBox.PropertyChanged += (s, e) =>
            {
                if (e.Property.Name == nameof(ComboBox.Text))
                {
                    MarkDirtyAfterLoad();
                }
            };
        }

        private void MarkDirtyAfterLoad()
        {
            if (!_loadingUte)
            {
                MarkDocumentDirty();
            }
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            if (!_clearInitialDirtyOnOpen)
            {
                return;
            }

            ClearDirty();
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                if (_clearInitialDirtyOnOpen)
                {
                    ClearDirty();
                    _clearInitialDirtyOnOpen = false;
                }
            }, Avalonia.Threading.DispatcherPriority.Background);
        }

        private void SetupInstallation(OdyInstallation installation)
        {
            _installation = installation;
            if (_nameEdit != null)
            {
                _nameEdit.SetInstallation(installation);
            }

            TwoDA difficulties = installation.HtGetCache2DA(OdyInstallation.TwoDAEncDifficulties);
            if (_difficultySelect != null)
            {
                _difficultySelect.Items.Clear();
                if (difficulties != null)
                {
                    List<string> difficultyLabels = difficulties.GetColumn("label");
                    _difficultySelect.SetItems(difficultyLabels, sortAlphabetically: false);
                    _difficultySelect.SetContext(difficulties, installation, OdyInstallation.TwoDAEncDifficulties);
                }
            }

            TwoDA factions = installation.HtGetCache2DA(OdyInstallation.TwoDAFactions);
            if (_factionSelect != null)
            {
                _factionSelect.Items.Clear();
                if (factions != null)
                {
                    List<string> factionLabels = factions.GetColumn("label");
                    _factionSelect.SetItems(factionLabels, sortAlphabetically: false);
                    _factionSelect.SetContext(factions, installation, OdyInstallation.TwoDAFactions);
                }
            }

            SetupFileContextMenus();

            if (installation != null && !string.IsNullOrEmpty(base._filepath))
            {
                HashSet<FileResource> creatureResources = installation.GetRelevantResources(ResourceType.UTC, base._filepath);
                _relevantCreatureResnames = creatureResources
                    .Select(r => r.ResName.ToLowerInvariant())
                    .Distinct()
                    .OrderBy(r => r)
                    .ToList();
            }
            else
            {
                _relevantCreatureResnames = new List<string>();
            }
        }

        protected override void OnInstallationChanged()
        {
            if (_installation != null)
            {
                SetupInstallation(_installation);
                return;
            }

            if (_nameEdit != null)
            {
                _nameEdit.SetInstallation(null);
            }

            _relevantCreatureResnames = new List<string>();
        }

        private void SetupProgrammaticUI()
        {
            var scrollViewer = new ScrollViewer();
            var tabControl = new TabControl();

            // Basic Tab
            var basicTab = new TabItem { Header = "Basic" };
            var basicPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Name
            var nameLabel = new TextBlock { Text = "Name:" };
            try
            {
                _nameEdit = new LocalizedStringEdit();
                if (_installation != null)
                {
                    _nameEdit.SetInstallation(_installation);
                }
                basicPanel.Children.Add(_nameEdit);
            }
            catch
            {
                // If LocalizedStringEdit fails to initialize, use a simple TextBox
                _nameEdit = null;
                var nameTextBox = new TextBox();
                basicPanel.Children.Add(nameTextBox);
            }
            _nameEditBtn = new Button { Content = "Edit Name" };
            EditorHelpers.BindClick(_nameEditBtn, ChangeName);
            basicPanel.Children.Add(nameLabel);
            basicPanel.Children.Add(_nameEditBtn);

            // Tag
            var tagLabel = new TextBlock { Text = "Tag:" };
            _tagEdit = new TextBox();
            _tagGenerateBtn = new Button { Content = "-" };
            EditorHelpers.BindClick(_tagGenerateBtn, GenerateTag);
            var tagPanel = new StackPanel { Orientation = Orientation.Horizontal };
            tagPanel.Children.Add(_tagEdit);
            tagPanel.Children.Add(_tagGenerateBtn);
            basicPanel.Children.Add(tagLabel);
            basicPanel.Children.Add(tagPanel);

            // ResRef
            var resrefLabel = new TextBlock { Text = "ResRef:" };
            _resrefEdit = new TextBox { MaxLength = 16 };
            _resrefGenerateBtn = new Button { Content = "-" };
            EditorHelpers.BindClick(_resrefGenerateBtn, GenerateResref);
            var resrefPanel = new StackPanel { Orientation = Orientation.Horizontal };
            resrefPanel.Children.Add(_resrefEdit);
            resrefPanel.Children.Add(_resrefGenerateBtn);
            basicPanel.Children.Add(resrefLabel);
            basicPanel.Children.Add(resrefPanel);

            // Difficulty
            var difficultyLabel = new TextBlock { Text = "Difficulty:" };
            _difficultySelect = new ComboBox2DA();
            basicPanel.Children.Add(difficultyLabel);
            basicPanel.Children.Add(_difficultySelect);

            // Spawn Option
            var spawnLabel = new TextBlock { Text = "Spawn Option:" };
            _spawnSelect = new ComboBox();
            _spawnSelect.Items.Add("Single Shot");
            _spawnSelect.Items.Add("Continuous");
            EditorHelpers.BindSelectionChanged(_spawnSelect, SetContinuous);
            basicPanel.Children.Add(spawnLabel);
            basicPanel.Children.Add(_spawnSelect);

            // Min/Max Creatures
            var minCreatureLabel = new TextBlock { Text = "Min Creatures:" };
            _minCreatureSpin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue };
            var maxCreatureLabel = new TextBlock { Text = "Max Creatures:" };
            _maxCreatureSpin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue };
            basicPanel.Children.Add(minCreatureLabel);
            basicPanel.Children.Add(_minCreatureSpin);
            basicPanel.Children.Add(maxCreatureLabel);
            basicPanel.Children.Add(_maxCreatureSpin);

            basicTab.Content = basicPanel;
            tabControl.Items.Add(basicTab);

            // Advanced Tab
            var advancedTab = new TabItem { Header = "Advanced" };
            var advancedPanel = new StackPanel { Orientation = Orientation.Vertical };

            _activeCheckbox = new CheckBox { Content = "Active" };
            _playerOnlyCheckbox = new CheckBox { Content = "Player Triggered Only" };

            var factionLabel = new TextBlock { Text = "Faction:" };
            _factionSelect = new ComboBox2DA();

            _respawnsCheckbox = new CheckBox { Content = "Respawns" };
            _infiniteRespawnCheckbox = new CheckBox { Content = "Infinite Respawns" };
            EditorHelpers.BindCheckedChanged(_infiniteRespawnCheckbox, SetInfiniteRespawn);

            var respawnTimeLabel = new TextBlock { Text = "Respawn Time (s):" };
            _respawnTimeSpin = new NumericUpDown { Minimum = int.MinValue, Maximum = int.MaxValue };
            var respawnCountLabel = new TextBlock { Text = "Number of Respawns:" };
            _respawnCountSpin = new NumericUpDown { Minimum = 0, Maximum = 99999 };

            advancedPanel.Children.Add(_activeCheckbox);
            advancedPanel.Children.Add(_playerOnlyCheckbox);
            advancedPanel.Children.Add(factionLabel);
            advancedPanel.Children.Add(_factionSelect);
            advancedPanel.Children.Add(_respawnsCheckbox);
            advancedPanel.Children.Add(_infiniteRespawnCheckbox);
            advancedPanel.Children.Add(respawnTimeLabel);
            advancedPanel.Children.Add(_respawnTimeSpin);
            advancedPanel.Children.Add(respawnCountLabel);
            advancedPanel.Children.Add(_respawnCountSpin);

            advancedTab.Content = advancedPanel;
            tabControl.Items.Add(advancedTab);

            // Creatures Tab
            var creaturesTab = new TabItem { Header = "Creatures" };
            var creaturesPanel = new StackPanel { Orientation = Orientation.Vertical };

            _creatureTable = new DataGrid
            {
                AutoGenerateColumns = false,
                IsReadOnly = false,
                GridLinesVisibility = DataGridGridLinesVisibility.All,
                SelectionMode = DataGridSelectionMode.Single
            };

            // Add columns with proper bindings
            _creatureTable.Columns.Add(new DataGridCheckBoxColumn { Header = "SingleSpawn", Binding = new Avalonia.Data.Binding("SingleSpawn") });
            _creatureTable.Columns.Add(new DataGridTextColumn { Header = "CR", Binding = new Avalonia.Data.Binding("CR") });
            _creatureTable.Columns.Add(new DataGridTextColumn { Header = "Appearance", Binding = new Avalonia.Data.Binding("Appearance") });
            _creatureTable.Columns.Add(new DataGridTextColumn { Header = "ResRef", Binding = new Avalonia.Data.Binding("ResRef") });

            // Set ItemsSource to ObservableCollection for proper binding
            _creatureTable.ItemsSource = _creatureRows;

            var creatureButtonsPanel = new StackPanel { Orientation = Orientation.Horizontal };
            _removeCreatureButton = new Button { Content = "Remove" };
            EditorHelpers.BindClick(_removeCreatureButton, RemoveSelectedCreature);
            _addCreatureButton = new Button { Content = "Add" };
            EditorHelpers.BindClick(_addCreatureButton, () => AddCreature());

            creatureButtonsPanel.Children.Add(_removeCreatureButton);
            creatureButtonsPanel.Children.Add(_addCreatureButton);

            creaturesPanel.Children.Add(_creatureTable);
            creaturesPanel.Children.Add(creatureButtonsPanel);

            creaturesTab.Content = creaturesPanel;
            tabControl.Items.Add(creaturesTab);

            // Scripts Tab
            var scriptsTab = new TabItem { Header = "Scripts" };
            var scriptsPanel = new StackPanel { Orientation = Orientation.Vertical };

            var onEnterLabel = new TextBlock { Text = "OnEnter:" };
            _onEnterSelect = new ComboBox { IsEditable = true };
            var onExitLabel = new TextBlock { Text = "OnExit:" };
            _onExitSelect = new ComboBox { IsEditable = true };
            var onExhaustedLabel = new TextBlock { Text = "OnExhausted:" };
            _onExhaustedEdit = new ComboBox { IsEditable = true };
            var onHeartbeatLabel = new TextBlock { Text = "OnHeartbeat:" };
            _onHeartbeatSelect = new ComboBox { IsEditable = true };
            var onUserDefinedLabel = new TextBlock { Text = "OnUserDefined:" };
            _onUserDefinedSelect = new ComboBox { IsEditable = true };

            scriptsPanel.Children.Add(onEnterLabel);
            scriptsPanel.Children.Add(_onEnterSelect);
            scriptsPanel.Children.Add(onExitLabel);
            scriptsPanel.Children.Add(_onExitSelect);
            scriptsPanel.Children.Add(onExhaustedLabel);
            scriptsPanel.Children.Add(_onExhaustedEdit);
            scriptsPanel.Children.Add(onHeartbeatLabel);
            scriptsPanel.Children.Add(_onHeartbeatSelect);
            scriptsPanel.Children.Add(onUserDefinedLabel);
            scriptsPanel.Children.Add(_onUserDefinedSelect);

            scriptsTab.Content = scriptsPanel;
            tabControl.Items.Add(scriptsTab);

            // Comments Tab
            var commentsTab = new TabItem { Header = "Comments" };
            var commentsPanel = new StackPanel { Orientation = Orientation.Vertical };
            var commentsLabel = new TextBlock { Text = "Comment:" };
            _commentsEdit = new TextBox { AcceptsReturn = true, AcceptsTab = true };
            commentsPanel.Children.Add(commentsLabel);
            commentsPanel.Children.Add(_commentsEdit);
            commentsTab.Content = commentsPanel;
            tabControl.Items.Add(commentsTab);

            scrollViewer.Content = tabControl;

            var dock = new DockPanel();
            dock.Children.Add(BuildMenu());
            DockPanel.SetDock(dock.Children[0], Dock.Top);
            dock.Children.Add(scrollViewer);
            SetContentOrInject(dock);
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            var gff = GFFAuto.ReadGff(data, fileFormat: restype);
            _ute = UTEHelpers.ConstructUte(gff);
            LoadUTE(_ute);
        }

        private void LoadUTE(UTE ute)
        {
            _ute = ute;
            _loadingUte = true;
            try
            {

                // Basic
                if (_nameEdit != null)
                {
                    _nameEdit.SetLocString(ute.Name);
                }
                if (_tagEdit != null)
                {
                    _tagEdit.Text = ute.Tag;
                }
                if (_resrefEdit != null)
                {
                    _resrefEdit.Text = ute.ResRef.ToString();
                }
                if (_difficultySelect != null)
                {
                    _difficultySelect.SetSelectedIndex(ute.DifficultyId);
                }
                if (_spawnSelect != null)
                {
                    // single_shot=True => Single Shot => index 0; single_shot=False => Continuous => index 1
                    _spawnSelect.SelectedIndex = ute.SingleShot ? 0 : 1;
                    // Ensure respawn fields are properly enabled/disabled based on spawn mode
                    SetContinuous();
                }
                if (_minCreatureSpin != null)
                {
                    _minCreatureSpin.Value = ute.RecCreatures;
                }
                if (_maxCreatureSpin != null)
                {
                    _maxCreatureSpin.Value = ute.MaxCreatures;
                }

                // Advanced
                if (_activeCheckbox != null)
                {
                    _activeCheckbox.IsChecked = ute.Active;
                }
                if (_playerOnlyCheckbox != null)
                {
                    _playerOnlyCheckbox.IsChecked = ute.PlayerOnly != 0;
                }
                if (_factionSelect != null)
                {
                    _factionSelect.SetSelectedIndex(ute.FactionId);
                }
                if (_respawnsCheckbox != null)
                {
                    _respawnsCheckbox.IsChecked = ute.Reset != 0;
                }
                if (_infiniteRespawnCheckbox != null)
                {
                    _infiniteRespawnCheckbox.IsChecked = ute.Respawns == -1;
                }
                if (_respawnTimeSpin != null)
                {
                    _respawnTimeSpin.Value = ute.ResetTime;
                }
                if (_respawnCountSpin != null)
                {
                    _respawnCountSpin.Value = ute.Respawns;
                }

                // Creatures
                if (_creatureTable != null && _creatureRows != null)
                {
                    _creatureRows.Clear();
                    foreach (var creature in ute.Creatures)
                    {
                        _creatureRows.Add(CreateCreatureRow(
                            creature.ResRef.ToString(),
                            creature.AppearanceId,
                            creature.ChallengeRating,
                            creature.SingleSpawnBool));
                    }
                }

                // Scripts
                // First, get relevant script resources and populate combo boxes
                if (_installation != null && !string.IsNullOrEmpty(base._filepath))
                {
                    HashSet<FileResource> scriptResources = _installation.GetRelevantResources(ResourceType.NCS, base._filepath);
                    _relevantScriptResnames = scriptResources
                        .Select(r => r.ResName.ToLowerInvariant())
                        .Distinct()
                        .OrderBy(r => r)
                        .ToList();

                    // Populate all script combo boxes with relevant script resources (matching Python populate_combo_box)
                    EditorHelpers.PopulateComboBox(_onEnterSelect, _relevantScriptResnames);
                    EditorHelpers.PopulateComboBox(_onExitSelect, _relevantScriptResnames);
                    EditorHelpers.PopulateComboBox(_onExhaustedEdit, _relevantScriptResnames);
                    EditorHelpers.PopulateComboBox(_onHeartbeatSelect, _relevantScriptResnames);
                    EditorHelpers.PopulateComboBox(_onUserDefinedSelect, _relevantScriptResnames);
                }

                // Then set the text values (matching Python set_combo_box_text)
                // This must be done after populating items to ensure the text is set correctly
                EditorHelpers.SetComboBoxText(_onEnterSelect, ute.OnEntered.ToString());
                EditorHelpers.SetComboBoxText(_onExitSelect, ute.OnExit.ToString());
                EditorHelpers.SetComboBoxText(_onExhaustedEdit, ute.OnExhausted.ToString());
                EditorHelpers.SetComboBoxText(_onHeartbeatSelect, ute.OnHeartbeat.ToString());
                EditorHelpers.SetComboBoxText(_onUserDefinedSelect, ute.OnUserDefined.ToString());

                // Comments
                if (_commentsEdit != null)
                {
                    _commentsEdit.Text = ute.Comment;
                }
            }
            finally
            {
                _loadingUte = false;
            }
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var ute = CopyUTE(_ute);

            // Basic - read from UI controls (matching Python which always reads from UI)
            ute.Name = _nameEdit?.GetLocString() ?? ute.Name ?? LocalizedString.FromInvalid();
            ute.Tag = _tagEdit?.Text ?? ute.Tag ?? "";
            ute.ResRef = _resrefEdit != null && !string.IsNullOrEmpty(_resrefEdit.Text)
                ? new ResRef(_resrefEdit.Text)
                : ute.ResRef;
            ute.DifficultyId = _difficultySelect?.SelectedIndex ?? ute.DifficultyId;
            ute.SingleShot = _spawnSelect?.SelectedIndex == 0;
            ute.RecCreatures = _minCreatureSpin?.Value != null ? (int)_minCreatureSpin.Value : ute.RecCreatures;
            ute.MaxCreatures = _maxCreatureSpin?.Value != null ? (int)_maxCreatureSpin.Value : ute.MaxCreatures;

            // Advanced
            ute.Active = _activeCheckbox?.IsChecked ?? ute.Active;
            ute.PlayerOnly = (_playerOnlyCheckbox?.IsChecked ?? (ute.PlayerOnly != 0)) ? 1 : 0;
            ute.FactionId = _factionSelect?.SelectedIndex ?? ute.FactionId;
            ute.Reset = (_respawnsCheckbox?.IsChecked ?? (ute.Reset != 0)) ? 1 : 0;
            ute.Respawns = _respawnCountSpin?.Value != null ? (int)_respawnCountSpin.Value : ute.Respawns;
            ute.ResetTime = _respawnTimeSpin?.Value != null ? (int)_respawnTimeSpin.Value : ute.ResetTime;

            // Creatures
            ute.Creatures.Clear();
            if (_creatureRows != null)
            {
                foreach (var row in _creatureRows)
                {
                    var creature = new UTECreature();
                    creature.ResRef = !string.IsNullOrEmpty(row.ResRef) ? new ResRef(row.ResRef) : ResRef.FromBlank();
                    creature.Appearance = row.Appearance;
                    creature.CR = (int)row.CR;
                    creature.SingleSpawn = row.SingleSpawn ? 1 : 0;
                    ute.Creatures.Add(creature);
                }
            }
            // Scripts
            if (_onEnterSelect != null)
                ute.OnEntered = ResRefFromText(_onEnterSelect.Text);
            if (_onExitSelect != null)
                ute.OnExit = ResRefFromText(_onExitSelect.Text);
            if (_onExhaustedEdit != null)
                ute.OnExhausted = ResRefFromText(_onExhaustedEdit.Text);
            if (_onHeartbeatSelect != null)
                ute.OnHeartbeat = ResRefFromText(_onHeartbeatSelect.Text);
            if (_onUserDefinedSelect != null)
                ute.OnUserDefined = ResRefFromText(_onUserDefinedSelect.Text);

            // Comments
            ute.Comment = _commentsEdit?.Text ?? ute.Comment ?? "";

            // Build GFF
            var game = _installation?.Game ?? Game.K2;
            var gff = UTEHelpers.DismantleUte(ute, game);
            ResourceType outputType = _restype == ResourceType.UTE_XML
                ? ResourceType.UTE_XML
                : (_restype == ResourceType.BTE ? ResourceType.BTE : ResourceType.UTE);
            if (outputType == ResourceType.BTE)
            {
                gff.Content = GFFContent.BTE;
            }
            byte[] data = GFFAuto.BytesGff(gff, outputType);
            return Tuple.Create(data, new byte[0]);
        }

        private static ResRef ResRefFromText(string text)
        {
            string value = (text ?? string.Empty).Trim();
            return !string.IsNullOrEmpty(value) ? new ResRef(value) : ResRef.FromBlank();
        }

        private UTE CopyUTE(UTE source)
        {
            // Deep copy LocalizedString objects (they're reference types)
            LocalizedString copyName = source.Name != null
                ? new LocalizedString(source.Name.StringRef, new Dictionary<int, string>(GetSubstringsDict(source.Name)))
                : null;

            var copy = new UTE
            {
                ResRef = source.ResRef,
                Tag = source.Tag,
                Comment = source.Comment,
                Active = source.Active,
                DifficultyId = source.DifficultyId,
                DifficultyIndex = source.DifficultyIndex,
                Faction = source.Faction,
                MaxCreatures = source.MaxCreatures,
                RecCreatures = source.RecCreatures,
                Respawn = source.Respawn,
                RespawnTime = source.RespawnTime,
                Reset = source.Reset,
                ResetTime = source.ResetTime,
                PlayerOnly = source.PlayerOnly,
                SingleSpawn = source.SingleSpawn,
                OnEnteredScript = source.OnEnteredScript,
                OnExitScript = source.OnExitScript,
                OnExhaustedScript = source.OnExhaustedScript,
                OnHeartbeatScript = source.OnHeartbeatScript,
                OnUserDefinedScript = source.OnUserDefinedScript,
                Name = copyName,
                PaletteId = source.PaletteId
            };

            // Copy creatures
            foreach (var creature in source.Creatures)
            {
                copy.Creatures.Add(new UTECreature
                {
                    ResRef = creature.ResRef,
                    Appearance = creature.Appearance,
                    SingleSpawn = creature.SingleSpawn,
                    CR = creature.CR,
                    GuaranteedCount = creature.GuaranteedCount
                });
            }

            return copy;
        }

        // Helper to extract substrings dictionary from LocalizedString for copying
        private Dictionary<int, string> GetSubstringsDict(LocalizedString locString)
        {
            var dict = new Dictionary<int, string>();
            if (locString != null)
            {
                foreach ((Language lang, Gender gender, string text) in locString)
                {
                    int substringId = LocalizedString.SubstringId(lang, gender);
                    dict[substringId] = text;
                }
            }
            return dict;
        }

        public override void New()
        {
            base.New();
            _ute = new UTE();
            LoadUTE(_ute);
        }

        private void ChangeName()
        {
            if (_installation == null) return;
            LocalizedString currentName = _nameEdit?.GetLocString() ?? _ute?.Name ?? LocalizedString.FromInvalid();
            var dialog = new LocalizedStringDialog(this, _installation, currentName);
            if (dialog.ShowDialog())
            {
                _ute.Name = dialog.LocString;
                if (_nameEdit != null)
                {
                    _nameEdit.SetLocString(_ute.Name);
                }
                MarkDocumentDirty();
            }
        }

        private void GenerateTag()
        {
            if (string.IsNullOrEmpty(_resrefEdit?.Text))
            {
                GenerateResref();
            }
            if (_tagEdit != null && _resrefEdit != null)
            {
                _tagEdit.Text = _resrefEdit.Text;
            }
            MarkDocumentDirty();
        }

        private void GenerateResref()
        {
            if (_resrefEdit != null)
            {
                _resrefEdit.Text = !string.IsNullOrEmpty(base._resname) ? base._resname : "m00xx_enc_000";
            }
            MarkDocumentDirty();
        }

        private void SetInfiniteRespawn()
        {
            if (_infiniteRespawnCheckbox?.IsChecked == true)
            {
                SetInfiniteRespawnMain(val: -1, enabled: false);
            }
            else
            {
                SetInfiniteRespawnMain(val: 0, enabled: true);
            }
        }

        private void SetInfiniteRespawnMain(int val, bool enabled)
        {
            if (_respawnCountSpin != null)
            {
                _respawnCountSpin.Minimum = val;
                _respawnCountSpin.Value = val;
                _respawnCountSpin.IsEnabled = enabled;
            }
        }

        private void SetContinuous()
        {
            bool isContinuous = _spawnSelect?.SelectedIndex == 1;
            if (_respawnsCheckbox != null)
            {
                _respawnsCheckbox.IsEnabled = isContinuous;
            }
            if (_infiniteRespawnCheckbox != null)
            {
                _infiniteRespawnCheckbox.IsEnabled = isContinuous;
            }
            if (_respawnCountSpin != null)
            {
                _respawnCountSpin.IsEnabled = isContinuous;
            }
            if (_respawnTimeSpin != null)
            {
                _respawnTimeSpin.IsEnabled = isContinuous;
            }
        }

        private void AddCreature(string resname = "", int appearanceId = 0, float challenge = 0.0f, bool single = false)
        {
            if (_creatureRows == null)
            {
                _creatureRows = new ObservableCollection<CreatureRow>();
            }

            // Add to ObservableCollection (DataGrid is bound to this)
            _creatureRows.Add(CreateCreatureRow(resname, appearanceId, challenge, single));
            MarkDocumentDirty();
        }

        private CreatureRow CreateCreatureRow(string resref, int appearanceId, float challenge, bool single)
        {
            return new CreatureRow
            {
                SingleSpawn = single,
                CR = challenge,
                Appearance = appearanceId,
                ResRef = resref,
                Changed = MarkDirtyAfterLoad
            };
        }

        private void RemoveSelectedCreature()
        {
            if (_creatureTable == null || _creatureRows == null) return;

            // Try to get selected item
            var selectedItem = _creatureTable.SelectedItem;
            if (selectedItem is CreatureRow creatureRow)
            {
                _creatureRows.Remove(creatureRow);
            }
            else if (selectedItem != null)
            {
                // Fallback: try to find by index
                int selectedIndex = _creatureTable.SelectedIndex;
                if (selectedIndex >= 0 && selectedIndex < _creatureRows.Count)
                {
                    _creatureRows.RemoveAt(selectedIndex);
                }
            }
            MarkDocumentDirty();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        // Setup context menus for file resource fields (scripts and creatures)
        // Based on PyKotor implementation: self._installation.setup_file_context_menu(...)
        // Provides right-click menus to open, create, or view referenced files
        private void SetupFileContextMenus()
        {
            if (_installation == null)
            {
                return;
            }

            // Setup context menus for script ComboBoxes (NSS/NCS files)
            SetupScriptComboBoxContextMenu(_onEnterSelect, "OnEnter Script");
            SetupScriptComboBoxContextMenu(_onExitSelect, "OnExit Script");
            SetupScriptComboBoxContextMenu(_onExhaustedEdit, "OnExhausted Script");
            SetupScriptComboBoxContextMenu(_onHeartbeatSelect, "OnHeartbeat Script");
            SetupScriptComboBoxContextMenu(_onUserDefinedSelect, "OnUserDefined Script");

            // Setup context menu for creature table (UTP files)
            SetupCreatureTableContextMenu();
        }

        // Create context menu for script ComboBox controls
        // Allows opening existing scripts in editor, creating new scripts, or viewing resource details
        private void SetupScriptComboBoxContextMenu(ComboBox comboBox, string scriptTypeName)
        {
            if (comboBox == null)
            {
                return;
            }

            var contextMenu = new ContextMenu();
            var menuItems = new List<MenuItem>();

            // "Open in OdyToolNSS" menu item - opens the script if it exists
            var openInEditorItem = new MenuItem
            {
                Header = "Open in OdyToolNSS",
                IsEnabled = false
            };
            openInEditorItem.Click += (sender, e) => OpenScriptInEditor(comboBox, scriptTypeName);
            menuItems.Add(openInEditorItem);

            var findReferencesItem = new MenuItem
            {
                Header = "Find References",
                IsEnabled = false
            };
            findReferencesItem.Click += (sender, e) => ScriptReferenceHelper.FindAndShowScriptReferences(this, comboBox, _installation);
            menuItems.Add(findReferencesItem);

            // Enable/disable based on whether script name is set
            // Note: ComboBox in Avalonia doesn't have TextChanged, use SelectionChanged or TextBox instead
            // For ComboBox, we'll use SelectionChanged and also check Text property if available
            comboBox.SelectionChanged += (sender, e) =>
            {
                string text = comboBox.SelectedItem?.ToString() ?? comboBox.Text ?? string.Empty;
                openInEditorItem.IsEnabled = !string.IsNullOrWhiteSpace(text);
                findReferencesItem.IsEnabled = !string.IsNullOrWhiteSpace(text) && _installation != null;
            };

            // "Create New Script" menu item - creates a new NSS file
            var createNewItem = new MenuItem
            {
                Header = "Create New Script"
            };
            createNewItem.Click += (sender, e) => CreateNewScript(comboBox, scriptTypeName);
            menuItems.Add(createNewItem);

            // "View Resource Location" menu item - shows where the script is located
            var viewLocationItem = new MenuItem
            {
                Header = "View Resource Location",
                IsEnabled = false
            };
            viewLocationItem.Click += (sender, e) => ViewScriptResourceLocation(comboBox, scriptTypeName);
            menuItems.Add(viewLocationItem);

            // Enable/disable based on whether script name is set
            // Note: ComboBox in Avalonia doesn't have TextChanged, use SelectionChanged or TextBox instead
            comboBox.SelectionChanged += (sender, e) =>
            {
                string text = comboBox.SelectedItem?.ToString() ?? comboBox.Text ?? string.Empty;
                viewLocationItem.IsEnabled = !string.IsNullOrWhiteSpace(text);
            };

            // AddRange doesn't exist in Avalonia ItemCollection, use a loop instead
            foreach (var item in menuItems)
            {
                contextMenu.Items.Add(item);
            }
            // Add separator after first menu items (Separator is not a MenuItem, so add directly to Items collection)
            contextMenu.Items.Insert(menuItems.Count - 1, new Separator());

            comboBox.ContextMenu = contextMenu;
        }

        // Open the script referenced in the ComboBox in an appropriate editor
        private void OpenScriptInEditor(ComboBox comboBox, string scriptTypeName)
        {
            if (comboBox == null || _installation == null)
            {
                return;
            }

            string scriptName = comboBox.Text?.Trim();
            if (string.IsNullOrEmpty(scriptName))
            {
                return;
            }

            try
            {
                // Try to find the script resource (NSS source preferred, fallback to NCS)
                var resourceResult = _installation.Resource(scriptName, ResourceType.NSS, null);
                ResourceType resourceType = ResourceType.NSS;

                if (resourceResult == null)
                {
                    // Try compiled version
                    resourceResult = _installation.Resource(scriptName, ResourceType.NCS, null);
                    resourceType = ResourceType.NCS;
                }

                if (resourceResult == null)
                {
                    // Resource not found - show message or create new
                    System.Console.WriteLine($"Script '{scriptName}' not found in installation.");
                    // Optionally create new script here
                    return;
                }

                // Open the script in the NSS editor
                var fileResource = new FileResource(
                    scriptName,
                    resourceType,
                    resourceResult.Data.Length,
                    0,
                    resourceResult.FilePath
                );

                OdyTools.Editors.WindowUtils.OpenResourceEditor(fileResource, _installation, this);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error opening script '{scriptName}': {ex.Message}");
            }
        }

        // Create a new script file and open it in the editor
        private void CreateNewScript(ComboBox comboBox, string scriptTypeName)
        {
            if (_installation == null)
            {
                return;
            }

            try
            {
                // Generate a default script name if not already set
                string scriptName = comboBox.Text?.Trim();
                if (string.IsNullOrEmpty(scriptName))
                {
                    // Generate based on encounter resref and script type
                    string baseName = !string.IsNullOrEmpty(_resrefEdit?.Text)
                        ? _resrefEdit.Text
                        : "m00xx_enc_000";
                    scriptName = $"{baseName}_{scriptTypeName.ToLowerInvariant().Replace(" ", "_")}";
                }

                // Limit to 16 characters (ResRef max length)
                if (scriptName.Length > 16)
                {
                    scriptName = scriptName.Substring(0, 16);
                }

#if !UTE_STANDALONE
                // Create a new NSS editor with empty content
                var nssEditor = new OdyToolNSS(this, _installation);
                nssEditor.New();

                // Show the editor - user will set the resref when saving
                OdyTools.Editors.WindowUtils.AddWindow(nssEditor, show: true);
#else
                OdyTools.Editors.WindowUtils.OpenResourceEditor(
                    null,
                    scriptName,
                    ResourceType.NSS,
                    Array.Empty<byte>(),
                    _installation,
                    this);
#endif
                // Update the combo box with the suggested script name
                // User can change this before saving
                comboBox.Text = scriptName;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error creating new script: {ex.Message}");
            }
        }

        // View the location/details of the script resource
        private void ViewScriptResourceLocation(ComboBox comboBox, string scriptTypeName)
        {
            if (comboBox == null || _installation == null)
            {
                return;
            }

            string scriptName = comboBox.Text?.Trim();
            if (string.IsNullOrEmpty(scriptName))
            {
                return;
            }

            try
            {
                // Find the script resource
                var locations = _installation.Locations(
                    new List<ResourceIdentifier> { new ResourceIdentifier(scriptName, ResourceType.NSS) },
                    null,
                    null);

                var nssIdentifier = new ResourceIdentifier(scriptName, ResourceType.NSS);
                if (locations.Count > 0 && locations.ContainsKey(nssIdentifier) &&
                    locations[nssIdentifier].Count > 0)
                {
                    var foundLocations = locations[nssIdentifier];
                    // Show dialog with all found locations
                    var dialog = new ResourceLocationDialog(
                        this,
                        scriptName,
                        ResourceType.NSS,
                        foundLocations,
                        _installation);
                    dialog.ShowDialog(this);
                }
                else
                {
                    // Try compiled version
                    locations = _installation.Locations(
                        new List<ResourceIdentifier> { new ResourceIdentifier(scriptName, ResourceType.NCS) },
                        null,
                        null);

                    var ncsIdentifier = new ResourceIdentifier(scriptName, ResourceType.NCS);
                    if (locations.Count > 0 && locations.ContainsKey(ncsIdentifier) &&
                        locations[ncsIdentifier].Count > 0)
                    {
                        var foundLocations = locations[ncsIdentifier];
                        // Show dialog with all found locations
                        var dialog = new ResourceLocationDialog(
                            this,
                            scriptName,
                            ResourceType.NCS,
                            foundLocations,
                            _installation);
                        dialog.ShowDialog(this);
                    }
                    else
                    {
                        // Show "not found" message
                        _ = DialogHelper.ShowAsync("Resource Not Found", $"Script '{scriptName}' not found in installation.\n\nSearched for:\n- {scriptName}.nss\n- {scriptName}.ncs", ButtonEnum.Ok, IconType.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error viewing script location '{scriptName}': {ex.Message}");
            }
        }

        // Setup context menu for creature table (UTP files)
        private void SetupCreatureTableContextMenu()
        {
            if (_creatureTable == null)
            {
                return;
            }

            var contextMenu = new ContextMenu();
            var menuItems = new List<MenuItem>();

            // "Open in OdyToolUTC" menu item
            var openCreatureItem = new MenuItem
            {
                Header = "Open in OdyToolUTC",
                IsEnabled = false
            };
            openCreatureItem.Click += (sender, e) => OpenCreatureInEditor();
            menuItems.Add(openCreatureItem);

            // "Create New Creature" menu item
            var createNewCreatureItem = new MenuItem
            {
                Header = "Create New Creature"
            };
            createNewCreatureItem.Click += (sender, e) => CreateNewCreature();
            menuItems.Add(createNewCreatureItem);

            // "View Creature Resource Location" menu item
            var viewCreatureLocationItem = new MenuItem
            {
                Header = "View Creature Resource Location",
                IsEnabled = false
            };
            viewCreatureLocationItem.Click += (sender, e) => ViewCreatureResourceLocation();
            menuItems.Add(viewCreatureLocationItem);

            // AddRange doesn't exist in Avalonia ItemCollection, use a loop instead
            foreach (var item in menuItems)
            {
                contextMenu.Items.Add(item);
            }
            // Add separator after first menu items (Separator is not a MenuItem, so add directly to Items collection)
            contextMenu.Items.Insert(menuItems.Count - 1, new Separator());
            _creatureTable.ContextMenu = contextMenu;

            // Update menu enabled state when selection changes
            _creatureTable.SelectionChanged += (sender, e) =>
            {
                bool hasSelection = _creatureTable.SelectedItem != null;
                openCreatureItem.IsEnabled = hasSelection;
                viewCreatureLocationItem.IsEnabled = hasSelection;
            };
        }

        // Open the selected creature in the UTC editor
        private void OpenCreatureInEditor()
        {
            if (_creatureTable?.SelectedItem == null || _installation == null)
            {
                return;
            }

            try
            {
                // Extract ResRef from selected row
                var selectedItem = _creatureTable.SelectedItem;
                var itemType = selectedItem.GetType();
                var resRefProp = itemType.GetProperty("ResRef");

                if (resRefProp == null)
                {
                    return;
                }

                var resRefValue = resRefProp.GetValue(selectedItem);
                if (resRefValue == null || string.IsNullOrEmpty(resRefValue.ToString()))
                {
                    return;
                }

                string creatureResRef = resRefValue.ToString().Trim();

                // Find the creature resource (UTC)
                var resourceResult = _installation.Resource(creatureResRef, EncounterCreatureResourceType, null);

                if (resourceResult == null)
                {
                    System.Console.WriteLine($"Creature '{creatureResRef}' not found in installation.");
                    return;
                }

                // Open the creature in the UTC editor
                var fileResource = new FileResource(
                    creatureResRef,
                    EncounterCreatureResourceType,
                    resourceResult.Data.Length,
                    0,
                    resourceResult.FilePath
                );

                OdyTools.Editors.WindowUtils.OpenResourceEditor(fileResource, _installation, this);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error opening creature: {ex.Message}");
            }
        }

        // Create a new creature file and open it in the editor
        private void CreateNewCreature()
        {
            if (_installation == null)
            {
                return;
            }

            try
            {
                // Generate a default creature name based on encounter resref
                string baseName = !string.IsNullOrEmpty(_resrefEdit?.Text)
                    ? _resrefEdit.Text
                    : "m00xx_enc_000";
                string creatureName = $"{baseName}_cre_000";

                // Limit to 16 characters (ResRef max length)
                if (creatureName.Length > 16)
                {
                    creatureName = creatureName.Substring(0, 16);
                }

#if !UTE_STANDALONE
                // Create a new UTC editor
                var utcEditor = new OdyToolUTC(this, _installation);
                utcEditor.New();

                // Show the editor
                OdyTools.Editors.WindowUtils.AddWindow(utcEditor, show: true);
#else
                OdyTools.Editors.WindowUtils.OpenResourceEditor(
                    null,
                    creatureName,
                    EncounterCreatureResourceType,
                    Array.Empty<byte>(),
                    _installation,
                    this);
#endif
                // Optionally add the new creature to the encounter table
                // User can manually add it via the Add button after creating
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error creating new creature: {ex.Message}");
            }
        }

        // View the location/details of the selected creature resource
        private void ViewCreatureResourceLocation()
        {
            if (_creatureTable?.SelectedItem == null || _installation == null)
            {
                return;
            }

            try
            {
                // Extract ResRef from selected row
                var selectedItem = _creatureTable.SelectedItem;
                var itemType = selectedItem.GetType();
                var resRefProp = itemType.GetProperty("ResRef");

                if (resRefProp == null)
                {
                    return;
                }

                var resRefValue = resRefProp.GetValue(selectedItem);
                if (resRefValue == null || string.IsNullOrEmpty(resRefValue.ToString()))
                {
                    return;
                }

                string creatureResRef = resRefValue.ToString().Trim();

                // Find the creature resource location
                var creatureIdentifier = new ResourceIdentifier(creatureResRef, EncounterCreatureResourceType);
                var locations = _installation.Locations(
                    new List<ResourceIdentifier> { creatureIdentifier },
                    null,
                    null);

                if (locations.Count > 0 && locations.ContainsKey(creatureIdentifier) &&
                    locations[creatureIdentifier].Count > 0)
                {
                    var foundLocations = locations[creatureIdentifier];
                    // Show dialog with all found locations
                    var dialog = new ResourceLocationDialog(
                        this,
                        creatureResRef,
                        EncounterCreatureResourceType,
                        foundLocations,
                        _installation);
                    dialog.ShowDialog(this);
                }
                else
                {
                    // Show "not found" message
                    _ = DialogHelper.ShowAsync("Resource Not Found", EncounterCreatureMissingMessage(creatureResRef), ButtonEnum.Ok, IconType.Info);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error viewing creature location: {ex.Message}");
            }
        }

        internal static string EncounterCreatureMissingMessage(string creatureResRef)
        {
            return $"Creature '{creatureResRef}' not found in installation.\n\nSearched for:\n- {creatureResRef}.{EncounterCreatureResourceType.Extension}";
        }
    }
}
