using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using BioWare.Common;
using BioWare.Resource;

namespace BioWare.Resource.Formats.TLK
{

    /// <summary>
    /// Auto-detection and convenience functions for TLK files.
    /// 1:1 port of Python tlk_auto.py from pykotor/resource/formats/tlk/tlk_auto.py
    /// </summary>
    public static class TLKAuto
    {
        private const string UnsupportedTlkFormatMessage = "Unsupported format specified; use TLK, TLK_XML, or TLK_JSON.";
        private const string UnsupportedSourceMessage = "Source must be string, byte[], or Stream";

        /// <summary>
        /// Writes the TLK data to the target location with the specified format (TLK, TLK_XML or TLK_JSON).
        /// 1:1 port of Python write_tlk function.
        /// </summary>
        public static void WriteTlk(TLK tlk, string target, ResourceType fileFormat)
        {
            if (tlk == null) throw new ArgumentNullException(nameof(tlk));
            if (string.IsNullOrWhiteSpace(target)) throw new ArgumentNullException(nameof(target));

            if (fileFormat == ResourceType.TLK)
            {
                var writer = new TLKBinaryWriter(tlk);
                byte[] data = writer.Write();
                File.WriteAllBytes(target, data);
            }
            else if (fileFormat == ResourceType.TLK_JSON)
            {
                var json = SerializeJson(tlk);
                File.WriteAllText(target, json, Encoding.UTF8);
            }
            else if (fileFormat == ResourceType.TLK_XML)
            {
                var doc = CreateXmlDocument(tlk);
                doc.Save(target);
            }
            else
            {
                throw new ArgumentException(UnsupportedTlkFormatMessage, nameof(fileFormat));
            }
        }

        /// <summary>
        /// Returns the TLK data as a byte array.
        /// 1:1 port of Python bytes_tlk function.
        /// </summary>
        public static byte[] BytesTlk(TLK tlk, ResourceType fileFormat)
        {
            if (tlk == null) throw new ArgumentNullException(nameof(tlk));

            if (fileFormat == null || fileFormat == ResourceType.TLK)
            {
                var writer = new TLKBinaryWriter(tlk);
                return writer.Write();
            }
            if (fileFormat == ResourceType.TLK_JSON)
            {
                return Encoding.UTF8.GetBytes(SerializeJson(tlk));
            }
            if (fileFormat == ResourceType.TLK_XML)
            {
                var doc = CreateXmlDocument(tlk);
                using (var ms = new MemoryStream())
                {
                    doc.Save(ms);
                    return ms.ToArray();
                }
            }

            throw new ArgumentException(UnsupportedTlkFormatMessage, nameof(fileFormat));
        }

        /// <summary>
        /// Returns the TLK data as a byte array (defaults to TLK format).
        /// </summary>
        public static byte[] BytesTlk(TLK tlk)
        {
            return BytesTlk(tlk, ResourceType.TLK);
        }

        /// <summary>
        /// Reads a TLK file from a file path or byte array.
        /// Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/tlk/tlk_auto.py
        /// </summary>
        public static TLK ReadTlk(object source)
        {
            return ReadTlk(source, null);
        }

        /// <summary>
        /// Reads a TLK file from file path, byte array, or stream in TLK/TLK_XML/TLK_JSON format.
        /// </summary>
        public static TLK ReadTlk(object source, ResourceType fileFormat)
        {
            ResourceType format = ResolveFormat(source, fileFormat);
            if (format == ResourceType.TLK)
            {
                return ReadBinaryTlk(source);
            }
            else if (format == ResourceType.TLK_JSON)
            {
                string json = ReadText(source);
                var model = JsonSerializer.Deserialize<TlkSerializableModel>(json) ?? new TlkSerializableModel();
                return DeserializeModel(model);
            }
            else if (format == ResourceType.TLK_XML)
            {
                string xml = ReadText(source);
                var doc = XDocument.Parse(xml);
                var root = doc.Root;
                var model = new TlkSerializableModel
                {
                    Language = root?.Attribute("language")?.Value ?? Language.English.ToString()
                };
                var entries = root?.Element("entries")?.Elements("entry") ?? Enumerable.Empty<XElement>();
                foreach (var entry in entries)
                {
                    model.Entries.Add(new TlkSerializableEntry
                    {
                        Text = entry.Element("text")?.Value ?? string.Empty,
                        Voiceover = entry.Attribute("voiceover")?.Value ?? string.Empty,
                        TextPresent = ParseBool(entry.Attribute("textPresent")?.Value, true),
                        SoundPresent = ParseBool(entry.Attribute("soundPresent")?.Value, true),
                        SoundLengthPresent = ParseBool(entry.Attribute("soundLengthPresent")?.Value, true),
                        SoundLength = ParseFloat(entry.Attribute("soundLength")?.Value, 0f)
                    });
                }
                return DeserializeModel(model);
            }

            throw new ArgumentException(UnsupportedSourceMessage);
        }

        private static TLK ReadBinaryTlk(object source)
        {
            byte[] data = ResourceAutoHelpers.SourceDispatcher.ToBytes(source);
            return new TLKBinaryReader(data).Load();
        }

        private static ResourceType ResolveFormat(object source, ResourceType explicitFormat)
        {
            if (explicitFormat != null)
            {
                return explicitFormat;
            }

            if (source is string path)
            {
                if (path.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) return ResourceType.TLK_JSON;
                if (path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)) return ResourceType.TLK_XML;
                return ResourceType.TLK;
            }

            if (source is byte[] data && data.Length > 0)
            {
                byte first = data.FirstOrDefault(b => !char.IsWhiteSpace((char)b));
                if (first == (byte)'{') return ResourceType.TLK_JSON;
                if (first == (byte)'<') return ResourceType.TLK_XML;
                return ResourceType.TLK;
            }

            return ResourceType.TLK;
        }

        private static string ReadText(object source)
        {
            byte[] data = ResourceAutoHelpers.SourceDispatcher.ToBytes(source);
            return Encoding.UTF8.GetString(data);
        }

        private static TLK DeserializeModel(TlkSerializableModel model)
        {
            Language language = Language.English;
            if (!string.IsNullOrWhiteSpace(model.Language))
            {
                Enum.TryParse(model.Language, true, out language);
            }
            var tlk = new TLK(language);
            foreach (var entry in model.Entries)
            {
                var tlkEntry = new TLKEntry(entry.Text ?? string.Empty, new ResRef(entry.Voiceover ?? string.Empty))
                {
                    TextPresent = entry.TextPresent,
                    SoundPresent = entry.SoundPresent,
                    SoundLengthPresent = entry.SoundLengthPresent,
                    SoundLength = entry.SoundLength
                };
                tlk.Entries.Add(tlkEntry);
            }
            return tlk;
        }

        private static bool ParseBool(string value, bool defaultValue)
        {
            bool parsed;
            return bool.TryParse(value, out parsed) ? parsed : defaultValue;
        }

        private static float ParseFloat(string value, float defaultValue)
        {
            float parsed;
            return float.TryParse(value, out parsed) ? parsed : defaultValue;
        }

        /// <summary>
        /// Creates the canonical TLK serializable model used by JSON/XML export paths.
        /// </summary>
        private static TlkSerializableModel CreateSerializableModel(TLK tlk)
        {
            return new TlkSerializableModel
            {
                Language = tlk.Language.ToString(),
                Entries = tlk.Entries.Select(e => new TlkSerializableEntry
                {
                    Text = e.Text ?? string.Empty,
                    Voiceover = e.Voiceover?.ToString() ?? string.Empty,
                    TextPresent = e.TextPresent,
                    SoundPresent = e.SoundPresent,
                    SoundLengthPresent = e.SoundLengthPresent,
                    SoundLength = e.SoundLength
                }).ToList()
            };
        }

        private static string SerializeJson(TLK tlk)
        {
            TlkSerializableModel payload = CreateSerializableModel(tlk);
            return JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
        }

        private static XDocument CreateXmlDocument(TLK tlk)
        {
            return new XDocument(
                new XElement("tlk",
                    new XAttribute("language", tlk.Language.ToString()),
                    new XElement("entries",
                        tlk.Entries.Select((e, i) =>
                            new XElement("entry",
                                new XAttribute("id", i),
                                new XAttribute("voiceover", e.Voiceover?.ToString() ?? string.Empty),
                                new XAttribute("textPresent", e.TextPresent),
                                new XAttribute("soundPresent", e.SoundPresent),
                                new XAttribute("soundLengthPresent", e.SoundLengthPresent),
                                new XAttribute("soundLength", e.SoundLength),
                                new XElement("text", e.Text ?? string.Empty)
                            ))
                    )
                )
            );
        }

        private sealed class TlkSerializableModel
        {
            public string Language { get; set; } = "English";
            public System.Collections.Generic.List<TlkSerializableEntry> Entries { get; set; } =
                new System.Collections.Generic.List<TlkSerializableEntry>();
        }

        private sealed class TlkSerializableEntry
        {
            public string Text { get; set; } = string.Empty;
            public string Voiceover { get; set; } = string.Empty;
            public bool TextPresent { get; set; } = true;
            public bool SoundPresent { get; set; } = true;
            public bool SoundLengthPresent { get; set; } = true;
            public float SoundLength { get; set; }
        }
    }
}

