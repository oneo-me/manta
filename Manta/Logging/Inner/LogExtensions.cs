using Avalonia;
using Avalonia.Logging;

namespace Manta.Logging.Inner;

static class LogExtensions
{
    public static AppBuilder LogToAxe(this AppBuilder builder, LogLevel logLevel = LogLevel.Error)
    {
        Logger.Sink = new InnerAvaloniaLogger(logLevel);
        return builder;
    }
}
