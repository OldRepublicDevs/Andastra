using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace HolocronToolset.Dialogs
{
    /// <summary>
    /// Dialog for navigating to a specific line number in the code editor.
    /// Matching standard IDE behavior (VS Code, Visual Studio, etc.).
    /// </summary>
    public partial class GoToLineDialog : Window
    {
        private TextBox _lineNumberEdit;
        private TextBlock _errorLabel;
        private Button _okButton;
        private Button _cancelButton;
        private int _currentLine;
        private int _totalLines;

        // Public parameterless constructor for XAML
        public GoToLineDialog() : this(1, 1)
        {
        }

        /// <summary>
        /// Creates a new Go to Line dialog.
        /// </summary>
        /// <param name="currentLine">The current line number (1-indexed).</param>
        /// <param name="totalLines">The total number of lines in the document.</param>
        public GoToLineDialog(int currentLine, int totalLines)
        {
            _currentLine = currentLine;
            _totalLines = totalLines;
            InitializeComponent();
            Title = "Go to Line";
            SetupUI();
        }

        private void InitializeComponent()
        {
            bool xamlLoaded = false;
            try
            {
                AvaloniaXamlLoader.Load(this);
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
            var mainPanel = new StackPanel { Margin = new Avalonia.Thickness(15), Spacing = 10 };

            // Instructions label
            var instructionsLabel = new TextBlock
            {
                Text = $"Enter a line number (1-{_totalLines}):",
                Margin = new Avalonia.Thickness(0, 0, 0, 5)
            };
            mainPanel.Children.Add(instructionsLabel);

            // Line number input
            var inputPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
            inputPanel.Children.Add(new TextBlock { Text = "Line number:", VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center });
            _lineNumberEdit = new TextBox
            {
                MinWidth = 150,
                Text = _currentLine.ToString(),
                Watermark = $"1-{_totalLines}"
            };
            _lineNumberEdit.TextChanged += (s, e) => ValidateInput();
            _lineNumberEdit.KeyDown += (s, e) =>
            {
                if (e.Key == Avalonia.Input.Key.Enter)
                {
                    if (IsValidInput())
                    {
                        Close();
                    }
                }
                else if (e.Key == Avalonia.Input.Key.Escape)
                {
                    Close();
                }
            };
            inputPanel.Children.Add(_lineNumberEdit);
            mainPanel.Children.Add(inputPanel);

            // Error label (initially hidden)
            _errorLabel = new TextBlock
            {
                Foreground = new SolidColorBrush(Colors.Red),
                IsVisible = false,
                Margin = new Avalonia.Thickness(0, 5, 0, 0)
            };
            mainPanel.Children.Add(_errorLabel);

            // Button panel
            var buttonPanel = new StackPanel
            {
                Orientation = Avalonia.Layout.Orientation.Horizontal,
                Spacing = 5,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right,
                Margin = new Avalonia.Thickness(0, 10, 0, 0)
            };
            _okButton = new Button { Content = "OK", MinWidth = 75 };
            _okButton.Click += (s, e) =>
            {
                if (IsValidInput())
                {
                    Close();
                }
            };
            _cancelButton = new Button { Content = "Cancel", MinWidth = 75 };
            _cancelButton.Click += (s, e) => Close();
            buttonPanel.Children.Add(_okButton);
            buttonPanel.Children.Add(_cancelButton);
            mainPanel.Children.Add(buttonPanel);

            Content = mainPanel;

            // Focus the line number input and select all text
            _lineNumberEdit.Focus();
            _lineNumberEdit.SelectAll();
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _lineNumberEdit = this.FindControl<TextBox>("lineNumberEdit");
            _errorLabel = this.FindControl<TextBlock>("errorLabel");
            _okButton = this.FindControl<Button>("okButton");
            _cancelButton = this.FindControl<Button>("cancelButton");

            if (_lineNumberEdit != null)
            {
                _lineNumberEdit.Text = _currentLine.ToString();
                _lineNumberEdit.Watermark = $"1-{_totalLines}";
                _lineNumberEdit.TextChanged += (s, e) => ValidateInput();
                _lineNumberEdit.KeyDown += (s, e) =>
                {
                    if (e.Key == Avalonia.Input.Key.Enter)
                    {
                        if (IsValidInput())
                        {
                            Close();
                        }
                    }
                    else if (e.Key == Avalonia.Input.Key.Escape)
                    {
                        Close();
                    }
                };
                _lineNumberEdit.Focus();
                _lineNumberEdit.SelectAll();
            }

            if (_okButton != null)
            {
                _okButton.Click += (s, e) =>
                {
                    if (IsValidInput())
                    {
                        Close();
                    }
                };
            }

            if (_cancelButton != null)
            {
                _cancelButton.Click += (s, e) => Close();
            }
        }

        private void ValidateInput()
        {
            if (_lineNumberEdit == null || _errorLabel == null || _okButton == null)
            {
                return;
            }

            string text = _lineNumberEdit.Text?.Trim() ?? "";
            bool isValid = IsValidInput();

            // Update error label visibility and text
            if (!isValid && !string.IsNullOrEmpty(text))
            {
                if (!int.TryParse(text, out int lineNumber))
                {
                    _errorLabel.Text = "Please enter a valid number.";
                }
                else if (lineNumber < 1)
                {
                    _errorLabel.Text = $"Line number must be at least 1.";
                }
                else if (lineNumber > _totalLines)
                {
                    _errorLabel.Text = $"Line number cannot exceed {_totalLines}.";
                }
                else
                {
                    _errorLabel.Text = "";
                }
                _errorLabel.IsVisible = !string.IsNullOrEmpty(_errorLabel.Text);
            }
            else
            {
                _errorLabel.IsVisible = false;
                _errorLabel.Text = "";
            }

            // Update OK button state
            _okButton.IsEnabled = isValid;
        }

        private bool IsValidInput()
        {
            if (_lineNumberEdit == null)
            {
                return false;
            }

            string text = _lineNumberEdit.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (!int.TryParse(text, out int lineNumber))
            {
                return false;
            }

            return lineNumber >= 1 && lineNumber <= _totalLines;
        }

        /// <summary>
        /// Gets the selected line number, or null if the dialog was cancelled.
        /// </summary>
        /// <returns>The line number (1-indexed) if valid, or null if cancelled or invalid.</returns>
        public int? GetLineNumber()
        {
            if (!IsValidInput())
            {
                return null;
            }

            string text = _lineNumberEdit?.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (int.TryParse(text, out int lineNumber))
            {
                return lineNumber;
            }

            return null;
        }
    }
}

