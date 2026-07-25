using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Editors;
using OdyTools.Utils;

namespace OdyTools.Widgets.Settings
{
    public partial class EnvVarsWidget : UserControl
    {
        private DataGrid _tableWidget;
        private Button _addButton;
        private Button _editButton;
        private Button _removeButton;
        private GlobalSettings _settings;
        private List<EnvironmentVariable> _environmentVariables;
        private bool _eventsAttached;

        public EnvVarsWidget()
        {
            InitializeComponent();
            _settings = new GlobalSettings();
            _environmentVariables = new List<EnvironmentVariable>();
            SetupUI();
            PopulateEnvironmentVariables();
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
            var panel = new StackPanel { Spacing = 10, Margin = new Avalonia.Thickness(10) };

            _tableWidget = new DataGrid
            {
                Name = "tableWidget",
                AutoGenerateColumns = false,
                CanUserReorderColumns = true,
                CanUserResizeColumns = true,
                CanUserSortColumns = true
            };
            _tableWidget.Columns.Add(new DataGridTextColumn { Header = "Key", Binding = new Avalonia.Data.Binding("Key") });
            _tableWidget.Columns.Add(new DataGridTextColumn { Header = "Value", Binding = new Avalonia.Data.Binding("Value") });

            _addButton = new Button { Name = "addButton", Content = "Add" };
            _addButton.Click += (s, e) => AddEnvironmentVariable();
            _editButton = new Button { Name = "editButton", Content = "Edit" };
            _editButton.Click += (s, e) => EditEnvironmentVariable();
            _removeButton = new Button { Name = "removeButton", Content = "Remove" };
            _removeButton.Click += (s, e) => RemoveEnvironmentVariable();
            _eventsAttached = true;

            var buttonPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
            buttonPanel.Children.Add(_addButton);
            buttonPanel.Children.Add(_editButton);
            buttonPanel.Children.Add(_removeButton);

            panel.Children.Add(_tableWidget);
            panel.Children.Add(buttonPanel);
            Content = panel;
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _tableWidget = EditorHelpers.FindControlSafe<DataGrid>(this, "tableWidget") ?? _tableWidget;
            _addButton = EditorHelpers.FindControlSafe<Button>(this, "addButton") ?? _addButton;
            _editButton = EditorHelpers.FindControlSafe<Button>(this, "editButton") ?? _editButton;
            _removeButton = EditorHelpers.FindControlSafe<Button>(this, "removeButton") ?? _removeButton;

            if (_tableWidget == null || _addButton == null || _editButton == null || _removeButton == null)
            {
                SetupProgrammaticUI();
                return;
            }

            if (_eventsAttached)
            {
                return;
            }

            if (_addButton != null)
            {
                _addButton.Click += (s, e) => AddEnvironmentVariable();
            }
            if (_editButton != null)
            {
                _editButton.Click += (s, e) => EditEnvironmentVariable();
            }
            if (_removeButton != null)
            {
                _removeButton.Click += (s, e) => RemoveEnvironmentVariable();
            }
            _eventsAttached = true;
        }

        private void PopulateEnvironmentVariables()
        {
            if (_tableWidget == null)
            {
                return;
            }

            _environmentVariables = EnvironmentVariableGridHelper.FromDictionary(_settings.AppEnvVariables);
            EnvironmentVariableGridHelper.RefreshGrid(_tableWidget, _environmentVariables);
        }

        private async void AddEnvironmentVariable()
        {
            Window parentWindow = ControlTreeHelper.GetParentWindow(this);
            if (parentWindow == null)
            {
                return;
            }

            var dialog = new EnvVariableDialog(parentWindow);
            await dialog.ShowDialog(parentWindow);

            // Get data from dialog after it closes
            var result = dialog.GetData();
            if (result != null && !string.IsNullOrWhiteSpace(result.Item1))
            {
                string key = result.Item1.Trim();
                string value = result.Item2 ?? "";

                // Check if key already exists
                if (HasEnvironmentVariableKey(key))
                {
                    await DialogHelper.ShowWarningAsync("Duplicate Variable", $"An environment variable with key '{key}' already exists.");
                    return;
                }

                // Add to list
                var newVar = new EnvironmentVariable(key, value);
                _environmentVariables.Add(newVar);
                RefreshEnvironmentVariablesGrid();

                // Save to settings
                SaveEnvironmentVariable(key, value);
            }
        }

        private async void EditEnvironmentVariable()
        {
            EnvironmentVariable selectedVar;
            if (!TryGetSelectedEnvironmentVariable(out selectedVar))
            {
                await DialogHelper.ShowWarningAsync("Edit Variable", "Please select a variable to edit.");
                return;
            }

            Window parentWindow = ControlTreeHelper.GetParentWindow(this);
            if (parentWindow == null)
            {
                return;
            }

            string oldKey = selectedVar.Key;
            var dialog = new EnvVariableDialog(parentWindow);
            dialog.SetData(selectedVar.Key, selectedVar.Value);
            await dialog.ShowDialog(parentWindow);

            // Get data from dialog after it closes
            var result = dialog.GetData();
            if (result != null && !string.IsNullOrWhiteSpace(result.Item1))
            {
                string newKey = result.Item1.Trim();
                string newValue = result.Item2 ?? "";

                // If key changed, check for duplicates
                if (!oldKey.Equals(newKey, StringComparison.OrdinalIgnoreCase))
                {
                    if (HasEnvironmentVariableKey(newKey, selectedVar))
                    {
                        await DialogHelper.ShowWarningAsync("Duplicate Variable", $"An environment variable with key '{newKey}' already exists.");
                        return;
                    }

                    // Remove old key from settings
                    RemoveEnvironmentVariableFromSettings(oldKey);
                }

                // Update the variable
                selectedVar.Key = newKey;
                selectedVar.Value = newValue;

                // Refresh the DataGrid
                RefreshEnvironmentVariablesGrid();

                // Save to settings
                SaveEnvironmentVariable(newKey, newValue);
            }
        }

        private async void RemoveEnvironmentVariable()
        {
            EnvironmentVariable selectedVar;
            if (!TryGetSelectedEnvironmentVariable(out selectedVar))
            {
                await DialogHelper.ShowWarningAsync("Remove Variable", "Please select a variable to remove.");
                return;
            }

            string key = selectedVar.Key;

            // Remove from the list
            _environmentVariables.Remove(selectedVar);
            RefreshEnvironmentVariablesGrid();

            // Remove from settings
            RemoveEnvironmentVariableFromSettings(key);
        }

        private void RefreshEnvironmentVariablesGrid()
        {
            EnvironmentVariableGridHelper.RefreshGrid(_tableWidget, _environmentVariables);
        }

        private bool TryGetSelectedEnvironmentVariable(out EnvironmentVariable selected)
        {
            return EnvironmentVariableGridHelper.TryGetSelected(_tableWidget, out selected);
        }

        private bool HasEnvironmentVariableKey(string key, EnvironmentVariable except = null)
        {
            return EnvironmentVariableGridHelper.HasKey(_environmentVariables, key, except);
        }

        private void RemoveEnvironmentVariableFromSettings(string key)
        {
            EnvironmentVariableGridHelper.RemoveSetting(_settings, key);
        }

        private void SaveEnvironmentVariable(string key, string value)
        {
            EnvironmentVariableGridHelper.UpsertSetting(_settings, key, value);
        }

        public void Save()
        {
            _settings.AppEnvVariables = EnvironmentVariableGridHelper.ToDictionary(_environmentVariables);
        }
    }
}
