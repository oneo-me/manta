using System.Diagnostics;
using Avalonia.Controls;
using ObservableCollections;

namespace AirKit.Reactive.Logging;

public static class Log
{
    static readonly ObservableList<LoggerBase> _loggers = [];

    static Log()
    {
        if (Design.IsDesignMode)
            AddLogger(new InnerConsoleLogger(false, true, LogLevel.All));
        else
            AddLogger(new InnerConsoleLogger(false, true, LogLevel.Important));
        if (Debugger.IsAttached)
            AddLogger(new InnerDebuggerLogger(false, true, LogLevel.All));
    }

    public static void AddLogger(LoggerBase logger)
    {
        _loggers.Add(logger);
    }

    public static void WriteLine(LogLevel level, string area, string messageTemplate, params object?[] propertyValues)
    {
        foreach (var logger in _loggers)
            logger.WriteLine(level, area, messageTemplate, propertyValues);
    }

    public static void Verbose(string area, string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Verbose, area, messageTemplate, propertyValues);
    }

    public static void Debug(string area, string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Debug, area, messageTemplate, propertyValues);
    }

    public static void Information(string area, string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Information, area, messageTemplate, propertyValues);
    }

    public static void Warning(string area, string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Warning, area, messageTemplate, propertyValues);
    }

    public static void Error(string area, string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Error, area, messageTemplate, propertyValues);
    }

    public static void Fatal(string area, string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Fatal, area, messageTemplate, propertyValues);
    }
}
