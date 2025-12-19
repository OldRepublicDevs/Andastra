using System;
using System.Collections.Generic;
using System.Linq;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Parsing;
using Andastra.Parsing.Installation;

namespace Andastra.Runtime.Engines.Odyssey.UI
{
    /// <summary>
    /// Upgrade screen implementation for KOTOR 1 (swkotor.exe).
    /// </summary>
    /// <remarks>
    /// K1 Upgrade Screen Implementation:
    /// - Based on swkotor.exe: FUN_006c7630 @ 0x006c7630 (constructor loads "upgradeitems")
    /// - Based on swkotor.exe: FUN_006c6b60 @ 0x006c6b60 (constructor loads "upcrystals" @ 0x006c6e20)
    /// - Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 (upgrade button click handler)
    /// - Based on swkotor.exe: FUN_006c59a0 @ 0x006c59a0 (ApplyUpgrade implementation)
    /// - Located via string references: "upgradeitems" @ 0x00757438, "upcrystals" @ 0x0075741c
    /// - Uses "upgradeitems" for regular items (not "upgradeitems_p" like K2)
    /// - Uses "upcrystals" for lightsabers (same as K2)
    /// - Has 4 upgrade slots for lightsabers (K2 has 6)
    /// - Inventory checking: FUN_00555ed0 @ 0x00555ed0
    /// - Stack count check: param_1[0xa3] - item stack size
    /// - Upgrade storage offset: 0x2f74
    /// - Upgrade list offset: 0x2f5c
    /// </remarks>
    public class K1UpgradeScreen : OdysseyUpgradeScreenBase
    {
        /// <summary>
        /// Initializes a new instance of the K1 upgrade screen.
        /// </summary>
        /// <param name="installation">Game installation for accessing 2DA files.</param>
        /// <param name="world">World context for entity access.</param>
        public K1UpgradeScreen(Installation installation, IWorld world)
            : base(installation, world)
        {
        }

        /// <summary>
        /// Gets the upgrade table name for regular items (not lightsabers).
        /// </summary>
        /// <returns>Table name for regular item upgrades.</returns>
        /// <remarks>
        /// K1 uses "upgradeitems" (not "upgradeitems_p" like K2).
        /// Based on swkotor.exe: FUN_006c7630 @ 0x006c7630 line 37 - loads "upgradeitems"
        /// </remarks>
        protected override string GetRegularUpgradeTableName()
        {
            return "upgradeitems";
        }

        /// <summary>
        /// Applies an upgrade to an item.
        /// </summary>
        /// <param name="item">Item to upgrade.</param>
        /// <param name="upgradeSlot">Upgrade slot index (0-based).</param>
        /// <param name="upgradeResRef">ResRef of upgrade item to apply.</param>
        /// <returns>True if upgrade was successful.</returns>
        /// <remarks>
        /// Apply Upgrade Logic (K1):
        /// - Based on swkotor.exe: FUN_006c59a0 @ 0x006c59a0 (ApplyUpgrade implementation)
        /// - Called from: FUN_006c6500 @ 0x006c6500 line 163
        /// - Original implementation:
        ///   1. Checks if upgrade item is already in upgrade list (offset 0x2f5c)
        ///   2. If found in list, removes from list and uses that item
        ///   3. If stack count < 2, removes from inventory (FUN_00555fd0 @ 0x00555fd0)
        ///   4. If stack count >= 2, decrements stack (FUN_0055f280 @ 0x0055f280)
        ///   5. Adds upgrade to slot array (offset 0x2f74)
        ///   6. Applies upgrade properties to item
        /// - Stack count check: param_1[0xa3] - item stack size
        /// - Character from: DAT_007a39fc
        /// </remarks>
        public override bool ApplyUpgrade(IEntity item, int upgradeSlot, string upgradeResRef)
        {
            if (item == null || string.IsNullOrEmpty(upgradeResRef))
            {
                return false;
            }

            if (upgradeSlot < 0)
            {
                return false;
            }

            IItemComponent itemComponent = item.GetComponent<IItemComponent>();
            if (itemComponent == null)
            {
                return false;
            }

            // Check if slot is already occupied
            // Based on swkotor.exe: FUN_006c59a0 @ 0x006c59a0 line 12 - checks upgrade list at offset 0x2f5c
            var existingUpgrade = itemComponent.Upgrades.FirstOrDefault(u => u.Index == upgradeSlot);
            if (existingUpgrade != null)
            {
                // Slot is occupied, cannot apply upgrade
                return false;
            }

            // Check if upgrade is compatible with item
            List<string> availableUpgrades = GetAvailableUpgrades(item, upgradeSlot);
            if (!availableUpgrades.Contains(upgradeResRef, StringComparer.OrdinalIgnoreCase))
            {
                // Upgrade is not compatible or not available
                return false;
            }

            // Get character inventory to find and remove upgrade item
            // Based on swkotor.exe: FUN_006c59a0 @ 0x006c59a0 line 24 - gets character from DAT_007a39fc
            IEntity character = _character;
            if (character == null)
            {
                // TODO: Get player character from world
                return false;
            }

            IInventoryComponent characterInventory = character.GetComponent<IInventoryComponent>();
            if (characterInventory == null)
            {
                return false;
            }

            // Find upgrade item in inventory
            // Based on swkotor.exe: FUN_00555ed0 @ 0x00555ed0 - searches inventory by ResRef
            IEntity upgradeItem = null;
            foreach (IEntity inventoryItem in characterInventory.GetAllItems())
            {
                IItemComponent invItemComponent = inventoryItem.GetComponent<IItemComponent>();
                if (invItemComponent != null && !string.IsNullOrEmpty(invItemComponent.TemplateResRef))
                {
                    string resRef = invItemComponent.TemplateResRef.ToLowerInvariant();
                    if (resRef.EndsWith(".uti"))
                    {
                        resRef = resRef.Substring(0, resRef.Length - 4);
                    }
                    if (resRef.Equals(upgradeResRef, StringComparison.OrdinalIgnoreCase))
                    {
                        upgradeItem = inventoryItem;
                        break;
                    }
                }
            }

            if (upgradeItem == null)
            {
                // Upgrade item not found in inventory
                return false;
            }

            // Check stack count and remove from inventory
            // Based on swkotor.exe: FUN_006c59a0 @ 0x006c59a0 line 18 - checks stack count at offset 0xa3
            // TODO: Get stack count from item component
            // If stack count < 2, remove from inventory (FUN_00555fd0)
            // If stack count >= 2, decrement stack (FUN_0055f280)
            // For now, just remove from inventory
            characterInventory.RemoveItem(upgradeItem);

            // Apply upgrade to item
            // Based on swkotor.exe: FUN_006c59a0 @ 0x006c59a0 line 57 - stores upgrade at offset 0x2f74
            ItemUpgrade upgrade = new ItemUpgrade
            {
                UpgradeType = upgradeSlot, // UpgradeType corresponds to slot index
                Index = upgradeSlot
            };

            itemComponent.AddUpgrade(upgrade);

            // Apply upgrade properties to item
            // TODO: Load upgrade item UTI template and apply properties (damage bonuses, AC bonuses, etc.)
            // TODO: Recalculate item stats and update display

            return true;
        }

        /// <summary>
        /// Removes an upgrade from an item.
        /// </summary>
        /// <param name="item">Item to modify.</param>
        /// <param name="upgradeSlot">Upgrade slot index (0-based).</param>
        /// <returns>True if upgrade was removed.</returns>
        /// <remarks>
        /// Remove Upgrade Logic (K1):
        /// - Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 lines 165-180 (removal logic)
        /// - Original implementation:
        ///   1. Gets upgrade item from slot array (offset 0x2f74)
        ///   2. Removes upgrade from slot array (sets to 0)
        ///   3. Returns upgrade item to inventory (FUN_0055d330 @ 0x0055d330)
        ///   4. Updates item stats (removes upgrade bonuses)
        ///   5. Recalculates item stats
        /// - Removal: FUN_006857a0 @ 0x006857a0 - removes from array
        /// </remarks>
        public override bool RemoveUpgrade(IEntity item, int upgradeSlot)
        {
            if (item == null)
            {
                return false;
            }

            if (upgradeSlot < 0)
            {
                return false;
            }

            IItemComponent itemComponent = item.GetComponent<IItemComponent>();
            if (itemComponent == null)
            {
                return false;
            }

            // Find upgrade in slot
            // Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 line 169 - gets upgrade from offset 0x2f74
            var upgrade = itemComponent.Upgrades.FirstOrDefault(u => u.Index == upgradeSlot);
            if (upgrade == null)
            {
                // No upgrade in slot
                return false;
            }

            // Get upgrade item ResRef from upgrade data
            // Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 line 169 - gets item from slot array
            // TODO: Get upgrade item ResRef from upgrade data
            // For now, we'll need to track upgrade ResRefs separately

            // Remove upgrade from item
            // Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 line 176 - removes from array using FUN_006857a0
            itemComponent.RemoveUpgrade(upgrade);

            // Return upgrade item to inventory
            // Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 line 171 - returns to inventory using FUN_0055d330
            // TODO: Create upgrade item entity and add to player/party inventory
            // TODO: Get upgrade ResRef from upgrade data (need to track this)

            // Update item stats (remove upgrade bonuses)
            // TODO: Remove upgrade properties from item (damage bonuses, AC bonuses, etc.)
            // TODO: Recalculate item stats and update display

            return true;
        }
    }
}

