using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MRepeatButtonVariant
{
    Default,
    Outline,
    Secondary,
    Ghost,
    Destructive,
    Link
}

public enum MRepeatButtonSize
{
    Default,
    Small,
    Large
}

public class MRepeatButton : RepeatButton
{
    public static readonly StyledProperty<MRepeatButtonVariant> VariantProperty = AvaloniaProperty.Register<MRepeatButton, MRepeatButtonVariant>(
        nameof(Variant));

    public MRepeatButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<MRepeatButtonSize> SizeProperty = AvaloniaProperty.Register<MRepeatButton, MRepeatButtonSize>(
        nameof(Size));

    public MRepeatButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
