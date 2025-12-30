namespace Manta.Charts.Models;

public class BarChartData : ChartDataBase
{
    public BarChartType Type { get; init; } = BarChartType.Bar;
    public IList<BarChartDataItem> Items { get; init; } = [];
}
