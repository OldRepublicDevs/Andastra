using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource;

namespace BioWare.Resource.Formats.GFF.Generics
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utw.py:155-184
    // Original: def read_utw, def write_utw, def bytes_utw
    public static class UTWAuto
    {
        private const string UnsupportedUtwSourceMessage = "Source must be string, byte[], or Stream for UTW";
        private const string UnsupportedUtwTargetMessage = "Target must be string or Stream for UTW";

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utw.py:155-161
        // Original: def read_utw(source: SOURCE_TYPES, offset: int = 0, size: int | None = None) -> UTW:
        public static UTW ReadUtw(object source, int offset = 0, int? size = null)
        {
            GFF gff = ReadUtwGff(source, offset, size ?? 0);
            return UTWHelpers.ConstructUtw(gff);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utw.py:164-173
        // Original: def write_utw(utw: UTW, target: TARGET_TYPES, game: Game = BioWareGame.K2, file_format: ResourceType = ResourceType.GFF, *, use_deprecated: bool = True):
        public static void WriteUtw(UTW utw, object target, BioWareGame game = BioWareGame.K2, ResourceType fileFormat = null, bool useDeprecated = true)
        {
            ResourceType format = fileFormat ?? ResourceType.GFF;
            GFF gff = UTWHelpers.DismantleUtw(utw, game, useDeprecated);
            WriteUtwTarget(gff, target, format);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utw.py:176-184
        // Original: def bytes_utw(utw: UTW, game: Game = BioWareGame.K2, file_format: ResourceType = ResourceType.GFF, *, use_deprecated: bool = True) -> bytes:
        public static byte[] BytesUtw(UTW utw, BioWareGame game = BioWareGame.K2, ResourceType fileFormat = null, bool useDeprecated = true)
        {
            ResourceType format = fileFormat ?? ResourceType.GFF;
            GFF gff = UTWHelpers.DismantleUtw(utw, game, useDeprecated);
            return GFFAuto.BytesGff(gff, format);
        }

        /// <summary>
        /// Loads source input into a GFF payload used to construct UTW.
        /// </summary>
        private static GFF ReadUtwGff(object source, int offset, int size)
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
        /// Writes UTW data to supported path or stream targets.
        /// </summary>
        private static void WriteUtwTarget(GFF gff, object target, ResourceType format)
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

            throw new ArgumentException(UnsupportedUtwTargetMessage);
        }
    }
}
