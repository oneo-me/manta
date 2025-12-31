using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MToggleSplitButtonVariant
{
    Default,
    Outline,
    Secondary,
    Ghost
}

public enum MToggleSplitButtonSize
{
    Default,
    Small,
    Large
}

public class MToggleSplitButton : ToggleSplitButton
{
    protected override Type StyleKeyOverride => typeof(MToggleSplitButton);

    public static readonly StyledProperty<MToggleSplitButtonVariant> VariantProperty = AvaloniaProperty.Register<MToggleSplitButton, MToggleSplitButtonVariant>(
        nameof(Variant));

    public MToggleSplitButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<MToggleSplitButtonSize> SizeProperty = AvaloniaProperty.Register<MToggleSplitButton, MToggleSplitButtonSize>(
        nameof(Size));

    public MToggleSplitButtonSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
