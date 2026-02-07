using Manta.Reactive.Views;

namespace Manta.NodeEditor.Core;

/// <summary>
/// Abstract base for all ports. Provides untyped access to port value and metadata.
/// Use <see cref="InputPort{T}"/> or <see cref="OutputPort{T}"/> for typed ports.
/// </summary>
public abstract class Port : Model
{
    public Guid Id { get; } = Guid.NewGuid();

    public string Name { get; }

    public string Label { get; }

    public Type ValueType { get; }

    public PortDirection Direction { get; }

    public NodeBase? Node { get; internal set; }

    bool _isConnected;
    public bool IsConnected
    {
        get => _isConnected;
        set => SetValue(ref _isConnected, value);
    }

    double _centerX;
    public double CenterX
    {
        get => _centerX;
        set => SetValue(ref _centerX, value);
    }

    double _centerY;
    public double CenterY
    {
        get => _centerY;
        set => SetValue(ref _centerY, value);
    }

    public abstract object? BoxedValue { get; set; }

    public bool IsInput => Direction == PortDirection.Input;
    public bool IsOutput => Direction == PortDirection.Output;

    protected Port(string name, string? label, Type valueType, PortDirection direction)
    {
        Name = name;
        Label = label ?? name;
        ValueType = valueType;
        Direction = direction;
    }
}
