using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BioWare.Resource.Formats.TPC;
using BioWare.Common;
using BioWare.Resource;
using OdyTools.Data;

namespace OdyTools.Editors
{
    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/editors/tpc.py:48
    // Original: class OdyToolTPC(Editor):
    public partial class OdyToolTPC : Editor
    {
        private TPC _tpc;

        public OdyToolTPC(Window parent = null, OdyInstallation installation = null)
            : base(parent, "Texture Viewer", "none",
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

        private void LoadTPC(TPC tpc)
        {
            // Load TPC data into UI
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
            Save();
        }
    }
}
