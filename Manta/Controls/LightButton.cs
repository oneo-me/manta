using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum LightButtonVariant
{
    Default,
    Outline,
    Secondary,
    Ghost,
    Destructive,
    Link
}

public enum LightButtonSize
{
    Default,
    Small,
    Large
}

public class LightButton : Button
{
    public static readonly StyledProperty<LightButtonVariant> VariantProperty = AvaloniaProperty.Register<LightButton, LightButtonVariant>(
        nameof(Variant));

    public LightButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<LightButtonSize> SizeProperty = AvaloniaProperty.Register<LightButton, LightButtonSize>(
        nameof(Size));

    public LightButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
