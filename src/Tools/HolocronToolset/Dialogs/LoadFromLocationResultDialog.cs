using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Andastra.Parsing.Extract;
using Andastra.Parsing.Resource;
using HolocronToolset.Data;
using HolocronToolset.Utils;
using MsBox.Avalonia;

namespace HolocronToolset.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_location_result.py
    // Original: class LoadFromLocationResultDialog(QMainWindow):
    public partial class LoadFromLocationResultDialog : Window
    {
        private DataGrid _tableWidget;
        private TextBox _searchEdit;
        private Button _openButton;
        private Button _extractButton;
        private List<FileResource> _resources;
        private HTInstallation _installation;
        
        // Helper class to store resource data with reference to FileResource
        private class ResourceItem
        {
            public string ResRef { get; set; }
            public string Type { get; set; }
            public string Path { get; set; }
            public FileResource Resource { get; set; }
        }

        // Public parameterless constructor for XAML
        public LoadFromLocationResultDialog() : this(null, null, null)
        {
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_location_result.py
        // Original: def __init__(self, parent, resources):
        // Updated to include HTInstallation parameter matching ResourceLocationDialog pattern
        public LoadFromLocationResultDialog(Window parent, List<FileResource> resources, HTInstallation installation = null)
        {
            InitializeComponent();
            Title = "Load From Location Result";
            Width = 1000;
            Height = 700;
            _resources = resources ?? new List<FileResource>();
            _installation = installation;
            SetupUI();
            PopulateResources();
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

            var searchPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
            _searchEdit = new TextBox { Watermark = "Search...", MinWidth = 200 };
            _searchEdit.TextChanged += (s, e) => OnSearchChanged();
            searchPanel.Children.Add(_searchEdit);
            mainPanel.Children.Add(searchPanel);

            _tableWidget = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserReorderColumns = true,
                CanUserResizeColumns = true,
                CanUserSortColumns = true
            };
            _tableWidget.Columns.Add(new DataGridTextColumn { Header = "ResRef", Binding = new Avalonia.Data.Binding("ResRef") });
            _tableWidget.Columns.Add(new DataGridTextColumn { Header = "Type", Binding = new Avalonia.Data.Binding("Type") });
            _tableWidget.Columns.Add(new DataGridTextColumn { Header = "Path", Binding = new Avalonia.Data.Binding("Path") });
            mainPanel.Children.Add(_tableWidget);

            var buttonPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            _openButton = new Button { Content = "Open" };
            _openButton.Click += (s, e) => OpenSelected();
            _extractButton = new Button { Content = "Extract" };
            _extractButton.Click += (s, e) => ExtractSelected();
            buttonPanel.Children.Add(_openButton);
            buttonPanel.Children.Add(_extractButton);
            mainPanel.Children.Add(buttonPanel);

            Content = mainPanel;
        }

        private void SetupUI()
        {
            // Find controls from XAML
            try
            {
                _tableWidget = this.FindControl<DataGrid>("tableWidget");
                _searchEdit = this.FindControl<TextBox>("searchEdit");
                _openButton = this.FindControl<Button>("openButton");
                _extractButton = this.FindControl<Button>("extractButton");
            }
            catch
            {
                // XAML not loaded or controls not found - will use programmatic UI
                // Controls are already set up in SetupProgrammaticUI
            }

            if (_searchEdit != null)
            {
                _searchEdit.TextChanged += (s, e) => OnSearchChanged();
            }
            if (_openButton != null)
            {
                _openButton.Click += (s, e) => OpenSelected();
            }
            if (_extractButton != null)
            {
                _extractButton.Click += (s, e) => ExtractSelected();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_location_result.py
        // Original: def populate_resources(self):
        private void PopulateResources()
        {
            if (_tableWidget != null)
            {
                // Create ResourceItem objects that include both display data and FileResource reference
                // This allows us to map selected items back to FileResource objects
                var items = _resources.Select(r => new ResourceItem
                {
                    ResRef = r.ResName,
                    Type = r.ResType.Extension,
                    Path = r.FilePath,
                    Resource = r
                }).ToList();
                _tableWidget.ItemsSource = items;
            }
        }

        // Matching PyKotor implementation pattern for resource filtering
        // Filters resources based on search text, checking ResRef, Type, and Path columns
        // Case-insensitive substring matching similar to ResourceProxyModel.filterAcceptsRow
        private void OnSearchChanged()
        {
            if (_tableWidget == null || _resources == null)
            {
                return;
            }

            string searchText = _searchEdit?.Text ?? "";
            string filterText = searchText.ToLowerInvariant();

            // If search text is empty, show all resources
            IEnumerable<FileResource> filteredResources = _resources;
            if (!string.IsNullOrWhiteSpace(filterText))
            {
                // Filter resources where search text appears in ResRef, Type, or Path
                // Matching PyKotor ResourceProxyModel.filterAcceptsRow pattern:
                // Checks if filter_string is a substring of filename or resname
                filteredResources = _resources.Where(r =>
                {
                    // Check ResRef (resource name)
                    string resRef = r.ResName?.ToLowerInvariant() ?? "";
                    if (resRef.Contains(filterText))
                    {
                        return true;
                    }

                    // Check Type (resource type extension)
                    string type = r.ResType?.Extension?.ToLowerInvariant() ?? "";
                    if (type.Contains(filterText))
                    {
                        return true;
                    }

                    // Check Path (file path)
                    string path = r.FilePath?.ToLowerInvariant() ?? "";
                    if (path.Contains(filterText))
                    {
                        return true;
                    }

                    return false;
                });
            }

            // Create ResourceItem objects from filtered resources
            var items = filteredResources.Select(r => new ResourceItem
            {
                ResRef = r.ResName,
                Type = r.ResType.Extension,
                Path = r.FilePath,
                Resource = r
            }).ToList();

            // Update DataGrid with filtered items
            _tableWidget.ItemsSource = items;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_location_result.py:927-936
        // Original: def open_selected_resource(self, resources, installation, gff_specialized=None):
        // This method opens the selected resources in appropriate editors
        private void OpenSelected()
        {
            var selectedResources = SelectedResources();
            if (selectedResources == null || selectedResources.Count == 0)
            {
                return;
            }

            try
            {
                // Get parent window for editor dialogs
                var parentWindow = this.Parent as Window ?? this;

                // Open each selected resource in the appropriate editor
                // Matching PyKotor: open_resource_editor(resource, installation, gff_specialized=gff_specialized)
                foreach (var resource in selectedResources)
                {
                    WindowUtils.OpenResourceEditor(resource, _installation, parentWindow);
                }
            }
            catch (Exception ex)
            {
                // Log error and show message to user
                System.Console.WriteLine($"Error opening selected resources: {ex}");
                
                // Show error dialog if MessageBox is available
                try
                {
                    var msgBox = MsBox.Avalonia.MessageBoxManager.GetMessageBoxStandard(
                        "Error",
                        $"Failed to open selected resources:\n{ex.Message}",
                        MsBox.Avalonia.Enums.ButtonEnum.Ok,
                        MsBox.Avalonia.Enums.Icon.Error);
                    msgBox.ShowAsync();
                }
                catch
                {
                    // MessageBox not available, just log to console
                }
            }
        }

        private void ExtractSelected()
        {
            // TODO: Extract selected resources
            System.Console.WriteLine("Extract selected not yet implemented");
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_location_result.py:689-690
        // Original: def selectedItems(self) -> list[ResourceTableWidgetItem]:
        // This method retrieves the selected resources from the DataGrid and maps them back to FileResource objects
        public List<FileResource> SelectedResources()
        {
            var selectedResources = new List<FileResource>();

            if (_tableWidget == null)
            {
                return selectedResources;
            }

            // Get selected items from DataGrid
            var selectedItems = _tableWidget.SelectedItems;
            if (selectedItems == null)
            {
                return selectedResources;
            }

            // Map selected ResourceItem objects back to FileResource objects
            foreach (var item in selectedItems)
            {
                if (item is ResourceItem resourceItem && resourceItem.Resource != null)
                {
                    selectedResources.Add(resourceItem.Resource);
                }
            }

            return selectedResources;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_location_result.py:1089-1105
        // Original: def resize_to_content(self):
        // This method resizes the window to fit the table content, using screen geometry instead of QDesktopWidget
        // In Qt, it uses QApplication.primaryScreen() which works for both Qt5 and Qt6 (QDesktopWidget is deprecated in Qt6)
        // In Avalonia, we use Screen API which is always available
        public void ResizeToContent()
        {
            if (_tableWidget == null)
            {
                return;
            }

            // Calculate width based on table columns
            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_location_result.py:1094-1096
            // Original: width = vert_header.width() + 4  # 4 for the frame
            //          for i in range(self.resource_table.columnCount()):
            //              width += self.resource_table.columnWidth(i)
            double width = 50; // Estimate for vertical header and frame padding

            if (_tableWidget.Columns != null)
            {
                foreach (var column in _tableWidget.Columns)
                {
                    // Estimate column width (header + content)
                    // In Avalonia DataGrid, we estimate based on typical content
                    width += 150; // Default column width estimate
                }
            }

            // Get screen bounds to ensure window doesn't exceed screen size
            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_location_result.py:1097-1102
            // Original: primary_screen: QScreen | None = QApplication.primaryScreen()
            //          if primary_screen is None:
            //              raise ValueError("Primary screen is not set")
            //          width = min(width, primary_screen.availableGeometry().width())
            try
            {
                var screens = Avalonia.Application.Current?.ApplicationLifetime as Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime;
                if (screens?.MainWindow != null)
                {
                    var screen = screens.MainWindow.Screens.ScreenFromWindow(screens.MainWindow);
                    if (screen != null)
                    {
                        var availableWidth = screen.WorkingArea.Width;
                        width = System.Math.Min(width, availableWidth * 0.9); // Max 90% of screen width
                    }
                }
            }
            catch
            {
                // If screen API is not available, use a reasonable default
                width = System.Math.Min(width, 1920); // Default max width
            }

            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_location_result.py:1103-1104
            // Original: height = self.height()  # keep the current height
            //          self.resize(width, height)
            // Set window width, keep current height
            Width = System.Math.Max(width, MinWidth);
        }
    }
}
