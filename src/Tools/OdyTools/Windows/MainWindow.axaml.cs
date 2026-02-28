using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Styling;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using BioWare.Extract;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.TPC;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Editors;
using OdyTools.Utils;
using OdyTools.Editors.DLG;
using OdyTools.Shell;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using FileResource = BioWare.Extract.FileResource;
using Control = Avalonia.Controls.Control;
using ResourceList = OdyTools.Widgets.ResourceList;
using MenuItem = Avalonia.Controls.MenuItem;
using TabItem = Avalonia.Controls.TabItem;
using TabControl = Avalonia.Controls.TabControl;
using Button = Avalonia.Controls.Button;
using GlobalSettings = OdyTools.Data.GlobalSettings;
#if !NET48
using UpdateManager = OdyTools.Windows.UpdateManager;
#endif

namespace OdyTools.Windows
{
    public partial class MainWindow : Window
    {
        private OdyInstallation _active;
        private Dictionary<string, OdyInstallation> _installations;
        private GlobalSettings _settings;
        private int _previousGameComboIndex;
#if !NET48
        private UpdateManager _updateManager;
        public UpdateManager UpdateManager => _updateManager;
#else
        public object UpdateManager => null;
#endif

        public MainWindowUi Ui { get; private set; }

        public OdyInstallation Active => _active;

        public Dictionary<string, OdyInstallation> Installations => _installations;

        public GlobalSettings Settings => _settings;

        // UI Widgets - populated from XAML or created programmatically
        private ComboBox _gameCombo;
        private TabControl _resourceTabs;
        private ResourceList _coreWidget;
        private ResourceList _modulesWidget;
        private ResourceList _overrideWidget;
        private ResourceList _savesWidget;
        private ResourceList _texturesWidget;
        private Button _openButton;
        private Button _extractButton;
        private Button _specialActionButton;
        private Button _erfEditorButton;
        private TabItem _coreTab;
        private TabItem _savesTab;
        private TabItem _modulesTab;
        private TabItem _overrideTab;
        private Button _openSaveEditorButton;
        private MenuItem _actionNewDLG;
        private MenuItem _actionNewUTC;
        private MenuItem _actionNewNSS;
        private bool _coreTabLoaded;
        private bool _modulesTabLoaded;
        private bool _overrideTabLoaded;
        private bool _savesTabLoaded;

        public MainWindow()
        {
            InitializeComponent();
            _active = null;
            _installations = new Dictionary<string, OdyInstallation>();
            _settings = new GlobalSettings();
            _previousGameComboIndex = 0;
#if !NET48
            _updateManager = new UpdateManager(silent: true);
#endif

            Title = "OdyTools";

            SetupUI();
            SetupSignals();
            ReloadSettings();
            UnsetInstallation();
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

            // Try to find controls from XAML (with try-catch for each to handle test scenarios)
            try
            {
                _gameCombo = this.FindControl<ComboBox>("gameCombo");
            }
            catch { }

            try
            {
                _resourceTabs = this.FindControl<TabControl>("resourceTabs");
            }
            catch { }

            try
            {
                _openButton = this.FindControl<Button>("openButton");
            }
            catch { }

            try
            {
                _extractButton = this.FindControl<Button>("extractButton");
            }
            catch { }

            try
            {
                _specialActionButton = this.FindControl<Button>("specialActionButton");
            }
            catch { }

            try
            {
                _erfEditorButton = this.FindControl<Button>("erfEditorButton");
            }
            catch { }

            // Find resource list widgets
            try
            {
                _coreWidget = this.FindControl<ResourceList>("coreWidget");
            }
            catch { }

            try
            {
                _modulesWidget = this.FindControl<ResourceList>("modulesWidget");
            }
            catch { }

            try
            {
                _overrideWidget = this.FindControl<ResourceList>("overrideWidget");
            }
            catch { }

            try
            {
                _savesWidget = this.FindControl<ResourceList>("savesWidget");
            }
            catch { }

            try
            {
                _texturesWidget = this.FindControl<ResourceList>("texturesWidget");
            }
            catch { }

            // Find tab items
            try
            {
                _coreTab = this.FindControl<TabItem>("coreTab");
            }
            catch { }

            try
            {
                _modulesTab = this.FindControl<TabItem>("modulesTab");
            }
            catch { }

            try
            {
                _overrideTab = this.FindControl<TabItem>("overrideTab");
            }
            catch { }

            try
            {
                _savesTab = this.FindControl<TabItem>("savesTab");
            }
            catch { }

            // Find menu items
            try
            {
                _actionNewDLG = this.FindControl<MenuItem>("actionNewDLG");
            }
            catch { }

            try
            {
                _actionNewUTC = this.FindControl<MenuItem>("actionNewUTC");
            }
            catch { }

            try
            {
                _actionNewNSS = this.FindControl<MenuItem>("actionNewNSS");
            }
            catch { }

            try
            {
                _openSaveEditorButton = this.FindControl<Button>("openSaveEditorButton");
            }
            catch { }

            if (!xamlLoaded)
            {
                SetupProgrammaticUI();
            }

            // Initially hide ERF editor button (matching PyKotor: self.erf_editor_button.hide())
            if (_erfEditorButton != null)
            {
                _erfEditorButton.IsVisible = false;
            }
        }

        private void SetupProgrammaticUI()
        {
            // Create basic UI structure programmatically
            var mainPanel = new StackPanel();

            // Game selection combo
            _gameCombo = new ComboBox();
            _gameCombo.Items.Add("[None]");
            mainPanel.Children.Add(_gameCombo);

            // Resource tabs
            _resourceTabs = new TabControl();
            _resourceTabs.Items.Add(new TabItem { Header = "Core", Content = new TextBlock { Text = "Core Tab" } });
            _resourceTabs.Items.Add(new TabItem { Header = "Modules", Content = new TextBlock { Text = "Modules Tab" } });
            _resourceTabs.Items.Add(new TabItem { Header = "Override", Content = new TextBlock { Text = "Override Tab" } });
            _resourceTabs.Items.Add(new TabItem { Header = "Textures", Content = new TextBlock { Text = "Textures Tab" } });
            _resourceTabs.Items.Add(new TabItem { Header = "Saves", Content = new TextBlock { Text = "Saves Tab" } });
            mainPanel.Children.Add(_resourceTabs);

            // Action buttons
            var buttonPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal };
            _openButton = new Button { Content = "Open Selected" };
            _extractButton = new Button { Content = "Extract Selected" };
            _specialActionButton = new Button { Content = "Designer" };
            _erfEditorButton = new Button { Content = "ERF Editor" };
            buttonPanel.Children.Add(_openButton);
            buttonPanel.Children.Add(_extractButton);
            buttonPanel.Children.Add(_specialActionButton);
            buttonPanel.Children.Add(_erfEditorButton);
            mainPanel.Children.Add(buttonPanel);

            Content = mainPanel;
        }

        private void SetupUI()
        {
            // Initialize widgets if not already done
            if (_coreWidget == null)
            {
                _coreWidget = new ResourceList();
            }
            if (_modulesWidget == null)
            {
                _modulesWidget = new ResourceList();
            }
            if (_overrideWidget == null)
            {
                _overrideWidget = new ResourceList();
            }
            if (_savesWidget == null)
            {
                _savesWidget = new ResourceList();
            }
            if (_texturesWidget == null)
            {
                _texturesWidget = new ResourceList();
            }

            // Create UI wrapper exposing all controls
            Ui = new MainWindowUi
            {
                GameCombo = _gameCombo,
                ResourceTabs = _resourceTabs,
                CoreWidget = _coreWidget,
                ModulesWidget = _modulesWidget,
                OverrideWidget = _overrideWidget,
                SavesWidget = _savesWidget,
                TexturesWidget = _texturesWidget,
                CoreTab = _coreTab,
                SavesTab = _savesTab,
                ModulesTab = _modulesTab,
                OverrideTab = _overrideTab,
                ActionNewDLG = _actionNewDLG,
                ActionNewUTC = _actionNewUTC,
                ActionNewNSS = _actionNewNSS
            };
        }

        private void SetupSignals()
        {
            if (_gameCombo != null)
            {
                _gameCombo.SelectionChanged += (sender, e) =>
                {
                    if (_gameCombo.SelectedIndex >= 0)
                    {
                        ChangeActiveInstallation(_gameCombo.SelectedIndex);
                    }
                };
            }

            if (_openButton != null)
            {
                _openButton.Click += (sender, e) => OnOpenResources(GetActiveResourceWidget().SelectedResources());
            }

            if (_extractButton != null)
            {
                _extractButton.Click += (sender, e) => OnExtractResources(GetActiveResourceWidget().SelectedResources());
            }

            if (_specialActionButton != null)
            {
                _specialActionButton.Click += (sender, e) => OpenModuleDesigner();
            }

            if (_erfEditorButton != null)
            {
                _erfEditorButton.Click += (sender, e) => OpenModuleTabErfEditor();
            }

            if (_openSaveEditorButton != null)
            {
                _openSaveEditorButton.Click += (sender, e) => OnOpenSaveEditor();
            }

            // Connect tab control selection changed event
            if (_resourceTabs != null)
            {
                _resourceTabs.SelectionChanged += (sender, e) => OnTabChanged();
            }

            // Connect ResourceList events
            ConnectResourceListEvents();

            // Connect menu actions from XAML
            ConnectMenuActions();
        }

        private void ConnectResourceListEvents()
        {
            // Connect coreWidget events
            if (_coreWidget != null)
            {
                _coreWidget.RefreshClicked += (sender, e) => OnCoreRefresh();
                _coreWidget.ResourceDoubleClicked += (sender, e) => OnOpenResources(e.Resources, e.UseSpecializedEditor);
                _coreWidget.ExtractRequested += (sender, e) => OnExtractResources(e.Resources);
            }

            // Connect modulesWidget events
            if (_modulesWidget != null)
            {
                _modulesWidget.SectionChanged += (sender, section) => OnModuleChanged(section);
                _modulesWidget.ReloadClicked += (sender, section) => OnModuleReload(section);
                _modulesWidget.RefreshClicked += (sender, e) => OnModuleRefresh();
                _modulesWidget.ResourceDoubleClicked += (sender, e) => OnOpenResources(e.Resources, e.UseSpecializedEditor);
                _modulesWidget.ExtractRequested += (sender, e) => OnExtractResources(e.Resources);
            }

            // Connect savesWidget events
            if (_savesWidget != null)
            {
                _savesWidget.IsSavesWidget = true;
                _savesWidget.SectionChanged += (sender, section) => OnSavePathChanged(section);
                _savesWidget.ReloadClicked += (sender, section) => OnSaveReload(section);
                _savesWidget.RefreshClicked += (sender, e) => OnSaveRefresh();
                _savesWidget.ResourceDoubleClicked += (sender, e) => OnOpenResources(e.Resources, e.UseSpecializedEditor);
                _savesWidget.ExtractRequested += (sender, e) => OnExtractResources(e.Resources);
                _savesWidget.RequestOpenSaveEditor += (sender, e) => OnOpenSaveEditor();
            }

            // Connect overrideWidget events
            if (_overrideWidget != null)
            {
                _overrideWidget.SectionChanged += (sender, section) => OnOverrideChanged(section);
                _overrideWidget.ReloadClicked += (sender, section) => OnOverrideReload(section);
                _overrideWidget.RefreshClicked += (sender, e) => OnOverrideRefresh();
                _overrideWidget.ResourceDoubleClicked += (sender, e) => OnOpenResources(e.Resources, e.UseSpecializedEditor);
                _overrideWidget.ExtractRequested += (sender, e) => OnExtractResources(e.Resources);
            }

            // Connect texturesWidget events
            if (_texturesWidget != null)
            {
                _texturesWidget.SectionChanged += (sender, section) => OnTexturesChanged(section);
                _texturesWidget.ResourceDoubleClicked += (sender, e) => OnOpenResources(e.Resources, e.UseSpecializedEditor);
                _texturesWidget.ExtractRequested += (sender, e) => OnExtractResources(e.Resources);
            }
        }

        // open selected save(s) in Save Editor
        private void OnOpenSaveEditor()
        {
            if (_savesWidget == null) return;
            var resources = _savesWidget.SelectedResources();
            if (resources == null || resources.Count == 0) return;
            OnOpenResources(resources, true);
        }

        // Wire File -> New submenu items to open new editors
        private void ConnectNewMenuActions()
        {
            ConnectNewMenuItem("actionNewDLG", () => OpenNewEditor(new OdyTools.Editors.DLG.OdyToolDLG(this, _active)));
            ConnectNewMenuItem("actionNewERF", () => OpenNewEditor(new OdyToolERF(this, _active)));
            ConnectNewMenuItem("actionNewGFF", () => OpenNewEditor(new OdyToolGFF(this, _active)));
            ConnectNewMenuItem("actionNewNSS", () => OpenNewEditor(new OdyToolNSS(this, _active)));
            ConnectNewMenuItem("actionNewSSF", () => OpenNewEditor(new OdyToolSSF(this, _active)));
            ConnectNewMenuItem("actionNewTLK", () => OpenNewEditor(new OdyToolTLK(this, _active)));
            ConnectNewMenuItem("actionNewTXT", () => OpenNewEditor(new OdyToolTXT(this, _active)));
            ConnectNewMenuItem("actionNewUTC", () => OpenNewEditor(new OdyToolUTC(this, _active)));
            ConnectNewMenuItem("actionNewUTD", () => OpenNewEditor(new OdyToolUTD(this, _active)));
            ConnectNewMenuItem("actionNewUTE", () => OpenNewEditor(new OdyToolUTE(this, _active)));
            ConnectNewMenuItem("actionNewUTI", () => OpenNewEditor(new OdyToolUTI(this, _active)));
            ConnectNewMenuItem("actionNewUTM", () => OpenNewEditor(new OdyToolUTM(this, _active)));
            ConnectNewMenuItem("actionNewUTP", () => OpenNewEditor(new OdyToolUTP(this, _active)));
            ConnectNewMenuItem("actionNewUTS", () => OpenNewEditor(new OdyToolUTS(this, _active)));
            ConnectNewMenuItem("actionNewUTT", () => OpenNewEditor(new OdyToolUTT(this, _active)));
            ConnectNewMenuItem("actionNewUTW", () => OpenNewEditor(new OdyToolUTW(this, _active)));
        }

        private void ConnectNewMenuItem(string name, Action openEditor)
        {
            var item = this.FindControl<MenuItem>(name);
            if (item != null)
            {
                item.Click += (s, e) => openEditor();
            }
        }

        private void OpenNewEditor(Editor editor)
        {
            if (editor != null)
            {
                WindowUtils.AddWindow(editor, show: true);
            }
        }

        private void ConnectMenuActions()
        {
            // Find menu items from XAML and connect them
            // Use try-catch to handle cases where XAML controls might not be available (e.g., in tests)
            try
            {
                // File -> New menu - wire all New X actions to open corresponding editors
                ConnectNewMenuActions();

                var actionSettings = this.FindControl<MenuItem>("actionSettings");
                if (actionSettings != null)
                {
                    actionSettings.Click += (s, e) => OpenSettingsDialog();
                }

                var actionExit = this.FindControl<MenuItem>("actionExit");
                if (actionExit != null)
                {
                    actionExit.Click += (s, e) => Close();
                }

                var openAction = this.FindControl<MenuItem>("openAction");
                if (openAction != null)
                {
                    openAction.Click += (s, e) => OpenFromFile();
                }

                var menuRecentFiles = this.FindControl<MenuItem>("menuRecentFiles");
                if (menuRecentFiles != null)
                {
                    menuRecentFiles.SubmenuOpened += (s, e) => PopulateRecentFilesMenu(menuRecentFiles);
                    PopulateRecentFilesMenu(menuRecentFiles); // Initial populate so submenu is visible
                }

                var menuTheme = this.FindControl<MenuItem>("menuTheme");
                if (menuTheme != null)
                {
                    menuTheme.SubmenuOpened += (s, e) => PopulateThemeMenu(menuTheme);
                    PopulateThemeMenu(menuTheme); // Initial populate so submenu is visible
                }

                // Help menu
                var actionHelpAbout = this.FindControl<MenuItem>("actionHelpAbout");
                if (actionHelpAbout != null)
                {
                    actionHelpAbout.Click += (s, e) => OpenAboutDialog();
                }

                var actionHelpUpdates = this.FindControl<MenuItem>("actionHelpUpdates");
                if (actionHelpUpdates != null)
                {
                    #if !NET48
                    actionHelpUpdates.Click += (s, e) => _updateManager?.CheckForUpdates(silent: false);
#endif
                }

                var actionInstructions = this.FindControl<MenuItem>("actionInstructions");
                if (actionInstructions != null)
                {
                    actionInstructions.Click += (s, e) => OpenInstructionsWindow();
                }

                // Tools menu
                var actionModuleDesigner = this.FindControl<MenuItem>("actionModuleDesigner");
                if (actionModuleDesigner != null)
                {
                    actionModuleDesigner.Click += (s, e) => OpenModuleDesigner();
                }

                var actionFileSearch = this.FindControl<MenuItem>("actionFileSearch");
                if (actionFileSearch != null)
                {
                    actionFileSearch.Click += (s, e) => OpenFileSearchDialog();
                }

                var actionCloneModule = this.FindControl<MenuItem>("actionCloneModule");
                if (actionCloneModule != null)
                {
                    actionCloneModule.Click += (s, e) => OpenCloneModuleDialog();
                }

                var actionIndoorMapBuilder = this.FindControl<MenuItem>("actionIndoorMapBuilder");
                if (actionIndoorMapBuilder != null)
                {
                    actionIndoorMapBuilder.Click += (s, e) => OpenIndoorMapBuilder();
                }

                var actionKotorDiff = this.FindControl<MenuItem>("actionKotorDiff");
                if (actionKotorDiff != null)
                {
                    actionKotorDiff.Click += (s, e) => OpenKotordiff();
                }

                var actionTSLPatchDataEditor = this.FindControl<MenuItem>("actionTSLPatchDataEditor");
                if (actionTSLPatchDataEditor != null)
                {
                    actionTSLPatchDataEditor.Click += (s, e) => OpenTslPatchDataEditor(null);
                }

                // Help -> Discord submenu
                var actionDiscordOdyTools = this.FindControl<MenuItem>("actionDiscordOdyTools");
                if (actionDiscordOdyTools != null)
                {
                    actionDiscordOdyTools.Click += (s, e) => OpenUrl("https://discord.gg/odytools");
                }
                var actionDiscordKotOR = this.FindControl<MenuItem>("actionDiscordKotOR");
                if (actionDiscordKotOR != null)
                {
                    actionDiscordKotOR.Click += (s, e) => OpenUrl("https://discord.gg/kotor");
                }
                var actionDiscordDeadlyStream = this.FindControl<MenuItem>("actionDiscordDeadlyStream");
                if (actionDiscordDeadlyStream != null)
                {
                    actionDiscordDeadlyStream.Click += (s, e) => OpenUrl("https://discord.gg/deadlystream");
                }

                // Language menu (populate on open)
                var menuLanguage = this.FindControl<MenuItem>("menuLanguage");
                if (menuLanguage != null)
                {
                    menuLanguage.SubmenuOpened += (s, e) => PopulateLanguageMenu(menuLanguage);
                    PopulateLanguageMenu(menuLanguage);
                }

                // Edit menu
                var actionEditTLK = this.FindControl<MenuItem>("actionEditTLK");
                if (actionEditTLK != null)
                {
                    actionEditTLK.Click += (s, e) => OpenActiveTalktable();
                }

                var actionEditJRL = this.FindControl<MenuItem>("actionEditJRL");
                if (actionEditJRL != null)
                {
                    actionEditJRL.Click += (s, e) => OpenActiveJournal();
                }
            }
            catch
            {
                // XAML controls not available - menu actions will not be connected in test scenarios
                // This is acceptable for headless test environments
            }
        }

        private void OnOpenResources(List<FileResource> resources, bool? useSpecializedEditor = null)
        {
            if (_active == null || resources == null || resources.Count == 0)
            {
                return;
            }

            foreach (var resource in resources)
            {
                // On Windows, prefer the installed file association/default editor when possible.
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    && !string.IsNullOrWhiteSpace(resource.FilePath)
                    && File.Exists(resource.FilePath)
                    && ShellFileActions.TryOpenWithSystemDefault(resource.FilePath))
                {
                    continue;
                }

                WindowUtils.OpenResourceEditor(
                    resource.FilePath,
                    resource.ResName,
                    resource.ResType,
                    resource.GetData(),
                    _active,
                    this,
                    useSpecializedEditor);
            }
        }

        private async void OnExtractResources(List<FileResource> resources)
        {
            if (resources == null || resources.Count == 0)
            {
                return;
            }

            // Build extract save paths - show folder picker dialog
            var extractResult = await BuildExtractSavePaths(resources);
            if (extractResult == null)
            {
                return; // User cancelled
            }

            var (folderPath, pathsToWrite) = extractResult.Value;

            // File conflict resolution: if any destination file exists, prompt Overwrite all / Skip existing / Cancel
            var existingPaths = pathsToWrite.Where(kvp => File.Exists(kvp.Value)).ToList();
            if (existingPaths.Count > 0)
            {
                int n = existingPaths.Count;
                var conflictBox = MessageBoxManager.GetMessageBoxStandard(
                    "File conflict",
                    $"{n} file(s) already exist at the destination.\n\nYes = Overwrite all\nNo = Skip existing files\nCancel = Abort extraction",
                    ButtonEnum.YesNoCancel,
                    MsBox.Avalonia.Enums.Icon.Question);
                var conflictResult = await conflictBox.ShowWindowDialogAsync(this);
                if (conflictResult == ButtonResult.Cancel)
                {
                    return;
                }
                if (conflictResult == ButtonResult.No)
                {
                    foreach (var kvp in existingPaths)
                    {
                        pathsToWrite.Remove(kvp.Key);
                    }
                    if (pathsToWrite.Count == 0)
                    {
                        return;
                    }
                }
            }

            // Determine final save paths (create dirs, collect failures)
            var failedSavePathHandlers = new Dictionary<string, Exception>();
            var resourceSavePaths = DetermineSavePaths(pathsToWrite, failedSavePathHandlers);
            if (resourceSavePaths.Count == 0)
            {
                return;
            }

            // Create progress dialog
            var progressDialog = new Dialogs.ExtractionProgressDialog(resourceSavePaths.Count);
            progressDialog.Show();

            // Show progress dialog and extract resources
            await ExtractResourcesAsync(resourceSavePaths, failedSavePathHandlers, progressDialog);
        }

        private async Task<(string FolderPath, Dictionary<FileResource, string> PathsToWrite)?> BuildExtractSavePaths(List<FileResource> resources)
        {
            var pathsToWrite = new Dictionary<FileResource, string>();

            // Show folder picker dialog
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null)
            {
                return null;
            }

            var options = new FolderPickerOpenOptions
            {
                Title = "Extract to folder",
                AllowMultiple = false
            };

            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(options);
            if (folders == null || folders.Count == 0)
            {
                // User cancelled
                return null;
            }

            var folderPath = folders[0].Path.LocalPath;

            // Build save paths for each resource. Use output extension when extraction converts format (e.g. TPC->TGA).
            foreach (var resource in resources)
            {
                string extension = resource.ResType?.Extension ?? "";
                if (resource.ResType == ResourceType.TPC)
                {
                    extension = "tga"; // ExtractResourceAsync decompiles TPC to TGA
                }
                // MDL uses default extension; .mdl.ascii would require an MDL decompiler
                var identifier = $"{resource.ResName}.{extension}";
                var savePath = Path.Combine(folderPath, identifier);
                pathsToWrite[resource] = savePath;
            }

            return (folderPath, pathsToWrite);
        }

        // Caller is responsible for file conflict resolution (filter pathsToWrite before calling if user chose Skip existing).
        private Dictionary<FileResource, string> DetermineSavePaths(Dictionary<FileResource, string> pathsToWrite, Dictionary<string, Exception> failedSavePathHandlers)
        {
            var resourceSavePaths = new Dictionary<FileResource, string>();

            foreach (var kvp in pathsToWrite)
            {
                var resource = kvp.Key;
                var desiredPath = kvp.Value;

                try
                {
                    if (File.Exists(desiredPath))
                    {
                        resourceSavePaths[resource] = desiredPath; // Overwrite (conflict already resolved by caller)
                    }
                    else
                    {
                        var directory = Path.GetDirectoryName(desiredPath);
                        if (!string.IsNullOrEmpty(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }
                        resourceSavePaths[resource] = desiredPath;
                    }
                }
                catch (Exception ex)
                {
                    failedSavePathHandlers[desiredPath] = ex;
                }
            }

            return resourceSavePaths;
        }

        // Async extraction of resources with progress dialog
        private async Task ExtractResourcesAsync(Dictionary<FileResource, string> resourceSavePaths, Dictionary<string, Exception> failedSavePathHandlers, Dialogs.ExtractionProgressDialog progressDialog)
        {
            if (resourceSavePaths.Count == 0)
            {
                return;
            }

            var errors = new List<Exception>();
            var successCount = 0;

            try
            {
                foreach (var kvp in resourceSavePaths)
                {
                    var resource = kvp.Key;
                    var savePath = kvp.Value;

                    try
                    {
                        // Update progress for current item being processed
                        progressDialog.UpdateProgress($"Processing resource: {resource.ResName}.{resource.ResType.Extension}");

                        // Extract the resource
                        await ExtractResourceAsync(resource, savePath);

                        // Increment progress after successful extraction
                        // Pass status text explicitly to avoid cross-thread UI access
                        successCount++;
                        progressDialog.IncrementProgress($"Extracted {successCount}/{resourceSavePaths.Count} resources");
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex);
                        progressDialog.UpdateProgress($"Error extracting {resource.ResName}.{resource.ResType.Extension}: {ex.Message}");
                    }
                }
            }
            finally
            {
                progressDialog.AllowClose();
                progressDialog.Close();
            }

            // Show results dialog
            await ShowExtractionResultsDialog(successCount, resourceSavePaths.Count, errors);
        }

        // Extract a single resource
        private async Task ExtractResourceAsync(FileResource resource, string savePath)
        {
            var data = resource.GetData();

            // Handle resource type specific processing
            if (resource.ResType == ResourceType.TPC)
            {
                // Decompile TPC to TGA format for extraction
                try
                {
                    var tpc = TPCAuto.ReadTpc(data);
                    data = TPCAuto.BytesTpc(tpc, ResourceType.TGA);
                    savePath = Path.ChangeExtension(savePath, ".tga");
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Failed to decompile TPC {resource.ResName}: {ex.Message}");
                    // Fall back to raw data
                }
            }
            else if (resource.ResType == ResourceType.MDL)
            {
                // MDL is extracted as binary; decompilation to .mdl.ascii would require an MDL→ASCII converter
            }
            // Other types (NCS, etc.) extract as-is; decompilation can be done separately (e.g. NCS via ScriptDecompiler)

            // Write the data to file
            await System.Threading.Tasks.Task.Run(() => File.WriteAllBytes(savePath, data));
        }

        // Show extraction results dialog
        private async Task ShowExtractionResultsDialog(int successCount, int totalCount, List<Exception> errors)
        {
            string message;
            string title;
            Icon icon;

            if (errors.Count == 0)
            {
                // Success
                title = "Extraction successful";
                message = $"Successfully extracted {successCount} files.";
                icon = MsBox.Avalonia.Enums.Icon.Info;
            }
            else
            {
                // Partial success or failure
                title = "Failed to extract some items";
                message = $"Failed to extract {errors.Count} files out of {totalCount}.";
                icon = MsBox.Avalonia.Enums.Icon.Warning;
            }

            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                title,
                message,
                ButtonEnum.Ok,
                icon);

            await messageBox.ShowAsync();
        }

        public async void ChangeActiveInstallation(int index)
        {
            if (index < 0)
            {
                return;
            }

            int prevIndex = _previousGameComboIndex;
            if (index == 0)
            {
                UnsetInstallation();
                _previousGameComboIndex = 0;
                return;
            }

            string name = _gameCombo?.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrEmpty(name) || name == "[None]")
            {
                return;
            }

            // Get installation path from settings
            var installations = _settings.Installations();
            if (!installations.ContainsKey(name))
            {
                // Installation not configured - prompt user to configure it
                var promptDialog = new Dialogs.InstallationConfigPromptDialog(name);
                bool shouldConfigure = await promptDialog.ShowDialogAsync(this);

                if (shouldConfigure)
                {
                    // Open settings dialog focused on installations tab
                    var settingsDialog = new Dialogs.SettingsDialog(this);
                    var result = await settingsDialog.ShowDialog<bool?>(this);

                    if (result == true && settingsDialog.InstallationEdited)
                    {
                        // Settings were saved and installations were edited - try again
                        // Re-run the installation selection logic
                        ChangeActiveInstallation(_gameCombo.SelectedIndex);
                        return;
                    }
                }

                // User cancelled or configuration failed - revert selection
                _gameCombo.SelectedIndex = prevIndex;
                return;
            }

            var installData = installations[name];
            string path = installData.ContainsKey("path") ? installData["path"]?.ToString() ?? "" : "";
            bool tsl = installData.ContainsKey("tsl") && installData["tsl"] is bool tslVal && tslVal;

            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard(
                    "Invalid installation path",
                    string.IsNullOrEmpty(path)
                        ? $"The installation path for \"{name}\" is not set. Open Settings to configure it?"
                        : $"The installation path for \"{name}\" does not exist or is not accessible:\n\n{path}\n\nOpen Settings to fix it?",
                    ButtonEnum.YesNo,
                    MsBox.Avalonia.Enums.Icon.Warning);
                var result = await messageBox.ShowWindowDialogAsync(this);
                if (result == ButtonResult.Yes)
                {
                    OpenSettingsDialog();
                }
                _gameCombo.SelectedIndex = prevIndex;
                return;
            }

            // Create or get installation
            if (!_installations.ContainsKey(name))
            {
                _active = new OdyInstallation(path, name, tsl);
                _installations[name] = _active;
            }
            else
            {
                _active = _installations[name];
            }

            // Enable tabs
            if (_resourceTabs != null)
            {
                _resourceTabs.IsEnabled = true;
            }

            // Load the currently active tab first to avoid a single heavy burst on installation switch.
            ResetResourceTabLoadState();
            EnsureActiveResourceTabLoaded(force: true);
            QueueBackgroundResourceTabRefresh();

            UpdateMenus();
            _previousGameComboIndex = index;
        }

        public void UnsetInstallation()
        {
            if (_gameCombo != null)
            {
                _gameCombo.SelectionChanged -= (sender, e) => { };
                _gameCombo.SelectedIndex = 0;
            }

            if (_coreWidget != null)
            {
                _coreWidget.SetResources(new List<FileResource>());
            }
            if (_modulesWidget != null)
            {
                _modulesWidget.SetResources(new List<FileResource>());
            }
            if (_overrideWidget != null)
            {
                _overrideWidget.SetResources(new List<FileResource>());
            }

            if (_resourceTabs != null)
            {
                _resourceTabs.IsEnabled = false;
            }

            ResetResourceTabLoadState();
            UpdateMenus();
            _active = null;
        }

        public void UpdateMenus()
        {
            // Update menu states based on active installation
            // Enable/disable New menu items that require installation (GFF-based editors)
            bool hasInstallation = _active != null;
            var newItemsRequiringInstallation = new[] {
                "actionNewDLG", "actionNewNSS", "actionNewUTC", "actionNewUTP", "actionNewUTD",
                "actionNewUTI", "actionNewUTS", "actionNewUTT", "actionNewUTM", "actionNewUTW", "actionNewUTE"
            };
            foreach (var name in newItemsRequiringInstallation)
            {
                try
                {
                    var item = this.FindControl<MenuItem>(name);
                    if (item != null) item.IsEnabled = hasInstallation;
                }
                catch { /* Control may not exist in test scenarios */ }
            }
            // Enable/disable Edit menu items that require installation
            foreach (var name in new[] { "actionEditTLK", "actionEditJRL" })
            {
                try
                {
                    var item = this.FindControl<MenuItem>(name);
                    if (item != null) item.IsEnabled = hasInstallation;
                }
                catch { }
            }
            // Enable/disable Tools menu items that require installation
            foreach (var name in new[] {
                "actionModuleDesigner", "actionIndoorMapBuilder", "actionKotorDiff", "actionTSLPatchDataEditor",
                "actionFileSearch", "actionCloneModule"
            })
            {
                try
                {
                    var item = this.FindControl<MenuItem>(name);
                    if (item != null) item.IsEnabled = hasInstallation;
                }
                catch { }
            }
            // Open Save Editor button (Saves tab) requires installation
            if (_openSaveEditorButton != null)
            {
                _openSaveEditorButton.IsEnabled = hasInstallation;
            }
        }

        public void RefreshCoreList(bool reload = true)
        {
            if (_active == null || _coreWidget == null)
            {
                return;
            }

            try
            {
                // Get core resources from installation
                var resources = _active.CoreResources();
                _coreWidget.SetResources(resources);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to refresh core list: {ex}");
            }
        }

        public void RefreshSavesList(bool reload = true)
        {
            if (_active == null || _savesWidget == null)
            {
                return;
            }

            try
            {
                var saveLocations = _active.SaveLocations();
                var sections = new List<string>();
                foreach (var location in saveLocations)
                {
                    sections.Add(location);
                }
                _savesWidget.SetSections(sections);
                // If there is at least one section and none selected, select the first so resources load
                var sectionCombo = _savesWidget.Ui?.SectionCombo;
                if (sectionCombo != null && sections.Count > 0 && sectionCombo.SelectedIndex < 0)
                {
                    sectionCombo.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to refresh saves list: {ex}");
            }
        }

        public void RefreshModuleList(bool reload = true, List<object> moduleItems = null)
        {
            if (_active == null || _modulesWidget == null)
            {
                return;
            }

            try
            {
                if (moduleItems != null)
                {
                    // Use provided module items (for testing)
                    var sections = new List<string>();
                    foreach (var item in moduleItems)
                    {
                        sections.Add(item.ToString());
                    }
                    _modulesWidget.SetSections(sections);
                }
                else
                {
                    // Get modules from installation
                    var moduleNames = _active.ModuleNames();
                    var sections = new List<string>();
                    foreach (var moduleName in moduleNames.Keys)
                    {
                        sections.Add(moduleName);
                    }
                    _modulesWidget.SetSections(sections);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to refresh module list: {ex}");
            }
        }

        public void RefreshOverrideList(bool reload = true)
        {
            if (_active == null || _overrideWidget == null)
            {
                return;
            }

            try
            {
                // Get override directories from installation
                var overrideList = _active.OverrideList();
                var sections = new List<string>();
                foreach (var dir in overrideList)
                {
                    sections.Add(dir);
                }
                _overrideWidget.SetSections(sections);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to refresh override list: {ex}");
            }
        }

        public void ReloadSettings()
        {
            ReloadInstallations();
        }

        public void ReloadInstallations()
        {
            if (_gameCombo == null)
            {
                return;
            }

            _gameCombo.Items.Clear();
            _gameCombo.Items.Add("[None]");

            var installations = _settings.Installations();
            foreach (var installName in installations.Keys)
            {
                _gameCombo.Items.Add(installName);
            }
        }

        public ResourceList GetActiveResourceWidget()
        {
            if (_resourceTabs == null)
            {
                return _coreWidget ?? new ResourceList();
            }

            int currentIndex = _resourceTabs.SelectedIndex;
            if (currentIndex == 0)
            {
                return _coreWidget ?? new ResourceList();
            }
            else if (currentIndex == 1)
            {
                return _modulesWidget ?? new ResourceList();
            }
            else if (currentIndex == 2)
            {
                return _overrideWidget ?? new ResourceList();
            }
            else if (currentIndex == 3)
            {
                return _texturesWidget ?? new ResourceList();
            }
            else if (currentIndex == 4)
            {
                return _savesWidget ?? new ResourceList();
            }

            return _coreWidget ?? new ResourceList();
        }

        public Control GetActiveResourceTab()
        {
            if (_resourceTabs?.SelectedItem is TabItem selectedTab)
            {
                return selectedTab;
            }
            return _coreTab;
        }

        public int GetActiveTabIndex()
        {
            if (_resourceTabs != null)
            {
                return _resourceTabs.SelectedIndex;
            }
            return 0;
        }

        public void OnTabChanged()
        {
            EnsureActiveResourceTabLoaded(force: false);

            // Handle tab change - update UI state based on active tab
            // Show/hide ERF editor button on modules tab
            if (_resourceTabs?.SelectedItem == _modulesTab)
            {
                // Show ERF editor button when on modules tab
                if (_erfEditorButton != null)
                {
                    _erfEditorButton.IsVisible = true;
                }
            }
            else
            {
                // Hide ERF editor button when not on modules tab
                if (_erfEditorButton != null)
                {
                    _erfEditorButton.IsVisible = false;
                }
            }
        }

        private void ResetResourceTabLoadState()
        {
            _coreTabLoaded = false;
            _modulesTabLoaded = false;
            _overrideTabLoaded = false;
            _savesTabLoaded = false;
        }

        private void EnsureActiveResourceTabLoaded(bool force)
        {
            if (_resourceTabs == null || _active == null)
            {
                return;
            }

            var selectedTab = _resourceTabs.SelectedItem as TabItem;
            if (selectedTab == _modulesTab)
            {
                if (force || !_modulesTabLoaded)
                {
                    RefreshModuleList(reload: false);
                    _modulesTabLoaded = true;
                }
                return;
            }

            if (selectedTab == _overrideTab)
            {
                if (force || !_overrideTabLoaded)
                {
                    RefreshOverrideList(reload: false);
                    _overrideTabLoaded = true;
                }
                return;
            }

            if (selectedTab == _savesTab)
            {
                if (force || !_savesTabLoaded)
                {
                    RefreshSavesList(reload: false);
                    _savesTabLoaded = true;
                }
                return;
            }

            if (force || !_coreTabLoaded)
            {
                RefreshCoreList(reload: false);
                _coreTabLoaded = true;
            }
        }

        private void QueueBackgroundResourceTabRefresh()
        {
            if (_active == null)
            {
                return;
            }

            var activeInstallation = _active;
            Avalonia.Threading.Dispatcher.UIThread.Post(() =>
            {
                if (_active != activeInstallation)
                {
                    return;
                }

                if (!_coreTabLoaded)
                {
                    RefreshCoreList(reload: false);
                    _coreTabLoaded = true;
                }

                if (!_modulesTabLoaded)
                {
                    RefreshModuleList(reload: false);
                    _modulesTabLoaded = true;
                }

                if (!_overrideTabLoaded)
                {
                    RefreshOverrideList(reload: false);
                    _overrideTabLoaded = true;
                }

                if (!_savesTabLoaded)
                {
                    RefreshSavesList(reload: false);
                    _savesTabLoaded = true;
                }
            }, Avalonia.Threading.DispatcherPriority.Background);
        }

        private void OpenModuleDesigner()
        {
            // Matching Python: assert self.active is not None, "No installation loaded."
            if (_active == null)
            {
                return;
            }

            // Matching Python: selected_module: Path | None = None
            string selectedModulePath = null;

            // Matching Python: try: combo_data = self.ui.modulesWidget.ui.sectionCombo.currentData(Qt.ItemDataRole.UserRole)
            // Matching Python: except Exception: combo_data = None
            try
            {
                if (_modulesWidget?.Ui?.SectionCombo != null && _modulesWidget.Ui.SectionCombo.SelectedItem != null)
                {
                    // Get the selected module filename from the section combo
                    string moduleFilename = _modulesWidget.Ui.SectionCombo.SelectedItem.ToString();
                    if (!string.IsNullOrEmpty(moduleFilename))
                    {
                        // Matching Python: selected_module = self.active.module_path() / Path(str(combo_data))
                        string modulePath = _active.ModulePath();
                        selectedModulePath = System.IO.Path.Combine(modulePath, moduleFilename);
                    }
                }
            }
            catch (Exception)
            {
                // If we can't get the selected module, continue without it (designer will open empty)
                selectedModulePath = null;
            }

            // Matching Python: try: designer_window = ModuleDesigner(None, self.active, mod_filepath=selected_module)
            // Matching Python: except TypeError as exc: ... designer_window = ModuleDesigner(None, self.active)
            ModuleDesignerWindow designerWindow = null;
            try
            {
                // Try to create designer with module path
                if (!string.IsNullOrEmpty(selectedModulePath))
                {
                    designerWindow = new ModuleDesignerWindow(this, _active, selectedModulePath);
                }
                else
                {
                    // Create designer without module path - user can open module via dialog
                    designerWindow = new ModuleDesignerWindow(this, _active, null);
                }
            }
            catch (Exception ex)
            {
                // Fallback: create designer without module path if constructor fails
                // Matching Python: RobustLogger().warning(f"ModuleDesigner signature mismatch: {exc}. Falling back without module path.")
                System.Console.WriteLine($"ModuleDesigner creation failed: {ex.Message}. Falling back without module path.");
                designerWindow = new ModuleDesignerWindow(this, _active, null);

                // If we had a selected module, open it after a short delay
                // Matching Python: if selected_module is not None: QTimer.singleShot(33, lambda: designer_window.open_module(selected_module))
                if (!string.IsNullOrEmpty(selectedModulePath))
                {
                    // Use Avalonia's dispatcher to defer opening the module
                    // This matches Python's QTimer.singleShot(33, ...) behavior
                    Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    {
                        designerWindow.OpenModule(selectedModulePath);
                    }, Avalonia.Threading.DispatcherPriority.Background);
                }
            }

            // Show the designer window
            // Matching Python: designer_window.show()
            if (designerWindow != null)
            {
                designerWindow.Show();
            }
        }

        private void PopulateThemeMenu(MenuItem menuTheme)
        {
            if (menuTheme == null) return;

            menuTheme.Items.Clear();

            var currentTheme = _settings?.SelectedTheme ?? "Light";

            var themes = new[] { ("Light", "Light"), ("Dark", "Dark"), ("System default", "Default") };
            foreach (var (displayName, value) in themes)
            {
                var header = string.Equals(currentTheme, value, StringComparison.OrdinalIgnoreCase)
                    ? "✓ " + displayName
                    : displayName;
                var item = new MenuItem { Header = header };
                var themeValue = value;
                item.Click += (s, e) => ApplyThemeSelection(themeValue);
                menuTheme.Items.Add(item);
            }
        }

        private void ApplyThemeSelection(string themeValue)
        {
            if (_settings == null) _settings = new GlobalSettings();
            _settings.SelectedTheme = themeValue;
            _settings.SetValue("SelectedTheme", themeValue);

            if (Application.Current != null)
            {
                if (string.Equals(themeValue, "Dark", StringComparison.OrdinalIgnoreCase))
                {
                    Application.Current.RequestedThemeVariant = ThemeVariant.Dark;
                }
                else if (string.Equals(themeValue, "Default", StringComparison.OrdinalIgnoreCase))
                {
                    Application.Current.RequestedThemeVariant = ThemeVariant.Default;
                }
                else
                {
                    Application.Current.RequestedThemeVariant = ThemeVariant.Light;
                }
            }
        }

        private void PopulateRecentFilesMenu(MenuItem menuRecentFiles)
        {
            if (menuRecentFiles == null) return;

            menuRecentFiles.Items.Clear();

            var settings = new OdyTools.Data.Settings("Global");
            var recentFiles = settings.GetValue("RecentFiles", new List<string>())
                .Where(fp => !string.IsNullOrEmpty(fp) && File.Exists(fp))
                .Take(15)
                .ToList();

            foreach (var filepath in recentFiles)
            {
                var displayName = Path.GetFileName(filepath);
                if (string.IsNullOrEmpty(displayName)) displayName = filepath;

                var item = new MenuItem { Header = displayName };
                ToolTip.SetTip(item, filepath);
                var path = filepath; // capture for closure
                item.Click += (s, e) => OpenRecentFile(path);
                menuRecentFiles.Items.Add(item);
            }

            if (recentFiles.Count == 0)
            {
                var emptyItem = new MenuItem
                {
                    Header = "(No recent files)",
                    IsEnabled = false
                };
                menuRecentFiles.Items.Add(emptyItem);
            }
        }

        private void OpenRecentFile(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath) || !File.Exists(filepath))
            {
                return;
            }

            try
            {
                var fileInfo = new FileInfo(filepath);
                string resname = Path.GetFileNameWithoutExtension(filepath);
                string extension = Path.GetExtension(filepath);
                ResourceType restype = ResourceType.FromExtension(extension);
                byte[] data = File.ReadAllBytes(filepath);

                var fileResource = new FileResource(
                    resname,
                    restype,
                    (int)fileInfo.Length,
                    0x0,
                    filepath);

                WindowUtils.OpenResourceEditor(fileResource, _active, this);
            }
            catch (Exception ex)
            {
                string errorType = ex.GetType().Name;
                string errorMessage = ex.Message;
                if (string.IsNullOrEmpty(errorMessage)) errorMessage = ex.ToString();

                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    $"Failed to open file ({errorType})",
                    errorMessage,
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                errorBox.ShowAsync();
            }
        }

        //          filepaths: list[str] = QFileDialog.getOpenFileNames(self, "Select files to open")[:-1][0]
        //          for filepath in filepaths:
        //              r_filepath = Path(filepath)
        //              try:
        //                  file_res = FileResource(r_filepath.stem, ResourceType.from_extension(r_filepath.suffix), r_filepath.stat().st_size, 0x0, r_filepath)
        //                  open_resource_editor(file_res, self.active, self)
        //              except (ValueError, OSError) as e:
        //                  etype, msg = universal_simplify_exception(e)
        //                  QMessageBox(QMessageBox.Icon.Critical, f"Failed to open file ({etype})", msg).exec()
        private async void OpenFromFile()
        {
            // Get the top-level window for file dialog
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null)
            {
                return;
            }

            // Create file picker options for multiple file selection
            var options = new FilePickerOpenOptions
            {
                Title = "Select files to open",
                AllowMultiple = true
            };

            try
            {
                // Show file dialog
                var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
                if (files == null || files.Count == 0)
                {
                    return;
                }

                // Process each selected file
                foreach (var file in files)
                {
                    string filepath = file.Path.LocalPath;
                    if (string.IsNullOrWhiteSpace(filepath))
                    {
                        continue;
                    }

                    try
                    {
                        // Get file info
                        var fileInfo = new FileInfo(filepath);
                        if (!fileInfo.Exists)
                        {
                            continue;
                        }

                        // Get resource name (stem - filename without extension)
                        string resname = Path.GetFileNameWithoutExtension(filepath);

                        // Get resource type from file extension
                        string extension = Path.GetExtension(filepath);
                        ResourceType restype = ResourceType.FromExtension(extension);

                        // Read file data
                        byte[] data = File.ReadAllBytes(filepath);

                        // Create FileResource
                        var fileResource = new FileResource(
                            resname,
                            restype,
                            (int)fileInfo.Length,
                            0x0,
                            filepath);

                        // Open resource editor
                        WindowUtils.OpenResourceEditor(fileResource, _active, this);
                    }
                    catch (Exception ex)
                    {
                        string errorType = ex.GetType().Name;
                        string errorMessage = ex.Message;
                        if (string.IsNullOrEmpty(errorMessage))
                        {
                            errorMessage = ex.ToString();
                        }

                        var errorBox = MessageBoxManager.GetMessageBoxStandard(
                            $"Failed to open file ({errorType})",
                            errorMessage,
                            ButtonEnum.Ok,
                            MsBox.Avalonia.Enums.Icon.Error);
                        await errorBox.ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle file dialog errors
                string errorType = ex.GetType().Name;
                string errorMessage = ex.Message;
                if (string.IsNullOrEmpty(errorMessage))
                {
                    errorMessage = ex.ToString();
                }

                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    $"Failed to open file dialog ({errorType})",
                    errorMessage,
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                await errorBox.ShowAsync();
            }
        }

        public void OnCoreRefresh()
        {
            RefreshCoreList(reload: true);
        }

        public void OnModuleChanged(string newModuleFile)
        {
            OnModuleReload(newModuleFile);
        }

        public void OnModuleReload(string moduleFile)
        {
            if (_active == null || string.IsNullOrWhiteSpace(moduleFile))
            {
                return;
            }

            try
            {
                var resources = _active.ModuleResources(moduleFile);
                _modulesWidget?.SetResources(resources);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to reload module '{moduleFile}': {ex}");
            }
        }

        public void OnModuleRefresh()
        {
            RefreshModuleList(reload: true);
        }

        public void OnOverrideChanged(string newDirectory)
        {
            if (_active == null)
            {
                return;
            }
            _overrideWidget?.SetResources(_active.OverrideResources(newDirectory));
        }

        public void OnOverrideReload(string fileOrFolder)
        {
            if (_active == null)
            {
                return;
            }

            try
            {
                var overridePath = _active.OverridePath();
                var fileOrFolderPath = Path.Combine(overridePath, fileOrFolder);
                if (File.Exists(fileOrFolderPath))
                {
                    var relFolderpath = Path.GetDirectoryName(fileOrFolderPath);
                    _active.ReloadOverrideFile(fileOrFolderPath);
                    _overrideWidget?.SetResources(_active.OverrideResources(relFolderpath ?? ""));
                }
                else if (Directory.Exists(fileOrFolderPath))
                {
                    _active.LoadOverride(fileOrFolder);
                    _overrideWidget?.SetResources(_active.OverrideResources(fileOrFolder));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to reload override '{fileOrFolder}': {ex}");
            }
        }

        public void OnOverrideRefresh()
        {
            RefreshOverrideList(reload: true);
        }

        public void OnSavePathChanged(string newSaveDir)
        {
            if (_active == null || string.IsNullOrWhiteSpace(newSaveDir))
            {
                return;
            }

            try
            {
                // Clear the saves widget model (matching PyKotor: self.ui.savesWidget.modules_model.invisibleRootItem().removeRows(...))
                if (_savesWidget != null)
                {
                    _savesWidget.SetResources(new List<FileResource>());
                }

                // Load saves for the selected directory: get all FileResources under this save location from installation
                var saves = _active.Saves;
                if (saves != null && saves.ContainsKey(newSaveDir))
                {
                    var saveDict = saves[newSaveDir];
                    var allResources = new List<FileResource>();
                    if (saveDict != null)
                    {
                        foreach (var list in saveDict.Values)
                        {
                            if (list != null)
                                allResources.AddRange(list);
                        }
                    }
                    if (_savesWidget != null && allResources.Count > 0)
                    {
                        _savesWidget.SetResources(allResources, customCategory: "Saves", clearExisting: true);
                    }
                }

                RefreshSavesList(reload: true);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to change save path to '{newSaveDir}': {ex}");
            }
        }

        public void OnSaveReload(string saveDir)
        {
            if (string.IsNullOrWhiteSpace(saveDir))
            {
                return;
            }

            System.Console.WriteLine($"Reloading save directory '{saveDir}'");
            // In PyKotor, this just calls on_savepath_changed
            OnSavePathChanged(saveDir);
        }

        private void OnTexturesChanged(string texturepackName)
        {
            if (_active == null)
            {
                return;
            }
            _texturesWidget?.SetResources(_active.TexturepackResources(texturepackName));
        }

        public void OnSaveRefresh()
        {
            RefreshSavesList(reload: true);
        }

        public void OnModuleFileUpdated(string filePath, string eventType)
        {
            if (_active == null || string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            if (eventType == "deleted")
            {
                RefreshModuleList(reload: true);
            }
            else if (eventType == "modified")
            {
                OnModuleReload(filePath);
            }
        }

        public void OnOverrideFileUpdated(string filePath, string eventType)
        {
            if (_active == null || string.IsNullOrWhiteSpace(filePath))
            {
                return;
            }

            if (eventType == "deleted")
            {
                RefreshOverrideList(reload: true);
            }
            else if (eventType == "modified")
            {
                OnOverrideReload(filePath);
            }
        }

        private async void OpenActiveTalktable()
        {
            if (_active == null)
            {
                return;
            }

            var tlkPath = Path.Combine(_active.Path, "dialog.tlk");
            if (!File.Exists(tlkPath))
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard(
                    "dialog.tlk not found",
                    $"Could not open the TalkTable editor, dialog.tlk not found at the expected location\n\n{tlkPath}.",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Info);
                await messageBox.ShowAsync();
                return;
            }

            var fileInfo = new FileInfo(tlkPath);
            var resource = new FileResource("dialog", ResourceType.TLK, (int)fileInfo.Length, 0, tlkPath);
            WindowUtils.OpenResourceEditor(resource, _active, this);
        }

        private async void OpenActiveJournal()
        {
            if (_active == null)
            {
                return;
            }

            // Search for global.jrl in OVERRIDE and CHITIN locations
            var jrlIdent = new ResourceIdentifier("global", ResourceType.JRL);
            var journalResources = _active.Locations(
                new List<ResourceIdentifier> { jrlIdent },
                new[] { SearchLocation.OVERRIDE, SearchLocation.CHITIN });

            if (journalResources == null || !journalResources.ContainsKey(jrlIdent) || journalResources[jrlIdent].Count == 0)
            {
                var messageBox = MessageBoxManager.GetMessageBoxStandard(
                    "global.jrl not found",
                    "Could not open the journal editor: 'global.jrl' not found.",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                await messageBox.ShowAsync();
                return;
            }

            var relevant = journalResources[jrlIdent];
            if (relevant.Count > 1)
            {
                // Multiple journal files found - show FileSelectionWindow for user selection
                var selectionWindow = new Dialogs.FileSelectionWindow(relevant, _active, this);
                selectionWindow.Show();
                WindowUtils.AddWindow(selectionWindow);
                return;
            }

            // Get the first (or only) journal location result
            var locationResult = relevant[0];

            // Ensure FileResource is set on LocationResult
            FileResource fileResource = locationResult.FileResource;
            if (fileResource == null)
            {
                // Create FileResource from LocationResult
                if (!File.Exists(locationResult.FilePath))
                {
                    System.Console.WriteLine($"Journal file not found at path: {locationResult.FilePath}");
                    return;
                }

                var fileInfo = new FileInfo(locationResult.FilePath);
                fileResource = new FileResource(
                    jrlIdent.ResName,
                    jrlIdent.ResType,
                    (int)fileInfo.Length,
                    locationResult.Offset,
                    locationResult.FilePath);
                locationResult.SetFileResource(fileResource);
            }

            // Open the journal editor with the resource
            WindowUtils.OpenResourceEditor(
                fileResource,
                _active,
                this);
        }

        private void OpenFileSearchDialog()
        {
            if (_active == null)
            {
                return;
            }

            //           dialog.file_results.connect(self.on_file_search_results)
            //           dialog.exec()
            var dialog = new Dialogs.FileSearcherDialog(this, _installations);

            // Connect file results event
            dialog.FileResults += (results, installation) =>
            {
                //           dialog = FileResults(self, results, installation)
                //           dialog.sig_searchresults_selected.connect(self.on_open_resources)
                //           dialog.exec()
                var resultsDialog = new Dialogs.FileResultsDialog(this, results, installation);
                resultsDialog.SearchResultsSelected += (resource) =>
                {
                    // Open the selected resource
                    if (resource != null)
                    {
                        OnOpenResources(new List<FileResource> { resource });
                    }
                };
                resultsDialog.Show();
                WindowUtils.AddWindow(resultsDialog);
            };

            dialog.Show();
            WindowUtils.AddWindow(dialog);
        }

        private void OpenModuleTabErfEditor()
        {
            if (_active == null)
            {
                return;
            }

            ResourceList reslist = GetActiveResourceWidget();
            if (reslist == null || reslist != _modulesWidget)
            {
                return;
            }

            // Get the selected module filename from the section combo
            string filename = null;
            if (reslist.Ui?.SectionCombo != null && reslist.Ui.SectionCombo.SelectedItem != null)
            {
                filename = reslist.Ui.SectionCombo.SelectedItem.ToString();
            }

            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            // Construct the full path to the module file
            string modulePath = _active.ModulePath();
            string erfFilepath = Path.Combine(modulePath, filename);
            if (!File.Exists(erfFilepath))
            {
                return;
            }

            // Create ResourceIdentifier from path
            var resIdent = ResourceIdentifier.FromPath(erfFilepath);
            if (resIdent.ResType == null)
            {
                return;
            }

            // Create FileResource for the module file
            var fileInfo = new FileInfo(erfFilepath);
            var erfFileResource = new FileResource(
                resIdent.ResName,
                resIdent.ResType,
                (int)fileInfo.Length,
                0x0,
                erfFilepath);

            // Open the ERF editor
            WindowUtils.OpenResourceEditor(
                erfFileResource,
                _active,
                this,
                gffSpecialized: null);
        }

        private void OpenIndoorMapBuilder()
        {
            if (_active == null)
            {
                return;
            }

            var builder = new IndoorBuilderWindow(null, _active);
            builder.Show();
            WindowUtils.AddWindow(builder);
        }

        private void OpenKotordiff()
        {
            var kotordiffWindow = new KotorDiffWindow(null, _installations, _active);
            kotordiffWindow.Show();
            WindowUtils.AddWindow(kotordiffWindow);
        }

        private void OpenInstructionsWindow()
        {
            var window = new HelpWindow(null);
            window.Show();
            WindowUtils.AddWindow(window);
        }

        private void OpenAboutDialog()
        {
            var dialog = new Dialogs.AboutDialog(this);
            dialog.ShowDialog(this);
        }

        private void OpenUrl(string url)
        {
            if (!string.IsNullOrWhiteSpace(url))
            {
                MiscUtils.OpenLink(url);
            }
        }

        private void PopulateLanguageMenu(MenuItem menuLanguage)
        {
            if (menuLanguage == null) return;
            menuLanguage.Items.Clear();
            var currentLang = _settings?.GetValue("Language", "en") ?? "en";
            var languages = new[] { ("English", "en") };
            foreach (var (displayName, value) in languages)
            {
                var header = string.Equals(currentLang, value, StringComparison.OrdinalIgnoreCase)
                    ? "✓ " + displayName
                    : displayName;
                var item = new MenuItem { Header = header };
                var langValue = value;
                item.Click += (s, e) =>
                {
                    if (_settings != null)
                    {
                        _settings.SetValue("Language", langValue);
                    }
                };
                menuLanguage.Items.Add(item);
            }
        }

        private async void OpenSettingsDialog()
        {
            var dialog = new Dialogs.SettingsDialog(this);

            // In Avalonia, ShowDialog returns a result indicating if dialog was accepted
            var result = await dialog.ShowDialog<bool?>(this);

            if (result == true && dialog.InstallationEdited)
            {
                // Show message box asking if user wants to reload installations
                var messageBox = MessageBoxManager.GetMessageBoxStandard(
                    "Reload the installations?",
                    "You appear to have made changes to your installations, would you like to reload?",
                    ButtonEnum.YesNo,
                    MsBox.Avalonia.Enums.Icon.Question);

                var messageResult = await messageBox.ShowAsync();

                if (messageResult == ButtonResult.Yes)
                {
                    ReloadSettings();
                }
            }
        }

        private void OpenCloneModuleDialog()
        {
            if (_active == null)
            {
                return;
            }

            // Create installations dictionary with active installation
            var installations = new Dictionary<string, OdyInstallation>();
            if (_active != null)
            {
                installations[_active.Name] = _active;
            }
            // Add other installations if available
            foreach (var kvp in _installations)
            {
                if (!installations.ContainsKey(kvp.Key))
                {
                    installations[kvp.Key] = kvp.Value;
                }
            }

            var dialog = new Dialogs.CloneModuleDialog(this, _active, installations);
            dialog.ShowDialog(this);
        }

        public void OpenTslPatchDataEditor(string tslpatchdataPath = null)
        {
            var dialog = new TSLPatchDataEditorDialog(this, _active, tslpatchdataPath);
            dialog.Show();
        }

    }

    public class MainWindowUi
    {
        public ComboBox GameCombo { get; set; }
        public TabControl ResourceTabs { get; set; }
        public ResourceList CoreWidget { get; set; }
        public ResourceList ModulesWidget { get; set; }
        public ResourceList OverrideWidget { get; set; }
        public ResourceList SavesWidget { get; set; }
        public ResourceList TexturesWidget { get; set; }
        public TabItem CoreTab { get; set; }
        public TabItem SavesTab { get; set; }
        public TabItem ModulesTab { get; set; }
        public TabItem OverrideTab { get; set; }
        public MenuItem ActionNewDLG { get; set; }
        public MenuItem ActionNewUTC { get; set; }
        public MenuItem ActionNewNSS { get; set; }
    }
}
