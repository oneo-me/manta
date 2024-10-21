using AirKit.Reactive.Attributes;
using AirKit.Reactive.Interfaces;

namespace AirKit.Reactive.Demo.Samples;

[View<TabItemView>]
sealed class TabItemView_Model : TabModel, ITabTarget<int>
{
    public static TabItemView_Model Design { get; } = new(0);
    public int Id { get; }
    public string Content { get; }

    public TabItemView_Model(int id)
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
