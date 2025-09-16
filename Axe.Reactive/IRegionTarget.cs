namespace Axe.Reactive;

public interface IRegionTarget<in T>
{
    bool IsTarget(T context);
}
