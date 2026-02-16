using Manta.Reactive.Views;

namespace Manta.NodeEditor.Core;

/// <summary>
/// Abstract base for all ports. Provides untyped access to port value and metadata.
/// Use <see cref="InputPort{T}"/> or <see cref="OutputPort{T}"/> for typed ports.
/// </summary>
public abstract class Port(string name, string? label, Type valueType, PortDirection direction)
    : Model
{
    public Guid Id { get; } = Guid.NewGuid();

    public string Name { get; } = name;

    public string Label { get; } = label ?? name;

    public Type ValueType { get; } = valueType;

    public PortDirection Direction { get; } = direction;

    public NodeBase? Node { get; internal set; }

    public bool IsConnected
    {
        get;
        set => SetValue(ref field, value);
    }

    public double CenterX
    {
        get;
        set => SetValue(ref field, value);
    }

    public double CenterY
    {
        get;
        set => SetValue(ref field, value);
    }

    public abstract object? BoxedValue { get; set; }

    public bool IsInput => Direction == PortDirection.Input;
    public bool IsOutput => Direction == PortDirection.Output;
}
