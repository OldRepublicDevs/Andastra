using Avalonia.Media;

namespace OdyTools.Editors.DLG
{
    /// <summary>
    /// Graph node colors matching vendor/src/toolset/gui/editors/dlg exactly.
    /// Entry/Reply from model.py and tree_view.py (QPalette.Link fallback QColor(0, 120, 212));
    /// Entry = link adjusted: R=min(255,red*1.5+100), G=green*0.3, B=blue*0.3 → #64243F.
    /// Reply = link color → #0078D4.
    /// Starter uses end_dialog formula from model.py: R=min(255,red*1.2+80), G=green*0.7+50, B=blue*0.4 → #508654.
    /// </summary>
    public static class DlgGraphColors
    {
        // Default link color (vendor: QColor(0, 120, 212) when palette Link invalid)
        public const string LinkHex = "#0078D4";

        // Entry: red-ish from link (vendor model.py entry_color formula)
        public const string EntryHex = "#64243F";
        public const string EntryFillHex = "#FCE4EC"; // light tint same hue for fill

        // Reply: link color (vendor model.py reply_color = link_color)
        public const string ReplyHex = "#0078D4";
        public const string ReplyFillHex = "#E3F2FD"; // light blue fill

        // Starter: end_dialog formula (vendor model.py end_dialog_color_obj)
        public const string StarterHex = "#508654";
        public const string StarterFillHex = "#E8F5E9"; // light green tint

        // Selected (darker tint of same hue)
        public const string EntryFillSelHex = "#F8BBD9";
        public const string ReplyFillSelHex = "#BBDEFB";
        public const string StarterFillSelHex = "#C8E6C9";

        public static readonly Color EntryStroke = Color.Parse(EntryHex);
        public static readonly Color EntryFill = Color.Parse(EntryFillHex);
        public static readonly Color EntryFillSel = Color.Parse(EntryFillSelHex);

        public static readonly Color ReplyStroke = Color.Parse(ReplyHex);
        public static readonly Color ReplyFill = Color.Parse(ReplyFillHex);
        public static readonly Color ReplyFillSel = Color.Parse(ReplyFillSelHex);

        public static readonly Color StarterStroke = Color.Parse(StarterHex);
        public static readonly Color StarterFill = Color.Parse(StarterFillHex);
        public static readonly Color StarterFillSel = Color.Parse(StarterFillSelHex);
    }
}
