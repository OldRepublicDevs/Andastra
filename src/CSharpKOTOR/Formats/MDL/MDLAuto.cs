using System;
using System.IO;
using CSharpKOTOR.Common;
using CSharpKOTOR.Resources;
using CSharpKOTOR.Formats.MDLData;

namespace CSharpKOTOR.Formats.MDL
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/mdl/mdl_auto.py
    // Simplified detector and dispatcher for MDL/MDL_ASCII
    public static class MDLAuto
    {
        public static ResourceType DetectMdl(object source, int offset = 0)
        {
            try
            {
                using (var reader = RawBinaryReader.FromAuto(source, offset, 4))
                {
                    var first4 = reader.ReadBytes(4);
                    // PyKotor heuristic: 0x00000000 -> binary MDL, otherwise ASCII
                    if (first4.Length == 4 && first4[0] == 0 && first4[1] == 0 && first4[2] == 0 && first4[3] == 0)
                    {
                        return ResourceType.MDL;
                    }
                    return ResourceType.MDL_ASCII;
                }
            }
            catch
            {
                return ResourceType.INVALID;
            }
        }

        public static MDLData.MDL ReadMdl(object source, int offset = 0, int? size = null, object sourceExt = null, int offsetExt = 0, int sizeExt = 0, ResourceType fileFormat = null)
        {
            var fmt = fileFormat ?? DetectMdl(source, offset);
            if (fmt == ResourceType.MDL)
            {
                return new MDLBinaryReader(source, offset, size ?? 0, sourceExt, offsetExt, sizeExt).Load();
            }
            if (fmt == ResourceType.MDL_ASCII)
            {
                return new MDLAsciiReader(source, offset, size ?? 0).Load();
            }
            throw new ArgumentException("Failed to determine the format of the MDL file.");
        }

        public static MDLData.MDL ReadMdlFast(object source, int offset = 0, int? size = null, object sourceExt = null, int offsetExt = 0, int sizeExt = 0)
        {
            var fmt = DetectMdl(source, offset);
            if (fmt == ResourceType.MDL)
            {
                return new MDLBinaryReader(source, offset, size ?? 0, sourceExt, offsetExt, sizeExt, fastLoad: true).Load();
            }
            if (fmt == ResourceType.MDL_ASCII)
            {
                return new MDLAsciiReader(source, offset, size ?? 0).Load();
            }
            throw new ArgumentException("Failed to determine the format of the MDL file.");
        }

        public static void WriteMdl(MDLData.MDL mdl, object target, ResourceType fileFormat = null, object targetExt = null)
        {
            var fmt = fileFormat ?? ResourceType.MDL;
            if (fmt == ResourceType.MDL)
            {
                new MDLBinaryWriter(mdl, target, targetExt ?? target).Write();
            }
            else if (fmt == ResourceType.MDL_ASCII)
            {
                new MDLAsciiWriter(mdl, target).Write();
            }
            else
            {
                throw new ArgumentException("Unsupported format specified; use MDL or MDL_ASCII.");
            }
        }
    }
}

