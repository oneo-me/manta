namespace Manta.DevTools;

public struct DevToolsOptions
{
    public bool AutoShowDevToolsOnStart { get; init; } = false;
    public double DevToolsDefaultHeight { get; init; } = 320;

    public DevToolsOptions()
    {
    }
}
