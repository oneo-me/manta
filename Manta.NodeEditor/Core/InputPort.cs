namespace Manta.NodeEditor.Core;

/// <summary>
/// A typed input port. Use as a property on <see cref="NodeBase"/> subclasses.
/// Example: <c>public InputPort&lt;double&gt; A { get; } = new("A");</c>
/// </summary>
public sealed class InputPort<T>(string name, string? label = null) : Port(name, label, typeof(T), PortDirection.Input)
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
