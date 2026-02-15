using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BioWare.Resource.Formats.TwoDA
{
    /// <summary>
    /// Writes 2DA to CSV (e.g. .2da.csv). First column = row label; first row = headers.
    /// </summary>
    public static class TwoDACsvWriter
    {
        /// <summary>
        /// Write TwoDA to CSV as bytes (UTF-8).
        /// </summary>
        public static byte[] Write(TwoDA twoda)
        {
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms, Encoding.UTF8))
            {
                Write(twoda, writer);
                writer.Flush();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Write TwoDA to CSV using a text writer.
        /// </summary>
        public static void Write(TwoDA twoda, TextWriter writer)
        {
            var headers = twoda.GetHeaders();
            if (headers.Count == 0)
            {
                writer.WriteLine(EscapeCsvField("label"));
                return;
            }

            // Header row: "label" for first column, then data column headers
            var headerRow = new List<string> { "label" };
            headerRow.AddRange(headers);
            writer.WriteLine(ToCsvLine(headerRow));

            for (int r = 0; r < twoda.GetHeight(); r++)
            {
                var row = new List<string> { twoda.GetLabel(r) };
                foreach (string h in headers)
                {
                    row.Add(twoda.GetCellString(r, h));
                }
                writer.WriteLine(ToCsvLine(row));
            }
        }

        private static string ToCsvLine(IList<string> fields)
        {
            if (fields == null || fields.Count == 0)
            {
                return "";
            }
            var sb = new StringBuilder();
            for (int i = 0; i < fields.Count; i++)
            {
                if (i > 0) sb.Append(',');
                sb.Append(EscapeCsvField(fields[i] ?? ""));
            }
            return sb.ToString();
        }

        private static string EscapeCsvField(string value)
        {
            if (value == null) return "\"\"";
            if (value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) >= 0)
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }
    }
}
