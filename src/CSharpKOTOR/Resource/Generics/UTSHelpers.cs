using System.Collections.Generic;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resources;

namespace CSharpKOTOR.Resource.Generics
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/uts.py
    // Original: construct_uts and dismantle_uts functions
    public static class UTSHelpers
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/uts.py:199-234
        // Original: def construct_uts(gff: GFF) -> UTS:
        public static UTS ConstructUts(GFF gff)
        {
            var uts = new UTS();
            var root = gff.Root;

            // Extract basic fields
            uts.Tag = root.Acquire<string>("Tag", "");
            uts.ResRef = root.Acquire<ResRef>("TemplateResRef", ResRef.FromBlank());
            uts.Active = root.Acquire<int>("Active", 0) != 0;
            uts.Continuous = root.Acquire<int>("Continuous", 0) != 0;
            uts.Looping = root.Acquire<int>("Looping", 0) != 0;
            uts.Positional = root.Acquire<int>("Positional", 0) != 0;
            uts.RandomPosition = root.Acquire<int>("RandomPosition", 0) != 0;
            uts.Random = root.Acquire<int>("Random", 0) != 0;
            uts.Volume = root.Acquire<int>("Volume", 0);
            uts.VolumeVariance = root.Acquire<int>("VolumeVrtn", 0);
            uts.PitchVariance = root.Acquire<float>("PitchVariation", 0.0f);
            uts.Elevation = root.Acquire<float>("Elevation", 0.0f);
            uts.MinDistance = root.Acquire<float>("MinDistance", 0.0f);
            uts.MaxDistance = root.Acquire<float>("MaxDistance", 0.0f);
            uts.DistanceCutoff = root.Acquire<float>("DistanceCutoff", 0.0f);
            uts.Priority = root.Acquire<int>("Priority", 0);
            uts.Hours = root.Acquire<int>("Hours", 0);
            uts.Times = root.Acquire<int>("Times", 0);
            uts.Interval = root.Acquire<int>("Interval", 0);
            uts.IntervalVariance = root.Acquire<int>("IntervalVrtn", 0);
            uts.Sound = root.Acquire<ResRef>("Sound", ResRef.FromBlank());
            uts.Comment = root.Acquire<string>("Comment", "");

            // Extract sounds list
            var soundsList = root.Acquire<GFFList>("Sounds", new GFFList());
            // uts.Sounds would need to be a List<ResRef> property
            // foreach (var soundStruct in soundsList) { ... }

            return uts;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/uts.py:237-311
        // Original: def dismantle_uts(uts: UTS, game: Game = Game.K2, *, use_deprecated: bool = True) -> GFF:
        public static GFF DismantleUts(UTS uts, Game game = Game.K2, bool useDeprecated = true)
        {
            var gff = new GFF(GFFContent.UTS);
            var root = gff.Root;

            // Set basic fields
            root.SetString("Tag", uts.Tag);
            root.SetResRef("TemplateResRef", uts.ResRef);
            root.SetUInt8("Active", uts.Active ? (byte)1 : (byte)0);
            root.SetUInt8("Continuous", uts.Continuous ? (byte)1 : (byte)0);
            root.SetUInt8("Looping", uts.Looping ? (byte)1 : (byte)0);
            root.SetUInt8("Positional", uts.Positional ? (byte)1 : (byte)0);
            root.SetUInt8("RandomPosition", uts.RandomPosition ? (byte)1 : (byte)0);
            root.SetUInt8("Random", uts.Random ? (byte)1 : (byte)0);
            root.SetUInt8("Volume", (byte)uts.Volume);
            root.SetUInt8("VolumeVrtn", (byte)uts.VolumeVariance);
            root.SetSingle("PitchVariation", uts.PitchVariance);
            root.SetSingle("Elevation", uts.Elevation);
            root.SetSingle("MinDistance", uts.MinDistance);
            root.SetSingle("MaxDistance", uts.MaxDistance);
            root.SetSingle("DistanceCutoff", uts.DistanceCutoff);
            root.SetUInt8("Priority", (byte)uts.Priority);
            root.SetUInt8("Hours", (byte)uts.Hours);
            root.SetUInt8("Times", (byte)uts.Times);
            root.SetUInt8("Interval", (byte)uts.Interval);
            root.SetUInt8("IntervalVrtn", (byte)uts.IntervalVariance);
            root.SetResRef("Sound", uts.Sound);
            root.SetString("Comment", uts.Comment);

            // Set sounds list
            var soundsList = new GFFList();
            root.SetList("Sounds", soundsList);
            // if (uts.Sounds != null) { foreach (var sound in uts.Sounds) { ... } }

            return gff;
        }
    }
}
