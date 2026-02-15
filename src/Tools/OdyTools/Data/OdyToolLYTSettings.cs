using System;
using System.Numerics;

namespace OdyTools.Data
{
    // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/settings/editor_settings/lyt.py:8
    // Original: class OdyToolLYTSettings(GITSettings):
    public class OdyToolLYTSettings : GITSettings
    {
        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/settings/editor_settings/lyt.py:9-15
        // Original: def __init__(self): super().__init__()
        public OdyToolLYTSettings() : base()
        {
            GridSize = 1.0f;
            ShowGrid = true;
            ShowRoomLabels = true;
            ShowDoorHookLabels = true;
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/settings/editor_settings/lyt.py:12-15
        // Original: LYT-specific settings
        public float GridSize
        {
            get => GetValue("GridSize", 1.0f);
            set => SetValue("GridSize", value);
        }

        public bool ShowGrid
        {
            get => GetValue("ShowGrid", true);
            set => SetValue("ShowGrid", value);
        }

        public bool ShowRoomLabels
        {
            get => GetValue("ShowRoomLabels", true);
            set => SetValue("ShowRoomLabels", value);
        }

        public bool ShowDoorHookLabels
        {
            get => GetValue("ShowDoorHookLabels", true);
            set => SetValue("ShowDoorHookLabels", value);
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/settings/editor_settings/lyt.py:17-22
        // Original: def load(self):
        public void Load()
        {
            GridSize = GetValue("GridSize", 1.0f);
            ShowGrid = GetValue("ShowGrid", true);
            ShowRoomLabels = GetValue("ShowRoomLabels", true);
            ShowDoorHookLabels = GetValue("ShowDoorHookLabels", true);
        }

        // Matching PyKotor implementation at Tools/OdyTools/src/toolset/gui/widgets/settings/editor_settings/lyt.py:24-29
        // Original: def save(self):
        // Intentionally hides base Save() to provide LYT-specific save semantics (sets LYT properties which auto-save via SetValue)
        public new void Save()
        {
            SetValue("GridSize", GridSize);
            SetValue("ShowGrid", ShowGrid);
            SetValue("ShowRoomLabels", ShowRoomLabels);
            SetValue("ShowDoorHookLabels", ShowDoorHookLabels);
        }
    }
}
