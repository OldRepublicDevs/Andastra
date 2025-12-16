using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using HolocronToolset.Data;

namespace HolocronToolset.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:35
    // Original: class TSLPatchDataEditor(QDialog):
    public partial class TSLPatchDataEditorDialog : Window
    {
        private HTInstallation _installation;
        private string _tslpatchdataPath;
        private TextBox _pathEdit;
        private TreeView _fileTree;
        private TabControl _configTabs;
        private Button _generateButton;
        private Button _previewButton;
        private Button _saveButton;

        // Public parameterless constructor for XAML
        public TSLPatchDataEditorDialog() : this(null, null, null)
        {
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:38-61
        // Original: def __init__(self, parent, installation=None, tslpatchdata_path=None):
        public TSLPatchDataEditorDialog(Window parent, HTInstallation installation, string tslpatchdataPath = null)
        {
            InitializeComponent();
            Title = "TSLPatchData Editor - Create HoloPatcher Mod";
            Width = 1400;
            Height = 900;
            _installation = installation;
            _tslpatchdataPath = tslpatchdataPath ?? "tslpatchdata";
            SetupUI();
            if (!string.IsNullOrEmpty(_tslpatchdataPath) && Directory.Exists(_tslpatchdataPath))
            {
                LoadExistingConfig();
            }
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
            var mainPanel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };

            var headerPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5 };
            headerPanel.Children.Add(new TextBlock { Text = "TSLPatchData Folder:", FontWeight = Avalonia.Media.FontWeight.Bold });
            _pathEdit = new TextBox { Text = _tslpatchdataPath, MinWidth = 300 };
            var browseButton = new Button { Content = "Browse..." };
            browseButton.Click += (s, e) => BrowseTslpatchdataPath();
            var createButton = new Button { Content = "Create New" };
            createButton.Click += (s, e) => CreateNewTslpatchdata();
            headerPanel.Children.Add(_pathEdit);
            headerPanel.Children.Add(browseButton);
            headerPanel.Children.Add(createButton);
            mainPanel.Children.Add(headerPanel);

            var splitter = new Grid();
            splitter.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            splitter.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });

            var leftPanel = new StackPanel();
            leftPanel.Children.Add(new TextBlock { Text = "Files to Package:", FontWeight = Avalonia.Media.FontWeight.Bold });
            _fileTree = new TreeView();
            leftPanel.Children.Add(_fileTree);
            Grid.SetColumn(leftPanel, 0);
            splitter.Children.Add(leftPanel);

            _configTabs = new TabControl();
            CreateGeneralTab();
            Create2DAMemoryTab();
            CreateTLKStrRefTab();
            CreateGFFFieldsTab();
            CreateScriptsTab();
            CreateINIPreviewTab();
            Grid.SetColumn(_configTabs, 1);
            splitter.Children.Add(_configTabs);

            mainPanel.Children.Add(splitter);

            var buttonPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 5, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
            _generateButton = new Button { Content = "Generate TSLPatchData" };
            _generateButton.Click += (s, e) => GenerateTslpatchdata();
            _previewButton = new Button { Content = "Preview INI" };
            _previewButton.Click += (s, e) => PreviewIni();
            _saveButton = new Button { Content = "Save Configuration" };
            _saveButton.Click += (s, e) => SaveConfiguration();
            buttonPanel.Children.Add(_generateButton);
            buttonPanel.Children.Add(_previewButton);
            buttonPanel.Children.Add(_saveButton);
            mainPanel.Children.Add(buttonPanel);

            Content = mainPanel;
        }

        private void SetupUI()
        {
            // Find controls from XAML
            _pathEdit = this.FindControl<TextBox>("pathEdit");
            _fileTree = this.FindControl<TreeView>("fileTree");
            _configTabs = this.FindControl<TabControl>("configTabs");
            _generateButton = this.FindControl<Button>("generateButton");
            _previewButton = this.FindControl<Button>("previewButton");
            _saveButton = this.FindControl<Button>("saveButton");
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:63-79
        // Original: def _setup_ui(self):
        private void CreateGeneralTab()
        {
            var tab = new TabItem { Header = "General Settings" };
            var content = new StackPanel();
            // TODO: Add general settings controls
            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        private void Create2DAMemoryTab()
        {
            var tab = new TabItem { Header = "2DA Memory" };
            var content = new StackPanel();
            // TODO: Add 2DA memory controls
            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        private void CreateTLKStrRefTab()
        {
            var tab = new TabItem { Header = "TLK StrRef" };
            var content = new StackPanel();
            // TODO: Add TLK StrRef controls
            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        private void CreateGFFFieldsTab()
        {
            var tab = new TabItem { Header = "GFF Fields" };
            var content = new StackPanel();
            // TODO: Add GFF fields controls
            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        private void CreateScriptsTab()
        {
            var tab = new TabItem { Header = "Scripts" };
            var content = new StackPanel();
            // TODO: Add scripts controls
            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        private void CreateINIPreviewTab()
        {
            var tab = new TabItem { Header = "INI Preview" };
            var content = new StackPanel();
            // TODO: Add INI preview controls
            tab.Content = content;
            if (_configTabs != null)
            {
                _configTabs.Items.Add(tab);
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/tslpatchdata_editor.py:150-459
        // Original: Various methods for TSLPatchData generation
        private void BrowseTslpatchdataPath()
        {
            // TODO: Implement folder browser dialog when available
            System.Console.WriteLine("Browse TSLPatchData path not yet implemented");
        }

        private void CreateNewTslpatchdata()
        {
            // TODO: Create new TSLPatchData folder structure
            System.Console.WriteLine("Create new TSLPatchData not yet implemented");
        }

        private void LoadExistingConfig()
        {
            // TODO: Load existing TSLPatchData configuration
            System.Console.WriteLine($"Loading existing config from {_tslpatchdataPath}");
        }

        private void GenerateTslpatchdata()
        {
            // TODO: Generate TSLPatchData files
            System.Console.WriteLine("Generate TSLPatchData not yet implemented");
        }

        private void PreviewIni()
        {
            // TODO: Preview generated INI file
            System.Console.WriteLine("Preview INI not yet implemented");
        }

        private void SaveConfiguration()
        {
            // TODO: Save configuration
            System.Console.WriteLine("Save configuration not yet implemented");
        }
    }
}
