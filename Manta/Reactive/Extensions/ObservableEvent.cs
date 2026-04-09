using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Manta.Reactive.Extensions;

public static class ObservableEvent
{
    public static IObservable<(object? sender, T args)> FromHandler<T>(Action<EventHandler<T>> addHandler, Action<EventHandler<T>> removeHandler) where T : EventArgs
    {
        return Observable.Create<(object? sender, T args)>(observer =>
        {
            EventHandler<T> handler = (sender, args) =>
            {
                observer.OnNext((sender, args));
            };

            addHandler(handler);

            return Disposable.Create(() =>
            {
                removeHandler(handler);
            });
        });
    }
}
