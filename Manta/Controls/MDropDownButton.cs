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
    Link
}

public enum MDropDownButtonSize
{
    Default,
    Small,
    Large
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

    public static readonly StyledProperty<MDropDownButtonSize> SizeProperty = AvaloniaProperty.Register<MDropDownButton, MDropDownButtonSize>(
        nameof(Size));

    public MDropDownButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
