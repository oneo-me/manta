namespace Manta.NodeEditor.Core;

/// <summary>
/// A typed output port. Use as a property on <see cref="NodeBase"/> subclasses.
/// Example: <c>public OutputPort&lt;double&gt; Result { get; } = new("Result");</c>
/// </summary>
public sealed class OutputPort<T>(string name, string? label = null) : Port(name, label, typeof(T), PortDirection.Output)
{
    T? _value;

    public T? Value
    {
        get => _value;
        set
        {
            if (SetValue(ref _value, value))
                OnPropertyChanged(nameof(BoxedValue));
        }
    }

    public override object? BoxedValue
    {
        get => _value;
        set => Value = value is T typed ? typed : default;
    }
}
