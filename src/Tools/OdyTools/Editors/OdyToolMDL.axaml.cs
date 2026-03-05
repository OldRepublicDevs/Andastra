using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
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

namespace OdyTools.Editors
{
    public partial class OdyToolMDL : Editor
    {
        private MDL _mdl;
        private ModelRenderer _modelRenderer;

        public OdyToolMDL() : this(null, null) { }
        public OdyToolMDL(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolMDL", "none",
                new[] { ResourceType.MDL, ResourceType.MDL_ASCII },
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

            // Set Content after AddHelpAction (which may wrap it in a DockPanel)
            if (Content == null && _modelRenderer != null)
            {
                SetContentOrInject(_modelRenderer);
            }

            New();
        }

        private void InitializeComponent()
        {
            try
            {
                AvaloniaXamlLoader.Load(this);
                _modelRenderer = this.FindControl<ModelRenderer>("modelRenderer");
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
        }

        private void LoadMDL(MDL mdl)
        {
            _mdl = mdl;
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
