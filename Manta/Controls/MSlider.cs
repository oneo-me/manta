using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MSliderSize
{
    Default,
    Small,
    Large
}

public class MSlider : Slider
{
    public static readonly StyledProperty<MSliderSize> SizeProperty = AvaloniaProperty.Register<MSlider, MSliderSize>(
        nameof(Size));

    public MSliderSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
