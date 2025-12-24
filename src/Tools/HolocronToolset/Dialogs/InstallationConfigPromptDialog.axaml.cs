using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace HolocronToolset.Dialogs
{
    // Dialog that prompts user to configure an installation that exists in the combo but has no configuration data
    public partial class InstallationConfigPromptDialog : Window
    {
        private string _installationName;
        private bool? _result;

        public InstallationConfigPromptDialog(string installationName)
        {
            InitializeComponent();
            _installationName = installationName;
            SetupDialog();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetupDialog()
        {
            Title = $"Configure Installation - {_installationName}";
            Width = 400;
            Height = 200;
            CanResize = false;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            var mainPanel = new StackPanel
            {
                Margin = new Avalonia.Thickness(20)
            };

            // Message
            var messageText = new TextBlock
            {
                Text = $"The installation '{_installationName}' exists but is not configured.\n\nWould you like to configure it now?",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                Margin = new Avalonia.Thickness(0, 0, 0, 20)
            };
            mainPanel.Children.Add(messageText);

            // Buttons
            var buttonPanel = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Spacing = 10
            };

            var configureButton = new Button
            {
                Content = "Configure",
                Width = 80
            };
            configureButton.Click += (s, e) => Close(true);

            var cancelButton = new Button
            {
                Content = "Cancel",
                Width = 80
            };
            cancelButton.Click += (s, e) => Close(false);

            buttonPanel.Children.Add(configureButton);
            buttonPanel.Children.Add(cancelButton);
            mainPanel.Children.Add(buttonPanel);

            Content = mainPanel;

            // Handle close button (X) as cancel
            Closing += (s, e) =>
            {
                if (_result == null)
                {
                    _result = false;
                }
            };
        }

        // Show dialog and return result
        public async Task<bool> ShowDialogAsync(Window parent)
        {
            _result = await ShowDialog<bool?>(parent);
            return _result ?? false;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (_result == null)
            {
                _result = false;
            }
        }
    }
}
