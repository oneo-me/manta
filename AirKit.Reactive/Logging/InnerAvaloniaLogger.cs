using Avalonia.Logging;

namespace AirKit.Reactive.Logging;

sealed class InnerAvaloniaLogger : ILogSink
{
    public bool IsEnabled(LogEventLevel level, string area)
    {
        return level >= LogEventLevel.Warning;
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
        Logging.Log.WriteLine(ConvertLogLevel(level), area, messageTemplate);
    }

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate, params object?[] propertyValues)
    {
        Logging.Log.WriteLine(ConvertLogLevel(level), area, messageTemplate, propertyValues);
    }
}
