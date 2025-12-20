using System.Collections.Generic;

namespace HolocronToolset.Editors
{
    // Matching PyKotor implementation at Tools/HolocronToolset/src/toolset/gui/editors/editor_wiki_mapping.py
    // Original: EDITOR_WIKI_MAP: dict[str, str | None]
    public static class EditorWikiMapping
    {
        // Editor class name -> wiki markdown filenames (array allows multiple documents per editor)
        // Empty array means no help available
        public static readonly Dictionary<string, string[]> EditorWikiMap = new Dictionary<string, string[]>
        {
            { "AREEditor", new string[] { "GFF-ARE.md", "Bioware-Aurora-AreaFile.md" } },
            { "BWMEditor", new string[] { "BWM-File-Format.md" } },
            { "DLGEditor", new string[] { "GFF-DLG.md", "Bioware-Aurora-Conversation.md" } },
            { "ERFEditor", new string[] { "ERF-File-Format.md", "Bioware-Aurora-ERF.md", "Bioware-Aurora-KeyBIF.md" } },
            { "GFFEditor", new string[] { "GFF-File-Format.md", "Bioware-Aurora-GFF.md", "Bioware-Aurora-CommonGFFStructs.md" } }, // Generic GFF editor uses general format doc
            { "GITEditor", new string[] { "GFF-GIT.md", "Bioware-Aurora-KeyBIF.md" } },
            { "IFOEditor", new string[] { "GFF-IFO.md", "Bioware-Aurora-IFO.md" } },
            { "JRLEditor", new string[] { "GFF-JRL.md", "Bioware-Aurora-Journal.md" } },
            { "LTREditor", new string[] { "LTR-File-Format.md" } },
            { "LYTEditor", new string[] { "LYT-File-Format.md" } },
            { "LIPEditor", new string[] { "LIP-File-Format.md" } },
            { "MDLEditor", new string[] { "MDL-MDX-File-Format.md" } },
            { "NSSEditor", new string[] { "NSS-File-Format.md", "NCS-File-Format.md" } },
            { "PTHEditor", new string[] { "GFF-PTH.md" } },
            { "SAVEditor", new string[] { "GFF-File-Format.md" } }, // Save game uses general GFF format doc
            { "SSFEditor", new string[] { "SSF-File-Format.md", "Bioware-Aurora-SSF.md" } },
            { "TLKEditor", new string[] { "TLK-File-Format.md", "Bioware-Aurora-TalkTable.md" } },
            { "TPCEditor", new string[] { "TPC-File-Format.md" } },
            // Note: TXTEditor intentionally not included - plain text, no specific format
            { "TwoDAEditor", new string[] { "2DA-File-Format.md", "Bioware-Aurora-2DA.md" } },
            { "UTCEditor", new string[] { "GFF-UTC.md", "Bioware-Aurora-Creature.md" } },
            { "UTDEditor", new string[] { "GFF-UTD.md", "Bioware-Aurora-DoorPlaceableGFF.md" } },
            { "UTEEditor", new string[] { "GFF-UTE.md", "Bioware-Aurora-Encounter.md" } },
            { "UTIEditor", new string[] { "GFF-UTI.md", "Bioware-Aurora-Item.md" } },
            { "UTMEditor", new string[] { "GFF-UTM.md", "Bioware-Aurora-Merchant.md" } },
            { "UTPEditor", new string[] { "GFF-UTP.md", "Bioware-Aurora-DoorPlaceableGFF.md" } },
            { "UTSEditor", new string[] { "GFF-UTS.md", "Bioware-Aurora-SoundObject.md" } },
            { "UTTEditor", new string[] { "GFF-UTT.md", "Bioware-Aurora-Trigger.md" } },
            { "UTWEditor", new string[] { "GFF-UTW.md", "Bioware-Aurora-Waypoint.md" } },
            { "WAVEditor", new string[] { "WAV-File-Format.md" } }, // WAV/Audio file format
            { "MetadataEditor", new string[] { "GFF-File-Format.md" } } // Metadata uses general GFF format doc
        };

        // Helper method to get wiki files for an editor class name
        // Returns null if editor has no wiki files (e.g., TXTEditor)
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
    }
}
