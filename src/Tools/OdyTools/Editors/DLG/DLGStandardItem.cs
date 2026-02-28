using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BioWare.Resource.Formats.GFF.Generics.DLG;

/// <summary>
/// Represents a standard item in the DLG tree model.
/// </summary>

namespace OdyTools.Editors.DLG
{
    public class DLGStandardItem
    {
        private readonly WeakReference<DLGLink> _linkRef;
        private readonly List<DLGStandardItem> _children = new List<DLGStandardItem>();
        private DLGStandardItem _parent;

        /// <summary>
        /// Gets the link associated with this item, or null if the reference is no longer valid.
        /// </summary>
        public DLGLink Link
        {
            get
            {
                if (_linkRef != null && _linkRef.TryGetTarget(out DLGLink link))
                {
                    return link;
                }
                return null;
            }
        }

        /// <summary>
        /// Gets the number of child items.
        /// </summary>
        public int RowCount => _children.Count;

        /// <summary>
        /// Gets the parent item, or null if this is a root item.
        /// </summary>
        public DLGStandardItem Parent => _parent;

        /// <summary>
        /// Gets all child items.
        /// </summary>
        public IReadOnlyList<DLGStandardItem> Children => _children;

        /// <summary>
        /// Removes a child item from this item.
        /// </summary>
        /// <param name="child">The child item to remove.</param>
        /// <returns>True if the child was removed, false if it was not found.</returns>
        public bool RemoveChild(DLGStandardItem child)
        {
            if (child == null)
            {
                return false;
            }
            if (_children.Remove(child))
            {
                child._parent = null;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Initializes a new instance of DLGStandardItem with the specified link.
        /// </summary>
        public DLGStandardItem(DLGLink link)
        {
            if (link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }
            _linkRef = new WeakReference<DLGLink>(link);
        }

        /// <summary>
        /// Adds a child item to this item.
        /// </summary>
        public void AddChild(DLGStandardItem child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }
            if (child._parent != null)
            {
                child._parent._children.Remove(child);
            }
            child._parent = this;
            _children.Add(child);
        }

        /// <summary>
        /// Inserts a child item at the specified index.
        /// </summary>
        /// <param name="index">The index at which to insert the child.</param>
        /// <param name="child">The child item to insert.</param>
        public void InsertChild(int index, DLGStandardItem child)
        {
            if (child == null)
            {
                throw new ArgumentNullException(nameof(child));
            }
            if (index < 0 || index > _children.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be between 0 and children count");
            }
            if (child._parent != null)
            {
                child._parent._children.Remove(child);
            }
            child._parent = this;
            _children.Insert(index, child);
        }

        /// <summary>
        /// Gets the index of this item in its parent's children list.
        /// </summary>
        public int GetIndex()
        {
            if (_parent == null)
            {
                return -1;
            }
            return _parent._children.IndexOf(this);
        }

        /// <summary>
        /// Gets the child item at the specified row and column.
        /// </summary>
        public DLGStandardItem Child(int row, int column = 0)
        {
            if (row < 0 || row >= _children.Count || column != 0)
            {
                return null;
            }
            return _children[row];
        }

        /// <summary>
        /// Gets whether this item has children.
        /// </summary>
        public bool HasChildren()
        {
            return _children.Count > 0;
        }
    }
}
