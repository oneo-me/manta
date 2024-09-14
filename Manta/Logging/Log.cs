using System.Globalization;
using Manta.Logging.Utilities;

namespace Manta.Logging;

public static class Log
{
    static readonly List<ILogger> _loggers = [];
    static readonly Lock _lock = new();

    public static void AddLogger(ILogger logger)
    {
        lock (_lock)
        {
            _loggers.Add(logger);
        }
    }

    public static void WriteLine(LogLevel level, string messageTemplate, params object?[] propertyValues)
    {
        var time = DateTimeOffset.Now;

        lock (_lock)
        {
            foreach (var logger in _loggers)
                logger.WriteLine(time, level, messageTemplate, propertyValues);
        }
    }

    public static void Verbose(string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Verbose, messageTemplate, propertyValues);
    }

    public static void Debug(string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Debug, messageTemplate, propertyValues);
    }

    public static void Information(string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Information, messageTemplate, propertyValues);
    }

    public static void Warning(string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Warning, messageTemplate, propertyValues);
    }

    public static void Error(string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Error, messageTemplate, propertyValues);
    }

    public static void Fatal(string messageTemplate, params object?[] propertyValues)
    {
        WriteLine(LogLevel.Fatal, messageTemplate, propertyValues);
    }

    public static string FormatTime(DateTimeOffset time, LogTimeType timeType)
    {
        switch (timeType)
        {
            case LogTimeType.None:
                return string.Empty;

            case LogTimeType.Time:
                return time.ToString("HH:mm:ss.fff", CultureInfo.InvariantCulture);

            case LogTimeType.DateTime:
                return time.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

            default:
                throw new ArgumentOutOfRangeException(nameof(timeType), timeType, null);
        }
    }

    public static char FormatLevel(LogLevel level)
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

    public static string FormatMessage(string? area, string template, object?[] propertyValues)
    {
        var stringBuilder = StringBuilderCache.Acquire(template.Length);
        var characterReader = new CharacterReader(template.AsSpan());
        var index = 0;

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
