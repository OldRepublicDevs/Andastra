using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace OdyTools.Widgets
{
    /// <summary>
    /// Monaco/VS Code-style line numbers margin drawn to the left of the code editor.
    /// Matches VS Code Dark+ theme colors and typography.
    /// </summary>
    public class MonacoLineNumbersMargin : Control
    {
        public static readonly StyledProperty<int> LineCountProperty =
            AvaloniaProperty.Register<MonacoLineNumbersMargin, int>(nameof(LineCount), 1);

        public static readonly StyledProperty<double> LineHeightProperty =
            AvaloniaProperty.Register<MonacoLineNumbersMargin, double>(nameof(LineHeight), 20);

        public static readonly StyledProperty<double> ScrollOffsetYProperty =
            AvaloniaProperty.Register<MonacoLineNumbersMargin, double>(nameof(ScrollOffsetY));

        public static readonly StyledProperty<int> FirstVisibleLineProperty =
            AvaloniaProperty.Register<MonacoLineNumbersMargin, int>(nameof(FirstVisibleLine), 1);

        public static readonly StyledProperty<int> ActiveLineNumberProperty =
            AvaloniaProperty.Register<MonacoLineNumbersMargin, int>(nameof(ActiveLineNumber), 0);

        public static readonly StyledProperty<IBrush> ForegroundProperty =
            AvaloniaProperty.Register<MonacoLineNumbersMargin, IBrush>(nameof(Foreground), OdyTools.Themes.MonacoColors.LineNumbersForegroundBrush);

        public static readonly StyledProperty<FontFamily> FontFamilyProperty =
            AvaloniaProperty.Register<MonacoLineNumbersMargin, FontFamily>(nameof(FontFamily), new FontFamily("Cascadia Code, Consolas, Menlo, Monaco, monospace"));

        public static readonly StyledProperty<double> FontSizeProperty =
            AvaloniaProperty.Register<MonacoLineNumbersMargin, double>(nameof(FontSize), 14);

        public IBrush Foreground
        {
            get => GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        public FontFamily FontFamily
        {
            get => GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public double FontSize
        {
            get => GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public int LineCount
        {
            get => GetValue(LineCountProperty);
            set => SetValue(LineCountProperty, value);
        }

        public double LineHeight
        {
            get => GetValue(LineHeightProperty);
            set => SetValue(LineHeightProperty, value);
        }

        public double ScrollOffsetY
        {
            get => GetValue(ScrollOffsetYProperty);
            set => SetValue(ScrollOffsetYProperty, value);
        }

        public int FirstVisibleLine
        {
            get => GetValue(FirstVisibleLineProperty);
            set => SetValue(FirstVisibleLineProperty, value);
        }

        public int ActiveLineNumber
        {
            get => GetValue(ActiveLineNumberProperty);
            set => SetValue(ActiveLineNumberProperty, value);
        }

        public MonacoLineNumbersMargin()
        {
            MinWidth = 50;
            ClipToBounds = true;
        }

        public override void Render(DrawingContext context)
        {
            context.FillRectangle(OdyTools.Themes.MonacoColors.EditorBackgroundBrush, Bounds);
            base.Render(context);

            if (LineCount <= 0 || LineHeight <= 0 || Bounds.Height <= 0)
                return;

            var fontFamily = FontFamily ?? new FontFamily("Cascadia Code, Consolas, Menlo, Monaco, monospace");
            var typeface = new Typeface(fontFamily, FontStyle.Normal, FontWeight.Normal);

            var foreground = Foreground ?? OdyTools.Themes.MonacoColors.LineNumbersForegroundBrush;

            double rightPadding = 16;
            double xOffset = Bounds.Width - rightPadding;

            int startLine = Math.Max(1, FirstVisibleLine);
            int endLine = startLine + (int)Math.Ceiling(Bounds.Height / LineHeight);

            for (int line = startLine; line <= Math.Min(endLine, LineCount); line++)
            {
                double y = (line - FirstVisibleLine) * LineHeight - (ScrollOffsetY % LineHeight);

                if (y + LineHeight < 0 || y > Bounds.Height)
                    continue;

                string num = line.ToString(CultureInfo.InvariantCulture);
                bool isActive = line == ActiveLineNumber;
                var brush = isActive ? OdyTools.Themes.MonacoColors.EditorForegroundBrush : foreground;

                var formattedText = new FormattedText(
                    num,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    FontSize > 0 ? FontSize : 14,
                    brush);

                double textX = xOffset - formattedText.Width;
                double textY = y + (LineHeight - formattedText.Height) / 2;

                context.DrawText(formattedText, new Point(textX, textY));
            }
        }
    }
}
