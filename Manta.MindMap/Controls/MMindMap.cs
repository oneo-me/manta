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

    const double DragThreshold = 5;

    Canvas? _canvas;
    MCanvas? _mCanvas;
    MindMapGraph? _subscribedGraph;
    bool _isRefreshing;

    // Single/multi-node drag state
    Point _dragStartPoint;
    bool _dragHasMovedEnough;
    readonly List<MindMapNode> _draggingNodes = new();
    readonly List<Point> _draggingStartPositions = new();
    MindMapNode? _dropTarget;
    Border? _dropTargetBorder;

    // Box selection state
    bool _isBoxSelecting;
    Point _boxSelectStart;
    Border? _selectionRect;

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

        if (_mCanvas != null)
            // Register PointerPressed directly on Canvas via bubble handler.
            // This fires BEFORE MCanvas.OnPointerPressed in the bubble chain,
            // so we can set e.Handled = true to prevent MCanvas from panning.
            _mCanvas.AddHandler(PointerPressedEvent, OnCanvasPointerPressed, RoutingStrategies.Bubble);

        SubscribeToGraph(MindMap);
        Refresh();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == MindMapProperty)
        {
            UnsubscribeFromGraph();
            SubscribeToGraph(MindMap);
            Refresh();
        }
    }

    void SubscribeToGraph(MindMapGraph? graph)
    {
        if (graph == null || graph == _subscribedGraph) return;

        UnsubscribeFromGraph();

        _subscribedGraph = graph;
        graph.Nodes.CollectionChanged += OnNodesChanged;
        graph.Edges.CollectionChanged += OnEdgesChanged;

        foreach (var node in graph.Nodes)
            SubscribeToNode(node);

        foreach (var edge in graph.Edges)
            SubscribeToEdge(edge);
    }

    void UnsubscribeFromGraph()
    {
        if (_subscribedGraph == null) return;

        _subscribedGraph.Nodes.CollectionChanged -= OnNodesChanged;
        _subscribedGraph.Edges.CollectionChanged -= OnEdgesChanged;

        foreach (var node in _subscribedGraph.Nodes)
            UnsubscribeFromNode(node);

        foreach (var edge in _subscribedGraph.Edges)
            UnsubscribeFromEdge(edge);

        _subscribedGraph = null;
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
                UnsubscribeFromNode(node);

        Refresh();
    }

    void OnEdgesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.NewItems != null)
            foreach (MindMapEdge edge in e.NewItems)
                SubscribeToEdge(edge);

        if (e.OldItems != null)
            foreach (MindMapEdge edge in e.OldItems)
                UnsubscribeFromEdge(edge);

        Refresh();
    }

    void OnNodePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_isRefreshing) return;
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
        if (_isRefreshing) return;
        if (sender is MindMapEdge edge)
            RefreshEdge(edge);
    }

    public void Refresh()
    {
        if (_canvas == null || MindMap == null) return;
        if (_isRefreshing) return;

        _isRefreshing = true;
        try
        {
            _canvas.Children.Clear();
            _nodeElements.Clear();
            _edgeElements.Clear();

            foreach (var edge in MindMap.Edges)
                DrawEdge(edge);

            foreach (var node in MindMap.Nodes)
                DrawNode(node);
        }
        finally
        {
            _isRefreshing = false;
        }
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
            newPath.PointerPressed += EdgeOnPointerPressed;
        }
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
        var figure = new PathFigure { StartPoint = new(startX, startY), IsClosed = false, IsFilled = false };

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

    // --- Pointer event handling ---
    // Box selection is initiated from a direct handler on Canvas (OnCanvasPointerPressed).
    // This fires BEFORE MCanvas.OnPointerPressed during bubbling, so we can mark
    // the event as handled to prevent MCanvas from panning.

    void OnCanvasPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // Check if click is on a node or edge
        var source = e.Source as Control;
        while (source != null)
        {
            if (source is Border { Tag: MindMapNode } or Path { Tag: MindMapEdge })
                return; // Let node/edge handlers deal with it
            if (source == _canvas)
                break;
            source = source.Parent as Control;
        }

        var props = e.GetCurrentPoint(this).Properties;

        if (props.IsLeftButtonPressed)
        {
            ClearSelection();
            StartBoxSelection(e);
            e.Handled = true; // Prevent MCanvas from panning
        }
    }

    void NodeOnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Border border || border.Tag is not MindMapNode node) return;

        var props = e.GetCurrentPoint(this).Properties;

        if (props.IsLeftButtonPressed)
        {
            SelectNode(node);

            // Root node is not draggable
            if (node.IsRoot)
            {
                e.Handled = true;
                return;
            }

            _dragStartPoint = e.GetPosition(this);
            _dragHasMovedEnough = false;

            // Build list of nodes to drag
            _draggingNodes.Clear();
            _draggingStartPositions.Clear();

            if (node.IsSelected && MindMap != null && MindMap.Nodes.Count(n => n.IsSelected) > 1)
            {
                // Multi-selection: drag all selected nodes (exclude root)
                foreach (var n in MindMap.Nodes.Where(n => n is { IsSelected: true, IsRoot: false }))
                {
                    _draggingNodes.Add(n);
                    _draggingStartPositions.Add(new(n.X, n.Y));
                }
            }
            else
            {
                _draggingNodes.Add(node);
                _draggingStartPositions.Add(new(node.X, node.Y));
            }

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

        if (_draggingNodes.Count > 0 && _mCanvas != null)
        {
            var position = e.GetPosition(this);
            var delta = position - _dragStartPoint;

            if (!_dragHasMovedEnough)
            {
                if (Math.Abs(delta.X) > DragThreshold || Math.Abs(delta.Y) > DragThreshold)
                {
                    _dragHasMovedEnough = true;
                    HideDraggingEdges();
                }
                else
                {
                    return;
                }
            }

            for (var i = 0; i < _draggingNodes.Count; i++)
            {
                _draggingNodes[i].X = _draggingStartPositions[i].X + delta.X / _mCanvas.Zoom;
                _draggingNodes[i].Y = _draggingStartPositions[i].Y + delta.Y / _mCanvas.Zoom;
            }

            // Hit-test for drop target (reparent)
            UpdateDropTarget(position);

            e.Handled = true;
        }
        else if (_isBoxSelecting && _canvas != null && _mCanvas != null)
        {
            var position = e.GetPosition(_canvas);
            UpdateBoxSelection(position);
            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (_draggingNodes.Count > 0)
        {
            if (_dragHasMovedEnough && _dropTarget != null && MindMap != null)
                // Reparent all dragged nodes to drop target
                foreach (var node in _draggingNodes.ToList())
                    MindMap.ReparentNode(node, _dropTarget);

            ClearDropTarget();

            if (MindMap != null)
            {
                MindMap.RefreshLayout();
                Refresh();
            }

            _draggingNodes.Clear();
            _draggingStartPositions.Clear();
            e.Pointer.Capture(null);
            e.Handled = true;
        }
        else if (_isBoxSelecting)
        {
            EndBoxSelection();
            e.Pointer.Capture(null);
            e.Handled = true;
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch (e.Key)
        {
            case Key.Tab:
                AddChildToSelected();
                e.Handled = true;
                break;

            case Key.Enter:
                AddSiblingToSelected();
                e.Handled = true;
                break;

            case Key.Delete:
            case Key.Back:
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

        var selectedNodes = MindMap.Nodes.Where(n => n is { IsSelected: true, IsRoot: false }).ToList();
        if (selectedNodes.Count > 0)
        {
            foreach (var node in selectedNodes)
                MindMap.RemoveNode(node);
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

    void AddChildToSelected()
    {
        if (MindMap == null) return;

        var selectedNode = MindMap.Nodes.FirstOrDefault(n => n.IsSelected);
        if (selectedNode == null) return;

        AddNode("New Node", selectedNode);
    }

    void AddSiblingToSelected()
    {
        if (MindMap == null) return;

        var selectedNode = MindMap.Nodes.FirstOrDefault(n => n.IsSelected);
        if (selectedNode?.Parent == null || selectedNode.IsRoot) return;

        AddNode("New Node", selectedNode.Parent);
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
            ClearSelection();
            root.IsSelected = true;
            RefreshNode(root);
        }
        else
        {
            var lastChild = parent.Children.LastOrDefault();
            var y = lastChild != null ? lastChild.Y + lastChild.Height + 20 : parent.Y;

            var newNode = MindMap.AddNode(text, parent, parent.X + parent.Width + 80, y);
            ClearSelection();
            newNode.IsSelected = true;
        }

        MindMap.RefreshLayout();
        Refresh();
    }

    void HideDraggingEdges()
    {
        if (MindMap == null) return;

        foreach (var edge in MindMap.Edges)
            if (_draggingNodes.Contains(edge.Source) || _draggingNodes.Contains(edge.Target))
                if (_edgeElements.TryGetValue(edge, out var path))
                    path.IsVisible = false;
    }

    void UpdateDropTarget(Point pointerPosition)
    {
        if (MindMap == null || _canvas == null || _mCanvas == null) return;

        var canvasPos = _mCanvas.CanvasToWorld(pointerPosition);
        MindMapNode? newTarget = null;

        foreach (var node in MindMap.Nodes)
        {
            if (_draggingNodes.Contains(node)) continue;

            if (canvasPos.X >= node.X && canvasPos.X <= node.X + node.Width &&
                canvasPos.Y >= node.Y && canvasPos.Y <= node.Y + node.Height)
            {
                // Validate: cannot reparent to a descendant of any dragged node
                var isDescendant = false;
                foreach (var dragged in _draggingNodes)
                    if (MindMapGraph.IsDescendantOf(node, dragged))
                    {
                        isDescendant = true;
                        break;
                    }

                if (!isDescendant)
                {
                    newTarget = node;
                    break;
                }
            }
        }

        if (newTarget != _dropTarget)
        {
            ClearDropTarget();

            if (newTarget != null && _nodeElements.TryGetValue(newTarget, out var border))
            {
                _dropTarget = newTarget;
                _dropTargetBorder = border;
                border.BorderBrush = SelectedBrush;
                border.BorderThickness = new(3);
            }
        }
    }

    void ClearDropTarget()
    {
        if (_dropTarget != null && _dropTargetBorder != null)
        {
            _dropTargetBorder.BorderBrush = _dropTarget.BorderBrush;
            _dropTargetBorder.BorderThickness = new(1);

            if (_dropTarget.IsSelected && SelectedBrush != null)
            {
                _dropTargetBorder.BorderBrush = SelectedBrush;
                _dropTargetBorder.BorderThickness = new(2);
            }
        }

        _dropTarget = null;
        _dropTargetBorder = null;
    }

    void StartBoxSelection(PointerPressedEventArgs e)
    {
        if (_canvas == null || _mCanvas == null) return;

        _isBoxSelecting = true;
        _boxSelectStart = e.GetPosition(_canvas);

        _selectionRect = new()
        {
            BorderBrush = SelectedBrush ?? Brushes.DodgerBlue,
            BorderThickness = new(1),
            Background = SelectedBrush is ISolidColorBrush scb
                ? new(scb.Color, 0.1)
                : new SolidColorBrush(Colors.DodgerBlue, 0.1),
            IsHitTestVisible = false
        };

        Canvas.SetLeft(_selectionRect, _boxSelectStart.X);
        Canvas.SetTop(_selectionRect, _boxSelectStart.Y);
        _canvas.Children.Add(_selectionRect);

        e.Pointer.Capture(_canvas);
    }

    void UpdateBoxSelection(Point currentPos)
    {
        if (_selectionRect == null || _canvas == null || MindMap == null) return;

        var x = Math.Min(_boxSelectStart.X, currentPos.X);
        var y = Math.Min(_boxSelectStart.Y, currentPos.Y);
        var w = Math.Abs(currentPos.X - _boxSelectStart.X);
        var h = Math.Abs(currentPos.Y - _boxSelectStart.Y);

        Canvas.SetLeft(_selectionRect, x);
        Canvas.SetTop(_selectionRect, y);
        _selectionRect.Width = w;
        _selectionRect.Height = h;

        var selectionBounds = new Rect(x, y, w, h);

        foreach (var node in MindMap.Nodes)
        {
            var nodeBounds = new Rect(node.X, node.Y, node.Width, node.Height);
            node.IsSelected = selectionBounds.Intersects(nodeBounds);
        }

        foreach (var node in MindMap.Nodes)
            RefreshNode(node);
    }

    void EndBoxSelection()
    {
        if (_selectionRect != null && _canvas != null)
        {
            _canvas.Children.Remove(_selectionRect);
            _selectionRect = null;
        }

        _isBoxSelecting = false;
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
