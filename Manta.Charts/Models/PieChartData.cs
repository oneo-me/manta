namespace Manta.Charts.Models;

public class PieChartData : ChartDataBase
{
    public IList<ChartDataValue> Values { get; init; } = [];
}
