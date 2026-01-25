namespace Manta.Reactive.Views;

public sealed class ReactiveValue<T>(T value) : Model
{
    public T Value
    {
        get;
        set => SetValue(ref field, value);
    } = value;
}
