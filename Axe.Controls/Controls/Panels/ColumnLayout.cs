using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace Axe.Controls.Controls.Panels;

static class ColumnLayout
{
    public static Size Measure(PanelExt panel, ReadOnlyCollection<Control> children, Size availableSize)
    {
        var spacing = panel.Spacing;
        var column = (int)Math.Max(1, Math.Round(panel.Column > 0 ? panel.Column : (availableSize.Width + spacing) / ((panel.ColumnSize > 0 ? panel.ColumnSize : 120d) + spacing)));
        var result = new Size(availableSize.Width, 0);
        var columnSize = Math.Max(0, (result.Width - spacing * (column - 1)) / column);
        var layoutSize = new Size(columnSize, double.PositiveInfinity);

        var maxHeight = 0d;

        PanelExtBase.ForeachItems(children, column, (_, items) =>
        {
            var height = 0d;

            foreach (var item in items)
            {
                item.Measure(layoutSize);
                height = Math.Max(height, item.DesiredSize.Height);
            }

            maxHeight = maxHeight + height + spacing;
        });

        result = result.WithHeight(maxHeight - spacing);

        return result;
    }

    public static void Arrange(PanelExt panel, ReadOnlyCollection<Control> children, Size finalSize)
    {
        var spacing = panel.Spacing;
        var column = (int)Math.Max(1, Math.Round(panel.Column > 0 ? panel.Column : (finalSize.Width + spacing) / ((panel.ColumnSize > 0 ? panel.ColumnSize : 120d) + spacing)));
        var columnSize = Math.Max(0, (finalSize.Width - spacing * (column - 1)) / column);

        var top = 0d;

        PanelExtBase.ForeachItems(children, column, (_, items) =>
        {
            var height = 0d;

            foreach (var item in items)
                height = Math.Max(height, item.DesiredSize.Height);

            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];
                item.Arrange(new(i * columnSize + i * spacing, top, columnSize, height));
            }

            top += height + spacing;
        });
    }
}
