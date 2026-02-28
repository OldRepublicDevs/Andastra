using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OdyTools.Data;
using OdyTools.Dialogs;
using OdyTools.Windows;
using BioWare;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Resource;
using BioWare.Tools;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ModuleClass = BioWare.Common.Module;
using GameModule = BioWare.Common.Module;

namespace OdyTools.Windows
{
    public class ModuleDesignerWindow : Window
    {
        private OdyInstallation _installation;
        private string _modulePath;
        private string _moduleName;
        private GameModule _module;
        private UndoStack _undoStack;
        private List<object> _selectedInstances; // GITInstance equivalents

        public ModuleDesignerWindowUi Ui { get; private set; }

        public ModuleClass GetModule() => _module;

        public UndoStack UndoStack => _undoStack;

        public ModuleDesignerWindow(
            Window parent = null,
            OdyInstallation installation = null,
            string modulePath = null)
        {
            InitializeComponent();
            _installation = installation;
            _modulePath = modulePath;
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
            Title = "Module Designer";
            Width = 1200;
            Height = 800;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Module Designer",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            panel.Children.Add(titleLabel);
            Content = panel;
        }

        private TreeView _moduleTree;
        private DataGrid _propertiesTable;

        private void SetupUI()
        {
            // Find controls from XAML
            try
            {
                _moduleTree = this.FindControl<TreeView>("moduleTree");
                _propertiesTable = this.FindControl<DataGrid>("propertiesTable");
            }
            catch
            {
                // XAML not loaded or controls not found - will use programmatic UI
                _moduleTree = null;
                _propertiesTable = null;
            }

            // Create UI wrapper for testing
            Ui = new ModuleDesignerWindowUi
            {
                ModuleTree = _moduleTree,
                PropertiesTable = _propertiesTable
            };

            // Initialize undo stack (matching Python: self.undo_stack: QUndoStack = QUndoStack(self))
            _undoStack = new UndoStack();
            _selectedInstances = new List<object>();

            // If module path provided, open it
            if (!string.IsNullOrEmpty(_modulePath))
            {
                // Defer opening to allow UI to initialize
                // Matching Python: QTimer().singleShot(33, lambda: self.open_module(mod_filepath))
                OpenModule(_modulePath);
            }
        }

        public void OpenModuleWithDialog()
        {
            if (_installation == null)
            {
                return;
            }

            // Matching Python: dialog = SelectModuleDialog(self, self._installation)
            var dialog = new SelectModuleDialog(this, _installation);

            // Matching Python: if dialog.exec():
            if (dialog.ShowDialog(this))
            {
                // Matching Python: mod_filepath = self._installation.module_path().joinpath(dialog.module)
                string selectedModule = dialog.SelectedModule;
                if (string.IsNullOrEmpty(selectedModule))
                {
                    return;
                }

                // Construct full module filepath by combining module path with selected module filename
                string modulePath = _installation.ModulePath();
                string modFilepath = System.IO.Path.Combine(modulePath, selectedModule);

                // Matching Python: self.open_module(mod_filepath)
                OpenModule(modFilepath);
            }
        }

        public void OpenModule(string modFilepath)
        {
            if (_installation == null || string.IsNullOrEmpty(modFilepath))
            {
                return;
            }

            try
            {
                // Matching Python: mod_root: str = self._installation.get_module_root(mod_filepath)
                // k1_win_gog_swkotor.exe: FUN_004094a0 - Module root extraction logic
                string modRoot = Installation.GetModuleRoot(modFilepath);

                // Matching Python: combined_module = Module(mod_root, self._installation, use_dot_mod=is_mod_file(mod_filepath))
                // k1_win_gog_swkotor.exe: FUN_004094a0 - Module loading with .mod override detection
                bool useDotMod = FileHelpers.IsModFile(modFilepath);
                _module = new ModuleClass(modRoot, _installation.Installation, useDotMod);

                // Store module path and name for UI display
                _modulePath = modFilepath;
                _moduleName = Path.GetFileNameWithoutExtension(modFilepath);

                // Matching Python: self._refresh_window_title()
                RefreshWindowTitle();

                // Matching Python: self.rebuild_resource_tree()
                RebuildResourceTree();

                // Matching Python: self.rebuild_instance_list()
                RebuildInstanceList();
            }
            catch (Exception ex)
            {
                var errorBox = MessageBoxManager.GetMessageBoxStandard(
                    "Failed to open module",
                    ex.Message ?? "Unknown error.",
                    ButtonEnum.Ok,
                    MsBox.Avalonia.Enums.Icon.Error);
                errorBox.ShowWindowDialogAsync(this);
                // Clear module on error to maintain consistent state
                _module = null;
                _modulePath = null;
                _moduleName = null;
                RefreshWindowTitle();
                RebuildResourceTree();
                RebuildInstanceList();
            }
        }

        public void UnloadModule()
        {
            // Matching Python: self._module = None
            _module = null;
            _modulePath = null;
            _moduleName = null;

            // Matching Python: self.rebuild_resource_tree()
            RebuildResourceTree();

            // Matching Python: self.rebuild_instance_list()
            RebuildInstanceList();

            // Matching Python: self._refresh_window_title()
            RefreshWindowTitle();
        }

        public void SaveGit()
        {
            if (_module == null)
            {
                return;
            }

            // Matching Python: git_module = self._module.git()
            // Matching Python: assert git_module is not None
            var gitModule = _module.Git();
            if (gitModule == null)
            {
                return;
            }

            // Matching Python: git_module.save()
            gitModule.Save();

            // Also save the layout if it has been modified
            // Matching Python: layout_module = self._module.layout()
            // Matching Python: if layout_module is not None: layout_module.save()
            var layoutModule = _module.Layout();
            if (layoutModule != null)
            {
                layoutModule.Save();
            }

            // Mark the current state as clean after saving
            // Matching Python: self._mark_clean_state()
            MarkCleanState();
        }

        public void RebuildResourceTree()
        {
            if (Ui.ModuleTree == null)
            {
                return;
            }

            // Only build if module is loaded
            if (_module == null)
            {
                Ui.ModuleTree.IsEnabled = false;
                Ui.ModuleTree.ItemsSource = null;
                return;
            }

            // Enable the tree
            Ui.ModuleTree.IsEnabled = true;

            // Create category tree items for resource types
            var categories = new Dictionary<ResourceType, TreeViewItem>
            {
                { ResourceType.UTC, new TreeViewItem { Header = "Creatures" } },
                { ResourceType.UTP, new TreeViewItem { Header = "Placeables" } },
                { ResourceType.UTD, new TreeViewItem { Header = "Doors" } },
                { ResourceType.UTI, new TreeViewItem { Header = "Items" } },
                { ResourceType.UTE, new TreeViewItem { Header = "Encounters" } },
                { ResourceType.UTT, new TreeViewItem { Header = "Triggers" } },
                { ResourceType.UTW, new TreeViewItem { Header = "Waypoints" } },
                { ResourceType.UTS, new TreeViewItem { Header = "Sounds" } },
                { ResourceType.UTM, new TreeViewItem { Header = "Merchants" } },
                { ResourceType.DLG, new TreeViewItem { Header = "Dialogs" } },
                { ResourceType.FAC, new TreeViewItem { Header = "Factions" } },
                { ResourceType.MDL, new TreeViewItem { Header = "Models" } },
                { ResourceType.TGA, new TreeViewItem { Header = "Textures" } },
                { ResourceType.NCS, new TreeViewItem { Header = "Scripts" } },
                { ResourceType.IFO, new TreeViewItem { Header = "Module Data" } },
                { ResourceType.INVALID, new TreeViewItem { Header = "Other" } }
            };

            // Map related resource types to same categories (matching PyKotor)
            categories[ResourceType.MDX] = categories[ResourceType.MDL];
            categories[ResourceType.WOK] = categories[ResourceType.MDL];
            categories[ResourceType.TPC] = categories[ResourceType.TGA];
            categories[ResourceType.ARE] = categories[ResourceType.IFO];
            categories[ResourceType.GIT] = categories[ResourceType.IFO];
            categories[ResourceType.LYT] = categories[ResourceType.IFO];
            categories[ResourceType.VIS] = categories[ResourceType.IFO];
            categories[ResourceType.PTH] = categories[ResourceType.IFO];
            categories[ResourceType.NSS] = categories[ResourceType.NCS];

            // Initialize ItemsSource for each category
            foreach (var category in categories.Values)
            {
                category.ItemsSource = new List<TreeViewItem>();
                category.IsExpanded = true;
            }

            // Iterate through module resources and add them to appropriate categories
            if (_module.Resources != null)
            {
                foreach (var kvp in _module.Resources)
                {
                    var resource = kvp.Value;
                    if (resource == null)
                    {
                        continue;
                    }

                    // Get resource name and type
                    string resname = resource.GetResName();
                    ResourceType restype = resource.GetResType();

                    // Determine category (default to "Other" if not found)
                    TreeViewItem category = categories.ContainsKey(restype)
                        ? categories[restype]
                        : categories[ResourceType.INVALID];

                    // Create resource item
                    string resourceDisplayName = $"{resname}.{restype.Extension}";
                    var resourceItem = new TreeViewItem
                    {
                        Header = resourceDisplayName,
                        Tag = resource
                    };

                    // Add to category's children list
                    var categoryChildren = category.ItemsSource as List<TreeViewItem>;
                    if (categoryChildren != null)
                    {
                        categoryChildren.Add(resourceItem);
                    }
                }
            }

            // Sort items alphabetically within each category
            foreach (var category in categories.Values)
            {
                var categoryChildren = category.ItemsSource as List<TreeViewItem>;
                if (categoryChildren != null && categoryChildren.Count > 0)
                {
                    categoryChildren.Sort((a, b) =>
                    {
                        string headerA = a?.Header?.ToString() ?? "";
                        string headerB = b?.Header?.ToString() ?? "";
                        return string.Compare(headerA, headerB, StringComparison.OrdinalIgnoreCase);
                    });
                }
            }

            // Get unique category items (since some resource types share categories)
            var uniqueCategories = categories.Values.Distinct().ToList();

            // Sort categories alphabetically
            uniqueCategories.Sort((a, b) =>
            {
                string headerA = a?.Header?.ToString() ?? "";
                string headerB = b?.Header?.ToString() ?? "";
                return string.Compare(headerA, headerB, StringComparison.OrdinalIgnoreCase);
            });

            // Set tree items source
            Ui.ModuleTree.ItemsSource = uniqueCategories;
        }

        public void RebuildInstanceList()
        {
            _selectedInstances?.Clear();
            // Clear properties table when no instance is selected (module unloaded or selection cleared)
            if (Ui?.PropertiesTable != null)
            {
                Ui.PropertiesTable.ItemsSource = null;
            }
            // Full GIT instance list (creatures, doors, placeables, etc.) would be populated from module.Git()
            // when instance list UI is present; resource tree selection drives properties table elsewhere.
        }

        private void RefreshWindowTitle()
        {
            if (string.IsNullOrEmpty(_moduleName))
            {
                Title = "Module Designer";
            }
            else
            {
                Title = $"Module Designer - {_moduleName}";
            }
        }

        private void OnUndo()
        {
            if (_undoStack != null && _undoStack.CanUndo())
            {
                _undoStack.Undo();
            }
        }

        private void OnRedo()
        {
            if (_undoStack != null && _undoStack.CanRedo())
            {
                _undoStack.Redo();
            }
        }

        /// <summary>
        /// Mark the current state as clean (no unsaved changes).
        /// </summary>
        private void MarkCleanState()
        {
            if (_undoStack != null)
            {
                _undoStack.SetClean();
            }
            RefreshWindowTitle();
        }
    }

    public class ModuleDesignerWindowUi
    {
        public TreeView ModuleTree { get; set; }
        public DataGrid PropertiesTable { get; set; }
    }
}
