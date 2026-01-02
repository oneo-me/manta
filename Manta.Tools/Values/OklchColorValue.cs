namespace Manta.Tools.Values;

public sealed class OklchColorValue
{
    public double L { get; set; }
    public double C { get; set; }
    public double H { get; set; }

    public string ToString(string key)
    {
        return $"<values:OklchColor x:Key=\"{{MBrushKey {key}}}\" L=\"{L}\" C=\"{C}\" H=\"{H}\" />";
    }
}
