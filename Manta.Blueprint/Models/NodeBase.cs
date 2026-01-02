using Avalonia;

namespace Manta.Blueprint.Models;

public abstract class NodeBase
{
    public Guid Id { get; internal set; } = Guid.NewGuid();
    public Point Position { get; set; }
    public bool IsSelected { get; set; }

    internal List<Port> InputPorts { get; } = [];
    internal List<Port> OutputPorts { get; } = [];

    public IReadOnlyList<Port> Inputs => InputPorts;
    public IReadOnlyList<Port> Outputs => OutputPorts;

    public virtual string DisplayName => GetType().Name;

    public virtual Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    protected T? GetInput<T>(string portName)
    {
        var port = InputPorts.Find(p => p.Name == portName);
        if (port?.Value is T value)
            return value;
        return default;
    }

    protected void SetOutput<T>(string portName, T value)
    {
        var port = OutputPorts.Find(p => p.Name == portName);
        if (port != null)
            port.Value = value;
    }
}
