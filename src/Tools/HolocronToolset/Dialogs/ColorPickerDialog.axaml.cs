using System;
using System.Globalization;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace HolocronToolset.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/edit/color.py:23-37
    // Original: QColorDialog for color selection
    public partial class ColorPickerDialog : Window
    {
        private Slider _redSlider;
        private Slider _greenSlider;
        private Slider _blueSlider;
        private Slider _alphaSlider;
        private NumericUpDown _redSpin;
        private NumericUpDown _greenSpin;
        private NumericUpDown _blueSpin;
        private NumericUpDown _alphaSpin;
        private TextBox _hexEdit;
        private Border _previewBorder;
        private Border _currentColorBorder;
        private Border _newColorBorder;
        private Border _colorPreviewBorder;
        private Canvas _checkerboardCanvas;
        private Button _okButton;
        private Button _cancelButton;
        private StackPanel _alphaPanel;

        private bool _allowAlpha;
        private Color _initialColor;
        private Color _selectedColor;
        private bool _updatingFromSlider;
        private bool _updatingFromRgb;
        private bool _updatingFromHex;

        public bool DialogResult { get; private set; }

        // Public parameterless constructor for XAML
        public ColorPickerDialog() : this(null, Colors.White, false)
        {
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/widgets/edit/color.py:23-37
        // Original: def open_color_dialog(self): QColorDialog(init_qcolor)
        public ColorPickerDialog(Window parent, Color initialColor, bool allowAlpha)
        {
            InitializeComponent();
            _initialColor = initialColor;
            _selectedColor = initialColor;
            _allowAlpha = allowAlpha;
            
            if (parent != null)
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            }
            
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
            else
            {
                // Find controls from XAML
                _redSlider = this.FindControl<Slider>("redSlider");
                _greenSlider = this.FindControl<Slider>("greenSlider");
                _blueSlider = this.FindControl<Slider>("blueSlider");
                _alphaSlider = this.FindControl<Slider>("alphaSlider");
                _redSpin = this.FindControl<NumericUpDown>("redSpin");
                _greenSpin = this.FindControl<NumericUpDown>("greenSpin");
                _blueSpin = this.FindControl<NumericUpDown>("blueSpin");
                _alphaSpin = this.FindControl<NumericUpDown>("alphaSpin");
                _hexEdit = this.FindControl<TextBox>("hexEdit");
                _previewBorder = this.FindControl<Border>("previewBorder");
                _currentColorBorder = this.FindControl<Border>("currentColorBorder");
                _newColorBorder = this.FindControl<Border>("newColorBorder");
                _colorPreviewBorder = this.FindControl<Border>("colorPreviewBorder");
                _checkerboardCanvas = this.FindControl<Canvas>("checkerboardCanvas");
                _okButton = this.FindControl<Button>("okButton");
                _cancelButton = this.FindControl<Button>("cancelButton");
                _alphaPanel = this.FindControl<StackPanel>("alphaPanel");

                if (_redSlider != null)
                {
                    _redSlider.ValueChanged += RgbSlider_ValueChanged;
                }
                if (_greenSlider != null)
                {
                    _greenSlider.ValueChanged += RgbSlider_ValueChanged;
                }
                if (_blueSlider != null)
                {
                    _blueSlider.ValueChanged += RgbSlider_ValueChanged;
                }
                if (_alphaSlider != null)
                {
                    _alphaSlider.ValueChanged += AlphaSlider_ValueChanged;
                }
                if (_redSpin != null)
                {
                    _redSpin.ValueChanged += RgbSpin_ValueChanged;
                }
                if (_greenSpin != null)
                {
                    _greenSpin.ValueChanged += RgbSpin_ValueChanged;
                }
                if (_blueSpin != null)
                {
                    _blueSpin.ValueChanged += RgbSpin_ValueChanged;
                }
                if (_alphaSpin != null)
                {
                    _alphaSpin.ValueChanged += AlphaSpin_ValueChanged;
                }
                if (_hexEdit != null)
                {
                    _hexEdit.TextChanged += HexEdit_TextChanged;
                }
                if (_okButton != null)
                {
                    _okButton.Click += (s, e) =>
                    {
                        DialogResult = true;
                        Close(true);
                    };
                }
                if (_cancelButton != null)
                {
                    _cancelButton.Click += (s, e) =>
                    {
                        DialogResult = false;
                        Close(false);
                    };
                }
            }
        }

        private void SetupProgrammaticUI()
        {
            // Programmatic UI setup if XAML fails
            // This is a fallback - normally XAML will be used
        }

        private void SetupUI()
        {
            // Set initial color
            UpdateColor(_initialColor, updateSlider: true, updateRgb: true, updateHex: true, updatePreview: true);

            // Show/hide alpha channel based on allowAlpha
            if (_alphaPanel != null)
            {
                _alphaPanel.IsVisible = _allowAlpha;
            }

            // Set current color display
            if (_currentColorBorder != null)
            {
                _currentColorBorder.Background = new SolidColorBrush(_initialColor);
            }

            // Draw checkerboard pattern for transparency preview
            DrawCheckerboard();
        }

        private void DrawCheckerboard()
        {
            if (_checkerboardCanvas == null || _colorPreviewBorder == null)
            {
                return;
            }

            _checkerboardCanvas.Children.Clear();
            
            // Wait for layout to complete before drawing
            if (_colorPreviewBorder.Bounds.Width <= 0 || _colorPreviewBorder.Bounds.Height <= 0)
            {
                // Schedule drawing after layout
                _colorPreviewBorder.LayoutUpdated += (s, e) =>
                {
                    if (_colorPreviewBorder.Bounds.Width > 0 && _colorPreviewBorder.Bounds.Height > 0)
                    {
                        DrawCheckerboardInternal();
                    }
                };
                return;
            }

            DrawCheckerboardInternal();
        }

        private void DrawCheckerboardInternal()
        {
            if (_checkerboardCanvas == null || _colorPreviewBorder == null)
            {
                return;
            }

            _checkerboardCanvas.Children.Clear();
            double width = _colorPreviewBorder.Bounds.Width;
            double height = _colorPreviewBorder.Bounds.Height;
            
            if (width <= 0 || height <= 0)
            {
                return;
            }

            _checkerboardCanvas.Width = width;
            _checkerboardCanvas.Height = height;

            double tileSize = 10;
            bool isLight = true;

            for (double y = 0; y < height; y += tileSize)
            {
                for (double x = 0; x < width; x += tileSize)
                {
                    var rect = new Avalonia.Controls.Shapes.Rectangle
                    {
                        Width = tileSize,
                        Height = tileSize,
                        Fill = new SolidColorBrush(isLight ? Colors.LightGray : Colors.White)
                    };
                    Avalonia.Controls.Canvas.SetLeft(rect, x);
                    Avalonia.Controls.Canvas.SetTop(rect, y);
                    _checkerboardCanvas.Children.Add(rect);
                    isLight = !isLight;
                }
                isLight = !isLight; // Alternate row
            }
        }

        // Matching PyKotor implementation: dialog.selectedColor()
        public Color GetSelectedColor()
        {
            return _selectedColor;
        }

        private void RgbSlider_ValueChanged(object sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (_updatingFromSlider || _updatingFromRgb)
            {
                return;
            }

            _updatingFromSlider = true;
            try
            {
                byte r = (byte)(_redSlider?.Value ?? 0);
                byte g = (byte)(_greenSlider?.Value ?? 0);
                byte b = (byte)(_blueSlider?.Value ?? 0);
                byte a = _allowAlpha ? (byte)(_alphaSlider?.Value ?? 255) : (byte)255;

                Color newColor = _allowAlpha ? Color.FromArgb(a, r, g, b) : Color.FromRgb(r, g, b);
                UpdateColor(newColor, updateSlider: false, updateRgb: true, updateHex: true, updatePreview: true);
            }
            finally
            {
                _updatingFromSlider = false;
            }
        }

        private void AlphaSlider_ValueChanged(object sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (_updatingFromSlider || _updatingFromRgb || !_allowAlpha)
            {
                return;
            }

            _updatingFromSlider = true;
            try
            {
                byte r = (byte)(_redSlider?.Value ?? 0);
                byte g = (byte)(_greenSlider?.Value ?? 0);
                byte b = (byte)(_blueSlider?.Value ?? 0);
                byte a = (byte)(_alphaSlider?.Value ?? 255);

                Color newColor = Color.FromArgb(a, r, g, b);
                UpdateColor(newColor, updateSlider: false, updateRgb: true, updateHex: true, updatePreview: true);
            }
            finally
            {
                _updatingFromSlider = false;
            }
        }

        private void RgbSpin_ValueChanged(object sender, NumericUpDownValueChangedEventArgs e)
        {
            if (_updatingFromRgb || _updatingFromSlider)
            {
                return;
            }

            _updatingFromRgb = true;
            try
            {
                byte r = (byte)(_redSpin?.Value ?? 0);
                byte g = (byte)(_greenSpin?.Value ?? 0);
                byte b = (byte)(_blueSpin?.Value ?? 0);
                byte a = _allowAlpha ? (byte)(_alphaSpin?.Value ?? 255) : (byte)255;

                Color newColor = _allowAlpha ? Color.FromArgb(a, r, g, b) : Color.FromRgb(r, g, b);
                UpdateColor(newColor, updateSlider: true, updateRgb: false, updateHex: true, updatePreview: true);
            }
            finally
            {
                _updatingFromRgb = false;
            }
        }

        private void AlphaSpin_ValueChanged(object sender, NumericUpDownValueChangedEventArgs e)
        {
            if (_updatingFromRgb || _updatingFromSlider || !_allowAlpha)
            {
                return;
            }

            _updatingFromRgb = true;
            try
            {
                byte r = (byte)(_redSpin?.Value ?? 0);
                byte g = (byte)(_greenSpin?.Value ?? 0);
                byte b = (byte)(_blueSpin?.Value ?? 0);
                byte a = (byte)(_alphaSpin?.Value ?? 255);

                Color newColor = Color.FromArgb(a, r, g, b);
                UpdateColor(newColor, updateSlider: true, updateRgb: false, updateHex: true, updatePreview: true);
            }
            finally
            {
                _updatingFromRgb = false;
            }
        }

        private void HexEdit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_updatingFromHex || _updatingFromSlider || _updatingFromRgb)
            {
                return;
            }

            string hexText = _hexEdit?.Text ?? "";
            if (string.IsNullOrEmpty(hexText))
            {
                return;
            }

            // Remove # if present
            hexText = hexText.TrimStart('#');

            // Try to parse hex color
            try
            {
                Color? parsedColor = null;
                if (hexText.Length == 6)
                {
                    // RGB format
                    uint rgb = uint.Parse(hexText, NumberStyles.HexNumber);
                    byte r = (byte)((rgb >> 16) & 0xFF);
                    byte g = (byte)((rgb >> 8) & 0xFF);
                    byte b = (byte)(rgb & 0xFF);
                    parsedColor = Color.FromRgb(r, g, b);
                }
                else if (hexText.Length == 8 && _allowAlpha)
                {
                    // RGBA format
                    uint rgba = uint.Parse(hexText, NumberStyles.HexNumber);
                    byte a = (byte)((rgba >> 24) & 0xFF);
                    byte r = (byte)((rgba >> 16) & 0xFF);
                    byte g = (byte)((rgba >> 8) & 0xFF);
                    byte b = (byte)(rgba & 0xFF);
                    parsedColor = Color.FromArgb(a, r, g, b);
                }

                if (parsedColor.HasValue)
                {
                    _updatingFromHex = true;
                    try
                    {
                        UpdateColor(parsedColor.Value, updateSlider: true, updateRgb: true, updateHex: false, updatePreview: true);
                    }
                    finally
                    {
                        _updatingFromHex = false;
                    }
                }
            }
            catch
            {
                // Invalid hex format - ignore
            }
        }

        private void UpdateColor(Color color, bool updateSlider, bool updateRgb, bool updateHex, bool updatePreview)
        {
            _selectedColor = color;

            if (updateSlider)
            {
                if (_redSlider != null)
                {
                    _redSlider.Value = color.R;
                }
                if (_greenSlider != null)
                {
                    _greenSlider.Value = color.G;
                }
                if (_blueSlider != null)
                {
                    _blueSlider.Value = color.B;
                }
                if (_alphaSlider != null && _allowAlpha)
                {
                    _alphaSlider.Value = color.A;
                }
            }

            if (updateRgb)
            {
                if (_redSpin != null)
                {
                    _redSpin.Value = color.R;
                }
                if (_greenSpin != null)
                {
                    _greenSpin.Value = color.G;
                }
                if (_blueSpin != null)
                {
                    _blueSpin.Value = color.B;
                }
                if (_alphaSpin != null && _allowAlpha)
                {
                    _alphaSpin.Value = color.A;
                }
            }

            if (updateHex && _hexEdit != null)
            {
                string hex = _allowAlpha
                    ? $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}"
                    : $"#{color.R:X2}{color.G:X2}{color.B:X2}";
                _hexEdit.Text = hex;
            }

            if (updatePreview)
            {
                if (_previewBorder != null)
                {
                    _previewBorder.Background = new SolidColorBrush(color);
                }
                if (_newColorBorder != null)
                {
                    _newColorBorder.Background = new SolidColorBrush(color);
                }
            }
        }

        /// <summary>
        /// Shows the dialog modally and returns true if the user clicked OK, false if Cancel was clicked or the dialog was closed.
        /// This is a blocking synchronous method that matches PyKotor's QColorDialog.exec() behavior.
        /// </summary>
        /// <param name="parent">The parent window for the dialog. If null, the dialog will be shown without a parent.</param>
        /// <returns>True if OK was clicked, false if Cancel was clicked or the dialog was closed.</returns>
        public new bool ShowDialog(Window parent = null)
        {
            Task<bool> dialogTask = ShowDialogAsync(parent);
            return dialogTask.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Shows the dialog modally asynchronously and returns a Task that completes with true if the user clicked OK, false if Cancel was clicked or the dialog was closed.
        /// </summary>
        /// <param name="parent">The parent window for the dialog. If null, the dialog will be shown without a parent.</param>
        /// <returns>A Task that completes with true if OK was clicked, false if Cancel was clicked or the dialog was closed.</returns>
        public async Task<bool> ShowDialogAsync(Window parent = null)
        {
            if (parent != null)
            {
                bool result = await ShowDialogAsync<bool>(parent);
                DialogResult = result;
                return result;
            }
            else
            {
                Window mainWindow = null;
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    mainWindow = desktop.MainWindow;
                }

                if (mainWindow != null)
                {
                    bool result = await ShowDialogAsync<bool>(mainWindow);
                    DialogResult = result;
                    return result;
                }
                else
                {
                    bool result = false;
                    EventHandler<WindowEventArgs> closedHandler = null;
                    closedHandler = (s, e) =>
                    {
                        this.Closed -= closedHandler;
                        result = DialogResult;
                    };
                    this.Closed += closedHandler;
                    Show();
                    while (this.IsVisible)
                    {
                        await Task.Delay(10);
                    }
                    return result;
                }
            }
        }
    }
}

