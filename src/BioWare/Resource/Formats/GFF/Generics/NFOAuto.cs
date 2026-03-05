using System;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource;

namespace BioWare.Resource.Formats.GFF.Generics
{
    /// <summary>
    /// Auto-reader/writer for Odyssey save metadata (<c>NFO</c>) stored as a GFF.
    /// </summary>
    public static class NFOAuto
    {
        public static NFOData ReadNfo(object source, int offset = 0, int? size = null)
        {
            GFF gff = GFFAuto.ReadGff(source, offset, size);
            return NFOHelpers.ConstructNfo(gff);
        }

        public static void WriteNfo(NFOData nfo, object target, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.GFF;
            if (nfo == null) throw new ArgumentNullException(nameof(nfo));
            ValidateNfoFormat(format, nameof(fileFormat));

            GFF gff = NFOHelpers.DismantleNfo(nfo);
            GFFAuto.WriteGff(gff, target, format);
        }

        public static byte[] BytesNfo(NFOData nfo, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.GFF;
            if (nfo == null) throw new ArgumentNullException(nameof(nfo));
            ValidateNfoFormat(format, nameof(fileFormat));

            GFF gff = NFOHelpers.DismantleNfo(nfo);
            return GFFAuto.BytesGff(gff, format);
        }

        /// <summary>
        /// Validates that NFO operations use GFF content format.
        /// </summary>
        /// <remarks>
        /// NFO is stored as a GFF payload on disk (typically <c>savenfo.res</c>).
        /// </remarks>
        private static void ValidateNfoFormat(ResourceType format, string paramName)
        {
            if (format != ResourceType.GFF)
            {
                throw new ArgumentException("Unsupported format specified; use GFF (NFO content).", paramName);
            }
        }
    }
}


