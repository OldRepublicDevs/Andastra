using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using OdyTools.Data;
using OdyTools.Editors;
using OdyTools.Widgets;
using OdyTools.Widgets.Edit;
using BioWare.Common;

namespace OdyTools.Widgets.Settings
{
    public partial class ModuleDesignerSettingsWidget : UserControl
    {
        private ModuleDesignerSettings _settings;
        private Dictionary<string, SetBindWidget> _binds;
        private Dictionary<string, ColorEdit> _colours;
        private NumericUpDown _fovSpin;
        private NumericUpDown _moveCameraSensitivity3dEdit;
        private NumericUpDown _rotateCameraSensitivity3dEdit;
        private NumericUpDown _zoomCameraSensitivity3dEdit;
        private NumericUpDown _boostedMoveCameraSensitivity3dEdit;
        private NumericUpDown _flySpeedFcEdit;
        private NumericUpDown _rotateCameraSensitivityFcEdit;
        private NumericUpDown _boostedFlyCameraSpeedFCEdit;
        private NumericUpDown _moveCameraSensitivity2dEdit;
        private NumericUpDown _rotateCameraSensitivity2dEdit;
        private NumericUpDown _zoomCameraSensitivity2dEdit;
        private Button _controls3dResetButton;
        private Button _controlsFcResetButton;
        private Button _controls2dResetButton;
        private Button _coloursResetButton;
        private bool _resetEventsAttached;

        internal int RegisteredBindCountForTest => _binds.Count;
        internal int RegisteredColourCountForTest => _colours.Count;
        internal bool HasCustomizationSurfaceForTest =>
            _fovSpin != null
            && _moveCameraSensitivity3dEdit != null
            && _flySpeedFcEdit != null
            && _moveCameraSensitivity2dEdit != null
            && _controls3dResetButton != null
            && _controlsFcResetButton != null
            && _controls2dResetButton != null
            && _coloursResetButton != null
            && _binds.Count > 0
            && _colours.Count > 0;

        public ModuleDesignerSettingsWidget()
        {
            _settings = new ModuleDesignerSettings();
            _binds = new Dictionary<string, SetBindWidget>();
            _colours = new Dictionary<string, ColorEdit>();
            InitializeComponent();
            SetupUI();
            SetupValues();
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
            var panel = new StackPanel { Spacing = 10, Margin = new Avalonia.Thickness(10) };

            AddSectionHeader(panel, "General");
            _fovSpin = AddNumeric(panel, "fovSpin", "Field of View", 0, 180, 60);

            AddSectionHeader(panel, "3D Controls");
            _moveCameraSensitivity3dEdit = AddNumeric(panel, "moveCameraSensitivity3dEdit", "Move Camera Sensitivity", 0, 1000, 1);
            _rotateCameraSensitivity3dEdit = AddNumeric(panel, "rotateCameraSensitivity3dEdit", "Rotate Camera Sensitivity", 0, 1000, 1);
            _zoomCameraSensitivity3dEdit = AddNumeric(panel, "zoomCameraSensitivity3dEdit", "Zoom Camera Sensitivity", 0, 1000, 1);
            _boostedMoveCameraSensitivity3dEdit = AddNumeric(panel, "boostedMoveCameraSensitivity3dEdit", "Boosted Move Camera Sensitivity", 0, 1000, 1);
            AddBindEditors(panel, new[]
            {
                "speedBoostCamera3dBind",
                "moveCameraXY3dBind",
                "moveCameraZ3dBind",
                "moveCameraPlane3dBind",
                "rotateCamera3dBind",
                "zoomCamera3dBind",
                "zoomCameraMM3dBind",
                "rotateSelected3dBind",
                "moveSelectedXY3dBind",
                "moveSelectedZ3dBind",
                "rotateObject3dBind",
                "selectObject3dBind",
                "toggleFreeCam3dBind",
                "deleteObject3dBind",
                "moveCameraToSelected3dBind",
                "moveCameraToCursor3dBind",
                "moveCameraToEntryPoint3dBind",
                "rotateCameraLeft3dBind",
                "rotateCameraRight3dBind",
                "rotateCameraUp3dBind",
                "rotateCameraDown3dBind",
                "moveCameraBackward3dBind",
                "moveCameraForward3dBind",
                "moveCameraLeft3dBind",
                "moveCameraRight3dBind",
                "moveCameraUp3dBind",
                "moveCameraDown3dBind",
                "zoomCameraIn3dBind",
                "zoomCameraOut3dBind",
                "duplicateObject3dBind",
                "resetCameraView3dBind"
            });

            AddSectionHeader(panel, "Free Camera Controls");
            _flySpeedFcEdit = AddNumeric(panel, "flySpeedFcEdit", "Fly Camera Speed", 0, 1000, 1);
            _rotateCameraSensitivityFcEdit = AddNumeric(panel, "rotateCameraSensitivityFcEdit", "Rotate Camera Sensitivity", 0, 1000, 1);
            _boostedFlyCameraSpeedFCEdit = AddNumeric(panel, "boostedFlyCameraSpeedFCEdit", "Boosted Fly Camera Speed", 0, 1000, 1);
            AddBindEditors(panel, new[]
            {
                "speedBoostCameraFcBind",
                "moveCameraForwardFcBind",
                "moveCameraBackwardFcBind",
                "moveCameraLeftFcBind",
                "moveCameraRightFcBind",
                "moveCameraUpFcBind",
                "moveCameraDownFcBind",
                "rotateCameraLeftFcBind",
                "rotateCameraRightFcBind",
                "rotateCameraUpFcBind",
                "rotateCameraDownFcBind",
                "zoomCameraInFcBind",
                "zoomCameraOutFcBind",
                "moveCameraToEntryPointFcBind",
                "moveCameraToCursorFcBind"
            });

            AddSectionHeader(panel, "2D Controls");
            _moveCameraSensitivity2dEdit = AddNumeric(panel, "moveCameraSensitivity2dEdit", "Move Camera Sensitivity", 0, 1000, 1);
            _rotateCameraSensitivity2dEdit = AddNumeric(panel, "rotateCameraSensitivity2dEdit", "Rotate Camera Sensitivity", 0, 1000, 1);
            _zoomCameraSensitivity2dEdit = AddNumeric(panel, "zoomCameraSensitivity2dEdit", "Zoom Camera Sensitivity", 0, 1000, 1);
            AddBindEditors(panel, new[]
            {
                "moveCamera2dBind",
                "zoomCamera2dBind",
                "rotateCamera2dBind",
                "selectObject2dBind",
                "moveObject2dBind",
                "rotateObject2dBind",
                "deleteObject2dBind",
                "snapCameraToSelected2dBind",
                "duplicateObject2dBind"
            });

            AddSectionHeader(panel, "Walkmesh Colours");
            AddColourEditors(panel, new[]
            {
                "undefinedMaterialColour",
                "dirtMaterialColour",
                "obscuringMaterialColour",
                "grassMaterialColour",
                "stoneMaterialColour",
                "woodMaterialColour",
                "waterMaterialColour",
                "nonWalkMaterialColour",
                "transparentMaterialColour",
                "carpetMaterialColour",
                "metalMaterialColour",
                "puddlesMaterialColour",
                "swampMaterialColour",
                "mudMaterialColour",
                "leavesMaterialColour",
                "doorMaterialColour",
                "lavaMaterialColour",
                "bottomlessPitMaterialColour",
                "deepWaterMaterialColour",
                "nonWalkGrassMaterialColour"
            });

            _controls3dResetButton = new Button { Content = "Reset 3D Controls" };
            _controls3dResetButton.Click += (s, e) => ResetControls3d();
            _controlsFcResetButton = new Button { Content = "Reset Fly Camera Controls" };
            _controlsFcResetButton.Click += (s, e) => ResetControlsFc();
            _controls2dResetButton = new Button { Content = "Reset 2D Controls" };
            _controls2dResetButton.Click += (s, e) => ResetControls2d();
            _coloursResetButton = new Button { Content = "Reset Colours" };
            _coloursResetButton.Click += (s, e) => ResetColours();

            panel.Children.Add(_controls3dResetButton);
            panel.Children.Add(_controlsFcResetButton);
            panel.Children.Add(_controls2dResetButton);
            panel.Children.Add(_coloursResetButton);

            Content = panel;
            _resetEventsAttached = true;
        }

        private static void AddSectionHeader(Panel panel, string text)
        {
            panel.Children.Add(new TextBlock
            {
                Text = text,
                FontWeight = Avalonia.Media.FontWeight.SemiBold,
                Margin = new Avalonia.Thickness(0, 8, 0, 0)
            });
        }

        private static NumericUpDown AddNumeric(Panel panel, string name, string label, decimal minimum, decimal maximum, decimal value)
        {
            var row = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 8 };
            row.Children.Add(new TextBlock { Text = label + ":", MinWidth = 220, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center });
            var spin = new NumericUpDown { Name = name, Minimum = minimum, Maximum = maximum, Value = value };
            row.Children.Add(spin);
            panel.Children.Add(row);
            return spin;
        }

        private static void AddBindEditors(Panel panel, IEnumerable<string> bindNames)
        {
            foreach (var bindName in bindNames)
            {
                var row = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 8 };
                row.Children.Add(new TextBlock { Text = bindName + ":", MinWidth = 220, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center });
                row.Children.Add(new SetBindWidget { Name = bindName + "Edit" });
                panel.Children.Add(row);
            }
        }

        private static void AddColourEditors(Panel panel, IEnumerable<string> colourNames)
        {
            foreach (var colourName in colourNames)
            {
                var row = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 8 };
                row.Children.Add(new TextBlock { Text = colourName + ":", MinWidth = 220, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center });
                var edit = new ColorEdit { Name = colourName + "Edit", AllowAlpha = true };
                row.Children.Add(edit);
                panel.Children.Add(row);
            }
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _fovSpin = EditorHelpers.FindControlSafe<NumericUpDown>(this, "fovSpin") ?? _fovSpin;
            _moveCameraSensitivity3dEdit = EditorHelpers.FindControlSafe<NumericUpDown>(this, "moveCameraSensitivity3dEdit") ?? _moveCameraSensitivity3dEdit;
            _rotateCameraSensitivity3dEdit = EditorHelpers.FindControlSafe<NumericUpDown>(this, "rotateCameraSensitivity3dEdit") ?? _rotateCameraSensitivity3dEdit;
            _zoomCameraSensitivity3dEdit = EditorHelpers.FindControlSafe<NumericUpDown>(this, "zoomCameraSensitivity3dEdit") ?? _zoomCameraSensitivity3dEdit;
            _boostedMoveCameraSensitivity3dEdit = EditorHelpers.FindControlSafe<NumericUpDown>(this, "boostedMoveCameraSensitivity3dEdit") ?? _boostedMoveCameraSensitivity3dEdit;
            _flySpeedFcEdit = EditorHelpers.FindControlSafe<NumericUpDown>(this, "flySpeedFcEdit") ?? _flySpeedFcEdit;
            _rotateCameraSensitivityFcEdit = EditorHelpers.FindControlSafe<NumericUpDown>(this, "rotateCameraSensitivityFcEdit") ?? _rotateCameraSensitivityFcEdit;
            _boostedFlyCameraSpeedFCEdit = EditorHelpers.FindControlSafe<NumericUpDown>(this, "boostedFlyCameraSpeedFCEdit") ?? _boostedFlyCameraSpeedFCEdit;
            _moveCameraSensitivity2dEdit = EditorHelpers.FindControlSafe<NumericUpDown>(this, "moveCameraSensitivity2dEdit") ?? _moveCameraSensitivity2dEdit;
            _rotateCameraSensitivity2dEdit = EditorHelpers.FindControlSafe<NumericUpDown>(this, "rotateCameraSensitivity2dEdit") ?? _rotateCameraSensitivity2dEdit;
            _zoomCameraSensitivity2dEdit = EditorHelpers.FindControlSafe<NumericUpDown>(this, "zoomCameraSensitivity2dEdit") ?? _zoomCameraSensitivity2dEdit;
            _controls3dResetButton = EditorHelpers.FindControlSafe<Button>(this, "controls3dResetButton") ?? _controls3dResetButton;
            _controlsFcResetButton = EditorHelpers.FindControlSafe<Button>(this, "controlsFcResetButton") ?? _controlsFcResetButton;
            _controls2dResetButton = EditorHelpers.FindControlSafe<Button>(this, "controls2dResetButton") ?? _controls2dResetButton;
            _coloursResetButton = EditorHelpers.FindControlSafe<Button>(this, "coloursResetButton") ?? _coloursResetButton;

            if (_resetEventsAttached)
            {
                return;
            }

            if (_controls3dResetButton != null)
            {
                _controls3dResetButton.Click += (s, e) => ResetControls3d();
            }
            if (_controlsFcResetButton != null)
            {
                _controlsFcResetButton.Click += (s, e) => ResetControlsFc();
            }
            if (_controls2dResetButton != null)
            {
                _controls2dResetButton.Click += (s, e) => ResetControls2d();
            }
            if (_coloursResetButton != null)
            {
                _coloursResetButton.Click += (s, e) => ResetColours();
            }
            _resetEventsAttached = true;
        }

        private void Load3dBindValues()
        {
            if (_moveCameraSensitivity3dEdit != null)
            {
                _moveCameraSensitivity3dEdit.Value = _settings.MoveCameraSensitivity3d.GetValue(_settings);
            }
            if (_rotateCameraSensitivity3dEdit != null)
            {
                _rotateCameraSensitivity3dEdit.Value = _settings.RotateCameraSensitivity3d.GetValue(_settings);
            }
            if (_zoomCameraSensitivity3dEdit != null)
            {
                _zoomCameraSensitivity3dEdit.Value = _settings.ZoomCameraSensitivity3d.GetValue(_settings);
            }
            if (_boostedMoveCameraSensitivity3dEdit != null)
            {
                _boostedMoveCameraSensitivity3dEdit.Value = _settings.BoostedMoveCameraSensitivity3d.GetValue(_settings);
            }

            // Load all 3D bind widgets
            RegisterBindIfExists("speedBoostCamera3dBind", _settings.SpeedBoostCamera3dBind);
            RegisterBindIfExists("moveCameraXY3dBind", _settings.MoveCameraXY3dBind);
            RegisterBindIfExists("moveCameraZ3dBind", _settings.MoveCameraZ3dBind);
            RegisterBindIfExists("moveCameraPlane3dBind", _settings.MoveCameraPlane3dBind);
            RegisterBindIfExists("rotateCamera3dBind", _settings.RotateCamera3dBind);
            RegisterBindIfExists("zoomCamera3dBind", _settings.ZoomCamera3dBind);
            RegisterBindIfExists("zoomCameraMM3dBind", _settings.ZoomCameraMM3dBind);
            RegisterBindIfExists("rotateSelected3dBind", _settings.RotateSelected3dBind);
            RegisterBindIfExists("moveSelectedXY3dBind", _settings.MoveSelectedXY3dBind);
            RegisterBindIfExists("moveSelectedZ3dBind", _settings.MoveSelectedZ3dBind);
            RegisterBindIfExists("rotateObject3dBind", _settings.RotateObject3dBind);
            RegisterBindIfExists("selectObject3dBind", _settings.SelectObject3dBind);
            RegisterBindIfExists("toggleFreeCam3dBind", _settings.ToggleFreeCam3dBind);
            RegisterBindIfExists("deleteObject3dBind", _settings.DeleteObject3dBind);
            RegisterBindIfExists("moveCameraToSelected3dBind", _settings.MoveCameraToSelected3dBind);
            RegisterBindIfExists("moveCameraToCursor3dBind", _settings.MoveCameraToCursor3dBind);
            RegisterBindIfExists("moveCameraToEntryPoint3dBind", _settings.MoveCameraToEntryPoint3dBind);
            RegisterBindIfExists("rotateCameraLeft3dBind", _settings.RotateCameraLeft3dBind);
            RegisterBindIfExists("rotateCameraRight3dBind", _settings.RotateCameraRight3dBind);
            RegisterBindIfExists("rotateCameraUp3dBind", _settings.RotateCameraUp3dBind);
            RegisterBindIfExists("rotateCameraDown3dBind", _settings.RotateCameraDown3dBind);
            RegisterBindIfExists("moveCameraBackward3dBind", _settings.MoveCameraBackward3dBind);
            RegisterBindIfExists("moveCameraForward3dBind", _settings.MoveCameraForward3dBind);
            RegisterBindIfExists("moveCameraLeft3dBind", _settings.MoveCameraLeft3dBind);
            RegisterBindIfExists("moveCameraRight3dBind", _settings.MoveCameraRight3dBind);
            RegisterBindIfExists("moveCameraUp3dBind", _settings.MoveCameraUp3dBind);
            RegisterBindIfExists("moveCameraDown3dBind", _settings.MoveCameraDown3dBind);
            RegisterBindIfExists("zoomCameraIn3dBind", _settings.ZoomCameraIn3dBind);
            RegisterBindIfExists("zoomCameraOut3dBind", _settings.ZoomCameraOut3dBind);
            RegisterBindIfExists("duplicateObject3dBind", _settings.DuplicateObject3dBind);
            RegisterBindIfExists("resetCameraView3dBind", _settings.ResetCameraView3dBind);
        }

        private void LoadFcBindValues()
        {
            if (_flySpeedFcEdit != null)
            {
                _flySpeedFcEdit.Value = _settings.FlyCameraSpeedFC.GetValue(_settings);
            }
            if (_rotateCameraSensitivityFcEdit != null)
            {
                _rotateCameraSensitivityFcEdit.Value = _settings.RotateCameraSensitivityFC.GetValue(_settings);
            }
            if (_boostedFlyCameraSpeedFCEdit != null)
            {
                _boostedFlyCameraSpeedFCEdit.Value = _settings.BoostedFlyCameraSpeedFC.GetValue(_settings);
            }

            // Load all FreeCam bind widgets
            RegisterBindIfExists("speedBoostCameraFcBind", _settings.SpeedBoostCameraFcBind);
            RegisterBindIfExists("moveCameraForwardFcBind", _settings.MoveCameraForwardFcBind);
            RegisterBindIfExists("moveCameraBackwardFcBind", _settings.MoveCameraBackwardFcBind);
            RegisterBindIfExists("moveCameraLeftFcBind", _settings.MoveCameraLeftFcBind);
            RegisterBindIfExists("moveCameraRightFcBind", _settings.MoveCameraRightFcBind);
            RegisterBindIfExists("moveCameraUpFcBind", _settings.MoveCameraUpFcBind);
            RegisterBindIfExists("moveCameraDownFcBind", _settings.MoveCameraDownFcBind);
            RegisterBindIfExists("rotateCameraLeftFcBind", _settings.RotateCameraLeftFcBind);
            RegisterBindIfExists("rotateCameraRightFcBind", _settings.RotateCameraRightFcBind);
            RegisterBindIfExists("rotateCameraUpFcBind", _settings.RotateCameraUpFcBind);
            RegisterBindIfExists("rotateCameraDownFcBind", _settings.RotateCameraDownFcBind);
            RegisterBindIfExists("zoomCameraInFcBind", _settings.ZoomCameraInFcBind);
            RegisterBindIfExists("zoomCameraOutFcBind", _settings.ZoomCameraOutFcBind);
            RegisterBindIfExists("moveCameraToEntryPointFcBind", _settings.MoveCameraToEntryPointFcBind);
            RegisterBindIfExists("moveCameraToCursorFcBind", _settings.MoveCameraToCursorFcBind);
        }

        private void Load2dBindValues()
        {
            if (_moveCameraSensitivity2dEdit != null)
            {
                _moveCameraSensitivity2dEdit.Value = _settings.MoveCameraSensitivity2d.GetValue(_settings);
            }
            if (_rotateCameraSensitivity2dEdit != null)
            {
                _rotateCameraSensitivity2dEdit.Value = _settings.RotateCameraSensitivity2d.GetValue(_settings);
            }
            if (_zoomCameraSensitivity2dEdit != null)
            {
                _zoomCameraSensitivity2dEdit.Value = _settings.ZoomCameraSensitivity2d.GetValue(_settings);
            }

            // Load all 2D bind widgets
            RegisterBindIfExists("moveCamera2dBind", _settings.MoveCamera2dBind);
            RegisterBindIfExists("zoomCamera2dBind", _settings.ZoomCamera2dBind);
            RegisterBindIfExists("rotateCamera2dBind", _settings.RotateCamera2dBind);
            RegisterBindIfExists("selectObject2dBind", _settings.SelectObject2dBind);
            RegisterBindIfExists("moveObject2dBind", _settings.MoveObject2dBind);
            RegisterBindIfExists("rotateObject2dBind", _settings.RotateObject2dBind);
            RegisterBindIfExists("deleteObject2dBind", _settings.DeleteObject2dBind);
            RegisterBindIfExists("snapCameraToSelected2dBind", _settings.SnapCameraToSelected2dBind);
            RegisterBindIfExists("duplicateObject2dBind", _settings.DuplicateObject2dBind);
        }

        private void LoadColourValues()
        {
            // Load all material colour widgets
            RegisterColourIfExists("undefinedMaterialColour", _settings.UndefinedMaterialColour);
            RegisterColourIfExists("dirtMaterialColour", _settings.DirtMaterialColour);
            RegisterColourIfExists("obscuringMaterialColour", _settings.ObscuringMaterialColour);
            RegisterColourIfExists("grassMaterialColour", _settings.GrassMaterialColour);
            RegisterColourIfExists("stoneMaterialColour", _settings.StoneMaterialColour);
            RegisterColourIfExists("woodMaterialColour", _settings.WoodMaterialColour);
            RegisterColourIfExists("waterMaterialColour", _settings.WaterMaterialColour);
            RegisterColourIfExists("nonWalkMaterialColour", _settings.NonWalkMaterialColour);
            RegisterColourIfExists("transparentMaterialColour", _settings.TransparentMaterialColour);
            RegisterColourIfExists("carpetMaterialColour", _settings.CarpetMaterialColour);
            RegisterColourIfExists("metalMaterialColour", _settings.MetalMaterialColour);
            RegisterColourIfExists("puddlesMaterialColour", _settings.PuddlesMaterialColour);
            RegisterColourIfExists("swampMaterialColour", _settings.SwampMaterialColour);
            RegisterColourIfExists("mudMaterialColour", _settings.MudMaterialColour);
            RegisterColourIfExists("leavesMaterialColour", _settings.LeavesMaterialColour);
            RegisterColourIfExists("doorMaterialColour", _settings.DoorMaterialColour);
            RegisterColourIfExists("lavaMaterialColour", _settings.LavaMaterialColour);
            RegisterColourIfExists("bottomlessPitMaterialColour", _settings.BottomlessPitMaterialColour);
            RegisterColourIfExists("deepWaterMaterialColour", _settings.DeepWaterMaterialColour);
            RegisterColourIfExists("nonWalkGrassMaterialColour", _settings.NonWalkGrassMaterialColour);
        }

        // Helper method to register a bind widget if it exists in the UI
        private void RegisterBindIfExists(string bindName, SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>> settingsProperty)
        {
            var widget = EditorHelpers.FindControlSafe<SetBindWidget>(this, bindName + "Edit")
                ?? EditorHelpers.FindControlSafe<SetBindWidget>(this, bindName + "BindEdit");
            if (widget != null)
            {
                var bind = _settings.GetValue(settingsProperty.Name, settingsProperty.Default);
                if (bind == null || bind.Item1 == null || bind.Item2 == null)
                {
                    bind = settingsProperty.Default;
                }
                widget.SetMouseAndKeyBinds(bind);
                _binds[settingsProperty.Name] = widget;
            }
        }

        // Helper method to register a colour widget if it exists in the UI
        private void RegisterColourIfExists(string colourName, SettingsProperty<int> settingsProperty)
        {
            var widget = EditorHelpers.FindControlSafe<ColorEdit>(this, colourName + "Edit")
                ?? EditorHelpers.FindControlSafe<ColorEdit>(this, colourName + "ColourEdit");
            if (widget != null)
            {
                int colorValue = _settings.GetValue(settingsProperty.Name, settingsProperty.Default);
                widget.SetColor(Color.FromRgbaInteger(colorValue));
                _colours[settingsProperty.Name] = widget;
            }
        }

        private void SetupValues()
        {
            if (_fovSpin != null)
            {
                _fovSpin.Value = _settings.FieldOfView.GetValue(_settings);
            }
            Load3dBindValues();
            LoadFcBindValues();
            Load2dBindValues();
            LoadColourValues();
        }

        public void Save()
        {
            if (_fovSpin != null)
            {
                _settings.FieldOfView.SetValue(_settings, (int)_fovSpin.Value);
            }

            // Save sensitivity values
            if (_moveCameraSensitivity3dEdit != null)
            {
                _settings.MoveCameraSensitivity3d.SetValue(_settings, (int)_moveCameraSensitivity3dEdit.Value);
            }
            if (_rotateCameraSensitivity3dEdit != null)
            {
                _settings.RotateCameraSensitivity3d.SetValue(_settings, (int)_rotateCameraSensitivity3dEdit.Value);
            }
            if (_zoomCameraSensitivity3dEdit != null)
            {
                _settings.ZoomCameraSensitivity3d.SetValue(_settings, (int)_zoomCameraSensitivity3dEdit.Value);
            }
            if (_boostedMoveCameraSensitivity3dEdit != null)
            {
                _settings.BoostedMoveCameraSensitivity3d.SetValue(_settings, (int)_boostedMoveCameraSensitivity3dEdit.Value);
            }

            if (_flySpeedFcEdit != null)
            {
                _settings.FlyCameraSpeedFC.SetValue(_settings, (int)_flySpeedFcEdit.Value);
            }
            if (_rotateCameraSensitivityFcEdit != null)
            {
                _settings.RotateCameraSensitivityFC.SetValue(_settings, (int)_rotateCameraSensitivityFcEdit.Value);
            }
            if (_boostedFlyCameraSpeedFCEdit != null)
            {
                _settings.BoostedFlyCameraSpeedFC.SetValue(_settings, (int)_boostedFlyCameraSpeedFCEdit.Value);
            }

            if (_moveCameraSensitivity2dEdit != null)
            {
                _settings.MoveCameraSensitivity2d.SetValue(_settings, (int)_moveCameraSensitivity2dEdit.Value);
            }
            if (_rotateCameraSensitivity2dEdit != null)
            {
                _settings.RotateCameraSensitivity2d.SetValue(_settings, (int)_rotateCameraSensitivity2dEdit.Value);
            }
            if (_zoomCameraSensitivity2dEdit != null)
            {
                _settings.ZoomCameraSensitivity2d.SetValue(_settings, (int)_zoomCameraSensitivity2dEdit.Value);
            }

            // Save all bind values
            foreach (var kvp in _binds)
            {
                var bind = kvp.Value.GetMouseAndKeyBinds();
                if (bind != null && bind.Item1 != null && bind.Item2 != null)
                {
                    _settings.SetValue(kvp.Key, bind);
                }
            }

            // Save all colour values
            foreach (var kvp in _colours)
            {
                int colorValue = kvp.Value.GetColor().ToRgbaInteger();
                _settings.SetValue(kvp.Key, colorValue);
            }
        }

        private void ResetControls3d()
        {
            _settings.ResetControls3d();
            Load3dBindValues();
        }

        private void ResetControlsFc()
        {
            _settings.ResetControlsFc();
            LoadFcBindValues();
        }

        private void ResetControls2d()
        {
            _settings.ResetControls2d();
            Load2dBindValues();
        }

        private void ResetColours()
        {
            _settings.ResetMaterialColors();
            LoadColourValues();
        }
    }
}
