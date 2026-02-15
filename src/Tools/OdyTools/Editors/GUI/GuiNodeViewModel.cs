using System.Collections.Generic;
using BioWare.Resource.Formats.GFF;

namespace OdyTools.Editors.GUI
{
    /// <summary>
    /// View model for a single node in the GUI tree (control or pseudo-child PROTOITEM/SCROLLBAR).
    /// </summary>
    public class GuiNodeViewModel
    {
        public string Label { get; set; }
        public GFFStruct Data { get; set; }
        public List<GuiNodeViewModel> Children { get; } = new List<GuiNodeViewModel>();

        public static GuiNodeViewModel FromStruct(GFFStruct node)
        {
            if (node == null) return null;
            var vm = new GuiNodeViewModel
            {
                Label = OdyToolGUIHelpers.GetNodeLabel(node),
                Data = node
            };
            var controls = OdyToolGUIHelpers.GetChildren(node);
            if (controls != null)
            {
                foreach (var c in controls)
                {
                    var child = FromStruct(c);
                    if (child != null) vm.Children.Add(child);
                }
            }
            var proto = OdyToolGUIHelpers.GetProtoItem(node);
            if (proto != null)
            {
                var p = FromStruct(proto);
                if (p != null) vm.Children.Add(p);
            }
            var scroll = OdyToolGUIHelpers.GetScrollBar(node);
            if (scroll != null)
            {
                var s = FromStruct(scroll);
                if (s != null) vm.Children.Add(s);
            }
            return vm;
        }

        /// <summary>Build tree from GFF root. Root may be the first control (CONTROLS[0]) or root struct with CONTROLS.</summary>
        public static GuiNodeViewModel FromGffRoot(GFF gff)
        {
            if (gff?.Root == null) return null;
            if (gff.Root.TryGetList(OdyToolGUIHelpers.ControllistLabel, out var list) && list.Count > 0)
            {
                var first = list.At(0);
                return FromStruct(first);
            }
            return FromStruct(gff.Root);
        }
    }
}
