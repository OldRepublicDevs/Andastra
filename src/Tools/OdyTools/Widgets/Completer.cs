using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;

namespace OdyTools.Widgets
{
    /// <summary>
    /// Represents a single completion option with optional different display and insert text.
    /// </summary>
    public struct CompletionItem
    {
        public string DisplayText { get; set; }
        public string InsertText { get; set; }

        public CompletionItem(string displayText, string insertText = null)
        {
            DisplayText = displayText ?? "";
            InsertText = insertText ?? displayText ?? "";
        }

        public override string ToString() => DisplayText;
    }

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
        private List<CompletionItem> _completionItems;
        private string _completionPrefix;
        private bool _caseSensitive;
        private bool _wrapAround;
        private CompletionMode _completionMode;

        /// <summary>
        /// Raised when the user commits a completion (Enter, Tab, or double-click).
        /// Argument is the text to insert at the cursor.
        /// </summary>
        public event Action<string> CompletionSelected;

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
        /// Sets the completion model (list of completion strings). Display and insert text are the same.
        /// </summary>
        public void SetModel(List<string> completionList)
        {
            _completionList = completionList ?? new List<string>();
            _completionItems = null;
        }

        /// <summary>
        /// Sets the completion model with separate display and insert text for full IntelliSense-style completion.
        /// </summary>
        public void SetModelWithInsertText(List<CompletionItem> items)
        {
            _completionItems = items ?? new List<CompletionItem>();
            _completionList = _completionItems.Select(i => i.DisplayText).ToList();
        }

        // Matching PyKotor: QCompleter.model() equivalent
        /// <summary>
        /// Gets the completion model (display strings).
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
            if ((_completionList == null && _completionItems == null) || string.IsNullOrEmpty(_completionPrefix))
            {
                return 0;
            }

            return GetFilteredCompletionsList().Count;
        }

        // Matching PyKotor: QCompleter.currentCompletion()
        /// <summary>
        /// Gets the currently selected completion display string.
        /// </summary>
        public string CurrentCompletion()
        {
            if (_listBox == null || _listBox.SelectedItem == null)
            {
                return "";
            }

            return _listBox.SelectedItem.ToString();
        }

        /// <summary>
        /// Gets the text to insert for the currently selected completion.
        /// </summary>
        public string CurrentInsertText()
        {
            if (_listBox == null || _listBox.SelectedItem == null)
            {
                return "";
            }

            if (_listBox.SelectedItem is CompletionItem item)
            {
                return item.InsertText ?? item.DisplayText ?? "";
            }

            return _listBox.SelectedItem.ToString();
        }

        /// <summary>
        /// Returns true if the completion popup is currently open.
        /// </summary>
        public bool IsPopupOpen => _popup != null && _popup.IsOpen;

        /// <summary>
        /// Closes the completion popup without inserting.
        /// </summary>
        public void ClosePopup()
        {
            if (_popup != null)
            {
                _popup.IsOpen = false;
            }
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
            var filtered = GetFilteredCompletionsList();
            if (_widget == null || filtered.Count == 0)
            {
                return;
            }

            if (_popup == null)
            {
                InitializePopup();
            }

            // Update list box with filtered completions (CompletionItem.ToString() shows DisplayText)
            _listBox.ItemsSource = filtered;
            _listBox.SelectedIndex = 0;

            // Position popup near the widget
            _popup.PlacementTarget = _widget;
            _popup.Placement = PlacementMode.Bottom;
            _popup.HorizontalOffset = rect.X;
            _popup.VerticalOffset = rect.Y + rect.Height;

            _popup.IsOpen = true;
        }

        /// <summary>
        /// Returns filtered completion items for the current prefix (matched against InsertText).
        /// </summary>
        private List<CompletionItem> GetFilteredCompletionsList()
        {
            StringComparison comparison = _caseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            if (_completionItems != null && _completionItems.Count > 0)
            {
                if (string.IsNullOrEmpty(_completionPrefix))
                {
                    return new List<CompletionItem>(_completionItems);
                }
                return _completionItems
                    .Where(item => (item.InsertText ?? item.DisplayText ?? "").StartsWith(_completionPrefix, comparison))
                    .ToList();
            }

            if (_completionList == null || _completionList.Count == 0)
            {
                return new List<CompletionItem>();
            }

            if (string.IsNullOrEmpty(_completionPrefix))
            {
                return _completionList.Select(s => new CompletionItem(s, s)).ToList();
            }

            return _completionList
                .Where(s => s.StartsWith(_completionPrefix, comparison))
                .Select(s => new CompletionItem(s, s))
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
            };

            // Style for alternating rows
            _listBox.Resources.Add("AlternateItemBackground", new SolidColorBrush(Color.FromRgb(245, 245, 245)));

            _listBox.SelectionChanged += (s, e) => { };

            _listBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                {
                    CommitSelection();
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape)
                {
                    ClosePopup();
                    e.Handled = true;
                }
            };

            _listBox.DoubleTapped += (s, e) =>
            {
                CommitSelection();
            };

            _popup.Child = _listBox;
        }

        private void CommitSelection()
        {
            if (!IsPopupOpen)
            {
                return;
            }

            string insertText = CurrentInsertText();
            ClosePopup();
            if (!string.IsNullOrEmpty(insertText))
            {
                CompletionSelected?.Invoke(insertText);
            }
        }
    }
}

