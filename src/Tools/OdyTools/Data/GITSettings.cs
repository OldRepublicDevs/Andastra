using System;
using System.Collections.Generic;
using System.Reflection;
using Avalonia.Input;
using BioWare.Common;
using KotorColor = BioWare.Common.Color;

namespace OdyTools.Data
{
    public class GITSettings : Settings
    {
        public GITSettings() : base("OdyToolGIT")
        {
        }

        // Material colours use plain GetValue/SetValue (not SettingsProperty), so we set defaults directly.
        public void ResetMaterialColors()
        {
            SetValue("BottomlessPitMaterialColour", new KotorColor(0.000f, 0.000f, 0.000f, 0.5f).ToRgbaInteger());
            SetValue("CarpetMaterialColour", new KotorColor(1.000f, 0.000f, 1.000f, 0.5f).ToRgbaInteger());
            SetValue("DeepWaterMaterialColour", new KotorColor(0.000f, 0.000f, 0.216f, 0.5f).ToRgbaInteger());
            SetValue("DirtMaterialColour", new KotorColor(0.610f, 0.235f, 0.050f, 0.5f).ToRgbaInteger());
            SetValue("DoorMaterialColour", new KotorColor(0.000f, 0.000f, 0.000f, 0.5f).ToRgbaInteger());
            SetValue("GrassMaterialColour", new KotorColor(0.000f, 0.600f, 0.000f, 0.5f).ToRgbaInteger());
            SetValue("LavaMaterialColour", new KotorColor(0.300f, 0.000f, 0.000f, 0.5f).ToRgbaInteger());
            SetValue("LeavesMaterialColour", new KotorColor(0.000f, 0.000f, 0.216f, 0.5f).ToRgbaInteger());
            SetValue("MetalMaterialColour", new KotorColor(0.434f, 0.552f, 0.730f, 0.5f).ToRgbaInteger());
            SetValue("MudMaterialColour", new KotorColor(0.091f, 0.147f, 0.028f, 0.5f).ToRgbaInteger());
            SetValue("NonWalkGrassMaterialColour", new KotorColor(0.000f, 0.600f, 0.000f, 0.5f).ToRgbaInteger());
            SetValue("NonWalkMaterialColour", new KotorColor(1.000f, 0.000f, 0.000f, 0.5f).ToRgbaInteger());
            SetValue("ObscuringMaterialColour", new KotorColor(0.100f, 0.100f, 0.100f, 0.5f).ToRgbaInteger());
            SetValue("PuddlesMaterialColour", new KotorColor(0.509f, 0.474f, 0.147f, 0.5f).ToRgbaInteger());
            SetValue("StoneMaterialColour", new KotorColor(0.162f, 0.216f, 0.279f, 0.5f).ToRgbaInteger());
            SetValue("SwampMaterialColour", new KotorColor(0.216f, 0.216f, 0.000f, 0.5f).ToRgbaInteger());
            SetValue("TransparentMaterialColour", new KotorColor(1.000f, 1.000f, 1.000f, 0.5f).ToRgbaInteger());
            SetValue("UndefinedMaterialColour", new KotorColor(0.400f, 0.400f, 0.400f, 0.5f).ToRgbaInteger());
            SetValue("WaterMaterialColour", new KotorColor(0.000f, 0.000f, 1.000f, 0.5f).ToRgbaInteger());
            SetValue("WoodMaterialColour", new KotorColor(0.258f, 0.059f, 0.007f, 0.5f).ToRgbaInteger());
        }

        // PyKotor: for setting in dir(self):
        //              if not setting.endswith("Bind"):
        //                  continue
        //              self.reset_setting(setting)
        public void ResetControls()
        {
            // Get all properties of this class that end with "Bind"
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.Name.EndsWith("Bind"))
                {
                    try
                    {
                        ResetSetting(property.Name);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine($"Warning: Could not reset bind setting '{property.Name}': {ex.Message}");
                    }
                }
            }
        }

        public string CreatureLabel
        {
            get => GetValue("CreatureLabel", "");
            set => SetValue("CreatureLabel", value);
        }

        public string DoorLabel
        {
            get => GetValue("DoorLabel", "");
            set => SetValue("DoorLabel", value);
        }

        public string PlaceableLabel
        {
            get => GetValue("PlaceableLabel", "");
            set => SetValue("PlaceableLabel", value);
        }

        public string StoreLabel
        {
            get => GetValue("StoreLabel", "");
            set => SetValue("StoreLabel", value);
        }

        public string SoundLabel
        {
            get => GetValue("SoundLabel", "");
            set => SetValue("SoundLabel", value);
        }

        public string WaypointLabel
        {
            get => GetValue("WaypointLabel", "");
            set => SetValue("WaypointLabel", value);
        }

        public string CameraLabel
        {
            get => GetValue("CameraLabel", "");
            set => SetValue("CameraLabel", value);
        }

        public string EncounterLabel
        {
            get => GetValue("EncounterLabel", "");
            set => SetValue("EncounterLabel", value);
        }

        public string TriggerLabel
        {
            get => GetValue("TriggerLabel", "");
            set => SetValue("TriggerLabel", value);
        }

        public int UndefinedMaterialColour
        {
            get => GetValue("UndefinedMaterialColour", new KotorColor(0.400f, 0.400f, 0.400f, 0.5f).ToRgbaInteger());
            set => SetValue("UndefinedMaterialColour", value);
        }

        public int DirtMaterialColour
        {
            get => GetValue("DirtMaterialColour", new KotorColor(0.610f, 0.235f, 0.050f, 0.5f).ToRgbaInteger());
            set => SetValue("DirtMaterialColour", value);
        }

        public int ObscuringMaterialColour
        {
            get => GetValue("ObscuringMaterialColour", new KotorColor(0.100f, 0.100f, 0.100f, 0.5f).ToRgbaInteger());
            set => SetValue("ObscuringMaterialColour", value);
        }

        public int GrassMaterialColour
        {
            get => GetValue("GrassMaterialColour", new KotorColor(0.000f, 0.600f, 0.000f, 0.5f).ToRgbaInteger());
            set => SetValue("GrassMaterialColour", value);
        }

        public int StoneMaterialColour
        {
            get => GetValue("StoneMaterialColour", new KotorColor(0.162f, 0.216f, 0.279f, 0.5f).ToRgbaInteger());
            set => SetValue("StoneMaterialColour", value);
        }

        public int WoodMaterialColour
        {
            get => GetValue("WoodMaterialColour", new KotorColor(0.258f, 0.059f, 0.007f, 0.5f).ToRgbaInteger());
            set => SetValue("WoodMaterialColour", value);
        }

        public int WaterMaterialColour
        {
            get => GetValue("WaterMaterialColour", new KotorColor(0.000f, 0.000f, 1.000f, 0.5f).ToRgbaInteger());
            set => SetValue("WaterMaterialColour", value);
        }

        public int NonWalkMaterialColour
        {
            get => GetValue("NonWalkMaterialColour", new KotorColor(1.000f, 0.000f, 0.000f, 0.5f).ToRgbaInteger());
            set => SetValue("NonWalkMaterialColour", value);
        }

        public int TransparentMaterialColour
        {
            get => GetValue("TransparentMaterialColour", new KotorColor(1.000f, 1.000f, 1.000f, 0.5f).ToRgbaInteger());
            set => SetValue("TransparentMaterialColour", value);
        }

        public int CarpetMaterialColour
        {
            get => GetValue("CarpetMaterialColour", new KotorColor(1.000f, 0.000f, 1.000f, 0.5f).ToRgbaInteger());
            set => SetValue("CarpetMaterialColour", value);
        }

        public int MetalMaterialColour
        {
            get => GetValue("MetalMaterialColour", new KotorColor(0.434f, 0.552f, 0.730f, 0.5f).ToRgbaInteger());
            set => SetValue("MetalMaterialColour", value);
        }

        public int PuddlesMaterialColour
        {
            get => GetValue("PuddlesMaterialColour", new KotorColor(0.509f, 0.474f, 0.147f, 0.5f).ToRgbaInteger());
            set => SetValue("PuddlesMaterialColour", value);
        }

        public int SwampMaterialColour
        {
            get => GetValue("SwampMaterialColour", new KotorColor(0.216f, 0.216f, 0.000f, 0.5f).ToRgbaInteger());
            set => SetValue("SwampMaterialColour", value);
        }

        public int MudMaterialColour
        {
            get => GetValue("MudMaterialColour", new KotorColor(0.091f, 0.147f, 0.028f, 0.5f).ToRgbaInteger());
            set => SetValue("MudMaterialColour", value);
        }

        public int LeavesMaterialColour
        {
            get => GetValue("LeavesMaterialColour", new KotorColor(0.000f, 0.000f, 0.216f, 0.5f).ToRgbaInteger());
            set => SetValue("LeavesMaterialColour", value);
        }

        public int DoorMaterialColour
        {
            get => GetValue("DoorMaterialColour", new KotorColor(0.000f, 0.000f, 0.000f, 0.5f).ToRgbaInteger());
            set => SetValue("DoorMaterialColour", value);
        }

        public int LavaMaterialColour
        {
            get => GetValue("LavaMaterialColour", new KotorColor(0.300f, 0.000f, 0.000f, 0.5f).ToRgbaInteger());
            set => SetValue("LavaMaterialColour", value);
        }

        public int BottomlessPitMaterialColour
        {
            get => GetValue("BottomlessPitMaterialColour", new KotorColor(0.000f, 0.000f, 0.000f, 0.5f).ToRgbaInteger());
            set => SetValue("BottomlessPitMaterialColour", value);
        }

        public int DeepWaterMaterialColour
        {
            get => GetValue("DeepWaterMaterialColour", new KotorColor(0.000f, 0.000f, 0.216f, 0.5f).ToRgbaInteger());
            set => SetValue("DeepWaterMaterialColour", value);
        }

        public int NonWalkGrassMaterialColour
        {
            get => GetValue("NonWalkGrassMaterialColour", new KotorColor(0.000f, 0.600f, 0.000f, 0.5f).ToRgbaInteger());
            set => SetValue("NonWalkGrassMaterialColour", value);
        }

        private static Tuple<HashSet<Key>, HashSet<PointerUpdateKind>> CreateBind(HashSet<Key> keys, HashSet<PointerUpdateKind> mouseButtons)
        {
            return Tuple.Create(keys ?? new HashSet<Key>(), mouseButtons ?? new HashSet<PointerUpdateKind>());
        }

        public SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>> MoveCameraBind { get; } =
            new SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>("moveCameraBind",
                CreateBind(new HashSet<Key> { Key.LeftCtrl }, new HashSet<PointerUpdateKind> { PointerUpdateKind.LeftButtonPressed }));

        public SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>> RotateCameraBind { get; } =
            new SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>("rotateCameraBind",
                CreateBind(new HashSet<Key> { Key.LeftCtrl }, new HashSet<PointerUpdateKind> { PointerUpdateKind.MiddleButtonPressed }));

        public SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>> ZoomCameraBind { get; } =
            new SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>("zoomCameraBind",
                CreateBind(new HashSet<Key> { Key.LeftCtrl }, new HashSet<PointerUpdateKind>()));

        public SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>> RotateSelectedToPointBind { get; } =
            new SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>("rotateSelectedToPointBind",
                CreateBind(new HashSet<Key>(), new HashSet<PointerUpdateKind> { PointerUpdateKind.MiddleButtonPressed }));

        public SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>> MoveSelectedBind { get; } =
            new SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>("moveSelectedBind",
                CreateBind(new HashSet<Key>(), new HashSet<PointerUpdateKind> { PointerUpdateKind.LeftButtonPressed }));

        public SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>> SelectUnderneathBind { get; } =
            new SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>("selectUnderneathBind",
                CreateBind(new HashSet<Key>(), new HashSet<PointerUpdateKind> { PointerUpdateKind.LeftButtonPressed }));

        public SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>> DeleteSelectedBind { get; } =
            new SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>("deleteSelectedBind",
                CreateBind(new HashSet<Key> { Key.Delete }, new HashSet<PointerUpdateKind>()));

        public SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>> DuplicateSelectedBind { get; } =
            new SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>("duplicateSelectedBind",
                CreateBind(new HashSet<Key> { Key.LeftAlt }, new HashSet<PointerUpdateKind> { PointerUpdateKind.LeftButtonPressed }));

        public SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>> ToggleLockInstancesBind { get; } =
            new SettingsProperty<Tuple<HashSet<Key>, HashSet<PointerUpdateKind>>>("toggleLockInstancesBind",
                CreateBind(new HashSet<Key> { Key.L }, new HashSet<PointerUpdateKind>()));
        // endregion
    }
}
