namespace Manta.Reactive.Views;

public sealed class ReactiveValue<T>(T value) : Model, IReactiveValue
{
    public T Value
    {
        get;
        set => SetField(ref field, value);
    } = value;

    object? IReactiveValue.GetValue()
    {
        return Value;
    }
}
