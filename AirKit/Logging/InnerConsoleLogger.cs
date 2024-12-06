namespace AirKit.Logging;

sealed class InnerConsoleLogger(bool writeTime, LogLevel logLevel, params string[] areas) : LoggerBase(writeTime, false, false, logLevel, areas)
{
    static readonly ConsoleColor _foregroundColor = Console.ForegroundColor;

    protected override void WriteLine(LogLevel level, string message)
    {
        switch (level)
        {
            case LogLevel.Verbose:
                Console.ForegroundColor = ConsoleColor.Gray;
                break;

            case LogLevel.Debug:
                Console.ForegroundColor = ConsoleColor.Cyan;
                break;

            case LogLevel.Information:
                Console.ForegroundColor = _foregroundColor;
                break;

            case LogLevel.Warning:
                Console.ForegroundColor = ConsoleColor.Yellow;
                break;

            case LogLevel.Error:
                Console.ForegroundColor = ConsoleColor.Red;
                break;

            case LogLevel.Fatal:
                Console.ForegroundColor = ConsoleColor.DarkRed;
                break;

            case LogLevel.Important:
            case LogLevel.All:
            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }

        Console.WriteLine(message);
    }
}
