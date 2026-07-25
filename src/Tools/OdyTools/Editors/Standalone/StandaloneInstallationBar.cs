using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using BioWare.Common;
using OdyTools.Data;
using OdyTools.Utils;
using OdyTools.Widgets.Settings;

namespace OdyTools.Editors.Standalone
{
    internal static class StandaloneInstallationBar
    {
        private const string NoneItem = "(No installation)";
        private const string BrowseItem = "Browse for game folder...";
        private const string AutoDetectItem = "Auto-detect installs";
        private const string ManageItem = "Manage installations...";

        public static void Attach(Editor editor)
        {
            if (editor == null || editor.Content == null)
            {
                return;
            }

            if (EditorHelpers.FindControlSafe<Control>(editor, "standaloneInstallationBar") != null)
            {
                return;
            }

            var original = editor.Content as Control;
            if (original == null)
            {
                return;
            }

            editor.Content = null;

            var dock = new DockPanel();
            var bar = new InstallationBarControl(editor);
            DockPanel.SetDock(bar, Dock.Top);
            dock.Children.Add(bar);
            dock.Children.Add(original);
            editor.Content = dock;
        }

        internal static List<string> BuildLaunchArguments(string key, string openPath, OdyInstallation installation)
        {
            var args = new List<string> { "--editor", key, "--theme", "light" };

            if (!string.IsNullOrWhiteSpace(openPath) && File.Exists(openPath))
            {
                args.Add("--open");
                args.Add(openPath);
            }

            if (installation != null && !string.IsNullOrWhiteSpace(installation.Path))
            {
                args.Add("--game-path");
                args.Add(installation.Path);
                args.Add(installation.Tsl ? "--tsl" : "--k1");
            }

            return args;
        }

        private sealed class InstallationBarControl : Border
        {
            private readonly Editor _editor;
            private readonly ComboBox _editorCombo;
            private readonly ComboBox _installationCombo;
            private readonly TextBlock _pathText;
            private readonly CheckBox _tslCheck;
            private bool _loading;
            private List<EditorLaunchInfo> _editorChoices = new List<EditorLaunchInfo>();
            private List<string> _installationNames = new List<string>();

            public InstallationBarControl(Editor editor)
            {
                _editor = editor;
                Name = "standaloneInstallationBar";
                Padding = new Thickness(10, 6);
                BorderThickness = new Thickness(0, 0, 0, 1);
                BorderBrush = Avalonia.Media.Brushes.LightGray;

                var grid = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions("Auto,180,Auto,220,Auto,Auto,*,Auto"),
                    RowDefinitions = new RowDefinitions("Auto"),
                    ColumnSpacing = 8,
                    VerticalAlignment = VerticalAlignment.Center
                };

                grid.Children.Add(new TextBlock
                {
                    Text = "Editor:",
                    VerticalAlignment = VerticalAlignment.Center
                });

                _editorCombo = new ComboBox
                {
                    Name = "standaloneEditorCombo",
                    MinWidth = 160,
                    MaxWidth = 220
                };
                _editorCombo.SelectionChanged += OnEditorSelectionChanged;
                Grid.SetColumn(_editorCombo, 1);
                grid.Children.Add(_editorCombo);

                var installLabel = new TextBlock
                {
                    Text = "Installation:",
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(installLabel, 2);
                grid.Children.Add(installLabel);

                _installationCombo = new ComboBox
                {
                    Name = "standaloneInstallationCombo",
                    MinWidth = 220,
                    MaxWidth = 260
                };
                _installationCombo.SelectionChanged += OnInstallationSelectionChanged;
                Grid.SetColumn(_installationCombo, 3);
                grid.Children.Add(_installationCombo);

                var browseButton = new Button
                {
                    Name = "standaloneBrowseInstallationButton",
                    Content = "Browse..."
                };
                browseButton.Click += (s, e) => _ = BrowseForInstallationAsync();
                Grid.SetColumn(browseButton, 4);
                grid.Children.Add(browseButton);

                _tslCheck = new CheckBox
                {
                    Name = "standaloneTslInstallationCheck",
                    Content = "TSL",
                    VerticalAlignment = VerticalAlignment.Center
                };
                _tslCheck.IsCheckedChanged += (s, e) => ReapplyCurrentPath();
                Grid.SetColumn(_tslCheck, 5);
                grid.Children.Add(_tslCheck);

                _pathText = new TextBlock
                {
                    Name = "standaloneInstallationPathText",
                    TextTrimming = Avalonia.Media.TextTrimming.CharacterEllipsis,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(_pathText, 6);
                grid.Children.Add(_pathText);

                var manageButton = new Button
                {
                    Name = "standaloneManageInstallationsButton",
                    Content = "Manage..."
                };
                manageButton.Click += (s, e) => _ = ManageInstallationsAsync();
                Grid.SetColumn(manageButton, 7);
                grid.Children.Add(manageButton);

                Child = grid;
                ReloadEditors();
                ReloadInstallations(selectCurrent: true);
            }

            private void ReloadEditors()
            {
                _loading = true;
                try
                {
                    _editorChoices = new List<EditorLaunchInfo>(StandaloneEditorRouting.KnownEditors());
                    _editorCombo.Items.Clear();
                    foreach (var choice in _editorChoices)
                    {
                        _editorCombo.Items.Add(choice.Label);
                    }

                    var currentKey = InferEditorKey(_editor);
                    var index = _editorChoices.FindIndex(info => string.Equals(info.Key, currentKey, StringComparison.OrdinalIgnoreCase));
                    _editorCombo.SelectedIndex = index >= 0 ? index : 0;
                }
                finally
                {
                    _loading = false;
                }
            }

            private void ReloadInstallations(bool selectCurrent)
            {
                _loading = true;
                try
                {
                    _installationNames = new List<string>();
                    _installationCombo.Items.Clear();
                    _installationCombo.Items.Add(NoneItem);

                    var installations = ReadSavedInstallations();
                    foreach (var kvp in installations)
                    {
                        if (!string.IsNullOrWhiteSpace(kvp.Key))
                        {
                            _installationNames.Add(kvp.Key);
                            _installationCombo.Items.Add(kvp.Key);
                        }
                    }

                    _installationCombo.Items.Add(BrowseItem);
                    _installationCombo.Items.Add(AutoDetectItem);
                    _installationCombo.Items.Add(ManageItem);

                    var current = _editor.Installation;
                    var index = 0;
                    if (selectCurrent && current != null)
                    {
                        index = _installationNames.FindIndex(name => string.Equals(name, current.Name, StringComparison.OrdinalIgnoreCase)) + 1;
                        if (index <= 0)
                        {
                            _installationCombo.Items.Insert(1, current.Name);
                            _installationNames.Insert(0, current.Name);
                            index = 1;
                        }
                    }

                    _installationCombo.SelectedIndex = index;
                    RefreshPathText();
                }
                finally
                {
                    _loading = false;
                }
            }

            private async void OnInstallationSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (_loading || _installationCombo.SelectedIndex < 0)
                {
                    return;
                }

                var index = _installationCombo.SelectedIndex;
                if (index == 0)
                {
                    _editor.SetStandaloneInstallation(null);
                    RefreshPathText();
                    return;
                }

                if (index <= _installationNames.Count)
                {
                    ApplyNamedInstallation(_installationNames[index - 1]);
                    return;
                }

                var item = _installationCombo.SelectedItem?.ToString();
                if (string.Equals(item, BrowseItem, StringComparison.Ordinal))
                {
                    await BrowseForInstallationAsync();
                }
                else if (string.Equals(item, AutoDetectItem, StringComparison.Ordinal))
                {
                    new GlobalSettings().MergeDetectedInstallationsFromDefault();
                    ReloadInstallations(selectCurrent: true);
                }
                else if (string.Equals(item, ManageItem, StringComparison.Ordinal))
                {
                    await ManageInstallationsAsync();
                }
            }

            private async void OnEditorSelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                if (_loading || _editorCombo.SelectedIndex < 0 || _editorCombo.SelectedIndex >= _editorChoices.Count)
                {
                    return;
                }

                var selected = _editorChoices[_editorCombo.SelectedIndex];
                var currentKey = InferEditorKey(_editor);
                if (string.Equals(selected.Key, currentKey, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                if (!await LaunchEditorAsync(selected.Key))
                {
                    _loading = true;
                    try
                    {
                        var index = _editorChoices.FindIndex(info => string.Equals(info.Key, currentKey, StringComparison.OrdinalIgnoreCase));
                        _editorCombo.SelectedIndex = index >= 0 ? index : 0;
                    }
                    finally
                    {
                        _loading = false;
                    }
                }
            }

            private void ApplyNamedInstallation(string name)
            {
                try
                {
                    var installations = ReadSavedInstallations();
                    if (installations == null || !installations.ContainsKey(name))
                    {
                        _editor.SetStandaloneInstallation(null);
                        RefreshPathText();
                        return;
                    }

                    var data = installations[name];
                    var path = data != null && data.ContainsKey("path") ? data["path"]?.ToString() : null;
                    var tsl = ReadTsl(data);
                    ApplyPath(path, name, tsl);
                }
                catch
                {
                    _editor.SetStandaloneInstallation(null);
                    RefreshPathText();
                }
            }

            private async Task BrowseForInstallationAsync()
            {
                var topLevel = TopLevel.GetTopLevel(_editor);
                if (topLevel?.StorageProvider == null)
                {
                    ReloadInstallations(selectCurrent: true);
                    return;
                }

                var options = new FolderPickerOpenOptions
                {
                    Title = "Select KOTOR game folder",
                    AllowMultiple = false
                };

                var currentPath = _editor.Installation?.Path;
                if (!string.IsNullOrWhiteSpace(currentPath) && Directory.Exists(currentPath))
                {
                    var currentFolder = await topLevel.StorageProvider.TryGetFolderFromPathAsync(currentPath);
                    if (currentFolder != null)
                    {
                        options.SuggestedStartLocation = currentFolder;
                    }
                }

                var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(options);
                if (folders == null || folders.Count == 0)
                {
                    ReloadInstallations(selectCurrent: true);
                    return;
                }

                var path = folders[0].TryGetLocalPath();
                if (string.IsNullOrWhiteSpace(path))
                {
                    ReloadInstallations(selectCurrent: true);
                    return;
                }

                var name = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = path;
                }

                var tsl = _tslCheck.IsChecked == true;
                if (ApplyPath(path, name, tsl))
                {
                    var savedName = new GlobalSettings().AddOrUpdateInstallation(name, path, tsl);
                    if (!string.IsNullOrWhiteSpace(savedName))
                    {
                        ApplyPath(path, savedName, tsl);
                    }
                }

                ReloadInstallations(selectCurrent: true);
            }

            private async Task ManageInstallationsAsync()
            {
                var widget = new InstallationsWidget();
                var dialog = new Window
                {
                    Title = "Manage installations",
                    Width = 580,
                    Height = 440,
                    MinWidth = 520,
                    MinHeight = 360,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                var root = new DockPanel();
                var buttons = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Spacing = 8,
                    Margin = new Thickness(10)
                };
                var okButton = new Button { Content = "OK", Width = 88 };
                var cancelButton = new Button { Content = "Cancel", Width = 88 };
                okButton.Click += (s, e) =>
                {
                    widget.Save();
                    dialog.Close(true);
                };
                cancelButton.Click += (s, e) => dialog.Close(false);
                buttons.Children.Add(okButton);
                buttons.Children.Add(cancelButton);
                DockPanel.SetDock(buttons, Dock.Bottom);
                root.Children.Add(buttons);

                var scroll = new ScrollViewer { Content = widget };
                root.Children.Add(scroll);
                dialog.Content = root;

                await dialog.ShowDialog(_editor);
                ReloadInstallations(selectCurrent: true);
            }

            private void ReapplyCurrentPath()
            {
                if (_loading)
                {
                    return;
                }

                var current = _editor.Installation;
                if (current == null || string.IsNullOrWhiteSpace(current.Path))
                {
                    return;
                }

                var tsl = _tslCheck.IsChecked == true;
                if (ApplyPath(current.Path, current.Name, tsl))
                {
                    new GlobalSettings().AddOrUpdateInstallation(current.Name, current.Path, tsl);
                    ReloadInstallations(selectCurrent: true);
                }
            }

            private bool ApplyPath(string path, string name, bool tsl)
            {
                if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                {
                    _editor.SetStandaloneInstallation(null);
                    RefreshPathText();
                    return false;
                }

                try
                {
                    _editor.SetStandaloneInstallation(new OdyInstallation(Path.GetFullPath(path), name, tsl));
                    RefreshPathText();
                    return true;
                }
                catch (Exception ex)
                {
                    DialogHelper.ShowWindow(_editor, "Installation Error", ex.Message, MsBox.Avalonia.Enums.Icon.Warning);
                    _editor.SetStandaloneInstallation(null);
                    RefreshPathText();
                    return false;
                }
            }

            private void RefreshPathText()
            {
                var installation = _editor.Installation;
                _loading = true;
                try
                {
                    if (installation == null)
                    {
                        _pathText.Text = "No game path selected";
                        ToolTip.SetTip(_pathText, null);
                        _tslCheck.IsChecked = false;
                    }
                    else
                    {
                        _pathText.Text = installation.Path;
                        ToolTip.SetTip(_pathText, installation.Path);
                        _tslCheck.IsChecked = installation.Tsl;
                    }
                }
                finally
                {
                    _loading = false;
                }
            }

            private static bool ReadTsl(Dictionary<string, object> data)
            {
                if (data == null || !data.ContainsKey("tsl"))
                {
                    return false;
                }

                if (data["tsl"] is bool boolValue)
                {
                    return boolValue;
                }

                bool parsed;
                return bool.TryParse(data["tsl"]?.ToString(), out parsed) && parsed;
            }

            private static Dictionary<string, Dictionary<string, object>> ReadSavedInstallations()
            {
                try
                {
                    return new GlobalSettings().GetValue(
                        "Installations",
                        new Dictionary<string, Dictionary<string, object>>())
                        ?? new Dictionary<string, Dictionary<string, object>>();
                }
                catch
                {
                    return new Dictionary<string, Dictionary<string, object>>();
                }
            }

            private async Task<bool> LaunchEditorAsync(string key)
            {
                var executable = FindUnifiedStandaloneExecutable();
                if (string.IsNullOrWhiteSpace(executable) || !File.Exists(executable))
                {
                    await DialogHelper.ShowWindowAsync(
                        _editor,
                        "Editor Launcher",
                        "Could not find OdyTools.Standalone to open the selected editor.",
                        MsBox.Avalonia.Enums.ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Warning);
                    return false;
                }

                try
                {
                    var args = BuildLaunchArguments(key, _editor.FilepathPublic, _editor.Installation);

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = executable,
                        Arguments = BuildArgumentString(args),
                        WorkingDirectory = Path.GetDirectoryName(executable) ?? AppContext.BaseDirectory,
                        UseShellExecute = false
                    };

                    Process.Start(startInfo);
                    _editor.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    await DialogHelper.ShowWindowAsync(
                        _editor,
                        "Editor Launcher",
                        ex.Message,
                        MsBox.Avalonia.Enums.ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Warning);
                    return false;
                }
            }

            private static string InferEditorKey(Editor editor)
            {
                if (editor == null)
                {
                    return "2da";
                }

                return StandaloneEditorRouting.NormalizeEditorKey(editor.GetType().Name) ?? "2da";
            }

            private static string FindUnifiedStandaloneExecutable()
            {
                var current = GetCurrentExecutablePath();
                if (IsUnifiedStandalone(current))
                {
                    return current;
                }

                var baseDir = AppContext.BaseDirectory;
                if (!string.IsNullOrWhiteSpace(baseDir))
                {
                    var directory = new DirectoryInfo(baseDir);
                    var framework = directory.Name;
                    var configuration = directory.Parent?.Name;
                    var cursor = directory;
                    while (cursor != null)
                    {
                        if (string.Equals(cursor.Name, "OdyTools", StringComparison.OrdinalIgnoreCase))
                        {
                            var candidate = Path.Combine(cursor.FullName, "bin", configuration ?? "Debug", framework ?? "net9.0", "OdyTools.Standalone");
                            if (File.Exists(candidate))
                            {
                                return candidate;
                            }
                        }

                        cursor = cursor.Parent;
                    }
                }

                return current;
            }

            private static bool IsUnifiedStandalone(string path)
            {
                var fileName = Path.GetFileNameWithoutExtension(path ?? string.Empty);
                return string.Equals(fileName, "OdyTools.Standalone", StringComparison.OrdinalIgnoreCase);
            }

            private static string GetCurrentExecutablePath()
            {
                try
                {
                    return Process.GetCurrentProcess().MainModule.FileName;
                }
                catch
                {
                    return null;
                }
            }

            private static string BuildArgumentString(IEnumerable<string> args)
            {
                var parts = new List<string>();
                foreach (var arg in args)
                {
                    parts.Add(QuoteArgument(arg ?? string.Empty));
                }

                return string.Join(" ", parts);
            }

            private static string QuoteArgument(string arg)
            {
                if (arg.Length == 0)
                {
                    return "\"\"";
                }

                if (arg.IndexOfAny(new[] { ' ', '\t', '\n', '"' }) < 0)
                {
                    return arg;
                }

                return "\"" + arg.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
            }
        }
    }
}
