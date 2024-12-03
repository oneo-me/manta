using AirKit;
using AirKit.Attributes;
using AirKit.Interfaces;

namespace AirKitDemo.Samples.Reactive;

[View<RegionItemView>]
sealed class RegionItemView_Model : RegionModel, ITabTarget<int>
{
    public static RegionItemView_Model Design { get; } = new(0);
    public int Id { get; }
    public string Content { get; }

    public RegionItemView_Model(int id)
    {
        Id = id;
        Header = $"TabItem {id}";
        Content = $"Content {id}";
    }

    public bool IsTarget(int context)
    {
        return Id == context;
    }
}
