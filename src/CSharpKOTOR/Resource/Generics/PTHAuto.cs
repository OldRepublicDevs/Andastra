using System;
using System.IO;
using CSharpKOTOR.Common;
using CSharpKOTOR.Formats.GFF;
using CSharpKOTOR.Resources;

namespace CSharpKOTOR.Resource.Generics
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/pth.py:212-241
    // Original: def read_pth, def write_pth, def bytes_pth
    public static class PTHAuto
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/pth.py:212-218
        // Original: def read_pth(source: SOURCE_TYPES, offset: int = 0, size: int | None = None) -> PTH:
        public static PTH ReadPth(object source, int offset = 0, int? size = null)
        {
            int sizeValue = size ?? 0;
            GFF gff;
            if (source is string filepath)
            {
                gff = GFFAuto.ReadGff(filepath, offset, size);
            }
            else if (source is byte[] data)
            {
                gff = GFFAuto.ReadGff(data, offset, sizeValue);
            }
            else if (source is Stream stream)
            {
                gff = GFFAuto.ReadGff(stream, offset, sizeValue);
            }
            else
            {
                throw new ArgumentException("Source must be string, byte[], or Stream for PTH");
            }
            return PTHHelpers.ConstructPth(gff);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/pth.py:221-230
        // Original: def write_pth(pth: PTH, target: TARGET_TYPES, game: Game = Game.K2, file_format: ResourceType = ResourceType.GFF, *, use_deprecated: bool = True):
        public static void WritePth(PTH pth, object target, Game game = Game.K2, ResourceType fileFormat = null, bool useDeprecated = true)
        {
            ResourceType format = fileFormat ?? ResourceType.GFF;
            GFF gff = PTHHelpers.DismantlePth(pth, game, useDeprecated);
            GFFAuto.WriteGff(gff, target, format);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/pth.py:233-241
        // Original: def bytes_pth(pth: PTH, game: Game = Game.K2, file_format: ResourceType = ResourceType.GFF, *, use_deprecated: bool = True) -> bytes:
        public static byte[] BytesPth(PTH pth, Game game = Game.K2, ResourceType fileFormat = null, bool useDeprecated = true)
        {
            ResourceType format = fileFormat ?? ResourceType.GFF;
            GFF gff = PTHHelpers.DismantlePth(pth, game, useDeprecated);
            return GFFAuto.BytesGff(gff, format);
        }
    }
}
