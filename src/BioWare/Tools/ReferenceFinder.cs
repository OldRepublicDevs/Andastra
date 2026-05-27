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
    /// Holocron/PyKotor reference finder surface (GFF script ResRef, Tag, TemplateResRef paths).
    /// NCS bytecode scanning remains deferred.
    /// </summary>
    public class ReferenceSearchOptions
    {
        public bool SearchOverride { get; set; } = true;
        public bool SearchModules { get; set; } = true;
        public bool SearchChitin { get; set; } = true;
        public bool CaseSensitive { get; set; }
        public bool PartialMatch { get; set; }
        public HashSet<ResourceType> FileTypes { get; set; }
    }

    public class ReferenceSearchResult
    {
        public FileResource Resource { get; set; }
        public string FieldPath { get; set; }
        public string MatchedValue { get; set; }

        public string DisplayLabel
        {
            get
            {
                if (Resource == null)
                {
                    return FieldPath ?? string.Empty;
                }

                string name = Resource.ResName + "." + Resource.ResType.Extension;
                if (string.IsNullOrEmpty(FieldPath))
                {
                    return name;
                }

                if (string.IsNullOrEmpty(MatchedValue))
                {
                    return name + " :: " + FieldPath;
                }

                return name + " :: " + FieldPath + " = " + MatchedValue;
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
            return SearchInstallation(
                installation,
                scriptResRef,
                options,
                (data, needle, searchOptions) => FindScriptResRefInGffBytes(data, needle, searchOptions));
        }

        public static List<ReferenceSearchResult> FindTagReferences(
            Installation installation,
            string tag,
            ReferenceSearchOptions options = null)
        {
            return SearchInstallation(
                installation,
                tag,
                options,
                (data, needle, searchOptions) => FindTagInGffBytes(data, needle, searchOptions));
        }

        public static List<ReferenceSearchResult> FindTemplateResRefReferences(
            Installation installation,
            string templateResRef,
            ReferenceSearchOptions options = null)
        {
            return SearchInstallation(
                installation,
                templateResRef,
                options,
                (data, needle, searchOptions) => FindTemplateResRefInGffBytes(data, needle, searchOptions));
        }

        /// <summary>
        /// Find GFF field paths whose ResRef value matches <paramref name="resRefNeedle"/>.
        /// </summary>
        public static List<string> FindScriptResRefInGffBytes(byte[] data, string resRefNeedle)
        {
            return FindScriptResRefInGffBytes(data, resRefNeedle, null);
        }

        public static List<string> FindScriptResRefInGffBytes(
            byte[] data,
            string resRefNeedle,
            ReferenceSearchOptions options)
        {
            return FindInGffBytes(
                data,
                resRefNeedle,
                options,
                RecurseGffForScriptResRef);
        }

        public static List<string> FindTagInGffBytes(byte[] data, string tagNeedle)
        {
            return FindTagInGffBytes(data, tagNeedle, null);
        }

        public static List<string> FindTagInGffBytes(
            byte[] data,
            string tagNeedle,
            ReferenceSearchOptions options)
        {
            return FindInGffBytes(
                data,
                tagNeedle,
                options,
                RecurseGffForTag);
        }

        public static List<string> FindTemplateResRefInGffBytes(byte[] data, string templateNeedle)
        {
            return FindTemplateResRefInGffBytes(data, templateNeedle, null);
        }

        public static List<string> FindTemplateResRefInGffBytes(
            byte[] data,
            string templateNeedle,
            ReferenceSearchOptions options)
        {
            return FindInGffBytes(
                data,
                templateNeedle,
                options,
                RecurseGffForTemplateResRef);
        }

        private delegate void GffRecurseAction(
            GFFStruct gffStruct,
            string pathPrefix,
            string needle,
            ReferenceSearchOptions options,
            List<string> paths);

        private static List<string> FindInGffBytes(
            byte[] data,
            string needle,
            ReferenceSearchOptions options,
            GffRecurseAction recurseAction)
        {
            if (data == null || data.Length == 0 || string.IsNullOrWhiteSpace(needle))
            {
                return new List<string>();
            }

            options = options ?? new ReferenceSearchOptions();
            string trimmedNeedle = needle.Trim();
            var paths = new List<string>();

            try
            {
                GFF gff = new GFFBinaryReader(data).Load();
                recurseAction(gff.Root, string.Empty, trimmedNeedle, options, paths);
            }
            catch
            {
                return new List<string>();
            }

            return paths;
        }

        private static List<ReferenceSearchResult> SearchInstallation(
            Installation installation,
            string needle,
            ReferenceSearchOptions options,
            Func<byte[], string, ReferenceSearchOptions, List<string>> findInBytes)
        {
            if (installation == null)
            {
                throw new ArgumentNullException(nameof(installation));
            }

            if (string.IsNullOrWhiteSpace(needle))
            {
                return new List<ReferenceSearchResult>();
            }

            options = options ?? new ReferenceSearchOptions();
            string trimmedNeedle = needle.Trim();
            var results = new List<ReferenceSearchResult>();

            foreach (FileResource resource in EnumerateResources(installation, options))
            {
                if (resource == null || !resource.ResType.IsGff())
                {
                    continue;
                }

                if (options.FileTypes != null && options.FileTypes.Count > 0 &&
                    !options.FileTypes.Contains(resource.ResType))
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

                    List<string> fieldPaths = findInBytes(data, trimmedNeedle, options);
                    foreach (string fieldPath in fieldPaths)
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

        private static bool ValueMatches(string haystack, string needle, ReferenceSearchOptions options)
        {
            if (haystack == null)
            {
                haystack = string.Empty;
            }

            if (needle == null)
            {
                needle = string.Empty;
            }

            options = options ?? new ReferenceSearchOptions();
            StringComparison comparison = options.CaseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            if (options.PartialMatch)
            {
                return haystack.IndexOf(needle, comparison) >= 0;
            }

            return string.Equals(haystack, needle, comparison);
        }

        private static void RecurseGffForScriptResRef(
            GFFStruct gffStruct,
            string pathPrefix,
            string needle,
            ReferenceSearchOptions options,
            List<string> paths)
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
                        string resRefText = resRef.ToString();
                        if (ValueMatches(resRefText, needle, options))
                        {
                            paths.Add(fieldPath);
                        }
                    }

                    RecurseNestedGff(value, fieldType, fieldPath, needle, options, paths, RecurseGffForScriptResRef);
                }
                catch
                {
                    // Continue scanning sibling fields.
                }
            }
        }

        private static void RecurseGffForTag(
            GFFStruct gffStruct,
            string pathPrefix,
            string needle,
            ReferenceSearchOptions options,
            List<string> paths)
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
                    if (fieldType == GFFFieldType.String &&
                        string.Equals(label, "Tag", StringComparison.OrdinalIgnoreCase) &&
                        value is string tagValue &&
                        ValueMatches(tagValue, needle, options))
                    {
                        paths.Add(fieldPath);
                    }

                    RecurseNestedGff(value, fieldType, fieldPath, needle, options, paths, RecurseGffForTag);
                }
                catch
                {
                    // Continue scanning sibling fields.
                }
            }
        }

        private static void RecurseGffForTemplateResRef(
            GFFStruct gffStruct,
            string pathPrefix,
            string needle,
            ReferenceSearchOptions options,
            List<string> paths)
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
                    if (fieldType == GFFFieldType.ResRef &&
                        string.Equals(label, "TemplateResRef", StringComparison.OrdinalIgnoreCase) &&
                        value is ResRef resRef)
                    {
                        string resRefText = resRef.ToString();
                        if (ValueMatches(resRefText, needle, options))
                        {
                            paths.Add(fieldPath);
                        }
                    }

                    RecurseNestedGff(value, fieldType, fieldPath, needle, options, paths, RecurseGffForTemplateResRef);
                }
                catch
                {
                    // Continue scanning sibling fields.
                }
            }
        }

        private static void RecurseNestedGff(
            object value,
            GFFFieldType fieldType,
            string fieldPath,
            string needle,
            ReferenceSearchOptions options,
            List<string> paths,
            GffRecurseAction recurseAction)
        {
            if (fieldType == GFFFieldType.Struct && value is GFFStruct nestedStruct)
            {
                recurseAction(nestedStruct, fieldPath, needle, options, paths);
            }

            if (fieldType == GFFFieldType.List && value is GFFList list)
            {
                for (int idx = 0; idx < list.Count; idx++)
                {
                    if (list[idx] is GFFStruct listStruct)
                    {
                        recurseAction(listStruct, fieldPath + "[" + idx + "]", needle, options, paths);
                    }
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
