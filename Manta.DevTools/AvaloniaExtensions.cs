using Avalonia;

namespace Manta.DevTools;

public static class AvaloniaExtensions
{
    extension(AppBuilder appBuilder)
    {
        public AppBuilder UseDevTools()
        {
            DevTools.IsEnabled = true;
            return appBuilder;
        }
    }
}
