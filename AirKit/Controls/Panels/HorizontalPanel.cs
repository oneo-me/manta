using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace AirKit.Controls.Panels;

static class HorizontalPanel
{
    public static Size Measure(PanelExt panel, ReadOnlyCollection<Control> children, Size availableSize)
    {
        var spacing = panel.Spacing;

        var result = new Size();
        var layoutSize = availableSize.WithWidth(double.PositiveInfinity);

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

    public static void Arrange(PanelExt panel, ReadOnlyCollection<Control> children, Size finalSize)
    {
        var spacing = panel.Spacing;

        var finalRect = new Rect(finalSize);
        var current = 0d;

        foreach (var child in children)
        {
            finalRect = finalRect.WithX(finalRect.X + current);

            current = child.DesiredSize.Width;

            finalRect = finalRect.WithWidth(current);
            finalRect = finalRect.WithHeight(Math.Max(finalSize.Height, child.DesiredSize.Height));

            current += spacing;

            child.Arrange(finalRect);
        }
    }
}
