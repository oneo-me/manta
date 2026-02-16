using Manta.Reactive.Views;

namespace Manta.NodeEditor.Models;

public sealed class NodeConnection : Model
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required NodePort Source { get; init; }

    public required NodePort Target { get; init; }

    public bool IsSelected
    {
        get;
        set => SetValue(ref field, value);
    }

    public NodeBase? SourceNode => Source.Owner;

    public NodeBase? TargetNode => Target.Owner;
}
