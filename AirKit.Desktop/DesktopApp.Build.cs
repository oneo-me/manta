using AirKit.Extensions;
using AirKit.Logging;
using Avalonia;

namespace AirKit.Desktop;

public static partial class DesktopApp
{
    public static AppBuilder Build<T>() where T : LightApp, new()
    {
        return AppBuilder.Configure<T>()
            .UseDefaultFontFamily()
            .UsePlatformDetect()
            .UseAvaloniaLogger();
    }
}
