using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Andastra.Runtime.Core.Actions;
using Andastra.Runtime.Core.Combat;
using Andastra.Runtime.Core.Enums;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Interfaces.Components;
using Andastra.Runtime.Core.Entities;
using Andastra.Game.Games.Common;
using JetBrains.Annotations;
using EffectType = Andastra.Runtime.Core.Combat.EffectType;

namespace Andastra.Runtime.Games.Common
{
    /// <summary>
    /// Unified AI controller system for all BioWare engines (Odyssey, Aurora, Eclipse).
    /// Handles perception, combat behavior, and action queue management for non-player creatures.
    /// </summary>
    /// <remarks>
    /// Unified AI Controller System:
    /// - Supports Odyssey (swkotor.exe/swkotor2.exe), Aurora (nwmain.exe), and Eclipse (daorigins.exe/DragonAge2.exe) engines
    /// - Uses conditional logic based on EngineFamily to handle engine-specific behaviors
    /// - Merged from Odyssey Systems/AIController, Aurora Systems/AuroraAIController, and Eclipse Systems/EclipseAIController
    ///
    /// Engine-specific behavior differences:
    /// - Idle behavior constants (wander radius, intervals) vary by engine
    /// - Perception systems: Aurora uses D20 stealth detection (Listen/Spot vs Move Silently/Hide), others use simpler line-of-sight
    /// - Patrol routes: Odyssey and Aurora support patrol waypoints, Eclipse does not
    /// - Random wandering: Odyssey uses ActionRandomWalk, Aurora/Eclipse use ActionMoveToLocation
    /// - Conversation checking: Eclipse has entity data-based checking, others use base implementation
    ///
    /// Based on verified components of:
    /// - Odyssey: swkotor.exe/swkotor2.exe AI system
    /// - Aurora: nwmain.exe AI system with D20 perception
    /// - Eclipse: daorigins.exe/DragonAge2.exe AI system with UnrealScript integration
    /// </remarks>
    [PublicAPI]
    public class AIControllerSystem
    {
        private readonly IWorld _world;
        private readonly EngineFamily _engineFamily;
        private readonly Action<IEntity, ScriptEvent, IEntity> _fireScriptEvent;
        private readonly Dictionary<IEntity, float> _heartbeatTimers;
        private readonly Dictionary<IEntity, float> _perceptionTimers;
        private readonly Dictionary<IEntity, IdleState> _idleStates;
        private readonly Dictionary<IEntity, float> _idleTimers;
        private readonly Random _random;

        // Constants
        private const float HeartbeatInterval = 6.0f;
        private const float PerceptionUpdateInterval = 0.5f;

        // Engine-specific idle behavior constants
        private readonly float _idleWanderRadius;
        private readonly float _idleWanderInterval;
        private readonly float _idleLookAroundInterval;
        private readonly float _idleAnimationInterval;
        private readonly float _patrolWaitTime;

        public AIControllerSystem([NotNull] IWorld world, EngineFamily engineFamily, Action<IEntity, ScriptEvent, IEntity> fireScriptEvent)
        {
            _world = world ?? throw new ArgumentNullException(nameof(world));
            _engineFamily = engineFamily;
            _fireScriptEvent = fireScriptEvent ?? throw new ArgumentNullException(nameof(fireScriptEvent));
            _heartbeatTimers = new Dictionary<IEntity, float>();
            _perceptionTimers = new Dictionary<IEntity, float>();
            _idleStates = new Dictionary<IEntity, IdleState>();
            _idleTimers = new Dictionary<IEntity, float>();
            _random = new Random();

            // Set engine-specific constants
            if (_engineFamily == EngineFamily.Odyssey)
            {
                _idleWanderRadius = 5.0f;
                _idleWanderInterval = 10.0f;
                _idleLookAroundInterval = 5.0f;
                _idleAnimationInterval = 8.0f;
                _patrolWaitTime = 2.0f;
            }
            else if (_engineFamily == EngineFamily.Aurora)
            {
                _idleWanderRadius = 6.0f;
                _idleWanderInterval = 12.0f;
                _idleLookAroundInterval = 6.0f;
                _idleAnimationInterval = 10.0f;
                _patrolWaitTime = 3.0f;
            }
            else if (_engineFamily == EngineFamily.Eclipse)
            {
                _idleWanderRadius = 4.0f;
                _idleWanderInterval = 8.0f;
                _idleLookAroundInterval = 4.0f;
                _idleAnimationInterval = 6.0f;
                _patrolWaitTime = 0f; // Eclipse doesn't use patrol routes
            }
            else
            {
                // Default values (fallback)
                _idleWanderRadius = 5.0f;
                _idleWanderInterval = 10.0f;
                _idleLookAroundInterval = 5.0f;
                _idleAnimationInterval = 8.0f;
                _patrolWaitTime = 2.0f;
            }
        }

        /// <summary>
        /// Updates AI for all NPCs in the world.
        /// </summary>
        public virtual void Update(float deltaTime)
        {
            if (_world.CurrentArea == null)
            {
                return;
            }

            // Get all creatures in the current area
            var creatures = GetCreaturesInCurrentArea();

            foreach (var entity in creatures)
            {
                UpdateCreatureAI(entity, deltaTime);
            }
        }

        /// <summary>
        /// Gets all creatures in the current area.
        /// </summary>
        private IEnumerable<IEntity> GetCreaturesInCurrentArea()
        {
            return _world.GetEntitiesInRadius(
                System.Numerics.Vector3.Zero,
                float.MaxValue,
                ObjectType.Creature);
        }

        /// <summary>
        /// Updates heartbeat timer for a creature.
        /// </summary>
        private void UpdateHeartbeat(IEntity creature, float deltaTime)
        {
            if (!_heartbeatTimers.ContainsKey(creature))
            {
                _heartbeatTimers[creature] = 0f;
            }

            _heartbeatTimers[creature] += deltaTime;

            if (_heartbeatTimers[creature] >= HeartbeatInterval)
            {
                _heartbeatTimers[creature] = 0f;
                FireHeartbeatScript(creature);
            }
        }

        /// <summary>
        /// Updates perception timer for a creature.
        /// </summary>
        private void UpdatePerception(IEntity creature, float deltaTime)
        {
            if (!_perceptionTimers.ContainsKey(creature))
            {
                _perceptionTimers[creature] = 0f;
            }

            _perceptionTimers[creature] += deltaTime;

            if (_perceptionTimers[creature] >= PerceptionUpdateInterval)
            {
                _perceptionTimers[creature] = 0f;
                CheckPerception(creature);
            }
        }

        /// <summary>
        /// Checks if creature is player-controlled.
        /// </summary>
        private bool IsPlayerControlled(IEntity creature)
        {
            if (creature == null)
            {
                return false;
            }

            // Check if creature has a tag indicating it's the player
            string tag = creature.Tag ?? string.Empty;
            return tag.Equals("Player", StringComparison.OrdinalIgnoreCase) ||
                   tag.Equals("PC", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if creature is in conversation.
        /// </summary>
        private bool IsInConversation(IEntity creature)
        {
            if (_engineFamily == EngineFamily.Eclipse)
            {
                if (creature == null || !creature.IsValid)
                {
                    return false;
                }

                if (creature is Entity concreteEntity)
                {
                    bool inConversation = concreteEntity.GetData<bool>("InConversation", false);
                    if (inConversation)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if creature is in combat.
        /// </summary>
        private bool IsInCombat(IEntity creature)
        {
            if (creature == null || !creature.IsValid)
            {
                return false;
            }

            // Check if creature's HP is below max (recently damaged)
            IStatsComponent stats = creature.GetComponent<IStatsComponent>();
            if (stats != null && stats.CurrentHP < stats.MaxHP)
            {
                return true;
            }

            // Check perception for hostile creatures
            IPerceptionComponent perception = creature.GetComponent<IPerceptionComponent>();
            if (perception != null)
            {
                IFactionComponent faction = creature.GetComponent<IFactionComponent>();
                if (faction != null)
                {
                    // Check all seen objects for hostile creatures
                    foreach (IEntity seenEntity in perception.GetSeenObjects())
                    {
                        if (seenEntity == null || !seenEntity.IsValid)
                        {
                            continue;
                        }

                        if (faction.IsHostile(seenEntity))
                        {
                            IStatsComponent seenStats = seenEntity.GetComponent<IStatsComponent>();
                            if (seenStats != null && seenStats.CurrentHP > 0)
                            {
                                return true;
                            }
                        }
                    }

                    // Check all heard objects for hostile creatures
                    foreach (IEntity heardEntity in perception.GetHeardObjects())
                    {
                        if (heardEntity == null || !heardEntity.IsValid)
                        {
                            continue;
                        }

                        if (faction.IsHostile(heardEntity))
                        {
                            IStatsComponent heardStats = heardEntity.GetComponent<IStatsComponent>();
                            if (heardStats != null && heardStats.CurrentHP > 0)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Fires heartbeat script for a creature.
        /// </summary>
        private void FireHeartbeatScript(IEntity creature)
        {
            IScriptHooksComponent scriptHooks = creature.GetComponent<IScriptHooksComponent>();
            if (scriptHooks != null)
            {
                string heartbeatScript = scriptHooks.GetScript(ScriptEvent.OnHeartbeat);
                if (!string.IsNullOrEmpty(heartbeatScript))
                {
                    _fireScriptEvent(creature, ScriptEvent.OnHeartbeat, null);
                }
            }
        }

        /// <summary>
        /// Checks perception for a creature.
        /// </summary>
        private void CheckPerception(IEntity creature)
        {
            IPerceptionComponent perception = creature.GetComponent<IPerceptionComponent>();
            if (perception == null)
            {
                return;
            }

            ITransformComponent transform = creature.GetComponent<ITransformComponent>();
            if (transform == null)
            {
                return;
            }

            float sightRange = perception.SightRange;
            float hearingRange = perception.HearingRange;
            float maxRange = Math.Max(sightRange, hearingRange);

            var nearbyCreatures = _world.GetEntitiesInRadius(
                transform.Position,
                maxRange,
                ObjectType.Creature);

            foreach (var other in nearbyCreatures)
            {
                if (other == creature || !other.IsValid)
                {
                    continue;
                }

                bool canSee = CanSee(creature, other, sightRange);
                bool canHear = CanHear(creature, other, hearingRange);

                if (canSee || canHear)
                {
                    // Aurora-specific: Update perception component state
                    if (_engineFamily == EngineFamily.Aurora)
                    {
                        perception.UpdatePerception(other, canSee, canHear);
                    }

                    // Fire OnPerception event
                    if (_engineFamily == EngineFamily.Aurora)
                    {
                        // Aurora: Fire only if this is a new detection
                        if ((canSee && !perception.WasSeen(other)) || (canHear && !perception.WasHeard(other)))
                        {
                            _fireScriptEvent(creature, ScriptEvent.OnPerception, other);
                        }
                    }
                    else
                    {
                        // Odyssey/Eclipse: Fire on every detection
                        _fireScriptEvent(creature, ScriptEvent.OnPerception, other);
                    }
                }
            }
        }

        /// <summary>
        /// Checks if subject can see target.
        /// </summary>
        private bool CanSee(IEntity subject, IEntity target, float range)
        {
            ITransformComponent subjectTransform = subject.GetComponent<ITransformComponent>();
            ITransformComponent targetTransform = target.GetComponent<ITransformComponent>();
            if (subjectTransform == null || targetTransform == null)
            {
                return false;
            }

            float distance = Vector3.Distance(subjectTransform.Position, targetTransform.Position);
            if (distance > range)
            {
                return false;
            }

            // Line-of-sight check through navigation mesh (common to all engines)
            if (_world.CurrentArea != null)
            {
                INavigationMesh navMesh = _world.CurrentArea.NavigationMesh;
                if (navMesh != null)
                {
                    Vector3 subjectEye = subjectTransform.Position + Vector3.UnitY * 1.5f;
                    Vector3 targetEye = targetTransform.Position + Vector3.UnitY * 1.5f;

                    if (!navMesh.TestLineOfSight(subjectEye, targetEye))
                    {
                        return false;
                    }
                }
            }

            // Aurora-specific: D20 stealth detection
            if (_engineFamily == EngineFamily.Aurora)
            {
                // Check if target is invisible
                bool targetIsInvisible = _world.EffectSystem.HasEffect(target, EffectType.Invisibility);
                if (targetIsInvisible)
                {
                    bool canSeeInvisible = _world.EffectSystem.HasEffect(subject, EffectType.TrueSeeing);
                    if (!canSeeInvisible)
                    {
                        return false;
                    }
                }

                // Perform stealth detection checks (hearing and sight)
                bool heardTarget = DoListenDetection(subject, target, targetIsInvisible ? 1 : 0);
                bool spottedTarget = DoSpotDetection(subject, target, targetIsInvisible ? 1 : 0);
                return heardTarget || spottedTarget;
            }

            return true;
        }

        /// <summary>
        /// Checks if subject can hear target.
        /// </summary>
        private bool CanHear(IEntity subject, IEntity target, float range)
        {
            ITransformComponent subjectTransform = subject.GetComponent<ITransformComponent>();
            ITransformComponent targetTransform = target.GetComponent<ITransformComponent>();
            if (subjectTransform == null || targetTransform == null)
            {
                return false;
            }

            float distance = Vector3.Distance(subjectTransform.Position, targetTransform.Position);
            return distance <= range;
        }

        /// <summary>
        /// Performs listen-based stealth detection (Aurora-specific: D20 Listen skill check).
        /// </summary>
        private bool DoListenDetection(IEntity subject, IEntity target, int param2)
        {
            if (subject == null || target == null)
            {
                return false;
            }

            IStatsComponent targetStats = target.GetComponent<IStatsComponent>();
            if (targetStats == null)
            {
                return false;
            }

            if (_world.EffectSystem.HasEffect(subject, EffectType.Deafness))
            {
                return false;
            }

            const int SKILL_MOVE_SILENTLY = 8;
            int targetMoveSilently = targetStats.GetSkillRank(SKILL_MOVE_SILENTLY);
            if (targetMoveSilently <= 0)
            {
                return true;
            }

            ITransformComponent subjectTransform = subject.GetComponent<ITransformComponent>();
            ITransformComponent targetTransform = target.GetComponent<ITransformComponent>();
            if (subjectTransform == null || targetTransform == null)
            {
                return false;
            }

            Vector3 subjectPos = subjectTransform.Position;
            Vector3 targetPos = targetTransform.Position;
            float distance = Vector3.Distance(subjectPos, targetPos);

            IPerceptionComponent subjectPerception = subject.GetComponent<IPerceptionComponent>();
            float hearingRange = 20.0f;
            if (subjectPerception != null)
            {
                hearingRange = subjectPerception.HearingRange;
            }

            if (distance > hearingRange)
            {
                return false;
            }

            const int SKILL_LISTEN = 6;
            IStatsComponent subjectStats = subject.GetComponent<IStatsComponent>();
            if (subjectStats == null)
            {
                return false;
            }

            int subjectListen = subjectStats.GetSkillRank(SKILL_LISTEN);
            int listenRoll = _random.Next(1, 21);
            int listenCheck = listenRoll + subjectListen;

            int moveSilentlyRoll = _random.Next(1, 21);
            int moveSilentlyCheck = moveSilentlyRoll + targetMoveSilently;

            return listenCheck >= moveSilentlyCheck;
        }

        /// <summary>
        /// Performs spot-based stealth detection (Aurora-specific: D20 Spot skill check).
        /// </summary>
        private bool DoSpotDetection(IEntity subject, IEntity target, int param2)
        {
            if (subject == null || target == null)
            {
                return false;
            }

            IStatsComponent targetStats = target.GetComponent<IStatsComponent>();
            if (targetStats == null)
            {
                return false;
            }

            ITransformComponent subjectTransform = subject.GetComponent<ITransformComponent>();
            ITransformComponent targetTransform = target.GetComponent<ITransformComponent>();
            if (subjectTransform == null || targetTransform == null)
            {
                return false;
            }

            Vector3 subjectPos = subjectTransform.Position;
            Vector3 targetPos = targetTransform.Position;
            float distance = Vector3.Distance(subjectPos, targetPos);

            IPerceptionComponent subjectPerception = subject.GetComponent<IPerceptionComponent>();
            float sightRange = 30.0f;
            if (subjectPerception != null)
            {
                sightRange = subjectPerception.SightRange;
            }

            if (distance > sightRange)
            {
                return false;
            }

            const int SKILL_HIDE = 7;
            int targetHide = targetStats.GetSkillRank(SKILL_HIDE);
            if (targetHide <= 0)
            {
                return true;
            }

            const int SKILL_SPOT = 5;
            IStatsComponent subjectStats = subject.GetComponent<IStatsComponent>();
            if (subjectStats == null)
            {
                return false;
            }

            int subjectSpot = subjectStats.GetSkillRank(SKILL_SPOT);
            int spotRoll = _random.Next(1, 21);
            int spotCheck = spotRoll + subjectSpot;

            int hideRoll = _random.Next(1, 21);
            int hideCheck = hideRoll + targetHide;

            return spotCheck >= hideCheck;
        }

        /// <summary>
        /// Handles combat AI for a creature.
        /// </summary>
        private void HandleCombatAI(IEntity creature)
        {
            IEntity nearestEnemy = FindNearestEnemy(creature);
            if (nearestEnemy != null)
            {
                IActionQueueComponent actionQueue = creature.GetComponent<IActionQueueComponent>();
                if (actionQueue != null)
                {
                    IAction currentAction = actionQueue.CurrentAction;
                    if (currentAction is ActionAttack)
                    {
                        return;
                    }

                    var attack = new ActionAttack(nearestEnemy.ObjectId);
                    actionQueue.Add(attack);
                }
            }
        }

        /// <summary>
        /// Finds the nearest enemy for a creature.
        /// </summary>
        private IEntity FindNearestEnemy(IEntity creature)
        {
            ITransformComponent transform = creature.GetComponent<ITransformComponent>();
            IFactionComponent faction = creature.GetComponent<IFactionComponent>();
            if (transform == null || faction == null)
            {
                return null;
            }

            var candidates = _world.GetEntitiesInRadius(
                transform.Position,
                50.0f,
                ObjectType.Creature);

            IEntity nearest = null;
            float nearestDistance = float.MaxValue;

            foreach (var candidate in candidates)
            {
                if (candidate == creature || !candidate.IsValid)
                {
                    continue;
                }

                if (!faction.IsHostile(candidate))
                {
                    continue;
                }

                IStatsComponent stats = candidate.GetComponent<IStatsComponent>();
                if (stats != null && stats.CurrentHP <= 0)
                {
                    continue;
                }

                ITransformComponent candidateTransform = candidate.GetComponent<ITransformComponent>();
                if (candidateTransform != null)
                {
                    float distance = Vector3.Distance(transform.Position, candidateTransform.Position);
                    if (distance < nearestDistance)
                    {
                        nearestDistance = distance;
                        nearest = candidate;
                    }
                }
            }

            return nearest;
        }

        /// <summary>
        /// Updates AI for a single creature, including idle behavior timing.
        /// </summary>
        private void UpdateCreatureAI(IEntity creature, float deltaTime)
        {
            // Skip if creature is invalid or is player-controlled
            if (creature == null || !creature.IsValid)
            {
                return;
            }

            // Check if this is a player character (skip AI)
            if (IsPlayerControlled(creature))
            {
                return;
            }

            // Check if creature is in conversation (skip AI during dialogue)
            if (IsInConversation(creature))
            {
                return;
            }

            // Process action queue first
            IActionQueueComponent actionQueue = creature.GetComponent<IActionQueueComponent>();
            if (actionQueue != null && actionQueue.CurrentAction != null)
            {
                // Action queue is processing, let it continue
                return;
            }

            // Update idle timer
            if (!_idleTimers.ContainsKey(creature))
            {
                _idleTimers[creature] = 0f;
            }
            _idleTimers[creature] += deltaTime;

            // Update heartbeat timer
            UpdateHeartbeat(creature, deltaTime);

            // Update perception
            UpdatePerception(creature, deltaTime);

            // Default combat behavior
            if (IsInCombat(creature))
            {
                HandleCombatAI(creature);
            }
            else
            {
                HandleIdleAI(creature);
            }
        }

        /// <summary>
        /// Handles idle AI for a creature.
        /// </summary>
        private void HandleIdleAI(IEntity creature)
        {
            if (creature == null || !creature.IsValid)
            {
                return;
            }

            float deltaTime = _idleTimers.ContainsKey(creature) ? _idleTimers[creature] : 0f;
            if (deltaTime > 0.1f)
            {
                _idleTimers[creature] = 0f;
            }

            if (!_idleStates.TryGetValue(creature, out IdleState idleState))
            {
                idleState = new IdleState
                {
                    SpawnPosition = GetSpawnPosition(creature),
                    LastWanderTime = 0f,
                    LastLookAroundTime = 0f,
                    LastIdleAnimationTime = 0f,
                    PatrolWaypoints = GetPatrolWaypoints(creature),
                    CurrentPatrolIndex = 0
                };
                _idleStates[creature] = idleState;
            }

            IActionQueueComponent actionQueue = creature.GetComponent<IActionQueueComponent>();
            if (actionQueue != null && actionQueue.CurrentAction != null)
            {
                return;
            }

            // Patrol behavior (Odyssey and Aurora only, Eclipse doesn't support patrol routes)
            if ((_engineFamily == EngineFamily.Odyssey || _engineFamily == EngineFamily.Aurora) &&
                idleState.PatrolWaypoints != null && idleState.PatrolWaypoints.Count > 0)
            {
                HandlePatrolBehavior(creature, idleState, actionQueue, deltaTime);
            }
            else
            {
                HandleRandomWanderBehavior(creature, idleState, actionQueue, deltaTime);
            }

            HandleLookAroundBehavior(creature, idleState, actionQueue, deltaTime);
            HandleIdleAnimations(creature, idleState, deltaTime);
        }

        /// <summary>
        /// Handles patrol behavior for a creature following waypoint route (Odyssey and Aurora only).
        /// </summary>
        private void HandlePatrolBehavior(IEntity creature, IdleState idleState, IActionQueueComponent actionQueue, float deltaTime)
        {
            if (actionQueue == null || idleState.PatrolWaypoints == null || idleState.PatrolWaypoints.Count == 0)
            {
                return;
            }

            ITransformComponent transform = creature.GetComponent<ITransformComponent>();
            if (transform == null)
            {
                return;
            }

            IEntity currentWaypoint = idleState.PatrolWaypoints[idleState.CurrentPatrolIndex];
            if (currentWaypoint == null || !currentWaypoint.IsValid)
            {
                idleState.CurrentPatrolIndex = (idleState.CurrentPatrolIndex + 1) % idleState.PatrolWaypoints.Count;
                return;
            }

            ITransformComponent waypointTransform = currentWaypoint.GetComponent<ITransformComponent>();
            if (waypointTransform == null)
            {
                return;
            }

            float distanceToWaypoint = Vector3.Distance(transform.Position, waypointTransform.Position);
            if (distanceToWaypoint < 1.0f)
            {
                if (idleState.PatrolWaitTimer <= 0f)
                {
                    idleState.PatrolWaitTimer = _patrolWaitTime;
                }
                else
                {
                    idleState.PatrolWaitTimer -= deltaTime;
                    if (idleState.PatrolWaitTimer <= 0f)
                    {
                        idleState.CurrentPatrolIndex = (idleState.CurrentPatrolIndex + 1) % idleState.PatrolWaypoints.Count;
                        idleState.PatrolWaitTimer = 0f;
                    }
                }
            }
            else
            {
                if (actionQueue.CurrentAction == null)
                {
                    var moveAction = new ActionMoveToLocation(waypointTransform.Position, false);
                    actionQueue.Add(moveAction);
                }
            }
        }

        /// <summary>
        /// Handles random wandering behavior for a creature.
        /// </summary>
        private void HandleRandomWanderBehavior(IEntity creature, IdleState idleState, IActionQueueComponent actionQueue, float deltaTime)
        {
            if (actionQueue == null)
            {
                return;
            }

            idleState.LastWanderTime += deltaTime;

            if (idleState.LastWanderTime < _idleWanderInterval)
            {
                return;
            }

            if (actionQueue.CurrentAction != null)
            {
                return;
            }

            // Odyssey uses ActionRandomWalk, Aurora/Eclipse use ActionMoveToLocation
            if (_engineFamily == EngineFamily.Odyssey)
            {
                var randomWalkAction = new ActionRandomWalk(_idleWanderRadius, 0f);
                actionQueue.Add(randomWalkAction);
            }
            else
            {
                // Aurora/Eclipse: Generate random destination and use ActionMoveToLocation
                ITransformComponent transform = creature.GetComponent<ITransformComponent>();
                if (transform == null)
                {
                    return;
                }

                float angle = (float)(_random.NextDouble() * Math.PI * 2.0);
                float distance = (float)(_random.NextDouble() * _idleWanderRadius);
                Vector3 offset = new Vector3(
                    (float)Math.Cos(angle) * distance,
                    0f,
                    (float)Math.Sin(angle) * distance
                );
                Vector3 destination = idleState.SpawnPosition + offset;

                if (_world.CurrentArea != null && _world.CurrentArea.NavigationMesh != null)
                {
                    Vector3? projected = _world.CurrentArea.NavigationMesh.ProjectPoint(destination);
                    if (projected.HasValue)
                    {
                        destination = projected.Value;
                    }
                }

                var moveAction = new ActionMoveToLocation(destination, false);
                actionQueue.Add(moveAction);
            }

            idleState.LastWanderTime = 0f;
        }

        /// <summary>
        /// Handles look-around behavior to make NPCs appear more alive.
        /// </summary>
        private void HandleLookAroundBehavior(IEntity creature, IdleState idleState, IActionQueueComponent actionQueue, float deltaTime)
        {
            if (actionQueue == null || actionQueue.CurrentAction != null)
            {
                return;
            }

            idleState.LastLookAroundTime += deltaTime;
            if (idleState.LastLookAroundTime < _idleLookAroundInterval)
            {
                return;
            }

            ITransformComponent transform = creature.GetComponent<ITransformComponent>();
            if (transform == null)
            {
                return;
            }

            float randomAngle = (float)(_random.NextDouble() * Math.PI * 2.0);
            float lookDistance = _engineFamily == EngineFamily.Eclipse ? 2.5f : 3.0f;
            Vector3 lookTarget = transform.Position + new Vector3(
                (float)Math.Cos(randomAngle) * lookDistance,
                0f,
                (float)Math.Sin(randomAngle) * lookDistance
            );

            var lookAction = new ActionMoveToLocation(lookTarget, false);
            actionQueue.Add(lookAction);

            idleState.LastLookAroundTime = 0f;
        }

        /// <summary>
        /// Handles idle animations for a creature.
        /// </summary>
        private void HandleIdleAnimations(IEntity creature, IdleState idleState, float deltaTime)
        {
            IActionQueueComponent actionQueue = creature.GetComponent<IActionQueueComponent>();
            if (actionQueue != null && actionQueue.CurrentAction != null)
            {
                return;
            }

            IAnimationComponent animation = creature.GetComponent<IAnimationComponent>();
            if (animation == null)
            {
                return;
            }

            idleState.LastIdleAnimationTime += deltaTime;

            if (idleState.LastIdleAnimationTime < _idleAnimationInterval)
            {
                return;
            }

            if (animation.CurrentAnimation == -1 || animation.AnimationComplete)
            {
                animation.PlayAnimation(0, 1.0f, true);
            }

            idleState.LastIdleAnimationTime = 0f;
        }

        /// <summary>
        /// Gets the spawn position for a creature (used as wander center).
        /// </summary>
        private Vector3 GetSpawnPosition(IEntity creature)
        {
            ITransformComponent transform = creature.GetComponent<ITransformComponent>();
            if (transform != null)
            {
                return transform.Position;
            }
            return Vector3.Zero;
        }

        /// <summary>
        /// Gets patrol waypoints for a creature (if assigned via tag pattern matching).
        /// Odyssey and Aurora only - Eclipse doesn't support patrol routes.
        /// </summary>
        private List<IEntity> GetPatrolWaypoints(IEntity creature)
        {
            List<IEntity> waypoints = new List<IEntity>();

            if (_world.CurrentArea == null)
            {
                return waypoints;
            }

            string creatureTag = creature.Tag;
            if (string.IsNullOrEmpty(creatureTag))
            {
                return waypoints;
            }

            foreach (IEntity waypoint in _world.CurrentArea.Waypoints)
            {
                if (waypoint == null || !waypoint.IsValid)
                {
                    continue;
                }

                string waypointTag = waypoint.Tag;
                if (!string.IsNullOrEmpty(waypointTag) && waypointTag.StartsWith(creatureTag + "_", StringComparison.OrdinalIgnoreCase))
                {
                    waypoints.Add(waypoint);
                }
            }

            waypoints.Sort((a, b) =>
            {
                string tagA = a.Tag ?? string.Empty;
                string tagB = b.Tag ?? string.Empty;
                return string.Compare(tagA, tagB, StringComparison.OrdinalIgnoreCase);
            });

            return waypoints;
        }


        /// <summary>
        /// Cleans up AI state for a destroyed entity.
        /// </summary>
        public virtual void OnEntityDestroyed(IEntity entity)
        {
            if (entity != null)
            {
                _heartbeatTimers.Remove(entity);
                _perceptionTimers.Remove(entity);
                _idleStates.Remove(entity);
                _idleTimers.Remove(entity);
            }
        }

        /// <summary>
        /// Internal state tracking for idle behavior.
        /// </summary>
        private class IdleState
        {
            public Vector3 SpawnPosition { get; set; }
            public float LastWanderTime { get; set; }
            public float LastLookAroundTime { get; set; }
            public float LastIdleAnimationTime { get; set; }
            public List<IEntity> PatrolWaypoints { get; set; }
            public int CurrentPatrolIndex { get; set; }
            public float PatrolWaitTimer { get; set; }
        }
    }
}
