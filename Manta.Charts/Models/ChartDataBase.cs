namespace Manta.Charts.Models;

public abstract class ChartDataBase
{
    public string Title { get; init; } = string.Empty;
    public bool ShowLegend { get; init; } = true;
}
