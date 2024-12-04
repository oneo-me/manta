using AirKit;
using AirKit.Attributes;
using AirKitDemo.Models;

namespace AirKitDemo.Samples.Reactive;

[Sample(SampleGroupType.Reactive, "TabManger")]
[View<RegionManagerSample>]
sealed class RegionManagerSample_Model : ViewModel, IDisposable
{
    public RegionManager<RegionItemView_Model> RegionManager { get; } = new();

    public int Index
    {
        get;
        set => SetValue(ref field, value);
    }

    public RegionManagerSample_Model()
    {
        for (var i = 0; i < 3; i++)
            Add();
    }

    public void Add()
    {
        RegionManager.Open(Index++, x => new RegionItemView_Model(x), x => x.Id == 1);
    }

    public void Dispose()
    {
        RegionManager.Dispose();
    }
}
