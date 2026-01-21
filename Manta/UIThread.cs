using System.Diagnostics.CodeAnalysis;
using Avalonia.Threading;

namespace Manta;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class UIThread
{
    public static bool CheckAccess()
    {
        return Dispatcher.UIThread.CheckAccess();
    }

    public static void Post(Action action)
    {
        Dispatcher.UIThread.Post(action);
    }

    public static void Post(Action action, DispatcherPriority priority)
    {
        Dispatcher.UIThread.Post(action, priority);
    }

    public static void Invoke(Action action)
    {
        Dispatcher.UIThread.Invoke(action);
    }

    public static void Invoke(Action action, DispatcherPriority priority)
    {
        Dispatcher.UIThread.Invoke(action, priority);
    }

    public static void Invoke(Action action, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        Dispatcher.UIThread.Invoke(action, priority, cancellationToken);
    }

    public static void Invoke(Action action, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
    {
        Dispatcher.UIThread.Invoke(action, priority, cancellationToken, timeout);
    }

    public static T Invoke<T>(Func<T> func)
    {
        return Dispatcher.UIThread.Invoke(func);
    }

    public static T Invoke<T>(Func<T> func, DispatcherPriority priority)
    {
        return Dispatcher.UIThread.Invoke(func, priority);
    }

    public static T Invoke<T>(Func<T> func, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        return Dispatcher.UIThread.Invoke(func, priority, cancellationToken);
    }

    public static T Invoke<T>(Func<T> func, DispatcherPriority priority, CancellationToken cancellationToken, TimeSpan timeout)
    {
        return Dispatcher.UIThread.Invoke(func, priority, cancellationToken, timeout);
    }

    public static DispatcherOperation InvokeAsync(Action action)
    {
        return Dispatcher.UIThread.InvokeAsync(action);
    }

    public static DispatcherOperation InvokeAsync(Action action, DispatcherPriority priority)
    {
        return Dispatcher.UIThread.InvokeAsync(action, priority);
    }

    public static DispatcherOperation InvokeAsync(Action action, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        return Dispatcher.UIThread.InvokeAsync(action, priority, cancellationToken);
    }

    public static DispatcherOperation<T> InvokeAsync<T>(Func<T> func)
    {
        return Dispatcher.UIThread.InvokeAsync(func);
    }

    public static DispatcherOperation<T> InvokeAsync<T>(Func<T> func, DispatcherPriority priority)
    {
        return Dispatcher.UIThread.InvokeAsync(func, priority);
    }

    public static DispatcherOperation<T> InvokeAsync<T>(Func<T> func, DispatcherPriority priority, CancellationToken cancellationToken)
    {
        return Dispatcher.UIThread.InvokeAsync(func, priority, cancellationToken);
    }

    public static Task InvokeAsync(Func<Task> func)
    {
        return Dispatcher.UIThread.InvokeAsync(func);
    }

    public static Task InvokeAsync(Func<Task> func, DispatcherPriority priority)
    {
        return Dispatcher.UIThread.InvokeAsync(func, priority);
    }

    public static Task<T> InvokeAsync<T>(Func<Task<T>> func)
    {
        return Dispatcher.UIThread.InvokeAsync(func);
    }

    public static Task<T> InvokeAsync<T>(Func<Task<T>> func, DispatcherPriority priority)
    {
        return Dispatcher.UIThread.InvokeAsync(func, priority);
    }
}
