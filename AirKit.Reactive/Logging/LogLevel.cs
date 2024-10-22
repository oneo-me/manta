namespace AirKit.Reactive.Logging;

[Flags]
public enum LogLevel
{
    Verbose = 1 << 0,
    Debug = 1 << 1,
    Information = 1 << 2,
    Warning = 1 << 3,
    Error = 1 << 4,
    Fatal = 1 << 5,

    Important = Information | Warning | Error | Fatal,
    All = Verbose | Debug | Information | Warning | Error | Fatal
}
