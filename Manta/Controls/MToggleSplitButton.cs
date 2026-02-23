using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MToggleSplitButtonVariant
{
    Outline,
    Ghost
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
}
