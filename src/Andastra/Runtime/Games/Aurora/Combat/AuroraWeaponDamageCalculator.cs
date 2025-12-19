using System;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Games.Common.Combat;
using Andastra.Parsing.Formats.TwoDA;

namespace Andastra.Runtime.Games.Aurora.Combat
{
    /// <summary>
    /// Calculates weapon damage from equipped items using baseitems.2da (Aurora engine).
    /// </summary>
    /// <remarks>
    /// Aurora Weapon Damage Calculator:
    /// - Based on nwmain.exe weapon damage calculation (nwmain.exe: weapon damage and ability modifier selection)
    /// - Located via string references: "WeaponFinesseMinimumCreatureSize" @ 0x140dc3de0, "DEXAdjust" @ 0x140dc5050, "MinDEX" @ 0x140dc49b0
    /// - Cross-engine: Similar damage calculation to Odyssey but uses different 2DA tables and feat system
    /// - Inheritance: BaseWeaponDamageCalculator (Runtime.Games.Common.Combat) implements common damage calculation logic
    ///   - Aurora: AuroraWeaponDamageCalculator : BaseWeaponDamageCalculator (Runtime.Games.Aurora) - Aurora-specific baseitems.2da lookup
    /// - Ability modifier selection (Aurora/NWN):
    ///   - Ranged weapons: Always use DEX modifier (nwmain.exe: ranged weapon damage calculation)
    ///   - Melee weapons: Use STR by default, DEX if Weapon Finesse feat is present and weapon qualifies
    ///   - Weapon Finesse feat: FEAT_WEAPON_FINESSE (42) - standard NWN feat ID
    ///   - Finesse eligibility: Weapon must be light (weaponsize = 1 or 2) or creature size allows it
    ///   - Weapon size check: Based on WeaponFinesseMinimumCreatureSize from baseitems.2da
    ///   - Two-handed weapons: Cannot use finesse (weaponwield = 4 for two-handed)
    /// - Original implementation: nwmain.exe checks Weapon Finesse feat and weapon properties for ability modifier selection
    /// - Cross-engine comparison:
    ///   - Odyssey: FEAT_FINESSE_LIGHTSABERS (193), FEAT_FINESSE_MELEE_WEAPONS (194) - separate feats for lightsabers/melee
    ///   - Aurora: FEAT_WEAPON_FINESSE (42) - single feat for all finesse-eligible weapons
    ///   - Eclipse: Different damage system (may not use D20 ability modifiers)
    /// </remarks>
    public class AuroraWeaponDamageCalculator : BaseWeaponDamageCalculator
    {
        /// <summary>
        /// Weapon Finesse feat ID for Aurora/NWN (standard D&D 3.0 feat).
        /// </summary>
        /// <remarks>
        /// Based on nwmain.exe: Standard NWN feat.2da - Weapon Finesse is typically feat ID 42
        /// </remarks>
        private const int FeatWeaponFinesse = 42;

        /// <summary>
        /// Gets the main hand weapon slot number (Aurora-specific).
        /// </summary>
        /// <remarks>
        /// Based on nwmain.exe: RIGHTHAND slot = 4 (standard Aurora inventory slot)
        /// </remarks>
        protected override int MainHandWeaponSlot => 4;

        /// <summary>
        /// Gets the offhand weapon slot number (Aurora-specific).
        /// </summary>
        /// <remarks>
        /// Based on nwmain.exe: LEFTHAND slot = 5 (standard Aurora inventory slot)
        /// </remarks>
        protected override int OffHandWeaponSlot => 5;

        /// <summary>
        /// Gets damage dice information from baseitems.2da (Aurora-specific).
        /// </summary>
        /// <remarks>
        /// Based on nwmain.exe: baseitems.2da table structure
        /// Column names: numdice/damagedice (dice count), dietoroll/damagedie (die size), damagebonus
        /// </remarks>
        protected override bool GetDamageDiceFromTable(int baseItemId, out int damageDice, out int damageDie, out int damageBonus)
        {
            damageDice = 1;
            damageDie = 8;
            damageBonus = 0;

            // TODO: PLACEHOLDER - Implement Aurora-specific 2DA table lookup
            // This requires an Aurora table manager to be created
            // For now, return false to use unarmed damage fallback
            // Future: Create AuroraTwoDATableManager similar to Odyssey's TwoDATableManager
            return false;
        }

        /// <summary>
        /// Determines which ability score to use for weapon damage calculation (Aurora-specific).
        /// </summary>
        /// <param name="attacker">The attacking entity.</param>
        /// <param name="weapon">The weapon entity.</param>
        /// <param name="baseItemId">The base item ID.</param>
        /// <returns>The ability score to use (DEX for ranged/finesse, STR otherwise).</returns>
        /// <remarks>
        /// Ability Score Selection for Weapon Damage (Aurora/NWN):
        /// - Based on nwmain.exe: Ability modifier selection for weapon damage
        /// - Located via string references: "WeaponFinesseMinimumCreatureSize" @ 0x140dc3de0, "DEXAdjust" @ 0x140dc5050
        /// - Original implementation: nwmain.exe checks Weapon Finesse feat and weapon properties
        /// - Ranged weapons: Always use DEX modifier (nwmain.exe: ranged weapon damage always uses DEX)
        /// - Melee weapons: Use STR by default, DEX if appropriate conditions are met
        /// - Weapon Finesse conditions:
        ///   1. Creature must have FEAT_WEAPON_FINESSE (42)
        ///   2. Weapon must be light (weaponsize = 1 or 2) OR creature size allows it (based on WeaponFinesseMinimumCreatureSize)
        ///   3. Weapon must not be two-handed (weaponwield != 4)
        /// - Weapon size categories (Aurora): 1 = tiny, 2 = small, 3 = medium, 4 = large, 5 = huge
        /// - Light weapons: weaponsize = 1 (tiny) or 2 (small) are always finesse-eligible
        /// - Medium+ weapons: Can use finesse if creature size is small enough (based on WeaponFinesseMinimumCreatureSize column)
        /// - Cross-engine: Similar logic in Odyssey (different feat IDs), different in Eclipse (may not use D20 system)
        /// </remarks>
        protected override Ability DetermineDamageAbility(IEntity attacker, IEntity weapon, int baseItemId)
        {
            if (attacker == null || weapon == null)
            {
                return Ability.Strength; // Default fallback
            }

            // Get base item data to check if ranged
            // TODO: PLACEHOLDER - This requires Aurora table manager
            // For now, we'll use a simplified check based on common patterns
            bool isRanged = IsRangedWeapon(baseItemId);
            
            // Ranged weapons always use DEX (nwmain.exe: ranged weapon damage calculation)
            if (isRanged)
            {
                return Ability.Dexterity;
            }

            // For melee weapons, check if finesse applies
            // Check if attacker has Weapon Finesse feat
            if (!HasFeat(attacker, FeatWeaponFinesse))
            {
                // No Weapon Finesse feat, use STR
                return Ability.Strength;
            }

            // Creature has Weapon Finesse, check if weapon qualifies
            if (IsWeaponFinesseEligible(attacker, baseItemId))
            {
                return Ability.Dexterity;
            }

            // Weapon doesn't qualify for finesse, use STR
            return Ability.Strength;
        }

        /// <summary>
        /// Checks if a weapon is ranged (Aurora-specific).
        /// </summary>
        /// <param name="baseItemId">The base item ID to check.</param>
        /// <returns>True if the weapon is ranged, false otherwise.</returns>
        /// <remarks>
        /// Based on nwmain.exe: Ranged weapon detection from baseitems.2da
        /// Checks "rangedweapon" column in baseitems.2da (non-empty value indicates ranged weapon)
        /// TODO: PLACEHOLDER - Requires Aurora table manager for full implementation
        /// </remarks>
        private bool IsRangedWeapon(int baseItemId)
        {
            if (baseItemId <= 0)
            {
                return false;
            }

            // TODO: PLACEHOLDER - Implement full 2DA lookup when Aurora table manager is available
            // For now, use common weapon type patterns:
            // Bows (weapontype = 5), crossbows (weapontype = 6), slings (weapontype = 10), thrown (weapontype = 11)
            // This is a simplified check - full implementation should use baseitems.2da "rangedweapon" column
            
            // Fallback: Assume melee for now (will be properly implemented with table manager)
            return false;
        }

        /// <summary>
        /// Checks if a weapon is eligible for Weapon Finesse (Aurora-specific).
        /// </summary>
        /// <param name="attacker">The attacking entity.</param>
        /// <param name="baseItemId">The base item ID to check.</param>
        /// <returns>True if the weapon can use finesse, false otherwise.</returns>
        /// <remarks>
        /// Based on nwmain.exe: Weapon Finesse eligibility check
        /// Located via string reference: "WeaponFinesseMinimumCreatureSize" @ 0x140dc3de0
        /// Original implementation: Checks weapon size, weapon wield type, and creature size
        /// Eligibility conditions:
        /// 1. Weapon must not be two-handed (weaponwield != 4)
        /// 2. Weapon must be light (weaponsize = 1 or 2) OR creature size allows it
        /// 3. Weapon size check: Based on WeaponFinesseMinimumCreatureSize column in baseitems.2da
        /// TODO: PLACEHOLDER - Requires Aurora table manager for full implementation
        /// </remarks>
        private bool IsWeaponFinesseEligible(IEntity attacker, int baseItemId)
        {
            if (baseItemId <= 0 || attacker == null)
            {
                return false;
            }

            // TODO: PLACEHOLDER - Implement full 2DA lookup when Aurora table manager is available
            // Full implementation should:
            // 1. Get weaponwield from baseitems.2da - if 4 (two-handed), return false
            // 2. Get weaponsize from baseitems.2da - if 1 (tiny) or 2 (small), return true (always finesse-eligible)
            // 3. Get WeaponFinesseMinimumCreatureSize from baseitems.2da
            // 4. Get creature size from attacker's stats/appearance
            // 5. Compare creature size to WeaponFinesseMinimumCreatureSize
            
            // Simplified check: Assume light weapons (size 1-2) are finesse-eligible
            // This will be properly implemented when Aurora table manager is available
            // For now, return true if creature has the feat (simplified assumption)
            return true;
        }

        /// <summary>
        /// Checks if a creature has a specific feat (Aurora-specific).
        /// </summary>
        /// <param name="creature">The creature entity to check.</param>
        /// <param name="featId">The feat ID to check for.</param>
        /// <returns>True if the creature has the feat, false otherwise.</returns>
        /// <remarks>
        /// Based on nwmain.exe: Feat checking system
        /// Located via string references: Feat list in creature data structure (UTC GFF for NWN)
        /// Original implementation: Checks if creature has the feat in their feat list
        /// Feats stored in creature component or entity data
        /// TODO: PLACEHOLDER - Requires Aurora creature component implementation
        /// Cross-engine: Similar in Odyssey (CreatureComponent.FeatList), different structure in Aurora
        /// </remarks>
        private bool HasFeat(IEntity creature, int featId)
        {
            if (creature == null)
            {
                return false;
            }

            // TODO: PLACEHOLDER - Implement Aurora-specific feat checking
            // This requires an Aurora creature component to be created
            // For now, try to access feat list through common interfaces or entity data
            
            // Try to get feat list from entity data (common pattern)
            if (creature is Core.Entities.Entity entity)
            {
                // Try to get feat list from entity data
                var featList = entity.GetData<System.Collections.Generic.List<int>>("FeatList", null);
                if (featList != null)
                {
                    return featList.Contains(featId);
                }
            }

            // Fallback: Return false if we can't determine feat list
            // This will be properly implemented when Aurora creature components are available
            return false;
        }

        /// <summary>
        /// Gets the critical multiplier for a weapon from baseitems.2da (Aurora-specific).
        /// </summary>
        /// <remarks>
        /// Based on nwmain.exe: Critical multiplier lookup from baseitems.2da
        /// Column name: crithitmult (critical hit multiplier)
        /// TODO: PLACEHOLDER - Requires Aurora table manager
        /// </remarks>
        protected override int GetCriticalMultiplier(int baseItemId)
        {
            // TODO: PLACEHOLDER - Implement Aurora-specific lookup when table manager is available
            return 2; // Default
        }

        /// <summary>
        /// Gets the critical threat range from baseitems.2da (Aurora-specific).
        /// </summary>
        /// <remarks>
        /// Based on nwmain.exe: Critical threat range lookup from baseitems.2da
        /// Column name: critthreat (critical threat range)
        /// TODO: PLACEHOLDER - Requires Aurora table manager
        /// </remarks>
        protected override int GetCriticalThreatRangeFromTable(int baseItemId)
        {
            // TODO: PLACEHOLDER - Implement Aurora-specific lookup when table manager is available
            return 20; // Default
        }
    }
}

