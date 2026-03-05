using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource.Formats.GFF;
using UTCHelpers = BioWare.Resource.Formats.GFF.Generics.UTC.UTCHelpers;
using UTC = BioWare.Resource.Formats.GFF.Generics.UTC.UTC;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.RIM;
using BioWare.Tools;
using OdyTools.Data;
using OdyTools.Common;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using FileResource = BioWare.Extract.FileResource;
using Module = BioWare.Common.Module;
using OdyTools.Utils;
using IconType = MsBox.Avalonia.Enums.Icon;

namespace OdyTools.Dialogs
{
    public partial class InsertInstanceDialog : Window
    {
        private OdyInstallation _installation;
        private Module _module;
        private ResourceType _restype;
        private string _resname;
        private byte[] _data;
        private string _filepath;
        private GlobalSettings _globalSettings;
        private Widgets.ModelRenderer _previewRenderer;
        private ObservableCollection<FileResource> _sourceResources;
        private CollectionViewSource _filteredResources;

        // Public parameterless constructor for XAML
        public InsertInstanceDialog() : this(null, null, null, ResourceType.UTC)
        {
        }

        public InsertInstanceDialog(Window parent, OdyInstallation installation, Module module, ResourceType restype)
        {
            InitializeComponent();
            _installation = installation;
            _module = module;
            _restype = restype;
            _resname = "";
            _data = new byte[0];
            _filepath = null;
            _globalSettings = new GlobalSettings();
            _sourceResources = new ObservableCollection<FileResource>();
            _filteredResources = new CollectionViewSource { Source = _sourceResources };
            SetupUI();
            SetupLocationSelect();
            SetupResourceList();
            MinHeight = 500;
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
            Title = "Insert Instance";
            Width = 800;
            Height = 600;

            var panel = new StackPanel();
            var titleLabel = new TextBlock
            {
                Text = "Insert Instance",
                FontSize = 18,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center
            };

            // Create UI controls programmatically for test scenarios
            _reuseResourceRadio = new RadioButton { Content = "Reuse Resource" };
            _copyResourceRadio = new RadioButton { Content = "Copy Resource" };
            _createResourceRadio = new RadioButton { Content = "Create Resource" };
            _resrefEdit = new TextBox { Watermark = "ResRef" };
            _locationSelect = new ComboBox();
            _resourceFilter = new TextBox { Watermark = "Filter" };
            _resourceList = new ListBox();
            _okButton = new Button { Content = "OK" };
            _cancelButton = new Button { Content = "Cancel" };
            _dynamicTextLabel = new TextBlock { Text = "No resource selected", TextWrapping = Avalonia.Media.TextWrapping.Wrap };
            _previewRenderer = new Widgets.ModelRenderer { Height = 260, MinHeight = 220 };
            if (_installation != null)
            {
                _previewRenderer.Installation = _installation;
            }

            // Connect events
            _reuseResourceRadio.IsCheckedChanged += (s, e) => OnResourceRadioToggled();
            _copyResourceRadio.IsCheckedChanged += (s, e) => OnResourceRadioToggled();
            _createResourceRadio.IsCheckedChanged += (s, e) => OnResourceRadioToggled();
            _resrefEdit.TextChanged += (s, e) => OnResrefEdited(_resrefEdit.Text);
            _resourceFilter.TextChanged += (s, e) => OnResourceFilterChanged();
            _resourceList.SelectionChanged += (s, e) => OnResourceSelected();
            _okButton.Click += async (s, e) => await Accept();
            _cancelButton.Click += (s, e) => Close();

            panel.Children.Add(titleLabel);
            panel.Children.Add(_reuseResourceRadio);
            panel.Children.Add(_copyResourceRadio);
            panel.Children.Add(_createResourceRadio);
            panel.Children.Add(_resrefEdit);
            panel.Children.Add(_locationSelect);
            panel.Children.Add(_resourceFilter);
            panel.Children.Add(_resourceList);
            panel.Children.Add(_okButton);
            panel.Children.Add(_cancelButton);
            panel.Children.Add(_dynamicTextLabel);
            panel.Children.Add(_previewRenderer);
            Content = panel;
        }

        private RadioButton _reuseResourceRadio;
        private RadioButton _copyResourceRadio;
        private RadioButton _createResourceRadio;
        private TextBox _resrefEdit;
        private ComboBox _locationSelect;
        private TextBox _resourceFilter;
        private ListBox _resourceList;
        private Button _okButton;
        private Button _cancelButton;
        private TextBlock _dynamicTextLabel;

        private void SetupUI()
        {
            // Use try-catch to handle cases where XAML controls might not be available (e.g., in tests)
            try
            {
                // Find controls from XAML
                _reuseResourceRadio = this.FindControl<RadioButton>("reuseResourceRadio");
                _copyResourceRadio = this.FindControl<RadioButton>("copyResourceRadio");
                _createResourceRadio = this.FindControl<RadioButton>("createResourceRadio");
                _resrefEdit = this.FindControl<TextBox>("resrefEdit");
                _locationSelect = this.FindControl<ComboBox>("locationSelect");
                _resourceFilter = this.FindControl<TextBox>("resourceFilter");
                _resourceList = this.FindControl<ListBox>("resourceList");
                _okButton = this.FindControl<Button>("okButton");
                _cancelButton = this.FindControl<Button>("cancelButton");
                _previewRenderer = this.FindControl<Widgets.ModelRenderer>("previewRenderer");
                _dynamicTextLabel = this.FindControl<TextBlock>("dynamicTextLabel");
            }
            catch
            {
                // XAML controls not available - create programmatic UI for tests
                SetupProgrammaticUI();
                return; // SetupProgrammaticUI already connects events
            }

            if (_reuseResourceRadio != null)
            {
                _reuseResourceRadio.IsCheckedChanged += (s, e) => OnResourceRadioToggled();
            }
            if (_copyResourceRadio != null)
            {
                _copyResourceRadio.IsCheckedChanged += (s, e) => OnResourceRadioToggled();
            }
            if (_createResourceRadio != null)
            {
                _createResourceRadio.IsCheckedChanged += (s, e) => OnResourceRadioToggled();
            }
            if (_resrefEdit != null)
            {
                _resrefEdit.TextChanged += (s, e) => OnResrefEdited(_resrefEdit.Text);
            }
            if (_resourceFilter != null)
            {
                _resourceFilter.TextChanged += (s, e) => OnResourceFilterChanged();
            }
            if (_resourceList != null)
            {
                _resourceList.ItemsSource = _filteredResources.View;
                _resourceList.SelectionChanged += (s, e) => OnResourceSelected();
            }
            if (_okButton != null)
            {
                _okButton.Click += async (s, e) => await Accept();
            }
            if (_cancelButton != null)
            {
                _cancelButton.Click += (s, e) => Close();
            }

            // Initialize preview renderer with installation
            if (_previewRenderer != null && _installation != null)
            {
                _previewRenderer.Installation = _installation;
            }
        }

        private void SetupLocationSelect()
        {
            if (_locationSelect == null || _installation == null || _module == null)
            {
                return;
            }

            _locationSelect.Items.Clear();
            _locationSelect.Items.Add(_installation.OverridePath());

            // Add module capsules
            var capsules = _module.Capsules();
            bool disableRIMSaving = _globalSettings.GetValue("DisableRIMSaving", false);

            foreach (var capsule in capsules)
            {
                if (capsule == null)
                {
                    continue;
                }

                string capsulePath = capsule.Path.ToString();

                // Skip RIM files if RIM saving is disabled
                if (FileHelpers.IsRimFile(capsulePath) && disableRIMSaving)
                {
                    continue;
                }

                // Add capsule path to location select
                _locationSelect.Items.Add(capsulePath);
            }

            // Set current selection to the last item (most recently added capsule or override path)
            if (_locationSelect.Items.Count > 0)
            {
                _locationSelect.SelectedIndex = _locationSelect.Items.Count - 1;
            }
        }

        private void SetupResourceList()
        {
            if (_resourceList == null || _installation == null)
            {
                return;
            }

            // Clear existing resources
            _sourceResources.Clear();

            // Add core resources
            var coreResources = _installation.CoreResources();
            foreach (var resource in coreResources)
            {
                if (resource.ResType == _restype)
                {
                    _sourceResources.Add(resource);
                }
            }

            // Add module resources
            if (_module != null)
            {
                var capsules = _module.Capsules();
                foreach (var capsule in capsules)
                {
                    if (capsule == null)
                    {
                        continue;
                    }

                    var capsuleResources = capsule.GetResources();
                    foreach (var capsuleResource in capsuleResources)
                    {
                        if (capsuleResource.ResType == _restype)
                        {
                            // Convert CapsuleResource to FileResource
                            // CapsuleResource.FilePath already contains the capsule path (set during CapsuleResource creation)
                            var fileResource = new FileResource(
                                capsuleResource.ResName,
                                capsuleResource.ResType,
                                capsuleResource.Size,
                                capsuleResource.Offset,
                                capsuleResource.FilePath
                            );
                            _sourceResources.Add(fileResource);
                        }
                    }
                }
            }

            // Refresh the filtered view
            _filteredResources.View.Refresh();
        }

        private async Task Accept()
        {
            bool newResource = true;

            if (_resourceList?.SelectedItem == null)
            {
                await DialogHelper.ShowAsync(Localization.Translate("Choose an instance"), Localization.Translate("You must choose an instance, use the radial buttons to determine where/how to create the GIT instance."), ButtonEnum.Ok, IconType.Error);
                return;
            }

            var resource = _resourceList.SelectedItem as FileResource;
            if (resource == null)
            {
                return;
            }

            if (_reuseResourceRadio?.IsChecked == true)
            {
                newResource = false;
                _resname = resource.ResName;
                _filepath = resource.FilePath;
                _data = resource.GetData();
            }
            else if (_copyResourceRadio?.IsChecked == true)
            {
                _resname = _resrefEdit?.Text ?? "";
                _filepath = _locationSelect?.SelectedItem?.ToString() ?? "";
                _data = resource.GetData();
            }
            else if (_createResourceRadio?.IsChecked == true)
            {
                _resname = _resrefEdit?.Text ?? "";
                _filepath = _locationSelect?.SelectedItem?.ToString() ?? "";
                // Create new resource data based on type
                _data = CreateNewResourceData(_restype);
            }

            // Save resource if new
            if (newResource && !string.IsNullOrEmpty(_filepath))
            {
                SaveResourceToFile(_filepath, _resname, _restype, _data);
            }

            // Add to module
            // Ensure filepath is set (should always be set at this point, but defensive check)
            if (_module != null && !string.IsNullOrEmpty(_resname) && !string.IsNullOrEmpty(_filepath))
            {
                // Convert filepath to absolute path if it's relative (matching PyKotor Path behavior)
                // PyKotor uses Path objects which are typically absolute when created from full paths
                string absoluteFilepath = _filepath;
                try
                {
                    if (!Path.IsPathRooted(_filepath))
                    {
                        absoluteFilepath = Path.GetFullPath(_filepath);
                    }
                }
                catch
                {
                    // If path resolution fails, use the original filepath
                    // This can happen in edge cases, but the original path should still be valid
                    absoluteFilepath = _filepath;
                }

                // Add location to module resource (creates ModuleResource if it doesn't exist)
                _module.AddLocations(_resname, _restype, new[] { absoluteFilepath });
            }

            Close();
        }

        // Comprehensive implementation matching PyKotor's resource creation logic
        private byte[] CreateNewResourceData(ResourceType restype)
        {
            // Get game type from installation, defaulting to K2 if not available
            BioWareGame gameToUse = _installation?.Game ?? BioWareGame.K2;

            // Create new resource instance based on type and convert to bytes
            if (restype == ResourceType.UTC)
            {
                UTC utc = new UTC();
                return UTCHelpers.BytesUtc(utc, gameToUse);
            }
            else if (restype == ResourceType.UTP)
            {
                UTP utp = new UTP();
                // UTP uses DismantleUtp + BytesGff pattern
                GFF utpGff = UTPHelpers.DismantleUtp(utp, gameToUse);
                return GFFAuto.BytesGff(utpGff, UTP.BinaryType);
            }
            else if (restype == ResourceType.UTD)
            {
                UTD utd = new UTD();
                // UTD uses DismantleUtd + BytesGff pattern
                GFF utdGff = UTDHelpers.DismantleUtd(utd, gameToUse);
                return GFFAuto.BytesGff(utdGff, UTD.BinaryType);
            }
            else if (restype == ResourceType.UTE)
            {
                UTE ute = new UTE();
                // UTE uses DismantleUte + BytesGff pattern
                GFF uteGff = UTEHelpers.DismantleUte(ute, gameToUse);
                return GFFAuto.BytesGff(uteGff, UTE.BinaryType);
            }
            else if (restype == ResourceType.UTT)
            {
                UTT utt = new UTT();
                return UTTAuto.BytesUtt(utt, gameToUse);
            }
            else if (restype == ResourceType.UTS)
            {
                UTS uts = new UTS();
                // UTS uses DismantleUts + BytesGff pattern
                GFF utsGff = UTSHelpers.DismantleUts(uts, gameToUse);
                return GFFAuto.BytesGff(utsGff, UTS.BinaryType);
            }
            else if (restype == ResourceType.UTM)
            {
                BioWare.Resource.Formats.GFF.Generics.UTM.UTM utm = new BioWare.Resource.Formats.GFF.Generics.UTM.UTM();
                // UTM uses DismantleUtm + BytesGff pattern
                GFF utmGff = BioWare.Resource.Formats.GFF.Generics.UTM.UTMHelpers.DismantleUtm(utm, gameToUse);
                return GFFAuto.BytesGff(utmGff, BioWare.Resource.Formats.GFF.Generics.UTM.UTM.BinaryType);
            }
            else if (restype == ResourceType.UTW)
            {
                UTW utw = new UTW();
                return UTWAuto.BytesUtw(utw, gameToUse);
            }
            else
            {
                // For unsupported resource types, return empty data
                return new byte[0];
            }
        }

        // Comprehensive implementation with full error handling and support for all ERF/RIM variants
        private void SaveResourceToFile(string filepath, string resname, ResourceType restype, byte[] data)
        {
            if (string.IsNullOrEmpty(filepath) || string.IsNullOrEmpty(resname) || data == null)
            {
                return;
            }

            try
            {
                // Ensure directory exists before writing
                string directory = Path.GetDirectoryName(filepath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                string fileName = Path.GetFileName(filepath);
                if (BioWare.Tools.FileHelpers.IsAnyErfTypeFile(fileName))
                {
                    // Handle ERF/MOD/SAV files
                    ERF erf;
                    if (File.Exists(filepath))
                    {
                        // Load existing ERF
                        erf = ERFAuto.ReadErf(filepath);
                    }
                    else
                    {
                        // Create new ERF with appropriate type
                        ERFType erfType = ERFTypeExtensions.FromExtension(Path.GetExtension(filepath));
                        erf = new ERF(erfType, Path.GetExtension(filepath).Equals(".sav", StringComparison.OrdinalIgnoreCase));
                    }

                    // Add or update resource in ERF
                    erf.SetData(resname, restype, data);

                    // Determine output format based on file extension
                    ResourceType outputFormat = ResourceType.ERF;
                    string ext = Path.GetExtension(filepath).ToLowerInvariant();
                    if (ext == ".mod")
                    {
                        outputFormat = ResourceType.MOD;
                    }
                    else if (ext == ".sav")
                    {
                        outputFormat = ResourceType.SAV;
                    }

                    // Write ERF to file
                    ERFAuto.WriteErf(erf, filepath, outputFormat);
                }
                else if (BioWare.Tools.FileHelpers.IsRimFile(fileName))
                {
                    // Handle RIM files
                    RIM rim;
                    if (File.Exists(filepath))
                    {
                        // Load existing RIM
                        rim = RIMAuto.ReadRim(filepath);
                    }
                    else
                    {
                        // Create new RIM
                        rim = new RIM();
                    }

                    // Add or update resource in RIM
                    rim.SetData(resname, restype, data);

                    // Write RIM to file
                    RIMAuto.WriteRim(rim, filepath, ResourceType.RIM);
                }
                else
                {
                    // Save as standalone file
                    string standalonePath = Path.Combine(
                        string.IsNullOrEmpty(directory) ? "." : directory,
                        $"{resname}.{restype.Extension}");

                    File.WriteAllBytes(standalonePath, data);
                    _filepath = standalonePath;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Error saving resource to file: {ex.Message}");
                DialogHelper.ShowWindow(this, "Error saving resource", ex.Message ?? "Unknown error.", IconType.Error);
                throw; // Re-throw to allow caller to handle
            }
        }

        private void OnResourceRadioToggled()
        {
            if (_resourceList != null)
            {
                _resourceList.IsEnabled = _createResourceRadio?.IsChecked != true;
            }
            if (_resourceFilter != null)
            {
                _resourceFilter.IsEnabled = _createResourceRadio?.IsChecked != true;
            }
            if (_resrefEdit != null)
            {
                _resrefEdit.IsEnabled = _reuseResourceRadio?.IsChecked != true;
            }

            if (_reuseResourceRadio?.IsChecked == true)
            {
                if (_okButton != null)
                {
                    _okButton.IsEnabled = true;
                }
            }
            else if (_copyResourceRadio?.IsChecked == true || _createResourceRadio?.IsChecked == true)
            {
                if (_okButton != null && _resrefEdit != null)
                {
                    _okButton.IsEnabled = IsValidResref(_resrefEdit.Text);
                }
            }
        }

        private void OnResourceSelected()
        {
            if (_resourceList?.SelectedItem is FileResource resource)
            {
                // Update dynamic text label
                string summaryText = GenerateResourceSummary(resource);
                if (_dynamicTextLabel != null)
                {
                    _dynamicTextLabel.Text = summaryText;
                }

                // Update preview
                if (resource.ResType == ResourceType.UTC && _globalSettings.ShowPreviewUTC)
                {
                    var utc = ResourceAutoHelpers.ReadUtc(resource.GetData());
                    if (_previewRenderer != null)
                    {
                        _previewRenderer.SetCreature(utc);
                    }
                }
                else
                {
                    byte[] mdlData = null;
                    byte[] mdxData = null;

                    if (resource.ResType == ResourceType.UTD && _globalSettings.ShowPreviewUTD)
                    {
                        var utd = ResourceAutoHelpers.ReadUtd(resource.GetData());
                        string modelName = BioWare.Tools.Door.GetModel(utd, _installation.Installation);
                        SetRenderModel(modelName);
                    }
                    else if (resource.ResType == ResourceType.UTP && _globalSettings.ShowPreviewUTP)
                    {
                        var utp = ResourceAutoHelpers.ReadUtp(resource.GetData());
                        string modelName = BioWare.Tools.Placeable.GetModel(utp, _installation.Installation);
                        SetRenderModel(modelName);
                    }
                    else if ((resource.ResType == ResourceType.MDL || resource.ResType == ResourceType.MDX) &&
                             (_globalSettings.ShowPreviewUTC || _globalSettings.ShowPreviewUTD || _globalSettings.ShowPreviewUTP))
                    {
                        byte[] data = resource.GetData();
                        if (resource.ResType == ResourceType.MDL)
                        {
                            mdlData = data;
                            // Try to get MDX from same container
                            string fileName = Path.GetFileName(resource.FilePath);
                            if (BioWare.Tools.FileHelpers.IsAnyErfTypeFile(fileName))
                            {
                                var erf = ERFAuto.ReadErf(resource.FilePath);
                                mdxData = erf.Get(resource.ResName, ResourceType.MDX);
                            }
                            else if (BioWare.Tools.FileHelpers.IsRimFile(fileName))
                            {
                                var rim = RIMAuto.ReadRim(resource.FilePath);
                                mdxData = rim.Get(resource.ResName, ResourceType.MDX);
                            }
                            else if (BioWare.Tools.FileHelpers.IsBifFile(fileName))
                            {
                                var mdxRes = _installation.Installation.Resources.LookupResource(resource.ResName, ResourceType.MDX);
                                if (mdxRes != null && mdxRes.Data != null)
                                {
                                    mdxData = mdxRes.Data;
                                }
                            }
                            else
                            {
                                string mdxPath = Path.ChangeExtension(resource.FilePath, ".mdx");
                                if (File.Exists(mdxPath))
                                {
                                    mdxData = File.ReadAllBytes(mdxPath);
                                }
                            }
                        }
                        else if (resource.ResType == ResourceType.MDX)
                        {
                            mdxData = data;
                            // Try to get MDL from same container
                            string fileName = Path.GetFileName(resource.FilePath);
                            if (BioWare.Tools.FileHelpers.IsAnyErfTypeFile(fileName))
                            {
                                var erf = ERFAuto.ReadErf(resource.FilePath);
                                mdlData = erf.Get(resource.ResName, ResourceType.MDL);
                            }
                            else if (BioWare.Tools.FileHelpers.IsRimFile(fileName))
                            {
                                var rim = RIMAuto.ReadRim(resource.FilePath);
                                mdlData = rim.Get(resource.ResName, ResourceType.MDL);
                            }
                            else if (BioWare.Tools.FileHelpers.IsBifFile(fileName))
                            {
                                var mdlRes = _installation.Installation.Resources.LookupResource(resource.ResName, ResourceType.MDL);
                                if (mdlRes != null && mdlRes.Data != null)
                                {
                                    mdlData = mdlRes.Data;
                                }
                            }
                            else
                            {
                                string mdlPath = Path.ChangeExtension(resource.FilePath, ".mdl");
                                if (File.Exists(mdlPath))
                                {
                                    mdlData = File.ReadAllBytes(mdlPath);
                                }
                            }
                        }

                        if (mdlData != null && mdxData != null && _previewRenderer != null)
                        {
                            _previewRenderer.SetModel(mdlData, mdxData);
                        }
                        else if (_previewRenderer != null)
                        {
                            _previewRenderer.ClearModel();
                        }
                    }
                    else
                    {
                        // Clear preview if preview is not enabled for this resource type
                        if (_previewRenderer != null)
                        {
                            _previewRenderer.ClearModel();
                        }
                    }
                }
            }
        }

        private void SetRenderModel(string modelName)
        {
            if (string.IsNullOrEmpty(modelName) || _installation == null || _previewRenderer == null)
            {
                return;
            }

            var mdlRes = _installation.Installation.Resources.LookupResource(modelName, ResourceType.MDL);
            var mdxRes = _installation.Installation.Resources.LookupResource(modelName, ResourceType.MDX);

            if (mdlRes != null && mdlRes.Data != null && mdxRes != null && mdxRes.Data != null)
            {
                _previewRenderer.SetModel(mdlRes.Data, mdxRes.Data);
            }
            else
            {
                _previewRenderer.ClearModel();
            }
        }

        private string GenerateResourceSummary(FileResource resource)
        {
            if (resource == null)
            {
                return "No resource selected";
            }

            var summary = new System.Collections.Generic.List<string>
            {
                $"Name: {resource.ResName}",
                $"Type: {resource.ResType.Name}",
                $"Size: {resource.GetData().Length} bytes"
            };

            // Calculate relative path from installation path
            if (_installation != null && !string.IsNullOrEmpty(_installation.Path) && !string.IsNullOrEmpty(resource.FilePath))
            {
                try
                {
                    string installationPath = Path.GetFullPath(_installation.Path);
                    string resourcePath = Path.GetFullPath(resource.FilePath);

                    // Check if resource path is within installation path
                    if (resourcePath.StartsWith(installationPath, StringComparison.OrdinalIgnoreCase))
                    {
                        // Use Substring for .NET Framework 4.x compatibility (Path.GetRelativePath requires .NET Core 2.1+)
                        string relativePath = resourcePath.Substring(installationPath.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        summary.Add($"Path: {relativePath}");
                    }
                    else
                    {
                        // If not relative, show full path
                        summary.Add($"Path: {resource.FilePath}");
                    }
                }
                catch
                {
                    // If path calculation fails, just show the file path
                    summary.Add($"Path: {resource.FilePath}");
                }
            }
            else
            {
                summary.Add($"Path: {resource.FilePath}");
            }

            return string.Join("\n", summary);
        }

        private void OnResrefEdited(string text)
        {
            if (_okButton != null)
            {
                _okButton.IsEnabled = IsValidResref(text);
            }
        }

        private void OnResourceFilterChanged()
        {
            string filterText = _resourceFilter?.Text?.ToLowerInvariant() ?? "";

            // Set filter on the CollectionViewSource
            // Filter resources based on whether their ResName contains the filter text (case-insensitive)
            _filteredResources.View.Filter = item =>
            {
                if (item is FileResource resource)
                {
                    // Show all items if filter is empty, otherwise check if resource name contains filter text
                    return string.IsNullOrEmpty(filterText) ||
                           resource.ResName.ToLowerInvariant().Contains(filterText);
                }
                return false;
            };

            // Refresh the view to apply the filter
            _filteredResources.View.Refresh();
        }

        private bool IsValidResref(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            // Validate ResRef format first
            if (!ResRef.IsValid(text))
            {
                return false;
            }

            // Check if resource already exists in module
            // A resref is valid for insertion if the resource does NOT already exist in the module
            if (_module != null)
            {
                var existingResource = _module.Resource(text, _restype);
                if (existingResource != null)
                {
                    // Resource already exists in module, so this resref is not valid for insertion
                    return false;
                }
            }

            // ResRef format is valid and resource doesn't exist in module
            return true;
        }

        public string ResName => _resname;
        public byte[] Data => _data;
        public string Filepath => _filepath;
    }
}
