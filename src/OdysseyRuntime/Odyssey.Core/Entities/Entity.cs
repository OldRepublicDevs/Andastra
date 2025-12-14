using System;
using System.Collections.Generic;
using Odyssey.Core.Enums;
using Odyssey.Core.Interfaces;

namespace Odyssey.Core.Entities
{
    /// <summary>
    /// Base entity implementation.
    /// </summary>
    public class Entity : IEntity
    {
        private static uint _nextObjectId = 1;
        private readonly Dictionary<Type, IComponent> _components;
        private bool _isValid;

        public Entity(ObjectType objectType, IWorld world)
        {
            ObjectId = _nextObjectId++;
            ObjectType = objectType;
            World = world;
            _components = new Dictionary<Type, IComponent>();
            _isValid = true;
            Tag = string.Empty;
        }

        public uint ObjectId { get; }
        public string Tag { get; set; }
        public ObjectType ObjectType { get; }
        public bool IsValid { get { return _isValid; } }
        public IWorld World { get; }
        
        /// <summary>
        /// The resource reference of the template this entity was spawned from.
        /// </summary>
        public string TemplateResRef { get; set; }

        public T GetComponent<T>() where T : class, IComponent
        {
            if (_components.TryGetValue(typeof(T), out IComponent component))
            {
                return component as T;
            }

            // Check for interface implementations
            foreach (var kvp in _components)
            {
                if (kvp.Value is T typedComponent)
                {
                    return typedComponent;
                }
            }

            return null;
        }

        public void AddComponent<T>(T component) where T : class, IComponent
        {
            if (component == null)
            {
                throw new ArgumentNullException("component");
            }

            Type type = typeof(T);
            if (_components.ContainsKey(type))
            {
                throw new InvalidOperationException("Component of type " + type.Name + " already exists on entity " + ObjectId);
            }

            _components[type] = component;
            component.Owner = this;
            component.OnAttach();
        }

        public bool RemoveComponent<T>() where T : class, IComponent
        {
            Type type = typeof(T);
            if (_components.TryGetValue(type, out IComponent component))
            {
                component.OnDetach();
                component.Owner = null;
                _components.Remove(type);
                return true;
            }
            return false;
        }

        public bool HasComponent<T>() where T : class, IComponent
        {
            if (_components.ContainsKey(typeof(T)))
            {
                return true;
            }

            foreach (var kvp in _components)
            {
                if (kvp.Value is T)
                {
                    return true;
                }
            }

            return false;
        }

        public IEnumerable<IComponent> GetAllComponents()
        {
            return _components.Values;
        }

        /// <summary>
        /// Marks this entity as destroyed.
        /// </summary>
        public void Destroy()
        {
            if (!_isValid)
            {
                return;
            }

            foreach (var component in _components.Values)
            {
                component.OnDetach();
                component.Owner = null;
            }

            _components.Clear();
            _isValid = false;
        }

        /// <summary>
        /// Resets the object ID counter (for testing).
        /// </summary>
        public static void ResetObjectIdCounter()
        {
            _nextObjectId = 1;
        }
    }
}

