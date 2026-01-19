namespace Manta.DevTools;

public sealed class DevToolsOptions
{
    public double DefaultHeight { get; init; } = 320;

    public bool AutoShowForMainWindow { get; init; } = false;
    public bool AutoShowForOtherWindows { get; init; } = false;

    public bool EnableFpsByDefault { get; init; } = false;
    public bool EnableDirtyRectsByDefault { get; init; } = false;
    public bool EnableTimeGraphByDefault { get; init; } = false;
}
