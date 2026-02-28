using System;
using System.Collections.Generic;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using OdyTools.Editors;
using Avalonia.Controls.Templates;

namespace OdyTools.Editors.DLG
{
    /// <summary>
    /// List widget for displaying DLG links with word wrap, custom item template, and full drag-and-drop.
    /// </summary>
    public class DLGListWidget : ListBox
    {
        private const int DisplayRole = 0;
        private const int ExtraDisplayRole = 2;

        private OdyToolDLG _editor;
        private DLGListWidgetItem _draggedItem;
        private DLGListWidgetItem _currentlyHoveredItem;
        private bool _useHoverText = true;
        private bool _useWordWrap = true;
        private Point? _dragStartPosition;
        private bool _listDragStarted;

        /// <summary>
        /// Initializes a new instance of the DLGListWidget class.
        /// </summary>
        public DLGListWidget()
        {
            EnsureItemTemplate();
            PointerMoved += OnPointerMoved;
            PointerPressed += OnPointerPressed;
            PointerReleased += OnPointerReleased;
            PointerExited += OnPointerExited;
            LostFocus += OnLostFocus;
        }

        /// <summary>
        /// Gets or sets the editor associated with this widget.
        /// </summary>
        public OdyToolDLG Editor
        {
            get => _editor;
            set => _editor = value;
        }

        /// <summary>
        /// Gets or sets whether to use hover text (swap display on hover).
        /// </summary>
        public bool UseHoverText
        {
            get => _useHoverText;
            set => _useHoverText = value;
        }

        /// <summary>
        /// Gets or sets whether list item text wraps. When true, long text wraps; when false, single line with ellipsis behavior.
        /// </summary>
        public bool UseWordWrap
        {
            get => _useWordWrap;
            set => _useWordWrap = value;
        }

        /// <summary>
        /// Initializes a new instance of DLGListWidget.
        /// </summary>
        /// <param name="editor">The DLG editor associated with this widget.</param>
        public DLGListWidget(OdyToolDLG editor)
        {
            _editor = editor;
            EnsureItemTemplate();
            SelectionChanged += OnSelectionChanged;
            DoubleTapped += OnDoubleTapped;
            PointerMoved += OnPointerMoved;
            PointerPressed += OnPointerPressed;
            PointerReleased += OnPointerReleased;
            PointerExited += OnPointerExited;
            LostFocus += OnLostFocus;
        }

        private void EnsureItemTemplate()
        {
            ItemTemplate = new FuncDataTemplate<DLGListWidgetItem>((item, _) =>
            {
                var textBlock = new TextBlock
                {
                    TextWrapping = _useWordWrap ? TextWrapping.Wrap : TextWrapping.NoWrap,
                    TextTrimming = TextTrimming.CharacterEllipsis
                };
                textBlock.Bind(TextBlock.TextProperty, new Binding(nameof(DLGListWidgetItem.PlainDisplayText)));
                var border = new Border
                {
                    Child = textBlock,
                    Padding = new Thickness(4, 2),
                    Background = Brushes.Transparent
                };
                border.Bind(ToolTip.TipProperty, new Binding(nameof(DLGListWidgetItem.TooltipText)));
                return border;
            });
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_editor != null && SelectedItem is DLGListWidgetItem item && item.Link != null)
            {
                _editor.JumpToNode(item.Link);
            }
        }

        private void OnDoubleTapped(object sender, TappedEventArgs e)
        {
            if (_editor != null && SelectedItem is DLGListWidgetItem item && item.Link != null)
            {
                _editor.FocusOnNode(item.Link);
            }
        }

        private DLGListWidgetItem GetItemAtPoint(Point point)
        {
            for (int i = 0; i < _items.Count; i++)
            {
                Control container = ContainerFromIndex(i);
                if (container == null)
                {
                    continue;
                }
                var transform = container.TransformToVisual(this);
                if (!transform.HasValue)
                {
                    continue;
                }
                var topLeft = transform.Value.Transform(new Point(0, 0));
                var bottomRight = transform.Value.Transform(new Point(container.Bounds.Width, container.Bounds.Height));
                var rect = new Rect(topLeft, bottomRight);
                if (rect.Contains(point))
                {
                    return GetItem(i);
                }
            }
            return null;
        }

        private static void SwapDisplayText(DLGListWidgetItem item)
        {
            if (item == null || item.IsDeleted())
            {
                return;
            }
            object hoverDisplay = item.GetData(ExtraDisplayRole);
            object defaultDisplay = item.GetData(DisplayRole);
            item.SetData(DisplayRole, hoverDisplay);
            item.SetData(ExtraDisplayRole, defaultDisplay);
        }

        private void ResetHover()
        {
            if (_currentlyHoveredItem != null && !_currentlyHoveredItem.IsDeleted())
            {
                int idx = _items.IndexOf(_currentlyHoveredItem);
                if (idx >= 0)
                {
                    DLGListWidget.SwapDisplayText(_currentlyHoveredItem);
                }
            }
            _currentlyHoveredItem = null;
            _draggedItem = null;
            _dragStartPosition = null;
            _listDragStarted = false;
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            Point point = e.GetPosition(this);
            var pointer = e.GetCurrentPoint(this);

            // Drag source: start drag when pointer moved beyond threshold with left button down
            if (_draggedItem != null && _editor != null && !_listDragStarted && pointer.Properties.IsLeftButtonPressed && _dragStartPosition.HasValue)
            {
                double dx = point.X - _dragStartPosition.Value.X;
                double dy = point.Y - _dragStartPosition.Value.Y;
                if (Math.Abs(dx) > 4 || Math.Abs(dy) > 4)
                {
                    _listDragStarted = true;
                    _dragStartPosition = null;
                    _editor.StartDragFromListWidget(_draggedItem, e);
                    return;
                }
            }

            if (!_useHoverText || _listDragStarted)
            {
                return;
            }
            DLGListWidgetItem item = GetItemAtPoint(point);
            if (item == null || item == _currentlyHoveredItem)
            {
                return;
            }
            if (_currentlyHoveredItem != null && !_currentlyHoveredItem.IsDeleted())
            {
                int idx = _items.IndexOf(_currentlyHoveredItem);
                if (idx >= 0)
                {
                    SwapDisplayText(_currentlyHoveredItem);
                }
            }
            _currentlyHoveredItem = item;
            SwapDisplayText(item);
            InvalidateVisual();
        }

        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            Point point = e.GetPosition(this);
            _draggedItem = GetItemAtPoint(point);
            _dragStartPosition = point;
            _listDragStarted = false;
        }

        private void OnPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            _draggedItem = null;
            _dragStartPosition = null;
            _listDragStarted = false;
        }

        private void OnPointerExited(object sender, PointerEventArgs e)
        {
            ResetHover();
        }

        private void OnLostFocus(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            ResetHover();
        }

        /// <summary>
        /// Updates the item text and formatting based on the node data.
        /// </summary>
        public void UpdateItem(DLGListWidgetItem item, Tuple<string, string, string> cachedPaths = null)
        {
            if (_editor == null || item == null || item.Link == null)
            {
                return;
            }

            Tuple<string, string, string> paths = cachedPaths ?? _editor.GetItemDlgPaths(item);
            string linkParentPath = paths.Item1;
            string linkPartialPath = paths.Item2;
            string nodePath = paths.Item3;

            bool isEntry = item.Link.Node is DLGEntry;
            string color = isEntry ? "red" : "blue";

            if (!string.IsNullOrEmpty(linkParentPath))
            {
                linkParentPath += "\\";
            }
            else
            {
                linkParentPath = "";
            }

            string hoverText1 = $"<span style='color:{color}; display:inline-block; vertical-align:top;'>{linkPartialPath} --></span>";
            string displayText2 = $"<div class='link-hover-text' style='display:inline-block; vertical-align:top; color:{color}; text-align:center;'>{nodePath}</div>";

            string defaultDisplay = $"<div class='link-container' style='white-space: nowrap;'>{displayText2}</div>";
            string hoverDisplay = $"<div class='link-container' style='white-space: nowrap;'>{hoverText1}{displayText2}</div>";

            item.SetData(DisplayRole, defaultDisplay);
            item.SetData(ExtraDisplayRole, hoverDisplay);

            // Get tooltip text
            string text;
            if (_editor?.Installation == null)
            {
                // When installation is not available, use a proper string representation similar to Python's repr()
                // Format: "DLGEntry(ListIndex=0)" or "DLGReply(ListIndex=1)"
                DLGNode node = item.Link?.Node;
                if (node == null)
                {
                    text = "";
                }
                else
                {
                    string nodeType = node is DLGEntry ? "DLGEntry" : "DLGReply";
                    text = $"{nodeType}(ListIndex={node.ListIndex})";
                }
            }
            else
            {
                // When installation is available, use it to get the localized string from TLK
                DLGNode node = item.Link?.Node;
                if (node?.Text != null)
                {
                    text = _editor.Installation.String(node.Text, "");
                }
                else
                {
                    text = "";
                }
            }
            item.TooltipText = $"{text}\n\n<i>Right click for more options</i>";
        }

        private readonly List<DLGListWidgetItem> _items = new List<DLGListWidgetItem>();

        /// <summary>
        /// Adds an item to the list.
        /// </summary>
        public void AddItem(DLGListWidgetItem item)
        {
            if (item == null)
            {
                return;
            }
            _items.Add(item);
            // In Avalonia ListBox, we need to use Items collection
            if (Items is System.Collections.IList list)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// Clears all items from the list.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
            if (Items is System.Collections.IList list)
            {
                list.Clear();
            }
        }

        /// <summary>
        /// Removes a single item from the list (used e.g. for Unpin).
        /// </summary>
        public void RemoveItem(DLGListWidgetItem item)
        {
            if (item == null)
            {
                return;
            }
            _items.Remove(item);
            if (Items is System.Collections.IList list)
            {
                list.Remove(item);
            }
        }

        /// <summary>
        /// Gets the item at the specified index.
        /// </summary>
        public DLGListWidgetItem GetItem(int index)
        {
            if (index >= 0 && index < _items.Count)
            {
                return _items[index];
            }
            return null;
        }

        /// <summary>
        /// Gets the number of items in the list.
        /// </summary>
        public int Count => _items.Count;
    }
}

