using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Games.Common.Components;
using Andastra.Runtime.Games.Infinity.Systems;

namespace Andastra.Runtime.Games.Infinity.Components
{
    /// <summary>
    /// Infinity engine-specific implementation of faction component for Mass Effect.
    /// </summary>
    /// <remarks>
    /// Infinity Faction Component:
    /// - Inherits common functionality from BaseFactionComponent
    /// - Implements Infinity-specific faction system using BioFaction classes
    /// - Based on MassEffect.exe and MassEffect2.exe faction systems
    ///
    /// Infinity-specific details:
    /// - MassEffect.exe: Infinity faction system with BioFaction classes
    /// - MassEffect2.exe: Enhanced Infinity faction system with similar structure
    /// - Located via string references: "intABioBaseSquadexecGetFaction" @ 0x11806a44 (MassEffect2.exe)
    /// - "intABioBaseSquadexecSetFaction" @ 0x11806c20 (MassEffect2.exe)
    /// - "intABioBaseSquadexecFactionRelationship" @ 0x11806ad0 (MassEffect2.exe)
    /// - "intABioPawnexecFactionRelationship" @ 0x118099c8 (MassEffect2.exe)
    /// - "OnFactionChanged" @ 0x11816fc8 (MassEffect2.exe)
    /// - "UBioFaction" @ 0x1181a084 (MassEffect2.exe)
    /// - "UBioFaction_Player" @ 0x1181a0ec (MassEffect2.exe)
    /// - "UBioFaction_Hacked" @ 0x1181a09c (MassEffect2.exe)
    /// - "UBioFaction_Vehicle" @ 0x1181a0c4 (MassEffect2.exe)
    /// - "SquadFaction" @ 0x1189c474 (MassEffect2.exe)
    /// - "EBioFactionTypes" @ 0x11a05a5c (MassEffect2.exe)
    /// - Error: "GetFaction: Error returning default netural for faction" @ 0x1189c518 (MassEffect2.exe)
    /// - Original implementation: FactionId references BioFaction type
    /// - Infinity engines use BioFaction enum/types for faction determination
    /// - Faction relationships may be determined by squad membership, plot flags, or other game state
    /// - Temporary hostility tracked per-entity (stored in TemporaryHostileTargets HashSet from base class)
    /// - Infinity-specific: Uses BioFaction types (Player, Hacked, Vehicle, etc.) rather than numeric IDs
    /// - InfinityFactionManager handles complex faction relationships and reputation lookups
    /// </remarks>
    public class InfinityFactionComponent : BaseFactionComponent
    {
        private InfinityFactionManager _factionManager;

        /// <summary>
        /// Initializes a new instance of the Infinity faction component.
        /// </summary>
        public InfinityFactionComponent()
        {
            FactionId = 0; // Default to neutral/unassigned faction
        }

        /// <summary>
        /// Initializes a new instance of the Infinity faction component with a faction manager.
        /// </summary>
        /// <param name="factionManager">The faction manager to use for reputation lookups.</param>
        public InfinityFactionComponent(InfinityFactionManager factionManager) : this()
        {
            _factionManager = factionManager;
        }

        /// <summary>
        /// Called when the component is attached to an entity.
        /// </summary>
        public override void OnAttach()
        {
            // FactionId is set during entity creation
            base.OnAttach();
        }

        /// <summary>
        /// Checks if this entity is hostile to another.
        /// </summary>
        /// <param name="other">The other entity to check hostility against.</param>
        /// <returns>True if hostile, false otherwise.</returns>
        /// <remarks>
        /// Based on MassEffect2.exe: intABioBaseSquadexecFactionRelationship @ 0x11806ad0
        /// Uses InfinityFactionManager for reputation-based hostility determination.
        /// Infinity-specific: Squad members are always friendly to each other.
        /// </remarks>
        public override bool IsHostile(IEntity other)
        {
            if (other == null || other == Owner)
            {
                return false;
            }

            // Check temporary hostility first (from base class)
            if (TemporaryHostileTargets.Contains(other.ObjectId))
            {
                return true;
            }

            // Use faction manager if available
            if (_factionManager != null)
            {
                return _factionManager.IsHostile(Owner, other);
            }

            // Fall back to simple faction comparison
            IFactionComponent otherFaction = other.GetComponent<IFactionComponent>();
            if (otherFaction == null)
            {
                return false;
            }

            // Same faction = friendly
            if (FactionId == otherFaction.FactionId)
            {
                return false;
            }

            // Default: different factions are neutral (Infinity-specific logic would check BioFaction types)
            return false;
        }

        /// <summary>
        /// Checks if this entity is friendly to another.
        /// </summary>
        /// <param name="other">The other entity to check friendliness against.</param>
        /// <returns>True if friendly, false otherwise.</returns>
        /// <remarks>
        /// Based on MassEffect2.exe: intABioPawnexecFactionRelationship @ 0x118099c8
        /// Uses InfinityFactionManager for reputation-based friendliness determination.
        /// Infinity-specific: Squad members are always friendly to each other.
        /// </remarks>
        public override bool IsFriendly(IEntity other)
        {
            if (other == null)
            {
                return false;
            }

            if (other == Owner)
            {
                return true;
            }

            // Temporarily hostile = not friendly (from base class)
            if (TemporaryHostileTargets.Contains(other.ObjectId))
            {
                return false;
            }

            // Use faction manager if available
            if (_factionManager != null)
            {
                return _factionManager.IsFriendly(Owner, other);
            }

            // Fall back to simple faction comparison
            IFactionComponent otherFaction = other.GetComponent<IFactionComponent>();
            if (otherFaction == null)
            {
                return false;
            }

            // Same faction = friendly
            return FactionId == otherFaction.FactionId;
        }

        /// <summary>
        /// Sets temporary hostility toward a target.
        /// </summary>
        /// <param name="target">The target entity.</param>
        /// <param name="hostile">True to set as hostile, false to clear hostility.</param>
        public override void SetTemporaryHostile(IEntity target, bool hostile)
        {
            base.SetTemporaryHostile(target, hostile);

            // Also update faction manager if available
            if (_factionManager != null)
            {
                _factionManager.SetTemporaryHostile(Owner, target, hostile);
            }
        }

        /// <summary>
        /// Sets the faction manager reference.
        /// </summary>
        /// <param name="manager">The faction manager to use for reputation lookups.</param>
        public void SetFactionManager(InfinityFactionManager manager)
        {
            _factionManager = manager;
        }
    }
}

