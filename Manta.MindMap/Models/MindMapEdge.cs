using System.ComponentModel;
using Avalonia.Media;

namespace Manta.MindMap.Models;

public class MindMapEdge : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Id { get; } = Guid.NewGuid().ToString();

    public MindMapNode Source { get; }

    public MindMapNode Target { get; }

    private IBrush? _stroke;
    public IBrush? Stroke
    {
        get => _stroke;
        set
        {
            if (_stroke != value)
            {
                _stroke = value;
                OnPropertyChanged(nameof(Stroke));
            }
        }
    }

    private double _strokeThickness = 2;
    public double StrokeThickness
    {
        get => _strokeThickness;
        set
        {
            if (Math.Abs(_strokeThickness - value) > 0.001)
            {
                _strokeThickness = value;
                OnPropertyChanged(nameof(StrokeThickness));
            }
        }
    }

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected != value)
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public MindMapEdge(MindMapNode source, MindMapNode target)
    {
        Source = source;
        Target = target;
    }
}
