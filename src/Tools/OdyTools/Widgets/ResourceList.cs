using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using BioWare.Extract;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Data;

namespace OdyTools.Widgets
{
    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:77
    // Original: class ResourceList(MainWindowList):
    public partial class ResourceList : UserControl
    {
        private const int SearchDebounceMilliseconds = 180;
        private ResourceModel _modulesModel;
        private OdyInstallation _installation;
        private ComboBox _sectionCombo;
        private TextBox _searchEdit;
        private Button _reloadButton;
        private Button _refreshButton;
        private TreeView _resourceTree;
        private readonly DispatcherTimer _searchDebounceTimer;
        private List<ResourceTreeNode> _rootNodes;

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py
        // Original: UI wrapper exposing controls for testing
        public ResourceListUi Ui { get; private set; }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:57
        // Original: sig_section_changed: Signal = Signal(str)
        // Event emitted when the section combo box selection changes
        public event EventHandler<string> SectionChanged;

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:80
        // Original: sig_request_reload: Signal = Signal(str)
        // Event emitted when the reload button is clicked, passing the selected section string
        public event EventHandler<string> ReloadClicked;

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:81
        // Original: sig_request_refresh: Signal = Signal()
        // Event emitted when the refresh button is clicked
        public event EventHandler RefreshClicked;

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:55
        // Original: sig_request_open_resource: Signal = Signal(list, object)
        // Event emitted when a resource is double-clicked, passing the list of selected resources and useSpecializedEditor flag
        public event EventHandler<ResourceOpenEventArgs> ResourceDoubleClicked;

        // Matching PyKotor: sig_request_extract_resource / requestExtractResource
        public event EventHandler<ExtractRequestedEventArgs> ExtractRequested;

        // Matching PyKotor: Open Save Editor from context (saves widget only)
        public event EventHandler RequestOpenSaveEditor;

        /// <summary>When true, context menu shows "Open Save Editor" (set by MainWindow for the saves list).</summary>
        public bool IsSavesWidget { get; set; }

        public ResourceList()
        {
            InitializeComponent();
            _modulesModel = new ResourceModel();
            _rootNodes = new List<ResourceTreeNode>();
            _searchDebounceTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(SearchDebounceMilliseconds)
            };
            _searchDebounceTimer.Tick += OnSearchDebounceTick;
            SetupSignals();
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

            if (xamlLoaded)
            {
                _sectionCombo = this.FindControl<ComboBox>("sectionCombo");
                _searchEdit = this.FindControl<TextBox>("searchEdit");
                _reloadButton = this.FindControl<Button>("reloadButton");
                _refreshButton = this.FindControl<Button>("refreshButton");
                _resourceTree = this.FindControl<TreeView>("resourceTree");
            }
            else
            {
                SetupProgrammaticUI();
            }
        }

        private void SetupProgrammaticUI()
        {
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Section Combo and Refresh Button
            var topGrid = new Grid { Margin = new Avalonia.Thickness(5) };
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            _sectionCombo = new ComboBox { Margin = new Avalonia.Thickness(0, 0, 5, 0) };
            _refreshButton = new Button { Content = "Refresh", Width = 70 };
            topGrid.Children.Add(_sectionCombo);
            Grid.SetColumn(_sectionCombo, 0);
            topGrid.Children.Add(_refreshButton);
            Grid.SetColumn(_refreshButton, 1);
            grid.Children.Add(topGrid);
            Grid.SetRow(topGrid, 0);

            // Search and Reload
            var searchGrid = new Grid { Margin = new Avalonia.Thickness(5, 0, 5, 5) };
            searchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            searchGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            _searchEdit = new TextBox { Watermark = "search...", Margin = new Avalonia.Thickness(0, 0, 5, 0) };
            _reloadButton = new Button { Content = "Reload", Width = 70 };
            searchGrid.Children.Add(_searchEdit);
            Grid.SetColumn(_searchEdit, 0);
            searchGrid.Children.Add(_reloadButton);
            Grid.SetColumn(_reloadButton, 1);
            grid.Children.Add(searchGrid);
            Grid.SetRow(searchGrid, 1);

            // Resource Tree
            _resourceTree = new TreeView();
            grid.Children.Add(_resourceTree);
            Grid.SetRow(_resourceTree, 2);

            Content = grid;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:122-129
        // Original: def setup_signals(self):
        private void SetupSignals()
        {
            // Create UI wrapper exposing controls for testing
            Ui = new ResourceListUi
            {
                SectionCombo = _sectionCombo,
                SearchEdit = _searchEdit,
                ReloadButton = _reloadButton,
                RefreshButton = _refreshButton,
                ResourceTree = _resourceTree
            };

            if (_searchEdit != null)
            {
                _searchEdit.TextChanged += (sender, e) => OnFilterStringUpdated();
            }
            if (_sectionCombo != null)
            {
                _sectionCombo.SelectionChanged += (sender, e) => OnSectionChanged();
            }
            if (_reloadButton != null)
            {
                _reloadButton.Click += (sender, e) => OnReloadClicked();
            }
            if (_refreshButton != null)
            {
                _refreshButton.Click += (sender, e) => OnRefreshClicked();
            }
            if (_resourceTree != null)
            {
                _resourceTree.DoubleTapped += (sender, e) => OnResourceDoubleClicked();
                _resourceTree.SelectionMode = SelectionMode.Multiple;
                _resourceTree.ContextRequested += OnResourceContextMenuRequested;
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:150-152
        // Original: def set_installation(self, installation: OdyInstallation):
        public void SetInstallation(OdyInstallation installation)
        {
            _installation = installation;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:154-187
        // Original: def set_resources(self, resources: list[FileResource], custom_category: str | None = None, *, clear_existing: bool = True):
        public void SetResources(List<FileResource> resources, string customCategory = null, bool clearExisting = true)
        {
            if (clearExisting)
            {
                _modulesModel.Clear();
            }
            _modulesModel.AddResourcesBatch(resources ?? new List<FileResource>(), customCategory);
            UpdateTreeView();
        }

        private void UpdateTreeView()
        {
            if (_resourceTree == null)
            {
                return;
            }

            var categoryNames = _modulesModel
                .GetCategories()
                .OrderBy(category => category, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var newRootNodes = new List<ResourceTreeNode>(categoryNames.Count);
            foreach (var category in categoryNames)
            {
                var resourcesInCategory = _modulesModel
                    .GetResourcesInCategory(category)
                    .OrderBy(item => item.Text, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                if (resourcesInCategory.Count == 0)
                {
                    continue;
                }

                var categoryNode = ResourceTreeNode.CreateCategory(
                    category,
                    resourcesInCategory.Count,
                    () =>
                    {
                        var childNodes = new List<ResourceTreeNode>(resourcesInCategory.Count);
                        foreach (var resourceItem in resourcesInCategory)
                        {
                            childNodes.Add(ResourceTreeNode.CreateResource(
                                string.Format("{0} ({1})", resourceItem.Text, resourceItem.Resource.ResType.Extension),
                                resourceItem.Resource));
                        }
                        return childNodes;
                    });

                newRootNodes.Add(categoryNode);
            }

            _rootNodes = newRootNodes;
            _resourceTree.ItemsSource = _rootNodes;
        }

        private void OnSearchDebounceTick(object sender, EventArgs e)
        {
            _searchDebounceTimer.Stop();
            ApplyFilterAndRefreshTree();
        }

        private void ApplyFilterAndRefreshTree()
        {
            string filterText = _searchEdit != null ? _searchEdit.Text ?? string.Empty : string.Empty;
            _modulesModel.SetFilterString(filterText);
            UpdateTreeView();
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:189-195
        // Original: def set_sections(self, sections: list[QStandardItem]):
        public void SetSections(List<string> sections)
        {
            if (_sectionCombo != null)
            {
                _sectionCombo.Items.Clear();
                foreach (var section in sections ?? new List<string>())
                {
                    _sectionCombo.Items.Add(section);
                }
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:226-227
        // Original: def selected_resources(self) -> list[FileResource]: returns from selectedIndexes()
        public List<FileResource> SelectedResources()
        {
            var selected = new List<FileResource>();
            if (_resourceTree == null) return selected;
            // Support multiple selection (matching PyKotor ExtendedSelection)
            var selectedItems = _resourceTree.SelectedItems;
            if (selectedItems != null && selectedItems.Count > 0)
            {
                foreach (var item in selectedItems)
                {
                    if (item is ResourceTreeNode node && node.Resource != null)
                        selected.Add(node.Resource);
                }
            }
            if (selected.Count == 0 && _resourceTree.SelectedItem is ResourceTreeNode singleNode && singleNode.Resource != null)
                selected.Add(singleNode.Resource);
            return selected;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:197-224
        // Original: def set_resource_selection(self, resource: FileResource):
        public void SetResourceSelection(FileResource resource)
        {
            if (resource == null || _resourceTree == null)
            {
                return;
            }

            ResourceTreeNode targetItem;
            ResourceTreeNode parentCategory;
            if (FindResourceItem(resource, out targetItem, out parentCategory))
            {
                if (parentCategory != null)
                {
                    parentCategory.IsExpanded = true;
                }

                _resourceTree.SelectedItem = targetItem;

                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    var container = _resourceTree.ContainerFromItem(targetItem) as Control;
                    if (container != null)
                    {
                        container.BringIntoView();
                    }
                }, Avalonia.Threading.DispatcherPriority.Loaded);
            }
        }

        // Helper method to find a category/resource node pair for the specified resource.
        private bool FindResourceItem(FileResource resource, out ResourceTreeNode resourceItem, out ResourceTreeNode parentCategory)
        {
            resourceItem = null;
            parentCategory = null;

            if (_rootNodes == null || resource == null)
            {
                return false;
            }

            foreach (var categoryNode in _rootNodes)
            {
                categoryNode.EnsureChildrenLoaded();

                foreach (var childNode in categoryNode.Children)
                {
                    if (childNode == null || childNode.Resource == null)
                    {
                        continue;
                    }

                    if (resource.Equals(childNode.Resource))
                    {
                        resourceItem = childNode;
                        parentCategory = categoryNode;
                        return true;
                    }
                }
            }

            return false;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:229-232
        // Original: def on_filter_string_updated(self):
        private void OnFilterStringUpdated()
        {
            _searchDebounceTimer.Stop();
            _searchDebounceTimer.Start();
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:234-238
        // Original: def on_section_changed(self):
        private void OnSectionChanged()
        {
            // Get the selected section string from the combo box
            string sectionData = null;
            if (_sectionCombo != null && _sectionCombo.SelectedItem != null)
            {
                // In PyKotor, this uses currentData(Qt.ItemDataRole.UserRole), but since we're storing strings directly,
                // we use the SelectedItem as the section string
                sectionData = _sectionCombo.SelectedItem.ToString();
            }

            // Emit section changed signal (matching PyKotor: self.sig_section_changed.emit(data))
            SectionChanged?.Invoke(this, sectionData);
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:240-244
        // Original: def on_reload_clicked(self):
        private void OnReloadClicked()
        {
            // Get the selected section string from the combo box
            string sectionData = null;
            if (_sectionCombo != null && _sectionCombo.SelectedItem != null)
            {
                // In PyKotor, this uses currentData(Qt.ItemDataRole.UserRole), but since we're storing strings directly,
                // we use the SelectedItem as the section string
                sectionData = _sectionCombo.SelectedItem.ToString();
            }

            // Emit reload signal (matching PyKotor: self.sig_request_reload.emit(data))
            ReloadClicked?.Invoke(this, sectionData);
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:246-250
        // Original: def on_refresh_clicked(self):
        private void OnRefreshClicked()
        {
            // Clear the modules model (matching PyKotor: self._clear_modules_model())
            _modulesModel.Clear();
            _rootNodes.Clear();
            if (_resourceTree != null)
            {
                _resourceTree.ItemsSource = _rootNodes;
            }

            // Emit refresh signal (matching PyKotor: self.sig_request_refresh.emit())
            RefreshClicked?.Invoke(this, EventArgs.Empty);
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:274-322
        // Original: def on_resource_context_menu(self, point: QPoint):
        private void OnResourceContextMenuRequested(object sender, ContextRequestedEventArgs e)
        {
            var resources = SelectedResources();
            if (resources == null || resources.Count == 0)
            {
                e.Handled = false;
                return;
            }
            var menu = new ContextMenu();
            var localResources = new List<FileResource>(resources);
            bool allGff = localResources.Count > 0 && localResources.All(r => r?.ResType != null && r.ResType.IsGff());

            var openItem = new MenuItem { Header = "Open" };
            openItem.Click += (s, ev) =>
            {
                ResourceDoubleClicked?.Invoke(this, new ResourceOpenEventArgs(localResources, true));
            };
            menu.Items.Add(openItem);

            if (allGff)
            {
                var gffItem = new MenuItem { Header = "Open with GFF Editor" };
                gffItem.Click += (s, ev) =>
                {
                    ResourceDoubleClicked?.Invoke(this, new ResourceOpenEventArgs(localResources, false));
                };
                menu.Items.Add(gffItem);
            }

            if (IsSavesWidget)
            {
                menu.Items.Add(new Separator());
                var saveEditorItem = new MenuItem { Header = "Open Save Editor" };
                saveEditorItem.Click += (s, ev) => RequestOpenSaveEditor?.Invoke(this, EventArgs.Empty);
                menu.Items.Add(saveEditorItem);
            }

            menu.Items.Add(new Separator());
            var extractItem = new MenuItem { Header = "Extract..." };
            extractItem.Click += (s, ev) => ExtractRequested?.Invoke(this, new ExtractRequestedEventArgs(localResources));
            menu.Items.Add(extractItem);

            menu.Open(_resourceTree);
            e.Handled = true;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:416-418
        // Original: def on_resource_double_clicked(self):
        private void OnResourceDoubleClicked()
        {
            // Get the selected resources from the tree view
            var selectedResources = SelectedResources();

            if (selectedResources == null || selectedResources.Count == 0)
            {
                return;
            }

            // Emit open resource signal (matching PyKotor: self.sig_request_open_resource.emit(self.selected_resources(), None))
            // The second parameter (useSpecializedEditor) defaults to None/null, which means use default editor behavior
            var args = new ResourceOpenEventArgs(selectedResources, null);
            ResourceDoubleClicked?.Invoke(this, args);
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:131-133
        // Original: def hide_reload_button(self):
        public void HideReloadButton()
        {
            if (_reloadButton != null)
            {
                _reloadButton.IsVisible = false;
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:135-138
        // Original: def hide_section(self):
        public void HideSection()
        {
            if (_sectionCombo != null)
            {
                _sectionCombo.IsVisible = false;
            }
            if (_refreshButton != null)
            {
                _refreshButton.IsVisible = false;
            }
        }
    }

    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:478
    // Original: class ResourceModel(QStandardItemModel):
    public class ResourceModel
    {
        private readonly Dictionary<string, ResourceCategoryItem> _categoryItems = new Dictionary<string, ResourceCategoryItem>();
        private string _filterString = "";

        public ResourceModel()
        {
        }

        public void Clear()
        {
            _categoryItems.Clear();
        }

        public void AddResource(FileResource resource, string customCategory = null)
        {
            string category = customCategory ?? resource.ResType.Category;
            if (!_categoryItems.ContainsKey(category))
            {
                _categoryItems[category] = new ResourceCategoryItem(category);
            }
            _categoryItems[category].AddResource(resource);
        }

        public void AddResourcesBatch(List<FileResource> resources, string customCategory = null)
        {
            if (resources == null || resources.Count == 0)
            {
                return;
            }

            var resourcesByCategory = new Dictionary<string, List<FileResource>>();
            foreach (var resource in resources)
            {
                if (resource == null || resource.ResType == null)
                {
                    continue;
                }

                string category = customCategory ?? resource.ResType.Category;
                if (!resourcesByCategory.ContainsKey(category))
                {
                    resourcesByCategory[category] = new List<FileResource>();
                }
                resourcesByCategory[category].Add(resource);
            }

            foreach (var kvp in resourcesByCategory)
            {
                if (!_categoryItems.ContainsKey(kvp.Key))
                {
                    _categoryItems[kvp.Key] = new ResourceCategoryItem(kvp.Key);
                }
                foreach (var resource in kvp.Value)
                {
                    _categoryItems[kvp.Key].AddResource(resource);
                }
            }
        }

        public void RemoveUnusedCategories()
        {
            var emptyCategories = _categoryItems.Where(kvp => kvp.Value.ResourceCount == 0).Select(kvp => kvp.Key).ToList();
            foreach (var category in emptyCategories)
            {
                _categoryItems.Remove(category);
            }
        }

        public void SetFilterString(string filterString)
        {
            _filterString = filterString == null ? string.Empty : filterString.Trim();
        }

        public IEnumerable<string> GetCategories()
        {
            return _categoryItems.Keys;
        }

        public IEnumerable<ResourceStandardItem> GetResourcesInCategory(string category)
        {
            if (!_categoryItems.ContainsKey(category))
            {
                return Enumerable.Empty<ResourceStandardItem>();
            }

            var items = _categoryItems[category].GetResources();
            if (string.IsNullOrEmpty(_filterString))
            {
                return items;
            }

            return items.Where(item =>
                (item.Text != null && item.Text.IndexOf(_filterString, StringComparison.OrdinalIgnoreCase) >= 0) ||
                (item.Resource != null && item.Resource.ResName != null &&
                 item.Resource.ResName.IndexOf(_filterString, StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }

    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:69
    // Original: class ResourceStandardItem(QStandardItem):
    public class ResourceStandardItem
    {
        public FileResource Resource { get; set; }
        public string Text { get; set; }

        public ResourceStandardItem(string text, FileResource resource)
        {
            Text = text;
            Resource = resource;
        }
    }

    public class ResourceCategoryItem
    {
        public string CategoryName { get; }
        private readonly List<ResourceStandardItem> _resources = new List<ResourceStandardItem>();

        public ResourceCategoryItem(string categoryName)
        {
            CategoryName = categoryName;
        }

        public void AddResource(FileResource resource)
        {
            _resources.Add(new ResourceStandardItem(resource.ResName, resource));
        }

        public int ResourceCount => _resources.Count;

        public IEnumerable<ResourceStandardItem> GetResources()
        {
            return _resources;
        }
    }

    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py
    // Original: UI wrapper class exposing all controls for testing
    public class ResourceListUi
    {
        public ComboBox SectionCombo { get; set; }
        public TextBox SearchEdit { get; set; }
        public Button ReloadButton { get; set; }
        public Button RefreshButton { get; set; }
        public TreeView ResourceTree { get; set; }
    }

    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/main_widgets.py:55
    // Original: sig_request_open_resource: Signal = Signal(list, object)
    // Event arguments for ResourceDoubleClicked event, containing the list of resources and useSpecializedEditor flag
    public class ResourceOpenEventArgs : EventArgs
    {
        public List<FileResource> Resources { get; }
        public bool? UseSpecializedEditor { get; }

        public ResourceOpenEventArgs(List<FileResource> resources, bool? useSpecializedEditor)
        {
            Resources = resources ?? new List<FileResource>();
            UseSpecializedEditor = useSpecializedEditor;
        }
    }

    // Matching PyKotor: sig_request_extract_resource
    public class ExtractRequestedEventArgs : EventArgs
    {
        public List<FileResource> Resources { get; }

        public ExtractRequestedEventArgs(List<FileResource> resources)
        {
            Resources = resources ?? new List<FileResource>();
        }
    }

    public class ResourceTreeNode : INotifyPropertyChanged
    {
        private static readonly FontWeight CategoryFontWeight = FontWeight.SemiBold;
        private static readonly FontWeight ResourceFontWeight = FontWeight.Normal;

        private readonly Func<IReadOnlyList<ResourceTreeNode>> _lazyChildrenLoader;
        private bool _childrenLoaded;
        private bool _isExpanded;

        private ResourceTreeNode(string title, bool isCategory, FileResource resource, int childCount, Func<IReadOnlyList<ResourceTreeNode>> lazyChildrenLoader)
        {
            Title = title ?? string.Empty;
            IsCategory = isCategory;
            Resource = resource;
            ChildCount = childCount;
            _lazyChildrenLoader = lazyChildrenLoader;
            Children = new ObservableCollection<ResourceTreeNode>();

            if (_lazyChildrenLoader != null && ChildCount > 0)
            {
                // Placeholder child keeps the expander visible without eagerly creating all rows.
                Children.Add(new ResourceTreeNode("Loading...", false, null, 0, null));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title { get; }
        public bool IsCategory { get; }
        public bool IsResourceNode => !IsCategory && Resource != null;
        public FileResource Resource { get; }
        public int ChildCount { get; }
        public string ItemCountLabel => ChildCount.ToString();
        public FontWeight TitleFontWeight => IsCategory ? CategoryFontWeight : ResourceFontWeight;
        public ObservableCollection<ResourceTreeNode> Children { get; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (_isExpanded == value)
                {
                    return;
                }

                _isExpanded = value;
                if (_isExpanded)
                {
                    EnsureChildrenLoaded();
                }
                OnPropertyChanged(nameof(IsExpanded));
            }
        }

        public static ResourceTreeNode CreateCategory(string title, int childCount, Func<IReadOnlyList<ResourceTreeNode>> lazyChildrenLoader)
        {
            return new ResourceTreeNode(title, true, null, childCount, lazyChildrenLoader);
        }

        public static ResourceTreeNode CreateResource(string title, FileResource resource)
        {
            return new ResourceTreeNode(title, false, resource, 0, null);
        }

        public void EnsureChildrenLoaded()
        {
            if (_childrenLoaded || _lazyChildrenLoader == null)
            {
                return;
            }

            _childrenLoaded = true;
            var nodes = _lazyChildrenLoader();
            Children.Clear();
            if (nodes == null)
            {
                return;
            }

            foreach (var node in nodes)
            {
                Children.Add(node);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
