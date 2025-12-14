using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using HolocronToolset.NET.Data;
using HolocronToolset.NET.Utils;
using FileResource = CSharpKOTOR.Resources.FileResource;

namespace HolocronToolset.NET.Dialogs
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:194
    // Original: class FileResults(QDialog):
    public class FileResultsDialog : Window
    {
        private List<FileResource> _results;
        private HTInstallation _installation;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:197-298
        // Original: def __init__(self, parent, results, installation):
        public FileResultsDialog(Window parent = null, List<FileResource> results = null, HTInstallation installation = null)
        {
            InitializeComponent();
            _results = results ?? new List<FileResource>();
            _installation = installation;
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
            Title = $"File Search Results ({_results?.Count ?? 0} results)";
            Width = 600;
            Height = 500;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = $"Found {_results?.Count ?? 0} results",
                FontSize = 16,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };
            panel.Children.Add(titleLabel);
            Content = panel;
        }

        private void SetupUI()
        {
            // Additional UI setup if needed
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:195
        // Original: sig_searchresults_selected = Signal(FileResource)
        public event Action<FileResource> SearchResultsSelected;

        private void OnSearchResultSelected(FileResource resource)
        {
            SearchResultsSelected?.Invoke(resource);
        }
    }
}
