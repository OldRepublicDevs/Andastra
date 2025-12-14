using System.Collections.Generic;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resources;

namespace CSharpKOTOR.Resource.Generics
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utp.py
    // Original: construct_utp and dismantle_utp functions
    public static class UTPHelpers
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utp.py:185-258
        // Original: def construct_utp(gff: GFF) -> UTP:
        public static UTP ConstructUtp(GFF gff)
        {
            var utp = new UTP();
            var root = gff.Root;

            // Extract basic fields
            utp.Tag = root.Acquire<string>("Tag", "");
            utp.Name = root.Acquire<LocalizedString>("LocName", LocalizedString.FromInvalid());
            utp.ResRef = root.Acquire<ResRef>("TemplateResRef", ResRef.FromBlank());
            utp.AppearanceId = root.Acquire<int>("Appearance", 0);
            utp.Static = root.Acquire<int>("Static", 0) != 0;
            utp.Useable = root.Acquire<int>("Useable", 0) != 0;
            utp.Locked = root.Acquire<int>("Locked", 0) != 0;
            utp.Plot = root.Acquire<int>("Plot", 0) != 0;
            utp.HasInventory = root.Acquire<int>("HasInventory", 0) != 0;
            utp.PartyInteract = root.Acquire<int>("PartyInteract", 0) != 0;
            utp.HP = root.Acquire<int>("HP", 0);
            utp.CurrentHP = root.Acquire<int>("CurrentHP", 0);
            utp.Hardness = root.Acquire<int>("Hardness", 0);
            utp.Fortitude = root.Acquire<int>("Fort", 0);
            utp.Reflex = root.Acquire<int>("Ref", 0);
            utp.Will = root.Acquire<int>("Will", 0);
            utp.KeyRequired = root.Acquire<ResRef>("KeyName", ResRef.FromBlank());
            utp.Description = root.Acquire<LocalizedString>("Description", LocalizedString.FromInvalid());

            // Extract script hooks
            utp.OnOpen = root.Acquire<ResRef>("OnOpen", ResRef.FromBlank());
            utp.OnClosed = root.Acquire<ResRef>("OnClosed", ResRef.FromBlank());
            utp.OnDamaged = root.Acquire<ResRef>("OnDamaged", ResRef.FromBlank());
            utp.OnDeath = root.Acquire<ResRef>("OnDeath", ResRef.FromBlank());
            utp.OnHeartbeat = root.Acquire<ResRef>("OnHeartbeat", ResRef.FromBlank());
            utp.OnMeleeAttacked = root.Acquire<ResRef>("OnMeleeAttacked", ResRef.FromBlank());
            utp.OnSpellCastAt = root.Acquire<ResRef>("OnSpellCastAt", ResRef.FromBlank());
            utp.OnUserDefined = root.Acquire<ResRef>("OnUserDefined", ResRef.FromBlank());
            utp.OnLock = root.Acquire<ResRef>("OnLock", ResRef.FromBlank());
            utp.OnUnlock = root.Acquire<ResRef>("OnUnlock", ResRef.FromBlank());
            utp.OnFailToOpen = root.Acquire<ResRef>("OnFailToOpen", ResRef.FromBlank());

            // Extract inventory
            var itemList = root.Acquire<GFFList>("ItemList", new GFFList());
            // utp.Inventory would need to be a List<InventoryItem> property
            // foreach (var itemStruct in itemList) { ... }

            return utp;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utp.py:261-377
        // Original: def dismantle_utp(utp: UTP, game: Game = Game.K2, *, use_deprecated: bool = True) -> GFF:
        public static GFF DismantleUtp(UTP utp, Game game = Game.K2, bool useDeprecated = true)
        {
            var gff = new GFF(GFFContent.UTP);
            var root = gff.Root;

            // Set basic fields
            root.SetString("Tag", utp.Tag);
            root.SetLocString("LocName", utp.Name);
            root.SetResRef("TemplateResRef", utp.ResRef);
            root.SetUInt32("Appearance", (uint)utp.AppearanceId);
            root.SetUInt8("Static", utp.Static ? (byte)1 : (byte)0);
            root.SetUInt8("Useable", utp.Useable ? (byte)1 : (byte)0);
            root.SetUInt8("Locked", utp.Locked ? (byte)1 : (byte)0);
            root.SetUInt8("Plot", utp.Plot ? (byte)1 : (byte)0);
            root.SetUInt8("HasInventory", utp.HasInventory ? (byte)1 : (byte)0);
            root.SetUInt8("PartyInteract", utp.PartyInteract ? (byte)1 : (byte)0);
            root.SetInt16("HP", (short)utp.HP);
            root.SetInt16("CurrentHP", (short)utp.CurrentHP);
            root.SetUInt8("Hardness", (byte)utp.Hardness);
            root.SetUInt8("Fort", (byte)utp.Fortitude);
            root.SetUInt8("Ref", (byte)utp.Reflex);
            root.SetUInt8("Will", (byte)utp.Will);
            root.SetResRef("KeyName", utp.KeyRequired);
            root.SetLocString("Description", utp.Description);

            // Set script hooks
            root.SetResRef("OnOpen", utp.OnOpen);
            root.SetResRef("OnClosed", utp.OnClosed);
            root.SetResRef("OnDamaged", utp.OnDamaged);
            root.SetResRef("OnDeath", utp.OnDeath);
            root.SetResRef("OnHeartbeat", utp.OnHeartbeat);
            root.SetResRef("OnMeleeAttacked", utp.OnMeleeAttacked);
            root.SetResRef("OnSpellCastAt", utp.OnSpellCastAt);
            root.SetResRef("OnUserDefined", utp.OnUserDefined);
            root.SetResRef("OnLock", utp.OnLock);
            root.SetResRef("OnUnlock", utp.OnUnlock);
            root.SetResRef("OnFailToOpen", utp.OnFailToOpen);

            // Set inventory
            var itemList = new GFFList();
            root.SetList("ItemList", itemList);
            // if (utp.Inventory != null) { foreach (var item in utp.Inventory) { ... } }

            return gff;
        }
    }
}
