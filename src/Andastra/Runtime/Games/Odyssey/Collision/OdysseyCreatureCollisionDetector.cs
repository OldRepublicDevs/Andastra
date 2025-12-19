using Andastra.Runtime.Core.Collision;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;

namespace Andastra.Runtime.Games.Odyssey.Collision
{
    /// <summary>
    /// Odyssey-specific creature collision detection.
    /// </summary>
    /// <remarks>
    /// Odyssey Creature Collision Detection:
    /// - Based on swkotor.exe and swkotor2.exe: FUN_005479f0 @ 0x005479f0 (swkotor2.exe)
    /// - Bounding box stored at offset 0x380 + 0x14 (width), 0x380 + 0xbc (height)
    /// - Uses appearance.2da hitradius for bounding box dimensions
    /// - Original implementation: FUN_004e17a0 @ 0x004e17a0 (spatial query), FUN_004f5290 @ 0x004f5290 (detailed collision)
    /// </remarks>
    public class OdysseyCreatureCollisionDetector : BaseCreatureCollisionDetector
    {
        /// <summary>
        /// Gets the bounding box for a creature entity.
        /// Based on swkotor2.exe: FUN_005479f0 @ 0x005479f0 uses creature bounding box from entity structure
        /// Bounding box stored at offset 0x380 + 0x14 (width), 0x380 + 0xbc (height)
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
            // Based on swkotor2.exe: FUN_005479f0 gets width and height from entity structure
            // Width stored at offset 0x380 + 0x14, height at offset 0x380 + 0xbc
            // For now, we use hitradius from appearance.2da as the base radius
            // The original engine uses width and height separately, but we approximate with radius
            float radius = 0.5f; // Default radius

            if (appearanceType >= 0 && entity.World != null && entity.World.GameDataProvider != null)
            {
                radius = entity.World.GameDataProvider.GetCreatureRadius(appearanceType, 0.5f);
            }

            // Based on swkotor2.exe: Bounding box uses width and height separately
            // Width is typically the horizontal extent (X/Z plane), height is vertical (Y axis)
            // For simplicity, we use radius for width/depth and height separately
            // Original engine: width at 0x380+0x14, height at 0x380+0xbc
            // We approximate: width = radius, height = radius (can be adjusted based on creature size)
            return new CreatureBoundingBox(radius, radius, radius);
        }
    }
}

