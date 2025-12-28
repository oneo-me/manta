using Avalonia.Media;

namespace Manta.Themes.Values;

public abstract class ColorBase
{
    public double Opacity { get; set; } = 1.0;
    public abstract Color ToColor(double opacityFactor = 1.0);
}
