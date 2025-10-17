using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls.Panels;

static class WrapLayout
{
    public static Size Measure(LightPanel panel, ReadOnlyCollection<Control> children, Size availableSize)
    {
        var spacing = panel.Spacing;
        var result = new Size();
        var layoutSize = new Size(availableSize.Width, double.PositiveInfinity);

        var left = 0d;
        var top = 0d;
        var lineHeight = 0d;

        foreach (var child in children)
        {
            child.Measure(layoutSize);

            var childDesiredSize = child.DesiredSize;
            if (left + childDesiredSize.Width > availableSize.Width)
            {
                left = 0;
                top += lineHeight + spacing;
                lineHeight = childDesiredSize.Height;
                result = result.WithHeight(top + lineHeight);
            }
            else
            {
                lineHeight = Math.Max(lineHeight, childDesiredSize.Height);
                result = result.WithHeight(Math.Max(result.Height, lineHeight));
            }

            result = result.WithWidth(Math.Max(result.Width, left + childDesiredSize.Width));

            left += childDesiredSize.Width + spacing;
        }

        return result;
    }

    public static void Arrange(LightPanel panel, ReadOnlyCollection<Control> children, Size finalSize)
    {
        var spacing = panel.Spacing;
        var width = finalSize.Width;

        var left = 0d;
        var top = 0d;
        var lineHeight = 0d;

        foreach (var child in children)
        {
            var childDesiredSize = child.DesiredSize;
            if (left + childDesiredSize.Width > width)
            {
                left = 0;
                top += lineHeight + spacing;
                lineHeight = childDesiredSize.Height;
            }
            else
            {
                lineHeight = Math.Max(lineHeight, childDesiredSize.Height);
            }

            child.Arrange(new(left, top, Math.Min(width, childDesiredSize.Width), childDesiredSize.Height));

            left += childDesiredSize.Width + spacing;
        }
    }

    public static void ArrangeFull(LightPanel panel, ReadOnlyCollection<Control> children, Size finalSize)
    {
        var spacing = panel.Spacing;
        var width = finalSize.Width;

        var left = 0d;
        var top = 0d;
        var lineHeight = 0d;

        var items = new List<(Control child, double top, double width, double height)>();

        foreach (var child in children)
        {
            var childDesiredSize = child.DesiredSize;
            if (left + childDesiredSize.Width > width)
            {
                left = 0;
                top += lineHeight + spacing;
                lineHeight = childDesiredSize.Height;

                FullItems(width, spacing, items);
            }
            else
            {
                lineHeight = Math.Max(lineHeight, childDesiredSize.Height);
            }

            items.Add((child, top, childDesiredSize.Width, childDesiredSize.Height));

            left += childDesiredSize.Width + spacing;
        }

        FullItems(width, spacing, items);
    }

    static void FullItems(double finalWidth, double spacing, List<(Control child, double top, double width, double height)> items)
    {
        if (items.Count <= 0)
            return;

        var flag = (finalWidth - items.Sum(x => x.width) - spacing * (items.Count - 1)) / items.Count;
        var left = 0d;

        foreach (var (child, top, width, height) in items)
        {
            child.Arrange(new(left, top, width + flag, height));
            left += width + flag + spacing;
        }

        items.Clear();
    }
}
