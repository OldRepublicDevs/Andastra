using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Layout;
using BioWare.Tools;

namespace OdyTools.Dialogs
{
    /// <summary>
    /// Scope toggles for Holocron-style reference search (override/modules/chitin).
    /// </summary>
    public class ReferenceSearchOptionsDialog : Window
    {
        private readonly bool _showStrRefNcsOptions;
        private CheckBox _overrideCheckbox;
        private CheckBox _modulesCheckbox;
        private CheckBox _chitinCheckbox;
        private CheckBox _caseSensitiveCheckbox;
        private CheckBox _partialMatchCheckbox;
        private TextBox _moduleGlobTextBox;
        private CheckBox _ncsScanCheckbox;
        private TextBox _ncsMinTextBox;
        private bool _accepted;

        public ReferenceSearchOptionsDialog() : this(null, false)
        {
        }

        public ReferenceSearchOptionsDialog(Window parent) : this(parent, false)
        {
        }

        public ReferenceSearchOptionsDialog(Window parent, bool showStrRefNcsOptions)
        {
            _showStrRefNcsOptions = showStrRefNcsOptions;
            Title = "Reference Search Options";
            Width = 360;
            Height = showStrRefNcsOptions ? 420 : 340;
            WindowStartupLocation = parent != null
                ? WindowStartupLocation.CenterOwner
                : WindowStartupLocation.CenterScreen;

            _overrideCheckbox = new CheckBox { Content = "Search Override", IsChecked = true };
            _modulesCheckbox = new CheckBox { Content = "Search Modules", IsChecked = true };
            _moduleGlobTextBox = new TextBox
            {
                AcceptsReturn = true,
                MinHeight = 48,
                Watermark = "e.g. tar_m02*, danm13* (comma or newline separated)"
            };
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
            panel.Children.Add(new TextBlock { Text = "Module filename globs (optional)" });
            panel.Children.Add(_moduleGlobTextBox);
            panel.Children.Add(_chitinCheckbox);
            _modulesCheckbox.IsCheckedChanged += (s, e) => UpdateModuleGlobFieldEnabled();
            panel.Children.Add(new TextBlock { Text = "Matching", FontWeight = Avalonia.Media.FontWeight.SemiBold, Margin = new Avalonia.Thickness(0, 8, 0, 0) });
            panel.Children.Add(_caseSensitiveCheckbox);
            panel.Children.Add(_partialMatchCheckbox);

            if (_showStrRefNcsOptions)
            {
                _ncsScanCheckbox = new CheckBox { Content = "Scan NCS bytecode (CONSTI)", IsChecked = true };
                _ncsMinTextBox = new TextBox { Watermark = "Default 100" };
                panel.Children.Add(new TextBlock { Text = "StrRef / NCS", FontWeight = Avalonia.Media.FontWeight.SemiBold, Margin = new Avalonia.Thickness(0, 8, 0, 0) });
                panel.Children.Add(_ncsScanCheckbox);
                panel.Children.Add(new TextBlock { Text = "Minimum CONSTI for cache indexing (blank = default)" });
                panel.Children.Add(_ncsMinTextBox);
            }

            panel.Children.Add(buttonRow);
            Content = panel;
            UpdateModuleGlobFieldEnabled();
        }

        public void SetDefaults(ReferenceSearchOptions defaults)
        {
            if (defaults == null)
            {
                return;
            }

            _overrideCheckbox.IsChecked = defaults.SearchOverride;
            _modulesCheckbox.IsChecked = defaults.SearchModules;
            _chitinCheckbox.IsChecked = defaults.SearchChitin;
            _caseSensitiveCheckbox.IsChecked = defaults.CaseSensitive;
            _partialMatchCheckbox.IsChecked = defaults.PartialMatch;
            _moduleGlobTextBox.Text = FormatModuleGlobFilters(defaults.ModuleGlobFilters);

            if (_showStrRefNcsOptions && _ncsScanCheckbox != null)
            {
                _ncsScanCheckbox.IsChecked = defaults.IncludeNcsStrRefScan;
                _ncsMinTextBox.Text = defaults.NcsStrRefCandidateMinimum.HasValue
                    ? defaults.NcsStrRefCandidateMinimum.Value.ToString()
                    : string.Empty;
            }

            UpdateModuleGlobFieldEnabled();
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

        public ReferenceSearchOptions ToSearchOptions()
        {
            var options = new ReferenceSearchOptions
            {
                SearchOverride = _overrideCheckbox.IsChecked ?? true,
                SearchModules = _modulesCheckbox.IsChecked ?? true,
                SearchChitin = _chitinCheckbox.IsChecked ?? true,
                CaseSensitive = _caseSensitiveCheckbox.IsChecked ?? false,
                PartialMatch = _partialMatchCheckbox.IsChecked ?? false,
                ModuleGlobFilters = ParseModuleGlobFilters(_moduleGlobTextBox.Text)
            };

            if (_showStrRefNcsOptions && _ncsScanCheckbox != null)
            {
                options.IncludeNcsStrRefScan = _ncsScanCheckbox.IsChecked ?? true;
                string minText = _ncsMinTextBox == null ? string.Empty : _ncsMinTextBox.Text;
                if (string.IsNullOrWhiteSpace(minText))
                {
                    options.NcsStrRefCandidateMinimum = null;
                }
                else if (int.TryParse(minText.Trim(), out int minValue) && minValue >= 0)
                {
                    options.NcsStrRefCandidateMinimum = minValue;
                }
            }

            return options;
        }

        private void UpdateModuleGlobFieldEnabled()
        {
            bool modulesEnabled = _modulesCheckbox.IsChecked ?? false;
            _moduleGlobTextBox.IsEnabled = modulesEnabled;
        }

        public static List<string> ParseModuleGlobFilters(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var patterns = new List<string>();
            foreach (string part in text.Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries))
            {
                string trimmed = part.Trim();
                if (trimmed.Length > 0)
                {
                    patterns.Add(trimmed);
                }
            }

            return patterns.Count > 0 ? patterns : null;
        }

        internal static string FormatModuleGlobFilters(IList<string> filters)
        {
            if (filters == null || filters.Count == 0)
            {
                return string.Empty;
            }

            var parts = new List<string>();
            for (int i = 0; i < filters.Count; i++)
            {
                string trimmed = filters[i] == null ? string.Empty : filters[i].Trim();
                if (trimmed.Length > 0)
                {
                    parts.Add(trimmed);
                }
            }

            return parts.Count > 0 ? string.Join(", ", parts) : string.Empty;
        }
    }
}
