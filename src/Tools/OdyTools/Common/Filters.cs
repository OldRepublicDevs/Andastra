using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;

namespace OdyTools.Common
{
    public abstract class TemplateFilterProxyModel
    {
        public abstract object GetSortValue(int index);
    }

    public class RobustSortFilterProxyModel : TemplateFilterProxyModel
    {
        private Dictionary<int, int> _sortStates = new Dictionary<int, int>();

        public void ToggleSort(int column)
        {
            if (!_sortStates.ContainsKey(column))
            {
                _sortStates[column] = 0;
            }
            _sortStates[column] = (_sortStates[column] + 1) % 3;

            if (_sortStates[column] == 0)
            {
                ResetSort();
            }
            else if (_sortStates[column] == 1)
            {
                // Sort ascending - would need to implement with actual model
            }
            else if (_sortStates[column] == 2)
            {
                // Sort descending - would need to implement with actual model
            }
        }

        public void ResetSort()
        {
            _sortStates.Clear();
        }

        public override object GetSortValue(int index)
        {
            // Would need actual model implementation
            return null;
        }
    }

    public class NoScrollEventFilter
    {
        private Control _parentWidget;
        private readonly HashSet<Control> _filteredControls = new HashSet<Control>();

        // In Avalonia, we handle scroll events by intercepting PointerWheelChanged events
        // and forwarding them to the parent widget to prevent scrollbar interaction with controls
        private void OnPointerWheelChanged(object sender, PointerWheelEventArgs evt)
        {
            if (sender is Control control && _parentWidget != null)
            {
                // Forward the wheel event to the parent widget
                // This prevents controls like ComboBox, Slider, etc. from intercepting scroll events
                // and allows the parent window/scrollviewer to handle scrolling instead
                // Avalonia PointerWheelEventArgs - forward the event by raising it on parent
                // Note: We can't create a new PointerWheelEventArgs easily, so we just mark as handled
                // and let the parent handle scrolling naturally
                // Mark event as handled to prevent the control from processing it
                evt.Handled = true;
            }
        }

        public void SetupFilter(Control parentWidget, Type[] includeTypes = null)
        {
            if (parentWidget == null)
            {
                return;
            }

            _parentWidget = parentWidget;

            if (includeTypes == null)
            {
                includeTypes = new[] { typeof(ComboBox), typeof(Slider), typeof(NumericUpDown), typeof(CheckBox) };
            }

            // Recursively install event filters on child widgets
            SetupFilterRecursive(parentWidget, includeTypes);
        }

        private void SetupFilterRecursive(Control widget, Type[] includeTypes)
        {
            if (widget == null)
            {
                return;
            }

            foreach (Type includeType in includeTypes)
            {
                if (includeType.IsInstanceOfType(widget))
                {
                    // Install event filter - attach PointerWheelChanged handler
                    // Only attach once per control to avoid duplicate handlers
                    if (!_filteredControls.Contains(widget))
                    {
                        widget.PointerWheelChanged += OnPointerWheelChanged;
                        _filteredControls.Add(widget);
                    }
                }
            }

            // Recursively process children
            // Handle different container types in Avalonia
            if (widget is Panel panel)
            {
                foreach (var child in panel.Children)
                {
                    if (child is Control control)
                    {
                        SetupFilterRecursive(control, includeTypes);
                    }
                }
            }
            else if (widget is Decorator decorator && decorator.Child is Control childControl)
            {
                SetupFilterRecursive(childControl, includeTypes);
            }
            else if (widget is ContentControl contentControl && contentControl.Content is Control contentChild)
            {
                SetupFilterRecursive(contentChild, includeTypes);
            }
        }
    }

    public class HoverEventFilter
    {
        public bool EventFilter(Control obj, PointerEventArgs evt)
        {
            // Handle hover events in Avalonia
            // This would need proper implementation with PointerEntered/PointerExited
            return false;
        }
    }
}
