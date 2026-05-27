using System;
using Avalonia.Controls;
using Avalonia.Layout;

namespace OdyTools.Dialogs
{
    /// <summary>
    /// Scope toggles for Holocron-style reference search (override/modules/chitin).
    /// </summary>
    public class ReferenceSearchOptionsDialog : Window
    {
        private CheckBox _overrideCheckbox;
        private CheckBox _modulesCheckbox;
        private CheckBox _chitinCheckbox;
        private CheckBox _caseSensitiveCheckbox;
        private CheckBox _partialMatchCheckbox;
        private bool _accepted;

        public ReferenceSearchOptionsDialog() : this(null)
        {
        }

        public ReferenceSearchOptionsDialog(Window parent)
        {
            Title = "Reference Search Options";
            Width = 360;
            Height = 280;
            WindowStartupLocation = parent != null
                ? WindowStartupLocation.CenterOwner
                : WindowStartupLocation.CenterScreen;

            _overrideCheckbox = new CheckBox { Content = "Search Override", IsChecked = true };
            _modulesCheckbox = new CheckBox { Content = "Search Modules", IsChecked = true };
            _chitinCheckbox = new CheckBox { Content = "Search Chitin / core", IsChecked = true };
            _caseSensitiveCheckbox = new CheckBox { Content = "Case sensitive", IsChecked = false };
            _partialMatchCheckbox = new CheckBox { Content = "Partial match", IsChecked = false };

            var okButton = new Button { Content = "Search", MinWidth = 80 };
            var cancelButton = new Button { Content = "Cancel", MinWidth = 80 };
            okButton.Click += (s, e) => { _accepted = true; Close(); };
            cancelButton.Click += (s, e) => { _accepted = false; Close(); };

            var buttonRow = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Spacing = 8,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Avalonia.Thickness(0, 12, 0, 0)
            };
            buttonRow.Children.Add(okButton);
            buttonRow.Children.Add(cancelButton);

            var panel = new StackPanel { Margin = new Avalonia.Thickness(16), Spacing = 8 };
            panel.Children.Add(new TextBlock { Text = "Search locations", FontWeight = Avalonia.Media.FontWeight.SemiBold });
            panel.Children.Add(_overrideCheckbox);
            panel.Children.Add(_modulesCheckbox);
            panel.Children.Add(_chitinCheckbox);
            panel.Children.Add(new TextBlock { Text = "Matching", FontWeight = Avalonia.Media.FontWeight.SemiBold, Margin = new Avalonia.Thickness(0, 8, 0, 0) });
            panel.Children.Add(_caseSensitiveCheckbox);
            panel.Children.Add(_partialMatchCheckbox);
            panel.Children.Add(buttonRow);
            Content = panel;
        }

        public bool ShowDialogAndAccepted(Window parent)
        {
            if (parent != null)
            {
                ShowDialog(parent);
            }
            else
            {
                Show();
            }

            return _accepted;
        }

        public BioWare.Tools.ReferenceSearchOptions ToSearchOptions()
        {
            return new BioWare.Tools.ReferenceSearchOptions
            {
                SearchOverride = _overrideCheckbox.IsChecked ?? true,
                SearchModules = _modulesCheckbox.IsChecked ?? true,
                SearchChitin = _chitinCheckbox.IsChecked ?? true,
                CaseSensitive = _caseSensitiveCheckbox.IsChecked ?? false,
                PartialMatch = _partialMatchCheckbox.IsChecked ?? false
            };
        }
    }
}
