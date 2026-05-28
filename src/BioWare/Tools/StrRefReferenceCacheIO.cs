using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BioWare.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BioWare.Tools
{
    /// <summary>
    /// JSON persistence for <see cref="StrRefReferenceCache"/> (KotorCLI find-strref --cache-file).
    /// </summary>
    public static class StrRefReferenceCacheIO
    {
        private sealed class StrRefCacheFilePayload
        {
            public string Game { get; set; }
            public Dictionary<string, object> CacheData { get; set; }
        }

        public static void Save(string cacheFilePath, StrRefReferenceCache cache)
        {
            if (string.IsNullOrWhiteSpace(cacheFilePath))
            {
                throw new ArgumentException("Cache file path is required.", nameof(cacheFilePath));
            }

            if (cache == null)
            {
                throw new ArgumentNullException(nameof(cache));
            }

            string directory = Path.GetDirectoryName(cacheFilePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var payload = new StrRefCacheFilePayload
            {
                Game = FormatGame(cache.Game),
                CacheData = ConvertToObjectDict(cache.ToDict())
            };

            string json = JsonConvert.SerializeObject(payload, Formatting.Indented);
            File.WriteAllText(cacheFilePath, json);
        }

        public static StrRefReferenceCache Load(string cacheFilePath)
        {
            return Load(cacheFilePath, BioWareGame.K1, validateGame: false);
        }

        public static StrRefReferenceCache Load(string cacheFilePath, BioWareGame expectedGame, bool validateGame)
        {
            if (string.IsNullOrWhiteSpace(cacheFilePath))
            {
                throw new ArgumentException("Cache file path is required.", nameof(cacheFilePath));
            }

            if (!File.Exists(cacheFilePath))
            {
                throw new FileNotFoundException("StrRef cache file not found.", cacheFilePath);
            }

            StrRefCacheFilePayload payload = JsonConvert.DeserializeObject<StrRefCacheFilePayload>(File.ReadAllText(cacheFilePath));
            if (payload == null || payload.CacheData == null)
            {
                throw new InvalidDataException("StrRef cache file is empty or invalid.");
            }

            BioWareGame game = ParseGame(payload.Game);
            if (validateGame && expectedGame != game)
            {
                throw new InvalidDataException(
                    "StrRef cache game mismatch: file is " + payload.Game + " but installation is " + FormatGame(expectedGame) + ".");
            }

            Dictionary<string, List<Dictionary<string, object>>> typedData = ConvertFromObjectDict(payload.CacheData);
            return StrRefReferenceCache.FromDict(game, typedData);
        }

        internal static string FormatGame(BioWareGame game)
        {
            return game == BioWareGame.TSL ? "k2" : "k1";
        }

        internal static BioWareGame ParseGame(string gameText)
        {
            if (string.IsNullOrWhiteSpace(gameText))
            {
                return BioWareGame.K1;
            }

            string normalized = gameText.Trim().ToLowerInvariant();
            if (normalized == "k2" || normalized == "tsl" || normalized == "kotor2")
            {
                return BioWareGame.TSL;
            }

            return BioWareGame.K1;
        }

        internal static Dictionary<string, object> ConvertToObjectDict(
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
            if (value is JArray jArray)
            {
                foreach (JToken token in jArray)
                {
                    Dictionary<string, object> converted = ConvertReferenceEntry(token);
                    if (converted != null)
                    {
                        references.Add(converted);
                    }
                }

                return references;
            }

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

            if (value is IEnumerable enumerable && !(value is string))
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
                if (typedEntry.ContainsKey("locations"))
                {
                    var copy = new Dictionary<string, object>(typedEntry);
                    copy["locations"] = ConvertLocationsToStrings(typedEntry["locations"]);
                    return copy;
                }

                return typedEntry;
            }

            if (item is JObject jObject)
            {
                var converted = new Dictionary<string, object>();
                foreach (JProperty property in jObject.Properties())
                {
                    if (property.Name == "locations")
                    {
                        converted[property.Name] = ConvertLocationsToStrings(property.Value);
                    }
                    else
                    {
                        converted[property.Name] = property.Value.Type == JTokenType.String
                            ? property.Value.ToString()
                            : property.Value.ToObject<object>();
                    }
                }

                return converted;
            }

            if (item is Dictionary<object, object> looseEntry)
            {
                var converted = new Dictionary<string, object>();
                foreach (KeyValuePair<object, object> kvp in looseEntry)
                {
                    string key = kvp.Key == null ? string.Empty : kvp.Key.ToString();
                    if (key == "locations")
                    {
                        converted[key] = ConvertLocationsToStrings(kvp.Value);
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

        private static List<string> ConvertLocationsToStrings(object value)
        {
            var locations = new List<string>();
            if (value == null)
            {
                return locations;
            }

            if (value is List<string> stringList)
            {
                locations.AddRange(stringList);
                return locations;
            }

            if (value is JArray jArray)
            {
                foreach (JToken token in jArray)
                {
                    if (token != null)
                    {
                        locations.Add(token.ToString());
                    }
                }

                return locations;
            }

            if (value is IEnumerable<object> objectList)
            {
                foreach (object item in objectList)
                {
                    if (item != null)
                    {
                        locations.Add(item.ToString());
                    }
                }

                return locations;
            }

            if (value is IEnumerable enumerable && !(value is string))
            {
                foreach (object item in enumerable)
                {
                    if (item != null)
                    {
                        locations.Add(item.ToString());
                    }
                }
            }

            return locations;
        }
    }
}
