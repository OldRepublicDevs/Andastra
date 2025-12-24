using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Xml;
using System.Xml.Linq;
using Andastra.Parsing.Common;
using JetBrains.Annotations;

namespace Andastra.Parsing.Formats.GFF
{
    /// <summary>
    /// Reads GFF data from XML format.
    /// Parses GFF3 XML format as used in PyKotor test files.
    /// </summary>
    public class GFFXmlReader
    {
        /// <summary>
        /// Loads a GFF from XML text.
        /// </summary>
        public GFF Load(string xmlText)
        {
            var doc = XDocument.Parse(xmlText);
            return LoadFromXmlDocument(doc);
        }

        /// <summary>
        /// Loads a GFF from XML stream.
        /// </summary>
        public GFF Load(Stream xmlStream)
        {
            var doc = XDocument.Load(xmlStream);
            return LoadFromXmlDocument(doc);
        }

        /// <summary>
        /// Loads a GFF from XML bytes.
        /// </summary>
        public GFF Load(byte[] xmlBytes)
        {
            using var ms = new MemoryStream(xmlBytes);
            return Load(ms);
        }

        private GFF LoadFromXmlDocument(XDocument doc)
        {
            var gff3Element = doc.Element("gff3");
            if (gff3Element == null)
            {
                throw new XmlException("Root element must be 'gff3'");
            }

            var structElement = gff3Element.Element("struct");
            if (structElement == null)
            {
                throw new XmlException("gff3 element must contain a 'struct' element");
            }

            // Parse struct id from attributes
            int structId = 0;
            var idAttr = structElement.Attribute("id");
            if (idAttr != null && !string.IsNullOrEmpty(idAttr.Value))
            {
                if (!int.TryParse(idAttr.Value, out structId))
                {
                    structId = 0;
                }
            }

            // Create GFF with default DLG content type
            var gff = new GFF(GFFContent.DLG);
            gff.Header.FileType = "DLG ";
            gff.Header.FileVersion = "V3.2";

            // Parse the root struct
            gff.Root = ParseStruct(structElement, structId);

            return gff;
        }

        private GFFStruct ParseStruct(XElement structElement, int structId)
        {
            var gffStruct = new GFFStruct(structId);

            foreach (var element in structElement.Elements())
            {
                string fieldName = element.Attribute("label")?.Value;
                if (string.IsNullOrEmpty(fieldName))
                {
                    continue;
                }

                string fieldType = element.Name.LocalName;
                object value = ParseFieldValue(element, fieldType);
                GFFFieldType gffFieldType = GetGFFFieldType(fieldType);

                gffStruct.Set(fieldName, gffFieldType, value);
            }

            return gffStruct;
        }

        private object ParseFieldValue(XElement element, string fieldType)
        {
            return fieldType switch
            {
                "uint8" => byte.Parse(element.Value),
                "byte" => byte.Parse(element.Value),
                "sint32" => int.Parse(element.Value),
                "int32" => int.Parse(element.Value),
                "uint32" => uint.Parse(element.Value),
                "sint16" => short.Parse(element.Value),
                "int16" => short.Parse(element.Value),
                "uint16" => ushort.Parse(element.Value),
                "float" => float.Parse(element.Value, CultureInfo.InvariantCulture),
                "single" => float.Parse(element.Value, CultureInfo.InvariantCulture),
                "exostring" => element.Value,
                "resref" => new ResRef(element.Value),
                "locstring" => ParseLocString(element),
                "list" => ParseList(element),
                "struct" => ParseStruct(element, int.Parse(element.Attribute("id")?.Value ?? "0")),
                _ => throw new XmlException($"Unknown field type: {fieldType}")
            };
        }

        private LocalizedString ParseLocString(XElement locStringElement)
        {
            var locString = new LocalizedString();

            // Check if there's a strref attribute
            var strrefAttr = locStringElement.Attribute("strref");
            if (strrefAttr != null)
            {
                if (int.TryParse(strrefAttr.Value, out int strref))
                {
                    locString.StringRef = strref;
                }
            }

            // Parse string elements
            foreach (var stringElement in locStringElement.Elements("string"))
            {
                var languageAttr = stringElement.Attribute("language");
                var genderAttr = stringElement.Attribute("gender");

                if (languageAttr != null && genderAttr != null)
                {
                    if (Enum.TryParse<Language>(languageAttr.Value, out var language) &&
                        Enum.TryParse<Gender>(genderAttr.Value, out var gender))
                    {
                        locString.SetData(language, gender, stringElement.Value);
                    }
                }
            }

            return locString;
        }

        private GFFList ParseList(XElement listElement)
        {
            var gffList = new GFFList();

            foreach (var structElement in listElement.Elements("struct"))
            {
                int structId = int.Parse(structElement.Attribute("id")?.Value ?? "0");
                var gffStruct = ParseStruct(structElement, structId);
                gffList.Add(gffStruct);
            }

            return gffList;
        }

        private GFFFieldType GetGFFFieldType(string xmlType)
        {
            return xmlType switch
            {
                "uint8" or "byte" => GFFFieldType.UInt8,
                "sint32" or "int32" => GFFFieldType.Int32,
                "uint32" => GFFFieldType.UInt32,
                "sint16" or "int16" => GFFFieldType.Int16,
                "uint16" => GFFFieldType.UInt16,
                "float" or "single" => GFFFieldType.Single,
                "exostring" => GFFFieldType.ExoString,
                "resref" => GFFFieldType.ResRef,
                "locstring" => GFFFieldType.LocString,
                "list" => GFFFieldType.List,
                "struct" => GFFFieldType.Struct,
                _ => throw new XmlException($"Unknown XML field type: {xmlType}")
            };
        }
    }
}
