namespace Manta.Reactive.Regions;

public interface IRegionTarget<in T>
{
    bool IsTarget(T context);
}
