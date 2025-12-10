using CSharpKOTOR.Common;
using CSharpKOTOR.Resources;
using JetBrains.Annotations;

namespace CSharpKOTOR.Resource.Generics
{
    /// <summary>
    /// Stores trigger data.
    ///
    /// UTT files are GFF-based format files that store trigger definitions including
    /// trap mechanics, script hooks, and activation settings.
    /// </summary>
    [PublicAPI]
    public sealed class UTT
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utt.py:16
        // Original: BINARY_TYPE = ResourceType.UTT
        public static readonly ResourceType BinaryType = ResourceType.UTT;

        // Basic UTT properties
        public ResRef ResRef { get; set; } = ResRef.FromInvalid();
        public string Tag { get; set; } = string.Empty;
        public bool AutoRemoveKey { get; set; }
        public int FactionId { get; set; }
        public int Cursor { get; set; }
        public int HighlightHeight { get; set; }
        public int KeyName { get; set; }
        public ResRef KeyRequired { get; set; } = ResRef.FromInvalid();
        public LocalizedString Name { get; set; } = LocalizedString.FromInvalid();
        public LocalizedString Description { get; set; } = LocalizedString.FromInvalid();
        public int TrapDetectable { get; set; }
        public int TrapDetectDc { get; set; }
        public int TrapDisarmable { get; set; }
        public int TrapDisarmDc { get; set; }
        public int TrapFlag { get; set; }
        public int TrapOneShot { get; set; }
        public ResRef TrapType { get; set; } = ResRef.FromInvalid();
        public int ScriptHeartbeat { get; set; }
        public int ScriptOnEnter { get; set; }
        public int ScriptOnExit { get; set; }
        public int ScriptUserDefined { get; set; }
        public ResRef OnHeartbeatScript { get; set; } = ResRef.FromInvalid();
        public ResRef OnEnterScript { get; set; } = ResRef.FromInvalid();
        public ResRef OnExitScript { get; set; } = ResRef.FromInvalid();
        public ResRef OnUserDefinedScript { get; set; } = ResRef.FromInvalid();
        public ResRef OnDisarmScript { get; set; } = ResRef.FromInvalid();
        public ResRef OnTrapTriggeredScript { get; set; } = ResRef.FromInvalid();
        public string Comment { get; set; } = string.Empty;

        public UTT()
        {
        }
    }
}
