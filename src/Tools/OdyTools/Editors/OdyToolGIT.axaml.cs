using BioWare.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource;
using OdyTools.Blender;
using OdyTools.Data;
using Game = BioWare.Common.BioWareGame;
using GFFAuto = BioWare.Resource.Formats.GFF.GFFAuto;

namespace OdyTools.Editors
{
    /// <summary>Display item for the instance list: type label + resref/tag.</summary>
    public sealed class GITInstanceItem
    {
        public string DisplayText { get; set; }
        public string TypeName { get; set; }
        public object Instance { get; set; }
    }

    public sealed class GITRenderArea : Control
    {
        private readonly HashSet<string> _visibleTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private GIT _git = new GIT();
        private object _selectedInstance;
        private bool _isPointerDown;
        private bool _draggingSelected;
        private Vector2 _lastWorldPosition;

        public event EventHandler<object> InstanceSelected;
        public event EventHandler SelectedInstanceMoved;

        public GITRenderCamera Camera { get; } = new GITRenderCamera();

        public GITRenderArea()
        {
            Focusable = true;
            foreach (var typeName in InstanceTypeNames)
            {
                _visibleTypes.Add(typeName);
            }

            PointerPressed += OnPointerPressed;
            PointerMoved += OnPointerMoved;
            PointerReleased += OnPointerReleased;
            PointerWheelChanged += OnPointerWheelChanged;
        }

        public static readonly string[] InstanceTypeNames =
        {
            "Creature", "Door", "Placeable", "Trigger", "Waypoint", "Sound", "Store", "Encounter", "Camera"
        };

        public void SetGit(GIT git)
        {
            _git = git ?? new GIT();
            if (_selectedInstance != null && !_git.Instances().Contains(_selectedInstance))
            {
                _selectedInstance = null;
            }
            CenterCamera();
            InvalidateVisual();
        }

        public void SelectInstance(object instance)
        {
            _selectedInstance = instance;
            InvalidateVisual();
        }

        public object SelectedInstance => _selectedInstance;

        public void SetTypeVisible(string typeName, bool visible)
        {
            if (string.IsNullOrWhiteSpace(typeName)) return;
            if (visible)
                _visibleTypes.Add(typeName);
            else
                _visibleTypes.Remove(typeName);
            InvalidateVisual();
        }

        public bool IsTypeVisible(string typeName)
        {
            return _visibleTypes.Contains(typeName);
        }

        public int VisibleInstanceCount()
        {
            return AllVisibleInstances().Count();
        }

        public Vector2 ScreenToWorld(Point screenPoint)
        {
            double centerX = Bounds.Width / 2.0;
            double centerY = Bounds.Height / 2.0;
            double worldX = (screenPoint.X - centerX) / Camera.Zoom + Camera.Position.X;
            double worldY = (screenPoint.Y - centerY) / Camera.Zoom + Camera.Position.Y;
            return new Vector2((float)worldX, (float)worldY);
        }

        private Point WorldToScreen(Vector3 position)
        {
            double centerX = Bounds.Width / 2.0;
            double centerY = Bounds.Height / 2.0;
            double screenX = (position.X - Camera.Position.X) * Camera.Zoom + centerX;
            double screenY = (position.Y - Camera.Position.Y) * Camera.Zoom + centerY;
            return new Point(screenX, screenY);
        }

        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            Focus();
            var point = e.GetCurrentPoint(this);
            var world = ScreenToWorld(point.Position);
            _isPointerDown = true;
            _lastWorldPosition = world;

            var hit = InstanceUnderPointer(point.Position);
            _selectedInstance = hit;
            _draggingSelected = hit != null && point.Properties.IsLeftButtonPressed;
            InstanceSelected?.Invoke(this, hit);
            InvalidateVisual();
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            if (!_isPointerDown) return;
            var point = e.GetCurrentPoint(this);
            var world = ScreenToWorld(point.Position);
            var delta = world - _lastWorldPosition;
            _lastWorldPosition = world;

            if (_draggingSelected && _selectedInstance != null)
            {
                MoveInstanceBy(_selectedInstance, delta.X, delta.Y);
                SelectedInstanceMoved?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                Camera.NudgePosition(-delta.X, -delta.Y);
            }
            InvalidateVisual();
        }

        private void OnPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            _isPointerDown = false;
            _draggingSelected = false;
        }

        private void OnPointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            Camera.NudgeZoom(e.Delta.Y > 0 ? 1.1f : 0.9f);
            InvalidateVisual();
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            context.FillRectangle(new SolidColorBrush(Avalonia.Media.Color.FromRgb(26, 28, 32)), new Rect(0, 0, Bounds.Width, Bounds.Height));
            DrawGrid(context);
            foreach (var instance in AllVisibleInstances())
            {
                DrawInstance(context, instance);
            }
        }

        private void DrawGrid(DrawingContext context)
        {
            var pen = new Pen(new SolidColorBrush(Avalonia.Media.Color.FromRgb(45, 49, 56)), 1);
            const int step = 48;
            for (double x = Bounds.Width / 2.0 % step; x < Bounds.Width; x += step)
                context.DrawLine(pen, new Point(x, 0), new Point(x, Bounds.Height));
            for (double y = Bounds.Height / 2.0 % step; y < Bounds.Height; y += step)
                context.DrawLine(pen, new Point(0, y), new Point(Bounds.Width, y));
        }

        private void DrawInstance(DrawingContext context, object instance)
        {
            var position = GetPosition(instance);
            var screen = WorldToScreen(position);
            var typeName = GetTypeName(instance);
            bool selected = ReferenceEquals(instance, _selectedInstance);
            var color = ColorForType(typeName);
            var brush = new SolidColorBrush(color);
            var pen = new Pen(new SolidColorBrush(selected ? Avalonia.Media.Color.FromRgb(255, 214, 102) : Avalonia.Media.Color.FromRgb(15, 18, 22)), selected ? 3 : 1);
            double radius = selected ? 8 : 6;

            context.DrawEllipse(brush, pen, screen, radius, radius);

            float bearing = GetBearing(instance);
            if (!float.IsNaN(bearing))
            {
                var end = new Point(screen.X + Math.Cos(bearing) * 18, screen.Y + Math.Sin(bearing) * 18);
                context.DrawLine(new Pen(Brushes.White, selected ? 2 : 1), screen, end);
            }
        }

        private object InstanceUnderPointer(Point screenPoint)
        {
            object best = null;
            double bestDistance = double.MaxValue;
            foreach (var instance in AllVisibleInstances())
            {
                var screen = WorldToScreen(GetPosition(instance));
                double dx = screen.X - screenPoint.X;
                double dy = screen.Y - screenPoint.Y;
                double dist = Math.Sqrt((dx * dx) + (dy * dy));
                if (dist <= 12 && dist < bestDistance)
                {
                    best = instance;
                    bestDistance = dist;
                }
            }
            return best;
        }

        private IEnumerable<object> AllVisibleInstances()
        {
            if (_git == null) yield break;
            foreach (var instance in _git.Instances())
            {
                if (IsTypeVisible(GetTypeName(instance)))
                    yield return instance;
            }
        }

        public void CenterCamera()
        {
            var instances = AllVisibleInstances().ToList();
            if (instances.Count == 0)
            {
                Camera.SetPosition(Vector2.Zero);
                return;
            }
            float x = instances.Average(instance => GetPosition(instance).X);
            float y = instances.Average(instance => GetPosition(instance).Y);
            Camera.SetPosition(new Vector2(x, y));
        }

        public static string GetTypeName(object instance)
        {
            if (instance is GITCreature) return "Creature";
            if (instance is GITDoor) return "Door";
            if (instance is GITPlaceable) return "Placeable";
            if (instance is GITTrigger) return "Trigger";
            if (instance is GITWaypoint) return "Waypoint";
            if (instance is GITSound) return "Sound";
            if (instance is GITStore) return "Store";
            if (instance is GITEncounter) return "Encounter";
            if (instance is GITCamera) return "Camera";
            return "Instance";
        }

        public static Vector3 GetPosition(object instance)
        {
            if (instance is GITCreature c) return c.Position;
            if (instance is GITDoor d) return d.Position;
            if (instance is GITPlaceable p) return p.Position;
            if (instance is GITTrigger t) return t.Position;
            if (instance is GITWaypoint w) return w.Position;
            if (instance is GITSound s) return s.Position;
            if (instance is GITStore st) return st.Position;
            if (instance is GITEncounter e) return e.Position;
            if (instance is GITCamera cam) return cam.Position;
            return Vector3.Zero;
        }

        public static void SetPosition(object instance, Vector3 position)
        {
            if (instance is GITCreature c) c.Position = position;
            else if (instance is GITDoor d) d.Position = position;
            else if (instance is GITPlaceable p) p.Position = position;
            else if (instance is GITTrigger t) t.Position = position;
            else if (instance is GITWaypoint w) w.Position = position;
            else if (instance is GITSound s) s.Position = position;
            else if (instance is GITStore st) st.Position = position;
            else if (instance is GITEncounter e) e.Position = position;
            else if (instance is GITCamera cam) cam.Position = position;
        }

        public static void MoveInstanceBy(object instance, float x, float y)
        {
            var position = GetPosition(instance);
            SetPosition(instance, new Vector3(position.X + x, position.Y + y, position.Z));
        }

        private static float GetBearing(object instance)
        {
            if (instance is GITCreature c) return c.Bearing;
            if (instance is GITDoor d) return d.Bearing;
            if (instance is GITPlaceable p) return p.Bearing;
            if (instance is GITWaypoint w) return w.Bearing;
            if (instance is GITStore st) return st.Bearing;
            return float.NaN;
        }

        private static Avalonia.Media.Color ColorForType(string typeName)
        {
            switch (typeName)
            {
                case "Creature": return Avalonia.Media.Color.FromRgb(116, 185, 255);
                case "Door": return Avalonia.Media.Color.FromRgb(253, 203, 110);
                case "Placeable": return Avalonia.Media.Color.FromRgb(85, 239, 196);
                case "Trigger": return Avalonia.Media.Color.FromRgb(255, 118, 117);
                case "Waypoint": return Avalonia.Media.Color.FromRgb(162, 155, 254);
                case "Sound": return Avalonia.Media.Color.FromRgb(129, 236, 236);
                case "Store": return Avalonia.Media.Color.FromRgb(250, 177, 160);
                case "Encounter": return Avalonia.Media.Color.FromRgb(225, 112, 85);
                case "Camera": return Avalonia.Media.Color.FromRgb(223, 230, 233);
                default: return Avalonia.Media.Color.FromRgb(178, 190, 195);
            }
        }
    }

    public sealed class GITRenderCamera
    {
        public Vector2 Position { get; private set; }
        public float Zoom { get; private set; } = 12.0f;

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
            if (amount <= 0) return;
            float next = Zoom * amount;
            if (next < 2.0f) next = 2.0f;
            if (next > 128.0f) next = 128.0f;
            Zoom = next;
        }
    }

    public partial class OdyToolGIT : Editor
    {
        private GIT _git;
        private GFF _originalGff;

        private List<GITInstanceItem> _instanceItems = new List<GITInstanceItem>();
        private object _selectedInstance;
        private readonly List<GIT> _undoStack = new List<GIT>();
        private readonly List<GIT> _redoStack = new List<GIT>();
        private const int UndoMaxLevels = 30;

        // XAML controls (optional when AXAML loaded)
        private TextBox _filterEdit;
        private ListBox _instanceList;
        private StackPanel _detailNoSelection;
        private StackPanel _detailInstance;
        private TextBlock _detailTypeLabel;
        private TextBox _detailResRef;
        private NumericUpDown _detailPosX, _detailPosY, _detailPosZ, _detailBearing;
        private TextBox _detailTag;
        private TextBlock _detailTagLabel;
        private ComboBox _addInstanceType;
        private Button _addInstanceButton;
        private Button _duplicateInstanceButton;
        private Button _removeInstanceButton;
        private MenuItem _openInBlenderMenuItem;
        private TextBlock _statusText;
        private GITRenderArea _renderArea;
        private StackPanel _visibilityPanel;
        private bool _syncingDetails;
        private string _blenderStatus;
        private Func<string, BlenderInfo> _detectBlender = BlenderDetection.DetectBlender;
        private Func<BlenderInfo, int, string, string, string, bool, System.Diagnostics.Process> _launchBlender = BlenderDetection.LaunchBlenderWithIpc;
        private static readonly string[] CreatableInstanceTypeNames =
        {
            "Creature", "Door", "Placeable", "Trigger", "Waypoint", "Sound", "Store", "Encounter", "Camera"
        };

        internal bool HasStructuredEditorSurface =>
            _filterEdit != null &&
            _instanceList != null &&
            _renderArea != null &&
            _detailResRef != null &&
            _detailPosX != null &&
            _detailPosY != null &&
            _detailPosZ != null &&
            _detailBearing != null &&
            _detailTag != null &&
            _addInstanceType != null &&
            _addInstanceButton != null &&
            _duplicateInstanceButton != null &&
            _removeInstanceButton != null;

        internal TextBox FilterEdit => _filterEdit;
        internal ListBox InstanceList => _instanceList;
        internal TextBox DetailResRef => _detailResRef;
        internal NumericUpDown DetailPosX => _detailPosX;
        internal NumericUpDown DetailPosY => _detailPosY;
        internal NumericUpDown DetailPosZ => _detailPosZ;
        internal NumericUpDown DetailBearing => _detailBearing;
        internal TextBox DetailTag => _detailTag;
        internal ComboBox AddInstanceType => _addInstanceType;
        internal Button AddInstanceButton => _addInstanceButton;
        internal Button DuplicateInstanceButton => _duplicateInstanceButton;
        internal Button RemoveInstanceButton => _removeInstanceButton;
        internal MenuItem OpenInBlenderMenuItem => _openInBlenderMenuItem;
        internal string BlenderStatusText => _blenderStatus;

        public OdyToolGIT() : this(null, null) { }
        public OdyToolGIT(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolGIT", "git",
                new[] { ResourceType.GIT, ResourceType.GIT_XML },
                new[] { ResourceType.GIT, ResourceType.GIT_XML },
                installation)
        {
            _git = new GIT();
            InitializeComponent();
            SetupUI();
            AddHelpAction(); // Auto-detects "GFF-GIT.md" for GIT
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
                SetupUI();
            }
        }

        private void SetupUI()
        {
            _filterEdit = EditorHelpers.FindControlSafe<TextBox>(this, "filterEdit");
            _instanceList = EditorHelpers.FindControlSafe<ListBox>(this, "instanceList");
            _detailNoSelection = EditorHelpers.FindControlSafe<StackPanel>(this, "detailNoSelection");
            _detailInstance = EditorHelpers.FindControlSafe<StackPanel>(this, "detailInstance");
            _detailTypeLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "detailTypeLabel");
            _detailResRef = EditorHelpers.FindControlSafe<TextBox>(this, "detailResRef");
            _detailPosX = EditorHelpers.FindControlSafe<NumericUpDown>(this, "detailPosX");
            _detailPosY = EditorHelpers.FindControlSafe<NumericUpDown>(this, "detailPosY");
            _detailPosZ = EditorHelpers.FindControlSafe<NumericUpDown>(this, "detailPosZ");
            _detailBearing = EditorHelpers.FindControlSafe<NumericUpDown>(this, "detailBearing");
            _detailTag = EditorHelpers.FindControlSafe<TextBox>(this, "detailTag");
            _detailTagLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "detailTagLabel");
            _addInstanceType = EditorHelpers.FindControlSafe<ComboBox>(this, "addInstanceType");
            _addInstanceButton = EditorHelpers.FindControlSafe<Button>(this, "addInstanceButton");
            _duplicateInstanceButton = EditorHelpers.FindControlSafe<Button>(this, "duplicateInstanceButton");
            _removeInstanceButton = EditorHelpers.FindControlSafe<Button>(this, "removeInstanceButton");
            _openInBlenderMenuItem = EditorHelpers.FindControlSafe<MenuItem>(this, "actionOpenInBlender");
            _statusText = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");
            _renderArea = EditorHelpers.FindControlSafe<GITRenderArea>(this, "renderArea");
            _visibilityPanel = EditorHelpers.FindControlSafe<StackPanel>(this, "visibilityPanel");

            if (_instanceList != null)
            {
                void BindLostFocus(TextBox textBox)
                {
                    if (textBox != null)
                    {
                        textBox.LostFocus += (s, e) => SaveDetailToInstance();
                    }
                }

                void BindValueChanged(NumericUpDown numericUpDown)
                {
                    if (numericUpDown != null)
                    {
                        numericUpDown.ValueChanged += (s, e) => SaveDetailToInstance();
                    }
                }

                _instanceList.SelectionChanged += OnInstanceListSelectionChanged;
                if (_filterEdit != null)
                    _filterEdit.TextChanged += (s, e) => ApplyFilter();
                BindLostFocus(_detailResRef);
                BindValueChanged(_detailPosX);
                BindValueChanged(_detailPosY);
                BindValueChanged(_detailPosZ);
                BindValueChanged(_detailBearing);
                BindLostFocus(_detailTag);
                if (_addInstanceType != null)
                {
                    _addInstanceType.ItemsSource = CreatableInstanceTypeNames.ToList();
                    _addInstanceType.SelectedIndex = 0;
                }
                if (_addInstanceButton != null) _addInstanceButton.Click += (s, e) => AddSelectedTypeInstance();
                if (_duplicateInstanceButton != null) _duplicateInstanceButton.Click += (s, e) => DuplicateSelectedInstance();
                if (_removeInstanceButton != null) _removeInstanceButton.Click += (s, e) => RemoveSelectedInstance();
                SetupRenderArea();
                SetupVisibilityToggles();
                RebuildInstanceList();
                UpdateStatusBar();
            }

            SetupMenuHandlers();
            RefreshBlenderActionState();
            if (_instanceList == null)
            {
                var panel = new StackPanel();
                Content = panel;
            }
        }

        private void SetupMenuHandlers()
        {
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
            EditorHelpers.BindMenuClicks(this, new (string menuItemName, Action handler)[]
            {
                ("actionUndoGit", UndoGitEdit),
                ("actionRedoGit", RedoGitEdit),
                ("actionDuplicateSelected", DuplicateSelectedInstance),
                ("actionDeleteSelected", RemoveSelectedInstance),
                ("actionOpenInBlender", () => TryLaunchBlenderForCurrentGit()),
            });
        }

        public bool TryLaunchBlenderForCurrentGit()
        {
            if (string.IsNullOrEmpty(Filepath))
            {
                SetBlenderStatus("Save or open a GIT before launching Blender.");
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

            SetBlenderStatus($"Launched Blender for {System.IO.Path.GetFileName(Filepath)}.");
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
                SetBlenderStatus("Open a GIT file to use Blender.");
            }
            else if (string.IsNullOrEmpty(_blenderStatus))
            {
                UpdateStatusBar();
            }
        }

        private void SetBlenderStatus(string status)
        {
            _blenderStatus = status ?? string.Empty;
            UpdateStatusBar();
        }

        private void SetupRenderArea()
        {
            if (_renderArea == null) return;
            _renderArea.SetGit(_git);
            _renderArea.InstanceSelected += (s, instance) => SelectInstance(instance, syncList: true);
            _renderArea.SelectedInstanceMoved += (s, e) =>
            {
                LoadDetailFromInstance();
                RebuildInstanceList();
                MarkDocumentDirty();
                UpdateStatusBar();
            };
        }

        private void SetupVisibilityToggles()
        {
            if (_visibilityPanel == null || _renderArea == null) return;
            foreach (var child in _visibilityPanel.Children)
            {
                if (child is CheckBox checkBox)
                {
                    var typeName = checkBox.Tag?.ToString();
                    if (string.IsNullOrWhiteSpace(typeName))
                    {
                        continue;
                    }
                    checkBox.IsChecked = _renderArea.IsTypeVisible(typeName);
                    checkBox.IsCheckedChanged += (s, e) =>
                    {
                        _renderArea.SetTypeVisible(typeName, checkBox.IsChecked == true);
                        ApplyFilter();
                        UpdateStatusBar();
                    };
                }
            }
        }

        protected override async System.Threading.Tasks.Task RunSaveAsAsync()
        {
            await base.RunSaveAsAsync();
            UpdateStatusBar();
        }

        private void RebuildInstanceList()
        {
            _instanceItems.Clear();
            if (_git == null) return;
            string resref(object o)
            {
                if (o is GITCreature c) return c.ResRef.ToString();
                if (o is GITDoor d) return d.ResRef.ToString();
                if (o is GITPlaceable p) return p.ResRef.ToString();
                if (o is GITTrigger t) return t.ResRef.ToString();
                if (o is GITWaypoint w) return w.ResRef.ToString();
                if (o is GITSound s) return s.ResRef.ToString();
                if (o is GITStore st) return st.ResRef.ToString();
                if (o is GITEncounter e) return e.ResRef.ToString();
                if (o is GITCamera cam) return cam.ResRef.ToString();
                return "";
            }
            string tag(object o)
            {
                if (o is GITDoor d) return d.Tag ?? "";
                if (o is GITPlaceable p) return p.Tag ?? "";
                if (o is GITTrigger t) return t.Tag ?? "";
                if (o is GITWaypoint w) return w.Tag ?? "";
                if (o is GITSound s) return s.Tag ?? "";
                return "";
            }
            foreach (var c in _git.Creatures)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Creature] {resref(c)}", TypeName = "Creature", Instance = c });
            foreach (var d in _git.Doors)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Door] {(string.IsNullOrEmpty(tag(d)) ? resref(d) : tag(d))}", TypeName = "Door", Instance = d });
            foreach (var p in _git.Placeables)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Placeable] {(tag(p).Length > 0 ? tag(p) : resref(p))}", TypeName = "Placeable", Instance = p });
            foreach (var t in _git.Triggers)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Trigger] {(tag(t).Length > 0 ? tag(t) : resref(t))}", TypeName = "Trigger", Instance = t });
            foreach (var w in _git.Waypoints)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Waypoint] {(tag(w).Length > 0 ? tag(w) : resref(w))}", TypeName = "Waypoint", Instance = w });
            foreach (var s in _git.Sounds)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Sound] {(tag(s).Length > 0 ? tag(s) : resref(s))}", TypeName = "Sound", Instance = s });
            foreach (var st in _git.Stores)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Store] {resref(st)}", TypeName = "Store", Instance = st });
            foreach (var e in _git.Encounters)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Encounter] {resref(e)}", TypeName = "Encounter", Instance = e });
            foreach (var cam in _git.Cameras)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Camera] {cam.CameraId}", TypeName = "Camera", Instance = cam });

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_instanceList == null) return;
            string filter = (_filterEdit?.Text ?? "").Trim().ToLowerInvariant();
            var filtered = _instanceItems.Where(x =>
                (_renderArea == null || _renderArea.IsTypeVisible(x.TypeName)) &&
                (string.IsNullOrEmpty(filter) || (x.DisplayText ?? "").ToLowerInvariant().Contains(filter))).ToList();
            _instanceList.SelectionChanged -= OnInstanceListSelectionChanged;
            _instanceList.ItemsSource = filtered;
            _instanceList.SelectedItem = filtered.FirstOrDefault(x => ReferenceEquals(x.Instance, _selectedInstance));
            _instanceList.SelectionChanged += OnInstanceListSelectionChanged;
        }

        private void OnInstanceListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = _instanceList?.SelectedItem as GITInstanceItem;
            SelectInstance(item?.Instance, syncList: false);
        }

        private void SelectInstance(object instance, bool syncList)
        {
            _selectedInstance = instance;
            _renderArea?.SelectInstance(instance);
            if (syncList && _instanceList != null)
            {
                var currentItems = (_instanceList.ItemsSource as IEnumerable<GITInstanceItem>)?.ToList() ?? new List<GITInstanceItem>();
                _instanceList.SelectionChanged -= OnInstanceListSelectionChanged;
                _instanceList.SelectedItem = currentItems.FirstOrDefault(x => ReferenceEquals(x.Instance, instance));
                _instanceList.SelectionChanged += OnInstanceListSelectionChanged;
            }
            LoadDetailFromInstance();
            if (_detailNoSelection != null) _detailNoSelection.IsVisible = _selectedInstance == null;
            if (_detailInstance != null) _detailInstance.IsVisible = _selectedInstance != null;
            if (_duplicateInstanceButton != null) _duplicateInstanceButton.IsEnabled = _selectedInstance != null;
            if (_removeInstanceButton != null) _removeInstanceButton.IsEnabled = _selectedInstance != null;
            UpdateStatusBar();
        }

        private void LoadDetailFromInstance()
        {
            if (_detailResRef == null) return;
            _syncingDetails = true;
            bool hasBearing = _selectedInstance is GITCreature || _selectedInstance is GITDoor || _selectedInstance is GITPlaceable || _selectedInstance is GITStore || _selectedInstance is GITWaypoint;
            bool hasTag = _selectedInstance is GITDoor || _selectedInstance is GITPlaceable || _selectedInstance is GITTrigger || _selectedInstance is GITWaypoint || _selectedInstance is GITSound;
            if (_detailBearing != null) _detailBearing.IsVisible = hasBearing;
            if (_detailTag != null) _detailTag.IsVisible = hasTag;
            if (_detailTagLabel != null) _detailTagLabel.IsVisible = hasTag;

            if (_selectedInstance == null)
            {
                _detailResRef.Text = "";
                if (_detailPosX != null) _detailPosX.Value = 0;
                if (_detailPosY != null) _detailPosY.Value = 0;
                if (_detailPosZ != null) _detailPosZ.Value = 0;
                if (_detailBearing != null) _detailBearing.Value = 0;
                if (_detailTag != null) _detailTag.Text = "";
                if (_detailTypeLabel != null) _detailTypeLabel.Text = "Instance";
                _syncingDetails = false;
                return;
            }

            if (_detailTypeLabel != null)
            {
                if (_selectedInstance is GITCreature) _detailTypeLabel.Text = "Creature";
                else if (_selectedInstance is GITDoor) _detailTypeLabel.Text = "Door";
                else if (_selectedInstance is GITPlaceable) _detailTypeLabel.Text = "Placeable";
                else if (_selectedInstance is GITTrigger) _detailTypeLabel.Text = "Trigger";
                else if (_selectedInstance is GITWaypoint) _detailTypeLabel.Text = "Waypoint";
                else if (_selectedInstance is GITSound) _detailTypeLabel.Text = "Sound";
                else if (_selectedInstance is GITStore) _detailTypeLabel.Text = "Store";
                else if (_selectedInstance is GITEncounter) _detailTypeLabel.Text = "Encounter";
                else if (_selectedInstance is GITCamera) _detailTypeLabel.Text = "Camera";
                else _detailTypeLabel.Text = "Instance";
            }

            if (_selectedInstance is GITCreature c1) { _detailResRef.Text = c1.ResRef.ToString(); SetPos(c1.Position); _detailBearing.Value = (decimal)c1.Bearing; _detailTag.Text = ""; }
            else if (_selectedInstance is GITDoor d) { _detailResRef.Text = d.ResRef.ToString(); SetPos(d.Position); _detailBearing.Value = (decimal)d.Bearing; _detailTag.Text = d.Tag ?? ""; }
            else if (_selectedInstance is GITPlaceable p) { _detailResRef.Text = p.ResRef.ToString(); SetPos(p.Position); _detailBearing.Value = (decimal)p.Bearing; _detailTag.Text = p.Tag ?? ""; }
            else if (_selectedInstance is GITTrigger t) { _detailResRef.Text = t.ResRef.ToString(); SetPos(t.Position); _detailTag.Text = t.Tag ?? ""; }
            else if (_selectedInstance is GITWaypoint w) { _detailResRef.Text = w.ResRef.ToString(); SetPos(w.Position); _detailBearing.Value = (decimal)w.Bearing; _detailTag.Text = w.Tag ?? ""; }
            else if (_selectedInstance is GITSound s) { _detailResRef.Text = s.ResRef.ToString(); SetPos(s.Position); _detailTag.Text = s.Tag ?? ""; }
            else if (_selectedInstance is GITStore st) { _detailResRef.Text = st.ResRef.ToString(); SetPos(st.Position); _detailBearing.Value = (decimal)st.Bearing; _detailTag.Text = ""; }
            else if (_selectedInstance is GITEncounter e) { _detailResRef.Text = e.ResRef.ToString(); SetPos(e.Position); _detailTag.Text = ""; }
            else if (_selectedInstance is GITCamera cam) { _detailResRef.Text = cam.ResRef.ToString(); SetPos(cam.Position); _detailTag.Text = ""; }
            _syncingDetails = false;
        }

        private void SetPos(System.Numerics.Vector3 v)
        {
            if (_detailPosX != null) _detailPosX.Value = (decimal)v.X;
            if (_detailPosY != null) _detailPosY.Value = (decimal)v.Y;
            if (_detailPosZ != null) _detailPosZ.Value = (decimal)v.Z;
        }

        private void SaveDetailToInstance()
        {
            if (_syncingDetails) return;
            if (_selectedInstance == null) return;
            try
            {
                var snapshot = CloneGit(_git);
                var beforeResRef = GetResRef(_selectedInstance)?.ToString() ?? string.Empty;
                var beforePosition = GITRenderArea.GetPosition(_selectedInstance);
                var beforeBearing = GetBearingForDetails(_selectedInstance);
                var beforeTag = GetTag(_selectedInstance);
                var resref = ResRefFromEditableText(_detailResRef?.Text);
                var pos = new System.Numerics.Vector3(
                    (float)(_detailPosX?.Value ?? 0),
                    (float)(_detailPosY?.Value ?? 0),
                    (float)(_detailPosZ?.Value ?? 0));

                if (_selectedInstance is GITCreature c1) { c1.ResRef = resref; c1.Position = pos; c1.Bearing = (float)(_detailBearing?.Value ?? 0); }
                else if (_selectedInstance is GITDoor d) { d.ResRef = resref; d.Position = pos; d.Bearing = (float)(_detailBearing?.Value ?? 0); d.Tag = _detailTag?.Text ?? ""; }
                else if (_selectedInstance is GITPlaceable p) { p.ResRef = resref; p.Position = pos; p.Bearing = (float)(_detailBearing?.Value ?? 0); p.Tag = _detailTag?.Text ?? ""; }
                else if (_selectedInstance is GITTrigger t) { t.ResRef = resref; t.Position = pos; t.Tag = _detailTag?.Text ?? ""; }
                else if (_selectedInstance is GITWaypoint w) { w.ResRef = resref; w.Position = pos; w.Bearing = (float)(_detailBearing?.Value ?? 0); w.Tag = _detailTag?.Text ?? ""; }
                else if (_selectedInstance is GITSound s) { s.ResRef = resref; s.Position = pos; s.Tag = _detailTag?.Text ?? ""; }
                else if (_selectedInstance is GITStore st) { st.ResRef = resref; st.Position = pos; st.Bearing = (float)(_detailBearing?.Value ?? 0); }
                else if (_selectedInstance is GITEncounter e) { e.ResRef = resref; e.Position = pos; }
                else if (_selectedInstance is GITCamera cam) { cam.ResRef = resref; cam.Position = pos; }
                _renderArea?.InvalidateVisual();
                RebuildInstanceList();
                var afterResRef = GetResRef(_selectedInstance)?.ToString() ?? string.Empty;
                var afterPosition = GITRenderArea.GetPosition(_selectedInstance);
                var afterBearing = GetBearingForDetails(_selectedInstance);
                var afterTag = GetTag(_selectedInstance);
                if (beforePosition != afterPosition ||
                    !string.Equals(beforeResRef, afterResRef, StringComparison.Ordinal) ||
                    !NullableFloatEquals(beforeBearing, afterBearing) ||
                    !string.Equals(beforeTag, afterTag, StringComparison.Ordinal))
                {
                    PushUndo(snapshot);
                    MarkDocumentDirty();
                }
            }
            catch { }
        }

        private static ResRef GetResRef(object instance)
        {
            if (instance is GITCreature c) return c.ResRef;
            if (instance is GITDoor d) return d.ResRef;
            if (instance is GITPlaceable p) return p.ResRef;
            if (instance is GITTrigger t) return t.ResRef;
            if (instance is GITWaypoint w) return w.ResRef;
            if (instance is GITSound s) return s.ResRef;
            if (instance is GITStore st) return st.ResRef;
            if (instance is GITEncounter e) return e.ResRef;
            if (instance is GITCamera cam) return cam.ResRef;
            return null;
        }

        internal static ResRef ResRefFromEditableText(string text)
        {
            string value = text?.Trim() ?? string.Empty;
            return string.IsNullOrEmpty(value) || !ResRef.IsValid(value) ? ResRef.FromBlank() : new ResRef(value);
        }

        private static string GetTag(object instance)
        {
            if (instance is GITDoor d) return d.Tag ?? string.Empty;
            if (instance is GITPlaceable p) return p.Tag ?? string.Empty;
            if (instance is GITTrigger t) return t.Tag ?? string.Empty;
            if (instance is GITWaypoint w) return w.Tag ?? string.Empty;
            if (instance is GITSound s) return s.Tag ?? string.Empty;
            return string.Empty;
        }

        private static float? GetBearingForDetails(object instance)
        {
            if (instance is GITCreature c) return c.Bearing;
            if (instance is GITDoor d) return d.Bearing;
            if (instance is GITPlaceable p) return p.Bearing;
            if (instance is GITWaypoint w) return w.Bearing;
            if (instance is GITStore st) return st.Bearing;
            return null;
        }

        private static bool NullableFloatEquals(float? left, float? right)
        {
            if (!left.HasValue || !right.HasValue) return left.HasValue == right.HasValue;
            return Math.Abs(left.Value - right.Value) < 0.0001f;
        }

        private void RemoveSelectedInstance()
        {
            if (_selectedInstance == null || _git == null) return;
            try
            {
                PushUndo();
                _git.Remove(_selectedInstance);
                _selectedInstance = null;
                _renderArea?.SelectInstance(null);
                RebuildInstanceList();
                LoadDetailFromInstance();
                if (_detailNoSelection != null) _detailNoSelection.IsVisible = true;
                if (_detailInstance != null) _detailInstance.IsVisible = false;
                if (_duplicateInstanceButton != null) _duplicateInstanceButton.IsEnabled = false;
                if (_removeInstanceButton != null) _removeInstanceButton.IsEnabled = false;
                _renderArea?.InvalidateVisual();
                MarkDocumentDirty();
                UpdateStatusBar();
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex); }
        }

        private void AddSelectedTypeInstance()
        {
            string typeName = _addInstanceType?.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(typeName))
            {
                typeName = "Creature";
            }

            AddNewInstance(typeName);
        }

        private object AddNewInstance(string typeName)
        {
            if (_git == null)
            {
                _git = new GIT();
            }

            var instance = CreateDefaultInstance(typeName);
            if (instance == null)
            {
                return null;
            }

            PushUndo();
            _git.Add(instance);
            _renderArea?.SetGit(_git);
            RebuildInstanceList();
            SelectInstance(instance, syncList: true);
            _renderArea?.InvalidateVisual();
            MarkDocumentDirty();
            UpdateStatusBar();
            return instance;
        }

        private static object CreateDefaultInstance(string typeName)
        {
            switch (typeName)
            {
                case "Creature":
                    return new GITCreature
                    {
                        ResRef = new ResRef("new_creature"),
                        Position = Vector3.Zero,
                        Bearing = 0f
                    };
                case "Door":
                    return new GITDoor
                    {
                        ResRef = new ResRef("new_door"),
                        Tag = "new_door",
                        Position = Vector3.Zero,
                        Bearing = 0f,
                        LinkedTo = string.Empty,
                        LinkedToModule = ResRef.FromBlank(),
                        TransitionDestination = LocalizedString.FromInvalid()
                    };
                case "Placeable":
                    return new GITPlaceable
                    {
                        ResRef = new ResRef("new_placeable"),
                        Tag = "new_placeable",
                        Position = Vector3.Zero,
                        Bearing = 0f
                    };
                case "Trigger":
                    return new GITTrigger
                    {
                        ResRef = new ResRef("new_trigger"),
                        Tag = "new_trigger",
                        Position = Vector3.Zero,
                        Geometry = DefaultVolumeGeometry(Vector3.Zero),
                        LinkedTo = string.Empty,
                        LinkedToModule = ResRef.FromBlank(),
                        TransitionDestination = LocalizedString.FromInvalid()
                    };
                case "Waypoint":
                    return new GITWaypoint
                    {
                        ResRef = new ResRef("new_waypoint"),
                        Tag = "new_waypoint",
                        Position = Vector3.Zero,
                        Bearing = 0f,
                        Name = LocalizedString.FromEnglish("New Waypoint"),
                        MapNote = null
                    };
                case "Sound":
                    return new GITSound
                    {
                        ResRef = new ResRef("new_sound"),
                        Tag = "new_sound",
                        Position = Vector3.Zero
                    };
                case "Store":
                    return new GITStore
                    {
                        ResRef = new ResRef("new_store"),
                        Position = Vector3.Zero,
                        Bearing = 0f
                    };
                case "Encounter":
                    return new GITEncounter
                    {
                        ResRef = new ResRef("new_encounter"),
                        Position = Vector3.Zero,
                        Geometry = DefaultVolumeGeometry(Vector3.Zero)
                    };
                case "Camera":
                    return new GITCamera
                    {
                        CameraId = 1,
                        ResRef = new ResRef("new_camera"),
                        Fov = 55f,
                        Orientation = new Vector4(0, 0, 0, 1),
                        Position = Vector3.Zero
                    };
                default:
                    return null;
            }
        }

        private static List<Vector3> DefaultVolumeGeometry(Vector3 origin)
        {
            return new List<Vector3>
            {
                origin,
                new Vector3(origin.X + 3f, origin.Y, origin.Z),
                new Vector3(origin.X + 3f, origin.Y + 3f, origin.Z)
            };
        }

        private void DuplicateSelectedInstance()
        {
            if (_selectedInstance == null || _git == null) return;
            try
            {
                var duplicate = CloneInstance(_selectedInstance);
                if (duplicate == null) return;

                PushUndo();
                OffsetInstanceForDuplicate(duplicate);
                _git.Add(duplicate);
                _renderArea?.SetGit(_git);
                RebuildInstanceList();
                SelectInstance(duplicate, syncList: true);
                _renderArea?.InvalidateVisual();
                MarkDocumentDirty();
                UpdateStatusBar();
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex); }
        }

        private static void OffsetInstanceForDuplicate(object instance)
        {
            var position = GITRenderArea.GetPosition(instance);
            GITRenderArea.SetPosition(instance, new System.Numerics.Vector3(position.X + 1f, position.Y + 1f, position.Z));
        }

        private object CloneInstance(object instance)
        {
            if (instance is GITCreature creature)
            {
                return new GITCreature
                {
                    ResRef = creature.ResRef,
                    Position = creature.Position,
                    Bearing = creature.Bearing
                };
            }
            if (instance is GITDoor door)
            {
                return new GITDoor
                {
                    ResRef = door.ResRef,
                    Bearing = door.Bearing,
                    TweakColor = door.TweakColor,
                    Tag = door.Tag ?? string.Empty,
                    LinkedTo = door.LinkedTo ?? string.Empty,
                    LinkedToFlags = door.LinkedToFlags,
                    LinkedToModule = door.LinkedToModule,
                    TransitionDestination = door.TransitionDestination,
                    Position = door.Position
                };
            }
            if (instance is GITPlaceable placeable)
            {
                return new GITPlaceable
                {
                    ResRef = placeable.ResRef,
                    Position = placeable.Position,
                    Bearing = placeable.Bearing,
                    TweakColor = placeable.TweakColor,
                    Tag = placeable.Tag ?? string.Empty
                };
            }
            if (instance is GITTrigger trigger)
            {
                return new GITTrigger
                {
                    ResRef = trigger.ResRef,
                    Position = trigger.Position,
                    Geometry = new List<System.Numerics.Vector3>(trigger.Geometry ?? new List<System.Numerics.Vector3>()),
                    Tag = trigger.Tag ?? string.Empty,
                    LinkedTo = trigger.LinkedTo ?? string.Empty,
                    LinkedToFlags = trigger.LinkedToFlags,
                    LinkedToModule = trigger.LinkedToModule,
                    TransitionDestination = trigger.TransitionDestination
                };
            }
            if (instance is GITWaypoint waypoint)
            {
                return new GITWaypoint
                {
                    ResRef = waypoint.ResRef,
                    Position = waypoint.Position,
                    Tag = waypoint.Tag ?? string.Empty,
                    Name = waypoint.Name,
                    MapNote = waypoint.MapNote,
                    MapNoteEnabled = waypoint.MapNoteEnabled,
                    HasMapNote = waypoint.HasMapNote,
                    Bearing = waypoint.Bearing
                };
            }
            if (instance is GITSound sound)
            {
                return new GITSound
                {
                    ResRef = sound.ResRef,
                    Position = sound.Position,
                    Tag = sound.Tag ?? string.Empty
                };
            }
            if (instance is GITStore store)
            {
                return new GITStore
                {
                    ResRef = store.ResRef,
                    Position = store.Position,
                    Bearing = store.Bearing
                };
            }
            if (instance is GITEncounter encounter)
            {
                return new GITEncounter
                {
                    ResRef = encounter.ResRef,
                    Position = encounter.Position,
                    Geometry = new List<System.Numerics.Vector3>(encounter.Geometry ?? new List<System.Numerics.Vector3>()),
                    SpawnPoints = new List<GITEncounterSpawnPoint>(
                        (encounter.SpawnPoints ?? new List<GITEncounterSpawnPoint>()).Select(spawn => new GITEncounterSpawnPoint
                        {
                            X = spawn.X,
                            Y = spawn.Y,
                            Z = spawn.Z,
                            Orientation = spawn.Orientation
                        }))
                };
            }
            if (instance is GITCamera camera)
            {
                return new GITCamera
                {
                    CameraId = _git?.NextCameraId() ?? camera.CameraId + 1,
                    Fov = camera.Fov,
                    Height = camera.Height,
                    MicRange = camera.MicRange,
                    Orientation = camera.Orientation,
                    Position = camera.Position,
                    Pitch = camera.Pitch,
                    ResRef = camera.ResRef
                };
            }

            return null;
        }

        private void PushUndo(GIT snapshot = null)
        {
            if (_git == null) return;
            _undoStack.Add(snapshot ?? CloneGit(_git));
            if (_undoStack.Count > UndoMaxLevels)
            {
                _undoStack.RemoveAt(0);
            }
            _redoStack.Clear();
        }

        private void UndoGitEdit()
        {
            if (_undoStack.Count == 0) return;
            _redoStack.Add(CloneGit(_git));
            var previous = _undoStack[_undoStack.Count - 1];
            _undoStack.RemoveAt(_undoStack.Count - 1);
            ApplyGitState(previous);
            MarkDocumentDirty();
        }

        private void RedoGitEdit()
        {
            if (_redoStack.Count == 0) return;
            _undoStack.Add(CloneGit(_git));
            var next = _redoStack[_redoStack.Count - 1];
            _redoStack.RemoveAt(_redoStack.Count - 1);
            ApplyGitState(next);
            MarkDocumentDirty();
        }

        private void ApplyGitState(GIT state)
        {
            _git = CloneGit(state);
            _selectedInstance = null;
            _renderArea?.SetGit(_git);
            _renderArea?.SelectInstance(null);
            RebuildInstanceList();
            LoadDetailFromInstance();
            if (_detailNoSelection != null) _detailNoSelection.IsVisible = true;
            if (_detailInstance != null) _detailInstance.IsVisible = false;
            if (_duplicateInstanceButton != null) _duplicateInstanceButton.IsEnabled = false;
            if (_removeInstanceButton != null) _removeInstanceButton.IsEnabled = false;
            _renderArea?.InvalidateVisual();
            UpdateStatusBar();
        }

        private static GIT CloneGit(GIT source)
        {
            if (source == null) return new GIT();
            return GITHelpers.ConstructGit(GITHelpers.DismantleGit(source, Game.K2));
        }

        private void UpdateStatusBar()
        {
            if (_statusText == null) return;
            int total = _instanceItems?.Count ?? 0;
            if (_git != null)
                total = (_git.Creatures?.Count ?? 0) + (_git.Doors?.Count ?? 0) + (_git.Placeables?.Count ?? 0) + (_git.Triggers?.Count ?? 0) + (_git.Waypoints?.Count ?? 0) + (_git.Sounds?.Count ?? 0) + (_git.Stores?.Count ?? 0) + (_git.Encounters?.Count ?? 0) + (_git.Cameras?.Count ?? 0);
            string text = _git != null ? $"{total} instance(s)" : "No GIT";
            if (_selectedInstance != null)
                text += " | 1 selected";
            if (_renderArea != null)
                text += $" | {_renderArea.VisibleInstanceCount()} visible";
            if (!string.IsNullOrEmpty(_blenderStatus))
                text += $" | Blender: {_blenderStatus}";
            _statusText.Text = text;
        }

        public override void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            try
            {
                _originalGff = GFFAuto.ReadGff(_revert, fileFormat: _restype ?? ResourceType.GIT);
                _git = GITHelpers.ConstructGit(_originalGff);
                LoadGIT(_git);
            }
            catch (Exception ex) { System.Console.WriteLine(ex); }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            // GIT is a GFF-based format - store original GFF to preserve unmodified fields
            _originalGff = data != null && data.Length > 0
                ? GFFAuto.ReadGff(data, fileFormat: restype)
                : null;
            _git = _originalGff != null ? GITHelpers.ConstructGit(_originalGff) : new GIT();
            LoadGIT(_git);
            _blenderStatus = null;
            RefreshBlenderActionState();
        }

        private void LoadGIT(GIT git)
        {
            // Load GIT data into UI
            _git = git;
            _selectedInstance = null;
            _undoStack.Clear();
            _redoStack.Clear();
            _renderArea?.SetGit(_git);
            _renderArea?.SelectInstance(null);
            RebuildInstanceList();
            LoadDetailFromInstance();
            if (_detailNoSelection != null) _detailNoSelection.IsVisible = true;
            if (_detailInstance != null) _detailInstance.IsVisible = false;
            if (_duplicateInstanceButton != null) _duplicateInstanceButton.IsEnabled = false;
            if (_removeInstanceButton != null) _removeInstanceButton.IsEnabled = false;
            UpdateStatusBar();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            SaveDetailToInstance();
            Game gameToUse = _installation?.Game ?? Game.K2;
            var gff = GITHelpers.DismantleGit(_git, gameToUse);

            // Preserve unmodified fields from original GFF that aren't yet supported by GIT object model
            // This ensures roundtrip tests pass by maintaining all original data
            if (_originalGff != null)
            {
                var originalRoot = _originalGff.Root;
                var newRoot = gff.Root;

                // List of fields that GITHelpers.DismantleGit explicitly sets
                var fieldsSetByDismantle = new System.Collections.Generic.HashSet<string>
                {
                    "UseTemplates",
                    "AreaProperties",
                    "CameraList",
                    "Creature List",
                    "Door List",
                    "Encounter List",
                    "Placeable List",
                    "SoundList",
                    "StoreList",
                    "TriggerList",
                    "WaypointList"
                };

                // Copy all fields from original that aren't explicitly set by DismantleGit
                foreach (var (label, fieldType, value) in originalRoot)
                {
                    if (!fieldsSetByDismantle.Contains(label) && !newRoot.Exists(label))
                    {
                        CopyGffField(originalRoot, newRoot, label, fieldType);
                    }
                }
            }

            ResourceType outputType = _restype == ResourceType.GIT_XML ? ResourceType.GIT_XML : ResourceType.GIT;
            byte[] data = GFFAuto.BytesGff(gff, outputType);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _git = new GIT();
            _originalGff = null; // Clear original GFF when creating new file
            LoadGIT(_git);
            RefreshBlenderActionState();
        }

        internal void SetBlenderServicesForTests(
            Func<string, BlenderInfo> detectBlender,
            Func<BlenderInfo, int, string, string, string, bool, System.Diagnostics.Process> launchBlender)
        {
            _detectBlender = detectBlender ?? BlenderDetection.DetectBlender;
            _launchBlender = launchBlender ?? BlenderDetection.LaunchBlenderWithIpc;
        }

        // Helper method to copy a GFF field from one struct to another, preserving type
        private static void CopyGffField(GFFStruct source, GFFStruct destination, string label, GFFFieldType fieldType)
        {
            switch (fieldType)
            {
                case GFFFieldType.UInt8:
                    destination.SetUInt8(label, source.GetUInt8(label));
                    break;
                case GFFFieldType.Int8:
                    destination.SetInt8(label, source.GetInt8(label));
                    break;
                case GFFFieldType.UInt16:
                    destination.SetUInt16(label, source.GetUInt16(label));
                    break;
                case GFFFieldType.Int16:
                    destination.SetInt16(label, source.GetInt16(label));
                    break;
                case GFFFieldType.UInt32:
                    destination.SetUInt32(label, source.GetUInt32(label));
                    break;
                case GFFFieldType.Int32:
                    destination.SetInt32(label, source.GetInt32(label));
                    break;
                case GFFFieldType.UInt64:
                    destination.SetUInt64(label, source.GetUInt64(label));
                    break;
                case GFFFieldType.Int64:
                    destination.SetInt64(label, source.GetInt64(label));
                    break;
                case GFFFieldType.Single:
                    destination.SetSingle(label, source.GetSingle(label));
                    break;
                case GFFFieldType.Double:
                    destination.SetDouble(label, source.GetDouble(label));
                    break;
                case GFFFieldType.String:
                    destination.SetString(label, source.GetString(label));
                    break;
                case GFFFieldType.ResRef:
                    destination.SetResRef(label, source.GetResRef(label));
                    break;
                case GFFFieldType.LocalizedString:
                    destination.SetLocString(label, source.GetLocString(label));
                    break;
                case GFFFieldType.Binary:
                    destination.SetBinary(label, source.GetBinary(label));
                    break;
                case GFFFieldType.Vector3:
                    destination.SetVector3(label, source.GetVector3(label));
                    break;
                case GFFFieldType.Vector4:
                    destination.SetVector4(label, source.GetVector4(label));
                    break;
                case GFFFieldType.Struct:
                    destination.SetStruct(label, source.GetStruct(label));
                    break;
                case GFFFieldType.List:
                    destination.SetList(label, source.GetList(label));
                    break;
            }
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        internal int InstanceCount => _git?.Instances().Count ?? 0;

        internal int VisibleInstanceCount => _renderArea?.VisibleInstanceCount() ?? 0;

        internal object SelectedInstanceForTest => _selectedInstance;

        internal void AddInstanceForTest(object instance)
        {
            _git.Add(instance);
            _renderArea?.SetGit(_git);
            RebuildInstanceList();
        }

        internal object AddNewInstanceForTest(string typeName)
        {
            return AddNewInstance(typeName);
        }

        internal void SelectInstanceForTest(object instance)
        {
            SelectInstance(instance, syncList: true);
        }

        internal void SetInstanceTypeVisibleForTest(string typeName, bool visible)
        {
            _renderArea?.SetTypeVisible(typeName, visible);
            ApplyFilter();
            UpdateStatusBar();
        }

        internal void MoveSelectedInstanceForTest(float x, float y)
        {
            if (_selectedInstance == null) return;
            PushUndo();
            GITRenderArea.MoveInstanceBy(_selectedInstance, x, y);
            _renderArea?.InvalidateVisual();
            LoadDetailFromInstance();
            RebuildInstanceList();
            MarkDocumentDirty();
        }

        internal void DuplicateSelectedInstanceForTest()
        {
            DuplicateSelectedInstance();
        }

        internal void UndoGitEditForTest()
        {
            UndoGitEdit();
        }

        internal void RedoGitEditForTest()
        {
            RedoGitEdit();
        }

        internal void CommitSelectedInstanceDetailsForTest()
        {
            SaveDetailToInstance();
        }
    }
}
