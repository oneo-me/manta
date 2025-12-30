namespace Manta.Charts.Models;

public class BarChartDataItem
{
    public string Category { get; init; } = string.Empty;
    public IReadOnlyCollection<ChartDataValue> Values { get; init; } = [];
}
