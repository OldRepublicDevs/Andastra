using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Andastra.Parsing.Common;

namespace Andastra.Parsing.Formats.GFF
{
    /// <summary>
    /// Writes GFF data to XML format.
    /// Generates GFF3 XML format compatible with PyKotor test files.
    /// </summary>
    public class GFFXmlWriter
    {
        /// <summary>
        /// Writes a GFF to XML string.
        /// </summary>
        public string Write(GFF gff)
        {
            var doc = CreateXmlDocument(gff);
            return doc.ToString();
        }

        /// <summary>
        /// Writes a GFF to XML stream.
        /// </summary>
        public void Write(GFF gff, Stream stream)
        {
            var doc = CreateXmlDocument(gff);
            doc.Save(stream);
        }

        private XDocument CreateXmlDocument(GFF gff)
        {
            var gff3Element = new XElement("gff3");
            var structElement = CreateStructElement(gff.Root);
            gff3Element.Add(structElement);

            return new XDocument(gff3Element);
        }

        private XElement CreateStructElement(GFFStruct gffStruct)
        {
            var structElement = new XElement("struct");
            structElement.SetAttributeValue("id", gffStruct.StructId);

            foreach (var fieldName in gffStruct.FieldNames())
            {
                var fieldType = gffStruct.GetFieldType(fieldName);
                var fieldValue = gffStruct.Get(fieldName, fieldType);

                var fieldElement = CreateFieldElement(fieldName, fieldType, fieldValue);
                structElement.Add(fieldElement);
            }

            return structElement;
        }

        private XElement CreateFieldElement(string fieldName, GFFFieldType fieldType, object value)
        {
            string xmlType = GetXmlType(fieldType);
            var element = new XElement(xmlType);
            element.SetAttributeValue("label", fieldName);

            switch (fieldType)
            {
                case GFFFieldType.UInt8:
                    element.Value = ((byte)value).ToString();
                    break;
                case GFFFieldType.Int32:
                    element.Value = ((int)value).ToString();
                    break;
                case GFFFieldType.UInt32:
                    element.Value = ((uint)value).ToString();
                    break;
                case GFFFieldType.Int16:
                    element.Value = ((short)value).ToString();
                    break;
                case GFFFieldType.UInt16:
                    element.Value = ((ushort)value).ToString();
                    break;
                case GFFFieldType.Single:
                    element.Value = ((float)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case GFFFieldType.ExoString:
                    element.Value = (string)value;
                    break;
                case GFFFieldType.ResRef:
                    element.Value = ((ResRef)value).ToString();
                    break;
                case GFFFieldType.LocString:
                    CreateLocStringElement(element, (LocalizedString)value);
                    break;
                case GFFFieldType.List:
                    CreateListElement(element, (GFFList)value);
                    break;
                case GFFFieldType.Struct:
                    var structElement = CreateStructElement((GFFStruct)value);
                    element.Add(structElement);
                    break;
                default:
                    throw new ArgumentException($"Unsupported field type: {fieldType}");
            }

            return element;
        }

        private void CreateLocStringElement(XElement parentElement, LocalizedString locString)
        {
            if (locString.StringRef.HasValue)
            {
                parentElement.SetAttributeValue("strref", locString.StringRef.Value.ToString());
            }

            // Add string elements for each language/gender combination
            foreach (Language language in Enum.GetValues(typeof(Language)))
            {
                foreach (Gender gender in Enum.GetValues(typeof(Gender)))
                {
                    string text = locString.GetString(language, gender);
                    if (!string.IsNullOrEmpty(text))
                    {
                        var stringElement = new XElement("string");
                        stringElement.SetAttributeValue("language", language.ToString());
                        stringElement.SetAttributeValue("gender", gender.ToString());
                        stringElement.Value = text;
                        parentElement.Add(stringElement);
                    }
                }
            }
        }

        private void CreateListElement(XElement parentElement, GFFList gffList)
        {
            for (int i = 0; i < gffList.Count; i++)
            {
                var structElement = CreateStructElement(gffList.At(i));
                parentElement.Add(structElement);
            }
        }

        private string GetXmlType(GFFFieldType fieldType)
        {
            return fieldType switch
            {
                GFFFieldType.UInt8 => "byte",
                GFFFieldType.Int32 => "int32",
                GFFFieldType.UInt32 => "uint32",
                GFFFieldType.Int16 => "int16",
                GFFFieldType.UInt16 => "uint16",
                GFFFieldType.Single => "float",
                GFFFieldType.ExoString => "exostring",
                GFFFieldType.ResRef => "resref",
                GFFFieldType.LocString => "locstring",
                GFFFieldType.List => "list",
                GFFFieldType.Struct => "struct",
                _ => throw new ArgumentException($"Unsupported field type: {fieldType}")
            };
        }
    }
}
