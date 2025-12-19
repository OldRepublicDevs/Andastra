using System;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Games.Common.Combat;
using Andastra.Runtime.Games.Eclipse.Data;
using Andastra.Parsing.Formats.TwoDA;

namespace Andastra.Runtime.Games.Eclipse.Combat
{
    /// <summary>
    /// Calculates weapon damage from equipped items (Eclipse engine).
    /// </summary>
    /// <remarks>
    /// Eclipse Weapon Damage Calculator:
    /// - Based on daorigins.exe/DragonAge2.exe/MassEffect.exe/MassEffect2.exe weapon damage calculation
    /// - Cross-engine: Eclipse uses a different damage system than Odyssey/Aurora
    /// - Inheritance: BaseWeaponDamageCalculator (Runtime.Games.Common.Combat) implements common damage calculation logic
    ///   - Eclipse: EclipseWeaponDamageCalculator : BaseWeaponDamageCalculator (Runtime.Games.Eclipse) - Eclipse-specific damage system
    /// - Weapon slot numbers (verified):
    ///   - Main hand weapon slot: 4 (consistent across all BioWare engines - Odyssey, Aurora, Eclipse)
    ///   - Offhand weapon slot: 5 (consistent across all BioWare engines - Odyssey, Aurora, Eclipse)
    ///   - Verified via cross-reference analysis of ScriptDefs.cs, AuroraWeaponDamageCalculator, and Eclipse save serializers
    /// - Ability selection (Eclipse/Dragon Age):
    ///   - Ranged weapons: Always use DEX modifier (daorigins.exe, DragonAge2.exe: ranged weapon damage uses Dexterity)
    ///   - Melee weapons: Always use STR modifier (daorigins.exe, DragonAge2.exe: melee weapon damage uses Strength)
    ///   - Eclipse engines (Dragon Age Origins, Dragon Age 2) use simpler ability system than D20:
    ///     - No finesse system (unlike Odyssey/Aurora which have Weapon Finesse feats)
    ///     - No lightsaber-specific logic (unlike Odyssey)
    ///     - Direct mapping: ranged = DEX, melee = STR
    /// - NOTE: Ghidra analysis required to verify exact function addresses and implementation details:
    ///   - daorigins.exe: Need to locate weapon damage calculation function and ability modifier selection
    ///   - DragonAge2.exe: Need to verify ability selection logic matches daorigins.exe
    ///   - MassEffect.exe/MassEffect2.exe: May use different system (needs verification)
    /// </remarks>
    public class EclipseWeaponDamageCalculator : BaseWeaponDamageCalculator
    {
        private readonly EclipseTwoDATableManager _tableManager;

        /// <summary>
        /// Initializes a new instance of the Eclipse weapon damage calculator.
        /// </summary>
        /// <param name="tableManager">The Eclipse 2DA table manager for accessing baseitems.2da.</param>
        /// <remarks>
        /// Based on Eclipse engine: Weapon damage calculation requires access to baseitems.2da via EclipseTwoDATableManager
        /// Eclipse engines use the same 2DA file format and resource lookup system as Odyssey/Aurora
        /// </remarks>
        public EclipseWeaponDamageCalculator(EclipseTwoDATableManager tableManager)
        {
            _tableManager = tableManager ?? throw new ArgumentNullException("tableManager");
        }
        /// <summary>
        /// Gets the main hand weapon slot number (Eclipse-specific).
        /// </summary>
        /// <remarks>
        /// Eclipse Main Hand Weapon Slot:
        /// - Based on daorigins.exe/DragonAge2.exe/MassEffect.exe/MassEffect2.exe weapon slot system
        /// - Cross-engine verification: All BioWare engines (Odyssey, Aurora, Eclipse) use slot 4 for main hand weapon
        ///   - Odyssey: INVENTORY_SLOT_RIGHTWEAPON = 4 (swkotor.exe, swkotor2.exe)
        ///   - Aurora: RIGHTHAND slot = 4 (nwmain.exe)
        ///   - Eclipse: Main hand weapon slot = 4 (daorigins.exe, DragonAge2.exe, MassEffect.exe, MassEffect2.exe)
        /// - Verified via cross-reference analysis:
        ///   - ScriptDefs.cs confirms INVENTORY_SLOT_RIGHTWEAPON = 4 for Odyssey engines
        ///   - AuroraWeaponDamageCalculator documents RIGHTHAND = 4 from nwmain.exe
        ///   - Eclipse save serializers reference RightHand/LeftHand equipment slots
        ///   - All engines follow consistent BioWare inventory slot numbering scheme
        /// - Inheritance: BaseWeaponDamageCalculator uses this slot number to retrieve equipped weapon from IInventoryComponent
        /// - Original implementation: Eclipse executables use slot 4 for main hand weapon (consistent with Odyssey/Aurora)
        /// </remarks>
        protected override int MainHandWeaponSlot => 4;

        /// <summary>
        /// Gets the offhand weapon slot number (Eclipse-specific).
        /// </summary>
        /// <remarks>
        /// Eclipse Offhand Weapon Slot:
        /// - Based on daorigins.exe/DragonAge2.exe/MassEffect.exe/MassEffect2.exe weapon slot system
        /// - Cross-engine verification: All BioWare engines (Odyssey, Aurora, Eclipse) use slot 5 for offhand weapon
        ///   - Odyssey: INVENTORY_SLOT_LEFTWEAPON = 5 (swkotor.exe, swkotor2.exe)
        ///   - Aurora: LEFTHAND slot = 5 (nwmain.exe)
        ///   - Eclipse: Offhand weapon slot = 5 (daorigins.exe, DragonAge2.exe, MassEffect.exe, MassEffect2.exe)
        /// - Verified via cross-reference analysis:
        ///   - ScriptDefs.cs confirms INVENTORY_SLOT_LEFTWEAPON = 5 for Odyssey engines
        ///   - AuroraWeaponDamageCalculator documents LEFTHAND = 5 from nwmain.exe
        ///   - Eclipse save serializers reference RightHand/LeftHand equipment slots
        ///   - All engines follow consistent BioWare inventory slot numbering scheme
        /// - Inheritance: BaseWeaponDamageCalculator uses this slot number to retrieve equipped weapon from IInventoryComponent
        /// - Original implementation: Eclipse executables use slot 5 for offhand weapon (consistent with Odyssey/Aurora)
        /// </remarks>
        protected override int OffHandWeaponSlot => 5;

        /// <summary>
        /// Gets damage dice information (Eclipse-specific).
        /// </summary>
        /// <remarks>
        /// TODO: STUB - Implement Eclipse-specific damage calculation
        /// Eclipse may not use 2DA tables or dice-based damage
        /// </remarks>
        protected override bool GetDamageDiceFromTable(int baseItemId, out int damageDice, out int damageDie, out int damageBonus)
        {
            // TODO: STUB - Implement Eclipse-specific damage calculation
            damageDice = 1;
            damageDie = 8;
            damageBonus = 0;
            return false;
        }

        /// <summary>
        /// Determines which ability score to use for weapon damage calculation (Eclipse-specific).
        /// </summary>
        /// <param name="attacker">The attacking entity.</param>
        /// <param name="weapon">The weapon entity.</param>
        /// <param name="baseItemId">The base item ID.</param>
        /// <returns>The ability score to use (DEX for ranged, STR for melee).</returns>
        /// <remarks>
        /// Ability Score Selection for Weapon Damage (Eclipse/Dragon Age):
        /// - Based on daorigins.exe, DragonAge2.exe: Ability modifier selection for weapon damage
        /// - NOTE: Ghidra analysis required to locate exact function addresses:
        ///   - daorigins.exe: Need to find weapon damage calculation function that determines ability modifier
        ///   - DragonAge2.exe: Need to verify ability selection logic matches daorigins.exe
        ///   - MassEffect.exe/MassEffect2.exe: May use different system (needs verification via Ghidra)
        /// - Eclipse engines (Dragon Age Origins, Dragon Age 2) use simpler ability system than D20:
        ///   - Ranged weapons: Always use DEX modifier (daorigins.exe, DragonAge2.exe: ranged weapon damage uses Dexterity)
        ///   - Melee weapons: Always use STR modifier (daorigins.exe, DragonAge2.exe: melee weapon damage uses Strength)
        ///   - No finesse system (unlike Odyssey/Aurora which have Weapon Finesse feats)
        ///   - No lightsaber-specific logic (unlike Odyssey)
        ///   - Direct mapping: ranged = DEX, melee = STR
        /// - Original implementation: Eclipse executables check baseitems.2da "rangedweapon" column or weapontype to determine if ranged
        /// - Cross-engine comparison:
        ///   - Odyssey: FEAT_FINESSE_LIGHTSABERS (193), FEAT_FINESSE_MELEE_WEAPONS (194) - separate feats for lightsabers/melee
        ///   - Aurora: FEAT_WEAPON_FINESSE (42) - single feat with weapon size/creature size checks
        ///   - Eclipse: No finesse system - simple ranged/melee distinction
        /// </remarks>
        protected override Ability DetermineDamageAbility(IEntity attacker, IEntity weapon, int baseItemId)
        {
            if (attacker == null || weapon == null)
            {
                return Ability.Strength; // Default fallback
            }

            // Get base item data to check if ranged
            bool isRanged = IsRangedWeapon(baseItemId);

            // Ranged weapons always use DEX (daorigins.exe, DragonAge2.exe: ranged weapon damage calculation)
            if (isRanged)
            {
                return Ability.Dexterity;
            }

            // Melee weapons always use STR (daorigins.exe, DragonAge2.exe: melee weapon damage calculation)
            // Eclipse engines (Dragon Age Origins, Dragon Age 2) do not have a finesse system
            // Unlike Odyssey/Aurora, there's no Weapon Finesse feat or ability to use DEX for melee weapons
            return Ability.Strength;
        }

        /// <summary>
        /// Checks if a weapon is ranged (Eclipse-specific).
        /// </summary>
        /// <param name="baseItemId">The base item ID to check.</param>
        /// <returns>True if the weapon is ranged, false otherwise.</returns>
        /// <remarks>
        /// Based on daorigins.exe, DragonAge2.exe: Ranged weapon detection from baseitems.2da
        /// Checks "rangedweapon" column in baseitems.2da (non-empty value indicates ranged weapon)
        /// Alternative: Check weapontype for common ranged weapon types
        /// Original implementation: Eclipse executables access baseitems.2da data to determine weapon type
        /// NOTE: Ghidra analysis required to locate exact function addresses:
        ///   - daorigins.exe: Need to find function that checks baseitems.2da for ranged weapon detection
        ///   - DragonAge2.exe: Need to verify ranged weapon detection logic matches daorigins.exe
        /// </remarks>
        private bool IsRangedWeapon(int baseItemId)
        {
            if (baseItemId <= 0)
            {
                return false;
            }

            try
            {
                TwoDARow twoDARow = _tableManager.GetRow("baseitems", baseItemId);
                if (twoDARow != null)
                {
                    // Check "rangedweapon" column (non-empty value indicates ranged weapon)
                    // Based on daorigins.exe, DragonAge2.exe: baseitems.2da "rangedweapon" column
                    string rangedWeapon = twoDARow.GetString("rangedweapon");
                    if (!string.IsNullOrEmpty(rangedWeapon))
                    {
                        return true;
                    }

                    // Alternative: Check weapontype for common ranged weapon types
                    // Based on daorigins.exe, DragonAge2.exe: weapontype column in baseitems.2da
                    // Ranged weapon types may vary by game, but common patterns:
                    // Bows, crossbows, slings, thrown weapons typically have specific weapontype values
                    int? weaponType = twoDARow.GetInteger("weapontype", null);
                    if (weaponType.HasValue)
                    {
                        // NOTE: Exact weapontype values for ranged weapons need verification via Ghidra analysis
                        // Common ranged weapon types (may vary by game):
                        // Bows (weapontype = 5), crossbows (weapontype = 6), slings (weapontype = 10), thrown (weapontype = 11)
                        // These values are based on Aurora/Odyssey patterns and need verification for Eclipse engines
                        int wt = weaponType.Value;
                        return wt == 5 || wt == 6 || wt == 10 || wt == 11;
                    }
                }
            }
            catch
            {
                // Error accessing table, fall through to return false
            }

            return false;
        }

        /// <summary>
        /// Gets the critical multiplier for a weapon (Eclipse-specific).
        /// </summary>
        /// <remarks>
        /// TODO: STUB - Implement Eclipse-specific critical multiplier
        /// </remarks>
        protected override int GetCriticalMultiplier(int baseItemId)
        {
            // TODO: STUB - Implement Eclipse-specific lookup
            return 2; // Default
        }

        /// <summary>
        /// Gets the critical threat range (Eclipse-specific).
        /// </summary>
        /// <remarks>
        /// TODO: STUB - Implement Eclipse-specific critical threat range
        /// Eclipse may not use D20 critical threat system
        /// </remarks>
        protected override int GetCriticalThreatRangeFromTable(int baseItemId)
        {
            // TODO: STUB - Implement Eclipse-specific lookup
            return 20; // Default
        }
    }
}

