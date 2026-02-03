namespace Manta.NodeEditor.Models;

public sealed class PortDefinition
{
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public required Type DataType { get; init; }
    public required PortDirection Direction { get; init; }
    public required int Index { get; init; }
}
