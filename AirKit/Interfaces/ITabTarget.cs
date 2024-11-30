namespace AirKit.Interfaces;

public interface ITabTarget<in T>
{
    bool IsTarget(T context);
}
