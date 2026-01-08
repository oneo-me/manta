using Manta.Themes.Values;

namespace Manta.Tools.Values;

public sealed class OklchColorValue(OklchColor? oklchColor = null)
{
    public OklchColor Color { get; } = oklchColor ?? new OklchColor();

    public string ToString(string key)
    {
        return $"<values:OklchColor x:Key=\"{{MBrushKey {key}}}\" L=\"{Color.L}\" C=\"{Color.C}\" H=\"{Color.H}\" Opacity=\"{Color.Opacity}\" />";
    }
}
