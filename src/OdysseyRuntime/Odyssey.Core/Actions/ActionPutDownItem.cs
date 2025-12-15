using System.Numerics;
using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;
using Odyssey.Core.Interfaces.Components;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Action to drop/put down an item from inventory to the world.
    /// </summary>
    /// <remarks>
    /// Put Down Item Action:
    /// - Based on swkotor2.exe ActionPutDownItem NWScript function
    /// - Original implementation: Removes item from inventory and places it in world at specified location
    /// - Item becomes a world-dropped item that can be picked up by other entities
    /// - Used for dropping items, giving items to other entities, placing items in containers
    /// - Item position set to drop location, item becomes visible in world
    /// </remarks>
    public class ActionPutDownItem : ActionBase
    {
        private readonly uint _itemObjectId;
        private readonly Vector3 _dropLocation;

        public ActionPutDownItem(uint itemObjectId, Vector3 dropLocation)
            : base(ActionType.GiveItem)
        {
            _itemObjectId = itemObjectId;
            _dropLocation = dropLocation;
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

            // Check if item is in inventory
            bool hasItem = false;
            foreach (IEntity invItem in inventory.GetAllItems())
            {
                if (invItem != null && invItem.ObjectId == _itemObjectId)
                {
                    hasItem = true;
                    break;
                }
            }

            if (!hasItem)
            {
                return ActionStatus.Failed;
            }

            // Remove item from inventory
            if (inventory.RemoveItem(actor.World.GetEntity(_itemObjectId)))
            {
                // Place item in world at drop location
                IEntity item = actor.World.GetEntity(_itemObjectId);
                if (item != null)
                {
                    ITransformComponent itemTransform = item.GetComponent<ITransformComponent>();
                    if (itemTransform != null)
                    {
                        itemTransform.Position = _dropLocation;
                    }
                }

                return ActionStatus.Complete;
            }

            return ActionStatus.Failed;
        }
    }
}

