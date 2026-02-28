using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource.Formats.BWM;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.LYT;
using OdyTools.Data;
using KotorColor = BioWare.Common.Color;
using PTH = BioWare.Resource.Formats.GFF.Generics.PTH;
using Window = Avalonia.Controls.Window;

namespace OdyTools.Editors
{
    public class PTHRenderArea : Control
    {
        private PTH _pth;
        private List<BWM> _walkmeshes;
        private Dictionary<SurfaceMaterial, Avalonia.Media.Color> _materialColors;
        private Vector2 _mousePosition;
        private bool _isMouseDown;
        private Vector2 _lastMousePosition;

        public RenderCamera Camera { get; private set; }
        public PathSelection PathSelection { get; private set; }

        // Signal events for proper Avalonia event handling
        public event EventHandler<PointerPressedEventArgs> SigMousePressed;
        public event EventHandler<PointerEventArgs> SigMouseMoved;
        public event EventHandler<PointerWheelEventArgs> SigMouseScrolled;
        public event EventHandler<PointerReleasedEventArgs> SigMouseReleased;
        public event EventHandler<KeyEventArgs> SigKeyPressed;

        public PTHRenderArea()
        {
            _pth = new PTH();
            _walkmeshes = new List<BWM>();
            _materialColors = new Dictionary<SurfaceMaterial, Avalonia.Media.Color>();
            _mousePosition = Vector2.Zero;
            _isMouseDown = false;
            _lastMousePosition = Vector2.Zero;
            Camera = new RenderCamera();
            PathSelection = new PathSelection();

            // Set up Avalonia control properties
            Focusable = true;

            // Set up event handlers
            PointerPressed += OnPointerPressed;
            PointerMoved += OnPointerMoved;
            PointerWheelChanged += OnPointerWheelChanged;
            PointerReleased += OnPointerReleased;
            KeyDown += OnKeyDown;
        }

        // Convert screen coordinates to world coordinates (exposed for status bar / context menu)
        public Vector2 ScreenToWorld(Point screenPoint)
        {
            var centerX = Bounds.Width / 2.0;
            var centerY = Bounds.Height / 2.0;

            // Apply camera transformations in reverse
            var worldX = (screenPoint.X - centerX) / Camera.Zoom + Camera.Position.X;
            var worldY = (screenPoint.Y - centerY) / Camera.Zoom + Camera.Position.Y;

            return new Vector2((float)worldX, (float)worldY);
        }

        // Convert world coordinates to screen coordinates
        private Point WorldToScreen(Vector2 worldPoint)
        {
            var centerX = Bounds.Width / 2.0;
            var centerY = Bounds.Height / 2.0;

            // Apply camera transformations
            var screenX = (worldPoint.X - Camera.Position.X) * Camera.Zoom + centerX;
            var screenY = (worldPoint.Y - Camera.Position.Y) * Camera.Zoom + centerY;

            return new Point(screenX, screenY);
        }

        // Event handlers
        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            var point = e.GetCurrentPoint(this);
            var worldPos = ScreenToWorld(point.Position);
            _mousePosition = worldPos;
            _isMouseDown = true;
            _lastMousePosition = worldPos;

            // Handle selection
            var nodesUnderMouse = PathNodesUnderMouse();
            if (nodesUnderMouse.Count > 0)
            {
                // Select the first node under mouse
                PathSelection.Select(new[] { nodesUnderMouse[0] });
            }
            else
            {
                // Clear selection if clicking empty space
                PathSelection.Clear();
            }

            InvalidateVisual();

            // Raise signal event
            SigMousePressed?.Invoke(this, e);
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            var point = e.GetCurrentPoint(this);
            var worldPos = ScreenToWorld(point.Position);
            _mousePosition = worldPos;

            // Handle dragging
            if (_isMouseDown)
            {
                var delta = worldPos - _lastMousePosition;
                _lastMousePosition = worldPos;

                // If we have selected nodes, move them
                var selected = PathSelection.All();
                if (selected.Count > 0)
                {
                    MoveSelected(delta.X, delta.Y);
                }
                else
                {
                    // Pan camera
                    Camera.NudgePosition(-delta.X, -delta.Y);
                }

                InvalidateVisual();
            }

            // Raise signal event
            SigMouseMoved?.Invoke(this, e);
        }

        private void OnPointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            var zoomFactor = e.Delta.Y > 0 ? 1.1f : 0.9f;
            Camera.NudgeZoom(zoomFactor);
            InvalidateVisual();

            // Raise signal event
            SigMouseScrolled?.Invoke(this, e);
        }

        private void OnPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            _isMouseDown = false;

            // Raise signal event
            SigMouseReleased?.Invoke(this, e);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // Raise signal event
            SigKeyPressed?.Invoke(this, e);
        }

        /// <summary>Set material colors for walkmesh rendering.</summary>
        public void SetMaterialColors(Dictionary<SurfaceMaterial, Avalonia.Media.Color> colors)
        {
            _materialColors = colors ?? new Dictionary<SurfaceMaterial, Avalonia.Media.Color>();
            InvalidateVisual();
        }

        // Render walkmesh (if loaded), then path connections and nodes
        public override void Render(DrawingContext context)
        {
            base.Render(context);

            // Draw background
            var backgroundBrush = new SolidColorBrush(Avalonia.Media.Color.FromArgb(255, 0, 0, 0));
            context.FillRectangle(backgroundBrush, new Rect(0, 0, Bounds.Width, Bounds.Height));

            // Draw walkmesh triangles first (behind path)
            DrawWalkmeshes(context);

            if (_pth == null || _pth.Count == 0)
            {
                return;
            }

            // Draw connections first (behind nodes)
            DrawConnections(context);

            // Draw nodes
            DrawNodes(context);

            // Draw selection highlights
            DrawSelectionHighlights(context);
        }

        private void DrawWalkmeshes(DrawingContext context)
        {
            if (_walkmeshes == null || _walkmeshes.Count == 0) return;
            var defaultColor = Avalonia.Media.Color.FromArgb(80, 60, 60, 60);
            foreach (var bwm in _walkmeshes)
            {
                if (bwm?.Faces == null) continue;
                foreach (var face in bwm.Faces)
                {
                    var v1 = WorldToScreen(new Vector2(face.V1.X, face.V1.Y));
                    var v2 = WorldToScreen(new Vector2(face.V2.X, face.V2.Y));
                    var v3 = WorldToScreen(new Vector2(face.V3.X, face.V3.Y));
                    var color = _materialColors != null && _materialColors.TryGetValue(face.Material, out var c)
                        ? new Avalonia.Media.Color((byte)(0.35 * 255), c.R, c.G, c.B)
                        : defaultColor;
                    var brush = new SolidColorBrush(color);
                    var geometry = new StreamGeometry();
                    using (var sgc = geometry.Open())
                    {
                        sgc.BeginFigure(new Point(v1.X, v1.Y), true);
                        sgc.LineTo(new Point(v2.X, v2.Y));
                        sgc.LineTo(new Point(v3.X, v3.Y));
                        sgc.EndFigure(true);
                    }
                    context.DrawGeometry(brush, null, geometry);
                }
            }
        }

        private void DrawConnections(DrawingContext context)
        {
            foreach (var connection in _pth.GetConnections())
            {
                var startPoint = _pth.GetPoint(connection.SourceIndex);
                var endPoint = _pth.GetPoint(connection.TargetIndex);

                var startScreen = WorldToScreen(startPoint);
                var endScreen = WorldToScreen(endPoint);

                // Create connection line
                var pen = new Pen(Brushes.Gray, 2.0);
                context.DrawLine(pen, startScreen, endScreen);
            }
        }

        private void DrawNodes(DrawingContext context)
        {
            const float nodeRadius = 8.0f;

            foreach (var point in _pth.GetPoints())
            {
                var screenPos = WorldToScreen(point);

                // Draw node circle
                var ellipseGeometry = new EllipseGeometry(new Rect(
                    screenPos.X - nodeRadius,
                    screenPos.Y - nodeRadius,
                    nodeRadius * 2,
                    nodeRadius * 2));

                // Use default color for nodes (can be extended to use material colors)
                var brush = Brushes.LightBlue;
                context.DrawGeometry(brush, new Pen(Brushes.DarkBlue, 1.0), ellipseGeometry);
            }
        }

        private void DrawSelectionHighlights(DrawingContext context)
        {
            var selectedNodes = PathSelection.All();
            if (selectedNodes.Count == 0)
            {
                return;
            }

            const float highlightRadius = 12.0f;

            foreach (var selectedPoint in selectedNodes)
            {
                var screenPos = WorldToScreen(selectedPoint);

                // Draw selection highlight
                var ellipseGeometry = new EllipseGeometry(new Rect(
                    screenPos.X - highlightRadius,
                    screenPos.Y - highlightRadius,
                    highlightRadius * 2,
                    highlightRadius * 2));

                var brush = Brushes.Yellow;
                var pen = new Pen(Brushes.Orange, 2.0);
                context.DrawGeometry(brush, pen, ellipseGeometry);
            }
        }

        // Move selected nodes by the specified delta
        private void MoveSelected(float deltaX, float deltaY)
        {
            var selected = PathSelection.All();
            if (selected.Count == 0)
            {
                return;
            }

            for (int i = 0; i < selected.Count; i++)
            {
                var point = selected[i];
                var index = _pth.Find(point);
                if (index.HasValue)
                {
                    var updated = new Vector2(point.X + deltaX, point.Y + deltaY);
                    _pth.SetPoint(index.Value, updated);
                    selected[i] = updated;
                }
            }

            PathSelection.Select(selected);
        }

        public void SetPth(PTH pth)
        {
            _pth = pth ?? new PTH();
        }

        public void SetMousePosition(Vector2 position)
        {
            _mousePosition = position;
        }

        public void CenterCamera()
        {
            if (_pth.Count == 0)
            {
                Camera.SetPosition(Vector2.Zero);
                return;
            }

            float sumX = 0f;
            float sumY = 0f;
            foreach (var point in _pth.GetPoints())
            {
                sumX += point.X;
                sumY += point.Y;
            }

            Camera.SetPosition(new Vector2(sumX / _pth.Count, sumY / _pth.Count));
        }

        public List<Vector2> PathNodesUnderMouse(float tolerance = 0.5f)
        {
            var hits = new List<Vector2>();
            foreach (var point in _pth.GetPoints())
            {
                var dx = point.X - _mousePosition.X;
                var dy = point.Y - _mousePosition.Y;
                if ((dx * dx) + (dy * dy) <= tolerance * tolerance)
                {
                    hits.Add(point);
                }
            }

            return hits;
        }

        public void SetWalkmeshes(List<BWM> walkmeshes)
        {
            _walkmeshes = walkmeshes ?? new List<BWM>();
            InvalidateVisual();
        }
    }

    public class RenderCamera
    {
        public Vector2 Position { get; private set; }
        public float Zoom { get; private set; }
        public float Rotation { get; private set; }

        public RenderCamera()
        {
            Position = Vector2.Zero;
            Zoom = 1.0f;
            Rotation = 0.0f;
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public void NudgePosition(float x, float y)
        {
            Position = new Vector2(Position.X + x, Position.Y + y);
        }

        public void NudgeZoom(float amount)
        {
            Zoom = amount <= 0 ? Zoom : Zoom * amount;
        }

        public void NudgeRotation(float angle)
        {
            Rotation += angle;
        }
    }

    public class PathSelection
    {
        private readonly List<Vector2> _selected = new List<Vector2>();

        public void Select(IEnumerable<Vector2> points)
        {
            _selected.Clear();
            if (points == null)
            {
                return;
            }

            foreach (var point in points)
            {
                if (!_selected.Contains(point))
                {
                    _selected.Add(point);
                }
            }
        }

        public void Clear()
        {
            _selected.Clear();
        }

        public List<Vector2> All()
        {
            return new List<Vector2>(_selected);
        }

        public Vector2? Last()
        {
            if (_selected.Count == 0)
            {
                return null;
            }

            return _selected[_selected.Count - 1];
        }
    }

    public partial class OdyToolPTH : Editor
    {
        private PTH _pth;
        private GITSettings _settings;
        private PTHControlScheme _controls;

        // Status bar labels
        public TextBlock LeftLabel { get; private set; }
        public TextBlock CenterLabel { get; private set; }
        public TextBlock RightLabel { get; private set; }

        // Status output handler
        public PTHStatusOut StatusOut { get; private set; }

        // Control scheme - exposed for testing
        public PTHControlScheme Controls => _controls;

        // Material colors dictionary - exposed for testing
        public Dictionary<SurfaceMaterial, Avalonia.Media.Color> MaterialColors { get; private set; }

        public PTHRenderArea RenderArea { get; private set; }

        // XAML-backed controls (optional when AXAML loads)
        private Avalonia.Controls.ListBox _nodeList;
        private TextBox _nodeIndexBox;
        private NumericUpDown _nodePosX;
        private NumericUpDown _nodePosY;
        private Button _addNodeHereButton;
        private Button _removeNodeButton;
        private Button _addEdgeButton;
        private Button _removeEdgeButton;
        private bool _syncingSelection;

        public OdyToolPTH(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolPTH", "pth",
                new[] { ResourceType.PTH },
                new[] { ResourceType.PTH },
                installation)
        {
            _pth = new PTH();
            _settings = new GITSettings();
            _controls = new PTHControlScheme(this);

            InitializeMaterialColors();

            InitializeComponent();
            SetupStatusBar();
            StatusOut = new PTHStatusOut(this);
            // RenderArea: from XAML (pthRenderArea) or created in code when no XAML
            if (RenderArea == null)
            {
                RenderArea = new PTHRenderArea();
                if (Content is Panel panel)
                {
                    panel.Children.Add(RenderArea);
                }
            }
            RenderArea.SetPth(_pth);
            RenderArea.SetMaterialColors(MaterialColors);
            SetupUI();
            AddHelpAction("GFF-PTH.md");
            New();
        }

        private void InitializeMaterialColors()
        {
            // Helper to convert integer color to Avalonia Color
            Avalonia.Media.Color IntColorToAvaloniaColor(int numColor)
            {
                var kotorColor = KotorColor.FromRgbaInteger(numColor);
                return new Avalonia.Media.Color(
                    (byte)(kotorColor.A * 255),
                    (byte)(kotorColor.R * 255),
                    (byte)(kotorColor.G * 255),
                    (byte)(kotorColor.B * 255)
                );
            }

            MaterialColors = new Dictionary<SurfaceMaterial, Avalonia.Media.Color>
            {
                { SurfaceMaterial.Undefined, IntColorToAvaloniaColor(_settings.UndefinedMaterialColour) },
                { SurfaceMaterial.Obscuring, IntColorToAvaloniaColor(_settings.ObscuringMaterialColour) },
                { SurfaceMaterial.Dirt, IntColorToAvaloniaColor(_settings.DirtMaterialColour) },
                { SurfaceMaterial.Grass, IntColorToAvaloniaColor(_settings.GrassMaterialColour) },
                { SurfaceMaterial.Stone, IntColorToAvaloniaColor(_settings.StoneMaterialColour) },
                { SurfaceMaterial.Wood, IntColorToAvaloniaColor(_settings.WoodMaterialColour) },
                { SurfaceMaterial.Water, IntColorToAvaloniaColor(_settings.WaterMaterialColour) },
                { SurfaceMaterial.NonWalk, IntColorToAvaloniaColor(_settings.NonWalkMaterialColour) },
                { SurfaceMaterial.Transparent, IntColorToAvaloniaColor(_settings.TransparentMaterialColour) },
                { SurfaceMaterial.Carpet, IntColorToAvaloniaColor(_settings.CarpetMaterialColour) },
                { SurfaceMaterial.Metal, IntColorToAvaloniaColor(_settings.MetalMaterialColour) },
                { SurfaceMaterial.Puddles, IntColorToAvaloniaColor(_settings.PuddlesMaterialColour) },
                { SurfaceMaterial.Swamp, IntColorToAvaloniaColor(_settings.SwampMaterialColour) },
                { SurfaceMaterial.Mud, IntColorToAvaloniaColor(_settings.MudMaterialColour) },
                { SurfaceMaterial.Leaves, IntColorToAvaloniaColor(_settings.LeavesMaterialColour) },
                { SurfaceMaterial.Lava, IntColorToAvaloniaColor(_settings.LavaMaterialColour) },
                { SurfaceMaterial.BottomlessPit, IntColorToAvaloniaColor(_settings.BottomlessPitMaterialColour) },
                { SurfaceMaterial.DeepWater, IntColorToAvaloniaColor(_settings.DeepWaterMaterialColour) },
                { SurfaceMaterial.Door, IntColorToAvaloniaColor(_settings.DoorMaterialColour) },
                { SurfaceMaterial.NonWalkGrass, IntColorToAvaloniaColor(_settings.NonWalkGrassMaterialColour) },
                { SurfaceMaterial.Trigger, IntColorToAvaloniaColor(_settings.NonWalkGrassMaterialColour) }
            };
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
                xamlLoaded = true;
                RenderArea = this.FindControl<PTHRenderArea>("pthRenderArea");
                LeftLabel = this.FindControl<TextBlock>("leftLabel");
                CenterLabel = this.FindControl<TextBlock>("centerLabel");
                RightLabel = this.FindControl<TextBlock>("rightLabel");
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
            var panel = new StackPanel();
            Content = panel;
        }

        private void SetupUI()
        {
            _nodeList = this.FindControl<Avalonia.Controls.ListBox>("nodeList");
            _nodeIndexBox = this.FindControl<TextBox>("nodeIndexBox");
            _nodePosX = this.FindControl<NumericUpDown>("nodePosX");
            _nodePosY = this.FindControl<NumericUpDown>("nodePosY");
            _addNodeHereButton = this.FindControl<Button>("addNodeHereButton");
            _removeNodeButton = this.FindControl<Button>("removeNodeButton");
            _addEdgeButton = this.FindControl<Button>("addEdgeButton");
            _removeEdgeButton = this.FindControl<Button>("removeEdgeButton");

            if (RenderArea != null)
            {
                RenderArea.SigMouseMoved += OnRenderAreaMouseMoved;
                RenderArea.SigMousePressed += OnRenderAreaMousePressed;
                RenderArea.SigKeyPressed += OnRenderAreaKeyPressed;
                RenderArea.SigMouseScrolled += OnRenderAreaMouseScrolled;
                SetupContextMenu();
            }

            if (_nodeList != null)
            {
                _nodeList.SelectionMode = Avalonia.Controls.SelectionMode.Multiple;
                _nodeList.SelectionChanged += OnNodeListSelectionChanged;
            }
            if (_nodePosX != null) _nodePosX.ValueChanged += (s, e) => ApplyNodePositionFromSpinners();
            if (_nodePosY != null) _nodePosY.ValueChanged += (s, e) => ApplyNodePositionFromSpinners();
            if (_addNodeHereButton != null) _addNodeHereButton.Click += (s, e) => AddNodeAtSpinnerPosition();
            if (_removeNodeButton != null) _removeNodeButton.Click += (s, e) => RemoveSelectedNodeFromList();
            if (_addEdgeButton != null) _addEdgeButton.Click += (s, e) => AddEdgeBetweenSelected();
            if (_removeEdgeButton != null) _removeEdgeButton.Click += (s, e) => RemoveEdgeBetweenSelected();

            SetupMenuHandlers();
            RebuildNodeList();
            UpdateNodeButtonsState();
        }

        private void OnRenderAreaMouseMoved(object sender, PointerEventArgs e)
        {
            if (RenderArea == null) return;
            var screenPoint = e.GetCurrentPoint(RenderArea).Position;
            var world = RenderArea.ScreenToWorld(screenPoint);
            UpdateMousePosition(world.X, world.Y);
        }

        private void OnRenderAreaMousePressed(object sender, PointerPressedEventArgs e)
        {
            if (RenderArea == null) return;
            var point = e.GetCurrentPoint(RenderArea);
            if (point.Properties.IsRightButtonPressed)
            {
                var screenPoint = point.Position;
                var world = RenderArea.ScreenToWorld(screenPoint);
                ShowContextMenu(world, screenPoint);
            }
            else
            {
                SyncSelectionFromCanvasToTable();
            }
        }

        private void OnRenderAreaKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                DeleteSelectedNode();
                e.Handled = true;
            }
        }

        private void OnRenderAreaMouseScrolled(object sender, PointerWheelEventArgs e)
        {
            if (RenderArea == null) return;
            float zoomFactor = e.Delta.Y > 0 ? 1.1f : 0.9f;
            ZoomCamera(zoomFactor);
        }

        private void SetupContextMenu()
        {
            if (RenderArea == null) return;
            var menu = new Avalonia.Controls.ContextMenu();
            var addNode = new MenuItem { Header = "Add Node" };
            addNode.Click += (s, e) => AddNodeAtLastContextWorld();
            menu.Items.Add(addNode);
            var copyXY = new MenuItem { Header = "Copy XY coords" };
            copyXY.Click += (s, e) => CopyXYToClipboard();
            menu.Items.Add(copyXY);
            var removeNode = new MenuItem { Header = "Remove Node" };
            removeNode.Click += (s, e) => RemoveNodeUnderContextMenu();
            menu.Items.Add(removeNode);
            menu.Items.Add(new Separator());
            var addEdge = new MenuItem { Header = "Add Edge" };
            addEdge.Click += (s, e) => AddEdgeBetweenSelected();
            menu.Items.Add(addEdge);
            var removeEdge = new MenuItem { Header = "Remove Edge" };
            removeEdge.Click += (s, e) => RemoveEdgeBetweenSelected();
            menu.Items.Add(removeEdge);
            RenderArea.ContextMenu = menu;
        }

        private Vector2 _contextMenuWorld;
        private void ShowContextMenu(Vector2 world, Point screenPoint)
        {
            _contextMenuWorld = world;
            if (RenderArea?.ContextMenu != null)
            {
                RenderArea.ContextMenu.Open(RenderArea);
            }
        }

        private void AddNodeAtLastContextWorld()
        {
            AddNode(_contextMenuWorld.X, _contextMenuWorld.Y);
            RebuildNodeList();
            RenderArea?.InvalidateVisual();
            MarkDirty();
        }

        private void RemoveNodeUnderContextMenu()
        {
            var under = PointsUnderMouse(0.5f);
            if (under.Count > 0)
            {
                var idx = _pth.Find(under[0]);
                if (idx.HasValue)
                {
                    RemoveNode(idx.Value);
                    RebuildNodeList();
                    UpdateNodeButtonsState();
                    MarkDirty();
                }
            }
        }

        private void CopyXYToClipboard()
        {
            var text = $"{_contextMenuWorld.X}, {_contextMenuWorld.Y}";
            if (TopLevel.GetTopLevel(this)?.Clipboard != null)
            {
                TopLevel.GetTopLevel(this).Clipboard.SetTextAsync(text);
            }
            if (StatusOut != null) StatusOut.Write($"Copy XY: {text}");
        }

        private void SetupMenuHandlers()
        {
            void Bind(string name, Action handler)
            {
                var item = EditorHelpers.FindControlSafe<MenuItem>(this, name);
                if (item != null) item.Click += (s, e) => handler();
            }
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
            Bind("actionAddNode", () => AddNodeAtLastContextWorld());
            Bind("actionRemoveNode", () => DeleteSelectedNode());
            Bind("actionAddEdge", () => AddEdgeBetweenSelected());
            Bind("actionRemoveEdge", () => RemoveEdgeBetweenSelected());
            Bind("actionCopyXY", () => CopyXYToClipboard());
            Bind("actionCenterSelection", () => MoveCameraToSelection());
            Bind("actionZoomIn", () => ZoomCamera(1.25f));
            Bind("actionZoomOut", () => ZoomCamera(0.8f));
            Bind("actionZoomReset", () => { if (RenderArea != null) RenderArea.CenterCamera(); RenderArea?.InvalidateVisual(); });
        }

        private void RebuildNodeList()
        {
            if (_nodeList == null) return;
            var items = new List<string>();
            for (int i = 0; i < _pth.Count; i++)
            {
                var pt = _pth.GetPoint(i);
                items.Add($"{i}: ({pt.X:F2}, {pt.Y:F2})");
            }
            _syncingSelection = true;
            _nodeList.ItemsSource = items;
            SyncSelectionFromCanvasToTable();
            _syncingSelection = false;
            UpdateNodeButtonsState();
        }

        private void OnNodeListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_syncingSelection || _nodeList == null) return;
            var indices = GetSelectedNodeIndices();
            var points = new List<Vector2>();
            foreach (var idx in indices)
            {
                if (idx >= 0 && idx < _pth.Count)
                    points.Add(_pth.GetPoint(idx));
            }
            RenderArea?.PathSelection.Select(points);
            RenderArea?.InvalidateVisual();
            UpdateDetailFromSelection();
            UpdateNodeButtonsState();
        }

        private List<int> GetSelectedNodeIndices()
        {
            if (_nodeList == null) return new List<int>();
            var items = _nodeList.ItemsSource as IList<string>;
            if (items == null) return new List<int>();
            var selected = _nodeList.SelectedItems?.Cast<string>().ToList() ?? new List<string>();
            var indices = new List<int>();
            foreach (var s in selected)
            {
                int i = items.IndexOf(s);
                if (i >= 0 && i < _pth.Count) indices.Add(i);
            }
            return indices;
        }

        private void SetSelectedNodeIndices(List<int> indices)
        {
            if (_nodeList == null || indices == null) return;
            var items = _nodeList.ItemsSource as IList<string>;
            if (items == null) return;
            try
            {
                var sel = _nodeList.Selection;
                if (sel != null)
                {
                    sel.Clear();
                    foreach (var i in indices)
                    {
                        if (i >= 0 && i < items.Count)
                            sel.Select(i);
                    }
                }
                else if (indices.Count > 0 && indices[0] < items.Count)
                {
                    _nodeList.SelectedIndex = indices[0];
                }
            }
            catch
            {
                if (indices.Count > 0 && indices[0] < (items?.Count ?? 0))
                    _nodeList.SelectedIndex = indices[0];
            }
        }

        private void SyncSelectionFromCanvasToTable()
        {
            if (_nodeList == null || RenderArea == null) return;
            var selected = RenderArea.PathSelection.All();
            var indices = new List<int>();
            foreach (var pt in selected)
            {
                var idx = _pth.Find(pt);
                if (idx.HasValue) indices.Add(idx.Value);
            }
            _syncingSelection = true;
            SetSelectedNodeIndices(indices);
            _syncingSelection = false;
            UpdateDetailFromSelection();
        }

        private void UpdateDetailFromSelection()
        {
            var selected = RenderArea?.PathSelection.All() ?? new List<Vector2>();
            if (selected.Count == 0)
            {
                if (_nodeIndexBox != null) _nodeIndexBox.Text = "";
                if (_nodePosX != null) _nodePosX.Value = 0;
                if (_nodePosY != null) _nodePosY.Value = 0;
                return;
            }
            var first = selected[0];
            var idx = _pth.Find(first);
            if (idx.HasValue)
            {
                if (_nodeIndexBox != null) _nodeIndexBox.Text = idx.Value.ToString();
                if (_nodePosX != null) _nodePosX.Value = (decimal)first.X;
                if (_nodePosY != null) _nodePosY.Value = (decimal)first.Y;
            }
        }

        private void ApplyNodePositionFromSpinners()
        {
            if (_nodePosX == null || _nodePosY == null) return;
            var selected = RenderArea?.PathSelection.All() ?? new List<Vector2>();
            if (selected.Count != 1) return;
            var pt = selected[0];
            var idx = _pth.Find(pt);
            if (!idx.HasValue) return;
            _pth.SetPoint(idx.Value, (float)_nodePosX.Value, (float)_nodePosY.Value);
            RenderArea.PathSelection.Select(new[] { new Vector2((float)_nodePosX.Value, (float)_nodePosY.Value) });
            RebuildNodeList();
            MarkDirty();
        }

        private void AddNodeAtSpinnerPosition()
        {
            if (_nodePosX == null || _nodePosY == null) return;
            AddNode((float)_nodePosX.Value, (float)_nodePosY.Value);
            RebuildNodeList();
            RenderArea?.InvalidateVisual();
            MarkDirty();
        }

        private void RemoveSelectedNodeFromList()
        {
            DeleteSelectedNode();
        }

        private void DeleteSelectedNode()
        {
            var selected = RenderArea?.PathSelection.All() ?? new List<Vector2>();
            if (selected.Count == 0) return;
            var idx = _pth.Find(selected[0]);
            if (idx.HasValue)
            {
                RemoveNode(idx.Value);
                RebuildNodeList();
                UpdateNodeButtonsState();
                MarkDirty();
            }
        }

        private void AddEdgeBetweenSelected()
        {
            var indices = GetSelectedNodeIndices();
            if (indices.Count < 2) return;
            int a = indices[0], b = indices[1];
            if (a < 0 || b < 0 || a >= _pth.Count || b >= _pth.Count) return;
            AddEdge(a, b);
            RenderArea?.InvalidateVisual();
            UpdateNodeButtonsState();
            MarkDirty();
        }

        private void RemoveEdgeBetweenSelected()
        {
            var indices = GetSelectedNodeIndices();
            if (indices.Count < 2) return;
            int a = indices[0], b = indices[1];
            if (a < 0 || b < 0 || a >= _pth.Count || b >= _pth.Count) return;
            RemoveEdge(a, b);
            RenderArea?.InvalidateVisual();
            MarkDirty();
        }

        private void UpdateNodeButtonsState()
        {
            var selCount = GetSelectedNodeIndices().Count;
            if (_removeNodeButton != null) _removeNodeButton.IsEnabled = selCount >= 1;
            if (_addEdgeButton != null) _addEdgeButton.IsEnabled = selCount >= 2;
            if (_removeEdgeButton != null) _removeEdgeButton.IsEnabled = selCount >= 2;
        }

        private void SetupStatusBar()
        {
            if (LeftLabel == null) LeftLabel = new TextBlock { Text = "" };
            if (CenterLabel == null) CenterLabel = new TextBlock { Text = "", HorizontalAlignment = HorizontalAlignment.Center };
            if (RightLabel == null) RightLabel = new TextBlock { Text = "" };
        }

        public void UpdateStatusBar(string left = "", string center = "", string right = "")
        {
            if (LeftLabel != null)
            {
                LeftLabel.Text = left ?? "";
            }
            if (CenterLabel != null)
            {
                CenterLabel.Text = center ?? "";
            }
            if (RightLabel != null)
            {
                RightLabel.Text = right ?? "";
            }
        }

        public void AddNode(float x, float y)
        {
            _pth.Add(x, y);
        }

        public void RemoveNode(int index)
        {
            _pth.Remove(index);
            if (RenderArea != null) RenderArea.PathSelection.Clear();
        }

        public void AddEdge(int source, int target)
        {
            if (source < 0 || target < 0 || source >= _pth.Count || target >= _pth.Count)
            {
                return;
            }

            // Create bidirectional connections like other path editors
            _pth.Connect(source, target);
            _pth.Connect(target, source);
        }

        public void RemoveEdge(int source, int target)
        {
            if (source < 0 || target < 0 || source >= _pth.Count || target >= _pth.Count)
            {
                return;
            }

            // Remove bidirectional connections like other path editors
            _pth.Disconnect(source, target);
            _pth.Disconnect(target, source);
        }

        /// <summary>
        /// Updates the cached mouse position used for hit testing.
        /// </summary>
        /// <param name="x">Mouse X coordinate in world space.</param>
        /// <param name="y">Mouse Y coordinate in world space.</param>
        public void UpdateMousePosition(float x, float y)
        {
            var position = new Vector2(x, y);
            if (StatusOut != null)
            {
                StatusOut.SetMousePosition(position);
            }
            if (RenderArea != null) RenderArea.SetMousePosition(position);
            if (StatusOut != null)
            {
                StatusOut.UpdateStatusBar();
            }
        }

        public List<Vector2> PointsUnderMouse(float tolerance = 0.5f)
        {
            return RenderArea?.PathNodesUnderMouse(tolerance) ?? new List<Vector2>();
        }

        public List<Vector2> SelectedNodes()
        {
            return RenderArea?.PathSelection.All() ?? new List<Vector2>();
        }

        public void MoveCameraToSelection()
        {
            if (RenderArea == null) return;
            var selection = RenderArea.PathSelection.Last();
            if (selection.HasValue)
            {
                RenderArea.Camera.SetPosition(selection.Value);
            }
            else
            {
                RenderArea.CenterCamera();
            }
        }

        public void MoveCamera(float x, float y)
        {
            if (RenderArea != null) RenderArea.Camera.NudgePosition(x, y);
        }

        public void ZoomCamera(float amount)
        {
            if (RenderArea != null) RenderArea.Camera.NudgeZoom(amount);
        }

        public void RotateCamera(float angle)
        {
            if (RenderArea != null) RenderArea.Camera.NudgeRotation(angle);
        }

        public void MoveSelected(float x, float y)
        {
            if (RenderArea == null) return;
            var selected = RenderArea.PathSelection.All();
            if (selected.Count == 0)
            {
                return;
            }

            for (int i = 0; i < selected.Count; i++)
            {
                var point = selected[i];
                var index = _pth.Find(point);
                if (index.HasValue)
                {
                    var updated = new Vector2(x, y);
                    _pth.SetPoint(index.Value, updated);
                    selected[i] = updated;
                }
            }

            RenderArea.PathSelection.Select(selected);
        }

        public void SelectNodeUnderMouse()
        {
            if (RenderArea == null) return;
            var underMouse = PointsUnderMouse();
            if (underMouse.Count > 0)
                RenderArea.PathSelection.Select(new[] { underMouse[0] });
            else
                RenderArea.PathSelection.Clear();
        }

        // k2_win_gog_aspyr_swkotor2.exe: PTH loading requires LYT file for context (room layout and walkmesh information)
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            // Search for corresponding LYT file (same resref, but .lyt extension)
            if (_installation != null)
            {
                SearchLocation[] searchOrder = new[] { SearchLocation.OVERRIDE, SearchLocation.CHITIN, SearchLocation.MODULES };
                ResourceResult lytResult = _installation.Resource(resref, ResourceType.LYT, searchOrder);

                if (lytResult != null)
                {
                    // Load the LYT layout
                    try
                    {
                        BioWare.Resource.Formats.LYT.LYT layout = LYTAuto.ReadLyt(lytResult.Data);
                        LoadLayout(layout);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load LYT layout: {ex}");
                        // Continue with PTH loading even if LYT fails
                    }
                }
                else
                {
                    // LYT file not found - show error message
                    string message = $"OdyToolPTH requires {resref}.lyt in order to load '{resref}.{restype}', but it could not be found.";
                    var errorBox = MessageBoxManager.GetMessageBoxStandard(
                        "Layout not found",
                        message,
                        ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Error);
                    errorBox.ShowAsync();
                    // Continue with PTH loading anyway (user may still want to edit the path)
                }
            }

            // Load the PTH data
            try
            {
                var pth = PTHAuto.ReadPth(data);
                LoadPTH(pth);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load PTH: {ex}");
                New();
            }
        }

        private void LoadPTH(PTH pth)
        {
            _pth = pth;
            RenderArea.SetPth(_pth);
            RenderArea.PathSelection.Clear();
            RenderArea.CenterCamera();
            RebuildNodeList();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            byte[] data = PTHAuto.BytesPth(_pth);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _pth = new PTH();
            RenderArea.SetPth(_pth);
            RenderArea.PathSelection.Clear();
            RenderArea.CenterCamera();
            RebuildNodeList();
        }

        public PTH Pth()
        {
            return _pth;
        }

        // k2_win_gog_aspyr_swkotor2.exe: LoadLayout loads walkmeshes for each room in the layout to provide visual context
        private void LoadLayout(BioWare.Resource.Formats.LYT.LYT layout)
        {
            if (_installation == null || layout == null)
            {
                return;
            }

            List<BWM> walkmeshes = new List<BWM>();
            SearchLocation[] searchOrder = new[] { SearchLocation.OVERRIDE, SearchLocation.CHITIN, SearchLocation.MODULES };

            // For each room in the layout, try to find and load its walkmesh (WOK file)
            foreach (LYTRoom room in layout.Rooms)
            {
                if (room == null || room.Model == null)
                {
                    continue;
                }

                string modelResRef = room.Model.ToString();
                ResourceResult wokResult = _installation.Resource(modelResRef, ResourceType.WOK, searchOrder);

                if (wokResult != null)
                {
                    try
                    {
                        if (StatusOut != null)
                        {
                            StatusOut.Write($"loadLayout BWM Found {wokResult.ResName}.{wokResult.ResType}");
                        }

                        BWM walkmesh = BWMAuto.ReadBwm(wokResult.Data);
                        walkmeshes.Add(walkmesh);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Failed to load walkmesh for room {modelResRef}: {ex}");
                        // Continue with other rooms even if one fails
                    }
                }
            }

            // Set walkmeshes on render area
            RenderArea.SetWalkmeshes(walkmeshes);
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        protected override async System.Threading.Tasks.Task RunSaveAsAsync()
        {
            var storage = (this as Avalonia.Controls.Window)?.StorageProvider;
            if (storage == null) return;
            string suggestedName = !string.IsNullOrEmpty(_resname) ? _resname : "path";
            var options = new Avalonia.Platform.Storage.FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".pth",
                FileTypeChoices = new[] { new Avalonia.Platform.Storage.FilePickerFileType("Path (PTH)") { Patterns = new[] { "*.pth" } }, new Avalonia.Platform.Storage.FilePickerFileType("All files") { Patterns = new[] { "*.*" } } }
            };
            var file = await storage.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            string ext = (System.IO.Path.GetExtension(path) ?? "").TrimStart('.').ToLowerInvariant();
            _restype = ResourceType.FromExtension(ext) ?? ResourceType.PTH;
            _resname = System.IO.Path.GetFileNameWithoutExtension(path);
            RefreshWindowTitle();
            Save();
        }
    }

    public class PTHStatusOut
    {
        private string _prevStatusOut = "";
        private string _prevStatusError = "";
        private Vector2 _mousePos = Vector2.Zero;
        private OdyToolPTH _editor;

        public PTHStatusOut(OdyToolPTH editor)
        {
            _editor = editor;
        }

        public Vector2 MousePosition
        {
            get { return _mousePos; }
        }

        public void Write(string text)
        {
            UpdateStatusBar(stdout: text);
        }

        public void Flush()
        {
            // Required for compatibility
        }

        public void UpdateStatusBar(string stdout = "", string stderr = "")
        {
            // Update stderr if provided
            if (!string.IsNullOrEmpty(stderr))
            {
                _prevStatusError = stderr;
            }

            // If a message is provided, use it as the last stdout
            if (!string.IsNullOrEmpty(stdout))
            {
                _prevStatusOut = stdout;
            }

            // Construct the status text using last known values
            string leftStatus = _mousePos.ToString();
            string centerStatus = _prevStatusOut;
            string rightStatus = _prevStatusError;
            _editor.UpdateStatusBar(leftStatus, centerStatus, rightStatus);
            if (_editor.RenderArea != null)
                _editor.RenderArea.SetMousePosition(_mousePos);
        }

        public void SetMousePosition(Vector2 position)
        {
            _mousePos = position;
        }
    }

    public class PTHControlScheme
    {
        public OdyToolPTH Editor { get; private set; }

        // Control properties for test compatibility
        public object PanCamera { get; private set; }
        public object RotateCamera { get; private set; }
        public object ZoomCamera { get; private set; }
        public object MoveSelected { get; private set; }
        public object SelectUnderneath { get; private set; }
        public object DeleteSelected { get; private set; }

        public PTHControlScheme(OdyToolPTH editor)
        {
            Editor = editor;
            // Initialize control properties (render area may be set later)
            PanCamera = new object();
            RotateCamera = new object();
            ZoomCamera = new object();
            MoveSelected = new object();
            SelectUnderneath = new object();
            DeleteSelected = new object();
        }
    }
}
