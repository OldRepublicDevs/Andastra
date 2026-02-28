using System;
using System.Collections.Generic;

namespace OdyTools.Editors
{
    public static class EditorWikiMapping
    {
        // Editor class name -> wiki markdown filenames (array allows multiple documents per editor)
        // Empty array means no help available
        public static readonly Dictionary<string, string[]> EditorWikiMap = new Dictionary<string, string[]>
        {
            { "OdyToolARE", new string[] { "GFF-ARE.md", "Bioware-Aurora-AreaFile.md" } },
            { "OdyToolBWM", new string[] { "BWM-File-Format.md" } },
            { "OdyToolDLG", new string[] { "GFF-DLG.md", "Bioware-Aurora-Conversation.md" } },
            { "OdyToolERF", new string[] { "ERF-File-Format.md", "Bioware-Aurora-ERF.md", "Bioware-Aurora-KeyBIF.md" } },
            { "OdyToolFAC", new string[] { "GFF-FAC.md" } },
            { "OdyToolGFF", new string[] { "GFF-File-Format.md", "Bioware-Aurora-GFF.md", "Bioware-Aurora-CommonGFFStructs.md" } }, // Generic GFF editor uses general format doc
            { "OdyToolGIT", new string[] { "GFF-GIT.md", "Bioware-Aurora-KeyBIF.md" } },
            { "OdyToolIFO", new string[] { "GFF-IFO.md", "Bioware-Aurora-IFO.md" } },
            { "OdyToolJRL", new string[] { "GFF-JRL.md", "Bioware-Aurora-Journal.md" } },
            { "OdyToolLTR", new string[] { "LTR-File-Format.md" } },
            { "OdyToolLYT", new string[] { "LYT-File-Format.md" } },
            { "OdyToolLIP", new string[] { "LIP-File-Format.md" } },
            { "OdyToolMDL", new string[] { "MDL-MDX-File-Format.md" } },
            { "OdyToolNSS", new string[] { "NSS-File-Format.md", "NCS-File-Format.md" } },
            { "OdyToolPTH", new string[] { "GFF-PTH.md" } },
            { "OdyToolSAV", new string[] { "GFF-File-Format.md" } }, // Save game uses general GFF format doc
            { "OdyToolSSF", new string[] { "SSF-File-Format.md", "Bioware-Aurora-SSF.md" } },
            { "OdyToolTLK", new string[] { "TLK-File-Format.md", "Bioware-Aurora-TalkTable.md" } },
            { "OdyToolTPC", new string[] { "TPC-File-Format.md" } },
            // Note: OdyToolTXT intentionally not included - plain text, no specific format
            { "OdyTool2DA", new string[] { "2DA-File-Format.md", "Bioware-Aurora-2DA.md" } },
            { "OdyToolUTC", new string[] { "GFF-UTC.md", "Bioware-Aurora-Creature.md" } },
            { "OdyToolUTD", new string[] { "GFF-UTD.md", "Bioware-Aurora-DoorPlaceableGFF.md" } },
            { "OdyToolUTE", new string[] { "GFF-UTE.md", "Bioware-Aurora-Encounter.md" } },
            { "OdyToolUTI", new string[] { "GFF-UTI.md", "Bioware-Aurora-Item.md" } },
            { "OdyToolUTM", new string[] { "GFF-UTM.md", "Bioware-Aurora-Merchant.md" } },
            { "OdyToolUTP", new string[] { "GFF-UTP.md", "Bioware-Aurora-DoorPlaceableGFF.md" } },
            { "OdyToolUTS", new string[] { "GFF-UTS.md", "Bioware-Aurora-SoundObject.md" } },
            { "OdyToolUTT", new string[] { "GFF-UTT.md", "Bioware-Aurora-Trigger.md" } },
            { "OdyToolUTW", new string[] { "GFF-UTW.md", "Bioware-Aurora-Waypoint.md" } },
            { "OdyToolWAV", new string[] { "WAV-File-Format.md" } }, // WAV/Audio file format
            { "OdyToolGUI", new string[] { "GFF-GUI.md" } },
            { "OdyToolMetadata", new string[] { "GFF-File-Format.md" } } // Metadata uses general GFF format doc
        };

        // Helper method to get wiki files for an editor class name
        // Returns null if editor has no wiki files (e.g., OdyToolTXT)
        // Returns array of filenames (can be single file or multiple files)
        public static string[] GetWikiFiles(string editorClassName)
        {
            return EditorWikiMap.TryGetValue(editorClassName, out string[] wikiFiles) ? wikiFiles : null;
        }

        // Helper method to get single wiki file for backward compatibility
        // Returns the first file if multiple files exist, or null if none exist
        public static string GetWikiFile(string editorClassName)
        {
            string[] files = GetWikiFiles(editorClassName);
            return files != null && files.Length > 0 ? files[0] : null;
        }

        /// <summary>
        /// Returns all unique wiki filenames for the help browser sidebar (all docs in the wiki).
        /// Sorted for display; includes index pages like Home and README.
        /// </summary>
        public static IReadOnlyList<string> GetAllWikiFilenames()
        {
            var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string[] files in EditorWikiMap.Values)
            {
                foreach (string f in files)
                    set.Add(f);
            }
            set.Add("Home.md");
            set.Add("README.md");
            var list = new List<string>(set);
            list.Sort((a, b) =>
            {
                bool aIndex = a.Equals("Home.md", StringComparison.OrdinalIgnoreCase) || a.Equals("README.md", StringComparison.OrdinalIgnoreCase);
                bool bIndex = b.Equals("Home.md", StringComparison.OrdinalIgnoreCase) || b.Equals("README.md", StringComparison.OrdinalIgnoreCase);
                if (aIndex && !bIndex) return -1;
                if (!aIndex && bIndex) return 1;
                if (aIndex && bIndex) return string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
                return string.Compare(a, b, StringComparison.OrdinalIgnoreCase);
            });
            return list;
        }
    }
}
