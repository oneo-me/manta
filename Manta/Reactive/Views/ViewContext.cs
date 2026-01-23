namespace Manta.Reactive.Views;

interface IViewContext
{
    string Key { get; }
    object? Value { get; set; }
    Type? Source { get; set; }
}

public sealed class ViewContext<T> : Model, IViewContext
{
    object? IViewContext.Value
    {
        get => Value!;
        set
        {
            switch (value)
            {
                case T typedData:
                    Value = typedData;
                    break;

                case null when default(T) is null:
                    break;

                default:
                    throw new ArgumentException($"Expected {typeof(T).Name}, got {value?.GetType().Name ?? "null"}");
            }
        }
    }

    public string Key { get; }

    internal ViewContext(string key)
    {
        Key = key;
    }

    public T? Value
    {
        get;
        set => SetValue(ref field, value);
    }

    public Type? Source
    {
        get;
        set => SetValue(ref field, value);
    }
}
