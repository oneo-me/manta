using System.Globalization;
using AirKit.Reactive.Utilities;
using Avalonia.Utilities;

namespace AirKit.Reactive.Logging;

public abstract class LoggerBase(bool writeTime, bool writeLevel, LogLevel logLevel, params string[] areas)
{
    protected abstract void WriteLine(string message);

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

        WriteLine(Format(level, area, messageTemplate, propertyValues));
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
            _                    => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    string Format(LogLevel level, string area, string template, object?[] propertyValues)
    {
        var stringBuilder = StringBuilderCache.Acquire(template.Length);
        var characterReader = new CharacterReader(template.AsSpan());
        var index = 0;

        if (writeTime)
        {
            stringBuilder.Append(FormatTime(DateTimeOffset.Now));
            stringBuilder.Append(' ');
        }

        if (writeLevel)
        {
            stringBuilder.Append('[');
            stringBuilder.Append(FormatLevel(level));
            stringBuilder.Append(']');
            stringBuilder.Append(' ');
        }

        stringBuilder.Append('[');
        stringBuilder.Append(area);
        stringBuilder.Append(']');
        stringBuilder.Append(' ');
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
