using System;
using System.Collections.Generic;
using BioWare.Resource.Formats.GFF.Generics.DLG;
using DLGType = BioWare.Resource.Formats.GFF.Generics.DLG.DLG;

namespace OdyTools.Editors.DLG
{
    /// <summary>
    /// Path-based, lazy tree accessor over DLG flat lists and link indices.
    /// This does not pre-build a physical tree; children are materialized on access and cached by DLG.Version.
    /// </summary>
    public readonly struct DlgTreePath
    {
        public DlgTreePath(int starterIndex, IReadOnlyList<int> childIndices)
        {
            StarterIndex = starterIndex;
            ChildIndices = childIndices ?? Array.Empty<int>();
        }

        public int StarterIndex { get; }
        public IReadOnlyList<int> ChildIndices { get; }
    }

    public sealed class DlgTreeItemView
    {
        private readonly DLGType _dlg;
        private readonly DlgTreePath _path;
        private readonly DlgTreeItemView _parent;

        private int _childrenCacheVersion = -1;
        private List<DlgTreeItemView> _childrenCache = new List<DlgTreeItemView>();

        public DlgTreeItemView(DLGType dlg, DlgTreePath path, DlgTreeItemView parent = null)
        {
            _dlg = dlg ?? throw new ArgumentNullException(nameof(dlg));
            _path = path;
            _parent = parent;
        }

        public DlgTreeItemView Parent => _parent;
        public DlgTreePath Path => _path;

        public DLGLink Link => ResolveLink();

        public IReadOnlyList<DlgTreeItemView> Children
        {
            get
            {
                if (_childrenCacheVersion == _dlg.Version)
                {
                    return _childrenCache;
                }

                _childrenCache = new List<DlgTreeItemView>();
                var link = ResolveLink();
                if (link?.Node?.Links != null)
                {
                    for (int i = 0; i < link.Node.Links.Count; i++)
                    {
                        var nextIndices = new List<int>(_path.ChildIndices.Count + 1);
                        if (_path.ChildIndices.Count > 0)
                        {
                            nextIndices.AddRange(_path.ChildIndices);
                        }
                        nextIndices.Add(i);
                        var childPath = new DlgTreePath(_path.StarterIndex, nextIndices);
                        _childrenCache.Add(new DlgTreeItemView(_dlg, childPath, this));
                    }
                }
                _childrenCacheVersion = _dlg.Version;
                return _childrenCache;
            }
        }

        private DLGLink ResolveLink()
        {
            if (_dlg.Starters == null || _path.StarterIndex < 0 || _path.StarterIndex >= _dlg.Starters.Count)
            {
                return null;
            }

            DLGLink current = _dlg.Starters[_path.StarterIndex];
            if (current == null)
            {
                return null;
            }

            if (_path.ChildIndices == null || _path.ChildIndices.Count == 0)
            {
                return current;
            }

            foreach (int childIndex in _path.ChildIndices)
            {
                var links = current.Node?.Links;
                if (links == null || childIndex < 0 || childIndex >= links.Count)
                {
                    return null;
                }
                current = links[childIndex];
                if (current == null)
                {
                    return null;
                }
            }
            return current;
        }

        public static IReadOnlyList<DlgTreeItemView> CreateRoots(DLGType dlg)
        {
            if (dlg == null || dlg.Starters == null || dlg.Starters.Count == 0)
            {
                return Array.Empty<DlgTreeItemView>();
            }

            var roots = new List<DlgTreeItemView>(dlg.Starters.Count);
            for (int i = 0; i < dlg.Starters.Count; i++)
            {
                roots.Add(new DlgTreeItemView(dlg, new DlgTreePath(i, Array.Empty<int>())));
            }
            return roots;
        }
    }
}
