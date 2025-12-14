using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using HolocronToolset.NET.Config;

namespace HolocronToolset.NET.Dialogs
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
            AvaloniaXamlLoader.Load(this);
            _aboutLabel = this.FindControl<TextBlock>("aboutLabel");
            _closeButton = this.FindControl<Button>("closeButton");
            _image = this.FindControl<Image>("image");
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/about.py:50-52
        // Original: self.ui.aboutLabel.setText(self.ui.aboutLabel.text().replace("X.X.X", LOCAL_PROGRAM_INFO["currentVersion"]))
        private void SetupUI()
        {
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
                text = text.Replace("X.X.X", ConfigInfo.CurrentVersion);
                // Update the Run that contains "Version X.X.X"
                UpdateVersionInTextBlock(_aboutLabel, text);
            }

            // TODO: Load icon image when resources are available
            // _image.Source = new Bitmap("path/to/sith.png");
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
