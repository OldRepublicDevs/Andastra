using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource;

namespace BioWare.Resource.Formats.GFF.Generics
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/pth.py:212-241
    // Original: def read_pth, def write_pth, def bytes_pth
    public static class PTHAuto
    {
        private const string UnsupportedPthSourceMessage = "Source must be string, byte[], or Stream for PTH";
        private const string UnsupportedPthTargetMessage = "Target must be string or Stream for PTH";

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/pth.py:212-218
        // Original: def read_pth(source: SOURCE_TYPES, offset: int = 0, size: int | None = None) -> PTH:
        public static PTH ReadPth(object source, int offset = 0, int? size = null)
        {
            GFF gff = ReadPthGff(source, offset, size ?? 0);
            return PTHHelpers.ConstructPth(gff);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/pth.py:221-230
        // Original: def write_pth(pth: PTH, target: TARGET_TYPES, game: Game = BioWareGame.K2, file_format: ResourceType = ResourceType.GFF, *, use_deprecated: bool = True):
        public static void WritePth(PTH pth, object target, BioWareGame game = BioWareGame.K2, ResourceType fileFormat = null, bool useDeprecated = true)
        {
            ResourceType format = fileFormat ?? ResourceType.GFF;
            GFF gff = PTHHelpers.DismantlePth(pth, game, useDeprecated);
            WritePthTarget(gff, target, format);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/pth.py:233-241
        // Original: def bytes_pth(pth: PTH, game: Game = BioWareGame.K2, file_format: ResourceType = ResourceType.GFF, *, use_deprecated: bool = True) -> bytes:
        public static byte[] BytesPth(PTH pth, BioWareGame game = BioWareGame.K2, ResourceType fileFormat = null, bool useDeprecated = true)
        {
            ResourceType format = fileFormat ?? ResourceType.GFF;
            GFF gff = PTHHelpers.DismantlePth(pth, game, useDeprecated);
            return GFFAuto.BytesGff(gff, format);
        }

        /// <summary>
        /// Loads source input into a GFF payload used to construct PTH.
        /// </summary>
        private static GFF ReadPthGff(object source, int offset, int size)
        {
            if (source is string filepath)
            {
                return new GFFBinaryReader(filepath).Load();
            }

            byte[] data = ResourceAutoHelpers.SourceDispatcher.ToBytes(source);
            using (var ms = new MemoryStream(data, offset, size > 0 ? size : data.Length - offset))
            {
                return new GFFBinaryReader(ms).Load();
            }
        }

        /// <summary>
        /// Writes PTH data to supported path or stream targets.
        /// </summary>
        private static void WritePthTarget(GFF gff, object target, ResourceType format)
        {
            if (target is string filepath)
            {
                GFFAuto.WriteGff(gff, filepath, format);
                return;
            }

            if (target is Stream stream)
            {
                byte[] data = GFFAuto.BytesGff(gff, format);
                stream.Write(data, 0, data.Length);
                return;
            }

            throw new ArgumentException(UnsupportedPthTargetMessage);
        }
    }
}
