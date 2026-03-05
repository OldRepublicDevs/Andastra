using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;
using JetBrains.Annotations;

namespace BioWare.Resource.Formats.TwoDA
{

    /// <summary>
    /// Auto-detection and convenience functions for 2DA files.
    /// 1:1 port of Python twoda_auto.py from pykotor/resource/formats/twoda/twoda_auto.py
    /// </summary>
    public static class TwoDAAuto
    {
        private const string UnsupportedSourceMessage = "Source must be string, byte[], or Stream";

        /// <summary>
        /// Reads a 2DA file from a file path, byte array, or stream.
        /// Supports binary .2da and CSV (.2da.csv or .csv). Format is auto-detected by path extension or content.
        /// </summary>
        public static TwoDA Read2DA(object source, int offset = 0, int? size = null, ResourceType fileFormat = null)
        {
            byte[] data = ResourceAutoHelpers.SourceDispatcher.ToBytes(source, out string filepath);
            return Read2DAFromBytes(data, filepath);
        }

        private static TwoDA Read2DAFromBytes(byte[] data, string filepath)
        {
            bool preferCsv = !string.IsNullOrEmpty(filepath) &&
                (filepath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase) ||
                 filepath.EndsWith(".2da.csv", StringComparison.OrdinalIgnoreCase));
            if (preferCsv || LooksLikeCsv(data))
            {
                return TwoDACsvReader.Load(data);
            }
            var reader = new TwoDABinaryReader(data);
            return reader.Load();
        }

        private static bool LooksLikeCsv(byte[] data)
        {
            if (data == null || data.Length < 4) return false;
            // Binary 2DA starts with "2DA " (ASCII)
            if (data.Length >= 4 && data[0] == '2' && data[1] == 'D' && data[2] == 'A' && data[3] == ' ')
            {
                return false;
            }
            // CSV: printable ASCII and commas/newlines
            int printable = 0;
            for (int i = 0; i < Math.Min(data.Length, 512); i++)
            {
                byte b = data[i];
                if (b == ',' || b == '\n' || b == '\r' || (b >= 0x20 && b < 0x7F)) printable++;
            }
            return printable > Math.Min(data.Length, 512) * 0.8;
        }

        /// <summary>
        /// Alias for Read2DA (alternative naming, matches common style).
        /// </summary>
        public static TwoDA ReadTwoDA(object source, int offset = 0, int? size = null, ResourceType fileFormat = null)
        {
            return Read2DA(source, offset, size, fileFormat);
        }

        /// <summary>
        /// Writes the 2DA data to the target location with the specified format.
        /// </summary>
        public static void Write2DA(TwoDA twoda, string target, ResourceType fileFormat)
        {
            byte[] data = Bytes2DA(twoda, fileFormat);
            File.WriteAllBytes(target, data);
        }

        /// <summary>
        /// Alias for Write2DA (alternative naming, matches common style).
        /// </summary>
        public static void WriteTwoDA(TwoDA twoda, string target, ResourceType fileFormat)
        {
            Write2DA(twoda, target, fileFormat);
        }

        /// <summary>
        /// Returns the 2DA data as a byte array (binary .2da or CSV depending on fileFormat).
        /// </summary>
        public static byte[] Bytes2DA(TwoDA twoda, [CanBeNull] ResourceType fileFormat = null)
        {
            if (fileFormat == ResourceType.TwoDA_CSV)
            {
                return TwoDACsvWriter.Write(twoda);
            }
            var writer = new TwoDABinaryWriter(twoda);
            return writer.Write();
        }

        /// <summary>
        /// Alias for Bytes2DA (alternative naming, matches common style).
        /// </summary>
        public static byte[] BytesTwoDA(TwoDA twoda, [CanBeNull] ResourceType fileFormat = null)
        {
            return Bytes2DA(twoda, fileFormat);
        }
    }
}

