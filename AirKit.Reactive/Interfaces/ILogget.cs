using AirKit.Reactive.Logging;

namespace AirKit.Reactive.Interfaces;

public interface ILogger
{
    void WriteLine(LogLevel level, string area, string messageTemplate, params object?[] propertyValues);
}
