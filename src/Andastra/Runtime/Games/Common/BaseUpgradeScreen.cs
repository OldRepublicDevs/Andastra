using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Parsing;
using Andastra.Parsing.Installation;
using Andastra.Parsing.Resource;
using Andastra.Parsing.Resource.Generics.UTI;
using Andastra.Parsing.Formats.TwoDA;
using Andastra.Parsing.Formats.GFF;
using Andastra.Runtime.Core.Enums;

namespace Andastra.Runtime.Games.Common
{
    /// <summary>
    /// Base implementation of upgrade screen functionality shared across all BioWare engines.
    /// </summary>
    /// <remarks>
    /// Base Upgrade Screen Implementation:
    /// - Common upgrade screen properties and methods across all engines
    /// - Handles item upgrade UI and logic for modifying weapons and armor
    /// - Provides base for engine-specific upgrade screen implementations
    /// - Cross-engine analysis: All engines that support item upgrades share common patterns
    /// - Common functionality: Upgrade slot management, inventory checking, property application
    /// - Engine-specific: 2DA file names, upgrade slot counts, UI implementation details
    ///
    /// Based on reverse engineering of:
    /// - swkotor.exe: FUN_006c7630 (constructor), FUN_006c6500 (button handler), FUN_006c59a0 (ApplyUpgrade)
    /// - swkotor2.exe: FUN_00731a00 (constructor), FUN_0072e260 (button handler), FUN_00729640 (ApplyUpgrade)
    /// - daorigins.exe: ItemUpgrade, GUIItemUpgrade, COMMAND_OPENITEMUPGRADEGUI
    /// - DragonAge2.exe: ItemUpgrade, GUIItemUpgrade, UpgradePrereqType, GetAbilityUpgradedValue
    ///
    /// Common structure across engines:
    /// - TargetItem: Item being upgraded (null for all items)
    /// - Character: Character whose skills/inventory are used
    /// - DisableItemCreation/DisableUpgrade: UI mode flags
    /// - Override2DA: Custom 2DA file override
    /// - IsVisible: Screen visibility state
    /// - GetAvailableUpgrades: Returns compatible upgrades from inventory
    /// - ApplyUpgrade/RemoveUpgrade: Modifies item properties
    /// </remarks>
    [PublicAPI]
    public abstract class BaseUpgradeScreen : IUpgradeScreen
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
        protected BaseUpgradeScreen(Installation installation, IWorld world)
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
        public abstract void Show();

        /// <summary>
        /// Hides the upgrade screen.
        /// </summary>
        public abstract void Hide();

        /// <summary>
        /// Gets available upgrade items for a given item and upgrade slot.
        /// </summary>
        /// <param name="item">Item to upgrade.</param>
        /// <param name="upgradeSlot">Upgrade slot index (0-based).</param>
        /// <returns>List of available upgrade items (ResRefs).</returns>
        public abstract List<string> GetAvailableUpgrades(IEntity item, int upgradeSlot);

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

        /// <summary>
        /// Gets the upgrade table name for regular items (not lightsabers).
        /// </summary>
        /// <returns>Table name for regular item upgrades.</returns>
        protected abstract string GetRegularUpgradeTableName();

        /// <summary>
        /// Gets the upgrade table name for lightsabers.
        /// </summary>
        /// <returns>Table name for lightsaber upgrades.</returns>
        protected virtual string GetLightsaberUpgradeTableName()
        {
            // Default: Most engines use "upcrystals" for lightsabers
            return "upcrystals";
        }

        /// <summary>
        /// Determines if an item is a lightsaber by checking baseitems.2da.
        /// </summary>
        /// <param name="baseItemId">Base item ID from baseitems.2da.</param>
        /// <returns>True if the item is a lightsaber, false otherwise.</returns>
        protected virtual bool IsLightsaberItem(int baseItemId)
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
                            int? itemClass = row.GetInteger("itemclass", null);
                            if (itemClass.HasValue && itemClass.Value == 15)
                            {
                                return true;
                            }

                            // Alternative: Check weapontype column if available
                            int? weaponType = row.GetInteger("weapontype", null);
                            if (weaponType.HasValue && weaponType.Value == 4)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Error loading baseitems.2da, fall through
            }

            return false;
        }

        /// <summary>
        /// Loads an upgrade item UTI template from the installation.
        /// </summary>
        /// <param name="upgradeResRef">ResRef of the upgrade item template to load.</param>
        /// <returns>UTI template if loaded successfully, null otherwise.</returns>
        protected UTI LoadUpgradeUTITemplate(string upgradeResRef)
        {
            if (string.IsNullOrEmpty(upgradeResRef))
            {
                return null;
            }

            try
            {
                // Normalize ResRef (ensure .uti extension if needed)
                string normalizedResRef = upgradeResRef;
                if (!normalizedResRef.EndsWith(".uti", StringComparison.OrdinalIgnoreCase))
                {
                    normalizedResRef = normalizedResRef + ".uti";
                }

                // Load UTI resource from installation
                ResourceResult utiResult = _installation.Resource(normalizedResRef, ResourceType.UTI, null, null);
                if (utiResult == null || utiResult.Data == null || utiResult.Data.Length == 0)
                {
                    return null;
                }

                // Parse UTI GFF data
                using (var stream = new MemoryStream(utiResult.Data))
                {
                    var reader = new GFFBinaryReader(stream);
                    GFF gff = reader.Load();
                    if (gff == null)
                    {
                        return null;
                    }

                    // Construct UTI from GFF
                    UTI utiTemplate = UTIHelpers.ConstructUti(gff);
                    return utiTemplate;
                }
            }
            catch (Exception)
            {
                // Error loading or parsing UTI template
                return null;
            }
        }

        /// <summary>
        /// Applies properties from an upgrade UTI template to an item.
        /// </summary>
        /// <param name="item">Item to apply upgrade properties to.</param>
        /// <param name="upgradeUTI">UTI template of the upgrade item.</param>
        /// <returns>True if properties were applied successfully.</returns>
        protected bool ApplyUpgradeProperties(IEntity item, UTI upgradeUTI)
        {
            if (item == null || upgradeUTI == null)
            {
                return false;
            }

            IItemComponent itemComponent = item.GetComponent<IItemComponent>();
            if (itemComponent == null)
            {
                return false;
            }

            // Extract properties from upgrade UTI template and add to item
            foreach (var utiProp in upgradeUTI.Properties)
            {
                // Convert UTI property to ItemProperty
                var itemProperty = new ItemProperty
                {
                    PropertyType = utiProp.PropertyName,
                    Subtype = utiProp.Subtype,
                    CostTable = utiProp.CostTable,
                    CostValue = utiProp.CostValue,
                    Param1 = utiProp.Param1,
                    Param1Value = utiProp.Param1Value
                };

                // Add property to item
                itemComponent.AddProperty(itemProperty);
            }

            return true;
        }

        /// <summary>
        /// Removes properties from an item that match those in an upgrade UTI template.
        /// </summary>
        /// <param name="item">Item to remove upgrade properties from.</param>
        /// <param name="upgradeUTI">UTI template of the upgrade item to remove.</param>
        /// <returns>True if properties were removed successfully.</returns>
        protected bool RemoveUpgradeProperties(IEntity item, UTI upgradeUTI)
        {
            if (item == null || upgradeUTI == null)
            {
                return false;
            }

            IItemComponent itemComponent = item.GetComponent<IItemComponent>();
            if (itemComponent == null)
            {
                return false;
            }

            // Remove properties from item that match upgrade UTI template
            var propertiesToRemove = new List<ItemProperty>();
            foreach (var itemProp in itemComponent.Properties)
            {
                // Check if this property matches any property in the upgrade UTI
                foreach (var utiProp in upgradeUTI.Properties)
                {
                    if (itemProp.PropertyType == utiProp.PropertyName &&
                        itemProp.Subtype == utiProp.Subtype &&
                        itemProp.CostTable == utiProp.CostTable &&
                        itemProp.CostValue == utiProp.CostValue &&
                        itemProp.Param1 == utiProp.Param1 &&
                        itemProp.Param1Value == utiProp.Param1Value)
                    {
                        // Property matches upgrade property - mark for removal
                        propertiesToRemove.Add(itemProp);
                        break; // Only remove first matching property (in case of duplicates)
                    }
                }
            }

            // Remove matched properties from item
            foreach (var propToRemove in propertiesToRemove)
            {
                itemComponent.RemoveProperty(propToRemove);
            }

            return true;
        }

        /// <summary>
        /// Recalculates item stats after applying or removing upgrades.
        /// </summary>
        /// <remarks>
        /// Item Stat Recalculation:
        /// - Based on swkotor.exe, swkotor2.exe: Item stats are recalculated after upgrades are applied/removed
        /// - Original implementation: FUN_006c59a0 @ 0x006c59a0 (swkotor.exe), FUN_00729640 @ 0x00729640 (swkotor2.exe)
        /// - Calculates final stats by combining base item stats from baseitems.2da with property bonuses
        /// - Stores computed stats on entity for UI display and system queries
        /// - Stats affected: damage, attack bonus, AC, saving throws, skills, ability scores, critical hit bonuses
        /// </remarks>
        /// <param name="item">Item to recalculate stats for.</param>
        protected virtual void RecalculateItemStats(IEntity item)
        {
            if (item == null)
            {
                return;
            }

            IItemComponent itemComponent = item.GetComponent<IItemComponent>();
            if (itemComponent == null)
            {
                return;
            }

            // Get base item ID
            int baseItemId = itemComponent.BaseItem;
            if (baseItemId < 0)
            {
                return;
            }

            // Access game data provider for 2DA table lookups
            IGameDataProvider gameDataProvider = _world?.GameDataProvider;
            if (gameDataProvider == null)
            {
                return;
            }

            // 1. Get base item stats from baseitems.2da
            TwoDA baseitemsTable = gameDataProvider.GetTable("baseitems");
            if (baseitemsTable == null || baseItemId >= baseitemsTable.GetHeight())
            {
                return;
            }

            TwoDARow baseItemRow = baseitemsTable.GetRow(baseItemId);
            if (baseItemRow == null)
            {
                return;
            }

            // Extract base stats from baseitems.2da
            int baseDamageDice = (int)gameDataProvider.GetTableFloat("baseitems", baseItemId, "damagedice", 0.0f);
            int baseDamageDie = (int)gameDataProvider.GetTableFloat("baseitems", baseItemId, "damagedie", 0.0f);
            int baseDamageBonus = (int)gameDataProvider.GetTableFloat("baseitems", baseItemId, "damagebonus", 0.0f);
            int baseACValue = (int)gameDataProvider.GetTableFloat("baseitems", baseItemId, "ACValue", 0.0f);
            int baseAttackMod = (int)gameDataProvider.GetTableFloat("baseitems", baseItemId, "attackmod", 0.0f);
            int baseCritHitMult = (int)gameDataProvider.GetTableFloat("baseitems", baseItemId, "crithitmult", 1.0f);
            int baseCritThreat = (int)gameDataProvider.GetTableFloat("baseitems", baseItemId, "critthreat", 20.0f);

            // Initialize cumulative bonuses
            int totalDamageBonus = baseDamageBonus;
            int totalAttackBonus = baseAttackMod;
            int totalACBonus = baseACValue;
            int totalSaveBonus = 0;
            int totalSaveFortBonus = 0;
            int totalSaveRefBonus = 0;
            int totalSaveWillBonus = 0;
            var abilityBonuses = new Dictionary<Ability, int>();
            var skillBonuses = new Dictionary<int, int>();
            int critHitMultBonus = 0;
            int critThreatBonus = 0;

            // Initialize ability bonuses dictionary
            foreach (Ability ability in Enum.GetValues(typeof(Ability)))
            {
                abilityBonuses[ability] = 0;
            }

            // 2. Calculate property bonuses from all item properties
            TwoDA itempropDefTable = gameDataProvider.GetTable("itempropdef");
            foreach (ItemProperty property in itemComponent.Properties)
            {
                CalculatePropertyBonuses(property, itempropDefTable, ref totalDamageBonus, ref totalAttackBonus,
                    ref totalACBonus, ref totalSaveBonus, ref totalSaveFortBonus, ref totalSaveRefBonus,
                    ref totalSaveWillBonus, abilityBonuses, skillBonuses, ref critHitMultBonus, ref critThreatBonus);
            }

            // Calculate final stats
            int finalDamageDice = baseDamageDice;
            int finalDamageDie = baseDamageDie;
            int finalDamageBonus = totalDamageBonus;
            int finalAttackBonus = totalAttackBonus;
            int finalACBonus = totalACBonus;
            int finalSaveBonus = totalSaveBonus;
            int finalCritHitMult = baseCritHitMult + critHitMultBonus;
            int finalCritThreat = baseCritThreat + critThreatBonus;

            // 3. Store computed stats on entity for UI display and system queries
            // Base stats
            item.SetData("ItemBaseDamageDice", finalDamageDice);
            item.SetData("ItemBaseDamageDie", finalDamageDie);
            item.SetData("ItemTotalDamageBonus", finalDamageBonus);
            item.SetData("ItemTotalAttackBonus", finalAttackBonus);
            item.SetData("ItemTotalACBonus", finalACBonus);
            item.SetData("ItemTotalSaveBonus", finalSaveBonus);
            item.SetData("ItemTotalSaveFortBonus", totalSaveFortBonus);
            item.SetData("ItemTotalSaveRefBonus", totalSaveRefBonus);
            item.SetData("ItemTotalSaveWillBonus", totalSaveWillBonus);
            item.SetData("ItemCritHitMult", finalCritHitMult);
            item.SetData("ItemCritThreat", finalCritThreat);

            // Ability bonuses (store as dictionary)
            item.SetData("ItemAbilityBonuses", new Dictionary<Ability, int>(abilityBonuses));

            // Skill bonuses (store as dictionary)
            item.SetData("ItemSkillBonuses", new Dictionary<int, int>(skillBonuses));

            // Store computed flag to indicate stats have been calculated
            item.SetData("ItemStatsCalculated", true);
        }

        /// <summary>
        /// Calculates stat bonuses from a single item property.
        /// </summary>
        /// <remarks>
        /// Property Bonus Calculation:
        /// - Based on swkotor.exe, swkotor2.exe: Property types map to stat bonuses via itempropdef.2da
        /// - Uses same property type mappings as ActionUseItem.ConvertPropertyToEffectHardcoded
        /// - Extracts bonus amounts from CostValue or Param1Value
        /// - Accumulates bonuses into cumulative totals
        /// </remarks>
        /// <param name="property">Item property to calculate bonuses from.</param>
        /// <param name="itempropDefTable">itempropdef.2da table (may be null for fallback).</param>
        /// <param name="totalDamageBonus">Cumulative damage bonus (modified by reference).</param>
        /// <param name="totalAttackBonus">Cumulative attack bonus (modified by reference).</param>
        /// <param name="totalACBonus">Cumulative AC bonus (modified by reference).</param>
        /// <param name="totalSaveBonus">Cumulative general save bonus (modified by reference).</param>
        /// <param name="totalSaveFortBonus">Cumulative Fortitude save bonus (modified by reference).</param>
        /// <param name="totalSaveRefBonus">Cumulative Reflex save bonus (modified by reference).</param>
        /// <param name="totalSaveWillBonus">Cumulative Will save bonus (modified by reference).</param>
        /// <param name="abilityBonuses">Dictionary of ability bonuses (modified by reference).</param>
        /// <param name="skillBonuses">Dictionary of skill bonuses (modified by reference).</param>
        /// <param name="critHitMultBonus">Cumulative critical hit multiplier bonus (modified by reference).</param>
        /// <param name="critThreatBonus">Cumulative critical threat range bonus (modified by reference).</param>
        private void CalculatePropertyBonuses(ItemProperty property, TwoDA itempropDefTable,
            ref int totalDamageBonus, ref int totalAttackBonus, ref int totalACBonus,
            ref int totalSaveBonus, ref int totalSaveFortBonus, ref int totalSaveRefBonus, ref int totalSaveWillBonus,
            Dictionary<Ability, int> abilityBonuses, Dictionary<int, int> skillBonuses,
            ref int critHitMultBonus, ref int critThreatBonus)
        {
            if (property == null)
            {
                return;
            }

            int propType = property.PropertyType;
            int costValue = property.CostValue;
            int param1Value = property.Param1Value;
            int subtype = property.Subtype;

            // Get amount from CostValue or Param1Value (property bonus amount)
            int amount = costValue != 0 ? costValue : param1Value;

            // ITEM_PROPERTY_ABILITY_BONUS (0): Ability score bonus
            if (propType == 0)
            {
                if (subtype >= 0 && subtype <= 5 && amount > 0)
                {
                    Ability ability = (Ability)subtype;
                    if (abilityBonuses.ContainsKey(ability))
                    {
                        abilityBonuses[ability] += amount;
                    }
                    else
                    {
                        abilityBonuses[ability] = amount;
                    }
                }
            }
            // ITEM_PROPERTY_AC_BONUS (1): Armor Class bonus
            else if (propType == 1)
            {
                if (amount > 0)
                {
                    totalACBonus += amount;
                }
            }
            // ITEM_PROPERTY_ENHANCEMENT_BONUS (5): Enhancement bonus (attack/damage)
            else if (propType == 5)
            {
                if (amount > 0)
                {
                    // Enhancement bonus affects both attack and damage
                    totalAttackBonus += amount;
                    totalDamageBonus += amount;
                }
            }
            // ITEM_PROPERTY_ATTACK_BONUS (38): Attack bonus
            else if (propType == 38)
            {
                if (amount > 0)
                {
                    totalAttackBonus += amount;
                }
            }
            // ITEM_PROPERTY_DAMAGE_BONUS (11): Damage bonus
            else if (propType == 11)
            {
                if (amount > 0)
                {
                    totalDamageBonus += amount;
                }
            }
            // ITEM_PROPERTY_IMPROVED_SAVING_THROW (26): Saving throw bonus (all saves)
            else if (propType == 26)
            {
                if (amount > 0)
                {
                    totalSaveBonus += amount;
                }
            }
            // ITEM_PROPERTY_IMPROVED_SAVING_THROW_SPECIFIC (27): Specific saving throw bonus
            else if (propType == 27)
            {
                if (amount > 0 && subtype >= 0)
                {
                    // Save type: 0 = Fortitude, 1 = Reflex, 2 = Will (Aurora engine standard)
                    if (subtype == 0)
                    {
                        totalSaveFortBonus += amount;
                    }
                    else if (subtype == 1)
                    {
                        totalSaveRefBonus += amount;
                    }
                    else if (subtype == 2)
                    {
                        totalSaveWillBonus += amount;
                    }
                }
            }
            // ITEM_PROPERTY_SKILL_BONUS (36): Skill bonus
            else if (propType == 36)
            {
                if (amount > 0 && subtype >= 0)
                {
                    int skillId = subtype;
                    if (skillBonuses.ContainsKey(skillId))
                    {
                        skillBonuses[skillId] += amount;
                    }
                    else
                    {
                        skillBonuses[skillId] = amount;
                    }
                }
            }
            // ITEM_PROPERTY_DECREASED_ABILITY_SCORE (19): Ability penalty
            else if (propType == 19)
            {
                if (subtype >= 0 && subtype <= 5 && amount > 0)
                {
                    Ability ability = (Ability)subtype;
                    if (abilityBonuses.ContainsKey(ability))
                    {
                        abilityBonuses[ability] -= amount;
                    }
                    else
                    {
                        abilityBonuses[ability] = -amount;
                    }
                }
            }
            // ITEM_PROPERTY_DECREASED_AC (20): AC penalty
            else if (propType == 20)
            {
                if (amount > 0)
                {
                    totalACBonus -= amount;
                }
            }
            // ITEM_PROPERTY_DECREASED_ATTACK_MODIFIER (41): Attack penalty
            else if (propType == 41)
            {
                if (amount > 0)
                {
                    totalAttackBonus -= amount;
                }
            }
            // ITEM_PROPERTY_DECREASED_SAVING_THROWS (33): Saving throw penalty
            else if (propType == 33)
            {
                if (amount > 0)
                {
                    totalSaveBonus -= amount;
                }
            }
            // Additional property types that may affect stats can be added here
            // Note: Critical hit bonuses are typically not item properties but base item stats
            // Property types like damage resistance, immunity, etc. are defensive and don't affect displayed stats
        }

        /// <summary>
        /// Gets character inventory ResRefs for upgrade availability checking.
        /// </summary>
        /// <param name="character">Character to get inventory from (null uses player).</param>
        /// <returns>Set of inventory item ResRefs (normalized, lowercase, no extension).</returns>
        protected HashSet<string> GetCharacterInventoryResRefs(IEntity character)
        {
            HashSet<string> inventoryResRefs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            if (character == null)
            {
                // Get player character from world using multiple fallback strategies
                character = _world.GetEntityByTag("Player", 0);

                if (character == null)
                {
                    character = _world.GetEntityByTag("PlayerCharacter", 0);
                }

                if (character == null)
                {
                    foreach (IEntity entity in _world.GetAllEntities())
                    {
                        if (entity == null)
                        {
                            continue;
                        }

                        string tag = entity.Tag;
                        if (!string.IsNullOrEmpty(tag))
                        {
                            if (string.Equals(tag, "Player", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(tag, "PlayerCharacter", StringComparison.OrdinalIgnoreCase) ||
                                string.Equals(tag, "player", StringComparison.OrdinalIgnoreCase))
                            {
                                character = entity;
                                break;
                            }
                        }

                        object isPlayerData = entity.GetData("IsPlayer");
                        if (isPlayerData is bool && (bool)isPlayerData)
                        {
                            character = entity;
                            break;
                        }
                    }
                }
            }

            // Collect all inventory items from character
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

            return inventoryResRefs;
        }
    }
}

