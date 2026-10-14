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
    Flat
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
}
