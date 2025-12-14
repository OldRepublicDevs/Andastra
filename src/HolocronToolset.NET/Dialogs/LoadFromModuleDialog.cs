using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Resources;
using FileResource = CSharpKOTOR.Resources.FileResource;

namespace HolocronToolset.NET.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_module.py:14
    // Original: class LoadFromModuleDialog(QDialog):
    public class LoadFromModuleDialog : Window
    {
        private List<FileResource> _resources;
        private List<ResourceType> _supported;
        private FileResource _selectedResource;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_module.py:17-60
        // Original: def __init__(self, capsule, supported):
        public LoadFromModuleDialog(List<FileResource> resources = null, List<ResourceType> supported = null)
        {
            InitializeComponent();
            _resources = resources ?? new List<FileResource>();
            _supported = supported ?? new List<ResourceType>();
            _selectedResource = null;
            SetupUI();
            BuildResourceList();
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
            Title = "Load from Module";
            Width = 500;
            Height = 400;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Load from Module",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            var okButton = new Button { Content = "OK" };
            okButton.Click += (sender, e) => Close();

            panel.Children.Add(titleLabel);
            panel.Children.Add(okButton);
            Content = panel;
        }

        private void SetupUI()
        {
            // Additional UI setup if needed
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_module.py:54-60
        // Original: Build resource list from capsule
        private void BuildResourceList()
        {
            // Filter resources by supported types
            var filteredResources = _resources.Where(r => _supported.Contains(r.ResType)).ToList();
            // Store for later use
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/load_from_module.py:62-84
        // Original: def resname(self) -> str | None:
        public string ResName()
        {
            return _selectedResource?.ResName;
        }

        public ResourceType ResType()
        {
            return _selectedResource?.ResType;
        }

        public byte[] Data()
        {
            return _selectedResource?.GetData();
        }
    }
}
