using System;
using System.IO;
using System.Text;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Andastra.Parsing.Formats.SSF
{
    /// <summary>
    /// Reads SSF files from XML format.
    /// XML is a human-readable format for easier editing of sound set files.
    /// Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/formats/ssf/io_ssf_xml.py:26-59
    /// 
    /// References:
    /// ----------
    ///     vendor/xoreos-tools/src/xml/ssfdumper.cpp (SSF to XML conversion)
    ///     vendor/xoreos-tools/src/xml/ssfcreator.cpp (XML to SSF conversion)
    ///     Note: XML format structure may vary between tools
    /// </summary>
    public class SSFXMLReader
    {
        /// <summary>
        /// Loads an SSF from XML text.
        /// </summary>
        public SSF Load(string xmlText)
        {
            var doc = XDocument.Parse(xmlText);
            return LoadFromXmlDocument(doc);
        }

        /// <summary>
        /// Loads an SSF from XML stream.
        /// </summary>
        public SSF Load(Stream xmlStream)
        {
            var doc = XDocument.Load(xmlStream);
            return LoadFromXmlDocument(doc);
        }

        /// <summary>
        /// Loads an SSF from XML bytes.
        /// </summary>
        public SSF Load(byte[] xmlBytes)
        {
            // Try to decode with UTF-8 first, fallback to other encodings if needed
            string xmlText = null;
            try
            {
                xmlText = Encoding.UTF8.GetString(xmlBytes);
            }
            catch
            {
                try
                {
                    xmlText = Encoding.ASCII.GetString(xmlBytes);
                }
                catch
                {
                    xmlText = Encoding.Default.GetString(xmlBytes);
                }
            }
            return Load(xmlText);
        }

        private SSF LoadFromXmlDocument(XDocument doc)
        {
            var ssf = new SSF();

            // Find root element (can be "xml" or any root)
            XElement rootElement = doc.Root;
            if (rootElement == null)
            {
                throw new InvalidDataException("XML data is not valid XML");
            }

            // Iterate through all child elements looking for "sound" elements
            foreach (var child in rootElement.Elements())
            {
                if (child.Name.LocalName == "sound")
                {
                    try
                    {
                        // Get id attribute (SSFSound enum value)
                        var idAttr = child.Attribute("id");
                        if (idAttr == null || string.IsNullOrEmpty(idAttr.Value))
                        {
                            continue;
                        }

                        if (!int.TryParse(idAttr.Value, out int soundId))
                        {
                            continue;
                        }

                        // Validate sound ID is in range (0-27)
                        if (soundId < 0 || soundId > 27)
                        {
                            continue;
                        }

                        // Get strref attribute (string reference)
                        var strrefAttr = child.Attribute("strref");
                        if (strrefAttr == null || string.IsNullOrEmpty(strrefAttr.Value))
                        {
                            continue;
                        }

                        if (!int.TryParse(strrefAttr.Value, out int stringref))
                        {
                            continue;
                        }

                        // Convert sound ID to SSFSound enum
                        SSFSound sound = (SSFSound)soundId;
                        ssf.SetData(sound, stringref);
                    }
                    catch (ArgumentException)
                    {
                        // Invalid SSFSound enum value, skip this element
                        continue;
                    }
                    catch (OverflowException)
                    {
                        // Invalid integer value, skip this element
                        continue;
                    }
                }
            }

            return ssf;
        }
    }
}

