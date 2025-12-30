using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Manta.Charts.Controls;

public class MChartLegend : TemplatedControl
{
    public static readonly StyledProperty<IBrush?> BrushProperty = AvaloniaProperty.Register<MChartLegend, IBrush?>(
        nameof(Brush));

    public IBrush? Brush
    {
        get => GetValue(BrushProperty);
        set => SetValue(BrushProperty, value);
    }

    public static readonly StyledProperty<string?> DataProperty = AvaloniaProperty.Register<MChartLegend, string?>(
        nameof(Data));

    public string? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}
