using Avalonia;

namespace Manta.Blueprint.Models;

public sealed class Node
{
    public required Guid Id { get; init; }
    public required NodeBase Instance { get; init; }
    public required NodeTypeInfo TypeInfo { get; init; }
    public required List<Port> InputPorts { get; init; }
    public required List<Port> OutputPorts { get; init; }

    public string DisplayName => Instance.DisplayName;

    public bool IsSelected
    {
        get => Instance.IsSelected;
        set => Instance.IsSelected = value;
    }

    public Point Position
    {
        get => Instance.Position;
        set => Instance.Position = value;
    }

    public IEnumerable<Port> AllPorts => InputPorts.Concat(OutputPorts);
}
