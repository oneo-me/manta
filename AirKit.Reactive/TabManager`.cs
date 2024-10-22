using System.Diagnostics.CodeAnalysis;
using AirKit.Reactive.Interfaces;
using AirKit.Reactive.Logging;
using ObservableCollections;

namespace AirKit.Reactive;

public sealed class TabManager<T> : Model, IDisposable where T : TabModel
{
    readonly ObservableList<T> _source = [];

    public NotifyCollectionChangedSynchronizedViewList<T> Items { get; }

    T? _selectedItem;

    public T? SelectedItem
    {
        get => _selectedItem;
        set => SetValue(ref _selectedItem, value);
    }

    public TabManager()
    {
        Items = _source.ToNotifyCollectionChangedSlim(AvaloniaCollectionEventDispatcher.Current);
    }

    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    TTabModel? FindTarget<TTabModel, TContext>(TContext context) where TTabModel : T
    {
        lock (_source.SyncRoot)
        {
            var regionType = typeof(TTabModel);

            foreach (var item in _source)
            {
                if (item.GetType() != regionType)
                    continue;

                if (item is not ITabTarget<TContext> regionTarget)
                    continue;

                if (regionTarget.IsTarget(context) is false)
                    continue;

                return (TTabModel)item;
            }

            return null;
        }
    }

    public void Open<TTabModel>(Func<TTabModel> create, Func<TTabModel, bool>? isActive = null) where TTabModel : T
    {
        Open<TTabModel, object>(null!, _ => create(), isActive);
    }

    public void Open<TTabModel, TContext>(TContext context, Func<TContext, TTabModel> create, Func<TTabModel, bool>? isActive = null) where TTabModel : T
    {
        lock (_source.SyncRoot)
        {
            Log.Verbose("TabManager", "Open tab {TabModel} with {context}", typeof(TTabModel), context);

            var regionModel = FindTarget<TTabModel, TContext>(context);
            if (regionModel is not null)
            {
                if (isActive?.Invoke(regionModel) is not false)
                    SelectedItem = regionModel;

                return;
            }

            regionModel = create(context);
            regionModel.CloseFunc = x => InnerClose((T)x);

            _source.Add(regionModel);

            if (isActive?.Invoke(regionModel) != false)
                SelectedItem = regionModel;
            else if (SelectedItem is null)
                SelectedItem = regionModel;
        }
    }

    void InnerClose(T regionModel)
    {
        lock (_source.SyncRoot)
        {
            var index = _source.IndexOf(regionModel);
            _source.Remove(regionModel);

            if (SelectedItem != null && SelectedItem != regionModel)
                return;

            var newIndex = index == 0 ? 0 : index - 1;
            var selectedItem = _source.Count > 0 ? _source[newIndex] : null;

            AvaloniaDispatcher.Current.Post(() => SelectedItem = selectedItem);
        }
    }

    public void Dispose()
    {
        Log.Verbose("TabManager", "Dispose");

        lock (_source.SyncRoot)
        {
            for (var i = _source.Count - 1; i >= 0; i--)
                _source[i].Close();
        }

        Items.Dispose();
    }
}
