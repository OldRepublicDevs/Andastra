using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;
using Odyssey.Core.Interfaces.Components;

namespace Odyssey.Kotor.Components
{
    /// <summary>
    /// Component for managing entity inventory and equipped items.
    /// </summary>
    /// <remarks>
    /// Inventory Component:
    /// - Based on swkotor2.exe inventory system
    /// - Located via string references: "Inventory" @ various locations, "INVENTORY_SLOT_*" constants
    /// - Inventory slots: Equipped items (weapon, armor, shield, etc.) and inventory bag (array of slots)
    /// - Original implementation: Inventory stored in GFF format (UTC creature templates, save files)
    /// - Slot indices: INVENTORY_SLOT_* constants from NWScript (0-17 for equipped, 18+ for inventory bag)
    /// - Based on KOTOR inventory system from vendor/PyKotor/wiki/ and plan documentation
    /// </remarks>
    public class InventoryComponent : IInventoryComponent
    {
        private readonly Dictionary<int, IEntity> _slots;
        private readonly IEntity _owner;
        private const int MaxInventorySlots = 100; // Maximum inventory bag size

        public InventoryComponent([NotNull] IEntity owner)
        {
            _owner = owner ?? throw new ArgumentNullException("owner");
            _slots = new Dictionary<int, IEntity>();
        }

        public IEntity Owner
        {
            get { return _owner; }
        }

        /// <summary>
        /// Gets the item in the specified inventory slot.
        /// </summary>
        public IEntity GetItemInSlot(int slot)
        {
            IEntity item;
            if (_slots.TryGetValue(slot, out item))
            {
                return item;
            }
            return null;
        }

        /// <summary>
        /// Sets an item in the specified inventory slot.
        /// </summary>
        public void SetItemInSlot(int slot, IEntity item)
        {
            if (item == null)
            {
                // Clear slot
                _slots.Remove(slot);
            }
            else
            {
                // Remove item from any previous slot
                RemoveItem(item);
                
                // Place item in new slot
                _slots[slot] = item;
            }
        }

        /// <summary>
        /// Adds an item to the inventory (finds first available slot).
        /// </summary>
        public bool AddItem(IEntity item)
        {
            if (item == null)
            {
                return false;
            }

            // Check if item is already in inventory
            if (HasItem(item))
            {
                return false; // Already in inventory
            }

            // Find first available inventory slot (start from slot 18, which is first inventory bag slot)
            for (int slot = 18; slot < 18 + MaxInventorySlots; slot++)
            {
                if (!_slots.ContainsKey(slot))
                {
                    _slots[slot] = item;
                    return true;
                }
            }

            return false; // Inventory full
        }

        /// <summary>
        /// Removes an item from the inventory.
        /// </summary>
        public bool RemoveItem(IEntity item)
        {
            if (item == null)
            {
                return false;
            }

            // Find and remove item from any slot
            var slotsToRemove = new List<int>();
            foreach (KeyValuePair<int, IEntity> kvp in _slots)
            {
                if (kvp.Value == item)
                {
                    slotsToRemove.Add(kvp.Key);
                }
            }

            foreach (int slot in slotsToRemove)
            {
                _slots.Remove(slot);
            }

            return slotsToRemove.Count > 0;
        }

        /// <summary>
        /// Checks if the entity has an item with the specified tag.
        /// </summary>
        public bool HasItemByTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return false;
            }

            foreach (IEntity item in _slots.Values)
            {
                if (item != null && item.IsValid && string.Equals(item.Tag, tag, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets all items in the inventory.
        /// </summary>
        public IEnumerable<IEntity> GetAllItems()
        {
            return _slots.Values.Where(item => item != null && item.IsValid).Distinct();
        }

        /// <summary>
        /// Checks if an item is in the inventory.
        /// </summary>
        private bool HasItem(IEntity item)
        {
            return _slots.Values.Contains(item);
        }
    }
}

