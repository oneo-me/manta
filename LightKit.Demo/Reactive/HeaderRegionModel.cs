using LightKit.Controls.Icons;
using LightKit.Reactive.Regions;
using LightKit.Demo.Langs;

namespace LightKit.Demo.Reactive;

abstract class HeaderRegionModel : RegionModel
{
    public abstract IconSource Icon { get; }
    public abstract LangKey Header { get; }
}
