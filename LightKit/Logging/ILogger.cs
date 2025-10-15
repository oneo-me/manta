namespace LightKit.Logging;

public interface ILogger
{
    void WriteLine(DateTimeOffset time, LogLevel level, string messageTemplate, params object?[] propertyValues);
}
