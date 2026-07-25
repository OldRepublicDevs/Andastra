using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using BioWare.Common;
using BioWare.Resource.Formats.BWM;
using BioWare.Resource.Formats.LYT;
using OdyTools.Blender;
using OdyTools.Data;
using MDLAuto = BioWare.Resource.Formats.MDL.MDLAuto;
using ResRef = BioWare.Common.ResRef;
using LYTRoom = BioWare.Resource.Formats.LYT.LYTRoom;
using Quaternion = System.Numerics.Quaternion;
using TPCAuto = BioWare.Resource.Formats.TPC.TPCAuto;
using TPC = BioWare.Resource.Formats.TPC.TPC;
using TPCTextureFormat = BioWare.Resource.Formats.TPC.TPCTextureFormat;
using OdyTools.Widgets;
using OdyTools.Editors.LYT;
using MediaColor = Avalonia.Media.Color;

namespace OdyTools.Editors
{
    public partial class OdyToolLYT : Editor
    {
        private BioWare.Resource.Formats.LYT.LYT _lyt;
        private OdyToolLYTSettings _settings;
        private Dictionary<string, string> _importedTextures = new Dictionary<string, string>(); // Maps texture name to file path
        private Dictionary<string, string> _importedModels = new Dictionary<string, string>(); // Maps model name (ResRef) to MDL file path
        private ModelBrowser _modelBrowser; // Model browser widget for displaying imported models
        private TextureBrowser _textureBrowser; // Texture browser widget for displaying imported textures
        private LYTGraphicsScene _graphicsScene; // Graphics scene for rendering LYT layout elements
        private ListBox _roomsList;
        private ListBox _tracksList;
        private ListBox _obstaclesList;
        private ListBox _doorHooksList;
        private TextBlock _summaryText;
        private TextBlock _selectionTitleText;
        private TextBlock _selectionPathText;
        private TextBlock _statusText;
        private TextBlock _zoomValueText;
        private TextBox _nameEdit;
        private TextBox _doorEdit;
        private TextBox _xEdit;
        private TextBox _yEdit;
        private TextBox _zEdit;
        private Button _applySelectionButton;
        private Button _addSelectedModelButton;
        private MenuItem _openInBlenderMenuItem;
        private object _selectedLayoutElement;
        private string _selectedAssetKind;
        private string _selectedAssetName;
        private string _blenderStatus = "";
        private Func<string, BlenderInfo> _detectBlender = BlenderDetection.DetectBlender;
        private Func<BlenderInfo, int, string, string, string, bool, System.Diagnostics.Process> _launchBlender = BlenderDetection.LaunchBlenderWithIpc;
        private bool _updatingSelectionUi;
        private LYTGraphicsScene _wiredGraphicsScene;

        public OdyToolLYT() : this(null, null) { }
        public OdyToolLYT(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolLYT", "lyt",
                new[] { ResourceType.LYT },
                new[] { ResourceType.LYT },
                installation)
        {
            _lyt = new BioWare.Resource.Formats.LYT.LYT();
            _settings = new OdyToolLYTSettings();

            InitializeComponent();
            SetupUI();
            New();
        }

        private void InitializeComponent()
        {
            try { AvaloniaXamlLoader.Load(this); } catch { /* XAML not available - use programmatic UI */ }
            SetupProgrammaticUI();
        }

        private void SetupProgrammaticUI()
        {
            var root = new DockPanel
            {
                LastChildFill = true,
                Background = new SolidColorBrush(MediaColor.FromRgb(243, 245, 249))
            };

            var menu = BuildMenu();
            DockPanel.SetDock(menu, Dock.Top);
            root.Children.Add(menu);

            _statusText = new TextBlock
            {
                Text = "Ready",
                Padding = new Thickness(10, 5),
                Background = new SolidColorBrush(MediaColor.FromRgb(225, 230, 238)),
                Foreground = new SolidColorBrush(MediaColor.FromRgb(47, 55, 68))
            };
            DockPanel.SetDock(_statusText, Dock.Bottom);
            root.Children.Add(_statusText);

            var mainGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,360"),
                RowDefinitions = new RowDefinitions("*"),
                Margin = new Thickness(10)
            };
            mainGrid.Children.Add(BuildCanvasPanel());

            var sideTabs = BuildSideTabs();
            Grid.SetColumn(sideTabs, 1);
            mainGrid.Children.Add(sideTabs);

            root.Children.Add(mainGrid);
            SetContentOrInject(root);
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

            var editMenu = new MenuItem { Header = "_Edit" };
            editMenu.Items.Add(new MenuItem { Header = "_Undo", Name = "actionUndo" });
            editMenu.Items.Add(new MenuItem { Header = "_Redo", Name = "actionRedo" });

            var toolsMenu = new MenuItem { Header = "_Tools" };
            var importModel = new MenuItem { Header = "Import _Model..." };
            importModel.Click += (s, e) => ImportModel();
            var importTexture = new MenuItem { Header = "Import _Texture..." };
            importTexture.Click += (s, e) => ImportTexture();
            var generateWalkmesh = new MenuItem { Header = "_Generate Walkmesh" };
            generateWalkmesh.Click += (s, e) => GenerateWalkmesh();
            _openInBlenderMenuItem = new MenuItem
            {
                Header = "Open in _Blender",
                Name = "actionOpenInBlender"
            };
            ToolTip.SetTip(_openInBlenderMenuItem, "Launch the current LYT in Blender using the kotorblender IPC bridge.");
            _openInBlenderMenuItem.Click += (s, e) => TryLaunchBlenderForCurrentLayout();
            toolsMenu.Items.Add(importModel);
            toolsMenu.Items.Add(importTexture);
            toolsMenu.Items.Add(new Separator());
            toolsMenu.Items.Add(generateWalkmesh);
            toolsMenu.Items.Add(_openInBlenderMenuItem);

            menu.Items.Add(fileMenu);
            menu.Items.Add(editMenu);
            menu.Items.Add(toolsMenu);
            RefreshBlenderActionState();
            return menu;
        }

        private Control BuildCanvasPanel()
        {
            var dock = new DockPanel { LastChildFill = true, Margin = new Thickness(0, 0, 10, 0) };

            var toolbar = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8,
                Margin = new Thickness(0, 0, 0, 8)
            };
            toolbar.Children.Add(MakeButton("Add Room", AddRoom));
            toolbar.Children.Add(MakeButton("Add Track", AddTrack));
            toolbar.Children.Add(MakeButton("Add Obstacle", AddObstacle));
            toolbar.Children.Add(MakeButton("Add Door Hook", AddDoorHook));
            toolbar.Children.Add(MakeButton("Import Model", ImportModel));
            toolbar.Children.Add(MakeButton("Import Texture", ImportTexture));

            var showGrid = new CheckBox
            {
                Content = "Grid",
                IsChecked = true,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(8, 0, 0, 0)
            };
            showGrid.IsCheckedChanged += (s, e) =>
            {
                if (_graphicsScene != null)
                {
                    _graphicsScene.ShowGrid = showGrid.IsChecked == true;
                }
            };
            toolbar.Children.Add(showGrid);

            toolbar.Children.Add(new TextBlock
            {
                Text = "Zoom",
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(8, 0, 0, 0)
            });
            var zoom = new Slider
            {
                Minimum = 10,
                Maximum = 200,
                Value = 100,
                Width = 160,
                VerticalAlignment = VerticalAlignment.Center
            };
            _zoomValueText = new TextBlock
            {
                Text = "100%",
                MinWidth = 42,
                VerticalAlignment = VerticalAlignment.Center
            };
            zoom.PropertyChanged += (s, e) =>
            {
                if (e.Property == Slider.ValueProperty)
                {
                    UpdateZoom((int)zoom.Value);
                    if (_zoomValueText != null)
                    {
                        _zoomValueText.Text = $"{(int)zoom.Value}%";
                    }
                }
            };
            toolbar.Children.Add(zoom);
            toolbar.Children.Add(_zoomValueText);

            DockPanel.SetDock(toolbar, Dock.Top);
            dock.Children.Add(toolbar);

            _summaryText = new TextBlock
            {
                Text = "Rooms 0 | Tracks 0 | Obstacles 0 | Door hooks 0",
                Padding = new Thickness(8, 5),
                Background = new SolidColorBrush(MediaColor.FromRgb(225, 230, 238)),
                Foreground = new SolidColorBrush(MediaColor.FromRgb(47, 55, 68))
            };
            DockPanel.SetDock(_summaryText, Dock.Bottom);
            dock.Children.Add(_summaryText);

            _graphicsScene = new LYTGraphicsScene
            {
                Name = "graphicsScene",
                MinHeight = 420
            };
            WireGraphicsSceneSelection();
            var canvasBorder = new Border
            {
                BorderBrush = new SolidColorBrush(MediaColor.FromRgb(178, 187, 201)),
                BorderThickness = new Thickness(1),
                Background = Brushes.Black,
                Child = _graphicsScene
            };
            dock.Children.Add(canvasBorder);

            return dock;
        }

        private TabControl BuildSideTabs()
        {
            _roomsList = MakeElementList();
            _tracksList = MakeElementList();
            _obstaclesList = MakeElementList();
            _doorHooksList = MakeElementList();
            _roomsList.SelectionChanged += (s, e) => SelectLayoutElement(_roomsList.SelectedItem);
            _tracksList.SelectionChanged += (s, e) => SelectLayoutElement(_tracksList.SelectedItem);
            _obstaclesList.SelectionChanged += (s, e) => SelectLayoutElement(_obstaclesList.SelectedItem);
            _doorHooksList.SelectionChanged += (s, e) => SelectLayoutElement(_doorHooksList.SelectedItem);

            _modelBrowser = new ModelBrowser();
            _modelBrowser.ModelSelected += OnModelSelected;
            _modelBrowser.ModelChanged += OnModelChanged;

            _textureBrowser = new TextureBrowser();
            _textureBrowser.TextureSelected += OnTextureSelected;
            _textureBrowser.TextureChanged += OnTextureChanged;

            var tabs = new TabControl();
            tabs.Items.Add(new TabItem { Header = "Layout", Content = BuildLayoutTab() });
            tabs.Items.Add(new TabItem { Header = "Models", Content = _modelBrowser });
            tabs.Items.Add(new TabItem { Header = "Textures", Content = _textureBrowser });
            tabs.Items.Add(new TabItem { Header = "Details", Content = BuildDetailsPanel() });
            return tabs;
        }

        private Control BuildLayoutTab()
        {
            var panel = new Grid
            {
                RowDefinitions = new RowDefinitions("*,*,*,*"),
                Margin = new Thickness(0)
            };
            panel.Children.Add(WrapElementList("Rooms", _roomsList, 0));
            panel.Children.Add(WrapElementList("Tracks", _tracksList, 1));
            panel.Children.Add(WrapElementList("Obstacles", _obstaclesList, 2));
            panel.Children.Add(WrapElementList("Door Hooks", _doorHooksList, 3));
            return panel;
        }

        private Control BuildDetailsPanel()
        {
            var panel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Spacing = 8,
                Margin = new Thickness(8)
            };

            _selectionTitleText = new TextBlock
            {
                Text = "No selection",
                FontWeight = FontWeight.Bold,
                FontSize = 15
            };
            panel.Children.Add(_selectionTitleText);

            _selectionPathText = new TextBlock
            {
                Text = "",
                TextWrapping = TextWrapping.Wrap,
                Foreground = new SolidColorBrush(MediaColor.FromRgb(80, 88, 104))
            };
            panel.Children.Add(_selectionPathText);

            _nameEdit = MakeTextBox();
            _doorEdit = MakeTextBox();
            _xEdit = MakeTextBox();
            _yEdit = MakeTextBox();
            _zEdit = MakeTextBox();

            panel.Children.Add(MakeLabeled("Model / Resource", _nameEdit));
            panel.Children.Add(MakeLabeled("Door", _doorEdit));
            panel.Children.Add(MakeVectorRow());

            _applySelectionButton = MakeButton("Apply", ApplySelectionEdits);
            _applySelectionButton.IsEnabled = false;
            _addSelectedModelButton = MakeButton("Add Model As Room", AddSelectedModelAsRoom);
            _addSelectedModelButton.IsEnabled = false;

            var buttons = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 8 };
            buttons.Children.Add(_applySelectionButton);
            buttons.Children.Add(_addSelectedModelButton);
            panel.Children.Add(buttons);

            return new ScrollViewer { Content = panel };
        }

        private StackPanel MakeVectorRow()
        {
            var row = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 6 };
            row.Children.Add(MakeLabeled("X", _xEdit, 78));
            row.Children.Add(MakeLabeled("Y", _yEdit, 78));
            row.Children.Add(MakeLabeled("Z", _zEdit, 78));
            return row;
        }

        private static TextBox MakeTextBox()
        {
            return new TextBox
            {
                MinWidth = 72,
                Height = 30
            };
        }

        private static Control MakeLabeled(string label, Control control, double width = double.NaN)
        {
            var panel = new StackPanel { Orientation = Orientation.Vertical, Spacing = 3 };
            panel.Children.Add(new TextBlock
            {
                Text = label,
                Foreground = new SolidColorBrush(MediaColor.FromRgb(80, 88, 104))
            });
            if (!double.IsNaN(width))
            {
                control.Width = width;
            }
            panel.Children.Add(control);
            return panel;
        }

        private static Button MakeButton(string text, Action action)
        {
            var button = new Button
            {
                Content = text,
                Padding = new Thickness(10, 5),
                MinHeight = 30
            };
            button.Click += (s, e) => action();
            return button;
        }

        private static ListBox MakeElementList()
        {
            return new ListBox
            {
                SelectionMode = SelectionMode.Single,
                MinHeight = 90
            };
        }

        private Control WrapElementList(string header, ListBox list, int row)
        {
            var panel = new DockPanel { LastChildFill = true, Margin = new Thickness(0, 0, 0, 8) };
            var label = new TextBlock
            {
                Text = header,
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(0, 0, 0, 4)
            };
            DockPanel.SetDock(label, Dock.Top);
            panel.Children.Add(label);
            panel.Children.Add(list);
            Grid.SetRow(panel, row);
            return panel;
        }

        private void SetupUI()
        {
            // UI setup (programmatic when XAML not used)
            // Initialize graphics scene
            InitializeGraphicsScene();
            // Initialize model browser widget
            InitializeModelBrowser();
            // Initialize texture browser widget
            InitializeTextureBrowser();
        }

        /// <summary>
        /// Initializes the graphics scene for rendering LYT layout elements.
        /// </summary>
        private void InitializeGraphicsScene()
        {
            try
            {
                // Try to find graphics scene from XAML if available
                _graphicsScene = EditorHelpers.FindControlSafe<LYTGraphicsScene>(this, "graphicsScene");
            }
            catch
            {
                // Graphics scene not found in XAML - will create programmatically if needed
            }

            // Create graphics scene if not found from XAML
            if (_graphicsScene == null)
            {
                _graphicsScene = new LYTGraphicsScene();
            }
            WireGraphicsSceneSelection();
        }

        private void WireGraphicsSceneSelection()
        {
            if (_graphicsScene == null || ReferenceEquals(_wiredGraphicsScene, _graphicsScene))
            {
                return;
            }

            if (_wiredGraphicsScene != null)
            {
                _wiredGraphicsScene.LayoutElementSelected -= OnGraphicsSceneElementSelected;
            }

            _graphicsScene.LayoutElementSelected += OnGraphicsSceneElementSelected;
            _wiredGraphicsScene = _graphicsScene;
        }

        private void OnGraphicsSceneElementSelected(object sender, object element)
        {
            SelectLayoutElement(element);
        }

        /// <summary>
        /// Initializes the model browser widget.
        /// </summary>
        private void InitializeModelBrowser()
        {
            try
            {
                // Try to find model browser from XAML if available
                _modelBrowser = EditorHelpers.FindControlSafe<ModelBrowser>(this, "modelBrowser");
            }
            catch
            {
                // Model browser not found in XAML - will create programmatically if needed
            }

            // Create model browser if not found from XAML
            if (_modelBrowser == null)
            {
                _modelBrowser = new ModelBrowser();
                _modelBrowser.ModelSelected += OnModelSelected;
                _modelBrowser.ModelChanged += OnModelChanged;
            }

            // Initialize with current imported models
            if (_modelBrowser != null && _importedModels != null)
            {
                _modelBrowser.UpdateModels(_importedModels);
            }
        }

        /// <summary>
        /// Initializes the texture browser widget.
        /// </summary>
        private void InitializeTextureBrowser()
        {
            try
            {
                // Try to find texture browser from XAML if available
                _textureBrowser = EditorHelpers.FindControlSafe<TextureBrowser>(this, "textureBrowser");
            }
            catch
            {
                // Texture browser not found in XAML - will create programmatically if needed
            }

            // Create texture browser if not found from XAML
            if (_textureBrowser == null)
            {
                _textureBrowser = new TextureBrowser();
                _textureBrowser.TextureSelected += OnTextureSelected;
                _textureBrowser.TextureChanged += OnTextureChanged;
            }

            // Initialize with current imported textures
            if (_textureBrowser != null && _importedTextures != null)
            {
                _textureBrowser.UpdateTextures(_importedTextures);
            }
        }

        /// <summary>
        /// Handles model selection in the browser.
        /// </summary>
        private void OnModelSelected(object sender, string modelName)
        {
            if (string.IsNullOrEmpty(modelName))
            {
                return;
            }

            SelectImportedAsset("Model", modelName, GetImportedModelPath(modelName));
        }

        /// <summary>
        /// Handles model change in the browser.
        /// </summary>
        private void OnModelChanged(object sender, string modelName)
        {
            // Model changed - update any dependent UI
            if (!string.IsNullOrEmpty(modelName))
            {
                SelectImportedAsset("Model", modelName, GetImportedModelPath(modelName));
            }
        }

        /// <summary>
        /// Handles texture selection in the browser.
        /// </summary>
        private void OnTextureSelected(object sender, string textureName)
        {
            if (string.IsNullOrEmpty(textureName))
            {
                return;
            }

            SelectImportedAsset("Texture", textureName, GetImportedTexturePath(textureName));
        }

        /// <summary>
        /// Handles texture change in the browser.
        /// </summary>
        private void OnTextureChanged(object sender, string textureName)
        {
            // Texture changed - update any dependent UI
            if (!string.IsNullOrEmpty(textureName))
            {
                SelectImportedAsset("Texture", textureName, GetImportedTexturePath(textureName));
            }
        }

        public void AddRoom()
        {
            var room = new LYTRoom(new ResRef("default_room"), new Vector3(0, 0, 0));
            _lyt.Rooms.Add(room);
            MarkDocumentDirty();
            UpdateScene();
        }

        public void AddTrack()
        {
            if (_lyt.Rooms.Count < 2)
            {
                return;
            }

            var track = new LYTTrack(new ResRef("default_track"), new Vector3(0, 0, 0));

            // Find path through connected rooms
            var startRoom = _lyt.Rooms[0];
            var endRoom = _lyt.Rooms.Count > 1 ? _lyt.Rooms[1] : startRoom;
            var path = FindPath(startRoom, endRoom);

            if (path != null && path.Count > 0)
            {
                _lyt.Tracks.Add(track);
                MarkDocumentDirty();
            }

            UpdateScene();
        }

        public List<LYTRoom> FindPath(LYTRoom start, LYTRoom end)
        {
            if (start == null || end == null)
            {
                return null;
            }

            if (start.Equals(end))
            {
                return new List<LYTRoom> { start };
            }

            // Simple pathfinding - check if rooms are connected
            if (start.Connections != null && start.Connections.Contains(end))
            {
                return new List<LYTRoom> { start, end };
            }

            // A* pathfinding implementation
            var queue = new List<Tuple<float, LYTRoom, List<LYTRoom>>>
            {
                Tuple.Create(0f, start, new List<LYTRoom> { start })
            };
            var visited = new HashSet<LYTRoom> { start };

            while (queue.Count > 0)
            {
                queue.Sort((a, b) => a.Item1.CompareTo(b.Item1));
                var current = queue[0];
                queue.RemoveAt(0);

                var (_, currentRoom, path) = current;

                if (currentRoom.Equals(end))
                {
                    return path;
                }

                if (currentRoom.Connections != null)
                {
                    foreach (var nextRoom in currentRoom.Connections.Where(conn => !visited.Contains(conn)))
                    {
                        visited.Add(nextRoom);
                        var newPath = new List<LYTRoom>(path) { nextRoom };
                        var priority = newPath.Count + (nextRoom.Position - end.Position).Length();
                        queue.Add(Tuple.Create(priority, nextRoom, newPath));
                    }
                }
            }

            return null;
        }

        internal int RoomCount => _lyt?.Rooms?.Count ?? 0;
        internal int TrackCount => _lyt?.Tracks?.Count ?? 0;
        internal int ObstacleCount => _lyt?.Obstacles?.Count ?? 0;
        internal int DoorHookCount => _lyt?.DoorHooks?.Count ?? 0;
        internal string SummaryText => _summaryText?.Text ?? "";
        internal string StatusText => _statusText?.Text ?? "";
        internal MenuItem OpenInBlenderMenuItem => _openInBlenderMenuItem;
        internal string BlenderStatusText => _blenderStatus;
        internal string SelectionTitleText => _selectionTitleText?.Text ?? "";
        internal bool AddSelectedModelButtonEnabledForTesting => _addSelectedModelButton?.IsEnabled == true;
        internal string SelectedRoomListTextForTesting => _roomsList?.SelectedItem?.ToString() ?? "";
        internal bool HasProgrammaticEditorSurfaceForTest =>
            _graphicsScene != null &&
            _roomsList != null &&
            _tracksList != null &&
            _obstaclesList != null &&
            _doorHooksList != null &&
            _modelBrowser != null &&
            _textureBrowser != null &&
            _summaryText != null &&
            _selectionTitleText != null &&
            _selectionPathText != null &&
            _nameEdit != null &&
            _xEdit != null &&
            _yEdit != null &&
            _zEdit != null &&
            _applySelectionButton != null &&
            _addSelectedModelButton != null;

        internal void SelectRoomForTesting(int index)
        {
            if (_lyt?.Rooms == null || index < 0 || index >= _lyt.Rooms.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _selectedLayoutElement = _lyt.Rooms[index];
            _selectedAssetKind = null;
            _selectedAssetName = null;
            _graphicsScene?.SelectElement(_selectedLayoutElement);
            PopulateSelectionDetails();
        }

        internal void SelectTrackForTesting(int index)
        {
            if (_lyt?.Tracks == null || index < 0 || index >= _lyt.Tracks.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _selectedLayoutElement = _lyt.Tracks[index];
            _selectedAssetKind = null;
            _selectedAssetName = null;
            _graphicsScene?.SelectElement(_selectedLayoutElement);
            PopulateSelectionDetails();
        }

        internal void SelectObstacleForTesting(int index)
        {
            if (_lyt?.Obstacles == null || index < 0 || index >= _lyt.Obstacles.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _selectedLayoutElement = _lyt.Obstacles[index];
            _selectedAssetKind = null;
            _selectedAssetName = null;
            _graphicsScene?.SelectElement(_selectedLayoutElement);
            PopulateSelectionDetails();
        }

        internal void SelectDoorHookForTesting(int index)
        {
            if (_lyt?.DoorHooks == null || index < 0 || index >= _lyt.DoorHooks.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _selectedLayoutElement = _lyt.DoorHooks[index];
            _selectedAssetKind = null;
            _selectedAssetName = null;
            _graphicsScene?.SelectElement(_selectedLayoutElement);
            PopulateSelectionDetails();
        }

        internal void SelectRoomInSceneForTesting(int index)
        {
            if (_lyt?.Rooms == null || index < 0 || index >= _lyt.Rooms.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            _graphicsScene?.SelectElement(_lyt.Rooms[index]);
            OnGraphicsSceneElementSelected(_graphicsScene, _lyt.Rooms[index]);
        }

        internal void SetSelectionEditsForTesting(string name, float x, float y, float z, string door = "")
        {
            SetText(_nameEdit, name);
            SetText(_doorEdit, door);
            SetText(_xEdit, x.ToString(System.Globalization.CultureInfo.InvariantCulture));
            SetText(_yEdit, y.ToString(System.Globalization.CultureInfo.InvariantCulture));
            SetText(_zEdit, z.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }

        internal void ApplySelectionEditsForTesting()
        {
            ApplySelectionEdits();
        }

        internal void RegisterImportedModelForTesting(string modelName, string path = "")
        {
            if (_importedModels == null)
            {
                _importedModels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            _importedModels[modelName] = path ?? "";
            UpdateModelBrowser();
        }

        internal void SelectImportedModelForTesting(string modelName)
        {
            OnModelSelected(_modelBrowser, modelName);
        }

        internal void AddSelectedModelAsRoomForTesting()
        {
            AddSelectedModelAsRoom();
        }

        internal void SetBlenderServicesForTests(
            Func<string, BlenderInfo> detectBlender,
            Func<BlenderInfo, int, string, string, string, bool, System.Diagnostics.Process> launchBlender)
        {
            _detectBlender = detectBlender ?? BlenderDetection.DetectBlender;
            _launchBlender = launchBlender ?? BlenderDetection.LaunchBlenderWithIpc;
        }

        internal bool IsGridVisibleForTesting
        {
            get { return _graphicsScene?.ShowGrid ?? false; }
            set
            {
                if (_graphicsScene != null)
                {
                    _graphicsScene.ShowGrid = value;
                }
            }
        }

        public void AddObstacle()
        {
            var obstacle = new LYTObstacle(new ResRef("default_obstacle"), new Vector3(0, 0, 0));
            _lyt.Obstacles.Add(obstacle);
            MarkDocumentDirty();
            UpdateScene();
        }

        public void AddDoorHook()
        {
            if (_lyt.Rooms.Count == 0)
            {
                return;
            }

            var firstRoom = _lyt.Rooms[0];

            var doorhook = new LYTDoorHook(
                firstRoom.Model,
                "",
                new Vector3(0, 0, 0),
                new Vector4(0, 0, 0, 1)
            );

            _lyt.Doorhooks.Add(doorhook);
            MarkDocumentDirty();
            UpdateScene();
        }

        public void GenerateWalkmesh()
        {
            GenerateWalkmeshFiles(overwriteExisting: false);
        }

        internal int GenerateWalkmeshFilesForTesting(bool overwriteExisting = false)
        {
            return GenerateWalkmeshFiles(overwriteExisting);
        }

        private int GenerateWalkmeshFiles(bool overwriteExisting)
        {
            if (_lyt?.Rooms == null || _lyt.Rooms.Count == 0)
            {
                SetStatus("No rooms available for walkmesh generation.");
                return 0;
            }

            if (string.IsNullOrEmpty(Filepath))
            {
                SetStatus("Save or open a LYT before generating WOK files.");
                return 0;
            }

            string directory = Path.GetDirectoryName(Filepath);
            if (string.IsNullOrWhiteSpace(directory) || !Directory.Exists(directory))
            {
                SetStatus("Cannot generate WOK files because the LYT directory does not exist.");
                return 0;
            }

            int generated = 0;
            int skipped = 0;
            foreach (var room in _lyt.Rooms)
            {
                string roomName = room?.Model?.ToString();
                if (string.IsNullOrWhiteSpace(roomName) || !ResRef.IsValid(roomName))
                {
                    skipped++;
                    continue;
                }

                string outputPath = Path.Combine(directory, roomName + ".wok");
                if (File.Exists(outputPath) && !overwriteExisting)
                {
                    skipped++;
                    continue;
                }

                var bwm = CreatePlaceholderRoomWalkmesh(room);
                File.WriteAllBytes(outputPath, BWMAuto.BytesBwm(bwm, ResourceType.WOK));
                generated++;
            }

            string skippedText = skipped > 0 ? $" ({skipped} skipped)" : "";
            SetStatus($"Generated {generated} WOK walkmesh file(s){skippedText}.");
            return generated;
        }

        private static BWM CreatePlaceholderRoomWalkmesh(LYTRoom room)
        {
            var center = room?.Position ?? Vector3.Zero;
            const float halfSize = 5f;
            var v1 = new Vector3(center.X - halfSize, center.Y - halfSize, center.Z);
            var v2 = new Vector3(center.X + halfSize, center.Y - halfSize, center.Z);
            var v3 = new Vector3(center.X + halfSize, center.Y + halfSize, center.Z);
            var v4 = new Vector3(center.X - halfSize, center.Y + halfSize, center.Z);

            var bwm = new BWM
            {
                WalkmeshType = BWMType.AreaModel
            };
            bwm.Faces.Add(new BWMFace(v1, v2, v3) { Material = SurfaceMaterial.Stone });
            bwm.Faces.Add(new BWMFace(v1, v3, v4) { Material = SurfaceMaterial.Stone });
            return bwm;
        }

        private void SetStatus(string status)
        {
            if (_statusText != null)
            {
                _statusText.Text = status ?? "";
            }
        }

        public bool TryLaunchBlenderForCurrentLayout()
        {
            if (string.IsNullOrEmpty(Filepath))
            {
                SetBlenderStatus("Save or open a LYT before launching Blender.");
                RefreshBlenderActionState();
                return false;
            }

            var blenderInfo = _detectBlender(null);
            if (blenderInfo == null || !blenderInfo.IsValid)
            {
                SetBlenderStatus(blenderInfo?.Error ?? "No valid Blender installation found.");
                return false;
            }

            if (!blenderInfo.HasKotorblender)
            {
                SetBlenderStatus(blenderInfo.Error ?? "Blender found, but kotorblender is not installed.");
                return false;
            }

            var process = _launchBlender(blenderInfo, 7531, _installation?.Path, Filepath, null, false);
            if (process == null)
            {
                SetBlenderStatus("Failed to launch Blender.");
                return false;
            }

            SetBlenderStatus($"Launched Blender for {Path.GetFileName(Filepath)}.");
            return true;
        }

        private void RefreshBlenderActionState()
        {
            if (_openInBlenderMenuItem != null)
            {
                _openInBlenderMenuItem.IsEnabled = !string.IsNullOrEmpty(Filepath);
            }

            if (string.IsNullOrEmpty(Filepath))
            {
                SetBlenderStatus("Open a LYT file to use Blender.");
            }
        }

        private void SetBlenderStatus(string status)
        {
            _blenderStatus = status ?? "";
            if (_statusText != null && !string.IsNullOrEmpty(_blenderStatus))
            {
                _statusText.Text = $"Blender: {_blenderStatus}";
            }
        }

        public void UpdateZoom(int value)
        {
            if (_graphicsScene == null)
            {
                InitializeGraphicsScene();
            }

            if (_graphicsScene != null)
            {
                // Convert value (0-100) to zoom level (0.1-10.0)
                double zoomLevel = value / 100.0;
                _graphicsScene.ZoomLevel = zoomLevel;
            }
        }

        public void UpdateScene()
        {
            if (_lyt == null)
            {
                return;
            }

            // Validate and ensure LYT data consistency
            ValidateAndFixLYTData();

            // Update room connections based on door hooks
            UpdateRoomConnections();

            // Update graphics scene to render all LYT elements
            UpdateGraphicsScene();
        }

        /// <summary>
        /// Updates the graphics scene to render all LYT layout elements.
        ///     self.scene.clear()
        ///     for room in self._lyt.rooms:
        ///         self.scene.addItem(RoomItem(room, self))
        ///     for track in self._lyt.tracks:
        ///         self.scene.addItem(TrackItem(track, self))
        ///     for obstacle in self._lyt.obstacles:
        ///         self.scene.addItem(ObstacleItem(obstacle, self))
        ///     for doorhook in self._lyt.doorhooks:
        ///         self.scene.addItem(DoorHookItem(doorhook, self))
        /// </summary>
        private void UpdateGraphicsScene()
        {
            if (_graphicsScene == null)
            {
                InitializeGraphicsScene();
            }

            if (_graphicsScene == null)
            {
                return;
            }

            // Clear existing items
            _graphicsScene.Clear();

            // Add room items
            if (_lyt.Rooms != null)
            {
                foreach (var room in _lyt.Rooms)
                {
                    if (room != null)
                    {
                        var roomItem = new RoomItem(_graphicsScene, room);
                        _graphicsScene.AddItem(roomItem);
                    }
                }
            }

            // Add track items
            if (_lyt.Tracks != null)
            {
                foreach (var track in _lyt.Tracks)
                {
                    if (track != null)
                    {
                        var trackItem = new TrackItem(_graphicsScene, track);
                        _graphicsScene.AddItem(trackItem);
                    }
                }
            }

            // Add obstacle items
            if (_lyt.Obstacles != null)
            {
                foreach (var obstacle in _lyt.Obstacles)
                {
                    if (obstacle != null)
                    {
                        var obstacleItem = new ObstacleItem(_graphicsScene, obstacle);
                        _graphicsScene.AddItem(obstacleItem);
                    }
                }
            }

            // Add door hook items
            if (_lyt.DoorHooks != null)
            {
                foreach (var doorHook in _lyt.DoorHooks)
                {
                    if (doorHook != null)
                    {
                        var doorHookItem = new DoorHookItem(_graphicsScene, doorHook);
                        _graphicsScene.AddItem(doorHookItem);
                    }
                }
            }

            RefreshElementLists();
            RefreshSummary();
        }

        private void RefreshElementLists()
        {
            if (_roomsList == null || _tracksList == null || _obstaclesList == null || _doorHooksList == null)
            {
                return;
            }

            _updatingSelectionUi = true;
            try
            {
                _roomsList.ItemsSource = (_lyt.Rooms ?? new List<LYTRoom>())
                    .Select((room, index) => new LayoutElementListItem(FormatRoom(room, index), room))
                    .ToList();
                _tracksList.ItemsSource = (_lyt.Tracks ?? new List<LYTTrack>())
                    .Select((track, index) => new LayoutElementListItem(FormatTrack(track, index), track))
                    .ToList();
                _obstaclesList.ItemsSource = (_lyt.Obstacles ?? new List<LYTObstacle>())
                    .Select((obstacle, index) => new LayoutElementListItem(FormatObstacle(obstacle, index), obstacle))
                    .ToList();
                _doorHooksList.ItemsSource = (_lyt.DoorHooks ?? new List<LYTDoorHook>())
                    .Select((hook, index) => new LayoutElementListItem(FormatDoorHook(hook, index), hook))
                    .ToList();

                SyncElementListSelection();
            }
            finally
            {
                _updatingSelectionUi = false;
            }
        }

        private void SyncElementListSelection()
        {
            SelectListItemByValue(_roomsList, _selectedLayoutElement is LYTRoom ? _selectedLayoutElement : null);
            SelectListItemByValue(_tracksList, _selectedLayoutElement is LYTTrack ? _selectedLayoutElement : null);
            SelectListItemByValue(_obstaclesList, _selectedLayoutElement is LYTObstacle ? _selectedLayoutElement : null);
            SelectListItemByValue(_doorHooksList, _selectedLayoutElement is LYTDoorHook ? _selectedLayoutElement : null);
        }

        private static void SelectListItemByValue(ListBox list, object value)
        {
            if (list == null)
            {
                return;
            }

            if (value == null)
            {
                list.SelectedItem = null;
                return;
            }

            if (list.Items != null)
            {
                foreach (var item in list.Items)
                {
                    var listItem = item as LayoutElementListItem;
                    if (listItem != null && ReferenceEquals(listItem.Value, value))
                    {
                        list.SelectedItem = listItem;
                        return;
                    }
                }
            }

            list.SelectedItem = null;
        }

        private void RefreshSummary()
        {
            int roomCount = _lyt?.Rooms?.Count ?? 0;
            int trackCount = _lyt?.Tracks?.Count ?? 0;
            int obstacleCount = _lyt?.Obstacles?.Count ?? 0;
            int hookCount = _lyt?.DoorHooks?.Count ?? 0;
            if (_summaryText != null)
            {
                _summaryText.Text = $"Rooms {roomCount} | Tracks {trackCount} | Obstacles {obstacleCount} | Door hooks {hookCount}";
            }
            if (_statusText != null)
            {
                var layoutStatus = $"Layout ready: {roomCount} rooms, {trackCount} tracks, {obstacleCount} obstacles, {hookCount} door hooks";
                _statusText.Text = string.IsNullOrEmpty(_blenderStatus)
                    ? layoutStatus
                    : $"{layoutStatus} | Blender: {_blenderStatus}";
            }
        }

        private void SelectLayoutElement(object item)
        {
            if (_updatingSelectionUi)
            {
                return;
            }

            _selectedAssetKind = null;
            _selectedAssetName = null;
            _selectedLayoutElement = item is LayoutElementListItem listItem ? listItem.Value : item;
            _graphicsScene?.SelectElement(_selectedLayoutElement);
            PopulateSelectionDetails();
        }

        private void SelectImportedAsset(string kind, string name, string path)
        {
            _selectedLayoutElement = null;
            _selectedAssetKind = kind;
            _selectedAssetName = name;
            _graphicsScene?.SelectElement(null);
            if (_selectionTitleText != null)
            {
                _selectionTitleText.Text = $"{kind}: {name}";
            }
            if (_selectionPathText != null)
            {
                _selectionPathText.Text = string.IsNullOrEmpty(path) ? "No file path recorded." : path;
            }
            SetText(_nameEdit, name);
            SetText(_doorEdit, "");
            SetText(_xEdit, "");
            SetText(_yEdit, "");
            SetText(_zEdit, "");
            if (_applySelectionButton != null)
            {
                _applySelectionButton.IsEnabled = false;
            }
            if (_addSelectedModelButton != null)
            {
                _addSelectedModelButton.IsEnabled = string.Equals(kind, "Model", StringComparison.OrdinalIgnoreCase);
            }
            if (_statusText != null)
            {
                _statusText.Text = $"{kind} selected: {name}";
            }
        }

        private void PopulateSelectionDetails()
        {
            if (_selectedLayoutElement == null)
            {
                if (_selectionTitleText != null) _selectionTitleText.Text = "No selection";
                if (_selectionPathText != null) _selectionPathText.Text = "";
                SetText(_nameEdit, "");
                SetText(_doorEdit, "");
                SetText(_xEdit, "");
                SetText(_yEdit, "");
                SetText(_zEdit, "");
                if (_applySelectionButton != null) _applySelectionButton.IsEnabled = false;
                if (_addSelectedModelButton != null) _addSelectedModelButton.IsEnabled = false;
                return;
            }

            string title;
            string name;
            string door = "";
            Vector3 position;

            switch (_selectedLayoutElement)
            {
                case LYTRoom room:
                    title = "Room";
                    name = room.Model?.ToString() ?? "";
                    position = room.Position;
                    break;
                case LYTTrack track:
                    title = "Track";
                    name = track.Model?.ToString() ?? "";
                    position = track.Position;
                    break;
                case LYTObstacle obstacle:
                    title = "Obstacle";
                    name = obstacle.Model?.ToString() ?? "";
                    position = obstacle.Position;
                    break;
                case LYTDoorHook hook:
                    title = "Door Hook";
                    name = hook.Room ?? "";
                    door = hook.Door ?? "";
                    position = hook.Position;
                    break;
                default:
                    return;
            }

            if (_selectionTitleText != null) _selectionTitleText.Text = $"{title}: {name}";
            if (_selectionPathText != null) _selectionPathText.Text = "Edit the selected layout element, then apply.";
            SetText(_nameEdit, name);
            SetText(_doorEdit, door);
            SetText(_xEdit, position.X.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
            SetText(_yEdit, position.Y.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
            SetText(_zEdit, position.Z.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture));
            if (_applySelectionButton != null) _applySelectionButton.IsEnabled = true;
            if (_addSelectedModelButton != null) _addSelectedModelButton.IsEnabled = false;
            if (_statusText != null) _statusText.Text = $"{title} selected";
        }

        private void ApplySelectionEdits()
        {
            if (_selectedLayoutElement == null)
            {
                return;
            }

            string name = _nameEdit?.Text?.Trim() ?? "";
            string door = _doorEdit?.Text?.Trim() ?? "";
            var position = new Vector3(
                ParseFloat(_xEdit?.Text),
                ParseFloat(_yEdit?.Text),
                ParseFloat(_zEdit?.Text));

            switch (_selectedLayoutElement)
            {
                case LYTRoom room:
                    room.Model = ResRefFromEditableText(name, "default_room");
                    room.Position = position;
                    break;
                case LYTTrack track:
                    track.Model = ResRefFromEditableText(name, "default_track");
                    track.Position = position;
                    break;
                case LYTObstacle obstacle:
                    obstacle.Model = ResRefFromEditableText(name, "default_obstacle");
                    obstacle.Position = position;
                    break;
                case LYTDoorHook hook:
                    hook.Room = name;
                    hook.Door = door;
                    hook.Position = position;
                    break;
                default:
                    return;
            }

            MarkDocumentDirty();
            UpdateScene();
            PopulateSelectionDetails();
        }

        private void AddSelectedModelAsRoom()
        {
            if (!string.Equals(_selectedAssetKind, "Model", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(_selectedAssetName))
            {
                return;
            }

            var model = ResRefFromEditableText(_selectedAssetName);
            if (model.IsBlank())
            {
                return;
            }

            var room = new LYTRoom(model, new Vector3(0, 0, 0));
            _lyt.Rooms.Add(room);
            _selectedLayoutElement = room;
            _selectedAssetKind = null;
            _selectedAssetName = null;
            MarkDocumentDirty();
            UpdateScene();
            PopulateSelectionDetails();
        }

        private static float ParseFloat(string text)
        {
            return float.TryParse(text, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float value)
                ? value
                : 0f;
        }

        internal static ResRef ResRefFromEditableText(string text)
        {
            string value = text?.Trim() ?? string.Empty;
            return string.IsNullOrEmpty(value) || !ResRef.IsValid(value) ? ResRef.FromBlank() : new ResRef(value);
        }

        private static ResRef ResRefFromEditableText(string text, string fallback)
        {
            ResRef value = ResRefFromEditableText(text);
            return value.IsBlank() ? new ResRef(fallback) : value;
        }

        private static void SetText(TextBox box, string text)
        {
            if (box != null)
            {
                box.Text = text ?? "";
            }
        }

        private static string FormatRoom(LYTRoom room, int index)
        {
            return $"{index + 1}. {room?.Model} ({FormatPosition(room?.Position ?? Vector3.Zero)})";
        }

        private static string FormatTrack(LYTTrack track, int index)
        {
            return $"{index + 1}. {track?.Model} ({FormatPosition(track?.Position ?? Vector3.Zero)})";
        }

        private static string FormatObstacle(LYTObstacle obstacle, int index)
        {
            return $"{index + 1}. {obstacle?.Model} ({FormatPosition(obstacle?.Position ?? Vector3.Zero)})";
        }

        private static string FormatDoorHook(LYTDoorHook hook, int index)
        {
            return $"{index + 1}. {hook?.Room}/{hook?.Door} ({FormatPosition(hook?.Position ?? Vector3.Zero)})";
        }

        private static string FormatPosition(Vector3 position)
        {
            return string.Format(
                System.Globalization.CultureInfo.InvariantCulture,
                "{0:0.##}, {1:0.##}, {2:0.##}",
                position.X,
                position.Y,
                position.Z);
        }

        private sealed class LayoutElementListItem
        {
            public LayoutElementListItem(string text, object value)
            {
                Text = text;
                Value = value;
            }

            public string Text { get; }
            public object Value { get; }

            public override string ToString()
            {
                return Text;
            }
        }

        /// <summary>
        /// Validates and fixes LYT data to ensure consistency.
        /// Performs comprehensive validation of all LYT components.
        /// </summary>
        private void ValidateAndFixLYTData()
        {
            if (_lyt == null)
            {
                return;
            }

            // Validate rooms
            ValidateRooms();

            // Validate tracks
            ValidateTracks();

            // Validate obstacles
            ValidateObstacles();

            // Validate door hooks
            ValidateDoorHooks();
        }

        /// <summary>
        /// Validates room data: ResRefs, positions, and ensures no duplicates.
        /// </summary>
        private void ValidateRooms()
        {
            if (_lyt.Rooms == null)
            {
                _lyt.Rooms = new List<LYTRoom>();
                return;
            }

            var validRooms = new List<LYTRoom>();
            var seenRoomNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var roomIndex = 0;

            foreach (var room in _lyt.Rooms)
            {
                if (room == null)
                {
                    System.Console.WriteLine($"Warning: Null room at index {roomIndex}, skipping.");
                    roomIndex++;
                    continue;
                }

                // Validate ResRef
                if (room.Model == null)
                {
                    System.Console.WriteLine($"Warning: Room at index {roomIndex} has null Model, setting to default.");
                    room.Model = new ResRef("default_room");
                }
                else
                {
                    string modelStr = room.Model.ToString();
                    if (string.IsNullOrEmpty(modelStr))
                    {
                        System.Console.WriteLine($"Warning: Room at index {roomIndex} has empty Model, setting to default.");
                        room.Model = new ResRef("default_room");
                    }
                    else if (!ResRef.IsValid(modelStr))
                    {
                        System.Console.WriteLine($"Warning: Room at index {roomIndex} has invalid ResRef '{modelStr}', truncating if needed.");
                        try
                        {
                            room.Model.SetData(modelStr, truncate: true);
                        }
                        catch
                        {
                            room.Model = new ResRef("default_room");
                        }
                    }
                }

                // Validate position
                if (float.IsNaN(room.Position.X) || float.IsNaN(room.Position.Y) || float.IsNaN(room.Position.Z) ||
                    float.IsInfinity(room.Position.X) || float.IsInfinity(room.Position.Y) || float.IsInfinity(room.Position.Z))
                {
                    System.Console.WriteLine($"Warning: Room '{room.Model}' at index {roomIndex} has invalid position (NaN/Infinity), resetting to (0, 0, 0).");
                    room.Position = new Vector3(0, 0, 0);
                }

                // Check for duplicate room names (case-insensitive)
                string roomName = room.Model.ToString().ToLowerInvariant();
                if (seenRoomNames.Contains(roomName))
                {
                    System.Console.WriteLine($"Warning: Duplicate room name '{room.Model}' at index {roomIndex}, appending index to make unique.");
                    room.Model = new ResRef($"{room.Model}_{roomIndex}");
                    roomName = room.Model.ToString().ToLowerInvariant();
                }

                seenRoomNames.Add(roomName);

                // Ensure Connections is initialized
                if (room.Connections == null)
                {
                    room.Connections = new HashSet<LYTRoom>();
                }

                validRooms.Add(room);
                roomIndex++;
            }

            _lyt.Rooms = validRooms;
        }

        /// <summary>
        /// Validates track data: ResRefs and positions.
        /// </summary>
        private void ValidateTracks()
        {
            if (_lyt.Tracks == null)
            {
                _lyt.Tracks = new List<LYTTrack>();
                return;
            }

            var validTracks = new List<LYTTrack>();
            var trackIndex = 0;

            foreach (var track in _lyt.Tracks)
            {
                if (track == null)
                {
                    System.Console.WriteLine($"Warning: Null track at index {trackIndex}, skipping.");
                    trackIndex++;
                    continue;
                }

                // Validate ResRef
                if (track.Model == null)
                {
                    System.Console.WriteLine($"Warning: Track at index {trackIndex} has null Model, setting to default.");
                    track.Model = new ResRef("default_track");
                }
                else
                {
                    string modelStr = track.Model.ToString();
                    if (string.IsNullOrEmpty(modelStr))
                    {
                        System.Console.WriteLine($"Warning: Track at index {trackIndex} has empty Model, setting to default.");
                        track.Model = new ResRef("default_track");
                    }
                    else if (!ResRef.IsValid(modelStr))
                    {
                        System.Console.WriteLine($"Warning: Track at index {trackIndex} has invalid ResRef '{modelStr}', truncating if needed.");
                        try
                        {
                            track.Model.SetData(modelStr, truncate: true);
                        }
                        catch
                        {
                            track.Model = new ResRef("default_track");
                        }
                    }
                }

                // Validate position
                if (float.IsNaN(track.Position.X) || float.IsNaN(track.Position.Y) || float.IsNaN(track.Position.Z) ||
                    float.IsInfinity(track.Position.X) || float.IsInfinity(track.Position.Y) || float.IsInfinity(track.Position.Z))
                {
                    System.Console.WriteLine($"Warning: Track '{track.Model}' at index {trackIndex} has invalid position (NaN/Infinity), resetting to (0, 0, 0).");
                    track.Position = new Vector3(0, 0, 0);
                }

                validTracks.Add(track);
                trackIndex++;
            }

            _lyt.Tracks = validTracks;
        }

        /// <summary>
        /// Validates obstacle data: ResRefs and positions.
        /// </summary>
        private void ValidateObstacles()
        {
            if (_lyt.Obstacles == null)
            {
                _lyt.Obstacles = new List<LYTObstacle>();
                return;
            }

            var validObstacles = new List<LYTObstacle>();
            var obstacleIndex = 0;

            foreach (var obstacle in _lyt.Obstacles)
            {
                if (obstacle == null)
                {
                    System.Console.WriteLine($"Warning: Null obstacle at index {obstacleIndex}, skipping.");
                    obstacleIndex++;
                    continue;
                }

                // Validate ResRef
                if (obstacle.Model == null)
                {
                    System.Console.WriteLine($"Warning: Obstacle at index {obstacleIndex} has null Model, setting to default.");
                    obstacle.Model = new ResRef("default_obstacle");
                }
                else
                {
                    string modelStr = obstacle.Model.ToString();
                    if (string.IsNullOrEmpty(modelStr))
                    {
                        System.Console.WriteLine($"Warning: Obstacle at index {obstacleIndex} has empty Model, setting to default.");
                        obstacle.Model = new ResRef("default_obstacle");
                    }
                    else if (!ResRef.IsValid(modelStr))
                    {
                        System.Console.WriteLine($"Warning: Obstacle at index {obstacleIndex} has invalid ResRef '{modelStr}', truncating if needed.");
                        try
                        {
                            obstacle.Model.SetData(modelStr, truncate: true);
                        }
                        catch
                        {
                            obstacle.Model = new ResRef("default_obstacle");
                        }
                    }
                }

                // Validate position
                if (float.IsNaN(obstacle.Position.X) || float.IsNaN(obstacle.Position.Y) || float.IsNaN(obstacle.Position.Z) ||
                    float.IsInfinity(obstacle.Position.X) || float.IsInfinity(obstacle.Position.Y) || float.IsInfinity(obstacle.Position.Z))
                {
                    System.Console.WriteLine($"Warning: Obstacle '{obstacle.Model}' at index {obstacleIndex} has invalid position (NaN/Infinity), resetting to (0, 0, 0).");
                    obstacle.Position = new Vector3(0, 0, 0);
                }

                validObstacles.Add(obstacle);
                obstacleIndex++;
            }

            _lyt.Obstacles = validObstacles;
        }

        /// <summary>
        /// Validates door hook data: room references, ResRefs, positions, and quaternions.
        /// </summary>
        private void ValidateDoorHooks()
        {
            if (_lyt.DoorHooks == null)
            {
                _lyt.DoorHooks = new List<LYTDoorHook>();
                return;
            }

            // Build a map of room names (case-insensitive) for quick lookup
            var roomNameMap = new Dictionary<string, LYTRoom>(StringComparer.OrdinalIgnoreCase);
            if (_lyt.Rooms != null)
            {
                foreach (var room in _lyt.Rooms)
                {
                    if (room != null && room.Model != null)
                    {
                        string roomName = room.Model.ToString().ToLowerInvariant();
                        if (!roomNameMap.ContainsKey(roomName))
                        {
                            roomNameMap[roomName] = room;
                        }
                    }
                }
            }

            var validDoorHooks = new List<LYTDoorHook>();
            var doorHookIndex = 0;

            foreach (var doorHook in _lyt.DoorHooks)
            {
                if (doorHook == null)
                {
                    System.Console.WriteLine($"Warning: Null door hook at index {doorHookIndex}, skipping.");
                    doorHookIndex++;
                    continue;
                }

                // Validate room reference (must match an existing room, case-insensitive)
                if (string.IsNullOrEmpty(doorHook.Room))
                {
                    System.Console.WriteLine($"Warning: Door hook at index {doorHookIndex} has empty Room name, setting to first room if available.");
                    if (_lyt.Rooms != null && _lyt.Rooms.Count > 0 && _lyt.Rooms[0] != null && _lyt.Rooms[0].Model != null)
                    {
                        doorHook.Room = _lyt.Rooms[0].Model.ToString();
                    }
                    else
                    {
                        System.Console.WriteLine($"Warning: No rooms available, skipping door hook at index {doorHookIndex}.");
                        doorHookIndex++;
                        continue;
                    }
                }

                string roomName = doorHook.Room.ToLowerInvariant();
                if (!roomNameMap.ContainsKey(roomName))
                {
                    System.Console.WriteLine($"Warning: Door hook at index {doorHookIndex} references non-existent room '{doorHook.Room}', setting to first room if available.");
                    if (_lyt.Rooms != null && _lyt.Rooms.Count > 0 && _lyt.Rooms[0] != null && _lyt.Rooms[0].Model != null)
                    {
                        doorHook.Room = _lyt.Rooms[0].Model.ToString();
                        roomName = doorHook.Room.ToLowerInvariant();
                    }
                    else
                    {
                        System.Console.WriteLine($"Warning: No rooms available, skipping door hook at index {doorHookIndex}.");
                        doorHookIndex++;
                        continue;
                    }
                }

                // Validate door name (should not be empty, but can be any string)
                if (string.IsNullOrEmpty(doorHook.Door))
                {
                    System.Console.WriteLine($"Warning: Door hook at index {doorHookIndex} has empty Door name, setting to default.");
                    doorHook.Door = "default_door";
                }

                // Validate position
                if (float.IsNaN(doorHook.Position.X) || float.IsNaN(doorHook.Position.Y) || float.IsNaN(doorHook.Position.Z) ||
                    float.IsInfinity(doorHook.Position.X) || float.IsInfinity(doorHook.Position.Y) || float.IsInfinity(doorHook.Position.Z))
                {
                    System.Console.WriteLine($"Warning: Door hook '{doorHook.Room}/{doorHook.Door}' at index {doorHookIndex} has invalid position (NaN/Infinity), resetting to (0, 0, 0).");
                    doorHook.Position = new Vector3(0, 0, 0);
                }

                // Validate and normalize quaternion
                if (doorHook.Orientation.X == 0 && doorHook.Orientation.Y == 0 && doorHook.Orientation.Z == 0 && doorHook.Orientation.W == 0)
                {
                    System.Console.WriteLine($"Warning: Door hook '{doorHook.Room}/{doorHook.Door}' at index {doorHookIndex} has zero quaternion, setting to identity.");
                    doorHook.Orientation = Quaternion.Identity;
                }
                else
                {
                    // Normalize quaternion if needed
                    doorHook.Orientation = NormalizeQuaternion(doorHook.Orientation);
                }

                validDoorHooks.Add(doorHook);
                doorHookIndex++;
            }

            _lyt.DoorHooks = validDoorHooks;
        }

        /// <summary>
        /// Normalizes a quaternion to ensure it has unit length.
        /// Returns identity quaternion if the input is invalid (zero length).
        /// </summary>
        /// <param name="q">The quaternion to normalize.</param>
        /// <returns>A normalized quaternion.</returns>
        private Quaternion NormalizeQuaternion(Quaternion q)
        {
            float lengthSquared = q.X * q.X + q.Y * q.Y + q.Z * q.Z + q.W * q.W;

            // Check for zero or near-zero length
            if (lengthSquared < float.Epsilon)
            {
                return Quaternion.Identity;
            }

            // Check for NaN or Infinity
            if (float.IsNaN(lengthSquared) || float.IsInfinity(lengthSquared) ||
                float.IsNaN(q.X) || float.IsNaN(q.Y) || float.IsNaN(q.Z) || float.IsNaN(q.W) ||
                float.IsInfinity(q.X) || float.IsInfinity(q.Y) || float.IsInfinity(q.Z) || float.IsInfinity(q.W))
            {
                return Quaternion.Identity;
            }

            float length = (float)Math.Sqrt(lengthSquared);

            // Normalize if not already normalized (allow small tolerance)
            if (Math.Abs(length - 1.0f) > 0.0001f)
            {
                float invLength = 1.0f / length;
                return new Quaternion(q.X * invLength, q.Y * invLength, q.Z * invLength, q.W * invLength);
            }

            return q;
        }

        /// <summary>
        /// Updates room connections based on door hooks.
        /// Rooms that share door hooks (same position within tolerance) are considered connected.
        /// </summary>
        private void UpdateRoomConnections()
        {
            if (_lyt == null || _lyt.Rooms == null || _lyt.DoorHooks == null)
            {
                return;
            }

            // Clear all existing connections
            foreach (var room in _lyt.Rooms)
            {
                if (room != null && room.Connections != null)
                {
                    room.Connections.Clear();
                }
            }

            // Build a map of room names (case-insensitive) for quick lookup
            var roomNameMap = new Dictionary<string, LYTRoom>(StringComparer.OrdinalIgnoreCase);
            foreach (var room in _lyt.Rooms)
            {
                if (room != null && room.Model != null)
                {
                    string roomName = room.Model.ToString().ToLowerInvariant();
                    if (!roomNameMap.ContainsKey(roomName))
                    {
                        roomNameMap[roomName] = room;
                    }
                }
            }

            // Group door hooks by position (within tolerance) to find connections
            // Door hooks at the same position (within 0.1 units) likely connect rooms
            const float positionTolerance = 0.1f;
            var doorHookGroups = new Dictionary<string, List<LYTDoorHook>>();

            foreach (var doorHook in _lyt.DoorHooks)
            {
                if (doorHook == null)
                {
                    continue;
                }

                // Create a position key rounded to tolerance
                int xKey = (int)(doorHook.Position.X / positionTolerance);
                int yKey = (int)(doorHook.Position.Y / positionTolerance);
                int zKey = (int)(doorHook.Position.Z / positionTolerance);
                string positionKey = $"{xKey}_{yKey}_{zKey}";

                if (!doorHookGroups.ContainsKey(positionKey))
                {
                    doorHookGroups[positionKey] = new List<LYTDoorHook>();
                }

                doorHookGroups[positionKey].Add(doorHook);
            }

            // Connect rooms that share door hook positions
            foreach (var group in doorHookGroups.Values)
            {
                if (group.Count < 2)
                {
                    continue; // Need at least 2 door hooks at same position to connect rooms
                }

                // Get all unique rooms from this group of door hooks
                var roomsInGroup = new HashSet<LYTRoom>();
                foreach (var doorHook in group)
                {
                    if (doorHook == null || string.IsNullOrEmpty(doorHook.Room))
                    {
                        continue;
                    }

                    string roomName = doorHook.Room.ToLowerInvariant();
                    if (roomNameMap.TryGetValue(roomName, out LYTRoom room))
                    {
                        roomsInGroup.Add(room);
                    }
                }

                // Connect all rooms in this group to each other (bidirectional)
                var roomsList = roomsInGroup.ToList();
                for (int i = 0; i < roomsList.Count; i++)
                {
                    for (int j = i + 1; j < roomsList.Count; j++)
                    {
                        var room1 = roomsList[i];
                        var room2 = roomsList[j];

                        if (room1 != null && room2 != null)
                        {
                            if (room1.Connections == null)
                            {
                                room1.Connections = new HashSet<LYTRoom>();
                            }
                            if (room2.Connections == null)
                            {
                                room2.Connections = new HashSet<LYTRoom>();
                            }

                            room1.Connections.Add(room2);
                            room2.Connections.Add(room1);
                        }
                    }
                }
            }
        }

        public async void ImportTexture()
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null)
            {
                return;
            }

            try
            {
                var options = new FilePickerOpenOptions
                {
                    Title = "Import Texture",
                    AllowMultiple = true,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Image Files")
                        {
                            Patterns = new[] { "*.tpc", "*.tga", "*.dds", "*.png", "*.jpg", "*.jpeg", "*.bmp" },
                            MimeTypes = new[] { "image/tga", "image/dds", "image/png", "image/jpeg", "image/bmp" }
                        },
                        new FilePickerFileType("TPC Files") { Patterns = new[] { "*.tpc" } },
                        new FilePickerFileType("TGA Files") { Patterns = new[] { "*.tga" } },
                        new FilePickerFileType("DDS Files") { Patterns = new[] { "*.dds" } },
                        new FilePickerFileType("PNG Files") { Patterns = new[] { "*.png" } },
                        new FilePickerFileType("JPEG Files") { Patterns = new[] { "*.jpg", "*.jpeg" } },
                        new FilePickerFileType("BMP Files") { Patterns = new[] { "*.bmp" } },
                        new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
                    }
                };

                var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
                if (files == null || files.Count == 0)
                {
                    return;
                }

                foreach (var file in files)
                {
                    string filePath = file.Path.LocalPath;
                    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    {
                        continue;
                    }

                    await ImportTextureFile(filePath);
                }

                UpdateTextureBrowser();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error importing texture: {ex}");
            }
        }

        private async Task ImportTextureFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    System.Console.WriteLine($"Error: Texture file does not exist: {filePath}");
                    return;
                }

                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath).ToLowerInvariant();
                string targetResref = fileName;

                // Validate file extension
                string[] supportedExtensions = { ".tpc", ".tga", ".dds", ".png", ".jpg", ".jpeg", ".bmp" };
                bool isSupported = false;
                foreach (string ext in supportedExtensions)
                {
                    if (extension == ext)
                    {
                        isSupported = true;
                        break;
                    }
                }

                if (!isSupported)
                {
                    System.Console.WriteLine($"Error: Unsupported texture format: {extension}. Supported formats: TPC, TGA, DDS, PNG, JPG, BMP");
                    return;
                }

                // Determine if we need to convert the texture
                bool needsConversion = extension != ".tpc" && extension != ".tga" && extension != ".dds";

                string overridePath = GetOverrideDirectory();
                if (string.IsNullOrEmpty(overridePath))
                {
                    System.Console.WriteLine("Warning: Could not determine override directory. Texture will not be saved to installation.");
                    return;
                }

                // Ensure override/textures directory exists
                string texturesPath = Path.Combine(overridePath, "textures");
                if (!Directory.Exists(texturesPath))
                {
                    try
                    {
                        Directory.CreateDirectory(texturesPath);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error: Could not create textures directory at {texturesPath}: {ex}");
                        return;
                    }
                }

                string outputTpcPath = Path.Combine(texturesPath, $"{targetResref}.tpc");
                string txiPath = Path.ChangeExtension(filePath, ".txi");

                TPC tpc = null;

                // Read the texture based on its format
                // TPCAuto.ReadTpc can directly handle TPC, TGA, and DDS formats
                if (extension == ".tpc" || extension == ".tga" || extension == ".dds")
                {
                    try
                    {
                        // TPC, TGA, and DDS can be read directly by TPCAuto
                        tpc = TPCAuto.ReadTpc(filePath, txiSource: File.Exists(txiPath) ? txiPath : null);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error: Failed to read {extension.ToUpperInvariant()} file {filePath}: {ex}");
                        return;
                    }
                }
                else if (needsConversion)
                {
                    // PNG, JPG, BMP formats require conversion to TPC
                    // Load the image using Avalonia's Bitmap, extract RGBA pixel data, and create TPC
                    try
                    {
                        tpc = ConvertImageToTpc(filePath);
                        if (tpc == null)
                        {
                            System.Console.WriteLine($"Error: Failed to convert {extension.ToUpperInvariant()} file {filePath} to TPC format.");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error: Failed to convert {extension.ToUpperInvariant()} file {filePath} to TPC: {ex}");
                        return;
                    }
                }

                if (tpc == null)
                {
                    System.Console.WriteLine($"Failed to load texture from {filePath}");
                    return;
                }

                // Check if output file already exists and handle overwrite
                if (File.Exists(outputTpcPath))
                {
                    System.Console.WriteLine($"Warning: Texture {targetResref}.tpc already exists in override directory. It will be overwritten.");
                }

                // Write as TPC to override directory
                try
                {
                    TPCAuto.WriteTpc(tpc, outputTpcPath, ResourceType.TPC);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Error: Failed to write TPC file to {outputTpcPath}: {ex}");
                    return;
                }

                // Write TXI file if it exists
                if (!string.IsNullOrEmpty(tpc.Txi))
                {
                    try
                    {
                        string outputTxiPath = Path.ChangeExtension(outputTpcPath, ".txi");
                        File.WriteAllText(outputTxiPath, tpc.Txi, System.Text.Encoding.ASCII);
                        System.Console.WriteLine($"Also wrote TXI file: {Path.GetFileName(outputTxiPath)}");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Warning: Failed to write TXI file: {ex}");
                    }
                }

                // Store the imported texture reference
                _importedTextures[targetResref] = outputTpcPath;

                System.Console.WriteLine($"Successfully imported texture: {targetResref} -> {outputTpcPath}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error importing texture file {filePath}: {ex}");
            }
        }

        /// <summary>
        /// Converts a PNG, JPG, or BMP image file to TPC format.
        /// Loads the image using Avalonia's Bitmap, extracts RGBA pixel data, and creates a TPC object.
        /// </summary>
        /// <param name="filePath">Path to the image file (PNG, JPG, or BMP).</param>
        /// <returns>TPC object created from the image, or null if conversion fails.</returns>
        private TPC ConvertImageToTpc(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            try
            {
                // Load the image using Avalonia's Bitmap
                Bitmap bitmap;
                using (var fileStream = File.OpenRead(filePath))
                {
                    bitmap = new Bitmap(fileStream);
                }

                // Get image dimensions
                int width = bitmap.PixelSize.Width;
                int height = bitmap.PixelSize.Height;

                if (width <= 0 || height <= 0)
                {
                    return null;
                }

                // Extract RGBA pixel data from the bitmap
                // Avalonia bitmaps use BGRA format, we need to convert to RGBA
                byte[] rgbaData = ExtractRgbaFromBitmap(bitmap, width, height);

                if (rgbaData == null || rgbaData.Length == 0)
                {
                    return null;
                }

                // Create TPC object
                TPC tpc = new TPC();
                tpc.IsAnimated = false;
                tpc.IsCubeMap = false;

                // Check if image has alpha channel
                bool hasAlpha = HasAlphaChannel(rgbaData);

                // Determine format
                TPCTextureFormat format = hasAlpha ? TPCTextureFormat.RGBA : TPCTextureFormat.RGB;

                // Set the mipmap data
                // For RGB format, we need to convert RGBA to RGB
                byte[] formatData;
                if (hasAlpha)
                {
                    formatData = rgbaData;
                }
                else
                {
                    // Convert RGBA to RGB by removing alpha channel
                    formatData = new byte[width * height * 3];
                    for (int i = 0; i < width * height; i++)
                    {
                        formatData[i * 3 + 0] = rgbaData[i * 4 + 0]; // R
                        formatData[i * 3 + 1] = rgbaData[i * 4 + 1]; // G
                        formatData[i * 3 + 2] = rgbaData[i * 4 + 2]; // B
                    }
                }

                // Use TPC.SetSingle to properly set the format and data
                tpc.SetSingle(formatData, format, width, height);

                return tpc;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error converting image to TPC: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Extracts RGBA pixel data from an Avalonia Bitmap.
        /// Converts from BGRA (Avalonia's native format) to RGBA.
        /// </summary>
        /// <param name="bitmap">The Avalonia Bitmap to extract pixels from.</param>
        /// <param name="width">Width of the image.</param>
        /// <param name="height">Height of the image.</param>
        /// <returns>RGBA pixel data as byte array, or null if extraction fails.</returns>
        private unsafe byte[] ExtractRgbaFromBitmap(Bitmap bitmap, int width, int height)
        {
            try
            {
                // Convert Bitmap to WriteableBitmap to access pixel data
                // Avalonia's regular Bitmap may not support Lock() in all versions
                WriteableBitmap writeableBitmap;
                if (bitmap is WriteableBitmap wb)
                {
                    writeableBitmap = wb;
                }
                else
                {
                    // Create a RenderTargetBitmap and render the original bitmap to it
                    // In Avalonia 11, RenderTargetBitmap doesn't have Lock(), so we save to stream and reload
                    var renderTarget = new RenderTargetBitmap(new PixelSize(width, height));
                    using (var context = renderTarget.CreateDrawingContext())
                    {
                        context.DrawImage(bitmap, new Rect(0, 0, width, height));
                    }

                    // Save RenderTargetBitmap to memory stream and decode as WriteableBitmap
                    using (var memoryStream = new MemoryStream())
                    {
                        renderTarget.Save(memoryStream);
                        memoryStream.Position = 0;
                        writeableBitmap = WriteableBitmap.Decode(memoryStream);
                    }
                    renderTarget.Dispose();
                }

                // Extract pixel data using Lock()
                byte[] rgbaData = new byte[width * height * 4];
                using (var lockedBitmap = writeableBitmap.Lock())
                {
                    int rowStride = lockedBitmap.RowBytes;
                    unsafe
                    {
                        byte* pixelPtr = (byte*)lockedBitmap.Address;

                        // Read pixel data row by row
                        for (int y = 0; y < height; y++)
                        {
                            for (int x = 0; x < width; x++)
                            {
                                // Calculate pixel offset (RGBA format, 4 bytes per pixel)
                                int pixelOffset = y * rowStride + x * 4;
                                int dstIndex = (y * width + x) * 4;

                                // Avalonia uses RGBA8888 format, so data is already in RGBA order
                                rgbaData[dstIndex + 0] = pixelPtr[pixelOffset + 0]; // R
                                rgbaData[dstIndex + 1] = pixelPtr[pixelOffset + 1]; // G
                                rgbaData[dstIndex + 2] = pixelPtr[pixelOffset + 2]; // B
                                rgbaData[dstIndex + 3] = pixelPtr[pixelOffset + 3]; // A
                            }
                        }
                    }
                }

                // Clean up if we created a new WriteableBitmap
                if (!(bitmap is WriteableBitmap))
                {
                    writeableBitmap.Dispose();
                }

                return rgbaData;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error extracting RGBA from bitmap: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Checks if pixel data contains an alpha channel with non-opaque values.
        /// </summary>
        /// <param name="pixels">RGBA pixel data (4 bytes per pixel).</param>
        /// <returns>True if any pixel has alpha < 255, false otherwise.</returns>
        private static bool HasAlphaChannel(byte[] pixels)
        {
            if (pixels == null || pixels.Length < 4)
            {
                return false;
            }

            // Check alpha channel (every 4th byte starting at index 3)
            // Early exit on first non-opaque pixel
            for (int i = 3; i < pixels.Length; i += 4)
            {
                if (pixels[i] != 0xFF)
                {
                    return true;
                }
            }

            return false;
        }

        private string GetOverrideDirectory()
        {
            if (_installation == null)
            {
                return null;
            }

            try
            {
                string installPath = _installation.Path;
                if (string.IsNullOrEmpty(installPath) || !Directory.Exists(installPath))
                {
                    return null;
                }

                // Standard KOTOR override directory is at <installPath>/override
                string overridePath = Path.Combine(installPath, "override");
                return overridePath;
            }
            catch
            {
                return null;
            }
        }

        public async void ImportModel()
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null)
            {
                return;
            }

            try
            {
                var options = new FilePickerOpenOptions
                {
                    Title = "Import Model",
                    AllowMultiple = true,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Model Files")
                        {
                            Patterns = new[] { "*.mdl", "*.mdx" },
                            MimeTypes = new[] { "application/x-binary" }
                        },
                        new FilePickerFileType("MDL Files") { Patterns = new[] { "*.mdl" } },
                        new FilePickerFileType("MDX Files") { Patterns = new[] { "*.mdx" } },
                        new FilePickerFileType("All Files") { Patterns = new[] { "*.*" } }
                    }
                };

                var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
                if (files == null || files.Count == 0)
                {
                    return;
                }

                foreach (var file in files)
                {
                    string filePath = file.Path.LocalPath;
                    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                    {
                        continue;
                    }

                    await ImportModelFile(filePath);
                }

                UpdateModelBrowser();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error importing model: {ex}");
            }
        }

        private async Task ImportModelFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    System.Console.WriteLine($"Error: Model file does not exist: {filePath}");
                    return;
                }

                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath).ToLowerInvariant();
                string targetResref = fileName;

                // Validate file extension
                bool isMdl = extension == ".mdl";
                bool isMdx = extension == ".mdx";

                if (!isMdl && !isMdx)
                {
                    System.Console.WriteLine($"Error: Unsupported model format: {extension}. Supported formats: MDL, MDX");
                    return;
                }

                string overridePath = GetOverrideDirectory();
                if (string.IsNullOrEmpty(overridePath))
                {
                    System.Console.WriteLine("Warning: Could not determine override directory. Model will not be saved to installation.");
                    return;
                }

                // Ensure override/models directory exists
                string modelsPath = Path.Combine(overridePath, "models");
                if (!Directory.Exists(modelsPath))
                {
                    try
                    {
                        Directory.CreateDirectory(modelsPath);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error: Could not create models directory at {modelsPath}: {ex}");
                        return;
                    }
                }

                string sourceMdlPath = null;
                string sourceMdxPath = null;
                string outputMdlPath = null;
                string outputMdxPath = null;

                if (isMdl)
                {
                    // User selected MDL file
                    sourceMdlPath = filePath;
                    outputMdlPath = Path.Combine(modelsPath, $"{targetResref}.mdl");

                    // Look for corresponding MDX file in the same directory
                    string sourceMdxPathCandidate = Path.ChangeExtension(filePath, ".mdx");
                    if (File.Exists(sourceMdxPathCandidate))
                    {
                        sourceMdxPath = sourceMdxPathCandidate;
                        outputMdxPath = Path.Combine(modelsPath, $"{targetResref}.mdx");
                    }
                    else
                    {
                        System.Console.WriteLine($"Warning: MDX file not found for {Path.GetFileName(filePath)}. MDX files contain geometry data and are typically required.");
                    }
                }
                else if (isMdx)
                {
                    // User selected MDX file
                    sourceMdxPath = filePath;
                    outputMdxPath = Path.Combine(modelsPath, $"{targetResref}.mdx");

                    // Look for corresponding MDL file in the same directory
                    string sourceMdlPathCandidate = Path.ChangeExtension(filePath, ".mdl");
                    if (File.Exists(sourceMdlPathCandidate))
                    {
                        sourceMdlPath = sourceMdlPathCandidate;
                        outputMdlPath = Path.Combine(modelsPath, $"{targetResref}.mdl");
                    }
                    else
                    {
                        System.Console.WriteLine($"Warning: MDL file not found for {Path.GetFileName(filePath)}. MDL files contain model structure and are required.");
                        // We can still copy the MDX, but it won't be usable without an MDL
                    }
                }

                // Validate MDL format if we have an MDL file
                if (!string.IsNullOrEmpty(sourceMdlPath))
                {
                    try
                    {
                        ResourceType detectedFormat = MDLAuto.DetectMdl(sourceMdlPath);
                        if (detectedFormat != ResourceType.MDL && detectedFormat != ResourceType.MDL_ASCII)
                        {
                            System.Console.WriteLine($"Warning: Could not detect valid MDL format for {Path.GetFileName(sourceMdlPath)}. File may be corrupted.");
                            // Continue anyway - the user might know what they're doing
                        }
                        else
                        {
                            System.Console.WriteLine($"Detected MDL format: {detectedFormat}");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Warning: Failed to validate MDL file {Path.GetFileName(sourceMdlPath)}: {ex}");
                        // Continue anyway - file might still be valid
                    }
                }

                // Copy MDL file
                if (!string.IsNullOrEmpty(sourceMdlPath) && !string.IsNullOrEmpty(outputMdlPath))
                {
                    try
                    {
                        if (File.Exists(outputMdlPath))
                        {
                            System.Console.WriteLine($"Warning: Model {targetResref}.mdl already exists in override directory. It will be overwritten.");
                        }

                        File.Copy(sourceMdlPath, outputMdlPath, overwrite: true);
                        System.Console.WriteLine($"Copied MDL: {targetResref}.mdl -> {outputMdlPath}");

                        // Store the imported model reference
                        _importedModels[targetResref] = outputMdlPath;

                        // Update model browser immediately after import
                        UpdateModelBrowser();
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error: Failed to copy MDL file to {outputMdlPath}: {ex}");
                        return;
                    }
                }

                // Copy MDX file
                if (!string.IsNullOrEmpty(sourceMdxPath) && !string.IsNullOrEmpty(outputMdxPath))
                {
                    try
                    {
                        if (File.Exists(outputMdxPath))
                        {
                            System.Console.WriteLine($"Warning: Model geometry {targetResref}.mdx already exists in override directory. It will be overwritten.");
                        }

                        File.Copy(sourceMdxPath, outputMdxPath, overwrite: true);
                        System.Console.WriteLine($"Copied MDX: {targetResref}.mdx -> {outputMdxPath}");
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Error: Failed to copy MDX file to {outputMdxPath}: {ex}");
                        // Don't return - MDL was copied successfully, MDX is supplementary
                    }
                }

                // Optionally add a room entry to the LYT using the imported model
                // This matches PyKotor behavior where importing a model makes it available for use
                // The user can then add it as a room manually or we could prompt them
                if (!string.IsNullOrEmpty(sourceMdlPath) && _lyt != null)
                {
                    // Check if a room with this model already exists
                    bool modelExists = false;
                    foreach (var room in _lyt.Rooms)
                    {
                        if (room.Model == new ResRef(targetResref))
                        {
                            modelExists = true;
                            break;
                        }
                    }

                    if (!modelExists)
                    {
                        // Add a new room entry with the imported model at origin
                        // User can reposition it later in the editor
                        var newRoom = new LYTRoom(new ResRef(targetResref), new Vector3(0, 0, 0));
                        _lyt.Rooms.Add(newRoom);
                        System.Console.WriteLine($"Added room entry for imported model: {targetResref} at position (0, 0, 0)");
                        UpdateScene();
                    }
                }

                System.Console.WriteLine($"Successfully imported model: {targetResref}");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error importing model file {filePath}: {ex}");
            }
        }

        // Update model browser with imported models (similar to UpdateTextureBrowser)
        public void UpdateModelBrowser()
        {
            // Ensure imported models list is maintained and valid
            if (_importedModels == null)
            {
                _importedModels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            // Remove invalid entries (models that no longer exist on disk)
            var validModels = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kvp in _importedModels)
            {
                if (string.IsNullOrEmpty(kvp.Value) || File.Exists(kvp.Value))
                {
                    validModels[kvp.Key] = kvp.Value;
                }
                else
                {
                    System.Console.WriteLine($"Warning: Imported model file no longer exists: {kvp.Value}, removing from list.");
                }
            }
            _importedModels = validModels;

            // Update model browser widget if available
            if (_modelBrowser != null)
            {
                _modelBrowser.UpdateModels(_importedModels);
            }
            else
            {
                // Initialize model browser if not already initialized
                InitializeModelBrowser();
                if (_modelBrowser != null)
                {
                    _modelBrowser.UpdateModels(_importedModels);
                }
            }

            // Log current state for debugging
            System.Console.WriteLine($"Model browser updated. {_importedModels.Count} model(s) available.");
            foreach (var kvp in _importedModels)
            {
                System.Console.WriteLine($"  - {kvp.Key}: {kvp.Value}");
            }
        }

        public List<string> GetImportedModels()
        {
            return new List<string>(_importedModels.Keys);
        }

        public string GetImportedModelPath(string modelName)
        {
            return _importedModels.TryGetValue(modelName, out string path) ? path : null;
        }

        /// <summary>
        /// Gets the model browser widget (for UI integration and testing).
        /// </summary>
        public ModelBrowser ModelBrowser
        {
            get { return _modelBrowser; }
        }

        /// <summary>
        /// Gets the texture browser widget (for UI integration and testing).
        /// </summary>
        public TextureBrowser TextureBrowser
        {
            get { return _textureBrowser; }
        }

        public void UpdateTextureBrowser()
        {
            // Ensure imported textures list is maintained and valid
            if (_importedTextures == null)
            {
                _importedTextures = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }

            // Remove invalid entries (textures that no longer exist on disk)
            var validTextures = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var kvp in _importedTextures)
            {
                if (string.IsNullOrEmpty(kvp.Value) || File.Exists(kvp.Value))
                {
                    validTextures[kvp.Key] = kvp.Value;
                }
                else
                {
                    System.Console.WriteLine($"Warning: Imported texture file no longer exists: {kvp.Value}, removing from list.");
                }
            }
            _importedTextures = validTextures;

            // Update texture browser widget if available
            if (_textureBrowser != null)
            {
                _textureBrowser.UpdateTextures(_importedTextures);
            }
            else
            {
                // Initialize texture browser if not already initialized
                InitializeTextureBrowser();
                if (_textureBrowser != null)
                {
                    _textureBrowser.UpdateTextures(_importedTextures);
                }
            }

            // Log current state for debugging
            System.Console.WriteLine($"Texture browser updated. {_importedTextures.Count} texture(s) available.");
            foreach (var kvp in _importedTextures)
            {
                System.Console.WriteLine($"  - {kvp.Key}: {kvp.Value}");
            }
        }

        public List<string> GetImportedTextures()
        {
            return new List<string>(_importedTextures.Keys);
        }

        public string GetImportedTexturePath(string textureName)
        {
            return _importedTextures.TryGetValue(textureName, out string path) ? path : null;
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            SetBlenderStatus(string.Empty);
            RefreshBlenderActionState();

            try
            {
                _lyt = LYTAuto.ReadLyt(data);
                LoadLYT(_lyt);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load LYT: {ex}");
                New();
            }
        }

        private void LoadLYT(BioWare.Resource.Formats.LYT.LYT lyt)
        {
            _lyt = lyt;
            UpdateScene();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            byte[] data = LYTAuto.BytesLyt(_lyt);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _lyt = new BioWare.Resource.Formats.LYT.LYT();
            SetBlenderStatus(string.Empty);
            RefreshBlenderActionState();
            UpdateScene();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        protected override async Task RunSaveAsAsync()
        {
            var storage = StorageProvider;
            if (storage == null) return;
            string suggestedName = !string.IsNullOrEmpty(_resname) ? _resname : "layout";
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".lyt",
                FileTypeChoices = new[] { new FilePickerFileType("Layout (LYT)") { Patterns = new[] { "*.lyt" } }, new FilePickerFileType("All files") { Patterns = new[] { "*.*" } } }
            };
            var file = await storage.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            string ext = (Path.GetExtension(path) ?? "").TrimStart('.').ToLowerInvariant();
            _restype = ResourceType.FromExtension(ext) ?? ResourceType.LYT;
            _resname = Path.GetFileNameWithoutExtension(path);
            RefreshWindowTitle();
            SetBlenderStatus(string.Empty);
            RefreshBlenderActionState();
            Save();
        }
    }
}
