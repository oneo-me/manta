using System.Collections.ObjectModel;
using Avalonia.Controls;
using Manta.Reactive.Views;

namespace Manta.NodeEditor.Models;

public abstract class NodeBase : Model
{
    public Guid Id { get; internal set; } = Guid.NewGuid();

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
