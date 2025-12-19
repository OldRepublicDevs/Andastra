using Andastra.Runtime.Core.Collision;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;

namespace Andastra.Runtime.Games.Infinity.Collision
{
    /// <summary>
    /// Base class for Infinity-specific creature collision detection.
    /// Defaults to ME2 (MassEffect2.exe) behavior for backward compatibility.
    /// </summary>
    /// <remarks>
    /// Infinity Creature Collision Detection Base:
    /// - Common collision detection logic shared between ME1 (MassEffect.exe) and ME2 (MassEffect2.exe)
    /// - Uses appearance.2da hitradius for bounding box dimensions
    /// - Infinity engine uses Unreal Engine collision system (different from older engines)
    /// - Defaults to ME2 behavior for backward compatibility
    /// - Inheritance structure:
    ///   - BaseCreatureCollisionDetector (Runtime.Core.Collision): Common collision detection logic
    ///   - InfinityCreatureCollisionDetector (Runtime.Games.Infinity.Collision): Common Infinity logic, defaults to ME2
    ///   - ME1CreatureCollisionDetector (Runtime.Games.Infinity.Collision): ME1-specific (MassEffect.exe: Unreal Engine)
    ///   - ME2CreatureCollisionDetector (Runtime.Games.Infinity.Collision): ME2-specific (MassEffect2.exe: Unreal Engine)
    /// </remarks>
    public class InfinityCreatureCollisionDetector : BaseCreatureCollisionDetector
    {
        /// <summary>
        /// Gets the appearance type from an entity.
        /// Common logic shared between ME1 and ME2.
        /// </summary>
        protected int GetAppearanceType(IEntity entity)
        {
            if (entity == null)
            {
                return -1;
            }

            // First, try to get appearance type from IRenderableComponent
            IRenderableComponent renderable = entity.GetComponent<IRenderableComponent>();
            if (renderable != null)
            {
                return renderable.AppearanceRow;
            }

            // If not found, try to get appearance type from engine-specific creature component using reflection
            var entityType = entity.GetType();
            var appearanceTypeProp = entityType.GetProperty("AppearanceType");
            if (appearanceTypeProp != null)
            {
                try
                {
                    object appearanceTypeValue = appearanceTypeProp.GetValue(entity);
                    if (appearanceTypeValue is int)
                    {
                        return (int)appearanceTypeValue;
                    }
                }
                catch
                {
                    // Ignore reflection errors
                }
            }

            return -1;
        }

        /// <summary>
        /// Gets the bounding box for a creature entity.
        /// Defaults to ME2 (MassEffect2.exe) behavior for backward compatibility.
        /// Based on Infinity engine collision system using Unreal Engine.
        /// </summary>
        /// <param name="entity">The creature entity.</param>
        /// <returns>The creature's bounding box.</returns>
        /// <remarks>
        /// Defaults to ME2 behavior for backward compatibility.
        /// For ME1-specific behavior, use ME1CreatureCollisionDetector.
        /// For explicit ME2 behavior, use ME2CreatureCollisionDetector.
        /// Infinity engine uses Unreal Engine collision system with UnrealScript functions.
        /// </remarks>
        protected override CreatureBoundingBox GetCreatureBoundingBox(IEntity entity)
        {
            if (entity == null)
            {
                return CreatureBoundingBox.FromRadius(0.5f); // Default radius
            }

            // Get appearance type from entity
            int appearanceType = GetAppearanceType(entity);

            // Get bounding box dimensions from GameDataProvider
            float radius = 0.5f; // Default radius

            if (appearanceType >= 0 && entity.World != null && entity.World.GameDataProvider != null)
            {
                radius = entity.World.GameDataProvider.GetCreatureRadius(appearanceType, 0.5f);
            }

            return new CreatureBoundingBox(radius, radius, radius);
        }
    }
}

