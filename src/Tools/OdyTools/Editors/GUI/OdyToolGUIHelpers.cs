using System;
using System.Collections.Generic;
using BioWare.Common;
using BioWare.Resource.Formats.GFF;

namespace OdyTools.Editors.GUI
{
    /// <summary>
    /// Helpers for KotOR GUI GFF structure (aligned with kotor-gui-editor and BioWare GUIReader/GUIWriter).
    /// Root has CONTROLS list; each control has TAG, EXTENT, BORDER, CONTROLS (children), PROTOITEM/SCROLLBAR structs.
    /// </summary>
    public static class OdyToolGUIHelpers
    {
        public const string ControllistLabel = "CONTROLS";
        public const string TagLabel = "TAG";
        public const string ExtentLabel = "EXTENT";
        public const string BorderLabel = "BORDER";
        public const string ProtoItemLabel = "PROTOITEM";
        public const string ScrollbarLabel = "SCROLLBAR";
        public const string FillStyleLabel = "FILLSTYLE";
        public const string FillResrefLabel = "FILL";
        public const int FillStyleImage = 2;

        /// <summary>Gets the display label for a GUI node (TAG string).</summary>
        public static string GetNodeLabel(GFFStruct node)
        {
            if (node == null) return string.Empty;
            return node.GetString(TagLabel) ?? string.Empty;
        }

        /// <summary>Gets child controls list (CONTROLS).</summary>
        public static GFFList GetChildren(GFFStruct node)
        {
            if (node == null) return null;
            return node.TryGetList(ControllistLabel, out var list) ? list : null;
        }

        /// <summary>Gets EXTENT struct (LEFT, TOP, WIDTH, HEIGHT).</summary>
        public static GFFStruct GetExtent(GFFStruct node)
        {
            if (node == null) return null;
            return node.TryGetStruct(ExtentLabel, out var extent) ? extent : null;
        }

        /// <summary>Gets BORDER struct.</summary>
        public static GFFStruct GetBorder(GFFStruct node)
        {
            if (node == null) return null;
            return node.TryGetStruct(BorderLabel, out var border) ? border : null;
        }

        /// <summary>Gets the texture fill resref from BORDER when FILLSTYLE is 2 (image fill).</summary>
        public static string GetBorderFillResRef(GFFStruct node)
        {
            var border = GetBorder(node);
            if (border == null) return null;
            if (border.GetInt32(FillStyleLabel) != FillStyleImage) return null;
            var resRef = border.GetResRef(FillResrefLabel);
            var s = resRef.ToString();
            return string.IsNullOrWhiteSpace(s) ? null : s.Trim();
        }

        /// <summary>Gets PROTOITEM nested struct if present.</summary>
        public static GFFStruct GetProtoItem(GFFStruct node)
        {
            if (node == null) return null;
            return node.TryGetStruct(ProtoItemLabel, out var s) ? s : null;
        }

        /// <summary>Gets SCROLLBAR nested struct if present.</summary>
        public static GFFStruct GetScrollBar(GFFStruct node)
        {
            if (node == null) return null;
            return node.TryGetStruct(ScrollbarLabel, out var s) ? s : null;
        }

        /// <summary>Returns extent values (left, top, width, height). Defaults to 0,0,1,1 if missing.</summary>
        public static void GetExtentValues(GFFStruct node, out int left, out int top, out int width, out int height)
        {
            left = 0; top = 0; width = 1; height = 1;
            var extent = GetExtent(node);
            if (extent == null) return;
            if (extent.Exists("LEFT")) left = extent.GetInt32("LEFT");
            if (extent.Exists("TOP")) top = extent.GetInt32("TOP");
            if (extent.Exists("WIDTH")) width = Math.Max(1, extent.GetInt32("WIDTH"));
            if (extent.Exists("HEIGHT")) height = Math.Max(1, extent.GetInt32("HEIGHT"));
        }

        /// <summary>Sets extent values on the node's EXTENT struct (creates if needed).</summary>
        public static void SetExtentValues(GFFStruct node, int left, int top, int width, int height)
        {
            var extent = node.TryGetStruct(ExtentLabel, out var e) ? e : new GFFStruct(0);
            extent.SetInt32("LEFT", left);
            extent.SetInt32("TOP", top);
            extent.SetInt32("WIDTH", Math.Max(1, width));
            extent.SetInt32("HEIGHT", Math.Max(1, height));
            if (!node.Exists(ExtentLabel))
                node.SetStruct(ExtentLabel, extent);
        }

        /// <summary>Collects all texture resrefs used for image fill (BORDER FILLSTYLE=2, FILL) under the given node.</summary>
        public static void CollectFillResRefs(GFFStruct node, HashSet<string> resRefs)
        {
            if (node == null || resRefs == null) return;
            string fill = GetBorderFillResRef(node);
            if (!string.IsNullOrEmpty(fill)) resRefs.Add(fill);

            var children = GetChildren(node);
            if (children != null)
            {
                foreach (var child in children)
                    CollectFillResRefs(child, resRefs);
            }
            var proto = GetProtoItem(node);
            if (proto != null) CollectFillResRefs(proto, resRefs);
            var scroll = GetScrollBar(node);
            if (scroll != null) CollectFillResRefs(scroll, resRefs);
        }
    }
}
