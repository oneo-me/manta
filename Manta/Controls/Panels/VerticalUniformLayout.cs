using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls.Panels;

public static class VerticalUniformLayout
{
    public static Size Measure(MtPanel panel, ReadOnlyCollection<Control> children, Size availableSize)
    {
        var spacing = panel.Spacing;
        var row = children.Count;
        var rowSize = Math.Max(0, (availableSize.Height - spacing * (row - 1)) / row);
        var layoutSize = new Size(availableSize.Width, rowSize);
        var result = new Size(double.IsInfinity(availableSize.Width) ? 0 : availableSize.Width, 0);

        foreach (var child in children)
        {
            child.Measure(layoutSize);

            var childDesiredSize = child.DesiredSize;

            result = result.WithHeight(result.Height + spacing + childDesiredSize.Height);
            result = result.WithWidth(Math.Max(result.Width, childDesiredSize.Width));
        }

        result = result.WithHeight(result.Height - spacing);

        return result;
    }

    public static void Arrange(MtPanel panel, ReadOnlyCollection<Control> children, Size finalSize)
    {
        var spacing = panel.Spacing;
        var row = children.Count;
        var rowSize = Math.Max(0, (finalSize.Height - spacing * (row - 1)) / row);
        var current = 0d;

        foreach (var child in children)
        {
            child.Arrange(new(0, current, finalSize.Width, rowSize));
            current += rowSize + spacing;
        }
    }
}
