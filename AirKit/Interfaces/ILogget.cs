using AirKit.Logging;

namespace AirKit.Interfaces;

public interface ILogger
{
    void WriteLine(LogLevel level, string area, string messageTemplate, params object?[] propertyValues);
}
