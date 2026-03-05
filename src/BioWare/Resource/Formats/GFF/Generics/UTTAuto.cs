using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource;

namespace BioWare.Resource.Formats.GFF.Generics
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utt.py:327-356
    // Original: def read_utt, def write_utt, def bytes_utt
    public static class UTTAuto
    {
        private const string UnsupportedUttSourceMessage = "Source must be string, byte[], or Stream for UTT";
        private const string UnsupportedUttTargetMessage = "Target must be string or Stream for UTT";

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utt.py:327-333
        // Original: def read_utt(source: SOURCE_TYPES, offset: int = 0, size: int | None = None) -> UTT:
        public static UTT ReadUtt(object source, int offset = 0, int? size = null)
        {
            GFF gff = ReadUttGff(source, offset, size ?? 0);
            return UTTHelpers.ConstructUtt(gff);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utt.py:336-345
        // Original: def write_utt(utt: UTT, target: TARGET_TYPES, game: Game = BioWareGame.K2, file_format: ResourceType = ResourceType.GFF, *, use_deprecated: bool = True):
        public static void WriteUtt(UTT utt, object target, BioWareGame game = BioWareGame.K2, ResourceType fileFormat = null, bool useDeprecated = true)
        {
            ResourceType format = fileFormat ?? ResourceType.GFF;
            GFF gff = UTTHelpers.DismantleUtt(utt, game, useDeprecated);
            WriteUttTarget(gff, target, format);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/utt.py:348-356
        // Original: def bytes_utt(utt: UTT, game: Game = BioWareGame.K2, file_format: ResourceType = ResourceType.GFF, *, use_deprecated: bool = True) -> bytes:
        public static byte[] BytesUtt(UTT utt, BioWareGame game = BioWareGame.K2, ResourceType fileFormat = null, bool useDeprecated = true)
        {
            ResourceType format = fileFormat ?? ResourceType.GFF;
            GFF gff = UTTHelpers.DismantleUtt(utt, game, useDeprecated);
            return GFFAuto.BytesGff(gff, format);
        }

        /// <summary>
        /// Loads source input into a GFF payload used to construct UTT.
        /// </summary>
        private static GFF ReadUttGff(object source, int offset, int size)
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
        /// Writes UTT data to supported path or stream targets.
        /// </summary>
        private static void WriteUttTarget(GFF gff, object target, ResourceType format)
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

            throw new ArgumentException(UnsupportedUttTargetMessage);
        }
    }
}
