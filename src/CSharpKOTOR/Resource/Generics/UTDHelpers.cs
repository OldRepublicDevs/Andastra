using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resources;

namespace CSharpKOTOR.Resource.Generics
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utd.py
    // Original: construct_utd and dismantle_utd functions
    public static class UTDHelpers
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utd.py:498-560
        // Original: def construct_utd(gff: GFF) -> UTD:
        public static UTD ConstructUtd(GFF gff)
        {
            var utd = new UTD();
            var root = gff.Root;

            // Extract basic fields
            utd.Tag = root.Acquire<string>("Tag", "");
            utd.Name = root.Acquire<LocalizedString>("LocName", LocalizedString.FromInvalid());
            utd.ResRef = root.Acquire<ResRef>("TemplateResRef", ResRef.FromBlank());
            utd.AppearanceId = root.Acquire<int>("GenericType", 0);
            utd.Static = root.Acquire<int>("Static", 0) != 0;
            utd.OpenState = root.Acquire<int>("OpenState", 0) != 0;
            utd.Locked = root.Acquire<int>("Locked", 0) != 0;
            utd.Plot = root.Acquire<int>("Plot", 0) != 0;
            utd.NotBlastable = root.Acquire<int>("NotBlastable", 0) != 0;
            utd.HP = root.Acquire<int>("HP", 0);
            utd.CurrentHP = root.Acquire<int>("CurrentHP", 0);
            utd.Fortitude = root.Acquire<int>("Fort", 0);
            utd.Reflex = root.Acquire<int>("Ref", 0);
            utd.DC = root.Acquire<int>("OpenLockDC", 0);
            utd.KeyRequired = root.Acquire<ResRef>("KeyName", ResRef.FromBlank());
            utd.Description = root.Acquire<LocalizedString>("Description", LocalizedString.FromInvalid());

            // Extract script hooks
            utd.OnOpen = root.Acquire<ResRef>("OnOpen", ResRef.FromBlank());
            utd.OnClosed = root.Acquire<ResRef>("OnClosed", ResRef.FromBlank());
            utd.OnDamaged = root.Acquire<ResRef>("OnDamaged", ResRef.FromBlank());
            utd.OnDeath = root.Acquire<ResRef>("OnDeath", ResRef.FromBlank());
            utd.OnHeartbeat = root.Acquire<ResRef>("OnHeartbeat", ResRef.FromBlank());
            utd.OnMeleeAttacked = root.Acquire<ResRef>("OnMeleeAttacked", ResRef.FromBlank());
            utd.OnSpellCastAt = root.Acquire<ResRef>("OnSpellCastAt", ResRef.FromBlank());
            utd.OnUserDefined = root.Acquire<ResRef>("OnUserDefined", ResRef.FromBlank());
            utd.OnLock = root.Acquire<ResRef>("OnLock", ResRef.FromBlank());
            utd.OnUnlock = root.Acquire<ResRef>("OnUnlock", ResRef.FromBlank());
            utd.OnFailToOpen = root.Acquire<ResRef>("OnFailToOpen", ResRef.FromBlank());

            return utd;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utd.py:563-665
        // Original: def dismantle_utd(utd: UTD, game: Game = Game.K2, *, use_deprecated: bool = True) -> GFF:
        public static GFF DismantleUtd(UTD utd, Game game = Game.K2, bool useDeprecated = true)
        {
            var gff = new GFF(GFFContent.UTD);
            var root = gff.Root;

            // Set basic fields
            root.SetString("Tag", utd.Tag);
            root.SetLocString("LocName", utd.Name);
            root.SetResRef("TemplateResRef", utd.ResRef);
            root.SetUInt32("GenericType", (uint)utd.AppearanceId);
            root.SetUInt8("Static", utd.Static ? (byte)1 : (byte)0);
            root.SetUInt8("OpenState", (byte)utd.OpenState);
            root.SetUInt8("Locked", utd.Locked ? (byte)1 : (byte)0);
            root.SetUInt8("Plot", utd.Plot ? (byte)1 : (byte)0);
            root.SetUInt8("NotBlastable", utd.NotBlastable ? (byte)1 : (byte)0);
            root.SetInt16("HP", (short)utd.HP);
            root.SetInt16("CurrentHP", (short)utd.CurrentHP);
            root.SetUInt8("Fort", (byte)utd.Fortitude);
            root.SetUInt8("Ref", (byte)utd.Reflex);
            root.SetUInt8("OpenLockDC", (byte)utd.DC);
            root.SetResRef("KeyName", utd.KeyRequired);
            root.SetLocString("Description", utd.Description);

            // Set script hooks
            root.SetResRef("OnOpen", utd.OnOpen);
            root.SetResRef("OnClosed", utd.OnClosed);
            root.SetResRef("OnDamaged", utd.OnDamaged);
            root.SetResRef("OnDeath", utd.OnDeath);
            root.SetResRef("OnHeartbeat", utd.OnHeartbeat);
            root.SetResRef("OnMeleeAttacked", utd.OnMeleeAttacked);
            root.SetResRef("OnSpellCastAt", utd.OnSpellCastAt);
            root.SetResRef("OnUserDefined", utd.OnUserDefined);
            root.SetResRef("OnLock", utd.OnLock);
            root.SetResRef("OnUnlock", utd.OnUnlock);
            root.SetResRef("OnFailToOpen", utd.OnFailToOpen);

            return gff;
        }
    }
}
