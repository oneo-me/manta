using Avalonia;
using Avalonia.Media;
using Manta.Charts.Models;

namespace Manta.Charts.Renders;

/// <summary>
///     Renders line charts.
/// </summary>
static class LineChartRenderer
{
    const double PointPadding = 10; // Padding for circles and highlight rings

    public static void Render
    (
        DrawingContext context,
        ChartRenderContext ctx,
        Rect contentRect,
        BarChartData data,
        int hoverGroupIndex,
        ChartHitTestResult hitResult
    )
    {
        if (data.Items.Count == 0) return;

        var labelRect = new Rect(contentRect.Left, contentRect.Bottom - ChartRenderContext.LabelHeight, contentRect.Width, ChartRenderContext.LabelHeight);
        // Add top padding to prevent circles from being clipped
        var chartRect = new Rect(contentRect.Left, contentRect.Top + PointPadding, contentRect.Width, labelRect.Top - contentRect.Top - PointPadding);

        var seriesCount = data.Items.FirstOrDefault()?.Values.Count ?? 0;
        if (seriesCount == 0) return;

        var allValues = data.Items.SelectMany(i => i.Values.Select(v => v.Value)).ToList();
        if (allValues.Count == 0) return;
        var maxValue = allValues.Max();
        if (maxValue <= 0) maxValue = 1;

        DrawGrid(context, ctx, chartRect);
        RegisterHitRects(contentRect, chartRect, data, hitResult);
        DrawLines(context, chartRect, data, maxValue, seriesCount, hoverGroupIndex);
        DrawCategoryLabels(context, ctx, labelRect, data, hoverGroupIndex);
    }

    static void DrawGrid(DrawingContext context, ChartRenderContext ctx, Rect chartRect)
    {
        var foreColor = ctx.ForegroundColor;
        var gridPen = new Pen(new SolidColorBrush(Color.FromArgb(25, foreColor.R, foreColor.G, foreColor.B)));
        const int gridLines = 4;

        for (var i = 0; i <= gridLines; i++)
        {
            var y = chartRect.Top + chartRect.Height * i / gridLines;
            context.DrawLine(gridPen, new(chartRect.Left, y), new(chartRect.Right, y));
        }
    }

    static void RegisterHitRects(Rect contentRect, Rect chartRect, BarChartData data, ChartHitTestResult hitResult)
    {
        var pointCount = data.Items.Count;
        var groupWidth = chartRect.Width / pointCount;

        for (var pointIndex = 0; pointIndex < pointCount; pointIndex++)
        {
            var item = data.Items.ElementAt(pointIndex);
            var hitRect = new Rect(chartRect.Left + pointIndex * groupWidth, contentRect.Top, groupWidth, contentRect.Height);
            hitResult.HitRects.Add(new(hitRect, pointIndex, item.Category));
        }
    }

    static void DrawLines
    (
        DrawingContext context,
        Rect chartRect,
        BarChartData data,
        double maxValue,
        int seriesCount,
        int hoverGroupIndex
    )
    {
        var pointCount = data.Items.Count;
        var groupWidth = chartRect.Width / pointCount;

        for (var seriesIndex = 0; seriesIndex < seriesCount; seriesIndex++)
        {
            var color = ChartRenderContext.DefaultColors[seriesIndex % ChartRenderContext.DefaultColors.Length];
            var linePen = new Pen(new SolidColorBrush(color), 2);
            var pointBrush = new SolidColorBrush(color);

            // Draw line
            var geometry = new StreamGeometry();
            using (var geometryContext = geometry.Open())
            {
                var firstPoint = true;
                for (var pointIndex = 0; pointIndex < pointCount; pointIndex++)
                {
                    var item = data.Items.ElementAt(pointIndex);
                    if (seriesIndex >= item.Values.Count) continue;

                    var value = item.Values.ElementAt(seriesIndex);
                    var x = chartRect.Left + pointIndex * groupWidth + groupWidth / 2;
                    var y = chartRect.Bottom - value.Value / maxValue * chartRect.Height;

                    if (firstPoint)
                    {
                        geometryContext.BeginFigure(new(x, y), false);
                        firstPoint = false;
                    }
                    else
                    {
                        geometryContext.LineTo(new(x, y));
                    }
                }
            }

            context.DrawGeometry(null, linePen, geometry);

            // Draw points
            for (var pointIndex = 0; pointIndex < pointCount; pointIndex++)
            {
                var item = data.Items.ElementAt(pointIndex);
                if (seriesIndex >= item.Values.Count) continue;

                var value = item.Values.ElementAt(seriesIndex);
                var x = chartRect.Left + pointIndex * groupWidth + groupWidth / 2;
                var y = chartRect.Bottom - value.Value / maxValue * chartRect.Height;

                var isHovered = pointIndex == hoverGroupIndex;
                var pointRadius = isHovered ? 6d : 4d;

                var actualBrush = value.CustomColor.HasValue
                    ? new(value.CustomColor.Value)
                    : pointBrush;

                if (isHovered)
                {
                    // Draw highlight ring
                    var highlightPen = new Pen(new SolidColorBrush(Color.FromArgb(80, color.R, color.G, color.B)), 3);
                    context.DrawEllipse(null, highlightPen, new(x, y), pointRadius + 3, pointRadius + 3);
                }

                context.DrawEllipse(actualBrush, null, new(x, y), pointRadius, pointRadius);
            }
        }
    }

    static void DrawCategoryLabels
    (
        DrawingContext context,
        ChartRenderContext ctx,
        Rect labelRect,
        BarChartData data,
        int hoverGroupIndex
    )
    {
        var foreColor = ctx.ForegroundColor;
        var labelBrush = new SolidColorBrush(Color.FromArgb(160, foreColor.R, foreColor.G, foreColor.B));

        var pointCount = data.Items.Count;
        var groupWidth = labelRect.Width / pointCount;
        var maxLabelWidth = groupWidth - 4; // Leave small margin on each side

        for (var pointIndex = 0; pointIndex < pointCount; pointIndex++)
        {
            var item = data.Items.ElementAt(pointIndex);
            var groupX = labelRect.Left + pointIndex * groupWidth;
            var isHovered = pointIndex == hoverGroupIndex;
            var label = TruncateLabelToFit(ctx, item.Category, maxLabelWidth, isHovered ? ctx.Foreground! : labelBrush);
            var labelX = groupX + (groupWidth - label.Width) / 2;
            context.DrawText(label, new(labelX, labelRect.Top + (ChartRenderContext.LabelHeight - label.Height) / 2));
        }
    }

    static FormattedText TruncateLabelToFit(ChartRenderContext ctx, string text, double maxWidth, IBrush brush)
    {
        var label = ctx.CreateText(text, ChartRenderContext.LabelFontSize - 1, brush);
        if (label.Width <= maxWidth) return label;

        // Truncate with ellipsis
        for (var len = text.Length - 1; len > 0; len--)
        {
            var truncated = text[..len] + "…";
            label = ctx.CreateText(truncated, ChartRenderContext.LabelFontSize - 1, brush);
            if (label.Width <= maxWidth) return label;
        }

        return ctx.CreateText("…", ChartRenderContext.LabelFontSize - 1, brush);
    }
}
