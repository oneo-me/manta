namespace AirKit.Logging;

sealed class InnerConsoleLogger(bool writeTime, bool writeLevel, LogLevel level, params string[] areas) : LoggerBase(writeTime, writeLevel, level, areas)
{
    protected override void WriteLine(string message)
    {
        Console.WriteLine(message);
    }
}
