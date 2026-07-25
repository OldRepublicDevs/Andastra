using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using IconType = MsBox.Avalonia.Enums.Icon;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Utils;

namespace OdyTools.Editors.GUI
{
    /// <summary>
    /// Visual KotOR GUI editor.
    /// Load .gui (GFF), set UI image folder for TPC/TGA, edit in tree + visual preview + property panel, save with backup.
    /// </summary>
    public partial class OdyToolGUI : Editor
    {
        private const int UndoMaxLevels = 50;
        private GFF _gff;
        private GFFStruct _selectedNode;
        private string _assetPath;
        private GUITextureCache _textureCache;
        private TreeView _treeView;
        private GUIPreviewControl _preview;
        private ScrollViewer _propertyScroll;
        private StackPanel _propertyPanel;
        private TextBox _assetPathBox;
        private TextBlock _statusText;
        private DockPanel _mainDock;
        private bool _darkMode;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private int _undoIndex = -1;
        private bool _undoRedoInProgress;

        internal string SelectedControlTagForTest => _selectedNode?.GetString(OdyToolGUIHelpers.TagLabel) ?? "";
        internal bool HasSelectedControlPropertyPanelForTest => _selectedNode != null && _propertyPanel?.Children?.Count > 1;

        public OdyToolGUI() : this(null, null) { }
        public OdyToolGUI(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolGUI", "none", new[] { ResourceType.GUI }, new[] { ResourceType.GUI }, installation)
        {
            MinWidth = 900;
            MinHeight = 500;
            Width = 1200;
            Height = 700;
            BuildUI();
            New();
        }

        private void BuildUI()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
            _mainDock = new DockPanel();
            var mainDock = _mainDock;
            var menu = new Menu();
            var fileMenu = new MenuItem { Header = "_File" };
            var openItem = new MenuItem { Header = "_Open" }; openItem.Click += (s, e) => OpenFile(); fileMenu.Items.Add(openItem);
            var saveItem = new MenuItem { Header = "_Save" }; saveItem.Click += (s, e) => Save(); fileMenu.Items.Add(saveItem);
            fileMenu.Items.Add(new Separator());
            var revertItem = new MenuItem { Header = "Revert to _Saved" }; revertItem.Click += (s, e) => Revert(); fileMenu.Items.Add(revertItem);
            fileMenu.Items.Add(new Separator());
            var exitItem = new MenuItem { Header = "E_xit" }; exitItem.Click += (s, e) => Close(); fileMenu.Items.Add(exitItem);
            menu.Items.Add(fileMenu);
            var editMenu = new MenuItem { Header = "_Edit" };
            var undoItem = new MenuItem { Header = "_Undo" }; undoItem.Click += (s, e) => Undo(); editMenu.Items.Add(undoItem);
            var redoItem = new MenuItem { Header = "_Redo" }; redoItem.Click += (s, e) => Redo(); editMenu.Items.Add(redoItem);
            editMenu.Items.Add(new Separator());
            var addRootItem = new MenuItem { Header = "Add _Root Control" }; addRootItem.Click += (s, e) => AddRootControl(); editMenu.Items.Add(addRootItem);
            var addChildItem = new MenuItem { Header = "Add _Child Control" }; addChildItem.Click += (s, e) => AddChildControl(); editMenu.Items.Add(addChildItem);
            var duplicateItem = new MenuItem { Header = "_Duplicate Control" }; duplicateItem.Click += (s, e) => DuplicateSelectedControl(); editMenu.Items.Add(duplicateItem);
            var deleteItem = new MenuItem { Header = "_Delete Control" }; deleteItem.Click += (s, e) => DeleteSelectedControl(); editMenu.Items.Add(deleteItem);
            menu.Items.Add(editMenu);
            DockPanel.SetDock(menu, Dock.Top);
            mainDock.Children.Add(menu);

            var toolbar = new Panel { Height = 36, Margin = new Thickness(4, 2) };
            var toolStack = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            toolStack.Children.Add(new TextBlock { Text = "UI Image Path:", VerticalAlignment = VerticalAlignment.Center });
            _assetPathBox = new TextBox { Width = 280, Watermark = "Folder with .tpc/.tga textures" };
            _assetPathBox.LostFocus += (s, e) => ApplyAssetPath();
            toolStack.Children.Add(_assetPathBox);
            var browseBtn = new Button { Content = "Browse..." };
            browseBtn.Click += async (s, e) =>
            {
                var storage = (this as Window)?.StorageProvider;
                if (storage == null) return;
                var folders = await storage.OpenFolderPickerAsync(new FolderPickerOpenOptions { Title = "Select UI texture folder" });
                if (folders?.Count > 0)
                {
                    _assetPathBox.Text = folders[0].Path.LocalPath;
                    ApplyAssetPath();
                }
            };
            toolStack.Children.Add(browseBtn);
            var reloadBtn = new Button { Content = "Reload Images" };
            reloadBtn.Click += (s, e) => ReloadTextures();
            toolStack.Children.Add(reloadBtn);
            var addRootBtn = new Button { Content = "Add Root" };
            addRootBtn.Click += (s, e) => AddRootControl();
            toolStack.Children.Add(addRootBtn);
            var addChildBtn = new Button { Content = "Add Child" };
            addChildBtn.Click += (s, e) => AddChildControl();
            toolStack.Children.Add(addChildBtn);
            var duplicateBtn = new Button { Content = "Duplicate" };
            duplicateBtn.Click += (s, e) => DuplicateSelectedControl();
            toolStack.Children.Add(duplicateBtn);
            var deleteBtn = new Button { Content = "Delete" };
            deleteBtn.Click += (s, e) => DeleteSelectedControl();
            toolStack.Children.Add(deleteBtn);
            var revertBtn = new Button { Content = "Revert" };
            revertBtn.Click += (s, e) => Revert();
            toolStack.Children.Add(revertBtn);
            var saveBtn = new Button { Content = "Save" };
            saveBtn.Click += (s, e) => Save();
            var darkToggle = new CheckBox { Content = "Dark mode", VerticalAlignment = VerticalAlignment.Center };
            darkToggle.IsCheckedChanged += (s, e) => { _darkMode = darkToggle.IsChecked == true; ApplyDarkMode(); };
            toolStack.Children.Add(darkToggle);
            toolbar.Children.Add(toolStack);
            DockPanel.SetDock(toolbar, Dock.Top);
            mainDock.Children.Add(toolbar);

            var grid = new Grid();
            grid.ColumnDefinitions = new ColumnDefinitions("250,1*,225");
            _treeView = new TreeView();
            _treeView.ItemTemplate = new FuncTreeDataTemplate<GuiNodeViewModel>(
                (node, _) => new TextBlock { Text = node?.Label ?? "(no name)" },
                node => node?.Children ?? (IEnumerable)Array.Empty<GuiNodeViewModel>());
            _treeView.SelectionChanged += (s, e) =>
            {
                if (_treeView.SelectedItem is GuiNodeViewModel vm)
                {
                    _selectedNode = vm.Data;
                    _preview.SelectedNode = _selectedNode;
                    RefreshPropertyPanel();
                }
            };
            Grid.SetColumn(_treeView, 0);
            grid.Children.Add(_treeView);

            _preview = new GUIPreviewControl();
            _preview.SelectionChanged += node =>
            {
                _selectedNode = node;
                RefreshPropertyPanel();
                SyncTreeSelectionToNode(node);
            };
            _preview.DataChanged += () => { PushState(); MarkDocumentDirty(); _preview.InvalidateVisual(); };
            Grid.SetColumn(_preview, 1);
            grid.Children.Add(_preview);

            _propertyPanel = new StackPanel { Spacing = 4, Margin = new Thickness(8) };
            _propertyScroll = new ScrollViewer { Content = _propertyPanel, HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            var propBorder = new Border { Child = _propertyScroll, BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1), Padding = new Thickness(4) };
            Grid.SetColumn(propBorder, 2);
            grid.Children.Add(propBorder);

            mainDock.Children.Add(grid);

            _statusText = new TextBlock { Text = "OdyToolGUI", Margin = new Thickness(8, 4), VerticalAlignment = VerticalAlignment.Center };
            var statusBorder = new Border { Child = _statusText, Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(0xE1, 0xE1, 0xE1)), Padding = new Thickness(8, 4) };
            DockPanel.SetDock(statusBorder, Dock.Bottom);
            mainDock.Children.Add(statusBorder);

            var contentRoot = EditorHelpers.FindControlSafe<ContentControl>(this, "contentRoot");
            if (contentRoot != null) contentRoot.Content = mainDock; else Content = mainDock;
            ApplyDarkMode();

            KeyDown += (s, e) =>
            {
                if (e.Key == Key.S && (e.KeyModifiers & KeyModifiers.Control) != 0) { Save(); e.Handled = true; }
                if (e.Key == Key.Z && (e.KeyModifiers & KeyModifiers.Control) != 0) { Undo(); e.Handled = true; }
                if (e.Key == Key.Y && (e.KeyModifiers & KeyModifiers.Control) != 0) { Redo(); e.Handled = true; }
            };
        }

        private void SyncTreeSelectionToNode(GFFStruct node)
        {
            if (node == null) return;
            var vm = FindViewModelByData(_treeView.ItemsSource as IEnumerable<GuiNodeViewModel>, node);
            if (vm != null)
                _treeView.SelectedItem = vm;
        }

        private GuiNodeViewModel FindViewModelByData(IEnumerable<GuiNodeViewModel> items, GFFStruct data)
        {
            if (items == null) return null;
            foreach (var item in items)
            {
                if (item?.Data == data) return item;
                var found = FindViewModelByData(item?.Children, data);
                if (found != null) return found;
            }
            return null;
        }

        internal bool SelectControlByTagForTest(string tag)
        {
            var node = FindControlByTag(_gff?.Root, tag);
            if (node == null)
            {
                return false;
            }

            _selectedNode = node;
            _preview.SelectedNode = node;
            SyncTreeSelectionToNode(node);
            RefreshPropertyPanel();
            return true;
        }

        internal bool EditSelectedControlForTest(string tag, int left, int top, int width, int height)
        {
            if (_selectedNode == null)
            {
                return false;
            }

            PushState();
            _selectedNode.SetString(OdyToolGUIHelpers.TagLabel, tag ?? "");
            OdyToolGUIHelpers.SetExtentValues(_selectedNode, left, top, width, height);
            MarkDocumentDirty();
            LoadTreeAndPreview();
            _preview.SelectedNode = _selectedNode;
            SyncTreeSelectionToNode(_selectedNode);
            RefreshPropertyPanel();
            _preview.InvalidateVisual();
            return true;
        }

        internal bool EditSelectedResRefFieldForTest(string label, string value)
        {
            if (_selectedNode == null || string.IsNullOrWhiteSpace(label))
            {
                return false;
            }

            ApplyResRefFieldEdit(_selectedNode, label, value);
            return true;
        }

        internal void AddRootControlForTest() => AddRootControl();

        internal bool AddChildControlForTest()
        {
            int before = CountControls(_gff?.Root);
            AddChildControl();
            return CountControls(_gff?.Root) > before;
        }

        internal bool DuplicateSelectedControlForTest()
        {
            int before = CountControls(_gff?.Root);
            DuplicateSelectedControl();
            return CountControls(_gff?.Root) > before;
        }

        internal bool DeleteSelectedControlForTest()
        {
            int before = CountControls(_gff?.Root);
            DeleteSelectedControl();
            return CountControls(_gff?.Root) < before;
        }

        internal int ControlCountForTest => CountControls(_gff?.Root);

        private void AddRootControl()
        {
            if (_gff == null)
                _gff = new GFF(GFFContent.GUI);

            PushState();
            var controls = EnsureControlList(_gff.Root);
            var control = CreateDefaultControl(NextControlTag("control"));
            controls.Add(control);
            CommitControlListChange(control);
        }

        private void AddChildControl()
        {
            if (_selectedNode == null)
            {
                AddRootControl();
                return;
            }

            PushState();
            var children = EnsureControlList(_selectedNode);
            var control = CreateDefaultControl(NextControlTag($"{SanitizeTag(_selectedNode.GetString(OdyToolGUIHelpers.TagLabel), "control")}_child"));
            children.Add(control);
            CommitControlListChange(control);
        }

        private void DuplicateSelectedControl()
        {
            if (_selectedNode == null)
                return;

            var parentList = FindOwningControlList(_gff?.Root, _selectedNode);
            if (parentList == null)
                return;

            PushState();
            var copy = CloneStruct(_selectedNode);
            string sourceTag = _selectedNode.GetString(OdyToolGUIHelpers.TagLabel);
            copy.SetString(OdyToolGUIHelpers.TagLabel, NextControlTag($"{SanitizeTag(sourceTag, "control")}_copy"));
            OdyToolGUIHelpers.GetExtentValues(copy, out int left, out int top, out int width, out int height);
            OdyToolGUIHelpers.SetExtentValues(copy, left + 16, top + 16, width, height);
            parentList.Add(copy);
            CommitControlListChange(copy);
        }

        private void DeleteSelectedControl()
        {
            if (_selectedNode == null)
                return;

            var parentList = FindOwningControlList(_gff?.Root, _selectedNode);
            if (parentList == null)
                return;

            for (int i = 0; i < parentList.Count; i++)
            {
                if (parentList.At(i) != _selectedNode)
                    continue;

                PushState();
                parentList.Remove(i);
                _selectedNode = null;
                CommitControlListChange(null);
                return;
            }
        }

        private void CommitControlListChange(GFFStruct selected)
        {
            _selectedNode = selected;
            MarkDocumentDirty();
            LoadTreeAndPreview();
            _preview.SelectedNode = selected;
            SyncTreeSelectionToNode(selected);
            RefreshPropertyPanel();
            UpdateStatusBar();
        }

        private GFFList EnsureControlList(GFFStruct node)
        {
            if (node == null)
                return new GFFList();

            if (!node.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var controls))
            {
                controls = new GFFList();
                node.SetList(OdyToolGUIHelpers.ControllistLabel, controls);
            }

            return controls;
        }

        private static GFFStruct CreateDefaultControl(string tag)
        {
            var control = new GFFStruct(0);
            control.SetString(OdyToolGUIHelpers.TagLabel, tag);
            OdyToolGUIHelpers.SetExtentValues(control, 16, 16, 160, 48);
            control.SetList(OdyToolGUIHelpers.ControllistLabel, new GFFList());
            return control;
        }

        private string NextControlTag(string requestedBase)
        {
            string baseTag = SanitizeTag(requestedBase, "control");
            var existing = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CollectControlTags(_gff?.Root, existing);

            if (!existing.Contains(baseTag))
                return baseTag;

            for (int i = 2; i < 10000; i++)
            {
                string candidate = $"{baseTag}_{i}";
                if (!existing.Contains(candidate))
                    return candidate;
            }

            return $"{baseTag}_{DateTime.UtcNow.Ticks}";
        }

        private static string SanitizeTag(string value, string fallback)
        {
            string tag = string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();
            tag = tag.Replace(' ', '_');
            return tag.Length <= 64 ? tag : tag.Substring(0, 64);
        }

        private static void CollectControlTags(GFFStruct node, HashSet<string> tags)
        {
            if (node == null || tags == null)
                return;

            string tag = node.GetString(OdyToolGUIHelpers.TagLabel);
            if (!string.IsNullOrWhiteSpace(tag))
                tags.Add(tag);

            var children = OdyToolGUIHelpers.GetChildren(node);
            if (children != null)
            {
                foreach (var child in children)
                    CollectControlTags(child, tags);
            }

            CollectControlTags(OdyToolGUIHelpers.GetProtoItem(node), tags);
            CollectControlTags(OdyToolGUIHelpers.GetScrollBar(node), tags);
        }

        private static int CountControls(GFFStruct node)
        {
            if (node == null)
                return 0;

            int total = 0;
            var children = OdyToolGUIHelpers.GetChildren(node);
            if (children != null)
            {
                foreach (var child in children)
                    total += 1 + CountControls(child);
            }

            total += CountControls(OdyToolGUIHelpers.GetProtoItem(node));
            total += CountControls(OdyToolGUIHelpers.GetScrollBar(node));
            return total;
        }

        private static GFFList FindOwningControlList(GFFStruct root, GFFStruct target)
        {
            if (root == null || target == null)
                return null;

            var children = OdyToolGUIHelpers.GetChildren(root);
            if (children != null)
            {
                foreach (var child in children)
                {
                    if (child == target)
                        return children;

                    var nested = FindOwningControlList(child, target);
                    if (nested != null)
                        return nested;
                }
            }

            var protoOwner = FindOwningControlList(OdyToolGUIHelpers.GetProtoItem(root), target);
            if (protoOwner != null)
                return protoOwner;

            return FindOwningControlList(OdyToolGUIHelpers.GetScrollBar(root), target);
        }

        private static GFFStruct CloneStruct(GFFStruct source)
        {
            var clone = new GFFStruct(source?.StructId ?? 0);
            if (source == null)
                return clone;

            foreach (var (label, fieldType, value) in source)
                clone.SetField(label, fieldType, CloneFieldValue(fieldType, value));

            return clone;
        }

        private static object CloneFieldValue(GFFFieldType fieldType, object value)
        {
            switch (fieldType)
            {
                case GFFFieldType.Struct:
                    return CloneStruct(value as GFFStruct);
                case GFFFieldType.List:
                    var clonedList = new GFFList();
                    if (value is GFFList sourceList)
                    {
                        foreach (var item in sourceList)
                            clonedList.Add(CloneStruct(item));
                    }
                    return clonedList;
                case GFFFieldType.Binary:
                    return value is byte[] bytes ? (byte[])bytes.Clone() : Array.Empty<byte>();
                default:
                    return value;
            }
        }

        private static GFFStruct FindControlByTag(GFFStruct node, string tag)
        {
            if (node == null)
            {
                return null;
            }

            if (string.Equals(node.GetString(OdyToolGUIHelpers.TagLabel), tag, StringComparison.OrdinalIgnoreCase))
            {
                return node;
            }

            GFFStruct found = FindControlByTag(OdyToolGUIHelpers.GetProtoItem(node), tag);
            if (found != null)
            {
                return found;
            }

            found = FindControlByTag(OdyToolGUIHelpers.GetScrollBar(node), tag);
            if (found != null)
            {
                return found;
            }

            var children = OdyToolGUIHelpers.GetChildren(node);
            if (children != null)
            {
                foreach (var child in children)
                {
                    found = FindControlByTag(child, tag);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            return null;
        }

        private void ApplyDarkMode()
        {
            if (_mainDock == null) return;
            if (_darkMode)
            {
                _mainDock.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(0x2d, 0x2d, 0x30));
            }
            else
            {
                _mainDock.Background = Brushes.Transparent;
            }
        }

        private void ApplyAssetPath()
        {
            _assetPath = _assetPathBox?.Text?.Trim() ?? string.Empty;
            ReloadTextures();
        }

        private void ReloadTextures()
        {
            _textureCache?.Clear();
            _textureCache = string.IsNullOrEmpty(_assetPath) ? null : new GUITextureCache(_assetPath);
            _preview.TextureCache = _textureCache;
            if (_gff != null)
            {
                var refs = new HashSet<string>();
                if (_gff.Root.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var list))
                    foreach (var s in list)
                        OdyToolGUIHelpers.CollectFillResRefs(s, refs);
                foreach (var r in refs)
                    _textureCache?.GetBitmap(r);
            }
            _preview?.InvalidateVisual();
        }

        private void RefreshPropertyPanel()
        {
            _propertyPanel.Children.Clear();
            if (_selectedNode == null)
            {
                _propertyPanel.Children.Add(new TextBlock { Text = "Nothing selected" });
                return;
            }
            _propertyPanel.Children.Add(new TextBlock { Text = "Node Properties", FontWeight = FontWeight.Bold });
            // Struct ID row: show -1 for 0xFFFFFFFF (kotor-gui-editor style)
            var idRow = CreateIdRow();
            if (idRow != null) _propertyPanel.Children.Add(idRow);
            foreach (var (label, fieldType, value) in _selectedNode)
            {
                if (label == OdyToolGUIHelpers.ProtoItemLabel || label == OdyToolGUIHelpers.ScrollbarLabel) continue;
                var row = CreatePropertyRow(label, fieldType, value);
                if (row != null) _propertyPanel.Children.Add(row);
            }
        }

        private Panel CreateIdRow()
        {
            var sp = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(0, 2) };
            sp.Children.Add(new TextBlock { Text = "ID (StructId)" });
            int raw = _selectedNode.StructId;
            long displayValue = raw < 0 ? (long)(uint)raw : raw; // show 4294967295 when -1
            var idBox = new NumericUpDown { Value = displayValue, Minimum = -1, Maximum = uint.MaxValue, Increment = 1 };
            idBox.ValueChanged += (s, e) =>
            {
                var v = (long)((NumericUpDown)s).Value;
                _selectedNode.StructId = v == uint.MaxValue || v == -1 ? -1 : (int)v;
                MarkDocumentDirty();
            };
            sp.Children.Add(idBox);
            return sp;
        }

        private Panel CreatePropertyRow(string label, GFFFieldType fieldType, object value)
        {
            var sp = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(0, 2) };
            sp.Children.Add(new TextBlock { Text = $"{label} ({fieldType})" });
            Control input = null;
            switch (fieldType)
            {
                case GFFFieldType.Int8:
                case GFFFieldType.Int16:
                case GFFFieldType.Int32:
                    var i = Convert.ToInt32(value ?? 0);
                    var intBox = new NumericUpDown { Value = i, Minimum = label == "WIDTH" || label == "HEIGHT" ? 1 : decimal.MinValue, Maximum = decimal.MaxValue };
                    intBox.ValueChanged += (s, e) => { _selectedNode.SetInt32(label, (int)((NumericUpDown)s).Value); MarkDocumentDirty(); _preview.InvalidateVisual(); };
                    input = intBox;
                    break;
                case GFFFieldType.UInt16:
                case GFFFieldType.UInt32:
                    var u = Convert.ToUInt32(value ?? 0);
                    var uintBox = new NumericUpDown { Value = u, Minimum = 0, Maximum = uint.MaxValue };
                    uintBox.ValueChanged += (s, e) => { _selectedNode.SetUInt32(label, (uint)((NumericUpDown)s).Value); MarkDocumentDirty(); _preview.InvalidateVisual(); };
                    input = uintBox;
                    break;
                case GFFFieldType.Single:
                case GFFFieldType.Double:
                    var d = Convert.ToDouble(value ?? 0);
                    var doubleBox = new NumericUpDown { Value = (decimal)d, Increment = 0.01M };
                    if (label == "ALPHA" || label == "ROTATE") { doubleBox.Minimum = 0; doubleBox.Maximum = 1; }
                    else doubleBox.Minimum = decimal.MinValue;
                    doubleBox.ValueChanged += (s, e) => { _selectedNode.SetDouble(label, (double)((NumericUpDown)s).Value); MarkDocumentDirty(); _preview.InvalidateVisual(); };
                    input = doubleBox;
                    break;
                case GFFFieldType.String:
                    var str = value?.ToString() ?? "";
                    var txt = new TextBox { Text = str };
                    txt.LostFocus += (s, e) => { _selectedNode.SetString(label, ((TextBox)s).Text ?? ""); MarkDocumentDirty(); };
                    input = txt;
                    break;
                case GFFFieldType.ResRef:
                    var resRef = value as ResRef ?? ResRef.FromBlank();
                    var resTxt = new TextBox { Text = resRef.ToString() ?? "", MaxLength = 16 };
                    resTxt.LostFocus += (s, e) => ApplyResRefFieldEdit(_selectedNode, label, ((TextBox)s).Text);
                    input = resTxt;
                    break;
                case GFFFieldType.Vector3:
                    var v3 = value is Vector3 v3val ? v3val : Vector3.Zero;
                    var v3Panel = new StackPanel { Orientation = Orientation.Vertical, Spacing = 2 };
                    var v3Row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
                    var v3X = new NumericUpDown { Value = (decimal)v3.X, Minimum = 0, Maximum = 1, Increment = 0.01M, Width = 70 };
                    var v3Y = new NumericUpDown { Value = (decimal)v3.Y, Minimum = 0, Maximum = 1, Increment = 0.01M, Width = 70 };
                    var v3Z = new NumericUpDown { Value = (decimal)v3.Z, Minimum = 0, Maximum = 1, Increment = 0.01M, Width = 70 };
                    void ApplyV3() { _selectedNode.SetVector3(label, new Vector3((float)v3X.Value, (float)v3Y.Value, (float)v3Z.Value)); MarkDocumentDirty(); _preview.InvalidateVisual(); }
                    v3X.ValueChanged += (s, e) => ApplyV3();
                    v3Y.ValueChanged += (s, e) => ApplyV3();
                    v3Z.ValueChanged += (s, e) => ApplyV3();
                    v3Row.Children.Add(new TextBlock { Text = "R", VerticalAlignment = VerticalAlignment.Center });
                    v3Row.Children.Add(v3X);
                    v3Row.Children.Add(new TextBlock { Text = "G", VerticalAlignment = VerticalAlignment.Center });
                    v3Row.Children.Add(v3Y);
                    v3Row.Children.Add(new TextBlock { Text = "B", VerticalAlignment = VerticalAlignment.Center });
                    v3Row.Children.Add(v3Z);
                    v3Panel.Children.Add(v3Row);
                    if (label == "COLOR")
                    {
                        var colorPreview = new Border { Width = 24, Height = 24, Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb((byte)(v3.X * 255), (byte)(v3.Y * 255), (byte)(v3.Z * 255))), BorderBrush = Brushes.Gray, BorderThickness = new Thickness(1) };
                        void UpdateColorPreview() { colorPreview.Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb((byte)((float)v3X.Value * 255), (byte)((float)v3Y.Value * 255), (byte)((float)v3Z.Value * 255))); }
                        v3X.ValueChanged += (s, e) => UpdateColorPreview();
                        v3Y.ValueChanged += (s, e) => UpdateColorPreview();
                        v3Z.ValueChanged += (s, e) => UpdateColorPreview();
                        var pickColorBtn = new Button { Content = "Pick color..." };
                        pickColorBtn.Click += async (s, ev) =>
                        {
                            var init = Avalonia.Media.Color.FromRgb((byte)((float)v3X.Value * 255), (byte)((float)v3Y.Value * 255), (byte)((float)v3Z.Value * 255));
                            var dialog = new ColorPickerDialog(this as Window, init, false);
                            if (await dialog.ShowDialogAsync(this as Window))
                            {
                                var c = dialog.GetSelectedColor();
                                float r = c.R / 255f, g = c.G / 255f, b = c.B / 255f;
                                v3X.Value = (decimal)r; v3Y.Value = (decimal)g; v3Z.Value = (decimal)b;
                                _selectedNode.SetVector3(label, new Vector3(r, g, b));
                                colorPreview.Background = new SolidColorBrush(c);
                                MarkDocumentDirty(); _preview.InvalidateVisual();
                            }
                        };
                        var colorRow = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
                        colorRow.Children.Add(colorPreview);
                        colorRow.Children.Add(pickColorBtn);
                        v3Panel.Children.Add(colorRow);
                    }
                    input = v3Panel;
                    break;
                case GFFFieldType.Vector4:
                    var v4 = value is Vector4 v4val ? v4val : Vector4.Zero;
                    var v4Row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
                    var v4X = new NumericUpDown { Value = (decimal)v4.X, Minimum = decimal.MinValue, Maximum = decimal.MaxValue, Increment = 0.01M, Width = 60 };
                    var v4Y = new NumericUpDown { Value = (decimal)v4.Y, Minimum = decimal.MinValue, Maximum = decimal.MaxValue, Increment = 0.01M, Width = 60 };
                    var v4Z = new NumericUpDown { Value = (decimal)v4.Z, Minimum = decimal.MinValue, Maximum = decimal.MaxValue, Increment = 0.01M, Width = 60 };
                    var v4W = new NumericUpDown { Value = (decimal)v4.W, Minimum = decimal.MinValue, Maximum = decimal.MaxValue, Increment = 0.01M, Width = 60 };
                    v4X.ValueChanged += (s, e) => { _selectedNode.SetVector4(label, new Vector4((float)v4X.Value, (float)v4Y.Value, (float)v4Z.Value, (float)v4W.Value)); MarkDocumentDirty(); };
                    v4Y.ValueChanged += (s, e) => { _selectedNode.SetVector4(label, new Vector4((float)v4X.Value, (float)v4Y.Value, (float)v4Z.Value, (float)v4W.Value)); MarkDocumentDirty(); };
                    v4Z.ValueChanged += (s, e) => { _selectedNode.SetVector4(label, new Vector4((float)v4X.Value, (float)v4Y.Value, (float)v4Z.Value, (float)v4W.Value)); MarkDocumentDirty(); };
                    v4W.ValueChanged += (s, e) => { _selectedNode.SetVector4(label, new Vector4((float)v4X.Value, (float)v4Y.Value, (float)v4Z.Value, (float)v4W.Value)); MarkDocumentDirty(); };
                    v4Row.Children.Add(new TextBlock { Text = "X", VerticalAlignment = VerticalAlignment.Center });
                    v4Row.Children.Add(v4X);
                    v4Row.Children.Add(new TextBlock { Text = "Y", VerticalAlignment = VerticalAlignment.Center });
                    v4Row.Children.Add(v4Y);
                    v4Row.Children.Add(new TextBlock { Text = "Z", VerticalAlignment = VerticalAlignment.Center });
                    v4Row.Children.Add(v4Z);
                    v4Row.Children.Add(new TextBlock { Text = "W", VerticalAlignment = VerticalAlignment.Center });
                    v4Row.Children.Add(v4W);
                    input = v4Row;
                    break;
                case GFFFieldType.UInt8:
                    if (label == "Obj_Locked" || label == "LOOPING" || label == "LEFTSCROLLBAR" || label == "ISSELECTED" || label == "PULSING" || label == "STARTFROMLEFT")
                    {
                        var chk = new CheckBox { IsChecked = Convert.ToByte(value ?? 0) != 0 };
                        chk.IsCheckedChanged += (s, e) => { _selectedNode.SetUInt8(label, (byte)(((CheckBox)s).IsChecked == true ? 1 : 0)); MarkDocumentDirty(); };
                        input = chk;
                    }
                    else
                    {
                        var u8 = Convert.ToByte(value ?? 0);
                        var u8Box = new NumericUpDown { Value = u8, Minimum = 0, Maximum = 255 };
                        u8Box.ValueChanged += (s, e) => { _selectedNode.SetUInt8(label, (byte)((NumericUpDown)s).Value); MarkDocumentDirty(); };
                        input = u8Box;
                    }
                    break;
                default:
                    input = new TextBlock { Text = value?.ToString() ?? "" };
                    break;
            }
            if (input != null)
            {
                bool addMathButton = (fieldType == GFFFieldType.Int8 || fieldType == GFFFieldType.Int16 || fieldType == GFFFieldType.Int32 ||
                    fieldType == GFFFieldType.UInt8 || fieldType == GFFFieldType.UInt16 || fieldType == GFFFieldType.UInt32 ||
                    fieldType == GFFFieldType.Single || fieldType == GFFFieldType.Double) && input is NumericUpDown;
                if (addMathButton && input is NumericUpDown numInput)
                {
                    var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 4 };
                    row.Children.Add(numInput);
                    var mathBtn = new Button { Content = "=", Width = 28 };
                    ToolTip.SetTip(mathBtn, "Math expression (e.g. 100+50, 640/2)");
                    mathBtn.Click += (s, e) => OpenMathExpressionDialog(label, fieldType, numInput);
                    row.Children.Add(mathBtn);
                    sp.Children.Add(row);
                }
                else
                    sp.Children.Add(input);
            }
            return sp;
        }

        internal static ResRef ResRefFromEditableText(string text)
        {
            string value = text?.Trim() ?? string.Empty;
            return string.IsNullOrEmpty(value) || !ResRef.IsValid(value) ? ResRef.FromBlank() : ResRef.FromString(value);
        }

        private void ApplyResRefFieldEdit(GFFStruct node, string label, string text)
        {
            if (node == null || string.IsNullOrWhiteSpace(label))
            {
                return;
            }

            node.SetResRef(label, ResRefFromEditableText(text));
            MarkDocumentDirty();
            ReloadTextures();
            _preview.InvalidateVisual();
        }

        private void OpenMathExpressionDialog(string label, GFFFieldType fieldType, NumericUpDown targetBox)
        {
            var dialog = new Window { Title = "Math expression", Width = 320, Height = 120, WindowStartupLocation = WindowStartupLocation.CenterOwner };
            var panel = new StackPanel { Margin = new Thickness(12), Spacing = 8 };
            panel.Children.Add(new TextBlock { Text = "Enter expression (e.g. 100+50 or 640/2):" });
            var txt = new TextBox { Text = targetBox.Value?.ToString() ?? "0", Watermark = "e.g. 100+50" };
            panel.Children.Add(txt);
            var btnPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            var okBtn = new Button { Content = "OK" };
            var cancelBtn = new Button { Content = "Cancel" };
            okBtn.Click += (s, e) =>
            {
                try
                {
                    string expr = (txt.Text ?? "").Trim().Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    if (string.IsNullOrEmpty(expr)) { dialog.Close(); return; }
                    var result = new DataTable().Compute(expr, null);
                    double val = Convert.ToDouble(result);
                    targetBox.Value = (decimal)val;
                    switch (fieldType)
                    {
                        case GFFFieldType.Int8:
                        case GFFFieldType.Int16:
                        case GFFFieldType.Int32:
                            _selectedNode.SetInt32(label, (int)val);
                            break;
                        case GFFFieldType.UInt8:
                            _selectedNode.SetUInt8(label, (byte)Math.Max(0, Math.Min(255, val)));
                            break;
                        case GFFFieldType.UInt16:
                        case GFFFieldType.UInt32:
                            _selectedNode.SetUInt32(label, (uint)val);
                            break;
                        case GFFFieldType.Single:
                        case GFFFieldType.Double:
                            _selectedNode.SetDouble(label, val);
                            break;
                    }
                    MarkDocumentDirty();
                    _preview.InvalidateVisual();
                    dialog.Close();
                }
                catch (Exception ex)
                {
                    _ = DialogHelper.ShowWindowAsync(dialog, "Invalid expression", ex.Message, ButtonEnum.Ok, IconType.Warning);
                }
            };
            cancelBtn.Click += (s, e) => dialog.Close();
            btnPanel.Children.Add(okBtn);
            btnPanel.Children.Add(cancelBtn);
            panel.Children.Add(btnPanel);
            dialog.Content = panel;
            dialog.SystemDecorations = SystemDecorations.BorderOnly;
            dialog.ShowDialog(this as Window);
        }

        private async void OpenFile()
        {
            var storage = (this as Window)?.StorageProvider;
            if (storage == null) return;
            var files = await storage.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open GUI file",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("GUI") { Patterns = new[] { "*.gui" } } }
            });
            if (files?.Count > 0)
            {
                var path = files[0].Path.LocalPath;
                try
                {
                    var data = File.ReadAllBytes(path);
                    Load(path, Path.GetFileNameWithoutExtension(path), ResourceType.GUI, data);
                }
                catch (Exception ex)
                {
                    await DialogHelper.ShowWindowAsync(this, "Error", "Failed to load: " + ex.Message, ButtonEnum.Ok, IconType.Error);
                }
            }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            if (data == null || data.Length == 0)
            {
                _gff = new GFF(GFFContent.GUI);
                _gff.Root.SetList(OdyToolGUIHelpers.ControllistLabel, new GFFList());
                LoadTreeAndPreview();
                return;
            }
            try
            {
                _gff = GFF.FromBytes(data);
                LoadTreeAndPreview();
                _undoStack.Clear();
                _undoIndex = -1;
                PushState();
            }
            catch (Exception ex)
            {
                _ = DialogHelper.ShowWindowAsync(this, "Error loading GUI", ex.Message, ButtonEnum.Ok, IconType.Error);
                _gff = new GFF(GFFContent.GUI);
                _gff.Root.SetList(OdyToolGUIHelpers.ControllistLabel, new GFFList());
                LoadTreeAndPreview();
            }
            UpdateStatusBar();
        }

        private void LoadTreeAndPreview()
        {
            var rootVm = GuiNodeViewModel.FromGffRoot(_gff);
            _treeView.ItemsSource = rootVm != null ? new[] { rootVm } : Array.Empty<GuiNodeViewModel>();
            GFFStruct previewRoot = null;
            if (_gff?.Root != null && _gff.Root.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var list) && list.Count > 0)
                previewRoot = list.At(0);
            else
                previewRoot = _gff?.Root;
            _preview.Root = previewRoot;
            _preview.TextureCache = _textureCache;
            _preview.SelectedNode = _selectedNode;
            _preview.InvalidateVisual();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            if (_gff == null) return Tuple.Create(new byte[0], new byte[0]);
            var data = GFFAuto.BytesGff(_gff, ResourceType.GUI);
            return Tuple.Create(data, new byte[0]);
        }

        public override void Save()
        {
            base.Save();
            UpdateStatusBar();
        }

        protected override FilePickerSaveOptions CreateSaveAsOptions()
        {
            var options = base.CreateSaveAsOptions();
            options.Title = "Save GUI As";
            return options;
        }

        public override void New()
        {
            base.New();
            _gff = new GFF(GFFContent.GUI);
            _gff.Root.SetList(OdyToolGUIHelpers.ControllistLabel, new GFFList());
            _selectedNode = null;
            _undoStack.Clear();
            _undoIndex = -1;
            LoadTreeAndPreview();
            RefreshPropertyPanel();
            UpdateStatusBar();
        }

        public override void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            Load(_filepath ?? "", _resname ?? "", ResourceType.GUI, _revert);
            _undoStack.Clear();
            _undoIndex = -1;
            ClearDirty();
        }

        private void PushState()
        {
            if (_undoRedoInProgress || _gff == null) return;
            var data = GFFAuto.BytesGff(_gff, ResourceType.GUI);
            if (_undoIndex >= 0 && _undoIndex < _undoStack.Count - 1)
                _undoStack.RemoveRange(_undoIndex + 1, _undoStack.Count - _undoIndex - 1);
            _undoStack.Add(data);
            if (_undoStack.Count > UndoMaxLevels) _undoStack.RemoveAt(0);
            _undoIndex = _undoStack.Count - 1;
        }

        private void Undo()
        {
            if (!CanUndo()) return;
            _undoRedoInProgress = true;
            try
            {
                _undoIndex--;
                var data = _undoStack[_undoIndex];
                _gff = GFF.FromBytes(data);
                LoadTreeAndPreview();
                RefreshPropertyPanel();
                MarkDocumentDirty();
            }
            finally { _undoRedoInProgress = false; }
        }

        private void Redo()
        {
            if (!CanRedo()) return;
            _undoRedoInProgress = true;
            try
            {
                _undoIndex++;
                var data = _undoStack[_undoIndex];
                _gff = GFF.FromBytes(data);
                LoadTreeAndPreview();
                RefreshPropertyPanel();
                MarkDocumentDirty();
            }
            finally { _undoRedoInProgress = false; }
        }

        private bool CanUndo() => _undoIndex > 0;
        private bool CanRedo() => _undoIndex >= 0 && _undoIndex < _undoStack.Count - 1;

        private void UpdateStatusBar()
        {
            if (_statusText == null) return;
            string path = string.IsNullOrEmpty(_filepath) ? "Untitled" : Path.GetFileName(_filepath);
            _statusText.Text = $"{path} | {(_gff?.Root != null ? "Loaded" : "Empty")}";
        }
    }
}
