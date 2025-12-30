using System.Globalization;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Manta.Charts.Models;
using Manta.Themes.Values;

namespace Manta.Charts.Controls;

public class MChart : TemplatedControl
{
    public static readonly StyledProperty<ChartDataBase?> DataProperty =
        AvaloniaProperty.Register<MChart, ChartDataBase?>(nameof(Data));

    public ChartDataBase? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    static MChart()
    {
        AffectsRender<MChart>(DataProperty);
    }

    static readonly Color[] _defaultColors =
    [
        new OklchColor { L = 0.7, C = 0.2 * 0.8, H = 134 }.ToColor(), // Green
        new OklchColor { L = 0.7, C = 0.25 * 0.8, H = 260 }.ToColor(), // Blue
        new OklchColor { L = 0.8, C = 0.3 * 0.8, H = 340 }.ToColor(), // Pink
        new OklchColor { L = 0.8, C = 0.3 * 0.8, H = 80 }.ToColor(), // Orange
        new OklchColor { L = 0.65, C = 0.25 * 0.8, H = 320 }.ToColor(), // Purple
        new OklchColor { L = 0.75, C = 0.25 * 0.8, H = 190 }.ToColor(), // Teal
        new OklchColor { L = 0.65, C = 0.3 * 0.8, H = 25 }.ToColor(), // Red
        new OklchColor { L = 0.9, C = 0.3 * 0.8, H = 90 }.ToColor() // Yellow
    ];

    const double TitleFontSize = 14d;
    const double LabelFontSize = 12d;
    const double TitleHeight = 24d;
    const double LabelHeight = 18d;
    const double LegendHeight = 22d;
    const double LegendItemSpacing = 12d;
    const double LegendRectWidth = 12d;
    const double LegendRectHeight = 8d;

    // Hover state
    Point? _hoverPoint;
    int _hoverGroupIndex = -1;
    readonly List<HitRect> _hitRects = [];
    PieHitInfo? _pieHitInfo;

    record struct HitRect(Rect Bounds, int GroupIndex, string Category);

    record struct PieHitInfo(Point Center, double Radius, double[] Angles, string[] Names, double[] Values, Color[] Colors);

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        var pos = e.GetPosition(this);
        var oldPoint = _hoverPoint;
        _hoverPoint = pos;

        var oldGroup = _hoverGroupIndex;
        _hoverGroupIndex = -1;

        // Check pie hit first
        if (_pieHitInfo.HasValue)
        {
            var pie = _pieHitInfo.Value;
            var dx = pos.X - pie.Center.X;
            var dy = pos.Y - pie.Center.Y;
            var dist = Math.Sqrt(dx * dx + dy * dy);

            if (dist <= pie.Radius)
            {
                var angle = Math.Atan2(dy, dx) * 180 / Math.PI;
                if (angle < -90) angle += 360;

                for (var i = 0; i < pie.Angles.Length - 1; i++)
                    if (angle >= pie.Angles[i] && angle < pie.Angles[i + 1])
                    {
                        _hoverGroupIndex = i;
                        break;
                    }
            }
        }
        else
        {
            // Check rect hits
            foreach (var rect in _hitRects)
                if (rect.Bounds.Contains(pos))
                {
                    _hoverGroupIndex = rect.GroupIndex;
                    break;
                }
        }

        // Always redraw when hovering to update tooltip position
        if (_hoverGroupIndex >= 0 || oldGroup != _hoverGroupIndex)
            InvalidateVisual();
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        _hoverPoint = null;
        if (_hoverGroupIndex != -1)
        {
            _hoverGroupIndex = -1;
            InvalidateVisual();
        }
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var data = Data;
        if (data is null) return;

        _hitRects.Clear();
        _pieHitInfo = null;

        var padding = new Thickness(12);
        var contentRect = new Rect(
            padding.Left,
            padding.Top,
            Bounds.Width - padding.Left - padding.Right,
            Bounds.Height - padding.Top - padding.Bottom
        );

        if (contentRect.Width <= 0 || contentRect.Height <= 0) return;

        switch (data)
        {
            case BarChartData barData when barData.Type == BarChartType.Bar:
                RenderBarChart(context, contentRect, barData);
                break;

            case BarChartData lineData when lineData.Type == BarChartType.Line:
                RenderLineChart(context, contentRect, lineData);
                break;

            case PieChartData pieData:
                RenderPieChart(context, contentRect, pieData);
                break;
        }

        // Draw tooltip
        if (_hoverGroupIndex >= 0 && _hoverPoint.HasValue)
            DrawTooltip(context, data);
    }

    FormattedText CreateText(string text, double fontSize, IBrush? brush, TextAlignment alignment = TextAlignment.Left)
    {
        return new(
            text,
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            new(FontFamily),
            fontSize,
            brush ?? Brushes.White
        )
        {
            TextAlignment = alignment
        };
    }

    void DrawTitle(DrawingContext context, Rect rect, string title)
    {
        if (string.IsNullOrEmpty(title)) return;

        var text = CreateText(title, TitleFontSize, Foreground);
        var x = rect.Left + (rect.Width - text.Width) / 2;
        var y = rect.Top + (TitleHeight - text.Height) / 2;
        context.DrawText(text, new(x, y));
    }

    void DrawLegend(DrawingContext context, Rect legendRect, IReadOnlyList<(string name, Color color)> items)
    {
        if (items.Count == 0) return;

        // Calculate total width
        var totalWidth = 0d;
        foreach (var item in items)
        {
            var text = CreateText(item.name, LabelFontSize, Foreground);
            totalWidth += LegendRectWidth + 6 + text.Width + LegendItemSpacing;
        }

        totalWidth -= LegendItemSpacing;

        var x = legendRect.Left + (legendRect.Width - totalWidth) / 2;
        var y = legendRect.Top + (legendRect.Height - LegendRectHeight) / 2;

        foreach (var (name, color) in items)
        {
            // Draw rounded rectangle
            var rectBrush = new SolidColorBrush(color);
            var rect = new Rect(x, y, LegendRectWidth, LegendRectHeight);
            context.DrawRectangle(rectBrush, null, rect, 2, 2);

            // Draw text
            x += LegendRectWidth + 6;
            var text = CreateText(name, LabelFontSize, Foreground);
            context.DrawText(text, new(x, y + (LegendRectHeight - text.Height) / 2));
            x += text.Width + LegendItemSpacing;
        }
    }

    void DrawVerticalLegend(DrawingContext context, Rect legendRect, IReadOnlyList<(string name, Color color)> items)
    {
        if (items.Count == 0) return;

        var lineHeight = 18d;
        var totalHeight = items.Count * lineHeight;
        var startY = legendRect.Top + (legendRect.Height - totalHeight) / 2;

        foreach (var (name, color) in items)
        {
            var rectBrush = new SolidColorBrush(color);
            var rect = new Rect(legendRect.Left, startY + (lineHeight - LegendRectHeight) / 2, LegendRectWidth, LegendRectHeight);
            context.DrawRectangle(rectBrush, null, rect, 2, 2);

            var text = CreateText(name, LabelFontSize, Foreground);
            context.DrawText(text, new(legendRect.Left + LegendRectWidth + 6, startY + (lineHeight - text.Height) / 2));
            startY += lineHeight;
        }
    }

    void DrawTooltip(DrawingContext context, ChartDataBase data)
    {
        if (!_hoverPoint.HasValue || _hoverGroupIndex < 0) return;

        var lines = new List<(string text, Color? color, bool isTitle)>();

        switch (data)
        {
            case BarChartData barData:
                if (_hoverGroupIndex < barData.Items.Count)
                {
                    var item = barData.Items.ElementAt(_hoverGroupIndex);
                    lines.Add((item.Category, null, true));
                    var idx = 0;
                    foreach (var v in item.Values)
                    {
                        var color = v.CustomColor ?? _defaultColors[idx % _defaultColors.Length];
                        lines.Add(($"{v.Name}: {v.Value:N0}", color, false));
                        idx++;
                    }
                }

                break;

            case PieChartData pieData:
                if (_hoverGroupIndex < pieData.Values.Count)
                {
                    var v = pieData.Values.ElementAt(_hoverGroupIndex);
                    var total = pieData.Values.Sum(x => x.Value);
                    var percent = total > 0 ? v.Value / total * 100 : 0;
                    var color = v.CustomColor ?? _defaultColors[_hoverGroupIndex % _defaultColors.Length];
                    lines.Add((v.Name, null, true));
                    lines.Add(($"{v.Value:N0} ({percent:F1}%)", color, false));
                }

                break;
        }

        if (lines.Count == 0) return;

        // Calculate tooltip size
        var maxWidth = 0d;
        var totalHeight = 0d;
        var lineHeight = LabelFontSize + 6;
        var titleHeight = TitleFontSize + 4;

        foreach (var (text, _, isTitle) in lines)
        {
            var fontSize = isTitle ? LabelFontSize + 1 : LabelFontSize;
            var ft = CreateText(text, fontSize, Brushes.Black);
            maxWidth = Math.Max(maxWidth, ft.Width + (isTitle ? 0 : LegendRectWidth + 6));
            totalHeight += isTitle ? titleHeight : lineHeight;
        }

        var tooltipPadding = 10d;
        var tooltipWidth = maxWidth + tooltipPadding * 2;
        var tooltipHeight = totalHeight + tooltipPadding * 2;

        // Position tooltip - follow mouse
        var tooltipX = _hoverPoint.Value.X + 16;
        var tooltipY = _hoverPoint.Value.Y + 16;

        // Keep tooltip within bounds
        if (tooltipX + tooltipWidth > Bounds.Width - 8)
            tooltipX = _hoverPoint.Value.X - tooltipWidth - 8;
        if (tooltipY + tooltipHeight > Bounds.Height - 8)
            tooltipY = _hoverPoint.Value.Y - tooltipHeight - 8;
        if (tooltipX < 8)
            tooltipX = 8;
        if (tooltipY < 8)
            tooltipY = 8;

        var tooltipRect = new Rect(tooltipX, tooltipY, tooltipWidth, tooltipHeight);

        // Draw shadow
        var shadowOffset = 3d;
        var shadowRect = new Rect(tooltipX + shadowOffset, tooltipY + shadowOffset, tooltipWidth, tooltipHeight);
        var shadowBrush = new SolidColorBrush(Color.FromArgb(40, 0, 0, 0));
        context.DrawRectangle(shadowBrush, null, shadowRect, 8, 8);

        // Draw tooltip background (white)
        var bgBrush = new SolidColorBrush(Colors.White);
        var borderPen = new Pen(new SolidColorBrush(Color.FromArgb(30, 0, 0, 0)));
        context.DrawRectangle(bgBrush, borderPen, tooltipRect, 6, 6);

        // Draw tooltip content
        var textX = tooltipX + tooltipPadding;
        var textY = tooltipY + tooltipPadding;

        foreach (var (text, color, isTitle) in lines)
        {
            var x = textX;
            var fontSize = isTitle ? LabelFontSize + 1 : LabelFontSize;
            var textBrush = isTitle
                ? new(Color.FromRgb(60, 60, 60))
                : new SolidColorBrush(Color.FromRgb(100, 100, 100));

            if (color.HasValue && !isTitle)
            {
                var rectBrush = new SolidColorBrush(color.Value);
                context.DrawRectangle(rectBrush, null, new(x, textY + 3, LegendRectWidth, LegendRectHeight), 2, 2);
                x += LegendRectWidth + 6;
            }

            var ft = CreateText(text, fontSize, textBrush);
            context.DrawText(ft, new(x, textY));
            textY += isTitle ? titleHeight : lineHeight;
        }
    }

    void RenderBarChart(DrawingContext context, Rect contentRect, BarChartData data)
    {
        if (data.Items.Count == 0) return;

        var titleRect = new Rect(contentRect.Left, contentRect.Top, contentRect.Width, TitleHeight);
        var legendRect = new Rect(contentRect.Left, contentRect.Bottom - LegendHeight, contentRect.Width, LegendHeight);
        var labelRect = new Rect(contentRect.Left, legendRect.Top - LabelHeight, contentRect.Width, LabelHeight);
        var chartRect = new Rect(contentRect.Left, titleRect.Bottom, contentRect.Width, labelRect.Top - titleRect.Bottom);

        DrawTitle(context, titleRect, data.Title);

        var seriesNames = data.Items.FirstOrDefault()?.Values.Select(v => v.Name).ToList() ?? [];
        var seriesCount = seriesNames.Count;
        if (seriesCount == 0) return;

        var allValues = data.Items.SelectMany(i => i.Values.Select(v => v.Value)).ToList();
        if (allValues.Count == 0) return;
        var maxValue = allValues.Max();
        if (maxValue <= 0) maxValue = 1;

        // Draw grid
        var foreColor = (Foreground as ISolidColorBrush)?.Color ?? Colors.White;
        var gridPen = new Pen(new SolidColorBrush(Color.FromArgb(25, foreColor.R, foreColor.G, foreColor.B)));
        const int gridLines = 4;
        for (var i = 0; i <= gridLines; i++)
        {
            var y = chartRect.Top + chartRect.Height * i / gridLines;
            context.DrawLine(gridPen, new(chartRect.Left, y), new(chartRect.Right, y));
        }

        // Calculate dimensions
        var groupCount = data.Items.Count;
        var groupWidth = chartRect.Width / groupCount;
        var barSpacing = 2d;
        var groupPadding = groupWidth * 0.12;
        var barWidth = (groupWidth - groupPadding * 2 - (seriesCount - 1) * barSpacing) / seriesCount;

        var labelBrush = new SolidColorBrush(Color.FromArgb(160, foreColor.R, foreColor.G, foreColor.B));

        for (var groupIndex = 0; groupIndex < groupCount; groupIndex++)
        {
            var item = data.Items.ElementAt(groupIndex);
            var groupX = chartRect.Left + groupIndex * groupWidth;
            var isHoveredGroup = groupIndex == _hoverGroupIndex;

            for (var seriesIndex = 0; seriesIndex < item.Values.Count; seriesIndex++)
            {
                var value = item.Values.ElementAt(seriesIndex);
                var barHeight = value.Value / maxValue * chartRect.Height;
                var barX = groupX + groupPadding + seriesIndex * (barWidth + barSpacing);
                var barY = chartRect.Bottom - barHeight;

                var color = value.CustomColor ?? _defaultColors[seriesIndex % _defaultColors.Length];

                // Hover effect
                if (isHoveredGroup)
                    color = Color.FromArgb(255,
                        (byte)Math.Min(255, color.R + 30),
                        (byte)Math.Min(255, color.G + 30),
                        (byte)Math.Min(255, color.B + 30));

                var barBrush = new SolidColorBrush(color);
                var rect = new Rect(barX, barY, barWidth, barHeight);
                context.DrawRectangle(barBrush, null, rect, 3, 3);

                // Register hit rect for the whole group (including label area, no gaps)
                if (seriesIndex == 0)
                {
                    var hitRect = new Rect(groupX, titleRect.Bottom, groupWidth, legendRect.Top - titleRect.Bottom);
                    _hitRects.Add(new(hitRect, groupIndex, item.Category));
                }
            }

            // Category label
            var label = item.Category.Length > 3 ? item.Category[..3] : item.Category;
            var labelText = CreateText(label, LabelFontSize - 1, isHoveredGroup ? Foreground! : labelBrush);
            var labelX = groupX + (groupWidth - labelText.Width) / 2;
            context.DrawText(labelText, new(labelX, labelRect.Top + (LabelHeight - labelText.Height) / 2));
        }

        if (data.ShowLegend)
        {
            var legendItems = seriesNames.Select((name, i) => (name, _defaultColors[i % _defaultColors.Length])).ToList();
            DrawLegend(context, legendRect, legendItems);
        }
    }

    void RenderLineChart(DrawingContext context, Rect contentRect, BarChartData data)
    {
        if (data.Items.Count == 0) return;

        var titleRect = new Rect(contentRect.Left, contentRect.Top, contentRect.Width, TitleHeight);
        var legendRect = new Rect(contentRect.Left, contentRect.Bottom - LegendHeight, contentRect.Width, LegendHeight);
        var labelRect = new Rect(contentRect.Left, legendRect.Top - LabelHeight, contentRect.Width, LabelHeight);
        var chartRect = new Rect(contentRect.Left, titleRect.Bottom, contentRect.Width, labelRect.Top - titleRect.Bottom);

        DrawTitle(context, titleRect, data.Title);

        var seriesNames = data.Items.FirstOrDefault()?.Values.Select(v => v.Name).ToList() ?? [];
        var seriesCount = seriesNames.Count;
        if (seriesCount == 0) return;

        var allValues = data.Items.SelectMany(i => i.Values.Select(v => v.Value)).ToList();
        if (allValues.Count == 0) return;
        var maxValue = allValues.Max();
        if (maxValue <= 0) maxValue = 1;

        // Draw grid
        var foreColor = (Foreground as ISolidColorBrush)?.Color ?? Colors.White;
        var gridPen = new Pen(new SolidColorBrush(Color.FromArgb(25, foreColor.R, foreColor.G, foreColor.B)));
        const int gridLines = 4;
        for (var i = 0; i <= gridLines; i++)
        {
            var y = chartRect.Top + chartRect.Height * i / gridLines;
            context.DrawLine(gridPen, new(chartRect.Left, y), new(chartRect.Right, y));
        }

        var pointCount = data.Items.Count;
        var groupWidth = chartRect.Width / pointCount;
        var labelBrush = new SolidColorBrush(Color.FromArgb(160, foreColor.R, foreColor.G, foreColor.B));

        // Register hit rects (including label area, no gaps)
        for (var pointIndex = 0; pointIndex < pointCount; pointIndex++)
        {
            var item = data.Items.ElementAt(pointIndex);
            var hitRect = new Rect(chartRect.Left + pointIndex * groupWidth, titleRect.Bottom, groupWidth, legendRect.Top - titleRect.Bottom);
            _hitRects.Add(new(hitRect, pointIndex, item.Category));
        }

        // Draw lines
        for (var seriesIndex = 0; seriesIndex < seriesCount; seriesIndex++)
        {
            var color = _defaultColors[seriesIndex % _defaultColors.Length];
            var linePen = new Pen(new SolidColorBrush(color), 2);
            var pointBrush = new SolidColorBrush(color);

            var geometry = new StreamGeometry();
            using (var ctx = geometry.Open())
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
                        ctx.BeginFigure(new(x, y), false);
                        firstPoint = false;
                    }
                    else
                    {
                        ctx.LineTo(new(x, y));
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

                var isHovered = pointIndex == _hoverGroupIndex;
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

        // Category labels
        for (var pointIndex = 0; pointIndex < pointCount; pointIndex++)
        {
            var item = data.Items.ElementAt(pointIndex);
            var x = chartRect.Left + pointIndex * groupWidth + groupWidth / 2;
            var isHovered = pointIndex == _hoverGroupIndex;
            var label = item.Category.Length > 3 ? item.Category[..3] : item.Category;
            var labelText = CreateText(label, LabelFontSize - 1, isHovered ? Foreground! : labelBrush);
            context.DrawText(labelText, new(x - labelText.Width / 2, labelRect.Top + (LabelHeight - labelText.Height) / 2));
        }

        if (data.ShowLegend)
        {
            var legendItems = seriesNames.Select((name, i) => (name, _defaultColors[i % _defaultColors.Length])).ToList();
            DrawLegend(context, legendRect, legendItems);
        }
    }

    void RenderPieChart(DrawingContext context, Rect contentRect, PieChartData data)
    {
        if (data.Values.Count == 0) return;

        var titleRect = new Rect(contentRect.Left, contentRect.Top, contentRect.Width, TitleHeight);
        DrawTitle(context, titleRect, data.Title);

        var bodyRect = new Rect(contentRect.Left, titleRect.Bottom, contentRect.Width, contentRect.Height - TitleHeight);
        var legendWidth = 80d;
        var pieAreaRect = new Rect(bodyRect.Left, bodyRect.Top, bodyRect.Width - legendWidth, bodyRect.Height);
        var legendRect = new Rect(bodyRect.Right - legendWidth, bodyRect.Top, legendWidth, bodyRect.Height);

        var pieSize = Math.Min(pieAreaRect.Width, pieAreaRect.Height) - 8;
        var centerX = pieAreaRect.Left + pieAreaRect.Width / 2;
        var centerY = pieAreaRect.Top + pieAreaRect.Height / 2;
        var radius = pieSize / 2;

        var total = data.Values.Sum(v => v.Value);
        if (total <= 0) return;

        var startAngle = -90d;
        var legendItems = new List<(string name, Color color)>();

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
            var color = value.CustomColor ?? _defaultColors[i % _defaultColors.Length];

            angles[i + 1] = angles[i] + sweepAngle;
            names[i] = value.Name;
            values[i] = value.Value;
            colors[i] = color;

            legendItems.Add((value.Name, color));

            var isHovered = i == _hoverGroupIndex;
            var drawRadius = isHovered ? radius + 4 : radius;
            var drawCenterX = centerX;
            var drawCenterY = centerY;

            // Explode effect on hover
            if (isHovered)
            {
                var midAngle = (startAngle + sweepAngle / 2) * Math.PI / 180;
                drawCenterX += 4 * Math.Cos(midAngle);
                drawCenterY += 4 * Math.Sin(midAngle);
            }

            var geometry = new StreamGeometry();
            using (var ctx = geometry.Open())
            {
                ctx.BeginFigure(new(drawCenterX, drawCenterY), true);

                var startRad = startAngle * Math.PI / 180;
                var endRad = (startAngle + sweepAngle) * Math.PI / 180;

                var startPoint = new Point(
                    drawCenterX + drawRadius * Math.Cos(startRad),
                    drawCenterY + drawRadius * Math.Sin(startRad)
                );
                var endPoint = new Point(
                    drawCenterX + drawRadius * Math.Cos(endRad),
                    drawCenterY + drawRadius * Math.Sin(endRad)
                );

                ctx.LineTo(startPoint);
                ctx.ArcTo(
                    endPoint,
                    new(drawRadius, drawRadius),
                    0,
                    sweepAngle > 180,
                    SweepDirection.Clockwise
                );
                ctx.LineTo(new(drawCenterX, drawCenterY));
            }

            var brush = new SolidColorBrush(color);
            context.DrawGeometry(brush, null, geometry);

            startAngle += sweepAngle;
        }

        // Store pie hit info for angle-based detection
        _pieHitInfo = new PieHitInfo(new(centerX, centerY), radius, angles, names, values, colors);

        if (data.ShowLegend)
            DrawVerticalLegend(context, legendRect, legendItems);
    }
}
