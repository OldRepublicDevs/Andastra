using System;
using System.Numerics;

namespace OdyTools.Data
{
    public class OdyToolLYTSettings : GITSettings
    {
        public OdyToolLYTSettings() : base()
        {
            GridSize = 1.0f;
            ShowGrid = true;
            ShowRoomLabels = true;
            ShowDoorHookLabels = true;
        }

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

        public void Load()
        {
            GridSize = GetValue("GridSize", 1.0f);
            ShowGrid = GetValue("ShowGrid", true);
            ShowRoomLabels = GetValue("ShowRoomLabels", true);
            ShowDoorHookLabels = GetValue("ShowDoorHookLabels", true);
        }

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
