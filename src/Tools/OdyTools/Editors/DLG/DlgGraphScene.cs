using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace OdyTools.Editors.DLG
{
    public enum DlgGraphNodeKind
    {
        Entry,
        Reply,
        Starter
    }

    [Flags]
    public enum DlgGraphNodeBadges
    {
        None = 0,
        HasScript = 1,
        HasCondition = 2,
        HasSound = 4,
        HasVoice = 8
    }

    public sealed class DlgGraphNodeData
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public object Tag { get; set; }
        public DlgGraphNodeKind Kind { get; set; }
        public DlgGraphNodeBadges Badges { get; set; }
        public int ChildCount { get; set; }
    }

    public sealed class DlgGraphEdgeData
    {
        public string FromKey { get; set; }
        public string ToKey { get; set; }
        public bool HasCondition { get; set; }
    }

    internal sealed class DlgGraphNodeState
    {
        public DlgGraphNodeData Data;
        public Point Center;
        public bool IsPinned;
        public bool IsOrphan;
    }

    /// <summary>
    /// Lightweight graph canvas for DLG node visualization.
    /// Supports selection, pan/zoom, and manual node dragging.
    /// </summary>
    public class DlgGraphScene : Control
    {
        public static readonly StyledProperty<IBrush> BackgroundProperty =
            Border.BackgroundProperty.AddOwner<DlgGraphScene>();

        public IBrush Background
        {
            get => GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        private readonly Dictionary<string, DlgGraphNodeState> _nodes = new Dictionary<string, DlgGraphNodeState>(StringComparer.OrdinalIgnoreCase);
        private readonly List<DlgGraphEdgeData> _edges = new List<DlgGraphEdgeData>();
        private readonly Dictionary<string, int> _levels = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        private string _selectedKey;
        private string _draggingNodeKey;
        private bool _isPanning;
        private Point _lastPointer;

        private string _linkDragSourceKey;
        private Point _linkDragStartScreen;
        private Point _linkDragCurrentScreen;
        private bool _isLinkDragging;
        private bool _linkDragExceededThreshold;
        private const double LinkDragThreshold = 8;

        private string _hoveredKey;
        private HashSet<string> _highlightedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private const double NodeWidth = 220;
        private const double NodeHeight = 88;
        private const double LevelSpacingX = 320;
        private const double LevelSpacingY = 140;

        public event Action<object> NodeSelected;
        public event Action<string, Point> NodePositionCommitted;
        public event Action ZoomChanged;

        /// <summary>
        /// Fired when a right-click link drag ends on a target node.
        /// Args: source node tag, target node tag, screen position.
        /// </summary>
        public event Action<object, object, Point> LinkDragCompletedOnNode;

        /// <summary>
        /// Fired when a right-click link drag ends on empty space.
        /// Args: source node tag, world position where released.
        /// </summary>
        public event Action<object, Point> LinkDragCompletedOnEmpty;

        public double Zoom { get; private set; } = 1.0;
        public Vector Pan { get; private set; } = new Vector(60, 60);
        public int OrphanCount { get; private set; }

        public DlgGraphScene()
        {
            ClipToBounds = true;
            Background = Brushes.White;
            Focusable = true;
        }

        public void SetGraph(
            IReadOnlyList<DlgGraphNodeData> nodes,
            IReadOnlyList<DlgGraphEdgeData> edges,
            IDictionary<string, Point> persistedPositions)
        {
            _nodes.Clear();
            _edges.Clear();
            _levels.Clear();

            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (node == null || string.IsNullOrWhiteSpace(node.Key))
                    {
                        continue;
                    }

                    var state = new DlgGraphNodeState
                    {
                        Data = node,
                        Center = new Point(0, 0),
                        IsPinned = false
                    };
                    if (persistedPositions != null && persistedPositions.TryGetValue(node.Key, out Point pos))
                    {
                        state.Center = pos;
                        state.IsPinned = true;
                    }
                    _nodes[node.Key] = state;
                }
            }

            if (edges != null)
            {
                foreach (var edge in edges)
                {
                    if (edge == null || string.IsNullOrWhiteSpace(edge.FromKey) || string.IsNullOrWhiteSpace(edge.ToKey))
                    {
                        continue;
                    }
                    if (!_nodes.ContainsKey(edge.FromKey) || !_nodes.ContainsKey(edge.ToKey))
                    {
                        continue;
                    }
                    _edges.Add(edge);
                }
            }

            ComputeLevels();
            ComputeOrphans();
            AutoLayout(keepPinned: true);
        }

        private void ComputeOrphans()
        {
            var reachable = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var queue = new Queue<string>();
            foreach (var kvp in _nodes)
            {
                if (kvp.Value.Data?.Kind == DlgGraphNodeKind.Starter)
                {
                    reachable.Add(kvp.Key);
                    queue.Enqueue(kvp.Key);
                }
            }
            while (queue.Count > 0)
            {
                var cur = queue.Dequeue();
                foreach (var edge in _edges)
                {
                    if (string.Equals(edge.FromKey, cur, StringComparison.OrdinalIgnoreCase)
                        && !reachable.Contains(edge.ToKey))
                    {
                        reachable.Add(edge.ToKey);
                        queue.Enqueue(edge.ToKey);
                    }
                }
            }

            OrphanCount = 0;
            foreach (var kvp in _nodes)
            {
                bool orphan = !reachable.Contains(kvp.Key);
                kvp.Value.IsOrphan = orphan;
                if (orphan) OrphanCount++;
            }
        }

        public void SetSelectedNodeKey(string key)
        {
            _selectedKey = key;
            InvalidateVisual();
        }

        public void SetHighlightedKeys(IEnumerable<string> keys)
        {
            _highlightedKeys.Clear();
            if (keys != null)
                foreach (var k in keys)
                    if (!string.IsNullOrEmpty(k))
                        _highlightedKeys.Add(k);
            InvalidateVisual();
        }

        public void ClearHighlights()
        {
            if (_highlightedKeys.Count > 0)
            {
                _highlightedKeys.Clear();
                InvalidateVisual();
            }
        }

        public void CenterOnNode(string key)
        {
            if (string.IsNullOrEmpty(key) || !_nodes.TryGetValue(key, out var state))
                return;
            Pan = new Vector(
                (Bounds.Width * 0.5) - (state.Center.X * Zoom),
                (Bounds.Height * 0.5) - (state.Center.Y * Zoom));
            InvalidateVisual();
        }

        public void CenterOnStarters()
        {
            double minX = double.MaxValue, sumX = 0, sumY = 0;
            int count = 0;
            foreach (var kvp in _nodes)
            {
                if (kvp.Value.Data?.Kind == DlgGraphNodeKind.Starter)
                {
                    sumX += kvp.Value.Center.X;
                    sumY += kvp.Value.Center.Y;
                    minX = Math.Min(minX, kvp.Value.Center.X);
                    count++;
                }
            }
            if (count == 0) { FitToContent(); return; }
            double cx = sumX / count;
            double cy = sumY / count;
            Pan = new Vector(
                (Bounds.Width * 0.5) - (cx * Zoom),
                (Bounds.Height * 0.5) - (cy * Zoom));
            InvalidateVisual();
        }

        public void AutoLayout(bool keepPinned)
        {
            var groups = new Dictionary<int, List<DlgGraphNodeState>>();
            foreach (var kvp in _nodes)
            {
                int level = 0;
                if (_levels.TryGetValue(kvp.Key, out int lvl))
                {
                    level = lvl;
                }
                if (!groups.ContainsKey(level))
                {
                    groups[level] = new List<DlgGraphNodeState>();
                }
                groups[level].Add(kvp.Value);
            }

            foreach (var group in groups)
            {
                var level = group.Key;
                var ordered = group.Value.OrderBy(n => n.Data?.Title ?? string.Empty, StringComparer.OrdinalIgnoreCase).ToList();
                for (int i = 0; i < ordered.Count; i++)
                {
                    var state = ordered[i];
                    if (keepPinned && state.IsPinned)
                    {
                        continue;
                    }
                    state.Center = new Point(120 + level * LevelSpacingX, 80 + i * LevelSpacingY);
                }
            }

            InvalidateVisual();
        }

        public Dictionary<string, Point> ExportNodePositions(bool pinnedOnly)
        {
            var result = new Dictionary<string, Point>(StringComparer.OrdinalIgnoreCase);
            foreach (var kvp in _nodes)
            {
                if (pinnedOnly && !kvp.Value.IsPinned)
                {
                    continue;
                }
                result[kvp.Key] = kvp.Value.Center;
            }
            return result;
        }

        public void FitToContent()
        {
            if (_nodes.Count == 0)
            {
                return;
            }

            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            foreach (var state in _nodes.Values)
            {
                minX = Math.Min(minX, state.Center.X - (NodeWidth / 2));
                minY = Math.Min(minY, state.Center.Y - (NodeHeight / 2));
                maxX = Math.Max(maxX, state.Center.X + (NodeWidth / 2));
                maxY = Math.Max(maxY, state.Center.Y + (NodeHeight / 2));
            }

            var contentWidth = Math.Max(1, maxX - minX);
            var contentHeight = Math.Max(1, maxY - minY);
            var viewportWidth = Math.Max(1, Bounds.Width - 30);
            var viewportHeight = Math.Max(1, Bounds.Height - 30);
            var scaleX = viewportWidth / contentWidth;
            var scaleY = viewportHeight / contentHeight;
            Zoom = Math.Max(0.35, Math.Min(1.5, Math.Min(scaleX, scaleY)));

            var cx = (minX + maxX) * 0.5;
            var cy = (minY + maxY) * 0.5;
            Pan = new Vector((Bounds.Width * 0.5) - (cx * Zoom), (Bounds.Height * 0.5) - (cy * Zoom));
            InvalidateVisual();
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            context.DrawRectangle(Background ?? Brushes.White, null, new Rect(Bounds.Size));

            foreach (var edge in _edges)
            {
                if (!_nodes.TryGetValue(edge.FromKey, out var from) || !_nodes.TryGetValue(edge.ToKey, out var to))
                {
                    continue;
                }

                Point p1 = WorldToScreen(from.Center);
                Point p2 = WorldToScreen(to.Center);
                DrawDirectedEdge(context, p1, p2, edge.HasCondition);
            }

            foreach (var kvp in _nodes)
            {
                var state = kvp.Value;
                bool selected = string.Equals(kvp.Key, _selectedKey, StringComparison.OrdinalIgnoreCase);
                bool hovered = string.Equals(kvp.Key, _hoveredKey, StringComparison.OrdinalIgnoreCase);
                bool highlighted = _highlightedKeys.Contains(kvp.Key);
                bool isLinkSource = _isLinkDragging && _linkDragExceededThreshold && string.Equals(kvp.Key, _linkDragSourceKey, StringComparison.OrdinalIgnoreCase);
                DrawNode(context, state, selected, isLinkSource, hovered, highlighted);
            }

            if (_isLinkDragging && _linkDragExceededThreshold && !string.IsNullOrEmpty(_linkDragSourceKey) && _nodes.TryGetValue(_linkDragSourceKey, out var dragSource))
            {
                Point from = WorldToScreen(dragSource.Center);
                Point to = _linkDragCurrentScreen;
                var world = ScreenToWorld(to);
                string hoverKey = HitTestNode(world);
                bool validTarget = !string.IsNullOrEmpty(hoverKey) && !string.Equals(hoverKey, _linkDragSourceKey, StringComparison.OrdinalIgnoreCase);
                var lineColor = validTarget ? Color.Parse("#1A73E8") : Color.Parse("#F9A825");
                var pen = new Pen(new SolidColorBrush(lineColor), 2.5, DashStyle.Dash);
                context.DrawLine(pen, from, to);
                DrawArrowHead(context, from, to, lineColor);
            }

            DrawMinimap(context);
        }

        private void DrawMinimap(DrawingContext context)
        {
            if (_nodes.Count == 0 || Bounds.Width < 200 || Bounds.Height < 200) return;

            double mmW = 140, mmH = 100;
            double mmMargin = 8;
            double mmX = Bounds.Width - mmW - mmMargin;
            double mmY = Bounds.Height - mmH - mmMargin;

            context.DrawRectangle(
                new SolidColorBrush(Color.FromArgb(210, 245, 245, 250)),
                new Pen(new SolidColorBrush(Color.Parse("#B0BEC5")), 1),
                new Rect(mmX, mmY, mmW, mmH), 4, 4);

            double gMinX = double.MaxValue, gMinY = double.MaxValue, gMaxX = double.MinValue, gMaxY = double.MinValue;
            foreach (var s in _nodes.Values)
            {
                gMinX = Math.Min(gMinX, s.Center.X);
                gMinY = Math.Min(gMinY, s.Center.Y);
                gMaxX = Math.Max(gMaxX, s.Center.X);
                gMaxY = Math.Max(gMaxY, s.Center.Y);
            }
            double gW = Math.Max(1, gMaxX - gMinX + NodeWidth);
            double gH = Math.Max(1, gMaxY - gMinY + NodeHeight);
            double mmPad = 8;
            double scaleX = (mmW - mmPad * 2) / gW;
            double scaleY = (mmH - mmPad * 2) / gH;
            double sc = Math.Min(scaleX, scaleY);

            double offX = mmX + mmPad + ((mmW - mmPad * 2) - gW * sc) / 2;
            double offY = mmY + mmPad + ((mmH - mmPad * 2) - gH * sc) / 2;

            foreach (var edge in _edges)
            {
                if (!_nodes.TryGetValue(edge.FromKey, out var mf) || !_nodes.TryGetValue(edge.ToKey, out var mt)) continue;
                var mp1 = new Point(offX + (mf.Center.X - gMinX + NodeWidth / 2) * sc, offY + (mf.Center.Y - gMinY + NodeHeight / 2) * sc);
                var mp2 = new Point(offX + (mt.Center.X - gMinX + NodeWidth / 2) * sc, offY + (mt.Center.Y - gMinY + NodeHeight / 2) * sc);
                context.DrawLine(new Pen(new SolidColorBrush(Color.FromArgb(80, 125, 135, 153)), 0.5), mp1, mp2);
            }

            foreach (var kvp in _nodes)
            {
                var s = kvp.Value;
                double nx = offX + (s.Center.X - gMinX + NodeWidth / 2) * sc;
                double ny = offY + (s.Center.Y - gMinY + NodeHeight / 2) * sc;
                var kind = s.Data?.Kind ?? DlgGraphNodeKind.Entry;
                Color c = kind == DlgGraphNodeKind.Reply ? ReplyStroke : kind == DlgGraphNodeKind.Starter ? StarterStroke : EntryStroke;
                bool sel = string.Equals(kvp.Key, _selectedKey, StringComparison.OrdinalIgnoreCase);
                double r = sel ? 3.5 : 2.5;
                context.DrawEllipse(new SolidColorBrush(c), null, new Point(nx, ny), r, r);
            }

            double vpLeft = (-Pan.X / Zoom - gMinX + NodeWidth / 2) * sc + offX;
            double vpTop = (-Pan.Y / Zoom - gMinY + NodeHeight / 2) * sc + offY;
            double vpW = (Bounds.Width / Zoom) * sc;
            double vpH = (Bounds.Height / Zoom) * sc;
            var vpRect = new Rect(vpLeft, vpTop, vpW, vpH);
            var mmRect = new Rect(mmX, mmY, mmW, mmH);
            vpRect = vpRect.Intersect(mmRect);
            if (vpRect.Width > 0 && vpRect.Height > 0)
            {
                context.DrawRectangle(
                    new SolidColorBrush(Color.FromArgb(30, 25, 118, 210)),
                    new Pen(new SolidColorBrush(Color.FromArgb(120, 25, 118, 210)), 1),
                    vpRect, 2, 2);
            }
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            Focus();
            _lastPointer = e.GetPosition(this);

            var props = e.GetCurrentPoint(this).Properties;

            if (props.IsRightButtonPressed)
            {
                var world = ScreenToWorld(_lastPointer);
                string key = HitTestNode(world);
                if (!string.IsNullOrEmpty(key))
                {
                    _linkDragSourceKey = key;
                    _linkDragStartScreen = _lastPointer;
                    _linkDragCurrentScreen = _lastPointer;
                    _isLinkDragging = true;
                    _linkDragExceededThreshold = false;
                    _selectedKey = key;
                    if (_nodes.TryGetValue(key, out var node))
                    {
                        NodeSelected?.Invoke(node.Data?.Tag);
                    }
                    e.Pointer.Capture(this);
                    e.Handled = true;
                    InvalidateVisual();
                }
                return;
            }

            if (!props.IsLeftButtonPressed)
            {
                return;
            }

            var worldL = ScreenToWorld(_lastPointer);
            string keyL = HitTestNode(worldL);
            if (!string.IsNullOrEmpty(keyL))
            {
                _draggingNodeKey = keyL;
                _selectedKey = keyL;
                if (_nodes.TryGetValue(keyL, out var node))
                {
                    NodeSelected?.Invoke(node.Data?.Tag);
                }
            }
            else
            {
                _isPanning = true;
            }

            e.Pointer.Capture(this);
            e.Handled = true;
            InvalidateVisual();
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if (e.Pointer.Captured != this)
            {
                return;
            }

            var now = e.GetPosition(this);
            var delta = now - _lastPointer;
            _lastPointer = now;

            if (_isLinkDragging)
            {
                _linkDragCurrentScreen = now;
                if (!_linkDragExceededThreshold)
                {
                    var dx = now.X - _linkDragStartScreen.X;
                    var dy = now.Y - _linkDragStartScreen.Y;
                    if (Math.Sqrt(dx * dx + dy * dy) >= LinkDragThreshold)
                        _linkDragExceededThreshold = true;
                }
                if (_linkDragExceededThreshold)
                    InvalidateVisual();
                e.Handled = true;
                return;
            }

            if (!string.IsNullOrEmpty(_draggingNodeKey) && _nodes.TryGetValue(_draggingNodeKey, out var state))
            {
                state.Center = new Point(state.Center.X + (delta.X / Zoom), state.Center.Y + (delta.Y / Zoom));
                state.IsPinned = true;
                InvalidateVisual();
                e.Handled = true;
                return;
            }

            if (_isPanning)
            {
                Pan += delta;
                InvalidateVisual();
                e.Handled = true;
                return;
            }

            UpdateHover(now);
        }

        private void UpdateHover(Point screenPos)
        {
            var world = ScreenToWorld(screenPos);
            string key = HitTestNode(world);
            if (!string.Equals(key ?? "", _hoveredKey ?? "", StringComparison.OrdinalIgnoreCase))
            {
                _hoveredKey = key;
                InvalidateVisual();

                if (!string.IsNullOrEmpty(key) && _nodes.TryGetValue(key, out var state))
                {
                    var tip = (state.Data?.Subtitle ?? "").Trim();
                    if (tip.Length > 120) tip = tip.Substring(0, 117) + "...";
                    if (!string.IsNullOrEmpty(tip))
                        ToolTip.SetTip(this, tip);
                    else
                        ToolTip.SetTip(this, null);
                    ToolTip.SetShowDelay(this, 300);
                }
                else
                {
                    ToolTip.SetTip(this, null);
                }
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (e.Pointer.Captured == this)
            {
                e.Pointer.Capture(null);
            }

            if (_isLinkDragging && !string.IsNullOrEmpty(_linkDragSourceKey))
            {
                bool exceeded = _linkDragExceededThreshold;
                _isLinkDragging = false;

                if (exceeded)
                {
                    var releasePos = e.GetPosition(this);
                    var world = ScreenToWorld(releasePos);
                    string targetKey = HitTestNode(world);

                    object sourceTag = null;
                    if (_nodes.TryGetValue(_linkDragSourceKey, out var srcState))
                    {
                        sourceTag = srcState.Data?.Tag;
                    }

                    if (!string.IsNullOrEmpty(targetKey) && !string.Equals(targetKey, _linkDragSourceKey, StringComparison.OrdinalIgnoreCase))
                    {
                        object targetTag = null;
                        if (_nodes.TryGetValue(targetKey, out var tgtState))
                        {
                            targetTag = tgtState.Data?.Tag;
                        }
                        LinkDragCompletedOnNode?.Invoke(sourceTag, targetTag, releasePos);
                    }
                    else if (string.IsNullOrEmpty(targetKey))
                    {
                        LinkDragCompletedOnEmpty?.Invoke(sourceTag, world);
                    }

                    _linkDragSourceKey = null;
                    InvalidateVisual();
                    e.Handled = true;
                    return;
                }

                _linkDragSourceKey = null;
                InvalidateVisual();
            }

            if (!string.IsNullOrEmpty(_draggingNodeKey) && _nodes.TryGetValue(_draggingNodeKey, out var state))
            {
                NodePositionCommitted?.Invoke(_draggingNodeKey, state.Center);
            }

            _draggingNodeKey = null;
            _isPanning = false;
        }

        protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
        {
            base.OnPointerWheelChanged(e);
            var before = ScreenToWorld(e.GetPosition(this));
            double factor = e.Delta.Y > 0 ? 1.12 : 0.9;
            Zoom = Math.Max(0.2, Math.Min(2.5, Zoom * factor));
            var after = ScreenToWorld(e.GetPosition(this));
            Pan += new Vector((after.X - before.X) * Zoom, (after.Y - before.Y) * Zoom);
            InvalidateVisual();
            ZoomChanged?.Invoke();
            e.Handled = true;
        }

        public void ZoomIn()
        {
            var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
            var before = ScreenToWorld(center);
            Zoom = Math.Max(0.2, Math.Min(2.5, Zoom * 1.25));
            var after = ScreenToWorld(center);
            Pan += new Vector((after.X - before.X) * Zoom, (after.Y - before.Y) * Zoom);
            InvalidateVisual();
            ZoomChanged?.Invoke();
        }

        public void ZoomOut()
        {
            var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
            var before = ScreenToWorld(center);
            Zoom = Math.Max(0.2, Math.Min(2.5, Zoom * 0.8));
            var after = ScreenToWorld(center);
            Pan += new Vector((after.X - before.X) * Zoom, (after.Y - before.Y) * Zoom);
            InvalidateVisual();
            ZoomChanged?.Invoke();
        }

        public void ZoomReset()
        {
            Zoom = 1.0;
            Pan = new Vector(60, 60);
            InvalidateVisual();
            ZoomChanged?.Invoke();
        }

        private void ComputeLevels()
        {
            var incoming = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (var key in _nodes.Keys)
            {
                incoming[key] = 0;
            }
            foreach (var edge in _edges)
            {
                if (incoming.ContainsKey(edge.ToKey))
                {
                    incoming[edge.ToKey] = incoming[edge.ToKey] + 1;
                }
            }

            var queue = new Queue<string>();
            foreach (var kvp in incoming)
            {
                if (kvp.Value == 0)
                {
                    _levels[kvp.Key] = 0;
                    queue.Enqueue(kvp.Key);
                }
            }

            if (queue.Count == 0)
            {
                foreach (var key in _nodes.Keys)
                {
                    _levels[key] = 0;
                }
                return;
            }

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                int baseLevel = _levels[current];
                foreach (var edge in _edges.Where(e => string.Equals(e.FromKey, current, StringComparison.OrdinalIgnoreCase)))
                {
                    if (!_levels.ContainsKey(edge.ToKey) || _levels[edge.ToKey] < baseLevel + 1)
                    {
                        _levels[edge.ToKey] = baseLevel + 1;
                    }
                    if (incoming.ContainsKey(edge.ToKey))
                    {
                        incoming[edge.ToKey] = incoming[edge.ToKey] - 1;
                        if (incoming[edge.ToKey] <= 0)
                        {
                            queue.Enqueue(edge.ToKey);
                        }
                    }
                }
            }

            foreach (var key in _nodes.Keys)
            {
                if (!_levels.ContainsKey(key))
                {
                    _levels[key] = 0;
                }
            }
        }

        private string HitTestNode(Point world)
        {
            foreach (var kvp in _nodes.Reverse())
            {
                var center = kvp.Value.Center;
                var rect = new Rect(center.X - (NodeWidth / 2), center.Y - (NodeHeight / 2), NodeWidth, NodeHeight);
                if (rect.Contains(world))
                {
                    return kvp.Key;
                }
            }
            return null;
        }

        private static readonly Color EdgeNormal = Color.Parse("#7D8799");
        private static readonly Color EdgeConditional = Color.Parse("#E65100");

        private void DrawDirectedEdge(DrawingContext context, Point p1, Point p2, bool hasCondition = false)
        {
            var edgeColor = hasCondition ? EdgeConditional : EdgeNormal;
            var edgeBrush = new SolidColorBrush(edgeColor);
            double hw = (NodeWidth * Zoom) / 2;
            Point cp1 = new Point(p1.X + hw * 0.6, p1.Y);
            Point cp2 = new Point(p2.X - hw * 0.6, p2.Y);
            var geo = new StreamGeometry();
            using (var gc = geo.Open())
            {
                gc.BeginFigure(p1, false);
                gc.CubicBezierTo(cp1, cp2, p2);
                gc.EndFigure(false);
            }
            var pen = hasCondition
                ? new Pen(edgeBrush, 1.8, DashStyle.Dash)
                : new Pen(edgeBrush, 1.5);
            context.DrawGeometry(null, pen, geo);

            Vector dir = p2 - cp2;
            double len = Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
            if (len < 1) { dir = p2 - p1; len = Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y); }
            if (len < 1) return;
            dir = new Vector(dir.X / len, dir.Y / len);
            Vector n = new Vector(-dir.Y, dir.X);
            Point tip = p2;
            var arrowGeo = new StreamGeometry();
            using (var gc = arrowGeo.Open())
            {
                gc.BeginFigure(tip, true);
                gc.LineTo(tip - dir * 10 + n * 4.5);
                gc.LineTo(tip - dir * 10 - n * 4.5);
                gc.EndFigure(true);
            }
            context.DrawGeometry(edgeBrush, null, arrowGeo);
        }

        // Node type colors from vendor/src/toolset/gui/editors/dlg (model.py, tree_view.py) - see DlgGraphColors.cs
        private static readonly Color EntryFill = DlgGraphColors.EntryFill;
        private static readonly Color EntryStroke = DlgGraphColors.EntryStroke;
        private static readonly Color EntryFillSel = DlgGraphColors.EntryFillSel;
        private static readonly Color ReplyFill = DlgGraphColors.ReplyFill;
        private static readonly Color ReplyStroke = DlgGraphColors.ReplyStroke;
        private static readonly Color ReplyFillSel = DlgGraphColors.ReplyFillSel;
        private static readonly Color StarterFill = DlgGraphColors.StarterFill;
        private static readonly Color StarterStroke = DlgGraphColors.StarterStroke;
        private static readonly Color StarterFillSel = DlgGraphColors.StarterFillSel;
        private static readonly Color HoverGlow = Color.Parse("#42A5F5");
        private static readonly Color LinkSrcFill = Color.Parse("#FFF8E1");
        private static readonly Color LinkSrcStroke = Color.Parse("#F9A825");

        private static readonly Color OrphanOutline = Color.Parse("#D32F2F");

        private static readonly Color SearchHighlightRing = Color.Parse("#00BFA5");

        private void DrawNode(DrawingContext context, DlgGraphNodeState state, bool selected, bool isLinkSource = false, bool hovered = false, bool highlighted = false)
        {
            Point c = WorldToScreen(state.Center);
            double rx = (NodeWidth * Zoom) / 2;
            double ry = (NodeHeight * Zoom) / 2;
            var kind = state.Data?.Kind ?? DlgGraphNodeKind.Entry;

            Color fillC, strokeC;
            if (isLinkSource)
            {
                fillC = LinkSrcFill;
                strokeC = LinkSrcStroke;
            }
            else if (selected)
            {
                switch (kind)
                {
                    case DlgGraphNodeKind.Reply: fillC = ReplyFillSel; strokeC = ReplyStroke; break;
                    case DlgGraphNodeKind.Starter: fillC = StarterFillSel; strokeC = StarterStroke; break;
                    default: fillC = EntryFillSel; strokeC = EntryStroke; break;
                }
            }
            else
            {
                switch (kind)
                {
                    case DlgGraphNodeKind.Reply: fillC = ReplyFill; strokeC = ReplyStroke; break;
                    case DlgGraphNodeKind.Starter: fillC = StarterFill; strokeC = StarterStroke; break;
                    default: fillC = EntryFill; strokeC = EntryStroke; break;
                }
            }

            double strokeWidth = (selected || isLinkSource) ? 2.5 : 1.5;

            if (state.IsOrphan && !isLinkSource)
            {
                var orphanPen = new Pen(new SolidColorBrush(OrphanOutline), 2, DashStyle.Dash);
                if (kind == DlgGraphNodeKind.Reply)
                    context.DrawRectangle(null, orphanPen, new Rect(c.X - rx - 4, c.Y - ry - 4, (rx + 4) * 2, (ry + 4) * 2), 12 * Zoom, 12 * Zoom);
                else
                    context.DrawEllipse(null, orphanPen, c, rx + 4, ry + 4);
            }

            if (hovered && !isLinkSource)
            {
                if (kind == DlgGraphNodeKind.Reply)
                    context.DrawRectangle(null, new Pen(new SolidColorBrush(HoverGlow), strokeWidth + 3), new Rect(c.X - rx - 3, c.Y - ry - 3, (rx + 3) * 2, (ry + 3) * 2), 12 * Zoom, 12 * Zoom);
                else
                    context.DrawEllipse(null, new Pen(new SolidColorBrush(HoverGlow), strokeWidth + 3), c, rx + 3, ry + 3);
            }

            if (highlighted && !isLinkSource)
            {
                var hlPen = new Pen(new SolidColorBrush(SearchHighlightRing), 3);
                if (kind == DlgGraphNodeKind.Reply)
                    context.DrawRectangle(null, hlPen, new Rect(c.X - rx - 5, c.Y - ry - 5, (rx + 5) * 2, (ry + 5) * 2), 14 * Zoom, 14 * Zoom);
                else
                    context.DrawEllipse(null, hlPen, c, rx + 5, ry + 5);
            }

            if (highlighted && !isLinkSource)
            {
                var hlPen = new Pen(new SolidColorBrush(SearchHighlightRing), 3);
                if (kind == DlgGraphNodeKind.Reply)
                    context.DrawRectangle(null, hlPen, new Rect(c.X - rx - 5, c.Y - ry - 5, (rx + 5) * 2, (ry + 5) * 2), 14 * Zoom, 14 * Zoom);
                else
                    context.DrawEllipse(null, hlPen, c, rx + 5, ry + 5);
            }

            var fill = new SolidColorBrush(fillC);
            var stroke = new Pen(new SolidColorBrush(strokeC), strokeWidth);

            if (kind == DlgGraphNodeKind.Reply)
            {
                var rect = new Rect(c.X - rx, c.Y - ry, rx * 2, ry * 2);
                context.DrawRectangle(fill, stroke, rect, 10 * Zoom, 10 * Zoom);
            }
            else
            {
                context.DrawEllipse(fill, stroke, c, rx, ry);
            }

            var kindLabel = kind == DlgGraphNodeKind.Reply ? "Reply" : kind == DlgGraphNodeKind.Starter ? "Starter" : "Entry";
            var kindText = new FormattedText(
                kindLabel,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI", FontStyle.Normal, FontWeight.Bold),
                9 * Zoom,
                new SolidColorBrush(strokeC));
            context.DrawText(kindText, new Point(c.X - kindText.Width / 2, c.Y - ry + 5 * Zoom));

            var title = (state.Data?.Title ?? "").Trim();
            var subtitle = (state.Data?.Subtitle ?? "").Trim();
            if (title.Length > 50) title = title.Substring(0, 47) + "...";
            if (subtitle.Length > 55) subtitle = subtitle.Substring(0, 52) + "...";

            var titleText = new FormattedText(
                title,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI", FontStyle.Normal, FontWeight.SemiBold),
                11 * Zoom,
                new SolidColorBrush(Color.Parse("#1F1F1F")));
            context.DrawText(titleText, new Point(c.X - titleText.Width / 2, c.Y - 6 * Zoom));

            var subtitleText = new FormattedText(
                subtitle,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI"),
                9.5 * Zoom,
                new SolidColorBrush(Color.Parse("#5F6368")));
            context.DrawText(subtitleText, new Point(c.X - subtitleText.Width / 2, c.Y + 7 * Zoom));

            DrawBadges(context, state, c, rx, ry, strokeC);
            DrawChildCountPill(context, state, c, rx, ry, strokeC);
        }

        private void DrawChildCountPill(DrawingContext context, DlgGraphNodeState state, Point c, double rx, double ry, Color accent)
        {
            int count = state.Data?.ChildCount ?? 0;
            if (count <= 0) return;

            string text = count.ToString();
            double fontSize = 8.5 * Zoom;
            var ft = new FormattedText(
                text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Segoe UI", FontStyle.Normal, FontWeight.Bold),
                fontSize,
                Brushes.White);

            double pillW = Math.Max(ft.Width + 8 * Zoom, 16 * Zoom);
            double pillH = ft.Height + 4 * Zoom;
            double px = c.X + rx - pillW * 0.5;
            double py = c.Y - ry - pillH * 0.3;

            var pillRect = new Rect(px, py, pillW, pillH);
            context.DrawRectangle(new SolidColorBrush(accent), null, pillRect, pillH / 2, pillH / 2);
            context.DrawText(ft, new Point(px + (pillW - ft.Width) / 2, py + (pillH - ft.Height) / 2));
        }

        private void DrawBadges(DrawingContext context, DlgGraphNodeState state, Point c, double rx, double ry, Color accentColor)
        {
            var badges = state.Data?.Badges ?? DlgGraphNodeBadges.None;
            if (badges == DlgGraphNodeBadges.None) return;

            double badgeSize = 7 * Zoom;
            double spacing = badgeSize * 2.2;
            var icons = new List<(string symbol, Color color)>();
            if ((badges & DlgGraphNodeBadges.HasScript) != 0) icons.Add(("S", Color.Parse("#6A1B9A")));
            if ((badges & DlgGraphNodeBadges.HasCondition) != 0) icons.Add(("?", Color.Parse("#E65100")));
            if ((badges & DlgGraphNodeBadges.HasSound) != 0) icons.Add(("\u266A", Color.Parse("#00695C")));
            if ((badges & DlgGraphNodeBadges.HasVoice) != 0) icons.Add(("\u25B6", Color.Parse("#0277BD")));

            double totalWidth = icons.Count * spacing;
            double startX = c.X - totalWidth / 2 + spacing / 2;
            double badgeY = c.Y + ry - badgeSize * 2.6;

            for (int i = 0; i < icons.Count; i++)
            {
                var pos = new Point(startX + i * spacing, badgeY);
                context.DrawEllipse(new SolidColorBrush(Color.Parse("#FFFFFF")), new Pen(new SolidColorBrush(icons[i].color), 1), pos, badgeSize, badgeSize);
                var ft = new FormattedText(
                    icons[i].symbol,
                    System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("Segoe UI", FontStyle.Normal, FontWeight.Bold),
                    badgeSize * 1.3,
                    new SolidColorBrush(icons[i].color));
                context.DrawText(ft, new Point(pos.X - ft.Width / 2, pos.Y - ft.Height / 2));
            }
        }

        private Point WorldToScreen(Point world)
        {
            return new Point((world.X * Zoom) + Pan.X, (world.Y * Zoom) + Pan.Y);
        }

        private Point ScreenToWorld(Point screen)
        {
            return new Point((screen.X - Pan.X) / Zoom, (screen.Y - Pan.Y) / Zoom);
        }

        /// <summary>
        /// Returns the Tag (e.g. DLGLink) of the node at the given control coordinates, or null if none.
        /// Used for context menu and selection from the host.
        /// </summary>
        public object GetNodeTagAt(Point controlPoint)
        {
            var world = ScreenToWorld(controlPoint);
            string key = HitTestNode(world);
            if (string.IsNullOrEmpty(key) || !_nodes.TryGetValue(key, out var state))
            {
                return null;
            }
            return state.Data?.Tag;
        }

        /// <summary>Converts screen (control) coordinates to world coordinates.</summary>
        public Point ScreenToWorldPublic(Point screen) => ScreenToWorld(screen);

        private void DrawArrowHead(DrawingContext context, Point from, Point to, Color color)
        {
            Vector dir = to - from;
            double len = Math.Sqrt((dir.X * dir.X) + (dir.Y * dir.Y));
            if (len < 1) return;
            dir = new Vector(dir.X / len, dir.Y / len);
            Vector normal = new Vector(-dir.Y, dir.X);
            Point left = to - (dir * 14) + (normal * 6);
            Point right = to - (dir * 14) - (normal * 6);
            var geometry = new StreamGeometry();
            using (var gc = geometry.Open())
            {
                gc.BeginFigure(to, true);
                gc.LineTo(left);
                gc.LineTo(right);
                gc.EndFigure(true);
            }
            context.DrawGeometry(new SolidColorBrush(color), null, geometry);
        }

        /// <summary>
        /// Pins a node at the given world position and refreshes the display.
        /// Used by the editor to place newly created nodes under the mouse.
        /// </summary>
        public void SetNodePosition(string key, Point worldPos)
        {
            if (string.IsNullOrEmpty(key) || !_nodes.TryGetValue(key, out var state))
                return;
            state.Center = worldPos;
            state.IsPinned = true;
            InvalidateVisual();
        }
    }
}
