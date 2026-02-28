using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OdyTools.Config;

namespace OdyTools.Windows
{
    public class HelpWindow : Window
    {
        private string _version;

        public HelpWindow(Window parent = null, string startingPage = null)
        {
            InitializeComponent();
            SetupUI();
            SetupContents();
            _startingPage = startingPage;
        }

        private string _startingPage;

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
            Title = "Help - OdyTools";
            Width = 800;
            Height = 600;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Help",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            panel.Children.Add(titleLabel);
            Content = panel;
        }

        public HelpWindowUi Ui { get; private set; }

        private void SetupUI()
        {
            // Create UI wrapper for testing
            Ui = new HelpWindowUi();
        }

        private void SetupContents()
        {
            try
            {
                _version = typeof(HelpWindow).Assembly.GetName().Version?.ToString() ?? "1.0";
                string[] candidatePaths =
                {
                    System.IO.Path.Combine(AppContext.BaseDirectory, "help", "contents.xml"),
                    System.IO.Path.Combine(Environment.CurrentDirectory, "help", "contents.xml")
                };

                string contentsPath = candidatePaths.FirstOrDefault(File.Exists);
                if (!string.IsNullOrEmpty(contentsPath))
                {
                    XDocument doc = XDocument.Load(contentsPath);
                    string xmlVersion = doc.Root?.Attribute("version")?.Value
                        ?? doc.Root?.Element("version")?.Value
                        ?? doc.Descendants("version").FirstOrDefault()?.Value;
                    if (!string.IsNullOrWhiteSpace(xmlVersion))
                    {
                        _version = xmlVersion.Trim();
                    }
                }
            }
            catch
            {
                _version = _version ?? "1.0";
            }
        }

        private void CheckForUpdates()
        {
#if !NET48
            // Help window delegates update checking to the app-level update manager.
            var manager = new UpdateManager(silent: false);
            manager.CheckForUpdates(silent: false);
#endif
        }

        private string WrapHtmlWithStyles(string htmlBody)
        {
            return $@"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 'Oxygen', 'Ubuntu', 'Cantarell', 'Fira Sans', 'Droid Sans', 'Helvetica Neue', sans-serif;
            line-height: 1.6;
            color: #333;
            max-width: 100%;
            margin: 0;
            padding: 24px;
            background-color: #ffffff;
        }}
        h1 {{ font-size: 2em; margin-top: 0; }}
        h2 {{ font-size: 1.5em; }}
        code {{ background-color: #f4f4f4; padding: 2px 4px; border-radius: 3px; }}
        pre {{ background-color: #f4f4f4; padding: 12px; border-radius: 5px; overflow-x: auto; }}
    </style>
</head>
<body>
{htmlBody}
</body>
</html>";
        }
    }

    public class HelpWindowUi
    {
    }
}
