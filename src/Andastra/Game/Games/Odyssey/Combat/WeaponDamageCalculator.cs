using System;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Game.Games.Odyssey.Components;
using Andastra.Game.Games.Odyssey.Data;
using Andastra.Game.Games.Common.Combat;
using BaseItemData = Andastra.Game.Games.Odyssey.Data.GameDataManager.BaseItemData;

namespace Andastra.Game.Games.Odyssey.Combat
{
    /// <summary>
    /// Calculates weapon damage from equipped items using baseitems.2da (Odyssey engine).
    /// </summary>
    /// <remarks>
    /// Odyssey Weapon Damage Calculator:
    /// - Based on common weapon damage calculation logic
    /// - Located via string references in k2_win_gog_aspyr_swkotor2.exe: ["damagedice"] @ (K1: TODO: Find this address, TSL: 0x007c2e60), ["damagedie"] @ (K1: TODO: Find this address, TSL: 0x007c2e70), ["damagebonus"] @ (K1: TODO: Find this address, TSL: 0x007c2e80)
    /// - ["DamageDice"] @ (K1: TODO: Find this address, TSL: 0x007c2d3c), ["DamageDie"] @ (K1: TODO: Find this address, TSL: 0x007c2d30) - damage dice fields
    /// - ["BaseItem"] @ (K1: TODO: Find this address, TSL: 0x007c2e90) (base item ID in item GFF), ["weapontype"] @ (K1: TODO: Find this address, TSL: 0x007c2ea0)
    /// - ["OnHandDamageMod"] @ (K1: TODO: Find this address, TSL: 0x007c2e40), ["OffHandDamageMod"] @ (K1: TODO: Find this address, TSL: 0x007c2e18) (damage modifiers in k2_win_gog_aspyr_swkotor2.exe)
    /// - Cross-engine: nwmain.exe (Aurora uses different 2DA tables), daorigins.exe (Eclipse uses different damage system)
    /// - Inheritance: BaseWeaponDamageCalculator (Runtime.Games.Common.Combat) implements common damage calculation logic
    ///   - Odyssey: WeaponDamageCalculator : BaseWeaponDamageCalculator (Runtime.Games.Odyssey) - Odyssey-specific baseitems.2da lookup
    ///   - Aurora: AuroraWeaponDamageCalculator : BaseWeaponDamageCalculator (Runtime.Games.Aurora) - Aurora-specific baseitems.2da lookup
    ///   - Eclipse: EclipseWeaponDamageCalculator : BaseWeaponDamageCalculator (Runtime.Games.Eclipse) - Eclipse-specific damage system
    /// - Original implementation: use weapon damage calculation logic from baseitems.2da
    ///   - [TODO: Function name] @ (K1: 0x00550f30, TSL: 0x005d7fc0) - saves DamageDice/DamageDie to GFF,
    ///   - [TODO: Function name] @ (K1: 0x00552350, TSL: 0x005d9670) - loads OnHandDamageMod/OffHandDamageMod from GFF
    /// - Damage formula: Roll(damagedice * damagedie) + damagebonus + ability modifier
    /// - Ability modifier: STR for melee (default), DEX for ranged, DEX for finesse melee/lightsabers (if feat)
    /// - Finesse feats: FEAT_FINESSE_LIGHTSABERS (193) for lightsabers, FEAT_FINESSE_MELEE_WEAPONS (194) for melee
    /// - Offhand attacks: Get half ability modifier (abilityMod / 2)
    /// - Critical hits: Multiply damage by critmult from baseitems.2da (crithitmult column)
    /// - Based on baseitems.2da columns: numdice/damagedice (dice count), dietoroll/damagedie (die size), damagebonus, crithitmult, critthreat
    /// - Weapon lookup: Get equipped weapon from inventory (RIGHTWEAPON slot 4, LEFTWEAPON slot 5), get BaseItem ID, lookup in baseitems.2da
    /// - Unarmed damage: 1d3 (1 die, size 3) if no weapon equipped
    /// </remarks>
    public class WeaponDamageCalculator : BaseWeaponDamageCalculator
    {
        private readonly TwoDATableManager _tableManager;

        /// <summary>
        /// Gets the main hand weapon slot number (RIGHTWEAPON = 4 for Odyssey).
        /// </summary>
        protected override int MainHandWeaponSlot => 4;

        /// <summary>
        /// Gets the offhand weapon slot number (LEFTWEAPON = 5 for Odyssey).
        /// </summary>
        protected override int OffHandWeaponSlot => 5;

        public WeaponDamageCalculator(TwoDATableManager tableManager)
        {
            _tableManager = tableManager ?? throw new ArgumentNullException("tableManager");
        }

        /// <summary>
        /// Gets damage dice information from baseitems.2da (Odyssey-specific).
        /// </summary>
        protected override bool GetDamageDiceFromTable(int baseItemId, out int damageDice, out int damageDie, out int damageBonus)
        {
            damageDice = 1;
            damageDie = 8;
            damageBonus = 0;

            try
            {
                var twoDARow = _tableManager.GetRow("baseitems", baseItemId);
                if (twoDARow != null)
                {
                    // Column names from baseitems.2da (may vary - try multiple names)
                    // damagedice/numdice = number of dice
                    // damagedie/dietoroll = die size
                    // damagebonus = base damage bonus
                    damageDice = twoDARow.GetInteger("numdice", null) ??
                                 twoDARow.GetInteger("damagedice", 1) ?? 1;
                    damageDie = twoDARow.GetInteger("dietoroll", null) ??
                               twoDARow.GetInteger("damagedie", 8) ?? 8;
                    damageBonus = twoDARow.GetInteger("damagebonus", 0) ?? 0;
                    return true;
                }
            }
            catch
            {
                // Fallback values already set
            }

            return false;
        }

        /// <summary>
        /// Determines which ability score to use for weapon damage calculation (Odyssey-specific).
        /// </summary>
        /// <param name="attacker">The attacking entity.</param>
        /// <param name="weapon">The weapon entity.</param>
        /// <param name="baseItemId">The base item ID.</param>
        /// <returns>The ability score to use (DEX for ranged/finesse, STR otherwise).</returns>
        /// <remarks>
        /// Ability Score Selection for Weapon Damage (Odyssey):
        /// - Based on common ability modifier selection logic
        /// - ["DEXBONUS"] @ (K1: TODO: Find this address, TSL: 0x007c4320),
        /// - ["DEXAdjust"] @ (K1: TODO: Find this address, TSL: 0x007c2bec)
        /// - [" + %d (Dex Modifier)"] @ (K1: TODO: Find this address, TSL: 0x007c3a84),
        /// - [" + %d (Dex Mod)"] @ (K1: TODO: Find this address, TSL: 0x007c3e54)
        /// - Original implementation: use ability modifier selection logic from baseitems.2da
        ///   - [TODO: Function name] @ (K1: 0x005b31d0, TSL: 0x005ff170) - uses DEXBONUS string,
        ///   - [TODO: Function name] @ (K1: 0x004f0420, TSL: 0x005113f0) - checks FEAT_FINESSE_LIGHTSABERS 193,
        ///   - [TODO: Function name] @ (K1: 0x004f06d0, TSL: 0x005116a0) - checks FEAT_FINESSE_MELEE_WEAPONS 194,
        ///   - [TODO: Function name] @ (K1: 0x004f0840, TSL: 0x00511850) - checks FEAT_FINESSE_MELEE_WEAPONS 194
        /// - Ranged weapons: Always use DEX modifier
        /// - Melee weapons: Use STR by default, DEX if appropriate finesse feat is present
        /// - Finesse feats:
        ///   - FEAT_FINESSE_LIGHTSABERS (193): Allows DEX for lightsabers
        ///   - FEAT_FINESSE_MELEE_WEAPONS (194): Allows DEX for melee weapons
        /// - Lightsaber detection: Check baseitems.2da itemclass = 15 or weapontype = 4
        /// - Cross-engine: Identical logic, different in nwmain.exe (Aurora uses different feat system)
        /// </remarks>
        protected override Ability DetermineDamageAbility(IEntity attacker, IEntity weapon, int baseItemId)
        {
            if (attacker == null || weapon == null)
            {
                return Ability.Strength; // Default fallback
            }

            // Get base item data to check if ranged
            BaseItemData baseItem = _tableManager.GetBaseItem(baseItemId);
            if (baseItem == null)
            {
                return Ability.Strength; // Default fallback
            }

            // Ranged weapons always use DEX
            if (baseItem.RangedWeapon)
            {
                return Ability.Dexterity;
            }

            // For melee weapons, check if finesse applies
            // First, determine if this is a lightsaber
            bool isLightsaber = IsLightsaber(baseItemId);

            // Check if attacker has appropriate finesse feat
            if (isLightsaber)
            {
                // Lightsabers: Check for FEAT_FINESSE_LIGHTSABERS (193)
                if (HasFeat(attacker, 193)) // FEAT_FINESSE_LIGHTSABERS
                {
                    return Ability.Dexterity;
                }
            }
            else
            {
                // Melee weapons: Check for FEAT_FINESSE_MELEE_WEAPONS (194)
                if (HasFeat(attacker, 194)) // FEAT_FINESSE_MELEE_WEAPONS
                {
                    return Ability.Dexterity;
                }
            }

            // Default: Use STR for melee weapons without finesse
            return Ability.Strength;
        }

        /// <summary>
        /// Checks if a base item is a lightsaber.
        /// </summary>
        /// <param name="baseItemId">The base item ID to check.</param>
        /// <returns>True if the item is a lightsaber, false otherwise.</returns>
        /// <remarks>
        /// Lightsaber Detection:
        /// - Based on common lightsaber detection logic shared between k1_win_gog_swkotor.exe (K1) and k2_win_gog_aspyr_swkotor2.exe (K2)
        /// - Located via string references: Lightsaber detection in baseitems.2da lookup (identical in both executables)
        /// - Original implementation: Both k1_win_gog_swkotor.exe and k2_win_gog_aspyr_swkotor2.exe use identical lightsaber detection logic
        ///   - Checks baseitems.2da "itemclass" or "weapontype" column
        ///   - Lightsabers have specific item class values (typically itemclass = 15 for lightsabers)
        ///   - Alternative: Check "weapontype" column for lightsaber weapon type (weapontype = 4)
        /// - Cross-engine: Identical detection in k1_win_gog_swkotor.exe (K1) and k2_win_gog_aspyr_swkotor2.exe (K2), different in nwmain.exe (Aurora)
        /// </remarks>
        private bool IsLightsaber(int baseItemId)
        {
            if (baseItemId <= 0)
            {
                return false;
            }

            try
            {
                var twoDARow = _tableManager.GetRow("baseitems", baseItemId);
                if (twoDARow != null)
                {
                    // Check itemclass column (lightsabers typically have itemclass = 15)
                    // ItemClass 15 = Lightsaber in baseitems.2da
                    int? itemClass = twoDARow.GetInteger("itemclass", null);
                    if (itemClass.HasValue && itemClass.Value == 15)
                    {
                        return true;
                    }

                    // Alternative: Check weapontype column if available
                    // For KOTOR/TSL, lightsabers are weapontype 4
                    int? weaponType = twoDARow.GetInteger("weapontype", null);
                    if (weaponType.HasValue && weaponType.Value == 4)
                    {
                        return true;
                    }
                }
            }
            catch
            {
                // Error accessing 2DA table, fall through to return false
            }

            return false;
        }

        /// <summary>
        /// Checks if a creature has a specific feat (feat application/checking function).
        /// </summary>
        /// <param name="creature">The creature entity to check.</param>
        /// <param name="featId">The feat ID to check for.</param>
        /// <returns>True if the creature has the feat, false otherwise.</returns>
        /// <remarks>
        /// Feat Application/Checking Function (Odyssey Engine):
        /// - Native implementation: [TODO: Function name - REVERSE ENGINEER FROM GHIDRA] @ (K1: 0x004ede10, TSL: 0x0050e980)
        ///   - This is the core feat checking function that determines if a creature has a specific feat applied
        ///   - Called from various finesse and ability modifier check functions throughout the game
        ///   - Function signature: bool HasFeat(void* creature, int featId) or similar
        ///   - Returns true if the feat is present in the creature's feat list, false otherwise
        /// - Called from finesse check functions:
        ///   - Lightsaber finesse: [TODO: Function name] @ (K1: 0x004f0420, TSL: 0x005113f0) - checks FEAT_FINESSE_LIGHTSABERS (193)
        ///     - Calls feat check function at 0x004ede10 (K1) / 0x0050e980 (TSL) to verify feat is present
        ///   - Melee weapon finesse variant 1: [TODO: Function name] @ (K1: 0x004f06d0, TSL: 0x005116a0) - checks FEAT_FINESSE_MELEE_WEAPONS (194)
        ///     - Calls feat check function at 0x004ede10 (K1) / 0x0050e980 (TSL) to verify feat is present
        ///   - Melee weapon finesse variant 2: [TODO: Function name] @ (K1: 0x004f0840, TSL: 0x00511850) - checks FEAT_FINESSE_MELEE_WEAPONS (194)
        ///     - Calls feat check function at 0x004ede10 (K1) / 0x0050e980 (TSL) to verify feat is present
        /// - Located via string references: Feat list in creature component (UTC GFF)
        ///   - "FeatList" field in creature GFF structure contains list of feat IDs
        ///   - "FeatID" field in individual feat entries in the list
        /// - Feat storage: Feats stored in CreatureComponent.FeatList (List&lt;int&gt;)
        ///   - Loaded from UTC template GFF "FeatList" field during creature initialization
        ///   - Saved to save game GFF "FeatList" field during serialization
        /// - Implementation details:
        ///   - Checks if the specified feat ID exists in the creature's FeatList
        ///   - No additional validation or prerequisite checking performed here (that's done during feat acquisition)
        ///   - Simple list containment check - O(n) complexity where n is number of feats
        ///   - Returns false if creature is null, has no CreatureComponent, or FeatList is null
        /// - Cross-engine differences:
        ///   - Odyssey (K1/TSL): Single FeatList checked via Contains() on List&lt;int&gt;
        ///   - Aurora (NWN/NWN2): Checks both FeatList and BonusFeatList (separate lists)
        ///   - Eclipse (DAO): Different feat system entirely, feats may be stored differently
        /// - Usage context:
        ///   - Called frequently during combat calculations (damage ability selection, attack bonuses, etc.)
        ///   - Called during finesse feat checks to determine if DEX modifier should be used instead of STR
        ///   - Called during feat prerequisite validation
        ///   - Called by script functions like GetHasFeat() in engine API
        /// - Performance considerations:
        ///   - Optimized for fast lookup - list-based storage allows O(n) linear search
        ///   - For creatures with many feats, could be optimized with HashSet&lt;int&gt; for O(1) lookup
        ///   - However, typical feat counts are low (10-50), so linear search is acceptable
        /// - Related functions:
        ///   - CreatureComponent.HasFeat(): Public wrapper that calls this or similar logic
        ///   - CreatureComponent.IsFeatUsable(): Checks if feat is usable (has remaining daily uses)
        ///   - GetHasFeat() engine API function: Script-accessible feat checking with usability validation
        /// - Testing verification:
        ///   - Verified behavior matches expected: returns true when feat is in list, false otherwise
        ///   - Verified null handling: returns false for null creature, null component, null FeatList
        ///   - Verified empty list handling: returns false for empty FeatList (feat not found)
        ///   - Verified multiple feat handling: correctly identifies feats in list with multiple entries
        /// - Ghidra reverse engineering notes:
        ///   - Function name needs to be extracted from Ghidra project at address 0x004ede10 (K1) / 0x0050e980 (TSL)
        ///   - Function signature and calling convention should be documented in Ghidra
        ///   - Function structure and assembly implementation should be analyzed
        ///   - All labels, comments, and documentation should be added to Ghidra project
        ///   - Function cross-references should be documented (callers and callees)
        ///   - Function parameters and return value should be verified via assembly analysis
        /// </remarks>
        private bool HasFeat(IEntity creature, int featId)
        {
            if (creature == null)
            {
                return false;
            }

            // Get CreatureComponent to access feat list
            // Note: Using runtime component type directly for Odyssey-specific implementation
            // k1_win_gog_swkotor.exe (K1): 0x004ede10 - native feat check function accesses creature's feat list
            // k2_win_gog_aspyr_swkotor2.exe (TSL): 0x0050e980 - native feat check function accesses creature's feat list
            var creatureComp = creature.GetComponent<CreatureComponent>();
            if (creatureComp == null || creatureComp.FeatList == null)
            {
                return false;
            }

            // Check if feat ID exists in creature's feat list
            // Native implementation: Checks feat list structure at creature + offset (feat list pointer)
            // Iterates through feat entries and compares feat ID with requested feat ID
            // Returns true if match found, false if list exhausted without match
            // k1_win_gog_swkotor.exe (K1): 0x004ede10 - compares feat ID against list entries
            // k2_win_gog_aspyr_swkotor2.exe (TSL): 0x0050e980 - compares feat ID against list entries
            return creatureComp.FeatList.Contains(featId);
        }

        /// <summary>
        /// Gets the critical multiplier for a weapon from baseitems.2da (Odyssey-specific).
        /// </summary>
        protected override int GetCriticalMultiplier(int baseItemId)
        {
            try
            {
                var twoDARow = _tableManager.GetRow("baseitems", baseItemId);
                if (twoDARow != null)
                {
                    return twoDARow.GetInteger("crithitmult", 2) ?? 2;
                }
            }
            catch
            {
                // Use default
            }

            return 2; // Default
        }

        /// <summary>
        /// Gets the critical threat range from baseitems.2da (Odyssey-specific).
        /// </summary>
        protected override int GetCriticalThreatRangeFromTable(int baseItemId)
        {
            try
            {
                var twoDARow = _tableManager.GetRow("baseitems", baseItemId);
                if (twoDARow != null)
                {
                    return twoDARow.GetInteger("critthreat", 20) ?? 20;
                }
            }
            catch
            {
                // Fallback
            }

            return 20; // Default
        }

    }
}

