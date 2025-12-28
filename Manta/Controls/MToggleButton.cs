using Avalonia;
using Avalonia.Controls.Primitives;

namespace Manta.Controls;

public enum MToggleButtonVariant
{
    Outline,
    WithText
}

public enum MToggleButtonSize
{
    Default,
    Small,
    Large
}

public class MToggleButton : ToggleButton
{
    public static readonly StyledProperty<MToggleButtonVariant> VariantProperty = AvaloniaProperty.Register<MToggleButton, MToggleButtonVariant>(
        nameof(Variant));

    public MToggleButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<MToggleButtonSize> SizeProperty = AvaloniaProperty.Register<MToggleButton, MToggleButtonSize>(
        nameof(Size));

    public MToggleButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
