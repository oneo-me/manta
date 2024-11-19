using AirKit.Reactive.Attributes;
using AirKit.Reactive.Demo.Models;

namespace AirKit.Reactive.Demo.Samples;

[View<RegionManagerSample>]
sealed class RegionManagerSample_Model : SampleRegionModel
{
    public RegionManager<RegionItemView_Model> RegionManager { get; } = new();

    public int Index
    {
        get;
        set => SetValue(ref field, value);
    }

    public RegionManagerSample_Model()
    {
        Header = "TabManger";

        for (var i = 0; i < 3; i++)
            Add();
    }

    public void Add()
    {
        RegionManager.Open(Index++, x => new RegionItemView_Model(x), x => x.Id == 1);
    }

    protected override void OnClosed()
    {
        base.OnClosed();
        RegionManager.Dispose();
    }
}
