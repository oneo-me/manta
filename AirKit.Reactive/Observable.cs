using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace AirKit.Reactive;

public static class Observable
{
    [SuppressMessage("Reliability", "CA2000:丢失范围之前释放对象")]
    public static IObservable<(object? sender, T args)> FromEventHandler<T>(Action<EventHandler<T>> addHandler, Action<EventHandler<T>> removeHandler) where T : EventArgs
    {
        var subject = new Subject<(object? sender, T args)>();
        var handler = new EventHandler<T>((sender, args) => subject.OnNext((sender, args)));

        addHandler(handler);

        return subject.Finally(() =>
        {
            removeHandler(handler);
            subject.Dispose();
        });
    }
}
