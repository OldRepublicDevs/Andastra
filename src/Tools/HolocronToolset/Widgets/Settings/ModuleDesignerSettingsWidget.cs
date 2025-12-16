using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HolocronToolset.Data;
using HolocronToolset.Widgets.Edit;

namespace HolocronToolset.Widgets.Settings
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:20
    // Original: class ModuleDesignerWidget(SettingsWidget):
    public partial class ModuleDesignerSettingsWidget : UserControl
    {
        private object _settings; // TODO: Use ModuleDesignerSettings type when available
        private NumericUpDown _fovSpin;
        private Button _controls3dResetButton;
        private Button _controlsFcResetButton;
        private Button _controls2dResetButton;
        private Button _coloursResetButton;

        public ModuleDesignerSettingsWidget()
        {
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

            _fovSpin = new NumericUpDown { Minimum = 0, Maximum = 180, Value = 60 };
            panel.Children.Add(new TextBlock { Text = "Field of View:" });
            panel.Children.Add(_fovSpin);

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
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _fovSpin = this.FindControl<NumericUpDown>("fovSpin");
            _controls3dResetButton = this.FindControl<Button>("controls3dResetButton");
            _controlsFcResetButton = this.FindControl<Button>("controlsFcResetButton");
            _controls2dResetButton = this.FindControl<Button>("controls2dResetButton");
            _coloursResetButton = this.FindControl<Button>("coloursResetButton");

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
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:76-84
        // Original: def _load3dBindValues(self):
        private void Load3dBindValues()
        {
            // TODO: Load 3D bind values when ModuleDesignerSettings is available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:86-93
        // Original: def _loadFcBindValues(self):
        private void LoadFcBindValues()
        {
            // TODO: Load fly camera bind values when ModuleDesignerSettings is available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:95-102
        // Original: def _load2dBindValues(self):
        private void Load2dBindValues()
        {
            // TODO: Load 2D bind values when ModuleDesignerSettings is available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:104-107
        // Original: def _loadColourValues(self):
        private void LoadColourValues()
        {
            // TODO: Load colour values when ModuleDesignerSettings is available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:109-114
        // Original: def setup_values(self):
        private void SetupValues()
        {
            if (_fovSpin != null && _settings != null)
            {
                // TODO: Set FOV from settings when available
                _fovSpin.Value = 60; // Default
            }
            Load3dBindValues();
            LoadFcBindValues();
            Load2dBindValues();
            LoadColourValues();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:116-129
        // Original: def save(self):
        public void Save()
        {
            if (_fovSpin != null && _settings != null)
            {
                // TODO: Save FOV to settings when available
            }
            // TODO: Save bind and colour values when ModuleDesignerSettings is available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:131-133
        // Original: def resetControls3d(self):
        private void ResetControls3d()
        {
            // TODO: Reset 3D controls when ModuleDesignerSettings is available
            Load3dBindValues();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:135-137
        // Original: def resetControlsFc(self):
        private void ResetControlsFc()
        {
            // TODO: Reset fly camera controls when ModuleDesignerSettings is available
            LoadFcBindValues();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:139-141
        // Original: def resetControls2d(self):
        private void ResetControls2d()
        {
            // TODO: Reset 2D controls when ModuleDesignerSettings is available
            Load2dBindValues();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/settings/widgets/module_designer.py:143-145
        // Original: def resetColours(self):
        private void ResetColours()
        {
            // TODO: Reset material colors when ModuleDesignerSettings is available
            LoadColourValues();
        }
    }
}
