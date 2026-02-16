using BioWare.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.VisualTree;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using System.Numerics;
using BioWare.Resource.Formats.GFF;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using OdyTools.Common;
using OdyTools.Data;

namespace OdyTools.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/gff.py:47
    // Original: class OdyToolGFF(Editor):
    public partial class OdyToolGFF : Editor
    {
        private const int MinEditorWidth = 400;
        private const int MinEditorHeight = 250;
        private const int UndoMaxLevels = 30;

        private GFF _gff;
        private TreeView _treeView;
        private Panel _fieldBox;
        private ComboBox _typeCombo;
        private TextBox _labelEdit;
        private Panel _pages;
        private NumericUpDown _intSpin;
        private NumericUpDown _floatSpin;
        private TextBox _lineEdit;
        private TextBox _textEdit;
        private NumericUpDown _xVec3Spin;
        private NumericUpDown _yVec3Spin;
        private NumericUpDown _zVec3Spin;
        private NumericUpDown _xVec4Spin;
        private NumericUpDown _yVec4Spin;
        private NumericUpDown _zVec4Spin;
        private NumericUpDown _wVec4Spin;
        private NumericUpDown _stringrefSpin;
        private ListBox _substringList;
        private Button _addSubstringButton;
        private Button _removeSubstringButton;
        private TextBox _substringEdit;
        private ComboBox _substringLangCombo;
        private ComboBox _substringGenderCombo;
        private TextBox _tlkTextEdit;
        private ContentControl _pagesControl;
        private Panel _intPage;
        private Panel _floatPage;
        private Panel _linePage;
        private Panel _textPage;
        private Panel _vector3Page;
        private Panel _vector4Page;
        private Panel _substringPage;
        private Panel _blankPage;
        private TextBlock _binaryHexLabel;
        private Button _copyBinaryButton;
        private Button _convertBinaryButton;
        private GFFTreeNodeViewModel _selectedNode;
        private ObservableCollection<SubstringListItem> _substringItems;
        private TextBlock _statusText;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();
        private bool _undoRedoInProgress;
        private string _findQuery = "";
        private string _replaceText = "";
        private bool _findMatchCase;
        private bool _findInLabels = true;
        private bool _findInValues = true;
        private bool _replaceInLabels = true;
        private bool _replaceInValues = true;
        private int _findStartIndex;
        private List<GFFTreeNodeViewModel> _findMatches = new List<GFFTreeNodeViewModel>();
        private GFFTreeNodeViewModel _copiedNode;
        // ProperTree-style drag-and-drop reorder
        private GFFTreeNodeViewModel _dragSourceNode;
        private Avalonia.Point _dragStartPos;
        private bool _isDragging;
        // Tiled-style tree zoom (font scale)
        private ComboBox _zoomCombo;
        private Grid _mainGrid;
        private static readonly double[] TreeZoomFactors = { 0.5, 0.75, 1.0, 1.25, 1.5, 2.0 };
        /// <summary>Track the last opened context menu so we can close it before opening another (prevents overlapping menus).</summary>
        private ContextMenu _openContextMenu;
        private const double DefaultTreeFontSize = 12;
        private int _zoomIndex = 2; // 1.0 = 100%
        private static readonly IBrush PropertyPanelForeground = new SolidColorBrush(Avalonia.Media.Color.FromRgb(0x21, 0x21, 0x21));

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/gff.py:48-81
        // Original: def __init__(self, parent, installation):
        public OdyToolGFF(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolGFF", "none",
                GetSupportedTypes(),
                GetSupportedTypes(),
                installation)
        {
            InitializeComponent();
            SetupUI();
            SetupSignals();
            SetupMenuHandlers();
            MinWidth = MinEditorWidth;
            MinHeight = MinEditorHeight;
            Width = 668;
            Height = 486;
            New();
        }

        private static ResourceType[] GetSupportedTypes()
        {
            return new[]
            {
                ResourceType.ARE,
                ResourceType.DLG,
                ResourceType.GFF_XML,
                ResourceType.GFF,
                ResourceType.GIT,
                ResourceType.IFO,
                ResourceType.JRL,
                ResourceType.PTH,
                ResourceType.UTC,
                ResourceType.UTD,
                ResourceType.UTE,
                ResourceType.UTI,
                ResourceType.UTM,
                ResourceType.UTP,
                ResourceType.UTS,
                ResourceType.UTT,
                ResourceType.UTW
            };
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
            var mainSplitter = new Grid();
            mainSplitter.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            mainSplitter.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            _treeView = new TreeView();
            Grid.SetColumn(_treeView, 0);
            mainSplitter.Children.Add(_treeView);

            var rightPanel = new StackPanel();
            Grid.SetColumn(rightPanel, 1);
            mainSplitter.Children.Add(rightPanel);

            _fieldBox = new StackPanel { IsEnabled = false };
            _labelEdit = new TextBox { Watermark = "Label", MaxLength = 16, Foreground = PropertyPanelForeground };
            _typeCombo = new ComboBox { Foreground = PropertyPanelForeground };
            _typeCombo.ItemsSource = Enum.GetValues(typeof(GFFFieldType)).Cast<GFFFieldType>().Select(t => t.ToString()).ToList();

            _intSpin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _intPage = new StackPanel();
            _intPage.Children.Add(new TextBlock { Text = "Value:", Foreground = PropertyPanelForeground });
            _intPage.Children.Add(_intSpin);

            _floatSpin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _floatPage = new StackPanel();
            _floatPage.Children.Add(new TextBlock { Text = "Value:", Foreground = PropertyPanelForeground });
            _floatPage.Children.Add(_floatSpin);

            _lineEdit = new TextBox { Foreground = PropertyPanelForeground, MaxLength = 16 };
            _linePage = new StackPanel();
            _linePage.Children.Add(new TextBlock { Text = "ResRef:", Foreground = PropertyPanelForeground });
            _linePage.Children.Add(_lineEdit);

            _textEdit = new TextBox { AcceptsReturn = true, Foreground = PropertyPanelForeground };
            _textPage = new StackPanel();
            _textPage.Children.Add(new TextBlock { Text = "Text:", Foreground = PropertyPanelForeground });
            _textPage.Children.Add(_textEdit);

            var vec3Panel = new StackPanel { Orientation = Orientation.Horizontal };
            _xVec3Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _yVec3Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _zVec3Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            vec3Panel.Children.Add(_xVec3Spin);
            vec3Panel.Children.Add(_yVec3Spin);
            vec3Panel.Children.Add(_zVec3Spin);
            _vector3Page = new StackPanel();
            _vector3Page.Children.Add(new TextBlock { Text = "X, Y, Z:", Foreground = PropertyPanelForeground });
            _vector3Page.Children.Add(vec3Panel);

            var vec4Panel = new StackPanel { Orientation = Orientation.Horizontal };
            _xVec4Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _yVec4Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _zVec4Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _wVec4Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            vec4Panel.Children.Add(_xVec4Spin);
            vec4Panel.Children.Add(_yVec4Spin);
            vec4Panel.Children.Add(_zVec4Spin);
            vec4Panel.Children.Add(_wVec4Spin);
            _vector4Page = new StackPanel();
            _vector4Page.Children.Add(new TextBlock { Text = "X, Y, Z, W:", Foreground = PropertyPanelForeground });
            _vector4Page.Children.Add(vec4Panel);

            _stringrefSpin = new NumericUpDown { Minimum = -1, Foreground = PropertyPanelForeground };
            _tlkTextEdit = new TextBox { IsReadOnly = true, AcceptsReturn = true, Height = 40, Foreground = PropertyPanelForeground };
            _substringList = new ListBox();
            _substringLangCombo = new ComboBox { ItemsSource = Enum.GetNames(typeof(Language)), Foreground = PropertyPanelForeground };
            _substringGenderCombo = new ComboBox { ItemsSource = Enum.GetNames(typeof(Gender)), Foreground = PropertyPanelForeground };
            _addSubstringButton = new Button { Content = "Add Substring" };
            _removeSubstringButton = new Button { Content = "Remove Substring" };
            _substringEdit = new TextBox { AcceptsReturn = true, Foreground = PropertyPanelForeground };
            _substringPage = new StackPanel();
            _substringPage.Children.Add(new TextBlock { Text = "StringRef:", Foreground = PropertyPanelForeground });
            _substringPage.Children.Add(_stringrefSpin);
            _substringPage.Children.Add(new TextBlock { Text = "TLK preview:", Foreground = PropertyPanelForeground });
            _substringPage.Children.Add(_tlkTextEdit);
            _substringPage.Children.Add(new TextBlock { Text = "Substrings:", Foreground = PropertyPanelForeground });
            _substringPage.Children.Add(_substringList);
            var substringButtonRow = new StackPanel { Orientation = Orientation.Horizontal };
            substringButtonRow.Children.Add(_substringLangCombo);
            substringButtonRow.Children.Add(_substringGenderCombo);
            substringButtonRow.Children.Add(_addSubstringButton);
            substringButtonRow.Children.Add(_removeSubstringButton);
            _substringPage.Children.Add(substringButtonRow);
            _substringPage.Children.Add(new TextBlock { Text = "Substring text:", Foreground = PropertyPanelForeground });
            _substringPage.Children.Add(_substringEdit);

            _binaryHexLabel = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap, Foreground = PropertyPanelForeground };
            _copyBinaryButton = new Button { Content = "Copy Binary Data" };
            _convertBinaryButton = new Button { Content = "Convert value...", IsVisible = false };
            _blankPage = new StackPanel();
            _blankPage.Children.Add(_binaryHexLabel);
            _blankPage.Children.Add(_copyBinaryButton);
            _blankPage.Children.Add(_convertBinaryButton);

            _pagesControl = new ContentControl();
            _pages = new StackPanel(); // keep for compatibility; active content goes in _pagesControl

            if (_fieldBox is Panel fieldBoxPanel)
            {
                fieldBoxPanel.Children.Add(new TextBlock { Text = "Label:", Foreground = PropertyPanelForeground });
                fieldBoxPanel.Children.Add(_labelEdit);
                fieldBoxPanel.Children.Add(new TextBlock { Text = "Type:", Foreground = PropertyPanelForeground });
                fieldBoxPanel.Children.Add(_typeCombo);
                fieldBoxPanel.Children.Add(_pagesControl);
            }

            rightPanel.Children.Add(_fieldBox);

            // When XAML doesn't load, build full layout with menu so the window has File/Edit/View/Tools/Language like OdyTool2DA.
            var menu = BuildProgrammaticMenu();
            _statusText = new TextBlock { Text = "OdyToolGFF", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Foreground = PropertyPanelForeground };
            _zoomCombo = new ComboBox { MinWidth = 72 };
            _zoomCombo.ItemsSource = TreeZoomFactors.Select(z => $"{(int)(z * 100)}%").ToList();
            _zoomCombo.SelectedIndex = _zoomIndex;
            _zoomCombo.SelectionChanged += (s, e) =>
            {
                if (_zoomCombo?.SelectedIndex >= 0 && _zoomCombo.SelectedIndex < TreeZoomFactors.Length)
                {
                    _zoomIndex = _zoomCombo.SelectedIndex;
                    ApplyTreeZoom();
                }
            };
            var statusBar = new Border
            {
                Padding = new Avalonia.Thickness(8, 4),
                Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(0xE8, 0xE8, 0xE8)),
                BorderBrush = new SolidColorBrush(Avalonia.Media.Color.FromRgb(0xD0, 0xD0, 0xD0)),
                BorderThickness = new Avalonia.Thickness(0, 1, 0, 0)
            };
            var statusGrid = new Grid { ColumnDefinitions = new ColumnDefinitions("*,Auto") };
            statusGrid.Children.Add(_statusText);
            Grid.SetColumn(_statusText, 0);
            var zoomStack = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            zoomStack.Children.Add(new TextBlock { Text = "Zoom:", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, Foreground = PropertyPanelForeground, Margin = new Avalonia.Thickness(0, 0, 4, 0) });
            zoomStack.Children.Add(_zoomCombo);
            statusGrid.Children.Add(zoomStack);
            Grid.SetColumn(zoomStack, 1);
            statusBar.Child = statusGrid;

            var contentGrid = new Grid { RowDefinitions = new RowDefinitions("*,Auto") };
            Grid.SetRow(mainSplitter, 0);
            contentGrid.Children.Add(mainSplitter);
            Grid.SetRow(statusBar, 1);
            contentGrid.Children.Add(statusBar);

            var dockPanel = new DockPanel();
            DockPanel.SetDock(menu, Dock.Top);
            dockPanel.Children.Add(menu);
            dockPanel.Children.Add(contentGrid);

            Content = dockPanel;
        }

        private static Menu BuildProgrammaticMenu()
        {
            var menu = new Menu
            {
                Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(0xF1, 0xF3, 0xF4)),
                Foreground = new SolidColorBrush(Avalonia.Media.Color.FromRgb(0x20, 0x21, 0x24)),
                BorderBrush = new SolidColorBrush(Avalonia.Media.Color.FromRgb(0xD7, 0xDB, 0xE6)),
                BorderThickness = new Avalonia.Thickness(0, 0, 0, 1)
            };
            var fileMenu = new MenuItem { Header = "_File", Name = "menuFile" };
            fileMenu.Items.Add(new MenuItem { Header = "_New", Name = "actionNew" });
            fileMenu.Items.Add(new MenuItem { Header = "_Open", Name = "actionOpen" });
            fileMenu.Items.Add(new MenuItem { Header = "_Save", Name = "actionSave", HotKey = KeyGesture.Parse("Ctrl+S") });
            fileMenu.Items.Add(new MenuItem { Header = "Save _As", Name = "actionSave_As" });
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(new MenuItem { Header = "Revert to _Saved", Name = "actionRevert" });
            fileMenu.Items.Add(new Separator());
            fileMenu.Items.Add(new MenuItem { Header = "E_xit", Name = "actionExit" });
            menu.Items.Add(fileMenu);

            var editMenu = new MenuItem { Header = "_Edit", Name = "menuEdit" };
            editMenu.Items.Add(new MenuItem { Header = "_Undo", Name = "actionUndo", HotKey = KeyGesture.Parse("Ctrl+Z") });
            editMenu.Items.Add(new MenuItem { Header = "_Redo", Name = "actionRedo", HotKey = KeyGesture.Parse("Ctrl+Y") });
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(new MenuItem { Header = "_Find in Tree...", Name = "actionFind", HotKey = KeyGesture.Parse("Ctrl+F") });
            editMenu.Items.Add(new MenuItem { Header = "Use Selection for _Find", Name = "actionUseSelectionForFind", HotKey = KeyGesture.Parse("Ctrl+E") });
            editMenu.Items.Add(new MenuItem { Header = "_Replace in Tree...", Name = "actionReplace", HotKey = KeyGesture.Parse("Ctrl+H") });
            editMenu.Items.Add(new MenuItem { Header = "Find _Next", Name = "actionFindNext", HotKey = KeyGesture.Parse("F3") });
            editMenu.Items.Add(new MenuItem { Header = "Find _Previous", Name = "actionFindPrevious", HotKey = KeyGesture.Parse("Shift+F3") });
            editMenu.Items.Add(new MenuItem { Header = "_Go to Selection", Name = "actionGoToSelection", HotKey = KeyGesture.Parse("Ctrl+J") });
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(new MenuItem { Header = "Move _Up", Name = "actionMoveNodeUp", HotKey = KeyGesture.Parse("Alt+Up") });
            editMenu.Items.Add(new MenuItem { Header = "Move _Down", Name = "actionMoveNodeDown", HotKey = KeyGesture.Parse("Alt+Down") });
            editMenu.Items.Add(new MenuItem { Header = "_Sort Children", Name = "actionSortChildren" });
            editMenu.Items.Add(new MenuItem { Header = "Insert Sibling _Before", Name = "actionInsertSiblingBefore" });
            editMenu.Items.Add(new MenuItem { Header = "Insert Sibling _After", Name = "actionInsertSiblingAfter" });
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(new MenuItem { Header = "_Cut Node", Name = "actionCutNode", HotKey = KeyGesture.Parse("Ctrl+X") });
            editMenu.Items.Add(new MenuItem { Header = "_Copy Node", Name = "actionCopyNode", HotKey = KeyGesture.Parse("Ctrl+C") });
            editMenu.Items.Add(new MenuItem { Header = "_Paste Node", Name = "actionPasteNode", HotKey = KeyGesture.Parse("Ctrl+V") });
            editMenu.Items.Add(new MenuItem { Header = "_Duplicate Node", Name = "actionDuplicateNode" });
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(new MenuItem { Header = "E_xpand All", Name = "actionExpandAll" });
            editMenu.Items.Add(new MenuItem { Header = "_Collapse All", Name = "actionCollapseAll" });
            editMenu.Items.Add(new Separator());
            editMenu.Items.Add(new MenuItem { Header = "_Go to Struct ID...", Name = "actionGoToStructId", HotKey = KeyGesture.Parse("Ctrl+G") });
            menu.Items.Add(editMenu);

            var viewMenu = new MenuItem { Header = "_View", Name = "menuView" };
            viewMenu.Items.Add(new MenuItem { Header = "Zoom _In", Name = "actionZoomIn", HotKey = KeyGesture.Parse("Ctrl+Plus") });
            viewMenu.Items.Add(new MenuItem { Header = "Zoom _Out", Name = "actionZoomOut", HotKey = KeyGesture.Parse("Ctrl+Minus") });
            viewMenu.Items.Add(new MenuItem { Header = "_Normal Size", Name = "actionZoomNormal", HotKey = KeyGesture.Parse("Ctrl+0") });
            viewMenu.Items.Add(new Separator());
            viewMenu.Items.Add(new MenuItem { Header = "Search _Actions...", Name = "actionSearchActions", HotKey = KeyGesture.Parse("Ctrl+Shift+P") });
            viewMenu.Items.Add(new Separator());
            viewMenu.Items.Add(new MenuItem { Header = "_Reset Layout", Name = "actionResetLayout" });
            menu.Items.Add(viewMenu);

            var toolsMenu = new MenuItem { Header = "_Tools", Name = "menuTools" };
            toolsMenu.Items.Add(new MenuItem { Header = "_Convert value...", Name = "actionConvertValue" });
            toolsMenu.Items.Add(new MenuItem { Header = "Set TLK", Name = "actionSetTLK" });
            menu.Items.Add(toolsMenu);

            var langMenu = new MenuItem { Header = "_Language", Name = "menuLanguage" };
            langMenu.Items.Add(new MenuItem { Header = "English", Name = "actionLangEnglish" });
            langMenu.Items.Add(new MenuItem { Header = "Français", Name = "actionLangFrench" });
            langMenu.Items.Add(new MenuItem { Header = "Deutsch", Name = "actionLangGerman" });
            langMenu.Items.Add(new MenuItem { Header = "Italiano", Name = "actionLangItalian" });
            langMenu.Items.Add(new MenuItem { Header = "Español", Name = "actionLangSpanish" });
            langMenu.Items.Add(new MenuItem { Header = "Polski", Name = "actionLangPolish" });
            menu.Items.Add(langMenu);

            return menu;
        }

        private void SetupUI()
        {
            if (_treeView != null && _fieldBox != null && _typeCombo != null && _labelEdit != null && _pagesControl != null)
            {
                return;
            }

            try
            {
                _treeView = this.FindControl<TreeView>("treeView");
                var fieldBoxBorder = this.FindControl<Border>("fieldBox");
                if (fieldBoxBorder != null && fieldBoxBorder.Child is Panel fieldBoxPanel)
                {
                    _fieldBox = fieldBoxPanel;
                }
                _typeCombo = this.FindControl<ComboBox>("typeCombo");
                _labelEdit = this.FindControl<TextBox>("labelEdit");
                _pagesControl = this.FindControl<ContentControl>("pages");
                if (_typeCombo != null && _typeCombo.ItemsSource == null)
                {
                    _typeCombo.ItemsSource = Enum.GetValues(typeof(GFFFieldType)).Cast<GFFFieldType>().Select(t => t.ToString()).ToList();
                }
                if (_pagesControl != null && _intPage == null)
                {
                    CreateTypePages();
                }
                _statusText = this.FindControl<TextBlock>("statusText");
                _zoomCombo = this.FindControl<ComboBox>("zoomCombo");
                _mainGrid = this.FindControl<Grid>("mainGrid");
                if (_zoomCombo != null)
                {
                    _zoomCombo.ItemsSource = TreeZoomFactors.Select(z => $"{(int)(z * 100)}%").ToList();
                    _zoomCombo.SelectedIndex = _zoomIndex;
                    _zoomCombo.SelectionChanged += (s, e) =>
                    {
                        if (_zoomCombo?.SelectedIndex >= 0 && _zoomCombo.SelectedIndex < TreeZoomFactors.Length)
                        {
                            _zoomIndex = _zoomCombo.SelectedIndex;
                            ApplyTreeZoom();
                        }
                    };
                    ApplyTreeZoom();
                }
            }
            catch
            {
                if (_treeView == null) _treeView = new TreeView();
                if (_fieldBox == null) _fieldBox = new StackPanel();
                if (_typeCombo == null) _typeCombo = new ComboBox();
                if (_labelEdit == null) _labelEdit = new TextBox();
                if (_pagesControl == null) _pagesControl = new ContentControl();
                if (_intPage == null) CreateTypePages();
            }
        }

        private void CreateTypePages()
        {
            _intSpin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _intPage = new StackPanel();
            _intPage.Children.Add(new TextBlock { Text = "Value:", Foreground = PropertyPanelForeground });
            _intPage.Children.Add(_intSpin);

            _floatSpin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _floatPage = new StackPanel();
            _floatPage.Children.Add(new TextBlock { Text = "Value:", Foreground = PropertyPanelForeground });
            _floatPage.Children.Add(_floatSpin);

            _lineEdit = new TextBox { Foreground = PropertyPanelForeground, MaxLength = 16 };
            _linePage = new StackPanel();
            _linePage.Children.Add(new TextBlock { Text = "ResRef:", Foreground = PropertyPanelForeground });
            _linePage.Children.Add(_lineEdit);

            _textEdit = new TextBox { AcceptsReturn = true, Foreground = PropertyPanelForeground };
            _textPage = new StackPanel();
            _textPage.Children.Add(new TextBlock { Text = "Text:", Foreground = PropertyPanelForeground });
            _textPage.Children.Add(_textEdit);

            var vec3Panel = new StackPanel { Orientation = Orientation.Horizontal };
            _xVec3Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _yVec3Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _zVec3Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            vec3Panel.Children.Add(_xVec3Spin);
            vec3Panel.Children.Add(_yVec3Spin);
            vec3Panel.Children.Add(_zVec3Spin);
            _vector3Page = new StackPanel();
            _vector3Page.Children.Add(new TextBlock { Text = "X, Y, Z:", Foreground = PropertyPanelForeground });
            _vector3Page.Children.Add(vec3Panel);

            var vec4Panel = new StackPanel { Orientation = Orientation.Horizontal };
            _xVec4Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _yVec4Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _zVec4Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            _wVec4Spin = new NumericUpDown { Foreground = PropertyPanelForeground };
            vec4Panel.Children.Add(_xVec4Spin);
            vec4Panel.Children.Add(_yVec4Spin);
            vec4Panel.Children.Add(_zVec4Spin);
            vec4Panel.Children.Add(_wVec4Spin);
            _vector4Page = new StackPanel();
            _vector4Page.Children.Add(new TextBlock { Text = "X, Y, Z, W:", Foreground = PropertyPanelForeground });
            _vector4Page.Children.Add(vec4Panel);

            _stringrefSpin = new NumericUpDown { Minimum = -1, Foreground = PropertyPanelForeground };
            _tlkTextEdit = new TextBox { IsReadOnly = true, AcceptsReturn = true, Height = 40, Foreground = PropertyPanelForeground };
            _substringList = new ListBox();
            _substringLangCombo = new ComboBox { ItemsSource = Enum.GetNames(typeof(Language)), Foreground = PropertyPanelForeground };
            _substringGenderCombo = new ComboBox { ItemsSource = Enum.GetNames(typeof(Gender)), Foreground = PropertyPanelForeground };
            _addSubstringButton = new Button { Content = "Add Substring" };
            _removeSubstringButton = new Button { Content = "Remove Substring" };
            _substringEdit = new TextBox { AcceptsReturn = true, Foreground = PropertyPanelForeground };
            _substringPage = new StackPanel();
            _substringPage.Children.Add(new TextBlock { Text = "StringRef:", Foreground = PropertyPanelForeground });
            _substringPage.Children.Add(_stringrefSpin);
            _substringPage.Children.Add(new TextBlock { Text = "TLK preview:", Foreground = PropertyPanelForeground });
            _substringPage.Children.Add(_tlkTextEdit);
            _substringPage.Children.Add(new TextBlock { Text = "Substrings:", Foreground = PropertyPanelForeground });
            _substringPage.Children.Add(_substringList);
            var substringButtonRow = new StackPanel { Orientation = Orientation.Horizontal };
            substringButtonRow.Children.Add(_substringLangCombo);
            substringButtonRow.Children.Add(_substringGenderCombo);
            substringButtonRow.Children.Add(_addSubstringButton);
            substringButtonRow.Children.Add(_removeSubstringButton);
            _substringPage.Children.Add(substringButtonRow);
            _substringPage.Children.Add(new TextBlock { Text = "Substring text:", Foreground = PropertyPanelForeground });
            _substringPage.Children.Add(_substringEdit);

            _binaryHexLabel = new TextBlock { TextWrapping = Avalonia.Media.TextWrapping.Wrap, Foreground = PropertyPanelForeground };
            _copyBinaryButton = new Button { Content = "Copy Binary Data" };
            _convertBinaryButton = new Button { Content = "Convert value...", IsVisible = false };
            _blankPage = new StackPanel();
            _blankPage.Children.Add(_binaryHexLabel);
            _blankPage.Children.Add(_copyBinaryButton);
            _blankPage.Children.Add(_convertBinaryButton);
        }

        private void SetupSignals()
        {
            if (_treeView != null)
            {
                _treeView.SelectionChanged += (s, e) => SelectionChanged();
                _treeView.AddHandler(PointerPressedEvent, OnTreeViewPointerPressed, RoutingStrategies.Tunnel);
                _treeView.AddHandler(PointerMovedEvent, OnTreeViewPointerMoved, RoutingStrategies.Bubble);
                _treeView.AddHandler(PointerReleasedEvent, OnTreeViewPointerReleased, RoutingStrategies.Bubble);
                _treeView.AddHandler(PointerCaptureLostEvent, OnTreeViewPointerCaptureLost, RoutingStrategies.Bubble);
                KeyDown += OnWindowKeyDown;
            }

            Opened += (s, e) =>
            {
                UpdateStatusBar();
                _treeView?.Focus();
            };

            if (_intSpin != null) _intSpin.ValueChanged += (s, e) => UpdateData();
            if (_floatSpin != null) _floatSpin.ValueChanged += (s, e) => UpdateData();
            if (_lineEdit != null) _lineEdit.LostFocus += (s, e) => UpdateData();
            if (_textEdit != null) _textEdit.LostFocus += (s, e) => UpdateData();
            if (_xVec3Spin != null) _xVec3Spin.ValueChanged += (s, e) => UpdateData();
            if (_yVec3Spin != null) _yVec3Spin.ValueChanged += (s, e) => UpdateData();
            if (_zVec3Spin != null) _zVec3Spin.ValueChanged += (s, e) => UpdateData();
            if (_xVec4Spin != null) _xVec4Spin.ValueChanged += (s, e) => UpdateData();
            if (_yVec4Spin != null) _yVec4Spin.ValueChanged += (s, e) => UpdateData();
            if (_zVec4Spin != null) _zVec4Spin.ValueChanged += (s, e) => UpdateData();
            if (_wVec4Spin != null) _wVec4Spin.ValueChanged += (s, e) => UpdateData();
            if (_labelEdit != null) _labelEdit.LostFocus += (s, e) => UpdateData();
            if (_typeCombo != null) _typeCombo.SelectionChanged += (s, e) => TypeChanged();
            if (_addSubstringButton != null) _addSubstringButton.Click += (s, e) => AddSubstring();
            if (_removeSubstringButton != null) _removeSubstringButton.Click += (s, e) => RemoveSubstring();
            if (_substringEdit != null) _substringEdit.LostFocus += (s, e) => SubstringEdited();
            if (_stringrefSpin != null)
            {
                _stringrefSpin.ValueChanged += (s, e) => ChangeLocStringText();
                _stringrefSpin.LostFocus += (s, e) => UpdateData();
            }
            if (_substringList != null) _substringList.SelectionChanged += (s, e) => SubstringSelected();
            if (_copyBinaryButton != null) _copyBinaryButton.Click += (s, e) => CopyBinaryData();
            if (_convertBinaryButton != null) _convertBinaryButton.Click += (s, e) => ShowValueConverterDialog();
        }

        private void SetupMenuHandlers()
        {
            void Bind(string name, Action handler)
            {
                try
                {
                    var item = this.FindControl<MenuItem>(name);
                    if (item != null) item.Click += (s, e) => handler();
                }
                catch { }
            }
            Bind("actionNew", () => New());
            Bind("actionOpen", async () => { if (await ConfirmDiscardUnsavedChangesAsync()) await RunOpenAsync(); });
            Bind("actionSave", () => Save());
            Bind("actionSave_As", () => _ = RunSaveAsAsync());
            Bind("actionRevert", () => Revert());
            Bind("actionExit", () => Close());
            Bind("actionUndo", () => Undo());
            Bind("actionRedo", () => Redo());
            Bind("actionFind", () => ShowFindDialog());
            Bind("actionUseSelectionForFind", () => UseSelectionForFind());
            Bind("actionReplace", () => ShowReplaceDialog());
            Bind("actionFindNext", () => FindNextMatch());
            Bind("actionFindPrevious", () => FindPreviousMatch());
            Bind("actionGoToSelection", () => GoToSelection());
            Bind("actionMoveNodeUp", () => MoveNodeUp());
            Bind("actionMoveNodeDown", () => MoveNodeDown());
            Bind("actionSortChildren", () => SortChildren());
            Bind("actionCutNode", () => CutNode());
            Bind("actionCopyNode", () => CopyNode());
            Bind("actionPasteNode", () => PasteNode());
            Bind("actionDuplicateNode", () => DuplicateNode());
            Bind("actionInsertSiblingBefore", () => InsertSiblingBefore());
            Bind("actionInsertSiblingAfter", () => InsertSiblingAfter());
            Bind("actionExpandAll", () => ExpandAll());
            Bind("actionCollapseAll", () => CollapseAll());
            Bind("actionGoToStructId", () => ShowGoToStructIdDialog());
            Bind("actionZoomIn", () => TreeZoomIn());
            Bind("actionZoomOut", () => TreeZoomOut());
            Bind("actionZoomNormal", () => TreeZoomNormal());
            Bind("actionSearchActions", () => ShowSearchActionsDialog());
            Bind("actionResetLayout", () => ResetLayout());
            Bind("actionSetTLK", () => ShowSetTLKInfo());
            Bind("actionConvertValue", () => ShowValueConverterDialog());
            Bind("actionLangEnglish", () => { Localization.SetLanguage(ToolsetLanguage.English); RefreshLocalizedStrings(); });
            Bind("actionLangFrench", () => { Localization.SetLanguage(ToolsetLanguage.French); RefreshLocalizedStrings(); });
            Bind("actionLangGerman", () => { Localization.SetLanguage(ToolsetLanguage.German); RefreshLocalizedStrings(); });
            Bind("actionLangItalian", () => { Localization.SetLanguage(ToolsetLanguage.Italian); RefreshLocalizedStrings(); });
            Bind("actionLangSpanish", () => { Localization.SetLanguage(ToolsetLanguage.Spanish); RefreshLocalizedStrings(); });
            Bind("actionLangPolish", () => { Localization.SetLanguage(ToolsetLanguage.Polish); RefreshLocalizedStrings(); });
        }

        private void RefreshLocalizedStrings()
        {
            // GFF editor menu headers could be localized here; for now no-op so language switch applies app-wide.
        }

        private void ApplyTreeZoom()
        {
            if (_treeView == null) return;
            double scale = TreeZoomFactors[_zoomIndex];
            _treeView.FontSize = DefaultTreeFontSize * scale;
            if (_zoomCombo != null && _zoomCombo.SelectedIndex != _zoomIndex)
                _zoomCombo.SelectedIndex = _zoomIndex;
            UpdateStatusBar();
        }

        private void TreeZoomIn()
        {
            if (_zoomIndex < TreeZoomFactors.Length - 1) { _zoomIndex++; ApplyTreeZoom(); if (_zoomCombo != null) _zoomCombo.SelectedIndex = _zoomIndex; }
        }

        private void TreeZoomOut()
        {
            if (_zoomIndex > 0) { _zoomIndex--; ApplyTreeZoom(); if (_zoomCombo != null) _zoomCombo.SelectedIndex = _zoomIndex; }
        }

        private void TreeZoomNormal()
        {
            _zoomIndex = 2; // 100%
            ApplyTreeZoom();
            if (_zoomCombo != null) _zoomCombo.SelectedIndex = _zoomIndex;
        }

        /// <summary>Tiled-style: Search Actions (Ctrl+Shift+P) - run a command by name.</summary>
        private void ShowSearchActionsDialog()
        {
            var actions = new (string Label, Action Run)[]
            {
                ("Find in Tree...", () => ShowFindDialog()),
                ("Replace in Tree...", () => ShowReplaceDialog()),
                ("Go to Struct ID...", () => ShowGoToStructIdDialog()),
                ("Use Selection for Find", () => UseSelectionForFind()),
                ("Go to Selection", () => GoToSelection()),
                ("Expand All", () => ExpandAll()),
                ("Collapse All", () => CollapseAll()),
                ("Sort Children", () => SortChildren()),
                ("Revert to Saved", () => Revert()),
            };
            var dialog = new Window { Title = "Search Actions", Width = 400, Height = 320, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            var stack = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var searchBox = new TextBox { Watermark = "Type to filter actions...", Margin = new Avalonia.Thickness(0, 0, 0, 8) };
            var listBox = new ListBox { MinHeight = 200 };
            listBox.ItemsSource = actions.Select(a => a.Label).ToList();
            listBox.SelectedIndex = 0;
            stack.Children.Add(searchBox);
            stack.Children.Add(listBox);
            var runBtn = new Button { Content = "Run", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            stack.Children.Add(runBtn);
            dialog.Content = stack;
            void RunSelected()
            {
                var selectedLabel = listBox.SelectedItem as string;
                if (selectedLabel == null) return;
                var match = actions.FirstOrDefault(a => a.Label == selectedLabel);
                if (match.Run != null) { dialog.Close(); match.Run(); }
            }
            runBtn.Click += (s, e) => RunSelected();
            listBox.DoubleTapped += (s, e) => RunSelected();
            searchBox.TextChanged += (s, e) =>
            {
                var q = (searchBox.Text ?? "").Trim().ToLowerInvariant();
                var filtered = string.IsNullOrEmpty(q) ? actions.Select(a => a.Label).ToList() : actions.Where(a => a.Label.ToLowerInvariant().Contains(q)).Select(a => a.Label).ToList();
                listBox.ItemsSource = filtered;
                listBox.SelectedIndex = filtered.Count > 0 ? 0 : -1;
            };
            dialog.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter) { RunSelected(); e.Handled = true; }
                if (e.Key == Key.Escape) { dialog.Close(); e.Handled = true; }
            };
            dialog.ShowDialog(this as Window);
            searchBox.Focus();
        }

        /// <summary>Tiled-style: Reset layout to default.</summary>
        private void ResetLayout()
        {
            if (_mainGrid != null && _mainGrid.ColumnDefinitions.Count >= 2)
            {
                _mainGrid.ColumnDefinitions[0] = new ColumnDefinition(new GridLength(2, GridUnitType.Star));
                _mainGrid.ColumnDefinitions[1] = new ColumnDefinition(new GridLength(1, GridUnitType.Star));
            }
            UpdateStatusBar();
        }

        // PropertyListEditor parity: Revert to Saved discards all changes and clears undo/redo.
        private void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            try
            {
                Load(_filepath ?? "", _resname ?? "", _restype ?? ResourceType.GFF, _revert);
                _undoStack.Clear();
                _redoStack.Clear();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Revert failed: {ex}");
            }
        }

        private async System.Threading.Tasks.Task RunSaveAsAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            string suggestedName = string.IsNullOrEmpty(_resname) ? "resource" : _resname;
            var ext = _restype != null ? _restype.Extension : "gff";
            var options = new FilePickerSaveOptions
            {
                Title = "Save GFF As",
                SuggestedFileName = suggestedName + "." + ext,
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("GFF (binary)") { Patterns = new[] { "*.gff", "*.are", "*.dlg", "*.git", "*.ifo", "*.jrl", "*.pth", "*.utc", "*.utd", "*.ute", "*.uti", "*.utm", "*.utp", "*.uts", "*.utt", "*.utw" } },
                    new FilePickerFileType("GFF XML") { Patterns = new[] { "*.gff.xml", "*.xml" } },
                    new FilePickerFileType("GFF JSON") { Patterns = new[] { "*.gff.json", "*.json" } }
                }
            };
            var file = await storageProvider.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path.LocalPath;
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            string pathExt = System.IO.Path.GetExtension(path).TrimStart('.').ToLowerInvariant();
            if (pathExt == "xml") _restype = ResourceType.GFF_XML;
            else if (pathExt == "json") _restype = ResourceType.GFF_JSON;
            else _restype = ResourceType.FromExtension(pathExt) ?? _restype ?? ResourceType.GFF;
            RefreshWindowTitle();
            Save();
            UpdateStatusBar();
        }

        /// <summary>Opens a GFF file from disk (File → Open). Used by standalone and when opening from toolset.</summary>
        private async System.Threading.Tasks.Task RunOpenAsync()
        {
            var storageProvider = (this as Window)?.StorageProvider;
            if (storageProvider == null) return;
            var options = new FilePickerOpenOptions
            {
                Title = "Open GFF",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("GFF (binary)") { Patterns = new[] { "*.gff", "*.are", "*.dlg", "*.git", "*.ifo", "*.jrl", "*.pth", "*.utc", "*.utd", "*.ute", "*.uti", "*.utm", "*.utp", "*.uts", "*.utt", "*.utw" } },
                    new FilePickerFileType("GFF XML") { Patterns = new[] { "*.gff.xml", "*.xml" } },
                    new FilePickerFileType("GFF JSON") { Patterns = new[] { "*.gff.json", "*.json" } },
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
                string pathExt = System.IO.Path.GetExtension(path).TrimStart('.').ToLowerInvariant();
                ResourceType restype = ResourceType.GFF;
                if (pathExt == "xml") restype = ResourceType.GFF_XML;
                else if (pathExt == "json") restype = ResourceType.GFF_JSON;
                else restype = ResourceType.FromExtension(pathExt) ?? ResourceType.GFF;
                Load(path, resname, restype, data);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GFF Open failed: " + ex);
                var box = MessageBoxManager.GetMessageBoxStandard("Open GFF", "Could not open file: " + ex.Message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await box.ShowWindowDialogAsync(this as Window);
            }
        }

        /// <summary>Shows info about TLK resolution. When used from the main app, installation's dialog.tlk is used for LocalizedString preview.</summary>
        private void ShowSetTLKInfo()
        {
            string msg = _installation != null
                ? "TLK resolution uses the current installation's dialog.tlk for LocalizedString preview. Change the installation in the main window to use a different TLK."
                : "No installation is set. Open a GFF from the main window with an installation to use TLK resolution for LocalizedString (string ref) preview.";
            var box = MessageBoxManager.GetMessageBoxStandard("Set TLK", msg, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info);
            _ = box.ShowWindowDialogAsync(this as Window);
        }

        private void OnTreeViewPointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                // Close any previously opened context menu so we don't stack multiple menus.
                if (_openContextMenu != null && _openContextMenu.IsOpen)
                {
                    _openContextMenu.Close();
                    _openContextMenu = null;
                }
                // Use the node under the pointer so the menu applies to the row we right-clicked.
                var node = GetNodeFromVisual(e.Source as Control) ?? GetSelectedNodeFromTree();
                if (node == null) return;
                _treeView.SelectedItem = node;
                _selectedNode = node;
                LoadItem(node);
                var menu = BuildContextMenu(node);
                if (menu != null)
                {
                    menu.Placement = PlacementMode.Pointer;
                    menu.Closing += (s, ev) =>
                    {
                        if (s == _openContextMenu)
                            _openContextMenu = null;
                    };
                    _openContextMenu = menu;
                    menu.Open((Control)_treeView);
                }
                return;
            }
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                _dragSourceNode = GetSelectedNodeFromTree();
                _dragStartPos = e.GetPosition(_treeView);
                _isDragging = false;
            }
        }

        private void OnTreeViewPointerMoved(object sender, PointerEventArgs e)
        {
            if (_dragSourceNode == null) return;
            if (!e.GetCurrentPoint(_treeView).Properties.IsLeftButtonPressed) return;
            var pos = e.GetPosition(_treeView);
            if (Math.Abs(pos.X - _dragStartPos.X) + Math.Abs(pos.Y - _dragStartPos.Y) > 6)
                _isDragging = true;
        }

        private void OnTreeViewPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (_dragSourceNode == null) return;
            if (_isDragging)
            {
                var dropTarget = GetNodeAtPointerSource(e);
                if (dropTarget != null && dropTarget != _dragSourceNode)
                {
                    var parent = GetParentOf(_dragSourceNode);
                    var targetParent = GetParentOf(dropTarget);
                    if (parent != null && parent == targetParent)
                    {
                        int oldIdx = parent.Children.IndexOf(_dragSourceNode);
                        int newIdx = parent.Children.IndexOf(dropTarget);
                        if (oldIdx >= 0 && newIdx >= 0 && oldIdx != newIdx)
                        {
                            PushState();
                            parent.Children.RemoveAt(oldIdx);
                            if (newIdx > oldIdx) newIdx--;
                            parent.Children.Insert(newIdx, _dragSourceNode);
                            RefreshItemText(parent);
                            UpdateStatusBar();
                            MarkDirty();
                        }
                    }
                }
            }
            _dragSourceNode = null;
            _isDragging = false;
        }

        private void OnTreeViewPointerCaptureLost(object sender, PointerCaptureLostEventArgs e)
        {
            _dragSourceNode = null;
            _isDragging = false;
        }

        /// <summary>Finds the tree node (DataContext) for the TreeViewItem under the pointer event source.</summary>
        private static GFFTreeNodeViewModel GetNodeAtPointerSource(PointerReleasedEventArgs e)
        {
            return GetNodeFromVisual(e.Source as Control);
        }

        private static GFFTreeNodeViewModel GetNodeFromVisual(Control start)
        {
            for (var v = start; v != null; v = v.GetVisualParent() as Control)
            {
                if (v is TreeViewItem tvi && tvi.DataContext is GFFTreeNodeViewModel vm)
                    return vm;
            }
            return null;
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
                else if (e.Key == Key.G) { ShowGoToStructIdDialog(); e.Handled = true; }
                else if (e.Key == Key.X) { CutNode(); e.Handled = true; }
                else if (e.Key == Key.C) { CopyNode(); e.Handled = true; }
                else if (e.Key == Key.V) { PasteNode(); e.Handled = true; }
                else if (e.Key == Key.R) { ShowReplaceDialog(); e.Handled = true; }
                else if (e.Key == Key.E) { UseSelectionForFind(); e.Handled = true; }
                else if (e.Key == Key.J) { GoToSelection(); e.Handled = true; }
                else if (e.Key == Key.D0) { TreeZoomNormal(); e.Handled = true; }
                else if (e.Key == Key.Add || e.Key == Key.OemPlus) { TreeZoomIn(); e.Handled = true; }
                else if (e.Key == Key.Subtract || e.Key == Key.OemMinus) { TreeZoomOut(); e.Handled = true; }
                else if (e.Key == Key.P && (mod & KeyModifiers.Shift) != 0) { ShowSearchActionsDialog(); e.Handled = true; }
            }
            else if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                if (_treeView?.IsFocused == true)
                {
                    RemoveSelectedNodes();
                    e.Handled = true;
                }
            }
            else if (e.Key == Key.F3)
            {
                bool shift = (mod & KeyModifiers.Shift) != 0;
                if (shift) FindPreviousMatch();
                else FindNextMatch();
                e.Handled = true;
            }
            else if (e.Key == Key.Up && (mod & KeyModifiers.Alt) != 0)
            {
                MoveNodeUp();
                e.Handled = true;
            }
            else if (e.Key == Key.Down && (mod & KeyModifiers.Alt) != 0)
            {
                MoveNodeDown();
                e.Handled = true;
            }
        }

        /// <summary>Runs an action after the context menu closes so the tree and selection are in a consistent state.</summary>
        private void RunAfterMenuClose(Action action)
        {
            if (_openContextMenu != null && _openContextMenu.IsOpen)
            {
                _openContextMenu.Close();
                _openContextMenu = null;
            }
            Dispatcher.UIThread.Post(() =>
            {
                try { action(); }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("GFF context menu action failed: " + ex);
                    var box = MessageBoxManager.GetMessageBoxStandard("GFF Editor", "Action failed: " + ex.Message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                    _ = box.ShowWindowDialogAsync(this as Window);
                }
            }, DispatcherPriority.Normal);
        }

        private ContextMenu BuildContextMenu(GFFTreeNodeViewModel node)
        {
            if (node == null) return null;
            var menu = new ContextMenu();
            GFFFieldType? nestedType = node.FieldType;
            if (nestedType == GFFFieldType.List)
            {
                var addStruct = new MenuItem { Header = "Add Struct" };
                var listNode = node;
                addStruct.Click += (s, ev) => RunAfterMenuClose(() => AddNode(listNode));
                menu.Items.Add(addStruct);
            }
            else if (nestedType == GFFFieldType.Struct || nestedType == null)
            {
                var addMenu = new MenuItem { Header = "Add" };
                var parentNode = node;
                var addStruct = new MenuItem { Header = "Struct" };
                addStruct.Click += (s, ev) => RunAfterMenuClose(() => InsertNode(parentNode, "New Struct", GFFFieldType.Struct, new GFFStruct()));
                var addList = new MenuItem { Header = "List" };
                addList.Click += (s, ev) => RunAfterMenuClose(() => InsertNode(parentNode, "New List", GFFFieldType.List, new GFFList()));
                addMenu.Items.Add(addStruct);
                addMenu.Items.Add(addList);
                addMenu.Items.Add(new Separator());
                foreach (GFFFieldType ft in Enum.GetValues(typeof(GFFFieldType)))
                {
                    if (ft == GFFFieldType.Struct || ft == GFFFieldType.List) continue;
                    var item = new MenuItem { Header = ft.ToString() };
                    object defaultValue = GetDefaultValueForType(ft);
                    string label = "New " + ft;
                    var p = parentNode;
                    var f = ft;
                    var d = defaultValue;
                    var l = label;
                    item.Click += (s, ev) => RunAfterMenuClose(() => InsertNode(p, l, f, d));
                    addMenu.Items.Add(item);
                }
                menu.Items.Add(addMenu);
            }
            var remove = new MenuItem { Header = "Remove" };
            var nodeToRemove = node;
            remove.Click += (s, ev) => RunAfterMenuClose(() => RemoveNode(nodeToRemove));
            menu.Items.Add(remove);
            var parent = GetParentOf(node);
            if (parent != null && parent.Children.IndexOf(node) > 0)
            {
                var moveUp = new MenuItem { Header = "Move Up" };
                moveUp.Click += (s, ev) => RunAfterMenuClose(() => MoveNodeUp());
                menu.Items.Add(moveUp);
            }
            if (parent != null && parent.Children.IndexOf(node) >= 0 && parent.Children.IndexOf(node) < parent.Children.Count - 1)
            {
                var moveDown = new MenuItem { Header = "Move Down" };
                moveDown.Click += (s, ev) => RunAfterMenuClose(() => MoveNodeDown());
                menu.Items.Add(moveDown);
            }
            if (parent != null)
            {
                var insertBefore = new MenuItem { Header = "Insert Sibling Before" };
                insertBefore.Click += (s, ev) => RunAfterMenuClose(() => InsertSiblingBefore());
                menu.Items.Add(insertBefore);
                var insertAfter = new MenuItem { Header = "Insert Sibling After" };
                insertAfter.Click += (s, ev) => RunAfterMenuClose(() => InsertSiblingAfter());
                menu.Items.Add(insertAfter);
            }
            if (node.Children != null && node.Children.Count >= 2)
            {
                var sortChildren = new MenuItem { Header = "Sort Children" };
                sortChildren.Click += (s, ev) => RunAfterMenuClose(() => SortChildren());
                menu.Items.Add(sortChildren);
            }
            if (node.FieldType == GFFFieldType.Binary)
            {
                var convertVal = new MenuItem { Header = "Convert value..." };
                convertVal.Click += (s, ev) => RunAfterMenuClose(() => ShowValueConverterDialog());
                menu.Items.Add(convertVal);
            }
            menu.Items.Add(new Separator());
            var copyNode = new MenuItem { Header = "Copy Node" };
            copyNode.Click += (s, ev) => RunAfterMenuClose(() => CopyNode());
            menu.Items.Add(copyNode);
            var pasteNode = new MenuItem { Header = "Paste Node" };
            pasteNode.Click += (s, ev) => RunAfterMenuClose(() => PasteNode());
            menu.Items.Add(pasteNode);
            var duplicateNode = new MenuItem { Header = "Duplicate Node" };
            duplicateNode.Click += (s, ev) => RunAfterMenuClose(() => DuplicateNode());
            menu.Items.Add(duplicateNode);
            return menu;
        }

        private static object GetDefaultValueForType(GFFFieldType ft)
        {
            switch (ft)
            {
                case GFFFieldType.UInt8: case GFFFieldType.UInt16: case GFFFieldType.UInt32: case GFFFieldType.UInt64:
                case GFFFieldType.Int8: case GFFFieldType.Int16: case GFFFieldType.Int32: case GFFFieldType.Int64:
                    return 0;
                case GFFFieldType.Single: case GFFFieldType.Double: return 0.0;
                case GFFFieldType.String: return "";
                case GFFFieldType.ResRef: return ResRef.FromBlank();
                case GFFFieldType.LocalizedString: return LocalizedString.FromInvalid();
                case GFFFieldType.Binary: return new byte[0];
                case GFFFieldType.Vector3: return Vector3.Zero;
                case GFFFieldType.Vector4: return Vector4.Zero;
                default: return null;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/gff.py:120-142
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            if (data == null || data.Length == 0)
            {
                GFFContent content = GFFContent.GFF;
                if (!string.IsNullOrEmpty(resref))
                {
                    // Try to determine content type from resname
                    content = GFFContentExtensions.FromResName(resref);
                }
                _gff = new GFF(content);
                LoadGff(_gff);
                return;
            }
            try
            {
                _gff = GFF.FromBytes(data);
                LoadGff(_gff);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load GFF: {ex}");
                _ = MessageBoxManager.GetMessageBoxStandard(
                    "Error loading GFF",
                    "Error while loading GFF: " + ex.Message,
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error).ShowWindowDialogAsync(this);
                GFFContent content = GFFContent.GFF;
                if (!string.IsNullOrEmpty(resref))
                    content = GFFContentExtensions.FromResName(resref);
                _gff = new GFF(content);
                LoadGff(_gff);
            }
        }

        private void LoadGff(GFF gff)
        {
            if (_treeView == null)
            {
                return;
            }

            var rootNode = new GFFTreeNodeViewModel("[ROOT]", GFFFieldType.Struct, null, gff.Root) { IsRoot = true };
            LoadStruct(rootNode, gff.Root);
            RefreshAllNodeTexts(rootNode);
            _treeView.ItemsSource = new[] { rootNode };
            _undoStack.Clear();
            _redoStack.Clear();
            _copiedNode = null;
            _findMatches.Clear();
            _findStartIndex = 0;
            UpdateStatusBar();
        }

        private void LoadStruct(GFFTreeNodeViewModel node, GFFStruct gffStruct)
        {
            foreach ((string label, GFFFieldType ftype, object value) in gffStruct)
            {
                var childNode = new GFFTreeNodeViewModel("", ftype, label, value);
                if (value is GFFStruct childStruct)
                {
                    childNode.StructId = childStruct.StructId;
                }
                node.Children.Add(childNode);

                if (ftype == GFFFieldType.List && value is GFFList gffList)
                {
                    LoadList(childNode, gffList);
                }
                else if (ftype == GFFFieldType.Struct && value is GFFStruct cs)
                {
                    LoadStruct(childNode, cs);
                }
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/gff.py:171-185
        // Original: def _load_list(self, node, gff_list):
        private void LoadList(GFFTreeNodeViewModel node, GFFList gffList)
        {
            foreach (var gffStruct in gffList)
            {
                var childNode = new GFFTreeNodeViewModel("", GFFFieldType.Struct, null, gffStruct);
                childNode.StructId = gffStruct.StructId;
                node.Children.Add(childNode);
                LoadStruct(childNode, gffStruct);
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/gff.py:187-205
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            if (_gff == null)
            {
                return Tuple.Create(new byte[0], new byte[0]);
            }

            if (_treeView?.ItemsSource is IEnumerable<GFFTreeNodeViewModel> items && items.Any())
            {
                var rootNode = items.First();
                BuildStruct(rootNode, _gff.Root);
            }

            ResourceType gffType = _restype ?? ResourceType.GFF;
            byte[] data = GFFAuto.BytesGff(_gff, gffType);
            return Tuple.Create(data, new byte[0]);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/gff.py:207-261
        // Original: def _build_struct(self, item, gff_struct):
        private void BuildStruct(GFFTreeNodeViewModel item, GFFStruct gffStruct)
        {
            foreach (var child in item.Children)
            {
                string label = child.Label ?? "";
                GFFFieldType ftype = child.FieldType;
                object value = child.Value;

                if (ftype == GFFFieldType.UInt8)
                {
                    gffStruct.SetUInt8(label, Convert.ToByte(value));
                }
                else if (ftype == GFFFieldType.UInt16)
                {
                    gffStruct.SetUInt16(label, Convert.ToUInt16(value));
                }
                else if (ftype == GFFFieldType.UInt32)
                {
                    gffStruct.SetUInt32(label, Convert.ToUInt32(value));
                }
                else if (ftype == GFFFieldType.UInt64)
                {
                    gffStruct.SetUInt64(label, Convert.ToUInt64(value));
                }
                else if (ftype == GFFFieldType.Int8)
                {
                    gffStruct.SetInt8(label, Convert.ToSByte(value));
                }
                else if (ftype == GFFFieldType.Int16)
                {
                    gffStruct.SetInt16(label, Convert.ToInt16(value));
                }
                else if (ftype == GFFFieldType.Int32)
                {
                    gffStruct.SetInt32(label, Convert.ToInt32(value));
                }
                else if (ftype == GFFFieldType.Int64)
                {
                    gffStruct.SetInt64(label, Convert.ToInt64(value));
                }
                else if (ftype == GFFFieldType.Single)
                {
                    gffStruct.SetSingle(label, Convert.ToSingle(value));
                }
                else if (ftype == GFFFieldType.Double)
                {
                    gffStruct.SetDouble(label, Convert.ToDouble(value));
                }
                else if (ftype == GFFFieldType.ResRef)
                {
                    gffStruct.SetResRef(label, value as ResRef ?? ResRef.FromBlank());
                }
                else if (ftype == GFFFieldType.String)
                {
                    gffStruct.SetString(label, value?.ToString() ?? "");
                }
                else if (ftype == GFFFieldType.LocalizedString)
                {
                    gffStruct.SetLocString(label, value as LocalizedString ?? LocalizedString.FromInvalid());
                }
                else if (ftype == GFFFieldType.Binary)
                {
                    gffStruct.SetBinary(label, value as byte[] ?? new byte[0]);
                }
                else if (ftype == GFFFieldType.Vector3)
                {
                    gffStruct.SetVector3(label, value is Vector3 v3 ? v3 : new Vector3(0, 0, 0));
                }
                else if (ftype == GFFFieldType.Vector4)
                {
                    gffStruct.SetVector4(label, value is Vector4 v4 ? v4 : new Vector4(0, 0, 0, 0));
                }
                else if (ftype == GFFFieldType.Struct && value is GFFStruct childStruct)
                {
                    var newStruct = new GFFStruct(childStruct.StructId);
                    gffStruct.SetStruct(label, newStruct);
                    BuildStruct(child, newStruct);
                }
                else if (ftype == GFFFieldType.List)
                {
                    var newList = new GFFList();
                    gffStruct.SetList(label, newList);
                    BuildList(child, newList);
                }
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/gff.py:262-272
        // Original: def _build_list(self, item, gff_list):
        private void BuildList(GFFTreeNodeViewModel item, GFFList gffList)
        {
            foreach (var child in item.Children)
            {
                int structId = child.StructId;
                var gffStruct = gffList.Add(structId);
                BuildStruct(child, gffStruct);
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/gff.py:274-282
        // Original: def new(self):
        public override void New()
        {
            base.New();
            _undoStack.Clear();
            _redoStack.Clear();
            _copiedNode = null;
            _findMatches.Clear();
            _findQuery = "";
            _findStartIndex = 0;
            GFFContent content = GFFContent.GFF;
            if (!string.IsNullOrEmpty(_resname))
            {
                content = GFFContentExtensions.FromResName(_resname);
            }
            _gff = new GFF(content);
            if (_treeView != null)
            {
                var rootNode = new GFFTreeNodeViewModel("[ROOT]", GFFFieldType.Struct, null, _gff.Root) { IsRoot = true };
                LoadStruct(rootNode, _gff.Root);
                RefreshAllNodeTexts(rootNode);
                _treeView.ItemsSource = new[] { rootNode };
                _selectedNode = null;
                _treeView.SelectedItem = null;
                if (_pagesControl != null) _pagesControl.Content = null;
                if (_fieldBox != null) _fieldBox.IsEnabled = false;
            }
            UpdateStatusBar();
        }

        private GFFTreeNodeViewModel GetSelectedNodeFromTree()
        {
            return _treeView?.SelectedItem as GFFTreeNodeViewModel;
        }

        private void SelectionChanged()
        {
            var selectedNode = GetSelectedNodeFromTree();
            _selectedNode = selectedNode;
            if (selectedNode != null)
            {
                LoadItem(selectedNode);
            }
            UpdateStatusBar();
        }

        private void SetIntSpinRange(long min, long max)
        {
            if (_intSpin == null) return;
            _intSpin.Minimum = min;
            _intSpin.Maximum = max;
        }

        private void LoadItem(GFFTreeNodeViewModel item)
        {
            if (_pagesControl == null) return;

            if (item.Label == null)
            {
                _fieldBox.IsEnabled = false;
                SetIntSpinRange(-1, 0xFFFFFFFF);
                if (_intSpin != null) _intSpin.Value = item.Value is int sid ? sid : (item.Value is GFFStruct gs ? gs.StructId : 0);
                _pagesControl.Content = _intPage;
                return;
            }

            _fieldBox.IsEnabled = true;
            _typeCombo.SelectedItem = item.FieldType.ToString();
            _labelEdit.Text = item.Label ?? "";

            if (item.FieldType == GFFFieldType.Int8)
            {
                SetIntSpinRange(-0x80, 0x7F);
                _intSpin.Value = Convert.ToDecimal(item.Value ?? 0);
                _pagesControl.Content = _intPage;
            }
            else if (item.FieldType == GFFFieldType.Int16)
            {
                SetIntSpinRange(-0x8000, 0x7FFF);
                _intSpin.Value = Convert.ToDecimal(item.Value ?? 0);
                _pagesControl.Content = _intPage;
            }
            else if (item.FieldType == GFFFieldType.Int32)
            {
                SetIntSpinRange(-0x80000000L, 0x7FFFFFFFL);
                _intSpin.Value = Convert.ToDecimal(item.Value ?? 0);
                _pagesControl.Content = _intPage;
            }
            else if (item.FieldType == GFFFieldType.Int64)
            {
                SetIntSpinRange(long.MinValue, long.MaxValue);
                _intSpin.Value = Convert.ToDecimal(item.Value ?? 0);
                _pagesControl.Content = _intPage;
            }
            else if (item.FieldType == GFFFieldType.UInt8)
            {
                SetIntSpinRange(0, 0xFF);
                _intSpin.Value = Convert.ToDecimal(item.Value ?? 0);
                _pagesControl.Content = _intPage;
            }
            else if (item.FieldType == GFFFieldType.UInt16)
            {
                SetIntSpinRange(0, 0xFFFF);
                _intSpin.Value = Convert.ToDecimal(item.Value ?? 0);
                _pagesControl.Content = _intPage;
            }
            else if (item.FieldType == GFFFieldType.UInt32)
            {
                SetIntSpinRange(0, 0xFFFFFFFFL);
                _intSpin.Value = Convert.ToDecimal(item.Value ?? 0);
                _pagesControl.Content = _intPage;
            }
            else if (item.FieldType == GFFFieldType.UInt64)
            {
                SetIntSpinRange(0, long.MaxValue);
                _intSpin.Value = Convert.ToDecimal(item.Value ?? 0);
                _pagesControl.Content = _intPage;
            }
            else if (item.FieldType == GFFFieldType.Single || item.FieldType == GFFFieldType.Double)
            {
                _floatSpin.Value = Convert.ToDecimal(item.Value ?? 0);
                _pagesControl.Content = _floatPage;
            }
            else if (item.FieldType == GFFFieldType.ResRef)
            {
                _lineEdit.Text = item.Value?.ToString() ?? "";
                _pagesControl.Content = _linePage;
            }
            else if (item.FieldType == GFFFieldType.String)
            {
                _textEdit.Text = item.Value?.ToString() ?? "";
                _pagesControl.Content = _textPage;
            }
            else if (item.FieldType == GFFFieldType.Vector3 && item.Value is Vector3 vec3)
            {
                _xVec3Spin.Value = Convert.ToDecimal(vec3.X);
                _yVec3Spin.Value = Convert.ToDecimal(vec3.Y);
                _zVec3Spin.Value = Convert.ToDecimal(vec3.Z);
                _pagesControl.Content = _vector3Page;
            }
            else if (item.FieldType == GFFFieldType.Vector4 && item.Value is Vector4 vec4)
            {
                _xVec4Spin.Value = Convert.ToDecimal(vec4.X);
                _yVec4Spin.Value = Convert.ToDecimal(vec4.Y);
                _zVec4Spin.Value = Convert.ToDecimal(vec4.Z);
                _wVec4Spin.Value = Convert.ToDecimal(vec4.W);
                _pagesControl.Content = _vector4Page;
            }
            else if (item.FieldType == GFFFieldType.Struct)
            {
                SetIntSpinRange(-1, 0xFFFFFFFF);
                _intSpin.Value = item.Value is GFFStruct gs ? gs.StructId : (item.Value is int i ? i : 0);
                _pagesControl.Content = _intPage;
            }
            else if (item.FieldType == GFFFieldType.List)
            {
                int n = item.Children?.Count ?? 0;
                _binaryHexLabel.Text = $"List contains {n} item{(n == 1 ? "" : "s")}. Right-click the list in the tree to add structs.";
                _copyBinaryButton.IsVisible = false;
                if (_convertBinaryButton != null) _convertBinaryButton.IsVisible = false;
                _pagesControl.Content = _blankPage;
            }
            else if (item.FieldType == GFFFieldType.Binary)
            {
                var bytes = item.Value as byte[] ?? new byte[0];
                _binaryHexLabel.Text = bytes.Length == 0 ? "(empty)" : string.Join(" ", bytes.Select(b => b.ToString("X2")));
                _copyBinaryButton.IsVisible = true;
                if (_convertBinaryButton != null) _convertBinaryButton.IsVisible = true;
                _pagesControl.Content = _blankPage;
            }
            else if (item.FieldType == GFFFieldType.LocalizedString)
            {
                var loc = item.Value as LocalizedString ?? LocalizedString.FromInvalid();
                _stringrefSpin.Value = loc.StringRef;
                _substringItems = new ObservableCollection<SubstringListItem>();
                foreach (var (lang, gender, text) in loc)
                {
                    _substringItems.Add(new SubstringListItem { Id = LocalizedString.SubstringId(lang, gender), Text = text, Display = $"{lang}, {gender}" });
                }
                _substringList.ItemsSource = _substringItems;
                _substringEdit.IsEnabled = false;
                _substringEdit.Text = "";
                ChangeLocStringText();
                _pagesControl.Content = _substringPage;
            }
        }

        private sealed class SubstringListItem
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public string Display { get; set; }
            public override string ToString() => Display ?? "";
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/gff.py:452-796
        // Original: def update_data(self):
        private void UpdateData()
        {
            if (_selectedNode == null) return;

            if (_labelEdit != null)
            {
                _selectedNode.Label = _labelEdit.Text;
            }

            decimal spinVal = _intSpin?.Value ?? 0;
            switch (_selectedNode.FieldType)
            {
                case GFFFieldType.Int8: _selectedNode.Value = Convert.ToSByte(spinVal); break;
                case GFFFieldType.Int16: _selectedNode.Value = Convert.ToInt16(spinVal); break;
                case GFFFieldType.Int32: _selectedNode.Value = Convert.ToInt32(spinVal); break;
                case GFFFieldType.Int64: _selectedNode.Value = Convert.ToInt64(spinVal); break;
                case GFFFieldType.UInt8: _selectedNode.Value = Convert.ToByte(spinVal); break;
                case GFFFieldType.UInt16: _selectedNode.Value = Convert.ToUInt16(spinVal); break;
                case GFFFieldType.UInt32: _selectedNode.Value = Convert.ToUInt32(spinVal); break;
                case GFFFieldType.UInt64: _selectedNode.Value = Convert.ToUInt64(spinVal); break;
            }
            if (_selectedNode.FieldType == GFFFieldType.Single || _selectedNode.FieldType == GFFFieldType.Double)
            {
                if (_floatSpin != null)
                {
                    _selectedNode.Value = Convert.ToDouble(_floatSpin.Value ?? 0);
                }
            }
            else if (_selectedNode.FieldType == GFFFieldType.ResRef)
            {
                if (_lineEdit != null)
                {
                    _selectedNode.Value = new ResRef(_lineEdit.Text);
                }
            }
            else if (_selectedNode.FieldType == GFFFieldType.String)
            {
                if (_textEdit != null)
                {
                    _selectedNode.Value = _textEdit.Text;
                }
            }
            else if (_selectedNode.FieldType == GFFFieldType.Vector3)
            {
                if (_xVec3Spin != null && _yVec3Spin != null && _zVec3Spin != null)
                {
                    _selectedNode.Value = new Vector3(
                        Convert.ToSingle(_xVec3Spin.Value ?? 0),
                        Convert.ToSingle(_yVec3Spin.Value ?? 0),
                        Convert.ToSingle(_zVec3Spin.Value ?? 0));
                }
            }
            else if (_selectedNode.FieldType == GFFFieldType.Vector4)
            {
                if (_xVec4Spin != null && _yVec4Spin != null && _zVec4Spin != null && _wVec4Spin != null)
                {
                    _selectedNode.Value = new Vector4(
                        Convert.ToSingle(_xVec4Spin.Value ?? 0),
                        Convert.ToSingle(_yVec4Spin.Value ?? 0),
                        Convert.ToSingle(_zVec4Spin.Value ?? 0),
                        Convert.ToSingle(_wVec4Spin.Value ?? 0));
                }
            }
            else if (_selectedNode.FieldType == GFFFieldType.LocalizedString && _selectedNode.Value is LocalizedString locStr)
            {
                locStr.StringRef = (int)(_stringrefSpin?.Value ?? -1);
            }
            else if (_selectedNode.FieldType == GFFFieldType.Struct && _selectedNode.Value is GFFStruct gs)
            {
                int newId = (int)(_intSpin?.Value ?? 0);
                if (gs.StructId != newId)
                {
                    _selectedNode.Value = new GFFStruct(newId);
                    _selectedNode.StructId = newId;
                }
            }

            RefreshItemText(_selectedNode);
            MarkDirty();
            UpdateStatusBar();
        }

        /// <summary>Refreshes display text for one node. Call after load or when value/label changes.</summary>
        private void RefreshItemText(GFFTreeNodeViewModel item)
        {
            item.TypeDisplay = item.FieldType.ToString();
            item.ValueSummary = GetDisplayValueString(item);
            item.KeyDisplay = item.IsRoot ? "[ROOT]" : (item.Label ?? (item.FieldType == GFFFieldType.Struct ? $"Struct (ID: {item.StructId})" : "[?]"));
            if (item.Label == null)
            {
                item.Text = item.IsRoot ? "[ROOT]" : (item.FieldType == GFFFieldType.Struct ? $"Struct (ID: {item.StructId})" : "[?]");
            }
            else
            {
                string label = item.Label ?? "";
                string valueStr = GetDisplayValueString(item);
                item.Text = $"{label}: {valueStr}";
            }
        }

        private static string GetDisplayValueString(GFFTreeNodeViewModel item)
        {
            if (item.FieldType == GFFFieldType.List)
            {
                int n = item.Children?.Count ?? 0;
                return $"List ({n} item{(n == 1 ? "" : "s")})";
            }
            if (item.FieldType == GFFFieldType.Struct && item.Value is GFFStruct)
            {
                return $"Struct (ID: {item.StructId})";
            }
            return item.Value?.ToString() ?? "";
        }

        private void RefreshAllNodeTexts(GFFTreeNodeViewModel node)
        {
            if (node == null) return;
            RefreshItemText(node);
            if (node.Children != null)
            {
                foreach (var child in node.Children)
                    RefreshAllNodeTexts(child);
            }
        }

        private void TypeChanged()
        {
            if (_selectedNode == null || _typeCombo?.SelectedItem == null) return;
            if (!Enum.TryParse(_typeCombo.SelectedItem.ToString(), out GFFFieldType newType)) return;
            if (_selectedNode.FieldType == newType) return;
            PushState();
            _selectedNode.FieldType = newType;
            bool numeric = _selectedNode.Value is int || _selectedNode.Value is long || _selectedNode.Value is float || _selectedNode.Value is double;
            if (!numeric && (newType == GFFFieldType.UInt8 || newType == GFFFieldType.Int8 || newType == GFFFieldType.UInt16 || newType == GFFFieldType.Int16 ||
                newType == GFFFieldType.UInt32 || newType == GFFFieldType.Int32 || newType == GFFFieldType.UInt64 || newType == GFFFieldType.Int64 ||
                newType == GFFFieldType.Single || newType == GFFFieldType.Double))
            {
                _selectedNode.Value = 0;
            }
            else if (newType == GFFFieldType.String) _selectedNode.Value = "";
            else if (newType == GFFFieldType.LocalizedString) _selectedNode.Value = LocalizedString.FromInvalid();
            else if (newType == GFFFieldType.ResRef) _selectedNode.Value = ResRef.FromBlank();
            else if (newType == GFFFieldType.Vector3) _selectedNode.Value = Vector3.Zero;
            else if (newType == GFFFieldType.Vector4) _selectedNode.Value = Vector4.Zero;
            else if (newType == GFFFieldType.Binary) _selectedNode.Value = new byte[0];
            else if (newType == GFFFieldType.Struct)
            {
                int id = GetNextStructId();
                var gs = new GFFStruct(id);
                _selectedNode.Value = gs;
                _selectedNode.StructId = id;
                _selectedNode.Children.Clear();
            }
            else if (newType == GFFFieldType.List)
            {
                _selectedNode.Value = new GFFList();
                _selectedNode.Children.Clear();
            }
            MarkDirty();
            LoadItem(_selectedNode);
            RefreshItemText(_selectedNode);
            UpdateStatusBar();
        }

        private void SubstringSelected()
        {
            if (_substringList?.SelectedItem is SubstringListItem item)
            {
                _substringEdit.IsEnabled = true;
                _substringEdit.Text = item.Text ?? "";
            }
            else
            {
                _substringEdit.IsEnabled = false;
                _substringEdit.Text = "";
            }
        }

        private void SubstringEdited()
        {
            if (!(_selectedNode?.Value is LocalizedString locStr)) return;
            if (!(_substringList?.SelectedItem is SubstringListItem item)) return;
            LocalizedString.SubstringPair(item.Id, out Language lang, out Gender gender);
            locStr.SetData(lang, gender, _substringEdit?.Text ?? "");
            item.Text = _substringEdit?.Text ?? "";
            MarkDirty();
            RefreshItemText(_selectedNode);
        }

        private void AddSubstring()
        {
            if (!(_selectedNode?.Value is LocalizedString locStr)) return;
            if (_substringLangCombo?.SelectedItem == null || _substringGenderCombo?.SelectedItem == null) return;
            if (!Enum.TryParse(_substringLangCombo.SelectedItem.ToString(), out Language lang) ||
                !Enum.TryParse(_substringGenderCombo.SelectedItem.ToString(), out Gender gender)) return;
            int subId = LocalizedString.SubstringId(lang, gender);
            if (_substringItems != null)
            {
                foreach (var it in _substringItems)
                {
                    if (it.Id == subId) return;
                }
                PushState();
                _substringItems.Add(new SubstringListItem { Id = subId, Text = "", Display = $"{lang}, {gender}" });
            }
            locStr.SetData(lang, gender, "");
            MarkDirty();
            RefreshItemText(_selectedNode);
            UpdateStatusBar();
        }

        private void RemoveSubstring()
        {
            if (!(_selectedNode?.Value is LocalizedString locStr)) return;
            if (_substringLangCombo?.SelectedItem == null || _substringGenderCombo?.SelectedItem == null) return;
            if (!Enum.TryParse(_substringLangCombo.SelectedItem.ToString(), out Language lang) ||
                !Enum.TryParse(_substringGenderCombo.SelectedItem.ToString(), out Gender gender)) return;
            int subId = LocalizedString.SubstringId(lang, gender);
            if (_substringItems != null)
            {
                for (int i = _substringItems.Count - 1; i >= 0; i--)
                {
                    if (_substringItems[i].Id == subId)
                    {
                        PushState();
                        _substringItems.RemoveAt(i);
                        break;
                    }
                }
            }
            locStr.Remove(lang, gender);
            MarkDirty();
            RefreshItemText(_selectedNode);
            UpdateStatusBar();
        }

        private void ChangeLocStringText()
        {
            if (_tlkTextEdit == null) return;
            try
            {
                var talk = _installation?.TalkTable();
                if (talk != null && _stringrefSpin != null)
                {
                    int refId = (int)(_stringrefSpin.Value ?? -1);
                    var text = talk.GetString(refId);
                    _tlkTextEdit.Text = text ?? "";
                }
                else
                {
                    _tlkTextEdit.Text = "";
                }
            }
            catch
            {
                _tlkTextEdit.Text = "";
            }
        }

        private void CopyBinaryData()
        {
            if (_binaryHexLabel?.Text == null || _binaryHexLabel.Text == "(empty)") return;
            try
            {
                var topLevel = GetTopLevel(this);
                if (topLevel?.Clipboard != null)
                    _ = topLevel.Clipboard.SetTextAsync(_binaryHexLabel.Text);
            }
            catch { }
        }

        private void ShowValueConverterDialog()
        {
            if (_selectedNode == null) return;
            if (_selectedNode.FieldType == GFFFieldType.Binary)
            {
                var bytes = _selectedNode.Value as byte[] ?? new byte[0];
                var dialog = new Window
                {
                    Title = "Convert value (Binary)",
                    Width = 480,
                    Height = 320,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
                var formatLabel = new TextBlock { Text = "View/Edit as:" };
                var formatCombo = new ComboBox { ItemsSource = new[] { "Hex", "Base64", "ASCII" }, SelectedIndex = 0 };
                var textLabel = new TextBlock { Text = "Value:" };
                var editBox = new TextBox { AcceptsReturn = true, Height = 160, FontFamily = "Courier New" };
                string ToHex(byte[] b)
                {
                    if (b == null || b.Length == 0) return "";
                    return string.Join(" ", b.Select(x => x.ToString("X2")));
                }
                string ToBase64(byte[] b) => b == null || b.Length == 0 ? "" : Convert.ToBase64String(b);
                string ToAscii(byte[] b)
                {
                    if (b == null || b.Length == 0) return "";
                    return new string(b.Select(x => x >= 32 && x < 127 ? (char)x : '.').ToArray());
                }
                void RefreshEditBox()
                {
                    int idx = formatCombo.SelectedIndex;
                    if (idx == 0) editBox.Text = ToHex(bytes);
                    else if (idx == 1) editBox.Text = ToBase64(bytes);
                    else editBox.Text = ToAscii(bytes);
                }
                formatCombo.SelectionChanged += (s, e) => RefreshEditBox();
                RefreshEditBox();
                var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
                var okBtn = new Button { Content = "OK", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
                var cancelBtn = new Button { Content = "Cancel" };
                okBtn.Click += (s, e) =>
                {
                    try
                    {
                        int idx = formatCombo.SelectedIndex;
                        string text = editBox?.Text ?? "";
                        byte[] newBytes = null;
                        if (idx == 0)
                        {
                            var hex = new string(text.Where(c => char.IsLetterOrDigit(c)).ToArray());
                            if (hex.Length % 2 != 0) hex = "0" + hex;
                            newBytes = new byte[hex.Length / 2];
                            for (int i = 0; i < newBytes.Length; i++)
                                newBytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                        }
                        else if (idx == 1)
                        {
                            text = text.Trim();
                            if (string.IsNullOrEmpty(text)) newBytes = new byte[0];
                            else newBytes = Convert.FromBase64String(text);
                        }
                        else
                        {
                            newBytes = Encoding.UTF8.GetBytes(text ?? "");
                        }
                        PushState();
                        _selectedNode.Value = newBytes ?? new byte[0];
                        RefreshItemText(_selectedNode);
                        LoadItem(_selectedNode);
                        UpdateStatusBar();
                        dialog.Close();
                    }
                    catch (Exception ex)
                    {
                        var box = MessageBoxManager.GetMessageBoxStandard("Convert value", "Invalid value: " + ex.Message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                        _ = box.ShowWindowDialogAsync(this as Window);
                    }
                };
                cancelBtn.Click += (s, e) => dialog.Close();
                buttons.Children.Add(okBtn);
                buttons.Children.Add(cancelBtn);
                panel.Children.Add(formatLabel);
                panel.Children.Add(formatCombo);
                panel.Children.Add(textLabel);
                panel.Children.Add(editBox);
                panel.Children.Add(buttons);
                dialog.Content = panel;
                editBox.Focus();
                _ = dialog.ShowDialog(this as Window);
                return;
            }
            if (IsIntegerField(_selectedNode.FieldType))
            {
                var dialog = new Window
                {
                    Title = "View as Hex (Integer)",
                    Width = 320,
                    Height = 120,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                long val = Convert.ToInt64(_selectedNode.Value ?? 0);
                var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
                var decLabel = new TextBlock { Text = "Decimal: " + val };
                var hexLabel = new TextBlock { Text = "Hex: 0x" + val.ToString("X") };
                var closeBtn = new Button { Content = "Close", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left };
                closeBtn.Click += (s, e) => dialog.Close();
                panel.Children.Add(decLabel);
                panel.Children.Add(hexLabel);
                panel.Children.Add(closeBtn);
                dialog.Content = panel;
                _ = dialog.ShowDialog(this as Window);
            }
        }

        private static bool IsIntegerField(GFFFieldType ft)
        {
            return ft == GFFFieldType.Int8 || ft == GFFFieldType.Int16 || ft == GFFFieldType.Int32 || ft == GFFFieldType.Int64
                || ft == GFFFieldType.UInt8 || ft == GFFFieldType.UInt16 || ft == GFFFieldType.UInt32 || ft == GFFFieldType.UInt64;
        }

        private void InsertNode(GFFTreeNodeViewModel parent, string label, GFFFieldType ftype, object value)
        {
            if (parent == null) return;
            if (parent.Children == null) return;
            PushState();
            if (value is GFFStruct gs)
            {
                int id = GetNextStructId();
                gs.StructId = id;
            }
            var node = new GFFTreeNodeViewModel("", ftype, label, value);
            if (value is GFFStruct gs2) node.StructId = gs2.StructId;
            parent.Children.Add(node);
            RefreshItemText(node);
            RefreshItemText(parent);
            MarkDirty();
            UpdateStatusBar();
            ExpandNode(parent);
            _treeView.SelectedItem = node;
            _selectedNode = node;
            LoadItem(node);
            Dispatcher.UIThread.Post(() =>
            {
                SelectAndExpandTo(node);
                UpdateStatusBar();
            }, DispatcherPriority.Background);
        }

        /// <summary>Plist-pad style: insert a new sibling before the selected node.</summary>
        private void InsertSiblingBefore()
        {
            var node = GetSelectedNodeFromTree();
            var parent = GetParentOf(node);
            if (parent == null) return;
            PushState();
            var newNode = new GFFTreeNodeViewModel("", GFFFieldType.String, "New Field", "");
            int idx = parent.Children.IndexOf(node);
            parent.Children.Insert(idx, newNode);
            RefreshItemText(newNode);
            RefreshItemText(parent);
            MarkDirty();
            UpdateStatusBar();
            ExpandNode(parent);
            _treeView.SelectedItem = newNode;
            _selectedNode = newNode;
            LoadItem(newNode);
        }

        /// <summary>Plist-pad style: insert a new sibling after the selected node.</summary>
        private void InsertSiblingAfter()
        {
            var node = GetSelectedNodeFromTree();
            var parent = GetParentOf(node);
            if (parent == null) return;
            PushState();
            var newNode = new GFFTreeNodeViewModel("", GFFFieldType.String, "New Field", "");
            int idx = parent.Children.IndexOf(node);
            parent.Children.Insert(idx + 1, newNode);
            RefreshItemText(newNode);
            RefreshItemText(parent);
            MarkDirty();
            UpdateStatusBar();
            ExpandNode(parent);
            _treeView.SelectedItem = newNode;
            _selectedNode = newNode;
            LoadItem(newNode);
        }

        private void AddNode(GFFTreeNodeViewModel listNode)
        {
            if (listNode == null || listNode.Children == null) return;
            if (listNode.FieldType != GFFFieldType.List) return;
            PushState();
            int structId = GetNextStructId();
            var newStruct = new GFFStruct(structId);
            var node = new GFFTreeNodeViewModel("", GFFFieldType.Struct, null, newStruct) { StructId = structId };
            listNode.Children.Add(node);
            RefreshItemText(node);
            RefreshItemText(listNode);
            MarkDirty();
            UpdateStatusBar();
            ExpandNode(listNode);
            _treeView.SelectedItem = node;
            _selectedNode = node;
            LoadItem(node);
            Dispatcher.UIThread.Post(() =>
            {
                SelectAndExpandTo(node);
                UpdateStatusBar();
            }, DispatcherPriority.Background);
        }

        private void RemoveNode(GFFTreeNodeViewModel node)
        {
            var rootNode = GetRootNode();
            if (rootNode == null || node == null || node == rootNode) return;
            PushState();
            if (RemoveNodeFromParent(rootNode, node))
                MarkDirty();
            UpdateStatusBar();
        }

        private bool RemoveNodeFromParent(GFFTreeNodeViewModel parent, GFFTreeNodeViewModel toRemove)
        {
            if (parent.Children.Remove(toRemove))
            {
                RefreshItemText(parent);
                return true;
            }
            foreach (var child in parent.Children)
            {
                if (RemoveNodeFromParent(child, toRemove)) return true;
            }
            return false;
        }

        private void RemoveSelectedNodes()
        {
            var node = GetSelectedNodeFromTree();
            if (node != null)
            {
                RemoveNode(node);
                UpdateStatusBar();
            }
        }

        private void MoveNodeUp()
        {
            var node = GetSelectedNodeFromTree();
            if (node == null) return;
            var parent = GetParentOf(node);
            if (parent == null) return;
            int idx = parent.Children.IndexOf(node);
            if (idx <= 0) return;
            PushState();
            parent.Children.RemoveAt(idx);
            parent.Children.Insert(idx - 1, node);
            RefreshItemText(parent);
            MarkDirty();
            UpdateStatusBar();
        }

        private void MoveNodeDown()
        {
            var node = GetSelectedNodeFromTree();
            if (node == null) return;
            var parent = GetParentOf(node);
            if (parent == null) return;
            int idx = parent.Children.IndexOf(node);
            if (idx < 0 || idx >= parent.Children.Count - 1) return;
            PushState();
            parent.Children.RemoveAt(idx);
            parent.Children.Insert(idx + 1, node);
            RefreshItemText(parent);
            MarkDirty();
            UpdateStatusBar();
        }

        /// <summary>Sorts the selected node's children by label/display text (PlistOxide-style Sort).</summary>
        private void SortChildren()
        {
            var node = GetSelectedNodeFromTree();
            if (node?.Children == null || node.Children.Count < 2) return;
            PushState();
            var sorted = node.Children.OrderBy(c => GetSortKeyForNode(c), StringComparer.OrdinalIgnoreCase).ToList();
            node.Children.Clear();
            foreach (var c in sorted)
                node.Children.Add(c);
            RefreshItemText(node);
            MarkDirty();
            UpdateStatusBar();
        }

        private static string GetSortKeyForNode(GFFTreeNodeViewModel n)
        {
            return n.Label ?? n.Text ?? "";
        }

        private void UpdateStatusBar()
        {
            try
            {
                if (_statusText == null)
                    _statusText = this.FindControl<TextBlock>("statusText");
                if (_statusText == null) return;
                var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
                var rootNode = root?.FirstOrDefault();
                int totalNodes = CountNodes(rootNode);
                int structCount = CountStructs(rootNode);
                string baseText = $"{totalNodes} nodes";
                if (structCount > 0) baseText += $" | {structCount} structs";
                if (_undoStack.Count > 0) baseText += " | Undo";
                if (_redoStack.Count > 0) baseText += " | Redo";
                if (_findMatches.Count > 0 && !string.IsNullOrEmpty(_findQuery))
                {
                    int currentIdx = _findStartIndex > 0 ? _findStartIndex - 1 : 0;
                    if (currentIdx < 0) currentIdx = _findMatches.Count - 1;
                    baseText += $" | {_findMatches.Count} match{(_findMatches.Count == 1 ? "" : "es")}";
                }
                if (_selectedNode != null)
                {
                    string label = _selectedNode.Label ?? "(struct)";
                    string typeStr = _selectedNode.FieldType.ToString();
                    baseText += $" | Selected: {label} ({typeStr})";
                }
                else
                    baseText += " | No selection";
                _statusText.Text = baseText;
            }
            catch { }
        }

        private static int CountStructs(GFFTreeNodeViewModel node)
        {
            if (node == null) return 0;
            int n = node.FieldType == GFFFieldType.Struct ? 1 : 0;
            if (node.Children != null)
                foreach (var c in node.Children) n += CountStructs(c);
            return n;
        }

        private static int CountNodes(GFFTreeNodeViewModel node)
        {
            if (node == null) return 0;
            int n = 1;
            if (node.Children != null)
                foreach (var c in node.Children) n += CountNodes(c);
            return n;
        }

        private void PushState()
        {
            if (_undoRedoInProgress || _gff == null) return;
            try
            {
                var (data, _) = Build();
                if (data != null && data.Length > 0)
                {
                    _redoStack.Clear();
                    _undoStack.Add(data);
                    if (_undoStack.Count > UndoMaxLevels) _undoStack.RemoveAt(0);
                }
            }
            catch { }
        }

        private void ApplyState(byte[] data)
        {
            if (data == null || data.Length == 0) return;
            try
            {
                _gff = GFF.FromBytes(data);
                LoadGff(_gff);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"ApplyState failed: {ex}");
            }
        }

        public void Undo()
        {
            if (_undoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                var data = _undoStack[_undoStack.Count - 1];
                _undoStack.RemoveAt(_undoStack.Count - 1);
                try
                {
                    var (current, _) = Build();
                    if (current != null && current.Length > 0) _redoStack.Add(current);
                }
                catch { }
                ApplyState(data);
                UpdateStatusBar();
            }
            finally { _undoRedoInProgress = false; }
        }

        public void Redo()
        {
            if (_redoStack.Count == 0) return;
            _undoRedoInProgress = true;
            try
            {
                var data = _redoStack[_redoStack.Count - 1];
                _redoStack.RemoveAt(_redoStack.Count - 1);
                try
                {
                    var (current, _) = Build();
                    if (current != null && current.Length > 0) _undoStack.Add(current);
                }
                catch { }
                ApplyState(data);
                UpdateStatusBar();
            }
            finally { _undoRedoInProgress = false; }
        }

        private void CollectFindMatches(GFFTreeNodeViewModel node, string query, bool matchCase, bool inLabels, bool inValues, List<GFFTreeNodeViewModel> results)
        {
            if (node == null || string.IsNullOrEmpty(query)) return;
            string q = matchCase ? query : query.ToLowerInvariant();
            string label = node.Label ?? "";
            string valueStr = GetDisplayValueString(node);
            if (!matchCase) { label = label.ToLowerInvariant(); valueStr = valueStr.ToLowerInvariant(); }
            bool labelMatch = inLabels && label.Contains(q);
            bool valueMatch = inValues && valueStr.Contains(q);
            if (labelMatch || valueMatch) results.Add(node);
            if (node.Children != null)
                foreach (var c in node.Children) CollectFindMatches(c, query, matchCase, inLabels, inValues, results);
        }

        private void ShowFindDialog()
        {
            var dialog = new Window
            {
                Title = "Find in Tree",
                Width = 400,
                Height = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var findLabel = new TextBlock { Text = "Find what:" };
            var queryBox = new TextBox { Text = _findQuery, Watermark = "Search label or value" };
            var matchCaseCb = new CheckBox { Content = "Match case", IsChecked = _findMatchCase };
            var findInLabelsCb = new CheckBox { Content = "Search in labels", IsChecked = _findInLabels };
            var findInValuesCb = new CheckBox { Content = "Search in values", IsChecked = _findInValues };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var findNextBtn = new Button { Content = "Find Next", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var closeBtn = new Button { Content = "Close" };
            panel.Children.Add(findLabel);
            panel.Children.Add(queryBox);
            panel.Children.Add(matchCaseCb);
            panel.Children.Add(findInLabelsCb);
            panel.Children.Add(findInValuesCb);
            buttons.Children.Add(findNextBtn);
            buttons.Children.Add(closeBtn);
            panel.Children.Add(buttons);
            dialog.Content = panel;
            findNextBtn.Click += async (s, e) =>
            {
                _findQuery = queryBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                _findInLabels = findInLabelsCb.IsChecked == true;
                _findInValues = findInValuesCb.IsChecked == true;
                _findMatches.Clear();
                var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
                var rootNode = root?.FirstOrDefault();
                CollectFindMatches(rootNode, _findQuery, _findMatchCase, _findInLabels, _findInValues, _findMatches);
                _findStartIndex = 0;
                if (_findMatches.Count > 0)
                {
                    SelectAndExpandTo(_findMatches[0]);
                    _findStartIndex = 1;
                    dialog.Close();
                }
                else
                {
                    var msg = string.IsNullOrWhiteSpace(_findQuery)
                        ? "Enter text to search for."
                        : "No matches found.";
                    var box = MessageBoxManager.GetMessageBoxStandard("Find in Tree", msg, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info);
                    await box.ShowWindowDialogAsync(dialog);
                }
            };
            closeBtn.Click += (s, e) => dialog.Close();
            queryBox.Focus();
            _ = dialog.ShowDialog(this as Window);
        }

        private void FindNextMatch()
        {
            if (string.IsNullOrEmpty(_findQuery))
            {
                ShowFindDialog();
                return;
            }
            if (_findMatches.Count == 0)
            {
                var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
                var rootNode = root?.FirstOrDefault();
                CollectFindMatches(rootNode, _findQuery, _findMatchCase, _findInLabels, _findInValues, _findMatches);
                _findStartIndex = 0;
            }
            if (_findMatches.Count == 0)
            {
                _ = MessageBoxManager.GetMessageBoxStandard("Find in Tree", "No matches found.", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this as Window);
                return;
            }
            int idx = _findStartIndex % _findMatches.Count;
            SelectAndExpandTo(_findMatches[idx]);
            _findStartIndex = idx + 1;
        }

        private void FindPreviousMatch()
        {
            if (string.IsNullOrEmpty(_findQuery)) { ShowFindDialog(); return; }
            if (_findMatches.Count == 0)
            {
                var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
                var rootNode = root?.FirstOrDefault();
                CollectFindMatches(rootNode, _findQuery, _findMatchCase, _findInLabels, _findInValues, _findMatches);
                _findStartIndex = 0;
            }
            if (_findMatches.Count == 0)
            {
                _ = MessageBoxManager.GetMessageBoxStandard("Find in Tree", "No matches found.", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this as Window);
                return;
            }
            _findStartIndex = (_findStartIndex - 2 + _findMatches.Count) % _findMatches.Count;
            if (_findStartIndex < 0) _findStartIndex += _findMatches.Count;
            SelectAndExpandTo(_findMatches[_findStartIndex]);
            _findStartIndex++;
        }

        /// <summary>PropertyListEditor parity: use selected node's label or string value as find query and jump to first match.</summary>
        private void UseSelectionForFind()
        {
            var node = GetSelectedNodeFromTree();
            if (node == null) return;
            string query = null;
            if (!string.IsNullOrEmpty(node.Label)) query = node.Label;
            if (string.IsNullOrEmpty(query) && node.Value is string s && !string.IsNullOrEmpty(s)) query = s;
            if (string.IsNullOrEmpty(query))
            {
                _ = MessageBoxManager.GetMessageBoxStandard("Find in Tree", "Selection has no label or string value to search for.", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Info).ShowWindowDialogAsync(this as Window);
                return;
            }
            _findQuery = query;
            var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
            var rootNode = root?.FirstOrDefault();
            _findMatches.Clear();
            CollectFindMatches(rootNode, _findQuery, _findMatchCase, _findInLabels, _findInValues, _findMatches);
            _findStartIndex = 0;
            if (_findMatches.Count > 0)
            {
                SelectAndExpandTo(_findMatches[0]);
                _findStartIndex = 1;
            }
            UpdateStatusBar();
        }

        /// <summary>PropertyListEditor parity: scroll selection into view (Jump to Selection).</summary>
        private void GoToSelection()
        {
            var node = GetSelectedNodeFromTree();
            if (node == null || _treeView == null) return;
            SelectAndExpandTo(node);
            // Bring the TreeViewItem for this node into view
            var container = FindTreeViewItemForNode(_treeView, node);
            if (container != null)
                container.BringIntoView();
        }

        private static TreeViewItem FindTreeViewItemForNode(Control parent, GFFTreeNodeViewModel target)
        {
            if (parent == null || target == null) return null;
            if (parent is TreeViewItem tvi && tvi.DataContext == target)
                return tvi;
            foreach (var child in parent.GetVisualChildren())
            {
                if (child is Control c)
                {
                    var found = FindTreeViewItemForNode(c, target);
                    if (found != null) return found;
                }
            }
            return null;
        }

        private void SelectAndExpandTo(GFFTreeNodeViewModel target)
        {
            if (target == null || _treeView == null) return;
            _treeView.SelectedItem = target;
            ExpandAll(); // Expand so the selected node is visible
            _selectedNode = target;
            LoadItem(target);
            UpdateStatusBar();
        }

        private void ShowReplaceDialog()
        {
            var dialog = new Window
            {
                Title = "Replace in Tree",
                Width = 400,
                Height = 260,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var findLabel = new TextBlock { Text = "Find what:" };
            var findBox = new TextBox { Text = _findQuery, Watermark = "Search label or value" };
            var replaceLabel = new TextBlock { Text = "Replace with:" };
            var replaceBox = new TextBox { Text = _replaceText, Watermark = "Replacement" };
            var matchCaseCb = new CheckBox { Content = "Match case", IsChecked = _findMatchCase };
            var replaceInLabelsCb = new CheckBox { Content = "Replace in labels", IsChecked = _replaceInLabels };
            var replaceInValuesCb = new CheckBox { Content = "Replace in values", IsChecked = _replaceInValues };
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
            panel.Children.Add(replaceInLabelsCb);
            panel.Children.Add(replaceInValuesCb);
            buttons.Children.Add(findNextBtn);
            buttons.Children.Add(replaceOneBtn);
            buttons.Children.Add(replaceAllBtn);
            buttons.Children.Add(closeBtn);
            panel.Children.Add(buttons);
            dialog.Content = panel;
            findNextBtn.Click += (s, e) =>
            {
                _findQuery = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                _findMatches.Clear();
                var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
                CollectFindMatches(root?.FirstOrDefault(), _findQuery, _findMatchCase, _findInLabels, _findInValues, _findMatches);
                _findStartIndex = 0;
                if (_findMatches.Count > 0)
                {
                    SelectAndExpandTo(_findMatches[0]);
                    _findStartIndex = 1;
                }
            };
            replaceOneBtn.Click += (s, e) =>
            {
                _findQuery = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                _replaceInLabels = replaceInLabelsCb.IsChecked == true;
                _replaceInValues = replaceInValuesCb.IsChecked == true;
                ReplaceOne();
            };
            replaceAllBtn.Click += (s, e) =>
            {
                _findQuery = findBox.Text ?? "";
                _replaceText = replaceBox.Text ?? "";
                _findMatchCase = matchCaseCb.IsChecked == true;
                _replaceInLabels = replaceInLabelsCb.IsChecked == true;
                _replaceInValues = replaceInValuesCb.IsChecked == true;
                if (string.IsNullOrEmpty(_findQuery)) { dialog.Close(); return; }
                PushState();
                var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
                ReplaceInTree(root?.FirstOrDefault(), _findQuery, _replaceText, _findMatchCase, _replaceInLabels, _replaceInValues);
                RefreshAllNodeTexts(root?.FirstOrDefault());
                UpdateStatusBar();
                dialog.Close();
            };
            closeBtn.Click += (s, e) => dialog.Close();
            findBox.Focus();
            _ = dialog.ShowDialog(this as Window);
        }

        private void ReplaceOne()
        {
            if (string.IsNullOrEmpty(_findQuery)) return;
            if (_findMatches.Count == 0)
            {
                var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
                var rootNode = root?.FirstOrDefault();
                CollectFindMatches(rootNode, _findQuery, _findMatchCase, _findInLabels, _findInValues, _findMatches);
                _findStartIndex = 0;
            }
            if (_findMatches.Count == 0) return;
            int idx = _findStartIndex > 0 ? _findStartIndex - 1 : 0;
            var node = _findMatches[idx];
            bool changed = false;
            if (_replaceInLabels && node.Label != null && node.Label.IndexOf(_findQuery, _findMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (!changed) PushState();
                node.Label = _findMatchCase ? node.Label.Replace(_findQuery, _replaceText ?? "") : ReplaceIgnoreCase(node.Label, _findQuery, _replaceText ?? "");
                changed = true;
            }
            if (_replaceInValues && node.FieldType == GFFFieldType.String && node.Value is string str && str.IndexOf(_findQuery, _findMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (!changed) PushState();
                node.Value = _findMatchCase ? str.Replace(_findQuery, _replaceText ?? "") : ReplaceIgnoreCase(str, _findQuery, _replaceText ?? "");
                changed = true;
            }
            if (changed)
            {
                RefreshItemText(node);
                UpdateStatusBar();
            }
            _findStartIndex = idx + 1;
            if (_findStartIndex < _findMatches.Count)
                SelectAndExpandTo(_findMatches[_findStartIndex]);
        }

        private void ReplaceInTree(GFFTreeNodeViewModel node, string find, string replace, bool matchCase, bool replaceInLabels, bool replaceInValues)
        {
            if (node == null) return;
            if (replaceInLabels && node.Label != null)
            {
                string s = node.Label;
                node.Label = matchCase ? s.Replace(find, replace) : ReplaceIgnoreCase(s, find, replace);
            }
            if (replaceInValues && node.FieldType == GFFFieldType.String && node.Value is string str)
            {
                node.Value = matchCase ? str.Replace(find, replace) : ReplaceIgnoreCase(str, find, replace);
            }
            if (node.Children != null)
                foreach (var c in node.Children) ReplaceInTree(c, find, replace, matchCase, replaceInLabels, replaceInValues);
        }

        private static string ReplaceIgnoreCase(string text, string find, string replace)
        {
            if (string.IsNullOrEmpty(find)) return text;
            int i = text.IndexOf(find, StringComparison.OrdinalIgnoreCase);
            if (i < 0) return text;
            return text.Substring(0, i) + replace + ReplaceIgnoreCase(text.Substring(i + find.Length), find, replace);
        }

        /// <summary>Plist-pad style: copy then remove selected node.</summary>
        private void CutNode()
        {
            var node = GetSelectedNodeFromTree();
            if (node == null) return;
            var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
            var rootNode = root?.FirstOrDefault();
            if (rootNode == null || node == rootNode) return;
            _copiedNode = DeepCloneNode(node);
            RemoveNode(node);
        }

        private void CopyNode()
        {
            var node = GetSelectedNodeFromTree();
            if (node == null) return;
            _copiedNode = DeepCloneNode(node);
        }

        private void PasteNode()
        {
            if (_copiedNode == null) return;
            var parent = GetSelectedNodeFromTree();
            if (parent == null) return;
            GFFTreeNodeViewModel targetParent = parent.FieldType == GFFFieldType.List ? parent : GetParentOf(parent);
            if (targetParent == null) return;
            PushState();
            var clone = DeepCloneNode(_copiedNode);
            AssignNewStructIds(clone);
            targetParent.Children.Add(clone);
            RefreshAllNodeTexts(clone);
            RefreshItemText(targetParent);
            MarkDirty();
            UpdateStatusBar();
            ExpandNode(targetParent);
            _treeView.SelectedItem = clone;
            _selectedNode = clone;
            LoadItem(clone);
        }

        private void DuplicateNode()
        {
            var node = GetSelectedNodeFromTree();
            if (node == null) return;
            var parent = GetParentOf(node);
            if (parent == null) return;
            PushState();
            var clone = DeepCloneNode(node);
            AssignNewStructIds(clone);
            int idx = parent.Children.IndexOf(node);
            if (idx >= 0) parent.Children.Insert(idx + 1, clone);
            else parent.Children.Add(clone);
            RefreshAllNodeTexts(clone);
            RefreshItemText(parent);
            MarkDirty();
            UpdateStatusBar();
            ExpandNode(parent);
            _treeView.SelectedItem = clone;
            _selectedNode = clone;
            LoadItem(clone);
        }

        private GFFTreeNodeViewModel GetRootNode()
        {
            var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
            return root?.FirstOrDefault();
        }

        private GFFTreeNodeViewModel GetParentOf(GFFTreeNodeViewModel target)
        {
            var rootNode = GetRootNode();
            return FindParent(rootNode, target);
        }

        /// <summary>Returns the next available struct ID (max existing + 1) for new structs.</summary>
        private int GetNextStructId()
        {
            var root = GetRootNode();
            int max = -1;
            void Walk(GFFTreeNodeViewModel n)
            {
                if (n == null) return;
                if (n.FieldType == GFFFieldType.Struct && n.StructId > max) max = n.StructId;
                if (n.Children != null)
                    foreach (var c in n.Children) Walk(c);
            }
            Walk(root);
            return max + 1;
        }

        /// <summary>Expands the TreeViewItem for the given node so its children are visible.</summary>
        private void ExpandNode(GFFTreeNodeViewModel node)
        {
            if (node == null || _treeView == null) return;
            var tvi = FindTreeViewItemForNode(_treeView, node);
            if (tvi != null) tvi.IsExpanded = true;
        }

        private static GFFTreeNodeViewModel FindParent(GFFTreeNodeViewModel node, GFFTreeNodeViewModel target)
        {
            if (node?.Children == null) return null;
            if (node.Children.Contains(target)) return node;
            foreach (var c in node.Children)
            {
                var found = FindParent(c, target);
                if (found != null) return found;
            }
            return null;
        }

        private static GFFTreeNodeViewModel DeepCloneNode(GFFTreeNodeViewModel node)
        {
            if (node == null) return null;
            object value = node.Value;
            if (value is GFFStruct gs)
            {
                var newStruct = new GFFStruct(gs.StructId);
                value = newStruct;
            }
            else if (value is GFFList gl)
            {
                var newList = new GFFList();
                value = newList;
            }
            else if (value is byte[] arr) value = (byte[])arr.Clone();
            else if (value is Vector3 v3) value = v3;
            else if (value is Vector4 v4) value = v4;
            else if (value is LocalizedString ls)
            {
                var dict = new Dictionary<int, string>();
                foreach (var (lang, gender, text) in ls) dict[LocalizedString.SubstringId(lang, gender)] = text;
                value = new LocalizedString(ls.StringRef, dict);
            }
            else if (value is ResRef rr) value = new ResRef(rr.ToString());
            var clone = new GFFTreeNodeViewModel(node.Text, node.FieldType, node.Label, value) { StructId = node.StructId };
            foreach (var child in node.Children)
                clone.Children.Add(DeepCloneNode(child));
            return clone;
        }

        /// <summary>Assigns new unique struct IDs to all struct nodes under the given node (for paste/duplicate).</summary>
        private void AssignNewStructIds(GFFTreeNodeViewModel node)
        {
            if (node == null) return;
            if (node.FieldType == GFFFieldType.Struct && node.Value is GFFStruct gs)
            {
                int id = GetNextStructId();
                gs.StructId = id;
                node.StructId = id;
            }
            if (node.Children != null)
                foreach (var c in node.Children) AssignNewStructIds(c);
        }

        public void ExpandAll()
        {
            if (_treeView == null) return;
            SetExpandAllTreeViewItems(_treeView, true);
        }

        /// <summary>Collapse all nodes; keep root expanded (plist-pad behavior).</summary>
        public void CollapseAll()
        {
            if (_treeView == null) return;
            SetExpandAllTreeViewItems(_treeView, false);
            var rootNode = (_treeView.ItemsSource as IEnumerable<GFFTreeNodeViewModel>)?.FirstOrDefault();
            if (rootNode != null)
            {
                foreach (var v in _treeView.GetVisualDescendants())
                {
                    if (v is TreeViewItem tvi && ReferenceEquals(tvi.DataContext, rootNode))
                    {
                        tvi.IsExpanded = true;
                        break;
                    }
                }
            }
        }

        private static void SetExpandAllTreeViewItems(Control parent, bool expand)
        {
            if (parent == null) return;
            if (parent is TreeViewItem tvi)
            {
                tvi.IsExpanded = expand;
            }
            if (parent is Panel panel)
            {
                foreach (var child in panel.Children.OfType<Control>())
                    SetExpandAllTreeViewItems(child, expand);
            }
            else if (parent is ContentControl cc && cc.Content is Control content)
            {
                SetExpandAllTreeViewItems(content, expand);
            }
            else if (parent is ItemsControl ic && ic.Items != null)
            {
                foreach (var item in ic.Items.OfType<Control>())
                    SetExpandAllTreeViewItems(item, expand);
            }
        }

        private void ShowGoToStructIdDialog()
        {
            var dialog = new Window
            {
                Title = "Go to Struct ID",
                Width = 320,
                Height = 120,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var panel = new StackPanel { Margin = new Avalonia.Thickness(12) };
            var label = new TextBlock { Text = "Struct ID (0–4294967295):" };
            var spin = new NumericUpDown { Minimum = 0, Maximum = 0xFFFFFFFF, Value = 0 };
            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Avalonia.Thickness(0, 8, 0, 0) };
            var goBtn = new Button { Content = "Go", Margin = new Avalonia.Thickness(0, 0, 8, 0) };
            var cancelBtn = new Button { Content = "Cancel" };
            goBtn.Click += async (s, e) =>
            {
                int id = (int)(spin.Value ?? 0);
                var root = _treeView?.ItemsSource as IEnumerable<GFFTreeNodeViewModel>;
                var found = FindStructById(root?.FirstOrDefault(), id);
                if (found != null)
                {
                    SelectAndExpandTo(found);
                    dialog.Close();
                }
                else
                {
                    var box = MessageBoxManager.GetMessageBoxStandard("Go to Struct ID", $"Struct ID {id} not found.", ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Warning);
                    await box.ShowWindowDialogAsync(dialog);
                }
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

        private static GFFTreeNodeViewModel FindStructById(GFFTreeNodeViewModel node, int structId)
        {
            if (node == null) return null;
            if (node.FieldType == GFFFieldType.Struct && node.StructId == structId) return node;
            if (node.Children != null)
                foreach (var c in node.Children)
                {
                    var found = FindStructById(c, structId);
                    if (found != null) return found;
                }
            return null;
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }
    }
}
