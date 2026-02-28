using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace OdyTools.Widgets
{
    /// <summary>
    /// Monaco/VS Code-style minimap: scaled overview of document on the right edge.
    /// </summary>
    public class MonacoMinimap : Control
    {
        public static readonly StyledProperty<string> TextProperty =
            AvaloniaProperty.Register<MonacoMinimap, string>(nameof(Text));

        public static readonly StyledProperty<double> LineHeightProperty =
            AvaloniaProperty.Register<MonacoMinimap, double>(nameof(LineHeight), 18);

        public static readonly StyledProperty<double> CharWidthProperty =
            AvaloniaProperty.Register<MonacoMinimap, double>(nameof(CharWidth), 8);

        public static readonly StyledProperty<int> VisibleStartLineProperty =
            AvaloniaProperty.Register<MonacoMinimap, int>(nameof(VisibleStartLine), 0);

        public static readonly StyledProperty<int> VisibleLineCountProperty =
            AvaloniaProperty.Register<MonacoMinimap, int>(nameof(VisibleLineCount), 20);

        public static readonly StyledProperty<FontFamily> FontFamilyProperty =
            AvaloniaProperty.Register<MonacoMinimap, FontFamily>(nameof(FontFamily),
                new FontFamily("Cascadia Code, Consolas, Menlo, Monaco, monospace"));

        public static readonly StyledProperty<double> FontSizeProperty =
            AvaloniaProperty.Register<MonacoMinimap, double>(nameof(FontSize), 12);

        public string Text { get => GetValue(TextProperty); set => SetValue(TextProperty, value); }
        public double LineHeight { get => GetValue(LineHeightProperty); set => SetValue(LineHeightProperty, value); }
        public double CharWidth { get => GetValue(CharWidthProperty); set => SetValue(CharWidthProperty, value); }
        public int VisibleStartLine { get => GetValue(VisibleStartLineProperty); set => SetValue(VisibleStartLineProperty, value); }
        public int VisibleLineCount { get => GetValue(VisibleLineCountProperty); set => SetValue(VisibleLineCountProperty, value); }
        public FontFamily FontFamily { get => GetValue(FontFamilyProperty); set => SetValue(FontFamilyProperty, value); }
        public double FontSize { get => GetValue(FontSizeProperty); set => SetValue(FontSizeProperty, value); }

        private const double MinimapCharScale = 0.2;
        private const double MinimapLineScale = 0.15;

        public MonacoMinimap()
        {
            MinWidth = 80;
            Width = 80;
            ClipToBounds = true;
        }

        public override void Render(DrawingContext context)
        {
            var bg = OdyTools.Themes.MonacoColors.EditorBackgroundBrush;
            context.FillRectangle(bg, Bounds);
            base.Render(context);

            var text = Text;
            if (string.IsNullOrEmpty(text) || Bounds.Width <= 0 || Bounds.Height <= 0)
                return;

            var lines = text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            if (lines.Length == 0) return;

            var fontFamily = FontFamily ?? new FontFamily("Consolas, monospace");
            var typeface = new Typeface(fontFamily, FontStyle.Normal, FontWeight.Normal);
            var brush = OdyTools.Themes.MonacoColors.EditorForegroundBrush;
            var scaledFontSize = Math.Max(2, FontSize * MinimapCharScale);
            var scaledLineHeight = LineHeight * MinimapLineScale;
            var maxCharsPerLine = (int)Math.Max(1, Bounds.Width / (CharWidth * MinimapCharScale));

            int startLine = Math.Max(0, VisibleStartLine);
            int endLine = Math.Min(lines.Length, startLine + (int)Math.Ceiling(Bounds.Height / scaledLineHeight));

            for (int i = startLine; i < endLine; i++)
            {
                var line = lines[i];
                if (line.Length > maxCharsPerLine)
                    line = line.Substring(0, maxCharsPerLine);
                if (string.IsNullOrEmpty(line)) line = " ";

                var ft = new FormattedText(line, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
                    typeface, scaledFontSize, brush);
                double y = (i - startLine) * scaledLineHeight;
                context.DrawText(ft, new Point(0, y));
            }

            // Visible region highlight
            double viewY = VisibleStartLine * scaledLineHeight;
            double viewH = VisibleLineCount * scaledLineHeight;
            var highlight = new SolidColorBrush(OdyTools.Themes.MonacoColors.EditorSelectionHighlightBackground);
            context.FillRectangle(highlight, new Rect(Bounds.Width - 4, viewY, 4, Math.Min(viewH, Bounds.Height)));
        }
    }
}
