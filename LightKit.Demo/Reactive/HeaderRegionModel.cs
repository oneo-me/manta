using LightKit.Controls.Icons;
using LightKit.Reactive.Regions;

namespace LightKit.Demo.Reactive;

abstract class HeaderRegionModel : RegionModel
{
    public abstract IconSource Icon { get; }
    public abstract string Header { get; }
}
