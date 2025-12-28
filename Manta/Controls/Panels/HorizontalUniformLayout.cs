using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls.Panels;

public static class HorizontalUniformLayout
{
    public static Size Measure(MPanel panel, ReadOnlyCollection<Control> children, Size availableSize)
    {
        var spacing = panel.Spacing;
        var column = children.Count;
        var columnSize = Math.Max(0, (availableSize.Width - spacing * (column - 1)) / column);
        var layoutSize = new Size(columnSize, availableSize.Height);
        var result = new Size(0, double.IsInfinity(availableSize.Height) ? 0 : availableSize.Height);

        foreach (var child in children)
        {
            child.Measure(layoutSize);

            var childDesiredSize = child.DesiredSize;

            result = result.WithWidth(result.Width + spacing + childDesiredSize.Width);
            result = result.WithHeight(Math.Max(result.Height, childDesiredSize.Height));
        }

        result = result.WithWidth(result.Width - spacing);

        return result;
    }

    public static void Arrange(MPanel panel, ReadOnlyCollection<Control> children, Size finalSize)
    {
        var spacing = panel.Spacing;
        var column = children.Count;
        var columnSize = Math.Max(0, (finalSize.Width - spacing * (column - 1)) / column);
        var current = 0d;

        foreach (var child in children)
        {
            child.Arrange(new(current, 0, columnSize, finalSize.Height));
            current += columnSize + spacing;
        }
    }
}
