using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;

namespace BioWare.Resource.Formats.GFF
{

    /// <summary>
    /// Auto-detection and convenience functions for GFF files.
    /// 1:1 port of Python gff_auto.py from pykotor/resource/formats/gff/gff_auto.py
    /// </summary>
    public static class GFFAuto
    {
        private const string UnsupportedGffFormatMessage = "Unsupported format specified; use GFF, GFF_XML, or GFF_JSON.";

        /// <summary>
        /// Compatibility overload for helpers that call <c>ReadGff(source, offset, size)</c>.
        /// </summary>
        public static GFF ReadGff(object source, int offset = 0, int? size = null)
        {
            return ReadGff(source, offset, size, null);
        }

        /// <summary>
        /// Reads the GFF data from the source location with the specified format (GFF, GFF_XML, or GFF_JSON).
        /// 1:1 port of Python read_gff function.
        /// Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/gff/gff_auto.py:66-107
        /// </summary>
        public static GFF ReadGff(object source, int offset = 0, int? size = null, ResourceType fileFormat = null)
        {
            ResourceType format = NormalizeGffFormat(fileFormat);

            if (format == ResourceType.GFF_JSON)
            {
                var reader = new GFFJsonReader();
                if (source is string filePath)
                {
                    return reader.Load(File.ReadAllText(filePath));
                }
                else if (source is byte[] bytes)
                {
                    return reader.Load(bytes);
                }
                else if (source is Stream stream)
                {
                    return reader.Load(stream);
                }
                else
                {
                    throw new ArgumentException("Source must be a file path, byte array, or stream.", nameof(source));
                }
            }
            else if (format == ResourceType.GFF_XML)
            {
                var reader = new GFFXmlReader();
                if (source is string str)
                {
                    // Check if the string is XML content (starts with '<') or a file path
                    // GFFXmlReader.Load(string) expects XML content directly, not a file path
                    // Tests pass raw XML text (e.g., "<gff3>...</gff3>"), so we detect XML by checking for '<'
                    if (!string.IsNullOrWhiteSpace(str) && str.TrimStart().StartsWith("<"))
                    {
                        // Raw XML content - pass directly to reader
                        return reader.Load(str);
                    }
                    else
                    {
                        // File path - read file content first
                        return reader.Load(File.ReadAllText(str));
                    }
                }
                else if (source is byte[] bytes)
                {
                    return reader.Load(bytes);
                }
                else if (source is Stream stream)
                {
                    return reader.Load(stream);
                }
                else
                {
                    throw new ArgumentException("Source must be XML content, file path, byte array, or stream.", nameof(source));
                }
            }
            else if (format.IsGff())
            {
                byte[] data = ResourceAutoHelpers.SourceDispatcher.ToBytes(source);
                var reader = new GFFBinaryReader(data, offset, size ?? 0);
                return reader.Load();
            }
            else
            {
                throw new ArgumentException(UnsupportedGffFormatMessage, nameof(fileFormat));
            }
        }


        /// <summary>
        /// Writes the GFF data to the target location with the specified format (GFF, GFF_XML, or GFF_JSON).
        /// 1:1 port of Python write_gff function.
        /// Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/gff/gff_auto.py:109-143
        /// </summary>
        public static void WriteGff(GFF gff, object target, ResourceType fileFormat = null)
        {
            ResourceType format = NormalizeGffFormat(fileFormat);

            if (format == ResourceType.GFF_JSON)
            {
                ResourceAutoHelpers.SourceDispatcher.WriteText(new GFFJsonWriter().Write(gff), target);
            }
            else if (format == ResourceType.GFF_XML)
            {
                ResourceAutoHelpers.SourceDispatcher.WriteText(new GFFXmlWriter().Write(gff), target);
            }
            else if (format.IsGff())
            {
                // Set content type from filename if not already set
                if (gff.Content == GFFContent.GFF && target is string targetPath && !string.IsNullOrEmpty(targetPath))
                {
                    gff.Content = GFFContentExtensions.FromResName(targetPath);
                }

                var writer = new GFFBinaryWriter(gff);
                byte[] data = writer.Write();

                ResourceAutoHelpers.SourceDispatcher.WriteBytes(data, target);
            }
            else
            {
                throw new ArgumentException(UnsupportedGffFormatMessage, nameof(fileFormat));
            }
        }


        /// <summary>
        /// Returns the GFF data as a byte array.
        /// 1:1 port of Python bytes_gff function.
        /// Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/gff/gff_auto.py:145-169
        /// </summary>
        public static byte[] BytesGff(GFF gff, ResourceType fileFormat = null)
        {
            ResourceType format = NormalizeGffFormat(fileFormat);

            if (format == ResourceType.GFF_JSON)
            {
                return System.Text.Encoding.UTF8.GetBytes(new GFFJsonWriter().Write(gff));
            }
            else if (format == ResourceType.GFF_XML)
            {
                return System.Text.Encoding.UTF8.GetBytes(new GFFXmlWriter().Write(gff));
            }
            else
            {
                return new GFFBinaryWriter(gff).Write();
            }
        }

        private static ResourceType NormalizeGffFormat(ResourceType fileFormat)        {
            ResourceType format = fileFormat ?? ResourceType.GFF;

            // Map plaintext GFF abstractions (e.g. DLG_XML, DLG_JSON) to GFF_XML / GFF_JSON
            if (format.TargetMember != null && format.Contents == "plaintext")
            {
                if (format.Extension != null && format.Extension.IndexOf("json", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return ResourceType.GFF_JSON;
                }

                if (format.Extension != null && format.Extension.IndexOf("xml", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return ResourceType.GFF_XML;
                }
            }

            return format;
        }
    }
}

