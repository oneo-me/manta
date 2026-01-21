using Manta.Controls.Icons;
using Manta.Reactive.Regions;

namespace MantaDemo.Reactive;

abstract class HeaderRegionModel : RegionModel
{
    public abstract IconSource Icon { get; }
    public abstract string Header { get; }
}
