using System.Reactive.Linq;

namespace AirKit.Extensions;

public static class ObservableExtensions
{
    public static IObservable<T> ObserveOnDispatcher<T>(this IObservable<T> source)
    {
        return source.ObserveOn(AvaloniaScheduler.Instance);
    }
}
