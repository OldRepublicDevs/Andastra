using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using BioWare.Common;
using BioWare.Resource.Formats.GFF.Generics.DLG;

namespace OdyTools.Editors.DLG
{
    public class DLGModel
    {
        private List<DLGStandardItem> _rootItems = new List<DLGStandardItem>();
        private OdyToolDLG _editor;

        // Note: C# doesn't have WeakKeyDictionary, so we use ConditionalWeakTable which provides similar functionality
        private Dictionary<DLGLink, List<DLGStandardItem>> _linkToItems = new Dictionary<DLGLink, List<DLGStandardItem>>();
        private Dictionary<DLGNode, List<DLGStandardItem>> _nodeToItems = new Dictionary<DLGNode, List<DLGStandardItem>>();

        /// <summary>
        /// Gets the dictionary mapping links to their items.
        /// </summary>
        public Dictionary<DLGLink, List<DLGStandardItem>> LinkToItems => _linkToItems;

        /// <summary>
        /// Gets the dictionary mapping nodes to their items.
        /// </summary>
        public Dictionary<DLGNode, List<DLGStandardItem>> NodeToItems => _nodeToItems;

        public DLGModel()
        {
        }

        public DLGModel(OdyToolDLG editor)
        {
            _editor = editor;
        }

        public int RowCount => _rootItems.Count;

        private int _selectedIndex = -1;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value >= -1 && value < _rootItems.Count)
                {
                    _selectedIndex = value;
                }
            }
        }

        public void ResetModel()
        {
            _rootItems.Clear();
            _selectedIndex = -1;
            _linkToItems.Clear();
            _nodeToItems.Clear();
        }

        /// <summary>
        /// Clears the model (alias for ResetModel for Python compatibility).
        /// </summary>
        public void Clear()
        {
            ResetModel();
        }

        public void AddStarter(DLGLink link)
        {
            if (link == null)
            {
                return;
            }
            RegisterNodeInFlatLists(link.Node);
            var item = new DLGStandardItem(link);
            _rootItems.Add(item);

            // Register in dictionaries
            if (!_linkToItems.ContainsKey(link))
            {
                _linkToItems[link] = new List<DLGStandardItem>();
            }
            if (!_linkToItems[link].Contains(item))
            {
                _linkToItems[link].Add(item);
            }

            if (link.Node != null)
            {
                if (!_nodeToItems.TryGetValue(link.Node, out List<DLGStandardItem> value))
                {
                    value = new List<DLGStandardItem>();
                    _nodeToItems[link.Node] = value;
                }
                if (!value.Contains(item))
                {
                    value.Add(item);
                }
            }

            // Also add to CoreDlg.Starters if editor is available
            if (_editor != null && _editor.CoreDlg != null)
            {
                if (!_editor.CoreDlg.Starters.Contains(link))
                {
                    _editor.CoreDlg.Starters.Add(link);
                    TouchCoreDlg();
                }
            }
        }

        /// <summary>
        /// Inserts a link as a child of the given parent at the specified row.
        /// Used when restoring an orphaned node to a specific position in the tree.
        /// </summary>
        /// <param name="parent">Parent item, or null to insert at root.</param>
        /// <param name="link">The link to insert (will be added to Starters or parent's Links).</param>
        /// <param name="row">Index at which to insert (0-based).</param>
        /// <returns>The new DLGStandardItem wrapping the link.</returns>
        public DLGStandardItem InsertLinkToParentAsItem(DLGStandardItem parent, DLGLink link, int row)
        {
            if (link == null)
            {
                throw new ArgumentNullException(nameof(link));
            }
            RegisterNodeInFlatLists(link.Node);

            var newItem = new DLGStandardItem(link);

            if (parent == null)
            {
                int insertRow = row < 0 || row > _rootItems.Count ? _rootItems.Count : row;
                _rootItems.Insert(insertRow, newItem);
                if (_editor != null && _editor.CoreDlg != null)
                {
                    if (insertRow >= _editor.CoreDlg.Starters.Count)
                    {
                        _editor.CoreDlg.Starters.Add(link);
                    }
                    else
                    {
                        _editor.CoreDlg.Starters.Insert(insertRow, link);
                    }
                    TouchCoreDlg();
                }
            }
            else
            {
                if (parent.Link?.Node == null)
                {
                    throw new InvalidOperationException("Parent item must have a valid link with node.");
                }
                int insertRow = row < 0 || row > parent.RowCount ? parent.RowCount : row;
                if (insertRow >= parent.Link.Node.Links.Count)
                {
                    parent.Link.Node.Links.Add(link);
                }
                else
                {
                    parent.Link.Node.Links.Insert(insertRow, link);
                }
                TouchCoreDlg();
                parent.InsertChild(insertRow, newItem);
            }

            if (!_linkToItems.ContainsKey(link))
            {
                _linkToItems[link] = new List<DLGStandardItem>();
            }
            if (!_linkToItems[link].Contains(newItem))
            {
                _linkToItems[link].Add(newItem);
            }
            if (link.Node != null)
            {
                if (!_nodeToItems.ContainsKey(link.Node))
                {
                    _nodeToItems[link.Node] = new List<DLGStandardItem>();
                }
                if (!_nodeToItems[link.Node].Contains(newItem))
                {
                    _nodeToItems[link.Node].Add(newItem);
                }
            }

            UpdateItemDisplayText(newItem);
            if (parent != null)
            {
                UpdateItemDisplayText(parent);
            }
            if (_editor != null)
            {
                _editor.UpdateTreeView();
            }
            return newItem;
        }

        /// <summary>
        /// Adds a root node to the dialog graph.
        /// </summary>
        public DLGStandardItem AddRootNode()
        {
            var newEntry = new DLGEntry();
            newEntry.PlotIndex = -1;
            var newLink = new DLGLink(newEntry);
            RegisterNodeInFlatLists(newLink.Node);

            var newItem = new DLGStandardItem(newLink);
            _rootItems.Add(newItem);

            // Add to CoreDlg.Starters
            if (_editor != null && _editor.CoreDlg != null)
            {
                _editor.CoreDlg.Starters.Add(newLink);
                TouchCoreDlg();
            }

            UpdateItemDisplayText(newItem);

            // Update tree view if editor is available
            if (_editor != null)
            {
                _editor.UpdateTreeView();
            }

            return newItem;
        }

        /// <summary>
        /// Adds a child node to the specified parent item.
        /// </summary>
        public DLGStandardItem AddChildToItem(DLGStandardItem parentItem, DLGLink link = null)
        {
            if (parentItem == null)
            {
                throw new ArgumentNullException(nameof(parentItem));
            }

            if (parentItem.Link == null)
            {
                throw new InvalidOperationException("Parent item must have a valid link");
            }

            if (link == null)
            {
                // Create new node - if parent is Reply, create Entry; if parent is Entry, create Reply
                DLGNode newNode;
                if (parentItem.Link.Node is DLGReply)
                {
                    newNode = new DLGEntry();
                }
                else
                {
                    newNode = new DLGReply();
                }
                newNode.PlotIndex = -1;
                RegisterNodeInFlatLists(newNode);
                link = new DLGLink(newNode);
            }
            RegisterNodeInFlatLists(link.Node);

            // Link the nodes
            if (parentItem.Link.Node != null)
            {
                link.ListIndex = parentItem.Link.Node.Links.Count;
                parentItem.Link.Node.Links.Add(link);
                TouchCoreDlg();
            }

            var newItem = new DLGStandardItem(link);
            parentItem.AddChild(newItem);

            // Register in dictionaries
            if (!_linkToItems.ContainsKey(link))
            {
                _linkToItems[link] = new List<DLGStandardItem>();
            }
            if (!_linkToItems[link].Contains(newItem))
            {
                _linkToItems[link].Add(newItem);
            }

            if (link.Node != null)
            {
                if (!_nodeToItems.ContainsKey(link.Node))
                {
                    _nodeToItems[link.Node] = new List<DLGStandardItem>();
                }
                if (!_nodeToItems[link.Node].Contains(newItem))
                {
                    _nodeToItems[link.Node].Add(newItem);
                }
            }

            UpdateItemDisplayText(newItem);
            UpdateItemDisplayText(parentItem);

            return newItem;
        }

        /// <summary>
        /// Gets the item at the specified row and column.
        /// </summary>
        public DLGStandardItem Item(int row, int column = 0)
        {
            if (row < 0 || row >= _rootItems.Count || column != 0)
            {
                return null;
            }
            return _rootItems[row];
        }

        private void RegisterNodeInFlatLists(DLGNode node)
        {
            if (node == null || _editor?.CoreDlg == null)
            {
                return;
            }

            if (node is DLGEntry entry)
            {
                if (!_editor.CoreDlg.EntryList.Contains(entry))
                {
                    entry.ListIndex = _editor.CoreDlg.EntryList.Count;
                    _editor.CoreDlg.EntryList.Add(entry);
                }
            }
            else if (node is DLGReply reply)
            {
                if (!_editor.CoreDlg.ReplyList.Contains(reply))
                {
                    reply.ListIndex = _editor.CoreDlg.ReplyList.Count;
                    _editor.CoreDlg.ReplyList.Add(reply);
                }
            }
        }

        private void TouchCoreDlg()
        {
            _editor?.CoreDlg?.Touch();
        }

        /// <summary>
        /// Gets a new list index for a node.
        /// </summary>
        private int GetNewNodeListIndex(DLGNode node)
        {
            if (_editor?.CoreDlg == null)
            {
                return 0;
            }

            if (node is DLGEntry)
            {
                return _editor.CoreDlg.EntryList.Count;
            }
            else if (node is DLGReply)
            {
                return _editor.CoreDlg.ReplyList.Count;
            }
            return 0;
        }

        /// <summary>
        /// Counts the number of references to a node in the UI tree model.
        /// </summary>
        /// <param name="link">The link to count references for.</param>
        /// <returns>The number of references to the node.</returns>
        public int CountItemRefs(DLGLink link)
        {
            if (link?.Node == null)
            {
                return 0;
            }

            if (_nodeToItems.TryGetValue(link.Node, out List<DLGStandardItem> items))
            {
                return items.Count;
            }

            return 0;
        }

        /// <summary>
        /// Checks if an item is a copy (has multiple items referencing the same node).
        /// </summary>
        /// <param name="item">The item to check.</param>
        /// <returns>True if the item is a copy (multiple items reference the same node), false otherwise.</returns>
        private bool IsCopy(DLGStandardItem item)
        {
            if (item?.Link?.Node == null)
            {
                return false;
            }

            // An item is a copy if there are multiple items referencing the same node
            if (_nodeToItems.TryGetValue(item.Link.Node, out List<DLGStandardItem> items))
            {
                return items.Count > 1;
            }

            return false;
        }

        /// <summary>
        /// Updates the display text for an item.
        /// </summary>
        /// <param name="item">The DLGStandardItem to update.</param>
        /// <param name="updateCopies">If true, also updates all copies of this item.</param>
        public void UpdateItemDisplayText(DLGStandardItem item, bool updateCopies = true)
        {
            if (item == null || item.Link == null || _editor == null)
            {
                return;
            }

            var link = item.Link;
            var node = link.Node;
            if (node == null)
            {
                return;
            }

            // Determine color and prefix based on node type and whether it's a copy
            string color = "#646464";
            string prefix = "N";
            string extraNodeInfo = "";

            bool isCopy = IsCopy(item);
            if (node is DLGEntry)
            {
                color = isCopy ? "#D25A5A" : "#FF0000";
                prefix = "E";
            }
            else if (node is DLGReply)
            {
                color = isCopy ? "#5A5AD2" : "#0000FF";
                prefix = "R";
                extraNodeInfo = " This means the player will not see this reply as a choice, and will (continue) to next entry.";
            }

            // Get text from node
            string text;
            if (_editor.Installation == null)
            {
                text = node.Text?.ToString() ?? "";
            }
            else
            {
                text = _editor.Installation.String(node.Text, "") ?? "";
            }

            // Format display text based on node state
            string displayText;
            string tooltipText = null;
            if (node.Links == null || node.Links.Count == 0)
            {
                string endDialogColor = "#FF7F50";
                displayText = $"{text} <span style='color:{endDialogColor};'><b>[End Dialog]</b></span>";
            }
            else if (string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text))
            {
                if (node.Text?.StringRef == -1)
                {
                    displayText = "(continue)";
                    tooltipText = $"<i>No text set.{extraNodeInfo}<br><br>" +
                        "Change this behavior by:<br>" +
                        "- <i>Right-click and select '<b>Edit Text</b>'</i><br>" +
                        "- <i>Double-click to edit text</i>";
                }
                else
                {
                    displayText = $"(invalid strref: {node.Text?.StringRef ?? -1})";
                    tooltipText = $"StrRef {node.Text?.StringRef ?? -1} not found in TLK.<br><br>" +
                        "Fix this issue by:<br>" +
                        "- <i>Right-click and select '<b>Edit Text</b>'</i><br>" +
                        "- <i>Double-click to edit text</i>";
                }
            }
            else
            {
                displayText = text;
            }

            // Build list prefix with node index
            string listPrefix = $"<b>{prefix}{node.ListIndex}:</b> ";

            // Get text size (default to 9pt if not available)
            int textSize = 9; // Default text size

            // Build formatted display text with HTML
            string formattedText = $"<span style=\"color:{color}; font-size:{textSize}pt;\">{listPrefix}{displayText}</span>";

            // Update the tree view item's header if editor is available
            if (_editor != null)
            {
                _editor.UpdateItemPresentation(item, formattedText, tooltipText);
            }

            // Check for various properties to determine icons (matching Python implementation)
            bool hasConditional = (link.Active1 != null && !link.Active1.IsBlank()) || (link.Active2 != null && !link.Active2.IsBlank());
            bool hasScript = (node.Script1 != null && !node.Script1.IsBlank()) || (node.Script2 != null && !node.Script2.IsBlank());
            bool hasAnimation = (node.CameraAnim.HasValue && node.CameraAnim.Value != -1) || (node.Animations != null && node.Animations.Count > 0);
            bool hasSound = node.Sound != null && !node.Sound.IsBlank() && node.SoundExists != 0;
            bool hasVoice = node.VoResRef != null && !node.VoResRef.IsBlank();
            bool isPlotOrQuestRelated = node.PlotIndex != -1 || (node.QuestEntry.HasValue && node.QuestEntry.Value != 0) || !string.IsNullOrEmpty(node.Quest);

            // Icon display would require custom TreeViewItem templates (Python used item.setData(icon_data, ICONS_DATA_ROLE)).
            // This method updates the item text display; when updateCopies is true, all copies of this node are updated.

            // Update copies if requested
            if (updateCopies && _nodeToItems.TryGetValue(node, out List<DLGStandardItem> items))
            {
                foreach (var copiedItem in items)
                {
                    if (copiedItem == item)
                    {
                        continue;
                    }
                    // Recursively update copies without updating their copies to avoid infinite loops
                    UpdateItemDisplayText(copiedItem, updateCopies: false);
                }
            }
        }

        /// <summary>
        /// Inserts a starter link at the specified index.
        /// </summary>
        public void InsertStarter(int index, DLGLink link)
        {
            if (link == null)
            {
                return;
            }
            var item = new DLGStandardItem(link);
            if (index < 0 || index > _rootItems.Count)
            {
                _rootItems.Add(item);
            }
            else
            {
                _rootItems.Insert(index, item);
            }
        }

        /// <summary>
        /// Gets the starter link at the specified index.
        /// </summary>
        public DLGLink GetStarterAt(int index)
        {
            if (index < 0 || index >= _rootItems.Count)
            {
                return null;
            }
            return _rootItems[index].Link;
        }

        /// <summary>
        /// Removes a starter link from the model.
        /// </summary>
        public void RemoveStarter(DLGLink link)
        {
            for (int i = _rootItems.Count - 1; i >= 0; i--)
            {
                if (_rootItems[i].Link == link)
                {
                    _rootItems.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// Moves a starter link from one index to another.
        /// </summary>
        public void MoveStarter(int oldIndex, int newIndex)
        {
            if (oldIndex < 0 || oldIndex >= _rootItems.Count || newIndex < 0 || newIndex >= _rootItems.Count)
            {
                return;
            }

            var item = _rootItems[oldIndex];
            _rootItems.RemoveAt(oldIndex);
            _rootItems.Insert(newIndex, item);
        }

        /// <summary>
        /// Moves the selected item up in the starter list.
        /// Returns true if the move was successful, false if the move was invalid (already at top or no selection).
        /// </summary>
        /// <returns>True if the move was successful, false otherwise.</returns>
        public bool MoveItemUp()
        {
            int selectedIndex = _selectedIndex;

            // Check if selection is valid and not already at top
            if (selectedIndex <= 0 || selectedIndex >= _rootItems.Count)
            {
                return false; // No selection, invalid index, or already at top
            }

            int newIndex = selectedIndex - 1;
            DLGLink link = _rootItems[selectedIndex].Link;

            // Synchronize with CoreDlg.Starters if editor is available (before moving in model)
            if (_editor != null && _editor.CoreDlg != null && selectedIndex < _editor.CoreDlg.Starters.Count)
            {
                _editor.CoreDlg.Starters.RemoveAt(selectedIndex);
                _editor.CoreDlg.Starters.Insert(newIndex, link);
                TouchCoreDlg();
            }

            // Move in model
            MoveStarter(selectedIndex, newIndex);

            // Update selected index to track the moved item
            _selectedIndex = newIndex;

            return true;
        }

        /// <summary>
        /// Moves the selected item down in the starter list.
        /// Returns true if the move was successful, false if the move was invalid (already at bottom or no selection).
        /// </summary>
        /// <returns>True if the move was successful, false otherwise.</returns>
        public bool MoveItemDown()
        {
            int selectedIndex = _selectedIndex;

            // Check if selection is valid and not already at bottom
            if (selectedIndex < 0 || selectedIndex >= _rootItems.Count - 1)
            {
                return false; // No selection, invalid index, or already at bottom
            }

            int newIndex = selectedIndex + 1;
            DLGLink link = _rootItems[selectedIndex].Link;

            // Synchronize with CoreDlg.Starters if editor is available (before moving in model)
            if (_editor != null && _editor.CoreDlg != null && selectedIndex < _editor.CoreDlg.Starters.Count)
            {
                _editor.CoreDlg.Starters.RemoveAt(selectedIndex);
                _editor.CoreDlg.Starters.Insert(newIndex, link);
                TouchCoreDlg();
            }

            // Move in model
            MoveStarter(selectedIndex, newIndex);

            // Update selected index to track the moved item
            _selectedIndex = newIndex;

            return true;
        }

        /// <summary>
        /// Gets all root items in the model.
        /// </summary>
        public IReadOnlyList<DLGStandardItem> GetRootItems()
        {
            return _rootItems;
        }

        /// <summary>
        /// Recursively loads a dialog item and all its children into the model.
        /// Detects cycles in the DLG graph to avoid stack overflow (e.g. Entry A -> Reply B -> Entry A).
        /// </summary>
        /// <param name="itemToLoad">The item to load recursively.</param>
        public void LoadDlgItemRec(DLGStandardItem itemToLoad)
        {
            LoadDlgItemRec(itemToLoad, new HashSet<DLGNode>());
        }

        private void RegisterItemMappings(DLGStandardItem itemToLoad)
        {
            if (itemToLoad == null || itemToLoad.Link == null)
            {
                return;
            }
            var link = itemToLoad.Link;
            var node = link.Node;

            if (!_linkToItems.ContainsKey(link))
            {
                _linkToItems[link] = new List<DLGStandardItem>();
            }
            if (!_linkToItems[link].Contains(itemToLoad))
            {
                _linkToItems[link].Add(itemToLoad);
            }

            if (node != null)
            {
                if (!_nodeToItems.ContainsKey(node))
                {
                    _nodeToItems[node] = new List<DLGStandardItem>();
                }
                if (!_nodeToItems[node].Contains(itemToLoad))
                {
                    _nodeToItems[node].Add(itemToLoad);
                }
            }
        }

        /// <summary>
        /// Lazily materializes and returns an item for a link by walking paths from roots.
        /// Does not pre-build the full tree.
        /// </summary>
        public DLGStandardItem MaterializeItemForLink(DLGLink target)
        {
            if (target == null)
            {
                return null;
            }
            if (_linkToItems.TryGetValue(target, out var existingList) && existingList != null && existingList.Count > 0)
            {
                return existingList[0];
            }

            foreach (var root in _rootItems)
            {
                if (root?.Link == target)
                {
                    RegisterItemMappings(root);
                    return root;
                }

                var found = MaterializeRecursive(root, target, new HashSet<DLGNode>());
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        private DLGStandardItem MaterializeRecursive(DLGStandardItem parentItem, DLGLink target, HashSet<DLGNode> visited)
        {
            if (parentItem?.Link?.Node == null)
            {
                return null;
            }
            var node = parentItem.Link.Node;
            if (visited.Contains(node))
            {
                return null;
            }
            visited.Add(node);

            var childLinks = node.Links;
            if (childLinks != null)
            {
                foreach (var childLink in childLinks)
                {
                    if (childLink == null)
                    {
                        continue;
                    }

                    DLGStandardItem childItem = null;
                    foreach (var existingChild in parentItem.Children)
                    {
                        if (ReferenceEquals(existingChild?.Link, childLink))
                        {
                            childItem = existingChild;
                            break;
                        }
                    }

                    if (childItem == null)
                    {
                        childItem = new DLGStandardItem(childLink);
                        parentItem.AddChild(childItem);
                    }
                    RegisterItemMappings(childItem);

                    if (ReferenceEquals(childLink, target))
                    {
                        return childItem;
                    }

                    var found = MaterializeRecursive(childItem, target, visited);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            visited.Remove(node);
            return null;
        }

        /// <summary>
        /// Recursively loads a dialog item and all its children into the model.
        /// </summary>
        /// <param name="itemToLoad">The item to load recursively.</param>
        /// <param name="visitedInPath">Nodes already visited in the current path (for cycle detection).</param>
        private void LoadDlgItemRec(DLGStandardItem itemToLoad, HashSet<DLGNode> visitedInPath)
        {
            if (itemToLoad == null || itemToLoad.Link == null)
            {
                return;
            }

            var link = itemToLoad.Link;
            var node = link.Node;

            if (node == null)
            {
                return;
            }

            RegisterItemMappings(itemToLoad);

            // Recursively load all child links
            foreach (var childLink in node.Links)
            {
                if (childLink == null || childLink.Node == null)
                {
                    continue;
                }

                var childNode = childLink.Node;

                // Cycle detection: if we've already visited this node in the current path, add the item
                // (so the tree shows the reference) but do not recurse - avoids stack overflow
                bool isCycle = visitedInPath.Contains(childNode);

                var childItem = new DLGStandardItem(childLink);
                itemToLoad.AddChild(childItem);

                if (!isCycle)
                {
                    visitedInPath.Add(childNode);
                    try
                    {
                        LoadDlgItemRec(childItem, visitedInPath);
                    }
                    finally
                    {
                        visitedInPath.Remove(childNode);
                    }
                }
            }
        }

        /// <summary>
        /// Shifts an item in the tree by a given amount.
        /// </summary>
        /// <param name="item">The item to shift.</param>
        /// <param name="amount">The amount to shift (positive = down, negative = up).</param>
        public void ShiftItem(DLGStandardItem item, int amount)
        {
            if (item == null)
            {
                return;
            }

            var parent = item.Parent;
            var oldRow = parent == null ? _rootItems.IndexOf(item) : parent.Children.ToList().IndexOf(item);

            if (oldRow < 0)
            {
                return;
            }

            var newRow = oldRow + amount;
            var maxRow = parent == null ? _rootItems.Count : parent.Children.Count();

            if (newRow < 0 || newRow >= maxRow)
            {
                return;
            }

            // Move the item
            if (parent == null)
            {
                _rootItems.RemoveAt(oldRow);
                _rootItems.Insert(newRow, item);
            }
            else
            {
                // For child items, we need to update the parent's Links list
                if (parent.Link?.Node != null)
                {
                    var links = parent.Link.Node.Links;
                    if (oldRow < links.Count && newRow < links.Count)
                    {
                        var linkToMove = links[oldRow];
                        links.RemoveAt(oldRow);
                        links.Insert(newRow, linkToMove);

                        // Update list_index for all affected links
                        for (int i = 0; i < links.Count; i++)
                        {
                            links[i].ListIndex = i;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Moves an item to a specific index within the model.
        /// Vendor: move_item_to_index.
        /// </summary>
        /// <param name="item">The item to move.</param>
        /// <param name="newIndex">The target index.</param>
        /// <param name="targetParent">The target parent (null for root).</param>
        public void MoveItemToIndex(DLGStandardItem item, int newIndex, DLGStandardItem targetParent)
        {
            if (item == null || _editor == null || _editor.CoreDlg == null)
            {
                return;
            }

            var sourceParent = item.Parent;
            int oldRow = sourceParent == null ? _rootItems.IndexOf(item) : sourceParent.Children.ToList().IndexOf(item);
            if (oldRow < 0)
            {
                return;
            }

            if (targetParent == sourceParent && newIndex == oldRow)
            {
                return;
            }

            if (newIndex < 0 || newIndex > (targetParent == null ? _rootItems.Count : targetParent.Children.Count))
            {
                return;
            }

            // Adjust newIndex when moving within same parent (removing shifts indices)
            int adjustedNewIndex = newIndex;
            if (sourceParent == targetParent && newIndex > oldRow)
            {
                adjustedNewIndex = newIndex - 1;
            }

            DLGLink linkToMove = item.Link;
            if (linkToMove == null)
            {
                return;
            }

            // Remove from source
            if (sourceParent == null)
            {
                _rootItems.RemoveAt(oldRow);
                if (_editor.CoreDlg.Starters != null && oldRow < _editor.CoreDlg.Starters.Count)
                {
                    _editor.CoreDlg.Starters.RemoveAt(oldRow);
                    TouchCoreDlg();
                }
            }
            else
            {
                sourceParent.RemoveChild(item);
                sourceParent.Link?.Node?.Links?.RemoveAt(oldRow);
            }

            // Insert at target
            if (targetParent == null)
            {
                _rootItems.Insert(adjustedNewIndex, item);
                if (_editor.CoreDlg.Starters != null && !_editor.CoreDlg.Starters.Contains(linkToMove))
                {
                    if (adjustedNewIndex <= _editor.CoreDlg.Starters.Count)
                    {
                        _editor.CoreDlg.Starters.Insert(adjustedNewIndex, linkToMove);
                    }
                    else
                    {
                        _editor.CoreDlg.Starters.Add(linkToMove);
                    }
                    TouchCoreDlg();
                }
            }
            else
            {
                targetParent.InsertChild(adjustedNewIndex, item);
                var linksList = targetParent.Link?.Node?.Links;
                if (linksList != null)
                {
                    int insertIdx = Math.Min(adjustedNewIndex, linksList.Count);
                    linksList.Insert(insertIdx, linkToMove);
                    TouchCoreDlg();
                    for (int i = 0; i < linksList.Count; i++)
                    {
                        if (linksList[i] != null)
                        {
                            linksList[i].ListIndex = i;
                        }
                    }
                }
            }

            _editor.UpdateTreeView();
        }

        /// <summary>
        /// Copies a link and node to the clipboard as JSON.
        /// Note: In Python, this only sets clipboard. The editor's _copy is set separately.
        /// In C#, we set both clipboard and editor._copy for convenience.
        /// </summary>
        /// <param name="link">The link to copy, or null to do nothing.</param>
        /// <param name="editor">The DLG editor instance to access clipboard and set _copy.</param>
        public async System.Threading.Tasks.Task CopyLinkAndNode(DLGLink link, OdyToolDLG editor)
        {
            if (link == null || editor == null)
            {
                return;
            }

            try
            {
                Dictionary<string, object> nodeMap = new Dictionary<string, object>();
                Dictionary<string, object> linkDict = link.ToDict(nodeMap);

                // Serialize to JSON
                string json = JsonSerializer.Serialize(linkDict, new JsonSerializerOptions
                {
                    WriteIndented = false,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    MaxDepth = 256
                });

                // Set clipboard text
                var topLevel = Avalonia.Controls.TopLevel.GetTopLevel(editor);
                if (topLevel?.Clipboard != null)
                {
                    await topLevel.Clipboard.SetTextAsync(json);
                }

                // Set editor's _copy field (matching Python: editor._copy = link)
                // In Python, this is set separately, but we do it here for convenience
                editor.SetCopyLink(link);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Pastes a link as a child of the specified parent item.
        /// </summary>
        /// <param name="parentItem">The parent item to paste under, or null for root.</param>
        /// <param name="pastedLink">The link to paste.</param>
        /// <param name="row">Optional row index to insert at, or null to append.</param>
        /// <param name="asNewBranches">If true, creates deep copies of nodes; if false, links to existing nodes.</param>
        public void PasteItem(DLGStandardItem parentItem, DLGLink pastedLink, int? row = null, bool asNewBranches = true)
        {
            if (pastedLink == null || _editor == null || _editor.CoreDlg == null)
            {
                return;
            }

            // If as_new_branches is True, we need to deep copy the entire link tree
            DLGLink linkToPaste = pastedLink;
            if (asNewBranches)
            {
                // Deep copy the link using ToDict/FromDict
                Dictionary<string, object> nodeMap = new Dictionary<string, object>();
                Dictionary<string, object> linkDict = pastedLink.ToDict(nodeMap);
                linkToPaste = DLGLink.FromDict(linkDict, nodeMap);
            }

            // Set is_child property based on whether parentItem is a DLGStandardItem or null
            linkToPaste.IsChild = (parentItem != null);

            // Note: When as_new_branches is True, we create a deep copy via ToDict/FromDict,
            // which creates new objects with new hash caches automatically (set in constructors).
            // The Python code explicitly sets _hash_cache, but in C# the hash cache is readonly
            // and set in the constructor, so new objects created via FromDict already have unique hashes.

            // Ensure the link is not already in link_to_items
            if (_linkToItems.ContainsKey(linkToPaste))
            {
                // Link already exists, this shouldn't happen with a new hash, but handle it gracefully
                return;
            }

            // Get all existing entry and reply indices
            HashSet<int> entryIndices = new HashSet<int>();
            foreach (var entry in _editor.CoreDlg.EntryList)
            {
                if (entry.ListIndex >= 0)
                {
                    entryIndices.Add(entry.ListIndex);
                }
            }

            HashSet<int> replyIndices = new HashSet<int>();
            foreach (var reply in _editor.CoreDlg.ReplyList)
            {
                if (reply.ListIndex >= 0)
                {
                    replyIndices.Add(reply.ListIndex);
                }
            }

            // Traverse all nodes in the pasted link tree and assign new list indices
            Queue<DLGNode> queue = new Queue<DLGNode>();
            HashSet<DLGNode> visited = new HashSet<DLGNode>();

            if (linkToPaste.Node != null)
            {
                queue.Enqueue(linkToPaste.Node);
            }

            while (queue.Count > 0)
            {
                DLGNode curNode = queue.Dequeue();
                if (curNode == null || visited.Contains(curNode))
                {
                    continue;
                }
                visited.Add(curNode);

                // Assign new list index if as_new_branches or node doesn't exist in node_to_items
                if (asNewBranches || !_nodeToItems.ContainsKey(curNode))
                {
                    int newIndex = GetNewNodeListIndex(curNode, entryIndices, replyIndices);
                    curNode.ListIndex = newIndex;

                    // Update the appropriate index set
                    if (curNode is DLGEntry)
                    {
                        entryIndices.Add(newIndex);
                        if (!_editor.CoreDlg.EntryList.Contains((DLGEntry)curNode))
                        {
                            _editor.CoreDlg.EntryList.Add((DLGEntry)curNode);
                        }
                    }
                    else if (curNode is DLGReply)
                    {
                        replyIndices.Add(newIndex);
                        if (!_editor.CoreDlg.ReplyList.Contains((DLGReply)curNode))
                        {
                            _editor.CoreDlg.ReplyList.Add((DLGReply)curNode);
                        }
                    }
                }

                // Note: When as_new_branches is True, nodes are deep copied via ToDict/FromDict,
                // which creates new node objects with new hash caches automatically (set in constructors).
                // The Python code explicitly sets _hash_cache, but in C# the hash cache is readonly
                // and set in the constructor, so new nodes created via FromDict already have unique hashes.

                // Add child nodes to queue
                if (curNode.Links != null)
                {
                    foreach (var link in curNode.Links)
                    {
                        if (link?.Node != null)
                        {
                            queue.Enqueue(link.Node);
                        }
                    }
                }
            }

            // If as_new_branches, also assign new list index to the root node of the pasted link
            if (asNewBranches && linkToPaste.Node != null)
            {
                int newIndex = GetNewNodeListIndex(linkToPaste.Node, entryIndices, replyIndices);
                linkToPaste.Node.ListIndex = newIndex;

                if (linkToPaste.Node is DLGEntry)
                {
                    entryIndices.Add(newIndex);
                }
                else if (linkToPaste.Node is DLGReply)
                {
                    replyIndices.Add(newIndex);
                }
            }

            // Create new item for the pasted link
            DLGStandardItem newItem = new DLGStandardItem(linkToPaste);

            // Add the item to the model
            if (parentItem == null)
            {
                // Add to root
                if (row.HasValue && row.Value >= 0 && row.Value <= _rootItems.Count)
                {
                    _rootItems.Insert(row.Value, newItem);
                }
                else
                {
                    _rootItems.Add(newItem);
                }

                // Add to CoreDlg.Starters
                if (!_editor.CoreDlg.Starters.Contains(linkToPaste))
                {
                    if (row.HasValue && row.Value >= 0 && row.Value <= _editor.CoreDlg.Starters.Count)
                    {
                        _editor.CoreDlg.Starters.Insert(row.Value, linkToPaste);
                    }
                    else
                    {
                        _editor.CoreDlg.Starters.Add(linkToPaste);
                    }
                    TouchCoreDlg();
                }
            }
            else
            {
                // Add as child
                int insertIndex = row.HasValue && row.Value >= 0 && row.Value <= parentItem.Children.Count
                    ? row.Value
                    : parentItem.Children.Count;

                // Insert child at the specified index
                parentItem.InsertChild(insertIndex, newItem);

                // Add link to parent node's Links collection at the correct position
                if (parentItem.Link?.Node != null)
                {
                    var linksList = parentItem.Link.Node.Links;

                    // Check if link already exists in the list (shouldn't happen for new pasted links, but handle it)
                    int existingIndex = linksList.IndexOf(linkToPaste);
                    if (existingIndex >= 0)
                    {
                        // Link already exists, move it if needed
                        if (existingIndex != insertIndex)
                        {
                            linksList.RemoveAt(existingIndex);
                            if (existingIndex < insertIndex)
                            {
                                insertIndex--; // Adjust index if removing from before the insert position
                            }
                        }
                    }

                    // Insert link at the correct position
                    if (insertIndex >= 0 && insertIndex <= linksList.Count)
                    {
                        linksList.Insert(insertIndex, linkToPaste);
                    }
                    else
                    {
                        linksList.Add(linkToPaste);
                    }
                    TouchCoreDlg();

                    // Update list_index for all links to maintain order
                    for (int i = 0; i < linksList.Count; i++)
                    {
                        if (linksList[i] != null)
                        {
                            linksList[i].ListIndex = i;
                        }
                    }
                }
            }

            // Register in dictionaries
            if (!_linkToItems.ContainsKey(linkToPaste))
            {
                _linkToItems[linkToPaste] = new List<DLGStandardItem>();
            }
            if (!_linkToItems[linkToPaste].Contains(newItem))
            {
                _linkToItems[linkToPaste].Add(newItem);
            }

            if (linkToPaste.Node != null)
            {
                if (!_nodeToItems.ContainsKey(linkToPaste.Node))
                {
                    _nodeToItems[linkToPaste.Node] = new List<DLGStandardItem>();
                }
                if (!_nodeToItems[linkToPaste.Node].Contains(newItem))
                {
                    _nodeToItems[linkToPaste.Node].Add(newItem);
                }
            }

            // Register only this item; children are materialized lazily on access.
            RegisterItemMappings(newItem);

            // Update parent item display text if parent is a DLGStandardItem
            if (parentItem != null)
            {
                UpdateItemDisplayText(parentItem);
            }

            // Update tree view if editor is available
            if (_editor != null)
            {
                _editor.UpdateTreeView();
            }
        }

        /// <summary>
        /// Gets a new unique list index for a node.
        /// </summary>
        /// <param name="node">The node to get a new index for.</param>
        /// <param name="entryIndices">Optional set of existing entry indices.</param>
        /// <param name="replyIndices">Optional set of existing reply indices.</param>
        /// <returns>A new unique list index.</returns>
        private int GetNewNodeListIndex(DLGNode node, HashSet<int> entryIndices = null, HashSet<int> replyIndices = null)
        {
            if (_editor == null || _editor.CoreDlg == null)
            {
                return 0;
            }

            HashSet<int> indices;
            if (node is DLGEntry)
            {
                if (entryIndices == null)
                {
                    indices = new HashSet<int>();
                    foreach (var entry in _editor.CoreDlg.EntryList)
                    {
                        if (entry.ListIndex >= 0)
                        {
                            indices.Add(entry.ListIndex);
                        }
                    }
                }
                else
                {
                    indices = entryIndices;
                }
            }
            else if (node is DLGReply)
            {
                if (replyIndices == null)
                {
                    indices = new HashSet<int>();
                    foreach (var reply in _editor.CoreDlg.ReplyList)
                    {
                        if (reply.ListIndex >= 0)
                        {
                            indices.Add(reply.ListIndex);
                        }
                    }
                }
                else
                {
                    indices = replyIndices;
                }
            }
            else
            {
                throw new ArgumentException($"Unknown node type: {node.GetType().Name}");
            }

            int newIndex = (indices.Count > 0 ? indices.Max() : -1) + 1;
            while (indices.Contains(newIndex))
            {
                newIndex++;
            }

            return newIndex;
        }

        /// <summary>
        /// Removes all occurrences of a node and all links to it from the model and CoreDlg.
        /// </summary>
        /// <param name="nodeToRemove">The node to remove everywhere.</param>
        public void DeleteNodeEverywhere(DLGNode nodeToRemove)
        {
            if (nodeToRemove == null || _editor == null || _editor.CoreDlg == null)
            {
                return;
            }

            // First, remove all links from CoreDlg that point to this node
            // This includes links from all nodes' Links collections and from Starters
            RemoveLinksToNode(nodeToRemove);

            // Recursively remove items from the model tree
            RemoveLinksRecursive(nodeToRemove, null);

            // Update tree view if editor is available
            if (_editor != null)
            {
                _editor.UpdateTreeView();
            }
        }

        /// <summary>
        /// Recursively removes links to a node from the model tree.
        /// </summary>
        /// <param name="nodeToRemove">The node to remove.</param>
        /// <param name="parentItem">The parent item to search in, or null to search root items.</param>
        private void RemoveLinksRecursive(DLGNode nodeToRemove, DLGStandardItem parentItem)
        {
            if (nodeToRemove == null)
            {
                return;
            }

            // Get the list of children to iterate over
            List<DLGStandardItem> children;
            if (parentItem == null)
            {
                // Search root items
                children = new List<DLGStandardItem>(_rootItems);
            }
            else
            {
                // Search children of parent item
                children = new List<DLGStandardItem>(parentItem.Children);
            }

            // Iterate in reverse to safely remove items while iterating
            for (int i = children.Count - 1; i >= 0; i--)
            {
                var childItem = children[i];
                if (childItem == null)
                {
                    continue;
                }

                var childLink = childItem.Link;
                if (childLink == null)
                {
                    continue;
                }

                // Check if this child item's node is the one we want to remove
                if (childLink.Node == nodeToRemove)
                {
                    // First, recursively remove all children of this item
                    RemoveLinksRecursive(childLink.Node, childItem);

                    // Remove the link from the parent node's Links collection
                    if (parentItem != null && parentItem.Link != null && parentItem.Link.Node != null)
                    {
                        parentItem.Link.Node.Links.Remove(childLink);
                        TouchCoreDlg();
                    }

                    // Remove the item from the model
                    if (parentItem == null)
                    {
                        // Remove from root items
                        _rootItems.Remove(childItem);
                    }
                    else
                    {
                        // Remove from parent's children
                        parentItem.RemoveChild(childItem);
                    }
                }
                else
                {
                    // Continue searching recursively in this child
                    RemoveLinksRecursive(nodeToRemove, childItem);
                }
            }
        }

        /// <summary>
        /// Removes all links from CoreDlg that point to the specified node.
        /// This includes links from Starters and from all nodes' Links collections.
        /// </summary>
        /// <param name="nodeToRemove">The node to remove links to.</param>
        private void RemoveLinksToNode(DLGNode nodeToRemove)
        {
            if (nodeToRemove == null || _editor == null || _editor.CoreDlg == null)
            {
                return;
            }

            // Remove from Starters
            for (int i = _editor.CoreDlg.Starters.Count - 1; i >= 0; i--)
            {
                if (_editor.CoreDlg.Starters[i]?.Node == nodeToRemove)
                {
                    _editor.CoreDlg.Starters.RemoveAt(i);
                    TouchCoreDlg();
                }
            }

            // Remove links from all entries
            foreach (var entry in _editor.CoreDlg.EntryList)
            {
                if (entry != null && entry.Links != null)
                {
                    for (int i = entry.Links.Count - 1; i >= 0; i--)
                    {
                        if (entry.Links[i]?.Node == nodeToRemove)
                        {
                            entry.Links.RemoveAt(i);
                            TouchCoreDlg();
                        }
                    }
                }
            }

            // Remove links from all replies
            foreach (var reply in _editor.CoreDlg.ReplyList)
            {
                if (reply != null && reply.Links != null)
                {
                    for (int i = reply.Links.Count - 1; i >= 0; i--)
                    {
                        if (reply.Links[i]?.Node == nodeToRemove)
                        {
                            reply.Links.RemoveAt(i);
                            TouchCoreDlg();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Deletes a node from the DLG and UI tree model.
        /// </summary>
        /// <param name="item">The DLGStandardItem to delete.</param>
        public void DeleteNode(DLGStandardItem item)
        {
            if (item == null)
            {
                return;
            }

            var parentItem = item.Parent;
            var link = item.Link;

            if (parentItem == null)
            {
                // Root item - remove from root items list
                int index = _rootItems.IndexOf(item);
                if (index >= 0)
                {
                    _rootItems.RemoveAt(index);

                    // Also remove from CoreDlg.Starters if editor is available
                    if (_editor != null && _editor.CoreDlg != null && link != null)
                    {
                        _editor.CoreDlg.Starters.Remove(link);
                        TouchCoreDlg();
                    }

                    // Clean up dictionaries
                    if (link != null)
                    {
                        if (_linkToItems.ContainsKey(link))
                        {
                            _linkToItems[link].Remove(item);
                            if (_linkToItems[link].Count == 0)
                            {
                                _linkToItems.Remove(link);
                            }
                        }

                        if (link.Node != null && _nodeToItems.ContainsKey(link.Node))
                        {
                            _nodeToItems[link.Node].Remove(item);
                            if (_nodeToItems[link.Node].Count == 0)
                            {
                                _nodeToItems.Remove(link.Node);
                            }
                        }
                    }
                }
            }
            else
            {
                // Child item - remove from parent
                parentItem.RemoveChild(item);

                // Remove the link from the parent node's Links collection
                if (parentItem.Link != null && parentItem.Link.Node != null && link != null)
                {
                    parentItem.Link.Node.Links.Remove(link);
                    TouchCoreDlg();
                }

                // Clean up dictionaries
                if (link != null)
                {
                    if (_linkToItems.ContainsKey(link))
                    {
                        _linkToItems[link].Remove(item);
                        if (_linkToItems[link].Count == 0)
                        {
                            _linkToItems.Remove(link);
                        }
                    }

                    if (link.Node != null && _nodeToItems.ContainsKey(link.Node))
                    {
                        _nodeToItems[link.Node].Remove(item);
                        if (_nodeToItems[link.Node].Count == 0)
                        {
                            _nodeToItems.Remove(link.Node);
                        }
                    }
                }

                // Update parent item display text
                UpdateItemDisplayText(parentItem);
            }

            // Update tree view if editor is available
            if (_editor != null)
            {
                _editor.UpdateTreeView();
            }
        }

        /// <summary>
        /// Serializes items to MIME data format for drag-and-drop operations.
        /// Creates a JSON-based format compatible with Avalonia drag-and-drop.
        /// </summary>
        /// <param name="items">The items to serialize. Items should be in the order they appear in the model.</param>
        /// <returns>A JSON string containing the serialized MIME data.</returns>
        public string MimeData(IEnumerable<DLGStandardItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            // Each entry contains: row, column, num_roles, and role/value pairs
            var itemDataList = new List<Dictionary<string, object>>();
            int itemIndex = 0;

            foreach (var item in items)
            {
                if (item == null || item.Link == null)
                {
                    continue;
                }

                // Get row and column for this item
                // In Qt, row comes from QModelIndex.row(), but since we're working with items directly,
                // we'll use the sequential index as the row (matching the order items are passed in)
                int row = itemIndex;
                int column = 0; // Always 0 in our model (matching Qt's single-column tree)

                // Get display text
                string displayText = GetItemDisplayText(item);

                // Serialize link to dictionary
                Dictionary<string, object> nodeMap = new Dictionary<string, object>();
                Dictionary<string, object> linkDict = item.Link.ToDict(nodeMap);

                // Create item data entry matching Qt format structure
                var itemData = new Dictionary<string, object>
                    {
                        { "row", row },
                        { "column", column },
                        { "roles", new Dictionary<string, object>() }
                    };

                var roles = (Dictionary<string, object>)itemData["roles"];

                // Role 0 = DisplayRole (Qt.ItemDataRole.DisplayRole)
                roles["0"] = displayText;

                // Role 261 = _DLG_MIME_DATA_ROLE (Qt.ItemDataRole.UserRole + 5 = 256 + 5)
                string linkJson = JsonSerializer.Serialize(linkDict, new JsonSerializerOptions
                {
                    WriteIndented = false,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    MaxDepth = 256
                });
                roles["261"] = linkJson;

                // Role 262 = _MODEL_INSTANCE_ID_ROLE (Qt.ItemDataRole.UserRole + 6 = 256 + 6)
                // Use object hash code as instance ID (similar to Python's id())
                int modelInstanceId = RuntimeHelpers.GetHashCode(this);
                roles["262"] = modelInstanceId;

                itemDataList.Add(itemData);
                itemIndex++;
            }

            // Serialize to JSON
            string json = JsonSerializer.Serialize(itemDataList, new JsonSerializerOptions
            {
                WriteIndented = false,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                MaxDepth = 256
            });

            return json;
        }

        /// <summary>
        /// Gets the display text for an item (uses resolved text: TLK when strref != -1, custom when -1).
        /// </summary>
        private string GetItemDisplayText(DLGStandardItem item)
        {
            if (item?.Link?.Node == null)
            {
                return "Unknown";
            }

            var node = item.Link.Node;
            string nodeType = node is DLGEntry ? "Entry" : "Reply";
            string text = _editor != null ? _editor.GetResolvedNodeText(node) : (node.Text?.GetString(0, Gender.Male) ?? "");
            if (string.IsNullOrEmpty(text))
            {
                text = "<empty>";
            }
            return $"{nodeType}: {text}";
        }

        /// <summary>
        /// Parses MIME data from JSON format back into item data structures.
        /// </summary>
        /// <param name="jsonData">The JSON string containing serialized MIME data.</param>
        /// <returns>A list of item data dictionaries containing row, column, and roles.</returns>
        public List<Dictionary<string, object>> ParseMimeData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData))
            {
                return new List<Dictionary<string, object>>();
            }

            try
            {
                // Deserialize JSON using JsonDocument to handle nested dictionaries properly
                using (JsonDocument doc = JsonDocument.Parse(jsonData))
                {
                    var itemDataList = new List<Dictionary<string, object>>();
                    foreach (var element in doc.RootElement.EnumerateArray())
                    {
                        var itemData = new Dictionary<string, object>();
                        if (element.TryGetProperty("row", out JsonElement rowElement))
                        {
                            itemData["row"] = rowElement.GetInt32();
                        }
                        if (element.TryGetProperty("column", out JsonElement columnElement))
                        {
                            itemData["column"] = columnElement.GetInt32();
                        }
                        if (element.TryGetProperty("roles", out JsonElement rolesElement))
                        {
                            var rolesDict = new Dictionary<string, object>();
                            foreach (var prop in rolesElement.EnumerateObject())
                            {
                                // Get the value as string or number
                                if (prop.Value.ValueKind == JsonValueKind.String)
                                {
                                    rolesDict[prop.Name] = prop.Value.GetString();
                                }
                                else if (prop.Value.ValueKind == JsonValueKind.Number)
                                {
                                    rolesDict[prop.Name] = prop.Value.GetInt64();
                                }
                                else
                                {
                                    rolesDict[prop.Name] = prop.Value.GetRawText();
                                }
                            }
                            itemData["roles"] = rolesDict;
                        }
                        itemDataList.Add(itemData);
                    }
                    return itemDataList;
                }
            }
            catch
            {
                return new List<Dictionary<string, object>>();
            }
        }
    }
}
