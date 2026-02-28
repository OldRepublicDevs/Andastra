#if NET48

using System.Collections.Generic;

namespace KotorCLI
{
    internal static class DictionaryExtensions
    {
        public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue = default)
        {
            return dict != null && dict.TryGetValue(key, out var value) ? value : defaultValue;
        }
    }
}

#endif
