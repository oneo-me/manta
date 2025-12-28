using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum LightBadgeVariant
{
    Default,
    Secondary,
    Destructive,
    Outline
}

public class LightBadge : ContentControl
{
    public static readonly StyledProperty<LightBadgeVariant> VariantProperty = AvaloniaProperty.Register<LightBadge, LightBadgeVariant>(
        nameof(Variant));

    public LightBadgeVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
