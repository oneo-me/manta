using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MtBadgeVariant
{
    Default,
    Secondary,
    Destructive,
    Outline
}

public class MtBadge : ContentControl
{
    public static readonly StyledProperty<MtBadgeVariant> VariantProperty = AvaloniaProperty.Register<MtBadge, MtBadgeVariant>(
        nameof(Variant));

    public MtBadgeVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
