using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using BioWare.Resource.Formats.GFF;

namespace OdyTools.Editors.GUI
{
    /// <summary>
    /// Canvas that draws GUI nodes from GFF and supports selection, drag-move and resize handles.
    /// </summary>
    public class GUIPreviewControl : Control
    {
        public static readonly StyledProperty<GFFStruct> RootProperty =
            AvaloniaProperty.Register<GUIPreviewControl, GFFStruct>(nameof(Root));

        public static readonly StyledProperty<GFFStruct> SelectedNodeProperty =
            AvaloniaProperty.Register<GUIPreviewControl, GFFStruct>(nameof(SelectedNode));

        public static readonly StyledProperty<GUITextureCache> TextureCacheProperty =
            AvaloniaProperty.Register<GUIPreviewControl, GUITextureCache>(nameof(TextureCache));

        public static readonly StyledProperty<double> ZoomProperty =
            AvaloniaProperty.Register<GUIPreviewControl, double>(nameof(Zoom), 1.0);

        public GFFStruct Root { get => GetValue(RootProperty); set => SetValue(RootProperty, value); }
        public GFFStruct SelectedNode { get => GetValue(SelectedNodeProperty); set => SetValue(SelectedNodeProperty, value); }
        public GUITextureCache TextureCache { get => GetValue(TextureCacheProperty); set => SetValue(TextureCacheProperty, value); }
        public double Zoom { get => GetValue(ZoomProperty); set => SetValue(ZoomProperty, value); }

        public event Action<GFFStruct> SelectionChanged;
        public event Action DataChanged;

        private (double x, double y) _dragStart;
        private bool _isDragging;
        private bool _resizeHandle;
        private string _resizeEdge; // "left","right","top","bottom"

        public GUIPreviewControl()
        {
            ClipToBounds = true;
            Focusable = true;
        }

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            var p = e.GetPosition(this);
            var node = HitTestNode(p);
            if (node != null)
            {
                SelectedNode = node;
                SelectionChanged?.Invoke(node);
                _dragStart = (p.X, p.Y);
                _isDragging = true;
                _resizeHandle = false;
            }
            else
            {
                SelectedNode = null;
                SelectionChanged?.Invoke(null);
            }
            InvalidateVisual();
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if (!_isDragging) return;
            var pos = e.GetPosition(this);
            double dx = (pos.X - _dragStart.x) / Zoom;
            double dy = (pos.Y - _dragStart.y) / Zoom;
            _dragStart = (pos.X, pos.Y);

            if (_resizeHandle && !string.IsNullOrEmpty(_resizeEdge))
                ApplyResize(SelectedNode, _resizeEdge, dx, dy);
            else
                ApplyMove(SelectedNode, dx, dy);
            DataChanged?.Invoke();
            InvalidateVisual();
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            _isDragging = false;
            _resizeHandle = false;
            _resizeEdge = null;
        }

        private void ApplyMove(GFFStruct node, double dx, double dy)
        {
            if (node == null) return;
            OdyToolGUIHelpers.GetExtentValues(node, out int left, out int top, out int w, out int h);
            OdyToolGUIHelpers.SetExtentValues(node, (int)(left + dx), (int)(top + dy), w, h);
        }

        private void ApplyResize(GFFStruct node, string edge, double dx, double dy)
        {
            if (node == null) return;
            OdyToolGUIHelpers.GetExtentValues(node, out int left, out int top, out int width, out int height);
            switch (edge)
            {
                case "left":
                    left += (int)dx; width -= (int)dx;
                    break;
                case "right":
                    width += (int)dx;
                    break;
                case "top":
                    top += (int)dy; height -= (int)dy;
                    break;
                case "bottom":
                    height += (int)dy;
                    break;
            }
            if (width < 1) width = 1;
            if (height < 1) height = 1;
            OdyToolGUIHelpers.SetExtentValues(node, left, top, width, height);
        }

        private GFFStruct HitTestNode(Point p)
        {
            var list = new List<(GFFStruct node, double depth)>();
            CollectNodes(Root, 0, list);
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var node = list[i].node;
                OdyToolGUIHelpers.GetExtentValues(node, out int left, out int top, out int width, out int height);
                double x = left * Zoom, y = top * Zoom, w = width * Zoom, h = height * Zoom;
                if (p.X >= x && p.X <= x + w && p.Y >= y && p.Y <= y + h)
                    return node;
            }
            return null;
        }

        private void CollectNodes(GFFStruct node, int depth, List<(GFFStruct, double)> outList)
        {
            if (node == null) return;
            outList.Add((node, depth));
            var children = OdyToolGUIHelpers.GetChildren(node);
            if (children != null)
                foreach (var c in children) CollectNodes(c, depth + 1, outList);
            var proto = OdyToolGUIHelpers.GetProtoItem(node);
            if (proto != null) CollectNodes(proto, depth + 1, outList);
            var scroll = OdyToolGUIHelpers.GetScrollBar(node);
            if (scroll != null) CollectNodes(scroll, depth + 1, outList);
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            if (Root == null) return;
            var cache = TextureCache;
            double z = Zoom;
            RenderNode(context, Root, 0, 0, z, cache);
            if (SelectedNode != null)
                RenderSelectionHandles(context, SelectedNode, z);
        }

        private void RenderNode(DrawingContext context, GFFStruct node, double offsetX, double offsetY, double zoom, GUITextureCache cache)
        {
            OdyToolGUIHelpers.GetExtentValues(node, out int left, out int top, out int width, out int height);
            double x = offsetX + left * zoom, y = offsetY + top * zoom, w = width * zoom, h = height * zoom;

            var fillResRef = OdyToolGUIHelpers.GetBorderFillResRef(node);
            if (!string.IsNullOrEmpty(fillResRef) && cache != null)
            {
                var bmp = cache.GetBitmap(fillResRef);
                if (bmp != null)
                {
                    var src = new Rect(0, 0, bmp.PixelSize.Width, bmp.PixelSize.Height);
                    var dst = new Rect(x, y, w, h);
                    context.DrawImage(bmp, src, dst);
                }
                else
                {
                    context.FillRectangle(new SolidColorBrush(Color.FromArgb(60, 80, 80, 80)), new Rect(x, y, w, h));
                }
            }
            else
            {
                context.DrawRectangle(null, new Pen(Brushes.Gray, 1), new Rect(x, y, w, h));
            }

            var children = OdyToolGUIHelpers.GetChildren(node);
            if (children != null)
                foreach (var c in children) RenderNode(context, c, offsetX + left * zoom, offsetY + top * zoom, zoom, cache);
            var proto = OdyToolGUIHelpers.GetProtoItem(node);
            if (proto != null) RenderNode(context, proto, offsetX + left * zoom, offsetY + top * zoom, zoom, cache);
            var scroll = OdyToolGUIHelpers.GetScrollBar(node);
            if (scroll != null) RenderNode(context, scroll, offsetX + left * zoom, offsetY + top * zoom, zoom, cache);
        }

        private void RenderSelectionHandles(DrawingContext context, GFFStruct node, double zoom)
        {
            OdyToolGUIHelpers.GetExtentValues(node, out int left, out int top, out int width, out int height);
            double x = left * zoom, y = top * zoom, w = width * zoom, h = height * zoom;
            var pen = new Pen(Brushes.Lime, 2);
            context.DrawRectangle(null, pen, new Rect(x, y, w, h));
            double hs = Math.Max(4, 10 / zoom);
            var brush = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            context.FillRectangle(brush, new Rect(x - hs / 2, y + h / 2 - hs / 2, hs, hs)); // left
            context.FillRectangle(brush, new Rect(x + w - hs / 2, y + h / 2 - hs / 2, hs, hs)); // right
            context.FillRectangle(brush, new Rect(x + w / 2 - hs / 2, y - hs / 2, hs, hs)); // top
            context.FillRectangle(brush, new Rect(x + w / 2 - hs / 2, y + h - hs / 2, hs, hs)); // bottom
        }
    }
}
