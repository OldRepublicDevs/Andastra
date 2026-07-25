using BioWare.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using BioWare;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.GFF.Generics.UTI;
using BioWare.Resource;
using OdyTools.Common;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Utils;
using OdyTools.Widgets;
using JetBrains.Annotations;
using Game = BioWare.Common.BioWareGame;
using GFFAuto = BioWare.Resource.Formats.GFF.GFFAuto;

namespace OdyTools.Editors
{
    public partial class OdyToolUTI : Editor
    {
        private const int MinEditorWidth = 700;
        private const int MinEditorHeight = 350;
        private const int UndoMaxLevels = 30;

        private UTI _uti;

        private Avalonia.Controls.TextBlock _statusText;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;
        private string _findText = "";
        private bool _findMatchCase;

        // UI Controls - Basic
        private LocalizedStringEdit _nameEdit;
        private LocalizedStringEdit _descEdit;
        private TextBox _tagEdit;
        private Button _tagGenerateBtn;
        private TextBox _resrefEdit;
        private Button _resrefGenerateBtn;
        private ComboBox _baseSelect;
        private NumericUpDown _costSpin;
        private NumericUpDown _additionalCostSpin;
        private NumericUpDown _upgradeSpin;
        private CheckBox _plotCheckbox;
        private NumericUpDown _chargesSpin;
        private NumericUpDown _stackSpin;
        private NumericUpDown _modelVarSpin;
        private NumericUpDown _bodyVarSpin;
        private NumericUpDown _textureVarSpin;
        private Image _iconLabel;

        // UI Controls - Properties
        private TreeView _availablePropertyList;
        private ListBox _assignedPropertiesList;
        private Button _addPropertyBtn;
        private Button _removePropertyBtn;
        private Button _editPropertyBtn;

        // UI Controls - Comments
        private TextBox _commentsEdit;
        private Grid _editorSurface;
        private StackPanel _previewPanel;
        private TextBlock _modelInfoSummary;

        public LocalizedStringEdit NameEdit => _nameEdit;
        public LocalizedStringEdit DescEdit => _descEdit;
        public TextBox TagEdit => _tagEdit;
        public TextBox ResrefEdit => _resrefEdit;
        public ComboBox BaseSelect => _baseSelect;
        // Property to expose ItemCount for testing (matching Python's count())
        public int BaseSelectItemCount
        {
            get
            {
                if (_baseSelect?.Items == null)
                {
                    return 0;
                }
                if (_baseSelect.Items is System.Collections.ICollection collection)
                {
                    return collection.Count;
                }
                // Fallback: count items manually
                int count = 0;
                foreach (var item in _baseSelect.Items)
                {
                    count++;
                }
                return count;
            }
        }
        public NumericUpDown CostSpin => _costSpin;
        public NumericUpDown AdditionalCostSpin => _additionalCostSpin;
        public NumericUpDown UpgradeSpin => _upgradeSpin;
        public CheckBox PlotCheckbox => _plotCheckbox;
        public NumericUpDown ChargesSpin => _chargesSpin;
        public NumericUpDown StackSpin => _stackSpin;
        public NumericUpDown ModelVarSpin => _modelVarSpin;
        public NumericUpDown BodyVarSpin => _bodyVarSpin;
        public NumericUpDown TextureVarSpin => _textureVarSpin;
        public Button TagGenerateBtn => _tagGenerateBtn;
        public Button ResrefGenerateBtn => _resrefGenerateBtn;
        public TreeView AvailablePropertyList => _availablePropertyList;
        // Property to expose ItemCount for testing (matching Python's topLevelItemCount())
        public int AvailablePropertyListItemCount
        {
            get
            {
                if (_availablePropertyList?.Items == null)
                {
                    return 0;
                }
                if (_availablePropertyList.Items is System.Collections.ICollection collection)
                {
                    return collection.Count;
                }
                // Fallback: count items manually
                int count = 0;
                foreach (var item in _availablePropertyList.Items)
                {
                    count++;
                }
                return count;
            }
        }
        public ListBox AssignedPropertiesList => _assignedPropertiesList;
        // Property to expose ItemCount for testing (matching Python's count())
        public int AssignedPropertiesListItemCount
        {
            get
            {
                if (_assignedPropertiesList?.Items == null)
                {
                    return 0;
                }
                if (_assignedPropertiesList.Items is System.Collections.ICollection collection)
                {
                    return collection.Count;
                }
                // Fallback: count items manually
                int count = 0;
                foreach (var item in _assignedPropertiesList.Items)
                {
                    count++;
                }
                return count;
            }
        }
        public Button AddPropertyBtn => _addPropertyBtn;
        public Button RemovePropertyBtn => _removePropertyBtn;
        public Button EditPropertyBtn => _editPropertyBtn;
        public TextBox CommentsEdit => _commentsEdit;
        public Image IconLabel => _iconLabel;
        internal bool HasStructuredEditorSurface => _editorSurface != null && _previewPanel != null;

        public OdyToolUTI() : this(null, null) { }
        public OdyToolUTI(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolUTI", "item",
                new[] { ResourceType.UTI, ResourceType.BTI, ResourceType.UTI_XML },
                new[] { ResourceType.UTI, ResourceType.BTI, ResourceType.UTI_XML },
                installation)
        {
            _installation = installation;
            _uti = new UTI();

            InitializeComponent();
            SetupUI();
            SetupMenuHandlers();
            Opened += (s, e) => { UpdateStatusBar(); _tagEdit?.Focus(); };
            KeyDown += OnWindowKeyDown;
            // SetupInstallation is now called from InitializeComponent after UI is set up
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
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

                // Try to find controls from XAML
                _nameEdit = EditorHelpers.FindControlSafe<LocalizedStringEdit>(this, "nameEdit");
                _descEdit = EditorHelpers.FindControlSafe<LocalizedStringEdit>(this, "descEdit");
                _tagEdit = EditorHelpers.FindControlSafe<TextBox>(this, "tagEdit");
                _tagGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "tagGenerateBtn");
                _resrefEdit = EditorHelpers.FindControlSafe<TextBox>(this, "resrefEdit");
                _resrefGenerateBtn = EditorHelpers.FindControlSafe<Button>(this, "resrefGenerateBtn");
                _baseSelect = EditorHelpers.FindControlSafe<ComboBox>(this, "baseSelect");
                _costSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "costSpin");
                _additionalCostSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "additionalCostSpin");
                _upgradeSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "upgradeSpin");
                _plotCheckbox = EditorHelpers.FindControlSafe<CheckBox>(this, "plotCheckbox");
                _chargesSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "chargesSpin");
                _stackSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "stackSpin");
                _modelVarSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "modelVarSpin");
                _bodyVarSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "bodyVarSpin");
                _textureVarSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "textureVarSpin");
                _iconLabel = EditorHelpers.FindControlSafe<Image>(this, "iconLabel");
                _availablePropertyList = EditorHelpers.FindControlSafe<TreeView>(this, "availablePropertyList");
                _assignedPropertiesList = EditorHelpers.FindControlSafe<ListBox>(this, "assignedPropertiesList");
                _addPropertyBtn = EditorHelpers.FindControlSafe<Button>(this, "addPropertyBtn");
                _removePropertyBtn = EditorHelpers.FindControlSafe<Button>(this, "removePropertyBtn");
                _editPropertyBtn = EditorHelpers.FindControlSafe<Button>(this, "editPropertyBtn");
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
            }
            else
            {
                // XAML loaded, set up signals and commit handlers
                SetupSignals();
                AttachCommitHandlers();
                AttachReferenceSearchMenus();
            }

            // Setup installation after UI is initialized
            if (_installation != null)
            {
                SetupInstallation(_installation);
                // Set installation on LocalizedStringEdit widgets
                if (_nameEdit != null)
                {
                    _nameEdit.SetInstallation(_installation);
                }
                if (_descEdit != null)
                {
                    _descEdit.SetInstallation(_installation);
                }
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
                && _baseSelect != null
                && _availablePropertyList != null
                && _assignedPropertiesList != null;
        }

        private void SetupProgrammaticUI()
        {
            var dock = new DockPanel();
            var menu = BuildMenu();
            DockPanel.SetDock(menu, Dock.Top);
            dock.Children.Add(menu);

            var toolbar = new Border
            {
                Background = Avalonia.Media.Brushes.WhiteSmoke,
                BorderBrush = Avalonia.Media.Brushes.LightGray,
                BorderThickness = new Avalonia.Thickness(0, 0, 0, 1),
                Padding = new Avalonia.Thickness(10, 8)
            };
            DockPanel.SetDock(toolbar, Dock.Top);
            var toolbarGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("Auto,220,Auto,220,*"),
                ColumnSpacing = 8
            };
            toolbarGrid.Children.Add(new TextBlock { Text = "Tag:", VerticalAlignment = VerticalAlignment.Center });
            _tagEdit = new TextBox { Watermark = "item tag", MaxLength = 64 };
            Grid.SetColumn(_tagEdit, 1);
            toolbarGrid.Children.Add(_tagEdit);
            _tagGenerateBtn = new Button { Content = "Use ResRef" };
            Grid.SetColumn(_tagGenerateBtn, 2);
            toolbarGrid.Children.Add(_tagGenerateBtn);
            _resrefEdit = new TextBox { Watermark = "template resref", MaxLength = 16 };
            Grid.SetColumn(_resrefEdit, 3);
            toolbarGrid.Children.Add(_resrefEdit);
            _resrefGenerateBtn = new Button { Content = "Use file name" };
            Grid.SetColumn(_resrefGenerateBtn, 4);
            toolbarGrid.Children.Add(_resrefGenerateBtn);
            toolbar.Child = toolbarGrid;
            dock.Children.Add(toolbar);

            _statusText = new Avalonia.Controls.TextBlock { Name = "statusText", Text = "Item", Margin = new Avalonia.Thickness(10, 4) };
            DockPanel.SetDock(_statusText, Dock.Bottom);
            dock.Children.Add(_statusText);

            _editorSurface = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("320,*,260"),
                Margin = new Avalonia.Thickness(10),
                ColumnSpacing = 10
            };

            var identityPanel = new StackPanel { Spacing = 10 };
            identityPanel.Children.Add(new TextBlock { Text = "Identity", FontWeight = Avalonia.Media.FontWeight.SemiBold, FontSize = 14 });

            _nameEdit = new LocalizedStringEdit();
            if (_installation != null)
            {
                _nameEdit.SetInstallation(_installation);
            }
            identityPanel.Children.Add(new TextBlock { Text = "Name" });
            identityPanel.Children.Add(_nameEdit);

            _descEdit = new LocalizedStringEdit();
            if (_installation != null)
            {
                _descEdit.SetInstallation(_installation);
            }
            identityPanel.Children.Add(new TextBlock { Text = "Description" });
            identityPanel.Children.Add(_descEdit);

            AttachReferenceSearchMenus();

            _baseSelect = new ComboBox();
            EditorHelpers.BindSelectionChanged(_baseSelect, UpdateIcon);
            identityPanel.Children.Add(new TextBlock { Text = "Base item" });
            identityPanel.Children.Add(_baseSelect);

            _iconLabel = new Image
            {
                Width = 96,
                Height = 96,
                Margin = new Avalonia.Thickness(0, 8, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };
            identityPanel.Children.Add(_iconLabel);

            _costSpin = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue };
            _additionalCostSpin = new NumericUpDown { Minimum = 0, Maximum = int.MaxValue };
            _upgradeSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _plotCheckbox = new CheckBox { Content = "Plot" };
            _chargesSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            _stackSpin = new NumericUpDown { Minimum = 0, Maximum = 32767 };

            _modelVarSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            EditorHelpers.BindValueChanged(_modelVarSpin, UpdateIcon);
            _bodyVarSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            EditorHelpers.BindValueChanged(_bodyVarSpin, UpdateIcon);
            _textureVarSpin = new NumericUpDown { Minimum = 0, Maximum = 255 };
            EditorHelpers.BindValueChanged(_textureVarSpin, UpdateIcon);

            var valueGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("Auto,*"),
                RowDefinitions = new RowDefinitions("Auto,Auto,Auto,Auto,Auto,Auto,Auto,Auto,Auto"),
                ColumnSpacing = 8,
                RowSpacing = 6
            };
            AddLabeledControl(valueGrid, 0, "Cost", _costSpin);
            AddLabeledControl(valueGrid, 1, "Additional Cost", _additionalCostSpin);
            AddLabeledControl(valueGrid, 2, "Upgrade Level", _upgradeSpin);
            AddLabeledControl(valueGrid, 3, "Charges", _chargesSpin);
            AddLabeledControl(valueGrid, 4, "Stack Size", _stackSpin);
            AddLabeledControl(valueGrid, 5, "Model Variation", _modelVarSpin);
            AddLabeledControl(valueGrid, 6, "Body Variation", _bodyVarSpin);
            AddLabeledControl(valueGrid, 7, "Texture Variation", _textureVarSpin);
            Grid.SetRow(_plotCheckbox, 8);
            Grid.SetColumn(_plotCheckbox, 1);
            valueGrid.Children.Add(_plotCheckbox);
            identityPanel.Children.Add(valueGrid);

            var identityScroll = new ScrollViewer { Content = identityPanel };
            Grid.SetColumn(identityScroll, 0);
            _editorSurface.Children.Add(identityScroll);

            var propertiesGrid = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto,*,Auto,*,Auto"),
                RowSpacing = 8
            };
            propertiesGrid.Children.Add(new TextBlock { Text = "Item Properties", FontWeight = Avalonia.Media.FontWeight.SemiBold, FontSize = 14 });
            _availablePropertyList = new TreeView();
            Grid.SetRow(_availablePropertyList, 1);
            propertiesGrid.Children.Add(_availablePropertyList);
            _assignedPropertiesList = new ListBox();
            Grid.SetRow(_assignedPropertiesList, 3);
            propertiesGrid.Children.Add(_assignedPropertiesList);
            var assignedLabel = new TextBlock { Text = "Assigned", FontWeight = Avalonia.Media.FontWeight.SemiBold };
            Grid.SetRow(assignedLabel, 2);
            propertiesGrid.Children.Add(assignedLabel);
            var propertyButtonsPanel = new StackPanel { Orientation = Orientation.Horizontal };
            _addPropertyBtn = new Button { Content = "Add" };
            EditorHelpers.BindClick(_addPropertyBtn, AddSelectedProperty);
            _removePropertyBtn = new Button { Content = "Remove" };
            EditorHelpers.BindClick(_removePropertyBtn, RemoveSelectedProperty);
            _editPropertyBtn = new Button { Content = "Edit" };
            EditorHelpers.BindClickAsync(_editPropertyBtn, EditSelectedProperty);
            propertyButtonsPanel.Children.Add(_addPropertyBtn);
            propertyButtonsPanel.Children.Add(_removePropertyBtn);
            propertyButtonsPanel.Children.Add(_editPropertyBtn);
            Grid.SetRow(propertyButtonsPanel, 4);
            propertiesGrid.Children.Add(propertyButtonsPanel);
            Grid.SetColumn(propertiesGrid, 1);
            _editorSurface.Children.Add(propertiesGrid);

            _previewPanel = new StackPanel { Spacing = 10 };
            _previewPanel.Children.Add(new TextBlock { Text = "Preview", FontWeight = Avalonia.Media.FontWeight.SemiBold, FontSize = 14 });
            _modelInfoSummary = new TextBlock { Text = "No installation - icon and model metadata unavailable.", TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            _previewPanel.Children.Add(_modelInfoSummary);
            _previewPanel.Children.Add(new TextBlock { Text = "Comments", FontWeight = Avalonia.Media.FontWeight.SemiBold });
            _commentsEdit = new TextBox { AcceptsReturn = true, AcceptsTab = true };
            _previewPanel.Children.Add(_commentsEdit);
            Grid.SetColumn(_previewPanel, 2);
            _editorSurface.Children.Add(_previewPanel);

            dock.Children.Add(_editorSurface);
            Content = dock;
            SetupSignals();
            AttachCommitHandlers();
        }

        private static void AddLabeledControl(Grid grid, int row, string label, Control control)
        {
            var text = new TextBlock { Text = label, VerticalAlignment = VerticalAlignment.Center };
            Grid.SetRow(text, row);
            Grid.SetColumn(text, 0);
            grid.Children.Add(text);
            Grid.SetRow(control, row);
            Grid.SetColumn(control, 1);
            grid.Children.Add(control);
        }

        private void SetupUI()
        {
            if (_statusText == null)
                _statusText = EditorHelpers.FindControlSafe<Avalonia.Controls.TextBlock>(this, "statusText");
        }

        private void AttachCommitHandlers()
        {
            void OnCommit(object s, EventArgs e) { if (!_undoRedoInProgress) PushState(); }
            EditorHelpers.BindLostFocus(_tagEdit, OnCommit);
            EditorHelpers.BindLostFocus(_resrefEdit, OnCommit);
            EditorHelpers.BindLostFocus(_commentsEdit, OnCommit);
            EditorHelpers.BindLostFocus(_costSpin, OnCommit);
            EditorHelpers.BindLostFocus(_additionalCostSpin, OnCommit);
            EditorHelpers.BindLostFocus(_upgradeSpin, OnCommit);
            EditorHelpers.BindLostFocus(_chargesSpin, OnCommit);
            EditorHelpers.BindLostFocus(_stackSpin, OnCommit);
            EditorHelpers.BindLostFocus(_modelVarSpin, OnCommit);
            EditorHelpers.BindLostFocus(_bodyVarSpin, OnCommit);
            EditorHelpers.BindLostFocus(_textureVarSpin, OnCommit);
            EditorHelpers.BindLostFocus(_plotCheckbox, OnCommit);
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
                _uti = new UTI();
                LoadUTI(_uti);
            }
            else
            {
                try
                {
                    var gff = GFFAuto.ReadGff(data, fileFormat: _restype ?? ResourceType.UTI);
                    _uti = UTIHelpers.ConstructUti(gff);
                    LoadUTI(_uti);
                }
                catch
                {
                    _uti = new UTI();
                    LoadUTI(_uti);
                }
            }
            _undoRedoInProgress = true;
            try { UpdateStatusBar(); }
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
                string text = _uti == null ? "Item" : (_uti.Tag ?? "Item");
                if (!string.IsNullOrEmpty(_uti?.ResRef?.ToString())) text += " | " + _uti.ResRef;
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
            if (Match(_commentsEdit?.Text) && _commentsEdit != null) { _commentsEdit.Focus(); return; }
        }

        private void OnWindowKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.S && (e.KeyModifiers & KeyModifiers.Control) != 0) { Save(); e.Handled = true; return; }
            if (e.Key == Key.Z && (e.KeyModifiers & KeyModifiers.Control) != 0) { Undo(); e.Handled = true; return; }
            if (e.Key == Key.Y && (e.KeyModifiers & KeyModifiers.Control) != 0) { Redo(); e.Handled = true; return; }
            if (e.Key == Key.F && (e.KeyModifiers & KeyModifiers.Control) != 0) { ShowFindDialog(); e.Handled = true; return; }
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
                System.Console.WriteLine($"Failed to load UTI: {ex}");
                New();
            }
        }

        private void SetupSignals()
        {
            EditorHelpers.BindClick(_tagGenerateBtn, GenerateTag);
            EditorHelpers.BindClick(_resrefGenerateBtn, GenerateResref);
            EditorHelpers.BindClickAsync(_editPropertyBtn, EditSelectedProperty);
            EditorHelpers.BindClick(_removePropertyBtn, RemoveSelectedProperty);
            EditorHelpers.BindClick(_addPropertyBtn, AddSelectedProperty);
            EditorHelpers.BindValueChanged(_modelVarSpin, UpdateIcon);
            EditorHelpers.BindValueChanged(_bodyVarSpin, UpdateIcon);
            EditorHelpers.BindValueChanged(_textureVarSpin, UpdateIcon);
            EditorHelpers.BindSelectionChanged(_baseSelect, UpdateIcon);
            // Note: Name and Description editing is handled by LocalizedStringEdit's built-in edit button

            if (_availablePropertyList != null)
            {
                _availablePropertyList.DoubleTapped += (s, e) => OnAvailablePropertyListDoubleClicked();
                _availablePropertyList.SelectionChanged += (s, e) => UpdatePropertyButtonsState();
            }
            if (_assignedPropertiesList != null)
            {
                _assignedPropertiesList.DoubleTapped += (s, e) => OnAssignedPropertyListDoubleClicked();
                _assignedPropertiesList.SelectionChanged += (s, e) => UpdatePropertyButtonsState();
            }

            // Note: In Avalonia, we handle KeyDown event instead of QShortcut
            this.KeyDown += (s, e) =>
            {
                if (e.Key == Avalonia.Input.Key.Delete && _assignedPropertiesList != null && _assignedPropertiesList.IsFocused)
                {
                    OnDelShortcut();
                    e.Handled = true;
                }
            };

            UpdatePropertyButtonsState();
        }

        private void LoadUTI(UTI uti)
        {
            _uti = uti;

            // Basic
            if (_nameEdit != null)
            {
                _nameEdit.SetLocString(uti.Name);
            }
            if (_descEdit != null)
            {
                _descEdit.SetLocString(uti.Description);
            }
            if (_tagEdit != null)
            {
                _tagEdit.Text = uti.Tag;
            }
            if (_resrefEdit != null)
            {
                _resrefEdit.Text = uti.ResRef.ToString();
            }
            if (_baseSelect != null)
            {
                _baseSelect.SelectedIndex = uti.BaseItem;
            }
            if (_costSpin != null)
            {
                _costSpin.Value = uti.Cost;
            }
            if (_additionalCostSpin != null)
            {
                _additionalCostSpin.Value = uti.AddCost;
            }
            if (_upgradeSpin != null)
            {
                _upgradeSpin.Value = uti.UpgradeLevel;
            }
            if (_plotCheckbox != null)
            {
                _plotCheckbox.IsChecked = uti.Plot != 0;
            }
            if (_chargesSpin != null)
            {
                _chargesSpin.Value = uti.Charges;
            }
            if (_stackSpin != null)
            {
                _stackSpin.Value = uti.StackSize;
            }
            if (_modelVarSpin != null)
            {
                _modelVarSpin.Value = uti.ModelVariation;
            }
            if (_bodyVarSpin != null)
            {
                _bodyVarSpin.Value = uti.BodyVariation;
            }
            if (_textureVarSpin != null)
            {
                _textureVarSpin.Value = uti.TextureVariation;
            }

            // Properties
            if (_assignedPropertiesList != null)
            {
                _assignedPropertiesList.Items.Clear();
                if (uti.Properties != null)
                {
                    foreach (var prop in uti.Properties)
                    {
                        string summary = PropertySummary(prop);
                        _assignedPropertiesList.Items.Add(new PropertyListItem { Text = summary, Property = prop });
                    }
                }
            }

            // Comments
            if (_commentsEdit != null)
            {
                _commentsEdit.Text = uti.Comment;
            }

            // Update icon display after loading UTI data
            UpdateIcon();
            UpdatePropertyButtonsState();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            // Since C# 7.3 doesn't have deepcopy, manually copy the UTI
            var uti = CopyUTI(_uti);

            // Basic - read from UI controls (matching Python which always reads from UI)
            if (_nameEdit != null)
            {
                uti.Name = _nameEdit.GetLocString();
            }
            if (_descEdit != null)
            {
                uti.Description = _descEdit.GetLocString();
            }
            uti.Tag = _tagEdit?.Text ?? uti.Tag ?? "";
            if (_resrefEdit != null)
            {
                uti.ResRef = ResRefFromText(_resrefEdit.Text);
            }
            uti.BaseItem = _baseSelect?.SelectedIndex ?? uti.BaseItem;
            uti.Cost = _costSpin?.Value != null ? (int)_costSpin.Value : uti.Cost;
            uti.AddCost = _additionalCostSpin?.Value != null ? (int)_additionalCostSpin.Value : uti.AddCost;
            uti.UpgradeLevel = _upgradeSpin?.Value != null ? (int)_upgradeSpin.Value : uti.UpgradeLevel;
            uti.Plot = (_plotCheckbox?.IsChecked ?? (uti.Plot != 0)) ? 1 : 0;
            uti.Charges = _chargesSpin?.Value != null ? (int)_chargesSpin.Value : uti.Charges;
            uti.StackSize = _stackSpin?.Value != null ? (int)_stackSpin.Value : uti.StackSize;
            uti.ModelVariation = _modelVarSpin?.Value != null ? (int)_modelVarSpin.Value : uti.ModelVariation;
            uti.BodyVariation = _bodyVarSpin?.Value != null ? (int)_bodyVarSpin.Value : uti.BodyVariation;
            uti.TextureVariation = _textureVarSpin?.Value != null ? (int)_textureVarSpin.Value : uti.TextureVariation;

            // Properties - read from UI list
            uti.Properties.Clear();
            if (_assignedPropertiesList?.Items != null)
            {
                foreach (var item in _assignedPropertiesList.Items)
                {
                    if (item is PropertyListItem propItem && propItem.Property != null)
                    {
                        // Create a deep copy of the property to avoid reference issues
                        var propCopy = new UTIProperty
                        {
                            PropertyName = propItem.Property.PropertyName,
                            Subtype = propItem.Property.Subtype,
                            CostTable = propItem.Property.CostTable,
                            CostValue = propItem.Property.CostValue,
                            Param1 = propItem.Property.Param1,
                            Param1Value = propItem.Property.Param1Value,
                            ChanceAppear = propItem.Property.ChanceAppear,
                            UpgradeType = propItem.Property.UpgradeType
                        };
                        uti.Properties.Add(propCopy);
                    }
                }
            }

            // Comments
            uti.Comment = _commentsEdit?.Text ?? "";

            // Build GFF
            Game game = _installation?.Game ?? Game.K2;
            var gff = UTIHelpers.DismantleUti(uti, game);
            ResourceType outputType = _restype == ResourceType.UTI_XML
                ? ResourceType.UTI_XML
                : (_restype == ResourceType.BTI ? ResourceType.BTI : ResourceType.UTI);
            if (outputType == ResourceType.BTI)
            {
                gff.Content = GFFContent.BTI;
            }
            byte[] data = GFFAuto.BytesGff(gff, outputType);
            return Tuple.Create(data, new byte[0]);
        }

        private static ResRef ResRefFromText(string text)
        {
            string value = (text ?? string.Empty).Trim();
            return !string.IsNullOrEmpty(value) ? new ResRef(value) : ResRef.FromBlank();
        }

        private UTI CopyUTI(UTI source)
        {
            // Deep copy LocalizedString objects (they're reference types)
            LocalizedString copyName = source.Name != null
                ? new LocalizedString(source.Name.StringRef, new Dictionary<int, string>(GetSubstringsDict(source.Name)))
                : null;
            LocalizedString copyDesc = source.Description != null
                ? new LocalizedString(source.Description.StringRef, new Dictionary<int, string>(GetSubstringsDict(source.Description)))
                : null;
            LocalizedString copyDescUnid = source.DescriptionUnidentified != null
                ? new LocalizedString(source.DescriptionUnidentified.StringRef, new Dictionary<int, string>(GetSubstringsDict(source.DescriptionUnidentified)))
                : null;

            var copy = new UTI
            {
                ResRef = source.ResRef,
                BaseItem = source.BaseItem,
                Name = copyName,
                Description = copyDesc,
                DescriptionUnidentified = copyDescUnid,
                Cost = source.Cost,
                StackSize = source.StackSize,
                Charges = source.Charges,
                Plot = source.Plot,
                AddCost = source.AddCost,
                Stolen = source.Stolen,
                Identified = source.Identified,
                ItemType = source.ItemType,
                BaseItemType = source.BaseItemType,
                UpgradeLevel = source.UpgradeLevel,
                BodyVariation = source.BodyVariation,
                TextureVariation = source.TextureVariation,
                ModelVariation = source.ModelVariation,
                PaletteId = source.PaletteId,
                Comment = source.Comment,
                Tag = source.Tag
            };

            // Copy properties
            foreach (var prop in source.Properties)
            {
                copy.Properties.Add(new UTIProperty
                {
                    PropertyName = prop.PropertyName,
                    Subtype = prop.Subtype,
                    CostTable = prop.CostTable,
                    CostValue = prop.CostValue,
                    Param1 = prop.Param1,
                    Param1Value = prop.Param1Value,
                    ChanceAppear = prop.ChanceAppear,
                    UpgradeType = prop.UpgradeType
                });
            }

            // Copy upgrades
            foreach (var upgrade in source.Upgrades)
            {
                copy.Upgrades.Add(new UTIUpgrade
                {
                    Upgrade = upgrade.Upgrade,
                    Name = upgrade.Name,
                    Description = upgrade.Description
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
            _undoStack.Clear();
            _redoStack.Clear();
            _uti = new UTI();
            LoadUTI(_uti);
            UpdateStatusBar();
        }

        // Note: Name editing is now handled by LocalizedStringEdit's built-in edit button
        // The widget opens LocalizedStringDialog internally, so this method is no longer needed
        // However, we keep it for backwards compatibility if needed elsewhere
        private void EditName()
        {
            // LocalizedStringEdit handles editing internally via its edit button
            // This method is kept for compatibility but is no longer called
        }

        // Note: Description editing is now handled by LocalizedStringEdit's built-in edit button
        // The widget opens LocalizedStringDialog internally, so this method is no longer needed
        // However, we keep it for backwards compatibility if needed elsewhere
        private void EditDescription()
        {
            // LocalizedStringEdit handles editing internally via its edit button
            // This method is kept for compatibility but is no longer called
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
                _resrefEdit.Text = !string.IsNullOrEmpty(base._resname) ? base._resname : "m00xx_itm_000";
            }
            MarkDocumentDirty();
        }

        private async System.Threading.Tasks.Task EditSelectedProperty()
        {
            if (_assignedPropertiesList?.SelectedItem == null)
            {
                return;
            }

            if (!(_assignedPropertiesList.SelectedItem is PropertyListItem selectedItem) || selectedItem.Property == null)
            {
                return;
            }

            var dialog = new PropertyEditorDialog(this, _installation, selectedItem.Property);

            // Use ShowDialogAsync for proper modal dialog handling
            var resultObj = await dialog.ShowDialogAsync(this);
            bool result = resultObj is bool b ? b : false;
            if (!result)
            {
                return;
            }

            UTIProperty updatedProperty = dialog.GetUtiProperty();
            selectedItem.Property = updatedProperty;
            selectedItem.Text = PropertySummary(updatedProperty);
            UpdatePropertyButtonsState();
            MarkDocumentDirty();
        }

        private void AddSelectedProperty()
        {
            if (_availablePropertyList?.SelectedItem == null)
            {
                return;
            }

            if (_availablePropertyList.SelectedItem is TreeViewItem selectedItem && selectedItem.Tag is PropertyTreeItemData itemData)
            {
                int propertyId = itemData.PropertyIndex;
                int subtypeId = itemData.SubPropertyIndex;
                AddPropertyMain(propertyId, subtypeId);
            }
        }

        private void AddPropertyMain(int propertyId, int subtypeId)
        {
            if (_installation == null)
            {
                return;
            }

            TwoDA itemProps = _installation.HtGetCache2DA(OdyInstallation.TwoDAItemProperties);
            if (itemProps == null)
            {
                return;
            }

            var utiProperty = new UTIProperty();
            utiProperty.PropertyName = propertyId;
            utiProperty.Subtype = subtypeId;

            TwoDARow propertyRow = itemProps.GetRow(propertyId);
            int? costTableNullable = propertyRow.GetInteger("costtableresref", 255);
            utiProperty.CostTable = costTableNullable ?? 255;

            utiProperty.CostValue = 0;

            int? param1Nullable = propertyRow.GetInteger("param1resref", 255);
            utiProperty.Param1 = param1Nullable ?? 255;

            utiProperty.Param1Value = 0;

            utiProperty.ChanceAppear = 100;

            string text = PropertySummary(utiProperty);

            var listItem = new PropertyListItem { Text = text, Property = utiProperty };
            if (_assignedPropertiesList != null)
            {
                _assignedPropertiesList.Items.Add(listItem);
                _assignedPropertiesList.SelectedItem = listItem;
                MarkDocumentDirty();
            }
            UpdatePropertyButtonsState();
        }

        private void RemoveSelectedProperty()
        {
            if (_assignedPropertiesList?.SelectedItem != null)
            {
                _assignedPropertiesList.Items.Remove(_assignedPropertiesList.SelectedItem);
                MarkDocumentDirty();
            }
            UpdatePropertyButtonsState();
        }

        private void UpdatePropertyButtonsState()
        {
            if (_addPropertyBtn != null)
            {
                _addPropertyBtn.IsEnabled = _installation != null && IsSelectedAvailablePropertyLeaf();
            }

            bool hasAssignedProperty = HasSelectedAssignedProperty();
            if (_removePropertyBtn != null)
            {
                _removePropertyBtn.IsEnabled = hasAssignedProperty;
            }
            if (_editPropertyBtn != null)
            {
                _editPropertyBtn.IsEnabled = hasAssignedProperty;
            }
        }

        private bool IsSelectedAvailablePropertyLeaf()
        {
            if (!(_availablePropertyList?.SelectedItem is TreeViewItem selectedItem) ||
                !(selectedItem.Tag is PropertyTreeItemData))
            {
                return false;
            }

            if (selectedItem.ItemsSource is System.Collections.ICollection collection)
            {
                return collection.Count == 0;
            }

            return selectedItem.ItemsSource == null;
        }

        private bool HasSelectedAssignedProperty()
        {
            return _assignedPropertiesList?.SelectedItem is PropertyListItem selectedItem &&
                   selectedItem.Property != null;
        }

        private string PropertySummary(UTIProperty prop)
        {
            if (_installation == null)
            {
                return $"Property {prop.PropertyName}: Subtype {prop.Subtype}";
            }

            string propName = GetPropertyName(_installation, prop.PropertyName);

            string subpropName = GetSubpropertyName(_installation, prop.PropertyName, prop.Subtype);

            string costName = CostName(_installation, prop.CostTable, prop.CostValue);

            if (!string.IsNullOrEmpty(costName) && !string.IsNullOrEmpty(subpropName))
            {
                return $"{propName}: {subpropName} [{costName}]";
            }

            if (!string.IsNullOrEmpty(subpropName))
            {
                return $"{propName}: {subpropName}";
            }

            if (!string.IsNullOrEmpty(costName))
            {
                return $"{propName}: [{costName}]";
            }

            return propName;
        }

        public static string CostName(OdyInstallation installation, int cost, int value)
        {
            TwoDA costTableList = installation.HtGetCache2DA(OdyInstallation.TwoDAIprpCosttable);
            if (costTableList == null)
            {
                System.Console.WriteLine("Failed to retrieve IPRP_COSTTABLE 2DA.");
                return null;
            }

            string costtableName = costTableList.GetCellString(cost, "name");
            if (string.IsNullOrEmpty(costtableName))
            {
                System.Console.WriteLine($"Failed to retrieve costtable 'name' for cost '{cost}'.");
                return null;
            }

            TwoDA costtable = installation.HtGetCache2DA(costtableName);
            if (costtable == null)
            {
                System.Console.WriteLine($"Failed to retrieve '{costtableName}' 2DA.");
                return null;
            }

            try
            {
                TwoDARow row = costtable.GetRow(value);
                int? stringref = row.GetInteger("name");
                if (stringref.HasValue)
                {
                    return installation.GetStringFromStringRef(stringref.Value);
                }
            }
            catch (Exception)
            {
                System.Console.WriteLine("Could not get the costtable 2DA row/value");
            }
            return null;
        }

        public static string ParamName(OdyInstallation installation, int paramtable, int param)
        {
            // Get the IPRP_PARAMTABLE TwoDA
            TwoDA paramtableList = installation.HtGetCache2DA(OdyInstallation.TwoDAIprpParamtable);
            if (paramtableList == null)
            {
                System.Console.WriteLine("Failed to retrieve IPRP_PARAMTABLE 2DA.");
                return null;
            }

            try
            {
                // Get the specific parameter table TwoDA
                string tableResref = paramtableList.GetCellString(paramtable, "tableresref");
                if (string.IsNullOrEmpty(tableResref))
                {
                    System.Console.WriteLine($"Failed to retrieve table_resref for paramtable: '{paramtable}'.");
                    return null;
                }

                TwoDA paramtable2da = installation.HtGetCache2DA(tableResref);
                if (paramtable2da == null)
                {
                    System.Console.WriteLine($"Failed to retrieve 2DA file: {tableResref}.");
                    return null;
                }

                // Get the string reference for the parameter name
                TwoDARow paramRow = paramtable2da.GetRow(param);
                int? stringref = paramRow.GetInteger("name");
                if (stringref.HasValue)
                {
                    return installation.GetStringFromStringRef(stringref.Value);
                }
                else
                {
                    System.Console.WriteLine($"Failed to get 'name' value for param '{param}' in '{tableResref}'");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Exception getting param name: {ex.Message}");
            }
            return null;
        }

        private void UpdateIcon()
        {
            if (_installation == null)
            {
                return;
            }

            int baseItem = _baseSelect?.SelectedIndex ?? 0;

            int modelVariation = _modelVarSpin?.Value != null ? (int)_modelVarSpin.Value : 0;

            int textureVariation = _textureVarSpin?.Value != null ? (int)_textureVarSpin.Value : 0;

            var bitmap = _installation.GetItemIcon(baseItem, modelVariation, textureVariation);

            if (bitmap != null && _iconLabel != null)
            {
                _iconLabel.Source = bitmap;
            }

            if (_iconLabel != null)
            {
                string tooltip = GenerateIconTooltip(true);
                Avalonia.Controls.ToolTip.SetTip(_iconLabel, tooltip);
            }
        }

        private string GenerateIconTooltip(bool asHtml = false)
        {
            if (_installation == null)
            {
                return "";
            }

            int baseItem = _baseSelect?.SelectedIndex ?? 0;

            int modelVariation = _modelVarSpin?.Value != null ? (int)_modelVarSpin.Value : 0;

            int textureVariation = _textureVarSpin?.Value != null ? (int)_textureVarSpin.Value : 0;

            string baseItemName = _installation.GetItemBaseName(baseItem);

            string modelVarName = _installation.GetModelVarName(modelVariation);

            string textureVarName = _installation.GetTextureVarName(textureVariation);

            string iconPath = _installation.GetItemIconPath(baseItem, modelVariation, textureVariation);

            if (asHtml)
            {
                return $"<b>Base Item:</b> {baseItemName} (ID: {baseItem})<br>" +
                       $"<b>Model Variation:</b> {modelVarName} (ID: {modelVariation})<br>" +
                       $"<b>Texture Variation:</b> {textureVarName} (ID: {textureVariation})<br>" +
                       $"<b>Icon Name:</b> {iconPath}";
            }
            else
            {
                return $"Base Item: {baseItemName} (ID: {baseItem})\n" +
                       $"Model Variation: {modelVarName} (ID: {modelVariation})\n" +
                       $"Texture Variation: {textureVarName} (ID: {textureVariation})\n" +
                       $"Icon Name: {iconPath}";
            }
        }

        private void SetupInstallation(OdyInstallation installation)
        {
            if (installation == null)
            {
                return;
            }

            _installation = installation;

            // Set installation on LocalizedStringEdit widgets
            if (_nameEdit != null)
            {
                _nameEdit.SetInstallation(installation);
            }
            if (_descEdit != null)
            {
                _descEdit.SetInstallation(installation);
            }

            var required = new List<string> { OdyInstallation.TwoDABaseitems, OdyInstallation.TwoDAItemProperties };
            installation.HtBatchCache2DA(required);

            TwoDA baseitems = installation.HtGetCache2DA(OdyInstallation.TwoDABaseitems);
            if (baseitems == null)
            {
                System.Console.WriteLine("Failed to retrieve BASEITEMS 2DA.");
            }
            else
            {
                if (_baseSelect != null)
                {
                    _baseSelect.Items.Clear();
                    for (int i = 0; i < baseitems.GetHeight(); i++)
                    {
                        string label = baseitems.GetCellString(i, "label") ?? "";
                        _baseSelect.Items.Add(label);
                    }
                }
            }

            if (_availablePropertyList == null)
            {
                System.Console.WriteLine("AvailablePropertyList is null - cannot populate properties");
                return;
            }

            _availablePropertyList.Items.Clear();

            TwoDA itemProperties = installation.HtGetCache2DA(OdyInstallation.TwoDAItemProperties);
            if (itemProperties == null)
            {
                System.Console.WriteLine("Failed to retrieve ITEM_PROPERTIES 2DA.");
                return;
            }

            if (itemProperties != null)
            {
                for (int i = 0; i < itemProperties.GetHeight(); i++)
                {
                    string propName = GetPropertyName(installation, i);

                    var item = new TreeViewItem
                    {
                        Header = propName
                    };
                    // Store property index and subproperty index in Tag (using a simple object)
                    item.Tag = new PropertyTreeItemData { PropertyIndex = i, SubPropertyIndex = i };

                    string subtypeResname = itemProperties.GetCellString(i, "subtyperesref") ?? "";
                    if (string.IsNullOrEmpty(subtypeResname))
                    {
                        // No subtype, just add the item
                        if (_availablePropertyList != null)
                        {
                            _availablePropertyList.Items.Add(item);
                        }
                        continue;
                    }

                    TwoDA subtype = installation.HtGetCache2DA(subtypeResname);
                    if (subtype == null)
                    {
                        System.Console.WriteLine($"Failed to retrieve subtype '{subtypeResname}' for property name '{propName}' at index {i}. Skipping...");
                        if (_availablePropertyList != null)
                        {
                            _availablePropertyList.Items.Add(item);
                        }
                        continue;
                    }

                    var childItems = new List<TreeViewItem>();
                    for (int j = 0; j < subtype.GetHeight(); j++)
                    {
                        string name = GetSubpropertyName(installation, i, j);
                        if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
                        {
                            continue;
                        }

                        var child = new TreeViewItem
                        {
                            Header = name
                        };
                        child.Tag = new PropertyTreeItemData { PropertyIndex = i, SubPropertyIndex = j };
                        childItems.Add(child);
                    }
                    item.ItemsSource = childItems;
                    if (_availablePropertyList != null)
                    {
                        _availablePropertyList.Items.Add(item);
                    }
                }
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
            if (_descEdit != null)
            {
                _descEdit.SetInstallation(null);
            }
        }

        // Helper class to store property tree item data
        private class PropertyTreeItemData
        {
            public int PropertyIndex { get; set; }
            public int SubPropertyIndex { get; set; }
        }

        public static string GetPropertyName(OdyInstallation installation, int prop)
        {
            TwoDA properties = installation.HtGetCache2DA(OdyInstallation.TwoDAItemProperties);
            if (properties == null)
            {
                System.Console.WriteLine("Failed to retrieve ITEM_PROPERTIES 2DA.");
                return "Unknown";
            }

            TwoDARow row = properties.GetRow(prop);
            int? stringrefNullable = row.GetInteger("name");
            if (!stringrefNullable.HasValue)
            {
                System.Console.WriteLine($"Failed to retrieve name stringref for property {prop}.");
                return "Unknown";
            }
            int stringref = stringrefNullable.Value;

            return installation.GetStringFromStringRef(stringref);
        }

        [CanBeNull]
        public static string GetSubpropertyName(OdyInstallation installation, int prop, int subprop)
        {
            TwoDA properties = installation.HtGetCache2DA(OdyInstallation.TwoDAItemProperties);
            if (properties == null)
            {
                System.Console.WriteLine("Failed to retrieve ITEM_PROPERTIES 2DA.");
                return null;
            }

            TwoDARow propRow = properties.GetRow(prop);
            string subtypeResname = propRow.GetString("subtyperesref") ?? "";
            if (string.IsNullOrEmpty(subtypeResname))
            {
                System.Console.WriteLine($"Failed to retrieve subtype_resname for property {prop}.");
                return null;
            }

            TwoDA subproperties = installation.HtGetCache2DA(subtypeResname);
            if (subproperties == null)
            {
                return null;
            }

            string headerStrref = subproperties.GetHeaders().Contains("name") ? "name" : "string_ref";

            TwoDARow subpropRow = subproperties.GetRow(subprop);
            int? nameStrrefNullable = subpropRow.GetInteger(headerStrref);
            if (nameStrrefNullable.HasValue)
            {
                return installation.GetStringFromStringRef(nameStrrefNullable.Value);
            }

            return subpropRow.GetString("label") ?? "";
        }

        // Helper class for property list items
        private class PropertyListItem
        {
            public string Text { get; set; }
            public UTIProperty Property { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void OnAvailablePropertyListDoubleClicked()
        {
            if (_availablePropertyList?.SelectedItem is TreeViewItem selectedItem)
            {
                // Check if it's a leaf node (no children)
                bool isLeafNode = selectedItem.ItemsSource == null ||
                                  (selectedItem.ItemsSource is System.Collections.IList list && list.Count == 0);

                if (isLeafNode)
                {
                    AddSelectedProperty();
                }
            }
        }

        private async void OnAssignedPropertyListDoubleClicked()
        {
            await EditSelectedProperty();
        }

        private void OnDelShortcut()
        {
            if (_assignedPropertiesList != null && _assignedPropertiesList.IsFocused)
            {
                RemoveSelectedProperty();
            }
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }
    }
}
