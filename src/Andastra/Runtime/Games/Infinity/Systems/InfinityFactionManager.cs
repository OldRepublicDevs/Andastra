using System;
using System.Collections.Generic;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;

namespace Andastra.Runtime.Games.Infinity.Systems
{
    /// <summary>
    /// Standard BioFaction types for Infinity engine (Mass Effect, Mass Effect 2).
    /// </summary>
    /// <remarks>
    /// Infinity BioFaction Types:
    /// - Based on MassEffect.exe and MassEffect2.exe BioFaction classes
    /// - Located via string references: "UBioFaction" @ 0x1181a084 (MassEffect2.exe)
    /// - "UBioFaction_Player" @ 0x1181a0ec (MassEffect2.exe)
    /// - "UBioFaction_Hacked" @ 0x1181a09c (MassEffect2.exe)
    /// - "UBioFaction_Vehicle" @ 0x1181a0c4 (MassEffect2.exe)
    /// - "EBioFactionTypes" @ 0x11a05a5c (MassEffect2.exe)
    /// - "SquadFaction" @ 0x1189c474 (MassEffect2.exe)
    /// - Infinity engines use BioFaction enum/types for faction determination
    /// - Faction relationships may be determined by squad membership, plot flags, or other game state
    /// </remarks>
    public static class InfinityFactions
    {
        /// <summary>
        /// Player faction (UBioFaction_Player).
        /// </summary>
        public const int Player = 1;

        /// <summary>
        /// Hostile faction (enemies).
        /// </summary>
        public const int Hostile = 2;

        /// <summary>
        /// Neutral faction (non-combatants).
        /// </summary>
        public const int Neutral = 3;

        /// <summary>
        /// Hacked faction (UBioFaction_Hacked - hacked/controlled entities).
        /// </summary>
        public const int Hacked = 4;

        /// <summary>
        /// Vehicle faction (UBioFaction_Vehicle - vehicles).
        /// </summary>
        public const int Vehicle = 5;

        /// <summary>
        /// Squad faction (SquadFaction - squad members).
        /// </summary>
        public const int Squad = 6;
    }

    /// <summary>
    /// Manages faction relationships and hostility for Infinity engine (Mass Effect, Mass Effect 2).
    /// </summary>
    /// <remarks>
    /// Infinity Faction Manager System:
    /// - Based on MassEffect.exe and MassEffect2.exe faction systems
    /// - Located via string references: "intABioBaseSquadexecGetFaction" @ 0x11806a44 (MassEffect2.exe)
    /// - "intABioBaseSquadexecSetFaction" @ 0x11806c20 (MassEffect2.exe)
    /// - "intABioBaseSquadexecFactionRelationship" @ 0x11806ad0 (MassEffect2.exe)
    /// - "intABioPawnexecFactionRelationship" @ 0x118099c8 (MassEffect2.exe)
    /// - "OnFactionChanged" @ 0x11816fc8 (MassEffect2.exe)
    /// - Error: "GetFaction: Error returning default netural for faction" @ 0x1189c518 (MassEffect2.exe)
    /// - Original implementation: FactionId references BioFaction type
    /// - Infinity engines use BioFaction enum/types for faction determination
    /// - Faction relationships may be determined by squad membership, plot flags, or other game state
    /// - Personal reputation: Individual entity overrides (stored per entity pair, overrides faction reputation)
    /// - Temporary hostility: Combat-triggered hostility (cleared on combat end or entity death)
    ///
    /// Reputation values (0-100 range, consistent with other engines):
    /// - 0-10: Hostile (will attack on sight)
    /// - 11-89: Neutral (will not attack, but not friendly)
    /// - 90-100: Friendly (allied, will assist in combat)
    ///
    /// Infinity-specific behavior:
    /// - Squad members are always friendly to each other
    /// - Hacked entities may have special faction relationships
    /// - Vehicles may have different faction rules
    /// - Plot flags and script events can modify faction relationships dynamically
    /// </remarks>
    public class InfinityFactionManager
    {
        private readonly IWorld _world;

        // Faction to faction reputation matrix
        // _factionReputation[source][target] = reputation (0-100)
        private readonly Dictionary<int, Dictionary<int, int>> _factionReputation;

        // Personal reputation overrides (creature to creature)
        // _personalReputation[sourceId][targetId] = reputation
        private readonly Dictionary<uint, Dictionary<uint, int>> _personalReputation;

        // Temporary hostility flags (cleared on combat end)
        private readonly Dictionary<uint, HashSet<uint>> _temporaryHostility;

        // Squad membership tracking (Infinity-specific)
        // _squadMembers[entityId] = true if entity is in player's squad
        private readonly HashSet<uint> _squadMembers;

        /// <summary>
        /// Threshold below which factions are hostile.
        /// </summary>
        public const int HostileThreshold = 10;

        /// <summary>
        /// Threshold above which factions are friendly.
        /// </summary>
        public const int FriendlyThreshold = 90;

        /// <summary>
        /// Initializes a new instance of the Infinity faction manager.
        /// </summary>
        /// <param name="world">The world this faction manager belongs to.</param>
        public InfinityFactionManager(IWorld world)
        {
            _world = world ?? throw new ArgumentNullException("world");
            _factionReputation = new Dictionary<int, Dictionary<int, int>>();
            _personalReputation = new Dictionary<uint, Dictionary<uint, int>>();
            _temporaryHostility = new Dictionary<uint, HashSet<uint>>();
            _squadMembers = new HashSet<uint>();

            // Initialize default faction relationships
            InitializeDefaultFactions();
        }

        /// <summary>
        /// Initializes default faction relationships for Infinity engines.
        /// </summary>
        /// <remarks>
        /// Infinity engines (MassEffect.exe, MassEffect2.exe) use BioFaction types.
        /// Default relationships ensure basic hostility/friendliness behavior.
        /// Based on MassEffect2.exe: intABioBaseSquadexecFactionRelationship @ 0x11806ad0
        /// </remarks>
        private void InitializeDefaultFactions()
        {
            // Player faction (friendly to squad, neutral to most, hostile to enemies)
            SetFactionReputation(InfinityFactions.Player, InfinityFactions.Player, 100);
            SetFactionReputation(InfinityFactions.Player, InfinityFactions.Squad, 100);
            SetFactionReputation(InfinityFactions.Player, InfinityFactions.Hostile, 0);
            SetFactionReputation(InfinityFactions.Player, InfinityFactions.Neutral, 50);
            SetFactionReputation(InfinityFactions.Player, InfinityFactions.Hacked, 50);
            SetFactionReputation(InfinityFactions.Player, InfinityFactions.Vehicle, 100);

            // Hostile faction (hostile to everyone except themselves)
            SetFactionReputation(InfinityFactions.Hostile, InfinityFactions.Hostile, 100);
            SetFactionReputation(InfinityFactions.Hostile, InfinityFactions.Player, 0);
            SetFactionReputation(InfinityFactions.Hostile, InfinityFactions.Squad, 0);
            SetFactionReputation(InfinityFactions.Hostile, InfinityFactions.Neutral, 0);
            SetFactionReputation(InfinityFactions.Hostile, InfinityFactions.Hacked, 0);
            SetFactionReputation(InfinityFactions.Hostile, InfinityFactions.Vehicle, 0);

            // Neutral faction (neutral to most)
            SetFactionReputation(InfinityFactions.Neutral, InfinityFactions.Neutral, 100);
            SetFactionReputation(InfinityFactions.Neutral, InfinityFactions.Player, 50);
            SetFactionReputation(InfinityFactions.Neutral, InfinityFactions.Squad, 50);
            SetFactionReputation(InfinityFactions.Neutral, InfinityFactions.Hostile, 0);
            SetFactionReputation(InfinityFactions.Neutral, InfinityFactions.Hacked, 50);
            SetFactionReputation(InfinityFactions.Neutral, InfinityFactions.Vehicle, 50);

            // Hacked faction (controlled entities - friendly to player, neutral to most)
            SetFactionReputation(InfinityFactions.Hacked, InfinityFactions.Hacked, 100);
            SetFactionReputation(InfinityFactions.Hacked, InfinityFactions.Player, 100);
            SetFactionReputation(InfinityFactions.Hacked, InfinityFactions.Squad, 100);
            SetFactionReputation(InfinityFactions.Hacked, InfinityFactions.Hostile, 0);
            SetFactionReputation(InfinityFactions.Hacked, InfinityFactions.Neutral, 50);
            SetFactionReputation(InfinityFactions.Hacked, InfinityFactions.Vehicle, 50);

            // Vehicle faction (vehicles - friendly to player and squad)
            SetFactionReputation(InfinityFactions.Vehicle, InfinityFactions.Vehicle, 100);
            SetFactionReputation(InfinityFactions.Vehicle, InfinityFactions.Player, 100);
            SetFactionReputation(InfinityFactions.Vehicle, InfinityFactions.Squad, 100);
            SetFactionReputation(InfinityFactions.Vehicle, InfinityFactions.Hostile, 0);
            SetFactionReputation(InfinityFactions.Vehicle, InfinityFactions.Neutral, 50);
            SetFactionReputation(InfinityFactions.Vehicle, InfinityFactions.Hacked, 50);

            // Squad faction (squad members - always friendly to each other and player)
            SetFactionReputation(InfinityFactions.Squad, InfinityFactions.Squad, 100);
            SetFactionReputation(InfinityFactions.Squad, InfinityFactions.Player, 100);
            SetFactionReputation(InfinityFactions.Squad, InfinityFactions.Hostile, 0);
            SetFactionReputation(InfinityFactions.Squad, InfinityFactions.Neutral, 50);
            SetFactionReputation(InfinityFactions.Squad, InfinityFactions.Hacked, 50);
            SetFactionReputation(InfinityFactions.Squad, InfinityFactions.Vehicle, 100);
        }

        /// <summary>
        /// Gets the base reputation between two factions.
        /// </summary>
        /// <param name="sourceFaction">The source faction ID (BioFaction type).</param>
        /// <param name="targetFaction">The target faction ID (BioFaction type).</param>
        /// <returns>Reputation value (0-100).</returns>
        /// <remarks>
        /// Based on MassEffect2.exe: intABioBaseSquadexecFactionRelationship @ 0x11806ad0
        /// </remarks>
        public int GetFactionReputation(int sourceFaction, int targetFaction)
        {
            if (sourceFaction == targetFaction)
            {
                return 100; // Same faction always friendly
            }

            Dictionary<int, int> targetReps;
            if (_factionReputation.TryGetValue(sourceFaction, out targetReps))
            {
                int rep;
                if (targetReps.TryGetValue(targetFaction, out rep))
                {
                    return rep;
                }
            }

            return 50; // Default neutral
        }

        /// <summary>
        /// Sets the base reputation between two factions.
        /// </summary>
        /// <param name="sourceFaction">The source faction ID (BioFaction type).</param>
        /// <param name="targetFaction">The target faction ID (BioFaction type).</param>
        /// <param name="reputation">The reputation value (0-100, clamped).</param>
        /// <remarks>
        /// Based on MassEffect2.exe: intABioBaseSquadexecSetFaction @ 0x11806c20
        /// </remarks>
        public void SetFactionReputation(int sourceFaction, int targetFaction, int reputation)
        {
            reputation = Math.Max(0, Math.Min(100, reputation));

            if (!_factionReputation.ContainsKey(sourceFaction))
            {
                _factionReputation[sourceFaction] = new Dictionary<int, int>();
            }
            _factionReputation[sourceFaction][targetFaction] = reputation;
        }

        /// <summary>
        /// Adjusts the reputation between two factions.
        /// </summary>
        /// <param name="sourceFaction">The source faction ID (BioFaction type).</param>
        /// <param name="targetFaction">The target faction ID (BioFaction type).</param>
        /// <param name="adjustment">The adjustment value (can be negative).</param>
        public void AdjustFactionReputation(int sourceFaction, int targetFaction, int adjustment)
        {
            int current = GetFactionReputation(sourceFaction, targetFaction);
            SetFactionReputation(sourceFaction, targetFaction, current + adjustment);
        }

        /// <summary>
        /// Gets the effective reputation between two entities.
        /// </summary>
        /// <param name="source">The source entity.</param>
        /// <param name="target">The target entity.</param>
        /// <returns>Reputation value (0-100).</returns>
        /// <remarks>
        /// Based on MassEffect2.exe: intABioPawnexecFactionRelationship @ 0x118099c8
        /// Checks squad membership first (Infinity-specific), then temporary hostility, personal reputation, and faction reputation.
        /// </remarks>
        public int GetReputation(IEntity source, IEntity target)
        {
            if (source == null || target == null)
            {
                return 50;
            }

            if (source == target)
            {
                return 100; // Self
            }

            // Infinity-specific: Check squad membership first
            // Squad members are always friendly to each other
            if (_squadMembers.Contains(source.ObjectId) && _squadMembers.Contains(target.ObjectId))
            {
                return 100; // Squad members are always friendly
            }

            // Check temporary hostility
            if (IsTemporarilyHostile(source, target))
            {
                return 0;
            }

            // Check personal reputation override
            Dictionary<uint, int> personalReps;
            if (_personalReputation.TryGetValue(source.ObjectId, out personalReps))
            {
                int personalRep;
                if (personalReps.TryGetValue(target.ObjectId, out personalRep))
                {
                    return personalRep;
                }
            }

            // Fall back to faction reputation
            IFactionComponent sourceFaction = source.GetComponent<IFactionComponent>();
            IFactionComponent targetFaction = target.GetComponent<IFactionComponent>();

            int sourceFactionId = sourceFaction != null ? sourceFaction.FactionId : 0;
            int targetFactionId = targetFaction != null ? targetFaction.FactionId : 0;

            return GetFactionReputation(sourceFactionId, targetFactionId);
        }

        /// <summary>
        /// Sets personal reputation between two entities.
        /// </summary>
        /// <param name="source">The source entity.</param>
        /// <param name="target">The target entity.</param>
        /// <param name="reputation">The reputation value (0-100, clamped).</param>
        public void SetPersonalReputation(IEntity source, IEntity target, int reputation)
        {
            if (source == null || target == null)
            {
                return;
            }

            reputation = Math.Max(0, Math.Min(100, reputation));

            if (!_personalReputation.ContainsKey(source.ObjectId))
            {
                _personalReputation[source.ObjectId] = new Dictionary<uint, int>();
            }
            _personalReputation[source.ObjectId][target.ObjectId] = reputation;
        }

        /// <summary>
        /// Clears personal reputation between two entities.
        /// </summary>
        /// <param name="source">The source entity.</param>
        /// <param name="target">The target entity.</param>
        public void ClearPersonalReputation(IEntity source, IEntity target)
        {
            if (source == null || target == null)
            {
                return;
            }

            Dictionary<uint, int> personalReps;
            if (_personalReputation.TryGetValue(source.ObjectId, out personalReps))
            {
                personalReps.Remove(target.ObjectId);
            }
        }

        /// <summary>
        /// Checks if source is hostile to target.
        /// </summary>
        /// <param name="source">The source entity.</param>
        /// <param name="target">The target entity.</param>
        /// <returns>True if hostile, false otherwise.</returns>
        /// <remarks>
        /// Based on MassEffect2.exe: intABioBaseSquadexecFactionRelationship @ 0x11806ad0
        /// </remarks>
        public bool IsHostile(IEntity source, IEntity target)
        {
            return GetReputation(source, target) <= HostileThreshold;
        }

        /// <summary>
        /// Checks if source is friendly to target.
        /// </summary>
        /// <param name="source">The source entity.</param>
        /// <param name="target">The target entity.</param>
        /// <returns>True if friendly, false otherwise.</returns>
        public bool IsFriendly(IEntity source, IEntity target)
        {
            return GetReputation(source, target) >= FriendlyThreshold;
        }

        /// <summary>
        /// Checks if source is neutral to target.
        /// </summary>
        /// <param name="source">The source entity.</param>
        /// <param name="target">The target entity.</param>
        /// <returns>True if neutral, false otherwise.</returns>
        public bool IsNeutral(IEntity source, IEntity target)
        {
            int rep = GetReputation(source, target);
            return rep > HostileThreshold && rep < FriendlyThreshold;
        }

        /// <summary>
        /// Sets temporary hostility between two entities.
        /// </summary>
        /// <param name="source">The source entity.</param>
        /// <param name="target">The target entity.</param>
        /// <param name="hostile">True to set as hostile, false to clear hostility.</param>
        public void SetTemporaryHostile(IEntity source, IEntity target, bool hostile)
        {
            if (source == null || target == null)
            {
                return;
            }

            if (!_temporaryHostility.ContainsKey(source.ObjectId))
            {
                _temporaryHostility[source.ObjectId] = new HashSet<uint>();
            }

            if (hostile)
            {
                _temporaryHostility[source.ObjectId].Add(target.ObjectId);
            }
            else
            {
                _temporaryHostility[source.ObjectId].Remove(target.ObjectId);
            }
        }

        /// <summary>
        /// Checks if source is temporarily hostile to target.
        /// </summary>
        /// <param name="source">The source entity.</param>
        /// <param name="target">The target entity.</param>
        /// <returns>True if temporarily hostile, false otherwise.</returns>
        public bool IsTemporarilyHostile(IEntity source, IEntity target)
        {
            if (source == null || target == null)
            {
                return false;
            }

            HashSet<uint> targets;
            if (_temporaryHostility.TryGetValue(source.ObjectId, out targets))
            {
                return targets.Contains(target.ObjectId);
            }
            return false;
        }

        /// <summary>
        /// Clears all temporary hostility for an entity.
        /// </summary>
        /// <param name="entity">The entity to clear temporary hostility for.</param>
        public void ClearTemporaryHostility(IEntity entity)
        {
            if (entity == null)
            {
                return;
            }

            _temporaryHostility.Remove(entity.ObjectId);
        }

        /// <summary>
        /// Clears all temporary hostility in the world.
        /// </summary>
        public void ClearAllTemporaryHostility()
        {
            _temporaryHostility.Clear();
        }

        /// <summary>
        /// Adds an entity to the squad (Infinity-specific).
        /// </summary>
        /// <param name="entity">The entity to add to the squad.</param>
        /// <remarks>
        /// Based on MassEffect2.exe: SquadFaction @ 0x1189c474
        /// Squad members are always friendly to each other and the player.
        /// </remarks>
        public void AddToSquad(IEntity entity)
        {
            if (entity != null)
            {
                _squadMembers.Add(entity.ObjectId);
            }
        }

        /// <summary>
        /// Removes an entity from the squad (Infinity-specific).
        /// </summary>
        /// <param name="entity">The entity to remove from the squad.</param>
        public void RemoveFromSquad(IEntity entity)
        {
            if (entity != null)
            {
                _squadMembers.Remove(entity.ObjectId);
            }
        }

        /// <summary>
        /// Checks if an entity is in the squad (Infinity-specific).
        /// </summary>
        /// <param name="entity">The entity to check.</param>
        /// <returns>True if in squad, false otherwise.</returns>
        public bool IsInSquad(IEntity entity)
        {
            return entity != null && _squadMembers.Contains(entity.ObjectId);
        }

        /// <summary>
        /// Processes an attack event, updating faction relationships.
        /// </summary>
        /// <param name="attacker">The attacking entity.</param>
        /// <param name="target">The target entity being attacked.</param>
        /// <remarks>
        /// Based on MassEffect.exe and MassEffect2.exe: Combat triggers hostility
        /// Sets temporary hostility and optionally propagates to faction members.
        /// </remarks>
        public void OnAttack(IEntity attacker, IEntity target)
        {
            if (attacker == null || target == null)
            {
                return;
            }

            // Set temporary hostility
            SetTemporaryHostile(target, attacker, true);

            // Optionally propagate to faction members
            IFactionComponent targetFaction = target.GetComponent<IFactionComponent>();
            if (targetFaction != null)
            {
                // Make the entire target faction hostile to attacker
                foreach (IEntity entity in _world.GetAllEntities())
                {
                    IFactionComponent entityFaction = entity.GetComponent<IFactionComponent>();
                    if (entityFaction != null && entityFaction.FactionId == targetFaction.FactionId)
                    {
                        SetTemporaryHostile(entity, attacker, true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all entities hostile to the given entity.
        /// </summary>
        /// <param name="entity">The entity to check.</param>
        /// <returns>Enumerable of hostile entities.</returns>
        public IEnumerable<IEntity> GetHostileEntities(IEntity entity)
        {
            if (entity == null)
            {
                yield break;
            }

            foreach (IEntity other in _world.GetAllEntities())
            {
                if (other != entity && IsHostile(other, entity))
                {
                    yield return other;
                }
            }
        }

        /// <summary>
        /// Gets all entities friendly to the given entity.
        /// </summary>
        /// <param name="entity">The entity to check.</param>
        /// <returns>Enumerable of friendly entities.</returns>
        public IEnumerable<IEntity> GetFriendlyEntities(IEntity entity)
        {
            if (entity == null)
            {
                yield break;
            }

            foreach (IEntity other in _world.GetAllEntities())
            {
                if (other != entity && IsFriendly(other, entity))
                {
                    yield return other;
                }
            }
        }
    }
}

