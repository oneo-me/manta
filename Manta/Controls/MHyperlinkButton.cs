using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MHyperlinkButtonVariant
{
    Default,
    Subtle
}

public class MHyperlinkButton : HyperlinkButton
{
    public static readonly StyledProperty<MHyperlinkButtonVariant> VariantProperty = AvaloniaProperty.Register<MHyperlinkButton, MHyperlinkButtonVariant>(
        nameof(Variant));

    public MHyperlinkButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
