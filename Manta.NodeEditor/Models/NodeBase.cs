using System.Collections.ObjectModel;
using Avalonia.Controls;
using Manta.Reactive.Views;

namespace Manta.NodeEditor.Models;

public abstract class NodeBase : Model
{
    public Guid Id { get; internal set; } = Guid.NewGuid();

    public double X
    {
        get;
        set => SetValue(ref field, value);
    }

    public double Y
    {
        get;
        set => SetValue(ref field, value);
    }

    public bool IsSelected
    {
        get;
        set => SetValue(ref field, value);
    }

    public virtual string DisplayName => GetType().Name;

    public string Category { get; internal set; } = "Default";

    public ObservableCollection<NodePort> Inputs { get; } = [];

    public NodePort? Output { get; internal set; }

    public virtual Task ExecuteAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    protected T? GetInputValue<T>(string portName)
    {
        var port = Inputs.FirstOrDefault(p => p.Name == portName);
        return port?.Value is T value ? value : default;
    }

    protected void SetOutputValue(object? value)
    {
        if (Output is not null)
            Output.Value = value;
    }

    public virtual Control? CreateContentView() => null;
}
