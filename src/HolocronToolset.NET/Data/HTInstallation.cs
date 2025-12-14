using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.TwoDA;
using CSharpKOTOR.Installation;
using CSharpKOTOR.Resources;
using JetBrains.Annotations;
using ResourceResult = CSharpKOTOR.Installation.ResourceResult;

namespace HolocronToolset.NET.Data
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:48
    // Original: class HTInstallation(Installation):
    public class HTInstallation
    {
        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:49-91
        // Original: TwoDA_PORTRAITS: str = TwoDARegistry.PORTRAITS
        public const string TwoDAPortraits = "portraits.2da";
        public const string TwoDAAppearances = "appearance.2da";
        public const string TwoDASubraces = "subraces.2da";
        public const string TwoDASpeeds = "speeds.2da";
        public const string TwoDASoundsets = "soundset.2da";
        public const string TwoDAFactions = "factions.2da";
        public const string TwoDAGenders = "genders.2da";
        public const string TwoDAPerceptions = "perceptions.2da";
        public const string TwoDAClasses = "classes.2da";
        public const string TwoDAFeats = "feat.2da";
        public const string TwoDAPowers = "spells.2da";
        public const string TwoDABaseitems = "baseitems.2da";
        public const string TwoDAPlaceables = "placeables.2da";
        public const string TwoDADoors = "doortypes.2da";
        public const string TwoDACursors = "cursors.2da";
        public const string TwoDATraps = "traps.2da";
        public const string TwoDARaces = "racialtypes.2da";
        public const string TwoDASkills = "skills.2da";
        public const string TwoDAUpgrades = "upcrystals.2da";
        public const string TwoDAEncDifficulties = "encdifficulty.2da";
        public const string TwoDAItemProperties = "itemprops.2da";
        public const string TwoDAIprpParamtable = "iprp_paramtable.2da";
        public const string TwoDAIprpCosttable = "iprp_costtable.2da";
        public const string TwoDAIprpAbilities = "iprp_abilities.2da";
        public const string TwoDAIprpAligngrp = "iprp_aligngrp.2da";
        public const string TwoDAIprpCombatdam = "iprp_combatdam.2da";
        public const string TwoDAIprpDamagetype = "iprp_damagetype.2da";
        public const string TwoDAIprpProtection = "iprp_protection.2da";
        public const string TwoDAIprpAcmodtype = "iprp_acmodtype.2da";
        public const string TwoDAIprpImmunity = "iprp_immunity.2da";
        public const string TwoDAIprpSaveelement = "iprp_saveelement.2da";
        public const string TwoDAIprpSavingthrow = "iprp_savingthrow.2da";
        public const string TwoDAIprpOnhit = "iprp_onhit.2da";
        public const string TwoDAIprpAmmotype = "iprp_ammotype.2da";
        public const string TwoDAIprpMonsterhit = "iprp_monsterhit.2da";
        public const string TwoDAIprpWalk = "iprp_walk.2da";
        public const string TwoDAEmotions = "emotions.2da";
        public const string TwoDAExpressions = "expressions.2da";
        public const string TwoDAVideoEffects = "videoeffects.2da";
        public const string TwoDADialogAnims = "dialoganimations.2da";
        public const string TwoDAPlanets = "planetary.2da";
        public const string TwoDAPlot = "plot.2da";
        public const string TwoDACameras = "cameras.2da";

        private readonly Installation _installation;
        private readonly Dictionary<string, TwoDA> _cache2da = new Dictionary<string, TwoDA>();
        private bool? _tsl;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:93-120
        // Original: def __init__(self, path: str | os.PathLike, name: str, *, tsl: bool | None = None, ...):
        public HTInstallation(string path, string name, bool? tsl = null)
        {
            _installation = new Installation(path);
            Name = name;
            _tsl = tsl;
        }

        public string Name { get; set; }
        public Installation Installation => _installation;
        public Game Game => _installation.Game;
        public string Path => _installation.Path;

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:444-469
        // Original: def ht_get_cache_2da(self, resname: str) -> TwoDA | None:
        [CanBeNull]
        public TwoDA HtGetCache2DA(string resname)
        {
            resname = resname.ToLowerInvariant();
            if (!_cache2da.ContainsKey(resname))
            {
                ResourceResult result = _installation.Resource(
                    resname,
                    ResourceType.TwoDA,
                    new[] { SearchLocation.OVERRIDE, SearchLocation.CHITIN });
                if (result == null)
                {
                    return null;
                }
                var reader = new TwoDABinaryReader(result.Data);
                _cache2da[resname] = reader.Load();
            }
            return _cache2da[resname];
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:673-677
        // Original: @property def tsl(self) -> bool:
        public bool Tsl
        {
            get
            {
                if (!_tsl.HasValue)
                {
                    _tsl = Game == Game.TSL;
                }
                return _tsl.Value;
            }
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:549-550
        // Original: def htClearCache2DA(self):
        public void HtClearCache2DA()
        {
            _cache2da.Clear();
        }

        // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/data/installation.py:146-151
        // Original: def clear_all_caches(self):
        public void ClearAllCaches()
        {
            _cache2da.Clear();
            _installation.ClearCache();
        }
    }
}
