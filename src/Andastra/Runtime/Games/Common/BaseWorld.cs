using System;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using Andastra.Runtime.Core.Interfaces;
using Andastra.Runtime.Core.Enums;

namespace Andastra.Runtime.Games.Common
{
    /// <summary>
    /// Base implementation of world management shared across all BioWare engines.
    /// </summary>
    /// <remarks>
    /// Base World Implementation:
    /// - Common entity container and state manager across all engines
    /// - Provides entity lookup by ID, tag, and type
    /// - Manages area-entity relationships
    ///
    /// Based on reverse engineering of:
    /// - swkotor.exe: Entity management and world systems
    /// - swkotor2.exe: World management with ObjectId @ 0x007bce5c, AreaId @ 0x007bef48
    /// - nwmain.exe: Aurora world management functions
    /// - daorigins.exe: Eclipse world systems
    /// - Common entity structure: ObjectId, Tag, ObjectType, AreaId
    ///
    /// Common functionality across engines:
    /// - Entity registration and lookup by ObjectId (O(1))
    /// - Tag-based lookup (case-insensitive, supports nth occurrence)
    /// - ObjectType-based enumeration
    /// - Area-entity relationships (entities belong to areas)
    /// - Spatial queries (GetEntitiesInRadius)
    /// - Entity lifecycle management (add/remove)
    /// - Serialization support for save/load
    /// </remarks>
    [PublicAPI]
    public abstract class BaseWorld : IWorld
    {
        protected readonly Dictionary<uint, IEntity> _entitiesById = new Dictionary<uint, IEntity>();
        protected readonly Dictionary<string, List<IEntity>> _entitiesByTag = new Dictionary<string, List<IEntity>>(StringComparer.OrdinalIgnoreCase);
        protected readonly Dictionary<ObjectType, List<IEntity>> _entitiesByType = new Dictionary<ObjectType, List<IEntity>>();
        protected readonly IEventBus _eventBus;

        protected BaseWorld(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        /// <summary>
        /// The event bus for this world.
        /// </summary>
        public IEventBus EventBus => _eventBus;

        /// <summary>
        /// Gets an entity by its unique ObjectId.
        /// </summary>
        /// <remarks>
        /// O(1) lookup by ObjectId.
        /// Returns null if entity doesn't exist or is invalid.
        /// Common across all engines.
        /// </remarks>
        public virtual IEntity GetEntity(uint objectId)
        {
            return _entitiesById.TryGetValue(objectId, out var entity) && entity.IsValid ? entity : null;
        }

        /// <summary>
        /// Gets an entity by tag.
        /// </summary>
        /// <remarks>
        /// Case-insensitive tag lookup.
        /// Supports nth occurrence for multiple entities with same tag.
        /// Common across all engines.
        /// </remarks>
        public virtual IEntity GetEntityByTag(string tag, int nth = 0)
        {
            if (string.IsNullOrEmpty(tag) || !_entitiesByTag.TryGetValue(tag, out var entities))
                return null;

            // Find nth valid entity
            int count = 0;
            foreach (var entity in entities)
            {
                if (entity.IsValid)
                {
                    if (count == nth)
                        return entity;
                    count++;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets all entities of a specific type.
        /// </summary>
        /// <remarks>
        /// Returns all valid entities of the specified ObjectType.
        /// Common across all engines.
        /// </remarks>
        public virtual IEnumerable<IEntity> GetEntities(ObjectType objectType)
        {
            if (!_entitiesByType.TryGetValue(objectType, out var entities))
                return Array.Empty<IEntity>();

            return entities.FindAll(e => e.IsValid);
        }

        /// <summary>
        /// Gets entities within a radius of a point.
        /// </summary>
        /// <remarks>
        /// Spatial query for entities near a position.
        /// Engine-specific implementations may use different spatial partitioning.
        /// Common interface across all engines.
        /// </remarks>
        public abstract IEnumerable<IEntity> GetEntitiesInRadius(Vector3 center, float radius, ObjectType objectType = ObjectType.All);

        /// <summary>
        /// Gets all entities in the world.
        /// </summary>
        /// <remarks>
        /// Returns all valid entities regardless of type.
        /// Common across all engines.
        /// </remarks>
        public virtual IEnumerable<IEntity> GetAllEntities()
        {
            foreach (var entity in _entitiesById.Values)
            {
                if (entity.IsValid)
                    yield return entity;
            }
        }

        /// <summary>
        /// Adds an entity to the world.
        /// </summary>
        /// <remarks>
        /// Registers entity in lookup tables.
        /// Sets entity's world reference.
        /// Common across all engines.
        /// </remarks>
        public virtual void AddEntity(IEntity entity)
        {
            if (entity == null || !entity.IsValid)
                return;

            // Add to ID lookup
            _entitiesById[entity.ObjectId] = entity;

            // Add to tag lookup
            if (!string.IsNullOrEmpty(entity.Tag))
            {
                if (!_entitiesByTag.TryGetValue(entity.Tag, out var tagList))
                {
                    tagList = new List<IEntity>();
                    _entitiesByTag[entity.Tag] = tagList;
                }
                tagList.Add(entity);
            }

            // Add to type lookup
            if (!_entitiesByType.TryGetValue(entity.ObjectType, out var typeList))
            {
                typeList = new List<IEntity>();
                _entitiesByType[entity.ObjectType] = typeList;
            }
            typeList.Add(entity);

            // Set world reference
            entity.World = this;

            OnEntityAdded(entity);
        }

        /// <summary>
        /// Removes an entity from the world.
        /// </summary>
        /// <remarks>
        /// Unregisters entity from lookup tables.
        /// Clears entity's world reference.
        /// Common across all engines.
        /// </remarks>
        public virtual void RemoveEntity(IEntity entity)
        {
            if (entity == null)
                return;

            OnEntityRemoving(entity);

            // Remove from ID lookup
            _entitiesById.Remove(entity.ObjectId);

            // Remove from tag lookup
            if (!string.IsNullOrEmpty(entity.Tag) && _entitiesByTag.TryGetValue(entity.Tag, out var tagList))
            {
                tagList.Remove(entity);
                if (tagList.Count == 0)
                    _entitiesByTag.Remove(entity.Tag);
            }

            // Remove from type lookup
            if (_entitiesByType.TryGetValue(entity.ObjectType, out var typeList))
            {
                typeList.Remove(entity);
                if (typeList.Count == 0)
                    _entitiesByType.Remove(entity.ObjectType);
            }

            // Clear world reference
            entity.World = null;
        }

        /// <summary>
        /// Updates the world state.
        /// </summary>
        /// <remarks>
        /// Updates all entities and systems.
        /// Called once per frame.
        /// Engine-specific subclasses implement update logic.
        /// </remarks>
        public abstract void Update(float deltaTime);

        /// <summary>
        /// Called when an entity is added to the world.
        /// </summary>
        /// <remarks>
        /// Hook for engine-specific logic when entities are added.
        /// Subclasses can override for additional setup.
        /// </remarks>
        protected virtual void OnEntityAdded(IEntity entity)
        {
            // Default: no additional logic
        }

        /// <summary>
        /// Called when an entity is about to be removed from the world.
        /// </summary>
        /// <remarks>
        /// Hook for engine-specific cleanup when entities are removed.
        /// Subclasses can override for additional cleanup.
        /// </remarks>
        protected virtual void OnEntityRemoving(IEntity entity)
        {
            // Default: no additional logic
        }

        /// <summary>
        /// Validates world state integrity.
        /// </summary>
        /// <remarks>
        /// Debug function to check lookup table consistency.
        /// Can be called during development to catch bugs.
        /// </remarks>
        public virtual void ValidateIntegrity()
        {
            // Check that all entities in ID lookup are also in type/tag lookups
            foreach (var kvp in _entitiesById)
            {
                var entity = kvp.Value;
                if (!entity.IsValid)
                    continue;

                // Check type lookup
                if (_entitiesByType.TryGetValue(entity.ObjectType, out var typeList))
                {
                    if (!typeList.Contains(entity))
                        throw new InvalidOperationException($"Entity {entity.ObjectId} missing from type lookup");
                }

                // Check tag lookup
                if (!string.IsNullOrEmpty(entity.Tag) && _entitiesByTag.TryGetValue(entity.Tag, out var tagList))
                {
                    if (!tagList.Contains(entity))
                        throw new InvalidOperationException($"Entity {entity.ObjectId} missing from tag lookup");
                }
            }
        }

        /// <summary>
        /// Gets world statistics.
        /// </summary>
        /// <remarks>
        /// Returns counts of entities by type for debugging.
        /// Useful for performance monitoring and debugging.
        /// </remarks>
        public virtual WorldStats GetStats()
        {
            var stats = new WorldStats
            {
                TotalEntities = _entitiesById.Count,
                EntitiesByType = new Dictionary<ObjectType, int>()
            };

            foreach (var kvp in _entitiesByType)
            {
                stats.EntitiesByType[kvp.Key] = kvp.Value.Count(e => e.IsValid);
            }

            return stats;
        }
    }

    /// <summary>
    /// World statistics for debugging and monitoring.
    /// </summary>
    public struct WorldStats
    {
        /// <summary>
        /// Total number of entities in the world.
        /// </summary>
        public int TotalEntities;

        /// <summary>
        /// Number of entities by type.
        /// </summary>
        public Dictionary<ObjectType, int> EntitiesByType;
    }
}
