using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utt.py:23
    // Original: class UTTEditor(Editor):
    public class UTTEditor : Editor
    {
        private UTT _utt;

        public UTTEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Trigger Editor", "trigger",
                new[] { ResourceType.UTT, ResourceType.BTT },
                new[] { ResourceType.UTT, ResourceType.BTT },
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
            _utt = new UTT();
            LoadUTT(_utt);
        }

        private void LoadUTT(UTT utt)
        {
            // Load UTT data into UI
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var gff = new GFF(GFFContent.UTT);
            byte[] data = gff.ToBytes();
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _utt = new UTT();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
