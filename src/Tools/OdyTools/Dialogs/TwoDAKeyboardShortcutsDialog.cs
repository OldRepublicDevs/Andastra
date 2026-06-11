using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;

namespace OdyTools.Dialogs
{
    /// <summary>
    /// Dialog listing keyboard shortcuts for the OdyTool 2DA spreadsheet editor.
    /// </summary>
    public class TwoDAKeyboardShortcutsDialog : Window
    {
        private StackPanel _mainPanel;

        public TwoDAKeyboardShortcutsDialog()
        {
            Title = "Keyboard Shortcuts";
            Width = 600;
            Height = 500;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CanResize = true;
            SetupUI();
        }

        private void SetupUI()
        {
            _mainPanel = new StackPanel
            {
                Margin = new Thickness(15),
                Spacing = 15
            };

            _mainPanel.Children.Add(new TextBlock
            {
                Text = "OdyTool 2DA Keyboard Shortcuts",
                FontSize = 18,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            });

            AddShortcutCategory("File", new[]
            {
                ("Ctrl+S", "Save"),
                ("Ctrl+Shift+S", "Save As"),
            });

            AddShortcutCategory("Edit", new[]
            {
                ("Ctrl+Z", "Undo"),
                ("Ctrl+Y", "Redo"),
                ("Ctrl+X", "Cut"),
                ("Ctrl+C", "Copy"),
                ("Ctrl+V", "Paste"),
                ("Ctrl+A", "Select All"),
                ("Ctrl+D", "Fill Down"),
                ("Ctrl+F", "Find"),
                ("Ctrl+H", "Replace"),
                ("Ctrl+G", "Go to Row"),
                ("Shift+Delete", "Remove Rows"),
            });

            AddShortcutCategory("Navigation", new[]
            {
                ("F2", "Edit cell"),
                ("F3", "Find next"),
                ("Home", "First cell in row"),
                ("End", "Last cell in row"),
                ("Ctrl+Home", "Jump to first row"),
                ("Ctrl+End", "Jump to last row"),
                ("PgUp / PgDn", "Scroll page"),
                ("Shift+Arrow", "Extend selection"),
                ("Esc", "Refocus grid"),
            });

            AddShortcutCategory("Rows & Columns", new[]
            {
                ("Alt+Up", "Move row up"),
                ("Alt+Down", "Move row down"),
                ("Ctrl+Shift+Left", "Move column left"),
                ("Ctrl+Shift+Right", "Move column right"),
            });

            AddShortcutCategory("View", new[]
            {
                ("F9", "Toggle sidebar"),
                ("Ctrl+L", "Focus search"),
            });

            var closeButton = new Button
            {
                Content = "Close",
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0),
                MinWidth = 100
            };
            closeButton.Click += (s, e) => Close();
            _mainPanel.Children.Add(closeButton);

            Content = new ScrollViewer
            {
                Content = _mainPanel,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            KeyDown += (s, e) =>
            {
                if (e.Key == Key.Escape)
                {
                    Close();
                }
            };
        }

        private void AddShortcutCategory(string categoryName, IEnumerable<(string shortcut, string description)> shortcuts)
        {
            if (_mainPanel == null) return;

            _mainPanel.Children.Add(new TextBlock
            {
                Text = categoryName,
                FontSize = 14,
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(0, 10, 0, 5),
                Foreground = new SolidColorBrush(Color.FromRgb(0, 122, 204))
            });

            foreach (var (shortcut, description) in shortcuts)
            {
                var shortcutPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 2, 0, 2)
                };

                shortcutPanel.Children.Add(new TextBlock
                {
                    Text = shortcut,
                    FontFamily = new FontFamily("Consolas, Monaco, 'Courier New', monospace"),
                    Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                    Padding = new Thickness(6, 2, 6, 2),
                    Margin = new Thickness(0, 0, 10, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 11,
                    FontWeight = FontWeight.Bold
                });

                shortcutPanel.Children.Add(new TextBlock
                {
                    Text = description,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 12
                });

                _mainPanel.Children.Add(shortcutPanel);
            }
        }
    }
}
