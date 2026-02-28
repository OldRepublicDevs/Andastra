using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using BioWare.Resource.Formats.TPC;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Data;

namespace OdyTools.Editors
{
    public partial class OdyToolTPC : Editor
    {
        private TPC _tpc;

        public OdyToolTPC(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolTPC", "none",
                new[] { ResourceType.TPC, ResourceType.TGA, ResourceType.JPG, ResourceType.PNG, ResourceType.BMP },
                new[] { ResourceType.TPC, ResourceType.TGA, ResourceType.JPG, ResourceType.PNG, ResourceType.BMP },
                installation)
        {
            InitializeComponent();
            SetupUI();
            New();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetupUI()
        {
            // Keep XAML content (menu bar and main area); do not replace Content so Exit menu works.
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _tpc = TPCAuto.ReadTpc(data);
            LoadTPC(_tpc);
        }

        /// <summary>
        /// Loads TPC into editor state. This editor supports open/save only; texture preview/editing UI can be added later.
        /// </summary>
        private void LoadTPC(TPC tpc)
        {
            _tpc = tpc ?? _tpc;
        }

        public override Tuple<byte[], byte[]> Build()
        {
            ResourceType tpcType = _restype ?? ResourceType.TPC;
            byte[] data = TPCAuto.BytesTpc(_tpc, tpcType);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _tpc = new TPC();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        protected override async Task RunSaveAsAsync()
        {
            var storage = StorageProvider;
            if (storage == null) return;
            string suggestedName = !string.IsNullOrEmpty(_resname) ? _resname : "texture";
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".tpc",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Texture (TPC)") { Patterns = new[] { "*.tpc" } },
                    new FilePickerFileType("TGA") { Patterns = new[] { "*.tga" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
                }
            };
            var file = await storage.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            string ext = (Path.GetExtension(path) ?? "").TrimStart('.').ToLowerInvariant();
            _restype = ResourceType.FromExtension(ext) ?? ResourceType.TPC;
            _resname = Path.GetFileNameWithoutExtension(path);
            RefreshWindowTitle();
            Save();
        }
    }
}
