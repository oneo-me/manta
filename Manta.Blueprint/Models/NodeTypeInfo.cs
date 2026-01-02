namespace Manta.Blueprint.Models;

public sealed class NodeTypeInfo
{
    public required Type NodeType { get; init; }
    public required string DisplayName { get; init; }
    public required string Category { get; init; }
    public required List<PortDefinition> InputPortDefinitions { get; init; }
    public required List<PortDefinition> OutputPortDefinitions { get; init; }

    public string TypeFullName => NodeType.FullName ?? NodeType.Name;
}
