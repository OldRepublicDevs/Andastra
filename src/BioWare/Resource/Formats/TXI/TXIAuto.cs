using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;

namespace BioWare.Resource.Formats.TXI
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/txi/txi_auto.py
    // Original: read_txi, write_txi, bytes_txi functions
    public static class TXIAuto
    {
        private const string UnsupportedTxiFormatMessage = "Unsupported format specified; use TXI.";
        private const string UnsupportedTxiSourceMessage = "Source must be string, byte[], or Stream for TXI";
        private const string UnsupportedTxiTargetMessage = "Target must be string or Stream for TXI";

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/txi/txi_auto.py:13-19
        // Original: def read_txi(source: SOURCE_TYPES, offset: int = 0, size: int | None = None) -> TXI
        public static TXI ReadTxi(object source, int offset = 0, int? size = null)
        {
            int sizeValue = size ?? 0;
            byte[] data = ResourceAutoHelpers.SourceDispatcher.ToBytes(source);
            return new TXIBinaryReader(data, offset, sizeValue).Load();
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/txi/txi_auto.py:22-32
        // Original: def write_txi(txi: TXI, target: TARGET_TYPES, file_format: ResourceType = ResourceType.TXI)
        public static void WriteTxi(TXI txi, object target, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.TXI;
            if (txi == null) throw new ArgumentNullException(nameof(txi));
            ValidateTxiFormat(format, nameof(fileFormat));
            WriteTxiTarget(
                target,
                filepath => new TXIBinaryWriter(txi, filepath).Write(),
                stream => new TXIBinaryWriter(txi, stream).Write());
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/txi/txi_auto.py:35-42
        // Original: def bytes_txi(txi: TXI, file_format: ResourceType = ResourceType.TXI) -> bytes
        public static byte[] BytesTxi(TXI txi, ResourceType fileFormat = null)
        {
            if (txi == null) throw new ArgumentNullException(nameof(txi));
            using (var ms = new MemoryStream())
            {
                WriteTxi(txi, ms, fileFormat);
                return ms.ToArray();
            }
        }

        private static void ValidateTxiFormat(ResourceType format, string formatParamName)
        {
            if (format != ResourceType.TXI)
            {
                throw new ArgumentException(UnsupportedTxiFormatMessage, formatParamName);
            }
        }

        /// <summary>
        /// Dispatches TXI output to either a filesystem path or stream target.
        /// </summary>
        private static void WriteTxiTarget(object target, Action<string> writeToPath, Action<Stream> writeToStream)
        {
            ResourceAutoHelpers.SourceDispatcher.DispatchWrite(target, writeToPath, writeToStream, "TXI");
        }
    }
}

