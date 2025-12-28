using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Manta.Reactive.Extensions;

public static class ObservableEvent
{
    public static IObservable<(object? sender, T args)> FromHandler<T>(Action<EventHandler<T>> addHandler, Action<EventHandler<T>> removeHandler) where T : EventArgs
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
