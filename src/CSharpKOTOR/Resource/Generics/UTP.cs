using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resources;
using JetBrains.Annotations;

namespace CSharpKOTOR.Resource.Generics
{
    /// <summary>
    /// Stores placeable data.
    ///
    /// UTP files are GFF-based format files that store placeable object definitions including
    /// lock/unlock mechanics, HP, inventory, scripts, and appearance.
    /// </summary>
    [PublicAPI]
    public sealed class UTP
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utp.py:17
        // Original: BINARY_TYPE = ResourceType.UTP
        public static readonly ResourceType BinaryType = ResourceType.UTP;

        // Basic placeable properties
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utp.py:33-41
        // Original: resref: "TemplateResRef" field
        public ResRef ResRef { get; set; }

        // Original: tag: "Tag" field
        public string Tag { get; set; }

        // Original: name: "LocName" field
        public LocalizedString Name { get; set; }

        // Original: appearance_id: "Appearance" field
        public int AppearanceId { get; set; }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utp.py:17
        // Original: def __init__(self):
        public UTP()
        {
            ResRef = ResRef.FromBlank();
            Tag = string.Empty;
            Name = LocalizedString.FromInvalid();
            AppearanceId = 0;
        }

        // Additional basic properties that would be implemented in full version
        public LocalizedString Description { get; set; } = LocalizedString.FromInvalid();
        public bool Static { get; set; }
        public bool Useable { get; set; }
        public bool Locked { get; set; }
        public bool Plot { get; set; }
        public bool HasInventory { get; set; }
        public bool PartyInteract { get; set; }
        public bool BodyBag { get; set; }
        public bool DeadSelectable { get; set; }
        public int HP { get; set; }
        public int CurrentHP { get; set; }
        public int Hardness { get; set; }
        public int Fortitude { get; set; }
        public int Reflex { get; set; }
        public int Will { get; set; }
        public ResRef KeyRequired { get; set; } = ResRef.FromBlank();
        public ResRef KeyName { get; set; } = ResRef.FromBlank();

        // Script hooks (simplified)
        public ResRef OnOpen { get; set; } = ResRef.FromBlank();
        public ResRef OnClosed { get; set; } = ResRef.FromBlank();
        public ResRef OnDamaged { get; set; } = ResRef.FromBlank();
        public ResRef OnDeath { get; set; } = ResRef.FromBlank();
        public ResRef OnHeartbeat { get; set; } = ResRef.FromBlank();
        public ResRef OnMeleeAttacked { get; set; } = ResRef.FromBlank();
        public ResRef OnSpellCastAt { get; set; } = ResRef.FromBlank();
        public ResRef OnUserDefined { get; set; } = ResRef.FromBlank();
        public ResRef OnLock { get; set; } = ResRef.FromBlank();
        public ResRef OnUnlock { get; set; } = ResRef.FromBlank();
        public ResRef OnFailToOpen { get; set; } = ResRef.FromBlank();
    }
}
