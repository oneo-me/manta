using Avalonia;
using Avalonia.Controls.Primitives;

namespace Manta.Controls;

public enum LightToggleButtonVariant
{
    Outline,
    WithText
}

public enum LightToggleButtonSize
{
    Default,
    Small,
    Large
}

public class LightToggleButton : ToggleButton
{
    public static readonly StyledProperty<LightToggleButtonVariant> VariantProperty = AvaloniaProperty.Register<LightToggleButton, LightToggleButtonVariant>(
        nameof(Variant));

    public LightToggleButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<LightToggleButtonSize> SizeProperty = AvaloniaProperty.Register<LightToggleButton, LightToggleButtonSize>(
        nameof(Size));

    public LightToggleButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
