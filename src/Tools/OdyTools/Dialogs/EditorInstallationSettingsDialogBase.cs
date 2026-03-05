using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using OdyTools.Data;
using OdyTools.Widgets.Settings;
using OdyTools.Utils;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Dialogs
{
    /// <summary>
    /// Base class for editor settings dialogs that include installation selection, auto-detect, and add new installation.
    /// Subclasses add their specific settings UI below the installation combo.
    /// </summary>
    public abstract class EditorInstallationSettingsDialogBase : Window
    {
        protected const string NoneItem = "(None)";
        protected const string AutoDetectItem = "Auto-detect paths now";
        protected const string AddNewInstallationItem = "Add new installation…";

        protected ComboBox InstallationCombo { get; private set; }
        protected List<string> InstallationNames { get; } = new List<string>();
        protected int AutoDetectIndex { get; private set; } = -1;
        protected int AddNewInstallationIndex { get; private set; } = -1;

        private bool _handlingSpecialSelection;

        public bool? Result { get; protected set; }

        protected EditorInstallationSettingsDialogBase()
        {
        }

        protected void InitializeInstallationSection(ComboBox installationCombo, Button okButton, Button cancelButton)
        {
            InstallationCombo = installationCombo;
            if (InstallationCombo != null)
                InstallationCombo.SelectionChanged += OnInstallationComboSelectionChanged;
            if (okButton != null)
                okButton.Click += (s, e) => Accept();
            if (cancelButton != null)
                cancelButton.Click += (s, e) => { Result = false; Close(); };

            LoadInstallationNames();
        }

        protected static string GetInstallationPath(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            try
            {
                var installations = new GlobalSettings().Installations();
                if (installations == null || !installations.ContainsKey(name)) return null;
                var installData = installations[name];
                return installData != null && installData.ContainsKey("path")
                    ? installData["path"]?.ToString()?.Trim()
                    : null;
            }
            catch { return null; }
        }

        protected void LoadInstallationNames()
        {
            if (InstallationCombo == null) return;
            InstallationNames.Clear();
            InstallationCombo.Items.Clear();
            InstallationCombo.Items.Add(NoneItem);
            try
            {
                var installations = new GlobalSettings().Installations();
                if (installations != null)
                {
                    foreach (var key in installations.Keys)
                    {
                        if (!string.IsNullOrWhiteSpace(key))
                            InstallationNames.Add(key);
                    }
                }
            }
            catch { }
            foreach (var name in InstallationNames)
                InstallationCombo.Items.Add(name);
            InstallationCombo.Items.Add(AutoDetectItem);
            InstallationCombo.Items.Add(AddNewInstallationItem);
            AutoDetectIndex = InstallationNames.Count + 1;
            AddNewInstallationIndex = InstallationNames.Count + 2;
        }

        private async void OnInstallationComboSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_handlingSpecialSelection || InstallationCombo?.SelectedIndex < 0)
                return;
            int idx = InstallationCombo.SelectedIndex;
            if (idx == AutoDetectIndex)
            {
                _handlingSpecialSelection = true;
                try
                {
                    int added = new GlobalSettings().MergeDetectedInstallationsFromDefault();
                    LoadInstallationNames();
                    InstallationCombo.SelectedIndex = InstallationNames.Count > 0 ? 1 : 0;
                    await DialogHelper.ShowWindowAsync(this, "Auto-detect", added > 0
                            ? $"Found and added {added} installation(s) from default paths."
                            : "No new installations found. Existing installations were left unchanged.", ButtonEnum.Ok, IconType.Info);
                }
                finally
                {
                    _handlingSpecialSelection = false;
                }
            }
            else if (idx == AddNewInstallationIndex)
            {
                _handlingSpecialSelection = true;
                try
                {
                    await RunAddNewInstallationAsync();
                    LoadInstallationNames();
                    InstallationCombo.SelectedIndex = InstallationNames.Count > 0 ? 1 : 0;
                }
                finally
                {
                    _handlingSpecialSelection = false;
                }
            }
            else
            {
                OnInstallationSelectionChanged();
            }
        }

        /// <summary>Opens the Add new installation dialog. Override to customize.</summary>
        protected virtual async Task RunAddNewInstallationAsync()
        {
            var installWidget = new InstallationsWidget();
            var addWin = new Window
            {
                Title = "Add new installation",
                Width = 520,
                Height = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            grid.RowDefinitions.Add(new RowDefinition(GridLength.Auto));
            var scroll = new ScrollViewer { Content = installWidget };
            Grid.SetRow(scroll, 0);
            grid.Children.Add(scroll);
            var okBtn = new Button { Content = "OK", Width = 80 };
            var cancelBtn = new Button { Content = "Cancel", Width = 80 };
            cancelBtn.Click += (s, _) => addWin.Close(false);
            okBtn.Click += (s, _) =>
            {
                installWidget.Save();
                addWin.Close(true);
            };
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Spacing = 8,
                Margin = new Thickness(10)
            };
            buttonPanel.Children.Add(okBtn);
            buttonPanel.Children.Add(cancelBtn);
            Grid.SetRow(buttonPanel, 1);
            grid.Children.Add(buttonPanel);
            addWin.Content = grid;
            await addWin.ShowDialog(this);
        }

        /// <summary>Called when user selects a normal installation (not Auto-detect or Add new). Override to update placeholders etc.</summary>
        protected virtual void OnInstallationSelectionChanged()
        {
        }

        protected virtual void Accept()
        {
            SaveValues();
            Result = true;
            Close();
        }

        protected abstract void SaveValues();

        protected async Task BrowseForFileAsync(TextBox target, string suggestedName, string[] patterns = null)
        {
            var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(this);
            if (topLevel?.StorageProvider == null) return;
            var storage = topLevel.StorageProvider;
            if (patterns == null || patterns.Length == 0)
                patterns = new[] { "*.tlk", "*.2da", "*.*" };
            var options = new FilePickerOpenOptions
            {
                Title = "Select file",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType(suggestedName) { Patterns = patterns } }
            };
            if (!string.IsNullOrWhiteSpace(target?.Text) && File.Exists(target.Text))
            {
                try
                {
                    var dir = Path.GetDirectoryName(target.Text);
                    if (!string.IsNullOrEmpty(dir))
                    {
                        var folder = await storage.TryGetFolderFromPathAsync(dir);
                        if (folder != null)
                            options.SuggestedStartLocation = folder;
                    }
                }
                catch { }
            }
            var files = await storage.OpenFilePickerAsync(options);
            if (files != null && files.Count > 0 && target != null)
            {
                var path = files[0].TryGetLocalPath();
                if (!string.IsNullOrEmpty(path))
                    target.Text = path;
            }
        }

        protected async Task BrowseForFolderAsync(TextBox target, string title = "Select folder")
        {
            var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(this);
            if (topLevel?.StorageProvider == null) return;
            var storage = topLevel.StorageProvider;
            if (storage == null) return;
            var options = new FolderPickerOpenOptions { Title = title, AllowMultiple = false };
            if (!string.IsNullOrWhiteSpace(target?.Text) && Directory.Exists(target.Text))
            {
                try
                {
                    var folder = await storage.TryGetFolderFromPathAsync(target.Text);
                    if (folder != null)
                        options.SuggestedStartLocation = folder;
                }
                catch { }
            }
            var folders = await storage.OpenFolderPickerAsync(options);
            if (folders != null && folders.Count > 0 && target != null)
            {
                var path = folders[0].TryGetLocalPath();
                if (!string.IsNullOrEmpty(path))
                    target.Text = path;
            }
        }
    }
}
