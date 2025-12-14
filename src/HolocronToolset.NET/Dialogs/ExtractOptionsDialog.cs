using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HolocronToolset.NET.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:8
    // Original: class ExtractOptionsDialog(QDialog):
    public class ExtractOptionsDialog : Window
    {
        private bool _tpcDecompile;
        private bool _tpcExtractTxi;
        private bool _mdlDecompile;
        private bool _mdlExtractTextures;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:11-40
        // Original: def __init__(self, parent=None):
        public ExtractOptionsDialog(Window parent = null)
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
            Title = "Extract Options";
            Width = 400;
            Height = 300;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Extract Options",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            var okButton = new Button { Content = "OK" };
            okButton.Click += (sender, e) => Close();

            panel.Children.Add(titleLabel);
            panel.Children.Add(okButton);
            Content = panel;
        }

        private void SetupUI()
        {
            // Additional UI setup if needed
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:22-40
        // Original: @property def tpc_decompile(self) -> bool:
        public bool TpcDecompile
        {
            get => _tpcDecompile;
            set => _tpcDecompile = value;
        }

        public bool TpcExtractTxi
        {
            get => _tpcExtractTxi;
            set => _tpcExtractTxi = value;
        }

        public bool MdlDecompile
        {
            get => _mdlDecompile;
            set => _mdlDecompile = value;
        }

        public bool MdlExtractTextures
        {
            get => _mdlExtractTextures;
            set => _mdlExtractTextures = value;
        }
    }
}
