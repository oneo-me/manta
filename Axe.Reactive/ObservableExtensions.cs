using System.Reactive.Linq;

namespace Axe.Reactive;

public static class ObservableExtensions
{
    public static IObservable<T> ObserveOnDispatcher<T>(this IObservable<T> source)
    {
        return source.ObserveOn(AvaloniaScheduler.Instance);
    }
}
