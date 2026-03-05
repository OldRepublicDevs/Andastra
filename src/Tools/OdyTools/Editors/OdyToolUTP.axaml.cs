using BioWare.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
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
using DLGType = BioWare.Resource.Formats.GFF.Generics.DLG.DLG;
using DLGHelper = BioWare.Resource.Formats.GFF.Generics.DLG.DLGHelper;
using BioWare.Extract.Capsule;
using Avalonia.Controls.Primitives;
using OdyTools.Common;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Utils;
using OdyTools.Widgets;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using GFFAuto = BioWare.Resource.Formats.GFF.GFFAuto;
using Window = Avalonia.Controls.Window;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Editors
{
    public partial class OdyToolUTP : Editor
    {
        private const int MinEditorWidth = 700;
        private const int MinEditorHeight = 500;
        private const int UndoMaxLevels = 30;

        private UTP _utp;

        private Avalonia.Controls.TextBlock _statusText;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;
        private string _findText = "";
        private bool _findMatchCase;
        private bool _appearancePreviewHooked;

        // UI Controls - Basic
        private TextBox _nameEdit;
        private Button _nameEditBtn;
        private TextBox _tagEdit;
        private Button _tagGenerateBtn;
        private TextBox _resrefEdit;
        private Button _resrefGenerateBtn;
        private ComboBox _appearanceSelect;
        private ModelRenderer _previewRenderer;
        private ComboBox _conversationEdit;
        private Button _conversationModifyBtn;
        private Button _inventoryBtn;
        private TextBlock _inventoryCountLabel;

        // UI Controls - Advanced
        private CheckBox _hasInventoryCheckbox;
        private CheckBox _partyInteractCheckbox;
        private CheckBox _useableCheckbox;
        private CheckBox _min1HpCheckbox;
        private CheckBox _plotCheckbox;
        private CheckBox _staticCheckbox;
        private CheckBox _notBlastableCheckbox;
        private ComboBox _factionSelect;
        private NumericUpDown _animationStateSpin;
        private NumericUpDown _currentHpSpin;
        private NumericUpDown _maxHpSpin;
        private NumericUpDown _hardnessSpin;
        private NumericUpDown _fortitudeSpin;
        private NumericUpDown _reflexSpin;
        private NumericUpDown _willSpin;

        // UI Controls - Lock
        private CheckBox _needKeyCheckbox;
        private CheckBox _removeKeyCheckbox;
        private TextBox _keyEdit;
        private CheckBox _lockedCheckbox;
        private NumericUpDown _openLockSpin;
        private NumericUpDown _difficultySpin;
        private NumericUpDown _difficultyModSpin;

        // UI Controls - Scripts (editable combos with prefilled script resnames, matching vendor utp.py FilterComboBox)
        private Dictionary<string, ComboBox> _scriptFields;

        // UI Controls - Comments
        private TextBox _commentsEdit;

        public TextBox TagEdit => _tagEdit;
        public Button TagGenerateBtn => _tagGenerateBtn;
        public TextBox ResrefEdit => _resrefEdit;
        public Button ResrefGenerateBtn => _resrefGenerateBtn;

        public OdyToolUTP() : this(null, null) { }
        public OdyToolUTP(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolUTP", "placeable",
                new[] { ResourceType.UTP, ResourceType.BTP },
                new[] { ResourceType.UTP, ResourceType.BTP },
                installation)
        {
            _installation = installation;
            _utp = new UTP();
            _scriptFields = new Dictionary<string, ComboBox>();

            InitializeComponent();
            SetupUI();
            SetupMenuHandlers();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            Opened += (s, e) => { UpdateStatusBar(); _tagEdit?.Focus(); };
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
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;
                _nameEdit = EditorHelpers.FindControlSafe<TextBox>(this, "nameEdit");
                _tagEdit = EditorHelpers.FindControlSafe<TextBox>(this, "tagEdit");
                _resrefEdit = EditorHelpers.FindControlSafe<TextBox>(this, "resrefEdit");
                _commentsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "commentsEdit");
                if (_nameEdit == null || _tagEdit == null || _resrefEdit == null || _commentsEdit == null)
                    xamlLoaded = false;
            }
            catch { xamlLoaded = false; }

            if (!xamlLoaded)
            {
                SetupProgrammaticUI();
            }
            else
            {
                SetupUIFromXaml();
            }
        }

        private void SetupUIFromXaml()
        {
            _tagGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "tagGenerateBtn");
            _resrefGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "resrefGenerateBtn");
            _nameEditBtn = EditorHelpers.FindControlSafe<Button>(this, "nameEditBtn");
            _appearanceSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "appearanceSelect");
            _conversationEdit = EditorHelpers.FindControlSafe<ComboBox>(this, "conversationEdit");
            _conversationModifyBtn = EditorHelpers.FindControlSafe<Button>(this, "conversationModifyBtn");
            _inventoryBtn = EditorHelpers.FindControlSafe<Button>(this, "inventoryBtn");
            _inventoryCountLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "inventoryCountLabel");
            _hasInventoryCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "hasInventoryCheckbox");
            _partyInteractCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "partyInteractCheckbox");
            _useableCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "useableCheckbox");
            _min1HpCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "min1HpCheckbox");
            _plotCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "plotCheckbox");
            _staticCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "staticCheckbox");
            _notBlastableCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "notBlastableCheckbox");
            _factionSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "factionSelect");
            _animationStateSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "animationStateSpin");
            _currentHpSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "currentHpSpin");
            _maxHpSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "maxHpSpin");
            _hardnessSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "hardnessSpin");
            _fortitudeSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "fortitudeSpin");
            _reflexSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "reflexSpin");
            _willSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "willSpin");
            _needKeyCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "needKeyCheckbox");
            _removeKeyCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "removeKeyCheckbox");
            _keyEdit = EditorHelpers.FindControlSafe<TextBox>(this, "keyEdit");
            _lockedCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "lockedCheckbox");
            _openLockSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "openLockSpin");
            _difficultySpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "difficultySpin");
            _difficultyModSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "difficultyModSpin");

            string[] scriptNames = { "OnClosed", "OnDamaged", "OnDeath", "OnEndDialog", "OnOpenFailed",
                "OnHeartbeat", "OnInventory", "OnMelee", "OnOpen", "OnLock", "OnUnlock", "OnUsed", "OnUserDefined" };
            foreach (string scriptName in scriptNames)
            {
                string controlName = scriptName.ToLowerInvariant() + "Edit";
                var scriptCombo = EditorHelpers.FindControlSafe<ComboBox>(this, controlName);
                if (scriptCombo != null)
                {
                    scriptCombo.IsEditable = true;
                    SetupScriptComboBoxContextMenu(scriptCombo, scriptName);
                    _scriptFields[scriptName] = scriptCombo;
                }
            }

            var previewHost = EditorHelpers.FindControlSafe<ContentControl>(this, "previewRendererHost");
            if (previewHost != null)
            {
                _previewRenderer = new ModelRenderer { Height = 260, MinHeight = 220 };
                if (_installation != null) _previewRenderer.Installation = _installation;
                previewHost.Content = _previewRenderer;
            }

            _statusText = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");

            SetupSignals();
            HookAppearancePreviewEvent();
            AttachCommitHandlers();
            if (_installation != null) SetupFileContextMenus();
            _xamlControlsLoaded = true;
        }

        private void SetupSignals()
        {
            if (_tagGenerateBtn != null) _tagGenerateBtn.Click += (s, e) => GenerateTag();
            if (_resrefGenerateBtn != null) _resrefGenerateBtn.Click += (s, e) => GenerateResref();
            if (_nameEditBtn != null) _nameEditBtn.Click += (s, e) => EditName();
            if (_conversationModifyBtn != null) _conversationModifyBtn.Click += (s, e) => EditConversation();
            if (_inventoryBtn != null) _inventoryBtn.Click += (s, e) => OpenInventory();
        }

        private void SetupProgrammaticUI()
        {
            var scrollViewer = new ScrollViewer();
            var mainPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Basic Group
            var basicGroup = new Expander { Header = "Basic", IsExpanded = true };
            var basicPanel = new StackPanel { Orientation = Orientation.Vertical };

            // Name
            var nameLabel = new TextBlock { Text = "Name:" };
            _nameEdit = new TextBox { IsReadOnly = true };
            _nameEditBtn = new Button { Content = "Edit Name" };
            _nameEditBtn.Click += (s, e) => EditName();
            basicPanel.Children.Add(nameLabel);
            basicPanel.Children.Add(_nameEdit);
            basicPanel.Children.Add(_nameEditBtn);

            // Tag
            var tagLabel = new TextBlock { Text = "Tag:" };
            _tagEdit = new TextBox();
            _tagGenerateBtn = new Button { Content = "Generate" };
            _tagGenerateBtn.Click += (s, e) => GenerateTag();
            basicPanel.Children.Add(tagLabel);
            basicPanel.Children.Add(_tagEdit);
            basicPanel.Children.Add(_tagGenerateBtn);

            // ResRef
            var resrefLabel = new TextBlock { Text = "ResRef:" };
            _resrefEdit = new TextBox();
            _resrefGenerateBtn = new Button { Content = "Generate" };
            _resrefGenerateBtn.Click += (s, e) => GenerateResref();
            basicPanel.Children.Add(resrefLabel);
            basicPanel.Children.Add(_resrefEdit);
            basicPanel.Children.Add(_resrefGenerateBtn);

            // Appearance
            var appearanceLabel = new TextBlock { Text = "Appearance:" };
            _appearanceSelect = new ComboBox();
            basicPanel.Children.Add(appearanceLabel);
            basicPanel.Children.Add(_appearanceSelect);

            var previewLabel = new TextBlock { Text = "Model Preview:" };
            _previewRenderer = new ModelRenderer
            {
                Height = 260,
                MinHeight = 220,
            };
            if (_installation != null)
            {
                _previewRenderer.Installation = _installation;
            }
            basicPanel.Children.Add(previewLabel);
            basicPanel.Children.Add(_previewRenderer);
            HookAppearancePreviewEvent();

            // Conversation
            var conversationLabel = new TextBlock { Text = "Conversation:" };
            _conversationEdit = new ComboBox { IsEditable = true };
            _conversationModifyBtn = new Button { Content = "Edit" };
            _conversationModifyBtn.Click += (s, e) => EditConversation();
            SetupConversationComboBoxContextMenu(_conversationEdit);
            basicPanel.Children.Add(conversationLabel);
            basicPanel.Children.Add(_conversationEdit);
            basicPanel.Children.Add(_conversationModifyBtn);

            // Inventory
            _inventoryBtn = new Button { Content = "Edit Inventory" };
            _inventoryBtn.Click += (s, e) => OpenInventory();
            _inventoryCountLabel = new TextBlock { Text = "Total Items: 0" };
            basicPanel.Children.Add(_inventoryBtn);
            basicPanel.Children.Add(_inventoryCountLabel);

            basicGroup.Content = basicPanel;
            mainPanel.Children.Add(basicGroup);

            // Advanced Group
            var advancedGroup = new Expander { Header = "Advanced", IsExpanded = false };
            var advancedPanel = new StackPanel { Orientation = Orientation.Vertical };

            _hasInventoryCheckbox = new CheckBox { Content = "Has Inventory" };
            _partyInteractCheckbox = new CheckBox { Content = "Party Interact" };
            _useableCheckbox = new CheckBox { Content = "Useable" };
            _min1HpCheckbox = new CheckBox { Content = "Min 1 HP" };
            _plotCheckbox = new CheckBox { Content = "Plot" };
            _staticCheckbox = new CheckBox { Content = "Static" };
            _notBlastableCheckbox = new CheckBox { Content = "Not Blastable" };
            var factionLabel = new TextBlock { Text = "Faction:" };
            _factionSelect = new ComboBox();
            var animationStateLabel = new TextBlock { Text = "Animation State:" };
            _animationStateSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            var currentHpLabel = new TextBlock { Text = "Current HP:" };
            _currentHpSpin = new NumericUpDown { Minimum = 0, Maximum = 32767 };
            var maxHpLabel = new TextBlock { Text = "Maximum HP:" };
            _maxHpSpin = new NumericUpDown { Minimum = 0, Maximum = 32767 };
            var hardnessLabel = new TextBlock { Text = "Hardness:" };
            _hardnessSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            var fortitudeLabel = new TextBlock { Text = "Fortitude:" };
            _fortitudeSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            var reflexLabel = new TextBlock { Text = "Reflex:" };
            _reflexSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            var willLabel = new TextBlock { Text = "Will:" };
            _willSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };

            advancedPanel.Children.Add(_hasInventoryCheckbox);
            advancedPanel.Children.Add(_partyInteractCheckbox);
            advancedPanel.Children.Add(_useableCheckbox);
            advancedPanel.Children.Add(_min1HpCheckbox);
            advancedPanel.Children.Add(_plotCheckbox);
            advancedPanel.Children.Add(_staticCheckbox);
            advancedPanel.Children.Add(_notBlastableCheckbox);
            advancedPanel.Children.Add(factionLabel);
            advancedPanel.Children.Add(_factionSelect);
            advancedPanel.Children.Add(animationStateLabel);
            advancedPanel.Children.Add(_animationStateSpin);
            advancedPanel.Children.Add(currentHpLabel);
            advancedPanel.Children.Add(_currentHpSpin);
            advancedPanel.Children.Add(maxHpLabel);
            advancedPanel.Children.Add(_maxHpSpin);
            advancedPanel.Children.Add(hardnessLabel);
            advancedPanel.Children.Add(_hardnessSpin);
            advancedPanel.Children.Add(fortitudeLabel);
            advancedPanel.Children.Add(_fortitudeSpin);
            advancedPanel.Children.Add(reflexLabel);
            advancedPanel.Children.Add(_reflexSpin);
            advancedPanel.Children.Add(willLabel);
            advancedPanel.Children.Add(_willSpin);

            advancedGroup.Content = advancedPanel;
            mainPanel.Children.Add(advancedGroup);

            // Lock Group
            var lockGroup = new Expander { Header = "Lock", IsExpanded = false };
            var lockPanel = new StackPanel { Orientation = Orientation.Vertical };

            _needKeyCheckbox = new CheckBox { Content = "Key Required" };
            _removeKeyCheckbox = new CheckBox { Content = "Auto Remove Key" };
            var keyLabel = new TextBlock { Text = "Key Name:" };
            _keyEdit = new TextBox();
            _lockedCheckbox = new CheckBox { Content = "Locked" };
            var openLockLabel = new TextBlock { Text = "Unlock DC:" };
            _openLockSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            var difficultyLabel = new TextBlock { Text = "Unlock Difficulty:" };
            _difficultySpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            var difficultyModLabel = new TextBlock { Text = "Unlock Difficulty Mod:" };
            _difficultyModSpin = new NumericUpDown { Minimum = -128, Maximum = 127 };

            lockPanel.Children.Add(_needKeyCheckbox);
            lockPanel.Children.Add(_removeKeyCheckbox);
            lockPanel.Children.Add(keyLabel);
            lockPanel.Children.Add(_keyEdit);
            lockPanel.Children.Add(_lockedCheckbox);
            lockPanel.Children.Add(openLockLabel);
            lockPanel.Children.Add(_openLockSpin);
            lockPanel.Children.Add(difficultyLabel);
            lockPanel.Children.Add(_difficultySpin);
            lockPanel.Children.Add(difficultyModLabel);
            lockPanel.Children.Add(_difficultyModSpin);

            lockGroup.Content = lockPanel;
            mainPanel.Children.Add(lockGroup);

            // Scripts Group
            var scriptsGroup = new Expander { Header = "Scripts", IsExpanded = false };
            var scriptsPanel = new StackPanel { Orientation = Orientation.Vertical };

            string[] scriptNames = { "OnClosed", "OnDamaged", "OnDeath", "OnEndDialog", "OnOpenFailed",
                "OnHeartbeat", "OnInventory", "OnMelee", "OnOpen", "OnLock", "OnUnlock", "OnUsed", "OnUserDefined" };
            foreach (string scriptName in scriptNames)
            {
                var scriptLabel = new TextBlock { Text = scriptName + ":" };
                var scriptCombo = new ComboBox { IsEditable = true };
                SetupScriptComboBoxContextMenu(scriptCombo, scriptName);
                _scriptFields[scriptName] = scriptCombo;
                scriptsPanel.Children.Add(scriptLabel);
                scriptsPanel.Children.Add(scriptCombo);
            }

            scriptsGroup.Content = scriptsPanel;
            mainPanel.Children.Add(scriptsGroup);

            // Comments Group
            var commentsGroup = new Expander { Header = "Comments", IsExpanded = false };
            var commentsPanel = new StackPanel { Orientation = Orientation.Vertical };
            var commentsLabel = new TextBlock { Text = "Comment:" };
            _commentsEdit = new TextBox { AcceptsReturn = true, AcceptsTab = true };
            commentsPanel.Children.Add(commentsLabel);
            commentsPanel.Children.Add(_commentsEdit);
            commentsGroup.Content = commentsPanel;
            mainPanel.Children.Add(commentsGroup);

            scrollViewer.Content = mainPanel;
            var dock = new DockPanel();
            dock.Children.Add(BuildMenu());
            DockPanel.SetDock(dock.Children[0], Dock.Top);
            dock.Children.Add(scrollViewer);
            _statusText = new Avalonia.Controls.TextBlock { Name = "statusText", Text = "Placeable", Margin = new Avalonia.Thickness(4, 2) };
            dock.Children.Add(_statusText);
            DockPanel.SetDock(_statusText, Dock.Bottom);
            SetContentOrInject(dock);
            AttachCommitHandlers();
        }

        private void AttachCommitHandlers()
        {
            void OnCommit(object s, EventArgs e) { if (!_undoRedoInProgress) PushState(); }
            EditorHelpers.BindLostFocus(_tagEdit, OnCommit);
            EditorHelpers.BindLostFocus(_resrefEdit, OnCommit);
            EditorHelpers.BindLostFocus(_conversationEdit, OnCommit);
            EditorHelpers.BindLostFocus(_keyEdit, OnCommit);
            EditorHelpers.BindLostFocus(_commentsEdit, OnCommit);
            EditorHelpers.BindLostFocus(_animationStateSpin, OnCommit);
            EditorHelpers.BindLostFocus(_currentHpSpin, OnCommit);
            EditorHelpers.BindLostFocus(_maxHpSpin, OnCommit);
            EditorHelpers.BindLostFocus(_hardnessSpin, OnCommit);
            EditorHelpers.BindLostFocus(_fortitudeSpin, OnCommit);
            EditorHelpers.BindLostFocus(_reflexSpin, OnCommit);
            EditorHelpers.BindLostFocus(_willSpin, OnCommit);
            EditorHelpers.BindLostFocus(_openLockSpin, OnCommit);
            EditorHelpers.BindLostFocus(_difficultySpin, OnCommit);
            EditorHelpers.BindLostFocus(_difficultyModSpin, OnCommit);
            EditorHelpers.BindLostFocus(_hasInventoryCheckbox, OnCommit);
            EditorHelpers.BindLostFocus(_partyInteractCheckbox, OnCommit);
            EditorHelpers.BindLostFocus(_useableCheckbox, OnCommit);
            EditorHelpers.BindLostFocus(_min1HpCheckbox, OnCommit);
            EditorHelpers.BindLostFocus(_plotCheckbox, OnCommit);
            EditorHelpers.BindLostFocus(_staticCheckbox, OnCommit);
            EditorHelpers.BindLostFocus(_notBlastableCheckbox, OnCommit);
            EditorHelpers.BindLostFocus(_needKeyCheckbox, OnCommit);
            EditorHelpers.BindLostFocus(_removeKeyCheckbox, OnCommit);
            EditorHelpers.BindLostFocus(_lockedCheckbox, OnCommit);
            if (_scriptFields != null)
                foreach (var kv in _scriptFields)
                    EditorHelpers.BindLostFocus(kv.Value, OnCommit);
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
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
            Bind("actionUndo", () => Undo());
            Bind("actionRedo", () => Redo());
            Bind("actionFind", () => ShowFindDialog());
            Bind("actionFindNext", () => FindNextMatch());
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
                _utp = new UTP();
                LoadUTP(_utp);
            }
            else
            {
                try
                {
                    var gff = GFF.FromBytes(data);
                    _utp = UTPHelpers.ConstructUtp(gff);
                    LoadUTP(_utp);
                }
                catch
                {
                    _utp = new UTP();
                    LoadUTP(_utp);
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
                string text = _utp == null ? "Placeable" : (_utp.Tag ?? "Placeable");
                if (!string.IsNullOrEmpty(_utp?.ResRef?.ToString())) text += " | " + _utp.ResRef;
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
            if (string.IsNullOrEmpty(_findText)) return;
            string t = _findMatchCase ? _findText : _findText.ToLowerInvariant();
            bool Match(string value) => value != null && (_findMatchCase ? value : value.ToLowerInvariant()).Contains(t);
            if (Match(_tagEdit?.Text) && _tagEdit != null) { _tagEdit.Focus(); return; }
            if (Match(_resrefEdit?.Text) && _resrefEdit != null) { _resrefEdit.Focus(); return; }
            if (Match(_conversationEdit?.Text) && _conversationEdit != null) { _conversationEdit.Focus(); return; }
            if (Match(_keyEdit?.Text) && _keyEdit != null) { _keyEdit.Focus(); return; }
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

        private bool _xamlControlsLoaded;

        private void SetupUI()
        {
            if (_xamlControlsLoaded) return;
            if (_statusText == null)
                _statusText = EditorHelpers.FindControlSafe<Avalonia.Controls.TextBlock>(this, "statusText");
            // Try to find controls from XAML if available
            // Use reflection to find controls by name if they were loaded from XAML
            // This matches PyKotor behavior where UI elements are found by name after XAML loading

            // Basic controls (use FindControlSafe: OdyToolUTP may have no XAML / name scope; only assign when found so programmatic UI is not overwritten)
            var nameEdit = EditorHelpers.FindControlSafe<TextBox>(this, "NameEdit") ?? EditorHelpers.FindControlSafe<TextBox>(this, "nameEdit");
            if (nameEdit != null) _nameEdit = nameEdit;
            var nameEditBtn = EditorHelpers.FindControlSafe<Button>(this, "NameEditBtn") ?? EditorHelpers.FindControlSafe<Button>(this, "nameEditBtn");
            if (nameEditBtn != null) _nameEditBtn = nameEditBtn;
            var tagEdit = EditorHelpers.FindControlSafe<TextBox>(this, "TagEdit") ?? EditorHelpers.FindControlSafe<TextBox>(this, "tagEdit");
            if (tagEdit != null) _tagEdit = tagEdit;
            var tagGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "TagGenerateBtn") ?? EditorHelpers.FindControlSafe<Button>(this, "tagGenerateBtn");
            if (tagGenerateBtn != null) _tagGenerateBtn = tagGenerateBtn;
            var resrefEdit = EditorHelpers.FindControlSafe<TextBox>(this, "ResrefEdit") ?? EditorHelpers.FindControlSafe<TextBox>(this, "resrefEdit");
            if (resrefEdit != null) _resrefEdit = resrefEdit;
            var resrefGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "ResrefGenerateBtn") ?? EditorHelpers.FindControlSafe<Button>(this, "resrefGenerateBtn");
            if (resrefGenerateBtn != null) _resrefGenerateBtn = resrefGenerateBtn;
            var appearanceSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "AppearanceSelect") ?? EditorHelpers.FindControlSafe<ComboBox>(this, "appearanceSelect");
            if (appearanceSelect != null) _appearanceSelect = appearanceSelect;
            var previewRenderer = EditorHelpers.FindControlSafe<ModelRenderer>(this, "previewRenderer");
            if (previewRenderer != null) _previewRenderer = previewRenderer;
            var conversationEdit = EditorHelpers.FindControlSafe<ComboBox>(this, "ConversationEdit") ?? EditorHelpers.FindControlSafe<ComboBox>(this, "conversationEdit");
            if (conversationEdit != null) _conversationEdit = conversationEdit;
            var conversationModifyBtn = EditorHelpers.FindControlSafe<Button>(this, "ConversationModifyBtn") ?? EditorHelpers.FindControlSafe<Button>(this, "conversationModifyBtn");
            if (conversationModifyBtn != null) _conversationModifyBtn = conversationModifyBtn;
            var inventoryBtn = EditorHelpers.FindControlSafe<Button>(this, "InventoryBtn") ?? EditorHelpers.FindControlSafe<Button>(this, "inventoryBtn");
            if (inventoryBtn != null) _inventoryBtn = inventoryBtn;
            var inventoryCountLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "InventoryCountLabel") ?? EditorHelpers.FindControlSafe<TextBlock>(this, "inventoryCountLabel");
            if (inventoryCountLabel != null) _inventoryCountLabel = inventoryCountLabel;

            // Advanced controls
            var ctrl = EditorHelpers.FindControlSafe<CheckBox>(this, "HasInventoryCheckbox") ?? EditorHelpers.FindControlSafe<CheckBox>(this, "hasInventoryCheckbox");
            if (ctrl != null) _hasInventoryCheckbox = ctrl;
            ctrl = EditorHelpers.FindControlSafe<CheckBox>(this, "PartyInteractCheckbox") ?? EditorHelpers.FindControlSafe<CheckBox>(this, "partyInteractCheckbox");
            if (ctrl != null) _partyInteractCheckbox = ctrl;
            ctrl = EditorHelpers.FindControlSafe<CheckBox>(this, "UseableCheckbox") ?? EditorHelpers.FindControlSafe<CheckBox>(this, "useableCheckbox");
            if (ctrl != null) _useableCheckbox = ctrl;
            ctrl = EditorHelpers.FindControlSafe<CheckBox>(this, "Min1HpCheckbox") ?? EditorHelpers.FindControlSafe<CheckBox>(this, "min1HpCheckbox");
            if (ctrl != null) _min1HpCheckbox = ctrl;
            ctrl = EditorHelpers.FindControlSafe<CheckBox>(this, "PlotCheckbox") ?? EditorHelpers.FindControlSafe<CheckBox>(this, "plotCheckbox");
            if (ctrl != null) _plotCheckbox = ctrl;
            ctrl = EditorHelpers.FindControlSafe<CheckBox>(this, "StaticCheckbox") ?? EditorHelpers.FindControlSafe<CheckBox>(this, "staticCheckbox");
            if (ctrl != null) _staticCheckbox = ctrl;
            ctrl = EditorHelpers.FindControlSafe<CheckBox>(this, "NotBlastableCheckbox") ?? EditorHelpers.FindControlSafe<CheckBox>(this, "notBlastableCheckbox");
            if (ctrl != null) _notBlastableCheckbox = ctrl;
            var factionSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "FactionSelect") ?? EditorHelpers.FindControlSafe<ComboBox>(this, "factionSelect");
            if (factionSelect != null) _factionSelect = factionSelect;
            var animationStateSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "AnimationStateSpin") ?? EditorHelpers.FindControlSafe<NumericUpDown>(this, "animationStateSpin");
            if (animationStateSpin != null) _animationStateSpin = animationStateSpin;
            var currentHpSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "CurrentHpSpin") ?? EditorHelpers.FindControlSafe<NumericUpDown>(this, "currentHpSpin");
            if (currentHpSpin != null) _currentHpSpin = currentHpSpin;
            var maxHpSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "MaxHpSpin") ?? EditorHelpers.FindControlSafe<NumericUpDown>(this, "maxHpSpin");
            if (maxHpSpin != null) _maxHpSpin = maxHpSpin;
            var hardnessSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "HardnessSpin") ?? EditorHelpers.FindControlSafe<NumericUpDown>(this, "hardnessSpin");
            if (hardnessSpin != null) _hardnessSpin = hardnessSpin;
            var fortitudeSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "FortitudeSpin") ?? EditorHelpers.FindControlSafe<NumericUpDown>(this, "fortitudeSpin");
            if (fortitudeSpin != null) _fortitudeSpin = fortitudeSpin;
            var reflexSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "ReflexSpin") ?? EditorHelpers.FindControlSafe<NumericUpDown>(this, "reflexSpin");
            if (reflexSpin != null) _reflexSpin = reflexSpin;
            var willSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "WillSpin") ?? EditorHelpers.FindControlSafe<NumericUpDown>(this, "willSpin");
            if (willSpin != null) _willSpin = willSpin;

            // Lock controls
            ctrl = EditorHelpers.FindControlSafe<CheckBox>(this, "NeedKeyCheckbox") ?? EditorHelpers.FindControlSafe<CheckBox>(this, "needKeyCheckbox");
            if (ctrl != null) _needKeyCheckbox = ctrl;
            ctrl = EditorHelpers.FindControlSafe<CheckBox>(this, "RemoveKeyCheckbox") ?? EditorHelpers.FindControlSafe<CheckBox>(this, "removeKeyCheckbox");
            if (ctrl != null) _removeKeyCheckbox = ctrl;
            var keyEdit = EditorHelpers.FindControlSafe<TextBox>(this, "KeyEdit") ?? EditorHelpers.FindControlSafe<TextBox>(this, "keyEdit");
            if (keyEdit != null) _keyEdit = keyEdit;
            ctrl = EditorHelpers.FindControlSafe<CheckBox>(this, "LockedCheckbox") ?? EditorHelpers.FindControlSafe<CheckBox>(this, "lockedCheckbox");
            if (ctrl != null) _lockedCheckbox = ctrl;
            var openLockSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "OpenLockSpin") ?? EditorHelpers.FindControlSafe<NumericUpDown>(this, "openLockSpin");
            if (openLockSpin != null) _openLockSpin = openLockSpin;
            var difficultySpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "DifficultySpin") ?? EditorHelpers.FindControlSafe<NumericUpDown>(this, "difficultySpin");
            if (difficultySpin != null) _difficultySpin = difficultySpin;
            var difficultyModSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "DifficultyModSpin") ?? EditorHelpers.FindControlSafe<NumericUpDown>(this, "difficultyModSpin");
            if (difficultyModSpin != null) _difficultyModSpin = difficultyModSpin;

            // Script controls - find by name pattern
            string[] scriptNames = { "OnClosed", "OnDamaged", "OnDeath", "OnEndDialog", "OnOpenFailed",
                "OnHeartbeat", "OnInventory", "OnMelee", "OnOpen", "OnLock", "OnUnlock", "OnUsed", "OnUserDefined" };

            foreach (string scriptName in scriptNames)
            {
                var scriptCombo = EditorHelpers.FindControlSafe<ComboBox>(this, scriptName + "Edit") ?? EditorHelpers.FindControlSafe<ComboBox>(this, scriptName.ToLower() + "Edit");
                if (scriptCombo != null)
                {
                    scriptCombo.IsEditable = true;
                    SetupScriptComboBoxContextMenu(scriptCombo, scriptName);
                    _scriptFields[scriptName] = scriptCombo;
                }
            }

            // Comments control
            var commentsEdit = EditorHelpers.FindControlSafe<TextBox>(this, "CommentsEdit") ?? EditorHelpers.FindControlSafe<TextBox>(this, "commentsEdit");
            if (commentsEdit != null) _commentsEdit = commentsEdit;

            // Set up event handlers for controls that were found from XAML
            if (_nameEditBtn != null)
            {
                _nameEditBtn.Click += (s, e) => EditName();
            }
            if (_tagGenerateBtn != null)
            {
                _tagGenerateBtn.Click += (s, e) => GenerateTag();
            }
            if (_resrefGenerateBtn != null)
            {
                _resrefGenerateBtn.Click += (s, e) => GenerateResref();
            }
            if (_conversationModifyBtn != null)
            {
                _conversationModifyBtn.Click += (s, e) => EditConversation();
            }
            if (_inventoryBtn != null)
            {
                _inventoryBtn.Click += (s, e) => OpenInventory();
            }

            if (_previewRenderer != null && _installation != null)
            {
                _previewRenderer.Installation = _installation;
            }

            HookAppearancePreviewEvent();

            // Set up context menus for script fields if they were found from XAML
            if (_installation != null)
            {
                SetupFileContextMenus();
            }
        }

        private void HookAppearancePreviewEvent()
        {
            if (_appearancePreviewHooked || _appearanceSelect == null)
            {
                return;
            }

            _appearanceSelect.SelectionChanged += (s, e) => RefreshPlaceablePreview();
            _appearancePreviewHooked = true;
        }

        private void SetupInstallation(OdyInstallation installation)
        {
            _installation = installation;

            List<string> required = new List<string> { OdyInstallation.TwoDAPlaceables, OdyInstallation.TwoDAFactions };
            installation.HtBatchCache2DA(required);

            TwoDA appearances = installation.HtGetCache2DA(OdyInstallation.TwoDAPlaceables);
            if (_appearanceSelect != null && appearances != null)
            {
                _appearanceSelect.Items.Clear();
                List<string> appearanceLabels = appearances.GetColumn("label");
                foreach (string label in appearanceLabels)
                {
                    _appearanceSelect.Items.Add(label);
                }
            }

            if (_previewRenderer != null)
            {
                _previewRenderer.Installation = _installation;
            }

            TwoDA factions = installation.HtGetCache2DA(OdyInstallation.TwoDAFactions);
            if (_factionSelect != null && factions != null)
            {
                _factionSelect.Items.Clear();
                List<string> factionLabels = factions.GetColumn("label");
                foreach (string label in factionLabels)
                {
                    _factionSelect.Items.Add(label);
                }
            }

            if (_notBlastableCheckbox != null)
            {
                _notBlastableCheckbox.IsVisible = installation.Tsl;
            }
            if (_difficultySpin != null)
            {
                _difficultySpin.IsVisible = installation.Tsl;
            }
            if (_difficultyModSpin != null)
            {
                _difficultyModSpin.IsVisible = installation.Tsl;
            }

            SetupFileContextMenus();

            HookAppearancePreviewEvent();
            RefreshPlaceablePreview();
        }

        private void SetupFileContextMenus()
        {
            if (_installation == null)
            {
                return;
            }

            // Script combos get context menu when created/found; PopulateScriptComboBoxes called from LoadUTP

            // Setup context menu for conversation field (DLG files)
            if (_conversationEdit != null)
            {
                SetupConversationComboBoxContextMenu(_conversationEdit);
            }
        }

        private void SetupScriptComboBoxContextMenu(ComboBox comboBox, string scriptTypeName)
        {
            if (comboBox == null) return;
            var contextMenu = new ContextMenu();
            var openInEditorItem = new MenuItem { Header = "Open in OdyToolNSS", IsEnabled = false };
            openInEditorItem.Click += (sender, e) => OpenScriptInEditor(comboBox, scriptTypeName);
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

        // Create context menu for conversation ComboBox (Open in OdyToolDLG)
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

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _undoStack.Clear();
            _redoStack.Clear();
            try { LoadFromBytes(data); }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load UTP: {ex}");
                New();
            }
        }

        private void LoadUTP(UTP utp)
        {
            _utp = utp;

            // Basic
            if (_nameEdit != null)
            {
                _nameEdit.Text = _installation != null ? _installation.String(utp.Name) : utp.Name.StringRef.ToString();
            }
            if (_tagEdit != null)
            {
                _tagEdit.Text = utp.Tag;
            }
            if (_resrefEdit != null)
            {
                _resrefEdit.Text = utp.ResRef.ToString();
            }
            if (_appearanceSelect != null)
            {
                _appearanceSelect.SelectedIndex = utp.AppearanceId;
            }
            if (_conversationEdit != null)
            {
                _conversationEdit.Text = utp.Conversation.ToString();
            }

            // Advanced
            if (_hasInventoryCheckbox != null) _hasInventoryCheckbox.IsChecked = utp.HasInventory;
            if (_partyInteractCheckbox != null) _partyInteractCheckbox.IsChecked = utp.PartyInteract;
            if (_useableCheckbox != null) _useableCheckbox.IsChecked = utp.Useable;
            if (_min1HpCheckbox != null) _min1HpCheckbox.IsChecked = utp.Min1Hp;
            if (_plotCheckbox != null) _plotCheckbox.IsChecked = utp.Plot;
            if (_staticCheckbox != null) _staticCheckbox.IsChecked = utp.Static;
            if (_notBlastableCheckbox != null) _notBlastableCheckbox.IsChecked = utp.NotBlastable;
            if (_factionSelect != null) _factionSelect.SelectedIndex = utp.FactionId;
            if (_animationStateSpin != null) _animationStateSpin.Value = utp.AnimationState;
            if (_currentHpSpin != null) _currentHpSpin.Value = utp.CurrentHp;
            if (_maxHpSpin != null) _maxHpSpin.Value = utp.MaximumHp;
            if (_hardnessSpin != null) _hardnessSpin.Value = utp.Hardness;
            if (_fortitudeSpin != null) _fortitudeSpin.Value = utp.Fortitude;
            if (_reflexSpin != null) _reflexSpin.Value = utp.Reflex;
            if (_willSpin != null) _willSpin.Value = utp.Will;

            // Lock
            if (_needKeyCheckbox != null) _needKeyCheckbox.IsChecked = utp.KeyRequired;
            if (_removeKeyCheckbox != null) _removeKeyCheckbox.IsChecked = utp.AutoRemoveKey;
            if (_keyEdit != null) _keyEdit.Text = utp.KeyName;
            if (_lockedCheckbox != null) _lockedCheckbox.IsChecked = utp.Locked;
            if (_openLockSpin != null) _openLockSpin.Value = utp.UnlockDc;
            if (_difficultySpin != null) _difficultySpin.Value = utp.UnlockDiff;
            if (_difficultyModSpin != null) _difficultyModSpin.Value = utp.UnlockDiffMod;

            // Set script values from UTP
            if (_scriptFields.ContainsKey("OnClosed") && _scriptFields["OnClosed"] != null)
                _scriptFields["OnClosed"].Text = utp.OnClosed.ToString();
            if (_scriptFields.ContainsKey("OnDamaged") && _scriptFields["OnDamaged"] != null)
                _scriptFields["OnDamaged"].Text = utp.OnDamaged.ToString();
            if (_scriptFields.ContainsKey("OnDeath") && _scriptFields["OnDeath"] != null)
                _scriptFields["OnDeath"].Text = utp.OnDeath.ToString();
            if (_scriptFields.ContainsKey("OnEndDialog") && _scriptFields["OnEndDialog"] != null)
                _scriptFields["OnEndDialog"].Text = utp.OnEndDialog.ToString();
            if (_scriptFields.ContainsKey("OnOpenFailed") && _scriptFields["OnOpenFailed"] != null)
                _scriptFields["OnOpenFailed"].Text = utp.OnOpenFailed.ToString();
            if (_scriptFields.ContainsKey("OnHeartbeat") && _scriptFields["OnHeartbeat"] != null)
                _scriptFields["OnHeartbeat"].Text = utp.OnHeartbeat.ToString();
            if (_scriptFields.ContainsKey("OnInventory") && _scriptFields["OnInventory"] != null)
                _scriptFields["OnInventory"].Text = utp.OnInventory.ToString();
            if (_scriptFields.ContainsKey("OnMelee") && _scriptFields["OnMelee"] != null)
                _scriptFields["OnMelee"].Text = utp.OnMelee.ToString();
            if (_scriptFields.ContainsKey("OnOpen") && _scriptFields["OnOpen"] != null)
                _scriptFields["OnOpen"].Text = utp.OnOpen.ToString();
            if (_scriptFields.ContainsKey("OnLock") && _scriptFields["OnLock"] != null)
                _scriptFields["OnLock"].Text = utp.OnLock.ToString();
            if (_scriptFields.ContainsKey("OnUnlock") && _scriptFields["OnUnlock"] != null)
                _scriptFields["OnUnlock"].Text = utp.OnUnlock.ToString();
            if (_scriptFields.ContainsKey("OnUsed") && _scriptFields["OnUsed"] != null)
                _scriptFields["OnUsed"].Text = utp.OnUsed.ToString();
            if (_scriptFields.ContainsKey("OnUserDefined") && _scriptFields["OnUserDefined"] != null)
                _scriptFields["OnUserDefined"].Text = utp.OnUserDefined.ToString();

            PopulateScriptComboBoxes();
            PopulateConversationComboBox();

            // Comments
            if (_commentsEdit != null) _commentsEdit.Text = utp.Comment;

            RefreshPlaceablePreview();
        }

        private void RefreshPlaceablePreview()
        {
            if (_previewRenderer == null)
            {
                return;
            }

            _previewRenderer.Installation = _installation;

            if (_installation == null || _utp == null)
            {
                _previewRenderer.ClearModel();
                return;
            }

            try
            {
                UTP previewUtp = CopyUtp(_utp);
                if (_appearanceSelect != null && _appearanceSelect.SelectedIndex >= 0)
                {
                    previewUtp.AppearanceId = _appearanceSelect.SelectedIndex;
                }

                string modelName = BioWare.Tools.Placeable.GetModel(previewUtp, _installation.Installation);
                if (string.IsNullOrWhiteSpace(modelName))
                {
                    _previewRenderer.ClearModel();
                    return;
                }

                var mdlResult = _installation.Resource(modelName, ResourceType.MDL, null);
                var mdxResult = _installation.Resource(modelName, ResourceType.MDX, null);
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
                System.Console.WriteLine("Failed to refresh UTP model preview: " + ex.Message);
                _previewRenderer.ClearModel();
            }
        }

        public override Tuple<byte[], byte[]> Build()
        {
            // Matching Python: utp: UTP = deepcopy(self._utp)
            var utp = CopyUtp(_utp);

            // Basic - read from UI controls (matching Python which always reads from UI)
            // Python: utp.name = self.ui.nameEdit.locstring()
            // In C#, nameEdit is TextBox (read-only), LocalizedString is stored in _utp.Name and updated via EditName()
            // So we use utp.Name from the copy (which preserves the value set by EditName())
            // Note: This matches Python behavior where locstring() returns the stored LocalizedString
            utp.Name = utp.Name ?? LocalizedString.FromInvalid();
            utp.Tag = _tagEdit?.Text ?? "";
            utp.ResRef = new ResRef(_resrefEdit?.Text ?? "");
            utp.AppearanceId = _appearanceSelect?.SelectedIndex ?? 0;
            utp.Conversation = new ResRef(_conversationEdit?.Text ?? "");
            utp.HasInventory = _hasInventoryCheckbox?.IsChecked == true;

            // Advanced - read from UI controls
            utp.Min1Hp = _min1HpCheckbox?.IsChecked == true;
            utp.PartyInteract = _partyInteractCheckbox?.IsChecked == true;
            utp.Useable = _useableCheckbox?.IsChecked == true;
            utp.Plot = _plotCheckbox?.IsChecked == true;
            utp.Static = _staticCheckbox?.IsChecked == true;
            utp.NotBlastable = _notBlastableCheckbox?.IsChecked == true;
            utp.FactionId = _factionSelect?.SelectedIndex ?? 0;
            utp.AnimationState = (int)(_animationStateSpin?.Value ?? 0);
            utp.CurrentHp = (int)(_currentHpSpin?.Value ?? 0);
            utp.MaximumHp = (int)(_maxHpSpin?.Value ?? 0);
            utp.Hardness = (int)(_hardnessSpin?.Value ?? 0);
            utp.Fortitude = (int)(_fortitudeSpin?.Value ?? 0);
            utp.Reflex = (int)(_reflexSpin?.Value ?? 0);
            utp.Will = (int)(_willSpin?.Value ?? 0);

            // Lock - read from UI controls
            utp.Locked = _lockedCheckbox?.IsChecked == true;
            utp.UnlockDc = (int)(_openLockSpin?.Value ?? 0);
            utp.UnlockDiff = (int)(_difficultySpin?.Value ?? 0);
            utp.UnlockDiffMod = (int)(_difficultyModSpin?.Value ?? 0);
            utp.KeyRequired = _needKeyCheckbox?.IsChecked == true;
            utp.AutoRemoveKey = _removeKeyCheckbox?.IsChecked == true;
            utp.KeyName = _keyEdit?.Text ?? "";

            // Scripts - read from UI controls
            if (_scriptFields.ContainsKey("OnClosed") && _scriptFields["OnClosed"] != null)
                utp.OnClosed = new ResRef(_scriptFields["OnClosed"].Text);
            if (_scriptFields.ContainsKey("OnDamaged") && _scriptFields["OnDamaged"] != null)
                utp.OnDamaged = new ResRef(_scriptFields["OnDamaged"].Text);
            if (_scriptFields.ContainsKey("OnDeath") && _scriptFields["OnDeath"] != null)
                utp.OnDeath = new ResRef(_scriptFields["OnDeath"].Text);
            if (_scriptFields.ContainsKey("OnEndDialog") && _scriptFields["OnEndDialog"] != null)
                utp.OnEndDialog = new ResRef(_scriptFields["OnEndDialog"].Text);
            if (_scriptFields.ContainsKey("OnOpenFailed") && _scriptFields["OnOpenFailed"] != null)
                utp.OnOpenFailed = new ResRef(_scriptFields["OnOpenFailed"].Text);
            if (_scriptFields.ContainsKey("OnHeartbeat") && _scriptFields["OnHeartbeat"] != null)
                utp.OnHeartbeat = new ResRef(_scriptFields["OnHeartbeat"].Text);
            if (_scriptFields.ContainsKey("OnInventory") && _scriptFields["OnInventory"] != null)
                utp.OnInventory = new ResRef(_scriptFields["OnInventory"].Text);
            if (_scriptFields.ContainsKey("OnMelee") && _scriptFields["OnMelee"] != null)
                utp.OnMelee = new ResRef(_scriptFields["OnMelee"].Text);
            if (_scriptFields.ContainsKey("OnOpen") && _scriptFields["OnOpen"] != null)
                utp.OnOpen = new ResRef(_scriptFields["OnOpen"].Text);
            if (_scriptFields.ContainsKey("OnLock") && _scriptFields["OnLock"] != null)
                utp.OnLock = new ResRef(_scriptFields["OnLock"].Text);
            if (_scriptFields.ContainsKey("OnUnlock") && _scriptFields["OnUnlock"] != null)
                utp.OnUnlock = new ResRef(_scriptFields["OnUnlock"].Text);
            if (_scriptFields.ContainsKey("OnUsed") && _scriptFields["OnUsed"] != null)
                utp.OnUsed = new ResRef(_scriptFields["OnUsed"].Text);
            if (_scriptFields.ContainsKey("OnUserDefined") && _scriptFields["OnUserDefined"] != null)
                utp.OnUserDefined = new ResRef(_scriptFields["OnUserDefined"].Text);

            // Comments - read from UI controls
            utp.Comment = _commentsEdit?.Text ?? "";

            // Matching Python: gff: GFF = dismantle_utp(utp); write_gff(gff, data)
            BioWareGame game = _installation?.Game ?? BioWareGame.K2;
            var gff = UTPHelpers.DismantleUtp(utp, game);
            byte[] data = GFFAuto.BytesGff(gff, ResourceType.UTP);
            return Tuple.Create(data, new byte[0]);
        }

        // Matching Python: deepcopy(self._utp)
        private static UTP CopyUtp(UTP source)
        {
            // Use Dismantle/Construct pattern for reliable deep copy (matching Python deepcopy behavior)
            BioWareGame game = BioWareGame.K2; // Default game for serialization
            var gff = UTPHelpers.DismantleUtp(source, game);
            return UTPHelpers.ConstructUtp(gff);
        }

        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            _utp = new UTP();
            LoadUTP(_utp);
            UpdateItemCount();
            UpdateStatusBar();
        }

        private void UpdateItemCount()
        {
            if (_inventoryCountLabel != null && _utp != null)
            {
                int count = _utp.Inventory != null ? _utp.Inventory.Count : 0;
                _inventoryCountLabel.Text = $"Total Items: {count}";
            }
        }

        private void EditName()
        {
            if (_installation == null) return;
            var dialog = new LocalizedStringDialog(this, _installation, _utp.Name);
            if (dialog.ShowDialog())
            {
                _utp.Name = dialog.LocString;
                if (_nameEdit != null)
                {
                    _nameEdit.Text = _installation.String(_utp.Name);
                }
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
        }

        private void GenerateResref()
        {
            if (_resrefEdit != null)
            {
                _resrefEdit.Text = !string.IsNullOrEmpty(base._resname) ? base._resname : "m00xx_plc_000";
            }
        }

        private void EditConversation()
        {
            string resname = (_conversationEdit?.Text ?? "").Trim();
            byte[] data = null;
            string filepath = null;

            if (string.IsNullOrEmpty(resname))
            {
                _ = DialogHelper.ShowAsync("Failed to open OdyToolDLG", "Conversation field cannot be blank.", ButtonEnum.Ok, IconType.Error);
                return;
            }

            if (_installation == null)
            {
                return;
            }

            var search = _installation.Resource(resname, ResourceType.DLG);

            if (search == null)
            {
                var result = DialogHelper.ShowAsync(
                    "DLG file not found",
                    "Do you wish to create a file in the override?",
                    ButtonEnum.YesNo,
                    IconType.Question).GetAwaiter().GetResult();

                if (result == ButtonResult.Yes)
                {
                    var dlg = new DLGType();
                    var gff = DLGHelper.DismantleDlg(dlg, _installation.Game);
                    data = GFFAuto.BytesGff(gff, ResourceType.DLG);
                    filepath = System.IO.Path.Combine(_installation.OverridePath(), $"{resname}.dlg");
                    File.WriteAllBytes(filepath, data);
                }
            }
            else
            {
                resname = search.ResName;
                filepath = search.FilePath;
                data = search.Data;
            }

            if (data != null)
            {
                WindowUtils.OpenResourceEditor(filepath, resname, ResourceType.DLG, data, _installation, this);
            }
        }

        private void OpenInventory()
        {
            if (_installation == null)
            {
                return;
            }

            var capsules = new List<Capsule>();
            try
            {
                string root = null;
                if (!string.IsNullOrEmpty(_filepath))
                {
                    root = Module.FilepathToRoot(_filepath);
                }

                var moduleNames = _installation.ModuleNames();
                var matchingModules = new List<string>();
                foreach (var kvp in moduleNames)
                {
                    string modulePath = kvp.Value ?? kvp.Key;
                    if (root != null && modulePath.Contains(root) && modulePath != _filepath)
                    {
                        matchingModules.Add(kvp.Key);
                    }
                }

                foreach (string modFilename in matchingModules)
                {
                    string modulePath = System.IO.Path.Combine(_installation.ModulePath(), modFilename);
                    if (File.Exists(modulePath))
                    {
                        try
                        {
                            var capsule = new Capsule(modulePath, createIfNotExist: false);
                            capsules.Add(capsule);
                        }
                        catch
                        {
                            // Skip invalid capsule files
                        }
                    }
                }
            }
            catch
            {
            }

            var inventoryEditor = new InventoryDialog(
                this,
                _installation,
                capsules,
                new List<string>(), // folders parameter
                _utp.Inventory ?? new List<InventoryItem>(),
                new Dictionary<EquipmentSlot, InventoryItem>(), // equipment parameter
                droid: false,
                hideEquipment: true,
                isStore: false
            );

            if (inventoryEditor.ShowDialog())
            {
                _utp.Inventory = inventoryEditor.Inventory ?? new List<InventoryItem>();
                UpdateItemCount();
            }
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }
    }
}
