using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using OdyTools.Common;

namespace OdyTools.Editors
{
    /// <summary>
    /// Static utility helpers for OdyTool editors, centralizing common patterns for:
    /// - Event wiring (click, selection, value changes)
    /// - ComboBox population and text management
    /// - Control discovery from XAML trees
    /// - Menu configuration
    /// 
    /// Using these helpers reduces duplication across editors and ensures consistent behavior.
    /// All methods are null-safe: they silently skip null parameters rather than throwing exceptions.
    /// </summary>
    public static class EditorHelpers
    {
        /// <summary>
        /// Safely finds a control in a XAML control tree by name. Returns null if not found or if a discovery error occurs.
        /// This method is useful for locating controls in XAML-defined editors where the control may not always be present.
        /// </summary>
        /// <typeparam name="T">The control type to find (e.g., Button, ComboBox).</typeparam>
        /// <param name="control">The root control to search from.</param>
        /// <param name="name">The control's name to search for.</param>
        /// <returns>The found control of type T, or null if not found or an error occurred.</returns>
        public static T FindControlSafe<T>(Control control, string name) where T : Control
        {
            T nameScopeControl = null;
            try
            {
                nameScopeControl = FindInNameScope<T>(control, name);
            }
            catch
            {
                // Some controls can be queried before Avalonia has attached a name scope.
                // Fall back to walking the logical/visual tree below.
            }

            if (nameScopeControl != null)
            {
                return nameScopeControl;
            }

            try
            {
                return FindNamedDescendant<T>(control, name);
            }
            catch
            {
                return null;
            }
        }

        private static T FindInNameScope<T>(Control root, string name) where T : Control
        {
            if (root == null || string.IsNullOrEmpty(name))
            {
                return null;
            }

            var nameScope = NameScope.GetNameScope(root);
            if (nameScope == null)
            {
                return null;
            }

            return nameScope.Find(name) as T;
        }

        private static T FindNamedDescendant<T>(Control root, string name) where T : Control
        {
            if (root == null || string.IsNullOrEmpty(name))
            {
                return null;
            }

            var visited = new HashSet<Control>();
            var stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (current == null || !visited.Add(current))
                {
                    continue;
                }

                if (current is T match && match.Name == name)
                {
                    return match;
                }

                PushLogicalChildren(current, stack);
                PushVisualChildren(current, stack);
                PushContentChild(current, stack);
            }

            return null;
        }

        private static void PushLogicalChildren(Control root, Stack<Control> stack)
        {
            try
            {
                foreach (var child in root.GetLogicalChildren())
                {
                    if (child is Control control)
                    {
                        stack.Push(control);
                    }
                }
            }
            catch
            {
                // Logical children can be unavailable while detached.
            }
        }

        private static void PushVisualChildren(Control root, Stack<Control> stack)
        {
            try
            {
                foreach (var child in root.GetVisualChildren())
                {
                    if (child is Control control)
                    {
                        stack.Push(control);
                    }
                }
            }
            catch
            {
                // Visual children can be unavailable while detached.
            }
        }

        private static void PushContentChild(Control root, Stack<Control> stack)
        {
            if (root is ContentControl contentControl && contentControl.Content is Control content)
            {
                stack.Push(content);
            }
        }

        /// <summary>
        /// Finds and configures script field combo boxes from a XAML control tree.
        /// </summary>
        public static void InitializeScriptFieldsFromXaml(
            Control root,
            IEnumerable<string> scriptNames,
            IDictionary<string, ComboBox> scriptFields,
            Action<ComboBox, string> configureScriptCombo,
            bool includePascalCaseNameFallback = false)
        {
            if (root == null || scriptNames == null || scriptFields == null)
            {
                return;
            }

            foreach (string scriptName in scriptNames)
            {
                ComboBox scriptCombo = FindControlSafe<ComboBox>(root, scriptName.ToLowerInvariant() + "Edit");
                if (scriptCombo == null && includePascalCaseNameFallback)
                {
                    scriptCombo = FindControlSafe<ComboBox>(root, scriptName + "Edit");
                }

                if (scriptCombo == null)
                {
                    continue;
                }

                scriptCombo.IsEditable = true;
                configureScriptCombo?.Invoke(scriptCombo, scriptName);
                scriptFields[scriptName] = scriptCombo;
            }
        }

        /// <summary>
        /// Creates editable script field controls and appends them to the provided panel.
        /// </summary>
        public static void AddScriptFieldEditors(
            Panel container,
            IEnumerable<string> scriptNames,
            IDictionary<string, ComboBox> scriptFields,
            Action<ComboBox, string> configureScriptCombo)
        {
            if (container == null || scriptNames == null || scriptFields == null)
            {
                return;
            }

            foreach (string scriptName in scriptNames)
            {
                var scriptLabel = new TextBlock { Text = scriptName + ":" };
                var scriptCombo = new ComboBox { IsEditable = true };

                configureScriptCombo?.Invoke(scriptCombo, scriptName);
                scriptFields[scriptName] = scriptCombo;

                container.Children.Add(scriptLabel);
                container.Children.Add(scriptCombo);
            }
        }

        /// <summary>
        /// Wires a click handler only when the button exists.
        /// </summary>
        public static void BindClick(Button button, Action handler)
        {
            if (button == null || handler == null)
            {
                return;
            }

            button.Click += (s, e) => handler();
        }

        /// <summary>
        /// Wires a menu item click handler by control name when the item exists.
        /// </summary>
        public static void BindMenuClick(Control root, string menuItemName, Action handler)
        {
            if (root == null || string.IsNullOrWhiteSpace(menuItemName) || handler == null)
            {
                return;
            }

            MenuItem item = FindControlSafe<MenuItem>(root, menuItemName);
            if (item != null)
            {
                item.Click += (s, e) => handler();
            }
        }

        /// <summary>
        /// Wires multiple menu item click handlers in one pass.
        /// </summary>
        public static void BindMenuClicks(Control root, IEnumerable<(string menuItemName, Action handler)> bindings)
        {
            if (root == null || bindings == null)
            {
                return;
            }

            foreach (var (menuItemName, handler) in bindings)
            {
                BindMenuClick(root, menuItemName, handler);
            }
        }

        /// <summary>
        /// Wires a button click handler by control name when the button exists.
        /// </summary>
        public static void BindButtonClick(Control root, string buttonName, Action handler)
        {
            if (root == null || string.IsNullOrWhiteSpace(buttonName) || handler == null)
            {
                return;
            }

            Button button = FindControlSafe<Button>(root, buttonName);
            if (button != null)
            {
                button.Click += (s, e) => handler();
            }
        }

        /// <summary>
        /// Wires multiple button click handlers in one pass.
        /// </summary>
        public static void BindButtonClicks(Control root, IEnumerable<(string buttonName, Action handler)> bindings)
        {
            if (root == null || bindings == null)
            {
                return;
            }

            foreach (var (buttonName, handler) in bindings)
            {
                BindButtonClick(root, buttonName, handler);
            }
        }

        /// <summary>
        /// Wires both menu item and button click handlers by the same control name.
        /// Useful for actions exposed in both menu and toolbar contexts.
        /// </summary>
        public static void BindMenuOrButtonClick(Control root, string controlName, Action handler)
        {
            BindMenuClick(root, controlName, handler);
            BindButtonClick(root, controlName, handler);
        }

        /// <summary>
        /// Sets a menu item's header by name when the menu item exists.
        /// </summary>
        public static void SetMenuHeader(Control root, string menuItemName, string header)
        {
            if (root == null || string.IsNullOrWhiteSpace(menuItemName) || header == null)
            {
                return;
            }

            MenuItem item = FindControlSafe<MenuItem>(root, menuItemName);
            if (item != null)
            {
                item.Header = header;
            }
        }

        /// <summary>
        /// Sets a menu item's localized header by translation key.
        /// </summary>
        public static void SetLocalizedMenuHeader(Control root, string menuItemName, string localizationKey, bool prependUnderscore = true)
        {
            if (string.IsNullOrWhiteSpace(localizationKey))
            {
                return;
            }

            string localized = Localization.Tr(localizationKey);
            SetMenuHeader(root, menuItemName, prependUnderscore ? "_" + localized : localized);
        }

        /// <summary>
        /// Sets localized headers for multiple menu items in one pass.
        /// </summary>
        public static void SetLocalizedMenuHeaders(
            Control root,
            IEnumerable<(string menuItemName, string localizationKey)> items,
            bool prependUnderscore = true)
        {
            if (root == null || items == null)
            {
                return;
            }

            foreach (var item in items)
            {
                SetLocalizedMenuHeader(root, item.menuItemName, item.localizationKey, prependUnderscore);
            }
        }

        /// <summary>
        /// Sets the localized header of a parent menu by resolving the parent from a known child menu item.
        /// </summary>
        public static void SetLocalizedParentMenuHeader(Control root, string childMenuItemName, string localizationKey, bool prependUnderscore = true)
        {
            if (root == null || string.IsNullOrWhiteSpace(childMenuItemName) || string.IsNullOrWhiteSpace(localizationKey))
            {
                return;
            }

            MenuItem child = FindControlSafe<MenuItem>(root, childMenuItemName);
            MenuItem parent = child?.Parent as MenuItem;
            if (parent != null)
            {
                string localized = Localization.Tr(localizationKey);
                parent.Header = prependUnderscore ? "_" + localized : localized;
            }
        }

        /// <summary>
        /// Wires a combo box selection changed event to an action handler. No-op if either argument is null.
        /// </summary>
        public static void BindSelectionChanged(ComboBox comboBox, Action handler)
        {
            if (comboBox == null || handler == null)
            {
                return;
            }

            comboBox.SelectionChanged += (s, e) => handler();
        }

        /// <summary>
        /// Wires a check box IsCheckedChanged event to an action handler. No-op if either argument is null.
        /// </summary>
        public static void BindCheckedChanged(CheckBox checkBox, Action handler)
        {
            if (checkBox == null || handler == null)
            {
                return;
            }

            checkBox.IsCheckedChanged += (s, e) => handler();
        }

        /// <summary>
        /// Wires a <see cref="NumericUpDown"/> value changed event to an action handler. No-op if either argument is null.
        /// </summary>
        public static void BindValueChanged(NumericUpDown spin, Action handler)
        {
            if (spin == null || handler == null)
            {
                return;
            }

            spin.ValueChanged += (s, e) => handler();
        }

        /// <summary>
        /// Wires a <see cref="Slider"/> value changed event to an action handler. No-op if either argument is null.
        /// </summary>
        public static void BindValueChanged(Slider slider, Action handler)
        {
            if (slider == null || handler == null)
            {
                return;
            }

            slider.ValueChanged += (s, e) => handler();
        }

        /// <summary>
        /// Wires an async click handler to a button. No-op if either argument is null.
        /// </summary>
        public static void BindClickAsync(Button button, Func<Task> handler)
        {
            if (button == null || handler == null)
            {
                return;
            }

            button.Click += async (s, e) => await handler();
        }

        /// <summary>
        /// Wires a LostFocus event handler to a control. No-op if either argument is null.
        /// Useful for commit handlers on TextBox, NumericUpDown, and other input controls.
        /// </summary>
        public static void BindLostFocus(Control control, Action handler)
        {
            if (control == null || handler == null)
            {
                return;
            }

            control.LostFocus += (s, e) => handler();
        }

        /// <summary>
        /// Wires a LostFocus event handler to a control. No-op if either argument is null.
        /// Overload for handlers that use the standard <see cref="EventHandler"/> signature.
        /// </summary>
        public static void BindLostFocus(Control control, EventHandler handler)
        {
            if (control == null || handler == null)
            {
                return;
            }

            control.LostFocus += (s, e) => handler(s, e);
        }

        /// <summary>
        /// Clears a combo box and repopulates it with the given string items.
        /// No-op if <paramref name="combo"/> is null; skips null/empty item collections.
        /// </summary>
        public static void PopulateComboBox(ComboBox combo, IEnumerable<string> items)
        {
            if (combo == null)
            {
                return;
            }

            combo.Items.Clear();
            if (items != null)
            {
                foreach (string item in items)
                {
                    combo.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Wires a radio button's <see cref="Avalonia.Controls.Primitives.ToggleButton.Checked"/> event to an action handler.
        /// No-op if either argument is null.
        /// </summary>
        public static void BindRadioChecked(RadioButton radioButton, Action handler)
        {
            if (radioButton == null || handler == null)
            {
                return;
            }

            radioButton.Checked += (s, e) => handler();
        }

        /// <summary>
        /// Sets a combo box's editable text and attempts to select a matching item by value.
        /// If the text matches an item in the list the item is selected; otherwise the text is
        /// preserved as free-form input. No-op if <paramref name="combo"/> is null.
        /// </summary>
        public static void SetComboBoxText(ComboBox combo, string text)
        {
            if (combo == null)
            {
                return;
            }

            combo.Text = text ?? "";
            if (!string.IsNullOrEmpty(text))
            {
                int index = combo.Items.IndexOf(text);
                if (index >= 0)
                {
                    combo.SelectedIndex = index;
                }
            }
        }
    }
}
