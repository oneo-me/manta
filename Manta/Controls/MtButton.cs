using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MtButtonVariant
{
    Default,
    Outline,
    Secondary,
    Ghost,
    Destructive,
    Link
}

public enum MtButtonSize
{
    Default,
    Small,
    Large
}

public class MtButton : Button
{
    public static readonly StyledProperty<MtButtonVariant> VariantProperty = AvaloniaProperty.Register<MtButton, MtButtonVariant>(
        nameof(Variant));

    public MtButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<MtButtonSize> SizeProperty = AvaloniaProperty.Register<MtButton, MtButtonSize>(
        nameof(Size));

    public MtButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
