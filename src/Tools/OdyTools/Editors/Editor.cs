using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.VisualTree;
using Avalonia.Platform.Storage;
using BioWare.Common;
using OdyTools.Data;
using OdyTools.Utils;
using JetBrains.Annotations;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace OdyTools.Editors
{
    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor.py:291
    // Original: class Editor(QMainWindow):
    public abstract class Editor : Window
    {
        protected const string CapsuleFilter = "*.mod *.erf *.rim *.sav";

        protected OdyInstallation _installation;
        protected string _editorTitle;
        protected string _filepath;
        protected string _resname;
        protected ResourceType _restype;
        protected byte[] _revert;
        protected bool _isSaveGameResource;
        private bool _dirty;
        protected ResourceType[] _readSupported;
        protected ResourceType[] _writeSupported;

        // Expose filepath for derived classes and testing
        protected string Filepath => _filepath;

        // Public property for testing
        public string FilepathPublic => _filepath;

        // Expose installation for widgets and derived classes
        // Matching PyKotor: widgets access editor._installation directly
        internal OdyInstallation Installation => _installation;

        /// <summary>True when the document has unsaved changes.</summary>
        public bool IsDirty => _dirty;

        /// <summary>Marks the document as modified (unsaved changes). Call when user edits content.</summary>
        protected void MarkDirty()
        {
            if (_dirty) return;
            _dirty = true;
            RefreshWindowTitle();
        }

        /// <summary>Clears the dirty flag. Called after Load, Save, or New.</summary>
        protected void ClearDirty()
        {
            if (!_dirty) return;
            _dirty = false;
            RefreshWindowTitle();
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor.py:303-350
        // Original: def __init__(self, parent, title, iconName, readSupported, writeSupported, installation):
        private bool _exitMenuItemWired;

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
            _readSupported = readSupported ?? new ResourceType[0];
            _writeSupported = writeSupported ?? new ResourceType[0];

            SetupEditorFilters();
            Opened += OnEditorOpened;
            Closing += OnEditorClosing;
        }

        private async void OnEditorClosing(object sender, WindowClosingEventArgs e)
        {
            if (!_dirty) return;
            e.Cancel = true;
            var result = await ShowSaveChangesDialogAsync();
            if (result == SaveChangesResult.Save)
            {
                try { Save(); } catch { return; }
                ClearDirty();
                Close();
            }
            else if (result == SaveChangesResult.DontSave)
            {
                ClearDirty();
                Close();
            }
        }

        protected enum SaveChangesResult { Cancel, Save, DontSave }

        /// <summary>Shows "Do you want to save changes?" dialog. Returns Save, DontSave, or Cancel.</summary>
        protected async Task<SaveChangesResult> ShowSaveChangesDialogAsync()
        {
            string docName = !string.IsNullOrEmpty(_filepath) ? System.IO.Path.GetFileName(_filepath)
                : (!string.IsNullOrEmpty(_resname) && _restype != null ? $"{_resname}.{_restype.Extension}" : "Untitled");
            var box = MessageBoxManager.GetMessageBoxStandard(
                "Unsaved changes",
                $"Do you want to save the changes you made to \"{docName}\"?",
                ButtonEnum.YesNoCancel,
                MsBox.Avalonia.Enums.Icon.Question);
            var result = await box.ShowWindowDialogAsync(this);
            if (result == ButtonResult.Yes) return SaveChangesResult.Save;
            if (result == ButtonResult.No) return SaveChangesResult.DontSave;
            return SaveChangesResult.Cancel;
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

        private void OnEditorOpened(object sender, EventArgs e)
        {
            EnsureExitMenuItemWired();
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
            // Skip if Content already has a visual parent (window is shown, can't re-parent controls)
            if (Content is Control existing && existing.GetVisualParent() != null)
                return;

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
                if (Content is Control ctrl)
                {
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor.py:489-516
        // Original: def setupEditorFilters(self, readSupported, writeSupported):
        protected void SetupEditorFilters()
        {
            // Setup file filters for open/save dialogs
            // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor.py:489-516
            // Original: Additional formats handling
            // Add format variants (XML, JSON, CSV, ASCII, YAML) for each base resource type
            var additionalFormats = new[] { "XML", "JSON", "CSV", "ASCII", "YAML" };
            var readList = _readSupported.ToList();
            var writeList = _writeSupported.ToList();

            // Add format variants for read supported types
            // For each base type, look for variants like {FieldName}_XML, {FieldName}_JSON, etc.
            // Matching PyKotor: uses restype.name (field name) to construct variant names
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
            // Matching PyKotor: uses restype.name (field name) to construct variant names
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

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editor/base.py refresh_window_title()
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor.py:523-589
        // Original: def save_as(self):
        public abstract void SaveAs();

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor.py:590-644
        // Original: def save(self):
        public virtual void Save()
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                SaveAs();
                return;
            }

            try
            {
                var (data, dataExt) = Build();
                if (data == null)
                {
                    return;
                }

                _revert = data;
                ClearDirty();

                // Save to file
                File.WriteAllBytes(_filepath, data);
            }
            catch (Exception ex)
            {
                _ = MessageBoxManager.GetMessageBoxStandard(
                    "Error saving",
                    "Error while saving: " + ex.Message,
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error).ShowWindowDialogAsync(this);
                throw;
            }
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor.py:700-750
        // Original: def load(self, filepath, resref, restype, data):
        public virtual void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            _filepath = filepath;
            _resname = resref;
            _restype = restype;
            _revert = data;
            ClearDirty();
            RefreshWindowTitle();
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor.py:750-780
        // Original: def new(self):
        public virtual void New()
        {
            _filepath = null;
            _resname = null;
            _restype = null;
            _revert = null;
            ClearDirty();
            RefreshWindowTitle();
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor.py:750-780
        // Original: def build(self) -> tuple[bytes, bytes]:
        public abstract Tuple<byte[], byte[]> Build();

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor.py:518-521
        // Original: def getOpenedFileName(self) -> str:
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor/base.py:187-239
        // Original: def _add_help_action(self, wiki_filename: str | None = None):
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

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editor/base.py:241-251
        // Original: def _show_help_dialog(self, wiki_filename: str):
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
                    if (item is MenuItem menuItem && menuItem.Header?.ToString() == "Help")
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
                // Create a menu bar if it doesn't exist
                // In Avalonia, menus are typically added to a DockPanel or directly to the window
                mainMenu = new Menu();
                // Add menu to window - wrap content in DockPanel if needed
                if (Content is Panel panel)
                {
                    var dockPanel = new DockPanel();
                    dockPanel.Children.Add(mainMenu);
                    DockPanel.SetDock(mainMenu, Dock.Top);
                    // Move existing content
                    var children = panel.Children.ToList();
                    foreach (var child in children)
                    {
                        panel.Children.Remove(child);
                        dockPanel.Children.Add(child);
                    }
                    Content = dockPanel;
                }
                else
                {
                    var dockPanel = new DockPanel();
                    dockPanel.Children.Add(mainMenu);
                    DockPanel.SetDock(mainMenu, Dock.Top);
                    if (Content != null && Content is Control content)
                    {
                        dockPanel.Children.Add(content);
                    }
                    Content = dockPanel;
                }
            }

            // Create Help menu item
            var helpMenuItem = new MenuItem { Header = "Help" };
            mainMenu.Items.Add(helpMenuItem);
            return helpMenuItem;
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
    }
}
