using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using BioWare.Common;

namespace BioWare.Resource.Formats.GFF
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
            gff3Element.SetAttributeValue("type", gff.Content.ToFourCC().Trim());
            var structElement = CreateStructElement(gff.Root);
            gff3Element.Add(structElement);

            return new XDocument(gff3Element);
        }

        private XElement CreateStructElement(GFFStruct gffStruct)
        {
            var structElement = new XElement("struct");
            structElement.SetAttributeValue("id", gffStruct.StructId);

            foreach ((string fieldName, GFFFieldType fieldType, object fieldValue) in gffStruct)
            {
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
                case GFFFieldType.Int8:
                    element.Value = ((sbyte)value).ToString();
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
                case GFFFieldType.UInt64:
                    element.Value = ((ulong)value).ToString();
                    break;
                case GFFFieldType.Int64:
                    element.Value = ((long)value).ToString();
                    break;
                case GFFFieldType.Single:
                    element.Value = ((float)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case GFFFieldType.Double:
                    element.Value = ((double)value).ToString(CultureInfo.InvariantCulture);
                    break;
                case GFFFieldType.String:
                    element.Value = value?.ToString() ?? string.Empty;
                    break;
                case GFFFieldType.ResRef:
                    element.Value = ((ResRef)value).ToString();
                    break;
                case GFFFieldType.LocalizedString:
                    CreateLocStringElement(element, (LocalizedString)value);
                    break;
                case GFFFieldType.List:
                    CreateListElement(element, (GFFList)value);
                    break;
                case GFFFieldType.Binary:
                    element.Value = Convert.ToBase64String((byte[])value);
                    break;
                case GFFFieldType.Struct:
                    var structElement = CreateStructElement((GFFStruct)value);
                    element.Add(structElement);
                    break;
                case GFFFieldType.Vector3:
                    CreateVectorElement(element, (Vector3)value);
                    break;
                case GFFFieldType.Vector4:
                    CreateVectorElement(element, (Vector4)value);
                    break;
                default:
                    throw new ArgumentException($"Unsupported field type: {fieldType}");
            }

            return element;
        }

        private void CreateLocStringElement(XElement parentElement, LocalizedString locString)
        {
            if (locString == null)
            {
                return;
            }

            if (locString != null && locString.StringRef >= 0)
            {
                parentElement.SetAttributeValue("strref", locString.StringRef.ToString());
            }

            // Add only explicitly stored substrings. Scanning every known
            // language/gender pair is wasteful and can emit fallback values.
            foreach ((Language language, Gender gender, string text) in locString)
            {
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

        private void CreateListElement(XElement parentElement, GFFList gffList)
        {
            for (int i = 0; i < gffList.Count; i++)
            {
                var structElement = CreateStructElement(gffList.At(i));
                parentElement.Add(structElement);
            }
        }

        private static void CreateVectorElement(XElement parentElement, Vector3 vector)
        {
            parentElement.SetAttributeValue("x", vector.X.ToString(CultureInfo.InvariantCulture));
            parentElement.SetAttributeValue("y", vector.Y.ToString(CultureInfo.InvariantCulture));
            parentElement.SetAttributeValue("z", vector.Z.ToString(CultureInfo.InvariantCulture));
        }

        private static void CreateVectorElement(XElement parentElement, Vector4 vector)
        {
            parentElement.SetAttributeValue("x", vector.X.ToString(CultureInfo.InvariantCulture));
            parentElement.SetAttributeValue("y", vector.Y.ToString(CultureInfo.InvariantCulture));
            parentElement.SetAttributeValue("z", vector.Z.ToString(CultureInfo.InvariantCulture));
            parentElement.SetAttributeValue("w", vector.W.ToString(CultureInfo.InvariantCulture));
        }

        private string GetXmlType(GFFFieldType fieldType)
        {
            switch (fieldType)
            {
                case GFFFieldType.UInt8:
                    return "byte";
                case GFFFieldType.Int8:
                    return "int8";
                case GFFFieldType.Int32:
                    return "int32";
                case GFFFieldType.UInt32:
                    return "uint32";
                case GFFFieldType.Int16:
                    return "int16";
                case GFFFieldType.UInt16:
                    return "uint16";
                case GFFFieldType.UInt64:
                    return "uint64";
                case GFFFieldType.Int64:
                    return "int64";
                case GFFFieldType.Single:
                    return "float";
                case GFFFieldType.Double:
                    return "double";
                case GFFFieldType.String:
                    return "exostring";
                case GFFFieldType.ResRef:
                    return "resref";
                case GFFFieldType.LocalizedString:
                    return "locstring";
                case GFFFieldType.Binary:
                    return "binary";
                case GFFFieldType.List:
                    return "list";
                case GFFFieldType.Struct:
                    return "struct";
                case GFFFieldType.Vector3:
                    return "vector3";
                case GFFFieldType.Vector4:
                    return "vector4";
                default:
                    throw new ArgumentException($"Unsupported field type: {fieldType}");
            }
        }
    }
}
