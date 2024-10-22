using System.Diagnostics;
using System.Globalization;
using AirKit.Reactive.Interfaces;
using AirKit.Reactive.Utilities;
using Avalonia.Controls;
using Avalonia.Utilities;
using ObservableCollections;

namespace AirKit.Reactive.Logging;

public static class Log
{
    static readonly ObservableList<ILogger> _loggers = [];

    static Log()
    {
        if (Design.IsDesignMode)
            AddLogger(new InnerConsoleLogger(false, true, LogLevel.All));
        else
            AddLogger(new InnerConsoleLogger(false, true, LogLevel.Important));
        if (Debugger.IsAttached)
            AddLogger(new InnerDebuggerLogger(false, true, Environment.GetCommandLineArgs().Contains("--debug") ? LogLevel.All : LogLevel.Important));
    }

    public static void AddLogger(ILogger logger)
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

    static string FormatTime(DateTimeOffset time)
    {
        return time.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
    }

    static char FormatLevel(LogLevel level)
    {
        return level switch
        {
            LogLevel.Verbose     => 'V',
            LogLevel.Debug       => 'D',
            LogLevel.Information => 'I',
            LogLevel.Warning     => 'W',
            LogLevel.Error       => 'E',
            LogLevel.Fatal       => 'F',

            LogLevel.Important => throw new ArgumentOutOfRangeException(nameof(level), level, null),
            LogLevel.All       => throw new ArgumentOutOfRangeException(nameof(level), level, null),
            _                  => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    public static string FormatMessage(string template, object?[] propertyValues, DateTimeOffset? time = null, LogLevel? level = null, string? area = null)
    {
        var stringBuilder = StringBuilderCache.Acquire(template.Length);
        var characterReader = new CharacterReader(template.AsSpan());
        var index = 0;

        if (time.HasValue)
        {
            stringBuilder.Append(FormatTime(time.Value));
            stringBuilder.Append(' ');
        }

        if (level.HasValue)
        {
            stringBuilder.Append('[');
            stringBuilder.Append(FormatLevel(level.Value));
            stringBuilder.Append(']');
            stringBuilder.Append(' ');
        }

        if (area is not null)
        {
            stringBuilder.Append('[');
            stringBuilder.Append(area);
            stringBuilder.Append(']');
            stringBuilder.Append(' ');
        }

        while (!characterReader.End)
        {
            var ch = characterReader.Take();
            if (ch != '{')
            {
                stringBuilder.Append(ch);
            }
            else if (characterReader.Peek != '{')
            {
                stringBuilder.Append('\'');
                stringBuilder.Append(index < propertyValues.Length ? propertyValues[index++] : null);
                stringBuilder.Append('\'');
                characterReader.TakeUntil('}');
                characterReader.Take();
            }
            else
            {
                stringBuilder.Append('{');
            }
        }

        return StringBuilderCache.GetStringAndRelease(stringBuilder);
    }
}
