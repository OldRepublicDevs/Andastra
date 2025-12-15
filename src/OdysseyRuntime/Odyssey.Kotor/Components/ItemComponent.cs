using System.Collections.Generic;
using Odyssey.Core.Interfaces.Components;

namespace Odyssey.Kotor.Components
{
    /// <summary>
    /// Component for item entities that stores UTI template data.
    /// </summary>
    /// <remarks>
    /// Item Component:
    /// - Based on swkotor2.exe item system
    /// - Located via string references: "ItemList" @ 0x007c2f28 (inventory item list), "Equip_ItemList" @ 0x007c2f20 (equipped items)
    /// - "BaseItem" @ 0x007c2f34 (base item type ID), "Properties" @ 0x007c2f3c (item properties list)
    /// - "Charges" @ 0x007c2f48 (item charges), "Cost" @ 0x007c2f50 (item cost/price)
    /// - "StackSize" @ 0x007c2f5c (stack size for stackable items), "Identified" @ 0x007c2f64 (identified flag)
    /// - Template loading: FUN_005fb0f0 @ 0x005fb0f0 loads item templates from UTI GFF files
    /// - Original implementation: Items have base item ID, properties, upgrades, charges, stack size
    /// - UTI file format: GFF with "UTI " signature containing item data (BaseItem, Properties, Charges, Cost)
    /// - Item properties modify item behavior (damage bonuses, AC bonuses, effects, etc.) - stored as PropertyList array
    /// - Upgrades modify item stats (damage, AC, etc.) - stored as UpgradeList array (K2 feature)
    /// - Charges: -1 = unlimited charges, 0+ = limited charges (items with charges consume one per use)
    /// - Stack size: 1 = not stackable, 2+ = stackable (maximum stack size from baseitems.2da)
    /// - Identified: false = unidentified item (shows generic name, can be identified via IdentifyItem function)
    /// - Based on UTI file format documentation in vendor/PyKotor/wiki/
    /// </remarks>
    public class ItemComponent : IItemComponent
    {
        private readonly List<ItemProperty> _properties;
        private readonly List<ItemUpgrade> _upgrades;

        public IEntity Owner { get; set; }

        public void OnAttach() { }
        public void OnDetach() { }

        public ItemComponent()
        {
            _properties = new List<ItemProperty>();
            _upgrades = new List<ItemUpgrade>();
            BaseItem = 0;
            StackSize = 1;
            Charges = -1; // -1 = unlimited charges
            Cost = 0;
            Identified = true;
            TemplateResRef = string.Empty;
        }

        public int BaseItem { get; set; }
        public int StackSize { get; set; }
        public int Charges { get; set; }
        public int Cost { get; set; }
        public bool Identified { get; set; }
        public string TemplateResRef { get; set; }

        public IReadOnlyList<ItemProperty> Properties
        {
            get { return _properties; }
        }

        public IReadOnlyList<ItemUpgrade> Upgrades
        {
            get { return _upgrades; }
        }

        public void AddProperty(ItemProperty property)
        {
            if (property != null)
            {
                _properties.Add(property);
            }
        }

        public void RemoveProperty(ItemProperty property)
        {
            if (property != null)
            {
                _properties.Remove(property);
            }
        }

        public void AddUpgrade(ItemUpgrade upgrade)
        {
            if (upgrade != null)
            {
                _upgrades.Add(upgrade);
            }
        }

        public void RemoveUpgrade(ItemUpgrade upgrade)
        {
            if (upgrade != null)
            {
                _upgrades.Remove(upgrade);
            }
        }
    }
}

