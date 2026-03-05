using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OdyTools.Data;
using OdyTools.Widgets.Settings;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using OdyTools.Utils;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Dialogs
{
    public partial class SettingsDialog : Window
    {
        private bool _isResetting;
        private bool _installationEdited;
        private GlobalSettings _settings;
        private Control _installationsWidget;
        private Control _dlgSettingsWidget;
        private Control _miscWidget;
        private Control _gitEditorWidget;
        private Control _moduleDesignerWidget;
        private Control _applicationSettingsWidget;

        public SettingsDialogUi Ui { get; private set; }

        public bool IsResetting => _isResetting;

        public bool InstallationEdited => _installationEdited;

        // Dialog result property (true if OK clicked, false if Cancel or closed)
        public bool? Result { get; private set; }

        public SettingsDialog() : this(null) { }
        public SettingsDialog(Window parent = null)
        {
            InitializeComponent();
            _isResetting = false;
            _installationEdited = false;
            _settings = new GlobalSettings();
            if (parent != null)
            {
                Owner = parent;
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
        }

        private void SetupProgrammaticUI()
        {
            Title = "Settings";
            Width = 600;
            Height = 500;

            // Create programmatic UI matching XAML structure
            var mainGrid = new Grid();
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            // Create splitter grid
            var splitGrid = new Grid();
            splitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
            splitGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Grid.SetRow(splitGrid, 0);
            mainGrid.Children.Add(splitGrid);

            // Create settings tree
            var settingsTree = new TreeView();
            settingsTree.Items.Add(new TreeViewItem { Header = "Installations", IsSelected = true });
            settingsTree.Items.Add(new TreeViewItem { Header = "DLG Settings" });
            settingsTree.Items.Add(new TreeViewItem { Header = "GIT Editor" });
            settingsTree.Items.Add(new TreeViewItem { Header = "Module Designer" });
            settingsTree.Items.Add(new TreeViewItem { Header = "Misc" });
            settingsTree.Items.Add(new TreeViewItem { Header = "Application" });
            Grid.SetColumn(settingsTree, 0);
            splitGrid.Children.Add(settingsTree);

            // Create settings stack
            var settingsStack = new ContentControl();
            Grid.SetColumn(settingsStack, 1);
            splitGrid.Children.Add(settingsStack);

            // Create button grid
            var buttonGrid = new Grid();
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            buttonGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Grid.SetRow(buttonGrid, 1);
            mainGrid.Children.Add(buttonGrid);

            var okButton = new Button { Content = "OK", Width = 75, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right, Margin = new Avalonia.Thickness(0, 0, 5, 0) };
            Grid.SetColumn(okButton, 0);
            buttonGrid.Children.Add(okButton);

            var cancelButton = new Button { Content = "Cancel", Width = 75, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left };
            Grid.SetColumn(cancelButton, 1);
            buttonGrid.Children.Add(cancelButton);

            // Create actual settings widgets (matching PyKotor implementation). Defensive creation.
            _installationsWidget = CreateWidgetSafe(() => new InstallationsWidget(), nameof(InstallationsWidget))
                ?? new ContentControl { Content = new TextBlock { Text = "Installations (failed to load)" } };
            _dlgSettingsWidget = CreateWidgetSafe(() => new DLGSettingsWidget(), nameof(DLGSettingsWidget))
                ?? new ContentControl { Content = new TextBlock { Text = "DLG Settings (failed to load)" } };
            _miscWidget = CreateWidgetSafe(() => new MiscSettingsWidget(), nameof(MiscSettingsWidget))
                ?? new ContentControl { Content = new TextBlock { Text = "Misc (failed to load)" } };
            _gitEditorWidget = CreateWidgetSafe(() => new GITSettingsWidget(), nameof(GITSettingsWidget))
                ?? new ContentControl { Content = new TextBlock { Text = "GIT Editor (failed to load)" } };
            _moduleDesignerWidget = CreateWidgetSafe(() => new ModuleDesignerSettingsWidget(), nameof(ModuleDesignerSettingsWidget))
                ?? new ContentControl { Content = new TextBlock { Text = "Module Designer (failed to load)" } };
            _applicationSettingsWidget = CreateWidgetSafe(() => new ApplicationSettingsWidget(), nameof(ApplicationSettingsWidget))
                ?? new ContentControl { Content = new TextBlock { Text = "Application (failed to load)" } };

            if (_installationsWidget is InstallationsWidget iwProg)
            {
                iwProg.SettingsEdited += OnInstallationEdited;
            }

            Ui = new SettingsDialogUi
            {
                SettingsTree = settingsTree,
                SettingsStack = settingsStack,
                InstallationsPage = _installationsWidget,
                DLGSettingsPage = _dlgSettingsWidget,
                GitEditorPage = _gitEditorWidget,
                MiscPage = _miscWidget,
                ModuleDesignerPage = _moduleDesignerWidget,
                ApplicationSettingsPage = _applicationSettingsWidget,
                OkButton = okButton,
                CancelButton = cancelButton
            };

            // Set up signals
            okButton.Click += (s, e) => Accept();
            cancelButton.Click += (s, e) => Close(false);

            var pageDict = new Dictionary<string, Control>
            {
                { "Installations", Ui.InstallationsPage },
                { "DLG Settings", Ui.DLGSettingsPage },
                { "GIT Editor", Ui.GitEditorPage },
                { "Misc", Ui.MiscPage },
                { "Module Designer", Ui.ModuleDesignerPage },
                { "Application", Ui.ApplicationSettingsPage }
            };

            settingsTree.SelectionChanged += (s, e) => OnSettingsTreeSelectionChanged(pageDict);

            // Set initial page
            settingsStack.Content = _installationsWidget;

            Content = mainGrid;
        }

        private void SetupUI()
        {
            // Find all controls from XAML and expose via Ui property
            TreeView settingsTree = null;
            ContentControl settingsStack = null;
            Button okButton = null;
            Button cancelButton = null;

            try
            {
                settingsTree = this.FindControl<TreeView>("settingsTree");
                settingsStack = this.FindControl<ContentControl>("settingsStack");
                okButton = this.FindControl<Button>("okButton");
                cancelButton = this.FindControl<Button>("cancelButton");
            }
            catch (InvalidOperationException)
            {
                // XAML not loaded or controls not found - create programmatic UI
                SetupProgrammaticUI();
                return;
            }

            // Set up close handler to set result to false if closed without Accept
            this.Closing += (s, e) =>
            {
                if (Result == null)
                {
                    Result = false;
                }
            };

            // Create actual settings widgets (matching PyKotor implementation). Defensive: one failing widget must not crash the dialog.
            _installationsWidget = CreateWidgetSafe(() => new InstallationsWidget(), nameof(InstallationsWidget));
            _dlgSettingsWidget = CreateWidgetSafe(() => new DLGSettingsWidget(), nameof(DLGSettingsWidget));
            _miscWidget = CreateWidgetSafe(() => new MiscSettingsWidget(), nameof(MiscSettingsWidget));
            _gitEditorWidget = CreateWidgetSafe(() => new GITSettingsWidget(), nameof(GITSettingsWidget));
            _moduleDesignerWidget = CreateWidgetSafe(() => new ModuleDesignerSettingsWidget(), nameof(ModuleDesignerSettingsWidget));
            _applicationSettingsWidget = CreateWidgetSafe(() => new ApplicationSettingsWidget(), nameof(ApplicationSettingsWidget));

            if (_installationsWidget == null)
            {
                _installationsWidget = new ContentControl { Content = new TextBlock { Text = "Installations (failed to load)" } };
            }
            if (_dlgSettingsWidget == null)
            {
                _dlgSettingsWidget = new ContentControl { Content = new TextBlock { Text = "DLG Settings (failed to load)" } };
            }
            if (_miscWidget == null)
            {
                _miscWidget = new ContentControl { Content = new TextBlock { Text = "Misc (failed to load)" } };
            }
            if (_gitEditorWidget == null)
            {
                _gitEditorWidget = new ContentControl { Content = new TextBlock { Text = "GIT Editor (failed to load)" } };
            }
            if (_moduleDesignerWidget == null)
            {
                _moduleDesignerWidget = new ContentControl { Content = new TextBlock { Text = "Module Designer (failed to load)" } };
            }
            if (_applicationSettingsWidget == null)
            {
                _applicationSettingsWidget = new ContentControl { Content = new TextBlock { Text = "Application (failed to load)" } };
            }

            if (_installationsWidget is InstallationsWidget iw)
            {
                iw.SettingsEdited += OnInstallationEdited;
            }

            Ui = new SettingsDialogUi
            {
                SettingsTree = settingsTree,
                SettingsStack = settingsStack,
                InstallationsPage = _installationsWidget,
                DLGSettingsPage = _dlgSettingsWidget,
                GitEditorPage = _gitEditorWidget,
                MiscPage = _miscWidget,
                ModuleDesignerPage = _moduleDesignerWidget,
                ApplicationSettingsPage = _applicationSettingsWidget,
                OkButton = okButton,
                CancelButton = cancelButton
            };

            var pageDict = new Dictionary<string, Control>
            {
                { "Installations", Ui.InstallationsPage },
                { "DLG Settings", Ui.DLGSettingsPage },
                { "GIT Editor", Ui.GitEditorPage },
                { "Misc", Ui.MiscPage },
                { "Module Designer", Ui.ModuleDesignerPage },
                { "Application", Ui.ApplicationSettingsPage }
            };

            if (Ui.OkButton != null)
            {
                Ui.OkButton.Click += (s, e) => Accept();
            }
            if (Ui.CancelButton != null)
            {
                Ui.CancelButton.Click += (s, e) => Close(false);
            }
            if (Ui.SettingsTree != null)
            {
                Ui.SettingsTree.SelectionChanged += (s, e) => OnSettingsTreeSelectionChanged(pageDict);
            }

            // Set initial page
            if (Ui.SettingsStack != null && Ui.InstallationsPage != null)
            {
                Ui.SettingsStack.Content = Ui.InstallationsPage;
            }
        }

        private void OnSettingsTreeSelectionChanged(Dictionary<string, Control> pageDict)
        {
            if (Ui?.SettingsTree?.SelectedItem is TreeViewItem item)
            {
                string pageName = item.Header?.ToString() ?? "";
                if (pageDict.ContainsKey(pageName) && Ui.SettingsStack != null)
                {
                    Ui.SettingsStack.Content = pageDict[pageName];
                }
            }
        }

        private void OnInstallationEdited(object sender, EventArgs e)
        {
            _installationEdited = true;
        }

        private static Control CreateWidgetSafe(Func<Control> factory, string name)
        {
            try
            {
                return factory();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SettingsDialog: Failed to create {name}: {ex.Message}");
                return null;
            }
        }

        public void Accept()
        {
            // Save settings
            if (!_isResetting)
            {
                if (_miscWidget is MiscSettingsWidget mw) mw.Save();
                if (_gitEditorWidget is GITSettingsWidget gw) gw.Save();
                if (_moduleDesignerWidget is ModuleDesignerSettingsWidget mdw) mdw.Save();
                if (_dlgSettingsWidget is DLGSettingsWidget dsw) dsw.Save();
                if (_installationsWidget is InstallationsWidget iw) iw.Save();
                // ApplicationSettingsWidget doesn't have Save() - settings are saved directly when changed
            }
            // Set result to true (accepted) to match PyKotor's dialog.exec() behavior
            Result = true;
            Close(true);
        }

        public async void OnResetAllSettings()
        {

            var confirmResult = await DialogHelper.ShowAsync("Reset All Settings", "Are you sure you want to reset all settings to their default values? This action cannot be undone.", ButtonEnum.YesNo, IconType.Question);

            if (confirmResult == ButtonResult.Yes)
            {
                _settings.Clear();

                await DialogHelper.ShowAsync("Settings Reset", "All settings have been cleared and reset to their default values.", ButtonEnum.Ok, IconType.Info);

                _isResetting = true;
                Close(true);
            }
        }
    }

    public class SettingsDialogUi
    {
        public TreeView SettingsTree { get; set; }
        public ContentControl SettingsStack { get; set; }
        public Control InstallationsPage { get; set; }
        public Control DLGSettingsPage { get; set; }
        public Control GitEditorPage { get; set; }
        public Control MiscPage { get; set; }
        public Control ModuleDesignerPage { get; set; }
        public Control ApplicationSettingsPage { get; set; }
        public Button OkButton { get; set; }
        public Button CancelButton { get; set; }
    }
}
