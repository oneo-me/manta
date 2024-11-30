using Avalonia;
using Avalonia.Logging;

namespace AirKit.Logging;

public static class LogExtensions
{
    public static AppBuilder UseAvaloniaLogger(this AppBuilder builder)
    {
        Logger.Sink = new InnerAvaloniaLogger();
        return builder;
    }
}
