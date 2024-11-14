using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using AirKit.Reactive.Interfaces;
using AirKit.Reactive.Logging;
using DynamicData;

namespace AirKit.Reactive;

public sealed class TabManager<T> : Model, IDisposable where T : TabModel
{
    readonly object _lock = new();
    readonly SourceList<T> _source = new();

    public ReadOnlyObservableCollection<T> Items { get; }

    T? _selectedItem;

    public T? SelectedItem
    {
        get => _selectedItem;
        set => SetValue(ref _selectedItem, value);
    }

    public TabManager()
    {
        _source.Connect()
            .Synchronize()
            .ObserveOn(AvaloniaScheduler.Instance)
            .Bind(out var items)
            .Subscribe();
        Items = items;
    }

    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    TTabModel? FindTarget<TTabModel, TContext>(TContext context) where TTabModel : T
    {
        lock (_lock)
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
        lock (_lock)
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
        lock (_lock)
        {
            var index = _source.IndexOf(regionModel);
            _source.Remove(regionModel);

            if (SelectedItem != null && SelectedItem != regionModel)
                return;

            var newIndex = index == 0 ? 0 : index - 1;
            SelectedItem = _source.Count > 0 ? _source[newIndex] : null;
        }
    }

    public void Dispose()
    {
        Log.Verbose("TabManager", "Dispose");

        lock (_lock)
        {
            for (var i = _source.Count - 1; i >= 0; i--)
                _source[i].Close();
        }

        Items.Dispose();
    }
}
