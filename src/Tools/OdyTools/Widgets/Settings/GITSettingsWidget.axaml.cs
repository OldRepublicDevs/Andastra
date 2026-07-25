using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OdyTools.Data;
using OdyTools.Editors;
using OdyTools.Widgets.Edit;

namespace OdyTools.Widgets.Settings
{
    public partial class GITSettingsWidget : SettingsWidget
    {
        private Button _coloursResetButton;
        private Button _controlsResetButton;
        private bool _resetEventsAttached;

        public GITSettingsWidget()
        {
            _settings = new GITSettings();
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

            // Create ColorEdit widgets for all material colors
            var colorNames = new[]
            {
                "UndefinedMaterialColour",
                "DirtMaterialColour",
                "ObscuringMaterialColour",
                "GrassMaterialColour",
                "StoneMaterialColour",
                "WoodMaterialColour",
                "WaterMaterialColour",
                "NonWalkMaterialColour",
                "TransparentMaterialColour",
                "CarpetMaterialColour",
                "MetalMaterialColour",
                "PuddlesMaterialColour",
                "SwampMaterialColour",
                "MudMaterialColour",
                "LeavesMaterialColour",
                "DoorMaterialColour",
                "LavaMaterialColour",
                "BottomlessPitMaterialColour",
                "DeepWaterMaterialColour",
                "NonWalkGrassMaterialColour"
            };

            foreach (var colorName in colorNames)
            {
                var label = new TextBlock { Text = colorName.Replace("MaterialColour", "").Replace("Colour", "") + ":" };
                var colorEdit = new ColorEdit(this);
                colorEdit.AllowAlpha = true;
                colorEdit.Name = colorName + "Edit";

                var colorRow = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
                colorRow.Children.Add(label);
                colorRow.Children.Add(colorEdit);
                panel.Children.Add(colorRow);
            }

            _coloursResetButton = new Button { Content = "Reset Colours" };
            _coloursResetButton.Click += (s, e) => ResetColours();
            _controlsResetButton = new Button { Content = "Reset Controls" };
            _controlsResetButton.Click += (s, e) => ResetControls();

            panel.Children.Add(_coloursResetButton);
            panel.Children.Add(_controlsResetButton);
            Content = panel;
            _resetEventsAttached = true;
        }

        private void SetupUI()
        {
            // Find controls from XAML (or keep programmatic ones if FindControl returns null)
            _coloursResetButton = EditorHelpers.FindControlSafe<Button>(this, "coloursResetButton") ?? _coloursResetButton;
            _controlsResetButton = EditorHelpers.FindControlSafe<Button>(this, "controlsResetButton") ?? _controlsResetButton;

            if (_resetEventsAttached)
            {
                return;
            }

            if (_coloursResetButton != null)
            {
                _coloursResetButton.Click += (s, e) => ResetColours();
            }
            if (_controlsResetButton != null)
            {
                _controlsResetButton.Click += (s, e) => ResetControls();
            }
            _resetEventsAttached = true;
        }

        private void SetupColourValues()
        {
            // Find all ColorEdit widgets in the UI and register them
            // PyKotor pattern: for colorEdit in [widget for widget in dir(self.ui) if "ColourEdit" in widget]:
            //                     self._registerColour(getattr(self.ui, colorEdit), colorEdit[:-4])

            // List of all material color property names from GITSettings
            var colorPropertyNames = new[]
            {
                "UndefinedMaterialColour",
                "DirtMaterialColour",
                "ObscuringMaterialColour",
                "GrassMaterialColour",
                "StoneMaterialColour",
                "WoodMaterialColour",
                "WaterMaterialColour",
                "NonWalkMaterialColour",
                "TransparentMaterialColour",
                "CarpetMaterialColour",
                "MetalMaterialColour",
                "PuddlesMaterialColour",
                "SwampMaterialColour",
                "MudMaterialColour",
                "LeavesMaterialColour",
                "DoorMaterialColour",
                "LavaMaterialColour",
                "BottomlessPitMaterialColour",
                "DeepWaterMaterialColour",
                "NonWalkGrassMaterialColour"
            };

            // Try to find ColorEdit widgets from XAML first (naming: "undefinedMaterialColourEdit", etc.)
            foreach (var colorName in colorPropertyNames)
            {
                // Try camelCase version first (PyKotor UI naming convention)
                var camelCaseName = char.ToLowerInvariant(colorName[0]) + colorName.Substring(1) + "Edit";
                var colorEdit = EditorHelpers.FindControlSafe<ColorEdit>(this, camelCaseName);

                // If not found, try with exact case
                if (colorEdit == null)
                {
                    colorEdit = EditorHelpers.FindControlSafe<ColorEdit>(this, colorName + "Edit");
                }

                if (colorEdit != null)
                {
                    // Set AllowAlpha = true for all material colors (matching PyKotor lines 28-47)
                    colorEdit.AllowAlpha = true;
                    RegisterColour(colorEdit, colorName);
                }
            }

            // Also search for any ColorEdit widgets that might be named differently
            // This handles cases where widgets might be added programmatically or have different naming
            var allControls = GetAllChildControls(this);
            foreach (var control in allControls)
            {
                if (control is ColorEdit colorEditWidget && !_colours.ContainsValue(colorEditWidget))
                {
                    // Try to infer the color name from the widget name
                    var widgetName = colorEditWidget.Name;
                    if (!string.IsNullOrEmpty(widgetName) && widgetName.Contains("Colour") && widgetName.EndsWith("Edit"))
                    {
                        // Remove "Edit" suffix and convert to property name format
                        var baseName = widgetName.Substring(0, widgetName.Length - 4);
                        // Convert camelCase to PascalCase
                        var propertyName = char.ToUpperInvariant(baseName[0]) + baseName.Substring(1);

                        // Check if this property exists in GITSettings
                        var property = typeof(GITSettings).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (property != null && property.PropertyType == typeof(int))
                        {
                            colorEditWidget.AllowAlpha = true;
                            RegisterColour(colorEditWidget, propertyName);
                        }
                    }
                }
            }
        }

        // Helper method to recursively get all child controls
        private List<Control> GetAllChildControls(Control parent)
        {
            var controls = new List<Control>();
            if (parent == null)
            {
                return controls;
            }

            controls.Add(parent);

            if (parent is Panel panel)
            {
                foreach (var child in panel.Children.OfType<Control>())
                {
                    controls.AddRange(GetAllChildControls(child));
                }
            }
            else if (parent is ContentControl contentControl && contentControl.Content is Control contentChild)
            {
                controls.AddRange(GetAllChildControls(contentChild));
            }
            else if (parent is Decorator decorator && decorator.Child is Control decoratorChild)
            {
                controls.AddRange(GetAllChildControls(decoratorChild));
            }

            return controls;
        }

        // PyKotor: for bindEdit in [widget for widget in dir(self.ui) if "BindEdit" in widget]:
        //              self._registerBind(getattr(self.ui, bindEdit), bindEdit[:-4])
        private void SetupBindValues()
        {
            if (_settings == null || !(_settings is GITSettings gitSettings))
            {
                return;
            }

            // List of all bind property names from GITSettings
            var bindPropertyNames = new[]
            {
                "moveCameraBind",
                "rotateCameraBind",
                "zoomCameraBind",
                "rotateSelectedToPointBind",
                "moveSelectedBind",
                "selectUnderneathBind",
                "deleteSelectedBind",
                "duplicateSelectedBind",
                "toggleLockInstancesBind"
            };

            // Try to find SetBindWidget widgets from XAML first (naming: "moveCameraBindEdit", etc.)
            foreach (var bindName in bindPropertyNames)
            {
                // Try camelCase version first (PyKotor UI naming convention)
                var camelCaseName = char.ToLowerInvariant(bindName[0]) + bindName.Substring(1) + "Edit";
                var bindWidget = EditorHelpers.FindControlSafe<SetBindWidget>(this, camelCaseName);

                // If not found, try with exact case
                if (bindWidget == null)
                {
                    bindWidget = EditorHelpers.FindControlSafe<SetBindWidget>(this, bindName + "Edit");
                }

                if (bindWidget != null)
                {
                    RegisterBind(bindWidget, bindName);
                }
            }

            // Also search for any SetBindWidget widgets that might be named differently
            // This handles cases where widgets might be added programmatically or have different naming
            var allControls = GetAllChildControls(this);
            foreach (var control in allControls)
            {
                if (control is SetBindWidget bindWidget && !_binds.ContainsValue(bindWidget))
                {
                    // Try to infer the bind name from the widget name
                    var widgetName = bindWidget.Name;
                    if (!string.IsNullOrEmpty(widgetName) && widgetName.Contains("Bind") && widgetName.EndsWith("Edit"))
                    {
                        // Remove "Edit" suffix and convert to property name format
                        var baseName = widgetName.Substring(0, widgetName.Length - 4);
                        // Convert camelCase to property name format (already should be correct)
                        var propertyName = baseName;

                        // Check if this property exists in GITSettings
                        var property = typeof(GITSettings).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (property != null && property.PropertyType.IsGenericType &&
                            property.PropertyType.GetGenericTypeDefinition() == typeof(SettingsProperty<>))
                        {
                            var genericArg = property.PropertyType.GetGenericArguments()[0];
                            if (genericArg.IsGenericType && genericArg.GetGenericTypeDefinition() == typeof(Tuple<,>))
                            {
                                var tupleArgs = genericArg.GetGenericArguments();
                                if (tupleArgs.Length == 2 &&
                                    tupleArgs[0].IsGenericType && tupleArgs[0].GetGenericTypeDefinition() == typeof(HashSet<>) &&
                                    tupleArgs[1].IsGenericType && tupleArgs[1].GetGenericTypeDefinition() == typeof(HashSet<>))
                                {
                                    RegisterBind(bindWidget, propertyName);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetupValues()
        {
            SetupColourValues();
            SetupBindValues();
        }

        private void ResetColours()
        {
            if (_settings != null && _settings is GITSettings gitSettings)
            {
                gitSettings.ResetMaterialColors();
                SetupColourValues();
            }
        }

        private void ResetControls()
        {
            if (_settings != null && _settings is GITSettings gitSettings)
            {
                gitSettings.ResetControls();
                SetupBindValues();
            }
        }

        public new void Save()
        {
            // Call base Save to save all registered colors and binds
            base.Save();
            // Also call settings Save to persist to disk
            if (_settings != null)
            {
                _settings.Save();
            }
        }
    }
}
