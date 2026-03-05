using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace OdyTools.Utils
{
    /// <summary>
    /// Helper for formatting NWScript (NSS) source: indentation by braces and consistent line breaks.
    /// Used by the NSS editor "Format Document" action.
    /// </summary>
    public static class NssFormatHelper
    {
        /// <summary>
        /// Default indent string (4 spaces). Callers can use tab or different width via options later.
        /// </summary>
        public const string DefaultIndent = "    ";

        /// <summary>
        /// Formats NSS source with brace-based indentation. Preserves blank lines and comments.
        /// Does not change semantics. Uses spaces only for indent when using default settings.
        /// </summary>
        /// <param name="source">Raw NSS source.</param>
        /// <param name="indentString">String used for one indent level (default 4 spaces).</param>
        /// <returns>Formatted source.</returns>
        public static string FormatDocument(string source, string indentString = DefaultIndent)
        {
            if (string.IsNullOrEmpty(source))
            {
                return source;
            }

            var lines = new List<string>();
            int pos = 0;
            while (pos < source.Length)
            {
                int next = source.IndexOf('\n', pos);
                if (next < 0)
                {
                    lines.Add(source.Substring(pos));
                    break;
                }
                lines.Add(source.Substring(pos, next - pos));
                pos = next + 1;
            }

            var sb = new StringBuilder();
            int indentLevel = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                string line = lines[i];
                string trimmed = line.Trim();
                if (trimmed.Length == 0)
                {
                    sb.AppendLine();
                    continue;
                }

                // Decrease indent before line if it starts with }
                if (trimmed.StartsWith("}", StringComparison.Ordinal))
                {
                    indentLevel = Math.Max(0, indentLevel - 1);
                }

                // Emit indent
                for (int k = 0; k < indentLevel; k++)
                {
                    sb.Append(indentString);
                }
                sb.AppendLine(trimmed);

                // Count braces on this line (ignore inside strings/comments for simplicity)
                int open = CountUnquoted(trimmed, '{');
                int close = CountUnquoted(trimmed, '}');
                indentLevel += open - close;
                indentLevel = Math.Max(0, indentLevel);
            }

            return sb.ToString().TrimEnd();
        }

        private static int CountUnquoted(string s, char c)
        {
            int n = 0;
            bool inDouble = false;
            bool inSingle = false;
            bool inLineComment = false;
            int i = 0;
            while (i < s.Length)
            {
                if (inLineComment)
                {
                    i++;
                    continue;
                }
                if (s[i] == '"' && !inSingle)
                {
                    inDouble = !inDouble;
                    i++;
                    continue;
                }
                if (s[i] == '\'' && !inDouble)
                {
                    inSingle = !inSingle;
                    i++;
                    continue;
                }
                if (!inDouble && !inSingle && i + 1 < s.Length && s[i] == '/' && s[i + 1] == '/')
                {
                    inLineComment = true;
                    i++;
                    continue;
                }
                if (!inDouble && !inSingle && s[i] == c)
                {
                    n++;
                }
                i++;
            }
            return n;
        }
    }
}
