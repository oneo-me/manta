using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace AirKit.Controls.Panels;

static class WaterfallLayout
{
    public static Size Measure(LightPanel panel, ReadOnlyCollection<Control> children, Size availableSize, bool isBest)
    {
        var spacing = panel.Spacing;
        var column = (int)Math.Max(1, Math.Round(panel.Column > 0 ? panel.Column : availableSize.Width / (panel.ColumnSize > 0 ? panel.ColumnSize : 120d)));
        var result = new Size(availableSize.Width, 0);
        var columnSize = Math.Max(0, (result.Width - spacing * (column - 1)) / column);
        var layoutSize = new Size(columnSize, double.PositiveInfinity);

        var tops = new double[column];
        var maxHeight = 0d;

        LightPanelBase.ForeachItems(children, column, (row, items) =>
        {
            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];
                item.Measure(layoutSize);

                var top = 0d;
                var index = i;

                if (row > 0)
                {
                    if (isBest)
                    {
                        top = tops.Min(x => x);
                        index = Array.IndexOf(tops, top);
                    }
                    else
                    {
                        top = tops[i];
                    }
                }

                var nextTop = top + item.DesiredSize.Height + spacing;
                tops[index] = nextTop;

                maxHeight = Math.Max(maxHeight, nextTop);
            }
        });

        result = result.WithHeight(maxHeight - spacing);

        return result;
    }

    public static void Arrange(LightPanel panel, ReadOnlyCollection<Control> children, Size finalSize, bool isBest)
    {
        var spacing = panel.Spacing;
        var column = (int)Math.Max(1, Math.Round(panel.Column > 0 ? panel.Column : finalSize.Width / (panel.ColumnSize > 0 ? panel.ColumnSize : 120d)));
        var columnSize = Math.Max(0, (finalSize.Width - spacing * (column - 1)) / column);

        var tops = new double[column];

        LightPanelBase.ForeachItems(children, column, (row, items) =>
        {
            for (var i = 0; i < items.Length; i++)
            {
                var item = items[i];

                var top = 0d;
                var index = i;

                if (row > 0)
                {
                    if (isBest)
                    {
                        top = tops.Min(x => x);
                        index = Array.IndexOf(tops, top);
                    }
                    else
                    {
                        top = tops[i];
                    }
                }

                var nextTop = top + item.DesiredSize.Height + spacing;
                tops[index] = nextTop;

                item.Arrange(new(index * columnSize + index * spacing, top, columnSize, item.DesiredSize.Height));
            }
        });
    }
}
