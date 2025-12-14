using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/jrl.py:40
    // Original: class JRLEditor(Editor):
    public class JRLEditor : Editor
    {
        private JRL _jrl;

        public JRLEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Journal Editor", "journal",
                new[] { ResourceType.JRL },
                new[] { ResourceType.JRL },
                installation)
        {
            InitializeComponent();
            SetupUI();
            Width = 400;
            Height = 250;
            New();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SetupUI()
        {
            var panel = new StackPanel();
            Content = panel;
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            var gff = GFF.FromBytes(data);
            // JRL conversion will be implemented when JRL conversion methods are available
            _jrl = new JRL();
            LoadJRL(_jrl);
        }

        private void LoadJRL(JRL jrl)
        {
            // Load JRL data into UI tree
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var gff = new GFF(GFFContent.JRL);
            byte[] data = gff.ToBytes();
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _jrl = new JRL();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
