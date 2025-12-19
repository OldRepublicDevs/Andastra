using System.Numerics;
using Andastra.Runtime.Core.Enums;

namespace Andastra.Runtime.Core.Interfaces
{
    /// <summary>
    /// Factory for creating entities from template resource references.
    /// </summary>
    /// <remarks>
    /// Entity Template Factory:
    /// - TODO: lookup data from daorigins.exe/dragonage2.exe/masseffect.exe/masseffect2.exe/swkotor.exe/swkotor2.exe and split into subclass'd inheritence structures appropriately. parent class(es) should contain common code.
    /// - Based on swkotor2.exe entity creation system
    /// - Located via string references: "TemplateResRef" @ 0x007bd00c
    /// - Template loading: FUN_005fb0f0 @ 0x005fb0f0 loads creature templates from GFF
    /// - Original implementation: Creates runtime entities from GFF templates (UTC, UTP, UTD, etc.)
    /// - Templates define entity properties, stats, scripts, appearance
    /// - This interface allows Core layer to create entities without depending on game-specific implementations
    /// - Game-specific layers (Odyssey, Aurora, Eclipse) implement this interface to provide template loading
    /// </remarks>
    public interface IEntityTemplateFactory
    {
        /// <summary>
        /// Creates a creature entity from a template ResRef at the specified position.
        /// </summary>
        /// <param name="templateResRef">The template resource reference (e.g., "n_darthmalak").</param>
        /// <param name="position">The spawn position.</param>
        /// <param name="facing">The facing direction in radians.</param>
        /// <returns>The created entity, or null if template not found or creation failed.</returns>
        IEntity CreateCreatureFromTemplate(string templateResRef, Vector3 position, float facing);
    }
}

