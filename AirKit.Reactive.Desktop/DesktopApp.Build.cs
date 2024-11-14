using AirKit.Reactive.Extensions;
using AirKit.Reactive.Logging;
using Avalonia;

namespace AirKit.Reactive.Desktop;

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
