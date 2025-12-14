using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace HolocronToolset.NET.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/blender_choice.py:40
    // Original: class BlenderChoiceDialog(QDialog):
    public partial class BlenderChoiceDialog : Window
    {
        private string _choice = "builtin"; // "blender" or "builtin"
        private CheckBox _rememberCheckbox;
        private Button _blenderButton;
        private Button _builtinButton;
        private Button _cancelButton;
        private Border _infoFrame;
        private StackPanel _infoLayout;

        // Public parameterless constructor for XAML
        public BlenderChoiceDialog() : this(null, null, "Module Designer")
        {
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/blender_choice.py:49-66
        // Original: def __init__(self, parent, blender_info=None, context="Module Designer"):
        public BlenderChoiceDialog(Window parent, object blenderInfo = null, string context = "Module Designer")
        {
            InitializeComponent();
            Title = "Choose Editor";
            MinWidth = 500;
            SetupUI(context);
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
            var mainPanel = new StackPanel { Margin = new Avalonia.Thickness(20), Spacing = 16 };

            // Info frame
            _infoFrame = new Border
            {
                BorderBrush = new SolidColorBrush(Colors.Gray),
                BorderThickness = new Avalonia.Thickness(1),
                Padding = new Avalonia.Thickness(10)
            };
            _infoLayout = new StackPanel { Spacing = 5 };
            _infoFrame.Child = _infoLayout;
            mainPanel.Children.Add(_infoFrame);

            // Question label
            var questionLabel = new TextBlock
            {
                Text = "How would you like to open the Module Designer?",
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            mainPanel.Children.Add(questionLabel);

            // Buttons layout
            var buttonsPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 16, HorizontalAlignment = HorizontalAlignment.Center };
            _blenderButton = new Button { Content = "Open in Blender\n(Recommended)", MinHeight = 60 };
            _blenderButton.Click += (s, e) => ChooseBlender();
            _builtinButton = new Button { Content = "Use Built-in Editor", MinHeight = 60 };
            _builtinButton.Click += (s, e) => ChooseBuiltin();
            buttonsPanel.Children.Add(_blenderButton);
            buttonsPanel.Children.Add(_builtinButton);
            mainPanel.Children.Add(buttonsPanel);

            // Remember checkbox
            _rememberCheckbox = new CheckBox { Content = "Remember my choice" };
            mainPanel.Children.Add(_rememberCheckbox);

            // Cancel button
            var cancelPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right };
            _cancelButton = new Button { Content = "Cancel", Width = 75 };
            _cancelButton.Click += (s, e) => Close();
            cancelPanel.Children.Add(_cancelButton);
            mainPanel.Children.Add(cancelPanel);

            Content = mainPanel;
        }

        private void SetupUI(string context)
        {
            // Find controls from XAML
            _blenderButton = this.FindControl<Button>("blenderButton");
            _builtinButton = this.FindControl<Button>("builtinButton");
            _rememberCheckbox = this.FindControl<CheckBox>("rememberCheckbox");
            _cancelButton = this.FindControl<Button>("cancelButton");
            _infoFrame = this.FindControl<Border>("infoFrame");
            _infoLayout = this.FindControl<StackPanel>("infoLayout");

            if (_blenderButton != null)
            {
                _blenderButton.Click += (s, e) => ChooseBlender();
            }
            if (_builtinButton != null)
            {
                _builtinButton.Click += (s, e) => ChooseBuiltin();
            }
            if (_cancelButton != null)
            {
                _cancelButton.Click += (s, e) => Close();
            }

            UpdateBlenderInfoDisplay();
            UpdateBlenderButtonState();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/blender_choice.py:68-71
        // Original: @property def choice(self) -> str:
        public string Choice => _choice;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/blender_choice.py:73-76
        // Original: @property def remember_choice(self) -> bool:
        public bool RememberChoice => _rememberCheckbox?.IsChecked ?? false;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/blender_choice.py:158-251
        // Original: def _update_blender_info_display(self):
        private void UpdateBlenderInfoDisplay()
        {
            if (_infoLayout == null)
            {
                return;
            }

            _infoLayout.Children.Clear();

            // TODO: Implement Blender detection when available
            // For now, show a placeholder
            var statusLabel = new TextBlock
            {
                Text = "Blender detection not yet implemented",
                Foreground = new SolidColorBrush(Colors.Orange)
            };
            _infoLayout.Children.Add(statusLabel);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/blender_choice.py:290-310
        // Original: def _update_blender_button_state(self):
        private void UpdateBlenderButtonState()
        {
            if (_blenderButton != null)
            {
                // TODO: Enable/disable based on Blender detection
                _blenderButton.IsEnabled = false; // Disabled until Blender detection is implemented
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/blender_choice.py:312-315
        // Original: def _choose_blender(self):
        private void ChooseBlender()
        {
            _choice = "blender";
            Close();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/blender_choice.py:317-320
        // Original: def _choose_builtin(self):
        private void ChooseBuiltin()
        {
            _choice = "builtin";
            Close();
        }
    }
}
