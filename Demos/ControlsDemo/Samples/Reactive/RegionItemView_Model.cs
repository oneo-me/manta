using Axe.Reactive.Regions;
using Axe.Reactive.Views;

namespace ControlsDemo.Samples.Reactive;

[View<RegionItemView>]
sealed class RegionItemView_Model(int id) : RegionModel, IRegionTarget<int>
{
    public static RegionItemView_Model Design { get; } = new(0);
    public int Id { get; } = id;
    public string Content { get; } = $"Content {id}";

    public bool IsTarget(int context)
    {
        return Id == context;
    }
}
