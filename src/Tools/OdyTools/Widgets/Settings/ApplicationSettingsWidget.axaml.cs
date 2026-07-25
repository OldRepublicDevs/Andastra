using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Editors;
using OdyTools.Utils;
using FontInfo = OdyTools.Dialogs.FontInfo;

namespace OdyTools.Widgets.Settings
{
    public partial class ApplicationSettingsWidget : UserControl
    {
        private Button _resetAttributesButton;
        private TextBlock _currentFontLabel;
        private Button _fontButton;
        private DataGrid _tableWidget;
        private Button _addButton;
        private Button _editButton;
        private Button _removeButton;
        private StackPanel _verticalLayoutMisc;
        private StackPanel _verticalLayout3;
        private GlobalSettings _settings;
        private List<EnvironmentVariable> _environmentVariables;
        private CheckBox _backupsEnabledCheck;
        private NumericUpDown _backupCountSpin;
        private CheckBox _crashRecoveryEnabledCheck;
        private NumericUpDown _crashRecoveryIntervalSpin;

        public ApplicationSettingsWidget()
        {
            InitializeComponent();
            _settings = new GlobalSettings();
            _environmentVariables = new List<EnvironmentVariable>();
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ApplicationSettingsWidget: XAML load failed: {ex.Message}");
            }

            if (xamlLoaded)
            {
                _resetAttributesButton = EditorHelpers.FindControlSafe<Button>(this, "resetAttributesButton");
                _currentFontLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "currentFontLabel");
                _fontButton = EditorHelpers.FindControlSafe<Button>(this, "fontButton");
                _tableWidget = EditorHelpers.FindControlSafe<DataGrid>(this, "tableWidget");
                _addButton = EditorHelpers.FindControlSafe<Button>(this, "addButton");
                _editButton = EditorHelpers.FindControlSafe<Button>(this, "editButton");
                _removeButton = EditorHelpers.FindControlSafe<Button>(this, "removeButton");
                _verticalLayoutMisc = EditorHelpers.FindControlSafe<StackPanel>(this, "verticalLayout_misc");
                _verticalLayout3 = EditorHelpers.FindControlSafe<StackPanel>(this, "verticalLayout_3");
                if (_tableWidget != null)
                {
                    ConfigureDataGridColumns();
                }
            }

            if (_resetAttributesButton == null || _currentFontLabel == null || _fontButton == null || _tableWidget == null ||
                _addButton == null || _editButton == null || _removeButton == null || _verticalLayoutMisc == null || _verticalLayout3 == null)
            {
                SetupProgrammaticUI();
            }
        }

        private void SetupProgrammaticUI()
        {
            var root = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto,*")
            };

            _resetAttributesButton = new Button
            {
                Name = "resetAttributesButton",
                Content = "Reset All on this Page",
                Height = 50,
                Margin = new Thickness(0, 0, 0, 10)
            };
            Grid.SetRow(_resetAttributesButton, 0);
            root.Children.Add(_resetAttributesButton);

            var scroll = new ScrollViewer();
            Grid.SetRow(scroll, 1);
            var stack = new StackPanel { Spacing = 10 };
            scroll.Content = stack;

            var fontPanel = new StackPanel { Spacing = 5, Margin = new Thickness(5) };
            _currentFontLabel = new TextBlock { Name = "currentFontLabel", Text = "Current Font: Default" };
            _fontButton = new Button { Name = "fontButton", Content = "Select Font...", HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left };
            fontPanel.Children.Add(_currentFontLabel);
            fontPanel.Children.Add(_fontButton);
            stack.Children.Add(new Expander
            {
                Header = "Global Font Settings",
                IsExpanded = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Content = fontPanel
            });

            var envGrid = new Grid
            {
                RowDefinitions = new RowDefinitions("*,Auto"),
                Margin = new Thickness(5)
            };
            _tableWidget = new DataGrid { Name = "tableWidget", AutoGenerateColumns = true, Margin = new Thickness(0, 0, 0, 5) };
            Grid.SetRow(_tableWidget, 0);
            envGrid.Children.Add(_tableWidget);
            var envButtons = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
            _addButton = new Button { Name = "addButton", Content = "Add" };
            _editButton = new Button { Name = "editButton", Content = "Edit" };
            _removeButton = new Button { Name = "removeButton", Content = "Remove" };
            envButtons.Children.Add(_addButton);
            envButtons.Children.Add(_editButton);
            envButtons.Children.Add(_removeButton);
            Grid.SetRow(envButtons, 1);
            envGrid.Children.Add(envButtons);
            stack.Children.Add(new Expander
            {
                Header = "Environment Variables",
                IsExpanded = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Content = envGrid
            });

            _verticalLayoutMisc = new StackPanel { Name = "verticalLayout_misc", Spacing = 5, Margin = new Thickness(5) };
            stack.Children.Add(new Expander
            {
                Header = "Miscellaneous Settings",
                IsExpanded = true,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Content = _verticalLayoutMisc
            });

            _verticalLayout3 = new StackPanel { Name = "verticalLayout_3", Spacing = 5, Margin = new Thickness(5) };
            stack.Children.Add(new Expander
            {
                Header = "Experimental settings (may cause app crashes)",
                IsExpanded = false,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                Content = _verticalLayout3
            });

            root.Children.Add(scroll);
            Content = root;
            ConfigureDataGridColumns();
        }

        private void ConfigureDataGridColumns()
        {
            if (_tableWidget == null) return;
            _tableWidget.AutoGenerateColumns = false;
            _tableWidget.Columns.Clear();
            _tableWidget.Columns.Add(new DataGridTextColumn { Header = "Key", Binding = new Avalonia.Data.Binding("Key") });
            _tableWidget.Columns.Add(new DataGridTextColumn { Header = "Value", Binding = new Avalonia.Data.Binding("Value") });
        }

        private void SetupUI()
        {
            if (_resetAttributesButton != null)
            {
                _resetAttributesButton.Click += (s, e) => ResetAttributes();
            }
            if (_fontButton != null)
            {
                _fontButton.Click += (s, e) => SelectFont();
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

            UpdateFontLabel();
            PopulateAll();
        }

        private void UpdateFontLabel()
        {
            if (_currentFontLabel == null)
            {
                return;
            }

            string fontString = _settings.GlobalFont;
            if (!string.IsNullOrEmpty(fontString))
            {
                try
                {
                    // Parse font string (format: "Family,Size,Style,Weight" or similar)
                    // For simplicity, we'll store as "Family|Size|Style|Weight"
                    var parts = fontString.Split('|');
                    if (parts.Length >= 2)
                    {
                        string family = parts[0];
                        if (double.TryParse(parts[1], out double size))
                        {
                            _currentFontLabel.Text = $"Current Font: {family}, {size} pt";
                            return;
                        }
                    }
                }
                catch
                {
                    // If parsing fails, fall through to default
                }
            }

            _currentFontLabel.Text = "Current Font: Default";
        }

        private async void SelectFont()
        {
            Window parentWindow = ControlTreeHelper.GetParentWindow(this);
            if (parentWindow == null)
            {
                return;
            }

            // Get current font from settings or use default
            FontInfo currentFont = null;
            string fontString = _settings.GlobalFont;
            if (!string.IsNullOrEmpty(fontString))
            {
                try
                {
                    // Parse font string (format: "Family|Size|Style|Weight")
                    var parts = fontString.Split('|');
                    if (parts.Length >= 2)
                    {
                        string family = parts[0].Trim();
                        if (double.TryParse(parts[1].Trim(), out double size))
                        {
                            bool isBold = false;
                            bool isItalic = false;

                            // Parse weight if available
                            if (parts.Length >= 4 && int.TryParse(parts[3].Trim(), out int weightValue))
                            {
                                isBold = weightValue >= 700;
                            }

                            // Parse style if available
                            if (parts.Length >= 3)
                            {
                                string styleStr = parts[2].Trim().ToLowerInvariant();
                                if (styleStr.Contains("italic"))
                                {
                                    isItalic = true;
                                }
                            }

                            currentFont = new FontInfo
                            {
                                FamilyName = family,
                                Size = size,
                                IsBold = isBold,
                                IsItalic = isItalic
                            };
                        }
                    }
                }
                catch
                {
                    // Use default font if parsing fails
                }
            }

            // Create and show font dialog
            var fontDialog = new FontDialog(parentWindow);
            if (currentFont != null)
            {
                fontDialog.SetCurrentFont(currentFont);
            }

            // Show dialog and wait for result
            await fontDialog.ShowDialog(parentWindow);

            // If user clicked OK, save the font
            if (fontDialog.DialogResult && fontDialog.SelectedFont != null)
            {
                var selectedFont = fontDialog.SelectedFont;

                // Save font as string (format: "Family|Size|Style|Weight")
                string fontStringToSave = $"{selectedFont.FamilyName ?? "Arial"}|{selectedFont.Size}|" +
                    $"{(selectedFont.IsItalic ? "Italic" : "Normal")}|" +
                    $"{(selectedFont.IsBold ? "700" : "400")}";

                _settings.GlobalFont = fontStringToSave;

                // Update the label
                UpdateFontLabel();

                // Apply the font globally to the application
                // In Avalonia, we apply fonts via styles to achieve the same effect
                OdyTools.Utils.FontApplicationHelper.ApplyGlobalFont(fontStringToSave);
            }
        }

        private void PopulateAll()
        {
            // Populate environment variables from settings
            PopulateEnvironmentVariables();

            PopulateSaveResilienceSettings();
        }

        private void PopulateSaveResilienceSettings()
        {
            if (_verticalLayoutMisc == null)
            {
                return;
            }

            _verticalLayoutMisc.Children.Clear();

            _verticalLayoutMisc.Children.Add(new TextBlock
            {
                Text = "Save & Recovery",
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(0, 0, 0, 4)
            });

            _verticalLayoutMisc.Children.Add(new TextBlock
            {
                Text = $"Autosave is always enabled (every {GlobalSettings.ManagedAutosaveIntervalMinutes} minutes) and stored as a managed working copy in app local data.",
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(0, 0, 0, 6)
            });

            _backupCountSpin = CreateNumericSettingRow(
                _verticalLayoutMisc,
                "Backup versions to keep:",
                1,
                50,
                1,
                _settings.MaxBackupCount,
                _settings.BackupsEnabled,
                value => _settings.MaxBackupCount = Math.Max(1, value),
                out _backupsEnabledCheck,
                "Create backups on save",
                _settings.BackupsEnabled,
                enabled => _settings.BackupsEnabled = enabled);

            _crashRecoveryIntervalSpin = CreateNumericSettingRow(
                _verticalLayoutMisc,
                "Crash recovery interval (seconds):",
                5,
                300,
                5,
                _settings.CrashRecoveryIntervalSeconds,
                _settings.CrashRecoveryEnabled,
                value => _settings.CrashRecoveryIntervalSeconds = Math.Max(5, value),
                out _crashRecoveryEnabledCheck,
                "Enable crash recovery",
                _settings.CrashRecoveryEnabled,
                enabled => _settings.CrashRecoveryEnabled = enabled);
        }

        private NumericUpDown CreateNumericSettingRow(
            Panel parent,
            string label,
            decimal min,
            decimal max,
            decimal increment,
            decimal value,
            bool enabled,
            Action<int> onValueChanged,
            out CheckBox toggle,
            string toggleLabel,
            bool toggleInitial,
            Action<bool> onToggleChanged)
        {
            var numeric = new NumericUpDown
            {
                Minimum = min,
                Maximum = max,
                Increment = increment,
                Value = value,
                Width = 90,
                IsEnabled = enabled
            };

            numeric.ValueChanged += (s, e) =>
            {
                onValueChanged?.Invoke(Convert.ToInt32(numeric.Value));
            };

            CheckBox localToggle = new CheckBox { Content = toggleLabel, IsChecked = toggleInitial };
            localToggle.IsCheckedChanged += (s, e) =>
            {
                bool isEnabled = localToggle.IsChecked == true;
                onToggleChanged?.Invoke(isEnabled);
                numeric.IsEnabled = isEnabled;
            };
            parent.Children.Add(localToggle);

            var row = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 8 };
            row.Children.Add(new TextBlock { Text = label, VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center });
            row.Children.Add(numeric);
            parent.Children.Add(row);

            toggle = localToggle;

            return numeric;
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

        private void ResetAttributes()
        {
            // Reset all attributes to defaults
            // Settings binding (when settings are fully available)
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

    }
}
