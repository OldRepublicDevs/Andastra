using System;
using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;
using Odyssey.Core.Interfaces.Components;
using Odyssey.Kotor.Components;
using JetBrains.Annotations;

namespace Odyssey.Kotor.Systems
{
    /// <summary>
    /// Initializes default components for entities based on their object type.
    /// </summary>
    /// <remarks>
    /// Component Initializer:
    /// - Based on swkotor2.exe component initialization system
    /// - Automatically adds required components when entities are created
    /// - Ensures entities have appropriate components for their type
    /// - Based on swkotor2.exe: Components are added during entity creation
    /// </remarks>
    public static class ComponentInitializer
    {
        /// <summary>
        /// Initializes default components for an entity based on its object type.
        /// </summary>
        public static void InitializeComponents([NotNull] IEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            // Add TransformComponent if not present
            if (!entity.HasComponent<ITransformComponent>())
            {
                var transform = new TransformComponent();
                transform.Position = entity.Position;
                transform.Facing = entity.Facing;
                entity.AddComponent(transform);
            }

            // Add RenderableComponent for renderable entity types
            if (ShouldHaveRenderableComponent(entity.ObjectType))
            {
                if (!entity.HasComponent<IRenderableComponent>())
                {
                    var renderable = new RenderableComponent();
                    entity.AddComponent(renderable);
                }
            }

            // Add type-specific components
            switch (entity.ObjectType)
            {
                case ObjectType.Creature:
                    if (!entity.HasComponent<CreatureComponent>())
                    {
                        entity.AddComponent(new CreatureComponent());
                    }
                    if (!entity.HasComponent<IStatsComponent>())
                    {
                        entity.AddComponent(new StatsComponent());
                    }
                    break;

                case ObjectType.Door:
                    if (!entity.HasComponent<DoorComponent>())
                    {
                        entity.AddComponent(new DoorComponent());
                    }
                    break;

                case ObjectType.Placeable:
                    if (!entity.HasComponent<PlaceableComponent>())
                    {
                        entity.AddComponent(new PlaceableComponent());
                    }
                    break;

                case ObjectType.Trigger:
                    if (!entity.HasComponent<TriggerComponent>())
                    {
                        entity.AddComponent(new TriggerComponent());
                    }
                    break;

                case ObjectType.Waypoint:
                    if (!entity.HasComponent<WaypointComponent>())
                    {
                        entity.AddComponent(new WaypointComponent());
                    }
                    break;

                case ObjectType.Sound:
                    if (!entity.HasComponent<SoundComponent>())
                    {
                        entity.AddComponent(new SoundComponent());
                    }
                    break;

                case ObjectType.Store:
                    if (!entity.HasComponent<StoreComponent>())
                    {
                        entity.AddComponent(new StoreComponent());
                    }
                    break;

                case ObjectType.Encounter:
                    if (!entity.HasComponent<EncounterComponent>())
                    {
                        entity.AddComponent(new EncounterComponent());
                    }
                    break;
            }

            // Add ScriptHooksComponent for all entities (most entities have scripts)
            if (!entity.HasComponent<IScriptHooksComponent>())
            {
                entity.AddComponent(new ScriptHooksComponent());
            }
        }

        /// <summary>
        /// Determines if an entity type should have a RenderableComponent.
        /// </summary>
        private static bool ShouldHaveRenderableComponent(ObjectType objectType)
        {
            switch (objectType)
            {
                case ObjectType.Creature:
                case ObjectType.Door:
                case ObjectType.Placeable:
                case ObjectType.Item:
                    return true;
                default:
                    return false;
            }
        }
    }
}

