using Axe.Reactive.Regions;
using AxeDemo.Controls;
using AxeDemo.Langs;

namespace AxeDemo.Reactive;

public abstract class HeaderRegionModel : RegionModel
{
    public abstract IconKey Icon { get; }
    public abstract LangKey Header { get; }
}
