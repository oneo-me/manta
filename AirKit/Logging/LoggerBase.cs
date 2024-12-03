using AirKit.Interfaces;

namespace AirKit.Logging;

public abstract class LoggerBase(bool writeTime, bool writeDate, bool writeLevel, LogLevel logLevel, params string[] areas) : ILogger
{
    protected abstract void WriteLine(LogLevel level, string message);

    public bool IsEnabled(LogLevel level, string area)
    {
        if ((level & logLevel) == 0)
            return false;

        return areas.Length == 0 || areas.Contains(area);
    }

    public void WriteLine(LogLevel level, string area, string messageTemplate, params object?[] propertyValues)
    {
        if (IsEnabled(level, area) is false)
            return;

        WriteLine(level, Format(level, area, messageTemplate, propertyValues));
    }

    string Format(LogLevel level, string area, string template, object?[] propertyValues)
    {
        return Log.FormatMessage(template, propertyValues, writeTime ? DateTimeOffset.Now : null, writeDate, writeLevel ? level : null, area);
    }
}
