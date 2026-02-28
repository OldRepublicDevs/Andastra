using System;
using System.IO;

namespace KotorDiff
{
    /// <summary>
    /// Path extension methods that work on all target frameworks (e.g. net48 and net9.0).
    /// Use these instead of Path.GetRelativePath so the same API works on .NET Framework.
    /// </summary>
    internal static class PathExtensions
    {
        /// <summary>
        /// Returns a relative path from <paramref name="basePath"/> to <paramref name="path"/>.
        /// Same semantics as Path.GetRelativePath; provided as an extension for net48 compatibility.
        /// </summary>
        public static string GetRelativePath(this string basePath, string path)
        {
            if (string.IsNullOrEmpty(basePath))
                throw new ArgumentNullException(nameof(basePath));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            string baseFull = Path.GetFullPath(basePath);
            string pathFull = Path.GetFullPath(path);

            if (!baseFull.EndsWith(Path.DirectorySeparatorChar.ToString(), StringComparison.Ordinal))
                baseFull += Path.DirectorySeparatorChar;

            var baseUri = new Uri(baseFull);
            var pathUri = new Uri(pathFull);
            Uri relativeUri = baseUri.MakeRelativeUri(pathUri);
            string result = Uri.UnescapeDataString(relativeUri.ToString())
                .Replace('/', Path.DirectorySeparatorChar);
            return result;
        }
    }
}
