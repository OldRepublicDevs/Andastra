using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/uti.py:45
    // Original: class UTIEditor(Editor):
    public class UTIEditor : Editor
    {
        private UTI _uti;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/uti.py:46-87
        // Original: def __init__(self, parent, installation):
        public UTIEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Item Editor", "item",
                new[] { ResourceType.UTI },
                new[] { ResourceType.UTI },
                installation)
        {
            InitializeComponent();
            SetupUI();
            MinWidth = 700;
            MinHeight = 350;
            New();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetupUI()
        {
            // Setup UI elements - will be implemented when UI controls are created
            var panel = new StackPanel();
            Content = panel;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/uti.py:200-300
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            // UTI is a GFF-based format
            var gff = GFF.FromBytes(data);
            // UTI conversion will be implemented when UTI conversion methods are available
            _uti = new UTI();
            LoadUTI(_uti);
        }

        private void LoadUTI(UTI uti)
        {
            // Load UTI data into UI
            // This will be implemented when UI controls are created
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/uti.py:400-500
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            // Build UTI from UI
            // This will be implemented when UI controls are created
            // UTI conversion will be implemented when UTI conversion methods are available
            var gff = new GFF(GFFContent.UTI);
            byte[] data = gff.ToBytes();
            return Tuple.Create(data, new byte[0]);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/uti.py:502-510
        // Original: def new(self):
        public override void New()
        {
            base.New();
            _uti = new UTI();
            // Clear UI
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
