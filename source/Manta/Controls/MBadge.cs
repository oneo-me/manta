using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MBadgeVariant
{
    Default,
    Secondary,
    Destructive,
    Outline
}

public class MBadge : ContentControl
{
    public static readonly StyledProperty<MBadgeVariant> VariantProperty = AvaloniaProperty.Register<MBadge, MBadgeVariant>(
        nameof(Variant));

    public MBadgeVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
