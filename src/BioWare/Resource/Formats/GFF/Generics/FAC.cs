using System.Collections.Generic;
using BioWare.Common;
using BioWare.Resource;
using JetBrains.Annotations;

namespace BioWare.Resource.Formats.GFF.Generics
{
    /// <summary>
    /// Stores faction data.
    ///
    /// FAC files are GFF-based format files that store faction information including
    /// faction names, parent relationships, global flags, and reputation values between factions.
    /// </summary>
    [PublicAPI]
    public sealed class FAC
    {
        // Matching PyKotor implementation pattern
        // Original: BINARY_TYPE = ResourceType.FAC
        public static readonly ResourceType BinaryType = ResourceType.FAC;

        // List of factions
        public List<FACFaction> Factions { get; set; } = new List<FACFaction>();

        // List of reputation entries (relationships between factions)
        public List<FACReputation> Reputations { get; set; } = new List<FACReputation>();

        public FAC()
        {
        }
    }

    /// <summary>
    /// Stores data of an individual faction.
    /// </summary>
    [PublicAPI]
    public sealed class FACFaction
    {
        // Engine references: k2_win_gog_aspyr_swkotor2.exe:0x005acf30 line 40, k1_win_gog_swkotor.exe:0x0052b5c0 line 40
        // Engine default: "" (k2_win_gog_aspyr_swkotor2.exe:0x005acf30 line 38, k1_win_gog_swkotor.exe:0x0052b5c0 line 38)
        public string Name { get; set; } = string.Empty;

        // Engine default: 0 (k2_win_gog_aspyr_swkotor2.exe:0x005acf30 line 47, k1_win_gog_swkotor.exe:0x0052b5c0 line 47)
        // Standard factions use 0xFFFFFFFF (-1) for no parent
        public int ParentId { get; set; } = unchecked((int)0xFFFFFFFF);

        // Engine default: 0, but if field missing defaults to 1 (k2_win_gog_aspyr_swkotor2.exe:0x005acf30 lines 48-52, k1_win_gog_swkotor.exe:0x0052b5c0 lines 48-52)
        public bool IsGlobal { get; set; }

        public FACFaction()
        {
        }
    }

    /// <summary>
    /// Stores reputation data between two factions.
    /// </summary>
    [PublicAPI]
    public sealed class FACReputation
    {
        // Engine reference: k2_win_gog_aspyr_swkotor2.exe:0x005ad1a0 line 23, k1_win_gog_swkotor.exe:0x0052b830 line 23
        public int FactionId1 { get; set; }

        // Engine reference: k2_win_gog_aspyr_swkotor2.exe:0x005ad1a0 line 24, k1_win_gog_swkotor.exe:0x0052b830 line 24
        public int FactionId2 { get; set; }

        // Engine default: 100 (k2_win_gog_aspyr_swkotor2.exe:0x005ad1a0 line 21, k1_win_gog_swkotor.exe:0x0052b830 line 20)
        // Note: Only written if != 100, so default is 100
        public int Reputation { get; set; } = 100;

        public FACReputation()
        {
        }
    }
}

