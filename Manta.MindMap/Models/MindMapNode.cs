using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Media;

namespace Manta.MindMap.Models;

public class MindMapNode : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public string Id { get; } = Guid.NewGuid().ToString();

    public string Text
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(nameof(Text));
            }
        }
    } = string.Empty;

    public double X
    {
        get;
        set
        {
            if (Math.Abs(field - value) > 0.001)
            {
                field = value;
                OnPropertyChanged(nameof(X));
            }
        }
    }

    public double Y
    {
        get;
        set
        {
            if (Math.Abs(field - value) > 0.001)
            {
                field = value;
                OnPropertyChanged(nameof(Y));
            }
        }
    }

    public double Width
    {
        get;
        set
        {
            if (Math.Abs(field - value) > 0.001)
            {
                field = value;
                OnPropertyChanged(nameof(Width));
            }
        }
    } = 120;

    public double Height
    {
        get;
        set
        {
            if (Math.Abs(field - value) > 0.001)
            {
                field = value;
                OnPropertyChanged(nameof(Height));
            }
        }
    } = 40;

    public bool IsExpanded
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(nameof(IsExpanded));
            }
        }
    } = true;

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

    public bool IsRoot
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(nameof(IsRoot));
            }
        }
    }

    public IBrush? Background
    {
        get;
        set
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(nameof(Background));
            }
        }
    }

    public IBrush? Foreground
    {
        get;
        set
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(nameof(Foreground));
            }
        }
    }

    public IBrush? BorderBrush
    {
        get;
        set
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(nameof(BorderBrush));
            }
        }
    }

    public double? FontSize
    {
        get;
        set
        {
            if (Math.Abs((field ?? 0) - (value ?? 0)) > 0.001)
            {
                field = value;
                OnPropertyChanged(nameof(FontSize));
            }
        }
    }

    public FontWeight? FontWeight
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(nameof(FontWeight));
            }
        }
    }

    public ObservableCollection<MindMapNode> Children { get; } = new();

    public MindMapNode? Parent
    {
        get;
        set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(nameof(Parent));
            }
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
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
