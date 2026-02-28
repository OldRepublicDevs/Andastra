using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using OdyTools.Themes;

namespace OdyTools.Widgets
{
    /// <summary>
    /// VS Code-style Activity Bar: vertical strip with icon buttons and active indicator.
    /// 1:1 visual parity with VS Code Dark+ theme.
    /// </summary>
    public class ActivityBarWidget : Panel
    {
        public static readonly StyledProperty<int> SelectedIndexProperty =
            AvaloniaProperty.Register<ActivityBarWidget, int>(nameof(SelectedIndex), 0);

        public int SelectedIndex
        {
            get => GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        public event EventHandler<int> ItemClicked;

        private const double ActionHeight = 48;
        private const double IconSize = 24;
        private readonly List<ActivityBarItem> _items = new List<ActivityBarItem>();
        private StackPanel _stack;

        public ActivityBarWidget()
        {
            ClipToBounds = true;
            MinWidth = 48;
            Width = 48;
            Background = new SolidColorBrush(MonacoColors.ActivityBarBackground);
            _stack = new StackPanel { Orientation = Orientation.Vertical };
            Children.Add(_stack);
        }

        public void AddItem(string id, string tooltip, string iconSymbol = null)
        {
            var item = new ActivityBarItem
            {
                Index = _items.Count,
                Id = id,
                Tooltip = tooltip,
                IconSymbol = iconSymbol ?? GetDefaultIcon(_items.Count),
                IsActive = _items.Count == SelectedIndex
            };
            item.PointerPressed += (s, e) =>
            {
                SelectedIndex = item.Index;
                ItemClicked?.Invoke(this, item.Index);
            };
            _items.Add(item);
            _stack.Children.Add(item);
        }

        private static string GetDefaultIcon(int index)
        {
            switch (index)
            {
                case 0: return "\uE8B5"; // folder (codicons files)
                case 1: return "\uE8B6"; // search
                case 2: return "\uE8BA"; // list
                case 3: return "\uE717"; // warning
                default: return "\uE8AA"; // extensions
            }
        }

        public void Clear()
        {
            _items.Clear();
            _stack.Children.Clear();
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);
            if (change.Property == SelectedIndexProperty)
            {
                int idx = change.NewValue is int ? (int)change.NewValue : 0;
                for (int i = 0; i < _items.Count; i++)
                    _items[i].IsActive = i == idx;
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _stack.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            _stack.Measure(availableSize);
            return _stack.DesiredSize;
        }

        private class ActivityBarItem : Border
        {
            public int Index { get; set; }
            public string Id { get; set; }
            public string Tooltip { get; set; }
            public string IconSymbol { get; set; }
            private bool _isActive;
            private TextBlock _icon;
            private Border _activeIndicator;

            public bool IsActive
            {
                get => _isActive;
                set { _isActive = value; UpdateVisual(); }
            }

            public ActivityBarItem()
            {
                Height = ActionHeight;
                MinHeight = ActionHeight;
                Background = Brushes.Transparent;
                Cursor = new Cursor(StandardCursorType.Hand);
                HorizontalAlignment = HorizontalAlignment.Stretch;

                var grid = new Grid { ColumnDefinitions = new ColumnDefinitions("3,*") };
                _activeIndicator = new Border
                {
                    Width = 3,
                    Background = new SolidColorBrush(MonacoColors.ActivityBarActiveBorder),
                    IsVisible = false
                };
                var iconContainer = new Grid
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                _icon = new TextBlock
                {
                    Text = "E",
                    FontSize = 18,
                    FontFamily = new FontFamily("Segoe Fluent Icons, Segoe MDL2 Assets, Segoe UI Symbol, symbola, sans-serif"),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                iconContainer.Children.Add(_icon);
                Grid.SetColumn(_activeIndicator, 0);
                Grid.SetColumn(iconContainer, 1);
                grid.Children.Add(_activeIndicator);
                grid.Children.Add(iconContainer);
                Child = grid;
            }

            protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
            {
                base.OnAttachedToVisualTree(e);
                _icon.Text = IconSymbol ?? "E";
                ToolTip.SetTip(this, Tooltip ?? Id);
            }

            private void UpdateVisual()
            {
                _activeIndicator.IsVisible = _isActive;
                Background = _isActive ? new SolidColorBrush(MonacoColors.ActivityBarActiveBackground) : new SolidColorBrush(Colors.Transparent);
                _icon.Foreground = _isActive
                    ? new SolidColorBrush(MonacoColors.ActivityBarForeground)
                    : new SolidColorBrush(MonacoColors.ActivityBarInactiveForeground);
            }
        }
    }
}
