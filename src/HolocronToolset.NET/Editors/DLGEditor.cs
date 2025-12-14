using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics.DLG;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/dlg/editor.py:88
    // Original: class DLGEditor(Editor):
    public class DLGEditor : Editor
    {
        private DLG _dlg;

        public DLGEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Dialog Editor", "dialog",
                new[] { ResourceType.DLG },
                new[] { ResourceType.DLG },
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
            // DLG conversion will be implemented when DLG conversion methods are available
            _dlg = new DLG();
            LoadDLG(_dlg);
        }

        private void LoadDLG(DLG dlg)
        {
            // Load DLG data into UI tree
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var gff = new GFF(GFFContent.DLG);
            byte[] data = gff.ToBytes();
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _dlg = new DLG();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
