using System.Collections.Generic;

namespace Andastra.Runtime.Core.Interfaces.Components
{
    /// <summary>
    /// Component for managing entity status effects (buffs, debuffs).
    /// </summary>
    /// <remarks>
    /// Effect Component Interface:
    /// - Common interface for effect management across all BioWare engines
    /// - Provides status effect tracking (buffs, debuffs, temporary effects)
    /// - Effects have duration, icon, and type (buff/debuff/neutral)
    /// - GetActiveEffects: Returns all currently active effects on the entity
    /// - AddEffect: Adds a new effect to the entity
    /// - RemoveEffect: Removes an effect from the entity
    /// - HasEffect: Checks if entity has a specific effect type
    /// </remarks>
    public interface IEffectComponent : IComponent
    {
        /// <summary>
        /// Gets all active effects on this entity.
        /// </summary>
        /// <returns>Collection of active effects.</returns>
        IEnumerable<IActiveEffect> GetActiveEffects();

        /// <summary>
        /// Adds an effect to this entity.
        /// </summary>
        /// <param name="effect">The effect to add.</param>
        void AddEffect(IActiveEffect effect);

        /// <summary>
        /// Removes an effect from this entity.
        /// </summary>
        /// <param name="effect">The effect to remove.</param>
        void RemoveEffect(IActiveEffect effect);

        /// <summary>
        /// Checks if the entity has a specific effect type.
        /// </summary>
        /// <param name="effectType">The effect type to check for.</param>
        /// <returns>True if the entity has the effect, false otherwise.</returns>
        bool HasEffect(string effectType);
    }

    /// <summary>
    /// Represents an active status effect on an entity.
    /// </summary>
    public interface IActiveEffect
    {
        /// <summary>
        /// Effect type identifier (e.g., "Regeneration", "Poison", "Haste").
        /// </summary>
        string EffectType { get; }

        /// <summary>
        /// Effect icon texture name/resource reference.
        /// </summary>
        string IconResRef { get; }

        /// <summary>
        /// Remaining duration in seconds (0 = permanent).
        /// </summary>
        float RemainingDuration { get; set; }

        /// <summary>
        /// Whether this is a buff (true) or debuff (false).
        /// </summary>
        bool IsBuff { get; }

        /// <summary>
        /// Effect stack count (for stackable effects).
        /// </summary>
        int StackCount { get; set; }
    }
}

