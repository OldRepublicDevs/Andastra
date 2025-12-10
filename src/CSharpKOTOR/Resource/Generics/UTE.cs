using System.Collections.Generic;
using CSharpKOTOR.Common;
using CSharpKOTOR.Resource.Formats.GFF;
using JetBrains.Annotations;

namespace CSharpKOTOR.Resource.Generics
{
    /// <summary>
    /// Stores encounter data.
    ///
    /// UTE files are GFF-based format files that store encounter definitions including
    /// creature spawn lists, difficulty, respawn settings, and script hooks.
    /// </summary>
    [PublicAPI]
    public sealed class UTE
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/ute.py:15
        // Original: BINARY_TYPE = ResourceType.UTE
        public static readonly ResourceType BinaryType = ResourceType.UTE;

        // Basic UTE properties
        public ResRef ResRef { get; set; } = ResRef.FromInvalid();
        public string Tag { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public bool Active { get; set; }
        public int DifficultyId { get; set; }
        public int DifficultyIndex { get; set; }
        public int Faction { get; set; }
        public int MaxCreatures { get; set; }
        public int RecCreatures { get; set; }
        public int Respawn { get; set; }
        public int RespawnTime { get; set; }
        public int Reset { get; set; }
        public int ResetTime { get; set; }
        public int PlayerOnly { get; set; }
        public int SingleSpawn { get; set; }
        public int OnEntered { get; set; }
        public int OnExit { get; set; }
        public int OnExhausted { get; set; }
        public int OnHeartbeat { get; set; }
        public int OnUserDefined { get; set; }
        public ResRef OnEnteredScript { get; set; } = ResRef.FromInvalid();
        public ResRef OnExitScript { get; set; } = ResRef.FromInvalid();
        public ResRef OnExhaustedScript { get; set; } = ResRef.FromInvalid();
        public ResRef OnHeartbeatScript { get; set; } = ResRef.FromInvalid();
        public ResRef OnUserDefinedScript { get; set; } = ResRef.FromInvalid();

        // Creature spawn list
        public List<UTECreature> Creatures { get; set; } = new List<UTECreature>();

        public UTE()
        {
        }
    }

    /// <summary>
    /// Represents a creature spawn in an encounter.
    /// </summary>
    [PublicAPI]
    public sealed class UTECreature
    {
        public ResRef ResRef { get; set; } = ResRef.FromInvalid();
        public int Appearance { get; set; }
        public int SingleSpawn { get; set; }
        public int CR { get; set; }
        public int GuaranteedCount { get; set; }

        public UTECreature()
        {
        }
    }
}
