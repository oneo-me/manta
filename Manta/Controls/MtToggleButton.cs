using Avalonia;
using Avalonia.Controls.Primitives;

namespace Manta.Controls;

public enum MtToggleButtonVariant
{
    Outline,
    WithText
}

public enum MtToggleButtonSize
{
    Default,
    Small,
    Large
}

public class MtToggleButton : ToggleButton
{
    public static readonly StyledProperty<MtToggleButtonVariant> VariantProperty = AvaloniaProperty.Register<MtToggleButton, MtToggleButtonVariant>(
        nameof(Variant));

    public MtToggleButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<MtToggleButtonSize> SizeProperty = AvaloniaProperty.Register<MtToggleButton, MtToggleButtonSize>(
        nameof(Size));

    public MtToggleButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
