using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using BioWare.Common;
using BioWare.Resource.Formats.BWM;
using OdyTools.Data;

namespace OdyTools.Editors
{
    public partial class OdyToolBWM : Editor
    {
        private BWM _bwm;

        public OdyToolBWM(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolBWM", "walkmesh",
                new[] { ResourceType.WOK, ResourceType.DWK, ResourceType.PWK },
                new[] { ResourceType.WOK, ResourceType.DWK, ResourceType.PWK },
                installation)
        {
            InitializeComponent();
            New();
        }

        private void InitializeComponent()
        {
            Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
            SetupUI();
        }

        private void SetupUI()
        {
            var panel = new StackPanel();
            SetContentOrInject(panel);
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            _bwm = BWMAuto.ReadBwm(data);
            LoadBWM(_bwm);
        }

        /// <summary>
        /// Loads BWM into editor state. This editor supports open/save only; no visual walkmesh renderer is implemented.
        /// </summary>
        private void LoadBWM(BWM bwm)
        {
            _bwm = bwm ?? _bwm;
        }

        public override Tuple<byte[], byte[]> Build()
        {
            ResourceType bwmType = _restype ?? ResourceType.WOK;
            byte[] data = BWMAuto.BytesBwm(_bwm, bwmType);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _bwm = new BWM();
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }

        protected override async Task RunSaveAsAsync()
        {
            var storage = StorageProvider;
            if (storage == null) return;
            string suggestedName = !string.IsNullOrEmpty(_resname) ? _resname : "walkmesh";
            var options = new FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = suggestedName + ".wok",
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("Walkmesh (WOK)") { Patterns = new[] { "*.wok" } },
                    new FilePickerFileType("Door walkmesh (DWK)") { Patterns = new[] { "*.dwk" } },
                    new FilePickerFileType("Placeable walkmesh (PWK)") { Patterns = new[] { "*.pwk" } },
                    new FilePickerFileType("All files") { Patterns = new[] { "*.*" } }
                }
            };
            var file = await storage.SaveFilePickerAsync(options);
            if (file == null) return;
            string path = file.Path?.LocalPath ?? "";
            if (string.IsNullOrWhiteSpace(path)) return;
            _filepath = path;
            string ext = (Path.GetExtension(path) ?? "").TrimStart('.').ToLowerInvariant();
            _restype = ResourceType.FromExtension(ext) ?? ResourceType.WOK;
            _resname = Path.GetFileNameWithoutExtension(path);
            RefreshWindowTitle();
            Save();
        }
    }
}
