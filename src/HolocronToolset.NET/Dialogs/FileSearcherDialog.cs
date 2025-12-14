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

        // Public parameterless constructor for XAML
        public FileSearcherDialog() : this(null, null)
        {
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:42-72
        // Original: def __init__(self, parent, installations):
        public FileSearcherDialog(Window parent, Dictionary<string, HTInstallation> installations)
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
            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:64-72
            // Original: Setup installations in combo box
            var installationSelect = this.FindControl<ComboBox>("installationSelect");
            if (installationSelect != null && _installations != null)
            {
                installationSelect.Items.Clear();
                foreach (var kvp in _installations)
                {
                    installationSelect.Items.Add(kvp.Key);
                }
                if (installationSelect.Items.Count > 0)
                {
                    installationSelect.SelectedIndex = 0;
                }
            }

            // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:70-71
            // Original: Connect Select All checkbox
            var selectAllCheck = this.FindControl<CheckBox>("selectAllCheck");
            if (selectAllCheck != null)
            {
                selectAllCheck.IsCheckedChanged += (sender, e) => ToggleAllCheckboxes(selectAllCheck.IsChecked ?? false);
            }

            // Connect search button
            var searchButton = this.FindControl<Button>("searchButton");
            if (searchButton != null)
            {
                searchButton.Click += (sender, e) => OnSearch();
            }

            // Connect cancel button
            var cancelButton = this.FindControl<Button>("cancelButton");
            if (cancelButton != null)
            {
                cancelButton.Click += (sender, e) => Close();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:73-97
        // Original: def toggle_all_checkboxes(self, state: Qt.CheckState):
        private void ToggleAllCheckboxes(bool checkState)
        {
            var checkBoxes = new[]
            {
                this.FindControl<CheckBox>("typeARECheck"),
                this.FindControl<CheckBox>("typeGITCheck"),
                this.FindControl<CheckBox>("typeIFOCheck"),
                this.FindControl<CheckBox>("typeVISCheck"),
                this.FindControl<CheckBox>("typeLYTCheck"),
                this.FindControl<CheckBox>("typeDLGCheck"),
                this.FindControl<CheckBox>("typeJRLCheck"),
                this.FindControl<CheckBox>("typeUTCCheck"),
                this.FindControl<CheckBox>("typeUTDCheck"),
                this.FindControl<CheckBox>("typeUTECheck"),
                this.FindControl<CheckBox>("typeUTICheck"),
                this.FindControl<CheckBox>("typeUTPCheck"),
                this.FindControl<CheckBox>("typeUTMCheck"),
                this.FindControl<CheckBox>("typeUTWCheck"),
                this.FindControl<CheckBox>("typeUTSCheck"),
                this.FindControl<CheckBox>("typeUTTCheck"),
                this.FindControl<CheckBox>("type2DACheck"),
                this.FindControl<CheckBox>("typeNSSCheck"),
                this.FindControl<CheckBox>("typeNCSCheck")
            };

            foreach (var checkBox in checkBoxes)
            {
                if (checkBox != null)
                {
                    checkBox.IsChecked = checkState;
                }
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/dialogs/search.py:99-149
        // Original: def accept(self):
        private void OnSearch()
        {
            var installationSelect = this.FindControl<ComboBox>("installationSelect");
            var searchTextEdit = this.FindControl<TextBox>("searchTextEdit");
            var coreCheck = this.FindControl<CheckBox>("coreCheck");
            var modulesCheck = this.FindControl<CheckBox>("modulesCheck");
            var overrideCheck = this.FindControl<CheckBox>("overrideCheck");
            var caseSensitiveRadio = this.FindControl<RadioButton>("caseSensitiveRadio");
            var filenamesOnlyCheck = this.FindControl<CheckBox>("filenamesOnlyCheck");

            if (installationSelect == null || installationSelect.SelectedItem == null)
            {
                return;
            }

            string selectedInstallationName = installationSelect.SelectedItem.ToString();
            if (!_installations.ContainsKey(selectedInstallationName))
            {
                return;
            }

            HTInstallation installation = _installations[selectedInstallationName];
            _selectedInstallation = installation;

            var checkTypes = new List<ResourceType>();
            if (this.FindControl<CheckBox>("typeARECheck")?.IsChecked == true) checkTypes.Add(ResourceType.ARE);
            if (this.FindControl<CheckBox>("typeGITCheck")?.IsChecked == true) checkTypes.Add(ResourceType.GIT);
            if (this.FindControl<CheckBox>("typeIFOCheck")?.IsChecked == true) checkTypes.Add(ResourceType.IFO);
            if (this.FindControl<CheckBox>("typeVISCheck")?.IsChecked == true) checkTypes.Add(ResourceType.VIS);
            if (this.FindControl<CheckBox>("typeLYTCheck")?.IsChecked == true) checkTypes.Add(ResourceType.LYT);
            if (this.FindControl<CheckBox>("typeDLGCheck")?.IsChecked == true) checkTypes.Add(ResourceType.DLG);
            if (this.FindControl<CheckBox>("typeJRLCheck")?.IsChecked == true) checkTypes.Add(ResourceType.JRL);
            if (this.FindControl<CheckBox>("typeUTCCheck")?.IsChecked == true) checkTypes.Add(ResourceType.UTC);
            if (this.FindControl<CheckBox>("typeUTDCheck")?.IsChecked == true) checkTypes.Add(ResourceType.UTD);
            if (this.FindControl<CheckBox>("typeUTECheck")?.IsChecked == true) checkTypes.Add(ResourceType.UTE);
            if (this.FindControl<CheckBox>("typeUTICheck")?.IsChecked == true) checkTypes.Add(ResourceType.UTI);
            if (this.FindControl<CheckBox>("typeUTPCheck")?.IsChecked == true) checkTypes.Add(ResourceType.UTP);
            if (this.FindControl<CheckBox>("typeUTMCheck")?.IsChecked == true) checkTypes.Add(ResourceType.UTM);
            if (this.FindControl<CheckBox>("typeUTWCheck")?.IsChecked == true) checkTypes.Add(ResourceType.UTW);
            if (this.FindControl<CheckBox>("typeUTSCheck")?.IsChecked == true) checkTypes.Add(ResourceType.UTS);
            if (this.FindControl<CheckBox>("typeUTTCheck")?.IsChecked == true) checkTypes.Add(ResourceType.UTT);
            if (this.FindControl<CheckBox>("type2DACheck")?.IsChecked == true) checkTypes.Add(ResourceType.TwoDA);
            if (this.FindControl<CheckBox>("typeNSSCheck")?.IsChecked == true) checkTypes.Add(ResourceType.NSS);
            if (this.FindControl<CheckBox>("typeNCSCheck")?.IsChecked == true) checkTypes.Add(ResourceType.NCS);

            var query = new FileSearchQuery
            {
                Installation = installation,
                CaseSensitive = caseSensitiveRadio?.IsChecked ?? false,
                FilenamesOnly = filenamesOnlyCheck?.IsChecked ?? false,
                Text = searchTextEdit?.Text ?? "",
                SearchCore = coreCheck?.IsChecked ?? false,
                SearchModules = modulesCheck?.IsChecked ?? false,
                SearchOverride = overrideCheck?.IsChecked ?? false,
                CheckTypes = checkTypes
            };

            Search(query);
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
