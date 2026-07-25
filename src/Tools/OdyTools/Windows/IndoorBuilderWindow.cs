using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OdyTools.Blender;
using OdyTools.Data;
using OdyTools.Dialogs;
using DuplicateRoomsCommand = OdyTools.Windows.DuplicateRoomsCommand;

namespace OdyTools.Windows
{
    public class IndoorBuilderWindow : Window
    {
        private OdyInstallation _installation;
        private string _filepath;
        private List<Kit> _kits = new List<Kit>();
        private Func<string, BlenderInfo> _detectBlender = BlenderDetection.DetectBlender;
        private Func<BlenderInfo, int, string, string, string, bool, System.Diagnostics.Process> _launchBlender = BlenderDetection.LaunchBlenderWithIpc;


        public IndoorBuilderWindow(Window parent = null, OdyInstallation installation = null)
        {
            InitializeComponent();
            _installation = installation;

            if (installation != null)
            {
                ModuleKitManager = new ModuleKitManager(installation);
            }
            else
            {
                ModuleKitManager = null;
            }

            LoadKitsFromDefaultPath();

            SetupUI();

            // Disable ActionSettings when no installation is provided (matching Python test expectation)
            if (Ui != null)
            {
                Ui.ActionSettingsEnabled = (_installation != null);
            }
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
            Title = "Indoor Builder";
            Width = 1200;
            Height = 800;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Indoor Builder",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            panel.Children.Add(titleLabel);
            Content = panel;
        }

        public IndoorBuilderWindowUi Ui { get; private set; }

        public IndoorMap Map { get; private set; }

        public UndoStack UndoStack { get; private set; }

        // Intentionally hides base Clipboard property (IClipboard? - system clipboard)
        // to provide domain-specific room clipboard (List<RoomClipboardData>)
        public new List<RoomClipboardData> Clipboard { get; private set; }

        // Module kit management (lazy loading) - handles converting game modules to kit-like components
        public ModuleKitManager ModuleKitManager { get; private set; }

        private void SetupUI()
        {
            // Create UI wrapper for testing
            Ui = new IndoorBuilderWindowUi();

            // Initialize map (matching Python: self._map = IndoorMap())
            Map = new IndoorMap();

            // Initialize undo stack (matching Python: self._undo_stack: QUndoStack = QUndoStack(self))
            UndoStack = new UndoStack();

            // Initialize clipboard (matching Python: self._clipboard: list[RoomClipboardData] = [])
            Clipboard = new List<RoomClipboardData>();

            // Initialize MapRenderer (matching Python: self.ui.mapRenderer)
            Ui.MapRenderer = new IndoorMapRenderer();
            Ui.MapRenderer.SetMap(Map);
            // Matching Python: self.ui.mapRenderer.set_undo_stack(self._undo_stack)
            Ui.MapRenderer.SetUndoStack(UndoStack);
            // Matching Python: self.ui.mapRenderer.set_status_callback(self._refresh_status_bar)
            Ui.MapRenderer.SetStatusCallback(RefreshStatusBar);

            // Setup select all action (matching Python: self.ui.actionSelectAll.triggered.connect(self.select_all))
            Ui.ActionSelectAll = SelectAll;

            // Setup deselect all action (matching Python: self.ui.actionDeselectAll.triggered.connect(self.deselect_all))
            Ui.ActionDeselectAll = DeselectAll;

            // Setup delete selected action (matching Python: self.ui.actionDeleteSelected.triggered.connect(self.delete_selected))
            Ui.ActionDeleteSelected = DeleteSelected;

            // Setup duplicate action (matching Python: self.ui.actionDuplicate.triggered.connect(self.duplicate_selected))
            Ui.ActionDuplicate = DuplicateSelected;

            // Setup undo/redo actions (matching Python lines 690-703)
            // Matching Python: self.ui.actionUndo.triggered.connect(self._undo_stack.undo)
            Ui.ActionUndo = () => UndoStack.Undo();
            // Matching Python: self.ui.actionRedo.triggered.connect(self._undo_stack.redo)
            Ui.ActionRedo = () => UndoStack.Redo();

            // Matching Python: self._undo_stack.canUndoChanged.connect(self.ui.actionUndo.setEnabled)
            UndoStack.CanUndoChanged += (sender, canUndo) => Ui.ActionUndoEnabled = canUndo;
            // Matching Python: self._undo_stack.canRedoChanged.connect(self.ui.actionRedo.setEnabled)
            UndoStack.CanRedoChanged += (sender, canRedo) => Ui.ActionRedoEnabled = canRedo;

            // Matching Python lines 702-703: self.ui.actionUndo.setEnabled(False); self.ui.actionRedo.setEnabled(False)
            Ui.ActionUndoEnabled = false;
            Ui.ActionRedoEnabled = false;

            // Matching Python line 609: self.ui.actionZoomIn.triggered.connect(lambda: self.ui.mapRenderer.zoom_in_camera(ZOOM_STEP))
            Ui.ActionZoomIn = () => Ui.MapRenderer.ZoomInCamera(0.2f); // ZOOM_STEP = 0.2

            // Matching Python line 610: self.ui.actionZoomOut.triggered.connect(lambda: self.ui.mapRenderer.zoom_in_camera(-ZOOM_STEP))
            Ui.ActionZoomOut = () => Ui.MapRenderer.ZoomInCamera(-0.2f); // ZOOM_STEP = 0.2

            // Setup spinbox bindings for grid size and rotation snap
            Ui.GridSizeSpinValueChanged = (value) => Ui.MapRenderer.SetGridSize((float)value);
            Ui.RotSnapSpinValueChanged = (value) => Ui.MapRenderer.SetRotationSnap((float)value);
            // Setup checkbox binding for snap to hooks
            Ui.SnapToHooksCheckToggled = (value) => Ui.MapRenderer.SetSnapToHooks(value);

            InitializeOptionsUI();

            SetupModules();

            // Wire up module selection event
            Ui.ModuleSelect.SetCurrentIndexChangedHandler(OnModuleSelected);

            // Wire up module component selection event
            // Module component selection (moduleComponentList UI)

            Ui.ActionSettings = OpenSettings;
            Ui.ActionSave = Save;
            Ui.ActionSaveAs = SaveAs;
            Ui.ActionOpen = Open;
            Ui.ActionOpenMod = OpenMod;
            Ui.ActionBuild = BuildMapFromUi;
            Ui.ActionOpenInBlender = () => TryLaunchBlenderForCurrentMap();
            RefreshBlenderActionState();
        }

        private void LoadKitsFromDefaultPath()
        {
            try
            {
                string kitsPath = Path.Combine(Directory.GetCurrentDirectory(), "kits");
                if (Directory.Exists(kitsPath))
                {
                    _kits = KitLoader.LoadKits(kitsPath);
                }
            }
            catch
            {
                _kits = new List<Kit>();
            }
        }

        /// <summary>Test hook to supply kits without filesystem layout.</summary>
        public void SetKitsForTesting(List<Kit> kits)
        {
            _kits = kits ?? new List<Kit>();
        }

        public string FilePath => _filepath;

        public bool BuildMap(string outputPath)
        {
            if (_installation == null)
            {
                Ui.LastErrorMessage = "No installation selected. Select an installation before building a module.";
                return false;
            }
            if (string.IsNullOrWhiteSpace(outputPath))
            {
                Ui.LastErrorMessage = "Output path is required.";
                return false;
            }
            if (Map == null || Map.Rooms.Count == 0)
            {
                Ui.LastErrorMessage = "Add at least one room before building.";
                return false;
            }

            try
            {
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                Map.Build(_installation, _kits, outputPath);
                Ui.LastErrorMessage = null;
                _filepath = outputPath;
                RefreshWindowTitle();
                SetBlenderStatus(string.Empty);
                RefreshBlenderActionState();
                return File.Exists(outputPath);
            }
            catch (Exception ex)
            {
                Ui.LastErrorMessage = ex.Message;
                return false;
            }
        }

        private void BuildMapFromUi()
        {
            if (_installation == null)
            {
                Ui.LastErrorMessage = "No installation selected. Select an installation before building a module.";
                return;
            }

            string outputPath = Path.Combine(_installation.ModulePath(), Map.ModuleId + ".mod");
            BuildMap(outputPath);
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                SaveAs();
                return;
            }

            SaveMapToPath(_filepath);
        }

        public void SaveAs()
        {
            if (!string.IsNullOrEmpty(Ui.SaveAsPathOverride))
            {
                SaveMapToPath(Ui.SaveAsPathOverride);
                _filepath = Ui.SaveAsPathOverride;
                RefreshWindowTitle();
                return;
            }

            Ui.LastErrorMessage = "Save path not configured. Use SaveMapToPath for programmatic saves.";
        }

        public void SaveMapToPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("Path is required.", nameof(path));
            }

            byte[] data = Map.Write();
            File.WriteAllBytes(path, data);
            _filepath = path;
            UndoStack.SetClean();
            RefreshWindowTitle();
            SetBlenderStatus(string.Empty);
            RefreshBlenderActionState();
        }

        public bool OpenFromPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            {
                Ui.LastErrorMessage = "Indoor map file not found.";
                return false;
            }

            try
            {
                byte[] raw = File.ReadAllBytes(path);
                List<MissingRoomInfo> missing = Map.Load(raw, _kits);
                Map.RebuildRoomConnections();
                Ui.MapRenderer.SetMap(Map);
                _filepath = path;
                UndoStack.Clear();
                UndoStack.SetClean();
                RefreshWindowTitle();
                SetBlenderStatus(string.Empty);
                RefreshBlenderActionState();
                Ui.LastMissingRooms = missing;
                Ui.LastErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                Ui.LastErrorMessage = ex.Message;
                return false;
            }
        }

        public void Open()
        {
            if (!string.IsNullOrEmpty(Ui.OpenPathOverride))
            {
                OpenFromPath(Ui.OpenPathOverride);
            }
        }

        public bool OpenModFromPath(string modPath)
        {
            if (string.IsNullOrWhiteSpace(modPath) || !File.Exists(modPath))
            {
                Ui.LastErrorMessage = "Module file not found.";
                return false;
            }

            byte[] embedded = BioWare.Tools.IndoorMapIo.TryExtractEmbeddedIndoorJsonFromModuleFiles(new[] { modPath });
            if (embedded == null || embedded.Length == 0)
            {
                Ui.LastErrorMessage = "No embedded indoormap.txt found in module.";
                return false;
            }

            try
            {
                List<MissingRoomInfo> missing = Map.Load(embedded, _kits);
                Map.RebuildRoomConnections();
                Ui.MapRenderer.SetMap(Map);
                _filepath = modPath;
                UndoStack.Clear();
                UndoStack.SetClean();
                RefreshWindowTitle();
                SetBlenderStatus(string.Empty);
                RefreshBlenderActionState();
                Ui.LastMissingRooms = missing;
                Ui.LastErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                Ui.LastErrorMessage = ex.Message;
                return false;
            }
        }

        public bool TryLaunchBlenderForCurrentMap()
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                SetBlenderStatus("Save or open an indoor map before launching Blender.");
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

            var process = _launchBlender(blenderInfo, 7531, _installation?.Path, _filepath, null, false);
            if (process == null)
            {
                SetBlenderStatus("Failed to launch Blender.");
                return false;
            }

            SetBlenderStatus($"Launched Blender for {Path.GetFileName(_filepath)}.");
            return true;
        }

        private void RefreshBlenderActionState()
        {
            if (Ui == null)
            {
                return;
            }

            Ui.ActionOpenInBlenderEnabled = !string.IsNullOrEmpty(_filepath);
            if (string.IsNullOrEmpty(_filepath))
            {
                SetBlenderStatus("Save or open an indoor map to use Blender.");
            }
        }

        private void SetBlenderStatus(string status)
        {
            if (Ui != null)
            {
                Ui.BlenderStatusText = status ?? string.Empty;
            }
        }

        internal void SetBlenderServicesForTests(
            Func<string, BlenderInfo> detectBlender,
            Func<BlenderInfo, int, string, string, string, bool, System.Diagnostics.Process> launchBlender)
        {
            _detectBlender = detectBlender ?? BlenderDetection.DetectBlender;
            _launchBlender = launchBlender ?? BlenderDetection.LaunchBlenderWithIpc;
        }

        public void OpenMod()
        {
            if (!string.IsNullOrEmpty(Ui.OpenModPathOverride))
            {
                OpenModFromPath(Ui.OpenModPathOverride);
            }
        }

        private void OpenSettings()
        {
            if (_installation == null)
            {
                return;
            }

            var dialog = new IndoorMapSettingsDialog(this, _installation, Map, _kits);
            dialog.Show();
        }

        private void RefreshWindowTitle()
        {
            string name = string.IsNullOrEmpty(_filepath) ? "Untitled" : Path.GetFileName(_filepath);
            Title = "Indoor Builder - " + name;
        }

        //     """Initialize Options UI to match renderer's initial state."""
        private void InitializeOptionsUI()
        {
            // Matching Python line 1224: renderer = self.ui.mapRenderer
            var renderer = Ui.MapRenderer;

            // Matching Python lines 1226-1231: Block signals temporarily to avoid triggering updates during initialization
            // In Avalonia/C#, we use a flag to prevent event handlers from firing during initialization
            Ui.BlockSpinboxSignals = true;
            Ui.BlockCheckboxSignals = true;

            // Matching Python lines 1234-1239: Set UI to match renderer state
            // Matching Python line 1234: self.ui.snapToGridCheck.setChecked(renderer.snap_to_grid)
            // snapToGridCheck (optional)
            // Matching Python line 1235: self.ui.snapToHooksCheck.setChecked(renderer.snap_to_hooks)
            Ui.SetSnapToHooksCheckChecked(renderer.SnapToHooks);
            // Matching Python line 1238: self.ui.gridSizeSpin.setValue(renderer.grid_size)
            Ui.GridSizeSpinValue = (decimal)renderer.GridSize;
            // Matching Python line 1239: self.ui.rotSnapSpin.setValue(int(renderer.rotation_snap))
            Ui.RotSnapSpinValue = (decimal)renderer.RotationSnap;

            // Matching Python lines 1242-1247: Unblock signals
            Ui.BlockSpinboxSignals = false;
            Ui.BlockCheckboxSignals = false;
        }

        private void SelectAll()
        {
            // Matching Python: self.ui.mapRenderer.select_all_rooms()
            // Original implementation:
            // def select_all(self):
            //     renderer = self.ui.mapRenderer
            //     renderer.clear_selected_rooms()
            //     for room in self._map.rooms:
            //         renderer.select_room(room, clear_existing=False)
            var renderer = Ui.MapRenderer;
            renderer.ClearSelectedRooms();
            foreach (var room in Map.Rooms)
            {
                renderer.SelectRoom(room, clearExisting: false);
            }
        }

        private void DeselectAll()
        {
            // Matching Python: self.ui.mapRenderer.clear_selected_rooms()
            // Original implementation:
            // def deselect_all(self):
            //     self.ui.mapRenderer.clear_selected_rooms()
            //     self.ui.mapRenderer.set_cursor_component(None)
            //     self.ui.componentList.clearSelection()
            //     self.ui.componentList.setCurrentItem(None)
            //     self.ui.moduleComponentList.clearSelection()
            //     self.ui.moduleComponentList.setCurrentItem(None)
            //     self._set_preview_image(None)
            //     self._refresh_status_bar()
            var renderer = Ui.MapRenderer;
            renderer.ClearSelectedRooms();
            // Matching Python line 1758: self.ui.mapRenderer.set_cursor_component(None)
            renderer.SetCursorComponent(null);
            // Note: Additional UI clearing (componentList, moduleComponentList, preview image, status bar)
            // Optional UI components (added when available)
            // Matching Python lines 1759-1764:
            // self.ui.componentList.clearSelection()
            // self.ui.componentList.setCurrentItem(None)
            // self.ui.moduleComponentList.clearSelection()
            // self.ui.moduleComponentList.setCurrentItem(None)
            // self._set_preview_image(None)
            // self._refresh_status_bar()
        }

        private void DeleteSelected()
        {
            // Matching Python implementation:
            // def delete_selected(self):
            //     selected = self.ui.mapRenderer.selected_rooms()
            //     if not selected:
            //         return
            //     cmd = DeleteRoomsCommand(self._map, selected)
            //     self._undo_stack.push(cmd)
            var renderer = Ui.MapRenderer;
            var selected = renderer.SelectedRooms();
            if (selected == null || selected.Count == 0)
            {
                return;
            }

            var cmd = new DeleteRoomsCommand(Map, selected);
            UndoStack.Push(cmd);
        }

        private void DuplicateSelected()
        {
            // Matching Python implementation:
            // def duplicate_selected(self):
            //     rooms: list[IndoorMapRoom] = self.ui.mapRenderer.selected_rooms()
            //     if not rooms:
            //         return
            //     duplicate_cmd = DuplicateRoomsCommand(
            //         self._map,
            //         rooms,
            //         Vector3(DUPLICATE_OFFSET_X, DUPLICATE_OFFSET_Y, DUPLICATE_OFFSET_Z),
            //         self._invalidate_rooms,
            //     )
            //     self._undo_stack.push(duplicate_cmd)
            //     # Select the duplicates
            //     self.ui.mapRenderer.clear_selected_rooms()
            //     for room in duplicate_cmd.duplicates:
            //         self.ui.mapRenderer.select_room(room, clear_existing=False)
            var renderer = Ui.MapRenderer;
            var selected = renderer.SelectedRooms();
            if (selected == null || selected.Count == 0)
            {
                return;
            }

            var duplicateCmd = new DuplicateRoomsCommand(Map, selected);
            UndoStack.Push(duplicateCmd);

            // Select the duplicates (matching Python lines 1669-1671)
            renderer.ClearSelectedRooms();
            foreach (var room in duplicateCmd.Duplicates)
            {
                renderer.SelectRoom(room, clearExisting: false);
            }
        }

        public void ResetView()
        {
            // Matching Python: self.ui.mapRenderer.set_camera_position(DEFAULT_CAMERA_POSITION_X, DEFAULT_CAMERA_POSITION_Y)
            Ui.MapRenderer.SetCameraPosition(0.0f, 0.0f); // DEFAULT_CAMERA_POSITION_X/Y = 0.0
            // Matching Python: self.ui.mapRenderer.set_camera_rotation(DEFAULT_CAMERA_ROTATION)
            Ui.MapRenderer.SetCameraRotation(0.0f); // DEFAULT_CAMERA_ROTATION = 0.0
            // Matching Python: self.ui.mapRenderer.set_camera_zoom(DEFAULT_CAMERA_ZOOM)
            Ui.MapRenderer.SetCameraZoom(1.0f); // DEFAULT_CAMERA_ZOOM = 1.0
        }

        public void CenterOnSelection()
        {
            // Matching Python line 1776: rooms = self.ui.mapRenderer.selected_rooms()
            var rooms = Ui.MapRenderer.SelectedRooms();
            // Matching Python line 1777: if not rooms: return
            if (rooms == null || rooms.Count == 0)
            {
                return;
            }

            // Matching Python lines 1780-1781: Calculate average position
            // cx = sum(r.position.x for r in rooms) / len(rooms)
            // cy = sum(r.position.y for r in rooms) / len(rooms)
            float cx = rooms.Sum(r => r.Position.X) / rooms.Count;
            float cy = rooms.Sum(r => r.Position.Y) / rooms.Count;

            // Matching Python line 1782: self.ui.mapRenderer.set_camera_position(cx, cy)
            Ui.MapRenderer.SetCameraPosition(cx, cy);
        }

        private void RefreshStatusBar(System.Numerics.Vector2? mousePos, HashSet<int> mouseButtons, HashSet<int> keys)
        {
            // Matching Python line 1002: self._update_status_bar(screen, buttons, keys)
            UpdateStatusBar(mousePos, mouseButtons, keys);
        }

        /// <summary>
        /// Rich status bar mirroring Module Designer style.
        /// Updates status bar with mouse position, hover room, selection, keys/buttons, and mode/status.
        /// </summary>
        private void UpdateStatusBar(System.Numerics.Vector2? mousePos, HashSet<int> mouseButtons, HashSet<int> keys)
        {
            var renderer = Ui.MapRenderer;
            if (renderer == null)
            {
                return;
            }

            // Matching Python lines 1013-1021: Resolve screen coords
            System.Numerics.Vector2 screenVec;
            if (mousePos.HasValue)
            {
                screenVec = mousePos.Value;
            }
            else
            {
                // If no mouse position provided, use (0, 0) as default
                // In a full implementation, this would get cursor position from the renderer
                screenVec = new System.Numerics.Vector2(0, 0);
            }

            // Matching Python lines 1023-1039: Resolve buttons/keys - ensure they are sets
            if (mouseButtons == null)
            {
                mouseButtons = new HashSet<int>();
                // In a full implementation, this would get mouse buttons from renderer.mouse_down()
            }
            if (keys == null)
            {
                keys = new HashSet<int>();
                // In a full implementation, this would get keys from renderer.keys_down()
            }

            // Matching Python line 1041: world: Vector3 = renderer.to_world_coords(screen_vec.x, screen_vec.y)
            // When IndoorMapRenderer exposes to_world_coords, use it here; until then use screen-space vector with Z=0
            System.Numerics.Vector3 world = new System.Numerics.Vector3(screenVec.X, screenVec.Y, 0.0f);

            // Matching Python line 1042: hover_room: IndoorMapRoom | None = renderer.room_under_mouse()
            // room_under_mouse: implement in IndoorMapRenderer when needed
            IndoorMapRoom hoverRoom = null; // Set when renderer.room_under_mouse() is available

            // Matching Python line 1043: sel_rooms = renderer.selected_rooms()
            var selRooms = renderer.SelectedRooms();

            // Matching Python line 1044: sel_hook = renderer.selected_hook()
            // selected_hook: implement in IndoorMapRenderer when needed
            Tuple<IndoorMapRoom, int> selHook = null; // Set when renderer.selected_hook() is available

            // Matching Python lines 1046-1052: Mouse/hover
            string hoverText;
            if (hoverRoom != null && hoverRoom.Component != null)
            {
                hoverText = $"<b><span style=\"{EmojiStyle}\">🧩</span>&nbsp;Hover:</b> <span style='color:#0055B0'>{System.Security.SecurityElement.Escape(hoverRoom.Component.Name)}</span>";
            }
            else
            {
                hoverText = $"<b><span style=\"{EmojiStyle}\">🧩</span>&nbsp;Hover:</b> <span style='color:#a6a6a6'><i>None</i></span>";
            }
            Ui.StatusBarHoverText = hoverText;

            // Matching Python lines 1054-1058: Mouse coordinates
            string mouseText = $"<b><span style=\"{EmojiStyle}\">🖱</span>&nbsp;Coords:</b> " +
                               $"<span style='color:#0055B0'>{world.X:F2}</span>, " +
                               $"<span style='color:#228800'>{world.Y:F2}</span>";
            Ui.StatusBarMouseText = mouseText;

            // Matching Python lines 1060-1068: Selection
            string selText;
            if (selHook != null)
            {
                var hookRoom = selHook.Item1;
                var hookIdx = selHook.Item2;
                if (hookRoom != null && hookRoom.Component != null)
                {
                    selText = $"<b><span style=\"{EmojiStyle}\">🎯</span>&nbsp;Selected Hook:</b> <span style='color:#0055B0'>{System.Security.SecurityElement.Escape(hookRoom.Component.Name)}</span> (#{hookIdx})";
                }
                else
                {
                    selText = $"<b><span style=\"{EmojiStyle}\">🎯</span>&nbsp;Selected Hook:</b> <span style='color:#a6a6a6'><i>None</i></span>";
                }
            }
            else if (selRooms != null && selRooms.Count > 0)
            {
                selText = $"<b><span style=\"{EmojiStyle}\">🟦</span>&nbsp;Selected Rooms:</b> <span style='color:#0055B0'>{selRooms.Count}</span>";
            }
            else
            {
                selText = $"<b><span style=\"{EmojiStyle}\">🟦</span>&nbsp;Selected:</b> <span style='color:#a6a6a6'><i>None</i></span>";
            }
            Ui.StatusBarSelectionText = selText;

            // Matching Python lines 1073-1094: Keys/buttons (sorted with modifiers first)
            var keysSorted = SortWithModifiers(keys, GetKeyString, "QtKey");
            var buttonsSorted = SortWithModifiers(mouseButtons, GetButtonString, "QtMouse");

            // Matching Python lines 1135-1149: Format keys and buttons
            string keysText = FormatItems(keysSorted, GetKeyString, "#a13ac8");
            string buttonsText = FormatItems(buttonsSorted, GetButtonString, "#228800");
            string sep = (keysText.Length > 0 && buttonsText.Length > 0) ? " + " : "";
            string keysButtonsText = $"<b><span style=\"{EmojiStyle}\">⌨</span>&nbsp;Keys/<span style=\"{EmojiStyle}\">🖱</span>&nbsp;Buttons:</b> {keysText}{sep}{buttonsText}";
            Ui.StatusBarKeysText = keysButtonsText;

            // Matching Python lines 1154-1171: Mode/status line
            var modeParts = new List<string>();
            // _painting_walkmesh / _current_material: implement when walkmesh paint UI is added
            // if (_paintingWalkmesh)
            // {
            //     var material = _currentMaterial();
            //     string matText = material != null ? material.Name.Replace("_", " ").Title() : "Material";
            //     modeParts.Add($"<span style='color:#c46811'>Paint: {matText}</span>");
            // }
            // _colorize_materials: implement when material rendering is added
            // if (_colorizeMaterials)
            // {
            //     modeParts.Add("Colorized");
            // }
            if (renderer.SnapToGrid)
            {
                modeParts.Add("Grid Snap");
            }
            if (renderer.SnapToHooks)
            {
                modeParts.Add("Hook Snap");
            }
            string modeText = $"<b><span style=\"{EmojiStyle}\">ℹ</span>&nbsp;Status:</b> " +
                             (modeParts.Count > 0 ? string.Join(" | ", modeParts) : "<span style='color:#a6a6a6'><i>Idle</i></span>");
            Ui.StatusBarModeText = modeText;
        }

        private const string EmojiStyle = "font-size:12pt; font-family:'Segoe UI Emoji','Apple Color Emoji','Noto Color Emoji','EmojiOne','Twemoji Mozilla','Segoe UI Symbol',sans-serif; vertical-align:middle;";

        private List<int> SortWithModifiers(HashSet<int> items, Func<int, string> getStringFunc, string qtEnumType)
        {
            if (items == null || items.Count == 0)
            {
                return new List<int>();
            }

            var modifiers = new List<int>();
            var normal = new List<int>();

            if (qtEnumType == "QtKey")
            {
                // Modifier key codes (Windows virtual key codes: VK_CONTROL=17, VK_SHIFT=16, VK_MENU=18, VK_LWIN=91)
                var modifierSet = new HashSet<int>
                {
                    17, 16, 18, 91
                };
                foreach (var item in items)
                {
                    if (modifierSet.Contains(item))
                    {
                        modifiers.Add(item);
                    }
                    else
                    {
                        normal.Add(item);
                    }
                }
            }
            else
            {
                normal.AddRange(items);
            }

            modifiers.Sort((a, b) => string.Compare(getStringFunc(a), getStringFunc(b), StringComparison.Ordinal));
            normal.Sort((a, b) => string.Compare(getStringFunc(a), getStringFunc(b), StringComparison.Ordinal));
            return modifiers.Concat(normal).ToList();
        }

        private string GetKeyString(int key)
        {
            // Matching Python: Remove "Key_" prefix if present. Returns display string for the key (Avalonia Input.Key).
            return $"Key{key}";
        }

        private string GetButtonString(int button)
        {
            // Matching Python: Remove "Button" suffix if present. Returns display string for the mouse button.
            return $"Btn{button}";
        }

        private string FormatItems(List<int> seq, Func<int, string> formatter, string color)
        {
            if (seq == null || seq.Count == 0)
            {
                return "";
            }
            var formattedItems = seq.Select(item => System.Security.SecurityElement.Escape(formatter(item))).ToList();
            var coloredItems = formattedItems.Select(item => $"<span style='color: {color}'>{item}</span>").ToList();
            return string.Join("&nbsp;+&nbsp;", coloredItems);
        }

        /// <summary>
        /// Set up the module selection combobox with available modules from the installation.
        /// Uses ModuleKitManager to get module roots and display names.
        /// Modules are loaded lazily when selected.
        /// </summary>
        private void SetupModules()
        {
            if (Ui == null)
            {
                return;
            }

            // Matching Python line 489: self.ui.moduleSelect.clear()
            Ui.ModuleSelect.Clear();
            // Matching Python line 490: self.ui.moduleComponentList.clear()
            Ui.ModuleComponentList.Clear();

            if (_installation == null || ModuleKitManager == null)
            {
                // Matching Python lines 492-495: Disable modules UI if no installation is available
                Ui.ModulesGroupBoxEnabled = false;
                return;
            }

            // Matching Python line 498: module_roots: list[str] = self._module_kit_manager.get_module_roots()
            var moduleRoots = ModuleKitManager.GetModuleRoots();

            // Matching Python lines 501-504: Populate the combobox with module names
            foreach (var moduleRoot in moduleRoots)
            {
                string displayName = ModuleKitManager.GetModuleDisplayName(moduleRoot);
                Ui.ModuleSelect.AddItem(displayName, moduleRoot);
            }
        }

        /// <summary>
        /// Handle module selection from the combobox.
        /// Loads module components lazily when a module is selected in the combobox.
        /// Uses ModuleKitManager to convert module resources into kit components.
        /// </summary>
        private void OnModuleSelected(int index = -1)
        {
            if (Ui == null)
            {
                return;
            }

            // Matching Python line 638: self.ui.moduleComponentList.clear()
            Ui.ModuleComponentList.Clear();
            // Matching Python line 639: self._set_preview_image(None)
            // Preview image clearing (when preview image UI is present)

            // Matching Python line 641: module_root: str | None = self.ui.moduleSelect.currentData()
            string moduleRoot = Ui.ModuleSelect.CurrentData();
            // Matching Python line 642: if not module_root or not self._installation:
            if (string.IsNullOrEmpty(moduleRoot) || _installation == null || ModuleKitManager == null)
            {
                return;
            }

            try
            {
                // Matching Python line 647: module_kit = self._module_kit_manager.get_module_kit(module_root)
                var moduleKit = ModuleKitManager.GetModuleKit(moduleRoot);

                // Matching Python line 650: if not module_kit.ensure_loaded():
                if (!moduleKit.EnsureLoaded())
                {
                    // Matching Python lines 651-653: Warning logged if no components found
                    new BioWare.Common.Logger.RobustLogger().Warning($"No components found for module '{moduleRoot}'");
                    return;
                }

                // Matching Python lines 657-662: Populate the list with components from the module kit
                foreach (var component in moduleKit.Components)
                {
                    // ensure_component_image (preload component thumbnail) is optional; we add the component to the list directly.
                    Ui.ModuleComponentList.AddItem(component.Name, component);
                }
            }
            catch (Exception ex)
            {
                // Matching Python lines 664-667: Exception handling
                new BioWare.Common.Logger.RobustLogger().Exception($"Failed to load module '{moduleRoot}': {ex.Message}");
            }
        }

        /// <summary>
        /// Handle module component selection from the list.
        /// </summary>
        private void OnModuleComponentSelected(object item = null)
        {
            if (Ui == null)
            {
                return;
            }

            // Matching Python lines 671-674: Clear preview and cursor if no item selected
            if (item == null)
            {
                // Preview image and cursor component (when those UI components are available)
                return;
            }

            // Matching Python line 676: component: KitComponent | None = item.data(Qt.ItemDataRole.UserRole)
            var component = Ui.ModuleComponentList.GetItemData(item);
            if (component == null)
            {
                return;
            }

            // Matching Python lines 679-690: Set preview image and cursor component
            // Preview image and cursor component setting (when those UI components are available)
        }
    }

    public class IndoorBuilderWindowUi
    {
        public Action ActionSelectAll { get; set; }

        public Action ActionDeselectAll { get; set; }

        public Action ActionDeleteSelected { get; set; }

        public Action ActionDuplicate { get; set; }

        public Action ActionUndo { get; set; }

        public Action ActionSettings { get; set; }

        public Action ActionRedo { get; set; }

        public bool ActionUndoEnabled { get; set; }

        public bool ActionRedoEnabled { get; set; }

        public bool ActionSettingsEnabled { get; set; }

        public Action ActionSave { get; set; }

        public Action ActionSaveAs { get; set; }

        public Action ActionOpen { get; set; }

        public Action ActionOpenMod { get; set; }

        public Action ActionBuild { get; set; }

        public Action ActionOpenInBlender { get; set; }

        public bool ActionOpenInBlenderEnabled { get; set; }

        public string BlenderStatusText { get; set; }

        public string SaveAsPathOverride { get; set; }

        public string OpenPathOverride { get; set; }

        public string OpenModPathOverride { get; set; }

        public string LastErrorMessage { get; set; }

        public List<MissingRoomInfo> LastMissingRooms { get; set; }

        public IndoorMapRenderer MapRenderer { get; set; }

        public Action ActionZoomIn { get; set; }

        public Action ActionZoomOut { get; set; }

        // Grid size spinbox minimum property (matching UI file: minimum = 0.5)
        private const decimal GridSizeSpinMinimum = 0.5m;
        public decimal GridSizeSpinMinimumValue
        {
            get { return GridSizeSpinMinimum; }
        }

        // Grid size spinbox maximum property (matching UI file: maximum = 10.0)
        private const decimal GridSizeSpinMaximum = 10.0m;
        public decimal GridSizeSpinMaximumValue
        {
            get { return GridSizeSpinMaximum; }
        }

        // Grid size spinbox value property (decimal for NumericUpDown compatibility)
        // Values are clamped to min/max range to match QDoubleSpinBox behavior
        private decimal _gridSizeSpinValue = 1.0m; // DEFAULT_GRID_SIZE = 1.0
        public decimal GridSizeSpinValue
        {
            get { return _gridSizeSpinValue; }
            set
            {
                // Clamp value to min/max range (matching QDoubleSpinBox behavior)
                decimal clampedValue = value;
                if (clampedValue < GridSizeSpinMinimum)
                {
                    clampedValue = GridSizeSpinMinimum;
                }
                else if (clampedValue > GridSizeSpinMaximum)
                {
                    clampedValue = GridSizeSpinMaximum;
                }

                if (_gridSizeSpinValue != clampedValue)
                {
                    _gridSizeSpinValue = clampedValue;
                    // Trigger value changed event if signals are not blocked
                    if (!BlockSpinboxSignals && GridSizeSpinValueChanged != null)
                    {
                        GridSizeSpinValueChanged((double)clampedValue);
                    }
                }
            }
        }

        // Rotation snap spinbox value property (decimal for NumericUpDown compatibility)
        private decimal _rotSnapSpinValue = 15m; // DEFAULT_ROTATION_SNAP = 15
        public decimal RotSnapSpinValue
        {
            get { return _rotSnapSpinValue; }
            set
            {
                if (_rotSnapSpinValue != value)
                {
                    _rotSnapSpinValue = value;
                    // Trigger value changed event if signals are not blocked
                    if (!BlockSpinboxSignals && RotSnapSpinValueChanged != null)
                    {
                        RotSnapSpinValueChanged((double)value);
                    }
                }
            }
        }

        // Flag to prevent value changed events from firing during initialization
        public bool BlockSpinboxSignals { get; set; } = false;
        public bool BlockCheckboxSignals { get; set; } = false;

        // Action to call when grid size spinbox value changes
        public Action<double> GridSizeSpinValueChanged { get; set; }

        // Action to call when rotation snap spinbox value changes
        public Action<double> RotSnapSpinValueChanged { get; set; }

        // Snap to hooks checkbox checked property
        private bool _snapToHooksCheckChecked = true; // Default matches renderer default (SnapToHooks = true)
        public bool SnapToHooksCheckChecked
        {
            get { return _snapToHooksCheckChecked; }
            set
            {
                if (_snapToHooksCheckChecked != value)
                {
                    _snapToHooksCheckChecked = value;
                    // Trigger toggled event if signals are not blocked
                    if (!BlockCheckboxSignals && SnapToHooksCheckToggled != null)
                    {
                        SnapToHooksCheckToggled(value);
                    }
                }
            }
        }

        // Action to call when snap to hooks checkbox is toggled
        public Action<bool> SnapToHooksCheckToggled { get; set; }

        // Method to set snap to hooks checkbox checked state programmatically (for testing and initialization)
        public void SetSnapToHooksCheckChecked(bool value)
        {
            SnapToHooksCheckChecked = value;
        }

        // Property to access checkbox for testing (matches Python API: builder.ui.snapToHooksCheck.setChecked())
        private SnapToHooksCheckboxWrapper _snapToHooksCheck;
        public SnapToHooksCheckboxWrapper SnapToHooksCheck
        {
            get
            {
                if (_snapToHooksCheck == null)
                {
                    _snapToHooksCheck = new SnapToHooksCheckboxWrapper(this);
                }
                return _snapToHooksCheck;
            }
        }

        // Method to set grid size spinbox value programmatically (for testing and initialization)
        // Values are automatically clamped to min/max range (matching QDoubleSpinBox behavior)
        public void SetGridSizeSpinValue(double value)
        {
            GridSizeSpinValue = (decimal)value;
        }

        // Property to access spinbox for testing (matches Python API: builder.ui.gridSizeSpin.value(), builder.ui.gridSizeSpin.minimum(), builder.ui.gridSizeSpin.maximum())
        private GridSizeSpinboxWrapper _gridSizeSpin;
        public GridSizeSpinboxWrapper GridSizeSpin
        {
            get
            {
                if (_gridSizeSpin == null)
                {
                    _gridSizeSpin = new GridSizeSpinboxWrapper(this);
                }
                return _gridSizeSpin;
            }
        }

        // Method to set rotation snap spinbox value programmatically (for testing and initialization)
        public void SetRotSnapSpinValue(int value)
        {
            RotSnapSpinValue = (decimal)value;
        }

        // These properties store the status bar text that can be displayed when UI is available
        public string StatusBarMouseText { get; set; } = "";
        public string StatusBarHoverText { get; set; } = "";
        public string StatusBarSelectionText { get; set; } = "";
        public string StatusBarKeysText { get; set; } = "";
        public string StatusBarModeText { get; set; } = "";

        private ModuleSelectComboBoxWrapper _moduleSelect;
        public ModuleSelectComboBoxWrapper ModuleSelect
        {
            get
            {
                if (_moduleSelect == null)
                {
                    _moduleSelect = new ModuleSelectComboBoxWrapper();
                }
                return _moduleSelect;
            }
        }


        private ModuleComponentListWrapper _moduleComponentList;
        public ModuleComponentListWrapper ModuleComponentList
        {
            get
            {
                if (_moduleComponentList == null)
                {
                    _moduleComponentList = new ModuleComponentListWrapper();
                }
                return _moduleComponentList;
            }
        }

        private bool _modulesGroupBoxEnabled = true;
        public bool ModulesGroupBoxEnabled
        {
            get { return _modulesGroupBoxEnabled; }
            set { _modulesGroupBoxEnabled = value; }
        }
    }

    // Wrapper class to match Python API where checkbox has setChecked method
    public class SnapToHooksCheckboxWrapper
    {
        private readonly IndoorBuilderWindowUi _ui;

        public SnapToHooksCheckboxWrapper(IndoorBuilderWindowUi ui)
        {
            _ui = ui;
        }

        public void SetChecked(bool value)
        {
            _ui.SetSnapToHooksCheckChecked(value);
        }

        public bool IsChecked()
        {
            return _ui.SnapToHooksCheckChecked;
        }
    }

    // Wrapper class to match Python API where spinbox has setValue, value, minimum, and maximum methods
    public class GridSizeSpinboxWrapper
    {
        private readonly IndoorBuilderWindowUi _ui;

        public GridSizeSpinboxWrapper(IndoorBuilderWindowUi ui)
        {
            _ui = ui;
        }

        public void SetValue(double value)
        {
            _ui.SetGridSizeSpinValue(value);
        }

        public double Value()
        {
            return (double)_ui.GridSizeSpinValue;
        }

        public double Minimum()
        {
            return (double)_ui.GridSizeSpinMinimumValue;
        }

        public double Maximum()
        {
            return (double)_ui.GridSizeSpinMaximumValue;
        }
    }

    // Wrapper class to match Python API where combobox has addItem, currentData, clear, count, setCurrentIndex methods
    public class ModuleSelectComboBoxWrapper
    {
        private readonly List<ModuleSelectItem> _items = new List<ModuleSelectItem>();
        private int _currentIndex = -1;
        private Action<int> _currentIndexChanged;

        // Event handler for currentIndexChanged (set from UI wrapper)
        public void SetCurrentIndexChangedHandler(Action<int> handler)
        {
            _currentIndexChanged = handler;
        }

        public void AddItem(string displayName, string moduleRoot)
        {
            _items.Add(new ModuleSelectItem { DisplayName = displayName, ModuleRoot = moduleRoot });
        }

        public string CurrentData()
        {
            if (_currentIndex >= 0 && _currentIndex < _items.Count)
            {
                return _items[_currentIndex].ModuleRoot;
            }
            return null;
        }

        public void Clear()
        {
            _items.Clear();
            _currentIndex = -1;
        }

        public int Count()
        {
            return _items.Count;
        }

        public void SetCurrentIndex(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
                int oldIndex = _currentIndex;
                _currentIndex = index;
                // Trigger currentIndexChanged event if index actually changed
                if (oldIndex != _currentIndex && _currentIndexChanged != null)
                {
                    _currentIndexChanged(_currentIndex);
                }
            }
            else
            {
                int oldIndex = _currentIndex;
                _currentIndex = -1;
                // Trigger currentIndexChanged event if index actually changed
                if (oldIndex != _currentIndex && _currentIndexChanged != null)
                {
                    _currentIndexChanged(_currentIndex);
                }
            }
        }

        public int CurrentIndex()
        {
            return _currentIndex;
        }

        public string ItemText(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
                return _items[index].DisplayName;
            }
            return null;
        }

        public string ItemData(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
                return _items[index].ModuleRoot;
            }
            return null;
        }

        private class ModuleSelectItem
        {
            public string DisplayName { get; set; }
            public string ModuleRoot { get; set; }
        }
    }

    // Wrapper class to match Python API where list widget has addItem, clear, clearSelection, setCurrentItem methods
    public class ModuleComponentListWrapper
    {
        private readonly List<ModuleComponentListItem> _items = new List<ModuleComponentListItem>();
        private object _currentItem = null;

        // Where item is QListWidgetItem with component data
        public void AddItem(string componentName, KitComponent component)
        {
            _items.Add(new ModuleComponentListItem { Name = componentName, Component = component });
        }

        public void Clear()
        {
            _items.Clear();
            _currentItem = null;
        }

        public void ClearSelection()
        {
            _currentItem = null;
        }

        public void SetCurrentItem(object item)
        {
            _currentItem = item;
        }

        public KitComponent GetItemData(object item)
        {
            if (item is ModuleComponentListItem listItem)
            {
                return listItem.Component;
            }
            // Try to find by index if item is an integer
            if (item is int index && index >= 0 && index < _items.Count)
            {
                return _items[index].Component;
            }
            return null;
        }

        private class ModuleComponentListItem
        {
            public string Name { get; set; }
            public KitComponent Component { get; set; }
        }
    }

}
