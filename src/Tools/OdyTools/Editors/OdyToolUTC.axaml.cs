using BioWare.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using BioWare.Extract;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Styling;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using BioWare;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.LTR;
using BioWare.Resource.Formats.TPC;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource;
using BioWare.Tools;
using UTCHelpers = BioWare.Resource.Formats.GFF.Generics.UTC.UTCHelpers;
using UTCClass = BioWare.Resource.Formats.GFF.Generics.UTC.UTCClass;
using OdyTools.Common;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Utils;
using OdyTools.Widgets;
using Game = BioWare.Common.BioWareGame;
using GFFAuto = BioWare.Resource.Formats.GFF.GFFAuto;
using UTC = BioWare.Resource.Formats.GFF.Generics.UTC.UTC;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using IconType = MsBox.Avalonia.Enums.Icon;
using BioWare.Extract.Capsule;

namespace OdyTools.Editors
{
    public partial class OdyToolUTC : Editor
    {
        private const int MinEditorWidth = 798;
        private const int MinEditorHeight = 553;
        private const int UndoMaxLevels = 30;

        private UTC _utc;

        private Avalonia.Controls.TextBlock _statusText;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;
        private string _findText = "";
        private bool _findMatchCase;
        private OdyToolUTCSettings _settings;
        private static GlobalSettings _globalSettings;
        private bool _appearancePreviewHooked;

        // UI Controls - Basic
        private LocalizedStringEdit _firstNameEdit;
        private Button _firstNameRandomBtn;
        private LocalizedStringEdit _lastNameEdit;
        private Button _lastNameRandomBtn;
        private TextBox _tagEdit;
        private Button _tagGenerateBtn;
        private TextBox _resrefEdit;
        private ComboBox _appearanceSelect;
        private ComboBox _soundsetSelect;
        private ComboBox _portraitSelect;
        private Image _portraitPicture;
        private ModelRenderer _previewRenderer;
        private Slider _alignmentSlider;
        private ComboBox _conversationEdit;
        private Button _conversationModifyBtn;
        private Button _inventoryBtn;
        private TextBlock _inventoryCountLabel;

        // UI Controls - Advanced
        private CheckBox _disarmableCheckbox;
        private CheckBox _noPermDeathCheckbox;
        private CheckBox _min1HpCheckbox;
        private CheckBox _plotCheckbox;
        private CheckBox _isPcCheckbox;
        private CheckBox _noReorientateCheckbox;
        private CheckBox _noBlockCheckbox;
        private CheckBox _hologramCheckbox;
        private ComboBox _raceSelect;
        private ComboBox _subraceSelect;
        private ComboBox _speedSelect;
        private ComboBox _factionSelect;
        private ComboBox _genderSelect;
        private ComboBox _perceptionSelect;
        private NumericUpDown _challengeRatingSpin;
        private NumericUpDown _blindSpotSpin;
        private NumericUpDown _multiplierSetSpin;

        // UI Controls - Stats
        private NumericUpDown _strengthSpin;
        private NumericUpDown _dexteritySpin;
        private NumericUpDown _constitutionSpin;
        private NumericUpDown _intelligenceSpin;
        private NumericUpDown _wisdomSpin;
        private NumericUpDown _charismaSpin;
        private NumericUpDown _computerUseSpin;
        private NumericUpDown _demolitionsSpin;
        private NumericUpDown _stealthSpin;
        private NumericUpDown _awarenessSpin;
        private NumericUpDown _persuadeSpin;
        private NumericUpDown _repairSpin;
        private NumericUpDown _securitySpin;
        private NumericUpDown _treatInjurySpin;
        private NumericUpDown _fortitudeSpin;
        private NumericUpDown _reflexSpin;
        private NumericUpDown _willSpin;
        private NumericUpDown _armorClassSpin;
        private NumericUpDown _baseHpSpin;
        private NumericUpDown _currentHpSpin;
        private NumericUpDown _maxHpSpin;
        private NumericUpDown _currentFpSpin;
        private NumericUpDown _maxFpSpin;

        // UI Controls - Classes
        private ComboBox _class1Select;
        private NumericUpDown _class1LevelSpin;
        private ComboBox _class2Select;
        private NumericUpDown _class2LevelSpin;

        // UI Controls - Feats and Powers
        private ListBox _featList;
        private ListBox _powerList;

        // UI Controls - Scripts (editable combos with prefilled script resnames, matching vendor utc.py FilterComboBox)
        private Dictionary<string, ComboBox> _scriptFields;

        // UI Controls - Comments
        private TextBox _commentsEdit;
        private Expander _commentsExpander; // For tab title update testing

        public OdyToolUTC() : this(null, null) { }
        public OdyToolUTC(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolUTC", "creature",
                new[] { ResourceType.UTC, ResourceType.BTC, ResourceType.BIC },
                new[] { ResourceType.UTC, ResourceType.BTC, ResourceType.BIC },
                installation)
        {
            _utc = new UTC();
            _scriptFields = new Dictionary<string, ComboBox>();
            _settings = new OdyToolUTCSettings();
            _globalSettings = new GlobalSettings();

            ApplyInstallationFromSettings(_settings);

            InitializeComponent();
            SetupUI();
            SetupMenuHandlers();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            Opened += (s, e) => { UpdateStatusBar(); _tagEdit?.Focus(); };
            KeyDown += OnWindowKeyDown;
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
            fileMenu.Items.Add(new MenuItem { Header = "UTC _Settings...", Name = "actionUTCSettings" });
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(new MenuItem { Header = "_Revert", Name = "actionRevert" });
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(new MenuItem { Header = "E_xit", Name = "actionExit" });
            menu.Items.Add(fileMenu);
            var editMenu = new MenuItem { Header = "_Edit" };
            editMenu.Items.Add(new MenuItem { Header = "_Undo", Name = "actionUndo" });
            editMenu.Items.Add(new MenuItem { Header = "_Redo", Name = "actionRedo" });
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(new MenuItem { Header = "Find...", Name = "actionFind" });
            editMenu.Items.Add(new MenuItem { Header = "Find _Next", Name = "actionFindNext" });
            menu.Items.Add(editMenu);
            return menu;
        }

        private void InitializeComponent()
        {
            try { AvaloniaXamlLoader.Load(this); } catch { /* XAML not available - use programmatic UI */ }
            SetupProgrammaticUI();
        }

        private void SetupProgrammaticUI()
        {
            // Layout: left = model preview (resizable), right = tabbed properties (matching utc.ui)
            var mainGrid = new Grid();
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(380, GridUnitType.Pixel)) { MinWidth = 350 });
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            mainGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            // Left panel: model preview + model info (like utc.ui verticalLayout_preview)
            var leftPanel = new Grid();
            leftPanel.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            leftPanel.RowDefinitions.Add(new RowDefinition(GridLength.Auto));

            _previewRenderer = new ModelRenderer
            {
                MinWidth = 350,
                MinHeight = 200,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            if (_installation != null)
            {
                _previewRenderer.Installation = _installation;
            }
            Grid.SetRow(_previewRenderer, 0);
            leftPanel.Children.Add(_previewRenderer);

            var modelInfoExpander = new Expander
            {
                Header = "Model Info",
                IsExpanded = false,
                Padding = new Avalonia.Thickness(4),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
            };
            var modelInfoLabel = new TextBlock
            {
                Text = "Summary of the creature's 3D model (from Appearance). Expand to see details.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Avalonia.Thickness(4),
            };
            modelInfoExpander.Content = modelInfoLabel;
            Grid.SetRow(modelInfoExpander, 1);
            leftPanel.Children.Add(modelInfoExpander);

            var leftBorder = new Border
            {
                Child = leftPanel,
                Background = Avalonia.Media.Brushes.White,
                BorderThickness = new Avalonia.Thickness(0, 0, 1, 0),
                BorderBrush = Avalonia.Media.Brushes.LightGray,
                Padding = new Avalonia.Thickness(4),
            };
            Grid.SetColumn(leftBorder, 0);
            mainGrid.Children.Add(leftBorder);

            var splitter = new GridSplitter
            {
                Width = 4,
                Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Color.FromRgb(0xD7, 0xDB, 0xE6)),
                ResizeDirection = GridResizeDirection.Columns,
            };
            Grid.SetColumn(splitter, 1);
            mainGrid.Children.Add(splitter);

            // Right panel: tabbed properties (matching utc.ui tabWidget)
            var tabControl = new TabControl();
#if !NET48
            tabControl.Styles.Add(new Style(x => x.OfType<TabStripItem>())
            {
                Setters =
                {
                    new Setter(FontSizeProperty, 11.0),
                    new Setter(PaddingProperty, new Thickness(6, 3)),
                }
            });
#endif
            var basicTab = BuildBasicTab();
            var advancedTab = BuildAdvancedTab();
            var statsTab = BuildStatsTab();
            var classesTab = BuildClassesTab();
            var featsPowersTab = BuildFeatsPowersTab();
            var scriptsTab = BuildScriptsTab();
            var commentsTab = BuildCommentsTab();

            tabControl.Items.Add(new TabItem { Header = "Basic", Content = new ScrollViewer { Content = basicTab } });
            tabControl.Items.Add(new TabItem { Header = "Advanced", Content = new ScrollViewer { Content = advancedTab } });
            tabControl.Items.Add(new TabItem { Header = "Stats", Content = new ScrollViewer { Content = statsTab } });
            tabControl.Items.Add(new TabItem { Header = "Classes", Content = new ScrollViewer { Content = classesTab } });
            tabControl.Items.Add(new TabItem { Header = "Feats & Powers", Content = new ScrollViewer { Content = featsPowersTab } });
            tabControl.Items.Add(new TabItem { Header = "Scripts", Content = new ScrollViewer { Content = scriptsTab } });
            tabControl.Items.Add(new TabItem { Header = "Comments", Content = new ScrollViewer { Content = commentsTab } });

            var rightScroll = new ScrollViewer
            {
                Content = tabControl,
                Padding = new Avalonia.Thickness(8),
                Background = Avalonia.Media.Brushes.White,
            };
            Grid.SetColumn(rightScroll, 2);
            mainGrid.Children.Add(rightScroll);

            var dock = new DockPanel();
            dock.Children.Add(BuildMenu());
            DockPanel.SetDock(dock.Children[0], Dock.Top);
            dock.Children.Add(mainGrid);
            _statusText = new Avalonia.Controls.TextBlock { Name = "statusText", Text = "Creature", Margin = new Avalonia.Thickness(4, 2) };
            dock.Children.Add(_statusText);
            DockPanel.SetDock(_statusText, Dock.Bottom);
            SetContentOrInject(dock);
            AttachCommitHandlers();
        }

        private Panel BuildBasicTab()
        {
            var panel = new StackPanel { Spacing = 8 };

            // Profile group (matching utc.ui groupBox)
            var profileGroup = new Expander { Header = "Profile", IsExpanded = true };
            var profilePanel = new StackPanel { Spacing = 6 };
            AddFormRow(profilePanel, "First Name:", _firstNameEdit = new LocalizedStringEdit(), _firstNameRandomBtn = new Button { Content = "?" }, () => RandomizeFirstName());
            AddFormRow(profilePanel, "Last Name:", _lastNameEdit = new LocalizedStringEdit(), _lastNameRandomBtn = new Button { Content = "?" }, () => RandomizeLastName());
            AddFormRow(profilePanel, "Tag:", _tagEdit = new TextBox(), _tagGenerateBtn = new Button { Content = "-" }, () => GenerateTag());
            AddFormRow(profilePanel, "ResRef:", _resrefEdit = new TextBox { MaxLength = 16 });
            ReferenceSearchHelper.AttachTagFindReferencesMenu(_tagEdit, this, _installation);
            ReferenceSearchHelper.AttachTemplateResRefFindReferencesMenu(_resrefEdit, this, _installation);
            AddFormRow(profilePanel, "Appearance:", _appearanceSelect = new ComboBox());
            AddFormRow(profilePanel, "Soundset:", _soundsetSelect = new ComboBox());
            AddFormRow(profilePanel, "Conversation:", _conversationEdit = new ComboBox { IsEditable = true }, _conversationModifyBtn = new Button { Content = "Edit" }, () => EditConversation());
            SetupConversationComboBoxContextMenu(_conversationEdit);
            profileGroup.Content = profilePanel;
            panel.Children.Add(profileGroup);
            HookAppearancePreviewEvent();

            // Inventory group
            var invGroup = new Expander { Header = "Inventory", IsExpanded = true };
            var invPanel = new StackPanel { Spacing = 4 };
            _inventoryCountLabel = new TextBlock { Text = "Total Items: 0" };
            _inventoryBtn = new Button { Content = "Edit Inventory" };
            EditorHelpers.BindClick(_inventoryBtn, OpenInventory);
            invPanel.Children.Add(_inventoryCountLabel);
            invPanel.Children.Add(_inventoryBtn);
            invGroup.Content = invPanel;
            panel.Children.Add(invGroup);

            // Portrait group
            var portraitGroup = new Expander { Header = "Portrait", IsExpanded = true };
            var portraitPanel = new StackPanel { Spacing = 6 };
            _portraitPicture = new Image
            {
                Width = 64,
                Height = 64,
                Stretch = Avalonia.Media.Stretch.Uniform
            };
            portraitPanel.Children.Add(_portraitPicture);
            AddFormRow(portraitPanel, "Portrait:", _portraitSelect = new ComboBox());
            EditorHelpers.BindSelectionChanged(_portraitSelect, PortraitChanged);
            AddFormRow(portraitPanel, "Alignment:", _alignmentSlider = new Slider { Minimum = 0, Maximum = 100, Value = 50 });
            EditorHelpers.BindValueChanged(_alignmentSlider, PortraitChanged);
            portraitGroup.Content = portraitPanel;
            panel.Children.Add(portraitGroup);

            return panel;
        }

        private static void AddFormRow(Panel parent, string labelText, Control control, Button actionBtn = null, Action action = null)
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
            row.Children.Add(new TextBlock { Text = labelText, Width = 100, VerticalAlignment = VerticalAlignment.Center });
            control.VerticalAlignment = VerticalAlignment.Center;
            row.Children.Add(control);
            if (actionBtn != null && action != null)
            {
                actionBtn.Click += (s, e) => action();
                row.Children.Add(actionBtn);
            }
            parent.Children.Add(row);
        }

        private Panel BuildAdvancedTab()
        {
            var panel = new StackPanel { Spacing = 8 };
            var flagsGroup = new Expander { Header = "Flags", IsExpanded = true };
            var flagsPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 24 };
            var col1 = new StackPanel { Spacing = 4 };
            var col2 = new StackPanel { Spacing = 4 };
            col1.Children.Add(_disarmableCheckbox = new CheckBox { Content = "Disarmable" });
            col1.Children.Add(_noPermDeathCheckbox = new CheckBox { Content = "No Perm Death" });
            col1.Children.Add(_min1HpCheckbox = new CheckBox { Content = "Min 1 HP" });
            col1.Children.Add(_plotCheckbox = new CheckBox { Content = "Plot" });
            col1.Children.Add(_isPcCheckbox = new CheckBox { Content = "Is PC" });
            col2.Children.Add(_noReorientateCheckbox = new CheckBox { Content = "No Reorientate" });
            col2.Children.Add(_noBlockCheckbox = new CheckBox { Content = "No Block" });
            col2.Children.Add(_hologramCheckbox = new CheckBox { Content = "Hologram" });
            flagsPanel.Children.Add(col1);
            flagsPanel.Children.Add(col2);
            flagsGroup.Content = flagsPanel;
            panel.Children.Add(flagsGroup);

            var raceGroup = new Expander { Header = "Race", IsExpanded = true };
            var racePanel = new StackPanel { Spacing = 6 };
            AddFormRow(racePanel, "Race:", _raceSelect = new ComboBox());
            AddFormRow(racePanel, "Subrace:", _subraceSelect = new ComboBox());
            raceGroup.Content = racePanel;
            panel.Children.Add(raceGroup);

            var otherGroup = new Expander { Header = "Other", IsExpanded = true };
            var otherPanel = new StackPanel { Spacing = 6 };
            AddFormRow(otherPanel, "Speed:", _speedSelect = new ComboBox());
            AddFormRow(otherPanel, "Faction:", _factionSelect = new ComboBox());
            AddFormRow(otherPanel, "Gender:", _genderSelect = new ComboBox());
            AddFormRow(otherPanel, "Perception:", _perceptionSelect = new ComboBox());
            AddFormRow(otherPanel, "Challenge Rating:", _challengeRatingSpin = new NumericUpDown { Minimum = 0, Maximum = decimal.MaxValue });
            AddFormRow(otherPanel, "Blind Spot:", _blindSpotSpin = new NumericUpDown { Minimum = 0, Maximum = decimal.MaxValue });
            AddFormRow(otherPanel, "Multiplier Set:", _multiplierSetSpin = new NumericUpDown { Minimum = 0, Maximum = 255 });
            otherGroup.Content = otherPanel;
            panel.Children.Add(otherGroup);
            return panel;
        }

        private Panel BuildStatsTab()
        {
            var panel = new StackPanel { Spacing = 8 };
            _strengthSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _dexteritySpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _constitutionSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _intelligenceSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _wisdomSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _charismaSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            AddFormRow(panel, "Strength:", _strengthSpin);
            AddFormRow(panel, "Dexterity:", _dexteritySpin);
            AddFormRow(panel, "Constitution:", _constitutionSpin);
            AddFormRow(panel, "Intelligence:", _intelligenceSpin);
            AddFormRow(panel, "Wisdom:", _wisdomSpin);
            AddFormRow(panel, "Charisma:", _charismaSpin);

            _computerUseSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _demolitionsSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _stealthSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _awarenessSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _persuadeSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _repairSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _securitySpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _treatInjurySpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            AddFormRow(panel, "Computer Use:", _computerUseSpin);
            AddFormRow(panel, "Demolitions:", _demolitionsSpin);
            AddFormRow(panel, "Stealth:", _stealthSpin);
            AddFormRow(panel, "Awareness:", _awarenessSpin);
            AddFormRow(panel, "Persuade:", _persuadeSpin);
            AddFormRow(panel, "Repair:", _repairSpin);
            AddFormRow(panel, "Security:", _securitySpin);
            AddFormRow(panel, "Treat Injury:", _treatInjurySpin);

            _fortitudeSpin = new NumericUpDown { Minimum = -32768, Maximum = 32767 };
            _reflexSpin = new NumericUpDown { Minimum = -32768, Maximum = 32767 };
            _willSpin = new NumericUpDown { Minimum = -32768, Maximum = 32767 };
            _armorClassSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _baseHpSpin = new NumericUpDown { Minimum = 0, Maximum = 32767 };
            _currentHpSpin = new NumericUpDown { Minimum = 0, Maximum = 32767 };
            _maxHpSpin = new NumericUpDown { Minimum = 0, Maximum = 32767 };
            _currentFpSpin = new NumericUpDown { Minimum = 0, Maximum = 32767 };
            _maxFpSpin = new NumericUpDown { Minimum = 0, Maximum = 32767 };
            AddFormRow(panel, "Fortitude Bonus:", _fortitudeSpin);
            AddFormRow(panel, "Reflex Bonus:", _reflexSpin);
            AddFormRow(panel, "Will Bonus:", _willSpin);
            AddFormRow(panel, "Natural AC:", _armorClassSpin);
            AddFormRow(panel, "Base HP:", _baseHpSpin);
            AddFormRow(panel, "Current HP:", _currentHpSpin);
            AddFormRow(panel, "Max HP:", _maxHpSpin);
            AddFormRow(panel, "Current FP:", _currentFpSpin);
            AddFormRow(panel, "Max FP:", _maxFpSpin);
            return panel;
        }

        private Panel BuildClassesTab()
        {
            var panel = new StackPanel { Spacing = 8 };
            AddFormRow(panel, "Class 1:", _class1Select = new ComboBox());
            AddFormRow(panel, "Class 1 Level:", _class1LevelSpin = new NumericUpDown { Minimum = 0, Maximum = 50 });
            AddFormRow(panel, "Class 2:", _class2Select = new ComboBox());
            AddFormRow(panel, "Class 2 Level:", _class2LevelSpin = new NumericUpDown { Minimum = 0, Maximum = 50 });
            return panel;
        }

        private Panel BuildFeatsPowersTab()
        {
            var panel = new StackPanel { Spacing = 8 };
            panel.Children.Add(new TextBlock { Text = "Feats:" });
            panel.Children.Add(_featList = new ListBox());
            panel.Children.Add(new TextBlock { Text = "Powers:" });
            panel.Children.Add(_powerList = new ListBox());
            return panel;
        }

        private Panel BuildScriptsTab()
        {
            try
            {
                _scriptFields.Clear();
                var scriptsTab = new OdyToolUTC_ScriptsTab();
                string[] scriptNames = { "OnBlocked", "OnAttacked", "OnNotice", "OnDialog", "OnDamaged",
                    "OnDisturbed", "OnDeath", "OnEndRound", "OnEndDialog", "OnHeartbeat", "OnSpawn", "OnSpell", "OnUserDefined" };
                foreach (string scriptName in scriptNames)
                {
                    string controlName = scriptName.ToLowerInvariant() + "Edit";
                    var scriptCombo = EditorHelpers.FindControlSafe<ComboBox>(scriptsTab, controlName);
                    if (scriptCombo != null)
                    {
                        _scriptFields[scriptName] = scriptCombo;
                        SetupScriptComboBoxContextMenu(scriptCombo, scriptName);
                    }
                }
                if (_scriptFields.Count == scriptNames.Length)
                    return scriptsTab;
            }
            catch { /* fallback to programmatic */ }

            var panel = new StackPanel { Spacing = 6 };
            string[] names = { "OnBlocked", "OnAttacked", "OnNotice", "OnDialog", "OnDamaged",
                "OnDisturbed", "OnDeath", "OnEndRound", "OnEndDialog", "OnHeartbeat", "OnSpawn", "OnSpell", "OnUserDefined" };
            foreach (string scriptName in names)
            {
                var scriptCombo = new ComboBox { IsEditable = true };
                _scriptFields[scriptName] = scriptCombo;
                SetupScriptComboBoxContextMenu(scriptCombo, scriptName);
                AddFormRow(panel, scriptName + ":", scriptCombo);
            }
            return panel;
        }

        private void PopulateScriptComboBoxes()
        {
            if (_installation == null || _scriptFields == null)
            {
                return;
            }

            try
            {
                var relevantResources = _installation.GetRelevantResources(ResourceType.NCS, FilepathPublic);
                var resnames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (relevantResources != null)
                {
                    foreach (var res in relevantResources)
                    {
                        if (res != null && !string.IsNullOrEmpty(res.ResName))
                        {
                            resnames.Add(res.ResName.ToLowerInvariant());
                        }
                    }
                }

                var sorted = resnames.OrderBy(s => s, StringComparer.OrdinalIgnoreCase).ToList();
                foreach (var kv in _scriptFields)
                {
                    if (kv.Value == null) continue;
                    kv.Value.Items.Clear();
                    foreach (string r in sorted)
                    {
                        kv.Value.Items.Add(r);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to populate script combo boxes: {ex.Message}");
            }
        }

        private void SetupScriptComboBoxContextMenu(ComboBox comboBox, string scriptTypeName)
        {
            var contextMenu = new ContextMenu();
            var openInEditorItem = new MenuItem
            {
                Header = "Open in OdyToolNSS",
                IsEnabled = false
            };
            openInEditorItem.Click += (sender, e) => OpenScriptInEditor(comboBox, scriptTypeName);
            contextMenu.Items.Add(openInEditorItem);

            var findReferencesItem = new MenuItem
            {
                Header = "Find References",
                IsEnabled = false
            };
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

                string filepath = resourceResult.FilePath;
                byte[] data = resourceResult.Data;
                if (data == null && !string.IsNullOrEmpty(filepath) && System.IO.File.Exists(filepath))
                {
                    data = System.IO.File.ReadAllBytes(filepath);
                }
                if (data == null)
                {
                    System.Console.WriteLine($"No data for script '{scriptName}'.");
                    return;
                }

                var fileResource = new FileResource(scriptName, resourceType, data.Length, 0, filepath ?? string.Empty);
                WindowUtils.OpenResourceEditor(fileResource, _installation, this);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"OpenScriptInEditor failed: {ex.Message}");
            }
        }

        private void SetupConversationComboBoxContextMenu(ComboBox comboBox)
        {
            if (comboBox == null) return;

            var contextMenu = new ContextMenu();
            var openInEditorItem = new MenuItem { Header = "Open in OdyToolDLG", IsEnabled = false };
            openInEditorItem.Click += (sender, e) => EditConversation();
            contextMenu.Items.Add(openInEditorItem);

            void UpdateOpenEnabled(object s, EventArgs e)
            {
                string text = comboBox.SelectedItem?.ToString() ?? comboBox.Text ?? string.Empty;
                openInEditorItem.IsEnabled = !string.IsNullOrWhiteSpace(text);
            }
            comboBox.SelectionChanged += UpdateOpenEnabled;
            contextMenu.Opened += (s, e) => UpdateOpenEnabled(s, e);
            comboBox.ContextMenu = contextMenu;
        }

        private void PopulateConversationComboBox()
        {
            if (_installation == null || _conversationEdit == null) return;
            try
            {
                var relevantResources = _installation.GetRelevantResources(ResourceType.DLG, FilepathPublic);
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
                _conversationEdit.Items.Clear();
                foreach (string r in sorted)
                    _conversationEdit.Items.Add(r);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to populate conversation combo box: {ex.Message}");
            }
        }

        private Panel BuildCommentsTab()
        {
            var panel = new StackPanel { Spacing = 6 };
            _commentsExpander = new Expander { Header = "Comments", IsExpanded = true };
            var commentsPanel = new StackPanel { Spacing = 4 };
            var commentsLabel = new TextBlock { Text = "Comment:" };
            _commentsEdit = new TextBox { AcceptsReturn = true, AcceptsTab = true };
            _commentsEdit.TextChanged += (s, e) => UpdateCommentsTabTitle();
            commentsPanel.Children.Add(commentsLabel);
            commentsPanel.Children.Add(_commentsEdit);
            _commentsExpander.Content = commentsPanel;
            panel.Children.Add(_commentsExpander);
            return panel;
        }

        private void SetupUI()
        {
            if (_statusText == null)
                _statusText = EditorHelpers.FindControlSafe<Avalonia.Controls.TextBlock>(this, "statusText");

            var previewRenderer = EditorHelpers.FindControlSafe<ModelRenderer>(this, "previewRenderer");
            if (previewRenderer != null)
            {
                _previewRenderer = previewRenderer;
            }

            if (_previewRenderer != null && _installation != null)
            {
                _previewRenderer.Installation = _installation;
            }

            HookAppearancePreviewEvent();
        }

        private void HookAppearancePreviewEvent()
        {
            if (_appearancePreviewHooked || _appearanceSelect == null)
            {
                return;
            }

            _appearanceSelect.SelectionChanged += (s, e) => RefreshCreaturePreview();
            _appearancePreviewHooked = true;
        }

        private void AttachCommitHandlers()
        {
            void OnCommit(object s, EventArgs e) { if (!_undoRedoInProgress) PushState(); }
            EditorHelpers.BindLostFocus(_tagEdit, OnCommit);
            EditorHelpers.BindLostFocus(_resrefEdit, OnCommit);
            EditorHelpers.BindLostFocus(_conversationEdit, OnCommit);
            EditorHelpers.BindLostFocus(_commentsEdit, OnCommit);
            if (_scriptFields != null)
                foreach (var kv in _scriptFields)
                    EditorHelpers.BindLostFocus(kv.Value, OnCommit);
        }

        protected override string SettingsMenuActionName => "actionUTCSettings";

        protected override async Task ShowSettingsDialogAsync()
        {
            var dialog = new UTCSettingsDialog();
            bool result = await dialog.ShowDialog<bool>(this);
            if (result == true)
            {
                ApplyInstallationFromSettings(_settings);
                if (_previewRenderer != null) _previewRenderer.Installation = _installation;
                RefreshCreaturePreview();
                PopulateScriptComboBoxes();
                UpdateStatusBar();
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
                ("actionFindNext", FindNextMatch),
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
            {
                _utc = new UTC();
                LoadUTC(_utc);
            }
            else
            {
                try
                {
                    var gff = GFF.FromBytes(data);
                    _utc = UTCHelpers.ConstructUtc(gff);
                    LoadUTC(_utc);
                }
                catch
                {
                    _utc = new UTC();
                    LoadUTC(_utc);
                }
            }
            _undoRedoInProgress = true;
            try { UpdateStatusBar(); UpdateItemCount(); }
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
                System.Console.WriteLine($"Revert failed: {ex}");
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
                string text = _utc == null ? "Creature" : (_utc.Tag ?? "Creature");
                if (!string.IsNullOrEmpty(_utc?.ResRef?.ToString())) text += " | " + _utc.ResRef;
                var c = _statusText ?? EditorHelpers.FindControlSafe<Avalonia.Controls.TextBlock>(this, "statusText");
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
            var findBox = new TextBox { Watermark = "Find what:", Text = _findText, Margin = new Thickness(8) };
            var matchCase = new CheckBox { Content = "Match case", IsChecked = _findMatchCase, Margin = new Thickness(8) };
            var findNext = new Button { Content = "Find Next", Margin = new Thickness(8) };
            var closeBtn = new Button { Content = "Close", Margin = new Thickness(8) };
            var panel = new StackPanel { Margin = new Thickness(10) };
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
            if (string.IsNullOrEmpty(_findText)) return;
            string t = _findMatchCase ? _findText : _findText.ToLowerInvariant();
            bool Match(string value) => value != null && (_findMatchCase ? value : value.ToLowerInvariant()).Contains(t);
            if (Match(_tagEdit?.Text) && _tagEdit != null) { _tagEdit.Focus(); return; }
            if (Match(_resrefEdit?.Text) && _resrefEdit != null) { _resrefEdit.Focus(); return; }
            if (Match(_conversationEdit?.Text) && _conversationEdit != null) { _conversationEdit.Focus(); return; }
            if (Match(_commentsEdit?.Text) && _commentsEdit != null) { _commentsEdit.Focus(); return; }
            if (_scriptFields != null)
                foreach (var kv in _scriptFields)
                    if (Match(kv.Value?.Text)) { kv.Value.Focus(); return; }
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (e.KeyModifiers & KeyModifiers.Control) != 0) { Save(); e.Handled = true; return; }
            if (e.Key == Key.Z && (e.KeyModifiers & KeyModifiers.Control) != 0) { Undo(); e.Handled = true; return; }
            if (e.Key == Key.Y && (e.KeyModifiers & KeyModifiers.Control) != 0) { Redo(); e.Handled = true; return; }
            if (e.Key == Key.F && (e.KeyModifiers & KeyModifiers.Control) != 0) { ShowFindDialog(); e.Handled = true; return; }
            if (e.Key == Key.F3) { FindNextMatch(); e.Handled = true; }
        }

        /// <summary>
        /// Updates the Comments tab/expander title with a notification badge if comments are not blank.
        /// </summary>
        private void UpdateCommentsTabTitle()
        {
            string comments = _commentsEdit?.Text ?? "";

            if (_commentsExpander != null)
            {
                if (!string.IsNullOrWhiteSpace(comments))
                {
                    _commentsExpander.Header = "Comments *";
                }
                else
                {
                    _commentsExpander.Header = "Comments";
                }
            }
        }

        /// <summary>
        /// Gets the Comments Expander for testing.
        /// </summary>
        public Expander CommentsExpander => _commentsExpander;

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _undoStack.Clear();
            _redoStack.Clear();
            try
            {
                LoadFromBytes(data);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load UTC: {ex}");
                New();
            }
        }

        private void LoadUTC(UTC utc)
        {
            _utc = utc;

            // Basic
            if (_firstNameEdit != null)
            {
                _firstNameEdit.SetInstallation(_installation);
                _firstNameEdit.SetLocString(utc.FirstName);
            }
            if (_lastNameEdit != null)
            {
                _lastNameEdit.SetInstallation(_installation);
                _lastNameEdit.SetLocString(utc.LastName);
            }
            if (_tagEdit != null)
            {
                _tagEdit.Text = utc.Tag;
            }
            if (_resrefEdit != null)
            {
                _resrefEdit.Text = utc.ResRef.ToString();
            }
            if (_appearanceSelect != null)
            {
                _appearanceSelect.SelectedIndex = utc.AppearanceId;
            }
            if (_soundsetSelect != null)
            {
                _soundsetSelect.SelectedIndex = utc.SoundsetId;
            }
            if (_portraitSelect != null)
            {
                _portraitSelect.SelectedIndex = utc.PortraitId;
            }
            if (_alignmentSlider != null)
            {
                _alignmentSlider.Value = utc.Alignment;
            }
            if (_conversationEdit != null)
            {
                PopulateConversationComboBox();
                _conversationEdit.Text = utc.Conversation.ToString();
            }

            // Advanced
            if (_disarmableCheckbox != null) _disarmableCheckbox.IsChecked = utc.Disarmable;
            if (_noPermDeathCheckbox != null) _noPermDeathCheckbox.IsChecked = utc.NoPermDeath;
            if (_min1HpCheckbox != null) _min1HpCheckbox.IsChecked = utc.Min1Hp;
            if (_plotCheckbox != null) _plotCheckbox.IsChecked = utc.Plot;
            if (_isPcCheckbox != null) _isPcCheckbox.IsChecked = utc.IsPc;
            if (_noReorientateCheckbox != null) _noReorientateCheckbox.IsChecked = utc.NotReorienting;
            if (_noBlockCheckbox != null) _noBlockCheckbox.IsChecked = utc.IgnoreCrePath;
            if (_hologramCheckbox != null) _hologramCheckbox.IsChecked = utc.Hologram;
            if (_raceSelect != null) _raceSelect.SelectedIndex = utc.RaceId;
            if (_subraceSelect != null) _subraceSelect.SelectedIndex = utc.SubraceId;
            if (_speedSelect != null) _speedSelect.SelectedIndex = utc.WalkrateId;
            if (_factionSelect != null) _factionSelect.SelectedIndex = utc.FactionId;
            if (_genderSelect != null) _genderSelect.SelectedIndex = utc.GenderId;
            if (_perceptionSelect != null) _perceptionSelect.SelectedIndex = utc.PerceptionId;
            if (_challengeRatingSpin != null) _challengeRatingSpin.Value = (decimal?)utc.ChallengeRating;
            if (_blindSpotSpin != null) _blindSpotSpin.Value = (decimal?)utc.Blindspot;
            if (_multiplierSetSpin != null) _multiplierSetSpin.Value = utc.MultiplierSet;

            // Stats
            if (_strengthSpin != null) _strengthSpin.Value = utc.Strength;
            if (_dexteritySpin != null) _dexteritySpin.Value = utc.Dexterity;
            if (_constitutionSpin != null) _constitutionSpin.Value = utc.Constitution;
            if (_intelligenceSpin != null) _intelligenceSpin.Value = utc.Intelligence;
            if (_wisdomSpin != null) _wisdomSpin.Value = utc.Wisdom;
            if (_charismaSpin != null) _charismaSpin.Value = utc.Charisma;
            if (_computerUseSpin != null) _computerUseSpin.Value = utc.ComputerUse;
            if (_demolitionsSpin != null) _demolitionsSpin.Value = utc.Demolitions;
            if (_stealthSpin != null) _stealthSpin.Value = utc.Stealth;
            if (_awarenessSpin != null) _awarenessSpin.Value = utc.Awareness;
            if (_persuadeSpin != null) _persuadeSpin.Value = utc.Persuade;
            if (_repairSpin != null) _repairSpin.Value = utc.Repair;
            if (_securitySpin != null) _securitySpin.Value = utc.Security;
            if (_treatInjurySpin != null) _treatInjurySpin.Value = utc.TreatInjury;
            if (_fortitudeSpin != null) _fortitudeSpin.Value = utc.FortitudeBonus;
            if (_reflexSpin != null) _reflexSpin.Value = utc.ReflexBonus;
            if (_willSpin != null) _willSpin.Value = utc.WillpowerBonus;
            if (_armorClassSpin != null) _armorClassSpin.Value = utc.NaturalAc;
            if (_baseHpSpin != null) _baseHpSpin.Value = utc.Hp;
            if (_currentHpSpin != null) _currentHpSpin.Value = utc.CurrentHp;
            if (_maxHpSpin != null) _maxHpSpin.Value = utc.MaxHp;
            if (_currentFpSpin != null) _currentFpSpin.Value = utc.Fp;
            if (_maxFpSpin != null) _maxFpSpin.Value = utc.MaxFp;

            // Classes
            if (utc.Classes != null && utc.Classes.Count >= 1)
            {
                if (_class1Select != null) _class1Select.SelectedIndex = utc.Classes[0].ClassId;
                if (_class1LevelSpin != null) _class1LevelSpin.Value = utc.Classes[0].ClassLevel;
            }
            if (utc.Classes != null && utc.Classes.Count >= 2)
            {
                if (_class2Select != null) _class2Select.SelectedIndex = utc.Classes[1].ClassId + 1; // +1 for "[Unset]" option
                if (_class2LevelSpin != null) _class2LevelSpin.Value = utc.Classes[1].ClassLevel;
            }

            // Feats
            if (_featList != null && _installation != null)
            {
                _featList.Items.Clear();
                TwoDA feats = _installation.HtGetCache2DA(OdyInstallation.TwoDAFeats);
                if (feats != null)
                {
                    // First, uncheck all existing items
                    foreach (var existingItem in _featList.Items)
                    {
                        if (existingItem is CheckableListItem item)
                        {
                            item.IsChecked = false;
                        }
                    }

                    // Add all feats from 2DA
                    for (int i = 0; i < feats.GetHeight(); i++)
                    {
                        int featId = i;
                        int stringRef = feats.GetCellInt(i, "name", 0) ?? 0;
                        string text;
                        if (stringRef != 0 && _installation.TalkTable() != null)
                        {
                            text = _installation.TalkTable().GetString(stringRef);
                        }
                        else
                        {
                            text = feats.GetCellString(i, "label");
                        }
                        if (string.IsNullOrEmpty(text))
                        {
                            text = $"[Unused Feat ID: {featId}]";
                        }

                        var item = new CheckableListItem(text, featId);
                        _featList.Items.Add(item);
                    }

                    // Check feats that are in utc.Feats
                    if (utc.Feats != null)
                    {
                        foreach (int featId in utc.Feats)
                        {
                            var item = GetFeatItem(featId);
                            if (item == null)
                            {
                                // Modded feat not in 2DA - add it
                                item = new CheckableListItem($"[Modded Feat ID: {featId}]", featId);
                                _featList.Items.Add(item);
                            }
                            item.IsChecked = true;
                        }
                    }
                }
            }

            // Powers
            if (_powerList != null && _installation != null)
            {
                _powerList.Items.Clear();
                TwoDA powers = _installation.HtGetCache2DA(OdyInstallation.TwoDAPowers);
                if (powers != null)
                {
                    // First, uncheck all existing items
                    foreach (var existingItem in _powerList.Items)
                    {
                        if (existingItem is CheckableListItem item)
                        {
                            item.IsChecked = false;
                        }
                    }

                    // Add all powers from 2DA
                    for (int i = 0; i < powers.GetHeight(); i++)
                    {
                        int powerId = i;
                        int stringRef = powers.GetCellInt(i, "name", 0) ?? 0;
                        string text;
                        if (stringRef != 0 && _installation.TalkTable() != null)
                        {
                            text = _installation.TalkTable().GetString(stringRef);
                        }
                        else
                        {
                            text = powers.GetCellString(i, "label");
                        }
                        if (!string.IsNullOrEmpty(text))
                        {
                            text = text.Replace("_", " ").Replace("XXX", "").Replace("\n", "");
                            // Title case
                            if (text.Length > 0)
                            {
                                text = char.ToUpper(text[0]) + (text.Length > 1 ? text.Substring(1).ToLower() : "");
                            }
                        }
                        if (string.IsNullOrEmpty(text))
                        {
                            text = $"[Unused Power ID: {powerId}]";
                        }

                        var item = new CheckableListItem(text, powerId);
                        _powerList.Items.Add(item);
                    }

                    // Check powers that are in utc.Classes powers
                    if (utc.Classes != null)
                    {
                        foreach (var utcClass in utc.Classes)
                        {
                            if (utcClass.Powers != null)
                            {
                                foreach (int powerId in utcClass.Powers)
                                {
                                    var item = GetPowerItem(powerId);
                                    if (item == null)
                                    {
                                        // Modded power not in 2DA - add it
                                        item = new CheckableListItem($"[Modded Power ID: {powerId}]", powerId);
                                        _powerList.Items.Add(item);
                                    }
                                    item.IsChecked = true;
                                }
                            }
                        }
                    }
                }
            }

            // Scripts
            if (_scriptFields.ContainsKey("OnBlocked") && _scriptFields["OnBlocked"] != null)
                _scriptFields["OnBlocked"].Text = utc.OnBlocked.ToString();
            if (_scriptFields.ContainsKey("OnAttacked") && _scriptFields["OnAttacked"] != null)
                _scriptFields["OnAttacked"].Text = utc.OnAttacked.ToString();
            if (_scriptFields.ContainsKey("OnNotice") && _scriptFields["OnNotice"] != null)
                _scriptFields["OnNotice"].Text = utc.OnNotice.ToString();
            if (_scriptFields.ContainsKey("OnDialog") && _scriptFields["OnDialog"] != null)
                _scriptFields["OnDialog"].Text = utc.OnDialog.ToString();
            if (_scriptFields.ContainsKey("OnDamaged") && _scriptFields["OnDamaged"] != null)
                _scriptFields["OnDamaged"].Text = utc.OnDamaged.ToString();
            if (_scriptFields.ContainsKey("OnDisturbed") && _scriptFields["OnDisturbed"] != null)
                _scriptFields["OnDisturbed"].Text = utc.OnDisturbed.ToString();
            if (_scriptFields.ContainsKey("OnDeath") && _scriptFields["OnDeath"] != null)
                _scriptFields["OnDeath"].Text = utc.OnDeath.ToString();
            if (_scriptFields.ContainsKey("OnEndRound") && _scriptFields["OnEndRound"] != null)
                _scriptFields["OnEndRound"].Text = utc.OnEndRound.ToString();
            if (_scriptFields.ContainsKey("OnEndDialog") && _scriptFields["OnEndDialog"] != null)
                _scriptFields["OnEndDialog"].Text = utc.OnEndDialog.ToString();
            if (_scriptFields.ContainsKey("OnHeartbeat") && _scriptFields["OnHeartbeat"] != null)
                _scriptFields["OnHeartbeat"].Text = utc.OnHeartbeat.ToString();
            if (_scriptFields.ContainsKey("OnSpawn") && _scriptFields["OnSpawn"] != null)
                _scriptFields["OnSpawn"].Text = utc.OnSpawn.ToString();
            if (_scriptFields.ContainsKey("OnSpell") && _scriptFields["OnSpell"] != null)
                _scriptFields["OnSpell"].Text = utc.OnSpell.ToString();
            if (_scriptFields.ContainsKey("OnUserDefined") && _scriptFields["OnUserDefined"] != null)
                _scriptFields["OnUserDefined"].Text = utc.OnUserDefined.ToString();

            PopulateScriptComboBoxes();

            // Comments
            if (_commentsEdit != null)
            {
                _commentsEdit.Text = utc.Comment;
                UpdateCommentsTabTitle();
            }

            // Update portrait preview after loading all data
            PortraitChanged();
            RefreshCreaturePreview();
        }

        private void RefreshCreaturePreview()
        {
            if (_previewRenderer == null)
            {
                return;
            }

            _previewRenderer.Installation = _installation;

            if (_installation == null || _utc == null)
            {
                _previewRenderer.ClearModel();
                return;
            }

            try
            {
                UTC previewUtc = CopyUtc(_utc);
                if (_appearanceSelect != null && _appearanceSelect.SelectedIndex >= 0)
                {
                    previewUtc.AppearanceId = _appearanceSelect.SelectedIndex;
                }

                var modelTuple = BioWare.Tools.Creature.GetBodyModel(previewUtc, _installation.Installation);
                string bodyModel = modelTuple.Item1;
                if (string.IsNullOrWhiteSpace(bodyModel))
                {
                    _previewRenderer.ClearModel();
                    return;
                }

                var mdlResult = _installation.Resource(bodyModel, ResourceType.MDL, null);
                var mdxResult = _installation.Resource(bodyModel, ResourceType.MDX, null);
                if (mdlResult != null && mdlResult.Data != null && mdxResult != null && mdxResult.Data != null)
                {
                    _previewRenderer.SetModel(mdlResult.Data, mdxResult.Data);
                }
                else
                {
                    _previewRenderer.ClearModel();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Failed to refresh UTC model preview: " + ex.Message);
                _previewRenderer.ClearModel();
            }
        }

        public override Tuple<byte[], byte[]> Build()
        {
            // Matching Python: utc: UTC = deepcopy(self._utc)
            var utc = CopyUtc(_utc);

            // Basic - read from UI controls (matching Python which always reads from UI)
            // Python: utc.first_name = self.ui.firstnameEdit.locstring()
            // In C#, firstNameEdit/lastNameEdit are LocalizedStringEdit widgets that store the LocalizedString
            utc.FirstName = _firstNameEdit?.GetLocString() ?? utc.FirstName ?? LocalizedString.FromInvalid();
            utc.LastName = _lastNameEdit?.GetLocString() ?? utc.LastName ?? LocalizedString.FromInvalid();
            utc.Tag = _tagEdit?.Text ?? "";
            utc.ResRef = new ResRef(_resrefEdit?.Text ?? "");
            utc.AppearanceId = _appearanceSelect?.SelectedIndex ?? 0;
            utc.SoundsetId = _soundsetSelect?.SelectedIndex ?? 0;
            utc.Conversation = new ResRef(_conversationEdit?.Text ?? "");
            utc.PortraitId = _portraitSelect?.SelectedIndex ?? 0;
            utc.Alignment = (int)(_alignmentSlider?.Value ?? 50);

            // Advanced - read from UI controls
            utc.Disarmable = _disarmableCheckbox?.IsChecked == true;
            utc.NoPermDeath = _noPermDeathCheckbox?.IsChecked == true;
            utc.Min1Hp = _min1HpCheckbox?.IsChecked == true;
            utc.Plot = _plotCheckbox?.IsChecked == true;
            utc.IsPc = _isPcCheckbox?.IsChecked == true;
            utc.NotReorienting = _noReorientateCheckbox?.IsChecked == true;
            utc.IgnoreCrePath = _noBlockCheckbox?.IsChecked == true;
            utc.Hologram = _hologramCheckbox?.IsChecked == true;
            utc.RaceId = _raceSelect?.SelectedIndex ?? 0;
            utc.SubraceId = _subraceSelect?.SelectedIndex ?? 0;
            utc.WalkrateId = _speedSelect?.SelectedIndex ?? 0;
            utc.FactionId = _factionSelect?.SelectedIndex ?? 0;
            utc.GenderId = _genderSelect?.SelectedIndex ?? 0;
            utc.PerceptionId = _perceptionSelect?.SelectedIndex ?? 0;
            utc.ChallengeRating = (float)(_challengeRatingSpin?.Value ?? 0);
            utc.Blindspot = (float)(_blindSpotSpin?.Value ?? 0);
            utc.MultiplierSet = (int)(_multiplierSetSpin?.Value ?? 0);

            // Stats - read from UI controls
            utc.Strength = (int)(_strengthSpin?.Value ?? 0);
            utc.Dexterity = (int)(_dexteritySpin?.Value ?? 0);
            utc.Constitution = (int)(_constitutionSpin?.Value ?? 0);
            utc.Intelligence = (int)(_intelligenceSpin?.Value ?? 0);
            utc.Wisdom = (int)(_wisdomSpin?.Value ?? 0);
            utc.Charisma = (int)(_charismaSpin?.Value ?? 0);
            utc.ComputerUse = (int)(_computerUseSpin?.Value ?? 0);
            utc.Demolitions = (int)(_demolitionsSpin?.Value ?? 0);
            utc.Stealth = (int)(_stealthSpin?.Value ?? 0);
            utc.Awareness = (int)(_awarenessSpin?.Value ?? 0);
            utc.Persuade = (int)(_persuadeSpin?.Value ?? 0);
            utc.Repair = (int)(_repairSpin?.Value ?? 0);
            utc.Security = (int)(_securitySpin?.Value ?? 0);
            utc.TreatInjury = (int)(_treatInjurySpin?.Value ?? 0);
            utc.FortitudeBonus = (int)(_fortitudeSpin?.Value ?? 0);
            utc.ReflexBonus = (int)(_reflexSpin?.Value ?? 0);
            utc.WillpowerBonus = (int)(_willSpin?.Value ?? 0);
            utc.NaturalAc = (int)(_armorClassSpin?.Value ?? 0);
            utc.Hp = (int)(_baseHpSpin?.Value ?? 0);
            utc.CurrentHp = (int)(_currentHpSpin?.Value ?? 0);
            utc.MaxHp = (int)(_maxHpSpin?.Value ?? 0);
            utc.Fp = (int)(_currentFpSpin?.Value ?? 0);
            utc.MaxFp = (int)(_maxFpSpin?.Value ?? 0);

            // Classes - read from UI controls
            utc.Classes.Clear();
            if (_class1Select?.SelectedIndex >= 0)
            {
                int classId = _class1Select.SelectedIndex;
                int classLevel = (int)(_class1LevelSpin?.Value ?? 0);
                utc.Classes.Add(new UTCClass(classId, classLevel));
            }
            if (_class2Select?.SelectedIndex > 0) // > 0 because 0 is "[Unset]"
            {
                int classId = _class2Select.SelectedIndex - 1;
                int classLevel = (int)(_class2LevelSpin?.Value ?? 0);
                utc.Classes.Add(new UTCClass(classId, classLevel));
            }

            // Feats - read from checked items in _featList
            utc.Feats.Clear();
            if (_featList != null)
            {
                foreach (var item in _featList.Items)
                {
                    if (item is CheckableListItem checkableItem && checkableItem.IsChecked)
                    {
                        utc.Feats.Add(checkableItem.Id);
                    }
                }
            }

            // Powers - read from checked items in _powerList and add to last class
            if (utc.Classes.Count > 0)
            {
                var lastClass = utc.Classes[utc.Classes.Count - 1];
                if (lastClass.Powers == null)
                {
                    lastClass.Powers = new List<int>();
                }
                else
                {
                    lastClass.Powers.Clear();
                }

                if (_powerList != null)
                {
                    foreach (var item in _powerList.Items)
                    {
                        if (item is CheckableListItem checkableItem && checkableItem.IsChecked)
                        {
                            lastClass.Powers.Add(checkableItem.Id);
                        }
                    }
                }
            }

            // Scripts - read from UI controls
            if (_scriptFields.ContainsKey("OnBlocked") && _scriptFields["OnBlocked"] != null)
                utc.OnBlocked = new ResRef(_scriptFields["OnBlocked"].Text);
            if (_scriptFields.ContainsKey("OnAttacked") && _scriptFields["OnAttacked"] != null)
                utc.OnAttacked = new ResRef(_scriptFields["OnAttacked"].Text);
            if (_scriptFields.ContainsKey("OnNotice") && _scriptFields["OnNotice"] != null)
                utc.OnNotice = new ResRef(_scriptFields["OnNotice"].Text);
            if (_scriptFields.ContainsKey("OnDialog") && _scriptFields["OnDialog"] != null)
                utc.OnDialog = new ResRef(_scriptFields["OnDialog"].Text);
            if (_scriptFields.ContainsKey("OnDamaged") && _scriptFields["OnDamaged"] != null)
                utc.OnDamaged = new ResRef(_scriptFields["OnDamaged"].Text);
            if (_scriptFields.ContainsKey("OnDisturbed") && _scriptFields["OnDisturbed"] != null)
                utc.OnDisturbed = new ResRef(_scriptFields["OnDisturbed"].Text);
            if (_scriptFields.ContainsKey("OnDeath") && _scriptFields["OnDeath"] != null)
                utc.OnDeath = new ResRef(_scriptFields["OnDeath"].Text);
            if (_scriptFields.ContainsKey("OnEndRound") && _scriptFields["OnEndRound"] != null)
                utc.OnEndRound = new ResRef(_scriptFields["OnEndRound"].Text);
            if (_scriptFields.ContainsKey("OnEndDialog") && _scriptFields["OnEndDialog"] != null)
                utc.OnEndDialog = new ResRef(_scriptFields["OnEndDialog"].Text);
            if (_scriptFields.ContainsKey("OnHeartbeat") && _scriptFields["OnHeartbeat"] != null)
                utc.OnHeartbeat = new ResRef(_scriptFields["OnHeartbeat"].Text);
            if (_scriptFields.ContainsKey("OnSpawn") && _scriptFields["OnSpawn"] != null)
                utc.OnSpawn = new ResRef(_scriptFields["OnSpawn"].Text);
            if (_scriptFields.ContainsKey("OnSpell") && _scriptFields["OnSpell"] != null)
                utc.OnSpell = new ResRef(_scriptFields["OnSpell"].Text);
            if (_scriptFields.ContainsKey("OnUserDefined") && _scriptFields["OnUserDefined"] != null)
                utc.OnUserDefined = new ResRef(_scriptFields["OnUserDefined"].Text);

            // Comments - read from UI controls
            utc.Comment = _commentsEdit?.Text ?? "";

            // Matching Python: gff: GFF = dismantle_utc(utc); write_gff(gff, data)
            Game game = _installation?.Game ?? Game.K2;
            var gff = BioWare.Resource.Formats.GFF.Generics.UTC.UTCHelpers.DismantleUtc(utc, game);
            byte[] data = GFFAuto.BytesGff(gff, ResourceType.UTC);
            return Tuple.Create(data, new byte[0]);
        }

        // Matching Python: deepcopy(self._utc)
        private static UTC CopyUtc(UTC source)
        {
            // Use Dismantle/Construct pattern for reliable deep copy (matching Python deepcopy behavior)
            Game game = Game.K2; // Default game for serialization
            var gff = BioWare.Resource.Formats.GFF.Generics.UTC.UTCHelpers.DismantleUtc(source, game);
            return BioWare.Resource.Formats.GFF.Generics.UTC.UTCHelpers.ConstructUtc(gff);
        }

        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            _utc = new UTC();
            LoadUTC(_utc);
            UpdateItemCount();
            UpdateStatusBar();
        }

        private void RandomizeFirstName()
        {
            if (_installation == null)
            {
                System.Console.WriteLine("Cannot randomize first name: installation is not set");
                return;
            }

            // Determine LTR file based on gender: "humanf" if gender is 1 (female), "humanm" if male (0)
            // Matching Python: ltr_resname: Literal["humanf", "humanm"] = "humanf" if self.ui.genderSelect.currentIndex() == 1 else "humanm"
            int genderIndex = _genderSelect?.SelectedIndex ?? 0;
            string ltrResname = (genderIndex == 1) ? "humanf" : "humanm";

            try
            {
                // Load LTR resource from installation
                // Matching Python: ltr: LTR = read_ltr(self._installation.resource(ltr_resname, ResourceType.LTR).data)
                var resourceResult = _installation.Resource(ltrResname, ResourceType.LTR, null);
                if (resourceResult == null || resourceResult.Data == null || resourceResult.Data.Length == 0)
                {
                    System.Console.WriteLine($"Cannot randomize first name: LTR resource '{ltrResname}' not found");
                    return;
                }

                // Read LTR file
                LTR ltr = LTRAuto.ReadLtr(resourceResult.Data);

                // Generate random name
                // Matching Python: ltr.generate()
                string generatedName = ltr.Generate();

                // Update LocalizedString
                // Matching Python: locstring: LocalizedString = self.ui.firstnameEdit.locstring()
                // Matching Python: locstring.stringref = -1
                // Matching Python: locstring.set_data(Language.ENGLISH, Gender.MALE, ltr.generate())
                if (_utc.FirstName == null)
                {
                    _utc.FirstName = LocalizedString.FromInvalid();
                }
                _utc.FirstName.StringRef = -1;
                _utc.FirstName.SetData(Language.English, Gender.Male, generatedName);

                // Update UI display
                // Matching Python: self.ui.firstnameEdit.set_locstring(locstring)
                if (_firstNameEdit != null)
                {
                    _firstNameEdit.SetLocString(_utc.FirstName);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error randomizing first name: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        private void RandomizeLastName()
        {
            if (_installation == null)
            {
                System.Console.WriteLine("Cannot randomize last name: installation is not set");
                return;
            }

            // Always use "humanl" for last names
            // Matching Python: ltr: LTR = read_ltr(self._installation.resource("humanl", ResourceType.LTR).data)
            string ltrResname = "humanl";

            try
            {
                // Load LTR resource from installation
                var resourceResult = _installation.Resource(ltrResname, ResourceType.LTR, null);
                if (resourceResult == null || resourceResult.Data == null || resourceResult.Data.Length == 0)
                {
                    System.Console.WriteLine($"Cannot randomize last name: LTR resource '{ltrResname}' not found");
                    return;
                }

                // Read LTR file
                LTR ltr = LTRAuto.ReadLtr(resourceResult.Data);

                // Generate random name
                // Matching Python: ltr.generate()
                string generatedName = ltr.Generate();

                // Update LocalizedString
                // Matching Python: locstring: LocalizedString = self.ui.lastnameEdit.locstring()
                // Matching Python: locstring.stringref = -1
                // Matching Python: locstring.set_data(Language.ENGLISH, Gender.MALE, ltr.generate())
                if (_utc.LastName == null)
                {
                    _utc.LastName = LocalizedString.FromInvalid();
                }
                _utc.LastName.StringRef = -1;
                _utc.LastName.SetData(Language.English, Gender.Male, generatedName);

                // Update UI display
                // Matching Python: self.ui.lastnameEdit.set_locstring(locstring)
                if (_lastNameEdit != null)
                {
                    _lastNameEdit.SetLocString(_utc.LastName);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error randomizing last name: {ex.Message}");
                if (ex.InnerException != null)
                {
                    System.Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        private void GenerateTag()
        {
            if (_tagEdit != null && _resrefEdit != null)
            {
                _tagEdit.Text = _resrefEdit.Text;
            }
        }

        private void PortraitChanged()
        {
            if (_portraitPicture == null)
            {
                return;
            }

            int index = _portraitSelect?.SelectedIndex ?? 0;

            // Matching Python: if index == 0, create blank image
            if (index == 0)
            {
                // Create blank 64x64 RGB image (black)
                var blankBitmap = new WriteableBitmap(
                    new PixelSize(64, 64),
                    new Vector(96, 96),
                    PixelFormat.Rgba8888,
                    AlphaFormat.Premul);
                using (var lockedBitmap = blankBitmap.Lock())
                {
                    // Fill with black (zeros)
                    System.Runtime.InteropServices.Marshal.Copy(
                        new byte[64 * 64 * 4], 0, lockedBitmap.Address, 64 * 64 * 4);
                }
                _portraitPicture.Source = blankBitmap;
                ToolTip.SetTip(_portraitPicture, GeneratePortraitTooltip());
                return;
            }

            // Build pixmap from index
            var bitmap = BuildPortraitBitmap(index);
            if (bitmap != null)
            {
                _portraitPicture.Source = bitmap;
            }
            else
            {
                // Fallback to blank image if build failed
                var blankBitmap = new WriteableBitmap(
                    new PixelSize(64, 64),
                    new Vector(96, 96),
                    PixelFormat.Rgba8888,
                    AlphaFormat.Premul);
                using (var lockedBitmap = blankBitmap.Lock())
                {
                    System.Runtime.InteropServices.Marshal.Copy(
                        new byte[64 * 64 * 4], 0, lockedBitmap.Address, 64 * 64 * 4);
                }
                _portraitPicture.Source = blankBitmap;
            }

            // Set tooltip
            ToolTip.SetTip(_portraitPicture, GeneratePortraitTooltip());
        }

        /// <summary>
        /// Builds a portrait bitmap based on character alignment.
        ///
        /// Builds the portrait bitmap by:
        ///     1. Getting the character's alignment value
        ///     2. Looking up the character's portrait reference in the portraits 2DA based on alignment
        ///     3. Loading the texture for the portrait reference
        ///     4. Converting the texture to a Bitmap.
        /// </summary>
        /// <param name="index">The character index to build a portrait for.</param>
        /// <returns>A Bitmap of the character portrait, or null if loading fails.</returns>
        private Bitmap BuildPortraitBitmap(int index)
        {
            if (_installation == null)
            {
                return null;
            }

            // Get alignment value
            int alignment = (int)(_alignmentSlider?.Value ?? 50);

            // Get portraits 2DA
            TwoDA portraits = _installation.HtGetCache2DA(OdyInstallation.TwoDAPortraits);
            if (portraits == null)
            {
                System.Console.WriteLine("Cannot build portrait: portraits.2da not found");
                return null;
            }

            // Get base portrait resref
            string portrait = portraits.GetCellString(index, "baseresref");
            if (string.IsNullOrEmpty(portrait))
            {
                System.Console.WriteLine($"Cannot build portrait: baseresref not found for index {index}");
                return null;
            }

            // Check alignment-based variants (matching Python logic)
            // Python: if 40 >= alignment > 30 and portraits.get_cell(index, "baseresrefe"):
            if (alignment <= 40 && alignment > 30)
            {
                string variant = portraits.GetCellString(index, "baseresrefe");
                if (!string.IsNullOrEmpty(variant))
                {
                    portrait = variant;
                }
            }
            // Python: elif 30 >= alignment > 20 and portraits.get_cell(index, "baseresrefve"):
            else if (alignment <= 30 && alignment > 20)
            {
                string variant = portraits.GetCellString(index, "baseresrefve");
                if (!string.IsNullOrEmpty(variant))
                {
                    portrait = variant;
                }
            }
            // Python: elif 20 >= alignment > 10 and portraits.get_cell(index, "baseresrefvve"):
            else if (alignment <= 20 && alignment > 10)
            {
                string variant = portraits.GetCellString(index, "baseresrefvve");
                if (!string.IsNullOrEmpty(variant))
                {
                    portrait = variant;
                }
            }
            // Python: elif alignment <= 10 and portraits.get_cell(index, "baseresrefvvve"):
            else if (alignment <= 10)
            {
                string variant = portraits.GetCellString(index, "baseresrefvvve");
                if (!string.IsNullOrEmpty(variant))
                {
                    portrait = variant;
                }
            }

            // Load texture from installation
            // Matching Python: texture: TPC | None = self._installation.texture(portrait, [SearchLocation.TEXTURES_GUI])
            TPC texture = _installation.Texture(portrait, new[] { SearchLocation.TEXTURES_GUI });
            if (texture == null)
            {
                System.Console.WriteLine($"Cannot build portrait: texture '{portrait}' not found");
                // Return blank image on failure (matching Python behavior)
                var blankBitmap = new WriteableBitmap(
                    new PixelSize(64, 64),
                    new Vector(96, 96),
                    PixelFormat.Rgba8888,
                    AlphaFormat.Premul);
                using (var lockedBitmap = blankBitmap.Lock())
                {
                    System.Runtime.InteropServices.Marshal.Copy(
                        new byte[64 * 64 * 4], 0, lockedBitmap.Address, 64 * 64 * 4);
                }
                return blankBitmap;
            }

            // Get first mipmap from first layer
            // Note: DXT decompression is handled automatically by ConvertTpcMipmapToAvaloniaBitmap
            // Matching Python: mipmap: TPCMipmap = texture.get(0, 0)
            if (texture.Layers == null || texture.Layers.Count == 0 ||
                texture.Layers[0].Mipmaps == null || texture.Layers[0].Mipmaps.Count == 0)
            {
                System.Console.WriteLine($"Cannot build portrait: texture '{portrait}' has no mipmaps");
                var blankBitmap = new WriteableBitmap(
                    new PixelSize(64, 64),
                    new Vector(96, 96),
                    PixelFormat.Rgba8888,
                    AlphaFormat.Premul);
                using (var lockedBitmap = blankBitmap.Lock())
                {
                    System.Runtime.InteropServices.Marshal.Copy(
                        new byte[64 * 64 * 4], 0, lockedBitmap.Address, 64 * 64 * 4);
                }
                return blankBitmap;
            }

            TPCMipmap mipmap = texture.Layers[0].Mipmaps[0];

            // Convert TPC mipmap to Avalonia Bitmap
            // Matching Python: image = QImage(bytes(mipmap.data), mipmap.width, mipmap.height, texture.format().to_qimage_format())
            // Matching Python: return QPixmap.fromImage(image).transformed(QTransform().scale(1, -1))
            // Note: Python flips vertically with scale(1, -1), but Avalonia handles this differently
            var bitmap = OdyInstallation.ConvertTpcMipmapToAvaloniaBitmap(mipmap);
            if (bitmap == null)
            {
                System.Console.WriteLine($"Cannot build portrait: failed to convert texture '{portrait}' to bitmap");
                var blankBitmap = new WriteableBitmap(
                    new PixelSize(64, 64),
                    new Vector(96, 96),
                    PixelFormat.Rgba8888,
                    AlphaFormat.Premul);
                using (var lockedBitmap = blankBitmap.Lock())
                {
                    System.Runtime.InteropServices.Marshal.Copy(
                        new byte[64 * 64 * 4], 0, lockedBitmap.Address, 64 * 64 * 4);
                }
                return blankBitmap;
            }

            return bitmap;
        }

        /// <summary>
        /// Generates a detailed tooltip for the portrait picture.
        /// </summary>
        /// <returns>The tooltip text.</returns>
        private string GeneratePortraitTooltip()
        {
            string portrait = GetPortraitResref();
            // Matching Python: tooltip = f"<b>Portrait:</b> {portrait}<br>" "<br><i>Right-click for more options.</i>"
            // For Avalonia, we use plain text (HTML not supported in standard tooltips)
            return $"Portrait: {portrait}\n\nRight-click for more options.";
        }

        /// <summary>
        /// Gets the portrait resref based on the selected index and alignment.
        /// </summary>
        /// <returns>The portrait resref string.</returns>
        private string GetPortraitResref()
        {
            if (_installation == null)
            {
                return "Unknown";
            }

            int index = _portraitSelect?.SelectedIndex ?? 0;
            int alignment = (int)(_alignmentSlider?.Value ?? 50);

            TwoDA portraits = _installation.HtGetCache2DA(OdyInstallation.TwoDAPortraits);
            if (portraits == null)
            {
                return "Unknown";
            }

            string result = portraits.GetCellString(index, "baseresref");
            if (string.IsNullOrEmpty(result))
            {
                return "Unknown";
            }

            // Check alignment-based variants (matching Python logic)
            if (alignment <= 40 && alignment > 30)
            {
                string variant = portraits.GetCellString(index, "baseresrefe");
                if (!string.IsNullOrEmpty(variant))
                {
                    result = variant;
                }
            }
            else if (alignment <= 30 && alignment > 20)
            {
                string variant = portraits.GetCellString(index, "baseresrefve");
                if (!string.IsNullOrEmpty(variant))
                {
                    result = variant;
                }
            }
            else if (alignment <= 20 && alignment > 10)
            {
                string variant = portraits.GetCellString(index, "baseresrefvve");
                if (!string.IsNullOrEmpty(variant))
                {
                    result = variant;
                }
            }
            else if (alignment <= 10)
            {
                string variant = portraits.GetCellString(index, "baseresrefvvve");
                if (!string.IsNullOrEmpty(variant))
                {
                    result = variant;
                }
            }

            return result;
        }

        private void EditConversation()
        {
            if (_installation == null)
            {
                System.Console.WriteLine("Installation is not set");
                return;
            }

            string resname = (_conversationEdit?.Text ?? "").Trim();
            if (string.IsNullOrEmpty(resname))
            {
                _ = DialogHelper.ShowAsync("Invalid Dialog Reference", "Conversation field cannot be blank.", ButtonEnum.Ok, IconType.Error);
                return;
            }

            // Search for the DLG resource
            ResourceResult search = _installation.Resource(resname, ResourceType.DLG);
            string filepath = null;
            byte[] data = null;

            if (search == null)
            {
                // DLG not found - ask to create new
                var result = DialogHelper.ShowAsync("DLG file not found", "Do you wish to create a new dialog in the 'Override' folder?", ButtonEnum.YesNo, IconType.Question).GetAwaiter().GetResult();

                if (result == ButtonResult.Yes)
                {
                    // Create blank DLG file in override folder
                    string overridePath = _installation.OverridePath();
                    if (!string.IsNullOrEmpty(overridePath))
                    {
                        filepath = System.IO.Path.Combine(overridePath, $"{resname}.dlg");
                        Game game = _installation.Game;
                        var blankDlg = new BioWare.Resource.Formats.GFF.Generics.DLG.DLG();
                        var gff = BioWare.Resource.Formats.GFF.Generics.DLG.DLGHelper.DismantleDlg(blankDlg, game);
                        data = GFFAuto.BytesGff(gff, ResourceType.DLG);
                        System.IO.File.WriteAllBytes(filepath, data);
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                filepath = search.FilePath;
                if (search.Data != null)
                {
                    data = search.Data;
                }
                else if (!string.IsNullOrEmpty(filepath) && System.IO.File.Exists(filepath))
                {
                    data = System.IO.File.ReadAllBytes(filepath);
                }
            }

            if (data == null || string.IsNullOrEmpty(filepath))
            {
                System.Console.WriteLine($"Data/filepath cannot be null in EditConversation() (resname={resname}, filepath={filepath})");
                return;
            }

            // Open DLG editor
            OdyTools.Editors.WindowUtils.OpenResourceEditor(
                filepath,
                resname,
                ResourceType.DLG,
                data,
                _installation,
                this);
        }

        private void OpenInventory()
        {
            if (_installation == null || _utc == null)
            {
                System.Console.WriteLine("Installation or UTC is not set");
                return;
            }

            // Determine if droid (race ID 0 = Droid)
            bool droid = _raceSelect?.SelectedIndex == 0;

            // Load capsules to search
            List<Capsule> capsulesToSearch = new List<Capsule>();

            if (_filepath != null)
            {
                if (BioWare.Tools.FileHelpers.IsSavFile(_filepath))
                {
                    // Search capsules inside the .sav outer capsule
                    try
                    {
                        var outerCapsule = new Capsule(_filepath);
                        foreach (var res in outerCapsule)
                        {
                            // Check if the resource name (resname + extension) is a capsule file
                            string resourceFilename = $"{res.ResName}.{res.ResType.Extension}";
                            if (BioWare.Tools.FileHelpers.IsCapsuleFile(resourceFilename))
                            {
                                // The resource is inside a capsule (since we're iterating through a capsule)
                                // Construct the nested capsule path: outerCapsulePath/resourceFilename
                                string nestedCapsulePath = System.IO.Path.Combine(_filepath, resourceFilename);
                                try
                                {
                                    capsulesToSearch.Add(new Capsule(nestedCapsulePath));
                                }
                                catch
                                {
                                    // Skip invalid capsules
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Failed to load outer capsule
                    }
                }
                else if (BioWare.Tools.FileHelpers.IsCapsuleFile(_filepath))
                {
                    // Get capsules matching the module
                    // This finds all capsules in the module that match the current file's module
                    try
                    {
                        string root = null;
                        if (!string.IsNullOrEmpty(_filepath))
                        {
                            // Extract root from filepath (similar to Module.filepath_to_root)
                            string filename = System.IO.Path.GetFileName(_filepath);
                            if (filename.Contains("_"))
                            {
                                root = filename.Substring(0, filename.IndexOf('_'));
                            }
                            else if (filename.Contains("."))
                            {
                                root = filename.Substring(0, filename.IndexOf('.'));
                            }
                        }

                        if (root != null)
                        {
                            string caseRoot = root.ToLowerInvariant();
                            var moduleNames = _installation.ModuleNames();
                            string filepathFilename = System.IO.Path.GetFileName(_filepath) ?? "";

                            foreach (var kvp in moduleNames)
                            {
                                string moduleFilename = kvp.Key;
                                string moduleFilenameLower = moduleFilename.ToLowerInvariant();

                                // Check if root is contained in module filename and it's not the same as the current filepath
                                if (moduleFilenameLower.Contains(caseRoot) && moduleFilename != filepathFilename)
                                {
                                    string fullModulePath = System.IO.Path.Combine(_installation.ModulePath(), moduleFilename);
                                    if (System.IO.File.Exists(fullModulePath))
                                    {
                                        try
                                        {
                                            var capsule = new Capsule(fullModulePath, createIfNotExist: false);
                                            capsulesToSearch.Add(capsule);
                                        }
                                        catch
                                        {
                                            // Skip invalid capsule files
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Failed to get module capsules - continue with empty list
                    }
                }
            }

            // Create inventory dialog
            var inventoryDialog = new InventoryDialog(
                this,
                _installation,
                capsulesToSearch,
                new List<string>(), // folders - not used in UTC editor
                _utc.Inventory ?? new List<InventoryItem>(),
                _utc.Equipment ?? new Dictionary<EquipmentSlot, InventoryItem>(),
                droid: droid);

            // Show dialog and update if OK was clicked
            bool result = inventoryDialog.ShowDialog();
            if (result)
            {
                _utc.Inventory = inventoryDialog.Inventory;
                _utc.Equipment = inventoryDialog.Equipment;
                UpdateItemCount();
                // Note: update3dPreview() would be called here if 3D preview is implemented
            }
        }

        private void UpdateItemCount()
        {
            if (_inventoryCountLabel != null && _utc != null)
            {
                int count = _utc.Inventory != null ? _utc.Inventory.Count : 0;
                _inventoryCountLabel.Text = $"Total Items: {count}";
            }
        }

        private CheckableListItem GetFeatItem(int featId)
        {
            if (_featList == null)
            {
                return null;
            }

            foreach (var item in _featList.Items)
            {
                if (item is CheckableListItem checkableItem && checkableItem.Id == featId)
                {
                    return checkableItem;
                }
            }
            return null;
        }

        private CheckableListItem GetPowerItem(int powerId)
        {
            if (_powerList == null)
            {
                return null;
            }

            foreach (var item in _powerList.Items)
            {
                if (item is CheckableListItem checkableItem && checkableItem.Id == powerId)
                {
                    return checkableItem;
                }
            }
            return null;
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        // Expose Settings for testing (matching Python implementation)
        public OdyToolUTCSettings Settings => _settings;

        // Expose GlobalSettings for testing (matching Python implementation)
        public GlobalSettings GlobalSettings => _globalSettings;
    }

    // Helper class for checkable list items in Avalonia ListBox
    public class CheckableListItem : ContentControl
    {
        private CheckBox _checkBox;
        private TextBlock _textBlock;
        private int _id;
        private bool _isChecked;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    if (_checkBox != null)
                    {
                        _checkBox.IsChecked = value;
                    }
                }
            }
        }

        public string Text
        {
            get { return _textBlock?.Text ?? ""; }
            set
            {
                if (_textBlock != null)
                {
                    _textBlock.Text = value ?? "";
                }
            }
        }

        public CheckableListItem(string text, int id)
        {
            _id = id;
            _isChecked = false;

            var panel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(2)
            };

            _checkBox = new CheckBox
            {
                IsChecked = false,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };
            _checkBox.Checked += (s, e) => { _isChecked = true; };
            _checkBox.Unchecked += (s, e) => { _isChecked = false; };

            _textBlock = new TextBlock
            {
                Text = text ?? "",
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                Margin = new Thickness(5, 0, 0, 0)
            };

            panel.Children.Add(_checkBox);
            panel.Children.Add(_textBlock);
            Content = panel;
        }
    }
}
