using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/ute.py:26
    // Original: class UTEEditor(Editor):
    public class UTEEditor : Editor
    {
        private UTE _ute;

        public UTEEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Encounter Editor", "encounter",
                new[] { ResourceType.UTE, ResourceType.BTE },
                new[] { ResourceType.UTE, ResourceType.BTE },
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
            var panel = new StackPanel();
            Content = panel;
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            var gff = GFF.FromBytes(data);
            _ute = new UTE();
            LoadUTE(_ute);
        }

        private void LoadUTE(UTE ute)
        {
            // Load UTE data into UI
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var gff = new GFF(GFFContent.UTE);
            byte[] data = gff.ToBytes();
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _ute = new UTE();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
