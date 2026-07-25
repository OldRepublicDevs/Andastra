using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Platform.Storage;
using BioWare.Common;
using OdyTools.Data;
using OdyTools.Utils;
using JetBrains.Annotations;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using Avalonia;

namespace OdyTools.Editors
{
    public abstract class Editor : Window
    {
        protected sealed class SaveArtifact
        {
            public SaveArtifact(string path, byte[] data, bool createBackup, int maxBackups)
            {
                Path = path;
                Data = data;
                CreateBackup = createBackup;
                MaxBackups = maxBackups;
            }

            public string Path { get; }
            public byte[] Data { get; }
            public bool CreateBackup { get; }
            public int MaxBackups { get; }
        }

        protected const string CapsuleFilter = "*.mod *.erf *.rim *.sav";

        protected OdyInstallation _installation;
        protected string _editorTitle;
        protected string _filepath;
        protected string _resname;
        protected ResourceType _restype;
        protected byte[] _revert;
        protected bool _isSaveGameResource;
        private bool _dirty;
        private AutosaveService _autosaveService;
        protected ResourceType[] _readSupported;
        protected ResourceType[] _writeSupported;

        // Expose filepath for derived classes and testing
        protected string Filepath => _filepath;

        // Public property for testing
        public string FilepathPublic => _filepath;

        internal string ResourceName => _resname;

        internal ResourceType ResourceType => _restype;

        // Expose installation for widgets and derived classes
        internal OdyInstallation Installation => _installation;
        private readonly List<byte[]> _undoStack = new List<byte[]>();
        private readonly List<byte[]> _redoStack = new List<byte[]>();

        /// <summary>True when the document has unsaved changes.</summary>
        public bool IsDirty => _dirty;

        public bool IsSaveGameResource => _isSaveGameResource;

        /// <summary>Marks the document as modified (unsaved changes). Call when user edits content.</summary>
        protected void MarkDirty()
        {
            if (_dirty) return;
            _dirty = true;
            RefreshWindowTitle();
        }

        protected void MarkDocumentDirty()
        {
            MarkDirty();
            _autosaveService?.NotifyEdited();
        }

        /// <summary>Clears the dirty flag. Called after Load, Save, or New.</summary>
        protected void ClearDirty()
        {
            if (!_dirty) return;
            _dirty = false;
            RefreshWindowTitle();
        }

        private bool _exitMenuItemWired;
        /// <summary>True while closing after user answered the save-changes dialog; prevents Showing the dialog again on the subsequent Closing event.</summary>
        private bool _closingAfterSavePrompt;

        protected virtual bool IsAutosaveEnabled => GlobalSettings.ManagedAutosaveEnabled;
        protected virtual int AutosaveIntervalMinutes => GlobalSettings.ManagedAutosaveIntervalMinutes;
        protected virtual bool CreateBackupsOnSave => GlobalSettings.Instance.BackupsEnabled;
        protected virtual int BackupCount => GlobalSettings.Instance.MaxBackupCount;

        protected Editor(
            Window parent,
            string title,
            string iconName,
            ResourceType[] readSupported,
            ResourceType[] writeSupported,
            OdyInstallation installation = null)
        {
            _installation = installation;
            _editorTitle = title;
            Title = title;
            ApplyEditorWindowDefaults();
            _readSupported = readSupported ?? new ResourceType[0];
            _writeSupported = writeSupported ?? new ResourceType[0];

            SetupEditorFilters();
            RefreshWindowTitle();
            Opened += OnEditorOpened;
            Closing += OnEditorClosing;
        }

        private void ApplyEditorWindowDefaults()
        {
            // Shared defaults keep standalone and in-app editors consistent.
            MinWidth = 900;
            MinHeight = 620;
            Width = double.IsNaN(Width) || Width < 1200 ? 1200 : Width;
            Height = double.IsNaN(Height) || Height < 780 ? 780 : Height;
        }

        private async void OnEditorClosing(object sender, WindowClosingEventArgs e)
        {
            if (_closingAfterSavePrompt)
            {
                ClearAutosaveAndDisposeService();
                return;
            }

            if (!_dirty)
            {
                ClearAutosaveAndDisposeService();
                return;
            }

            if (_isHeadlessTest)
            {
                ClearAutosaveAndDisposeService();
                return; // Allow closing without blocking in tests
            }
            e.Cancel = true;
            var result = await ShowSaveChangesDialogAsync();
            if (result == SaveChangesResult.Save)
            {
                try { Save(); } catch { return; }
                ClearDirty();
                ClearAutosaveAndDisposeService();
                _closingAfterSavePrompt = true;
                Close();
            }
            else if (result == SaveChangesResult.DontSave)
            {
                ClearDirty();
                ClearAutosaveAndDisposeService();
                _closingAfterSavePrompt = true;
                Close();
            }
        }

        protected enum SaveChangesResult { Cancel, Save, DontSave }

        /// <summary>Shows "Do you want to save changes?" dialog. Returns Save, DontSave, or Cancel.</summary>
        protected async Task<SaveChangesResult> ShowSaveChangesDialogAsync()
        {
            string docName = !string.IsNullOrEmpty(_filepath) ? System.IO.Path.GetFileName(_filepath)
                : (!string.IsNullOrEmpty(_resname) && _restype != null ? $"{_resname}.{_restype.Extension}" : "Untitled");
            var result = await DialogHelper.ShowWindowAsync(
                this,
                "Unsaved changes",
                $"Do you want to save the changes you made to \"{docName}\"?",
                ButtonEnum.YesNoCancel,
                MsBox.Avalonia.Enums.Icon.Question);
            if (result == ButtonResult.Yes) return SaveChangesResult.Save;
            if (result == ButtonResult.No) return SaveChangesResult.DontSave;
            return SaveChangesResult.Cancel;
        }

        protected void ShowEditorMessage(string title, string message, MsBox.Avalonia.Enums.Icon icon)
        {
            DialogHelper.ShowWindow(this, title, message, icon);
        }

        protected void ShowOpenFailedUnsupportedTypeMessage()
        {
            ShowEditorMessage("Open Failed", "This editor cannot open this file type.", MsBox.Avalonia.Enums.Icon.Warning);
        }

        protected void ShowOpenFailedException(Exception ex)
        {
            ShowEditorMessage("Open Failed", "Could not open file: " + ex.Message, MsBox.Avalonia.Enums.Icon.Error);
        }

        protected void ShowSaveFailedException(Exception ex)
        {
            ShowEditorMessage("Error saving", "Could not save: " + ex.Message, MsBox.Avalonia.Enums.Icon.Error);
        }

        protected bool ShouldRestoreAutosave(string message)
        {
            if (_isHeadlessTest)
            {
                return false;
            }

            var result = DialogHelper.ShowAsync(
                "Autosave Found",
                message,
                ButtonEnum.YesNo,
                MsBox.Avalonia.Enums.Icon.Question).GetAwaiter().GetResult();
            return result == ButtonResult.Yes;
        }

        /// <summary>Returns true if we can discard (user chose Save or Don't Save). False if cancelled.</summary>
        protected async Task<bool> ConfirmDiscardUnsavedChangesAsync()
        {
            if (!_dirty) return true;
            var result = await ShowSaveChangesDialogAsync();
            if (result == SaveChangesResult.Cancel) return false;
            if (result == SaveChangesResult.Save)
            {
                try { Save(); } catch { return false; }
            }
            ClearDirty();
            return true;
        }

        private bool _recentFilesMenuWired;

        private void OnEditorOpened(object sender, EventArgs e)
        {
            ApplyGenericInstallationSettingsOnOpen();
            EnsureExitMenuItemWired();
            EnsureFileMenuWithRecentFiles();
            EnsureStandardFileMenuActionsWired();
            EnsureSettingsMenuWired();
            EnsureHelpActionWired();
            EnsureAutosaveService();
        }

        private void ApplyGenericInstallationSettingsOnOpen()
        {
            if (!string.IsNullOrEmpty(SettingsMenuActionName))
            {
                return;
            }

            if (_installation != null)
            {
                RefreshWindowTitle();
                OnInstallationChanged();
                return;
            }

            ApplyInstallationFromSettings(new GenericEditorInstallationSettings(_editorTitle));
        }

        private void EnsureAutosaveService()
        {
            if (!IsAutosaveEnabled || _autosaveService != null)
            {
                return;
            }

            if (_isHeadlessTest) return; // Do not start background polling timers during headless tests.

            _autosaveService = new AutosaveService(this, AutosaveIntervalMinutes);
            _autosaveService.Start();
        }

        private void ClearAutosaveAndDisposeService()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(_filepath))
                {
                    AtomicFileWriter.DeleteAutosaveFor(_filepath);
                }
            }
            catch
            {
                // Intentionally ignored.
            }

            try
            {
                _autosaveService?.Dispose();
                _autosaveService = null;
            }
            catch
            {
                // Intentionally ignored.
            }
        }

        // Evaluated once to reliably detect if we are running in headless test mode, avoiding UI dialog deadlocks
        private static readonly bool _isTestRun = AppDomain.CurrentDomain.GetAssemblies().Any(a =>
            a.FullName?.StartsWith("nunit.framework", StringComparison.OrdinalIgnoreCase) == true ||
            a.FullName?.StartsWith("testhost", StringComparison.OrdinalIgnoreCase) == true);
        private static readonly bool _isHeadlessTest = _isTestRun || (Avalonia.Application.Current?.ApplicationLifetime == null ||
            Avalonia.Application.Current.ApplicationLifetime.GetType().Name.Contains("Headless"));

        /// <summary>Override to false when the editor has fully custom File menu handling (e.g. ERF, SAV).</summary>
        protected virtual bool UseStandardFileMenuWiring => true;

        private bool _standardFileMenuWired;
        private void EnsureStandardFileMenuActionsWired()
        {
            if (!UseStandardFileMenuWiring || _standardFileMenuWired) return;
            _standardFileMenuWired = true;
            WireFileAction("actionNew", async () =>
            {
                if (await ConfirmDiscardUnsavedChangesAsync()) New();
            });
            WireFileAction("actionOpen", () => _ = RunOpenAsync());
            WireFileAction("actionSave", () => Save());
            WireFileAction("actionSaveAs", () => _ = RunSaveAsAsync());
            WireFileAction("actionSave_As", () => _ = RunSaveAsAsync()); // GFF uses this name
            WireFileAction("actionRevert", () => _ = RevertAsync());
        }

        private void WireFileAction(string name, Action handler)
        {
            var item = FindControlSafe<MenuItem>(name);
            if (item != null)
                item.Click += (s, e) => handler();
        }

        private void WireFileAction(string name, Func<Task> asyncHandler)
        {
            var item = FindControlSafe<MenuItem>(name);
            if (item != null)
                item.Click += (s, e) => _ = asyncHandler();
        }

        /// <summary>Override to return the name of the Settings menu action (e.g. "actionDLGSettings") to wire it to ShowSettingsDialogAsync.</summary>
        protected virtual string SettingsMenuActionName => null;

        /// <summary>Opens the editor's settings dialog. Override in subclasses that have settings (e.g. DLG).</summary>
        protected virtual async Task ShowSettingsDialogAsync()
        {
            var settings = new GenericEditorInstallationSettings(_editorTitle);
            var dialog = new GenericEditorInstallationSettingsDialog(_editorTitle, settings);
            await dialog.ShowDialog(this);
            if (dialog.Result == true)
            {
                ApplyInstallationFromSettings(settings);
            }
        }

        protected virtual void OnInstallationChanged()
        {
        }

        internal void SetStandaloneInstallation(OdyInstallation installation)
        {
            _installation = installation;
            RefreshWindowTitle();
            OnInstallationChanged();
        }

        /// <summary>
        /// Resolves and sets _installation from IEditorInstallationSettings.
        /// When UseInstallation is false, sets _installation to null.
        /// When UseInstallation is true but SelectedInstallationName is empty, preserves current _installation (e.g. passed from main app).
        /// When UseInstallation is true and SelectedInstallationName is set, creates OdyInstallation from GlobalSettings.Installations.
        /// Call from constructor/Opened and after settings dialog OK. Refreshes window title when installation changes.
        /// </summary>
        protected void ApplyInstallationFromSettings(IEditorInstallationSettings settings)
        {
            if (settings == null) return;
            if (!settings.UseInstallation(true))
            {
                _installation = null;
                RefreshWindowTitle();
                OnInstallationChanged();
                return;
            }
            string name = settings.SelectedInstallationName("")?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                return;
            }
            try
            {
                var installations = new GlobalSettings().Installations();
                if (installations == null || !installations.ContainsKey(name))
                {
                    _installation = null;
                    RefreshWindowTitle();
                    OnInstallationChanged();
                    return;
                }
                var installData = installations[name];
                string path = installData != null && installData.ContainsKey("path") ? installData["path"]?.ToString()?.Trim() : null;
                bool tsl = installData != null && installData.ContainsKey("tsl") && installData["tsl"] is bool tslVal && tslVal;
                if (string.IsNullOrEmpty(path) || !System.IO.Directory.Exists(path))
                {
                    _installation = null;
                    RefreshWindowTitle();
                    OnInstallationChanged();
                    return;
                }
                _installation = new OdyInstallation(path, name, tsl);
                RefreshWindowTitle();
                OnInstallationChanged();
            }
            catch
            {
                _installation = null;
                RefreshWindowTitle();
                OnInstallationChanged();
            }
        }

        private bool _settingsMenuWired;

        private void EnsureSettingsMenuWired()
        {
            if (_settingsMenuWired) return;
            _settingsMenuWired = true;

            if (!string.IsNullOrEmpty(SettingsMenuActionName))
            {
                WireFileAction(SettingsMenuActionName, () => ShowSettingsDialogAsync());
                return;
            }

            var settingsItem = GetOrCreateGenericSettingsMenuItem();
            if (settingsItem != null)
            {
                settingsItem.Click += (s, e) => _ = ShowSettingsDialogAsync();
            }
        }

        private MenuItem GetOrCreateGenericSettingsMenuItem()
        {
            var fileMenu = FindFileMenuItem();
            if (fileMenu == null) return null;

            foreach (var item in fileMenu.Items)
            {
                if (item is MenuItem existing && IsSettingsHeader(existing.Header))
                    return existing;
            }

            var settingsItem = new MenuItem { Header = "_Settings...", Name = "actionSettings" };
            int exitIndex = -1;
            for (int i = 0; i < fileMenu.Items.Count; i++)
            {
                if (fileMenu.Items[i] is MenuItem m && IsExitHeader(m.Header))
                {
                    exitIndex = i;
                    break;
                }
            }

            if (exitIndex >= 0)
            {
                fileMenu.Items.Insert(exitIndex, new Separator());
                fileMenu.Items.Insert(exitIndex, settingsItem);
            }
            else
            {
                fileMenu.Items.Add(new Separator());
                fileMenu.Items.Add(settingsItem);
            }

            return settingsItem;
        }

        private static bool IsSettingsHeader(object header)
        {
            if (header == null) return false;
            var s = header.ToString()?.Replace("_", "").Replace(".", "").Trim();
            return string.Equals(s, "Settings", StringComparison.OrdinalIgnoreCase)
                || s.EndsWith(" Settings", StringComparison.OrdinalIgnoreCase);
        }

        private T FindControlSafe<T>(string name) where T : Control
        {
            return EditorHelpers.FindControlSafe<T>(this, name);
        }

        private void EnsureExitMenuItemWired()
        {
            if (_exitMenuItemWired) return;
            _exitMenuItemWired = true;
            var exitItem = FindExitMenuItem();
            if (exitItem != null)
            {
                exitItem.Click += (s, ev) => Close();
                return;
            }
            AddExitMenuBar();
        }

        private void AddExitMenuBar()
        {
            try
            {
                var exitItem = new MenuItem { Header = "E_xit" };
                exitItem.Click += (s, ev) => Close();
                var fileMenu = new MenuItem { Header = "_File" };
                fileMenu.Items.Add(exitItem);
                var menu = new Menu();
                menu.Items.Add(fileMenu);
                var dock = new DockPanel();
                DockPanel.SetDock(menu, Dock.Top);
                dock.Children.Add(menu);

                Control ctrl = Content as Control;
                if (ctrl != null)
                {
                    Content = null;
                    dock.Children.Add(ctrl);
                }

                Content = dock;
            }
            catch
            {
                // Safety catch for edge cases during window initialization.
            }
        }

        private MenuItem FindExitMenuItem()
        {
            foreach (var menu in FindControls<Menu>(this))
            {
                foreach (var item in menu.Items)
                {
                    if (item is MenuItem topItem)
                    {
                        foreach (var sub in topItem.Items)
                        {
                            if (sub is MenuItem subItem && IsExitHeader(subItem.Header))
                                return subItem;
                        }
                    }
                }
            }
            return null;
        }

        private static bool IsExitHeader(object header)
        {
            if (header == null) return false;
            var s = header.ToString()?.Replace("_", "").Trim();
            return string.Equals(s, "Exit", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Finds or creates the Recent Files submenu in the File menu. Automatically called on editor open.</summary>
        private void EnsureFileMenuWithRecentFiles()
        {
            if (_recentFilesMenuWired) return;
            var fileMenu = FindFileMenuItem();
            if (fileMenu == null) return;
            var recentSubmenu = GetOrCreateRecentFilesSubmenu(fileMenu);
            if (recentSubmenu == null) return;
            _recentFilesMenuWired = true;
        }

        private MenuItem FindFileMenuItem()
        {
            foreach (var menu in FindControls<Menu>(this))
            {
                foreach (var item in menu.Items)
                {
                    if (item is MenuItem mi && IsFileMenuHeader(mi.Header))
                        return mi;
                }
            }
            return null;
        }

        private static bool IsFileMenuHeader(object header)
        {
            if (header == null) return false;
            var s = header.ToString()?.Replace("_", "").Trim();
            return string.Equals(s, "File", StringComparison.OrdinalIgnoreCase);
        }

        private MenuItem GetOrCreateRecentFilesSubmenu(MenuItem fileMenu)
        {
            // Check if Recent Files submenu already exists (by name or header)
            foreach (var item in fileMenu.Items)
            {
                if (item is MenuItem sub && IsRecentFilesHeader(sub.Header))
                {
                    sub.SubmenuOpened += (s, e) => PopulateRecentFilesMenu(sub);
                    PopulateRecentFilesMenu(sub);
                    return sub;
                }
            }
            // Find index of Exit item - insert Recent Files + separator before it
            int exitIndex = -1;
            for (int i = 0; i < fileMenu.Items.Count; i++)
            {
                if (fileMenu.Items[i] is MenuItem m && IsExitHeader(m.Header))
                {
                    exitIndex = i;
                    break;
                }
            }
            var recentItem = new MenuItem { Header = "_Recent Files" };
            recentItem.SubmenuOpened += (s, e) => PopulateRecentFilesMenu(recentItem);
            PopulateRecentFilesMenu(recentItem); // Initial populate so submenu shows items
            if (exitIndex >= 0)
            {
                fileMenu.Items.Insert(exitIndex, new Separator());
                fileMenu.Items.Insert(exitIndex, recentItem);
            }
            else
            {
                fileMenu.Items.Add(new Separator());
                fileMenu.Items.Add(recentItem);
            }
            return recentItem;
        }

        private static bool IsRecentFilesHeader(object header)
        {
            if (header == null) return false;
            var s = header.ToString()?.Replace("_", "").Trim();
            return string.Equals(s, "Recent Files", StringComparison.OrdinalIgnoreCase);
        }

        private void PopulateRecentFilesMenu(MenuItem menuRecentFiles)
        {
            if (menuRecentFiles == null) return;
            menuRecentFiles.Items.Clear();
            var recentPaths = GetRecentFilesFilteredForEditor();
            foreach (var path in recentPaths)
            {
                var display = Path.GetFileName(path);
                if (string.IsNullOrEmpty(display)) display = path;
                var item = new MenuItem { Header = display };
                ToolTip.SetTip(item, path);
                var captured = path;
                item.Click += (s, e) => _ = OpenRecentFileAsync(captured);
                menuRecentFiles.Items.Add(item);
            }
            if (recentPaths.Count == 0)
            {
                menuRecentFiles.Items.Add(new MenuItem { Header = "(No recent files)", IsEnabled = false });
            }
        }

        private List<string> GetRecentFilesFilteredForEditor()
        {
            var settings = new Settings("Global");
            var all = settings.GetValue("RecentFiles", new List<string>());
            var supported = new List<string>();
            foreach (var fp in all)
            {
                if (string.IsNullOrEmpty(fp) || !File.Exists(fp)) continue;
                if (IsPathSupportedByEditor(fp))
                    supported.Add(fp);
                if (supported.Count >= 15) break;
            }
            return supported;
        }

        /// <summary>Returns true if this editor can open the given file path (extension matches _readSupported).</summary>
        protected virtual bool IsPathSupportedByEditor(string filepath)
        {
            if (_readSupported == null || _readSupported.Length == 0) return false;
            return TryResolveReadIdentity(filepath, out _, out _);
        }

        public virtual bool CanLoadPath(string filepath)
        {
            return !string.IsNullOrWhiteSpace(filepath)
                && File.Exists(filepath)
                && IsPathSupportedByEditor(filepath);
        }

        protected virtual bool TryResolveReadIdentity(string path, out ResourceType restype, out string resname)
        {
            if (TryResolveResourceTypeFromPath(path, _readSupported, out restype, out resname))
            {
                return true;
            }

            return TryResolveSyntheticGffFormat(path, _readSupported, out restype, out resname);
        }

        protected virtual bool TryLoadFromPath(string path)
        {
            byte[] data = File.ReadAllBytes(path);
            if (!TryResolveReadIdentity(path, out var restype, out var resname))
            {
                return false;
            }

            if (restype == null)
            {
                return false;
            }

            Load(path, resname, restype, data);
            return true;
        }

        protected bool TryLoadPathWithMessages(string path)
        {
            try
            {
                if (!TryLoadFromPath(path))
                {
                    ShowOpenFailedUnsupportedTypeMessage();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                ShowOpenFailedException(ex);
                return false;
            }
        }

        public bool TryLoadStartupPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || (!File.Exists(path) && !Directory.Exists(path)))
            {
                return false;
            }

            return TryLoadPathWithMessages(path);
        }

        /// <summary>Opens a recent file in this editor. Called when user selects from Recent Files menu. Override to customize.</summary>
        protected virtual async Task OpenRecentFileAsync(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath) || !File.Exists(filepath))
                return;
            if (!await ConfirmDiscardUnsavedChangesAsync()) return;
            _ = TryLoadPathWithMessages(filepath);
        }

        protected void SetupEditorFilters()
        {
            // Setup file filters for open/save dialogs
            // Add format variants (XML, JSON, CSV, ASCII, YAML) for each base resource type
            var additionalFormats = new[] { "XML", "JSON", "CSV", "ASCII", "YAML" };
            var readList = _readSupported.ToList();
            var writeList = _writeSupported.ToList();

            // Add format variants for read supported types
            // For each base type, look for variants like {FieldName}_XML, {FieldName}_JSON, etc.
            var readVariants = new List<ResourceType>();
            foreach (var restype in _readSupported)
            {
                string fieldName = restype.GetFieldName();
                if (string.IsNullOrEmpty(fieldName))
                {
                    continue;
                }

                foreach (var addFormat in additionalFormats)
                {
                    string variantFieldName = $"{fieldName}_{addFormat}";
                    ResourceType variant = ResourceType.FromName(variantFieldName);
                    if (variant != null && !variant.IsInvalid)
                    {
                        readVariants.Add(variant);
                    }
                }
            }
            readList.AddRange(readVariants);

            // Add format variants for write supported types
            // For each base type, look for variants like {FieldName}_XML, {FieldName}_JSON, etc.
            var writeVariants = new List<ResourceType>();
            foreach (var restype in _writeSupported)
            {
                string fieldName = restype.GetFieldName();
                if (string.IsNullOrEmpty(fieldName))
                {
                    continue;
                }

                foreach (var addFormat in additionalFormats)
                {
                    string variantFieldName = $"{fieldName}_{addFormat}";
                    ResourceType variant = ResourceType.FromName(variantFieldName);
                    if (variant != null && !variant.IsInvalid)
                    {
                        writeVariants.Add(variant);
                    }
                }
            }
            writeList.AddRange(writeVariants);

            _readSupported = readList.ToArray();
            _writeSupported = writeList.ToArray();
        }

        // Title format: (installation name)\relpath\to\filename - Editor(installation)
        // When the file is inside a container (BIF/RIM/ERF/MOD/SAV), show logical resource path (e.g. data\mainmenu8x6.gui) not the container path (gui.bif).
        protected void RefreshWindowTitle()
        {
            string installationName = _installation == null ? "No Installation" : _installation.Name;
            string baseTitle;
            if (string.IsNullOrEmpty(_filepath) || string.IsNullOrEmpty(_resname) || _restype == null)
            {
                baseTitle = $"{_editorTitle}({installationName})";
            }
            else
            {
                string displayPath = BuildWindowTitlePath();
                baseTitle = $"{displayPath} - {_editorTitle}({installationName})";
            }
            Title = _dirty ? baseTitle + " *" : baseTitle;
        }

        /// <summary>
        /// Builds the path segment for the window title: (installation name)\relpath\to\filename.
        /// For resources inside a container (BIF, RIM, etc.), uses the logical resource path (e.g. data\mainmenu8x6.gui) instead of the container path.
        /// </summary>
        private string BuildWindowTitlePath()
        {
            bool isContainer = IsContainerFile(_filepath);
            string installPath = _installation?.Path;
            bool underInstallation = _installation != null && !string.IsNullOrEmpty(installPath) &&
                Path.GetFullPath(_filepath).StartsWith(Path.GetFullPath(installPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar), StringComparison.OrdinalIgnoreCase);

            if (underInstallation)
            {
                string relativePart;
                if (isContainer && !string.IsNullOrEmpty(_resname) && _restype != null)
                {
                    string containerDir = Path.GetDirectoryName(_filepath);
                    relativePart = PathUtils.GetRelativePath(installPath, containerDir);
                    if (!string.IsNullOrEmpty(relativePart))
                        relativePart = relativePart.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar + _resname + "." + _restype.Extension;
                    else
                        relativePart = _resname + "." + _restype.Extension;
                }
                else
                {
                    relativePart = PathUtils.GetRelativePath(installPath, _filepath);
                }
                string installationName = _installation?.Name ?? "No Installation";
                return installationName + Path.DirectorySeparatorChar + relativePart;
            }

            if (isContainer && !string.IsNullOrEmpty(_resname) && _restype != null)
            {
                string containerDir = Path.GetDirectoryName(_filepath);
                return (containerDir ?? "") + Path.DirectorySeparatorChar + _resname + "." + _restype.Extension;
            }

            return _filepath;
        }

        private static bool IsContainerFile(string filepath)
        {
            if (string.IsNullOrEmpty(filepath)) return false;
            var ext = Path.GetExtension(filepath)?.ToLowerInvariant() ?? "";
            return ext == ".bif" || ext == ".rim" || ext == ".erf" || ext == ".mod" || ext == ".sav";
        }

        /// <summary>Save As entry point. Default implementation calls RunSaveAsAsync. Override for custom behavior.</summary>
        public virtual void SaveAs() => _ = RunSaveAsAsync();

        public virtual void Save()
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                SaveAs();
                return;
            }

            try
            {
                if (!TrySaveToPath(_filepath, out var primaryData))
                {
                    return;
                }

                _revert = primaryData;
                ClearDirty();
                _autosaveService?.ClearForCurrentFile();
            }
            catch (Exception ex)
            {
                ShowSaveFailedException(ex);
                throw;
            }
        }

        public virtual void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            if (!string.IsNullOrWhiteSpace(filepath)
                && IsAutosaveEnabled
                && AtomicFileWriter.TryReadAutosaveIfNewer(filepath, out var autosaveData, out var autosaveWriteUtc, out var fileWriteUtc)
                && autosaveData != null)
            {
                string message =
                    $"A newer autosave was found for this file.\n\nAutosave: {autosaveWriteUtc:u}\nFile: {fileWriteUtc:u}\n\nRestore autosave content?";
                try
                {
                    if (ShouldRestoreAutosave(message))
                    {
                        data = autosaveData;
                    }
                }
                catch
                {
                    // Fallback to on-disk content when prompt fails.
                }
            }

            _filepath = filepath;
            _resname = resref;
            _restype = restype;
            _isSaveGameResource = IsSaveGameResourcePath(filepath);
            _revert = data;
            ClearDirty();
            RefreshWindowTitle();
            AddToRecentFilesWhenLoaded(filepath);
        }

        public static bool IsSaveGameResourcePath(string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath))
            {
                return false;
            }

            var parts = filepath.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                if (string.Equals(part, "SAVEGAME.sav", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                if (i < parts.Length - 1 && string.Equals(Path.GetExtension(part), ".sav", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>Adds the file to recent files list when Load is called with a valid file path. Override to disable or customize.</summary>
        protected virtual void AddToRecentFilesWhenLoaded(string filepath)
        {
            if (!string.IsNullOrEmpty(filepath) && File.Exists(filepath))
                WindowUtils.AddRecentFile(filepath);
        }

        public virtual void New()
        {
            _filepath = null;
            _resname = null;
            _restype = null;
            _revert = null;
            ClearDirty();
            RefreshWindowTitle();
        }

        /// <summary>Reverts the document to the last saved state. Override for custom revert logic (e.g. clearing undo stack).</summary>
        public virtual void Revert()
        {
            if (_revert == null || string.IsNullOrEmpty(_filepath)) return;
            Load(_filepath, _resname, _restype, _revert);
        }

        /// <summary>Confirms discard of unsaved changes, then calls Revert. Used by File > Revert.</summary>
        protected virtual async Task RevertAsync()
        {
            if (_revert == null || string.IsNullOrEmpty(_filepath)) return;
            if (!await ConfirmDiscardUnsavedChangesAsync()) return;
            Revert();
        }

        /// <summary>Save As: file picker, build, write, reload. Override for custom Save As behavior (e.g. format options).</summary>
        protected virtual async Task RunSaveAsAsync()
        {
            if (!await ConfirmDiscardUnsavedChangesAsync()) return;
            var storage = StorageProvider;
            if (storage == null) return;
            var options = CreateSaveAsOptions();
            var file = await storage.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path)) return;
            try
            {
                if (!TrySaveToPath(path, out var primaryData)) return;
                if (TryResolveSaveIdentity(path, out var resname, out var restype) && restype != null)
                {
                    _revert = primaryData;
                    ClearDirty();
                    AtomicFileWriter.DeleteAutosaveFor(path);
                    Load(path, resname, restype, primaryData);
                }
            }
            catch (Exception ex)
            {
                ShowSaveFailedException(ex);
            }
        }

        protected virtual bool TryResolveSaveIdentity(string path, out string resname, out ResourceType restype)
        {
            if (TryResolveResourceTypeFromPath(path, _writeSupported, out restype, out resname))
            {
                return true;
            }

            if (TryResolveSyntheticGffFormat(path, _writeSupported, out restype, out resname))
            {
                return true;
            }

            resname = Path.GetFileNameWithoutExtension(path);
            restype = null;
            return false;
        }

        protected virtual FilePickerSaveOptions CreateSaveAsOptions()
        {
            var patterns = BuildSavePatterns(_writeSupported);
            if (patterns.Count == 0) patterns.Add("*.*");

            var filter = new List<FilePickerFileType>
            {
                new FilePickerFileType("Supported") { Patterns = patterns },
                new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
            };

            string suggested = string.IsNullOrEmpty(_resname) ? "file" : _resname;
            string ext = _restype?.Extension ?? patterns.FirstOrDefault()?.TrimStart('*') ?? ".bin";
            if (!ext.StartsWith(".")) ext = "." + ext;

            return new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggested + ext,
                FileTypeChoices = filter
            };
        }

        private static string RemoveKnownSuffix(string fileName, string extension)
        {
            if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(extension))
            {
                return fileName;
            }

            string suffix = "." + extension;
            return fileName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
                ? fileName.Substring(0, fileName.Length - suffix.Length)
                : fileName;
        }

        private static bool TryResolveResourceTypeFromPath(string path, IEnumerable<ResourceType> supportedTypes, out ResourceType restype, out string resname)
        {
            restype = null;
            resname = Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrWhiteSpace(path) || supportedTypes == null)
            {
                return false;
            }

            string fileName = Path.GetFileName(path);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            foreach (var candidate in supportedTypes
                         .Where(r => r != null && !string.IsNullOrWhiteSpace(r.Extension))
                         .Distinct()
                         .OrderByDescending(r => r.Extension.Length))
            {
                string suffix = "." + candidate.Extension;
                if (!fileName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                restype = candidate;
                resname = RemoveKnownSuffix(fileName, candidate.Extension);
                return true;
            }

            return false;
        }

        private static List<string> BuildSavePatterns(IEnumerable<ResourceType> supportedTypes)
        {
            var patterns = supportedTypes != null
                ? supportedTypes.Where(r => r != null && !string.IsNullOrEmpty(r.Extension)).Select(r => "*." + r.Extension).Distinct().ToList()
                : new List<string>();

            if (supportedTypes != null)
            {
                var baseGffTypes = supportedTypes
                    .Where(r => r != null)
                    .Select(r => r.TargetType())
                    .Where(t => t != null && !t.IsInvalid && t.IsGff() && !string.IsNullOrWhiteSpace(t.Extension))
                    .Distinct()
                    .ToList();

                foreach (var gffType in baseGffTypes)
                {
                    patterns.Add("*." + gffType.Extension + ".xml");
                    patterns.Add("*." + gffType.Extension + ".json");
                }
            }

            return patterns.Distinct().ToList();
        }

        private static bool TryResolveSyntheticGffFormat(string path, IEnumerable<ResourceType> supportedTypes, out ResourceType restype, out string resname)
        {
            restype = null;
            resname = Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrWhiteSpace(path) || supportedTypes == null)
            {
                return false;
            }

            string fileName = Path.GetFileName(path);
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return false;
            }

            string format = null;
            if (fileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                format = "XML";
            }
            else if (fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                format = "JSON";
            }

            if (format == null)
            {
                return false;
            }

            var baseGffTypes = supportedTypes
                .Where(r => r != null)
                .Select(r => r.TargetType())
                .Where(t => t != null && !t.IsInvalid && t.IsGff() && !string.IsNullOrWhiteSpace(t.Extension))
                .Distinct()
                .OrderByDescending(t => t.Extension.Length)
                .ToList();

            foreach (var baseType in baseGffTypes)
            {
                string suffix = "." + baseType.Extension + "." + format.ToLowerInvariant();
                if (!fileName.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                string variantName = baseType.GetFieldName() + "_" + format;
                ResourceType variant = ResourceType.FromName(variantName);
                restype = (variant != null && !variant.IsInvalid)
                    ? variant
                    : (format == "XML" ? ResourceType.GFF_XML : ResourceType.GFF_JSON);
                resname = fileName.Substring(0, fileName.Length - suffix.Length);
                return true;
            }

            return false;
        }

        protected virtual IReadOnlyList<SaveArtifact> BuildSaveArtifactsForPath(string path)
        {
            var (data, _) = Build();
            if (data == null)
            {
                return Array.Empty<SaveArtifact>();
            }

            return new[]
            {
                new SaveArtifact(path, data, CreateBackupsOnSave, Math.Max(1, BackupCount))
            };
        }

        protected virtual void PersistSaveArtifacts(IReadOnlyList<SaveArtifact> artifacts)
        {
            if (artifacts == null || artifacts.Count == 0)
            {
                return;
            }

            foreach (var artifact in artifacts)
            {
                if (artifact == null || string.IsNullOrWhiteSpace(artifact.Path) || artifact.Data == null)
                {
                    continue;
                }

                AtomicFileWriter.WriteAtomic(artifact.Path, artifact.Data, new AtomicWriteOptions
                {
                    CreateBackup = artifact.CreateBackup,
                    MaxBackups = Math.Max(1, artifact.MaxBackups)
                });
            }
        }

        protected bool TrySaveToPath(string path, out byte[] primaryData)
        {
            primaryData = null;
            var artifacts = BuildSaveArtifactsForPath(path);
            if (artifacts == null || artifacts.Count == 0)
            {
                return false;
            }

            var primary = artifacts[0];
            if (primary == null || primary.Data == null)
            {
                return false;
            }

            PersistSaveArtifacts(artifacts);
            primaryData = primary.Data;
            return true;
        }

        /// <summary>
        /// Opens a file picker, reads the selected file, and loads it via Load(filepath, resname, restype, data).
        /// Uses _readSupported to build the file type filter. Override for custom open flow (e.g. DLG module browser).
        /// </summary>
        protected virtual async Task RunOpenAsync()
        {
            if (!await ConfirmDiscardUnsavedChangesAsync()) return;
            var storage = StorageProvider;
            if (storage == null) return;
            var patterns = _readSupported != null
                ? _readSupported.Where(r => r != null && !string.IsNullOrEmpty(r.Extension)).Select(r => "*." + r.Extension).Distinct().ToList()
                : new List<string>();
            if (patterns.Count == 0) patterns.Add("*.*");
            var filter = new List<FilePickerFileType>
            {
                new FilePickerFileType("Supported") { Patterns = patterns },
                new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
            };
            var options = new FilePickerOpenOptions
            {
                Title = "Open",
                AllowMultiple = false,
                FileTypeFilter = filter
            };
            var files = await storage.OpenFilePickerAsync(options);
            if (files == null || files.Count == 0) return;
            var f = files[0];
            string path = f.Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return;
            _ = TryLoadPathWithMessages(path);
        }

        public abstract Tuple<byte[], byte[]> Build();

        public string GetOpenedFileName()
        {
            if (!string.IsNullOrEmpty(_filepath) && !string.IsNullOrEmpty(_resname) && _restype != null)
            {
                return $"{_resname}.{_restype.Extension}";
            }
            return "";
        }

        /// <summary>
        /// Returns info needed for crash recovery. Used by EditorCrashRecoveryService.
        /// </summary>
        public (string filepath, string resname, ResourceType restype) GetRecoveryInfo()
        {
            return (_filepath, _resname, _restype);
        }

        // Helper method for editors to safely initialize XAML
        protected bool TryLoadXaml()
        {
            try
            {
                Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>When editor has .axaml with ContentControl x:Name="contentRoot", injects content there; otherwise sets Window.Content.</summary>
        protected void SetContentOrInject(object content)
        {
            var root = EditorHelpers.FindControlSafe<ContentControl>(this, "contentRoot");
            if (root != null) root.Content = content; else Content = content;
        }

        /// <summary>
        /// Creates an expander-based form section (e.g. "Basic", "Advanced", "Comments") and adds it to the main panel.
        /// Use the returned Content panel to add labeled fields. Shared by UTS/UTD/UTE and other form-style editors.
        /// </summary>
        /// <param name="mainPanel">Parent vertical StackPanel (e.g. inside a ScrollViewer).</param>
        /// <param name="header">Section header text.</param>
        /// <param name="isExpanded">Whether the section is expanded by default.</param>
        /// <returns>The Expander and its content StackPanel; add child controls to Content.</returns>
        protected static (Expander Expander, StackPanel Content) CreateFormSection(StackPanel mainPanel, string header, bool isExpanded = true)
        {
            var expander = new Expander { Header = header, IsExpanded = isExpanded };
            var content = new StackPanel { Orientation = Orientation.Vertical };
            expander.Content = content;
            mainPanel.Children.Add(expander);
            return (expander, content);
        }

        public void AddHelpAction(string wikiFilename = null)
        {
            string[] wikiFilenames = null;

            // Auto-detect wiki files if not provided
            if (string.IsNullOrEmpty(wikiFilename))
            {
                string editorClassName = GetType().Name;
                wikiFilenames = EditorWikiMapping.GetWikiFiles(editorClassName);
                if (wikiFilenames == null || wikiFilenames.Length == 0)
                {
                    // No wiki files for this editor, skip adding help
                    return;
                }
            }
            else
            {
                // Single file provided, convert to array for consistency
                wikiFilenames = new[] { wikiFilename };
            }

            // Find or create Help menu item
            MenuItem helpMenuItem = FindHelpMenuItem();
            if (helpMenuItem == null)
            {
                helpMenuItem = CreateHelpMenuItem();
            }

            // Check if Documentation action already exists (idempotent)
            MenuItem docAction = FindDocumentationAction(helpMenuItem);
            if (docAction == null)
            {
                // Add help action with question mark icon
                docAction = new MenuItem
                {
                    Header = "Documentation"
                };
                docAction.Click += (sender, e) => ShowHelpDialog(wikiFilenames);

                // Add F1 shortcut
                var shortcut = new KeyGesture(Key.F1);
                docAction.HotKey = shortcut;

                helpMenuItem.Items.Add(docAction);
            }
        }

        private void EnsureHelpActionWired()
        {
            AddHelpAction();
        }

        public void ShowHelpDialog(string wikiFilename)
        {
            if (string.IsNullOrEmpty(wikiFilename))
            {
                return;
            }

            ShowHelpDialog(new[] { wikiFilename });
        }

        // Overload to support multiple wiki files
        public void ShowHelpDialog(string[] wikiFilenames)
        {
            if (wikiFilenames == null || wikiFilenames.Length == 0)
            {
                return;
            }

            // Create non-blocking dialog with multiple files
            var dialog = new Dialogs.EditorHelpDialog(this, wikiFilenames);
            dialog.Show(); // Non-blocking show
        }

        // Helper method to find Help menu item in the window
        private MenuItem FindHelpMenuItem()
        {
            // Search for Menu controls in the window
            var menus = FindControls<Menu>(this);
            foreach (var menu in menus)
            {
                // Check if this menu contains a Help item
                foreach (var item in menu.Items)
                {
                    if (item is MenuItem menuItem && IsHelpHeader(menuItem.Header))
                    {
                        return menuItem;
                    }
                }
            }
            return null;
        }

        // Helper method to create Help menu item
        private MenuItem CreateHelpMenuItem()
        {
            // Find the main menu bar
            var mainMenu = FindControls<Menu>(this).FirstOrDefault();
            if (mainMenu == null)
            {
                var existingContent = Content as Control;

                mainMenu = new Menu();

                var dockPanel = new DockPanel();
                dockPanel.Children.Add(mainMenu);
                DockPanel.SetDock(mainMenu, Dock.Top);

                if (existingContent != null)
                {
                    Content = null;
                    dockPanel.Children.Add(existingContent);
                }

                Content = dockPanel;
            }

            // Create Help menu item
            var helpMenuItem = new MenuItem { Header = "_Help", Name = "menuHelp" };
            mainMenu.Items.Add(helpMenuItem);
            return helpMenuItem;
        }

        private static bool IsHelpHeader(object header)
        {
            return string.Equals(NormalizeMenuHeader(header), "help", StringComparison.OrdinalIgnoreCase);
        }

        private static string NormalizeMenuHeader(object header)
        {
            return (header?.ToString() ?? string.Empty)
                .Replace("_", string.Empty)
                .Replace(".", string.Empty)
                .Trim();
        }

        // Helper method to find Documentation action in Help menu item
        private MenuItem FindDocumentationAction(MenuItem helpMenuItem)
        {
            if (helpMenuItem == null)
            {
                return null;
            }

            foreach (var item in helpMenuItem.Items)
            {
                if (item is MenuItem menuItem && menuItem.Header?.ToString() == "Documentation")
                {
                    return menuItem;
                }
            }
            return null;
        }

        // Helper method to find controls recursively
        private static IEnumerable<T> FindControls<T>(Control parent) where T : Control
        {
            var results = new List<T>();
            if (parent is T match)
            {
                results.Add(match);
            }

            if (parent is Panel panel)
            {
                foreach (var child in panel.Children)
                {
                    if (child is Control control)
                    {
                        results.AddRange(FindControls<T>(control));
                    }
                }
            }
            else if (parent is ContentControl contentControl && contentControl.Content is Control content)
            {
                results.AddRange(FindControls<T>(content));
            }

            return results;
        }

        private sealed class GenericEditorInstallationSettings : Settings, IEditorInstallationSettings
        {
            public GenericEditorInstallationSettings(string editorTitle)
                : base(string.IsNullOrWhiteSpace(editorTitle) ? "Editor" : editorTitle)
            {
            }

            public bool UseInstallation(bool defaultValue = true)
            {
                return GetValue("UseInstallation", defaultValue);
            }

            public string SelectedInstallationName(string defaultValue = "")
            {
                return GetValue("SelectedInstallationName", defaultValue) ?? defaultValue;
            }

            public void SetUseInstallation(bool value)
            {
                SetValue("UseInstallation", value);
            }

            public void SetSelectedInstallationName(string value)
            {
                SetValue("SelectedInstallationName", value ?? string.Empty);
            }
        }

        private sealed class GenericEditorInstallationSettingsDialog : OdyTools.Dialogs.EditorInstallationSettingsDialogBase
        {
            private readonly GenericEditorInstallationSettings _settings;
            private readonly CheckBox _useInstallationCheck;
            private readonly ComboBox _installationCombo;

            public GenericEditorInstallationSettingsDialog(string editorTitle, GenericEditorInstallationSettings settings)
            {
                _settings = settings ?? throw new ArgumentNullException(nameof(settings));
                Title = string.IsNullOrWhiteSpace(editorTitle) ? "Editor Settings" : editorTitle + " Settings";
                Width = 560;
                Height = 320;
                MinWidth = 480;
                MinHeight = 260;
                WindowStartupLocation = WindowStartupLocation.CenterOwner;

                var root = new DockPanel { Margin = new Thickness(14) };

                var buttons = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Spacing = 8,
                    Margin = new Thickness(0, 12, 0, 0)
                };
                var okButton = new Button { Content = "OK", Width = 88 };
                var cancelButton = new Button { Content = "Cancel", Width = 88 };
                buttons.Children.Add(okButton);
                buttons.Children.Add(cancelButton);
                DockPanel.SetDock(buttons, Dock.Bottom);
                root.Children.Add(buttons);

                var panel = new StackPanel { Orientation = Orientation.Vertical, Spacing = 10 };
                panel.Children.Add(new TextBlock
                {
                    Text = "Game installation",
                    FontWeight = Avalonia.Media.FontWeight.Bold
                });

                _useInstallationCheck = new CheckBox { Content = "Use a game installation for this editor" };
                panel.Children.Add(_useInstallationCheck);

                var comboRow = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*") };
                comboRow.Children.Add(new TextBlock
                {
                    Text = "Installation:",
                    VerticalAlignment = VerticalAlignment.Center,
                    Margin = new Thickness(0, 0, 8, 0)
                });
                _installationCombo = new ComboBox { MinWidth = 300 };
                Grid.SetColumn(_installationCombo, 1);
                comboRow.Children.Add(_installationCombo);
                panel.Children.Add(comboRow);

                panel.Children.Add(new TextBlock
                {
                    Text = "Choose an existing installation, auto-detect common paths, or add a new install path.",
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap
                });

                root.Children.Add(panel);
                Content = root;

                InitializeInstallationSection(_installationCombo, okButton, cancelButton);
                LoadValues();
            }

            private void LoadValues()
            {
                bool useInstallation = _settings.UseInstallation(true);
                _useInstallationCheck.IsChecked = useInstallation;

                string selectedName = _settings.SelectedInstallationName(string.Empty);
                if (!useInstallation || string.IsNullOrWhiteSpace(selectedName))
                {
                    _installationCombo.SelectedIndex = 0;
                    return;
                }

                int index = InstallationNames.FindIndex(n => string.Equals(n, selectedName, StringComparison.OrdinalIgnoreCase));
                _installationCombo.SelectedIndex = index >= 0 ? index + 1 : 0;
            }

            protected override void SaveValues()
            {
                bool useInstallation = _useInstallationCheck.IsChecked == true;
                _settings.SetUseInstallation(useInstallation);

                string selectedName = string.Empty;
                if (useInstallation && _installationCombo.SelectedIndex > 0 && _installationCombo.SelectedIndex <= InstallationNames.Count)
                {
                    selectedName = InstallationNames[_installationCombo.SelectedIndex - 1];
                }
                _settings.SetSelectedInstallationName(selectedName);
            }
        }
    }
}
