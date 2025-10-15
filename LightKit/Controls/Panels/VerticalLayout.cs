using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace LightKit.Controls.Panels;

static class VerticalLayout
{
    public static Size Measure(PanelExt panel, ReadOnlyCollection<Control> children, Size availableSize)
    {
        var spacing = panel.Spacing;

        var result = new Size();
        var layoutSize = availableSize.WithHeight(double.PositiveInfinity);

        foreach (var child in children)
        {
            child.Measure(layoutSize);

            var childDesiredSize = child.DesiredSize;

            result = result.WithWidth(Math.Max(result.Width, childDesiredSize.Width));
            result = result.WithHeight(result.Height + spacing + childDesiredSize.Height);
        }

        result = result.WithHeight(result.Height - spacing);

        return result;
    }

    public static void Arrange(PanelExt panel, ReadOnlyCollection<Control> children, Size finalSize)
    {
        var spacing = panel.Spacing;

        var finalRect = new Rect(finalSize);
        var current = 0d;

        foreach (var child in children)
        {
            finalRect = finalRect.WithY(finalRect.Y + current);

            current = child.DesiredSize.Height;

            finalRect = finalRect.WithWidth(Math.Max(finalSize.Width, child.DesiredSize.Width));
            finalRect = finalRect.WithHeight(current);

            current += spacing;

            child.Arrange(finalRect);
        }
    }
}
