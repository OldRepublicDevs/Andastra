using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BioWare.Resource.Formats.TwoDA
{
    /// <summary>
    /// Reads 2DA from CSV (e.g. .2da.csv). First row = column headers; first column = row label.
    /// </summary>
    public static class TwoDACsvReader
    {
        /// <summary>
        /// Load a TwoDA from CSV bytes (UTF-8 or ASCII).
        /// </summary>
        public static TwoDA Load(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            using (var reader = new StreamReader(ms, Encoding.UTF8, detectEncodingFromByteOrderMarks: true))
            {
                return Load(reader);
            }
        }

        /// <summary>
        /// Load a TwoDA from a text reader (CSV).
        /// </summary>
        public static TwoDA Load(TextReader reader)
        {
            var headers = ParseCsvLine(reader.ReadLine());
            if (headers == null || headers.Count == 0)
            {
                return new TwoDA();
            }

            // First column = row label (not a data column). Rest = column headers.
            var columnHeaders = new List<string>();
            for (int i = 1; i < headers.Count; i++)
            {
                string h = (headers[i] ?? "").Trim();
                columnHeaders.Add(string.IsNullOrEmpty(h) ? $"Column{i}" : h);
            }

            var twoda = new TwoDA(columnHeaders);

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var cells = ParseCsvLine(line);
                if (cells.Count == 0)
                {
                    continue;
                }

                string rowLabel = cells.Count > 0 ? (cells[0] ?? "").Trim() : "";
                var cellDict = new Dictionary<string, object>();
                for (int i = 0; i < columnHeaders.Count; i++)
                {
                    int cellIndex = i + 1;
                    string val = cellIndex < cells.Count ? (cells[cellIndex] ?? "").Trim() : "";
                    cellDict[columnHeaders[i]] = val;
                }

                twoda.AddRow(rowLabel, cellDict);
            }

            return twoda;
        }

        /// <summary>
        /// Parse a single CSV line into a list of fields (handles quoted fields).
        /// </summary>
        internal static List<string> ParseCsvLine(string line)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(line))
            {
                return result;
            }

            var current = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            current.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else if (c == ',')
                    {
                        result.Add(current.ToString());
                        current.Clear();
                    }
                    else
                    {
                        current.Append(c);
                    }
                }
            }

            result.Add(current.ToString());
            return result;
        }
    }
}
