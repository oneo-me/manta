using System.Text;

namespace Manta.Logging;

public sealed class FileLogger : ILogger, IDisposable
{
    readonly LogLevel _logLevel;
    readonly LogTimeType _timeType;
    readonly StreamWriter _writer;
    readonly Lock _lock = new();
    bool _disposed;

    public FileLogger(string file, LogTimeType timeType = LogTimeType.DateTime, LogLevel logLevel = LogLevel.Important)
    {
        _logLevel = logLevel;
        _timeType = timeType;

        var directory = Path.GetDirectoryName(file);
        if (string.IsNullOrWhiteSpace(directory) is false)
            Directory.CreateDirectory(directory);

        _writer = new(
            new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.Read),
            Encoding.UTF8)
        {
            AutoFlush = true
        };
    }

    public void WriteLine(DateTimeOffset time, LogLevel level, string messageTemplate, params object?[] propertyValues)
    {
        if ((level & _logLevel) == 0)
            return;

        var formatTime = Log.FormatTime(time, _timeType);
        var formatLevel = Log.FormatLevel(level);
        var message = Log.FormatMessage(null, messageTemplate, propertyValues);

        var line = string.IsNullOrWhiteSpace(formatTime)
            ? $"[{formatLevel}] {message}"
            : $"{formatTime} [{formatLevel}] {message}";

        lock (_lock)
        {
            if (_disposed)
                return;

            _writer.WriteLine(line);
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            if (_disposed)
                return;

            _disposed = true;
            _writer.Dispose();
        }
    }
}
