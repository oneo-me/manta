using System.Collections.ObjectModel;
using System.Reflection;
using Avalonia.Controls;
using Manta.Reactive.Views;

namespace Manta.NodeEditor.Core;

/// <summary>
/// Base class for all nodes. Ports are defined as typed properties
/// (<see cref="InputPort{T}"/>, <see cref="OutputPort{T}"/>) and auto-discovered.
/// </summary>
public abstract class NodeBase : Model
{
    public Guid Id { get; } = Guid.NewGuid();

    double _x;
    public double X
    {
        get => _x;
        set => SetValue(ref _x, value);
    }

    double _y;
    public double Y
    {
        get => _y;
        set => SetValue(ref _y, value);
    }

    bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set => SetValue(ref _isSelected, value);
    }

    public abstract string Title { get; }

    public virtual string Category => "Default";

    ReadOnlyCollection<Port>? _inputs;
    Port? _output;
    bool _portsDiscovered;

    public ReadOnlyCollection<Port> Inputs
    {
        get
        {
            EnsurePortsDiscovered();
            return _inputs!;
        }
    }

    public Port? Output
    {
        get
        {
            EnsurePortsDiscovered();
            return _output;
        }
    }

    void EnsurePortsDiscovered()
    {
        if (_portsDiscovered) return;
        _portsDiscovered = true;

        var inputs = new List<Port>();
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in properties)
        {
            if (!typeof(Port).IsAssignableFrom(prop.PropertyType)) continue;

            var port = prop.GetValue(this) as Port;
            if (port is null) continue;

            port.Node = this;

            if (port.IsInput)
                inputs.Add(port);
            else if (port.IsOutput)
                _output = port;
        }

        // Sort by declaration order (property metadata token)
        inputs.Sort((a, b) =>
        {
            var propA = GetType().GetProperties().First(p => p.GetValue(this) == a);
            var propB = GetType().GetProperties().First(p => p.GetValue(this) == b);
            return propA.MetadataToken.CompareTo(propB.MetadataToken);
        });

        _inputs = inputs.AsReadOnly();
    }

    /// <summary>
    /// Override to provide custom content for the node body.
    /// Return null to use the default (ports-only) layout.
    /// </summary>
    public virtual Control? BuildContent() => null;

    /// <summary>
    /// Override to execute the node's logic.
    /// Read values from input ports, compute, and write to output port.
    /// </summary>
    public virtual Task ExecuteAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
