using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using BioWare.Extract;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.MDL;
using BioWare.Resource.Formats.MDLData;
using BioWare.Resource.Formats.RIM;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Data;
using OdyTools.Utils;
using OdyTools.Widgets;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using IconType = MsBox.Avalonia.Enums.Icon;
using MediaColor = Avalonia.Media.Color;

namespace OdyTools.Editors
{
    public partial class OdyToolMDL : Editor
    {
        private MDL _mdl;
        private ModelRenderer _modelRenderer;
        private TextBlock _summaryText;
        private TextBlock _detailsText;
        private TextBox _nameEdit;
        private TextBox _supermodelEdit;
        private ComboBox _classificationEdit;
        private TabControl _inspectorTabs;
        private ListBox _nodeList;
        private ListBox _textureList;
        private ListBox _animationList;
        private TextBox _animationNameEdit;
        private NumericUpDown _animationLengthEdit;
        private NumericUpDown _animationTransitionEdit;
        private bool _updatingInspector;

        public OdyToolMDL() : this(null, null) { }
        public OdyToolMDL(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolMDL", "none",
                new[] { ResourceType.MDL, ResourceType.MDX, ResourceType.MDL_ASCII },
                new[] { ResourceType.MDL, ResourceType.MDL_ASCII },
                installation)
        {
            _installation = installation;
            _mdl = new MDL();

            InitializeComponent();
            SetupUI();
            SetupSignals();

            if (_modelRenderer != null)
            {
                _modelRenderer.Installation = installation;
            }

            AddHelpAction();

            // Set content after AddHelpAction; the help menu may wrap the XAML contentRoot in a DockPanel.
            if (_modelRenderer != null)
            {
                SetContentOrInject(BuildInspectorLayout());
            }

            New();
        }

        private void InitializeComponent()
        {
            try
            {
                AvaloniaXamlLoader.Load(this);
                _modelRenderer = EditorHelpers.FindControlSafe<ModelRenderer>(this, "modelRenderer");
            }
            catch { /* XAML not available - use programmatic UI */ }
            SetupUI();
        }

        private void SetupUI()
        {
            // Create model renderer if not found from XAML
            if (_modelRenderer == null)
            {
                _modelRenderer = new ModelRenderer();
            }
            // Don't set Content here - AddHelpAction will wrap it in a DockPanel if needed
            // Set Content after AddHelpAction is called
        }

        private void SetupSignals()
        {
            // Signals setup - currently empty in Python implementation
        }

        private Control BuildInspectorLayout()
        {
            var root = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("340,*"),
                RowDefinitions = new RowDefinitions("*"),
                Background = new SolidColorBrush(MediaColor.FromRgb(243, 245, 249)),
                Margin = new Avalonia.Thickness(10)
            };

            var inspector = new DockPanel
            {
                LastChildFill = true,
                Margin = new Avalonia.Thickness(0, 0, 10, 0)
            };

            _summaryText = new TextBlock
            {
                FontWeight = FontWeight.SemiBold,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Avalonia.Thickness(0, 0, 0, 8)
            };
            DockPanel.SetDock(_summaryText, Dock.Top);
            inspector.Children.Add(_summaryText);

            var metadataPanel = new StackPanel
            {
                Spacing = 6,
                Margin = new Avalonia.Thickness(0, 0, 0, 10)
            };
            metadataPanel.Children.Add(MakeLabel("Name"));
            _nameEdit = new TextBox();
            _nameEdit.LostFocus += (s, e) => ApplyMetadataEdits();
            metadataPanel.Children.Add(_nameEdit);

            metadataPanel.Children.Add(MakeLabel("Supermodel"));
            _supermodelEdit = new TextBox();
            _supermodelEdit.LostFocus += (s, e) => ApplyMetadataEdits();
            metadataPanel.Children.Add(_supermodelEdit);

            metadataPanel.Children.Add(MakeLabel("Classification"));
            _classificationEdit = new ComboBox
            {
                ItemsSource = Enum.GetValues(typeof(MDLClassification)).Cast<MDLClassification>().ToList()
            };
            _classificationEdit.SelectionChanged += (s, e) => ApplyMetadataEdits();
            metadataPanel.Children.Add(_classificationEdit);
            DockPanel.SetDock(metadataPanel, Dock.Top);
            inspector.Children.Add(metadataPanel);

            _inspectorTabs = new TabControl();
            _inspectorTabs.SelectionChanged += (s, e) => RefreshSelectedDetails();
            _nodeList = MakeListBox();
            _nodeList.SelectionChanged += (s, e) => RefreshSelectedDetails();
            _textureList = MakeListBox();
            _textureList.SelectionChanged += (s, e) => RefreshSelectedDetails();
            _animationList = MakeListBox();
            _animationList.SelectionChanged += (s, e) => RefreshSelectedDetails();
            _inspectorTabs.Items.Add(new TabItem { Header = "Nodes", Content = _nodeList });
            _inspectorTabs.Items.Add(new TabItem { Header = "Textures", Content = _textureList });
            _inspectorTabs.Items.Add(new TabItem { Header = "Animations", Content = _animationList });
            inspector.Children.Add(_inspectorTabs);

            var animationPanel = new StackPanel
            {
                Spacing = 6,
                Margin = new Avalonia.Thickness(0, 8, 0, 8)
            };
            animationPanel.Children.Add(MakeLabel("Selected animation"));
            animationPanel.Children.Add(MakeLabel("Name"));
            _animationNameEdit = new TextBox { Watermark = "Animation name" };
            _animationNameEdit.LostFocus += (s, e) => ApplyAnimationEdits();
            animationPanel.Children.Add(_animationNameEdit);

            var animationNumbers = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions("*,*"),
                RowDefinitions = new RowDefinitions("Auto,Auto"),
                ColumnSpacing = 8,
                RowSpacing = 4
            };
            var lengthLabel = MakeLabel("Length (s)");
            var transitionLabel = MakeLabel("Transition (s)");
            Grid.SetColumn(lengthLabel, 0);
            Grid.SetColumn(transitionLabel, 1);
            animationNumbers.Children.Add(lengthLabel);
            animationNumbers.Children.Add(transitionLabel);
            _animationLengthEdit = MakeAnimationNumberBox();
            _animationTransitionEdit = MakeAnimationNumberBox();
            _animationLengthEdit.ValueChanged += (s, e) => ApplyAnimationEdits();
            _animationTransitionEdit.ValueChanged += (s, e) => ApplyAnimationEdits();
            Grid.SetRow(_animationLengthEdit, 1);
            Grid.SetColumn(_animationLengthEdit, 0);
            Grid.SetRow(_animationTransitionEdit, 1);
            Grid.SetColumn(_animationTransitionEdit, 1);
            animationNumbers.Children.Add(_animationLengthEdit);
            animationNumbers.Children.Add(_animationTransitionEdit);
            animationPanel.Children.Add(animationNumbers);
            DockPanel.SetDock(animationPanel, Dock.Bottom);
            inspector.Children.Add(animationPanel);

            _detailsText = new TextBlock
            {
                TextWrapping = TextWrapping.Wrap,
                Padding = new Avalonia.Thickness(8),
                Background = new SolidColorBrush(MediaColor.FromRgb(232, 236, 243)),
                MinHeight = 72
            };
            DockPanel.SetDock(_detailsText, Dock.Bottom);
            inspector.Children.Add(_detailsText);

            root.Children.Add(inspector);

            Grid.SetColumn(_modelRenderer, 1);
            root.Children.Add(_modelRenderer);

            return root;
        }

        private static TextBlock MakeLabel(string text)
        {
            return new TextBlock
            {
                Text = text,
                FontWeight = FontWeight.SemiBold,
                Foreground = new SolidColorBrush(MediaColor.FromRgb(47, 55, 68))
            };
        }

        private static ListBox MakeListBox()
        {
            return new ListBox
            {
                MinHeight = 220
            };
        }

        private static NumericUpDown MakeAnimationNumberBox()
        {
            return new NumericUpDown
            {
                Minimum = 0,
                Maximum = 99999,
                Increment = 0.1m,
                FormatString = "0.###"
            };
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            byte[] mdlData = null;
            byte[] mdxData = null;

            // ASCII MDL: single file, no MDX; load via MDLAuto and compile to binary for 3D preview.
            ResourceType detected = (restype == ResourceType.MDL && data != null && data.Length >= 4)
                ? MDLAuto.DetectMdl(data, 0) : restype;
            if (restype == ResourceType.MDL_ASCII || detected == ResourceType.MDL_ASCII)
            {
                _mdl = MDLAuto.ReadMdl(data, 0, null, null, 0, 0, ResourceType.MDL_ASCII);
                _restype = ResourceType.MDL_ASCII;
                if (_modelRenderer != null)
                {
                    // Render preview from an in-memory binary conversion; save format remains ASCII.
                    using (var previewMdl = new MemoryStream())
                    using (var previewMdx = new MemoryStream())
                    {
                        MDLAuto.WriteMdl(_mdl, previewMdl, ResourceType.MDL, previewMdx);
                        _modelRenderer.SetModel(previewMdl.ToArray(), previewMdx.ToArray());
                    }
                }
                RefreshInspector();
                return;
            }

            if (restype == ResourceType.MDL)
            {
                mdlData = data;
                string filepathLower = filepath.ToLowerInvariant();
                if (filepathLower.EndsWith(".mdl"))
                {
                    string mdxPath = Path.ChangeExtension(filepath, ".mdx");
                    if (File.Exists(mdxPath))
                    {
                        mdxData = File.ReadAllBytes(mdxPath);
                    }
                }
                else if (BioWare.Tools.FileHelpers.IsAnyErfTypeFile(filepath))
                {
                    ERF erf = ERFAuto.ReadErf(filepath);
                    mdxData = erf.Get(resref, ResourceType.MDX);
                }
                else if (BioWare.Tools.FileHelpers.IsRimFile(filepath))
                {
                    RIM rim = RIMAuto.ReadRim(filepath);
                    mdxData = rim.Get(resref, ResourceType.MDX);
                }
                else if (BioWare.Tools.FileHelpers.IsBifFile(filepath))
                {
                    if (_installation != null)
                    {
                        var result = _installation.Resource(resref, ResourceType.MDX, new[] { SearchLocation.CHITIN });
                        if (result != null && result.Data != null)
                        {
                            mdxData = result.Data;
                        }
                    }
                }
            }
            else if (restype == ResourceType.MDX)
            {
                mdxData = data;
                string filepathLower = filepath.ToLowerInvariant();
                if (filepathLower.EndsWith(".mdx"))
                {
                    string mdlPath = Path.ChangeExtension(filepath, ".mdl");
                    if (File.Exists(mdlPath))
                    {
                        mdlData = File.ReadAllBytes(mdlPath);
                    }
                }
                else if (BioWare.Tools.FileHelpers.IsAnyErfTypeFile(filepath))
                {
                    ERF erf = ERFAuto.ReadErf(filepath);
                    mdlData = erf.Get(resref, ResourceType.MDL);
                }
                else if (BioWare.Tools.FileHelpers.IsRimFile(filepath))
                {
                    RIM rim = RIMAuto.ReadRim(filepath);
                    mdlData = rim.Get(resref, ResourceType.MDL);
                }
                else if (BioWare.Tools.FileHelpers.IsBifFile(filepath))
                {
                    if (_installation != null)
                    {
                        var result = _installation.Resource(resref, ResourceType.MDL, new[] { SearchLocation.CHITIN });
                        if (result != null && result.Data != null)
                        {
                            mdlData = result.Data;
                        }
                    }
                }
            }

            if (mdlData == null || mdxData == null)
            {
                _ = DialogHelper.ShowAsync($"Could not find the '{resref}' MDL/MDX", "", ButtonEnum.Ok, IconType.Error);
                return;
            }

            if (_modelRenderer != null)
            {
                // IMPLEMENTED: Now properly handles MDL header skipping (data[12:]) like Python implementation
                // The ModelRenderer.SetModel now parses starting at offset 12 to skip the 12-byte file header
                _modelRenderer.SetModel(mdlData, mdxData);
            }

            _mdl = MDLAuto.ReadMdl(mdlData, 0, null, mdxData, 0, 0);
            RefreshInspector();
        }

        private void LoadMDL(MDL mdl)
        {
            _mdl = mdl;
            RefreshInspector();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            if (_restype == ResourceType.MDL_ASCII)
            {
                byte[] data = MDLAuto.BytesMdl(_mdl, ResourceType.MDL_ASCII);
                return Tuple.Create(data, new byte[0]);
            }
            byte[] dataBin = new byte[0];
            byte[] dataExt = new byte[0];
            using (var ms = new MemoryStream())
            using (var msExt = new MemoryStream())
            {
                MDLAuto.WriteMdl(_mdl, ms, ResourceType.MDL, msExt);
                dataBin = ms.ToArray();
                dataExt = msExt.ToArray();
            }
            return Tuple.Create(dataBin, dataExt);
        }

        protected override IReadOnlyList<SaveArtifact> BuildSaveArtifactsForPath(string path)
        {
            var (data, dataExt) = Build();
            if (data == null)
            {
                return Array.Empty<SaveArtifact>();
            }

            var artifacts = new List<SaveArtifact>
            {
                new SaveArtifact(path, data, CreateBackupsOnSave, Math.Max(1, BackupCount))
            };

            if (_restype == ResourceType.MDL && dataExt != null && dataExt.Length > 0)
            {
                string mdxPath = Path.ChangeExtension(path, ".mdx");
                artifacts.Add(new SaveArtifact(mdxPath, dataExt, CreateBackupsOnSave, Math.Max(1, BackupCount)));
            }

            return artifacts;
        }

        public override void New()
        {
            base.New();
            _mdl = new MDL();
            if (_modelRenderer != null)
            {
                _modelRenderer.ClearModel();
            }
            RefreshInspector();
        }

        private void RefreshInspector()
        {
            if (_summaryText == null)
            {
                return;
            }

            _updatingInspector = true;
            try
            {
                var nodes = _mdl?.AllNodes() ?? new List<MDLNode>();
                var textures = _mdl?.AllTextures().OrderBy(name => name, StringComparer.OrdinalIgnoreCase).ToList() ?? new List<string>();
                var animations = _mdl?.Anims?.Select(anim => string.IsNullOrWhiteSpace(anim.Name) ? "(unnamed)" : anim.Name).ToList() ?? new List<string>();

                _summaryText.Text = string.Format(
                    "Nodes: {0} | Textures: {1} | Animations: {2}",
                    nodes.Count,
                    textures.Count,
                    animations.Count);
                if (_nameEdit != null) _nameEdit.Text = _mdl?.Name ?? string.Empty;
                if (_supermodelEdit != null) _supermodelEdit.Text = _mdl?.Supermodel ?? string.Empty;
                if (_classificationEdit != null) _classificationEdit.SelectedItem = _mdl != null ? _mdl.Classification : MDLClassification.OTHER;

                if (_nodeList != null)
                {
                    _nodeList.ItemsSource = nodes.Select(FormatNodeListItem).ToList();
                    _nodeList.SelectedIndex = nodes.Count > 0 ? 0 : -1;
                }

                if (_textureList != null)
                {
                    _textureList.ItemsSource = textures;
                    _textureList.SelectedIndex = textures.Count > 0 ? 0 : -1;
                }

                if (_animationList != null)
                {
                    _animationList.ItemsSource = animations;
                    _animationList.SelectedIndex = animations.Count > 0 ? 0 : -1;
                }
            }
            finally
            {
                _updatingInspector = false;
            }

            RefreshSelectedDetails();
        }

        private static string FormatNodeListItem(MDLNode node)
        {
            if (node == null)
            {
                return string.Empty;
            }

            string name = string.IsNullOrWhiteSpace(node.Name) ? "(unnamed)" : node.Name;
            return string.Format("{0}  [{1}]", name, node.NodeType);
        }

        private void RefreshSelectedDetails()
        {
            if (_detailsText == null)
            {
                return;
            }

            int selectedTab = _inspectorTabs?.SelectedIndex ?? 0;
            if (selectedTab == 0 && _nodeList?.SelectedIndex >= 0)
            {
                var nodes = _mdl?.AllNodes() ?? new List<MDLNode>();
                if (_nodeList.SelectedIndex < nodes.Count)
                {
                    var node = nodes[_nodeList.SelectedIndex];
                    string texture = node.Mesh != null ? node.Mesh.Texture1 : string.Empty;
                    _detailsText.Text = string.Format(
                        "Node: {0}\nType: {1}\nChildren: {2}\nTexture: {3}",
                        string.IsNullOrWhiteSpace(node.Name) ? "(unnamed)" : node.Name,
                        node.NodeType,
                        node.Children?.Count ?? 0,
                        string.IsNullOrWhiteSpace(texture) ? "None" : texture);
                    return;
                }
            }

            if (selectedTab == 1 && _textureList?.SelectedItem is string textureName)
            {
                _detailsText.Text = "Texture: " + textureName;
                RefreshAnimationEditor(null);
                return;
            }

            if (selectedTab == 2 && _animationList?.SelectedIndex >= 0 && _mdl?.Anims != null && _animationList.SelectedIndex < _mdl.Anims.Count)
            {
                var anim = _mdl.Anims[_animationList.SelectedIndex];
                RefreshAnimationEditor(anim);
                _detailsText.Text = string.Format(
                    "Animation: {0}\nLength: {1:0.###}s\nTransition: {2:0.###}s\nEvents: {3}",
                    string.IsNullOrWhiteSpace(anim.Name) ? "(unnamed)" : anim.Name,
                    anim.AnimLength,
                    anim.TransitionLength,
                    anim.Events?.Count ?? 0);
                return;
            }

            RefreshAnimationEditor(null);
            _detailsText.Text = "No model element selected";
        }

        private void RefreshAnimationEditor(MDLAnimation animation)
        {
            bool previousUpdating = _updatingInspector;
            _updatingInspector = true;
            try
            {
                bool enabled = animation != null;
                if (_animationNameEdit != null)
                {
                    _animationNameEdit.IsEnabled = enabled;
                    _animationNameEdit.Text = animation?.Name ?? string.Empty;
                }

                if (_animationLengthEdit != null)
                {
                    _animationLengthEdit.IsEnabled = enabled;
                    _animationLengthEdit.Value = animation != null ? (decimal)animation.AnimLength : 0m;
                }

                if (_animationTransitionEdit != null)
                {
                    _animationTransitionEdit.IsEnabled = enabled;
                    _animationTransitionEdit.Value = animation != null ? (decimal)animation.TransitionLength : 0m;
                }
            }
            finally
            {
                _updatingInspector = previousUpdating;
            }
        }

        private void ApplyMetadataEdits()
        {
            if (_updatingInspector || _mdl == null)
            {
                return;
            }

            string name = _nameEdit?.Text ?? string.Empty;
            string supermodel = _supermodelEdit?.Text ?? string.Empty;
            var classification = _classificationEdit?.SelectedItem is MDLClassification selected
                ? selected
                : _mdl.Classification;

            bool changed = _mdl.Name != name
                || _mdl.Supermodel != supermodel
                || _mdl.Classification != classification;
            if (!changed)
            {
                return;
            }

            _mdl.Name = name;
            _mdl.Supermodel = supermodel;
            _mdl.Classification = classification;
            MarkDocumentDirty();
            RefreshInspector();
        }

        private void ApplyAnimationEdits()
        {
            if (_updatingInspector || _mdl?.Anims == null || _animationList == null)
            {
                return;
            }

            int index = _animationList.SelectedIndex;
            if (index < 0 || index >= _mdl.Anims.Count)
            {
                return;
            }

            var animation = _mdl.Anims[index];
            string name = _animationNameEdit?.Text ?? string.Empty;
            float length = (float)(_animationLengthEdit?.Value ?? 0m);
            float transition = (float)(_animationTransitionEdit?.Value ?? 0m);
            bool changed = animation.Name != name
                || !animation.AnimLength.Equals(length)
                || !animation.TransitionLength.Equals(transition);
            if (!changed)
            {
                return;
            }

            animation.Name = name;
            animation.AnimLength = length;
            animation.TransitionLength = transition;
            MarkDocumentDirty();

            bool previousUpdating = _updatingInspector;
            _updatingInspector = true;
            try
            {
                var animations = _mdl.Anims
                    .Select(anim => string.IsNullOrWhiteSpace(anim.Name) ? "(unnamed)" : anim.Name)
                    .ToList();
                _animationList.ItemsSource = animations;
                _animationList.SelectedIndex = index;
            }
            finally
            {
                _updatingInspector = previousUpdating;
            }

            RefreshSelectedDetails();
        }

        internal string ModelSummaryForTests => _summaryText?.Text ?? string.Empty;
        internal string SelectedModelDetailsForTests => _detailsText?.Text ?? string.Empty;
        internal IReadOnlyList<string> NodeNamesForTests => (_mdl?.AllNodes() ?? new List<MDLNode>())
            .Select(node => string.IsNullOrWhiteSpace(node.Name) ? "(unnamed)" : node.Name)
            .ToList();
        internal IReadOnlyList<string> TextureNamesForTests => (_mdl?.AllTextures() ?? new HashSet<string>())
            .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
            .ToList();
        internal string ModelNameForTests => _mdl?.Name ?? string.Empty;
        internal string ModelSupermodelForTests => _mdl?.Supermodel ?? string.Empty;
        internal IReadOnlyList<string> AnimationNamesForTests => (_mdl?.Anims ?? new List<MDLAnimation>())
            .Select(anim => string.IsNullOrWhiteSpace(anim.Name) ? "(unnamed)" : anim.Name)
            .ToList();

        internal void SelectNodeForTests(string nodeName)
        {
            var nodes = _mdl?.AllNodes() ?? new List<MDLNode>();
            int index = nodes.FindIndex(node => string.Equals(node.Name, nodeName, StringComparison.OrdinalIgnoreCase));
            if (index >= 0 && _nodeList != null)
            {
                if (_inspectorTabs != null) _inspectorTabs.SelectedIndex = 0;
                _nodeList.SelectedIndex = index;
                RefreshSelectedDetails();
            }
        }

        internal void SelectTextureForTests(string textureName)
        {
            var textures = TextureNamesForTests;
            int index = textures.ToList().FindIndex(texture => string.Equals(texture, textureName, StringComparison.OrdinalIgnoreCase));
            if (index >= 0 && _textureList != null)
            {
                if (_inspectorTabs != null) _inspectorTabs.SelectedIndex = 1;
                _textureList.SelectedIndex = index;
                RefreshSelectedDetails();
            }
        }

        internal void SelectAnimationForTests(string animationName)
        {
            var animations = _mdl?.Anims ?? new List<MDLAnimation>();
            int index = animations.FindIndex(animation => string.Equals(animation.Name, animationName, StringComparison.OrdinalIgnoreCase));
            if (index >= 0 && _animationList != null)
            {
                if (_inspectorTabs != null) _inspectorTabs.SelectedIndex = 2;
                _animationList.SelectedIndex = index;
                RefreshSelectedDetails();
            }
        }

        internal void EditSelectedAnimationForTests(string name = null, float? length = null, float? transition = null)
        {
            if (name != null && _animationNameEdit != null) _animationNameEdit.Text = name;
            if (length.HasValue && _animationLengthEdit != null) _animationLengthEdit.Value = (decimal)length.Value;
            if (transition.HasValue && _animationTransitionEdit != null) _animationTransitionEdit.Value = (decimal)transition.Value;
            ApplyAnimationEdits();
        }

        internal void EditMetadataForTests(string name = null, string supermodel = null, MDLClassification? classification = null)
        {
            if (name != null && _nameEdit != null) _nameEdit.Text = name;
            if (supermodel != null && _supermodelEdit != null) _supermodelEdit.Text = supermodel;
            if (classification.HasValue && _classificationEdit != null) _classificationEdit.SelectedItem = classification.Value;
            ApplyMetadataEdits();
        }

        internal void LoadModelForTests(MDL mdl, ResourceType restype)
        {
            _mdl = mdl ?? new MDL();
            _restype = restype;
            RefreshInspector();
            ClearDirty();
        }

        protected override FilePickerSaveOptions CreateSaveAsOptions()
        {
            string suggestedName = !string.IsNullOrEmpty(_resname) ? _resname : "model";
            return new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".mdl",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Model (MDL binary)") { Patterns = new[] { "*.mdl" } },
                    new FilePickerFileType("Model (MDL ASCII)") { Patterns = new[] { "*.mdl.ascii" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
                }
            };
        }

        protected override bool TryResolveSaveIdentity(string path, out string resname, out ResourceType restype)
        {
            string pathLower = path.ToLowerInvariant();
            if (pathLower.EndsWith(".mdl.ascii"))
            {
                restype = ResourceType.MDL_ASCII;
                string namePart = Path.GetFileName(path);
                resname = namePart.EndsWith(".mdl.ascii", StringComparison.OrdinalIgnoreCase)
                    ? namePart.Substring(0, namePart.Length - ".mdl.ascii".Length) : Path.GetFileNameWithoutExtension(path);
                return true;
            }

            bool resolved = base.TryResolveSaveIdentity(path, out resname, out restype);
            if (!resolved || restype == null)
            {
                restype = ResourceType.MDL;
                resname = Path.GetFileNameWithoutExtension(path);
                return true;
            }

            return true;
        }
    }
}
