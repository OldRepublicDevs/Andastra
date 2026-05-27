using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace BioWare.Tools
{
    /// <summary>
    /// Wildcard matching for module capsule filenames during reference search.
    /// </summary>
    public static class ModuleGlobMatcher
    {
        public static bool MatchesAnyModuleGlob(string moduleFilePath, IList<string> patterns)
        {
            if (patterns == null || patterns.Count == 0)
            {
                return true;
            }

            if (string.IsNullOrEmpty(moduleFilePath))
            {
                return false;
            }

            string normalizedPath = moduleFilePath.Replace('\\', '/');
            string fileName = Path.GetFileName(normalizedPath);
            foreach (string pattern in patterns)
            {
                if (string.IsNullOrWhiteSpace(pattern))
                {
                    continue;
                }

                if (MatchesGlob(fileName, pattern.Trim()))
                {
                    return true;
                }
            }

            return false;
        }

        internal static bool MatchesGlob(string fileName, string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return false;
            }

            if (pattern == "*")
            {
                return true;
            }

            var regexBuilder = new StringBuilder("^");
            for (int i = 0; i < pattern.Length; i++)
            {
                char c = pattern[i];
                if (c == '*')
                {
                    regexBuilder.Append(".*");
                }
                else if (c == '?')
                {
                    regexBuilder.Append('.');
                }
                else if ("\\.[]{}()+|^$".IndexOf(c) >= 0)
                {
                    regexBuilder.Append('\\').Append(c);
                }
                else
                {
                    regexBuilder.Append(c);
                }
            }

            regexBuilder.Append('$');
            return Regex.IsMatch(fileName, regexBuilder.ToString(), RegexOptions.IgnoreCase);
        }
    }
}
