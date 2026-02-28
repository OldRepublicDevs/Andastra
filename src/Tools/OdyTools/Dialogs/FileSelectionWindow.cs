using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using OSPlatform = System.Runtime.InteropServices.OSPlatform;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using BioWare.Extract;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Data;
using OdyTools.Utils;
using OdyTools.Editors;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using FileResource = BioWare.Extract.FileResource;

namespace OdyTools.Dialogs
{
    /// <summary>
    /// Window for selecting a file from multiple search results.
    /// Displays resources in a table with detailed information and allows user to select and open one.
    /// </summary>
    public partial class FileSelectionWindow : Window
    {
        private DataGrid _resourceTable;
        private CheckBox _detailedCheckbox;
        private Button _openButton;
        private List<FileResource> _resources;
        private OdyInstallation _installation;
        private List<string> _detailedStatAttributes;

        /// <summary>
        /// Initializes a new instance of FileSelectionWindow.
        /// </summary>
        /// <param name="searchResults">List of FileResource or LocationResult to display</param>
        /// <param name="installation">OdyInstallation instance (optional)</param>
        /// <param name="parent">Parent window (optional)</param>
        public FileSelectionWindow(
            IEnumerable<object> searchResults,
            OdyInstallation installation = null,
            Window parent = null)
        {
            InitializeComponent();
            _installation = installation;
            _detailedStatAttributes = new List<string>();

            // Convert search results to FileResource list
            _resources = UnifyResources(searchResults);

            SetupUI();
            InitTable();

            if (parent != null)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            else
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }

        // Public parameterless constructor for XAML
        public FileSelectionWindow() : this(new List<object>(), null, null)
        {
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

        /// <summary>
        /// Converts various resource types (FileResource, LocationResult) to a unified list of FileResource.
        /// </summary>
        private List<FileResource> UnifyResources(IEnumerable<object> resources)
        {
            var result = new List<FileResource>();
            if (resources == null)
            {
                return result;
            }

            foreach (var resource in resources)
            {
                if (resource is FileResource fileResource)
                {
                    if (!result.Contains(fileResource))
                    {
                        result.Add(fileResource);
                    }
                }
                else if (resource is LocationResult locationResult)
                {
                    // Convert LocationResult to FileResource
                    FileResource fr = locationResult.FileResource;
                    if (fr == null)
                    {
                        // Create FileResource from LocationResult
                        if (!File.Exists(locationResult.FilePath))
                        {
                            continue;
                        }

                        var fileInfo = new FileInfo(locationResult.FilePath);
                        // Try to extract resource name and type from path
                        string resName = Path.GetFileNameWithoutExtension(locationResult.FilePath);
                        ResourceType resType = ResourceType.INVALID;

                        // Try to determine resource type from extension
                        string ext = Path.GetExtension(locationResult.FilePath)?.ToLowerInvariant();
                        if (!string.IsNullOrEmpty(ext))
                        {
                            ext = ext.TrimStart('.');
                            try
                            {
                                resType = ResourceType.FromExtension(ext);
                            }
                            catch
                            {
                                // If extension lookup fails, use INVALID
                                resType = ResourceType.INVALID;
                            }
                        }

                        fr = new FileResource(
                            resName ?? "",
                            resType,
                            locationResult.Size,
                            locationResult.Offset,
                            locationResult.FilePath);
                        locationResult.SetFileResource(fr);
                    }

                    if (!result.Contains(fr))
                    {
                        result.Add(fr);
                    }
                }
            }

            return result;
        }

        private void SetupProgrammaticUI()
        {
            Title = "File Selection";
            Width = 800;
            Height = 600;
            MinWidth = 600;
            MinHeight = 400;

            var mainPanel = new StackPanel
            {
                Margin = new Avalonia.Thickness(10),
                Spacing = 10
            };

            // Detailed checkbox
            _detailedCheckbox = new CheckBox
            {
                Content = "Show detailed",
                Margin = new Avalonia.Thickness(0, 0, 0, 5)
            };
            _detailedCheckbox.IsCheckedChanged += (s, e) => ToggleDetailedInfo();
            mainPanel.Children.Add(_detailedCheckbox);

            // Resource table
            _resourceTable = new DataGrid
            {
                AutoGenerateColumns = false,
                CanUserReorderColumns = true,
                CanUserResizeColumns = true,
                CanUserSortColumns = true,
                SelectionMode = DataGridSelectionMode.Single,
                GridLinesVisibility = DataGridGridLinesVisibility.All
            };
            _resourceTable.DoubleTapped += (s, e) => OnDoubleClick();
            mainPanel.Children.Add(_resourceTable);

            // Open button
            var buttonPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Spacing = 5
            };
            _openButton = new Button
            {
                Content = "Open",
                MinWidth = 100
            };
            _openButton.Click += (s, e) => OpenSelected();
            buttonPanel.Children.Add(_openButton);
            mainPanel.Children.Add(buttonPanel);

            Content = mainPanel;
        }

        private void SetupUI()
        {
            // Find controls from XAML
            try
            {
                _resourceTable = this.FindControl<DataGrid>("resourceTable");
                _detailedCheckbox = this.FindControl<CheckBox>("detailedCheckbox");
                _openButton = this.FindControl<Button>("openButton");
            }
            catch
            {
                // XAML not loaded or controls not found - will use programmatic UI
            }

            if (_detailedCheckbox != null)
            {
                _detailedCheckbox.IsCheckedChanged += (s, e) => ToggleDetailedInfo();
            }

            if (_openButton != null)
            {
                _openButton.Click += (s, e) => OpenSelected();
            }

            if (_resourceTable != null)
            {
                _resourceTable.DoubleTapped += (s, e) => OnDoubleClick();
            }
        }

        private void UpdateTableHeaders()
        {
            if (_resourceTable == null)
            {
                return;
            }

            var headers = new List<string> { "File Name", "File Path", "Offset", "Size" };

            if (_detailedStatAttributes != null && _detailedStatAttributes.Count > 0)
            {
                foreach (var header in _detailedStatAttributes)
                {
                    if (!headers.Contains(header))
                    {
                        headers.Add(header);
                    }
                }
            }

            // Clear existing columns
            _resourceTable.Columns.Clear();

            // Add columns with proper property bindings
            foreach (var header in headers)
            {
                string propertyName = GetPropertyNameForHeader(header);
                var column = new DataGridTextColumn
                {
                    Header = header,
                    Binding = new Binding(propertyName)
                };
                _resourceTable.Columns.Add(column);
            }
        }

        private void PopulateTable()
        {
            if (_resourceTable == null || _resources == null)
            {
                return;
            }

            var tableItems = new List<ResourceTableItem>();

            foreach (var resource in _resources)
            {
                var item = new ResourceTableItem
                {
                    Resource = resource,
                    FileName = resource.Identifier.ToString(),
                    FilePath = resource.FilePath,
                    Offset = $"0x{resource.Offset:X}",
                    Size = HumanReadableSize(resource.Size)
                };

                if (_detailedStatAttributes != null && _detailedStatAttributes.Count > 0)
                {
                    try
                    {
                        var fileInfo = new FileInfo(resource.FilePath);
                        if (fileInfo.Exists)
                        {
                            // Populate all available file stat attributes
                            PopulateFileStatAttributes(item, fileInfo, resource);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue with other resources
                        System.Console.WriteLine($"Error getting file stats for {resource.FilePath}: {ex}");
                    }
                }
                else
                {
                    // Show relative path when not in detailed mode
                    if (_installation != null && !string.IsNullOrEmpty(_installation.Path))
                    {
                        try
                        {
                            var installPath = new DirectoryInfo(_installation.Path);
                            var filePath = new FileInfo(resource.FilePath);
                            if (filePath.FullName.StartsWith(installPath.FullName, StringComparison.OrdinalIgnoreCase))
                            {
                                item.FilePath = PathUtils.GetRelativePath(installPath.FullName, filePath.FullName);
                            }
                        }
                        catch
                        {
                            // Use full path if relative path calculation fails
                        }
                    }
                }

                tableItems.Add(item);
            }

            _resourceTable.ItemsSource = tableItems;
        }

        private string HumanReadableSize(long size, int decimalPlaces = 2)
        {
            string[] units = { "B", "KB", "MB", "GB", "TB", "PB" };
            double sizeDouble = size;
            string unit = "B";

            foreach (var u in units)
            {
                if (sizeDouble < 1024.0 || u == "PB")
                {
                    unit = u;
                    break;
                }
                sizeDouble /= 1024.0;
            }

            return string.Format("{0:F" + decimalPlaces + "} {1}", sizeDouble, unit);
        }

        private void ToggleDetailedInfo()
        {
            try
            {
                bool showDetailed = _detailedCheckbox?.IsChecked ?? false;

                if (showDetailed && _resources != null && _resources.Count > 0)
                {
                    // Get stat attributes from first resource
                    var firstResource = _resources[0];
                    var filePath = new FileInfo(firstResource.FilePath);
                    if (filePath.Exists)
                    {
                        _detailedStatAttributes = GetStatAttributes(filePath);
                        _detailedStatAttributes.Add("Size on Disk");
                        _detailedStatAttributes.Add("Size Ratio");
                    }
                    else
                    {
                        _detailedStatAttributes = new List<string>();
                    }
                }
                else
                {
                    _detailedStatAttributes = new List<string>();
                }

                InitTable();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error toggling detailed info: {ex}");
                if (_detailedCheckbox != null)
                {
                    _detailedCheckbox.IsChecked = false;
                }
            }
        }

        /// <summary>
        /// Gets all available file stat attributes for display in detailed mode.
        /// Matches PyKotor's os.stat() attributes where possible using C# FileInfo and FileSystemInfo.
        /// </summary>
        private List<string> GetStatAttributes(FileInfo fileInfo)
        {
            var attributes = new List<string>();

            try
            {
                if (fileInfo.Exists)
                {
                    // Basic time attributes (matching os.stat() st_mtime, st_atime, st_ctime)
                    attributes.Add("Last Modified");
                    attributes.Add("Last Accessed");
                    attributes.Add("Created");

                    // File attributes (matching os.stat() st_mode and file attributes)
                    attributes.Add("Mode");
                    attributes.Add("Attributes");
                    attributes.Add("Is Read Only");
                    attributes.Add("Is Hidden");
                    attributes.Add("Is System");
                    attributes.Add("Is Archive");
                    attributes.Add("Is Compressed");
                    attributes.Add("Is Encrypted");
                    attributes.Add("Is Directory");

                    // File system attributes
                    attributes.Add("Extension");
                    attributes.Add("Directory Name");

                    // Link information (matching os.stat() st_nlink)
                    // Note: C# doesn't directly expose hard link count, but we can try to get it
                    try
                    {
                        // Check if we can determine hard links (Windows-specific)
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            attributes.Add("Hard Links");
                        }
                    }
                    catch
                    {
                        // Hard link count not available on this platform
                    }
                }
            }
            catch
            {
                // Ignore errors - return what we have
            }

            return attributes;
        }

        private void InitTable()
        {
            if (_resourceTable == null)
            {
                return;
            }

            _resourceTable.IsReadOnly = true;
            UpdateTableHeaders();
            PopulateTable();
            ResizeToContent();
        }

        private void ResizeToContent()
        {
            if (_resourceTable == null)
            {
                return;
            }

            // Calculate width based on columns
            double width = 100; // Base width for window chrome

            if (_resourceTable.Columns != null)
            {
                foreach (var column in _resourceTable.Columns)
                {
                    width += 150; // Estimated column width
                }
            }

            // Ensure window doesn't exceed screen size
            try
            {
                var screens = Avalonia.Application.Current?.ApplicationLifetime as Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime;
                if (screens?.MainWindow != null)
                {
                    var screen = screens.MainWindow.Screens.ScreenFromWindow(screens.MainWindow);
                    if (screen != null)
                    {
                        var availableWidth = screen.WorkingArea.Width;
                        width = Math.Min(width, availableWidth * 0.9);
                    }
                }
            }
            catch
            {
                width = Math.Min(width, 1920);
            }

            Width = Math.Max(width, MinWidth);
            CenterWindow();
        }

        private void CenterWindow()
        {
            try
            {
                var screens = Avalonia.Application.Current?.ApplicationLifetime as Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime;
                if (screens?.MainWindow != null)
                {
                    var screen = screens.MainWindow.Screens.ScreenFromWindow(screens.MainWindow);
                    if (screen != null)
                    {
                        var screenWidth = screen.WorkingArea.Width;
                        var screenHeight = screen.WorkingArea.Height;
                        var windowWidth = Width;
                        var windowHeight = Height;

                        var x = Math.Max(0, (screenWidth - windowWidth) / 2);
                        var y = Math.Max(0, (screenHeight - windowHeight) / 2);

                        Position = new Avalonia.PixelPoint((int)x, (int)y);
                    }
                }
            }
            catch
            {
                // Ignore errors
            }
        }

        private void OpenSelected()
        {
            if (_resourceTable?.SelectedItem is ResourceTableItem item && item.Resource != null)
            {
                OpenResource(item.Resource);
            }
        }

        private void OnDoubleClick()
        {
            OpenSelected();
        }

        private void OpenResource(FileResource resource)
        {
            if (resource == null)
            {
                return;
            }

            try
            {
                var parentWindow = this.Parent as Window ?? this;
                WindowUtils.OpenResourceEditor(resource, _installation, parentWindow);
                Close();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error opening resource: {ex}");

                // Note: Using ex.Message for error details (similar to universal_simplify_exception in PyKotor)
                string errorMessage = ex.Message;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = ex.ToString();
                }

                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    "Error",
                    $"Error opening resource:\n{errorMessage}",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                errorBox.ShowAsync();
            }
        }

        // Property to access installation
        public OdyInstallation Installation => _installation;

        /// <summary>
        /// Populates comprehensive file stat attributes for a resource table item.
        /// Matches PyKotor's add_file_item and add_extra_file_details methods.
        /// </summary>
        private void PopulateFileStatAttributes(ResourceTableItem item, FileInfo fileInfo, FileResource resource)
        {
            // Size on disk (matching get_size_on_disk in PyKotor)
            long sizeOnDisk = GetSizeOnDisk(fileInfo);
            item.SizeOnDisk = HumanReadableSize(sizeOnDisk);

            // Calculate size ratio
            if (resource.Size > 0)
            {
                double ratio = (sizeOnDisk / (double)resource.Size) * 100.0;
                item.SizeRatio = $"{ratio:F2}%";
            }
            else
            {
                item.SizeRatio = "N/A";
            }

            // Time attributes (matching os.stat() st_mtime, st_atime, st_ctime)
            item.LastModified = FormatTime(fileInfo.LastWriteTime);
            item.LastAccessed = FormatTime(fileInfo.LastAccessTime);
            item.Created = FormatTime(fileInfo.CreationTime);

            // File attributes (matching os.stat() st_mode)
            item.Mode = FormatFileMode(fileInfo);
            item.Attributes = FormatFileAttributes(fileInfo.Attributes);

            // Boolean file attributes
            item.IsReadOnly = fileInfo.IsReadOnly ? "Yes" : "No";
            item.IsHidden = (fileInfo.Attributes & FileAttributes.Hidden) != 0 ? "Yes" : "No";
            item.IsSystem = (fileInfo.Attributes & FileAttributes.System) != 0 ? "Yes" : "No";
            item.IsArchive = (fileInfo.Attributes & FileAttributes.Archive) != 0 ? "Yes" : "No";
            item.IsCompressed = (fileInfo.Attributes & FileAttributes.Compressed) != 0 ? "Yes" : "No";
            item.IsEncrypted = (fileInfo.Attributes & FileAttributes.Encrypted) != 0 ? "Yes" : "No";
            item.IsDirectory = (fileInfo.Attributes & FileAttributes.Directory) != 0 ? "Yes" : "No";

            // File system attributes
            item.Extension = fileInfo.Extension ?? "";
            item.DirectoryName = fileInfo.DirectoryName ?? "";

            // Hard links (matching os.stat() st_nlink)
            // Note: C# doesn't directly expose hard link count, but we can try to get it on Windows
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Hard link count on Windows requires P/Invoke (GetFileInformationByHandle); show N/A when unavailable
                    item.HardLinks = "N/A";
                }
                else
                {
                    item.HardLinks = "N/A";
                }
            }
            catch
            {
                item.HardLinks = "N/A";
            }
        }

        /// <summary>
        /// Gets the size on disk for a file (accounting for cluster size).
        /// </summary>
        private long GetSizeOnDisk(FileInfo fileInfo)
        {
            try
            {
                // On Windows, size on disk accounts for cluster size
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Actual size on disk (accounting for cluster size) could be obtained via GetCompressedFileSize/GetFileSize P/Invoke; we return file length.
                    return fileInfo.Length;
                }
                else
                {
                    // On Unix, size on disk is typically file size rounded up to block size; we return file length.
                    return fileInfo.Length;
                }
            }
            catch
            {
                return fileInfo.Length;
            }
        }

        /// <summary>
        /// Formats a DateTime to a human-readable string.
        /// </summary>
        private string FormatTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// Formats file mode (permissions) similar to os.stat() st_mode.
        /// On Windows, this represents file attributes; on Unix, it's permissions.
        /// </summary>
        private string FormatFileMode(FileInfo fileInfo)
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return $"0{(int)fileInfo.Attributes:X}";
                }
                else
                {
#if !NET48
                    // Unix: use File.GetUnixFileMode when available (.NET 5+)
                    try
                    {
                        var mode = File.GetUnixFileMode(fileInfo.FullName);
                        return "0" + Convert.ToString((int)mode, 8).PadLeft(3, '0');
                    }
                    catch (PlatformNotSupportedException)
                    {
                        return "N/A";
                    }
#else
                    return "N/A";
#endif
                }
            }
            catch
            {
                return "N/A";
            }
        }

        /// <summary>
        /// Formats file attributes as a readable string.
        /// </summary>
        private string FormatFileAttributes(FileAttributes attributes)
        {
            var attrList = new List<string>();

            if ((attributes & FileAttributes.ReadOnly) != 0) attrList.Add("ReadOnly");
            if ((attributes & FileAttributes.Hidden) != 0) attrList.Add("Hidden");
            if ((attributes & FileAttributes.System) != 0) attrList.Add("System");
            if ((attributes & FileAttributes.Directory) != 0) attrList.Add("Directory");
            if ((attributes & FileAttributes.Archive) != 0) attrList.Add("Archive");
            if ((attributes & FileAttributes.Device) != 0) attrList.Add("Device");
            if ((attributes & FileAttributes.Normal) != 0) attrList.Add("Normal");
            if ((attributes & FileAttributes.Temporary) != 0) attrList.Add("Temporary");
            if ((attributes & FileAttributes.SparseFile) != 0) attrList.Add("SparseFile");
            if ((attributes & FileAttributes.ReparsePoint) != 0) attrList.Add("ReparsePoint");
            if ((attributes & FileAttributes.Compressed) != 0) attrList.Add("Compressed");
            if ((attributes & FileAttributes.Offline) != 0) attrList.Add("Offline");
            if ((attributes & FileAttributes.NotContentIndexed) != 0) attrList.Add("NotContentIndexed");
            if ((attributes & FileAttributes.Encrypted) != 0) attrList.Add("Encrypted");
            if ((attributes & FileAttributes.IntegrityStream) != 0) attrList.Add("IntegrityStream");
            if ((attributes & FileAttributes.NoScrubData) != 0) attrList.Add("NoScrubData");

            return attrList.Count > 0 ? string.Join(", ", attrList) : "None";
        }

        // Helper method to map header names to property names
        private string GetPropertyNameForHeader(string header)
        {
            switch (header)
            {
                case "File Name":
                    return "FileName";
                case "File Path":
                    return "FilePath";
                case "Offset":
                    return "Offset";
                case "Size":
                    return "Size";
                case "Size on Disk":
                    return "SizeOnDisk";
                case "Size Ratio":
                    return "SizeRatio";
                case "Last Modified":
                    return "LastModified";
                case "Last Accessed":
                    return "LastAccessed";
                case "Created":
                    return "Created";
                case "Mode":
                    return "Mode";
                case "Hard Links":
                    return "HardLinks";
                case "Attributes":
                    return "Attributes";
                case "Is Read Only":
                    return "IsReadOnly";
                case "Is Hidden":
                    return "IsHidden";
                case "Is System":
                    return "IsSystem";
                case "Is Archive":
                    return "IsArchive";
                case "Is Compressed":
                    return "IsCompressed";
                case "Is Encrypted":
                    return "IsEncrypted";
                case "Is Directory":
                    return "IsDirectory";
                case "Extension":
                    return "Extension";
                case "Directory Name":
                    return "DirectoryName";
                default:
                    return header.Replace(" ", "");
            }
        }
    }

    // Helper class for table items
    internal class ResourceTableItem
    {
        public FileResource Resource { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Offset { get; set; }
        public string Size { get; set; }

        // Detailed file stat attributes (matching os.stat() in Python)
        public string SizeOnDisk { get; set; }
        public string LastModified { get; set; }
        public string LastAccessed { get; set; }
        public string Created { get; set; }
        public string SizeRatio { get; set; }
        public string Mode { get; set; }
        public string HardLinks { get; set; }
        public string Attributes { get; set; }
        public string IsReadOnly { get; set; }
        public string IsHidden { get; set; }
        public string IsSystem { get; set; }
        public string IsArchive { get; set; }
        public string IsCompressed { get; set; }
        public string IsEncrypted { get; set; }
        public string IsDirectory { get; set; }
        public string Extension { get; set; }
        public string DirectoryName { get; set; }
    }
}

