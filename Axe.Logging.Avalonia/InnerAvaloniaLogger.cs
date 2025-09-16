using Avalonia.Logging;

namespace Axe.Logging.Avalonia;

sealed class InnerAvaloniaLogger(LogLevel logLevel) : ILogSink
{
    readonly LogEventLevel _logEventLevel = ConvertLogEventLevel(logLevel);

    public bool IsEnabled(LogEventLevel level, string area)
    {
        return level >= _logEventLevel;
    }

    static LogEventLevel ConvertLogEventLevel(LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Verbose:
                return LogEventLevel.Verbose;

            case LogLevel.Debug:
                return LogEventLevel.Debug;

            case LogLevel.Information:
                return LogEventLevel.Information;

            case LogLevel.Warning:
                return LogEventLevel.Warning;

            case LogLevel.Error:
                return LogEventLevel.Error;

            case LogLevel.Fatal:
                return LogEventLevel.Fatal;

            case LogLevel.Important:
            case LogLevel.All:
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }
    }

    static LogLevel ConvertLogLevel(LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Verbose     => LogLevel.Verbose,
            LogEventLevel.Debug       => LogLevel.Debug,
            LogEventLevel.Information => LogLevel.Information,
            LogEventLevel.Warning     => LogLevel.Warning,
            LogEventLevel.Error       => LogLevel.Error,
            LogEventLevel.Fatal       => LogLevel.Fatal,
            _                         => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate)
    {
        Logging.Log.WriteLine(ConvertLogLevel(level), Logging.Log.FormatMessage(area, messageTemplate, []));
    }

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate, params object?[] propertyValues)
    {
        Logging.Log.WriteLine(ConvertLogLevel(level), Logging.Log.FormatMessage(area, messageTemplate, propertyValues));
    }
}
