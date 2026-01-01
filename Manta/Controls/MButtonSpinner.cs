using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MButtonSpinnerVariant
{
    Default,
    Outline
}

public enum MButtonSpinnerSize
{
    Default,
    Small,
    Large
}

public class MButtonSpinner : ButtonSpinner
{
    public static readonly StyledProperty<MButtonSpinnerVariant> VariantProperty = AvaloniaProperty.Register<MButtonSpinner, MButtonSpinnerVariant>(
        nameof(Variant));

    public MButtonSpinnerVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public static readonly StyledProperty<MButtonSpinnerSize> SizeProperty = AvaloniaProperty.Register<MButtonSpinner, MButtonSpinnerSize>(
        nameof(Size));

    public MButtonSpinnerSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }
}
