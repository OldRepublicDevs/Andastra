using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;
using BioWare.Resource.Formats.SSF;
using BioWare.Resource.Formats.TLK;
using BioWare.Resource.Formats.TwoDA;
using BioWare.Resource;

namespace BioWare.Tools
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py
    // Original: Format conversion utility functions for KOTOR game resources
    public static class Conversions
    {
        /// <summary>
        /// Reads a binary resource from disk and converts it to an in-memory model.
        /// </summary>
        private static TResource ReadBinaryResource<TResource>(string inputPath, Func<byte[], TResource> loader)
        {
            byte[] data = File.ReadAllBytes(inputPath);
            return loader(data);
        }

        /// <summary>
        /// Converts a binary input file to another format using a strongly-typed model.
        /// </summary>
        private static void ConvertBinaryResource<TResource>(
            string inputPath,
            string outputPath,
            Func<byte[], TResource> loader,
            Action<TResource, string, ResourceType> writer,
            ResourceType outputType)
        {
            TResource resource = ReadBinaryResource(inputPath, loader);
            writer(resource, outputPath, outputType);
        }

        /// <summary>
        /// Converts a resource read through an auto reader format to another output format.
        /// </summary>
        private static void ConvertAutoResource<TResource>(
            string inputPath,
            string outputPath,
            ResourceType inputType,
            ResourceType outputType,
            Func<string, ResourceType, TResource> reader,
            Action<TResource, string, ResourceType> writer)
        {
            TResource resource = reader(inputPath, inputType);
            writer(resource, outputPath, outputType);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:30-43
        // Original: def convert_gff_to_xml(input_path: Path, output_path: Path) -> None:
        public static void ConvertGffToXml(string inputPath, string outputPath)
        {
            ConvertBinaryResource(
                inputPath,
                outputPath,
                data => new GFFBinaryReader(data).Load(),
                GFFAuto.WriteGff,
                ResourceType.GFF_XML);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:46-60
        // Original: def convert_xml_to_gff(input_path: Path, output_path: Path, *, gff_content_type: str | None = None) -> None:
        public static void ConvertXmlToGff(string inputPath, string outputPath, string gffContentType = null)
        {
            ConvertAutoResource(
                inputPath,
                outputPath,
                ResourceType.GFF_XML,
                ResourceType.GFF,
                (path, format) => GFFAuto.ReadGff(path, fileFormat: format),
                GFFAuto.WriteGff);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:63-76
        // Original: def convert_tlk_to_xml(input_path: Path, output_path: Path) -> None:
        public static void ConvertTlkToXml(string inputPath, string outputPath)
        {
            ConvertBinaryResource(
                inputPath,
                outputPath,
                data => new TLKBinaryReader(data).Load(),
                TLKAuto.WriteTlk,
                ResourceType.TLK_XML);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:79-93
        // Original: def convert_xml_to_tlk(input_path: Path, output_path: Path, *, language: Language | None = None) -> None:
        public static void ConvertXmlToTlk(string inputPath, string outputPath, Language? language = null)
        {
            TLK tlk = TLKAuto.ReadTlk(inputPath, ResourceType.TLK_XML);
            if (language.HasValue)
            {
                tlk.Language = language.Value;
            }
            TLKAuto.WriteTlk(tlk, outputPath, ResourceType.TLK);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:96-105
        // Original: def convert_ssf_to_xml(input_path: Path, output_path: Path) -> None:
        public static void ConvertSsfToXml(string inputPath, string outputPath)
        {
            ConvertBinaryResource(
                inputPath,
                outputPath,
                data => new SSFBinaryReader(data).Load(),
                SSFAuto.WriteSsf,
                ResourceType.SSF_XML);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:108-117
        // Original: def convert_xml_to_ssf(input_path: Path, output_path: Path) -> None:
        public static void ConvertXmlToSsf(string inputPath, string outputPath)
        {
            SSF ssf = SSFAuto.ReadSsf(inputPath, 0, null, ResourceType.SSF_XML);
            SSFAuto.WriteSsf(ssf, outputPath, ResourceType.SSF);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:120-134
        // Original: def convert_2da_to_csv(input_path: Path, output_path: Path, *, delimiter: str = ",") -> None:
        public static void Convert2daToCsv(string inputPath, string outputPath, string delimiter = ",")
        {
            ConvertBinaryResource(
                inputPath,
                outputPath,
                data => new TwoDABinaryReader(data).Load(),
                TwoDAAuto.Write2DA,
                ResourceType.TwoDA_CSV);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:137-151
        // Original: def convert_csv_to_2da(input_path: Path, output_path: Path, *, delimiter: str = ",") -> None:
        public static void ConvertCsvTo2da(string inputPath, string outputPath, string delimiter = ",")
        {
            ConvertAutoResource(
                inputPath,
                outputPath,
                ResourceType.TwoDA_CSV,
                ResourceType.TwoDA,
                (path, format) => TwoDAAuto.Read2DA(path, fileFormat: format),
                TwoDAAuto.Write2DA);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:154-163
        // Original: def convert_gff_to_json(input_path: Path, output_path: Path) -> None:
        public static void ConvertGffToJson(string inputPath, string outputPath)
        {
            ConvertBinaryResource(
                inputPath,
                outputPath,
                data => new GFFBinaryReader(data).Load(),
                GFFAuto.WriteGff,
                ResourceType.GFF_JSON);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:166-176
        // Original: def convert_json_to_gff(input_path: Path, output_path: Path, *, gff_content_type: str | None = None) -> None:
        public static void ConvertJsonToGff(string inputPath, string outputPath, string gffContentType = null)
        {
            ConvertAutoResource(
                inputPath,
                outputPath,
                ResourceType.GFF_JSON,
                ResourceType.GFF,
                (path, format) => GFFAuto.ReadGff(path, fileFormat: format),
                GFFAuto.WriteGff);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:179-188
        // Original: def convert_tlk_to_json(input_path: Path, output_path: Path) -> None:
        public static void ConvertTlkToJson(string inputPath, string outputPath)
        {
            ConvertBinaryResource(
                inputPath,
                outputPath,
                data => new TLKBinaryReader(data).Load(),
                TLKAuto.WriteTlk,
                ResourceType.TLK_JSON);
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/tools/conversions.py:191-200
        // Original: def convert_json_to_tlk(input_path: Path, output_path: Path) -> None:
        public static void ConvertJsonToTlk(string inputPath, string outputPath)
        {
            TLK tlk = TLKAuto.ReadTlk(inputPath, ResourceType.TLK_JSON);
            TLKAuto.WriteTlk(tlk, outputPath, ResourceType.TLK);
        }
    }
}
