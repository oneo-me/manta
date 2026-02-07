using Manta.Reactive.Views;

namespace Manta.NodeEditor.Models;

public sealed class NodePort : Model
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Name { get; init; }

    public required string DisplayName { get; init; }

    public required Type DataType { get; init; }

    public required PortDirection Direction { get; init; }

    public required int Order { get; init; }

    public NodeBase? Owner { get; internal set; }

    object? _value;
    public object? Value
    {
        get => _value;
        set => SetValue(ref _value, value);
    }

    bool _isConnected;
    public bool IsConnected
    {
        get => _isConnected;
        set => SetValue(ref _isConnected, value);
    }

    double _portCenterX;
    public double PortCenterX
    {
        get => _portCenterX;
        set => SetValue(ref _portCenterX, value);
    }

    double _portCenterY;
    public double PortCenterY
    {
        get => _portCenterY;
        set => SetValue(ref _portCenterY, value);
    }

    public bool IsInput => Direction == PortDirection.Input;

    public bool IsOutput => Direction == PortDirection.Output;
}
