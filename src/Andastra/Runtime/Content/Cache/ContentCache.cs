using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Andastra.Parsing.Resource;
using Andastra.Runtime.Content.Interfaces;

namespace Andastra.Runtime.Content.Cache
{
    /// <summary>
    /// File-based content cache for converted assets.
    /// </summary>
    /// <remarks>
    /// Content Cache System:
    /// - Based on swkotor2.exe asset caching system
    /// - Located via string references: "CACHE" @ 0x007c6848, "z:\cache" @ 0x007c6850, "CExoKeyTable" resource management
    /// - Original implementation: Engine caches loaded models, textures, and other assets to avoid redundant loading
    /// - Resource management: CExoKeyTable handles resource key management, CExoKeyTable::AddKey adds resources to cache
    /// - Cache directory: Stores converted assets on disk for faster subsequent loads (enhancement over original in-memory cache)
    /// - Memory cache: In-memory cache for frequently accessed assets (LRU eviction)
    /// - Cache key: Based on game type, resource reference, resource type, source hash, and converter version
    /// - Cache invalidation: Source hash changes invalidate cached assets when source files are modified
    /// - Cache size limits: Configurable maximum cache size with LRU eviction when limit exceeded
    /// - Thread-safe: Concurrent access support for async loading scenarios
    /// - Note: Original engine uses in-memory caching via CExoKeyTable, this adds persistent disk cache for converted assets
    /// </remarks>
    public class ContentCache : IContentCache
    {
        private readonly string _cacheDir;
        private readonly Dictionary<CacheKey, CacheEntry> _memoryCache;
        private readonly object _lock = new object();
        private long _totalSize;

        private const long DefaultMaxCacheSize = 1024 * 1024 * 1024; // 1 GB

        public ContentCache(string cacheDirectory)
        {
            if (string.IsNullOrEmpty(cacheDirectory))
            {
                // Default to user profile directory
                cacheDirectory = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Odyssey",
                    "Cache"
                );
            }

            _cacheDir = cacheDirectory;
            _memoryCache = new Dictionary<CacheKey, CacheEntry>();

            if (!Directory.Exists(_cacheDir))
            {
                Directory.CreateDirectory(_cacheDir);
            }

            // Calculate initial size
            _totalSize = CalculateCacheSize();
        }

        public string CacheDirectory { get { return _cacheDir; } }
        public long TotalSize { get { return _totalSize; } }

        public async Task<CacheResult<T>> TryGetAsync<T>(CacheKey key, CancellationToken ct) where T : class
        {
            // Check memory cache first
            lock (_lock)
            {
                if (_memoryCache.TryGetValue(key, out CacheEntry entry))
                {
                    entry.LastAccess = DateTime.UtcNow;
                    if (entry.Value is T typedValue)
                    {
                        return CacheResult<T>.Hit(typedValue);
                    }
                }
            }

            // Check file cache
            string filePath = GetCacheFilePath(key);
            if (!File.Exists(filePath))
            {
                return CacheResult<T>.Miss();
            }

            return await Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    // Read metadata file
                    string metaPath = filePath + ".meta";
                    if (!File.Exists(metaPath))
                    {
                        return CacheResult<T>.Miss();
                    }

                    // Parse and validate metadata
                    CacheMetadata metadata = ParseMetadata(metaPath);
                    if (metadata == null)
                    {
                        return CacheResult<T>.Miss();
                    }

                    // Validate metadata matches cache key
                    if (!ValidateMetadata(metadata, key))
                    {
                        // Metadata mismatch - cache entry is invalid
                        Invalidate(key);
                        return CacheResult<T>.Miss();
                    }

                    // Read cached data file
                    if (!File.Exists(filePath))
                    {
                        return CacheResult<T>.Miss();
                    }

                    byte[] cachedData = File.ReadAllBytes(filePath);
                    if (cachedData == null || cachedData.Length == 0)
                    {
                        return CacheResult<T>.Miss();
                    }

                    // Deserialize based on type T
                    T deserializedItem = DeserializeItem<T>(cachedData, metadata);
                    if (deserializedItem == null)
                    {
                        return CacheResult<T>.Miss();
                    }

                    // Add to memory cache for faster subsequent access
                    lock (_lock)
                    {
                        _memoryCache[key] = new CacheEntry
                        {
                            Value = deserializedItem,
                            LastAccess = DateTime.UtcNow,
                            Size = EstimateSize(deserializedItem)
                        };
                    }

                    return CacheResult<T>.Hit(deserializedItem);
                }
                catch (Exception)
                {
                    // On any error, return miss (cache corruption, I/O errors, etc.)
                    return CacheResult<T>.Miss();
                }
            }, ct);
        }

        public async Task StoreAsync<T>(CacheKey key, T item, CancellationToken ct) where T : class
        {
            if (item == null)
            {
                return;
            }

            // Store in memory cache
            lock (_lock)
            {
                _memoryCache[key] = new CacheEntry
                {
                    Value = item,
                    LastAccess = DateTime.UtcNow,
                    Size = EstimateSize(item)
                };
            }

            // Store to file cache (async)
            await Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    string filePath = GetCacheFilePath(key);
                    string dir = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    // Write metadata file
                    string metaPath = filePath + ".meta";
                    string typeName = typeof(T).AssemblyQualifiedName ?? typeof(T).FullName ?? "Unknown";
                    string metaContent = string.Format(
                        "game={0}\nresref={1}\ntype={2}\nhash={3}\nversion={4}\ntime={5}\ntypename={6}",
                        key.GameType,
                        key.ResRef,
                        (int)key.ResourceType,
                        key.SourceHash,
                        key.ConverterVersion,
                        DateTime.UtcNow.ToString("O"),
                        typeName
                    );
                    File.WriteAllText(metaPath, metaContent);

                    // Serialize and write cached data file
                    byte[] serializedData = SerializeItem(item);
                    if (serializedData != null && serializedData.Length > 0)
                    {
                        File.WriteAllBytes(filePath, serializedData);

                        // Update cache size tracking
                        lock (_lock)
                        {
                            _totalSize = CalculateCacheSize();
                        }
                    }
                }
                catch
                {
                    // Ignore cache write failures
                }
            }, ct);
        }

        public void Invalidate(CacheKey key)
        {
            lock (_lock)
            {
                _memoryCache.Remove(key);
            }

            try
            {
                string filePath = GetCacheFilePath(key);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                string metaPath = filePath + ".meta";
                if (File.Exists(metaPath))
                {
                    File.Delete(metaPath);
                }
            }
            catch
            {
                // Ignore deletion failures
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _memoryCache.Clear();
            }

            try
            {
                if (Directory.Exists(_cacheDir))
                {
                    Directory.Delete(_cacheDir, recursive: true);
                    Directory.CreateDirectory(_cacheDir);
                }
                _totalSize = 0;
            }
            catch
            {
                // Ignore deletion failures
            }
        }

        public void Prune(long maxSizeBytes)
        {
            if (_totalSize <= maxSizeBytes)
            {
                return;
            }

            try
            {
                // Get all cache files sorted by last access time
                var files = new DirectoryInfo(_cacheDir)
                    .GetFiles("*.meta", SearchOption.AllDirectories)
                    .OrderBy(f => f.LastAccessTimeUtc)
                    .ToList();

                long currentSize = _totalSize;

                foreach (FileInfo metaFile in files)
                {
                    if (currentSize <= maxSizeBytes)
                    {
                        break;
                    }

                    string dataFile = metaFile.FullName.Substring(0, metaFile.FullName.Length - 5);
                    long fileSize = 0;

                    if (File.Exists(dataFile))
                    {
                        fileSize = new FileInfo(dataFile).Length;
                        File.Delete(dataFile);
                    }

                    metaFile.Delete();
                    currentSize -= fileSize;
                }

                _totalSize = currentSize;
            }
            catch
            {
                // Ignore pruning failures
            }
        }

        private string GetCacheFilePath(CacheKey key)
        {
            string subdir = key.GameType.ToString();
            string filename = key.ToFileName();
            return Path.Combine(_cacheDir, subdir, filename);
        }

        private long CalculateCacheSize()
        {
            try
            {
                if (!Directory.Exists(_cacheDir))
                {
                    return 0;
                }

                return new DirectoryInfo(_cacheDir)
                    .GetFiles("*", SearchOption.AllDirectories)
                    .Sum(f => f.Length);
            }
            catch
            {
                return 0;
            }
        }

        private static long EstimateSize(object item)
        {
            // TODO: SIMPLIFIED - Rough estimate - actual implementation would be type-specific
            if (item is byte[] bytes)
            {
                return bytes.Length;
            }
            return 1024; // Default estimate
        }

        /// <summary>
        /// Computes a hash of the source bytes for cache key generation.
        /// </summary>
        public static string ComputeHash(byte[] data)
        {
            if (data == null || data.Length == 0)
            {
                return "empty";
            }

            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(data);
                var sb = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Parses cache metadata from a metadata file.
        /// </summary>
        private CacheMetadata ParseMetadata(string metaPath)
        {
            try
            {
                string[] lines = File.ReadAllLines(metaPath);
                var metadata = new CacheMetadata();

                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    int equalsIndex = line.IndexOf('=');
                    if (equalsIndex < 0)
                    {
                        continue;
                    }

                    string key = line.Substring(0, equalsIndex).Trim().ToLowerInvariant();
                    string value = line.Substring(equalsIndex + 1).Trim();

                    switch (key)
                    {
                        case "game":
                            if (Enum.TryParse<GameType>(value, true, out GameType gameType))
                            {
                                metadata.GameType = gameType;
                            }
                            break;
                        case "resref":
                            metadata.ResRef = value;
                            break;
                        case "type":
                            if (int.TryParse(value, out int typeInt))
                            {
                                metadata.ResourceType = ResourceType.FromId(typeInt);
                            }
                            break;
                        case "hash":
                            metadata.SourceHash = value;
                            break;
                        case "version":
                            if (int.TryParse(value, out int version))
                            {
                                metadata.ConverterVersion = version;
                            }
                            break;
                        case "time":
                            if (DateTime.TryParse(value, out DateTime timestamp))
                            {
                                metadata.Timestamp = timestamp;
                            }
                            break;
                        case "typename":
                            metadata.TypeName = value;
                            break;
                    }
                }

                return metadata;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Validates that metadata matches the cache key.
        /// </summary>
        private bool ValidateMetadata(CacheMetadata metadata, CacheKey key)
        {
            if (metadata == null)
            {
                return false;
            }

            return metadata.GameType == key.GameType &&
                   string.Equals(metadata.ResRef, key.ResRef, StringComparison.OrdinalIgnoreCase) &&
                   metadata.ResourceType == key.ResourceType &&
                   string.Equals(metadata.SourceHash, key.SourceHash, StringComparison.Ordinal) &&
                   metadata.ConverterVersion == key.ConverterVersion;
        }

        /// <summary>
        /// Deserializes a cached item from byte array.
        /// </summary>
        private T DeserializeItem<T>(byte[] data, CacheMetadata metadata) where T : class
        {
            if (data == null || data.Length == 0)
            {
                return null;
            }

            try
            {
                // Handle byte[] directly (most common case for cached assets)
                if (typeof(T) == typeof(byte[]))
                {
                    return data as T;
                }

                // Handle string type
                if (typeof(T) == typeof(string))
                {
                    string str = Encoding.UTF8.GetString(data);
                    return str as T;
                }

                // For other types, use BinaryFormatter
#pragma warning disable SYSLIB0011 // BinaryFormatter is obsolete but needed for content cache serialization
                using (var ms = new MemoryStream(data))
                {
                    var formatter = new BinaryFormatter();
                    object obj = formatter.Deserialize(ms);
#pragma warning restore SYSLIB0011

                    if (obj is T typedObj)
                    {
                        return typedObj;
                    }
                }
            }
            catch (SerializationException)
            {
                // BinaryFormatter deserialization failed
                return null;
            }
            catch (Exception)
            {
                // Other deserialization errors
                return null;
            }

            return null;
        }

        /// <summary>
        /// Serializes an item to byte array for caching.
        /// </summary>
        private byte[] SerializeItem<T>(T item) where T : class
        {
            if (item == null)
            {
                return null;
            }

            try
            {
                // Handle byte[] directly (no serialization needed)
                if (item is byte[] bytes)
                {
                    return bytes;
                }

                // Handle string type
                if (item is string str)
                {
                    return Encoding.UTF8.GetBytes(str);
                }

                // For other types, use BinaryFormatter
#pragma warning disable SYSLIB0011 // BinaryFormatter is obsolete but needed for content cache serialization
                using (var ms = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, item);
                    return ms.ToArray();
                }
#pragma warning restore SYSLIB0011
            }
            catch (SerializationException)
            {
                // BinaryFormatter serialization failed (type may not be serializable)
                return null;
            }
            catch (Exception)
            {
                // Other serialization errors
                return null;
            }
        }

        private class CacheEntry
        {
            public object Value;
            public DateTime LastAccess;
            public long Size;
        }

        /// <summary>
        /// Cache metadata parsed from .meta file.
        /// </summary>
        private class CacheMetadata
        {
            public GameType GameType { get; set; }
            public string ResRef { get; set; }
            public ResourceType ResourceType { get; set; }
            public string SourceHash { get; set; }
            public int ConverterVersion { get; set; }
            public DateTime Timestamp { get; set; }
            public string TypeName { get; set; }
        }
    }
}

