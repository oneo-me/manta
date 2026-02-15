using System.Collections.Specialized;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Manta.Controls;
using Manta.MindMap.Models;
using Path = Avalonia.Controls.Shapes.Path;

namespace Manta.MindMap.Controls;

public class MMindMap : TemplatedControl
{
    public static readonly StyledProperty<MindMapGraph?> MindMapProperty =
        AvaloniaProperty.Register<MMindMap, MindMapGraph?>(nameof(MindMap));

    public static readonly StyledProperty<bool> ShowGridProperty =
        AvaloniaProperty.Register<MMindMap, bool>(nameof(ShowGrid), true);

    public static readonly StyledProperty<IBrush?> NodeBackgroundProperty =
        AvaloniaProperty.Register<MMindMap, IBrush?>(nameof(NodeBackground));

    public static readonly StyledProperty<IBrush?> NodeForegroundProperty =
        AvaloniaProperty.Register<MMindMap, IBrush?>(nameof(NodeForeground));

    public static readonly StyledProperty<IBrush?> RootNodeBackgroundProperty =
        AvaloniaProperty.Register<MMindMap, IBrush?>(nameof(RootNodeBackground));

    public static readonly StyledProperty<IBrush?> RootNodeForegroundProperty =
        AvaloniaProperty.Register<MMindMap, IBrush?>(nameof(RootNodeForeground));

    public static readonly StyledProperty<IBrush?> EdgeBrushProperty =
        AvaloniaProperty.Register<MMindMap, IBrush?>(nameof(EdgeBrush));

    public static readonly StyledProperty<IBrush?> SelectedBrushProperty =
        AvaloniaProperty.Register<MMindMap, IBrush?>(nameof(SelectedBrush));

    public static readonly RoutedEvent<NodeSelectedEventArgs> NodeSelectedEvent =
        RoutedEvent.Register<MMindMap, NodeSelectedEventArgs>(nameof(NodeSelected), RoutingStrategies.Direct);

    public event EventHandler<NodeSelectedEventArgs>? NodeSelected
    {
        add => AddHandler(NodeSelectedEvent, value);
        remove => RemoveHandler(NodeSelectedEvent, value);
    }

    public static readonly RoutedEvent<NodeDoubleClickedEventArgs> NodeDoubleClickedEvent =
        RoutedEvent.Register<MMindMap, NodeDoubleClickedEventArgs>(nameof(NodeDoubleClicked), RoutingStrategies.Direct);

    public event EventHandler<NodeDoubleClickedEventArgs>? NodeDoubleClicked
    {
        add => AddHandler(NodeDoubleClickedEvent, value);
        remove => RemoveHandler(NodeDoubleClickedEvent, value);
    }

    public static readonly RoutedEvent<EdgeSelectedEventArgs> EdgeSelectedEvent =
        RoutedEvent.Register<MMindMap, EdgeSelectedEventArgs>(nameof(EdgeSelected), RoutingStrategies.Direct);

    public event EventHandler<EdgeSelectedEventArgs>? EdgeSelected
    {
        add => AddHandler(EdgeSelectedEvent, value);
        remove => RemoveHandler(EdgeSelectedEvent, value);
    }

    Canvas? _canvas;
    MCanvas? _mCanvas;
    MindMapNode? _draggingNode;
    Point _dragStartPoint;
    Point _nodeStartPosition;
    readonly Dictionary<MindMapNode, Border> _nodeElements = new();
    readonly Dictionary<MindMapEdge, Path> _edgeElements = new();

    public MindMapGraph? MindMap
    {
        get => GetValue(MindMapProperty);
        set => SetValue(MindMapProperty, value);
    }

    public bool ShowGrid
    {
        get => GetValue(ShowGridProperty);
        set => SetValue(ShowGridProperty, value);
    }

    public IBrush? NodeBackground
    {
        get => GetValue(NodeBackgroundProperty);
        set => SetValue(NodeBackgroundProperty, value);
    }

    public IBrush? NodeForeground
    {
        get => GetValue(NodeForegroundProperty);
        set => SetValue(NodeForegroundProperty, value);
    }

    public IBrush? RootNodeBackground
    {
        get => GetValue(RootNodeBackgroundProperty);
        set => SetValue(RootNodeBackgroundProperty, value);
    }

    public IBrush? RootNodeForeground
    {
        get => GetValue(RootNodeForegroundProperty);
        set => SetValue(RootNodeForegroundProperty, value);
    }

    public IBrush? EdgeBrush
    {
        get => GetValue(EdgeBrushProperty);
        set => SetValue(EdgeBrushProperty, value);
    }

    public IBrush? SelectedBrush
    {
        get => GetValue(SelectedBrushProperty);
        set => SetValue(SelectedBrushProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _canvas = e.NameScope.Find<Canvas>("PART_Canvas");
        _mCanvas = e.NameScope.Find<MCanvas>("PART_MCanvas");

        SubscribeToGraph();
        Refresh();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == MindMapProperty)
        {
            UnsubscribeFromGraph();
            SubscribeToGraph();
            Refresh();
        }
    }

    void SubscribeToGraph()
    {
        if (MindMap == null) return;

        MindMap.Nodes.CollectionChanged += OnNodesChanged;
        MindMap.Edges.CollectionChanged += OnEdgesChanged;

        foreach (var node in MindMap.Nodes)
            SubscribeToNode(node);

        foreach (var edge in MindMap.Edges)
            SubscribeToEdge(edge);
    }

    void UnsubscribeFromGraph()
    {
        if (MindMap == null) return;

        MindMap.Nodes.CollectionChanged -= OnNodesChanged;
        MindMap.Edges.CollectionChanged -= OnEdgesChanged;

        foreach (var node in MindMap.Nodes)
            UnsubscribeFromNode(node);

        foreach (var edge in MindMap.Edges)
            UnsubscribeFromEdge(edge);
    }

    void SubscribeToNode(MindMapNode node)
    {
        node.PropertyChanged += OnNodePropertyChanged;
    }

    void UnsubscribeFromNode(MindMapNode node)
    {
        node.PropertyChanged -= OnNodePropertyChanged;
    }

    void SubscribeToEdge(MindMapEdge edge)
    {
        edge.PropertyChanged += OnEdgePropertyChanged;
    }

    void UnsubscribeFromEdge(MindMapEdge edge)
    {
        edge.PropertyChanged -= OnEdgePropertyChanged;
    }

    void OnNodesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (MindMapNode node in e.NewItems)
                SubscribeToNode(node);

        if (e.OldItems != null)
            foreach (MindMapNode node in e.OldItems)
            {
                UnsubscribeFromNode(node);
                if (_nodeElements.TryGetValue(node, out var border))
                {
                    _canvas?.Children.Remove(border);
                    _nodeElements.Remove(node);
                }
            }

        Refresh();
    }

    void OnEdgesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (MindMapEdge edge in e.NewItems)
                SubscribeToEdge(edge);

        if (e.OldItems != null)
            foreach (MindMapEdge edge in e.OldItems)
            {
                UnsubscribeFromEdge(edge);
                if (_edgeElements.TryGetValue(edge, out var path))
                {
                    _canvas?.Children.Remove(path);
                    _edgeElements.Remove(edge);
                }
            }

        Refresh();
    }

    void OnNodePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is MindMapNode node)
        {
            if (e.PropertyName is nameof(MindMapNode.X) or nameof(MindMapNode.Y) or nameof(MindMapNode.Width) or nameof(MindMapNode.Height))
                UpdateNodePosition(node);
            else
                RefreshNode(node);
        }
    }

    void OnEdgePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is MindMapEdge edge)
            RefreshEdge(edge);
    }

    public void Refresh()
    {
        if (_canvas == null || MindMap == null) return;

        _canvas.Children.Clear();
        _nodeElements.Clear();
        _edgeElements.Clear();

        foreach (var edge in MindMap.Edges)
            DrawEdge(edge);

        foreach (var node in MindMap.Nodes)
            DrawNode(node);
    }

    void DrawNode(MindMapNode node)
    {
        var border = new Border
        {
            Width = node.Width,
            Height = node.Height,
            Background = node.IsRoot ? node.Background ?? RootNodeBackground : node.Background ?? NodeBackground,
            BorderBrush = node.BorderBrush,
            BorderThickness = new(1),
            CornerRadius = new(8),
            Child = new TextBlock
            {
                Text = node.Text,
                Foreground = node.IsRoot ? node.Foreground ?? RootNodeForeground : node.Foreground ?? NodeForeground,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                TextTrimming = TextTrimming.CharacterEllipsis,
                FontSize = node.FontSize ?? (node.IsRoot ? 16 : 14),
                FontWeight = node.FontWeight ?? (node.IsRoot ? FontWeight.Bold : FontWeight.Normal)
            },
            Tag = node
        };

        if (node.IsSelected && SelectedBrush != null)
        {
            border.BorderBrush = SelectedBrush;
            border.BorderThickness = new(2);
        }

        border.PointerPressed += NodeOnPointerPressed;
        border.DoubleTapped += NodeOnDoubleTapped;

        Canvas.SetLeft(border, node.X);
        Canvas.SetTop(border, node.Y);
        _canvas!.Children.Add(border);
        _nodeElements[node] = border;
    }

    void UpdateNodePosition(MindMapNode node)
    {
        if (_nodeElements.TryGetValue(node, out var border))
        {
            Canvas.SetLeft(border, node.X);
            Canvas.SetTop(border, node.Y);
        }

        if (MindMap != null)
            foreach (var edge in MindMap.Edges.Where(e => e.Source == node || e.Target == node))
                RefreshEdge(edge);
    }

    void RefreshNode(MindMapNode node)
    {
        if (!_nodeElements.TryGetValue(node, out var border)) return;

        border.Width = node.Width;
        border.Height = node.Height;
        border.Background = node.IsRoot ? node.Background ?? RootNodeBackground : node.Background ?? NodeBackground;
        border.BorderBrush = node.BorderBrush;

        if (border.Child is TextBlock textBlock)
        {
            textBlock.Text = node.Text;
            textBlock.Foreground = node.IsRoot ? node.Foreground ?? RootNodeForeground : node.Foreground ?? NodeForeground;
            textBlock.FontSize = node.FontSize ?? (node.IsRoot ? 16 : 14);
            textBlock.FontWeight = node.FontWeight ?? (node.IsRoot ? FontWeight.Bold : FontWeight.Normal);
        }

        if (node.IsSelected && SelectedBrush != null)
        {
            border.BorderBrush = SelectedBrush;
            border.BorderThickness = new(2);
        }
        else
        {
            border.BorderBrush = node.BorderBrush;
            border.BorderThickness = new(1);
        }
    }

    void DrawEdge(MindMapEdge edge)
    {
        var path = CreateEdgePath(edge);

        path.PointerPressed += EdgeOnPointerPressed;

        _canvas!.Children.Insert(0, path);
        _edgeElements[edge] = path;
    }

    void RefreshEdge(MindMapEdge edge)
    {
        if (!_edgeElements.TryGetValue(edge, out var path)) return;

        var newPath = CreateEdgePath(edge);
        var index = _canvas?.Children.IndexOf(path) ?? -1;

        if (index >= 0)
        {
            _canvas!.Children[index] = newPath;
            _edgeElements[edge] = newPath;
        }

        newPath.PointerPressed += EdgeOnPointerPressed;
    }

    Path CreateEdgePath(MindMapEdge edge)
    {
        var source = edge.Source;
        var target = edge.Target;

        var startX = source.X + source.Width;
        var startY = source.Y + source.Height / 2;
        var endX = target.X;
        var endY = target.Y + target.Height / 2;

        var controlPointOffset = (endX - startX) / 2;

        var pathGeometry = new PathGeometry();
        var figure = new PathFigure { StartPoint = new(startX, startY) };

        var bezier = new BezierSegment
        {
            Point1 = new(startX + controlPointOffset, startY),
            Point2 = new(endX - controlPointOffset, endY),
            Point3 = new(endX, endY)
        };

        figure.Segments?.Add(bezier);
        pathGeometry.Figures?.Add(figure);

        var path = new Path
        {
            Data = pathGeometry,
            Stroke = edge.Stroke ?? EdgeBrush ?? Brushes.Gray,
            StrokeThickness = edge.StrokeThickness,
            Tag = edge
        };

        if (edge.IsSelected && SelectedBrush != null)
        {
            path.Stroke = SelectedBrush;
            path.StrokeThickness = edge.StrokeThickness + 1;
        }

        return path;
    }

    void NodeOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Border border || border.Tag is not MindMapNode node) return;

        var props = e.GetCurrentPoint(this).Properties;

        if (props.IsLeftButtonPressed)
        {
            SelectNode(node);

            _draggingNode = node;
            _dragStartPoint = e.GetPosition(this);
            _nodeStartPosition = new(node.X, node.Y);
            e.Pointer.Capture(border);
            e.Handled = true;
        }
    }

    void NodeOnDoubleTapped(object? sender, TappedEventArgs e)
    {
        if (sender is not Border border || border.Tag is not MindMapNode node) return;

        node.IsExpanded = !node.IsExpanded;

        if (MindMap != null)
        {
            MindMap.RefreshLayout();
            Refresh();
        }

        RaiseEvent(new NodeDoubleClickedEventArgs(NodeDoubleClickedEvent, node));
        e.Handled = true;
    }

    void EdgeOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Path path || path.Tag is not MindMapEdge edge) return;

        SelectEdge(edge);
        e.Handled = true;
    }

    public void SelectNode(MindMapNode node)
    {
        if (MindMap == null) return;

        foreach (var n in MindMap.Nodes)
            n.IsSelected = false;

        node.IsSelected = true;

        foreach (var n in MindMap.Nodes)
            RefreshNode(n);

        RaiseEvent(new NodeSelectedEventArgs(NodeSelectedEvent, node));
    }

    public void SelectEdge(MindMapEdge edge)
    {
        if (MindMap == null) return;

        foreach (var e in MindMap.Edges)
            e.IsSelected = false;

        edge.IsSelected = true;

        foreach (var e in MindMap.Edges)
            RefreshEdge(e);

        RaiseEvent(new EdgeSelectedEventArgs(EdgeSelectedEvent, edge));
    }

    public void ClearSelection()
    {
        if (MindMap == null) return;

        foreach (var node in MindMap.Nodes)
            node.IsSelected = false;

        foreach (var edge in MindMap.Edges)
            edge.IsSelected = false;

        Refresh();
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (_draggingNode != null && _mCanvas != null)
        {
            var position = e.GetPosition(this);
            var delta = position - _dragStartPoint;

            _draggingNode.X = _nodeStartPosition.X + delta.X / _mCanvas.Zoom;
            _draggingNode.Y = _nodeStartPosition.Y + delta.Y / _mCanvas.Zoom;

            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (_draggingNode != null)
        {
            if (MindMap != null)
            {
                MindMap.RefreshLayout();
                Refresh();
            }

            _draggingNode = null;
            e.Pointer.Capture(null);
            e.Handled = true;
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var source = e.Source as Control;
        var isClickOnNode = false;

        while (source != null)
        {
            if (source is Border { Tag: MindMapNode })
            {
                isClickOnNode = true;
                break;
            }

            source = source.Parent as Control;
        }

        var props = e.GetCurrentPoint(this).Properties;

        if (props.IsLeftButtonPressed && !isClickOnNode)
            ClearSelection();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch (e.Key)
        {
            case Key.Delete:
                DeleteSelected();
                e.Handled = true;
                break;

            case Key.Escape:
                ClearSelection();
                e.Handled = true;
                break;
        }
    }

    public void DeleteSelected()
    {
        if (MindMap == null) return;

        var selectedNode = MindMap.Nodes.FirstOrDefault(n => n.IsSelected);
        if (selectedNode != null)
        {
            MindMap.RemoveNode(selectedNode);
            return;
        }

        var selectedEdge = MindMap.Edges.FirstOrDefault(e => e.IsSelected);
        if (selectedEdge != null)
        {
            MindMap.Edges.Remove(selectedEdge);
            selectedEdge.Target.Parent?.RemoveChild(selectedEdge.Target);
            MindMap.Nodes.Remove(selectedEdge.Target);
        }
    }

    public void AddNode(string text, MindMapNode? parent = null)
    {
        if (MindMap == null) return;

        if (parent == null)
        {
            if (MindMap.RootNode != null) return;

            var root = new MindMapNode
            {
                Text = text,
                IsRoot = true,
                Width = 160,
                Height = 50
            };
            MindMap.SetRoot(root);
        }
        else
        {
            var lastChild = parent.Children.LastOrDefault();
            var y = lastChild != null ? lastChild.Y + lastChild.Height + 20 : parent.Y;

            MindMap.AddNode(text, parent, parent.X + parent.Width + 80, y);
        }

        MindMap.RefreshLayout();
        Refresh();
    }

    public void ResetView()
    {
        _mCanvas?.ResetView();
    }

    public void FitAll()
    {
        _mCanvas?.FitAll();
    }
}