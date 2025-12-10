using CSharpKOTOR.Common;
using CSharpKOTOR.Resources;
using JetBrains.Annotations;

namespace CSharpKOTOR.Resource.Generics
{
    /// <summary>
    /// Stores sound data.
    ///
    /// UTS files are GFF-based format files that store sound object definitions including
    /// audio settings, positioning, looping, and volume controls.
    /// </summary>
    [PublicAPI]
    public sealed class UTS
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/uts.py:16
        // Original: BINARY_TYPE = ResourceType.UTS
        public static readonly ResourceType BinaryType = ResourceType.UTS;

        // Basic UTS properties
        public ResRef ResRef { get; set; } = ResRef.FromBlank();
        public string Tag { get; set; } = string.Empty;
        public bool Active { get; set; }
        public bool Continuous { get; set; }
        public bool Looping { get; set; }
        public bool Positional { get; set; }
        public bool RandomPosition { get; set; }
        public bool Random { get; set; }
        public int Volume { get; set; }
        public int VolumeVariance { get; set; }
        public int PitchVariance { get; set; }
        public float Elevation { get; set; }
        public float MinDistance { get; set; }
        public float MaxDistance { get; set; }
        public float DistanceCutoff { get; set; }
        public int Priority { get; set; }
        public int Hours { get; set; }
        public int Times { get; set; }
        public int Interval { get; set; }
        public int IntervalVariance { get; set; }
        public ResRef Sound { get; set; } = ResRef.FromBlank();
        public string Comment { get; set; } = string.Empty;

        public UTS()
        {
        }
    }
}
