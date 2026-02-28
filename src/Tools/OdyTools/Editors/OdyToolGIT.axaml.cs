using BioWare.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.GFF.Generics;
using BioWare.Resource;
using OdyTools.Data;
using Game = BioWare.Common.BioWareGame;
using GFFAuto = BioWare.Resource.Formats.GFF.GFFAuto;

namespace OdyTools.Editors
{
    /// <summary>Display item for the instance list: type label + resref/tag.</summary>
    public sealed class GITInstanceItem
    {
        public string DisplayText { get; set; }
        public string TypeName { get; set; }
        public object Instance { get; set; }
    }

    public partial class OdyToolGIT : Editor
    {
        private GIT _git;
        private GFF _originalGff;

        private List<GITInstanceItem> _instanceItems = new List<GITInstanceItem>();
        private object _selectedInstance;

        // XAML controls (optional when AXAML loaded)
        private TextBox _filterEdit;
        private ListBox _instanceList;
        private StackPanel _detailNoSelection;
        private StackPanel _detailInstance;
        private TextBlock _detailTypeLabel;
        private TextBox _detailResRef;
        private NumericUpDown _detailPosX, _detailPosY, _detailPosZ, _detailBearing;
        private TextBox _detailTag;
        private TextBlock _detailTagLabel;
        private Button _removeInstanceButton;
        private TextBlock _statusText;

        public OdyToolGIT(Window parent = null, OdyInstallation installation = null)
            : base(parent, "OdyToolGIT", "git",
                new[] { ResourceType.GIT },
                new[] { ResourceType.GIT },
                installation)
        {
            _git = new GIT();
            InitializeComponent();
            SetupUI();
            AddHelpAction(); // Auto-detects "GFF-GIT.md" for GIT
            New();
        }

        private void InitializeComponent()
        {
            try
            {
                Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
            }
            catch
            {
                SetupUI();
            }
        }

        private void SetupUI()
        {
            _filterEdit = EditorHelpers.FindControlSafe<TextBox>(this, "filterEdit");
            _instanceList = EditorHelpers.FindControlSafe<ListBox>(this, "instanceList");
            _detailNoSelection = EditorHelpers.FindControlSafe<StackPanel>(this, "detailNoSelection");
            _detailInstance = EditorHelpers.FindControlSafe<StackPanel>(this, "detailInstance");
            _detailTypeLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "detailTypeLabel");
            _detailResRef = EditorHelpers.FindControlSafe<TextBox>(this, "detailResRef");
            _detailPosX = EditorHelpers.FindControlSafe<NumericUpDown>(this, "detailPosX");
            _detailPosY = EditorHelpers.FindControlSafe<NumericUpDown>(this, "detailPosY");
            _detailPosZ = EditorHelpers.FindControlSafe<NumericUpDown>(this, "detailPosZ");
            _detailBearing = EditorHelpers.FindControlSafe<NumericUpDown>(this, "detailBearing");
            _detailTag = EditorHelpers.FindControlSafe<TextBox>(this, "detailTag");
            _detailTagLabel = EditorHelpers.FindControlSafe<TextBlock>(this, "detailTagLabel");
            _removeInstanceButton = EditorHelpers.FindControlSafe<Button>(this, "removeInstanceButton");
            _statusText = EditorHelpers.FindControlSafe<TextBlock>(this, "statusText");

            if (_instanceList != null)
            {
                _instanceList.SelectionChanged += OnInstanceListSelectionChanged;
                if (_filterEdit != null)
                    _filterEdit.TextChanged += (s, e) => ApplyFilter();
                if (_detailResRef != null) _detailResRef.LostFocus += (s, e) => SaveDetailToInstance();
                if (_detailPosX != null) _detailPosX.ValueChanged += (s, e) => SaveDetailToInstance();
                if (_detailPosY != null) _detailPosY.ValueChanged += (s, e) => SaveDetailToInstance();
                if (_detailPosZ != null) _detailPosZ.ValueChanged += (s, e) => SaveDetailToInstance();
                if (_detailBearing != null) _detailBearing.ValueChanged += (s, e) => SaveDetailToInstance();
                if (_detailTag != null) _detailTag.LostFocus += (s, e) => SaveDetailToInstance();
                if (_removeInstanceButton != null) _removeInstanceButton.Click += (s, e) => RemoveSelectedInstance();
                RebuildInstanceList();
                UpdateStatusBar();
            }

            SetupMenuHandlers();
            if (_instanceList == null)
            {
                var panel = new StackPanel();
                Content = panel;
            }
        }

        private void SetupMenuHandlers()
        {
            void Bind(string name, Action handler)
            {
                var item = EditorHelpers.FindControlSafe<MenuItem>(this, name);
                if (item != null) item.Click += (s, e) => handler();
            }
            // actionNew, actionOpen, actionSave, actionSaveAs, actionRevert, actionExit wired by base Editor
        }

        protected override async System.Threading.Tasks.Task RunSaveAsAsync()
        {
            var provider = (this as Window)?.StorageProvider;
            if (provider == null) return;
            var options = new Avalonia.Platform.Storage.FilePickerSaveOptions
            {
                Title = "Save As",
                SuggestedFileName = (string.IsNullOrEmpty(_resname) ? "area" : _resname) + ".git",
                FileTypeChoices = new[] { new Avalonia.Platform.Storage.FilePickerFileType("GIT") { Patterns = new[] { "*.git" } } }
            };
            var file = await provider.SaveFilePickerAsync(options);
            if (file == null) return;
            _filepath = file.Path.LocalPath;
            if (string.IsNullOrWhiteSpace(_filepath)) return;
            RefreshWindowTitle();
            Save();
            UpdateStatusBar();
        }

        private void RebuildInstanceList()
        {
            _instanceItems.Clear();
            if (_git == null) return;
            string resref(object o)
            {
                if (o is GITCreature c) return c.ResRef.ToString();
                if (o is GITDoor d) return d.ResRef.ToString();
                if (o is GITPlaceable p) return p.ResRef.ToString();
                if (o is GITTrigger t) return t.ResRef.ToString();
                if (o is GITWaypoint w) return w.ResRef.ToString();
                if (o is GITSound s) return s.ResRef.ToString();
                if (o is GITStore st) return st.ResRef.ToString();
                if (o is GITEncounter e) return e.ResRef.ToString();
                if (o is GITCamera cam) return cam.ResRef.ToString();
                return "";
            }
            string tag(object o)
            {
                if (o is GITDoor d) return d.Tag ?? "";
                if (o is GITPlaceable p) return p.Tag ?? "";
                if (o is GITTrigger t) return t.Tag ?? "";
                if (o is GITWaypoint w) return w.Tag ?? "";
                if (o is GITSound s) return s.Tag ?? "";
                return "";
            }
            foreach (var c in _git.Creatures)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Creature] {resref(c)}", TypeName = "Creature", Instance = c });
            foreach (var d in _git.Doors)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Door] {(string.IsNullOrEmpty(tag(d)) ? resref(d) : tag(d))}", TypeName = "Door", Instance = d });
            foreach (var p in _git.Placeables)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Placeable] {(tag(p).Length > 0 ? tag(p) : resref(p))}", TypeName = "Placeable", Instance = p });
            foreach (var t in _git.Triggers)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Trigger] {(tag(t).Length > 0 ? tag(t) : resref(t))}", TypeName = "Trigger", Instance = t });
            foreach (var w in _git.Waypoints)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Waypoint] {(tag(w).Length > 0 ? tag(w) : resref(w))}", TypeName = "Waypoint", Instance = w });
            foreach (var s in _git.Sounds)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Sound] {(tag(s).Length > 0 ? tag(s) : resref(s))}", TypeName = "Sound", Instance = s });
            foreach (var st in _git.Stores)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Store] {resref(st)}", TypeName = "Store", Instance = st });
            foreach (var e in _git.Encounters)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Encounter] {resref(e)}", TypeName = "Encounter", Instance = e });
            foreach (var cam in _git.Cameras)
                _instanceItems.Add(new GITInstanceItem { DisplayText = $"[Camera] {cam.CameraId}", TypeName = "Camera", Instance = cam });

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (_instanceList == null) return;
            string filter = (_filterEdit?.Text ?? "").Trim().ToLowerInvariant();
            var filtered = string.IsNullOrEmpty(filter)
                ? _instanceItems
                : _instanceItems.Where(x => (x.DisplayText ?? "").ToLowerInvariant().Contains(filter)).ToList();
            _instanceList.ItemsSource = filtered;
        }

        private void OnInstanceListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = _instanceList?.SelectedItem as GITInstanceItem;
            _selectedInstance = item?.Instance;
            LoadDetailFromInstance();
            if (_detailNoSelection != null) _detailNoSelection.IsVisible = _selectedInstance == null;
            if (_detailInstance != null) _detailInstance.IsVisible = _selectedInstance != null;
            if (_removeInstanceButton != null) _removeInstanceButton.IsEnabled = _selectedInstance != null;
            UpdateStatusBar();
        }

        private void LoadDetailFromInstance()
        {
            if (_detailResRef == null) return;
            bool hasBearing = _selectedInstance is GITCreature || _selectedInstance is GITDoor || _selectedInstance is GITPlaceable || _selectedInstance is GITStore || _selectedInstance is GITWaypoint;
            bool hasTag = _selectedInstance is GITDoor || _selectedInstance is GITPlaceable || _selectedInstance is GITTrigger || _selectedInstance is GITWaypoint || _selectedInstance is GITSound;
            if (_detailBearing != null) _detailBearing.IsVisible = hasBearing;
            if (_detailTag != null) _detailTag.IsVisible = hasTag;
            if (_detailTagLabel != null) _detailTagLabel.IsVisible = hasTag;

            if (_selectedInstance == null)
            {
                _detailResRef.Text = "";
                if (_detailPosX != null) _detailPosX.Value = 0;
                if (_detailPosY != null) _detailPosY.Value = 0;
                if (_detailPosZ != null) _detailPosZ.Value = 0;
                if (_detailBearing != null) _detailBearing.Value = 0;
                if (_detailTag != null) _detailTag.Text = "";
                if (_detailTypeLabel != null) _detailTypeLabel.Text = "Instance";
                return;
            }

            if (_detailTypeLabel != null)
            {
                if (_selectedInstance is GITCreature) _detailTypeLabel.Text = "Creature";
                else if (_selectedInstance is GITDoor) _detailTypeLabel.Text = "Door";
                else if (_selectedInstance is GITPlaceable) _detailTypeLabel.Text = "Placeable";
                else if (_selectedInstance is GITTrigger) _detailTypeLabel.Text = "Trigger";
                else if (_selectedInstance is GITWaypoint) _detailTypeLabel.Text = "Waypoint";
                else if (_selectedInstance is GITSound) _detailTypeLabel.Text = "Sound";
                else if (_selectedInstance is GITStore) _detailTypeLabel.Text = "Store";
                else if (_selectedInstance is GITEncounter) _detailTypeLabel.Text = "Encounter";
                else if (_selectedInstance is GITCamera) _detailTypeLabel.Text = "Camera";
                else _detailTypeLabel.Text = "Instance";
            }

            if (_selectedInstance is GITCreature c1) { _detailResRef.Text = c1.ResRef.ToString(); SetPos(c1.Position); _detailBearing.Value = (decimal)c1.Bearing; _detailTag.Text = ""; }
            else if (_selectedInstance is GITDoor d) { _detailResRef.Text = d.ResRef.ToString(); SetPos(d.Position); _detailBearing.Value = (decimal)d.Bearing; _detailTag.Text = d.Tag ?? ""; }
            else if (_selectedInstance is GITPlaceable p) { _detailResRef.Text = p.ResRef.ToString(); SetPos(p.Position); _detailBearing.Value = (decimal)p.Bearing; _detailTag.Text = p.Tag ?? ""; }
            else if (_selectedInstance is GITTrigger t) { _detailResRef.Text = t.ResRef.ToString(); SetPos(t.Position); _detailTag.Text = t.Tag ?? ""; }
            else if (_selectedInstance is GITWaypoint w) { _detailResRef.Text = w.ResRef.ToString(); SetPos(w.Position); _detailBearing.Value = (decimal)w.Bearing; _detailTag.Text = w.Tag ?? ""; }
            else if (_selectedInstance is GITSound s) { _detailResRef.Text = s.ResRef.ToString(); SetPos(s.Position); _detailTag.Text = s.Tag ?? ""; }
            else if (_selectedInstance is GITStore st) { _detailResRef.Text = st.ResRef.ToString(); SetPos(st.Position); _detailBearing.Value = (decimal)st.Bearing; _detailTag.Text = ""; }
            else if (_selectedInstance is GITEncounter e) { _detailResRef.Text = e.ResRef.ToString(); SetPos(e.Position); _detailTag.Text = ""; }
            else if (_selectedInstance is GITCamera cam) { _detailResRef.Text = cam.ResRef.ToString(); SetPos(cam.Position); _detailTag.Text = ""; }
        }

        private void SetPos(System.Numerics.Vector3 v)
        {
            if (_detailPosX != null) _detailPosX.Value = (decimal)v.X;
            if (_detailPosY != null) _detailPosY.Value = (decimal)v.Y;
            if (_detailPosZ != null) _detailPosZ.Value = (decimal)v.Z;
        }

        private void SaveDetailToInstance()
        {
            if (_selectedInstance == null) return;
            try
            {
                var resref = ResRef.FromBlank();
                if (!string.IsNullOrWhiteSpace(_detailResRef?.Text))
                    resref = new ResRef(_detailResRef.Text.Trim());
                var pos = new System.Numerics.Vector3(
                    (float)(_detailPosX?.Value ?? 0),
                    (float)(_detailPosY?.Value ?? 0),
                    (float)(_detailPosZ?.Value ?? 0));

                if (_selectedInstance is GITCreature c1) { c1.ResRef = resref; c1.Position = pos; c1.Bearing = (float)(_detailBearing?.Value ?? 0); }
                else if (_selectedInstance is GITDoor d) { d.ResRef = resref; d.Position = pos; d.Bearing = (float)(_detailBearing?.Value ?? 0); d.Tag = _detailTag?.Text ?? ""; }
                else if (_selectedInstance is GITPlaceable p) { p.ResRef = resref; p.Position = pos; p.Bearing = (float)(_detailBearing?.Value ?? 0); p.Tag = _detailTag?.Text ?? ""; }
                else if (_selectedInstance is GITTrigger t) { t.ResRef = resref; t.Position = pos; t.Tag = _detailTag?.Text ?? ""; }
                else if (_selectedInstance is GITWaypoint w) { w.ResRef = resref; w.Position = pos; w.Bearing = (float)(_detailBearing?.Value ?? 0); w.Tag = _detailTag?.Text ?? ""; }
                else if (_selectedInstance is GITSound s) { s.ResRef = resref; s.Position = pos; s.Tag = _detailTag?.Text ?? ""; }
                else if (_selectedInstance is GITStore st) { st.ResRef = resref; st.Position = pos; st.Bearing = (float)(_detailBearing?.Value ?? 0); }
                else if (_selectedInstance is GITEncounter e) { e.ResRef = resref; e.Position = pos; }
                else if (_selectedInstance is GITCamera cam) { cam.ResRef = resref; cam.Position = pos; }
            }
            catch { }
        }

        private void RemoveSelectedInstance()
        {
            if (_selectedInstance == null || _git == null) return;
            try
            {
                _git.Remove(_selectedInstance);
                _selectedInstance = null;
                RebuildInstanceList();
                LoadDetailFromInstance();
                if (_detailNoSelection != null) _detailNoSelection.IsVisible = true;
                if (_detailInstance != null) _detailInstance.IsVisible = false;
                if (_removeInstanceButton != null) _removeInstanceButton.IsEnabled = false;
                UpdateStatusBar();
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex); }
        }

        private void UpdateStatusBar()
        {
            if (_statusText == null) return;
            int total = _instanceItems?.Count ?? 0;
            if (_git != null)
                total = (_git.Creatures?.Count ?? 0) + (_git.Doors?.Count ?? 0) + (_git.Placeables?.Count ?? 0) + (_git.Triggers?.Count ?? 0) + (_git.Waypoints?.Count ?? 0) + (_git.Sounds?.Count ?? 0) + (_git.Stores?.Count ?? 0) + (_git.Encounters?.Count ?? 0) + (_git.Cameras?.Count ?? 0);
            string text = _git != null ? $"{total} instance(s)" : "No GIT";
            if (_selectedInstance != null)
                text += " | 1 selected";
            _statusText.Text = text;
        }

        public override void Revert()
        {
            if (_revert == null || _revert.Length == 0) return;
            try
            {
                _originalGff = GFF.FromBytes(_revert);
                _git = ResourceAutoHelpers.ReadGit(_revert);
                LoadGIT(_git);
            }
            catch (Exception ex) { System.Console.WriteLine(ex); }
        }

        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);

            // GIT is a GFF-based format - store original GFF to preserve unmodified fields
            _originalGff = data != null && data.Length > 0 ? GFF.FromBytes(data) : null;
            _git = ResourceAutoHelpers.ReadGit(data);
            LoadGIT(_git);
        }

        private void LoadGIT(GIT git)
        {
            // Load GIT data into UI
            _git = git;
            _selectedInstance = null;
            RebuildInstanceList();
            LoadDetailFromInstance();
            if (_detailNoSelection != null) _detailNoSelection.IsVisible = true;
            if (_detailInstance != null) _detailInstance.IsVisible = false;
            if (_removeInstanceButton != null) _removeInstanceButton.IsEnabled = false;
            UpdateStatusBar();
        }

        public override Tuple<byte[], byte[]> Build()
        {
            SaveDetailToInstance();
            Game gameToUse = _installation?.Game ?? Game.K2;
            var gff = GITHelpers.DismantleGit(_git, gameToUse);

            // Preserve unmodified fields from original GFF that aren't yet supported by GIT object model
            // This ensures roundtrip tests pass by maintaining all original data
            if (_originalGff != null)
            {
                var originalRoot = _originalGff.Root;
                var newRoot = gff.Root;

                // List of fields that GITHelpers.DismantleGit explicitly sets
                var fieldsSetByDismantle = new System.Collections.Generic.HashSet<string>
                {
                    "UseTemplates",
                    "AreaProperties",
                    "CameraList",
                    "Creature List",
                    "Door List",
                    "Encounter List",
                    "Placeable List",
                    "SoundList",
                    "StoreList",
                    "TriggerList",
                    "WaypointList"
                };

                // Copy all fields from original that aren't explicitly set by DismantleGit
                foreach (var (label, fieldType, value) in originalRoot)
                {
                    if (!fieldsSetByDismantle.Contains(label) && !newRoot.Exists(label))
                    {
                        CopyGffField(originalRoot, newRoot, label, fieldType);
                    }
                }
            }

            byte[] data = GFFAuto.BytesGff(gff, ResourceType.GIT);
            return Tuple.Create(data, new byte[0]);
        }

        public override void New()
        {
            base.New();
            _git = new GIT();
            _originalGff = null; // Clear original GFF when creating new file
            LoadGIT(_git);
        }

        // Helper method to copy a GFF field from one struct to another, preserving type
        private static void CopyGffField(GFFStruct source, GFFStruct destination, string label, GFFFieldType fieldType)
        {
            switch (fieldType)
            {
                case GFFFieldType.UInt8:
                    destination.SetUInt8(label, source.GetUInt8(label));
                    break;
                case GFFFieldType.Int8:
                    destination.SetInt8(label, source.GetInt8(label));
                    break;
                case GFFFieldType.UInt16:
                    destination.SetUInt16(label, source.GetUInt16(label));
                    break;
                case GFFFieldType.Int16:
                    destination.SetInt16(label, source.GetInt16(label));
                    break;
                case GFFFieldType.UInt32:
                    destination.SetUInt32(label, source.GetUInt32(label));
                    break;
                case GFFFieldType.Int32:
                    destination.SetInt32(label, source.GetInt32(label));
                    break;
                case GFFFieldType.UInt64:
                    destination.SetUInt64(label, source.GetUInt64(label));
                    break;
                case GFFFieldType.Int64:
                    destination.SetInt64(label, source.GetInt64(label));
                    break;
                case GFFFieldType.Single:
                    destination.SetSingle(label, source.GetSingle(label));
                    break;
                case GFFFieldType.Double:
                    destination.SetDouble(label, source.GetDouble(label));
                    break;
                case GFFFieldType.String:
                    destination.SetString(label, source.GetString(label));
                    break;
                case GFFFieldType.ResRef:
                    destination.SetResRef(label, source.GetResRef(label));
                    break;
                case GFFFieldType.LocalizedString:
                    destination.SetLocString(label, source.GetLocString(label));
                    break;
                case GFFFieldType.Binary:
                    destination.SetBinary(label, source.GetBinary(label));
                    break;
                case GFFFieldType.Vector3:
                    destination.SetVector3(label, source.GetVector3(label));
                    break;
                case GFFFieldType.Vector4:
                    destination.SetVector4(label, source.GetVector4(label));
                    break;
                case GFFFieldType.Struct:
                    destination.SetStruct(label, source.GetStruct(label));
                    break;
                case GFFFieldType.List:
                    destination.SetList(label, source.GetList(label));
                    break;
            }
        }

        public override void SaveAs()
        {
            _ = RunSaveAsAsync();
        }
    }
}
