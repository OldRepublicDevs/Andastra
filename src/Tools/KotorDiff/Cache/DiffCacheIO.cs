using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BioWare;
using BioWare.Common;
using BioWare.Tools;
using JetBrains.Annotations;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Game = BioWare.Common.BioWareGame;

namespace KotorDiff.Cache
{
    /// <summary>
    /// I/O operations for diff cache.
    /// 1:1 port of cache I/O functions from vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/cache.py:114-251
    /// </summary>
    public static class DiffCacheIO
    {
        /// <summary>
        /// Save diff cache to YAML file with companion data directory.
        /// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/cache.py:114-175
        /// </summary>
        public static void SaveDiffCache(
            DiffCache cache,
            string cacheFile,
            string mine,
            string older,
            [CanBeNull] StrRefReferenceCache strrefCache = null,
            [CanBeNull] TwoDAMemoryReferenceCache twodaCache = null,
            [CanBeNull] Action<string> logFunc = null)
        {
            if (logFunc == null)
            {
                logFunc = Console.WriteLine;
            }

            if (strrefCache != null)
            {
                cache.StrrefCacheGame = FormatGame(strrefCache.Game);
                cache.StrrefCacheData = ConvertToObjectDict(strrefCache.ToDict());
            }

            if (twodaCache != null)
            {
                cache.TwodaCacheGame = FormatGame(twodaCache.Game);
                cache.TwodaCacheData = ConvertToObjectDict(twodaCache.ToDict());
            }

            // Create companion data directory
            string cacheDir = Path.Combine(Path.GetDirectoryName(cacheFile), Path.GetFileNameWithoutExtension(cacheFile) + "_data");
            Directory.CreateDirectory(cacheDir);

            string leftDir = Path.Combine(cacheDir, "left");
            string rightDir = Path.Combine(cacheDir, "right");
            Directory.CreateDirectory(leftDir);
            Directory.CreateDirectory(rightDir);

            // Copy modified/different files to cache
            var filesList = cache.Files ?? new List<CachedFileComparison>();
            foreach (var fileComp in filesList)
            {
                if ((fileComp.Status == "modified" || fileComp.Status == "missing_right") && fileComp.LeftExists)
                {
                    string src = Path.Combine(mine, fileComp.RelPath);
                    if (File.Exists(src))
                    {
                        string dst = Path.Combine(leftDir, fileComp.RelPath);
                        Directory.CreateDirectory(Path.GetDirectoryName(dst));
                        File.Copy(src, dst, overwrite: true);
                    }
                }

                if ((fileComp.Status == "modified" || fileComp.Status == "missing_left") && fileComp.RightExists)
                {
                    string src = Path.Combine(older, fileComp.RelPath);
                    if (File.Exists(src))
                    {
                        string dst = Path.Combine(rightDir, fileComp.RelPath);
                        Directory.CreateDirectory(Path.GetDirectoryName(dst));
                        File.Copy(src, dst, overwrite: true);
                    }
                }
            }

            // Save metadata to YAML
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            string yamlContent = serializer.Serialize(cache.ToDict());
            File.WriteAllText(cacheFile, yamlContent, System.Text.Encoding.UTF8);

            logFunc($"Saved diff cache to: {cacheFile}");
            logFunc($"  Cached {filesList.Count} file comparisons");
            logFunc($"  Cache data directory: {cacheDir}");
        }

        /// <summary>
        /// Load diff cache from YAML file.
        /// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/cache.py:178-215
        /// </summary>
        public static (DiffCache cache, string leftDir, string rightDir) LoadDiffCache(
            string cacheFile,
            [CanBeNull] Action<string> logFunc = null)
        {
            if (logFunc == null)
            {
                logFunc = Console.WriteLine;
            }

            // Load metadata from YAML
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            string yamlContent = File.ReadAllText(cacheFile, System.Text.Encoding.UTF8);
            var cacheData = deserializer.Deserialize<Dictionary<string, object>>(yamlContent);
            var cache = DiffCache.FromDict(cacheData);

            // Determine data directory paths
            string cacheDir = Path.Combine(Path.GetDirectoryName(cacheFile), Path.GetFileNameWithoutExtension(cacheFile) + "_data");
            string leftDir = Path.Combine(cacheDir, "left");
            string rightDir = Path.Combine(cacheDir, "right");

            logFunc($"Loaded diff cache from: {cacheFile}");
            logFunc($"  Cached {cache.Files?.Count ?? 0} file comparisons");
            logFunc($"  Original mine: {cache.Mine}");
            logFunc($"  Original older: {cache.Older}");

            return (cache, leftDir, rightDir);
        }

        /// <summary>
        /// Restore StrRef cache from DiffCache.
        /// Matching PyKotor implementation at vendor/PyKotor/Libraries/PyKotor/src/pykotor/tslpatcher/diff/cache.py:218-251
        /// </summary>
        [CanBeNull]
        public static StrRefReferenceCache RestoreStrrefCacheFromCache(
            DiffCache cache,
            Game? game = null)
        {
            if (cache == null || cache.StrrefCacheData == null || cache.StrrefCacheData.Count == 0)
            {
                return null;
            }

            Game resolvedGame = game ?? ParseGame(cache.StrrefCacheGame);
            Dictionary<string, List<Dictionary<string, object>>> typedData = ConvertFromObjectDict(cache.StrrefCacheData);
            if (typedData == null || typedData.Count == 0)
            {
                return null;
            }

            return StrRefReferenceCache.FromDict(resolvedGame, typedData);
        }

        /// <summary>
        /// Restore 2DA memory reference cache from DiffCache.
        /// </summary>
        [CanBeNull]
        public static TwoDAMemoryReferenceCache RestoreTwodaCacheFromCache(
            DiffCache cache,
            Game? game = null)
        {
            if (cache == null || cache.TwodaCacheData == null || cache.TwodaCacheData.Count == 0)
            {
                return null;
            }

            Game resolvedGame = game ?? ParseGame(cache.TwodaCacheGame);
            Dictionary<string, List<Dictionary<string, object>>> typedData = ConvertFromObjectDict(cache.TwodaCacheData);
            if (typedData == null || typedData.Count == 0)
            {
                return null;
            }

            return TwoDAMemoryReferenceCache.FromDict(resolvedGame, typedData);
        }

        public static string FormatGame(Game game)
        {
            if (game == Game.TSL)
            {
                return "k2";
            }

            return "k1";
        }

        internal static Game ParseGame(string gameText)
        {
            if (string.IsNullOrWhiteSpace(gameText))
            {
                return Game.K1;
            }

            string normalized = gameText.Trim().ToLowerInvariant();
            if (normalized == "k2" || normalized == "tsl" || normalized == "kotor2")
            {
                return Game.TSL;
            }

            return Game.K1;
        }

        public static Dictionary<string, object> ConvertToObjectDict(
            Dictionary<string, List<Dictionary<string, object>>> source)
        {
            var result = new Dictionary<string, object>();
            if (source == null)
            {
                return result;
            }

            foreach (KeyValuePair<string, List<Dictionary<string, object>>> kvp in source)
            {
                result[kvp.Key] = kvp.Value;
            }

            return result;
        }

        internal static Dictionary<string, List<Dictionary<string, object>>> ConvertFromObjectDict(
            Dictionary<string, object> source)
        {
            var result = new Dictionary<string, List<Dictionary<string, object>>>();
            if (source == null)
            {
                return result;
            }

            foreach (KeyValuePair<string, object> kvp in source)
            {
                List<Dictionary<string, object>> references = ConvertReferenceList(kvp.Value);
                if (references != null && references.Count > 0)
                {
                    result[kvp.Key] = references;
                }
            }

            return result;
        }

        private static List<Dictionary<string, object>> ConvertReferenceList(object value)
        {
            if (value == null)
            {
                return null;
            }

            var references = new List<Dictionary<string, object>>();
            if (value is IEnumerable<object> objectList)
            {
                foreach (object item in objectList)
                {
                    Dictionary<string, object> converted = ConvertReferenceEntry(item);
                    if (converted != null)
                    {
                        references.Add(converted);
                    }
                }

                return references;
            }

            if (value is System.Collections.IEnumerable enumerable && !(value is string))
            {
                foreach (object item in enumerable)
                {
                    Dictionary<string, object> converted = ConvertReferenceEntry(item);
                    if (converted != null)
                    {
                        references.Add(converted);
                    }
                }
            }

            return references;
        }

        private static Dictionary<string, object> ConvertReferenceEntry(object item)
        {
            if (item is Dictionary<string, object> typedEntry)
            {
                return typedEntry;
            }

            if (item is Dictionary<object, object> looseEntry)
            {
                var converted = new Dictionary<string, object>();
                foreach (KeyValuePair<object, object> kvp in looseEntry)
                {
                    string key = kvp.Key == null ? string.Empty : kvp.Key.ToString();
                    if (key == "locations")
                    {
                        converted[key] = ConvertLocations(kvp.Value);
                    }
                    else
                    {
                        converted[key] = kvp.Value;
                    }
                }

                return converted;
            }

            return null;
        }

        private static List<object> ConvertLocations(object value)
        {
            var locations = new List<object>();
            if (value is IEnumerable<object> objectList)
            {
                locations.AddRange(objectList);
                return locations;
            }

            if (value is System.Collections.IEnumerable enumerable && !(value is string))
            {
                foreach (object item in enumerable)
                {
                    locations.Add(item);
                }
            }

            return locations;
        }
    }
}
