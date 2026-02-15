using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Media;

namespace Manta.MindMap.Models;

public class MindMapNode : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Id { get; } = Guid.NewGuid().ToString();

    private string _text = string.Empty;
    public string Text
    {
        get => _text;
        set
        {
            if (_text != value)
            {
                _text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
    }

    private double _x;
    public double X
    {
        get => _x;
        set
        {
            if (Math.Abs(_x - value) > 0.001)
            {
                _x = value;
                OnPropertyChanged(nameof(X));
            }
        }
    }

    private double _y;
    public double Y
    {
        get => _y;
        set
        {
            if (Math.Abs(_y - value) > 0.001)
            {
                _y = value;
                OnPropertyChanged(nameof(Y));
            }
        }
    }

    private double _width = 120;
    public double Width
    {
        get => _width;
        set
        {
            if (Math.Abs(_width - value) > 0.001)
            {
                _width = value;
                OnPropertyChanged(nameof(Width));
            }
        }
    }

    private double _height = 40;
    public double Height
    {
        get => _height;
        set
        {
            if (Math.Abs(_height - value) > 0.001)
            {
                _height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
    }

    private bool _isExpanded = true;
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded != value)
            {
                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
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

    private bool _isRoot;
    public bool IsRoot
    {
        get => _isRoot;
        set
        {
            if (_isRoot != value)
            {
                _isRoot = value;
                OnPropertyChanged(nameof(IsRoot));
            }
        }
    }

    private IBrush? _background;
    public IBrush? Background
    {
        get => _background;
        set
        {
            if (_background != value)
            {
                _background = value;
                OnPropertyChanged(nameof(Background));
            }
        }
    }

    private IBrush? _foreground;
    public IBrush? Foreground
    {
        get => _foreground;
        set
        {
            if (_foreground != value)
            {
                _foreground = value;
                OnPropertyChanged(nameof(Foreground));
            }
        }
    }

    private IBrush? _borderBrush;
    public IBrush? BorderBrush
    {
        get => _borderBrush;
        set
        {
            if (_borderBrush != value)
            {
                _borderBrush = value;
                OnPropertyChanged(nameof(BorderBrush));
            }
        }
    }

    private double? _fontSize;
    public double? FontSize
    {
        get => _fontSize;
        set
        {
            if (_fontSize != value)
            {
                _fontSize = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }
    }

    private FontWeight? _fontWeight;
    public FontWeight? FontWeight
    {
        get => _fontWeight;
        set
        {
            if (_fontWeight != value)
            {
                _fontWeight = value;
                OnPropertyChanged(nameof(FontWeight));
            }
        }
    }

    public ObservableCollection<MindMapNode> Children { get; } = new();

    private MindMapNode? _parent;
    public MindMapNode? Parent
    {
        get => _parent;
        set
        {
            if (_parent != value)
            {
                _parent = value;
                OnPropertyChanged(nameof(Parent));
            }
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public MindMapNode()
    {
    }

    public MindMapNode(string text, double x, double y)
    {
        Text = text;
        X = x;
        Y = y;
    }

    public void AddChild(MindMapNode child)
    {
        child.Parent = this;
        Children.Add(child);
    }

    public void RemoveChild(MindMapNode child)
    {
        child.Parent = null;
        Children.Remove(child);
    }

    public void ClearChildren()
    {
        foreach (var child in Children)
            child.Parent = null;
        Children.Clear();
    }
}
