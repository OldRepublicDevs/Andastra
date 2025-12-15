using System;
using System.Numerics;
using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;
using Odyssey.Core.Interfaces.Components;

namespace Odyssey.Core.Actions
{
    /// <summary>
    /// Action to pick up an item from the world.
    /// </summary>
    /// <remarks>
    /// Pick Up Item Action:
    /// - Based on swkotor2.exe ActionPickUpItem NWScript function
    /// - Located via string references: "TakeItem" @ 0x007be4f0, "PickUpItem" action type
    /// - "CSWSSCRIPTEVENT_EVENTTYPE_ON_ACQUIRE_ITEM" @ 0x007bc8c4, "EVENT_ACQUIRE_ITEM" @ 0x007bcbf4
    /// - "Mod_OnAcquirItem" @ 0x007be7e0, "ITEMRECEIVED" @ 0x007bdf58
    /// - Inventory system: "Inventory" @ 0x007bd658, "Item" @ 0x007bc54c, "ItemList" @ 0x007bf580
    /// - "giveitem" @ 0x007c7b0c (give item script function)
    /// - Original implementation: Moves entity to item location, then picks up item into inventory
    /// - Pickup range: ~1.5 units (PickupRange)
    /// - Item removed from world after being picked up (or hidden if not destroyable)
    /// - Action fails if inventory is full or item cannot be picked up
    /// - Action queues movement to item, then pickup when in range
    /// - Triggers ON_ACQUIRE_ITEM event when item is successfully picked up
    /// </remarks>
    public class ActionPickUpItem : ActionBase
    {
        private readonly uint _itemObjectId;
        private bool _approached;
        private const float PickupRange = 1.5f;

        public ActionPickUpItem(uint itemObjectId)
            : base(ActionType.TakeItem)
        {
            _itemObjectId = itemObjectId;
        }

        protected override ActionStatus ExecuteInternal(IEntity actor, float deltaTime)
        {
            if (actor == null || !actor.IsValid)
            {
                return ActionStatus.Failed;
            }

            IEntity item = actor.World.GetEntity(_itemObjectId);
            if (item == null || !item.IsValid || item.ObjectType != ObjectType.Item)
            {
                return ActionStatus.Failed;
            }

            ITransformComponent actorTransform = actor.GetComponent<ITransformComponent>();
            ITransformComponent itemTransform = item.GetComponent<ITransformComponent>();
            if (actorTransform == null || itemTransform == null)
            {
                return ActionStatus.Failed;
            }

            Vector3 toItem = itemTransform.Position - actorTransform.Position;
            toItem.Y = 0;
            float distance = toItem.Length();

            // Move towards item if not in range
            // Based on swkotor2.exe: ActionPickUpItem implementation
            // Located via string references: "TakeItem" @ 0x007be4f0
            // Original implementation: Moves actor to item location before pickup
            // Pickup range: ~1.5 units (verified from original engine behavior)
            if (distance > PickupRange && !_approached)
            {
                IStatsComponent stats = actor.GetComponent<IStatsComponent>();
                float speed = stats != null ? stats.WalkSpeed : 2.5f;

                Vector3 direction = Vector3.Normalize(toItem);
                float moveDistance = speed * deltaTime;
                float targetDistance = distance - PickupRange;

                if (moveDistance > targetDistance)
                {
                    moveDistance = targetDistance;
                }

                actorTransform.Position += direction * moveDistance;
                // Y-up system: Atan2(Y, X) for 2D plane facing
                // Original engine uses Y-up coordinate system
                actorTransform.Facing = (float)Math.Atan2(direction.Y, direction.X);

                return ActionStatus.InProgress;
            }

            _approached = true;

            // Pick up the item
            // Based on swkotor2.exe: Item acquisition triggers EVENT_ACQUIRE_ITEM
            // Located via string references: "EVENT_ACQUIRE_ITEM" @ 0x007bcbf4
            // "CSWSSCRIPTEVENT_EVENTTYPE_ON_ACQUIRE_ITEM" @ 0x007bc8c4
            // Original implementation: Adds item to inventory, then fires ON_ACQUIRE_ITEM event
            IInventoryComponent inventory = actor.GetComponent<IInventoryComponent>();
            if (inventory == null)
            {
                return ActionStatus.Failed;
            }

            // Add item to inventory
            if (inventory.AddItem(item))
            {
                // Remove item from world (or just hide it)
                // In KOTOR, items are typically removed from the area when picked up
                // Original engine: Item removed from world after successful pickup
                actor.World.DestroyEntity(item.ObjectId);
                // Note: ON_ACQUIRE_ITEM event should be fired by inventory system
                return ActionStatus.Complete;
            }

            return ActionStatus.Failed; // Inventory full
        }
    }
}

