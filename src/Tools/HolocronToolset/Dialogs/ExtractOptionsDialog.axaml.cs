using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace HolocronToolset.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:8
    // Original: class ExtractOptionsDialog(QDialog):
    public partial class ExtractOptionsDialog : Window
    {
        private bool _tpcDecompile;
        private bool _tpcExtractTxi;
        private bool _mdlDecompile;
        private bool _mdlExtractTextures;

        // Public parameterless constructor for XAML
        public ExtractOptionsDialog() : this(null)
        {
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:11-40
        // Original: def __init__(self, parent=None):
        public ExtractOptionsDialog(Window parent)
        {
            InitializeComponent();
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
            Title = "Extract Options";
            Width = 400;
            Height = 300;

            // Create all UI controls programmatically for test scenarios
            _tpcDecompileCheckbox = new CheckBox { Content = "TPC Decompile" };
            _tpcTxiCheckbox = new CheckBox { Content = "TPC Extract TXI" };
            _mdlDecompileCheckbox = new CheckBox { Content = "MDL Decompile" };
            _mdlTexturesCheckbox = new CheckBox { Content = "MDL Extract Textures" };
            _okButton = new Button { Content = "OK" };
            _cancelButton = new Button { Content = "Cancel" };

            // Connect events
            _okButton.Click += (s, e) => { UpdateValues(); Close(); };
            _cancelButton.Click += (s, e) => Close();
            _tpcDecompileCheckbox.IsCheckedChanged += (s, e) => _tpcDecompile = _tpcDecompileCheckbox.IsChecked ?? false;
            _tpcTxiCheckbox.IsCheckedChanged += (s, e) => _tpcExtractTxi = _tpcTxiCheckbox.IsChecked ?? false;
            _mdlDecompileCheckbox.IsCheckedChanged += (s, e) => _mdlDecompile = _mdlDecompileCheckbox.IsChecked ?? false;
            _mdlTexturesCheckbox.IsCheckedChanged += (s, e) => _mdlExtractTextures = _mdlTexturesCheckbox.IsChecked ?? false;

            // Create UI wrapper for testing
            Ui = new ExtractOptionsDialogUi
            {
                TpcDecompileCheckbox = _tpcDecompileCheckbox,
                TpcTxiCheckbox = _tpcTxiCheckbox,
                MdlDecompileCheckbox = _mdlDecompileCheckbox,
                MdlTexturesCheckbox = _mdlTexturesCheckbox
            };

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Extract Options",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            panel.Children.Add(titleLabel);
            panel.Children.Add(_tpcDecompileCheckbox);
            panel.Children.Add(_tpcTxiCheckbox);
            panel.Children.Add(_mdlDecompileCheckbox);
            panel.Children.Add(_mdlTexturesCheckbox);
            var buttonPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
            buttonPanel.Children.Add(_okButton);
            buttonPanel.Children.Add(_cancelButton);
            panel.Children.Add(buttonPanel);
            Content = panel;
        }

        private CheckBox _tpcDecompileCheckbox;
        private CheckBox _tpcTxiCheckbox;
        private CheckBox _mdlDecompileCheckbox;
        private CheckBox _mdlTexturesCheckbox;
        private Button _okButton;
        private Button _cancelButton;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:14-15
        // Original: self.ui = Ui_ExtractOptionsDialog()
        // Expose UI widgets for testing
        public ExtractOptionsDialogUi Ui { get; private set; }

        private void SetupUI()
        {
            // If Ui is already initialized (e.g., by SetupProgrammaticUI), skip control finding
            if (Ui != null)
            {
                return;
            }

            // Use try-catch to handle cases where XAML controls might not be available (e.g., in tests)
            Ui = new ExtractOptionsDialogUi();
            
            try
            {
                // Find controls from XAML
                _tpcDecompileCheckbox = this.FindControl<CheckBox>("tpcDecompileCheckbox");
                _tpcTxiCheckbox = this.FindControl<CheckBox>("tpcTxiCheckbox");
                _mdlDecompileCheckbox = this.FindControl<CheckBox>("mdlDecompileCheckbox");
                _mdlTexturesCheckbox = this.FindControl<CheckBox>("mdlTexturesCheckbox");
                _okButton = this.FindControl<Button>("okButton");
                _cancelButton = this.FindControl<Button>("cancelButton");
            }
            catch
            {
                // XAML controls not available - create programmatic UI for tests
                SetupProgrammaticUI();
                return; // SetupProgrammaticUI already sets up Ui and connects events
            }

            // Create UI wrapper for testing
            Ui.TpcDecompileCheckbox = _tpcDecompileCheckbox;
            Ui.TpcTxiCheckbox = _tpcTxiCheckbox;
            Ui.MdlDecompileCheckbox = _mdlDecompileCheckbox;
            Ui.MdlTexturesCheckbox = _mdlTexturesCheckbox;

            if (_okButton != null)
            {
                _okButton.Click += (s, e) => { UpdateValues(); Close(); };
            }
            if (_cancelButton != null)
            {
                _cancelButton.Click += (s, e) => Close();
            }

            // Sync checkbox states with properties
            if (_tpcDecompileCheckbox != null)
            {
                _tpcDecompileCheckbox.IsCheckedChanged += (s, e) => _tpcDecompile = _tpcDecompileCheckbox.IsChecked ?? false;
            }
            if (_tpcTxiCheckbox != null)
            {
                _tpcTxiCheckbox.IsCheckedChanged += (s, e) => _tpcExtractTxi = _tpcTxiCheckbox.IsChecked ?? false;
            }
            if (_mdlDecompileCheckbox != null)
            {
                _mdlDecompileCheckbox.IsCheckedChanged += (s, e) => _mdlDecompile = _mdlDecompileCheckbox.IsChecked ?? false;
            }
            if (_mdlTexturesCheckbox != null)
            {
                _mdlTexturesCheckbox.IsCheckedChanged += (s, e) => _mdlExtractTextures = _mdlTexturesCheckbox.IsChecked ?? false;
            }
        }

        private void UpdateValues()
        {
            if (_tpcDecompileCheckbox != null)
            {
                _tpcDecompile = _tpcDecompileCheckbox.IsChecked ?? false;
            }
            if (_tpcTxiCheckbox != null)
            {
                _tpcExtractTxi = _tpcTxiCheckbox.IsChecked ?? false;
            }
            if (_mdlDecompileCheckbox != null)
            {
                _mdlDecompile = _mdlDecompileCheckbox.IsChecked ?? false;
            }
            if (_mdlTexturesCheckbox != null)
            {
                _mdlExtractTextures = _mdlTexturesCheckbox.IsChecked ?? false;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:22-25
        // Original: @property def tpc_decompile(self) -> bool: return self.ui.tpcDecompileCheckbox.isChecked()
        // Note: Using snake_case property names to match Python API for test compatibility
        public bool tpc_decompile
        {
            get => _tpcDecompileCheckbox?.IsChecked ?? false;
            set
            {
                _tpcDecompile = value;
                if (_tpcDecompileCheckbox != null)
                {
                    _tpcDecompileCheckbox.IsChecked = value;
                }
            }
        }

        // C# PascalCase property for normal usage
        public bool TpcDecompile
        {
            get => tpc_decompile;
            set => tpc_decompile = value;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:27-30
        // Original: @property def tpc_extract_txi(self) -> bool: return self.ui.tpcTxiCheckbox.isChecked()
        public bool tpc_extract_txi
        {
            get => _tpcTxiCheckbox?.IsChecked ?? false;
            set
            {
                _tpcExtractTxi = value;
                if (_tpcTxiCheckbox != null)
                {
                    _tpcTxiCheckbox.IsChecked = value;
                }
            }
        }

        // C# PascalCase property for normal usage
        public bool TpcExtractTxi
        {
            get => tpc_extract_txi;
            set => tpc_extract_txi = value;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:32-35
        // Original: @property def mdl_decompile(self) -> bool: return self.ui.mdlDecompileCheckbox.isChecked()
        public bool mdl_decompile
        {
            get => _mdlDecompileCheckbox?.IsChecked ?? false;
            set
            {
                _mdlDecompile = value;
                if (_mdlDecompileCheckbox != null)
                {
                    _mdlDecompileCheckbox.IsChecked = value;
                }
            }
        }

        // C# PascalCase property for normal usage
        public bool MdlDecompile
        {
            get => mdl_decompile;
            set => mdl_decompile = value;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:37-40
        // Original: @property def mdl_extract_textures(self) -> bool: return self.ui.mdlTexturesCheckbox.isChecked()
        public bool mdl_extract_textures
        {
            get => _mdlTexturesCheckbox?.IsChecked ?? false;
            set
            {
                _mdlExtractTextures = value;
                if (_mdlTexturesCheckbox != null)
                {
                    _mdlTexturesCheckbox.IsChecked = value;
                }
            }
        }

        // C# PascalCase property for normal usage
        public bool MdlExtractTextures
        {
            get => mdl_extract_textures;
            set => mdl_extract_textures = value;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/extract_options.py:14-15
        // Original: self.ui = Ui_ExtractOptionsDialog()
        // UI wrapper class for testing access
        public class ExtractOptionsDialogUi
        {
            public CheckBox TpcDecompileCheckbox { get; set; }
            public CheckBox TpcTxiCheckbox { get; set; }
            public CheckBox MdlDecompileCheckbox { get; set; }
            public CheckBox MdlTexturesCheckbox { get; set; }
        }
    }
}
