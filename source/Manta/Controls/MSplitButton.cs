using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MSplitButtonVariant
{
    Default,
    Outline,
    Secondary,
    Ghost,
    Destructive
}

public class MSplitButton : SplitButton
{
    public static readonly StyledProperty<MSplitButtonVariant> VariantProperty = AvaloniaProperty.Register<MSplitButton, MSplitButtonVariant>(
        nameof(Variant));

    public MSplitButtonVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
