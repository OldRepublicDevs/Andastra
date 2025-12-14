using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
        Task<IReadOnlyList<LocationResult>> LocateAsync(ResourceIdentifier id, SearchLocation[] order, CancellationToken ct);
        
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
    /// Identifies a resource by name and type.
    /// </summary>
    public struct ResourceIdentifier : IEquatable<ResourceIdentifier>
    {
        public readonly string ResRef;
        public readonly ResourceType Type;
        
        public ResourceIdentifier(string resRef, ResourceType type)
        {
            ResRef = resRef?.ToLowerInvariant() ?? string.Empty;
            Type = type;
        }
        
        public bool Equals(ResourceIdentifier other)
        {
            return string.Equals(ResRef, other.ResRef, StringComparison.OrdinalIgnoreCase) && Type == other.Type;
        }
        
        public override bool Equals(object obj)
        {
            return obj is ResourceIdentifier other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return ((ResRef != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ResRef) : 0) * 397) ^ (int)Type;
            }
        }
        
        public override string ToString()
        {
            return ResRef + "." + Type.ToString().ToLowerInvariant();
        }
        
        public static bool operator ==(ResourceIdentifier left, ResourceIdentifier right)
        {
            return left.Equals(right);
        }
        
        public static bool operator !=(ResourceIdentifier left, ResourceIdentifier right)
        {
            return !left.Equals(right);
        }
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
    /// Resource types (extension-based).
    /// </summary>
    public enum ResourceType : ushort
    {
        Invalid = 0,
        BMP = 1,
        TGA = 3,
        WAV = 4,
        PLT = 6,
        INI = 7,
        TXT = 10,
        MDL = 2002,
        MDX = 2003,
        NSS = 2009,
        NCS = 2010,
        ARE = 2012,
        SET = 2013,
        IFO = 2014,
        BIC = 2015,
        WOK = 2016,
        TwoDA = 2017,
        TLK = 2018,
        TXI = 2022,
        GIT = 2023,
        BTI = 2024,
        UTI = 2025,
        BTC = 2026,
        UTC = 2027,
        DLG = 2029,
        ITP = 2030,
        BTT = 2031,
        UTT = 2032,
        DDS = 2033,
        UTS = 2035,
        LTR = 2036,
        GFF = 2037,
        FAC = 2038,
        BTE = 2039,
        UTE = 2040,
        BTD = 2041,
        UTD = 2042,
        BTP = 2043,
        UTP = 2044,
        DTF = 2045,
        GIC = 2046,
        GUI = 2047,
        BTM = 2049,
        UTM = 2050,
        DWK = 2051,
        PWK = 2052,
        BTG = 2053,
        UTW = 2054,
        SSF = 2056,
        NDB = 2064,
        PTM = 2065,
        PTT = 2066,
        LYT = 3000,
        VIS = 3001,
        RIM = 3002,
        PTH = 3003,
        LIP = 3004,
        BWM = 3005,
        TXB = 3006,
        TPC = 3007,
        MDX2 = 3008,
        ERF = 9997,
        BIF = 9998,
        KEY = 9999
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

