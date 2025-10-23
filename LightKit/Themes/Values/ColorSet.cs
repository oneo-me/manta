namespace LightKit.Themes.Values;

public sealed class ColorSet
{
    public double H { get; init; }

    public double CMin { get; set; } = 0.05;
    public double CMax { get; set; } = 0.35;
    public double LMin { get; set; } = 0.4;
    public double LMax { get; set; } = 0.98;

    public OklchColor GetColor(int step, int totalSteps)
    {
        var mid = (totalSteps - 1) / 2;

        var l = LMin + step / (totalSteps - 1d) * (LMax - LMin);
        var t = Math.Abs(step - mid) / (double)mid;
        var c = CMax - t * (CMax - CMin);

        return new()
        {
            L = l,
            C = c,
            H = H
        };
    }
}
