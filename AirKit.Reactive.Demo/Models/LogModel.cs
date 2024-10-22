using AirKit.Reactive.Logging;

namespace AirKit.Reactive.Demo.Models;

public sealed class LogModel(DateTimeOffset time, LogLevel level, string area, string message)
{
    public DateTimeOffset Time { get; } = time;
    public LogLevel Level { get; } = level;
    public string Area { get; } = area;
    public string Message { get; } = message;
}
