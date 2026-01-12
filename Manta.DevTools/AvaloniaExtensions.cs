using Avalonia;
using Manta.Globalization;

namespace Manta.DevTools;

public static class AvaloniaExtensions
{
    static DevTools? _devTools;

    extension(AppBuilder appBuilder)
    {
        public AppBuilder UseDevTools(DevToolsOptions options = default)
        {
            return appBuilder.AfterSetup(_ =>
            {
                if (_devTools is not null)
                    return;

                _devTools = new(options);
                Lang.Register(LangData.LoadFromAssembly(typeof(DevTools).Assembly));
            });
        }
    }
}
