using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OdyTools.Dialogs.Save
{
    public enum RimSaveOption
    {
        Nothing = 0,
        MOD = 1,
        Override = 2
    }

    public partial class RimSaveDialog : Window
    {
        private RimSaveOption _option = RimSaveOption.Nothing;
        private Button _modSaveButton;
        private Button _overrideSaveButton;
        private Button _cancelButton;

        // Public parameterless constructor for XAML
        public RimSaveDialog() : this(null)
        {
        }

        public RimSaveDialog(Window parent)
        {
            InitializeComponent();
            SetupUI();
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
            Title = "Save in RIM";
            Width = 300;
            Height = 150;

            var panel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };
            _modSaveButton = new Button { Content = "Save as MOD" };
            _modSaveButton.Click += (s, e) => SaveAsMod();
            _overrideSaveButton = new Button { Content = "Save as Override" };
            _overrideSaveButton.Click += (s, e) => SaveAsOverride();
            _cancelButton = new Button { Content = "Cancel" };
            _cancelButton.Click += (s, e) => Close();

            panel.Children.Add(_modSaveButton);
            panel.Children.Add(_overrideSaveButton);
            panel.Children.Add(_cancelButton);
            Content = panel;
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _modSaveButton = this.FindControl<Button>("modSaveButton");
            _overrideSaveButton = this.FindControl<Button>("overrideSaveButton");
            _cancelButton = this.FindControl<Button>("cancelButton");

            if (_modSaveButton != null)
            {
                _modSaveButton.Click += (s, e) => SaveAsMod();
            }
            if (_overrideSaveButton != null)
            {
                _overrideSaveButton.Click += (s, e) => SaveAsOverride();
            }
            if (_cancelButton != null)
            {
                _cancelButton.Click += (s, e) => Close();
            }
        }

        private void SaveAsMod()
        {
            _option = RimSaveOption.MOD;
            Close();
        }

        private void SaveAsOverride()
        {
            _option = RimSaveOption.Override;
            Close();
        }

        public RimSaveOption Option => _option;
    }
}
