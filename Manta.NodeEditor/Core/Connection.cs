using Manta.Reactive.Views;

namespace Manta.NodeEditor.Core;

/// <summary>
/// A connection between an output port and an input port.
/// </summary>
public sealed class Connection : Model
{
    public Guid Id { get; } = Guid.NewGuid();

    public Port Source { get; }

    public Port Target { get; }

    bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetValue(ref _isSelected, value);
    }

    public Connection(Port source, Port target)
    {
        Source = source;
        Target = target;
    }
}
