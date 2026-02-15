using System.ComponentModel;
using Avalonia.Media;

namespace Manta.MindMap.Models;

public sealed class MindMapEdge(MindMapNode source, MindMapNode target) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Id { get; } = Guid.NewGuid().ToString();

    public MindMapNode Source { get; } = source;

    public MindMapNode Target { get; } = target;

    public IBrush? Stroke
    {
        get;
        set
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(nameof(Stroke));
            }
        }
    }

    public double StrokeThickness
    {
        get;
        set
        {
            if (Math.Abs(field - value) > 0.001)
            {
                field = value;
                OnPropertyChanged(nameof(StrokeThickness));
            }
        }
    } = 2;

    public bool IsSelected
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }
}
