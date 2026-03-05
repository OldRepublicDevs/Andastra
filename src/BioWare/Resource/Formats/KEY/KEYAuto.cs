using System;
using System.IO;
using BioWare.Common;
using BioWare.Resource;

namespace BioWare.Resource.Formats.KEY
{
    // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/key/key_auto.py
    // Original: detect_key, read_key, write_key, bytes_key
    public static class KEYAuto
    {
        private const string UnsupportedKeyFormatMessage = "Unsupported format specified; use KEY.";
        private const string UnsupportedKeySourceMessage = "Source must be string, byte[], or Stream for KEY";
        private const string UnsupportedKeyTargetMessage = "Target must be string or Stream for KEY";

        public static ResourceType DetectKey(object source, int offset = 0)
        {
            try
            {
                using (BioWare.Common.RawBinaryReader reader = CreateReader(source, offset, 0))
                {
                    reader.Seek(offset);
                    string fileType = reader.ReadString(4);
                    string fileVersion = reader.ReadString(4);
                    if (fileType == KEY.FileTypeConst && (fileVersion == KEY.FileVersionConst || fileVersion == "V1.1"))
                    {
                        return ResourceType.KEY;
                    }
                }
            }
            catch (Exception)
            {
                // Ignore and fall through to invalid
            }
            return ResourceType.INVALID;
        }

        public static KEY ReadKey(object source, int offset = 0, int? size = null)
        {
            ResourceType format = DetectKey(source, offset);
            if (format != ResourceType.KEY)
            {
                throw new ArgumentException("Invalid KEY file format");
            }

            return ReadKeySource(source, offset, size ?? 0);
        }

        public static void WriteKey(KEY key, object target, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.KEY;
            if (key == null) throw new ArgumentNullException(nameof(key));
            ValidateKeyFormat(format, nameof(fileFormat));

            WriteKeyTarget(
                target,
                filepath => new KEYBinaryWriter(key, filepath).Write(),
                stream => new KEYBinaryWriter(key, stream).Write());
        }

        public static byte[] BytesKey(KEY key, ResourceType fileFormat = null)
        {
            ResourceType format = fileFormat ?? ResourceType.KEY;
            if (key == null) throw new ArgumentNullException(nameof(key));
            ValidateKeyFormat(format, nameof(fileFormat));
            using (var ms = new MemoryStream())
            {
                WriteKey(key, ms, format);
                return ms.ToArray();
            }
        }

        private static void ValidateKeyFormat(ResourceType format, string formatParamName)
        {
            if (format != ResourceType.KEY)
            {
                throw new ArgumentException(UnsupportedKeyFormatMessage, formatParamName);
            }
        }

        private static KEY ReadKeySource(object source, int offset, int size)
        {
            byte[] data = ResourceAutoHelpers.SourceDispatcher.ToBytes(source);
            return new KEYBinaryReader(data, offset, size).Load();
        }

        /// <summary>
        /// Dispatches KEY output to either a filesystem path or stream target.
        /// </summary>
        private static void WriteKeyTarget(object target, Action<string> writeToPath, Action<Stream> writeToStream)
        {
            if (target is string filepath)
            {
                writeToPath(filepath);
                return;
            }
            if (target is Stream stream)
            {
                writeToStream(stream);
                return;
            }

            throw new ArgumentException(UnsupportedKeyTargetMessage);
        }

        private static BioWare.Common.RawBinaryReader CreateReader(object source, int offset, int size)
        {
            if (source is string filepath)
            {
                return BioWare.Common.RawBinaryReader.FromFile(filepath, offset, size > 0 ? (int?)size : null);
            }
            if (source is byte[] bytes)
            {
                return BioWare.Common.RawBinaryReader.FromBytes(bytes, offset, size > 0 ? (int?)size : null);
            }
            if (source is Stream stream)
            {
                return BioWare.Common.RawBinaryReader.FromStream(stream, offset, size > 0 ? (int?)size : null);
            }
            throw new ArgumentException(UnsupportedKeySourceMessage);
        }
    }
}

