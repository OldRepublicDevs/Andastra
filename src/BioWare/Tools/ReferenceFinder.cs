using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BioWare.Common;
using BioWare.Extract;
using BioWare.Extract.Capsule;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF;
using FileResource = BioWare.Extract.FileResource;

namespace BioWare.Tools
{
    /// <summary>
    /// Holocron/PyKotor reference finder surface (GFF script ResRef paths).
    /// NCS bytecode scanning remains deferred.
    /// </summary>
    public class ReferenceSearchOptions
    {
        public bool SearchOverride { get; set; } = true;
        public bool SearchModules { get; set; } = true;
        public bool SearchChitin { get; set; } = true;
    }

    public class ReferenceSearchResult
    {
        public FileResource Resource { get; set; }
        public string FieldPath { get; set; }

        public string DisplayLabel
        {
            get
            {
                if (Resource == null)
                {
                    return FieldPath ?? string.Empty;
                }

                string name = Resource.ResName + "." + Resource.ResType.Extension;
                return string.IsNullOrEmpty(FieldPath) ? name : name + " :: " + FieldPath;
            }
        }
    }

    public static class ReferenceFinder
    {
        public static List<ReferenceSearchResult> FindScriptReferences(
            Installation installation,
            string scriptResRef,
            ReferenceSearchOptions options = null)
        {
            if (installation == null)
            {
                throw new ArgumentNullException(nameof(installation));
            }

            if (string.IsNullOrWhiteSpace(scriptResRef))
            {
                return new List<ReferenceSearchResult>();
            }

            options = options ?? new ReferenceSearchOptions();
            string needle = scriptResRef.Trim();
            var results = new List<ReferenceSearchResult>();

            foreach (FileResource resource in EnumerateResources(installation, options))
            {
                if (resource == null || !resource.ResType.IsGff())
                {
                    continue;
                }

                try
                {
                    byte[] data = resource.GetData();
                    if (data == null || data.Length == 0)
                    {
                        continue;
                    }

                    foreach (string fieldPath in FindScriptResRefInGffBytes(data, needle))
                    {
                        results.Add(new ReferenceSearchResult
                        {
                            Resource = resource,
                            FieldPath = fieldPath
                        });
                    }
                }
                catch
                {
                    // Skip unreadable resources.
                }
            }

            return results;
        }

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


        private static FileResource TryCreateOverrideFileResource(string file)
        {
            try
            {
                ResourceIdentifier identifier = ResourceIdentifier.FromPath(file);
                if (identifier.ResType != ResourceType.INVALID && !identifier.ResType.IsInvalid)
                {
                    var fileInfo = new FileInfo(file);
                    return new FileResource(identifier.ResName, identifier.ResType, (int)fileInfo.Length, 0, file);
                }
            }
            catch
            {
                // Skip invalid override files.
            }

            return null;
        }

        private static IEnumerable<FileResource> GetModuleResources(string moduleFile)
        {
            var capsule = new LazyCapsule(moduleFile);
            return capsule.GetResources();
        }

        private static IEnumerable<FileResource> EnumerateResources(Installation installation, ReferenceSearchOptions options)
        {
            if (options.SearchChitin)
            {
                foreach (FileResource res in installation.ChitinResources())
                {
                    yield return res;
                }

                foreach (FileResource res in installation.CoreResources())
                {
                    yield return res;
                }
            }

            if (options.SearchOverride)
            {
                string overridePath = installation.OverridePath();
                if (Directory.Exists(overridePath))
                {
                    foreach (string file in Directory.GetFiles(overridePath, "*.*", SearchOption.AllDirectories))
                    {
                        FileResource overrideResource = TryCreateOverrideFileResource(file);
                        if (overrideResource != null)
                        {
                            yield return overrideResource;
                        }
                    }
                }
            }

            if (options.SearchModules)
            {
                string modulesPath = Installation.GetModulesPath(installation.Path);
                if (Directory.Exists(modulesPath))
                {
                    foreach (string moduleFile in Directory.GetFiles(modulesPath, "*.mod")
                        .Concat(Directory.GetFiles(modulesPath, "*.rim"))
                        .Concat(Directory.GetFiles(modulesPath, "*.erf")))
                    {
                        IEnumerable<FileResource> moduleResources;
                        try
                        {
                            moduleResources = GetModuleResources(moduleFile);
                        }
                        catch
                        {
                            continue;
                        }

                        foreach (FileResource res in moduleResources)
                        {
                            yield return res;
                        }
                    }
                }
            }
        }
    }
}
