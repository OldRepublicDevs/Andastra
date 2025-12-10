using System.Collections.Generic;
using CSharpKOTOR.Common;
using CSharpKOTOR.Resources;
using JetBrains.Annotations;

namespace CSharpKOTOR.Resource.Generics
{
    /// <summary>
    /// Stores journal (quest) data.
    ///
    /// JRL files are GFF-based format files that store journal/quest data including
    /// quest entries, priorities, and planet associations.
    /// </summary>
    [PublicAPI]
    public sealed class JRL
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/jrl.py:29
        // Original: BINARY_TYPE = ResourceType.JRL
        public static readonly ResourceType BinaryType = ResourceType.JRL;

        public List<JRLQuest> Quests { get; set; } = new List<JRLQuest>();

        public JRL()
        {
        }
    }

    /// <summary>
    /// Stores data of an individual quest.
    /// </summary>
    [PublicAPI]
    public sealed class JRLQuest
    {
        public LocalizedString Name { get; set; } = LocalizedString.FromInvalid();
        public int PlanetId { get; set; }
        public int PlotIndex { get; set; }
        public int Priority { get; set; }
        public string Tag { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public List<JRLQuestEntry> Entries { get; set; } = new List<JRLQuestEntry>();

        public JRLQuest()
        {
        }
    }

    /// <summary>
    /// Stores a quest entry (journal entry).
    /// </summary>
    [PublicAPI]
    public sealed class JRLQuestEntry
    {
        public LocalizedString Text { get; set; } = LocalizedString.FromInvalid();
        public int PlotIndex { get; set; }
        public int End { get; set; }
        public int Id { get; set; }

        public JRLQuestEntry()
        {
        }
    }
}
