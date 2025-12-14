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
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/ute.py:26
    // Original: class UTEEditor(Editor):
    public class UTEEditor : Editor
    {
        private UTE _ute;

        public UTEEditor(Window parent = null, HTInstallation installation = null)
            : base(parent, "Encounter Editor", "encounter",
                new[] { ResourceType.UTE, ResourceType.BTE },
                new[] { ResourceType.UTE, ResourceType.BTE },
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

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/ute.py:133-143
        // Original: def load(self, filepath, resref, restype, data):
        public override void Load(string filepath, string resref, ResourceType restype, byte[] data)
        {
            base.Load(filepath, resref, restype, data);
            // Matching PyKotor implementation: ute: UTE = read_ute(data); self._loadUTE(ute)
            var gff = GFF.FromBytes(data);
            _ute = UTEHelpers.ConstructUte(gff);
            LoadUTE(_ute);
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/ute.py:145-217
        // Original: def _loadUTE(self, ute: UTE):
        private void LoadUTE(UTE ute)
        {
            // Matching PyKotor implementation: self._ute = ute
            _ute = ute;
            // Load UTE data into UI - full implementation will populate UI controls
            // For now, just store the UTE object
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/ute.py:219-285
        // Original: def build(self) -> tuple[bytes, bytes]:
        public override Tuple<byte[], byte[]> Build()
        {
            // Matching PyKotor implementation: ute: UTE = deepcopy(self._ute)
            var ute = CopyUTE(_ute);

            // Matching PyKotor implementation: Basic fields from UI
            // For now, use existing values (full implementation will read from UI)
            // ute.tag = self.ui.tagEdit.text()
            // ute.resref = ResRef(self.ui.resrefEdit.text())
            // etc.

            // Matching PyKotor implementation: gff: GFF = dismantle_ute(ute); write_gff(gff, data)
            var game = _installation?.Game ?? Game.K2;
            var gff = UTEHelpers.DismantleUte(ute, game);
            byte[] data = GFFAuto.BytesGff(gff, ResourceType.UTE);
            return Tuple.Create(data, new byte[0]);
        }

        // Matching PyKotor implementation: Deep copy helper
        private UTE CopyUTE(UTE source)
        {
            var copy = new UTE
            {
                ResRef = source.ResRef,
                Tag = source.Tag,
                Comment = source.Comment,
                Active = source.Active,
                DifficultyId = source.DifficultyId,
                DifficultyIndex = source.DifficultyIndex,
                Faction = source.Faction,
                MaxCreatures = source.MaxCreatures,
                RecCreatures = source.RecCreatures,
                Respawn = source.Respawn,
                RespawnTime = source.RespawnTime,
                Reset = source.Reset,
                ResetTime = source.ResetTime,
                PlayerOnly = source.PlayerOnly,
                SingleSpawn = source.SingleSpawn,
                OnEntered = source.OnEntered,
                OnExit = source.OnExit,
                OnExhausted = source.OnExhausted,
                OnHeartbeat = source.OnHeartbeat,
                OnUserDefined = source.OnUserDefined,
                OnEnteredScript = source.OnEnteredScript,
                OnExitScript = source.OnExitScript,
                OnExhaustedScript = source.OnExhaustedScript,
                OnHeartbeatScript = source.OnHeartbeatScript,
                OnUserDefinedScript = source.OnUserDefinedScript
            };

            // Copy creatures
            foreach (var creature in source.Creatures)
            {
                copy.Creatures.Add(new UTECreature
                {
                    ResRef = creature.ResRef,
                    Appearance = creature.Appearance,
                    SingleSpawn = creature.SingleSpawn,
                    CR = creature.CR,
                    GuaranteedCount = creature.GuaranteedCount
                });
            }

            return copy;
        }

        public override void New()
        {
            base.New();
            _ute = new UTE();
        }

        public override void SaveAs()
        {
            Save();
        }
    }
}
