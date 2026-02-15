using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using OdyTools.Data;

namespace OdyTools.Editors.GUI
{
    /// <summary>
    /// Visual KotOR GUI editor (transpiled from kotor-gui-editor).
    /// Load .gui (GFF), set UI image folder for TPC/TGA, edit in tree + visual preview + property panel, save with backup.
    /// </summary>
    public class OdyToolGUI : Editor
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
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private int _undoIndex = -1;
        private bool _undoRedoInProgress;

        public OdyToolGUI(Window parent = null, OdyInstallation installation = null)
            : base(parent, "GUI Editor", "none", new[] { ResourceType.GUI }, new[] { ResourceType.GUI }, installation)
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
            var mainDock = new DockPanel();
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
            var revertBtn = new Button { Content = "Revert" };
            revertBtn.Click += (s, e) => Revert();
            toolStack.Children.Add(revertBtn);
            var saveBtn = new Button { Content = "Save" };
            saveBtn.Click += (s, e) => Save();
            toolStack.Children.Add(saveBtn);
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
            _preview.DataChanged += () => { PushState(); MarkDirty(); _preview.InvalidateVisual(); };
            Grid.SetColumn(_preview, 1);
            grid.Children.Add(_preview);

            _propertyPanel = new StackPanel { Spacing = 4, Margin = new Thickness(8) };
            _propertyScroll = new ScrollViewer { Content = _propertyPanel, HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto, VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto };
            var propBorder = new Border { Child = _propertyScroll, BorderBrush = Brushes.LightGray, BorderThickness = new Thickness(1), Padding = new Thickness(4) };
            Grid.SetColumn(propBorder, 2);
            grid.Children.Add(propBorder);

            mainDock.Children.Add(grid);

            _statusText = new TextBlock { Text = "GUI Editor", Margin = new Thickness(8, 4), VerticalAlignment = VerticalAlignment.Center };
            var statusBorder = new Border { Child = _statusText, Background = new SolidColorBrush(Avalonia.Media.Color.FromRgb(0xE1, 0xE1, 0xE1)), Padding = new Thickness(8, 4) };
            DockPanel.SetDock(statusBorder, Dock.Bottom);
            mainDock.Children.Add(statusBorder);

            Content = mainDock;

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
            foreach (var (label, fieldType, value) in _selectedNode)
            {
                if (label == OdyToolGUIHelpers.ProtoItemLabel || label == OdyToolGUIHelpers.ScrollbarLabel) continue;
                var row = CreatePropertyRow(label, fieldType, value);
                if (row != null) _propertyPanel.Children.Add(row);
            }
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
                    intBox.ValueChanged += (s, e) => { _selectedNode.SetInt32(label, (int)((NumericUpDown)s).Value); MarkDirty(); _preview.InvalidateVisual(); };
                    input = intBox;
                    break;
                case GFFFieldType.UInt16:
                case GFFFieldType.UInt32:
                    var u = Convert.ToUInt32(value ?? 0);
                    var uintBox = new NumericUpDown { Value = u, Minimum = 0, Maximum = uint.MaxValue };
                    uintBox.ValueChanged += (s, e) => { _selectedNode.SetUInt32(label, (uint)((NumericUpDown)s).Value); MarkDirty(); _preview.InvalidateVisual(); };
                    input = uintBox;
                    break;
                case GFFFieldType.Single:
                case GFFFieldType.Double:
                    var d = Convert.ToDouble(value ?? 0);
                    var doubleBox = new NumericUpDown { Value = (decimal)d, Increment = 0.01M };
                    if (label == "ALPHA" || label == "ROTATE") { doubleBox.Minimum = 0; doubleBox.Maximum = 1; }
                    else doubleBox.Minimum = decimal.MinValue;
                    doubleBox.ValueChanged += (s, e) => { _selectedNode.SetDouble(label, (double)((NumericUpDown)s).Value); MarkDirty(); _preview.InvalidateVisual(); };
                    input = doubleBox;
                    break;
                case GFFFieldType.String:
                    var str = value?.ToString() ?? "";
                    var txt = new TextBox { Text = str };
                    txt.LostFocus += (s, e) => { _selectedNode.SetString(label, ((TextBox)s).Text ?? ""); MarkDirty(); };
                    input = txt;
                    break;
                case GFFFieldType.ResRef:
                    var resRef = value as ResRef ?? ResRef.FromBlank();
                    var resTxt = new TextBox { Text = resRef.ToString() ?? "", MaxLength = 16 };
                    resTxt.LostFocus += (s, e) => { _selectedNode.SetResRef(label, ResRef.FromString(((TextBox)s).Text ?? "")); MarkDirty(); ReloadTextures(); _preview.InvalidateVisual(); };
                    input = resTxt;
                    break;
                case GFFFieldType.UInt8:
                    if (label == "Obj_Locked" || label == "LOOPING" || label == "LEFTSCROLLBAR" || label == "ISSELECTED" || label == "PULSING" || label == "STARTFROMLEFT")
                    {
                        var chk = new CheckBox { IsChecked = Convert.ToByte(value ?? 0) != 0 };
                        chk.IsCheckedChanged += (s, e) => { _selectedNode.SetUInt8(label, (byte)(((CheckBox)s).IsChecked == true ? 1 : 0)); MarkDirty(); };
                        input = chk;
                    }
                    else
                    {
                        var u8 = Convert.ToByte(value ?? 0);
                        var u8Box = new NumericUpDown { Value = u8, Minimum = 0, Maximum = 255 };
                        u8Box.ValueChanged += (s, e) => { _selectedNode.SetUInt8(label, (byte)((NumericUpDown)s).Value); MarkDirty(); };
                        input = u8Box;
                    }
                    break;
                default:
                    input = new TextBlock { Text = value?.ToString() ?? "" };
                    break;
            }
            if (input != null) sp.Children.Add(input);
            return sp;
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
                    await MessageBoxManager.GetMessageBoxStandard("Error", "Failed to load: " + ex.Message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowWindowDialogAsync(this);
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
                _ = MessageBoxManager.GetMessageBoxStandard("Error loading GUI", ex.Message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowWindowDialogAsync(this);
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
            if (string.IsNullOrEmpty(_filepath)) { _ = RunSaveAsAsync(); return; }
            try
            {
                var backupPath = _filepath + ".bak";
                if (File.Exists(_filepath))
                    File.Copy(_filepath, backupPath, true);
                var (data, _) = Build();
                File.WriteAllBytes(_filepath, data);
                _revert = data;
                ClearDirty();
                UpdateStatusBar();
            }
            catch (Exception ex)
            {
                _ = MessageBoxManager.GetMessageBoxStandard("Save failed", ex.Message, ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error).ShowWindowDialogAsync(this);
            }
        }

        private async System.Threading.Tasks.Task RunSaveAsAsync()
        {
            var storage = (this as Window)?.StorageProvider;
            if (storage == null) return;
            var file = await storage.SaveFilePickerAsync(new FilePickerSaveOptions { Title = "Save GUI As", FileTypeChoices = new[] { new FilePickerFileType("GUI") { Patterns = new[] { "*.gui" } } } });
            if (file != null && !string.IsNullOrEmpty(file.Path.LocalPath))
            {
                _filepath = file.Path.LocalPath;
                _resname = System.IO.Path.GetFileNameWithoutExtension(_filepath);
                _restype = ResourceType.GUI;
                RefreshWindowTitle();
                Save();
            }
        }

        public override void SaveAs()
        {
            Save();
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

        private void Revert()
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
                MarkDirty();
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
                MarkDirty();
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
