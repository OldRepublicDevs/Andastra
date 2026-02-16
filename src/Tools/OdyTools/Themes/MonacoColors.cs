using Avalonia.Media;

namespace OdyTools.Themes
{
    /// <summary>
    /// VS Code Dark+ / Monaco editor color constants - 1:1 parity with VS Code default dark theme.
    /// Source: microsoft/vscode extensions/theme-defaults/themes/dark_vs.json, dark_plus.json
    /// </summary>
    public static class MonacoColors
    {
        // Editor
        public static readonly Color EditorBackground = Color.Parse("#1E1E1E");
        public static readonly Color EditorForeground = Color.Parse("#D4D4D4");
        public static readonly Color EditorInactiveSelectionBackground = Color.Parse("#3A3D41");
        public static readonly Color EditorSelectionHighlightBackground = Color.Parse("#ADD6FF26");
        public static readonly Color EditorCursorForeground = Color.Parse("#AEAFAD");
        public static readonly Color EditorLineHighlightBackground = Color.Parse("#2D2D30");

        // Indent guides
        public static readonly Color EditorIndentGuideBackground = Color.Parse("#404040");
        public static readonly Color EditorIndentGuideActiveBackground = Color.Parse("#707070");

        // Line numbers
        public static readonly Color LineNumbersForeground = Color.Parse("#858585");
        public static readonly Color LineNumbersActiveForeground = Color.Parse("#C6C6C6");

        // Gutter / glyph margin
        public static readonly Color EditorGutterBackground = Color.Parse("#1E1E1E");

        // Token colors (syntax highlighting)
        public static readonly Color Function = Color.Parse("#DCDCAA");
        public static readonly Color Type = Color.Parse("#4EC9B0");
        public static readonly Color Keyword = Color.Parse("#569CD6");
        public static readonly Color ControlFlow = Color.Parse("#C586C0");
        public static readonly Color Variable = Color.Parse("#9CDCFE");
        public static readonly Color Constant = Color.Parse("#4FC1FF");
        public static readonly Color String = Color.Parse("#CE9178");
        public static readonly Color Number = Color.Parse("#B5CEA8");
        public static readonly Color Comment = Color.Parse("#6A9955");
        public static readonly Color Invalid = Color.Parse("#F44747");
        public static readonly Color Storage = Color.Parse("#569CD6");

        // Bracket matching
        public static readonly Color BracketMatchBackground = Color.Parse("#ADD6FF26");

        // Status bar
        public static readonly Color StatusBarBackground = Color.Parse("#007ACC");
        public static readonly Color StatusBarForeground = Color.Parse("#FFFFFF");
        public static readonly Color StatusBarNoFolderBackground = Color.Parse("#68217A");

        // Sidebar / activity bar
        public static readonly Color SideBarBackground = Color.Parse("#252526");
        public static readonly Color ActivityBarBackground = Color.Parse("#333333");
        public static readonly Color SideBarForeground = Color.Parse("#CCCCCC");
        public static readonly Color SideBarTitleForeground = Color.Parse("#BBBBBB");

        // Panel (output, terminal)
        public static readonly Color PanelBackground = Color.Parse("#252526");
        public static readonly Color PanelForeground = Color.Parse("#CCCCCC");

        // Tabs
        public static readonly Color TabSelectedBackground = Color.Parse("#1E1E1E");
        public static readonly Color TabSelectedForeground = Color.Parse("#FFFFFF");
        public static readonly Color TabInactiveBackground = Color.Parse("#2D2D2D");
        public static readonly Color TabInactiveForeground = Color.Parse("#FFFFFF80");

        // Widget / borders
        public static readonly Color WidgetBorder = Color.Parse("#303031");

        public static readonly SolidColorBrush EditorBackgroundBrush = new SolidColorBrush(EditorBackground);
        public static readonly SolidColorBrush EditorForegroundBrush = new SolidColorBrush(EditorForeground);
        public static readonly SolidColorBrush EditorLineHighlightBrush = new SolidColorBrush(EditorLineHighlightBackground);
        public static readonly SolidColorBrush LineNumbersForegroundBrush = new SolidColorBrush(LineNumbersForeground);
        public static readonly SolidColorBrush EditorIndentGuideBrush = new SolidColorBrush(EditorIndentGuideBackground);
        public static readonly SolidColorBrush EditorIndentGuideActiveBrush = new SolidColorBrush(EditorIndentGuideActiveBackground);
    }
}
