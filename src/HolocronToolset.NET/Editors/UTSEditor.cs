using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;

namespace HolocronToolset.NET.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/uts.py:28
    // Original: class UTSEditor(Editor):
    public class UTSEditor : Editor
    {
        private UTS _uts;

        public UTSEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Sound Editor", "sound",
                new[] { ResourceType.UTS },
                new[] { ResourceType.UTS },
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
            _uts = new UTS();
            LoadUTS(_uts);
        }

        private void LoadUTS(UTS uts)
        {
            // Load UTS data into UI
        }

        public override Tuple<byte[], byte[]> Build()
        {
            var gff = new GFF(GFFContent.UTS);
            byte[] data = gff.ToBytes();
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _uts = new UTS();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
