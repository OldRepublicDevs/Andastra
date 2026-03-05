using System;
using System.IO;
using BioWare.Common;

namespace BioWare.Resource.Formats.LIP
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/lip/lip_auto.py
    // Original: detect_lip, read_lip, write_lip, bytes_lip functions
    public static class LIPAuto
    {
        private const string UnsupportedLipFormatMessage = "Unsupported format specified; use LIP or LIP_XML or LIP_JSON.";

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/lip/lip_auto.py:16-60
        // Original: def detect_lip(source: SOURCE_TYPES, offset: int = 0) -> ResourceType
        public static ResourceType DetectLip(object source, int offset = 0)
        {
            ResourceType Check(string first4)
            {
                if (first4 == "LIP ")
                {
                    return ResourceType.LIP;
                }
                if (first4.Contains("<"))
                {
                    return ResourceType.LIP_XML;
                }
                if (first4.Contains("{"))
                {
                    return ResourceType.LIP_JSON;
                }
                return ResourceType.INVALID;
            }

            ResourceType fileFormat;
            try
            {
                using (var reader = RawBinaryReader.FromAuto(source, offset))
                {
                    fileFormat = Check(reader.ReadString(4));
                }
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (DirectoryNotFoundException)
            {
                throw;
            }
            catch (IOException)
            {
                fileFormat = ResourceType.INVALID;
            }

            return fileFormat;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/lip/lip_auto.py:63-99
        // Original: def read_lip(source: SOURCE_TYPES, offset: int = 0, size: int | None = None) -> LIP
        public static LIP ReadLip(object source, int offset = 0, int? size = null)
        {
            ResourceType fileFormat = DetectLip(source, offset);
            int sizeValue = size ?? 0;

            if (fileFormat == ResourceType.LIP)
            {
                return ReadLipFromSource(
                    source,
                    filepath => new LIPBinaryReader(filepath, offset, sizeValue).Load(),
                    bytes => new LIPBinaryReader(bytes, offset, sizeValue).Load(),
                    stream => new LIPBinaryReader(stream, offset, sizeValue).Load(),
                    "binary LIP");
            }
            if (fileFormat == ResourceType.LIP_XML)
            {
                return ReadLipFromSource(
                    source,
                    filepath => new LIPXMLReader(filepath, offset, sizeValue).Load(),
                    bytes => new LIPXMLReader(bytes, offset, sizeValue).Load(),
                    stream => new LIPXMLReader(stream, offset, sizeValue).Load(),
                    "XML LIP");
            }
            if (fileFormat == ResourceType.LIP_JSON)
            {
                return ReadLipFromSource(
                    source,
                    filepath => new LIPJSONReader(filepath, offset, sizeValue).Load(),
                    bytes => new LIPJSONReader(bytes, offset, sizeValue).Load(),
                    stream => new LIPJSONReader(stream, offset, sizeValue).Load(),
                    "JSON LIP");
            }
            throw new ArgumentException("Failed to determine the format of the LIP file.");
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/lip/lip_auto.py:102-129
        // Original: def write_lip(lip: LIP, target: TARGET_TYPES, file_format: ResourceType = ResourceType.LIP)
        public static void WriteLip(LIP lip, object target, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.LIP;
            if (lip == null) throw new ArgumentNullException(nameof(lip));
            ValidateLipSerializationFormat(format, nameof(fileFormat));

            if (format == ResourceType.LIP)
            {
                WriteLipToTarget(
                    target,
                    filepath => new LIPBinaryWriter(lip, filepath).Write(),
                    stream => new LIPBinaryWriter(lip, stream).Write(),
                    "binary LIP");
            }
            else if (format == ResourceType.LIP_XML)
            {
                WriteLipToTarget(
                    target,
                    filepath => new LIPXMLWriter(lip, filepath).Write(),
                    stream => new LIPXMLWriter(lip, stream).Write(),
                    "XML LIP");
            }
            else
            {
                WriteLipToTarget(
                    target,
                    filepath => new LIPJSONWriter(lip, filepath).Write(),
                    stream => new LIPJSONWriter(lip, stream).Write(),
                    "JSON LIP");
            }
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/lip/lip_auto.py:132-155
        // Original: def bytes_lip(lip: LIP, file_format: ResourceType = ResourceType.LIP) -> bytes
        // Matching BWMAuto.BytesBwm pattern - use constructor with no target and call Data()
        public static byte[] BytesLip(LIP lip, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.LIP;
            if (lip == null) throw new ArgumentNullException(nameof(lip));
            ValidateLipSerializationFormat(format, nameof(fileFormat));

            if (format == ResourceType.LIP)
            {
                return WriteLipBytes(new DisposableLipBinaryWriter(lip));
            }
            else if (format == ResourceType.LIP_XML)
            {
                return WriteLipBytes(new DisposableLipXmlWriter(lip));
            }
            else
            {
                return WriteLipBytes(new DisposableLipJsonWriter(lip));
            }
        }

        private static void ValidateLipSerializationFormat(ResourceType format, string formatParamName)
        {
            if (format != ResourceType.LIP && format != ResourceType.LIP_XML && format != ResourceType.LIP_JSON)
            {
                throw new ArgumentException(UnsupportedLipFormatMessage, formatParamName);
            }
        }

        /// <summary>
        /// Dispatches LIP input source handling across path/bytes/stream loaders.
        /// </summary>
        private static LIP ReadLipFromSource(
            object source,
            Func<string, LIP> readFromPath,
            Func<byte[], LIP> readFromBytes,
            Func<Stream, LIP> readFromStream,
            string formatName)
        {
            if (source is string filepath)
            {
                return readFromPath(filepath);
            }

            byte[] data = ResourceAutoHelpers.SourceDispatcher.ToBytes(source);
            return readFromBytes(data);
        }

        private static void WriteLipToTarget(object target, Action<string> writeToPath, Action<Stream> writeToStream, string formatName)
        {
            ResourceAutoHelpers.SourceDispatcher.DispatchWrite(target, writeToPath, writeToStream, formatName);
        }

        private static byte[] WriteLipBytes(IDisposableLipWriter writer)
        {
            using (writer)
            {
                writer.Write();
                return writer.Data();
            }
        }

        private interface IDisposableLipWriter : IDisposable
        {
            void Write();
            byte[] Data();
        }

        private sealed class DisposableLipBinaryWriter : IDisposableLipWriter
        {
            private readonly LIPBinaryWriter _writer;

            public DisposableLipBinaryWriter(LIP lip)
            {
                _writer = new LIPBinaryWriter(lip);
            }

            public void Write() => _writer.Write();
            public byte[] Data() => _writer.Data();
            public void Dispose() => _writer.Dispose();
        }

        private sealed class DisposableLipXmlWriter : IDisposableLipWriter
        {
            private readonly LIPXMLWriter _writer;

            public DisposableLipXmlWriter(LIP lip)
            {
                _writer = new LIPXMLWriter(lip);
            }

            public void Write() => _writer.Write();
            public byte[] Data() => _writer.Data();
            public void Dispose() => _writer.Dispose();
        }

        private sealed class DisposableLipJsonWriter : IDisposableLipWriter
        {
            private readonly LIPJSONWriter _writer;

            public DisposableLipJsonWriter(LIP lip)
            {
                _writer = new LIPJSONWriter(lip);
            }

            public void Write() => _writer.Write();
            public byte[] Data() => _writer.Data();
            public void Dispose() => _writer.Dispose();
        }
    }
}

