using Avalonia;
using Avalonia.Controls;

namespace Manta.Controls;

public enum MProgressBarVariant
{
    Default,
    Secondary,
    Destructive,
    Success,
    Warning
}

public class MProgressBar : ProgressBar
{
    public static readonly StyledProperty<MProgressBarVariant> VariantProperty = AvaloniaProperty.Register<MProgressBar, MProgressBarVariant>(
        nameof(Variant));

    public MProgressBarVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
