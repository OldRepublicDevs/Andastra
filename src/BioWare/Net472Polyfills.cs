// Polyfills for APIs available in .NET 9.0 but missing in .NET Framework 4.7.2.
// Only compiled when targeting net48. Extensions placed in System namespace for automatic availability.
#if NET472
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;

// ReSharper disable once CheckNamespace
namespace System
{
    internal static class Net472StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this string s, char c)
        {
            return s.IndexOf(c) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this string s, string value, StringComparison comparisonType)
        {
            return s.IndexOf(value, comparisonType) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Contains(this string s, char c, StringComparison comparisonType)
        {
            return s.IndexOf(c.ToString(), comparisonType) >= 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Replace(this string s, string oldValue, string newValue, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(oldValue))
                throw new ArgumentException("oldValue cannot be null or empty", nameof(oldValue));

            var sb = new Text.StringBuilder();
            int previousIndex = 0;
            int index;
            while ((index = s.IndexOf(oldValue, previousIndex, comparisonType)) >= 0)
            {
                sb.Append(s, previousIndex, index - previousIndex);
                sb.Append(newValue);
                previousIndex = index + oldValue.Length;
            }
            sb.Append(s, previousIndex, s.Length - previousIndex);
            return sb.ToString();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetHashCode(this string s, StringComparison comparisonType)
        {
            switch (comparisonType)
            {
                case StringComparison.OrdinalIgnoreCase:
                    return StringComparer.OrdinalIgnoreCase.GetHashCode(s);
                case StringComparison.Ordinal:
                    return StringComparer.Ordinal.GetHashCode(s);
                case StringComparison.CurrentCultureIgnoreCase:
                    return StringComparer.CurrentCultureIgnoreCase.GetHashCode(s);
                case StringComparison.InvariantCultureIgnoreCase:
                    return StringComparer.InvariantCultureIgnoreCase.GetHashCode(s);
                default:
                    return s.GetHashCode();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWith(this string s, char c)
        {
            return s.Length > 0 && s[0] == c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWith(this string s, char c)
        {
            return s.Length > 0 && s[s.Length - 1] == c;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] Split(this string s, char separator, StringSplitOptions options)
        {
            return s.Split(new[] { separator }, options);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string[] Split(this string s, string separator, StringSplitOptions options)
        {
            return s.Split(new[] { separator }, options);
        }
    }

    internal static class Net472KvpExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }

    internal static class Net472ConvertExtensions
    {
        public static byte[] FromHexString(string s)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (s.Length % 2 != 0) throw new FormatException("Hex string must have even length");
            byte[] bytes = new byte[s.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(s.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return bytes;
        }
    }

    internal static class Net472BitConverterExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe float Int32BitsToSingle(int value)
        {
            return *(float*)&value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int SingleToInt32Bits(float value)
        {
            return *(int*)&value;
        }
    }

    internal static class Net472DictionaryExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            return dict.TryGetValue(key, out value) ? value : default(TValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            TValue value;
            return dict.TryGetValue(key, out value) ? value : defaultValue;
        }
    }

    internal static class Net472PathExtensions
    {
        public static string GetRelativePath(string relativeTo, string path)
        {
            if (string.IsNullOrEmpty(relativeTo)) throw new ArgumentNullException(nameof(relativeTo));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            Uri fromUri = new Uri(AppendDirectorySeparator(IO.Path.GetFullPath(relativeTo)));
            Uri toUri = new Uri(IO.Path.GetFullPath(path));

            if (fromUri.Scheme != toUri.Scheme) return path;

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());
            if (string.Equals(toUri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath.Replace(IO.Path.AltDirectorySeparatorChar, IO.Path.DirectorySeparatorChar);
            }
            return relativePath;
        }

        private static string AppendDirectorySeparator(string path)
        {
            if (!path.EndsWith(IO.Path.DirectorySeparatorChar.ToString()) &&
                !path.EndsWith(IO.Path.AltDirectorySeparatorChar.ToString()))
                return path + IO.Path.DirectorySeparatorChar;
            return path;
        }
    }

    internal static class Net472EnumExtensions
    {
        public static TEnum[] GetValues<TEnum>() where TEnum : struct
        {
            return (TEnum[])Enum.GetValues(typeof(TEnum));
        }
    }

    internal static class Net472Vector3Extensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetAxis(this Numerics.Vector3 v, int axis)
        {
            switch (axis)
            {
                case 0: return v.X;
                case 1: return v.Y;
                case 2: return v.Z;
                default: throw new IndexOutOfRangeException("Vector3 axis must be 0, 1, or 2");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Numerics.Vector3 SetAxis(this Numerics.Vector3 v, int axis, float value)
        {
            switch (axis)
            {
                case 0: v.X = value; break;
                case 1: v.Y = value; break;
                case 2: v.Z = value; break;
                default: throw new IndexOutOfRangeException("Vector3 axis must be 0, 1, or 2");
            }
            return v;
        }
    }
}

namespace System.Linq
{
    internal static class Net472LinqExtensions
    {
        public static TSource MaxBy<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");
                TSource maxItem = enumerator.Current;
                TKey maxKey = keySelector(maxItem);
                while (enumerator.MoveNext())
                {
                    TKey key = keySelector(enumerator.Current);
                    if (key.CompareTo(maxKey) > 0)
                    {
                        maxKey = key;
                        maxItem = enumerator.Current;
                    }
                }
                return maxItem;
            }
        }

        public static TSource MinBy<TSource, TKey>(this Collections.Generic.IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable<TKey>
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext()) throw new InvalidOperationException("Sequence contains no elements");
                TSource minItem = enumerator.Current;
                TKey minKey = keySelector(minItem);
                while (enumerator.MoveNext())
                {
                    TKey key = keySelector(enumerator.Current);
                    if (key.CompareTo(minKey) < 0)
                    {
                        minKey = key;
                        minItem = enumerator.Current;
                    }
                }
                return minItem;
            }
        }
    }
}
#endif
