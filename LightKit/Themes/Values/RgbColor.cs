using Avalonia.Media;

namespace LightKit.Themes.Values;

public sealed class RgbColor : ColorBase
{
    public int R { get; init; }
    public int G { get; init; }
    public int B { get; init; }

    public override Color ToColor(double opacity)
    {
        return Color.FromArgb((byte)(255.0 * opacity), (byte)R, (byte)G, (byte)B);
    }
}
