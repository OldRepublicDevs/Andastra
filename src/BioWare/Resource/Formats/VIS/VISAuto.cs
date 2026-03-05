using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;

namespace BioWare.Resource.Formats.VIS
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/vis/vis_auto.py
    // Original: read_vis, write_vis, bytes_vis functions
    public static class VISAuto
    {
        private const string UnsupportedVisFormatMessage = "Unsupported format specified; use VIS.";
        private const string UnsupportedVisSourceMessage = "Source must be string, byte[], or Stream for VIS";
        private const string UnsupportedVisTargetMessage = "Target must be string or Stream for VIS";

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/vis/vis_auto.py:13-40
        // Original: def read_vis(source: SOURCE_TYPES, offset: int = 0, size: int | None = None) -> VIS
        public static VIS ReadVis(object source, int offset = 0, int? size = null)
        {
            int sizeValue = size ?? 0;
            byte[] data = ResourceAutoHelpers.SourceDispatcher.ToBytes(source);
            return new VISAsciiReader(data, offset, sizeValue).Load();
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/vis/vis_auto.py:43-66
        // Original: def write_vis(vis: VIS, target: TARGET_TYPES, file_format: ResourceType = ResourceType.VIS)
        public static void WriteVis(VIS vis, object target, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.VIS;
            if (vis == null) throw new ArgumentNullException(nameof(vis));
            ValidateVisFormat(format, nameof(fileFormat));
            WriteVisTarget(
                target,
                filepath => new VISAsciiWriter(vis, filepath).Write(),
                stream => new VISAsciiWriter(vis, stream).Write());
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/vis/vis_auto.py:69-92
        // Original: def bytes_vis(vis: VIS, file_format: ResourceType = ResourceType.VIS) -> bytes
        public static byte[] BytesVis(VIS vis, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.VIS;
            if (vis == null) throw new ArgumentNullException(nameof(vis));
            ValidateVisFormat(format, nameof(fileFormat));
            using (var ms = new MemoryStream())
            {
                WriteVis(vis, ms, format);
                return ms.ToArray();
            }
        }

        private static void ValidateVisFormat(ResourceType format, string formatParamName)
        {
            if (format != ResourceType.VIS)
            {
                throw new ArgumentException(UnsupportedVisFormatMessage, formatParamName);
            }
        }

        /// <summary>
        /// Dispatches VIS output to either a filesystem path or stream target.
        /// </summary>
        private static void WriteVisTarget(object target, Action<string> writeToPath, Action<Stream> writeToStream)
        {
            ResourceAutoHelpers.SourceDispatcher.DispatchWrite(target, writeToPath, writeToStream, "VIS");
        }
    }
}

