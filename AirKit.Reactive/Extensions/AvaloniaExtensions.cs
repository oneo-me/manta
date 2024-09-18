using Avalonia.Reactive;

namespace AirKit.Reactive.Extensions;

public static class AvaloniaExtensions
{
    public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext)
    {
        return observable.Subscribe(new AnonymousObserver<T>(onNext));
    }
}
