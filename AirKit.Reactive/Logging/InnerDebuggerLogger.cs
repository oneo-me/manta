using System.Diagnostics;

namespace AirKit.Reactive.Logging;

sealed class InnerDebuggerLogger(bool writeTime, bool writeLevel, LogLevel level, params string[] areas) : LoggerBase(writeTime, writeLevel, level, areas)
{
    protected override void WriteLine(string message)
    {
        Debugger.Log(0, null, message);
        Debugger.Log(0, null, Environment.NewLine);
    }
}
