namespace Manta.DevTools;

public sealed class DevToolsOptions
{
    public bool AutoShowForMainWindow { get; init; } = true;
    public bool AutoShowForOtherWindows { get; init; } = false;

    public bool EnableFpsByDefault { get; init; } = false;
    public bool EnableDirtyRectsByDefault { get; init; } = false;
    public bool EnableTimeGraphByDefault { get; init; } = false;
}
