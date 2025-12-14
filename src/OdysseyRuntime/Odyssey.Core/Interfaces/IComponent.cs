namespace Odyssey.Core.Interfaces
{
    /// <summary>
    /// Base interface for all entity components.
    /// </summary>
    public interface IComponent
    {
        /// <summary>
        /// The entity this component is attached to.
        /// </summary>
        IEntity Owner { get; set; }
        
        /// <summary>
        /// Called when the component is attached to an entity.
        /// </summary>
        void OnAttach();
        
        /// <summary>
        /// Called when the component is detached from an entity.
        /// </summary>
        void OnDetach();
    }
}

