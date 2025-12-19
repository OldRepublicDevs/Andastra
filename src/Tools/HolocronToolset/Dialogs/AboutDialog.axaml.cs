using System;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using HolocronToolset.Config;

namespace HolocronToolset.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/about.py:15
    // Original: class About(QDialog):
    public partial class AboutDialog : Window
    {
        private TextBlock _aboutLabel;
        private Button _closeButton;
        private Image _image;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/about.py:41-43
        // Original: self.ui = Ui_Dialog()
        // Expose UI widgets for testing
        public AboutDialogUi Ui { get; private set; }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/about.py:16-55
        // Original: def __init__(self, parent):
        public AboutDialog() : this(null)
        {
        }

        public AboutDialog(Window parent)
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

            if (xamlLoaded)
            {
                try
                {
                    _aboutLabel = this.FindControl<TextBlock>("aboutLabel");
                    _closeButton = this.FindControl<Button>("closeButton");
                    _image = this.FindControl<Image>("image");
                }
                catch
                {
                    // Controls not found - create programmatic UI
                    SetupProgrammaticUI();
                    return;
                }
            }
            else
            {
                SetupProgrammaticUI();
                return;
            }
        }

        private void SetupProgrammaticUI()
        {
            Title = "About";
            Width = 430;
            Height = 207;
            CanResize = false;

            // Create all UI controls programmatically for test scenarios
            // Matching XAML layout: Grid with image on left, text content on right
            _image = new Image
            {
                Width = 128,
                Height = 128,
                Margin = new Avalonia.Thickness(0, 0, 20, 0)
            };

            _aboutLabel = new TextBlock
            {
                Text = $"Holocron Toolset\nVersion {ConfigInfo.CurrentVersion}\n\nA toolset for editing KOTOR game files.",
                TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top
            };
            _closeButton = new Button { Content = "Close", Width = 75, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            _closeButton.Click += (sender, e) => Close();

            // Create right panel with text and button
            var rightPanel = new StackPanel
            {
                Spacing = 10
            };
            rightPanel.Children.Add(_aboutLabel);
            rightPanel.Children.Add(_closeButton);

            // Create main grid layout matching XAML
            var mainGrid = new Grid
            {
                Margin = new Avalonia.Thickness(20),
                ColumnDefinitions = new ColumnDefinitions("Auto,*")
            };
            Grid.SetColumn(_image, 0);
            Grid.SetColumn(rightPanel, 1);
            mainGrid.Children.Add(_image);
            mainGrid.Children.Add(rightPanel);
            Content = mainGrid;

            // Load icon image
            LoadIconImage();

            // Create UI wrapper for testing
            Ui = new AboutDialogUi
            {
                AboutLabel = _aboutLabel,
                CloseButton = _closeButton
            };
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/about.py:50-52
        // Original: self.ui.aboutLabel.setText(self.ui.aboutLabel.text().replace("X.X.X", LOCAL_PROGRAM_INFO["currentVersion"]))
        private void SetupUI()
        {
            // If Ui is already initialized (e.g., by SetupProgrammaticUI), skip
            if (Ui != null)
            {
                return;
            }

            // Create UI wrapper for testing
            Ui = new AboutDialogUi
            {
                AboutLabel = _aboutLabel,
                CloseButton = _closeButton
            };

            if (_closeButton != null)
            {
                _closeButton.Click += (sender, e) => Close();
            }

            if (_aboutLabel != null)
            {
                // Replace version placeholder with actual version
                // In Avalonia, TextBlock with Runs has empty Text property, so we need to extract from Inlines
                string text = ExtractTextFromTextBlock(_aboutLabel);
                if (!string.IsNullOrEmpty(text) && text.Contains("X.X.X"))
                {
                    text = text.Replace("X.X.X", ConfigInfo.CurrentVersion);
                    // Update the Run that contains "Version X.X.X"
                    UpdateVersionInTextBlock(_aboutLabel, text);
                }
            }

            // Load icon image from embedded resources
            LoadIconImage();
        }

        // Helper method to extract text from TextBlock with Runs
        private string ExtractTextFromTextBlock(TextBlock textBlock)
        {
            if (textBlock == null)
            {
                return "";
            }

            // If Text property is set, use it
            if (!string.IsNullOrEmpty(textBlock.Text))
            {
                return textBlock.Text;
            }

            // Otherwise, extract from Inlines
            var text = new System.Text.StringBuilder();
            foreach (var inline in textBlock.Inlines)
            {
                if (inline is Avalonia.Controls.Documents.Run run)
                {
                    text.Append(run.Text);
                }
                else if (inline is Avalonia.Controls.Documents.LineBreak)
                {
                    text.Append("\n");
                }
            }
            return text.ToString();
        }

        // Helper method to update version in TextBlock
        private void UpdateVersionInTextBlock(TextBlock textBlock, string newText)
        {
            if (textBlock == null)
            {
                return;
            }

            // Find and update the Run containing "Version X.X.X"
            foreach (var inline in textBlock.Inlines)
            {
                if (inline is Avalonia.Controls.Documents.Run run && run.Text.Contains("X.X.X"))
                {
                    run.Text = run.Text.Replace("X.X.X", ConfigInfo.CurrentVersion);
                    break;
                }
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/about.py:41-43
        // Original: self.ui = Ui_Dialog()
        // UI wrapper class for testing access
        public class AboutDialogUi
        {
            public TextBlock AboutLabel { get; set; }
            public Button CloseButton { get; set; }

            // Helper property to get text from AboutLabel (handles Runs)
            public string AboutLabelText
            {
                get
                {
                    if (AboutLabel == null)
                    {
                        return "";
                    }

                    // If Text property is set, use it
                    if (!string.IsNullOrEmpty(AboutLabel.Text))
                    {
                        return AboutLabel.Text;
                    }

                    // Otherwise, extract from Inlines
                    var text = new System.Text.StringBuilder();
                    foreach (var inline in AboutLabel.Inlines)
                    {
                        if (inline is Avalonia.Controls.Documents.Run run)
                        {
                            text.Append(run.Text);
                        }
                        else if (inline is Avalonia.Controls.Documents.LineBreak)
                        {
                            text.Append("\n");
                        }
                    }
                    return text.ToString();
                }
            }
        }

        // Load icon image from embedded resources
        // Matching PyKotor implementation at Tools/HolocronToolset/src/ui/dialogs/about.ui:28-29
        // Original: <property name="pixmap"><pixmap resource="../../resources/resources.qrc">:/images/icons/sith.png</pixmap></property>
        private void LoadIconImage()
        {
            if (_image == null)
            {
                // Image control not available (e.g., in programmatic UI mode)
                return;
            }

            // Try multiple resource URI formats to handle different assembly naming conventions
            var resourcePaths = new[]
            {
                "avares://HolocronToolset.NET/Resources/Icons/sith.png",
                "avares://HolocronToolset/Resources/Icons/sith.png",
                "avares://Resources/Icons/sith.png"
            };

            foreach (var resourcePath in resourcePaths)
            {
                try
                {
                    var resourceUri = new Uri(resourcePath, UriKind.Absolute);
                    using (var stream = AssetLoader.Open(resourceUri))
                    {
                        if (stream != null)
                        {
                            var bitmap = new Bitmap(stream);
                            _image.Source = bitmap;
                            return; // Successfully loaded, exit
                        }
                    }
                }
                catch (Exception)
                {
                    // Try next URI format
                    continue;
                }
            }

            // If all attempts failed, try loading from assembly manifest resources as fallback
            try
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceName = "HolocronToolset.NET.Resources.Icons.sith.png";
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        var bitmap = new Bitmap(stream);
                        _image.Source = bitmap;
                        return;
                    }
                }

                // Try alternative resource name format
                resourceName = "HolocronToolset.Resources.Icons.sith.png";
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        var bitmap = new Bitmap(stream);
                        _image.Source = bitmap;
                        return;
                    }
                }
            }
            catch (Exception)
            {
                // Resource not available - silently fail (icon is optional)
                // This allows the dialog to function even if the icon resource is missing
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/about.py:54-55
        // Original: def showEvent(self, event: QShowEvent): self.setFixedSize(self.size())
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            // Set fixed size when shown
            CanResize = false;
        }
    }
}
