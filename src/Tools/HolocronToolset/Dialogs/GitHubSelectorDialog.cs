using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace HolocronToolset.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:76
    // Original: class GitHubFileSelector(QDialog):
    public partial class GitHubSelectorDialog : Window
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:494-502
        // Original: @dataclass class TreeInfoData(AbstractAPIResult):
        private class TreeInfoData
        {
            public string Mode { get; set; }
            public string Type { get; set; } // "blob" for files, "tree" for directories
            public string Sha { get; set; }
            public int? Size { get; set; }
            public string Url { get; set; }
            public string Path { get; set; }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:504-519
        // Original: @dataclass class CompleteRepoData(AbstractAPIResult):
        private class CompleteRepoData
        {
            public List<TreeInfoData> Tree { get; set; }
        }

        private string _owner;
        private string _repo;
        private string _selectedPath;
        private TextBox _filterEdit;
        private TreeView _repoTreeWidget;
        private ComboBox _forkComboBox;
        private Button _searchButton;
        private Button _refreshButton;
        private Button _cloneButton;
        private Button _okButton;
        private Button _cancelButton;
        
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:120
        // Original: self.repo_data: CompleteRepoData | None = None
        private CompleteRepoData _repoData;
        
        // Dictionary to map file paths to TreeViewItems for efficient lookup during filtering
        private Dictionary<string, TreeViewItem> _pathToItemMap;

        // Public parameterless constructor for XAML
        public GitHubSelectorDialog() : this(null, null, null)
        {
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:77-183
        // Original: def __init__(self, *args, selected_files=None, parent=None):
        public GitHubSelectorDialog(string owner, string repo, List<string> selectedFiles = null, Window parent = null)
        {
            InitializeComponent();
            _owner = owner ?? "";
            _repo = repo ?? "";
            _selectedPath = null;
            _repoData = null;
            _pathToItemMap = new Dictionary<string, TreeViewItem>();
            SetupUI();
            InitializeRepoData();
            
            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:180-183
            // Original: if self.selected_files and self.repo_data is not None: self.filter_edit.setText(";".join(self.selected_files)); self.search_files()
            if (selectedFiles != null && selectedFiles.Count > 0 && _repoData != null && _filterEdit != null)
            {
                _filterEdit.Text = string.Join(";", selectedFiles);
                SearchFiles();
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
            Title = "Select a GitHub Repository File";
            MinWidth = 600;
            MinHeight = 400;

            var mainPanel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };

            var label = new TextBlock { Text = "Please select the correct script path or enter manually:" };
            mainPanel.Children.Add(label);

            var forkLabel = new TextBlock { Text = "Select Fork:" };
            _forkComboBox = new ComboBox { MinWidth = 300 };
            mainPanel.Children.Add(forkLabel);
            mainPanel.Children.Add(_forkComboBox);

            var filterPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
            _filterEdit = new TextBox { Watermark = "Type to filter paths...", MinWidth = 200 };
            _searchButton = new Button { Content = "Search" };
            _refreshButton = new Button { Content = "Refresh" };
            filterPanel.Children.Add(_filterEdit);
            filterPanel.Children.Add(_searchButton);
            filterPanel.Children.Add(_refreshButton);
            mainPanel.Children.Add(filterPanel);

            _repoTreeWidget = new TreeView();
            mainPanel.Children.Add(_repoTreeWidget);

            var buttonPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            _okButton = new Button { Content = "OK" };
            _okButton.Click += (s, e) => Accept();
            _cancelButton = new Button { Content = "Cancel" };
            _cancelButton.Click += (s, e) => Close();
            buttonPanel.Children.Add(_okButton);
            buttonPanel.Children.Add(_cancelButton);
            mainPanel.Children.Add(buttonPanel);

            _cloneButton = new Button { Content = "Clone Repository" };
            _cloneButton.Click += (s, e) => CloneRepository();
            mainPanel.Children.Add(_cloneButton);

            Content = mainPanel;
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _filterEdit = this.FindControl<TextBox>("filterEdit");
            _repoTreeWidget = this.FindControl<TreeView>("repoTreeWidget");
            _forkComboBox = this.FindControl<ComboBox>("forkComboBox");
            _searchButton = this.FindControl<Button>("searchButton");
            _refreshButton = this.FindControl<Button>("refreshButton");
            _cloneButton = this.FindControl<Button>("cloneButton");
            _okButton = this.FindControl<Button>("okButton");
            _cancelButton = this.FindControl<Button>("cancelButton");

            if (_searchButton != null)
            {
                _searchButton.Click += (s, e) => SearchFiles();
            }
            if (_refreshButton != null)
            {
                _refreshButton.Click += (s, e) => RefreshData();
            }
            if (_cloneButton != null)
            {
                _cloneButton.Click += (s, e) => CloneRepository();
            }
            if (_okButton != null)
            {
                _okButton.Click += (s, e) => Accept();
            }
            if (_cancelButton != null)
            {
                _cancelButton.Click += (s, e) => Close();
            }
            if (_filterEdit != null)
            {
                _filterEdit.TextChanged += (s, e) => OnFilterEditChanged();
            }
            if (_forkComboBox != null)
            {
                _forkComboBox.SelectionChanged += (s, e) => OnForkChanged();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:185-218
        // Original: def initialize_repo_data(self) -> CompleteRepoData | None:
        private void InitializeRepoData()
        {
            // TODO: Implement GitHub API integration when available
            System.Console.WriteLine($"Initializing repo data for {_owner}/{_repo}");
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:285-286
        // Original: def search_files(self):
        private void SearchFiles()
        {
            OnFilterEditChanged();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:288-307
        // Original: def on_filter_edit_changed(self):
        private void OnFilterEditChanged()
        {
            if (_filterEdit == null || _repoTreeWidget == null)
            {
                return;
            }

            string filterText = _filterEdit.Text ?? "";
            
            if (!string.IsNullOrWhiteSpace(filterText))
            {
                // Split by semicolon to support multiple file names (matching PyKotor behavior)
                string[] fileNames = filterText.ToLowerInvariant().Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                List<string> fileNamesList = fileNames.Select(f => f.Trim()).Where(f => !string.IsNullOrEmpty(f)).ToList();
                SearchAndHighlight(fileNamesList);
                ExpandAllItems();
            }
            else
            {
                // Unhide all items and collapse when filter is cleared
                UnhideAllItems();
                CollapseAllItems();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:406-424
        // Original: def get_selected_path(self) -> str | None:
        private string GetSelectedPath()
        {
            // TODO: Get selected path from tree widget when available
            return _selectedPath;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:418-424
        // Original: def accept(self) -> None:
        private void Accept()
        {
            _selectedPath = GetSelectedPath();
            if (string.IsNullOrEmpty(_selectedPath))
            {
                // Show warning message
                var msgBox = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(
                    "Warning",
                    "You must select a file.",
                    MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Warning);
                msgBox.ShowAsync();
                return;
            }
            System.Console.WriteLine($"User selected '{_selectedPath}'");
            Close(true);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:426-431
        // Original: def on_fork_changed(self, index: int) -> None:
        private void OnForkChanged()
        {
            // TODO: Reload repo data for selected fork when available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:520-537
        // Original: def clone_repository(self) -> None:
        private void CloneRepository()
        {
            // Get selected fork from combo box
            string selectedFork = null;
            if (_forkComboBox != null)
            {
                selectedFork = _forkComboBox.SelectedItem?.ToString() ?? _forkComboBox.Text;
                // Remove " (main)" suffix if present (matching PyKotor behavior)
                if (!string.IsNullOrEmpty(selectedFork))
                {
                    selectedFork = selectedFork.Replace(" (main)", "");
                }
            }

            // Validate that a fork is selected
            if (string.IsNullOrWhiteSpace(selectedFork))
            {
                var warningMsgBox = MessageBoxManager.GetMessageBoxStandard(
                    "No Fork Selected",
                    "Please select a fork to clone.",
                    ButtonEnum.Ok,
                    Icon.Warning);
                warningMsgBox.ShowAsync();
                return;
            }

            // Construct GitHub URL
            string url = $"https://github.com/{selectedFork}.git";

            try
            {
                // Check if git is available
                if (!IsGitAvailable())
                {
                    var errorMsgBox = MessageBoxManager.GetMessageBoxStandard(
                        "Git Not Found",
                        "Git is not installed or not available in PATH. Please install Git and ensure it is accessible from the command line.",
                        ButtonEnum.Ok,
                        Icon.Error);
                    errorMsgBox.ShowAsync();
                    return;
                }

                // Prepare git clone command
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = $"clone {url}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                // Windows-specific: Set CREATE_NO_WINDOW flag (matching PyKotor behavior)
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    startInfo.CreateNoWindow = true;
                }

                // Execute git clone
                using (Process process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        throw new InvalidOperationException("Failed to start git process");
                    }

                    // Read output and error streams
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        // Clone successful
                        var successMsgBox = MessageBoxManager.GetMessageBoxStandard(
                            "Clone Successful",
                            $"Repository {selectedFork} cloned successfully.",
                            ButtonEnum.Ok,
                            Icon.Success);
                        successMsgBox.ShowAsync();
                    }
                    else
                    {
                        // Clone failed
                        string errorMessage = !string.IsNullOrEmpty(error) ? error : output;
                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            errorMessage = $"Git clone failed with exit code {process.ExitCode}";
                        }
                        var errorMsgBox = MessageBoxManager.GetMessageBoxStandard(
                            "Clone Failed",
                            $"Failed to clone repository: {errorMessage}",
                            ButtonEnum.Ok,
                            Icon.Error);
                        errorMsgBox.ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions during git clone execution
                var errorMsgBox = MessageBoxManager.GetMessageBoxStandard(
                    "Clone Failed",
                    $"Failed to clone repository: {ex.Message}",
                    ButtonEnum.Ok,
                    Icon.Error);
                errorMsgBox.ShowAsync();
            }
        }

        /// <summary>
        /// Checks if git is available in the system PATH.
        /// </summary>
        /// <returns>True if git is available, false otherwise.</returns>
        private bool IsGitAvailable()
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = "--version",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    startInfo.CreateNoWindow = true;
                }

                using (Process process = Process.Start(startInfo))
                {
                    if (process == null)
                    {
                        return false;
                    }

                    process.WaitForExit(5000); // 5 second timeout
                    return process.ExitCode == 0;
                }
            }
            catch
            {
                // If we can't start git, it's not available
                return false;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:630-633
        // Original: def refresh_data(self) -> None:
        private void RefreshData()
        {
            InitializeRepoData();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:309-316
        // Original: def search_and_highlight(self, partial_file_or_folder_names: list[str]) -> None:
        private void SearchAndHighlight(List<string> partialFileOrFolderNames)
        {
            if (_repoData == null || _repoData.Tree == null || _repoTreeWidget == null)
            {
                return;
            }

            // Find paths that match any of the partial names (case-insensitive, matching against the last part of the path)
            HashSet<string> pathsToHighlight = new HashSet<string>();
            foreach (TreeInfoData item in _repoData.Tree)
            {
                if (string.IsNullOrEmpty(item.Path))
                {
                    continue;
                }

                // Get the last part of the path (filename or folder name)
                string lastPart = Path.GetFileName(item.Path).ToLowerInvariant();
                
                // Check if any of the search terms match
                foreach (string searchTerm in partialFileOrFolderNames)
                {
                    if (lastPart.Contains(searchTerm))
                    {
                        pathsToHighlight.Add(item.Path);
                        break;
                    }
                }
            }

            ExpandAndHighlightPaths(pathsToHighlight);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:318-386
        // Original: def expand_and_highlight_paths(self, paths: set[str]) -> None:
        private void ExpandAndHighlightPaths(HashSet<string> paths)
        {
            if (_repoTreeWidget == null)
            {
                return;
            }

            // Hide all items first
            HideAllItems();

            // Highlight each matching path
            foreach (string path in paths)
            {
                HighlightPath(path);
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:335-359
        // Original: def highlight_path(path: str):
        private void HighlightPath(string path)
        {
            if (_repoTreeWidget == null || string.IsNullOrEmpty(path))
            {
                return;
            }

            // Split path into parts
            string[] parts = path.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
            {
                return;
            }

            TreeViewItem currentItem = null;

            // Traverse the tree to find the item
            for (int i = 0; i < parts.Length; i++)
            {
                string part = parts[i];
                TreeViewItem nextItem = null;

                if (currentItem == null)
                {
                    // Top level - search in root items
                    if (_repoTreeWidget.ItemsSource == null)
                    {
                        return;
                    }

                    foreach (TreeViewItem topLevelItem in _repoTreeWidget.ItemsSource)
                    {
                        if (topLevelItem.Header?.ToString() == part)
                        {
                            nextItem = topLevelItem;
                            topLevelItem.Opacity = 1.0;
                            break;
                        }
                    }
                }
                else
                {
                    // Search in children
                    if (currentItem.ItemsSource == null)
                    {
                        return;
                    }

                    foreach (TreeViewItem childItem in currentItem.ItemsSource)
                    {
                        if (childItem.Header?.ToString() == part)
                        {
                            nextItem = childItem;
                            childItem.Opacity = 1.0;
                            break;
                        }
                    }
                }

                if (nextItem == null)
                {
                    // Path not found in tree
                    return;
                }

                currentItem = nextItem;
            }

            // Highlight the found item
            if (currentItem != null)
            {
                // Set background color to yellow for highlighting (matching PyKotor's QBrush(Qt.GlobalColor.yellow))
                currentItem.Background = new SolidColorBrush(Colors.Yellow);
                currentItem.IsExpanded = true;
                currentItem.Opacity = 1.0;

                // If it's a directory (tree), unhide all children
                TreeInfoData itemData = currentItem.Tag as TreeInfoData;
                if (itemData != null && itemData.Type == "tree")
                {
                    UnhideAllChildren(currentItem);
                }
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:361-367
        // Original: def unhide_all_children(item: QTreeWidgetItem):
        private void UnhideAllChildren(TreeViewItem item)
        {
            if (item == null || item.ItemsSource == null)
            {
                return;
            }

            foreach (TreeViewItem child in item.ItemsSource)
            {
                child.Opacity = 1.0;
                UnhideAllChildren(child);
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:369-383
        // Original: def hide_all_items(): ... def hide_item(item: QTreeWidgetItem):
        private void HideAllItems()
        {
            if (_repoTreeWidget == null || _repoTreeWidget.ItemsSource == null)
            {
                return;
            }

            foreach (TreeViewItem topLevelItem in _repoTreeWidget.ItemsSource)
            {
                HideItem(topLevelItem);
            }
        }

        private void HideItem(TreeViewItem item)
        {
            if (item == null)
            {
                return;
            }

            // Hide item using opacity (matching PyKotor's setHidden(True) behavior)
            item.Opacity = 0.0;
            // Clear background color (matching PyKotor's QBrush(Qt.GlobalColor.transparent))
            item.Background = new SolidColorBrush(Colors.Transparent);

            if (item.ItemsSource != null)
            {
                foreach (TreeViewItem child in item.ItemsSource)
                {
                    HideItem(child);
                }
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:296-307
        // Original: def unhide_item(item: QTreeWidgetItem):
        private void UnhideAllItems()
        {
            if (_repoTreeWidget == null || _repoTreeWidget.ItemsSource == null)
            {
                return;
            }

            foreach (TreeViewItem topLevelItem in _repoTreeWidget.ItemsSource)
            {
                UnhideItem(topLevelItem);
            }
        }

        private void UnhideItem(TreeViewItem item)
        {
            if (item == null)
            {
                return;
            }

            // Show item using opacity (matching PyKotor's setHidden(False) behavior)
            item.Opacity = 1.0;
            // Clear background color
            item.Background = new SolidColorBrush(Colors.Transparent);

            if (item.ItemsSource != null)
            {
                foreach (TreeViewItem child in item.ItemsSource)
                {
                    UnhideItem(child);
                }
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:388-396
        // Original: def expand_all_items(self):
        private void ExpandAllItems()
        {
            if (_repoTreeWidget == null || _repoTreeWidget.ItemsSource == null)
            {
                return;
            }

            Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
            
            // Add all top-level items to stack
            foreach (TreeViewItem item in _repoTreeWidget.ItemsSource)
            {
                stack.Push(item);
            }

            // Expand all items using iterative approach (matching PyKotor's stack-based implementation)
            while (stack.Count > 0)
            {
                TreeViewItem item = stack.Pop();
                item.IsExpanded = true;

                if (item.ItemsSource != null)
                {
                    foreach (TreeViewItem child in item.ItemsSource)
                    {
                        stack.Push(child);
                    }
                }
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/github_selector.py:398-404
        // Original: def collapse_all_items(self):
        private void CollapseAllItems()
        {
            if (_repoTreeWidget == null || _repoTreeWidget.ItemsSource == null)
            {
                return;
            }

            Stack<TreeViewItem> stack = new Stack<TreeViewItem>();
            
            // Add all top-level items to stack
            foreach (TreeViewItem item in _repoTreeWidget.ItemsSource)
            {
                stack.Push(item);
            }

            // Collapse all items using iterative approach (matching PyKotor's stack-based implementation)
            while (stack.Count > 0)
            {
                TreeViewItem item = stack.Pop();
                item.IsExpanded = false;

                if (item.ItemsSource != null)
                {
                    foreach (TreeViewItem child in item.ItemsSource)
                    {
                        stack.Push(child);
                    }
                }
            }
        }

        public string SelectedPath => _selectedPath;
    }
}
