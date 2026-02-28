using System;
using System.IO;

namespace BioWare.Utility
{
    /// <summary>
    /// Helper class that provides Path.GetRelativePath on both net9.0 and net48.
    /// </summary>
    internal static class PathHelper
    {
        public static string GetRelativePath(string relativeTo, string path)
        {
#if NET472
            if (string.IsNullOrEmpty(relativeTo)) throw new ArgumentNullException(nameof(relativeTo));
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));

            Uri fromUri = new Uri(AppendDirectorySeparator(Path.GetFullPath(relativeTo)));
            Uri toUri = new Uri(Path.GetFullPath(path));

            if (fromUri.Scheme != toUri.Scheme) return path;

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            string relativePath = Uri.UnescapeDataString(relativeUri.ToString());
            if (string.Equals(toUri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }
            return relativePath;
#else
            return Path.GetRelativePath(relativeTo, path);
#endif
        }

#if NET472
        private static string AppendDirectorySeparator(string path)
        {
            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()) &&
                !path.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
                return path + Path.DirectorySeparatorChar;
            return path;
        }
#endif
    }
}
