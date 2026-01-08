using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manta.Controls;

public enum MRepeatButtonVariant
{
    Default,
    Outline,
    Secondary,
    Ghost,
    Destructive,

    NoStyle
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

    public static readonly StyledProperty<BoxShadows> BoxShadowProperty = AvaloniaProperty.Register<MRepeatButton, BoxShadows>(
        nameof(BoxShadow));

    public BoxShadows BoxShadow
    {
        get => GetValue(BoxShadowProperty);
        set => SetValue(BoxShadowProperty, value);
    }
}
