using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace HolocronToolset.Widgets
{
    /// <summary>
    /// Model browser widget for displaying and selecting imported MDL models.
    /// Similar to TextureBrowser but for 3D models.
    /// </summary>
    /// <remarks>
    /// Matching PyKotor implementation concept at Tools/HolocronToolset/src/toolset/gui/widgets/renderer/texture_browser.py
    /// This widget provides a visual browser for imported models in the LYT editor.
    /// </remarks>
    public class ModelBrowser : UserControl
    {
        private ListBox _modelList;
        private Dictionary<string, string> _models; // Maps model name to file path
        private string _selectedModel;

        /// <summary>
        /// Event fired when a model is selected.
        /// </summary>
        public event EventHandler<string> ModelSelected;

        /// <summary>
        /// Event fired when the selected model changes.
        /// </summary>
        public event EventHandler<string> ModelChanged;

        /// <summary>
        /// Gets the currently selected model name, or null if none selected.
        /// </summary>
        public string SelectedModel
        {
            get { return _selectedModel; }
            private set
            {
                if (_selectedModel != value)
                {
                    _selectedModel = value;
                    ModelChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// Gets all available model names.
        /// </summary>
        public List<string> GetModels()
        {
            return new List<string>(_models.Keys);
        }

        /// <summary>
        /// Gets the file path for a model by name.
        /// </summary>
        public string GetModelPath(string modelName)
        {
            return _models.TryGetValue(modelName, out string path) ? path : null;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ModelBrowser()
        {
            _models = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Create main container
            var mainPanel = new StackPanel
            {
                Orientation = Orientation.Vertical,
                Spacing = 4,
                Margin = new Thickness(8)
            };

            // Create header label
            var headerLabel = new TextBlock
            {
                Text = "Imported Models",
                FontSize = 14,
                FontWeight = FontWeight.Bold,
                Margin = new Thickness(0, 0, 0, 8)
            };
            mainPanel.Children.Add(headerLabel);

            // Create model list
            _modelList = new ListBox
            {
                SelectionMode = SelectionMode.Single,
                MinHeight = 200,
                MaxHeight = 400
            };
            _modelList.SelectionChanged += OnModelSelectionChanged;
            _modelList.DoubleTapped += OnModelDoubleTapped;

            // Set item template for model list items
            _modelList.ItemTemplate = new FuncDataTemplate<object>((item, scope) =>
            {
                if (item is string modelName)
                {
                    var panel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 8,
                        Margin = new Thickness(4)
                    };

                    // Model icon (placeholder - could be enhanced with actual model preview)
                    var iconBorder = new Border
                    {
                        Width = 48,
                        Height = 48,
                        Background = new SolidColorBrush(Color.FromRgb(60, 60, 60)),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
                        BorderThickness = new Thickness(1),
                        CornerRadius = new CornerRadius(4),
                        Margin = new Thickness(0, 0, 8, 0)
                    };

                    var iconText = new TextBlock
                    {
                        Text = "MDL",
                        FontSize = 10,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Foreground = new SolidColorBrush(Color.FromRgb(200, 200, 200))
                    };
                    iconBorder.Child = iconText;

                    // Model name
                    var nameText = new TextBlock
                    {
                        Text = modelName,
                        VerticalAlignment = VerticalAlignment.Center,
                        FontSize = 12
                    };

                    // File path (if available)
                    string modelPath = GetModelPath(modelName);
                    if (!string.IsNullOrEmpty(modelPath))
                    {
                        var pathText = new TextBlock
                        {
                            Text = Path.GetFileName(modelPath),
                            VerticalAlignment = VerticalAlignment.Center,
                            FontSize = 10,
                            Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150)),
                            Margin = new Thickness(8, 0, 0, 0)
                        };
                        panel.Children.Add(iconBorder);
                        panel.Children.Add(nameText);
                        panel.Children.Add(pathText);
                    }
                    else
                    {
                        panel.Children.Add(iconBorder);
                        panel.Children.Add(nameText);
                    }

                    return panel;
                }
                return new TextBlock { Text = item?.ToString() ?? "" };
            });

            mainPanel.Children.Add(_modelList);

            // Create status label
            var statusLabel = new TextBlock
            {
                Name = "statusLabel",
                Text = "No models imported",
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.FromRgb(150, 150, 150)),
                Margin = new Thickness(0, 4, 0, 0)
            };
            mainPanel.Children.Add(statusLabel);

            Content = mainPanel;
        }

        /// <summary>
        /// Updates the model browser with the provided models.
        /// </summary>
        /// <param name="models">Dictionary mapping model names to file paths.</param>
        public void UpdateModels(Dictionary<string, string> models)
        {
            if (models == null)
            {
                _models.Clear();
            }
            else
            {
                _models = new Dictionary<string, string>(models, StringComparer.OrdinalIgnoreCase);
            }

            RefreshModelList();
        }

        /// <summary>
        /// Adds or updates a model in the browser.
        /// </summary>
        /// <param name="modelName">The model name (ResRef).</param>
        /// <param name="filePath">The file path to the model.</param>
        public void AddModel(string modelName, string filePath)
        {
            if (string.IsNullOrEmpty(modelName))
            {
                return;
            }

            _models[modelName] = filePath ?? "";
            RefreshModelList();
        }

        /// <summary>
        /// Removes a model from the browser.
        /// </summary>
        /// <param name="modelName">The model name to remove.</param>
        public void RemoveModel(string modelName)
        {
            if (string.IsNullOrEmpty(modelName))
            {
                return;
            }

            if (_models.Remove(modelName))
            {
                RefreshModelList();
                if (SelectedModel == modelName)
                {
                    SelectedModel = null;
                }
            }
        }

        /// <summary>
        /// Clears all models from the browser.
        /// </summary>
        public void ClearModels()
        {
            _models.Clear();
            SelectedModel = null;
            RefreshModelList();
        }

        /// <summary>
        /// Highlights/selects a specific model in the browser.
        /// </summary>
        /// <param name="modelName">The model name to highlight.</param>
        public void HighlightModel(string modelName)
        {
            if (string.IsNullOrEmpty(modelName))
            {
                return;
            }

            if (_modelList != null && _models.ContainsKey(modelName))
            {
                var items = _modelList.Items?.Cast<string>().ToList();
                if (items != null)
                {
                    int index = items.IndexOf(modelName);
                    if (index >= 0)
                    {
                        _modelList.SelectedIndex = index;
                        _modelList.ScrollIntoView(index);
                    }
                }
            }
        }

        /// <summary>
        /// Refreshes the model list display.
        /// </summary>
        private void RefreshModelList()
        {
            if (_modelList == null)
            {
                return;
            }

            var modelNames = new List<string>(_models.Keys);
            modelNames.Sort(StringComparer.OrdinalIgnoreCase);

            _modelList.ItemsSource = modelNames;

            // Update status label
            var statusLabel = this.FindControl<TextBlock>("statusLabel");
            if (statusLabel != null)
            {
                if (modelNames.Count == 0)
                {
                    statusLabel.Text = "No models imported";
                }
                else if (modelNames.Count == 1)
                {
                    statusLabel.Text = "1 model available";
                }
                else
                {
                    statusLabel.Text = $"{modelNames.Count} models available";
                }
            }
        }

        /// <summary>
        /// Handles model selection changes.
        /// </summary>
        private void OnModelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_modelList?.SelectedItem is string modelName)
            {
                SelectedModel = modelName;
                ModelSelected?.Invoke(this, modelName);
            }
            else
            {
                SelectedModel = null;
            }
        }

        /// <summary>
        /// Handles model double-tap (for potential actions like preview or use).
        /// </summary>
        private void OnModelDoubleTapped(object sender, TappedEventArgs e)
        {
            if (_modelList?.SelectedItem is string modelName)
            {
                // Double-tap could trigger model preview or usage
                // TODO: STUB - For now, just ensure it's selected
                SelectedModel = modelName;
                ModelSelected?.Invoke(this, modelName);
            }
        }
    }
}

