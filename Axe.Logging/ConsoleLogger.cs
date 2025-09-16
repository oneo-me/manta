namespace Axe.Logging;

public sealed class ConsoleLogger(LogTimeType timeType, LogLevel logLevel) : ILogger
{
    static readonly ConsoleColor _foregroundColor = Console.ForegroundColor;

    public void WriteLine(DateTimeOffset time, LogLevel level, string messageTemplate, params object?[] propertyValues)
    {
        if ((level & logLevel) == 0)
            return;

        var formatTime = Log.FormatTime(time, timeType);
        if (string.IsNullOrWhiteSpace(formatTime) is false)
        {
            Console.ForegroundColor = LogLevel2Color(LogLevel.Verbose);
            Console.Write(formatTime);
            Console.Write(' ');
        }

        var color = LogLevel2Color(level);

        var formatLevel = Log.FormatLevel(level);
        Console.ForegroundColor = LogLevel2Color(LogLevel.Information);
        Console.Write('[');
        Console.ForegroundColor = color;
        Console.Write(formatLevel);
        Console.ForegroundColor = LogLevel2Color(LogLevel.Information);
        Console.Write(']');
        Console.Write(' ');

        Console.ForegroundColor = color;
        Console.WriteLine(Log.FormatMessage(null, messageTemplate, propertyValues));
    }

    static ConsoleColor LogLevel2Color(LogLevel level)
    {
        switch (level)
        {
            case LogLevel.Verbose:
                return ConsoleColor.DarkGray;

            case LogLevel.Debug:
                return ConsoleColor.Blue;

            case LogLevel.Information:
                return _foregroundColor;

            case LogLevel.Warning:
                return ConsoleColor.Yellow;

            case LogLevel.Error:
                return ConsoleColor.DarkRed;

            case LogLevel.Fatal:
                return ConsoleColor.Red;

            case LogLevel.Important:
            case LogLevel.All:
            default:
                break;
        }

        throw new ArgumentOutOfRangeException(nameof(level), level, null);
    }
}
