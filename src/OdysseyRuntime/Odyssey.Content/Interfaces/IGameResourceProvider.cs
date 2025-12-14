using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CSharpKOTOR.Resources;

namespace Odyssey.Content.Interfaces
{
    /// <summary>
    /// Unified resource access - wraps all precedence logic for KOTOR resource lookup.
    /// </summary>
    public interface IGameResourceProvider
    {
        /// <summary>
        /// Opens a resource stream asynchronously.
        /// </summary>
        Task<Stream> OpenResourceAsync(ResourceIdentifier id, CancellationToken ct);
        
        /// <summary>
        /// Checks if a resource exists without opening it.
        /// </summary>
        Task<bool> ExistsAsync(ResourceIdentifier id, CancellationToken ct);
        
        /// <summary>
        /// Locates a resource across multiple search locations.
        /// </summary>
        Task<IReadOnlyList<Odyssey.Content.Interfaces.LocationResult>> LocateAsync(ResourceIdentifier id, SearchLocation[] order, CancellationToken ct);
        
        /// <summary>
        /// Enumerates all resources of a specific type.
        /// </summary>
        IEnumerable<ResourceIdentifier> EnumerateResources(ResourceType type);
        
        /// <summary>
        /// Gets the raw bytes of a resource.
        /// </summary>
        Task<byte[]> GetResourceBytesAsync(ResourceIdentifier id, CancellationToken ct);
        
        /// <summary>
        /// The game type (K1 or K2).
        /// </summary>
        GameType GameType { get; }
    }
    
    /// <summary>
    /// Result of locating a resource.
    /// </summary>
    public struct LocationResult
    {
        public SearchLocation Location;
        public string Path;
        public long Size;
        public long Offset;
    }
    
    /// <summary>
    /// Search locations for resource lookup, in precedence order.
    /// </summary>
    public enum SearchLocation
    {
        Override,
        Module,
        Save,
        TexturePacks,
        Chitin,
        Hardcoded
    }
    
    /// <summary>
    /// Game type.
    /// </summary>
    public enum GameType
    {
        Unknown,
        K1,
        K2
    }
}

