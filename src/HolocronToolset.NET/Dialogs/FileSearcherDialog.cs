using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;
using FileResource = CSharpKOTOR.Resources.FileResource;

namespace HolocronToolset.NET.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:39
    // Original: class FileSearcher(QDialog):
    public partial class FileSearcherDialog : Window
    {
        private Dictionary<string, HTInstallation> _installations;
        private HTInstallation _selectedInstallation;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:42-72
        // Original: def __init__(self, parent, installations):
        public FileSearcherDialog(Window parent = null, Dictionary<string, HTInstallation> installations = null)
        {
            InitializeComponent();
            _installations = installations ?? new Dictionary<string, HTInstallation>();
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
            Title = "File Search";
            Width = 500;
            Height = 400;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "File Search",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            panel.Children.Add(titleLabel);
            Content = panel;
        }

        private void SetupUI()
        {
            // Additional UI setup if needed
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:151-191
        // Original: def search(self, query):
        public void Search(FileSearchQuery query)
        {
            var results = new List<FileResource>();

            // Search core resources
            if (query.SearchCore)
            {
                results.AddRange(query.Installation.CoreResources());
            }

            // Search modules
            if (query.SearchModules)
            {
                var moduleNames = query.Installation.ModuleNames();
                foreach (var moduleName in moduleNames.Keys)
                {
                    results.AddRange(query.Installation.ModuleResources(moduleName));
                }
            }

            // Search override
            if (query.SearchOverride)
            {
                var overrideList = query.Installation.OverrideList();
                foreach (var folder in overrideList)
                {
                    results.AddRange(query.Installation.OverrideResources(folder));
                }
            }

            // Filter by search text
            if (!string.IsNullOrEmpty(query.Text))
            {
                string searchText = query.CaseSensitive ? query.Text : query.Text.ToLowerInvariant();
                results = results.Where(r =>
                {
                    string resName = query.CaseSensitive ? r.ResName : r.ResName.ToLowerInvariant();
                    if (resName.Contains(searchText))
                    {
                        return true;
                    }

                    if (query.FilenamesOnly)
                    {
                        return false;
                    }

                    if (!query.CheckTypes.Contains(r.ResType))
                    {
                        return false;
                    }

                    // Search in resource data
                    try
                    {
                        byte[] data = r.GetData();
                        string dataText = System.Text.Encoding.ASCII.GetString(data);
                        string dataTextLower = query.CaseSensitive ? dataText : dataText.ToLowerInvariant();
                        return dataTextLower.Contains(searchText);
                    }
                    catch
                    {
                        return false;
                    }
                }).ToList();
            }

            // Filter by resource types
            if (query.CheckTypes != null && query.CheckTypes.Count > 0)
            {
                results = results.Where(r => query.CheckTypes.Contains(r.ResType)).ToList();
            }

            OnFileResults(results, query.Installation);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:39-40
        // Original: file_results = Signal(list, HTInstallation)
        public event Action<List<FileResource>, HTInstallation> FileResults;

        private void OnFileResults(List<FileResource> results, HTInstallation installation)
        {
            FileResults?.Invoke(results, installation);
        }
    }

    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:25-36
    // Original: @dataclass class FileSearchQuery:
    public class FileSearchQuery
    {
        public HTInstallation Installation { get; set; }
        public bool CaseSensitive { get; set; }
        public bool FilenamesOnly { get; set; }
        public string Text { get; set; }
        public bool SearchCore { get; set; }
        public bool SearchModules { get; set; }
        public bool SearchOverride { get; set; }
        public List<ResourceType> CheckTypes { get; set; }

        public FileSearchQuery()
        {
            CheckTypes = new List<ResourceType>();
        }
    }
}
