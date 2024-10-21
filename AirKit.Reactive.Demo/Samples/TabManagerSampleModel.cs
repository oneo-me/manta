using AirKit.Reactive.Attributes;
using AirKit.Reactive.Demo.Models;

namespace AirKit.Reactive.Demo.Samples;

[View<TabManagerSample>]
sealed class TabManagerSampleModel : SampleTabModel
{
    public TabManager<TabItemView_Model> TabManager { get; } = new();

    int _index;

    public int Index
    {
        get => _index;
        set => SetValue(ref _index, value);
    }

    public TabManagerSampleModel()
    {
        Header = "TabManger";

        for (var i = 0; i < 3; i++)
            Add();
    }

    public void Add()
    {
        TabManager.Open(Index++, x => new TabItemView_Model(x), x => x.Id == 1);
    }

    protected override void OnClosed()
    {
        base.OnClosed();
        TabManager.Dispose();
    }
}
