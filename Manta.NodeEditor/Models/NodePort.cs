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

    public object? Value
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool IsConnected
    {
        get;
        set => SetValue(ref field, value);
    }

    public double PortCenterX
    {
        get;
        set => SetValue(ref field, value);
    }

    public double PortCenterY
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool IsInput => Direction == PortDirection.Input;

    public bool IsOutput => Direction == PortDirection.Output;
}
