using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public class LightAspectRatio : LightPanelBase
{
    static LightAspectRatio()
    {
        AffectsMeasure<LightAspectRatio>(RatioProperty);
    }

    public static readonly StyledProperty<string> RatioProperty = AvaloniaProperty.Register<LightAspectRatio, string>(
        nameof(Ratio));

    public string Ratio
    {
        get => GetValue(RatioProperty);
        set => SetValue(RatioProperty, value);
    }

    double _ratio = 1.0;

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == RatioProperty)
            if (change.NewValue is string str)
            {
                var doubles = str.Split('/').Select(x => double.Parse(x.Trim())).ToArray();
                _ratio = doubles[0] / doubles[1];
            }
    }

    protected override Size MeasureOverride(ReadOnlyCollection<Control> children, Size availableSize)
    {
        var result = new Size();

        foreach (var child in children)
        {
            child.Measure(availableSize);

            result = result.WithWidth(Math.Max(result.Width, child.DesiredSize.Width));
            result = result.WithHeight(Math.Max(result.Height, child.DesiredSize.Height));
        }

        if (double.IsInfinity(availableSize.Width) && double.IsInfinity(availableSize.Height))
        {
            result = result.WithHeight(result.Width / _ratio);
        }
        else if (double.IsInfinity(availableSize.Width))
        {
            result = result.WithHeight(Math.Min(result.Height, availableSize.Height));
            result = result.WithWidth(result.Height * _ratio);
        }
        else if (double.IsInfinity(availableSize.Height))
        {
            result = result.WithWidth(Math.Min(result.Width, availableSize.Width));
            result = result.WithHeight(result.Width / _ratio);
        }
        else
        {
            var heightBasedWidth = availableSize.Height * _ratio;
            if (heightBasedWidth <= availableSize.Width)
            {
                result = result.WithHeight(availableSize.Height);
                result = result.WithWidth(heightBasedWidth);
            }
            else
            {
                var widthBasedHeight = availableSize.Width / _ratio;
                result = result.WithWidth(availableSize.Width);
                result = result.WithHeight(widthBasedHeight);
            }
        }

        return result;
    }

    protected override void ArrangeOverride(ReadOnlyCollection<Control> children, Size finalSize)
    {
        var finalRect = new Rect(finalSize);

        foreach (var child in children)
            child.Arrange(finalRect);
    }
}
