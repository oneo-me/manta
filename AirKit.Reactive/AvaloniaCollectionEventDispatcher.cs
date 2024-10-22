using ObservableCollections;

namespace AirKit.Reactive;

public sealed class AvaloniaCollectionEventDispatcher : ICollectionEventDispatcher
{
    public static AvaloniaCollectionEventDispatcher Current { get; } = new();

    AvaloniaCollectionEventDispatcher()
    {
    }

    public void Post(CollectionEventDispatcherEventArgs ev)
    {
        AvaloniaDispatcher.Current.Post(ev.Invoke);
    }
}
