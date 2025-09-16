using Avalonia;
using Avalonia.Logging;

namespace Axe.Logging.Avalonia;

public static class LogExtensions
{
    public static AppBuilder LogToAxe(this AppBuilder builder, LogLevel logLevel = LogLevel.Warning)
    {
        Logger.Sink = new InnerAvaloniaLogger(logLevel);
        return builder;
    }
}
