using Avalonia;
using Avalonia.Controls.Primitives;
using Manta.Charts.Models;

namespace Manta.Charts.Controls;

public class MChartColumn : TemplatedControl
{
    public static readonly StyledProperty<MChartData.Column> DataProperty = AvaloniaProperty.Register<MChartColumn, MChartData.Column>(
        nameof(Data));

    public MChartData.Column Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}
