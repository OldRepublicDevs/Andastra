using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Parsing;
using Andastra.Parsing.Installation;
using Andastra.Parsing.Resource;
using Andastra.Parsing.Formats.TwoDA;
using Andastra.Runtime.Core.Enums;

namespace Andastra.Runtime.Engines.Odyssey.UI
{
    /// <summary>
    /// Base class for upgrade screen implementation common to both K1 and K2.
    /// </summary>
    /// <remarks>
    /// Common Upgrade Screen Implementation:
    /// - Shared functionality between swkotor.exe (K1) and swkotor2.exe (K2)
    /// - Both use "upcrystals" for lightsabers
    /// - Both use "UpgradeType" and "Template" columns in upgrade tables
    /// - Both check inventory using similar logic
    /// - Differences: K2 uses "upgradeitems_p", K1 uses "upgradeitems" for regular items
    /// - Differences: K2 has 6 upgrade slots for lightsabers, K1 has 4
    /// </remarks>
    public abstract class OdysseyUpgradeScreenBase : IUpgradeScreen
    {
        protected readonly Installation _installation;
        protected readonly IWorld _world;
        protected IEntity _targetItem;
        protected IEntity _character;
        protected bool _disableItemCreation;
        protected bool _disableUpgrade;
        protected string _override2DA;
        protected bool _isVisible;
        
        // Track upgrade ResRefs by item+slot for removal
        // Key: item ObjectId + "_" + upgradeSlot, Value: upgrade ResRef
        protected readonly Dictionary<string, string> _upgradeResRefMap = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the upgrade screen.
        /// </summary>
        /// <param name="installation">Game installation for accessing 2DA files.</param>
        /// <param name="world">World context for entity access.</param>
        protected OdysseyUpgradeScreenBase(Installation installation, IWorld world)
        {
            if (installation == null)
            {
                throw new ArgumentNullException("installation");
            }
            if (world == null)
            {
                throw new ArgumentNullException("world");
            }

            _installation = installation;
            _world = world;
            _isVisible = false;
            _override2DA = string.Empty;
        }

        /// <summary>
        /// Gets or sets the item being upgraded (null for all items).
        /// </summary>
        public IEntity TargetItem
        {
            get { return _targetItem; }
            set { _targetItem = value; }
        }

        /// <summary>
        /// Gets or sets the character whose skills will be used (null for player).
        /// </summary>
        public IEntity Character
        {
            get { return _character; }
            set { _character = value; }
        }

        /// <summary>
        /// Gets or sets whether item creation is disabled.
        /// </summary>
        public bool DisableItemCreation
        {
            get { return _disableItemCreation; }
            set { _disableItemCreation = value; }
        }

        /// <summary>
        /// Gets or sets whether upgrading is disabled (forces item creation).
        /// </summary>
        public bool DisableUpgrade
        {
            get { return _disableUpgrade; }
            set { _disableUpgrade = value; }
        }

        /// <summary>
        /// Gets or sets the override 2DA file name (empty for default).
        /// </summary>
        public string Override2DA
        {
            get { return _override2DA; }
            set { _override2DA = value ?? string.Empty; }
        }

        /// <summary>
        /// Gets whether the upgrade screen is visible.
        /// </summary>
        public bool IsVisible
        {
            get { return _isVisible; }
        }

        /// <summary>
        /// Shows the upgrade screen.
        /// </summary>
        public void Show()
        {
            _isVisible = true;
            // TODO: STUB - UI rendering system not yet implemented
            // In full implementation, this would:
            // 1. Load upgrade screen GUI (upgradeitems_p.gui or similar)
            // 2. Display item slots and upgrade slots
            // 3. Show available upgrade items from inventory
            // 4. Handle user input for applying/removing upgrades
        }

        /// <summary>
        /// Hides the upgrade screen.
        /// </summary>
        public void Hide()
        {
            _isVisible = false;
            // TODO: STUB - UI rendering system not yet implemented
            // In full implementation, this would:
            // 1. Hide upgrade screen GUI
            // 2. Save any pending changes
            // 3. Return control to game
        }

        /// <summary>
        /// Gets the upgrade table name for regular items (not lightsabers).
        /// </summary>
        /// <returns>Table name for regular item upgrades.</returns>
        protected abstract string GetRegularUpgradeTableName();

        /// <summary>
        /// Gets available upgrade items for a given item and upgrade slot.
        /// </summary>
        /// <param name="item">Item to upgrade.</param>
        /// <param name="upgradeSlot">Upgrade slot index (0-based).</param>
        /// <returns>List of available upgrade items (ResRefs).</returns>
        public List<string> GetAvailableUpgrades(IEntity item, int upgradeSlot)
        {
            if (item == null)
            {
                return new List<string>();
            }

            IItemComponent itemComponent = item.GetComponent<IItemComponent>();
            if (itemComponent == null)
            {
                return new List<string>();
            }

            // Get base item type to determine upgrade compatibility
            int baseItemId = itemComponent.BaseItem;
            if (baseItemId <= 0)
            {
                return new List<string>();
            }

            if (upgradeSlot < 0)
            {
                return new List<string>();
            }

            // Determine which 2DA file to use for upgrades
            // Check if item is a lightsaber by looking up baseitems.2da
            string upgradeTableName = _override2DA;
            if (string.IsNullOrEmpty(upgradeTableName))
            {
                // Default: Use regular upgrade table for most items, upcrystals for lightsabers
                // Check baseitems.2da to determine if item is a lightsaber
                bool isLightsaber = IsLightsaberItem(baseItemId);
                upgradeTableName = isLightsaber ? "upcrystals" : GetRegularUpgradeTableName();
            }

            List<string> availableUpgrades = new List<string>();

            try
            {
                // Load upgrade table (2DA file)
                ResourceResult upgradeTableResult = _installation.Resource(upgradeTableName, ResourceType.TwoDA, null, null);
                if (upgradeTableResult == null || upgradeTableResult.Data == null)
                {
                    return availableUpgrades;
                }

                // Parse 2DA file to get upgrade items
                TwoDA upgradeTable = null;
                using (var stream = new MemoryStream(upgradeTableResult.Data))
                {
                    var reader = new TwoDABinaryReader(stream);
                    try
                    {
                        upgradeTable = reader.Load();
                    }
                    catch (Exception)
                    {
                        // Error loading upgrade table, return empty list
                        return availableUpgrades;
                    }
                }

                if (upgradeTable == null)
                {
                    return availableUpgrades;
                }

                // Get character inventory (use _character if set, otherwise try to get party leader/player)
                IEntity character = _character;
                if (character == null)
                {
                    // TODO: Get player character from world - needs world.GetPlayerCharacter() or similar
                    // For now, use _character if set
                }

                // Collect all inventory items from character
                // Based on swkotor2.exe: FUN_0055f2a0 @ 0x0055f2a0 - searches inventory by ResRef
                // Based on swkotor.exe: FUN_00555ed0 @ 0x00555ed0 - searches inventory by ResRef
                // These functions iterate through inventory items and compare ResRefs
                HashSet<string> inventoryResRefs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                if (character != null)
                {
                    IInventoryComponent characterInventory = character.GetComponent<IInventoryComponent>();
                    if (characterInventory != null)
                    {
                        foreach (IEntity inventoryItem in characterInventory.GetAllItems())
                        {
                            IItemComponent invItemComponent = inventoryItem.GetComponent<IItemComponent>();
                            if (invItemComponent != null && !string.IsNullOrEmpty(invItemComponent.TemplateResRef))
                            {
                                // Normalize ResRef (remove extension, lowercase)
                                // Based on FUN_00631140 (K2) / FUN_005e6080 (K1) - ResRef normalization
                                string resRef = invItemComponent.TemplateResRef.ToLowerInvariant();
                                if (resRef.EndsWith(".uti"))
                                {
                                    resRef = resRef.Substring(0, resRef.Length - 4);
                                }
                                inventoryResRefs.Add(resRef);
                            }
                        }
                    }
                }

                // Get column headers to check if required columns exist
                // Based on swkotor2.exe: FUN_0072e260 @ 0x0072e260 - uses "UpgradeType" and "Template" columns
                // Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 - uses "UpgradeType" and "Template" columns
                List<string> headers = upgradeTable.GetHeaders();
                bool hasUpgradeTypeColumn = headers.Contains("UpgradeType", StringComparer.OrdinalIgnoreCase);
                bool hasTemplateColumn = headers.Contains("Template", StringComparer.OrdinalIgnoreCase);

                if (!hasUpgradeTypeColumn || !hasTemplateColumn)
                {
                    // Required columns missing from upgrade table
                    return availableUpgrades;
                }

                // Iterate through upgrade table rows and filter by compatibility
                // Based on swkotor2.exe: FUN_0072e260 @ 0x0072e260 lines 100-128, 144-172, 271-299
                // Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 lines 88-116
                // Loop through all rows in upgrade table
                for (int rowIndex = 0; rowIndex < upgradeTable.GetHeight(); rowIndex++)
                {
                    TwoDARow row = upgradeTable.GetRow(rowIndex);

                    // Check UpgradeType compatibility (this is the slot index)
                    // Based on swkotor2.exe: FUN_0072e260 @ 0x0072e260 line 102 - checks "UpgradeType" column
                    // Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 line 90 - checks "UpgradeType" column
                    // UpgradeType column specifies which slot this upgrade can be placed in
                    // Can be:
                    // - Specific slot index (0, 1, 2, etc.) - matches upgradeSlot parameter
                    // - 0 (null/empty) - special case for lightsaber crystals (slot 1 in K2)
                    // - Empty/"****" (no match)
                    string upgradeTypeValue = null;
                    try
                    {
                        upgradeTypeValue = row.GetString("UpgradeType");
                    }
                    catch (KeyNotFoundException)
                    {
                        continue; // UpgradeType column missing or invalid
                    }

                    // Check if UpgradeType matches the requested slot
                    // Based on swkotor2.exe: FUN_0072e260 @ 0x0072e260 line 107, 151, 278 - compares UpgradeType with slot
                    // Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 line 95 - compares UpgradeType with slot
                    bool slotMatches = false;
                    if (string.IsNullOrWhiteSpace(upgradeTypeValue) || upgradeTypeValue == "****")
                    {
                        // Empty UpgradeType - special case: for lightsaber crystals (slot 1), UpgradeType can be 0/empty
                        // Based on swkotor2.exe: FUN_0072e260 @ 0x0072e260 line 107 - checks if UpgradeType == 0
                        if (upgradeSlot == 1)
                        {
                            // For slot 1 (lightsaber crystals), empty UpgradeType is valid
                            slotMatches = true;
                        }
                        else
                        {
                            continue; // Skip rows with invalid UpgradeType for other slots
                        }
                    }
                    else
                    {
                        // Try to parse as integer for exact match
                        int? rowUpgradeType = row.GetInteger("UpgradeType", null);
                        if (rowUpgradeType.HasValue)
                        {
                            // UpgradeType must match upgradeSlot parameter
                            if (rowUpgradeType.Value == upgradeSlot)
                            {
                                slotMatches = true;
                            }
                        }
                    }

                    if (!slotMatches)
                    {
                        continue; // UpgradeType doesn't match requested slot
                    }

                    // Get Template ResRef from upgrade table row
                    // Based on swkotor2.exe: FUN_0072e260 @ 0x0072e260 line 108, 152, 279 - uses "Template" column
                    // Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 line 96, 98 - uses "Template" column
                    // Template column contains the upgrade item template ResRef
                    string upgradeResRef = null;
                    try
                    {
                        upgradeResRef = row.GetString("Template");
                    }
                    catch (KeyNotFoundException)
                    {
                        continue; // Template column missing or invalid
                    }

                    if (string.IsNullOrWhiteSpace(upgradeResRef) || upgradeResRef == "****")
                    {
                        continue; // Skip rows with invalid Template
                    }

                    // Normalize ResRef (remove extension, lowercase)
                    // Based on swkotor2.exe: FUN_00631140 @ 0x00631140 - ResRef normalization
                    // Based on swkotor.exe: FUN_005e6080 @ 0x005e6080 - ResRef normalization
                    string normalizedResRef = upgradeResRef.ToLowerInvariant();
                    if (normalizedResRef.EndsWith(".uti"))
                    {
                        normalizedResRef = normalizedResRef.Substring(0, normalizedResRef.Length - 4);
                    }

                    // Check if upgrade item is in inventory
                    // Based on swkotor2.exe: FUN_0055f2a0 @ 0x0055f2a0 - searches inventory by ResRef, returns item ID or 0x7f000000
                    // Based on swkotor.exe: FUN_00555ed0 @ 0x00555ed0 - searches inventory by ResRef, returns item ID or 0x7f000000
                    // FUN_0055f2a0/FUN_00555ed0 iterate through inventory items and compare ResRefs using string comparison
                    if (inventoryResRefs.Contains(normalizedResRef))
                    {
                        // Upgrade is compatible and available in inventory
                        // Based on swkotor2.exe: FUN_0072e260 @ 0x0072e260 line 79, 118, 162, 289 - checks if item found in inventory
                        // Based on swkotor.exe: FUN_006c6500 @ 0x006c6500 line 66, 106 - checks if item found in inventory
                        if (!availableUpgrades.Contains(normalizedResRef, StringComparer.OrdinalIgnoreCase))
                        {
                            availableUpgrades.Add(normalizedResRef);
                        }
                    }
                }

                return availableUpgrades;
            }
            catch (Exception)
            {
                // Error loading or parsing upgrade table
                return availableUpgrades;
            }
        }

        /// <summary>
        /// Determines if an item is a lightsaber by checking baseitems.2da.
        /// </summary>
        /// <param name="baseItemId">Base item ID from baseitems.2da.</param>
        /// <returns>True if the item is a lightsaber, false otherwise.</returns>
        /// <remarks>
        /// Lightsaber Detection:
        /// - Common to both K1 and K2
        /// - Based on swkotor2.exe: Lightsaber item type detection
        /// - Original implementation: Checks baseitems.2da "itemclass" or "weapontype" column
        /// - Lightsabers have specific item class values (typically itemclass = 15 for lightsabers)
        /// - Alternative: Check "weapontype" column for lightsaber weapon type
        /// - Falls back to base item ID range check if 2DA lookup fails
        /// </remarks>
        protected bool IsLightsaberItem(int baseItemId)
        {
            if (baseItemId <= 0)
            {
                return false;
            }

            try
            {
                // Load baseitems.2da to check item class
                ResourceResult baseitemsResult = _installation.Resource("baseitems", ResourceType.TwoDA, null, null);
                if (baseitemsResult != null && baseitemsResult.Data != null)
                {
                    using (var stream = new MemoryStream(baseitemsResult.Data))
                    {
                        var reader = new TwoDABinaryReader(stream);
                        TwoDA baseitems = reader.Load();

                        if (baseitems != null && baseItemId >= 0 && baseItemId < baseitems.GetHeight())
                        {
                            TwoDARow row = baseitems.GetRow(baseItemId);

                            // Check itemclass column (lightsabers typically have itemclass = 15)
                            // ItemClass 15 = Lightsaber in baseitems.2da
                            int? itemClass = row.GetInteger("itemclass", null);
                            if (itemClass.HasValue && itemClass.Value == 15)
                            {
                                return true;
                            }

                            // Alternative: Check weapontype column if available
                            // Some implementations use weapontype to identify lightsabers
                            int? weaponType = row.GetInteger("weapontype", null);
                            if (weaponType.HasValue)
                            {
                                // Lightsaber weapon type is typically 4 or 5 (varies by game)
                                // For KOTOR/TSL, lightsabers are weapontype 4
                                if (weaponType.Value == 4)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Error loading baseitems.2da, fall through to ID range check
            }

            // Fallback: Check base item ID range
            // Lightsabers in KOTOR/TSL typically have base item IDs in specific ranges
            // This is a heuristic fallback if 2DA lookup fails
            // KOTOR: Lightsabers are base item IDs 1-3 (single, double, short)
            // TSL: Lightsabers are base item IDs 1-3 (single, double, short)
            // Note: This is approximate and may need adjustment based on actual game data
            if (baseItemId >= 1 && baseItemId <= 3)
            {
                // Could be a lightsaber, but not definitive without 2DA lookup
                // Return false to be safe (use upgradeitems.2da by default)
                return false;
            }

            return false;
        }

        /// <summary>
        /// Applies an upgrade to an item.
        /// </summary>
        /// <param name="item">Item to upgrade.</param>
        /// <param name="upgradeSlot">Upgrade slot index (0-based).</param>
        /// <param name="upgradeResRef">ResRef of upgrade item to apply.</param>
        /// <returns>True if upgrade was successful.</returns>
        public abstract bool ApplyUpgrade(IEntity item, int upgradeSlot, string upgradeResRef);

        /// <summary>
        /// Removes an upgrade from an item.
        /// </summary>
        /// <param name="item">Item to modify.</param>
        /// <param name="upgradeSlot">Upgrade slot index (0-based).</param>
        /// <returns>True if upgrade was removed.</returns>
        public abstract bool RemoveUpgrade(IEntity item, int upgradeSlot);
    }
}

