using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HolocronToolset.NET.Config;

namespace HolocronToolset.NET.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/about.py:15
    // Original: class About(QDialog):
    public class AboutDialog : Window
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/about.py:16-55
        // Original: def __init__(self, parent):
        public AboutDialog(Window parent = null)
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
            Title = "About Holocron Toolset";
            Width = 400;
            Height = 300;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Holocron Toolset",
                FontSize = 24,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            var versionLabel = new TextBlock
            {
                Text = $"Version {ConfigInfo.CurrentVersion}",
                FontSize = 14,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            var closeButton = new Button
            {
                Content = "Close",
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            closeButton.Click += (sender, e) => Close();

            panel.Children.Add(titleLabel);
            panel.Children.Add(versionLabel);
            panel.Children.Add(closeButton);
            Content = panel;
        }

        private void SetupUI()
        {
            // Additional UI setup if needed
        }
    }
}
