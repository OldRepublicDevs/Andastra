using System.Collections.Generic;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resources;
using JetBrains.Annotations;

namespace CSharpKOTOR.Resource.Generics
{
    /// <summary>
    /// Stores creature data.
    ///
    /// UTC files are GFF-based format files that store creature definitions including
    /// stats, appearance, inventory, feats, and script hooks.
    /// </summary>
    [PublicAPI]
    public sealed class UTC
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utc.py:18
        // Original: BINARY_TYPE = ResourceType.UTC
        public static readonly ResourceType BinaryType = ResourceType.UTC;

        // Basic creature properties
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utc.py:36-47
        // Original: resref: "TemplateResRef" field
        public ResRef ResRef { get; set; }

        // Original: tag: "Tag" field
        public string Tag { get; set; }

        // Original: comment: "Comment" field
        public string Comment { get; set; }

        // Original: conversation: "Conversation" field
        public ResRef Conversation { get; set; }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utc.py:348
        // Original: self.first_name: LocalizedString = LocalizedString.from_invalid()
        public LocalizedString FirstName { get; set; }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utc.py:383
        // Original: self.alignment: int = 0
        public int Alignment { get; set; }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utc.py:434
        // Original: self.equipment: dict[EquipmentSlot, InventoryItem] = {}
        public Dictionary<EquipmentSlot, InventoryItem> Equipment { get; set; }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utc.py:18
        // Original: def __init__(self):
        public UTC()
        {
            ResRef = ResRef.FromBlank();
            Tag = string.Empty;
            Comment = string.Empty;
            Conversation = ResRef.FromBlank();
            FirstName = LocalizedString.FromInvalid();
            Alignment = 0;
            Equipment = new Dictionary<EquipmentSlot, InventoryItem>();
        }

        // Additional basic properties that would be implemented in full version
        public int AppearanceId { get; set; }
        public int SoundSetFile { get; set; }
        public int PortraitId { get; set; }
        public int FactionId { get; set; }
        public int Race { get; set; }
        public int Subrace { get; set; }
        public int Gender { get; set; }
        public int Class1 { get; set; }
        public int ClassLevel1 { get; set; }
        public int HitPoints { get; set; }
        public int CurrentHitPoints { get; set; }
        public int MaxHitPoints { get; set; }
        public int ForcePoints { get; set; }
        public int Experience { get; set; }
        public int NaturalAC { get; set; }
        public bool IsPC { get; set; }
        public bool Interruptable { get; set; }
        public bool NoPermDeath { get; set; }
        public bool Plot { get; set; }
        public bool NotReorienting { get; set; }

        // Equipment slots (simplified)
        public ResRef RightHand { get; set; } = ResRef.FromBlank();
        public ResRef LeftHand { get; set; } = ResRef.FromBlank();
        public ResRef Body { get; set; } = ResRef.FromBlank();
        public ResRef Head { get; set; } = ResRef.FromBlank();
        public ResRef Implant { get; set; } = ResRef.FromBlank();
        public ResRef Belt { get; set; } = ResRef.FromBlank();
        public ResRef Armor { get; set; } = ResRef.FromBlank();
        public ResRef Gloves { get; set; } = ResRef.FromBlank();
        public ResRef Boots { get; set; } = ResRef.FromBlank();

        // Script hooks (simplified)
        public ResRef OnSpawn { get; set; } = ResRef.FromBlank();
        public ResRef OnDeath { get; set; } = ResRef.FromBlank();
        public ResRef OnHeartbeat { get; set; } = ResRef.FromBlank();
        public ResRef OnNotice { get; set; } = ResRef.FromBlank();
        public ResRef OnSpellAt { get; set; } = ResRef.FromBlank();
        public ResRef OnAttacked { get; set; } = ResRef.FromBlank();
        public ResRef OnDamaged { get; set; } = ResRef.FromBlank();
        public ResRef OnEndRound { get; set; } = ResRef.FromBlank();
        public ResRef OnEndCombatRound { get; set; } = ResRef.FromBlank();
        public ResRef OnDialogue { get; set; } = ResRef.FromBlank();
        public ResRef OnBlocked { get; set; } = ResRef.FromBlank();
        public ResRef OnDisturbed { get; set; } = ResRef.FromBlank();
        public ResRef OnCombatRoundEnd { get; set; } = ResRef.FromBlank();
        public ResRef OnSpawned { get; set; } = ResRef.FromBlank();
        public ResRef OnRested { get; set; } = ResRef.FromBlank();
        public ResRef OnCreatureDamaged { get; set; } = ResRef.FromBlank();
        public ResRef OnInventoryDisturbed { get; set; } = ResRef.FromBlank();
        public ResRef OnDeath2 { get; set; } = ResRef.FromBlank();
    }
}
