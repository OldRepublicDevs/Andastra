using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using BioWare.Common;
using BioWare.Resource.Formats.BWM;
using OdyTools.Data;
using AColor = Avalonia.Media.Color;
using KotorColor = BioWare.Common.Color;

namespace OdyTools.Editors
{
    public class BWMRenderCamera
    {
        public System.Numerics.Vector2 Position { get; private set; }
        public float Zoom { get; private set; } = 32f;

        public void SetPosition(System.Numerics.Vector2 position)
        {
            Position = position;
        }

        public void NudgePosition(float x, float y)
        {
            Position = new System.Numerics.Vector2(Position.X + x, Position.Y + y);
        }

        public void SetZoom(float zoom)
        {
            Zoom = Math.Max(1f, zoom);
        }

        public void NudgeZoom(float amount)
        {
            if (amount > 0)
            {
                SetZoom(Zoom * amount);
            }
        }
    }

    public class BWMRenderArea : Control
    {
        private BWM _bwm;
        private Dictionary<SurfaceMaterial, AColor> _materialColors;
        private BWMFace _highlightedFace;
        private int? _highlightedEdge;
        private Point _lastPointer;

        public bool ShowRoomBoundaries { get; set; } = true;
        public bool ShowGrid { get; set; }
        public SurfaceMaterial SelectedMaterial { get; set; } = SurfaceMaterial.Stone;

        public event EventHandler<BWMFace> FaceMaterialChanged;
        public event EventHandler<string> StatusChanged;

        public BWMRenderArea()
        {
            _materialColors = OdyToolBWM.CreateDefaultMaterialColors();
            Focusable = true;
            PointerPressed += OnPointerPressed;
            PointerMoved += OnPointerMoved;
            PointerWheelChanged += OnPointerWheelChanged;
            KeyDown += OnKeyDown;
        }

        public void SetWalkmesh(BWM bwm)
        {
            _bwm = bwm;
            CenterCamera();
            InvalidateVisual();
        }

        public void SetMaterialColors(Dictionary<SurfaceMaterial, AColor> colors)
        {
            _materialColors = colors ?? OdyToolBWM.CreateDefaultMaterialColors();
            InvalidateVisual();
        }

        internal AColor MaterialColorForTests(SurfaceMaterial material)
        {
            return _materialColors != null && _materialColors.TryGetValue(material, out var color)
                ? color
                : default;
        }

        public void HighlightTransition(BWMFace face, int? edge)
        {
            _highlightedFace = face;
            _highlightedEdge = edge;
            InvalidateVisual();
        }

        public BWMRenderCamera Camera { get; } = new BWMRenderCamera();

        internal string FaceIndexForTests(BWMFace face)
        {
            return FaceIndex(face);
        }

        public void CenterCamera()
        {
            if (_bwm?.Faces == null || _bwm.Faces.Count == 0)
            {
                Camera.SetPosition(new System.Numerics.Vector2(0, 0));
                Camera.SetZoom(32);
                return;
            }

            GetBounds(out var minX, out var minY, out var maxX, out var maxY);
            Camera.SetPosition(new System.Numerics.Vector2((minX + maxX) / 2f, (minY + maxY) / 2f));
            var width = Math.Max(1f, maxX - minX);
            var height = Math.Max(1f, maxY - minY);
            var scaleX = Bounds.Width > 0 ? (Bounds.Width - 48) / width : 32;
            var scaleY = Bounds.Height > 0 ? (Bounds.Height - 48) / height : 32;
            Camera.SetZoom((float)Math.Max(8, Math.Min(96, Math.Min(scaleX, scaleY))));
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            context.FillRectangle(new SolidColorBrush(AColor.FromRgb(18, 22, 28)), new Rect(0, 0, Bounds.Width, Bounds.Height));

            if (ShowGrid)
            {
                DrawGrid(context);
            }

            if (_bwm?.Faces == null || _bwm.Faces.Count == 0)
            {
                return;
            }

            foreach (var face in _bwm.Faces)
            {
                DrawFace(context, face);
            }

            if (_highlightedFace != null && _highlightedEdge.HasValue)
            {
                DrawTransitionHighlight(context, _highlightedFace, _highlightedEdge.Value);
            }
        }

        private void DrawGrid(DrawingContext context)
        {
            var pen = new Pen(new SolidColorBrush(AColor.FromArgb(60, 120, 130, 145)), 1);
            var step = Math.Max(16, Camera.Zoom);
            var startX = Bounds.Width / 2 % step;
            var startY = Bounds.Height / 2 % step;
            for (double x = startX; x < Bounds.Width; x += step)
            {
                context.DrawLine(pen, new Point(x, 0), new Point(x, Bounds.Height));
            }
            for (double y = startY; y < Bounds.Height; y += step)
            {
                context.DrawLine(pen, new Point(0, y), new Point(Bounds.Width, y));
            }
        }

        private void DrawFace(DrawingContext context, BWMFace face)
        {
            var p1 = WorldToScreen(face.V1.X, face.V1.Y);
            var p2 = WorldToScreen(face.V2.X, face.V2.Y);
            var p3 = WorldToScreen(face.V3.X, face.V3.Y);
            var baseColor = _materialColors.TryGetValue(face.Material, out var color) ? color : AColor.FromRgb(95, 105, 115);
            var brush = new SolidColorBrush(AColor.FromArgb(125, baseColor.R, baseColor.G, baseColor.B));
            var outline = new Pen(new SolidColorBrush(AColor.FromArgb(145, 210, 216, 224)), ShowRoomBoundaries ? 1.2 : 0.6);

            var geometry = new StreamGeometry();
            using (var sgc = geometry.Open())
            {
                sgc.BeginFigure(p1, true);
                sgc.LineTo(p2);
                sgc.LineTo(p3);
                sgc.EndFigure(true);
            }

            context.DrawGeometry(brush, outline, geometry);
            DrawTransitionEdge(context, face, 0, face.Trans1);
            DrawTransitionEdge(context, face, 1, face.Trans2);
            DrawTransitionEdge(context, face, 2, face.Trans3);
        }

        private void DrawTransitionEdge(DrawingContext context, BWMFace face, int edge, int? transition)
        {
            if (!transition.HasValue)
            {
                return;
            }

            GetEdge(face, edge, out var a, out var b);
            var pen = new Pen(Brushes.Gold, 3);
            context.DrawLine(pen, WorldToScreen(a.X, a.Y), WorldToScreen(b.X, b.Y));
        }

        private void DrawTransitionHighlight(DrawingContext context, BWMFace face, int edge)
        {
            GetEdge(face, edge, out var a, out var b);
            var pen = new Pen(Brushes.DeepSkyBlue, 5);
            context.DrawLine(pen, WorldToScreen(a.X, a.Y), WorldToScreen(b.X, b.Y));
        }

        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            Focus();
            _lastPointer = e.GetPosition(this);
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                PaintFaceAt(_lastPointer);
            }
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            var point = e.GetPosition(this);
            var world = ScreenToWorld(point);
            var face = _bwm?.FaceAt((float)world.X, (float)world.Y);
            StatusChanged?.Invoke(this, "x: " + world.X.ToString("0.00") + ", y: " + world.Y.ToString("0.00") + ", face: " + FaceIndex(face));

            var properties = e.GetCurrentPoint(this).Properties;
            if (properties.IsLeftButtonPressed)
            {
                PaintFaceAt(point);
            }
            else if (properties.IsRightButtonPressed)
            {
                var delta = point - _lastPointer;
                Camera.NudgePosition((float)(-delta.X / Camera.Zoom), (float)(-delta.Y / Camera.Zoom));
                InvalidateVisual();
            }

            _lastPointer = point;
        }

        private void OnPointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            Camera.NudgeZoom(e.Delta.Y > 0 ? 1.1f : 0.9f);
            InvalidateVisual();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            float panStep = Math.Max(0.1f, 32f / Camera.Zoom);
            bool handled = true;

            switch (e.Key)
            {
                case Key.Left:
                case Key.A:
                    Camera.NudgePosition(-panStep, 0);
                    break;
                case Key.Right:
                case Key.D:
                    Camera.NudgePosition(panStep, 0);
                    break;
                case Key.Up:
                case Key.W:
                    Camera.NudgePosition(0, -panStep);
                    break;
                case Key.Down:
                case Key.S:
                    Camera.NudgePosition(0, panStep);
                    break;
                case Key.Add:
                case Key.OemPlus:
                    Camera.NudgeZoom(1.1f);
                    break;
                case Key.Subtract:
                case Key.OemMinus:
                    Camera.NudgeZoom(0.9f);
                    break;
                case Key.F:
                    CenterCamera();
                    break;
                default:
                    handled = false;
                    break;
            }

            if (!handled)
            {
                return;
            }

            e.Handled = true;
            InvalidateVisual();
        }

        internal bool PaintFaceAtWorldForTests(float x, float y, bool shiftPressed)
        {
            return PaintFaceAtWorld(x, y);
        }

        private bool PaintFaceAt(Point point)
        {
            var world = ScreenToWorld(point);
            return PaintFaceAtWorld((float)world.X, (float)world.Y);
        }

        private bool PaintFaceAtWorld(float x, float y)
        {
            if (_bwm == null)
            {
                return false;
            }

            var face = _bwm.FaceAt(x, y);
            if (face == null || face.Material == SelectedMaterial)
            {
                return false;
            }

            face.Material = SelectedMaterial;
            FaceMaterialChanged?.Invoke(this, face);
            InvalidateVisual();
            return true;
        }

        private string FaceIndex(BWMFace face)
        {
            if (face == null || _bwm?.Faces == null)
            {
                return "None";
            }

            for (int i = 0; i < _bwm.Faces.Count; i++)
            {
                if (ReferenceEquals(_bwm.Faces[i], face))
                {
                    return i.ToString();
                }
            }

            return "None";
        }

        private Point WorldToScreen(float x, float y)
        {
            return new Point(
                (x - Camera.Position.X) * Camera.Zoom + Bounds.Width / 2,
                (y - Camera.Position.Y) * Camera.Zoom + Bounds.Height / 2);
        }

        private Point ScreenToWorld(Point point)
        {
            return new Point(
                (point.X - Bounds.Width / 2) / Camera.Zoom + Camera.Position.X,
                (point.Y - Bounds.Height / 2) / Camera.Zoom + Camera.Position.Y);
        }

        private void GetBounds(out float minX, out float minY, out float maxX, out float maxY)
        {
            var vertices = _bwm.Faces.SelectMany(face => new[] { face.V1, face.V2, face.V3 }).ToList();
            minX = vertices.Min(v => v.X);
            minY = vertices.Min(v => v.Y);
            maxX = vertices.Max(v => v.X);
            maxY = vertices.Max(v => v.Y);
        }

        private static void GetEdge(BWMFace face, int edge, out System.Numerics.Vector3 a, out System.Numerics.Vector3 b)
        {
            if (edge == 0)
            {
                a = face.V1;
                b = face.V2;
            }
            else if (edge == 1)
            {
                a = face.V2;
                b = face.V3;
            }
            else
            {
                a = face.V3;
                b = face.V1;
            }
        }
    }

    public partial class OdyToolBWM : Editor
    {
        private BWM _bwm;
        private BWMRenderArea _renderArea;
        private ListBox _materialList;
        private ListBox _transitionList;
        private TextBlock _summaryText;
        private TextBlock _statusText;
        private Dictionary<SurfaceMaterial, AColor> _materialColors;
        private readonly ModuleDesignerSettings _settings;

        public OdyToolBWM() : this(null, null) { }
        public OdyToolBWM(Window parent = null, OdyInstallation installation = null)
            : this(parent, installation, new ModuleDesignerSettings())
        {
        }

        internal OdyToolBWM(Window parent, OdyInstallation installation, ModuleDesignerSettings settings)
            : base(parent, "OdyToolBWM", "walkmesh",
                new[] { ResourceType.WOK, ResourceType.DWK, ResourceType.PWK },
                new[] { ResourceType.WOK, ResourceType.DWK, ResourceType.PWK },
                installation)
        {
            _settings = settings ?? new ModuleDesignerSettings();
            InitializeComponent();
            New();
        }

        private void InitializeComponent()
        {
            try
            {
                Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
            }
            catch
            {
                SetContentOrInject(new Grid());
            }
            SetupUI();
        }

        private void SetupUI()
        {
            _materialColors = CreateMaterialColors(_settings);

            var root = new DockPanel { LastChildFill = true };
            var toolbar = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8,
                Margin = new Thickness(10, 8)
            };
            DockPanel.SetDock(toolbar, Dock.Top);

            var roomBoundaries = new CheckBox { Content = "Room boundaries", IsChecked = true };
            roomBoundaries.IsCheckedChanged += (_, __) =>
            {
                if (_renderArea == null) return;
                _renderArea.ShowRoomBoundaries = roomBoundaries.IsChecked == true;
                _renderArea.InvalidateVisual();
            };
            var grid = new CheckBox { Content = "Grid" };
            grid.IsCheckedChanged += (_, __) =>
            {
                if (_renderArea == null) return;
                _renderArea.ShowGrid = grid.IsChecked == true;
                _renderArea.InvalidateVisual();
            };
            var frameButton = new Button { Content = "Frame All" };
            frameButton.Click += (_, __) => FrameAll();
            var paintHint = new TextBlock
            {
                Text = "Left-drag paints selected material",
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.DimGray
            };
            toolbar.Children.Add(roomBoundaries);
            toolbar.Children.Add(grid);
            toolbar.Children.Add(frameButton);
            toolbar.Children.Add(paintHint);
            root.Children.Add(toolbar);

            var content = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("260,*"),
                RowDefinitions = new RowDefinitions("*,Auto")
            };

            var sidePanel = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto,*,Auto,*"),
                Margin = new Thickness(10, 0, 10, 10)
            };
            sidePanel.Children.Add(new TextBlock { Text = "Materials", FontWeight = FontWeight.Bold, Margin = new Thickness(0, 0, 0, 6) });
            _materialList = new ListBox { MinHeight = 180 };
            Grid.SetRow(_materialList, 1);
            sidePanel.Children.Add(_materialList);

            var transHeader = new TextBlock { Text = "Transitions", FontWeight = FontWeight.Bold, Margin = new Thickness(0, 12, 0, 6) };
            Grid.SetRow(transHeader, 2);
            sidePanel.Children.Add(transHeader);
            _transitionList = new ListBox { MinHeight = 140 };
            Grid.SetRow(_transitionList, 3);
            sidePanel.Children.Add(_transitionList);
            Grid.SetColumn(sidePanel, 0);
            content.Children.Add(sidePanel);

            _renderArea = new BWMRenderArea
            {
                MinHeight = 420,
                Margin = new Thickness(0, 0, 10, 10)
            };
            _renderArea.SetMaterialColors(_materialColors);
            _renderArea.FaceMaterialChanged += (_, __) =>
            {
                MarkDocumentDirty();
                UpdateSummary();
            };
            _renderArea.StatusChanged += (_, text) =>
            {
                if (_statusText != null)
                {
                    _statusText.Text = text;
                }
            };
            Grid.SetColumn(_renderArea, 1);
            content.Children.Add(_renderArea);

            var status = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,Auto"),
                Margin = new Thickness(10, 0, 10, 8)
            };
            _summaryText = new TextBlock { Text = "No walkmesh loaded" };
            _statusText = new TextBlock { Text = "x: -, y: -, face: None" };
            status.Children.Add(_summaryText);
            Grid.SetColumn(_statusText, 1);
            status.Children.Add(_statusText);
            Grid.SetRow(status, 1);
            Grid.SetColumnSpan(status, 2);
            content.Children.Add(status);

            root.Children.Add(content);
            SetContentOrInject(root);

            RebuildMaterialList();
            _materialList.SelectionChanged += (_, __) =>
            {
                ApplySelectedMaterial(_materialList?.SelectedItem as MaterialListItem);
            };
            _transitionList.SelectionChanged += (_, __) =>
            {
                if (_transitionList?.SelectedItem is TransitionListItem item)
                {
                    _renderArea?.HighlightTransition(item.Face, item.Edge);
                }
                else
                {
                    _renderArea?.HighlightTransition(null, null);
                }
            };
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _bwm = BWMAuto.ReadBwm(data);
            LoadBWM(_bwm);
        }

        protected override bool TryResolveReadIdentity(string path, out ResourceType restype, out string resname)
        {
            if (base.TryResolveReadIdentity(path, out restype, out resname))
            {
                return true;
            }

            string extension = Path.GetExtension(path).TrimStart('.').ToLowerInvariant();
            if (extension == "bwm")
            {
                restype = ResourceType.WOK;
                resname = Path.GetFileNameWithoutExtension(path);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads BWM into editor state and refreshes the walkmesh view, material list, transition list, and summary.
        /// </summary>
        private void LoadBWM(BWM bwm)
        {
            _bwm = bwm ?? _bwm;
            _renderArea?.SetWalkmesh(_bwm);
            RebuildTransitions();
            UpdateSummary();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            ResourceType bwmType = _restype ?? ResourceType.WOK;
            byte[] data = BWMAuto.BytesBwm(_bwm, bwmType);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _bwm = new BWM();
            _renderArea?.SetWalkmesh(_bwm);
            RebuildTransitions();
            UpdateSummary();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        protected override async Task RunSaveAsAsync()
        {
            var storage = StorageProvider;
            if (storage == null) return;
            string suggestedName = !string.IsNullOrEmpty(_resname) ? _resname : "walkmesh";
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".wok",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Walkmesh (WOK)") { Patterns = new[] { "*.wok" } },
                    new FilePickerFileType("Door walkmesh (DWK)") { Patterns = new[] { "*.dwk" } },
                    new FilePickerFileType("Placeable walkmesh (PWK)") { Patterns = new[] { "*.pwk" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
                }
            };
            var file = await storage.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            string ext = (Path.GetExtension(path) ?? "").TrimStart('.').ToLowerInvariant();
            _restype = ResourceType.FromExtension(ext) ?? ResourceType.WOK;
            _resname = Path.GetFileNameWithoutExtension(path);
            RefreshWindowTitle();
            Save();
        }

        public int FaceCount => _bwm?.Faces?.Count ?? 0;
        public int TransitionCount => CountTransitions(_bwm);

        internal BWMRenderArea RenderAreaForTests => _renderArea;
        internal string SummaryTextForTests => _summaryText?.Text ?? "";
        internal SurfaceMaterial SelectedMaterialForTests => _renderArea?.SelectedMaterial ?? SurfaceMaterial.Undefined;
        internal AColor MaterialColorForTests(SurfaceMaterial material)
        {
            return _materialColors != null && _materialColors.TryGetValue(material, out var color)
                ? color
                : default;
        }

        internal void SelectMaterialForTests(SurfaceMaterial material)
        {
            if (_materialList?.ItemsSource is IEnumerable<MaterialListItem> items)
            {
                var item = items.FirstOrDefault(entry => entry.Material == material);
                if (item != null)
                {
                    ApplySelectedMaterial(item);
                }
            }
        }

        public void FrameAll()
        {
            _renderArea?.CenterCamera();
            _renderArea?.InvalidateVisual();
        }

        public void MoveCamera(float x, float y)
        {
            if (_renderArea == null)
            {
                return;
            }

            _renderArea.Camera.NudgePosition(x, y);
            _renderArea.InvalidateVisual();
        }

        public void ZoomCamera(float amount)
        {
            if (_renderArea == null)
            {
                return;
            }

            _renderArea.Camera.NudgeZoom(amount);
            _renderArea.InvalidateVisual();
        }

        private void RebuildMaterialList()
        {
            if (_materialList == null)
            {
                return;
            }

            var items = Enum.GetValues(typeof(SurfaceMaterial))
                .Cast<SurfaceMaterial>()
                .Select(material => new MaterialListItem(material, _materialColors.TryGetValue(material, out var color) ? color : AColor.FromRgb(120, 120, 120)))
                .ToList();
            _materialList.ItemsSource = items;
            _materialList.SelectedItem = items.FirstOrDefault(item => item.Material == SurfaceMaterial.Stone) ?? items.FirstOrDefault();
        }

        private void ApplySelectedMaterial(MaterialListItem item)
        {
            if (item != null && _renderArea != null)
            {
                _renderArea.SelectedMaterial = item.Material;
            }
        }

        private void RebuildTransitions()
        {
            if (_transitionList == null)
            {
                return;
            }

            var items = new List<TransitionListItem>();
            if (_bwm?.Faces != null)
            {
                for (int i = 0; i < _bwm.Faces.Count; i++)
                {
                    var face = _bwm.Faces[i];
                    AddTransition(items, face, i, 0, face.Trans1);
                    AddTransition(items, face, i, 1, face.Trans2);
                    AddTransition(items, face, i, 2, face.Trans3);
                }
            }
            _transitionList.ItemsSource = items;
        }

        private void UpdateSummary()
        {
            if (_summaryText == null)
            {
                return;
            }

            var faces = FaceCount;
            var walkable = _bwm?.Faces?.Count(face => face.Material.Walkable()) ?? 0;
            var transitions = TransitionCount;
            _summaryText.Text = "Faces: " + faces + "  Walkable: " + walkable + "  Transitions: " + transitions;
        }

        private static void AddTransition(List<TransitionListItem> items, BWMFace face, int faceIndex, int edge, int? transition)
        {
            if (transition.HasValue)
            {
                items.Add(new TransitionListItem(face, faceIndex, edge, transition.Value));
            }
        }

        private static int CountTransitions(BWM bwm)
        {
            if (bwm?.Faces == null)
            {
                return 0;
            }

            return bwm.Faces.Sum(face =>
                (face.Trans1.HasValue ? 1 : 0) +
                (face.Trans2.HasValue ? 1 : 0) +
                (face.Trans3.HasValue ? 1 : 0));
        }

        public static Dictionary<SurfaceMaterial, AColor> CreateDefaultMaterialColors()
        {
            return new Dictionary<SurfaceMaterial, AColor>
            {
                { SurfaceMaterial.Undefined, AColor.FromRgb(92, 99, 112) },
                { SurfaceMaterial.Dirt, AColor.FromRgb(134, 98, 64) },
                { SurfaceMaterial.Obscuring, AColor.FromRgb(80, 80, 88) },
                { SurfaceMaterial.Grass, AColor.FromRgb(67, 132, 73) },
                { SurfaceMaterial.Stone, AColor.FromRgb(133, 140, 148) },
                { SurfaceMaterial.Wood, AColor.FromRgb(141, 104, 61) },
                { SurfaceMaterial.Water, AColor.FromRgb(54, 126, 180) },
                { SurfaceMaterial.NonWalk, AColor.FromRgb(164, 58, 62) },
                { SurfaceMaterial.Transparent, AColor.FromArgb(120, 210, 210, 220) },
                { SurfaceMaterial.Carpet, AColor.FromRgb(142, 65, 110) },
                { SurfaceMaterial.Metal, AColor.FromRgb(150, 158, 166) },
                { SurfaceMaterial.Puddles, AColor.FromRgb(60, 115, 130) },
                { SurfaceMaterial.Swamp, AColor.FromRgb(85, 112, 67) },
                { SurfaceMaterial.Mud, AColor.FromRgb(92, 74, 52) },
                { SurfaceMaterial.Leaves, AColor.FromRgb(107, 134, 59) },
                { SurfaceMaterial.Lava, AColor.FromRgb(206, 79, 43) },
                { SurfaceMaterial.BottomlessPit, AColor.FromRgb(34, 36, 43) },
                { SurfaceMaterial.DeepWater, AColor.FromRgb(42, 76, 138) },
                { SurfaceMaterial.Door, AColor.FromRgb(195, 157, 72) },
                { SurfaceMaterial.NonWalkGrass, AColor.FromRgb(146, 70, 76) },
                { SurfaceMaterial.SurfaceMaterial20, AColor.FromRgb(177, 159, 105) },
                { SurfaceMaterial.SurfaceMaterial21, AColor.FromRgb(176, 168, 150) },
                { SurfaceMaterial.SurfaceMaterial22, AColor.FromRgb(150, 150, 136) },
                { SurfaceMaterial.SurfaceMaterial23, AColor.FromRgb(120, 120, 120) },
                { SurfaceMaterial.SurfaceMaterial24, AColor.FromRgb(128, 128, 128) },
                { SurfaceMaterial.SurfaceMaterial25, AColor.FromRgb(136, 136, 136) },
                { SurfaceMaterial.SurfaceMaterial26, AColor.FromRgb(144, 144, 144) },
                { SurfaceMaterial.SurfaceMaterial27, AColor.FromRgb(152, 152, 152) },
                { SurfaceMaterial.SurfaceMaterial28, AColor.FromRgb(160, 160, 160) },
                { SurfaceMaterial.SurfaceMaterial29, AColor.FromRgb(168, 168, 168) },
                { SurfaceMaterial.Trigger, AColor.FromRgb(86, 171, 178) }
            };
        }

        public static Dictionary<SurfaceMaterial, AColor> CreateMaterialColors(ModuleDesignerSettings settings)
        {
            var colors = CreateDefaultMaterialColors();
            if (settings == null)
            {
                return colors;
            }

            colors[SurfaceMaterial.Undefined] = ToAvaloniaColor(settings.UndefinedMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Dirt] = ToAvaloniaColor(settings.DirtMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Obscuring] = ToAvaloniaColor(settings.ObscuringMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Grass] = ToAvaloniaColor(settings.GrassMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Stone] = ToAvaloniaColor(settings.StoneMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Wood] = ToAvaloniaColor(settings.WoodMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Water] = ToAvaloniaColor(settings.WaterMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.NonWalk] = ToAvaloniaColor(settings.NonWalkMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Transparent] = ToAvaloniaColor(settings.TransparentMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Carpet] = ToAvaloniaColor(settings.CarpetMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Metal] = ToAvaloniaColor(settings.MetalMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Puddles] = ToAvaloniaColor(settings.PuddlesMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Swamp] = ToAvaloniaColor(settings.SwampMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Mud] = ToAvaloniaColor(settings.MudMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Leaves] = ToAvaloniaColor(settings.LeavesMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Door] = ToAvaloniaColor(settings.DoorMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Lava] = ToAvaloniaColor(settings.LavaMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.BottomlessPit] = ToAvaloniaColor(settings.BottomlessPitMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.DeepWater] = ToAvaloniaColor(settings.DeepWaterMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.NonWalkGrass] = ToAvaloniaColor(settings.NonWalkGrassMaterialColour.GetValue(settings));
            colors[SurfaceMaterial.Trigger] = colors[SurfaceMaterial.NonWalkGrass];
            return colors;
        }

        private static AColor ToAvaloniaColor(int rgba)
        {
            var color = KotorColor.FromRgbaInteger(rgba);
            return AColor.FromArgb(
                (byte)(Math.Clamp(color.A, 0f, 1f) * 255f),
                (byte)(Math.Clamp(color.R, 0f, 1f) * 255f),
                (byte)(Math.Clamp(color.G, 0f, 1f) * 255f),
                (byte)(Math.Clamp(color.B, 0f, 1f) * 255f));
        }

        private sealed class MaterialListItem
        {
            public MaterialListItem(SurfaceMaterial material, AColor color)
            {
                Material = material;
                Color = color;
            }

            public SurfaceMaterial Material { get; }
            public AColor Color { get; }

            public override string ToString()
            {
                return ((int)Material).ToString("00") + "  " + Material;
            }
        }

        private sealed class TransitionListItem
        {
            public TransitionListItem(BWMFace face, int faceIndex, int edge, int transition)
            {
                Face = face;
                FaceIndex = faceIndex;
                Edge = edge;
                Transition = transition;
            }

            public BWMFace Face { get; }
            public int FaceIndex { get; }
            public int Edge { get; }
            public int Transition { get; }

            public override string ToString()
            {
                return "Face " + FaceIndex + " edge " + Edge + " -> " + Transition;
            }
        }
    }
}
