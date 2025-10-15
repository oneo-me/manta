using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Avalonia.Threading;

namespace LightKit.Reactive.Extensions;

public sealed class AvaloniaScheduler : LocalScheduler
{
    const int MaxReentrantSchedules = 32;

    int _reentrancyGuard;

    public static readonly AvaloniaScheduler Instance = new();

    AvaloniaScheduler()
    {
    }

    CompositeDisposable PostOnDispatcher<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
    {
        var composite = new CompositeDisposable(2);
        var cancellation = new CancellationDisposable();

        Dispatcher.UIThread.Post(() =>
        {
            if (cancellation.Token.IsCancellationRequested is false)
                composite.Add(action(this, state));
        }, DispatcherPriority.Background);

        composite.Add(cancellation);

        return composite;
    }

    public override IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
    {
        if (dueTime == TimeSpan.Zero)
        {
            if (Dispatcher.UIThread.CheckAccess() is false)
                return PostOnDispatcher(state, action);

            if (_reentrancyGuard >= MaxReentrantSchedules)
                return PostOnDispatcher(state, action);

            try
            {
                _reentrancyGuard++;
                return action(this, state);
            }
            finally
            {
                _reentrancyGuard--;
            }
        }

        var composite = new CompositeDisposable(2);
        composite.Add(DispatcherTimer.RunOnce(() => composite.Add(action(this, state)), dueTime));
        return composite;
    }
}
