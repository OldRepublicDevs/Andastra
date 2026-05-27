using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BioWare.Common;
using BioWare.Extract.Capsule;
using BioWare.Resource;
using BioWare.Resource.Formats.BIF;
using BioWare.Resource.Formats.ERF;
using BioWare.Resource.Formats.KEY;
using BioWare.Resource.Formats.RIM;
using KotorCLI.Logging;

namespace KotorCLI.Commands
{
    internal static class ArchiveCommandHelpers
    {
        internal struct ArchiveResourceEntry
        {
            public string ResRef;
            public ResourceType ResType;
            public int Size;
            public byte[] Data;
        }

        internal static bool MatchesFilter(string text, string pattern, bool caseSensitive = false)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                return true;
            }

            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            StringComparison comparison = caseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            if (pattern.Contains("*") || pattern.Contains("?"))
            {
                RegexOptions options = caseSensitive ? RegexOptions.None : RegexOptions.IgnoreCase;
                string regexPattern = "^" + Regex.Escape(pattern)
                    .Replace("\\*", ".*")
                    .Replace("\\?", ".") + "$";
                return Regex.IsMatch(text, regexPattern, options);
            }

            return text.IndexOf(pattern, comparison) >= 0;
        }

        internal static bool ContentMatches(byte[] data, string pattern, bool caseSensitive)
        {
            if (data == null || data.Length == 0 || string.IsNullOrEmpty(pattern))
            {
                return false;
            }

            string haystack;
            try
            {
                haystack = Encoding.UTF8.GetString(data);
            }
            catch
            {
                haystack = Encoding.ASCII.GetString(data);
            }

            StringComparison comparison = caseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            return haystack.IndexOf(pattern, comparison) >= 0;
        }

        internal static List<ArchiveResourceEntry> ReadArchiveResources(string archivePath, ILogger logger)
        {
            var entries = new List<ArchiveResourceEntry>();
            if (string.IsNullOrWhiteSpace(archivePath) || !File.Exists(archivePath))
            {
                return entries;
            }

            string extension = Path.GetExtension(archivePath).ToLowerInvariant();

            if (extension == ".rim")
            {
                RIM rim = new RIMBinaryReader(archivePath).Load();
                foreach (RIMResource resource in rim)
                {
                    entries.Add(new ArchiveResourceEntry
                    {
                        ResRef = resource.ResRef?.ToString() ?? string.Empty,
                        ResType = resource.ResType ?? ResourceType.INVALID,
                        Size = resource.Data?.Length ?? 0,
                        Data = resource.Data
                    });
                }

                return entries;
            }

            if (extension == ".erf" || extension == ".mod" || extension == ".sav" || extension == ".hak")
            {
                ERF erf = new ERFBinaryReader(archivePath).Load();
                foreach (ERFResource resource in erf)
                {
                    entries.Add(new ArchiveResourceEntry
                    {
                        ResRef = resource.ResRef?.ToString() ?? string.Empty,
                        ResType = resource.ResType ?? ResourceType.INVALID,
                        Size = resource.Data?.Length ?? 0,
                        Data = resource.Data
                    });
                }

                return entries;
            }

            if (extension == ".bif")
            {
                BIF bif = new BIFBinaryReader(archivePath).Load();
                int resourceIndex = 0;
                foreach (BIFResource resource in bif.Resources)
                {
                    entries.Add(new ArchiveResourceEntry
                    {
                        ResRef = "resource_" + resourceIndex.ToString("D5"),
                        ResType = resource.ResType ?? ResourceType.INVALID,
                        Size = resource.Data?.Length ?? 0,
                        Data = resource.Data
                    });
                    resourceIndex++;
                }

                return entries;
            }

            if (extension == ".key")
            {
                KEY key = KEYAuto.ReadKey(archivePath);
                foreach (KeyEntry keyEntry in key.KeyEntries)
                {
                    entries.Add(new ArchiveResourceEntry
                    {
                        ResRef = keyEntry.ResRef?.ToString() ?? string.Empty,
                        ResType = keyEntry.ResType ?? ResourceType.INVALID,
                        Size = 0,
                        Data = null
                    });
                }

                return entries;
            }

            try
            {
                var capsule = new LazyCapsule(archivePath);
                foreach (BioWare.Extract.FileResource resource in capsule.GetResources())
                {
                    byte[] data = null;
                    try
                    {
                        data = resource.GetData();
                    }
                    catch
                    {
                        // Leave data null for list-only paths.
                    }

                    entries.Add(new ArchiveResourceEntry
                    {
                        ResRef = resource.ResName,
                        ResType = resource.ResType,
                        Size = data != null ? data.Length : resource.Size,
                        Data = data
                    });
                }
            }
            catch (Exception ex)
            {
                logger.Error("Unsupported or unreadable archive: " + ex.Message);
            }

            return entries;
        }
    }
}
