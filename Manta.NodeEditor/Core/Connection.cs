using Manta.Reactive.Views;

namespace Manta.NodeEditor.Core;

/// <summary>
/// A connection between an output port and an input port.
/// </summary>
public sealed class Connection(Port source, Port target) : Model
{
    public Guid Id { get; } = Guid.NewGuid();

    public Port Source { get; } = source;

    public Port Target { get; } = target;

    public bool IsSelected
    {
        get;
        set => SetValue(ref field, value);
    }
}
