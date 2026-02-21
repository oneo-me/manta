using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public class MAspectRatio : MPanelBase
{
    static MAspectRatio()
    {
        AffectsMeasure<MAspectRatio>(RatioProperty);
    }

    public static readonly StyledProperty<string?> RatioProperty = AvaloniaProperty.Register<MAspectRatio, string?>(
        nameof(Ratio));

    public string? Ratio
    {
        get => GetValue(RatioProperty);
        set => SetValue(RatioProperty, value);
    }

    double _ratio = 1.0;

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == RatioProperty)
        {
            var doubles = change.GetNewValue<string?>()?.Split('/').Select(x => double.Parse(x.Trim())).ToArray();
            if (doubles is null || doubles.Length != 2 || doubles[1] == 0)
                _ratio = 1.0;
            else
                _ratio = doubles[0] / doubles[1];
        }
    }

    protected override Size MeasureOverride(ReadOnlyCollection<Control> children, Size availableSize)
    {
        var innerSize = new Size();

        foreach (var child in children)
        {
            child.Measure(availableSize);

            innerSize = innerSize.WithWidth(Math.Max(innerSize.Width, child.DesiredSize.Width));
            innerSize = innerSize.WithHeight(Math.Max(innerSize.Height, child.DesiredSize.Height));
        }

        var widthFinite = !double.IsInfinity(availableSize.Width);
        var heightFinite = !double.IsInfinity(availableSize.Height);

        double width, height;
        if (widthFinite && heightFinite)
        {
            // Both dimensions are limited: choose the maximum size that can fit a rectangular proportion (letterbox/pillarbox).
            var widthBasedHeight = availableSize.Width / _ratio;
            if (widthBasedHeight <= availableSize.Height)
            {
                width = availableSize.Width;
                height = widthBasedHeight;
            }
            else
            {
                height = availableSize.Height;
                width = height * _ratio;
            }
        }
        else if (widthFinite)
        {
            width = availableSize.Width;
            height = width / _ratio;
        }
        else if (heightFinite)
        {
            height = availableSize.Height;
            width = height * _ratio;
        }
        else
        {
            // 两维均无限：以内容宽度为基准
            width = innerSize.Width;
            height = width / _ratio;
        }

        return new(width, height);
    }

    protected override void ArrangeOverride(ReadOnlyCollection<Control> children, Size finalSize)
    {
        var finalRect = new Rect(finalSize);

        foreach (var child in children)
            child.Arrange(finalRect);
    }
}
