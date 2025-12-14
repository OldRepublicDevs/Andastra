using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utp.py:38
    // Original: class UTPEditor(Editor):
    public class UTPEditor : Editor
    {
        private UTP _utp;

        public UTPEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Placeable Editor", "placeable",
                new[] { ResourceType.UTP, ResourceType.BTP },
                new[] { ResourceType.UTP, ResourceType.BTP },
                installation)
        {
            InitializeComponent();
            SetupUI();
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
            var panel = new StackPanel();
            Content = panel;
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            var gff = GFF.FromBytes(data);
            _utp = new UTP();
            LoadUTP(_utp);
        }

        private void LoadUTP(UTP utp)
        {
            // Load UTP data into UI
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var gff = new GFF(GFFContent.UTP);
            byte[] data = gff.ToBytes();
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _utp = new UTP();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
