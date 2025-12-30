namespace Manta.Charts.Models;

public class PieChartData : ChartDataBase
{
    public IReadOnlyCollection<ChartDataValue> Values { get; init; } = [];
}
