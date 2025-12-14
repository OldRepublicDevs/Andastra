using System.IO;

namespace Odyssey.Content.Interfaces
{
    /// <summary>
    /// Resource precedence chain element.
    /// </summary>
    public interface IResourceProvider
    {
        /// <summary>
        /// Priority of this provider (higher = checked first).
        /// </summary>
        int Priority { get; }
        
        /// <summary>
        /// Tries to open a resource stream.
        /// </summary>
        bool TryOpen(ResourceIdentifier id, out Stream stream);
        
        /// <summary>
        /// Checks if a resource exists.
        /// </summary>
        bool Exists(ResourceIdentifier id);
        
        /// <summary>
        /// The search location this provider represents.
        /// </summary>
        SearchLocation Location { get; }
    }
}

