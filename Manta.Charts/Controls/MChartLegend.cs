using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Manta.Charts.Controls;

public class MChartLegend : TemplatedControl
{
    public static readonly StyledProperty<ChartLegendOrientation> OrientationProperty =
        AvaloniaProperty.Register<MChartLegend, ChartLegendOrientation>(nameof(Orientation));

    public ChartLegendOrientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly StyledProperty<ObservableCollection<ChartLegendItem>?> ItemsProperty =
        AvaloniaProperty.Register<MChartLegend, ObservableCollection<ChartLegendItem>?>(nameof(Items));

    public ObservableCollection<ChartLegendItem>? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
}

public enum ChartLegendOrientation
{
    Horizontal,
    Vertical
}

public class ChartLegendItem
{
    public string Name { get; init; } = string.Empty;
    public Color Color { get; init; }

    public IBrush ColorBrush => new SolidColorBrush(Color);
}
