using Avalonia;

namespace Manta.DevTools;

public static class AvaloniaExtensions
{
    static DevTools? _devTools;

    extension(AppBuilder appBuilder)
    {
        public AppBuilder UseDevTools(DevToolsOptions options = default)
        {
            _devTools ??= new(options);
            return appBuilder;
        }
    }
}
