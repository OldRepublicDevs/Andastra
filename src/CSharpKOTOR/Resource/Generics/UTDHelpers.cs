using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resources;
using static CSharpKOTOR.Common.GameExtensions;

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
            utd.AutoRemoveKey = root.Acquire<int>("AutoRemoveKey", 0) != 0;
            utd.Conversation = root.Acquire<ResRef>("Conversation", ResRef.FromBlank());
            utd.FactionId = root.Acquire<int>("Faction", 0);
            utd.Plot = root.Acquire<int>("Plot", 0) != 0;
            utd.Min1Hp = root.Acquire<int>("Min1HP", 0) != 0;
            utd.KeyRequired = root.Acquire<int>("KeyRequired", 0) != 0;
            utd.Lockable = root.Acquire<int>("Lockable", 0) != 0;
            utd.Locked = root.Acquire<int>("Locked", 0) != 0;
            utd.UnlockDc = root.Acquire<int>("OpenLockDC", 0);
            utd.KeyName = root.Acquire<string>("KeyName", "");
            utd.AnimationState = root.Acquire<int>("AnimationState", 0);
            utd.MaximumHp = root.Acquire<int>("HP", 0);
            utd.CurrentHp = root.Acquire<int>("CurrentHP", 0);
            utd.Hardness = root.Acquire<int>("Hardness", 0);
            utd.Fortitude = root.Acquire<int>("Fort", 0);
            utd.AppearanceId = root.Acquire<int>("GenericType", 0);
            utd.Static = root.Acquire<int>("Static", 0) != 0;
            utd.OnClick = root.Acquire<ResRef>("OnClick", ResRef.FromBlank());
            utd.OnOpenFailed = root.Acquire<ResRef>("OnFailToOpen", ResRef.FromBlank());
            utd.Comment = root.Acquire<string>("Comment", "");
            utd.UnlockDiff = root.Acquire<int>("OpenLockDiff", 0);
            utd.UnlockDiffMod = root.Acquire<int>("OpenLockDiffMod", 0);
            utd.Description = root.Acquire<LocalizedString>("Description", LocalizedString.FromInvalid());
            utd.Reflex = root.Acquire<int>("Ref", 0);
            utd.Willpower = root.Acquire<int>("Will", 0);
            utd.OpenState = root.Acquire<int>("OpenState", 0);
            utd.NotBlastable = root.Acquire<int>("NotBlastable", 0) != 0;

            // Extract script hooks
            utd.OnClosed = root.Acquire<ResRef>("OnClosed", ResRef.FromBlank());
            utd.OnDamaged = root.Acquire<ResRef>("OnDamaged", ResRef.FromBlank());
            utd.OnDeath = root.Acquire<ResRef>("OnDeath", ResRef.FromBlank());
            utd.OnHeartbeat = root.Acquire<ResRef>("OnHeartbeat", ResRef.FromBlank());
            utd.OnLock = root.Acquire<ResRef>("OnLock", ResRef.FromBlank());
            utd.OnMelee = root.Acquire<ResRef>("OnMeleeAttacked", ResRef.FromBlank());
            utd.OnOpen = root.Acquire<ResRef>("OnOpen", ResRef.FromBlank());
            utd.OnUnlock = root.Acquire<ResRef>("OnUnlock", ResRef.FromBlank());
            utd.OnUserDefined = root.Acquire<ResRef>("OnUserDefined", ResRef.FromBlank());
            utd.OnPower = root.Acquire<ResRef>("OnSpellCastAt", ResRef.FromBlank());

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
            root.SetUInt8("AutoRemoveKey", utd.AutoRemoveKey ? (byte)1 : (byte)0);
            root.SetResRef("Conversation", utd.Conversation);
            root.SetUInt32("Faction", (uint)utd.FactionId);
            root.SetUInt8("Plot", utd.Plot ? (byte)1 : (byte)0);
            root.SetUInt8("Min1HP", utd.Min1Hp ? (byte)1 : (byte)0);
            root.SetUInt8("KeyRequired", utd.KeyRequired ? (byte)1 : (byte)0);
            root.SetUInt8("Lockable", utd.Lockable ? (byte)1 : (byte)0);
            root.SetUInt8("Locked", utd.Locked ? (byte)1 : (byte)0);
            root.SetUInt8("OpenLockDC", (byte)utd.UnlockDc);
            root.SetString("KeyName", utd.KeyName);
            root.SetUInt8("AnimationState", (byte)utd.AnimationState);
            root.SetInt16("HP", (short)utd.MaximumHp);
            root.SetInt16("CurrentHP", (short)utd.CurrentHp);
            root.SetUInt8("Hardness", (byte)utd.Hardness);
            root.SetUInt8("Fort", (byte)utd.Fortitude);
            root.SetUInt8("GenericType", (byte)utd.AppearanceId);
            root.SetUInt8("Static", utd.Static ? (byte)1 : (byte)0);
            root.SetResRef("OnClick", utd.OnClick);
            root.SetResRef("OnFailToOpen", utd.OnOpenFailed);
            root.SetString("Comment", utd.Comment);

            // KotOR 2 only fields
            if (game.IsK2())
            {
                root.SetUInt8("OpenLockDiff", (byte)utd.UnlockDiff);
                root.SetInt8("OpenLockDiffMod", (sbyte)utd.UnlockDiffMod);
                root.SetUInt8("OpenState", (byte)utd.OpenState);
                root.SetUInt8("NotBlastable", utd.NotBlastable ? (byte)1 : (byte)0);
            }

            if (useDeprecated)
            {
                root.SetLocString("Description", utd.Description);
                root.SetUInt8("Ref", (byte)utd.Reflex);
                root.SetUInt8("Will", (byte)utd.Willpower);
            }

            // Set script hooks
            root.SetResRef("OnClosed", utd.OnClosed);
            root.SetResRef("OnDamaged", utd.OnDamaged);
            root.SetResRef("OnDeath", utd.OnDeath);
            root.SetResRef("OnHeartbeat", utd.OnHeartbeat);
            root.SetResRef("OnLock", utd.OnLock);
            root.SetResRef("OnMeleeAttacked", utd.OnMelee);
            root.SetResRef("OnOpen", utd.OnOpen);
            root.SetResRef("OnUnlock", utd.OnUnlock);
            root.SetResRef("OnUserDefined", utd.OnUserDefined);
            root.SetResRef("OnSpellCastAt", utd.OnPower);

            return gff;
        }
    }
}
