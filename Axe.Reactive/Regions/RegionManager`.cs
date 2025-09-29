using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using Axe.Reactive.Extensions;
using Axe.Reactive.Views;
using DynamicData;

namespace Axe.Reactive.Regions;

public sealed class RegionManager<T> : Model, IDisposable where T : RegionModel
{
    readonly Lock _lock = new();
    readonly SourceList<T> _source = new();

    public ReadOnlyObservableCollection<T> Items { get; }

    public T? SelectedItem
    {
        get;
        set => SetValue(ref field, value);
    }

    public RegionManager(params T[] initItems)
    {
        _source.AddRange(initItems);

        _source.Connect()
            .Synchronize()
            .ObserveOnDispatcher()
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

            foreach (var item in _source.Items)
            {
                if (item.GetType() != regionType)
                    continue;

                if (item is not IRegionTarget<TContext> regionTarget)
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
            var index = _source.Items.IndexOf(regionModel);
            _source.Remove(regionModel);

            if (SelectedItem != null && SelectedItem != regionModel)
                return;

            var newIndex = index == 0 ? 0 : index - 1;
            SelectedItem = _source.Count > 0 ? _source.Items[newIndex] : null;
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            for (var i = _source.Count - 1; i >= 0; i--)
                _source.Items[i].Close();

            _source.Dispose();
        }
    }
}
