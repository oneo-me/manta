namespace Manta.Charts.Models;

public class BarChartData : ChartDataBase
{
    public BarChartType Type { get; init; } = BarChartType.Bar;
    public IReadOnlyCollection<BarChartDataItem> Items { get; init; } = [];
}
