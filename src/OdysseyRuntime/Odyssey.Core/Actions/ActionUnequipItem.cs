using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;
using Odyssey.Core.Interfaces.Components;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Action to unequip an item from a specific inventory slot.
    /// </summary>
    /// <remarks>
    /// Unequip Item Action:
    /// - Based on swkotor2.exe ActionUnequipItem NWScript function
    /// - Original implementation: Removes item from specified equipment slot, returns it to inventory
    /// - Unequipping item removes stat modifications (AC, damage, abilities, etc.)
    /// - Item remains in entity's inventory after unequipping
    /// - Equipment slots: Armor, Helmet, Implant, RightArm, LeftArm, etc. (see InventorySlot enum)
    /// </remarks>
    public class ActionUnequipItem : ActionBase
    {
        private readonly int _inventorySlot;

        public ActionUnequipItem(int inventorySlot)
            : base(ActionType.UnequipItem)
        {
            _inventorySlot = inventorySlot;
        }

        protected override ActionStatus ExecuteInternal(IEntity actor, float deltaTime)
        {
            if (actor == null || !actor.IsValid)
            {
                return ActionStatus.Failed;
            }

            IInventoryComponent inventory = actor.GetComponent<IInventoryComponent>();
            if (inventory == null)
            {
                return ActionStatus.Failed;
            }

            // Clear the slot (unequip)
            inventory.SetItemInSlot(_inventorySlot, null);

            return ActionStatus.Complete;
        }
    }
}

