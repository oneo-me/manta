using Avalonia;
using Avalonia.Media;
using Manta.Charts.Models;

namespace Manta.Charts.Renders;

/// <summary>
///     Renders pie charts.
/// </summary>
static class PieChartRenderer
{
    const double HoverExpand = 4; // Radius expansion on hover
    const double HoverOffset = 4; // Center offset on hover (explode effect)
    const double PiePadding = HoverExpand + HoverOffset + 4; // Extra safety margin

    public static void Render
    (
        DrawingContext context,
        ChartRenderContext ctx,
        Rect contentRect,
        PieChartData data,
        int hoverGroupIndex,
        ChartHitTestResult hitResult
    )
    {
        if (data.Values.Count == 0) return;

        var pieSize = Math.Min(contentRect.Width, contentRect.Height) - PiePadding * 2;
        var centerX = contentRect.Left + contentRect.Width / 2;
        var centerY = contentRect.Top + contentRect.Height / 2;
        var radius = pieSize / 2;

        var total = data.Values.Sum(v => v.Value);
        if (total <= 0) return;

        DrawSlices(context, data, centerX, centerY, radius, total, hoverGroupIndex, hitResult);
    }

    static void DrawSlices
    (
        DrawingContext context,
        PieChartData data,
        double centerX,
        double centerY,
        double radius,
        double total,
        int hoverGroupIndex,
        ChartHitTestResult hitResult
    )
    {
        var startAngle = -90d;

        // Prepare pie hit info
        var angles = new double[data.Values.Count + 1];
        var names = new string[data.Values.Count];
        var values = new double[data.Values.Count];
        var colors = new Color[data.Values.Count];
        angles[0] = -90;

        for (var i = 0; i < data.Values.Count; i++)
        {
            var value = data.Values.ElementAt(i);
            var sweepAngle = value.Value / total * 360;
            var color = ChartRenderContext.GetColor(i, value.CustomColor);

            angles[i + 1] = angles[i] + sweepAngle;
            names[i] = value.Name;
            values[i] = value.Value;
            colors[i] = color;

            var isHovered = i == hoverGroupIndex;
            var drawRadius = isHovered ? radius + HoverExpand : radius;
            var drawCenterX = centerX;
            var drawCenterY = centerY;

            // Explode effect on hover
            if (isHovered)
            {
                var midAngle = (startAngle + sweepAngle / 2) * Math.PI / 180;
                drawCenterX += HoverOffset * Math.Cos(midAngle);
                drawCenterY += HoverOffset * Math.Sin(midAngle);
            }

            DrawSlice(context, drawCenterX, drawCenterY, drawRadius, startAngle, sweepAngle, color);
            startAngle += sweepAngle;
        }

        // Store pie hit info for angle-based detection
        hitResult.PieHitInfo = new PieHitInfo(new(centerX, centerY), radius, angles, names, values, colors);
    }

    static void DrawSlice
    (
        DrawingContext context,
        double centerX,
        double centerY,
        double radius,
        double startAngle,
        double sweepAngle,
        Color color
    )
    {
        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(new(centerX, centerY), true);

            var startRad = startAngle * Math.PI / 180;
            var endRad = (startAngle + sweepAngle) * Math.PI / 180;

            var startPoint = new Point(
                centerX + radius * Math.Cos(startRad),
                centerY + radius * Math.Sin(startRad)
            );
            var endPoint = new Point(
                centerX + radius * Math.Cos(endRad),
                centerY + radius * Math.Sin(endRad)
            );

            ctx.LineTo(startPoint);
            ctx.ArcTo(
                endPoint,
                new(radius, radius),
                0,
                sweepAngle > 180,
                SweepDirection.Clockwise
            );
            ctx.LineTo(new(centerX, centerY));
        }

        var brush = new SolidColorBrush(color);
        context.DrawGeometry(brush, null, geometry);
    }
}
