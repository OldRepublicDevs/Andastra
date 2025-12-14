using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utd.py:32
    // Original: class UTDEditor(Editor):
    public class UTDEditor : Editor
    {
        private UTD _utd;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utd.py:33-82
        // Original: def __init__(self, parent, installation):
        public UTDEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Door Editor", "door",
                new[] { ResourceType.UTD, ResourceType.BTD },
                new[] { ResourceType.UTD, ResourceType.BTD },
                installation)
        {
            InitializeComponent();
            SetupUI();
            Width = 654;
            Height = 495;
            New();
        }

        private void InitializeComponent()
        {
            if (!TryLoadXaml())
            {
                SetupUI();
            }
        }

        private void SetupUI()
        {
            // Setup UI elements - will be implemented when UI controls are created
            var panel = new StackPanel();
            Content = panel;
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            var gff = GFF.FromBytes(data);
            // UTD conversion will be implemented when UTD conversion methods are available
            _utd = new UTD();
            LoadUTD(_utd);
        }

        private void LoadUTD(UTD utd)
        {
            // Load UTD data into UI
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var gff = new GFF(GFFContent.UTD);
            byte[] data = gff.ToBytes();
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _utd = new UTD();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
