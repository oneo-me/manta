using Avalonia;
using Avalonia.Media;
using Manta.Charts.Models;

namespace Manta.Charts.Renders;

/// <summary>
///     Renders bar charts.
/// </summary>
static class BarChartRenderer
{
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
        var chartRect = new Rect(contentRect.Left, contentRect.Top, contentRect.Width, labelRect.Top - contentRect.Top);

        var seriesCount = data.Items.FirstOrDefault()?.Values.Count ?? 0;
        if (seriesCount == 0) return;

        var allValues = data.Items.SelectMany(i => i.Values.Select(v => v.Value)).ToList();
        if (allValues.Count == 0) return;
        var maxValue = allValues.Max();
        if (maxValue <= 0) maxValue = 1;

        DrawGrid(context, ctx, chartRect);
        DrawBars(context, chartRect, contentRect, data, maxValue, seriesCount, hoverGroupIndex, hitResult);
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

    static void DrawBars
    (
        DrawingContext context,
        Rect chartRect,
        Rect contentRect,
        BarChartData data,
        double maxValue,
        int seriesCount,
        int hoverGroupIndex,
        ChartHitTestResult hitResult
    )
    {
        var groupCount = data.Items.Count;
        var groupWidth = chartRect.Width / groupCount;
        var barSpacing = 2d;
        var groupPadding = groupWidth * 0.12;
        var barWidth = (groupWidth - groupPadding * 2 - (seriesCount - 1) * barSpacing) / seriesCount;

        for (var groupIndex = 0; groupIndex < groupCount; groupIndex++)
        {
            var item = data.Items.ElementAt(groupIndex);
            var groupX = chartRect.Left + groupIndex * groupWidth;
            var isHoveredGroup = groupIndex == hoverGroupIndex;

            for (var seriesIndex = 0; seriesIndex < item.Values.Count; seriesIndex++)
            {
                var value = item.Values.ElementAt(seriesIndex);
                var barHeight = value.Value / maxValue * chartRect.Height;
                var barX = groupX + groupPadding + seriesIndex * (barWidth + barSpacing);
                var barY = chartRect.Bottom - barHeight;

                var color = ChartRenderContext.GetColor(seriesIndex, value.CustomColor);

                if (isHoveredGroup)
                    color = ChartRenderContext.BrightenColor(color);

                var barBrush = new SolidColorBrush(color);
                var rect = new Rect(barX, barY, barWidth, barHeight);
                context.DrawRectangle(barBrush, null, rect, 3, 3);

                // Register hit rect for the whole group
                if (seriesIndex == 0)
                {
                    var hitRect = new Rect(groupX, contentRect.Top, groupWidth, contentRect.Height);
                    hitResult.HitRects.Add(new(hitRect, groupIndex, item.Category));
                }
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

        var groupCount = data.Items.Count;
        var groupWidth = labelRect.Width / groupCount;
        var maxLabelWidth = groupWidth - 4; // Leave small margin on each side

        for (var groupIndex = 0; groupIndex < groupCount; groupIndex++)
        {
            var item = data.Items.ElementAt(groupIndex);
            var groupX = labelRect.Left + groupIndex * groupWidth;
            var isHoveredGroup = groupIndex == hoverGroupIndex;

            var label = TruncateLabelToFit(ctx, item.Category, maxLabelWidth, isHoveredGroup ? ctx.Foreground! : labelBrush);
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
