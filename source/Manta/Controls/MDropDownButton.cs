using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MDropDownButtonVariant
{
    Default,
    Outline,
    Secondary,
    Ghost,
    Destructive,
    Flat
}

public class MDropDownButton : DropDownButton
{
    public static readonly StyledProperty<MDropDownButtonVariant> VariantProperty = AvaloniaProperty.Register<MDropDownButton, MDropDownButtonVariant>(
        nameof(Variant));

    public MDropDownButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
