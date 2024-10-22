using ObservableCollections;

namespace AirKit.Reactive;

public sealed class DialogManager<T> : IDisposable where T : DialogModel
{
    readonly ObservableList<T> _source = [];

    public INotifyCollectionChangedSynchronizedViewList<T> Items { get; }

    public DialogManager()
    {
        Items = _source.ToNotifyCollectionChangedSlim(AvaloniaCollectionEventDispatcher.Current);
    }

    public async Task OpenAsync(T dialogModel)
    {
        var taskCompletionSource = new TaskCompletionSource();
        _source.Add(dialogModel);
        dialogModel.CloseFunc = x => InnerClose((T)x, taskCompletionSource);
        await taskCompletionSource.Task.ConfigureAwait(false);
    }

    void InnerClose(T dialogModel, TaskCompletionSource taskCompletionSource)
    {
        dialogModel.CloseFunc = null;
        _source.Remove(dialogModel);
        taskCompletionSource.SetResult();
    }

    public void Dispose()
    {
        lock (_source.SyncRoot)
        {
            for (var i = _source.Count - 1; i >= 0; i--)
                _source[i].Close();
        }

        Items.Dispose();
    }
}
