using Manta.Reactive.Regions;

namespace MantaSample.Reactive;

abstract class PageModel : RegionModel
{
    public abstract string Header { get; }
}
