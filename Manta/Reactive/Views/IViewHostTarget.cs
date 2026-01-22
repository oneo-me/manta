namespace Manta.Reactive.Views;

public interface IViewHostTarget
{
    void SetData(object? data);
}

public interface IViewHostTarget<in T> : IViewHostTarget
{
    void SetData(T data);

    void IViewHostTarget.SetData(object? data)
    {
        switch (data)
        {
            case T typedData:
                SetData(typedData);
                break;

            case null when default(T) is null:
                break;

            default:
                throw new ArgumentException($"Expected {typeof(T).Name}, got {data?.GetType().Name ?? "null"}");
        }
    }
}
