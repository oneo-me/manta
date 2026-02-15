using System.Collections.ObjectModel;
using System.ComponentModel;
using Avalonia.Media;

namespace Manta.MindMap.Models;

public class MindMapGraph : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public ObservableCollection<MindMapNode> Nodes { get; } = new();
    public ObservableCollection<MindMapEdge> Edges { get; } = new();

    private MindMapNode? _rootNode;
    public MindMapNode? RootNode
    {
        get => _rootNode;
        private set
        {
            if (_rootNode != value)
            {
                _rootNode = value;
                OnPropertyChanged(nameof(RootNode));
            }
        }
    }

    private IBrush? _defaultNodeBackground;
    public IBrush? DefaultNodeBackground
    {
        get => _defaultNodeBackground;
        set
        {
            if (_defaultNodeBackground != value)
            {
                _defaultNodeBackground = value;
                OnPropertyChanged(nameof(DefaultNodeBackground));
            }
        }
    }

    private IBrush? _defaultNodeForeground;
    public IBrush? DefaultNodeForeground
    {
        get => _defaultNodeForeground;
        set
        {
            if (_defaultNodeForeground != value)
            {
                _defaultNodeForeground = value;
                OnPropertyChanged(nameof(DefaultNodeForeground));
            }
        }
    }

    private IBrush? _defaultEdgeStroke;
    public IBrush? DefaultEdgeStroke
    {
        get => _defaultEdgeStroke;
        set
        {
            if (_defaultEdgeStroke != value)
            {
                _defaultEdgeStroke = value;
                OnPropertyChanged(nameof(DefaultEdgeStroke));
            }
        }
    }

    private double _defaultNodeWidth = 120;
    public double DefaultNodeWidth
    {
        get => _defaultNodeWidth;
        set
        {
            if (Math.Abs(_defaultNodeWidth - value) > 0.001)
            {
                _defaultNodeWidth = value;
                OnPropertyChanged(nameof(DefaultNodeWidth));
            }
        }
    }

    private double _defaultNodeHeight = 40;
    public double DefaultNodeHeight
    {
        get => _defaultNodeHeight;
        set
        {
            if (Math.Abs(_defaultNodeHeight - value) > 0.001)
            {
                _defaultNodeHeight = value;
                OnPropertyChanged(nameof(DefaultNodeHeight));
            }
        }
    }

    private double _horizontalSpacing = 80;
    public double HorizontalSpacing
    {
        get => _horizontalSpacing;
        set
        {
            if (Math.Abs(_horizontalSpacing - value) > 0.001)
            {
                _horizontalSpacing = value;
                OnPropertyChanged(nameof(HorizontalSpacing));
            }
        }
    }

    private double _verticalSpacing = 20;
    public double VerticalSpacing
    {
        get => _verticalSpacing;
        set
        {
            if (Math.Abs(_verticalSpacing - value) > 0.001)
            {
                _verticalSpacing = value;
                OnPropertyChanged(nameof(VerticalSpacing));
            }
        }
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        LayoutNode(RootNode, 0, 0, 0);
    }

    double LayoutNode(MindMapNode node, double x, double y, int depth)
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
            var childHeight = LayoutNode(child, childX, currentY, depth + 1);
            currentY += childHeight + VerticalSpacing;
            totalHeight += childHeight + VerticalSpacing;
        }

        totalHeight -= VerticalSpacing;

        var nodeCenterY = y + node.Height / 2;
        var childrenCenterY = y + totalHeight / 2;
        node.Y = childrenCenterY - node.Height / 2;

        return Math.Max(totalHeight, node.Height);
    }
}
