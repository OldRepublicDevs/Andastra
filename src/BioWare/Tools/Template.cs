using System;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource;

namespace BioWare.Tools
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/template.py
    // Original: Template utility functions
    public static class TemplateTools
    {
        /// <summary>
        /// Parses raw GFF bytes into an in-memory GFF model.
        /// </summary>
        private static GFF ParseGff(byte[] data)
        {
            return new GFFBinaryReader(data).Load();
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/template.py:14-36
        // Original: def extract_name(data: bytes) -> LocalizedString:
        public static LocalizedString ExtractName(byte[] data)
        {
            GFF gff = ParseGff(data);
            if (gff.Content == GFFContent.UTC)
            {
                return gff.Root.GetLocString("FirstName");
            }
            if (gff.Content == GFFContent.UTT || gff.Content == GFFContent.UTW)
            {
                return gff.Root.GetLocString("LocalizedName");
            }
            return gff.Root.GetLocString("LocName");
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/template.py:39-43
        // Original: def extract_tag_from_gff(data: bytes) -> str:
        public static string ExtractTagFromGff(byte[] data)
        {
            GFF gff = ParseGff(data);
            return gff.Root.GetString("Tag");
        }
    }
}
