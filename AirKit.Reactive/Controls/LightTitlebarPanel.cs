using Avalonia;
using Avalonia.Controls;

namespace AirKit.Reactive.Controls;

public class LightTitlebarPanel : Panel
{
    static LightTitlebarPanel()
    {
        AffectsMeasure<LightPath>(IsFullProperty);
        AffectsArrange<LightPath>(IsFullProperty);
    }

    public static readonly StyledProperty<bool> IsFullProperty = AvaloniaProperty.Register<LightTitlebarPanel, bool>(
        nameof(IsFull));

    public bool IsFull
    {
        get => GetValue(IsFullProperty);
        set => SetValue(IsFullProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        if (Children.Count != 4)
            throw new InvalidOperationException("LightTitlebarPanel must have exactly 4 children");

        var measureSize = new Size(double.PositiveInfinity, availableSize.Height);
        var height = 0d;
        var width = 0d;

        foreach (var control in Children)
        {
            if (control.IsVisible is false)
                continue;

            control.Measure(measureSize);
            width += control.DesiredSize.Width;
            height = Math.Max(height, control.DesiredSize.Height);
        }

        return new(width, height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Children.Count != 4)
            throw new InvalidOperationException("LightTitlebarPanel must have exactly 4 children");

        var isFull = IsFull;

        var offsetChild = Children[0];
        var centerChild = Children[1];
        var rightChild = Children[2];
        var buttonsChild = Children[3];

        var left = 0d;
        var right = finalSize.Width;

        offsetChild.Arrange(new(left, 0, offsetChild.DesiredSize.Width, finalSize.Height));
        left += offsetChild.DesiredSize.Width;

        right -= buttonsChild.DesiredSize.Width;
        buttonsChild.Arrange(new(right, 0, buttonsChild.DesiredSize.Width, finalSize.Height));

        right -= rightChild.DesiredSize.Width;
        rightChild.Arrange(new(right, 0, rightChild.DesiredSize.Width, finalSize.Height));

        if (isFull)
        {
            centerChild.Arrange(new(left, 0, Math.Max(0, right - left), finalSize.Height));
            return finalSize;
        }

        var leftWidth = left;
        var rightWidth = finalSize.Width - right;
        var leftSpace = rightWidth > leftWidth ? rightWidth - leftWidth : 0;
        var rightSpace = leftWidth > rightWidth ? leftWidth - rightWidth : 0;

        var testWidth = right - leftWidth - leftSpace - rightSpace;
        var centerWidth = centerChild.DesiredSize.Width + 10 * 2;

        if (testWidth < centerWidth)
        {
            var offset = Math.Max(centerWidth - testWidth, 0);
            var scale = leftSpace <= 0 ? 0 : rightSpace <= 0 ? 1 : leftSpace / rightSpace;
            leftSpace = Math.Max(leftSpace - offset * scale, 0);
            rightSpace = Math.Max(rightSpace - offset * (1 - scale), 0);
        }

        centerChild.Arrange(new(leftWidth + leftSpace, 0, Math.Max(0, right - leftWidth - leftSpace - rightSpace), finalSize.Height));

        return finalSize;
    }
}
