using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Manta.MindMap.Models;
using AvaloniaObject = Avalonia.AvaloniaObject;
using Path = Avalonia.Controls.Shapes.Path;

namespace Manta.MindMap.Controls;

public class MMindMap : TemplatedControl
{
    public static readonly StyledProperty<MindMapGraph?> MindMapProperty =
        AvaloniaProperty.Register<MMindMap, MindMapGraph?>(nameof(MindMap));

    public const double DefaultMinZoom = 0.1;
    public const double DefaultMaxZoom = 5.0;

    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<MMindMap, double>(nameof(Zoom), 1.0,
            coerce: (_, v) => Math.Clamp(v, DefaultMinZoom, DefaultMaxZoom));

    public static readonly StyledProperty<Point> OffsetProperty =
        AvaloniaProperty.Register<MMindMap, Point>(nameof(Offset));

    public static readonly StyledProperty<double> MinZoomProperty =
        AvaloniaProperty.Register<MMindMap, double>(nameof(MinZoom), DefaultMinZoom);

    public static readonly StyledProperty<double> MaxZoomProperty =
        AvaloniaProperty.Register<MMindMap, double>(nameof(MaxZoom), DefaultMaxZoom);

    public const double ZoomFactor = 1.1;

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

    private Canvas? _canvas;
    private Panel? _transformContainer;
    private MindMapNode? _draggingNode;
    private Point _dragStartPoint;
    private Point _nodeStartPosition;
    private bool _isPanning;
    private Point _lastPanPosition;
    private bool _isSpaceHeld;
    private readonly Dictionary<MindMapNode, Border> _nodeElements = new();
    private readonly Dictionary<MindMapEdge, Path> _edgeElements = new();

    public MindMapGraph? MindMap
    {
        get => GetValue(MindMapProperty);
        set => SetValue(MindMapProperty, value);
    }

    public double Zoom
    {
        get => GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    public Point Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public double MinZoom
    {
        get => GetValue(MinZoomProperty);
        set => SetValue(MinZoomProperty, value);
    }

    public double MaxZoom
    {
        get => GetValue(MaxZoomProperty);
        set => SetValue(MaxZoomProperty, value);
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
        _transformContainer = e.NameScope.Find<Panel>("PART_TransformContainer");

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
        else if (change.Property == ZoomProperty || change.Property == OffsetProperty)
        {
            UpdateTransform();
        }
    }

    private void SubscribeToGraph()
    {
        if (MindMap == null) return;

        MindMap.Nodes.CollectionChanged += OnNodesChanged;
        MindMap.Edges.CollectionChanged += OnEdgesChanged;

        foreach (var node in MindMap.Nodes)
            SubscribeToNode(node);

        foreach (var edge in MindMap.Edges)
            SubscribeToEdge(edge);
    }

    private void UnsubscribeFromGraph()
    {
        if (MindMap == null) return;

        MindMap.Nodes.CollectionChanged -= OnNodesChanged;
        MindMap.Edges.CollectionChanged -= OnEdgesChanged;

        foreach (var node in MindMap.Nodes)
            UnsubscribeFromNode(node);

        foreach (var edge in MindMap.Edges)
            UnsubscribeFromEdge(edge);
    }

    private void SubscribeToNode(MindMapNode node)
    {
        node.PropertyChanged += OnNodePropertyChanged;
    }

    private void UnsubscribeFromNode(MindMapNode node)
    {
        node.PropertyChanged -= OnNodePropertyChanged;
    }

    private void SubscribeToEdge(MindMapEdge edge)
    {
        edge.PropertyChanged += OnEdgePropertyChanged;
    }

    private void UnsubscribeFromEdge(MindMapEdge edge)
    {
        edge.PropertyChanged -= OnEdgePropertyChanged;
    }

    private void OnNodesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (MindMapNode node in e.NewItems)
                SubscribeToNode(node);
        }

        if (e.OldItems != null)
        {
            foreach (MindMapNode node in e.OldItems)
            {
                UnsubscribeFromNode(node);
                if (_nodeElements.TryGetValue(node, out var border))
                {
                    _canvas?.Children.Remove(border);
                    _nodeElements.Remove(node);
                }
            }
        }

        Refresh();
    }

    private void OnEdgesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
        {
            foreach (MindMapEdge edge in e.NewItems)
                SubscribeToEdge(edge);
        }

        if (e.OldItems != null)
        {
            foreach (MindMapEdge edge in e.OldItems)
            {
                UnsubscribeFromEdge(edge);
                if (_edgeElements.TryGetValue(edge, out var path))
                {
                    _canvas?.Children.Remove(path);
                    _edgeElements.Remove(edge);
                }
            }
        }

        Refresh();
    }

    private void OnNodePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is MindMapNode node)
        {
            if (e.PropertyName == nameof(MindMapNode.X) || 
                e.PropertyName == nameof(MindMapNode.Y) ||
                e.PropertyName == nameof(MindMapNode.Width) ||
                e.PropertyName == nameof(MindMapNode.Height))
            {
                UpdateNodePosition(node);
            }
            else
            {
                RefreshNode(node);
            }
        }
    }

    private void OnEdgePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is MindMapEdge edge)
        {
            RefreshEdge(edge);
        }
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

        UpdateTransform();
    }

    private void DrawNode(MindMapNode node)
    {
        var border = new Border
        {
            Width = node.Width,
            Height = node.Height,
            Background = node.IsRoot ? (node.Background ?? RootNodeBackground) : (node.Background ?? NodeBackground),
            BorderBrush = node.BorderBrush,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(8),
            Child = new TextBlock
            {
                Text = node.Text,
                Foreground = node.IsRoot ? (node.Foreground ?? RootNodeForeground) : (node.Foreground ?? NodeForeground),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
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
            border.BorderThickness = new Thickness(2);
        }

        border.PointerPressed += NodeOnPointerPressed;
        border.DoubleTapped += NodeOnDoubleTapped;

        Canvas.SetLeft(border, node.X);
        Canvas.SetTop(border, node.Y);
        _canvas!.Children.Add(border);
        _nodeElements[node] = border;
    }

    private void UpdateNodePosition(MindMapNode node)
    {
        if (_nodeElements.TryGetValue(node, out var border))
        {
            Canvas.SetLeft(border, node.X);
            Canvas.SetTop(border, node.Y);
        }

        if (MindMap != null)
        {
            foreach (var edge in MindMap.Edges.Where(e => e.Source == node || e.Target == node))
            {
                RefreshEdge(edge);
            }
        }
    }

    private void RefreshNode(MindMapNode node)
    {
        if (!_nodeElements.TryGetValue(node, out var border)) return;

        border.Width = node.Width;
        border.Height = node.Height;
        border.Background = node.IsRoot ? (node.Background ?? RootNodeBackground) : (node.Background ?? NodeBackground);
        border.BorderBrush = node.BorderBrush;

        if (border.Child is TextBlock textBlock)
        {
            textBlock.Text = node.Text;
            textBlock.Foreground = node.IsRoot ? (node.Foreground ?? RootNodeForeground) : (node.Foreground ?? NodeForeground);
            textBlock.FontSize = node.FontSize ?? (node.IsRoot ? 16 : 14);
            textBlock.FontWeight = node.FontWeight ?? (node.IsRoot ? FontWeight.Bold : FontWeight.Normal);
        }

        if (node.IsSelected && SelectedBrush != null)
        {
            border.BorderBrush = SelectedBrush;
            border.BorderThickness = new Thickness(2);
        }
        else
        {
            border.BorderBrush = node.BorderBrush;
            border.BorderThickness = new Thickness(1);
        }
    }

    private void DrawEdge(MindMapEdge edge)
    {
        var path = CreateEdgePath(edge);

        path.PointerPressed += EdgeOnPointerPressed;

        _canvas!.Children.Insert(0, path);
        _edgeElements[edge] = path;
    }

    private void RefreshEdge(MindMapEdge edge)
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

    private Path CreateEdgePath(MindMapEdge edge)
    {
        var source = edge.Source;
        var target = edge.Target;

        var startX = source.X + source.Width;
        var startY = source.Y + source.Height / 2;
        var endX = target.X;
        var endY = target.Y + target.Height / 2;

        var controlPointOffset = (endX - startX) / 2;

        var pathGeometry = new PathGeometry();
        var figure = new PathFigure { StartPoint = new Point(startX, startY) };

        var bezier = new BezierSegment
        {
            Point1 = new Point(startX + controlPointOffset, startY),
            Point2 = new Point(endX - controlPointOffset, endY),
            Point3 = new Point(endX, endY)
        };

        figure.Segments.Add(bezier);
        pathGeometry.Figures.Add(figure);

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

    private void NodeOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Border border || border.Tag is not MindMapNode node) return;

        var props = e.GetCurrentPoint(this).Properties;

        if (props.IsLeftButtonPressed)
        {
            SelectNode(node);

            _draggingNode = node;
            _dragStartPoint = e.GetPosition(this);
            _nodeStartPosition = new Point(node.X, node.Y);
            e.Pointer.Capture(border);
            e.Handled = true;
        }
    }

    private void NodeOnDoubleTapped(object? sender, TappedEventArgs e)
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

    private void EdgeOnPointerPressed(object? sender, PointerPressedEventArgs e)
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

        if (_draggingNode != null)
        {
            var position = e.GetPosition(this);
            var delta = position - _dragStartPoint;

            _draggingNode.X = _nodeStartPosition.X + delta.X / Zoom;
            _draggingNode.Y = _nodeStartPosition.Y + delta.Y / Zoom;

            e.Handled = true;
        }
        else if (_isPanning)
        {
            var position = e.GetPosition(this);
            var delta = position - _lastPanPosition;
            Offset = new Point(Offset.X + delta.X, Offset.Y + delta.Y);
            _lastPanPosition = position;
            UpdateTransform();
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
        else if (_isPanning)
        {
            _isPanning = false;
            e.Pointer.Capture(null);
            e.Handled = true;
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        Focus();

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

        if (props.IsLeftButtonPressed)
        {
            if (_isSpaceHeld)
            {
                _isPanning = true;
                _lastPanPosition = e.GetPosition(this);
                e.Pointer.Capture(this);
                e.Handled = true;
            }
            else if (!isClickOnNode)
            {
                ClearSelection();
                _isPanning = true;
                _lastPanPosition = e.GetPosition(this);
                e.Pointer.Capture(this);
                e.Handled = true;
            }
        }
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        var position = e.GetPosition(this);
        var factor = e.Delta.Y > 0 ? ZoomFactor : 1.0 / ZoomFactor;
        var newZoom = Math.Clamp(Zoom * factor, MinZoom, MaxZoom);

        if (Math.Abs(newZoom - Zoom) > 0.001)
        {
            var actualFactor = newZoom / Zoom;
            var newOffset = new Point(
                position.X - (position.X - Offset.X) * actualFactor,
                position.Y - (position.Y - Offset.Y) * actualFactor
            );

            Zoom = newZoom;
            Offset = newOffset;
            UpdateTransform();
        }

        e.Handled = true;
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch (e.Key)
        {
            case Key.Space:
                if (!_isSpaceHeld)
                {
                    _isSpaceHeld = true;
                    Cursor = new Cursor(StandardCursorType.Hand);
                }
                e.Handled = true;
                break;

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

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        if (e.Key == Key.Space)
        {
            _isSpaceHeld = false;
            Cursor = Cursor.Default;
            e.Handled = true;
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
        Zoom = 1.0;
        Offset = new Point(50, 50);
        UpdateTransform();
    }

    private void UpdateTransform()
    {
        if (_transformContainer == null) return;

        _transformContainer.RenderTransform = new TransformGroup
        {
            Children =
            {
                new ScaleTransform(Zoom, Zoom),
                new TranslateTransform(Offset.X, Offset.Y)
            }
        };
    }
}

public class NodeSelectedEventArgs : RoutedEventArgs
{
    public MindMapNode Node { get; }

    public NodeSelectedEventArgs(RoutedEvent routedEvent, MindMapNode node) : base(routedEvent)
    {
        Node = node;
    }
}

public class NodeDoubleClickedEventArgs : RoutedEventArgs
{
    public MindMapNode Node { get; }

    public NodeDoubleClickedEventArgs(RoutedEvent routedEvent, MindMapNode node) : base(routedEvent)
    {
        Node = node;
    }
}

public class EdgeSelectedEventArgs : RoutedEventArgs
{
    public MindMapEdge Edge { get; }

    public EdgeSelectedEventArgs(RoutedEvent routedEvent, MindMapEdge edge) : base(routedEvent)
    {
        Edge = edge;
    }
}
