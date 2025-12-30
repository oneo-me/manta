using Avalonia;
using Avalonia.Controls;

namespace Manta.Charts.Controls;

public class MChartBarPanel : Panel
{
    public static readonly StyledProperty<double> RatioProperty = AvaloniaProperty.Register<MChartBarPanel, double>(
        nameof(Ratio));

    public double Ratio
    {
        get => GetValue(RatioProperty);
        set => SetValue(RatioProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var panelHeight = double.IsPositiveInfinity(availableSize.Height) ? 0.0 : availableSize.Height;
        var childConstraintHeight = panelHeight * Ratio;
        var childConstraint = new Size(availableSize.Width, childConstraintHeight);

        foreach (var child in Children)
            child.Measure(childConstraint);

        return new(availableSize.Width, panelHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var arrangeRect = new Rect(0, finalSize.Height * (1 - Ratio), finalSize.Width, finalSize.Height * Ratio);

        foreach (var child in Children)
            child.Arrange(arrangeRect);

        return finalSize;
    }
}
