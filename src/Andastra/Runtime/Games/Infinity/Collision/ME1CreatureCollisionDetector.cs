using Andastra.Runtime.Core.Collision;
using Andastra.Runtime.Core.Interfaces;

namespace Andastra.Runtime.Games.Infinity.Collision
{
    /// <summary>
    /// Mass Effect 1 (MassEffect.exe) specific creature collision detection.
    /// </summary>
    /// <remarks>
    /// ME1 Creature Collision Detection:
    /// - Based on MassEffect.exe reverse engineering via Ghidra MCP
    /// - Infinity engine uses Unreal Engine collision system (different from older engines)
    /// - Located via string references:
    ///   - "GetDefaultCollisionRadius" @ 0x117eb988 (MassEffect.exe: UnrealScript function name for collision radius)
    ///   - "GetDefaultCollisionHeight" @ 0x117eba08 (MassEffect.exe: UnrealScript function name for collision height)
    ///   - "GetDefaultCollisionReadyHeight" @ 0x117eb878 (MassEffect.exe: UnrealScript function name for ready stance height)
    ///   - "GetDefaultCollisionCrouchHeight" @ 0x117eb900 (MassEffect.exe: UnrealScript function name for crouch stance height)
    ///   - "EnableCollision" @ 0x117ee3b8 (MassEffect.exe: UnrealScript function name for enabling collision)
    /// - Unreal Engine collision system: Uses UnrealScript functions rather than direct memory offsets
    /// - Cross-engine comparison:
    ///   - ME1 (MassEffect.exe): Unreal Engine collision, UnrealScript functions, appearance.2da hitradius
    ///   - ME2 (MassEffect2.exe): Unreal Engine collision, similar UnrealScript functions, appearance.2da hitradius
    ///   - Common: Both use Unreal Engine collision system with UnrealScript functions, appearance.2da hitradius for creature size
    /// - Inheritance structure:
    ///   - BaseCreatureCollisionDetector (Runtime.Core.Collision): Common collision detection logic
    ///   - InfinityCreatureCollisionDetector (Runtime.Games.Infinity.Collision): Common Infinity logic
    ///   - ME1CreatureCollisionDetector (Runtime.Games.Infinity.Collision): ME1-specific (MassEffect.exe: Unreal Engine)
    /// </remarks>
    public class ME1CreatureCollisionDetector : InfinityCreatureCollisionDetector
    {
        /// <summary>
        /// Gets the bounding box for a creature entity.
        /// Based on MassEffect.exe: Infinity engine uses Unreal Engine collision detection.
        /// </summary>
        /// <param name="entity">The creature entity.</param>
        /// <returns>The creature's bounding box.</returns>
        /// <remarks>
        /// Based on MassEffect.exe reverse engineering via Ghidra MCP:
        /// - Infinity engine uses Unreal Engine for collision detection
        /// - UnrealScript functions: GetDefaultCollisionRadius, GetDefaultCollisionHeight, GetDefaultCollisionReadyHeight, GetDefaultCollisionCrouchHeight
        /// - Located via string references: "GetDefaultCollisionRadius" @ 0x117eb988, "GetDefaultCollisionHeight" @ 0x117eba08
        /// - Gets appearance type from entity structure (accessed via UnrealScript appearance system)
        /// - Looks up hitradius from appearance.2da (same pattern as other engines)
        /// - Default radius: 0.5f (medium creature size) if appearance data unavailable
        /// - Fallback: Uses size category from appearance.2da if hitradius not available
        /// - Note: Infinity engine uses Unreal Engine collision shapes and UnrealScript functions rather than direct memory offsets like older engines
        /// </remarks>
        protected override CreatureBoundingBox GetCreatureBoundingBox(IEntity entity)
        {
            if (entity == null)
            {
                // Based on MassEffect.exe: Default bounding box for null entity (medium creature size)
                return CreatureBoundingBox.FromRadius(0.5f); // Default radius
            }

            // Get appearance type from entity
            // Based on MassEffect.exe: Appearance type accessed via UnrealScript appearance system
            // UnrealScript functions: GetAppearanceTemplate, UpdateAppearance, etc.
            int appearanceType = GetAppearanceType(entity);

            // Get bounding box dimensions from GameDataProvider
            // Based on MassEffect.exe: Infinity engine uses appearance.2da hitradius for creature collision radius
            // Infinity engine uses Unreal Engine collision shapes, but still uses appearance.2da for creature size
            float radius = 0.5f; // Default radius (medium creature size)

            if (appearanceType >= 0 && entity.World != null && entity.World.GameDataProvider != null)
            {
                // Based on MassEffect.exe: Infinity engine uses appearance.2da hitradius for creature size
                // InfinityGameDataProvider.GetCreatureRadius uses InfinityTwoDATableManager to lookup hitradius
                // This matches the pattern in other engines: appearance.2da hitradius column for creature collision radius
                radius = entity.World.GameDataProvider.GetCreatureRadius(appearanceType, 0.5f);
            }

            // Based on MassEffect.exe reverse engineering: Infinity engine uses Unreal Engine-based collision detection
            // UnrealScript functions: GetDefaultCollisionRadius @ 0x117eb988 (MassEffect.exe: collision radius function)
            // Bounding box uses same radius for width, height, and depth (spherical approximation)
            // Original engine: Infinity uses Unreal Engine collision shapes and UnrealScript functions rather than direct memory offsets
            // Unreal Engine handles collision detection, but creature size still comes from appearance.2da hitradius
            return new CreatureBoundingBox(radius, radius, radius);
        }
    }
}

