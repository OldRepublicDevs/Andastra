using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;

namespace BioWare.Resource.Formats.LTR
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/ltr/ltr_auto.py
    // Original: read_ltr, write_ltr, bytes_ltr functions
    public static class LTRAuto
    {
        private const string UnsupportedLtrFormatMessage = "Unsupported format specified; use LTR.";
        private const string UnsupportedLtrSourceMessage = "Source must be string, byte[], or Stream for LTR";
        private const string UnsupportedLtrTargetMessage = "Target must be string or Stream for LTR";

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/ltr/ltr_auto.py:13-38
        // Original: def read_ltr(source: SOURCE_TYPES, offset: int = 0, size: int | None = None) -> LTR
        public static LTR ReadLtr(object source, int offset = 0, int? size = null)
        {
            try
            {
                return ReadLtrSource(source, offset, size ?? 0);
            }
            catch (IOException)
            {
                throw new ArgumentException("Tried to load an unsupported or corrupted LTR file.");
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/ltr/ltr_auto.py:41-62
        // Original: def write_ltr(ltr: LTR, target: TARGET_TYPES, file_format: ResourceType = ResourceType.LTR)
        public static void WriteLtr(LTR ltr, object target, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.LTR;
            if (ltr == null) throw new ArgumentNullException(nameof(ltr));
            ValidateLtrFormat(format, nameof(fileFormat));

            WriteLtrTarget(
                target,
                filepath => new LTRBinaryWriter(ltr, filepath).Write(),
                stream => new LTRBinaryWriter(ltr, stream).Write());
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/ltr/ltr_auto.py:65-88
        // Original: def bytes_ltr(ltr: LTR, file_format: ResourceType = ResourceType.LTR) -> bytes
        public static byte[] BytesLtr(LTR ltr, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.LTR;
            if (ltr == null) throw new ArgumentNullException(nameof(ltr));
            ValidateLtrFormat(format, nameof(fileFormat));
            using (var ms = new MemoryStream())
            {
                WriteLtr(ltr, ms, format);
                return ms.ToArray();
            }
        }

        private static void ValidateLtrFormat(ResourceType format, string formatParamName)
        {
            if (format != ResourceType.LTR)
            {
                throw new ArgumentException(UnsupportedLtrFormatMessage, formatParamName);
            }
        }

        private static LTR ReadLtrSource(object source, int offset, int size)
        {
            byte[] data = ResourceAutoHelpers.SourceDispatcher.ToBytes(source);
            return new LTRBinaryReader(data, offset, size).Load();
        }

        /// <summary>
        /// Dispatches LTR output to either a filesystem path or stream target.
        /// </summary>
        private static void WriteLtrTarget(object target, Action<string> writeToPath, Action<Stream> writeToStream)
        {
            ResourceAutoHelpers.SourceDispatcher.DispatchWrite(target, writeToPath, writeToStream, "LTR");
        }
    }
}

