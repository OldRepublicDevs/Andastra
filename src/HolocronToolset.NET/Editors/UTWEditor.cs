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
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utw.py:23
    // Original: class UTWEditor(Editor):
    public class UTWEditor : Editor
    {
        private UTW _utw;

        public UTWEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Waypoint Editor", "waypoint",
                new[] { ResourceType.UTW },
                new[] { ResourceType.UTW },
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

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utw.py:74-84
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            try
            {
                var utw = UTWAuto.ReadUtw(data);
                LoadUTW(utw);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"Failed to load UTW: {ex}");
                New();
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utw.py:86-113
        // Original: def _loadUTW(self, utw: UTW):
        private void LoadUTW(UTW utw)
        {
            _utw = utw;
            // UI loading will be implemented when UI elements are available
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/utw.py:115-150
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            // Build UTW from current state
            // For now, use the stored _utw object
            // Full UI integration will populate _utw from UI elements
            byte[] data = UTWAuto.BytesUtw(_utw);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _utw = new UTW();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
