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
    /// Holocron/PyKotor reference finder surface (GFF script ResRef, Tag, TemplateResRef, NCS CONSTS scan).
    /// </summary>
    public class ReferenceSearchOptions
    {
        public bool SearchOverride { get; set; } = true;
        public bool SearchModules { get; set; } = true;
        public bool SearchChitin { get; set; } = true;
        public bool CaseSensitive { get; set; }
        public bool PartialMatch { get; set; }
        public HashSet<ResourceType> FileTypes { get; set; }
        public bool IncludeNcsStrRefScan { get; set; } = true;
        /// <summary>
        /// Minimum CONSTI value indexed as a plausible StrRef during cache scans. Null uses <see cref="NcsConstiScanner.StrRefCandidateMinimum"/>.
        /// Explicit slow-path StrRef queries still match any CONSTI value regardless of this threshold.
        /// </summary>
        public int? NcsStrRefCandidateMinimum { get; set; }
        /// <summary>
        /// When non-empty, only module capsule files whose filename matches at least one glob are scanned.
        /// </summary>
        public List<string> ModuleGlobFilters { get; set; }
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

        public static List<ReferenceSearchResult> FindConversationResRefReferences(
            Installation installation,
            string conversationResRef,
            ReferenceSearchOptions options = null)
        {
            return SearchInstallation(
                installation,
                conversationResRef,
                options,
                (data, needle, searchOptions) => FindConversationResRefInGffBytes(data, needle, searchOptions));
        }

        /// <summary>
        /// Find GFF string/ResRef field values matching <paramref name="value"/>.
        /// When <paramref name="fieldNames"/> is null or empty, all string and ResRef fields are searched.
        /// </summary>
        public static List<ReferenceSearchResult> FindFieldValueReferences(
            Installation installation,
            string value,
            HashSet<string> fieldNames = null,
            ReferenceSearchOptions options = null)
        {
            if (installation == null)
            {
                throw new ArgumentNullException(nameof(installation));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                return new List<ReferenceSearchResult>();
            }

            HashSet<string> normalizedFieldNames = NormalizeFieldNameFilter(fieldNames);
            return SearchInstallation(
                installation,
                value,
                options,
                (data, needle, searchOptions) => FindFieldValueInGffBytes(
                    data,
                    needle,
                    searchOptions,
                    normalizedFieldNames));
        }

        public static List<string> FindFieldValueInGffBytes(
            byte[] data,
            string value,
            ReferenceSearchOptions options = null,
            HashSet<string> fieldNames = null)
        {
            return FindInGffBytes(
                data,
                value,
                options,
                (gffStruct, pathPrefix, needle, searchOptions, paths) =>
                    RecurseGffForFieldValue(gffStruct, pathPrefix, needle, searchOptions, fieldNames, paths));
        }

        private static HashSet<string> NormalizeFieldNameFilter(HashSet<string> fieldNames)
        {
            if (fieldNames == null || fieldNames.Count == 0)
            {
                return null;
            }

            var normalized = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string name in fieldNames)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    normalized.Add(name.Trim());
                }
            }

            return normalized.Count == 0 ? null : normalized;
        }

        private static void RecurseGffForFieldValue(
            GFFStruct gffStruct,
            string pathPrefix,
            string needle,
            ReferenceSearchOptions options,
            HashSet<string> fieldNames,
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
                object fieldValue = tuple.value;
                string fieldPath = string.IsNullOrEmpty(pathPrefix) ? label : pathPrefix + "." + label;

                try
                {
                    bool fieldAllowed = fieldNames == null || fieldNames.Contains(label);

                    if (fieldAllowed)
                    {
                        if (fieldType == GFFFieldType.String && fieldValue is string stringValue &&
                            ValueMatches(stringValue, needle, options))
                        {
                            paths.Add(fieldPath);
                        }
                        else if (fieldType == GFFFieldType.ResRef && fieldValue is ResRef resRef)
                        {
                            string resRefText = resRef.ToString();
                            if (ValueMatches(resRefText, needle, options))
                            {
                                paths.Add(fieldPath);
                            }
                        }
                    }

                    RecurseNestedGff(
                        fieldValue,
                        fieldType,
                        fieldPath,
                        needle,
                        options,
                        paths,
                        (nestedStruct, nestedPath, nestedNeedle, nestedOptions, nestedPaths) =>
                            RecurseGffForFieldValue(nestedStruct, nestedPath, nestedNeedle, nestedOptions, fieldNames, nestedPaths));
                }
                catch
                {
                    // Continue scanning sibling fields.
                }
            }
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

        public static List<string> FindConversationResRefInGffBytes(byte[] data, string conversationNeedle)
        {
            return FindConversationResRefInGffBytes(data, conversationNeedle, null);
        }

        public static List<string> FindConversationResRefInGffBytes(
            byte[] data,
            string conversationNeedle,
            ReferenceSearchOptions options)
        {
            return FindInGffBytes(
                data,
                conversationNeedle,
                options,
                RecurseGffForConversationResRef);
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
                if (resource == null)
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

                    List<string> fieldPaths;
                    if (resource.ResType == ResourceType.NCS)
                    {
                        fieldPaths = FindScriptResRefInNcsBytes(data, trimmedNeedle, options);
                    }
                    else if (resource.ResType.IsGff())
                    {
                        fieldPaths = findInBytes(data, trimmedNeedle, options);
                    }
                    else
                    {
                        continue;
                    }

                    foreach (string fieldPath in fieldPaths)
                    {
                        results.Add(new ReferenceSearchResult
                        {
                            Resource = resource,
                            FieldPath = fieldPath,
                            MatchedValue = trimmedNeedle
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
        /// Scan NCS bytecode for script ResRef string constants (CONSTS-first, byte-scan fallback).
        /// </summary>
        public static List<string> FindScriptResRefInNcsBytes(byte[] data, string resRefNeedle)
        {
            return FindScriptResRefInNcsBytes(data, resRefNeedle, null);
        }

        public static List<string> FindScriptResRefInNcsBytes(
            byte[] data,
            string resRefNeedle,
            ReferenceSearchOptions options)
        {
            var paths = new List<string>();
            var seenOffsets = new HashSet<int>();
            if (data == null || data.Length == 0 || string.IsNullOrWhiteSpace(resRefNeedle))
            {
                return paths;
            }

            options = options ?? new ReferenceSearchOptions();
            string needle = resRefNeedle.Trim();
            StringComparison comparison = options.CaseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            foreach (NcsConstStringScanner.ConstsInstruction instruction in NcsConstStringScanner.ExtractConstsInstructions(data))
            {
                if (!ValueMatches(instruction.Value, needle, options))
                {
                    continue;
                }

                if (seenOffsets.Add(instruction.StringByteOffset))
                {
                    paths.Add(FormatNcsBytecodeFieldPath(instruction.StringByteOffset));
                }
            }

            for (int offset = 0; offset < data.Length; offset++)
            {
                if (!ByteSequenceMatches(data, offset, needle, comparison))
                {
                    continue;
                }

                if (!IsEmbeddedResRefMatch(data, offset, needle.Length))
                {
                    continue;
                }

                if (seenOffsets.Add(offset))
                {
                    paths.Add(FormatNcsBytecodeFieldPath(offset));
                }
            }

            return paths;
        }

        private static string FormatNcsBytecodeFieldPath(int byteOffset)
        {
            return "(NCS bytecode) offset_" + byteOffset;
        }

        private static bool ByteSequenceMatches(byte[] data, int offset, string needle, StringComparison comparison)
        {
            if (offset + needle.Length > data.Length)
            {
                return false;
            }

            for (int i = 0; i < needle.Length; i++)
            {
                char dataChar = (char)data[offset + i];
                char needleChar = needle[i];
                if (string.Compare(dataChar.ToString(), needleChar.ToString(), comparison) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsEmbeddedResRefMatch(byte[] data, int offset, int length)
        {
            if (offset > 0)
            {
                byte prev = data[offset - 1];
                if (prev != 0 && (prev < 32 || prev > 126))
                {
                    return false;
                }
            }

            int nextIndex = offset + length;
            if (nextIndex < data.Length)
            {
                byte next = data[nextIndex];
                if (next != 0 && (next < 32 || next > 126))
                {
                    return false;
                }
            }

            return true;
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

        private static void RecurseGffForConversationResRef(
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
                        string.Equals(label, "Conversation", StringComparison.OrdinalIgnoreCase) &&
                        value is ResRef resRef)
                    {
                        string resRefText = resRef.ToString();
                        if (ValueMatches(resRefText, needle, options))
                        {
                            paths.Add(fieldPath);
                        }
                    }

                    RecurseNestedGff(value, fieldType, fieldPath, needle, options, paths, RecurseGffForConversationResRef);
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
                        if (!ModuleGlobMatcher.MatchesAnyModuleGlob(moduleFile, options?.ModuleGlobFilters))
                        {
                            continue;
                        }

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
