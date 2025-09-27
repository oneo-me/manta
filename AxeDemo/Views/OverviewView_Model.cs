using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Axe.Reactive;
using AxeDemo.Langs;
using AxeDemo.Reactive;
using DynamicData;
using DynamicData.Binding;

namespace AxeDemo.Views;

[View<OverviewView>]
sealed class OverviewView_Model : HeaderRegionModel
{
    public static OverviewView_Model Design => new(new(new ControlsView_Model(), new MultiLanguageView_Model(), new StylesView_Model(), new ReactiveView_Model()));

    readonly RegionManager<HeaderRegionModel> _regionManager;

    public override LangKey Header => LangKey.Overview;
    public ReadOnlyObservableCollection<HeaderRegionModel> Items { get; }

    public OverviewView_Model(RegionManager<HeaderRegionModel> regionManager)
    {
        _regionManager = regionManager;
        _regionManager.Items
            .ToObservableChangeSet()
            .Synchronize()
            .ObserveOnDispatcher()
            .Filter(x => x.Header != LangKey.Overview)
            .Bind(out var items)
            .Subscribe();

        Items = items;
    }

    public void OpenRegion(HeaderRegionModel regionModel)
    {
        _regionManager.SelectedItem = regionModel;
    }
}
