using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manta.Controls;

public class MProgressBar : ProgressBar
{
    public static readonly StyledProperty<IBrush?> BarBrushProperty =
        AvaloniaProperty.Register<MProgressBar, IBrush?>(nameof(BarBrush));

    public IBrush? BarBrush
    {
        get => GetValue(BarBrushProperty);
        set => SetValue(BarBrushProperty, value);
    }

    public static readonly StyledProperty<MProgressVariant> VariantProperty = AvaloniaProperty.Register<MProgressBar, MProgressVariant>(
        nameof(Variant));

    public MProgressVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }
}
