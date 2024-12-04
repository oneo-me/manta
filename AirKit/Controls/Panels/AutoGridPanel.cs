using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace AirKit.Controls.Panels;

static class AutoGridPanel
{
    static IEnumerable<Control> GetControlsFromColumn(IEnumerable<Control> children, int columnCount, int column)
    {
        return children.Where((_, i) => i % columnCount == column);
    }

    static IEnumerable<Control> GetControlsFromRow(IList<Control> children, int columnCount, int row)
    {
        for (var i = columnCount * row; i < Math.Min(columnCount * (row + 1), children.Count); i++)
            yield return children[i];
    }

    static double GetDefinitionWidthCore(ColumnDefinitions definitions, ColumnDefinition definition, double size, double spacing, bool star)
    {
        if (definition.Width.IsAbsolute)
            return definition.Width.Value;

        if (definition.Width.IsAuto || double.IsInfinity(size))
            return double.PositiveInfinity;

        if (definition.Width.IsStar && star)
        {
            var starCount = definitions.Where(x => x.Width.IsStar).Sum(x => x.Width.Value);
            return (size - (definitions.Count - 1) * spacing) / starCount * definition.Width.Value;
        }

        return 0d;
    }

    static double GetDefinitionWidth(ColumnDefinitions definitions, ColumnDefinition definition, double size, double spacing, bool star)
    {
        var width = GetDefinitionWidthCore(definitions, definition, size, spacing, star);
        return Math.Max(definition.MinWidth, Math.Min(definition.MaxWidth, width));
    }

    static void CalculateWidths(Dictionary<int, double> widths, IReadOnlyCollection<Control> children, ColumnDefinitions definitions, int columnCount, double size, double spacing, bool isMeasure)
    {
        for (var i = 0; i < columnCount; i++)
        {
            var maxWidth = 0d;

            var definition = definitions[i];

            foreach (var control in GetControlsFromColumn(children, columnCount, i))
            {
                var width = GetDefinitionWidth(definitions, definition, size, spacing, false);
                if (isMeasure)
                    control.Measure(new(width, double.PositiveInfinity));
                maxWidth = Math.Max(maxWidth, double.IsInfinity(width) ? control.DesiredSize.Width : width);
            }

            widths[i] = maxWidth;
        }
    }

    static void CalculateAutoWidths(Dictionary<int, double> widths, IReadOnlyCollection<Control> children, ColumnDefinitions definitions, int columnCount, double size, double spacing, bool isMeasure)
    {
        for (var i = 0; i < definitions.Count; i++)
        {
            var definition = definitions[i];
            if (definition.Width.IsStar is false)
                continue;

            var width = GetDefinitionWidth(definitions, definition, size, spacing, true);

            foreach (var control in GetControlsFromColumn(children, columnCount, i))
                if (isMeasure)
                    control.Measure(new(width, double.PositiveInfinity));

            widths[i] = width;
        }
    }

    public static Size Measure(AutoGrid panel, ReadOnlyCollection<Control> children, Size availableSize)
    {
        var spacing = panel.Spacing;
        var definitions = panel.Definitions is null || panel.Definitions.Count < 0 ? [new(GridLength.Star)] : panel.Definitions;
        var columnCount = definitions.Count;
        var columnWidths = new Dictionary<int, double>();
        CalculateWidths(columnWidths, children, definitions, columnCount, availableSize.Width, spacing, true);
        if (double.IsInfinity(availableSize.Width) is false)
            CalculateAutoWidths(columnWidths, children, definitions, columnCount, availableSize.Width - columnWidths.Values.Sum(), spacing, true);

        var rowCount = (int)Math.Ceiling(children.Count / (double)columnCount);
        var height = 0d;
        var maxRowWidth = 0d;

        for (var i = 0; i < rowCount; i++)
        {
            var maxHeight = 0d;
            var rowWidth = 0d;

            foreach (var control in GetControlsFromRow(children, columnCount, i))
            {
                maxHeight = Math.Max(maxHeight, control.DesiredSize.Height);
                rowWidth += control.DesiredSize.Width + spacing;
            }

            height += maxHeight;
            maxRowWidth = Math.Max(maxRowWidth, Math.Max(rowWidth - spacing, 0));
        }

        return new(maxRowWidth, height + spacing * (rowCount - 1));
    }

    public static void Arrange(AutoGrid panel, ReadOnlyCollection<Control> children, Size finalSize)
    {
        var spacing = panel.Spacing;
        var definitions = panel.Definitions is null || panel.Definitions.Count < 0 ? [new(GridLength.Star)] : panel.Definitions;
        var columnCount = definitions.Count;
        var columnWidths = new Dictionary<int, double>();
        CalculateWidths(columnWidths, children, definitions, columnCount, finalSize.Width, spacing, false);
        if (double.IsInfinity(finalSize.Width) is false)
            CalculateAutoWidths(columnWidths, children, definitions, columnCount, finalSize.Width - columnWidths.Values.Sum(), spacing, false);

        var rowCount = (int)Math.Ceiling(children.Count / (double)columnCount);
        var top = 0d;

        for (var i = 0; i < rowCount; i++)
        {
            var maxHeight = 0d;

            var controls = GetControlsFromRow(children, columnCount, i).ToList();
            foreach (var control in controls)
                maxHeight = Math.Max(maxHeight, control.DesiredSize.Height);

            for (var m = 0; m < controls.Count; m++)
            {
                var control = controls[m];
                control.Arrange(new(spacing * m + columnWidths.Values.Take(m).Sum(), top, columnWidths[m], maxHeight));
            }

            top += maxHeight + spacing;
        }
    }
}
