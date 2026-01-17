namespace Manta.DevTools;

public struct DevToolsOptions
{
    public double DefaultHeight { get; init; } = 320;

    public bool AutoShowByDefault { get; init; } = false;

    public bool EnableFpsByDefault { get; init; } = false;
    public bool EnableDirtyRectsByDefault { get; init; } = false;
    public bool EnableTimeGraphByDefault { get; init; } = false;

    public DevToolsOptions()
    {
    }
}
