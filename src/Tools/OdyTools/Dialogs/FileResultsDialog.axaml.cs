using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Tools;
using OdyTools.Data;
using OdyTools.Utils;
using OdyTools.Editors;
using FileResource = BioWare.Extract.FileResource;

namespace OdyTools.Dialogs
{
    public partial class FileResultsDialog : Window
    {
        private OdyInstallation _installation;
        private FileResource _selection;

        public FileResultsDialogUi Ui { get; private set; }

        public event Action<FileResource> SearchResultsSelected;

        // Public parameterless constructor for XAML
        public FileResultsDialog() : this(null, null, null)
        {
        }

        public FileResultsDialog(Window parent, IEnumerable<FileResource> results, OdyInstallation installation)
        {
            InitializeComponent();
            _installation = installation;
            SetupUI();
            PopulateResults(results ?? new List<FileResource>());
        }

        public static FileResultsDialog FromReferenceSearch(
            Window parent,
            IEnumerable<ReferenceSearchResult> results,
            OdyInstallation installation)
        {
            var dialog = new FileResultsDialog(parent, new List<FileResource>(), installation);
            dialog.PopulateReferenceResults(results ?? new List<ReferenceSearchResult>());
            return dialog;
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
            Title = "Search Results";
            Width = 303;
            Height = 401;

            var panel = new StackPanel { Margin = new Avalonia.Thickness(10), Spacing = 10 };
            var resultList = new ListBox();
            var buttonPanel = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right, Spacing = 5 };
            var openButton = new Button { Content = "Open" };
            openButton.Click += (s, e) => Open();
            var okButton = new Button { Content = "OK" };
            okButton.Click += (s, e) => Accept();
            buttonPanel.Children.Add(openButton);
            buttonPanel.Children.Add(okButton);
            panel.Children.Add(resultList);
            panel.Children.Add(buttonPanel);
            Content = panel;

            // Create Ui wrapper for programmatic UI
            Ui = new FileResultsDialogUi
            {
                ResultList = resultList,
                OpenButton = openButton,
                OkButton = okButton
            };
        }

        private void SetupUI()
        {
            // If Ui is already initialized (e.g., by SetupProgrammaticUI), skip control finding
            if (Ui != null)
            {
                return;
            }

            // Find all controls from XAML and expose via Ui property
            // Use try-catch to handle cases where XAML controls might not be available (e.g., in tests)
            Ui = new FileResultsDialogUi();
            
            try
            {
                Ui.ResultList = this.FindControl<ListBox>("resultList");
                Ui.OpenButton = this.FindControl<Button>("openButton");
                Ui.OkButton = this.FindControl<Button>("okButton");
            }
            catch
            {
                // XAML controls not available - create programmatic UI for tests
                SetupProgrammaticUI();
                return; // SetupProgrammaticUI already sets up Ui and connects events
            }

            if (Ui.OpenButton != null)
            {
                Ui.OpenButton.Click += (s, e) => Open();
            }
            if (Ui.OkButton != null)
            {
                Ui.OkButton.Click += (s, e) => Accept();
            }
            if (Ui.ResultList != null)
            {
                Ui.ResultList.DoubleTapped += (s, e) => Open();
            }
        }

        private void PopulateResults(IEnumerable<FileResource> results)
        {
            if (Ui?.ResultList == null)
            {
                return;
            }

            Ui.ResultList.Items.Clear();
            var resultList = new List<FileResourceResultItem>();

            foreach (var result in results)
            {
                string filename = result.Identifier.ToString();
                string filepath = result.FilePath ?? "";
                string parentName = Path.GetFileName(Path.GetDirectoryName(filepath)) ?? "";
                string displayText = string.IsNullOrEmpty(parentName) ? filename : $"{parentName}/{filename}";

                resultList.Add(new FileResourceResultItem
                {
                    DisplayText = displayText,
                    Resource = result,
                    Tooltip = filepath
                });
            }

            // Sort items alphabetically
            resultList.Sort((a, b) => string.Compare(a.DisplayText, b.DisplayText, StringComparison.OrdinalIgnoreCase));

            foreach (var item in resultList)
            {
                Ui.ResultList.Items.Add(item);
            }
        }

        private void PopulateReferenceResults(IEnumerable<ReferenceSearchResult> results)
        {
            if (Ui?.ResultList == null)
            {
                return;
            }

            Ui.ResultList.Items.Clear();
            var resultList = new List<FileResourceResultItem>();

            foreach (ReferenceSearchResult result in results)
            {
                if (result?.Resource == null)
                {
                    continue;
                }

                FileResource resource = result.Resource;
                string filepath = resource.FilePath ?? string.Empty;
                string parentName = Path.GetFileName(Path.GetDirectoryName(filepath)) ?? "";
                string filename = resource.Identifier.ToString();
                string baseDisplay = string.IsNullOrEmpty(parentName) ? filename : parentName + "/" + filename;
                string displayText = string.IsNullOrEmpty(result.FieldPath)
                    ? baseDisplay
                    : baseDisplay + " :: " + result.FieldPath;

                resultList.Add(new FileResourceResultItem
                {
                    DisplayText = displayText,
                    Resource = resource,
                    FieldPath = result.FieldPath,
                    Tooltip = filepath
                });
            }

            resultList.Sort((a, b) => string.Compare(a.DisplayText, b.DisplayText, StringComparison.OrdinalIgnoreCase));

            foreach (var item in resultList)
            {
                Ui.ResultList.Items.Add(item);
            }
        }

        public void Accept()
        {
            if (Ui?.ResultList?.SelectedItem is FileResourceResultItem item)
            {
                _selection = item.Resource;
                SearchResultsSelected?.Invoke(_selection);
            }
            Close();
        }

        private void Open()
        {
            if (Ui?.ResultList?.SelectedItem is FileResourceResultItem item)
            {
                FileResource resource = item.Resource;
                
                // Note: PyKotor's open() method does NOT emit the signal - only accept() does
                // Note: Dialog does NOT close on open (unlike accept method) - matches PyKotor behavior
                Window parentWindow = this.Parent as Window ?? this;
                WindowUtils.OpenResourceEditor(resource, _installation, parentWindow);
            }
            else
            {
                System.Console.WriteLine("Nothing to open, no item selected");
            }
        }

        public FileResource Selection => _selection;
    }

    public class FileResultsDialogUi
    {
        public ListBox ResultList { get; set; }
        public Button OpenButton { get; set; }
        public Button OkButton { get; set; }
    }

    // Helper class to store FileResource with display text in ListBox
    internal class FileResourceResultItem
    {
        public string DisplayText { get; set; }
        public FileResource Resource { get; set; }
        public string FieldPath { get; set; }
        public string Tooltip { get; set; }

        public override string ToString()
        {
            return DisplayText;
        }
    }
}
