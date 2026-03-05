using System;
using System.Collections.Generic;
using System.Linq;
using BioWare.Common;
using BioWare.Resource;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using DLGType = BioWare.Resource.Formats.GFF.Generics.DLG.DLG;

namespace OdyTools.Editors.DLG
{
    public static class DLGValidator
    {
        public static List<DLGValidationResult> Validate(DLGType dlg, BioWareGame game)
        {
            var results = new List<DLGValidationResult>();
            if (dlg == null)
            {
                results.Add(new DLGValidationResult
                {
                    Severity = DLGValidationSeverity.Error,
                    RuleId = "dlg.null",
                    Message = "Dialog model is null."
                });
                return results;
            }

            if (dlg.Starters == null || dlg.Starters.Count == 0)
            {
                results.Add(new DLGValidationResult
                {
                    Severity = DLGValidationSeverity.Error,
                    RuleId = "dlg.no_starters",
                    Message = "Dialogue has no starter links."
                });
            }

            var allNodes = new List<DLGNode>();
            if (dlg.EntryList != null)
            {
                allNodes.AddRange(dlg.EntryList.Where(n => n != null));
            }
            if (dlg.ReplyList != null)
            {
                allNodes.AddRange(dlg.ReplyList.Where(n => n != null));
            }

            var reachable = GetReachableNodes(dlg);
            int orphanCount = allNodes.Count(node => !reachable.Contains(node));
            if (orphanCount > 0)
            {
                results.Add(new DLGValidationResult
                {
                    Severity = DLGValidationSeverity.Warning,
                    RuleId = "dlg.orphans",
                    Message = $"Detected {orphanCount} orphaned nodes not reachable from any starter."
                });
            }

            foreach (DLGNode node in allNodes)
            {
                if (node.Text != null && node.Text.StringRef < -1)
                {
                    results.Add(new DLGValidationResult
                    {
                        Severity = DLGValidationSeverity.Error,
                        RuleId = "dlg.invalid_strref",
                        Message = $"Node has invalid StrRef value {node.Text.StringRef}.",
                        NodeReference = node
                    });
                }

                if (game.IsK2())
                {
                    string voice = node.VoResRef?.ToString() ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(voice) && node.Delay == 0)
                    {
                        results.Add(new DLGValidationResult
                        {
                            Severity = DLGValidationSeverity.Warning,
                            RuleId = "dlg.k2.delay_voice",
                            Message = "Node uses Delay=0 with VO_ResRef set. In TSL this can cause dialogue skip behavior.",
                            NodeReference = node
                        });
                    }
                }
            }

            if (game.IsK2() && allNodes.Count > 0)
            {
                int nonDefaultK2Fields = allNodes.Count(node =>
                    node.NodeId != 0 ||
                    node.AlienRaceNode != 0 ||
                    node.PostProcNode != 0 ||
                    node.Script2 != null && !node.Script2.IsBlank());

                if (nonDefaultK2Fields == 0)
                {
                    results.Add(new DLGValidationResult
                    {
                        Severity = DLGValidationSeverity.Info,
                        RuleId = "dlg.k2.fields_default",
                        Message = "All K2-specific node fields are default/blank. Verify source data if this file was migrated from legacy tools."
                    });
                }
            }

            return results;
        }

        private static HashSet<DLGNode> GetReachableNodes(DLGType dlg)
        {
            var reachable = new HashSet<DLGNode>();
            var queue = new Queue<DLGLink>();
            if (dlg.Starters != null)
            {
                foreach (DLGLink starter in dlg.Starters)
                {
                    if (starter != null)
                    {
                        queue.Enqueue(starter);
                    }
                }
            }

            while (queue.Count > 0)
            {
                DLGLink link = queue.Dequeue();
                DLGNode node = link?.Node;
                if (node == null || reachable.Contains(node))
                {
                    continue;
                }

                reachable.Add(node);
                if (node.Links == null)
                {
                    continue;
                }

                foreach (DLGLink child in node.Links)
                {
                    if (child != null)
                    {
                        queue.Enqueue(child);
                    }
                }
            }

            return reachable;
        }
    }
}
