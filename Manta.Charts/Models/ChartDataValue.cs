using Avalonia.Media;

namespace Manta.Charts.Models;

public class ChartDataValue
{
    public string Name { get; init; } = string.Empty;
    public double Value { get; init; } = 0d;
    public Color? CustomColor { get; init; }
}
