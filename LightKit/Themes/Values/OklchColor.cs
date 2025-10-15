using Avalonia.Media;

namespace LightKit.Themes.Values;

public sealed class OklchColor : ColorBase
{
    public double L { get; init; }
    public double C { get; init; }
    public double H { get; init; }

    public override Color ToColor(double opacity)
    {
        return this.ToRgbColor().ToColor(opacity);
    }
}
