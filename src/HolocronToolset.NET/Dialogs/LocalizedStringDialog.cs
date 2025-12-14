using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Common;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/edit/locstring.py:20
    // Original: class LocalizedStringDialog(QDialog):
    public class LocalizedStringDialog : Window
    {
        private HTInstallation _installation;
        public LocalizedString LocString { get; private set; }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/edit/locstring.py:21-107
        // Original: def __init__(self, parent, installation, locstring):
        public LocalizedStringDialog(Window parent, HTInstallation installation, LocalizedString locstring)
        {
            InitializeComponent();
            _installation = installation;
            LocString = locstring ?? LocalizedString.FromInvalid();
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
            Title = "Localized String Editor";
            Width = 500;
            Height = 400;

            var panel = new StackPanel();
            var stringrefLabel = new TextBlock { Text = "StringRef:" };
            var stringrefSpin = new NumericUpDown { Minimum = -1, Maximum = 999999 };
            var stringEdit = new TextBox { AcceptsReturn = true, Watermark = "Text" };
            var okButton = new Button { Content = "OK" };
            okButton.Click += (s, e) => { LocString = LocString ?? LocalizedString.FromInvalid(); Close(); };
            var cancelButton = new Button { Content = "Cancel" };
            cancelButton.Click += (s, e) => Close();

            panel.Children.Add(stringrefLabel);
            panel.Children.Add(stringrefSpin);
            panel.Children.Add(stringEdit);
            panel.Children.Add(okButton);
            panel.Children.Add(cancelButton);
            Content = panel;
        }

        private void SetupUI()
        {
            // Additional UI setup if needed
        }

        public bool ShowDialog()
        {
            // Show dialog and return result
            // This will be implemented when dialog system is available
            return true;
        }
    }
}
