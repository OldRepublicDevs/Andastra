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
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using GFFAuto = BioWare.Resource.Formats.GFF.GFFAuto;
using Window = Avalonia.Controls.Window;

namespace OdyTools.Editors
{
    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:38
    // Original: class OdyToolUTP(Editor):
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

        // UI Controls - Basic
        private TextBox _nameEdit;
        private Button _nameEditBtn;
        private TextBox _tagEdit;
        private Button _tagGenerateBtn;
        private TextBox _resrefEdit;
        private Button _resrefGenerateBtn;
        private ComboBox _appearanceSelect;
        private TextBox _conversationEdit;
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

        // UI Controls - Scripts
        private Dictionary<string, TextBox> _scriptFields;
        private List<string> _relevantScriptResnames;

        // UI Controls - Comments
        private TextBox _commentsEdit;

        // Matching PyKotor implementation: Expose UI controls for testing
        // Original: editor.ui.tagEdit, editor.ui.resrefEdit, etc.
        public TextBox TagEdit => _tagEdit;
        public Button TagGenerateBtn => _tagGenerateBtn;
        public TextBox ResrefEdit => _resrefEdit;
        public Button ResrefGenerateBtn => _resrefGenerateBtn;

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:39-84
        // Original: def __init__(self, parent, installation):
        public OdyToolUTP(Window parent = null, OdyInstallation installation = null)
            : base(parent, "Placeable Editor", "placeable",
                new[] { ResourceType.UTP, ResourceType.BTP },
                new[] { ResourceType.UTP, ResourceType.BTP },
                installation)
        {
            _installation = installation;
            _utp = new UTP();
            _scriptFields = new Dictionary<string, TextBox>();
            _relevantScriptResnames = new List<string>();

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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:86-109
        // Original: def _setup_signals(self):
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

            // Conversation
            var conversationLabel = new TextBlock { Text = "Conversation:" };
            _conversationEdit = new TextBox();
            _conversationModifyBtn = new Button { Content = "Edit" };
            _conversationModifyBtn.Click += (s, e) => EditConversation();
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
                var scriptEdit = new TextBox();
                _scriptFields[scriptName] = scriptEdit;
                scriptsPanel.Children.Add(scriptLabel);
                scriptsPanel.Children.Add(scriptEdit);
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
            Content = dock;
            AttachCommitHandlers();
        }

        private void AttachCommitHandlers()
        {
            void OnCommit(object s, EventArgs e) { if (!_undoRedoInProgress) PushState(); }
            if (_tagEdit != null) _tagEdit.LostFocus += OnCommit;
            if (_resrefEdit != null) _resrefEdit.LostFocus += OnCommit;
            if (_conversationEdit != null) _conversationEdit.LostFocus += OnCommit;
            if (_keyEdit != null) _keyEdit.LostFocus += OnCommit;
            if (_commentsEdit != null) _commentsEdit.LostFocus += OnCommit;
            if (_animationStateSpin != null) _animationStateSpin.LostFocus += OnCommit;
            if (_currentHpSpin != null) _currentHpSpin.LostFocus += OnCommit;
            if (_maxHpSpin != null) _maxHpSpin.LostFocus += OnCommit;
            if (_hardnessSpin != null) _hardnessSpin.LostFocus += OnCommit;
            if (_fortitudeSpin != null) _fortitudeSpin.LostFocus += OnCommit;
            if (_reflexSpin != null) _reflexSpin.LostFocus += OnCommit;
            if (_willSpin != null) _willSpin.LostFocus += OnCommit;
            if (_openLockSpin != null) _openLockSpin.LostFocus += OnCommit;
            if (_difficultySpin != null) _difficultySpin.LostFocus += OnCommit;
            if (_difficultyModSpin != null) _difficultyModSpin.LostFocus += OnCommit;
            if (_hasInventoryCheckbox != null) _hasInventoryCheckbox.LostFocus += OnCommit;
            if (_partyInteractCheckbox != null) _partyInteractCheckbox.LostFocus += OnCommit;
            if (_useableCheckbox != null) _useableCheckbox.LostFocus += OnCommit;
            if (_min1HpCheckbox != null) _min1HpCheckbox.LostFocus += OnCommit;
            if (_plotCheckbox != null) _plotCheckbox.LostFocus += OnCommit;
            if (_staticCheckbox != null) _staticCheckbox.LostFocus += OnCommit;
            if (_notBlastableCheckbox != null) _notBlastableCheckbox.LostFocus += OnCommit;
            if (_needKeyCheckbox != null) _needKeyCheckbox.LostFocus += OnCommit;
            if (_removeKeyCheckbox != null) _removeKeyCheckbox.LostFocus += OnCommit;
            if (_lockedCheckbox != null) _lockedCheckbox.LostFocus += OnCommit;
            if (_scriptFields != null)
                foreach (var kv in _scriptFields)
                    if (kv.Value != null) kv.Value.LostFocus += OnCommit;
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

        private void Revert()
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

        private async Task RunSaveAsAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            string suggestedName = string.IsNullOrEmpty(_resname) ? "placeable" : _resname;
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".utp",
                FileTypeChoices = new[] { new FilePickerFileType("UTP") { Patterns = new[] { "*.utp", "*.btp" } } }
            };
            var file = await storageProvider.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            RefreshWindowTitle();
            Save();
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

        private void SetupUI()
        {
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
            var conversationEdit = EditorHelpers.FindControlSafe<TextBox>(this, "ConversationEdit") ?? EditorHelpers.FindControlSafe<TextBox>(this, "conversationEdit");
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
                var scriptEdit = EditorHelpers.FindControlSafe<TextBox>(this, scriptName + "Edit") ?? EditorHelpers.FindControlSafe<TextBox>(this, scriptName.ToLower() + "Edit");
                if (scriptEdit != null)
                {
                    _scriptFields[scriptName] = scriptEdit;
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

            // Set up context menus for script fields if they were found from XAML
            if (_installation != null)
            {
                SetupFileContextMenus();
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:110-166
        // Original: def _setup_installation(self, installation):
        private void SetupInstallation(OdyInstallation installation)
        {
            _installation = installation;

            // Matching PyKotor implementation: Load required 2da files if they have not been loaded already
            List<string> required = new List<string> { OdyInstallation.TwoDAPlaceables, OdyInstallation.TwoDAFactions };
            installation.HtBatchCache2DA(required);

            // Matching PyKotor implementation: appearances: TwoDA | None = installation.ht_get_cache_2da(OdyInstallation.TwoDA_PLACEABLES)
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

            // Matching PyKotor implementation: factions: TwoDA | None = installation.ht_get_cache_2da(OdyInstallation.TwoDA_FACTIONS)
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

            // Matching PyKotor implementation: self.ui.notBlastableCheckbox.setVisible(installation.tsl)
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

            // Matching PyKotor implementation: self._installation.setup_file_context_menu(...)
            SetupFileContextMenus();

            // Matching PyKotor implementation: self.relevant_script_resnames = sorted(...)
            if (installation != null && !string.IsNullOrEmpty(base._filepath))
            {
                HashSet<FileResource> scriptResources = installation.GetRelevantResources(ResourceType.NCS, base._filepath);
                _relevantScriptResnames = scriptResources
                    .Select(r => r.ResName.ToLowerInvariant())
                    .Distinct()
                    .OrderBy(r => r)
                    .ToList();
            }
            else
            {
                _relevantScriptResnames = new List<string>();
            }
        }

        // Matching PyKotor implementation: self._installation.setup_file_context_menu(...)
        private void SetupFileContextMenus()
        {
            if (_installation == null)
            {
                return;
            }

            // Setup context menus for script TextBoxes (NSS/NCS files)
            foreach (var kvp in _scriptFields)
            {
                SetupScriptTextBoxContextMenu(kvp.Value, kvp.Key + " Script");
            }

            // Setup context menu for conversation field (DLG files)
            if (_conversationEdit != null)
            {
                SetupConversationTextBoxContextMenu(_conversationEdit);
            }
        }

        // Create context menu for script TextBox controls
        private void SetupScriptTextBoxContextMenu(TextBox textBox, string scriptTypeName)
        {
            if (textBox == null)
            {
                return;
            }

            var contextMenu = new ContextMenu();
            var menuItems = new List<MenuItem>();

            // "Open in Editor" menu item
            var openInEditorItem = new MenuItem
            {
                Header = "Open in Editor",
                IsEnabled = false
            };
            openInEditorItem.Click += (sender, e) => OpenScriptInEditor(textBox, scriptTypeName);
            menuItems.Add(openInEditorItem);

            // Enable/disable based on whether script name is set
            textBox.TextChanged += (sender, e) =>
            {
                string text = textBox.Text ?? string.Empty;
                openInEditorItem.IsEnabled = !string.IsNullOrWhiteSpace(text);
            };

            foreach (var item in menuItems)
            {
                contextMenu.Items.Add(item);
            }
            textBox.ContextMenu = contextMenu;
        }

        // Create context menu for conversation TextBox control
        private void SetupConversationTextBoxContextMenu(TextBox textBox)
        {
            if (textBox == null)
            {
                return;
            }

            var contextMenu = new ContextMenu();
            var menuItems = new List<MenuItem>();

            // "Open in Editor" menu item
            var openInEditorItem = new MenuItem
            {
                Header = "Open in Editor",
                IsEnabled = false
            };
            openInEditorItem.Click += (sender, e) => EditConversation();
            menuItems.Add(openInEditorItem);

            // Enable/disable based on whether conversation name is set
            textBox.TextChanged += (sender, e) =>
            {
                string text = textBox.Text ?? string.Empty;
                openInEditorItem.IsEnabled = !string.IsNullOrWhiteSpace(text);
            };

            foreach (var item in menuItems)
            {
                contextMenu.Items.Add(item);
            }
            textBox.ContextMenu = contextMenu;
        }

        // Open script in editor
        private void OpenScriptInEditor(TextBox textBox, string scriptTypeName)
        {
            if (_installation == null || textBox == null)
            {
                return;
            }

            string resname = textBox.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(resname))
            {
                return;
            }

            // Try NCS first, then NSS
            var search = _installation.Resource(resname, ResourceType.NCS)
                         ?? _installation.Resource(resname, ResourceType.NSS);

            if (search != null)
            {
                WindowUtils.OpenResourceEditor(
                    search.FilePath,
                    search.ResName,
                    search.ResType,
                    search.Data,
                    _installation,
                    this
                );
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:168-268
        // Original: def load(self, filepath, resref, restype, data):
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:182-268
        // Original: def _loadUTP(self, utp):
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

            // Scripts - populate with relevant resources first, then set values
            // Matching PyKotor implementation: self.ui.onClosedEdit.populate_combo_box(self.relevant_script_resnames)
            if (_installation != null && !string.IsNullOrEmpty(base._filepath))
            {
                // Populate script fields with relevant resources (for autocomplete-like behavior)
                // TODO: STUB - Note: In Python, these are ComboBoxes with populate_combo_box, but in C# we use TextBox
                // TODO:  So we'll just set the text value - autocomplete would require a different control
            }

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

            // Comments
            if (_commentsEdit != null) _commentsEdit.Text = utp.Comment;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:270-346
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            // Matching Python: utp: UTP = deepcopy(self._utp)
            var utp = CopyUtp(_utp);

            // Basic - read from UI controls (matching Python which always reads from UI)
            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:291
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
            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:298-312
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
            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:314-321
            utp.Locked = _lockedCheckbox?.IsChecked == true;
            utp.UnlockDc = (int)(_openLockSpin?.Value ?? 0);
            utp.UnlockDiff = (int)(_difficultySpin?.Value ?? 0);
            utp.UnlockDiffMod = (int)(_difficultyModSpin?.Value ?? 0);
            utp.KeyRequired = _needKeyCheckbox?.IsChecked == true;
            utp.AutoRemoveKey = _removeKeyCheckbox?.IsChecked == true;
            utp.KeyName = _keyEdit?.Text ?? "";

            // Scripts - read from UI controls
            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:323-337
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
            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:340
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:348-350
        // Original: def new(self):
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:352-355
        // Original: def update_item_count(self):
        private void UpdateItemCount()
        {
            if (_inventoryCountLabel != null && _utp != null)
            {
                int count = _utp.Inventory != null ? _utp.Inventory.Count : 0;
                _inventoryCountLabel.Text = $"Total Items: {count}";
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:357-363
        // Original: def change_name(self):
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:365-368
        // Original: def generate_tag(self):
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:370-374
        // Original: def generate_resref(self):
        private void GenerateResref()
        {
            if (_resrefEdit != null)
            {
                _resrefEdit.Text = !string.IsNullOrEmpty(base._resname) ? base._resname : "m00xx_plc_000";
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:376-406
        // Original: def edit_conversation(self):
        private void EditConversation()
        {
            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:378
            // Original: resname = self.ui.conversationEdit.currentText()
            string resname = _conversationEdit?.Text?.Trim() ?? "";
            byte[] data = null;
            string filepath = null;

            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:381-383
            // Original: if not resname or not resname.strip():
            if (string.IsNullOrEmpty(resname))
            {
                var errorBox = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(
                    "Failed to open DLG Editor",
                    "Conversation field cannot be blank.",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                errorBox.ShowAsync();
                return;
            }

            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:385-386
            // Original: assert self._installation is not None
            if (_installation == null)
            {
                return;
            }

            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:386
            // Original: search: ResourceResult | None = self._installation.resource(resname, ResourceType.DLG)
            var search = _installation.Resource(resname, ResourceType.DLG);

            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:387-401
            // Original: if search is None:
            if (search == null)
            {
                // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:388-393
                // Original: msgbox: int = QMessageBox(...).exec()
                var msgBox = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(
                    "DLG file not found",
                    "Do you wish to create a file in the override?",
                    ButtonEnum.YesNo,
                    MsBox.Avalonia.Enums.Icon.Question);
                var result = msgBox.ShowAsync().Result;

                // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:394
                // Original: if QMessageBox.StandardButton.Yes == msgbox:
                if (result == ButtonResult.Yes)
                {
                    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:395-401
                    // Original: data = bytearray(); write_gff(dismantle_dlg(DLG()), data); filepath = ...
                    var dlg = new DLGType();
                    var gff = DLGHelper.DismantleDlg(dlg, _installation.Game);
                    data = GFFAuto.BytesGff(gff, ResourceType.DLG);
                    filepath = System.IO.Path.Combine(_installation.OverridePath(), $"{resname}.dlg");
                    File.WriteAllBytes(filepath, data);
                }
            }
            else
            {
                // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:402-403
                // Original: resname, restype, filepath, data = search
                resname = search.ResName;
                filepath = search.FilePath;
                data = search.Data;
            }

            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:405-406
            // Original: if data is not None: open_resource_editor(...)
            if (data != null)
            {
                WindowUtils.OpenResourceEditor(filepath, resname, ResourceType.DLG, data, _installation, this);
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:408-440
        // Original: def open_inventory(self):
        private void OpenInventory()
        {
            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:418-420
            // Original: if self._installation is None: self.blink_window(); return
            if (_installation == null)
            {
                return;
            }

            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:421-426
            // Original: capsules: list[Capsule] = []; with suppress(Exception): root: str = Module.filepath_to_root(...)
            var capsules = new List<Capsule>();
            try
            {
                // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:423
                // Original: root: str = Module.filepath_to_root(self._filepath)
                string root = null;
                if (!string.IsNullOrEmpty(_filepath))
                {
                    root = Module.FilepathToRoot(_filepath);
                }

                // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:424
                // Original: moduleNames: list[str] = [path for path in self._installation.module_names() if root in path and path != self._filepath]
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

                // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:425
                // Original: newCapsules: list[Capsule] = [Capsule(self._installation.module_path() / mod_filename) for mod_filename in moduleNames]
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
                // Matching PyKotor implementation: suppress(Exception) - ignore errors
            }

            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:428-437
            // Original: inventoryEditor = InventoryEditor(self, self._installation, capsules, [], self._utp.inventory, {}, droid=False, hide_equipment=True)
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

            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/utp.py:438-440
            // Original: if inventoryEditor.exec(): self._utp.inventory = inventoryEditor.inventory; self.update_item_count()
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
