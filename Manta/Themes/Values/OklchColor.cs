using Avalonia.Media;

namespace Manta.Themes.Values;

public sealed class OklchColor : ColorBase
{
    public double L { get; init; }
    public double C { get; init; }
    public double H { get; init; }

    public override Color ToColor(double opacityFactor = 1.0)
    {
        return this.ToRgbColor().ToColor(opacityFactor);
    }
}
