using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BioWare.Common;
using BioWare.Resource;
using JetBrains.Annotations;

namespace BioWare.Resource.Formats.GFF.Generics.DLG
{
    /// <summary>
    /// Type of computer interface for dialog.
    /// </summary>
    [PublicAPI]
    public enum DLGComputerType
    {
        Modern = 0,
        Ancient = 1
    }

    /// <summary>
    /// Type of conversation for dialog.
    /// Matches vendor dlg.ui: Human, Computer, Type 3, Type 4, Type 5.
    /// </summary>
    [PublicAPI]
    public enum DLGConversationType
    {
        Human = 0,
        Computer = 1,
        Type3 = 2,
        Type4 = 3,
        Type5 = 4
    }

    /// <summary>
    /// Stores dialog data.
    ///
    /// DLG files are GFF-based format files that store dialog trees with entries, replies,
    /// links, and conversation metadata.
    /// </summary>
    [PublicAPI]
    public sealed class DLG
    {
        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/dlg/base.py:36
        // Original: class DLG:
        public static readonly ResourceType BinaryType = ResourceType.DLG;

        public List<DLGLink> Starters { get; set; } = new List<DLGLink>();
        /// <summary>
        /// Canonical GFF EntryList storage (index-aligned with serialized EntryList).
        /// </summary>
        public List<DLGEntry> EntryList { get; set; } = new List<DLGEntry>();
        /// <summary>
        /// Canonical GFF ReplyList storage (index-aligned with serialized ReplyList).
        /// </summary>
        public List<DLGReply> ReplyList { get; set; } = new List<DLGReply>();
        public List<DLGStunt> Stunts { get; set; } = new List<DLGStunt>();
        /// <summary>
        /// Structural version for cache invalidation in lazy accessors/views.
        /// </summary>
        public int Version { get; private set; }

        // Dialog metadata
        public ResRef AmbientTrack { get; set; } = ResRef.FromBlank();
        public int AnimatedCut { get; set; }
        public ResRef CameraModel { get; set; } = ResRef.FromBlank();
        public DLGComputerType ComputerType { get; set; } = DLGComputerType.Modern;
        public DLGConversationType ConversationType { get; set; } = DLGConversationType.Human;
        public ResRef OnAbort { get; set; } = ResRef.FromBlank();
        public ResRef OnEnd { get; set; } = ResRef.FromBlank();
        public int WordCount { get; set; }
        public bool OldHitCheck { get; set; }
        public bool Skippable { get; set; }
        public bool UnequipItems { get; set; }
        public bool UnequipHands { get; set; }
        public string VoId { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        // KotOR 2
        public int AlienRaceOwner { get; set; }
        public int NextNodeId { get; set; }
        public int PostProcOwner { get; set; }
        public int RecordNoVo { get; set; }

        // Deprecated
        public int DelayEntry { get; set; }
        public int DelayReply { get; set; }

        public DLG()
        {
        }

        /// <summary>
        /// Marks the DLG structure as changed (for cache invalidation).
        /// </summary>
        public void Touch()
        {
            unchecked
            {
                Version++;
            }
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/dlg/base.py:307
        // Original: def all_entries(self, *, as_sorted: bool = False) -> list[DLGEntry]:
        public List<DLGEntry> AllEntries(bool asSorted = false)
        {
            List<DLGEntry> entries = EntryList;
            if (!asSorted)
            {
                return entries;
            }
            return entries.OrderBy(e => e.ListIndex == -1).ThenBy(e => e.ListIndex).ToList();
        }

        /// <summary>
        /// Enumerates entries reachable from starters by traversing links.
        /// This preserves the previous traversal semantics of AllEntries().
        /// </summary>
        public IEnumerable<DLGEntry> ReachableEntries(bool asSorted = false)
        {
            var result = ReachableEntriesCore(Starters, new HashSet<DLGEntry>());
            if (!asSorted)
            {
                return result;
            }
            return result.OrderBy(e => e.ListIndex == -1).ThenBy(e => e.ListIndex);
        }

        private IEnumerable<DLGEntry> ReachableEntriesCore(IEnumerable<DLGLink> links, HashSet<DLGEntry> seenEntries)
        {
            if (links == null)
            {
                yield break;
            }
            foreach (DLGLink link in links)
            {
                DLGNode entry = link?.Node;
                if (!(entry is DLGEntry dlgEntry) || seenEntries.Contains(dlgEntry))
                {
                    continue;
                }

                seenEntries.Add(dlgEntry);
                yield return dlgEntry;

                foreach (DLGEntry child in ReachableEntriesCore(dlgEntry.Links?.SelectMany(l => l?.Node?.Links ?? Enumerable.Empty<DLGLink>()), seenEntries))
                {
                    yield return child;
                }
            }
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/dlg/base.py:363
        // Original: def all_replies(self, *, as_sorted: bool = False) -> list[DLGReply]:
        public List<DLGReply> AllReplies(bool asSorted = false)
        {
            List<DLGReply> replies = ReplyList;
            if (!asSorted)
            {
                return replies;
            }
            return replies.OrderBy(r => r.ListIndex == -1).ThenBy(r => r.ListIndex).ToList();
        }

        /// <summary>
        /// Enumerates replies reachable from starters by traversing links.
        /// This preserves the previous traversal semantics of AllReplies().
        /// </summary>
        public IEnumerable<DLGReply> ReachableReplies(bool asSorted = false)
        {
            var starterReplyLinks = Starters.Where(l => l.Node != null).SelectMany(l => l.Node.Links ?? new List<DLGLink>());
            var result = ReachableRepliesCore(starterReplyLinks, new HashSet<DLGReply>());
            if (!asSorted)
            {
                return result;
            }
            return result.OrderBy(r => r.ListIndex == -1).ThenBy(r => r.ListIndex);
        }

        private IEnumerable<DLGReply> ReachableRepliesCore(IEnumerable<DLGLink> links, HashSet<DLGReply> seenReplies)
        {
            if (links == null)
            {
                yield break;
            }
            foreach (DLGLink link in links)
            {
                DLGNode reply = link?.Node;
                if (!(reply is DLGReply dlgReply) || seenReplies.Contains(dlgReply))
                {
                    continue;
                }

                seenReplies.Add(dlgReply);
                yield return dlgReply;

                foreach (DLGReply child in ReachableRepliesCore(dlgReply.Links?.SelectMany(l => l?.Node?.Links ?? Enumerable.Empty<DLGLink>()), seenReplies))
                {
                    yield return child;
                }
            }
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/dlg/base.py:183
        // Original: def find_paths(self, target: DLGEntry | DLGReply | DLGLink) -> list[PureWindowsPath]:
        /// <summary>
        /// Find all paths to a target node or link.
        /// </summary>
        /// <param name="target">The target node or link to find paths to</param>
        /// <returns>A list of paths to the target</returns>
        public List<string> FindPaths(DLGNode target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            List<string> paths = new List<string>();
            // Build starter paths - iterate through starters and build paths from each
            for (int i = 0; i < Starters.Count; i++)
            {
                string starterPath = $"StartingList\\{i}";
                var starterLinks = new List<DLGLink> { Starters[i] };
                _FindPathsRecursive(starterLinks, target, starterPath, paths, new HashSet<object>());
            }
            return paths;
        }

        /// <summary>
        /// Find all paths to a target link.
        /// </summary>
        /// <param name="target">The target link to find paths to</param>
        /// <returns>A list of paths to the target</returns>
        public List<string> FindPaths(DLGLink target)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            List<string> paths = new List<string>();
            DLGNode parentNode = GetLinkParent(target);
            if (parentNode == null)
            {
                if (Starters.Contains(target))
                {
                    paths.Add($"StartingList\\{target.ListIndex}");
                }
                else
                {
                    throw new ArgumentException($"Target {target.GetType().Name} doesn't have a parent, and also not found in starters.");
                }
            }
            else
            {
                _FindPathsForLink(parentNode, target, paths);
            }
            return paths;
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/dlg/base.py:210
        // Original: def _find_paths_for_link(self, parent_node: DLGNode, target: DLGLink, paths: list[PureWindowsPath]):
        private void _FindPathsForLink(DLGNode parentNode, DLGLink target, List<string> paths)
        {
            string nodeListName = parentNode is DLGEntry ? "EntryList" : "ReplyList";
            string parentPath = $"{nodeListName}\\{parentNode.ListIndex}";

            string linkListName = parentNode is DLGEntry ? "RepliesList" : "EntriesList";
            paths.Add($"{parentPath}\\{linkListName}\\{target.ListIndex}");
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/dlg/base.py:236
        // Original: def _find_paths_recursive(self, links: Sequence[DLGLink[T]], target: DLGNode, current_path: PureWindowsPath, paths: list[PureWindowsPath], seen_links_and_nodes: set[DLGNode | DLGLink]):
        private void _FindPathsRecursive(List<DLGLink> links, DLGNode target, string currentPath, List<string> paths, HashSet<object> seenLinksAndNodes)
        {
            foreach (DLGLink link in links)
            {
                if (link == null || seenLinksAndNodes.Contains(link))
                {
                    continue;
                }

                seenLinksAndNodes.Add(link);
                DLGNode node = link.Node;
                if (node == null)
                {
                    continue;
                }

                if (node == target)
                {
                    if (seenLinksAndNodes.Contains(node))
                    {
                        continue;
                    }
                    seenLinksAndNodes.Add(node);
                    string nodeListName = node is DLGEntry ? "EntryList" : "ReplyList";
                    // Add the direct path (matching Python implementation)
                    paths.Add($"{nodeListName}\\{node.ListIndex}");
                    // Add the full path from starter if we have a currentPath
                    // When currentPath is set, we're traversing from a starter, so include full path
                    if (!string.IsNullOrEmpty(currentPath))
                    {
                        // currentPath already includes the path up to the link list
                        // Just append the link index to complete the path
                        paths.Add($"{currentPath}\\{link.ListIndex}");
                    }
                    continue;
                }

                if (!seenLinksAndNodes.Contains(node))
                {
                    seenLinksAndNodes.Add(node);
                    string nodeListName = node is DLGEntry ? "EntryList" : "ReplyList";
                    string linkListName = node is DLGEntry ? "RepliesList" : "EntriesList";
                    string nodePath = $"{nodeListName}\\{node.ListIndex}";
                    // Build new path: currentPath / nodePath / linkListName (matching Python)
                    string newPath = string.IsNullOrEmpty(currentPath) ? $"{nodePath}\\{linkListName}" : $"{currentPath}\\{nodePath}\\{linkListName}";
                    _FindPathsRecursive(node.Links, target, newPath, paths, seenLinksAndNodes);
                }
            }
        }

        // Matching PyKotor implementation at Libraries/PyKotor/src/pykotor/resource/generics/dlg/base.py:288
        // Original: def get_link_parent(self, target_link: DLGLink) -> DLGEntry | DLGReply | DLG | None:
        /// <summary>
        /// Find the parent node of a given link.
        /// </summary>
        /// <param name="targetLink">The link to find the parent for</param>
        /// <returns>The parent node or null if not found</returns>
        public DLGNode GetLinkParent(DLGLink targetLink)
        {
            if (targetLink == null)
            {
                return null;
            }

            if (Starters.Contains(targetLink))
            {
                return null; // Return null to indicate DLG is the parent (handled by caller)
            }

            foreach (DLGEntry entry in AllEntries())
            {
                if (entry.Links.Contains(targetLink))
                {
                    return entry;
                }
            }

            foreach (DLGReply reply in AllReplies())
            {
                if (reply.Links.Contains(targetLink))
                {
                    return reply;
                }
            }

            return null;
        }
    }
}
