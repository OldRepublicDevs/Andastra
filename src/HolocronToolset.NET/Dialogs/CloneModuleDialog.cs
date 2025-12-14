using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/clone_module.py:30
    // Original: class CloneModuleDialog(QDialog):
    public class CloneModuleDialog : Window
    {
        private HTInstallation _active;
        private Dictionary<string, HTInstallation> _installations;
        private string _selectedModuleRoot;
        private string _identifier;
        private string _prefix;
        private string _name;
        private bool _copyTextures;
        private bool _copyLightmaps;
        private bool _keepDoors;
        private bool _keepPlaceables;
        private bool _keepSounds;
        private bool _keepPathing;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/clone_module.py:31-83
        // Original: def __init__(self, parent, active, installations):
        public CloneModuleDialog(
            Window parent = null,
            HTInstallation active = null,
            Dictionary<string, HTInstallation> installations = null)
        {
            InitializeComponent();
            _active = active;
            _installations = installations ?? new Dictionary<string, HTInstallation>();
            SetupUI();
            LoadModules();
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
            Title = "Clone Module";
            Width = 500;
            Height = 500;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Clone Module",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            var createButton = new Button { Content = "Create" };
            createButton.Click += (sender, e) => Ok();
            var cancelButton = new Button { Content = "Cancel" };
            cancelButton.Click += (sender, e) => Close();

            panel.Children.Add(titleLabel);
            panel.Children.Add(createButton);
            panel.Children.Add(cancelButton);
            Content = panel;
        }

        private void SetupUI()
        {
            // Additional UI setup if needed
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/clone_module.py:150-174
        // Original: def load_modules(self):
        private void LoadModules()
        {
            var options = new Dictionary<string, ModuleOption>();
            foreach (var installation in _installations.Values)
            {
                var moduleNames = installation.ModuleNames();
                foreach (var kvp in moduleNames)
                {
                    string filename = kvp.Key;
                    string name = kvp.Value;
                    string root = System.IO.Path.GetFileNameWithoutExtension(filename);
                    if (!options.ContainsKey(root))
                    {
                        options[root] = new ModuleOption(name, root, new List<string>(), installation);
                    }
                    options[root].Files.Add(filename);
                }
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/clone_module.py:85-148
        // Original: def ok(self):
        private void Ok()
        {
            // Clone module - will be implemented when module cloning is available
            System.Console.WriteLine("Module cloning not yet fully implemented");
            Close();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/clone_module.py:23-27
        // Original: class ModuleOption(NamedTuple):
        public class ModuleOption
        {
            public string Name { get; set; }
            public string Root { get; set; }
            public List<string> Files { get; set; }
            public HTInstallation Installation { get; set; }

            public ModuleOption(string name, string root, List<string> files, HTInstallation installation)
            {
                Name = name;
                Root = root;
                Files = files;
                Installation = installation;
            }
        }
    }
}
