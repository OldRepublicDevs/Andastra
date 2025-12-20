using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using HolocronToolset.Data;
using HolocronToolset.Utils;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Andastra.Parsing.Mods.GFF;
using Andastra.Parsing.Resource;
using Andastra.Parsing.Formats.GFF;

namespace HolocronToolset.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:35
    // Original: class TSLPatchDataEditor(QDialog):
    public partial class TSLPatchDataEditorDialog : Window
    {
        private HTInstallation _installation;
        private string _tslpatchdataPath;
        private TextBox _pathEdit;
        private TreeView _fileTree;
        private TabControl _configTabs;
        private Button _generateButton;
        private Button _previewButton;
        private Button _saveButton;
        
        // General settings controls
        private TextBox _modNameEdit;
        private TextBox _modAuthorEdit;
        private TextBox _modDescriptionEdit;
        private CheckBox _installToOverrideCheck;
        private CheckBox _backupFilesCheck;
        private CheckBox _confirmOverwritesCheck;
        
        // INI Preview control
        private TextBox _iniPreviewText;
        
        // GFF Fields controls
        private ListBox _gffFileList;
        private TreeView _gffFieldsTree;
        private List<ModificationsGFF> _gffModifications = new List<ModificationsGFF>();
        
        // Scripts controls
        private ListBox _scriptList;
        // Dictionary to store full file paths for scripts (key: filename, value: full path)
        // This allows us to copy the actual files during generation
        private Dictionary<string, string> _scriptPaths = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Public parameterless constructor for XAML
        public TSLPatchDataEditorDialog() : this(null, null, null)
        {
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:38-61
        // Original: def __init__(self, parent, installation=None, tslpatchdata_path=None):
        public TSLPatchDataEditorDialog(Window parent, HTInstallation installation, string tslpatchdataPath = null)
        {
            InitializeComponent();
            Title = "TSLPatchData Editor - Create HoloPatcher Mod";
            Width = 1400;
            Height = 900;
            _installation = installation;
            _tslpatchdataPath = tslpatchdataPath ?? "tslpatchdata";
            SetupUI();
            if (!string.IsNullOrEmpty(_tslpatchdataPath) && Directory.Exists(_tslpatchdataPath))
            {
                LoadExistingConfig();
            }
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
            var mainPanel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };

            var headerPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
            headerPanel.Children.Add(new TextBlock { Text = "TSLPatchData Folder:", FontWeight = Avalonia.Media.FontWeight.Bold });
            _pathEdit = new TextBox { Text = _tslpatchdataPath, MinWidth = 300 };
            var browseButton = new Button { Content = "Browse..." };
            browseButton.Click += (s, e) => BrowseTslpatchdataPath();
            var createButton = new Button { Content = "Create New" };
            createButton.Click += (s, e) => CreateNewTslpatchdata();
            headerPanel.Children.Add(_pathEdit);
            headerPanel.Children.Add(browseButton);
            headerPanel.Children.Add(createButton);
            mainPanel.Children.Add(headerPanel);

            var splitter = new Grid();
            splitter.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            splitter.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            var leftPanel = new StackPanel();
            leftPanel.Children.Add(new TextBlock { Text = "Files to Package:", FontWeight = Avalonia.Media.FontWeight.Bold });
            _fileTree = new TreeView();
            leftPanel.Children.Add(_fileTree);
            Grid.SetColumn(leftPanel, 0);
            splitter.Children.Add(leftPanel);

            _configTabs = new TabControl();
            CreateGeneralTab();
            Create2DAMemoryTab();
            CreateTLKStrRefTab();
            CreateGFFFieldsTab();
            CreateScriptsTab();
            CreateINIPreviewTab();
            Grid.SetColumn(_configTabs, 1);
            splitter.Children.Add(_configTabs);

            mainPanel.Children.Add(splitter);

            var buttonPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            _generateButton = new Button { Content = "Generate TSLPatchData" };
            _generateButton.Click += (s, e) => GenerateTslpatchdata();
            _previewButton = new Button { Content = "Preview INI" };
            _previewButton.Click += (s, e) => PreviewIni();
            _saveButton = new Button { Content = "Save Configuration" };
            _saveButton.Click += (s, e) => SaveConfiguration();
            buttonPanel.Children.Add(_generateButton);
            buttonPanel.Children.Add(_previewButton);
            buttonPanel.Children.Add(_saveButton);
            mainPanel.Children.Add(buttonPanel);

            Content = mainPanel;
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _pathEdit = this.FindControl<TextBox>("pathEdit");
            _fileTree = this.FindControl<TreeView>("fileTree");
            _configTabs = this.FindControl<TabControl>("configTabs");
            _generateButton = this.FindControl<Button>("generateButton");
            _previewButton = this.FindControl<Button>("previewButton");
            _saveButton = this.FindControl<Button>("saveButton");
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:160-212
        // Original: def _create_general_tab(self):
        private void CreateGeneralTab()
        {
            var tab = new TabItem { Header = "General Settings" };
            var content = new StackPanel { Spacing = 10, Margin = new Avalonia.Thickness(10) };

            // Mod Information Group (using Expander instead of GroupBox - Avalonia doesn't have GroupBox)
            var modInfoGroup = new Expander { Header = "Mod Information", IsExpanded = true, Margin = new Avalonia.Thickness(0, 0, 0, 10) };
            var modInfoLayout = new StackPanel { Spacing = 5 };

            // Mod name
            var nameLayout = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            nameLayout.Children.Add(new TextBlock { Text = "Mod Name:", VerticalAlignment = VerticalAlignment.Center, MinWidth = 100 });
            _modNameEdit = new TextBox { MinWidth = 300 };
            nameLayout.Children.Add(_modNameEdit);
            modInfoLayout.Children.Add(nameLayout);

            // Mod author
            var authorLayout = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            authorLayout.Children.Add(new TextBlock { Text = "Author:", VerticalAlignment = VerticalAlignment.Center, MinWidth = 100 });
            _modAuthorEdit = new TextBox { MinWidth = 300 };
            authorLayout.Children.Add(_modAuthorEdit);
            modInfoLayout.Children.Add(authorLayout);

            // Description
            modInfoLayout.Children.Add(new TextBlock { Text = "Description:" });
            _modDescriptionEdit = new TextBox { MinHeight = 100, AcceptsReturn = true, TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            modInfoLayout.Children.Add(_modDescriptionEdit);

            modInfoGroup.Content = modInfoLayout;
            content.Children.Add(modInfoGroup);

            // Installation Options Group (using Expander instead of GroupBox - Avalonia doesn't have GroupBox)
            var installOptionsGroup = new Expander { Header = "Installation Options", IsExpanded = true, Margin = new Avalonia.Thickness(0, 0, 0, 10) };
            var installOptionsLayout = new StackPanel { Spacing = 5 };

            _installToOverrideCheck = new CheckBox { Content = "Install files to Override folder", IsChecked = true };
            installOptionsLayout.Children.Add(_installToOverrideCheck);

            _backupFilesCheck = new CheckBox { Content = "Backup original files", IsChecked = true };
            installOptionsLayout.Children.Add(_backupFilesCheck);

            _confirmOverwritesCheck = new CheckBox { Content = "Confirm before overwriting files", IsChecked = true };
            installOptionsLayout.Children.Add(_confirmOverwritesCheck);

            installOptionsGroup.Content = installOptionsLayout;
            content.Children.Add(installOptionsGroup);

            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        private void Create2DAMemoryTab()
        {
            var tab = new TabItem { Header = "2DA Memory" };
            var content = new StackPanel();
            // TODO: Add 2DA memory controls
            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        private void CreateTLKStrRefTab()
        {
            var tab = new TabItem { Header = "TLK StrRef" };
            var content = new StackPanel();
            // TODO: Add TLK StrRef controls
            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:305-348
        // Original: def _create_gff_fields_tab(self):
        private void CreateGFFFieldsTab()
        {
            var tab = new TabItem { Header = "GFF Fields" };
            var content = new StackPanel { Spacing = 10, Margin = new Avalonia.Thickness(10) };

            // Header
            content.Children.Add(new TextBlock 
            { 
                Text = "GFF Field Modifications:", 
                FontWeight = Avalonia.Media.FontWeight.Bold 
            });
            content.Children.Add(new TextBlock 
            { 
                Text = "View and edit fields that will be modified in GFF files." 
            });

            // Splitter for file list and field modifications
            var splitter = new Grid();
            splitter.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            splitter.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            // Left: File list
            var leftWidget = new StackPanel { Spacing = 5 };
            leftWidget.Children.Add(new TextBlock { Text = "Modified GFF Files:" });
            _gffFileList = new ListBox();
            _gffFileList.SelectionChanged += OnGffFileSelected;
            leftWidget.Children.Add(_gffFileList);
            Grid.SetColumn(leftWidget, 0);
            splitter.Children.Add(leftWidget);

            // Right: Field modifications
            var rightWidget = new StackPanel { Spacing = 5 };
            rightWidget.Children.Add(new TextBlock { Text = "Field Modifications:" });
            _gffFieldsTree = new TreeView();
            rightWidget.Children.Add(_gffFieldsTree);

            var btnLayout = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var openGffEditorBtn = new Button { Content = "Open in GFF Editor" };
            openGffEditorBtn.Click += (s, e) => OpenGffEditor();
            btnLayout.Children.Add(openGffEditorBtn);
            btnLayout.Children.Add(new TextBlock()); // Spacer
            rightWidget.Children.Add(btnLayout);

            Grid.SetColumn(rightWidget, 1);
            splitter.Children.Add(rightWidget);

            content.Children.Add(splitter);

            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        // Helper class for tree view items
        private class GFFFieldItem
        {
            public string FieldPath { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
            public string Type { get; set; }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:350-373
        // Original: def _create_scripts_tab(self):
        private void CreateScriptsTab()
        {
            var tab = new TabItem { Header = "Scripts" };
            var content = new StackPanel { Spacing = 10, Margin = new Avalonia.Thickness(10) };

            // Header labels
            content.Children.Add(new TextBlock { Text = "Scripts:", FontWeight = Avalonia.Media.FontWeight.Bold });
            content.Children.Add(new TextBlock { Text = "Compiled scripts (.ncs) that will be installed." });

            // Script list
            _scriptList = new ListBox { MinHeight = 300 };
            content.Children.Add(_scriptList);

            // Buttons
            var btnLayout = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var addScriptBtn = new Button { Content = "Add Script" };
            addScriptBtn.Click += async (s, e) => await AddScript();
            btnLayout.Children.Add(addScriptBtn);
            
            var removeScriptBtn = new Button { Content = "Remove Script" };
            removeScriptBtn.Click += (s, e) => RemoveScript();
            btnLayout.Children.Add(removeScriptBtn);
            
            btnLayout.Children.Add(new TextBlock()); // Spacer
            content.Children.Add(btnLayout);

            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:375-394
        // Original: def _create_ini_preview_tab(self):
        private void CreateINIPreviewTab()
        {
            var tab = new TabItem { Header = "INI Preview" };
            var content = new StackPanel { Spacing = 10, Margin = new Avalonia.Thickness(10) };

            content.Children.Add(new TextBlock { Text = "changes.ini Preview:", FontWeight = Avalonia.Media.FontWeight.Bold });

            _iniPreviewText = new TextBox
            {
                IsReadOnly = true,
                AcceptsReturn = true,
                TextWrapping = Avalonia.Media.TextWrapping.NoWrap,
                FontFamily = new Avalonia.Media.FontFamily("Consolas, Courier New, monospace"),
                MinHeight = 400
            };
            content.Children.Add(_iniPreviewText);

            var btnLayout = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 5 };
            var refreshPreviewBtn = new Button { Content = "Refresh Preview" };
            refreshPreviewBtn.Click += (s, e) => UpdateIniPreview();
            btnLayout.Children.Add(refreshPreviewBtn);
            btnLayout.Children.Add(new TextBlock()); // Spacer
            content.Children.Add(btnLayout);

            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:397-403
        // Original: def _browse_tslpatchdata_path(self):
        private async void BrowseTslpatchdataPath()
        {
            if (_pathEdit == null)
            {
                return;
            }

            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null)
            {
                return;
            }

            // Get initial directory from current path edit text if it's a valid path
            string initialDirectory = _pathEdit.Text;
            if (!string.IsNullOrEmpty(initialDirectory) && Directory.Exists(initialDirectory))
            {
                // Use the current path as initial directory
            }
            else if (!string.IsNullOrEmpty(initialDirectory))
            {
                // If it's not empty but doesn't exist, try to get the parent directory
                string parentDir = Path.GetDirectoryName(initialDirectory);
                if (!string.IsNullOrEmpty(parentDir) && Directory.Exists(parentDir))
                {
                    initialDirectory = parentDir;
                }
                else
                {
                    // Use current working directory or user's home
                    initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                }
            }
            else
            {
                // Use current working directory or user's home
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }

            try
            {
                var options = new FolderPickerOpenOptions
                {
                    Title = "Select TSLPatchData Folder",
                    AllowMultiple = false
                };

                // Set initial directory if available
                if (!string.IsNullOrEmpty(initialDirectory) && Directory.Exists(initialDirectory))
                {
                    var storageFolder = await topLevel.StorageProvider.TryGetFolderFromPathAsync(initialDirectory);
                    if (storageFolder != null)
                    {
                        options.SuggestedStartLocation = storageFolder;
                    }
                }

                var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(options);
                if (folders != null && folders.Count > 0)
                {
                    string selectedPath = folders[0].Path.LocalPath;
                    if (!string.IsNullOrWhiteSpace(selectedPath))
                    {
                        _tslpatchdataPath = selectedPath;
                        _pathEdit.Text = selectedPath;
                        // Reload existing configuration if the folder exists
                        if (Directory.Exists(_tslpatchdataPath))
                        {
                            LoadExistingConfig();
                        }
                    }
                }
            }
            catch
            {
                // Ignore errors - user may have cancelled or dialog failed
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:405-420
        // Original: def _create_new_tslpatchdata(self):
        private async void CreateNewTslpatchdata()
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null)
            {
                return;
            }

            try
            {
                // Open folder picker to select location for new TSLPatchData
                var options = new FolderPickerOpenOptions
                {
                    Title = "Select Location for New TSLPatchData",
                    AllowMultiple = false
                };

                // Get initial directory from current path edit text if it's a valid path
                string initialDirectory = null;
                if (!string.IsNullOrEmpty(_pathEdit?.Text))
                {
                    string currentPath = _pathEdit.Text;
                    if (Directory.Exists(currentPath))
                    {
                        initialDirectory = currentPath;
                    }
                    else
                    {
                        // Try parent directory
                        string parentDir = Path.GetDirectoryName(currentPath);
                        if (!string.IsNullOrEmpty(parentDir) && Directory.Exists(parentDir))
                        {
                            initialDirectory = parentDir;
                        }
                    }
                }

                // If no valid initial directory, use user's home directory
                if (string.IsNullOrEmpty(initialDirectory))
                {
                    initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                }

                // Set initial directory if available
                if (!string.IsNullOrEmpty(initialDirectory) && Directory.Exists(initialDirectory))
                {
                    var storageFolder = await topLevel.StorageProvider.TryGetFolderFromPathAsync(initialDirectory);
                    if (storageFolder != null)
                    {
                        options.SuggestedStartLocation = storageFolder;
                    }
                }

                var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(options);
                if (folders == null || folders.Count == 0)
                {
                    // User cancelled
                    return;
                }

                string selectedPath = folders[0].Path.LocalPath;
                if (string.IsNullOrWhiteSpace(selectedPath))
                {
                    return;
                }

                // Create tslpatchdata subdirectory in selected location
                string tslpatchdataPath = Path.Combine(selectedPath, "tslpatchdata");
                
                // Create directory if it doesn't exist (exist_ok=True in Python)
                if (!Directory.Exists(tslpatchdataPath))
                {
                    Directory.CreateDirectory(tslpatchdataPath);
                }

                // Update path
                _tslpatchdataPath = tslpatchdataPath;
                if (_pathEdit != null)
                {
                    _pathEdit.Text = tslpatchdataPath;
                }

                // Show success message
                var msgBox = MessageBoxManager.GetMessageBoxStandard(
                    "Created",
                    $"New tslpatchdata folder created at:\n{tslpatchdataPath}",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Success);
                await msgBox.ShowAsync();
            }
            catch (Exception ex)
            {
                // Show error message
                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    "Error",
                    $"Failed to create TSLPatchData folder:\n{ex.Message}",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                await errorBox.ShowAsync();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:422-430
        // Original: def _load_existing_config(self):
        private void LoadExistingConfig()
        {
            if (string.IsNullOrEmpty(_tslpatchdataPath) || !Directory.Exists(_tslpatchdataPath))
            {
                return;
            }

            string iniPath = Path.Combine(_tslpatchdataPath, "changes.ini");
            if (!File.Exists(iniPath))
            {
                return;
            }

            try
            {
                // Read INI file
                string[] iniLines = File.ReadAllLines(iniPath, Encoding.UTF8);
                bool inCompileListSection = false;
                List<string> scripts = new List<string>();

                foreach (string line in iniLines)
                {
                    string trimmedLine = line.Trim();
                    
                    // Check for section headers
                    if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
                    {
                        inCompileListSection = trimmedLine.Equals("[CompileList]", StringComparison.OrdinalIgnoreCase);
                        continue;
                    }

                    // If we're in CompileList section and line is not empty or a comment
                    if (inCompileListSection && !string.IsNullOrWhiteSpace(trimmedLine) && !trimmedLine.StartsWith(";") && !trimmedLine.StartsWith("#"))
                    {
                        // Script entries in CompileList are just filenames (without path)
                        scripts.Add(trimmedLine);
                    }
                }

                // Populate script list
                if (_scriptList != null)
                {
                    _scriptList.Items.Clear();
                    _scriptPaths.Clear();
                    foreach (string script in scripts)
                    {
                        _scriptList.Items.Add(script);
                        // Note: When loading from INI, we don't have the full paths
                        // The user will need to re-add scripts if they want to generate the mod
                        // This matches PyKotor behavior where only filenames are stored in INI
                    }
                }

                // Load other settings from INI
                // TODO: Load mod name, author, description, and other settings from [settings] section
                // TODO: Load 2DA memory tokens from [2DAMEMORY] section
                // TODO: Load TLK strings from [TLKList] section
                // TODO: Load GFF files from [GFF files] section

                UpdateIniPreview();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error loading existing config: {ex.Message}");
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:529-534
        // Original: def _add_script(self):
        private async Task AddScript()
        {
            if (_scriptList == null)
            {
                return;
            }

            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null)
            {
                return;
            }

            try
            {
                var options = new FilePickerOpenOptions
                {
                    Title = "Select Scripts (.ncs)",
                    AllowMultiple = true,
                    FileTypeFilter = new[]
                    {
                        new FilePickerFileType("Scripts")
                        {
                            Patterns = new[] { "*.ncs" }
                        },
                        new FilePickerFileType("All Files")
                        {
                            Patterns = new[] { "*" }
                        }
                    }
                };

                // Set initial directory if available
                if (!string.IsNullOrEmpty(_tslpatchdataPath) && Directory.Exists(_tslpatchdataPath))
                {
                    var storageFolder = await topLevel.StorageProvider.TryGetFolderFromPathAsync(_tslpatchdataPath);
                    if (storageFolder != null)
                    {
                        options.SuggestedStartLocation = storageFolder;
                    }
                }

                var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        string filePath = file.Path.LocalPath;
                        if (!string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath))
                        {
                            // Add just the filename to the list (matching PyKotor behavior)
                            string fileName = Path.GetFileName(filePath);
                            
                            // Check if script is already in the list
                            bool alreadyExists = false;
                            foreach (var item in _scriptList.Items)
                            {
                                if (item is string existingScript && existingScript.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                                {
                                    alreadyExists = true;
                                    break;
                                }
                            }

                            if (!alreadyExists)
                            {
                                _scriptList.Items.Add(fileName);
                                // Store the full path for later copying during generation
                                _scriptPaths[fileName] = filePath;
                            }
                        }
                    }
                    
                    // Update INI preview to reflect changes
                    UpdateIniPreview();
                }
            }
            catch (Exception ex)
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    "Error",
                    $"Failed to add script:\n{ex.Message}",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                await errorBox.ShowAsync();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:536-540
        // Original: def _remove_script(self):
        private void RemoveScript()
        {
            if (_scriptList == null)
            {
                return;
            }

            var selectedItem = _scriptList.SelectedItem;
            if (selectedItem != null)
            {
                string scriptName = selectedItem as string;
                if (!string.IsNullOrEmpty(scriptName))
                {
                    // Remove from list and dictionary
                    _scriptList.Items.Remove(selectedItem);
                    _scriptPaths.Remove(scriptName);
                    // Update INI preview to reflect changes
                    UpdateIniPreview();
                }
            }
        }

        private void GenerateTslpatchdata()
        {
            // TODO: Generate TSLPatchData files
            System.Console.WriteLine("Generate TSLPatchData not yet implemented");
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:566-571
        // Original: def _preview_ini(self):
        private void PreviewIni()
        {
            // Switch to INI Preview tab
            if (_configTabs != null && _configTabs.Items.Count > 0)
            {
                int previewTabIndex = _configTabs.Items.Count - 1; // Last tab is INI Preview
                _configTabs.SelectedIndex = previewTabIndex;
            }
            UpdateIniPreview();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:542-564
        // Original: def _update_ini_preview(self):
        private void UpdateIniPreview()
        {
            if (_iniPreviewText == null)
            {
                return;
            }

            // Generate preview from current configuration
            var previewLines = new StringBuilder();
            
            // Settings section
            previewLines.AppendLine("[settings]");
            string modName = _modNameEdit?.Text?.Trim() ?? "My Mod";
            string modAuthor = _modAuthorEdit?.Text?.Trim() ?? "Unknown";
            previewLines.AppendLine($"modname={modName}");
            previewLines.AppendLine($"author={modAuthor}");
            previewLines.AppendLine();

            // GFF files section
            previewLines.AppendLine("[GFFList]");
            previewLines.AppendLine("; Files to be patched");
            if (_gffModifications != null && _gffModifications.Count > 0)
            {
                foreach (var modGff in _gffModifications)
                {
                    string identifier = modGff.ReplaceFile ? $"Replace{modGff.SourceFile}" : modGff.SourceFile ?? "Unknown";
                    previewLines.AppendLine($"{identifier}={modGff.SourceFile ?? "Unknown"}");
                }
            }
            previewLines.AppendLine();

            // 2DAMEMORY section
            previewLines.AppendLine("[2DAMEMORY]");
            previewLines.AppendLine("; 2DA memory tokens");
            previewLines.AppendLine();

            // TLKList section
            previewLines.AppendLine("[TLKList]");
            previewLines.AppendLine("; TLK string additions");
            previewLines.AppendLine();

            // InstallList section
            previewLines.AppendLine("[InstallList]");
            previewLines.AppendLine("; Files to install");
            previewLines.AppendLine();

            // 2DAList section
            previewLines.AppendLine("[2DAList]");
            previewLines.AppendLine("; 2DA files to patch");
            previewLines.AppendLine();

            // CompileList section
            previewLines.AppendLine("[CompileList]");
            previewLines.AppendLine("; Scripts to compile");
            if (_scriptList != null && _scriptList.Items.Count > 0)
            {
                foreach (var item in _scriptList.Items)
                {
                    if (item is string scriptName && !string.IsNullOrWhiteSpace(scriptName))
                    {
                        previewLines.AppendLine(scriptName);
                    }
                }
            }
            previewLines.AppendLine();

            _iniPreviewText.Text = previewLines.ToString();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:573-588
        // Original: def _save_configuration(self):
        private async void SaveConfiguration()
        {
            try
            {
                // Ensure tslpatchdata directory exists
                if (string.IsNullOrEmpty(_tslpatchdataPath))
                {
                    var msgBox = MessageBoxManager.GetMessageBoxStandard(
                        "Error",
                        "TSLPatchData path is not set. Please specify a path first.",
                        ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Error);
                    await msgBox.ShowAsync();
                    return;
                }

                // Create directory if it doesn't exist
                if (!Directory.Exists(_tslpatchdataPath))
                {
                    Directory.CreateDirectory(_tslpatchdataPath);
                }

                // Build and update INI preview
                UpdateIniPreview();

                // Get the INI content from preview
                string iniContent = _iniPreviewText?.Text ?? string.Empty;
                if (string.IsNullOrWhiteSpace(iniContent))
                {
                    // If preview is empty, generate basic content
                    var sb = new StringBuilder();
                    sb.AppendLine("[settings]");
                    string modName = _modNameEdit?.Text?.Trim() ?? "My Mod";
                    string modAuthor = _modAuthorEdit?.Text?.Trim() ?? "Unknown";
                    sb.AppendLine($"modname={modName}");
                    sb.AppendLine($"author={modAuthor}");
                    sb.AppendLine();
                    iniContent = sb.ToString();
                }

                // Write to changes.ini
                string iniPath = Path.Combine(_tslpatchdataPath, "changes.ini");
                File.WriteAllText(iniPath, iniContent, Encoding.UTF8);

                // Show success message
                var successBox = MessageBoxManager.GetMessageBoxStandard(
                    "Saved",
                    $"Configuration saved to:\n{iniPath}",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Success);
                await successBox.ShowAsync();
            }
            catch (Exception ex)
            {
                // Show error message
                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    "Error",
                    $"Failed to save configuration:\n{ex.Message}",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                await errorBox.ShowAsync();
            }
        }
    }
}
