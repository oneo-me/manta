using Avalonia;
using Avalonia.Controls.Primitives;
using Manta.Charts.Models;

namespace Manta.Charts.Controls;

public class MChart : TemplatedControl
{
    public static readonly StyledProperty<ChartDataBase?> DataProperty = AvaloniaProperty.Register<MChart, ChartDataBase?>(
        nameof(Data));

    public ChartDataBase? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}
