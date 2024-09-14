using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MButtonVariant
{
    Default,
    Outline,
    Secondary,
    Ghost,
    Destructive,
    Link
}

public enum MButtonSize
{
    Default,
    Small,
    Large
}

public class MButton : Button
{
    public static readonly StyledProperty<MButtonVariant> VariantProperty = AvaloniaProperty.Register<MButton, MButtonVariant>(
        nameof(Variant));

    public MButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<MButtonSize> SizeProperty = AvaloniaProperty.Register<MButton, MButtonSize>(
        nameof(Size));

    public MButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
