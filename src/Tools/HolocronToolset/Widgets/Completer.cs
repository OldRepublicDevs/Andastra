using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;

namespace HolocronToolset.Widgets
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/nss.py:469-473
    // Original: QCompleter equivalent for Avalonia
    // Provides autocompletion functionality similar to Qt's QCompleter
    /// <summary>
    /// Provides autocompletion functionality for code editors.
    /// Equivalent to Qt's QCompleter, providing popup-based completion suggestions.
    /// </summary>
    public class Completer
    {
        private Control _widget;
        private Popup _popup;
        private ListBox _listBox;
        private List<string> _completionList;
        private string _completionPrefix;
        private bool _caseSensitive;
        private bool _wrapAround;
        private CompletionMode _completionMode;

        // Matching PyKotor: QCompleter.CompletionMode
        public enum CompletionMode
        {
            PopupCompletion,  // Show popup with completions
            InlineCompletion,  // Show inline completion
            UnfilteredPopupCompletion  // Show all completions in popup
        }

        // Matching PyKotor: QCompleter.setWidget()
        /// <summary>
        /// Sets the widget that this completer is associated with.
        /// </summary>
        public void SetWidget(Control widget)
        {
            _widget = widget;
        }

        // Matching PyKotor: QCompleter.widget()
        /// <summary>
        /// Gets the widget associated with this completer.
        /// </summary>
        public Control Widget()
        {
            return _widget;
        }

        // Matching PyKotor: QCompleter.setCompletionMode()
        /// <summary>
        /// Sets the completion mode (PopupCompletion, InlineCompletion, etc.).
        /// </summary>
        public void SetCompletionMode(CompletionMode mode)
        {
            _completionMode = mode;
        }

        // Matching PyKotor: QCompleter.setCaseSensitivity()
        /// <summary>
        /// Sets whether completion matching is case-sensitive.
        /// </summary>
        public void SetCaseSensitivity(bool caseSensitive)
        {
            _caseSensitive = caseSensitive;
        }

        // Matching PyKotor: QCompleter.setWrapAround()
        /// <summary>
        /// Sets whether completion wraps around when navigating.
        /// </summary>
        public void SetWrapAround(bool wrapAround)
        {
            _wrapAround = wrapAround;
        }

        // Matching PyKotor: QCompleter.setModel() equivalent
        /// <summary>
        /// Sets the completion model (list of completion strings).
        /// </summary>
        public void SetModel(List<string> completionList)
        {
            _completionList = completionList ?? new List<string>();
        }

        // Matching PyKotor: QCompleter.model() equivalent
        /// <summary>
        /// Gets the completion model.
        /// </summary>
        public List<string> Model()
        {
            return _completionList ?? new List<string>();
        }

        // Matching PyKotor: QCompleter.setCompletionPrefix()
        /// <summary>
        /// Sets the completion prefix (the text to match against).
        /// </summary>
        public void SetCompletionPrefix(string prefix)
        {
            _completionPrefix = prefix ?? "";
        }

        // Matching PyKotor: QCompleter.completionPrefix()
        /// <summary>
        /// Gets the current completion prefix.
        /// </summary>
        public string CompletionPrefix()
        {
            return _completionPrefix ?? "";
        }

        // Matching PyKotor: QCompleter.completionCount()
        /// <summary>
        /// Gets the number of available completions for the current prefix.
        /// </summary>
        public int CompletionCount()
        {
            if (_completionList == null || string.IsNullOrEmpty(_completionPrefix))
            {
                return 0;
            }

            return GetFilteredCompletions().Count;
        }

        // Matching PyKotor: QCompleter.currentCompletion()
        /// <summary>
        /// Gets the currently selected completion string.
        /// </summary>
        public string CurrentCompletion()
        {
            if (_listBox == null || _listBox.SelectedItem == null)
            {
                return "";
            }

            return _listBox.SelectedItem.ToString();
        }

        // Matching PyKotor: QCompleter.popup()
        /// <summary>
        /// Gets the popup widget used for displaying completions.
        /// </summary>
        public Popup Popup()
        {
            if (_popup == null)
            {
                InitializePopup();
            }
            return _popup;
        }

        // Matching PyKotor: QCompleter.complete()
        /// <summary>
        /// Shows the completion popup at the specified rectangle.
        /// </summary>
        public void Complete(Avalonia.Rect rect)
        {
            if (_widget == null || _completionList == null || _completionList.Count == 0)
            {
                return;
            }

            var filtered = GetFilteredCompletions();
            if (filtered.Count == 0)
            {
                return;
            }

            if (_popup == null)
            {
                InitializePopup();
            }

            // Update list box with filtered completions
            _listBox.ItemsSource = filtered;
            _listBox.SelectedIndex = 0;

            // Position popup near the widget
            var widgetBounds = _widget.Bounds;
            _popup.PlacementTarget = _widget;
            _popup.Placement = PlacementMode.Bottom;
            _popup.HorizontalOffset = rect.X;
            _popup.VerticalOffset = rect.Y + rect.Height;

            _popup.IsOpen = true;
        }

        // Get filtered completions based on prefix
        private List<string> GetFilteredCompletions()
        {
            if (_completionList == null || string.IsNullOrEmpty(_completionPrefix))
            {
                return _completionList ?? new List<string>();
            }

            StringComparison comparison = _caseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            return _completionList
                .Where(item => item.StartsWith(_completionPrefix, comparison))
                .ToList();
        }

        // Initialize the popup and list box
        private void InitializePopup()
        {
            _popup = new Popup
            {
                Placement = PlacementMode.Bottom,
                IsLightDismissEnabled = true,
                MaxHeight = 300,
                MinWidth = 200,
                MaxWidth = 500
            };

            _listBox = new ListBox
            {
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.Gray),
                BorderThickness = new Avalonia.Thickness(1),
                MaxHeight = 300,
                AlternationCount = 2
            };

            // Style for alternating rows
            _listBox.Resources.Add("AlternateItemBackground", new SolidColorBrush(Color.FromRgb(245, 245, 245)));

            _listBox.SelectionChanged += (s, e) =>
            {
                // Handle selection change
            };

            _listBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter || e.Key == Key.Return)
                {
                    // Insert completion
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape)
                {
                    _popup.IsOpen = false;
                    e.Handled = true;
                }
            };

            _popup.Child = _listBox;
        }
    }
}

