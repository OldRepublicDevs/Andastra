using Andastra.Runtime.Core.Collision;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;

namespace Andastra.Runtime.Games.Eclipse.Collision
{
    /// <summary>
    /// Eclipse-specific creature collision detection.
    /// </summary>
    /// <remarks>
    /// Eclipse Creature Collision Detection:
    /// - Based on daorigins.exe, DragonAge2.exe, MassEffect.exe, MassEffect2.exe collision systems
    /// - Uses appearance.2da hitradius for bounding box dimensions
    /// - TODO: Reverse engineer specific function addresses from Eclipse executables using Ghidra MCP
    /// </remarks>
    public class EclipseCreatureCollisionDetector : BaseCreatureCollisionDetector
    {
        /// <summary>
        /// Gets the bounding box for a creature entity.
        /// Based on Eclipse engine collision system.
        /// </summary>
        protected override CreatureBoundingBox GetCreatureBoundingBox(IEntity entity)
        {
            if (entity == null)
            {
                return CreatureBoundingBox.FromRadius(0.5f); // Default radius
            }

            // Get appearance type from entity
            int appearanceType = -1;

            // First, try to get appearance type from IRenderableComponent
            IRenderableComponent renderable = entity.GetComponent<IRenderableComponent>();
            if (renderable != null)
            {
                appearanceType = renderable.AppearanceRow;
            }

            // If not found, try to get appearance type from engine-specific creature component using reflection
            if (appearanceType < 0)
            {
                var entityType = entity.GetType();
                var appearanceTypeProp = entityType.GetProperty("AppearanceType");
                if (appearanceTypeProp != null)
                {
                    try
                    {
                        object appearanceTypeValue = appearanceTypeProp.GetValue(entity);
                        if (appearanceTypeValue is int)
                        {
                            appearanceType = (int)appearanceTypeValue;
                        }
                    }
                    catch
                    {
                        // Ignore reflection errors
                    }
                }
            }

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

