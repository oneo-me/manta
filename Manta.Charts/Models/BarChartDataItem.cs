namespace Manta.Charts.Models;

public class BarChartDataItem
{
    public string Category { get; init; } = string.Empty;
    public IList<ChartDataValue> Values { get; init; } = [];
}
