using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using OdyTools.Editors;

namespace OdyTools.Widgets
{
    public partial class CommandPalette : Window
    {
        private Dictionary<string, Dictionary<string, object>> _commands;
        private ListBox _commandList;
        private TextBox _searchEdit;
        private TextBlock _statusLabel;
        private List<string> _filteredCommandIds;

        public event Action<string> CommandSelected;

        public CommandPalette(Window parent = null)
        {
            InitializeComponent();
            _commands = new Dictionary<string, Dictionary<string, object>>();
            _filteredCommandIds = new List<string>();
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
            Title = "Command Palette";
            MinWidth = 500;
            MaxWidth = 700;
            Width = 600;
            Height = 400;

            var panel = new StackPanel { Margin = new Avalonia.Thickness(0), Spacing = 0 };

            _searchEdit = new TextBox
            {
                Name = "searchEdit",
                Watermark = "Type to search commands...",
                Margin = new Avalonia.Thickness(8)
            };
            _searchEdit.TextChanged += (s, e) => FilterCommands();
            _searchEdit.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    ExecuteSelected();
                }
            };

            _commandList = new ListBox { Name = "commandList" };
            _commandList.DoubleTapped += (s, e) => OnItemDoubleClicked();
            _commandList.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter)
                {
                    ExecuteSelected();
                }
            };

            _statusLabel = new TextBlock
            {
                Name = "statusLabel",
                Text = "",
                Margin = new Avalonia.Thickness(4, 8, 4, 8)
            };

            panel.Children.Add(_searchEdit);
            panel.Children.Add(_commandList);
            panel.Children.Add(_statusLabel);
            Content = panel;
        }

        private void SetupUI()
        {
            // If already set up programmatically, skip
            if (_searchEdit != null && _commandList != null && _statusLabel != null)
            {
                return;
            }

            // Find controls from XAML (may fail if not in visual tree)
            try
            {
                _searchEdit = EditorHelpers.FindControlSafe<TextBox>(this, "searchEdit");
                _commandList = EditorHelpers.FindControlSafe<ListBox>(this, "commandList");
                _statusLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "statusLabel");
            }
            catch (InvalidOperationException)
            {
                // Not in a visual tree (e.g., in unit tests) - will create programmatically
                _searchEdit = null;
                _commandList = null;
                _statusLabel = null;
            }

            // If not found in XAML, create programmatically (only if not already created)
            if (_searchEdit == null)
            {
                _searchEdit = new TextBox
                {
                    Name = "searchEdit",
                    Watermark = "Type to search commands...",
                    Margin = new Avalonia.Thickness(8)
                };
            }
            if (_commandList == null)
            {
                _commandList = new ListBox { Name = "commandList" };
            }
            if (_statusLabel == null)
            {
                _statusLabel = new TextBlock
                {
                    Name = "statusLabel",
                    Text = "",
                    Margin = new Avalonia.Thickness(4, 8, 4, 8)
                };
            }

            if (_searchEdit != null)
            {
                _searchEdit.TextChanged += (s, e) => FilterCommands();
                _searchEdit.KeyDown += (s, e) =>
                {
                    if (e.Key == Key.Enter)
                    {
                        ExecuteSelected();
                    }
                };
            }
            if (_commandList != null)
            {
                _commandList.DoubleTapped += (s, e) => OnItemDoubleClicked();
            }
        }

        public void RegisterCommand(string commandId, string label, Action callback, string category = null)
        {
            _commands[commandId] = new Dictionary<string, object>
            {
                { "label", label },
                { "callback", callback },
                { "category", category ?? "" }
            };
            FilterCommands();
        }

        private void FilterCommands()
        {
            if (_commandList == null || _searchEdit == null)
            {
                return;
            }

            string searchText = _searchEdit.Text?.ToLowerInvariant() ?? "";
            _filteredCommandIds.Clear();
            _commandList.Items.Clear();

            foreach (var kvp in _commands)
            {
                string label = kvp.Value.ContainsKey("label") ? kvp.Value["label"]?.ToString() ?? "" : "";
                if (string.IsNullOrEmpty(searchText) || label.ToLowerInvariant().Contains(searchText))
                {
                    _filteredCommandIds.Add(kvp.Key);
                    _commandList.Items.Add(label);
                }
            }

            if (_statusLabel != null)
            {
                _statusLabel.Text = $"{_filteredCommandIds.Count} command(s)";
            }
        }

        private void ExecuteSelected()
        {
            if (_commandList == null)
            {
                return;
            }

            int selectedIndex = _commandList.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < _filteredCommandIds.Count)
            {
                string commandId = _filteredCommandIds[selectedIndex];
                CommandSelected?.Invoke(commandId);
                if (_commands.ContainsKey(commandId) && _commands[commandId].ContainsKey("callback"))
                {
                    var callback = _commands[commandId]["callback"] as Action;
                    callback?.Invoke();
                }
                Close();
            }
        }

        private void OnItemDoubleClicked()
        {
            ExecuteSelected();
        }

        public void ShowPalette()
        {
            Show();
            if (_searchEdit != null)
            {
                _searchEdit.Focus();
            }
        }
    }
}
