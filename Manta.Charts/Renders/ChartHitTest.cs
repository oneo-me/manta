using Avalonia;
using Avalonia.Media;

namespace Manta.Charts.Renders;

/// <summary>
///     Hit test rectangle for bar/line charts.
/// </summary>
public readonly record struct HitRect(Rect Bounds, int GroupIndex, string Category);

/// <summary>
///     Hit test info for pie charts.
/// </summary>
public readonly record struct PieHitInfo(Point Center, double Radius, double[] Angles, string[] Names, double[] Values, Color[] Colors);

/// <summary>
///     Aggregates hit test results from renderers.
/// </summary>
public class ChartHitTestResult
{
    public List<HitRect> HitRects { get; } = [];
    public PieHitInfo? PieHitInfo { get; set; }

    public void Clear()
    {
        HitRects.Clear();
        PieHitInfo = null;
    }
}
