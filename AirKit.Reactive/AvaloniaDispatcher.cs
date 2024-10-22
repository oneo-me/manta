using Avalonia.Threading;

namespace AirKit.Reactive;

public sealed class AvaloniaDispatcher
{
    public static AvaloniaDispatcher Current { get; } = new();

    readonly Dispatcher _dispatcher;

    AvaloniaDispatcher()
    {
        _dispatcher = Dispatcher.UIThread;
    }

    public void Invoke(Action action)
    {
        _dispatcher.Invoke(action);
    }

    public DispatcherOperation InvokeAsync(Action action)
    {
        return _dispatcher.InvokeAsync(action);
    }

    public void Post(Action action)
    {
        _dispatcher.Post(action);
    }
}
