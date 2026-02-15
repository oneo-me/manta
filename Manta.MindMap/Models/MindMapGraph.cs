using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Media;

namespace Manta.MindMap.Models;

public class MindMapGraph : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<MindMapNode> Nodes { get; } = new();
    public ObservableCollection<MindMapEdge> Edges { get; } = new();

    public MindMapNode? RootNode
    {
        get;
        private set
        {
            if (field != value)
            {
                field = value;
                OnPropertyChanged(nameof(RootNode));
            }
        }
    }

    public IBrush? DefaultNodeBackground
    {
        get;
        set
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(nameof(DefaultNodeBackground));
            }
        }
    }

    public IBrush? DefaultNodeForeground
    {
        get;
        set
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(nameof(DefaultNodeForeground));
            }
        }
    }

    public IBrush? DefaultEdgeStroke
    {
        get;
        set
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(nameof(DefaultEdgeStroke));
            }
        }
    }

    public double DefaultNodeWidth
    {
        get;
        set
        {
            if (Math.Abs(field - value) > 0.001)
            {
                field = value;
                OnPropertyChanged(nameof(DefaultNodeWidth));
            }
        }
    } = 120;

    public double DefaultNodeHeight
    {
        get;
        set
        {
            if (Math.Abs(field - value) > 0.001)
            {
                field = value;
                OnPropertyChanged(nameof(DefaultNodeHeight));
            }
        }
    } = 40;

    public double HorizontalSpacing
    {
        get;
        set
        {
            if (Math.Abs(field - value) > 0.001)
            {
                field = value;
                OnPropertyChanged(nameof(HorizontalSpacing));
            }
        }
    } = 80;

    public double VerticalSpacing
    {
        get;
        set
        {
            if (Math.Abs(field - value) > 0.001)
            {
                field = value;
                OnPropertyChanged(nameof(VerticalSpacing));
            }
        }
    } = 20;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }

    public void SetRoot(MindMapNode root)
    {
        Clear();
        RootNode = root;
        root.IsRoot = true;
        Nodes.Add(root);
    }

    public MindMapNode AddNode(string text, MindMapNode parent, double? x = null, double? y = null)
    {
        var node = new MindMapNode
        {
            Text = text,
            Width = DefaultNodeWidth,
            Height = DefaultNodeHeight,
            Background = DefaultNodeBackground,
            Foreground = DefaultNodeForeground
        };

        if (x.HasValue)
            node.X = x.Value;
        if (y.HasValue)
            node.Y = y.Value;

        parent.AddChild(node);
        Nodes.Add(node);

        var edge = new MindMapEdge(parent, node)
        {
            Stroke = DefaultEdgeStroke
        };
        Edges.Add(edge);

        return node;
    }

    public void RemoveNode(MindMapNode node)
    {
        if (node == RootNode)
        {
            Clear();
            return;
        }

        var edgesToRemove = Edges.Where(e => e.Source == node || e.Target == node).ToList();
        foreach (var edge in edgesToRemove)
            Edges.Remove(edge);

        RemoveNodeRecursive(node);
    }

    void RemoveNodeRecursive(MindMapNode node)
    {
        foreach (var child in node.Children.ToList())
            RemoveNodeRecursive(child);

        node.Parent?.RemoveChild(node);
        Nodes.Remove(node);
    }

    public void Clear()
    {
        Nodes.Clear();
        Edges.Clear();
        RootNode = null;
    }

    public void RefreshLayout()
    {
        if (RootNode == null) return;

        LayoutNode(RootNode, 0, 0);
    }

    double LayoutNode(MindMapNode node, double x, double y)
    {
        node.X = x;
        node.Y = y;

        if (!node.IsExpanded || node.Children.Count == 0)
            return node.Height;

        var totalHeight = 0.0;
        var childX = x + node.Width + HorizontalSpacing;
        var currentY = y;

        foreach (var child in node.Children)
        {
            var childHeight = LayoutNode(child, childX, currentY);
            currentY += childHeight + VerticalSpacing;
            totalHeight += childHeight + VerticalSpacing;
        }

        totalHeight -= VerticalSpacing;

        var childrenCenterY = y + totalHeight / 2;
        node.Y = childrenCenterY - node.Height / 2;

        return Math.Max(totalHeight, node.Height);
    }
}
