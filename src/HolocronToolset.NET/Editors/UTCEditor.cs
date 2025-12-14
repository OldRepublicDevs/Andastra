using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utc.py:51
    // Original: class UTCEditor(Editor):
    public class UTCEditor : Editor
    {
        private UTC _utc;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utc.py:52-105
        // Original: def __init__(self, parent, installation):
        public UTCEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Creature Editor", "creature",
                new[] { ResourceType.UTC, ResourceType.BTC, ResourceType.BIC },
                new[] { ResourceType.UTC, ResourceType.BTC, ResourceType.BIC },
                installation)
        {
            InitializeComponent();
            MinWidth = 798;
            MinHeight = 553;
            New();
        }

        private void InitializeComponent()
        {
            if (!TryLoadXaml())
            {
                SetupProgrammaticUI();
            }
        }

        private void SetupProgrammaticUI()
        {
            // Initialize UI controls programmatically
            var panel = new StackPanel();
            Content = panel;
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utc.py:200-300
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            // UTC is a GFF-based format
            var gff = GFF.FromBytes(data);
            // UTC conversion will be implemented when UTC conversion methods are available
            _utc = new UTC();
            LoadUTC(_utc);
        }

        private void LoadUTC(UTC utc)
        {
            // Load UTC data into UI
            // This will be implemented when UI controls are created
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utc.py:400-500
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            // Build UTC from UI
            // This will be implemented when UI controls are created
            // UTC conversion will be implemented when UTC conversion methods are available
            var gff = new GFF(GFFContent.UTC);
            byte[] data = gff.ToBytes();
            return Tuple.Create(data, new byte[0]);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utc.py:502-510
        // Original: def new(self):
        public override void New()
        {
            base.New();
            _utc = new UTC();
            // Clear UI
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
