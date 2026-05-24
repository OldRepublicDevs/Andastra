using System;
using System.Collections.Generic;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;

namespace BioWare.Tools
{
    /// <summary>
    /// Holocron/PyKotor reference finder surface (phase 1: in-memory GFF script ResRef paths).
    /// Installation-wide search and NCS bytecode scanning are deferred.
    /// </summary>
    public static class ReferenceFinder
    {
        /// <summary>
        /// Find GFF field paths whose ResRef value matches <paramref name="resRefNeedle"/>.
        /// </summary>
        public static List<string> FindScriptResRefInGffBytes(byte[] data, string resRefNeedle)
        {
            if (data == null || data.Length == 0)
            {
                return new List<string>();
            }

            if (string.IsNullOrWhiteSpace(resRefNeedle))
            {
                return new List<string>();
            }

            string needle = resRefNeedle.Trim();
            var paths = new List<string>();

            try
            {
                GFF gff = new GFFBinaryReader(data).Load();
                RecurseGff(gff.Root, string.Empty, needle, paths);
            }
            catch
            {
                return new List<string>();
            }

            return paths;
        }

        private static void RecurseGff(GFFStruct gffStruct, string pathPrefix, string needle, List<string> paths)
        {
            if (gffStruct == null)
            {
                return;
            }

            foreach (var tuple in gffStruct)
            {
                string label = tuple.label;
                GFFFieldType fieldType = tuple.fieldType;
                object value = tuple.value;
                string fieldPath = string.IsNullOrEmpty(pathPrefix) ? label : pathPrefix + "." + label;

                try
                {
                    if (fieldType == GFFFieldType.ResRef && value is ResRef resRef)
                    {
                        if (string.Equals(resRef.ToString(), needle, StringComparison.OrdinalIgnoreCase))
                        {
                            paths.Add(fieldPath);
                        }
                    }

                    if (fieldType == GFFFieldType.Struct && value is GFFStruct nestedStruct)
                    {
                        RecurseGff(nestedStruct, fieldPath, needle, paths);
                    }

                    if (fieldType == GFFFieldType.List && value is GFFList list)
                    {
                        for (int idx = 0; idx < list.Count; idx++)
                        {
                            if (list[idx] is GFFStruct listStruct)
                            {
                                RecurseGff(listStruct, fieldPath + "[" + idx + "]", needle, paths);
                            }
                        }
                    }
                }
                catch
                {
                    // Continue scanning sibling fields.
                }
            }
        }
    }
}
