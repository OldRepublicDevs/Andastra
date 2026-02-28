using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;

namespace OdyTools.Widgets
{
    public class AnimatedProgressBar : ProgressBar
    {
        private DispatcherTimer _timer;
        private int _offset;

        public AnimatedProgressBar()
        {
            _offset = 0;
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(50) // Update every 50 ms
            };
            _timer.Tick += (s, e) => UpdateAnimation();
            _timer.Start();
        }

        private void UpdateAnimation()
        {
            if (Maximum == Minimum)
            {
                return;
            }

            double width = Bounds.Width;
            if (width <= 0)
            {
                return;
            }

            double filledWidth = width * (Value - Minimum) / (Maximum - Minimum);
            if (filledWidth <= 0)
            {
                return;
            }

            _offset = (_offset + 1) % (int)filledWidth;
            InvalidateVisual();
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);

            if (Maximum == Minimum)
            {
                return;
            }

            double width = Bounds.Width;
            double height = Bounds.Height;
            double filledWidth = width * (Value - Minimum) / (Maximum - Minimum);
            filledWidth = Math.Max(filledWidth, height); // Ensure minimum width

            if (filledWidth <= 0)
            {
                return;
            }

            DrawShimmeringEffect(context, width, height, filledWidth);
        }

        private void DrawShimmeringEffect(DrawingContext context, double width, double height, double filledWidth)
        {
            double lightWidth = height * 2; // Width of the shimmering light effect

            double lightLeft = _offset - lightWidth / 2;
            double lightTop = 0;
            double lightRight = lightLeft + lightWidth;
            double lightBottom = height;

            // In PyKotor: if light_rect.left() < rect.left(): light_rect.moveLeft(rect.left())
            // moveLeft moves the rectangle so its left edge is at the specified position
            if (lightLeft < 0)
            {
                lightRight = lightRight - lightLeft; // Adjust right edge by the same amount
                lightLeft = 0;
            }

            // In PyKotor: if light_rect.right() > rect.right(): light_rect.moveRight(rect.right())
            // moveRight moves the rectangle so its right edge is at the specified position
            if (lightRight > filledWidth)
            {
                lightLeft = lightLeft - (lightRight - filledWidth); // Adjust left edge by the same amount
                lightRight = filledWidth;
            }

            // Only draw if the light rectangle intersects the filled area
            if (lightRight <= 0 || lightLeft >= filledWidth)
            {
                return;
            }

            // Ensure light rectangle doesn't go outside filled area after adjustments
            // This handles the case where lightWidth > filledWidth
            if (lightLeft < 0)
            {
                lightLeft = 0;
            }
            if (lightRight > filledWidth)
            {
                lightRight = filledWidth;
            }

            double chunkRadius = height / 2;

            // QLinearGradient(light_rect.left(), 0, light_rect.right(), 0)
            // setColorAt(0, QColor(255, 255, 255, 0))  # Transparent at the edges
            // setColorAt(0.5, QColor(255, 255, 255, 150))  # Semi-transparent white in the center
            // setColorAt(1, QColor(255, 255, 255, 0))  # Transparent at the edges
            var gradientStops = new GradientStops();
            // GradientStop constructor: (Color color, double offset) - Color first, then offset
            // Color constructor: Color(byte a, byte r, byte g, byte b) - ARGB format
            gradientStops.Add(new GradientStop(new Color(0, 255, 255, 255), 0.0)); // Transparent at the edges (A=0)
            gradientStops.Add(new GradientStop(new Color(150, 255, 255, 255), 0.5)); // Semi-transparent white in the center (A=150)
            gradientStops.Add(new GradientStop(new Color(0, 255, 255, 255), 1.0)); // Transparent at the edges (A=0)

            // Create the linear gradient brush with absolute coordinates
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new RelativePoint(new Point(lightLeft, 0), RelativeUnit.Absolute),
                EndPoint = new RelativePoint(new Point(lightRight, 0), RelativeUnit.Absolute),
                GradientStops = gradientStops
            };

            // Create the rectangle for the shimmering effect
            var lightRect = new Rect(lightLeft, lightTop, lightRight - lightLeft, lightBottom - lightTop);

            // Create a rounded rectangle geometry using StreamGeometry
            var geometry = new StreamGeometry();
            using (var ctx = geometry.Open())
            {
                // Draw rounded rectangle path matching PyKotor's drawRoundedRect
                double radius = chunkRadius;
                double x = lightRect.X;
                double y = lightRect.Y;
                double w = lightRect.Width;
                double h = lightRect.Height;

                // Ensure we have valid dimensions
                if (w <= 0 || h <= 0)
                {
                    return;
                }

                // Start from top-left corner (after rounding)
                ctx.BeginFigure(new Point(x + radius, y), true);

                // Top edge
                ctx.LineTo(new Point(x + w - radius, y));

                // Top-right corner arc
                ctx.ArcTo(new Point(x + w, y + radius), new Size(radius, radius), 0, false, SweepDirection.Clockwise);

                // Right edge
                ctx.LineTo(new Point(x + w, y + h - radius));

                // Bottom-right corner arc
                ctx.ArcTo(new Point(x + w - radius, y + h), new Size(radius, radius), 0, false, SweepDirection.Clockwise);

                // Bottom edge
                ctx.LineTo(new Point(x + radius, y + h));

                // Bottom-left corner arc
                ctx.ArcTo(new Point(x, y + h - radius), new Size(radius, radius), 0, false, SweepDirection.Clockwise);

                // Left edge
                ctx.LineTo(new Point(x, y + radius));

                // Top-left corner arc
                ctx.ArcTo(new Point(x + radius, y), new Size(radius, radius), 0, false, SweepDirection.Clockwise);

                ctx.EndFigure(true);
            }

            // Clip to the filled width area to ensure shimmer only appears in progress area
            // This matches PyKotor behavior where the shimmer is constrained to the filled portion
            var filledRect = new Rect(0, 0, filledWidth, height);
            using (context.PushClip(filledRect))
            {
                // Draw the shimmering effect with the gradient brush
                // In Avalonia, we use DrawGeometry with a brush to fill the geometry
                context.DrawGeometry(gradientBrush, null, geometry);
            }
        }
    }
}
