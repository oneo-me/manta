using Avalonia;

namespace Manta.Blueprint.Models;

public sealed class Port
{
    public required Guid Id { get; init; }
    public required PortDefinition Definition { get; init; }
    public required Node Owner { get; init; }

    public object? Value { get; set; }

    public Point Position { get; set; }

    public string Name => Definition.Name;
    public string DisplayName => Definition.DisplayName;
    public Type DataType => Definition.DataType;
    public PortDirection Direction => Definition.Direction;
    public int Index => Definition.Index;

    public bool IsInput => Direction == PortDirection.Input;
    public bool IsOutput => Direction == PortDirection.Output;
}
