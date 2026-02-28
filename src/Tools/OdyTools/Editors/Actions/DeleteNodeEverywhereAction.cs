using System;
using System.Collections.Generic;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using OdyTools.Editors.DLG;

namespace OdyTools.Editors.Actions
{
    /// <summary>
    /// Action for deleting a node everywhere. Undo re-inserts all links that referenced the node.
    /// </summary>
    public class DeleteNodeEverywhereAction : IDLGAction
    {
        private readonly DLGNode _node;
        private readonly List<(DLGStandardItem parent, DLGLink link, int index)> _refs;

        public DeleteNodeEverywhereAction(OdyToolDLG editor, DLGNode node)
        {
            if (editor == null) throw new ArgumentNullException(nameof(editor));
            _node = node ?? throw new ArgumentNullException(nameof(node));
            _refs = new List<(DLGStandardItem parent, DLGLink link, int index)>();

            if (editor.Model?.NodeToItems == null || !editor.Model.NodeToItems.TryGetValue(node, out var items))
                return;

            foreach (var item in items)
            {
                if (item?.Link == null) continue;
                int index;
                if (item.Parent == null)
                {
                    index = editor.CoreDlg?.Starters?.IndexOf(item.Link) ?? -1;
                    _refs.Add((null, item.Link, index));
                }
                else
                {
                    var children = item.Parent.Children;
                    index = -1;
                    for (int i = 0; i < children.Count; i++)
                    {
                        if (children[i] == item) { index = i; break; }
                    }
                    if (index < 0) index = item.Parent.Link?.Node?.Links?.IndexOf(item.Link) ?? -1;
                    _refs.Add((item.Parent, item.Link, index));
                }
            }

            // Sort so starters (parent==null) are first, by index ascending, then children
            _refs.Sort((a, b) =>
            {
                if (a.parent == null && b.parent != null) return -1;
                if (a.parent != null && b.parent == null) return 1;
                if (a.parent == null && b.parent == null) return a.index.CompareTo(b.index);
                return 0;
            });
        }

        public void Apply(OdyToolDLG editor)
        {
            if (editor?.Model == null) return;
            editor.Model.DeleteNodeEverywhere(_node);
        }

        public void Undo(OdyToolDLG editor)
        {
            if (editor?.Model == null || editor.CoreDlg == null) return;
            foreach (var (parent, link, index) in _refs)
            {
                int insertAt = index >= 0 ? index : 0;
                if (parent == null)
                {
                    if (insertAt >= 0 && insertAt <= editor.CoreDlg.Starters.Count)
                    {
                        editor.CoreDlg.Starters.Insert(insertAt, link);
                        editor.Model.InsertStarter(insertAt, link);
                    }
                    else
                    {
                        editor.CoreDlg.Starters.Add(link);
                        editor.Model.AddStarter(link);
                    }
                }
                else
                {
                    editor.Model.InsertLinkToParentAsItem(parent, link, insertAt);
                }
            }
            editor.UpdateTreeView();
        }
    }
}
