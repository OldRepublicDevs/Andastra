using System.Collections.Generic;
using System.Text;
using BioWare.Tools;
using KotorCLI.Logging;

namespace KotorCLI.Commands
{
    internal static class ReferenceSearchOutputFormatter
    {
        public static int EmitReferenceResults(
            ILogger logger,
            string needle,
            string referenceType,
            List<ReferenceSearchResult> results,
            bool jsonOutput,
            bool countOnly)
        {
            int count = results == null ? 0 : results.Count;
            if (count == 0)
            {
                if (jsonOutput)
                {
                    logger.Info(FormatJson(needle, referenceType, results));
                }
                else if (countOnly)
                {
                    logger.Info(FormatCount(0));
                }
                else
                {
                    logger.Info("No references found.");
                }

                return 1;
            }

            if (jsonOutput)
            {
                logger.Info(FormatJson(needle, referenceType, results));
                return 0;
            }

            if (countOnly)
            {
                logger.Info(FormatCount(count));
                return 0;
            }

            foreach (ReferenceSearchResult result in results)
            {
                logger.Info(result.DisplayLabel);
            }

            logger.Info("Found " + count + " reference(s).");
            return 0;
        }

        public static string FormatCount(int count)
        {
            return count.ToString();
        }

        public static string FormatJson(string needle, string referenceType, IList<ReferenceSearchResult> results)
        {
            var sb = new StringBuilder(256);
            int count = results == null ? 0 : results.Count;
            sb.Append("{\"needle\":\"").Append(EscapeJson(needle));
            sb.Append("\",\"type\":\"").Append(EscapeJson(referenceType));
            sb.Append("\",\"count\":").Append(count);
            sb.Append(",\"references\":[");

            if (results != null)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(',');
                    }

                    ReferenceSearchResult result = results[i];
                    string resourceName = result.Resource == null
                        ? string.Empty
                        : result.Resource.ResName + "." + result.Resource.ResType.Extension;
                    string filepath = result.Resource == null ? string.Empty : result.Resource.FilePath ?? string.Empty;

                    sb.Append("{\"resource\":\"").Append(EscapeJson(resourceName));
                    sb.Append("\",\"filepath\":\"").Append(EscapeJson(filepath));
                    sb.Append("\",\"fieldPath\":\"").Append(EscapeJson(result.FieldPath ?? string.Empty));
                    sb.Append("\",\"matchedValue\":\"").Append(EscapeJson(result.MatchedValue ?? string.Empty));
                    sb.Append("\",\"displayLabel\":\"").Append(EscapeJson(result.DisplayLabel ?? string.Empty));
                    sb.Append("\"}");
                }
            }

            sb.Append("]}");
            return sb.ToString();
        }

        private static string EscapeJson(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var sb = new StringBuilder(value.Length + 8);
            foreach (char c in value)
            {
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }

            return sb.ToString();
        }
    }
}
