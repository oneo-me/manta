using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MToggleSwitchSize
{
    Default,
    Small,
    Large
}

public class MToggleSwitch : ToggleSwitch
{
    public static readonly StyledProperty<MToggleSwitchSize> SizeProperty = AvaloniaProperty.Register<MToggleSwitch, MToggleSwitchSize>(
        nameof(Size));

    public MToggleSwitchSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
