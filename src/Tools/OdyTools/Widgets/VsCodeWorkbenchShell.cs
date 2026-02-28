using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using OdyTools.Themes;

namespace OdyTools.Widgets
{
    /// <summary>
    /// VS Code/Monaco workbench shell: Activity Bar (48px) | Sidebar | Editor + Panel (bottom).
    /// 1:1 visual parity with VS Code Dark+ theme.
    /// </summary>
    public class VsCodeWorkbenchShell : Panel
    {
        private const double ActivityBarWidth = 48;
        private const double SidebarDefaultWidth = 260;
        private const double PanelDefaultHeight = 200;
        private const double StatusBarHeight = 22;

        private Grid _rootGrid;
        private Border _activityBar;
        private Border _sidebar;
        private Border _editorArea;
        private Border _panel;
        private Border _statusBar;
        private GridSplitter _sidebarSplitter;
        private GridSplitter _panelSplitter;

        private bool _sidebarVisible = true;
        private bool _panelVisible = false;
        private Control _sidebarContent;
        private Control _editorContent;
        private Control _panelContent;
        private Control _statusBarContent;

        public Control SidebarContent
        {
            get => _sidebarContent;
            set { _sidebarContent = value; UpdateSidebarContent(); }
        }

        public Control EditorContent
        {
            get => _editorContent;
            set { _editorContent = value; UpdateEditorContent(); }
        }

        public Control PanelContent
        {
            get => _panelContent;
            set { _panelContent = value; UpdatePanelContent(); }
        }

        public Control StatusBarContent
        {
            get => _statusBarContent;
            set { _statusBarContent = value; UpdateStatusBarContent(); }
        }

        public bool SidebarVisible
        {
            get => _sidebarVisible;
            set { _sidebarVisible = value; UpdateSidebarVisibility(); }
        }

        public bool PanelVisible
        {
            get => _panelVisible;
            set { _panelVisible = value; UpdatePanelVisibility(); }
        }

        public void ToggleSidebar() => SidebarVisible = !SidebarVisible;
        public void TogglePanel() => PanelVisible = !PanelVisible;

        public VsCodeWorkbenchShell()
        {
            ClipToBounds = true;
            Background = MonacoColors.EditorBackgroundBrush;
            BuildLayout();
        }

        private void BuildLayout()
        {
            _rootGrid = new Grid
            {
                RowDefinitions = new RowDefinitions("*,Auto"),
                ColumnDefinitions = new ColumnDefinitions("*")
            };

            // Main: Activity Bar | Sidebar | Splitter | Editor+Panel column
            var mainGrid = new Grid
            {
                RowDefinitions = new RowDefinitions("*"),
                ColumnDefinitions = new ColumnDefinitions($"Auto,{SidebarDefaultWidth},4,*")
            };

            // Activity Bar (VS Code 1:1: 48px wide, icons 48px height each)
            var activityBarWidget = new ActivityBarWidget();
            activityBarWidget.AddItem("explorer", "Explorer (Ctrl+Shift+E)", "\uE8B5");
            activityBarWidget.AddItem("search", "Search (Ctrl+Shift+F)", "\uE721");
            activityBarWidget.AddItem("outline", "Outline", "\uE8FD");
            activityBarWidget.AddItem("problems", "Problems (Ctrl+Shift+M)", "\uE7BA");
            activityBarWidget.ItemClicked += (s, idx) => OnActivityBarItemClicked(idx);
            _activityBar = new Border
            {
                Width = ActivityBarWidth,
                Background = new SolidColorBrush(MonacoColors.ActivityBarBackground),
                BorderBrush = new SolidColorBrush(MonacoColors.WidgetBorder),
                BorderThickness = new Thickness(0, 0, 1, 0),
                Child = activityBarWidget
            };
            Grid.SetColumn(_activityBar, 0);
            mainGrid.Children.Add(_activityBar);

            // Sidebar
            _sidebar = new Border
            {
                Width = SidebarDefaultWidth,
                MinWidth = 120,
                MaxWidth = 480,
                Background = new SolidColorBrush(MonacoColors.SideBarBackground),
                BorderBrush = new SolidColorBrush(MonacoColors.WidgetBorder),
                BorderThickness = new Thickness(0, 0, 1, 0)
            };
            Grid.SetColumn(_sidebar, 1);
            mainGrid.Children.Add(_sidebar);

            _sidebarSplitter = new GridSplitter
            {
                Width = 4,
                Background = new SolidColorBrush(MonacoColors.WidgetBorder),
                ResizeDirection = GridResizeDirection.Columns
            };
            Grid.SetColumn(_sidebarSplitter, 2);
            mainGrid.Children.Add(_sidebarSplitter);

            // Center: Editor (top) + Panel (bottom)
            var centerGrid = new Grid
            {
                RowDefinitions = new RowDefinitions("*,4,Auto"),
                ColumnDefinitions = new ColumnDefinitions("*")
            };

            _editorArea = new Border { Background = MonacoColors.EditorBackgroundBrush };
            Grid.SetRow(_editorArea, 0);
            centerGrid.Children.Add(_editorArea);

            _panelSplitter = new GridSplitter
            {
                Height = 4,
                Background = new SolidColorBrush(MonacoColors.WidgetBorder),
                ResizeDirection = GridResizeDirection.Rows
            };
            Grid.SetRow(_panelSplitter, 1);
            _panelSplitter.IsVisible = false;
            centerGrid.Children.Add(_panelSplitter);

            _panel = new Border
            {
                Height = PanelDefaultHeight,
                MinHeight = 80,
                MaxHeight = 600,
                Background = new SolidColorBrush(MonacoColors.PanelBackground),
                BorderBrush = new SolidColorBrush(MonacoColors.WidgetBorder),
                BorderThickness = new Thickness(0, 1, 0, 0),
                IsVisible = false
            };
            Grid.SetRow(_panel, 2);
            centerGrid.Children.Add(_panel);

            Grid.SetColumn(centerGrid, 3);
            mainGrid.Children.Add(centerGrid);

            _statusBar = new Border
            {
                Height = StatusBarHeight,
                Background = new SolidColorBrush(MonacoColors.StatusBarBackground),
                BorderBrush = new SolidColorBrush(MonacoColors.WidgetBorder),
                BorderThickness = new Thickness(0, 1, 0, 0)
            };
            _statusBar.Child = new DockPanel { LastChildFill = true };

            _rootGrid.Children.Add(mainGrid);
            Grid.SetRow(mainGrid, 0);
            _rootGrid.Children.Add(_statusBar);
            Grid.SetRow(_statusBar, 1);

            Children.Add(_rootGrid);
        }

        private void UpdateSidebarContent()
        {
            if (_sidebar != null) _sidebar.Child = _sidebarContent;
        }

        private void UpdateEditorContent()
        {
            if (_editorArea != null) _editorArea.Child = _editorContent;
        }

        private void UpdatePanelContent()
        {
            if (_panel != null) _panel.Child = _panelContent;
        }

        private void UpdateStatusBarContent()
        {
            if (_statusBar?.Child is DockPanel dock)
            {
                dock.Children.Clear();
                if (_statusBarContent != null) dock.Children.Add(_statusBarContent);
            }
        }

        private void UpdateSidebarVisibility()
        {
            if (_sidebar != null) _sidebar.IsVisible = _sidebarVisible;
            if (_sidebarSplitter != null) _sidebarSplitter.IsVisible = _sidebarVisible;
        }

        private void UpdatePanelVisibility()
        {
            if (_panel != null) _panel.IsVisible = _panelVisible;
            if (_panelSplitter != null) _panelSplitter.IsVisible = _panelVisible;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _rootGrid?.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            _rootGrid?.Measure(availableSize);
            return _rootGrid?.DesiredSize ?? availableSize;
        }

        protected virtual void OnActivityBarItemClicked(int index)
        {
        }
    }
}
