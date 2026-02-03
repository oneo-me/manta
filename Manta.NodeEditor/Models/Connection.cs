namespace Manta.NodeEditor.Models;

public sealed class Connection
{
    public required Guid Id { get; init; }
    public required Port SourcePort { get; init; }
    public required Port TargetPort { get; init; }

    public Node SourceNode => SourcePort.Owner;
    public Node TargetNode => TargetPort.Owner;

    public bool IsSelected { get; set; }
}
