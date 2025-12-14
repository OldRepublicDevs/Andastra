using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/select_module.py:18
    // Original: class SelectModuleDialog(QDialog):
    public class SelectModuleDialog : Window
    {
        private HTInstallation _installation;
        private string _selectedModule;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/select_module.py:19-67
        // Original: def __init__(self, parent, installation):
        public SelectModuleDialog(Window parent = null, HTInstallation installation = null)
        {
            InitializeComponent();
            _installation = installation;
            _selectedModule = "";
            SetupUI();
            BuildModuleList();
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
            Title = "Select Module";
            Width = 500;
            Height = 400;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Select Module",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            var openButton = new Button { Content = "Open" };
            openButton.Click += (sender, e) => Confirm();
            var cancelButton = new Button { Content = "Cancel" };
            cancelButton.Click += (sender, e) => Close();

            panel.Children.Add(titleLabel);
            panel.Children.Add(openButton);
            panel.Children.Add(cancelButton);
            Content = panel;
        }

        private void SetupUI()
        {
            // Additional UI setup if needed
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/select_module.py:69-98
        // Original: def _build_module_list(self):
        private void BuildModuleList()
        {
            if (_installation == null)
            {
                return;
            }

            var moduleNames = _installation.ModuleNames();
            var listedModules = new HashSet<string>();

            // Build module list - will be implemented when module list is available
            foreach (var kvp in moduleNames)
            {
                string moduleFile = kvp.Key;
                string moduleName = kvp.Value;
                string moduleRoot = System.IO.Path.GetFileNameWithoutExtension(moduleFile);
                string casefoldModuleFileName = moduleRoot.ToLowerInvariant();

                if (listedModules.Contains(casefoldModuleFileName))
                {
                    continue;
                }
                listedModules.Add(casefoldModuleFileName);
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/select_module.py:113-127
        // Original: def confirm(self):
        private void Confirm()
        {
            // Get selected module - will be implemented when UI controls are available
            Close();
        }

        public string SelectedModule => _selectedModule;
    }
}
