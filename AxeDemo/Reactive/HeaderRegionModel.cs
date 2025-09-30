using Axe.Icons;
using Axe.Reactive.Regions;
using AxeDemo.Langs;

namespace AxeDemo.Reactive;

public abstract class HeaderRegionModel : RegionModel
{
    public abstract IconSource Icon { get; }
    public abstract LangKey Header { get; }
}
