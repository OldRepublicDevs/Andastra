using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resource.Generics;
using CSharpKOTOR.Resources;
using HolocronToolset.NET.Data;
using GFFAuto = CSharpKOTOR.Formats.GFF.GFFAuto;

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

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utt.py:121-131
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            try
            {
                var utt = UTTAuto.ReadUtt(data);
                LoadUTT(utt);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load UTT: {ex}");
                New();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utt.py:133-185
        // Original: def _loadUTT(self, utt: UTT):
        private void LoadUTT(UTT utt)
        {
            _utt = utt;
            // UI loading will be implemented when UI elements are available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utt.py:187-240
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            // Build UTT from current state
            // For now, use the stored _utt object
            // Full UI integration will populate _utt from UI elements
            byte[] data = UTTAuto.BytesUtt(_utt);
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
