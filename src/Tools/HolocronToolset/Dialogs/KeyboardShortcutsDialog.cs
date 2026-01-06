using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace HolocronToolset.Dialogs
{
    /// <summary>
    /// Dialog showing all available keyboard shortcuts in the NSS Editor.
    /// Organized by category for easy reference.
    /// </summary>
    public partial class KeyboardShortcutsDialog : Window
    {
        private ScrollViewer _scrollViewer;
        private StackPanel _mainPanel;

        // Public parameterless constructor for XAML
        public KeyboardShortcutsDialog()
        {
            InitializeComponent();
            Title = "Keyboard Shortcuts";
            Width = 600;
            Height = 500;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            CanResize = true;
            SetupUI();
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
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

        private void SetupProgrammaticUI()
        {
            _mainPanel = new StackPanel
            {
                Margin = new Thickness(15),
                Spacing = 15
            };

            // Header
            var header = new TextBlock
            {
                Text = "NSS Editor Keyboard Shortcuts",
                FontSize = 18,
                FontWeight = FontWeight.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };
            _mainPanel.Children.Add(header);

            // Create shortcut categories
            AddShortcutCategory("File Operations", new[]
            {
                ("F5", "Compile Script"),
                ("Ctrl+Shift+P", "Show Command Palette")
            });

            AddShortcutCategory("Navigation", new[]
            {
                ("F12", "Go to Definition"),
                ("Shift+F12", "Find All References"),
                ("Ctrl+G", "Go to Line")
            });

            AddShortcutCategory("Editing", new[]
            {
                ("Ctrl+X", "Cut"),
                ("Ctrl+C", "Copy"),
                ("Ctrl+V", "Paste"),
                ("Ctrl+/", "Toggle Line Comment"),
                ("Ctrl+Z", "Undo"),
                ("Ctrl+Y", "Redo"),
                ("Ctrl+A", "Select All")
            });

            AddShortcutCategory("View", new[]
            {
                ("Ctrl+B", "Toggle File Explorer"),
                ("Ctrl+`", "Toggle Terminal Panel"),
                ("Ctrl+Mouse Wheel", "Zoom In/Out"),
                ("Ctrl+0", "Reset Zoom")
            });

            AddShortcutCategory("Search & Replace", new[]
            {
                ("Ctrl+F", "Find"),
                ("Ctrl+H", "Replace"),
                ("F3", "Find Next"),
                ("Shift+F3", "Find Previous"),
                ("Ctrl+Shift+F", "Find in Files")
            });

            AddShortcutCategory("Bookmarks", new[]
            {
                ("Ctrl+F2", "Toggle Bookmark"),
                ("F2", "Next Bookmark"),
                ("Shift+F2", "Previous Bookmark")
            });

            // Close button
            var closeButton = new Button
            {
                Content = "Close",
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 0),
                MinWidth = 100
            };
            closeButton.Click += (s, e) => Close();
            _mainPanel.Children.Add(closeButton);

            // Wrap in scroll viewer
            _scrollViewer = new ScrollViewer
            {
                Content = _mainPanel,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            Content = _scrollViewer;

            // Handle Escape key to close
            KeyDown += (s, e) =>
            {
                if (e.Key == Avalonia.Input.Key.Escape)
                {
                    Close();
                }
            };
        }

        private void SetupUI()
        {
            // Find controls from XAML if available
            _scrollViewer = this.FindControl<ScrollViewer>("scrollViewer");
            _mainPanel = this.FindControl<StackPanel>("mainPanel");

            if (_mainPanel != null)
            {
                // Clear existing content and rebuild
                _mainPanel.Children.Clear();

                // Header
                var header = new TextBlock
                {
                    Text = "NSS Editor Keyboard Shortcuts",
                    FontSize = 18,
                    FontWeight = FontWeight.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 0, 0, 10)
                };
                _mainPanel.Children.Add(header);

                // Add categories
                AddShortcutCategory("File Operations", new[]
                {
                    ("F5", "Compile Script"),
                    ("Ctrl+Shift+P", "Show Command Palette")
                });

                AddShortcutCategory("Navigation", new[]
                {
                    ("F12", "Go to Definition"),
                    ("Shift+F12", "Find All References"),
                    ("Ctrl+G", "Go to Line")
                });

                AddShortcutCategory("Editing", new[]
                {
                    ("Ctrl+X", "Cut"),
                    ("Ctrl+C", "Copy"),
                    ("Ctrl+V", "Paste"),
                    ("Ctrl+/", "Toggle Line Comment"),
                    ("Ctrl+Z", "Undo"),
                    ("Ctrl+Y", "Redo"),
                    ("Ctrl+A", "Select All")
                });

                AddShortcutCategory("View", new[]
                {
                    ("Ctrl+B", "Toggle File Explorer"),
                    ("Ctrl+`", "Toggle Terminal Panel"),
                    ("Ctrl+Mouse Wheel", "Zoom In/Out"),
                    ("Ctrl+0", "Reset Zoom")
                });

                AddShortcutCategory("Search & Replace", new[]
                {
                    ("Ctrl+F", "Find"),
                    ("Ctrl+H", "Replace"),
                    ("F3", "Find Next"),
                    ("Shift+F3", "Find Previous"),
                    ("Ctrl+Shift+F", "Find in Files")
                });

                AddShortcutCategory("Bookmarks", new[]
                {
                    ("Ctrl+F2", "Toggle Bookmark"),
                    ("F2", "Next Bookmark"),
                    ("Shift+F2", "Previous Bookmark")
                });

                // Close button
                var closeButton = this.FindControl<Button>("closeButton");
                if (closeButton != null)
                {
                    closeButton.Click += (s, e) => Close();
                }
                else
                {
                    closeButton = new Button
                    {
                        Content = "Close",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 20, 0, 0),
                        MinWidth = 100
                    };
                    closeButton.Click += (s, e) => Close();
                    _mainPanel.Children.Add(closeButton);
                }
            }

            // Handle Escape key to close
            KeyDown += (s, e) =>
            {
                if (e.Key == Avalonia.Input.Key.Escape)
                {
                    Close();
                }
            };
        }

        private void AddShortcutCategory(string categoryName, IEnumerable<(string shortcut, string description)> shortcuts)
        {
            if (_mainPanel == null) return;

            // Category header
            var categoryHeader = new TextBlock
            {
                Text = categoryName,
                FontSize = 14,
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(0, 10, 0, 5),
                Foreground = new SolidColorBrush(Color.FromRgb(0, 122, 204)) // Blue color
            };
            _mainPanel.Children.Add(categoryHeader);

            // Shortcuts list
            foreach (var (shortcut, description) in shortcuts)
            {
                var shortcutPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 2, 0, 2)
                };

                // Shortcut key
                var keyText = new TextBlock
                {
                    Text = shortcut,
                    FontFamily = new FontFamily("Consolas, Monaco, 'Courier New', monospace"),
                    Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                    Padding = new Thickness(6, 2, 6, 2),
                    Margin = new Thickness(0, 0, 10, 0),
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 11,
                    FontWeight = FontWeight.Bold
                };

                // Description
                var descText = new TextBlock
                {
                    Text = description,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 12
                };

                shortcutPanel.Children.Add(keyText);
                shortcutPanel.Children.Add(descText);
                _mainPanel.Children.Add(shortcutPanel);
            }
        }
    }
}
