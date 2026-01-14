using Avalonia;
using Avalonia.Controls.Primitives;

namespace Manta.Controls;

public enum MToggleButtonVariant
{
    Outline
}

public class MToggleButton : ToggleButton
{
    public static readonly StyledProperty<MToggleButtonVariant> VariantProperty = AvaloniaProperty.Register<MToggleButton, MToggleButtonVariant>(
        nameof(Variant));

    public MToggleButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
