using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace AirKit.Controls;

public abstract class PanelExtBase : Panel
{
    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<PanelExtBase, double>(nameof(Spacing));

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public static readonly StyledProperty<bool> ReverseOrderProperty = AvaloniaProperty.Register<PanelExtBase, bool>(
        nameof(ReverseOrder));

    public bool ReverseOrder
    {
        get => GetValue(ReverseOrderProperty);
        set => SetValue(ReverseOrderProperty, value);
    }

    static PanelExtBase()
    {
        AffectsMeasure<PanelExtBase>(SpacingProperty, ReverseOrderProperty);
        AffectsArrange<PanelExtBase>(SpacingProperty, ReverseOrderProperty);
    }

    public static void ForeachItems(ReadOnlyCollection<Control> children, int column, Action<int, Control[]> action)
    {
        var row = -1;

        foreach (var items in children.Chunk(column))
        {
            row++;
            action(row, items);
        }
    }

    ReadOnlyCollection<Control> GetVisibleChildren()
    {
        var children = new Collection<Control>();

        if (ReverseOrder)
            for (var i = Children.Count - 1; i >= 0; i--)
            {
                var child = Children[i];
                if (child.IsVisible)
                    children.Add(child);
            }
        else
            foreach (var child in Children)
                if (child.IsVisible)
                    children.Add(child);

        return new(children);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var children = GetVisibleChildren();
        if (children.Count <= 0)
            return new();

        return MeasureOverride(children, availableSize);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var children = GetVisibleChildren();
        if (children.Count <= 0)
            return new();

        ArrangeOverride(children, finalSize);
        return finalSize;
    }

    protected abstract Size MeasureOverride(ReadOnlyCollection<Control> children, Size availableSize);
    protected abstract void ArrangeOverride(ReadOnlyCollection<Control> children, Size finalSize);
}
