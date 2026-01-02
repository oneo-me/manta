using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Manta.Blueprint.Execution;
using Manta.Blueprint.Models;
using Manta.Blueprint.TypeSystem;

namespace Manta.Blueprint.Controls;

public class MBlueprint : TemplatedControl
{
    public static readonly StyledProperty<Graph?> GraphProperty =
        AvaloniaProperty.Register<MBlueprint, Graph?>(nameof(Graph));

    public static readonly StyledProperty<NodeRegistry?> RegistryProperty =
        AvaloniaProperty.Register<MBlueprint, NodeRegistry?>(nameof(Registry));

    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<MBlueprint, double>(nameof(Zoom), 1.0,
            coerce: (_, v) => Math.Clamp(v, MinZoom, MaxZoom));

    public static readonly StyledProperty<Point> OffsetProperty =
        AvaloniaProperty.Register<MBlueprint, Point>(nameof(Offset));

    public static readonly StyledProperty<bool> IsExecutingProperty =
        AvaloniaProperty.Register<MBlueprint, bool>(nameof(IsExecuting));

    public const double MinZoom = 0.1;
    public const double MaxZoom = 10.0;
    public const double ZoomFactor = 1.05; // 5% zoom per scroll step for smoother zooming

    Canvas? _canvas;
    Panel? _transformContainer;
    MBlueprintConnectionLayer? _connectionLayer;
    MBlueprintGridBackground? _gridBackground;
    bool _isPanning;
    Point _lastPanPosition;
    GraphExecutor? _executor;
    bool _isSpaceHeld;

    // Connection dragging state
    Port? _dragStartPort;
    Point _dragEndPoint;
    bool _isDraggingConnection;

    public Graph? Graph
    {
        get => GetValue(GraphProperty);
        set => SetValue(GraphProperty, value);
    }

    public NodeRegistry? Registry
    {
        get => GetValue(RegistryProperty);
        set => SetValue(RegistryProperty, value);
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

    public bool IsExecuting
    {
        get => GetValue(IsExecutingProperty);
        private set => SetValue(IsExecutingProperty, value);
    }

    public TypeCompatibility TypeCompatibility
    {
        get => field ??= TypeCompatibility.CreateDefault();
        set;
    }

    public bool IsDraggingConnection => _isDraggingConnection;
    public Port? DragStartPort => _dragStartPort;
    public Point DragEndPoint => _dragEndPoint;

    public event EventHandler<Node>? NodeSelected;
    public event EventHandler<Connection>? ConnectionSelected;
    public event EventHandler? SelectionCleared;
    public event EventHandler<GraphExecutionResult>? ExecutionCompleted;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _canvas = e.NameScope.Find<Canvas>("PART_Canvas");
        _transformContainer = e.NameScope.Find<Panel>("PART_TransformContainer");
        _connectionLayer = e.NameScope.Find<MBlueprintConnectionLayer>("PART_ConnectionLayer");
        _gridBackground = e.NameScope.Find<MBlueprintGridBackground>("PART_GridBackground");

        if (_connectionLayer != null)
        {
            _connectionLayer.Blueprint = this;
        }

        RefreshNodes();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == GraphProperty)
        {
            RefreshNodes();
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
            // Zoom towards mouse position
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

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        // Take focus so we can receive keyboard events
        Focus();

        var props = e.GetCurrentPoint(this).Properties;
        var isClickOnCanvas = !IsClickOnNode(e.Source);

        if (props.IsLeftButtonPressed)
        {
            // Space held = pan mode regardless of where clicked
            if (_isSpaceHeld)
            {
                _isPanning = true;
                _lastPanPosition = e.GetPosition(this);
                e.Pointer.Capture(this);
                e.Handled = true;
            }
            else if (isClickOnCanvas)
            {
                // Left click on empty canvas - clear selection and start panning
                ClearSelection();
                _isPanning = true;
                _lastPanPosition = e.GetPosition(this);
                e.Pointer.Capture(this);
                e.Handled = true;
            }
        }
    }

    static bool IsClickOnNode(object? source)
    {
        var current = source as Control;
        while (current != null)
        {
            if (current is MBlueprintNode or MBlueprintPort)
                return true;
            current = current.Parent as Control;
        }
        return false;
    }

    void ClearSelection()
    {
        Graph?.ClearSelection();
        UpdateNodeSelectionStates();
        SelectionCleared?.Invoke(this, EventArgs.Empty);
    }

    void UpdateNodeSelectionStates()
    {
        if (_canvas == null) return;

        foreach (var child in _canvas.Children)
        {
            if (child is MBlueprintNode nodeControl && nodeControl.Node != null)
            {
                nodeControl.IsSelected = nodeControl.Node.IsSelected;
            }
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (_isPanning)
        {
            var position = e.GetPosition(this);
            var delta = position - _lastPanPosition;
            Offset = new Point(Offset.X + delta.X, Offset.Y + delta.Y);
            _lastPanPosition = position;
            UpdateTransform();
            e.Handled = true;
        }
        else if (_isDraggingConnection)
        {
            _dragEndPoint = e.GetPosition(this);
            _connectionLayer?.InvalidateVisual();
            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (_isPanning)
        {
            _isPanning = false;
            e.Pointer.Capture(null);
            e.Handled = true;
        }
        else if (_isDraggingConnection)
        {
            // Try to find a port at the release position
            var releasePosition = e.GetPosition(this);
            var targetPort = FindPortAtPosition(releasePosition);

            if (targetPort != null && targetPort.IsInput)
            {
                CompleteConnectionDrag(targetPort);
            }
            else
            {
                CancelConnectionDrag();
            }
            e.Handled = true;
        }
    }

    Port? FindPortAtPosition(Point canvasPosition)
    {
        if (_canvas == null) return null;

        foreach (var child in _canvas.Children)
        {
            if (child is not MBlueprintNode nodeControl) continue;

            // Convert position to node's local coordinates
            var nodePos = nodeControl.TranslatePoint(new Point(0, 0), this);
            if (!nodePos.HasValue) continue;

            var localPos = new Point(
                (canvasPosition.X - nodePos.Value.X) / Zoom,
                (canvasPosition.Y - nodePos.Value.Y) / Zoom
            );

            var port = nodeControl.GetPortAtPosition(localPos);
            if (port != null) return port;
        }

        return null;
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
                    Cursor = new Avalonia.Input.Cursor(StandardCursorType.Hand);
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
            Cursor = Avalonia.Input.Cursor.Default;
            e.Handled = true;
        }
    }

    public void ResetView()
    {
        Zoom = 1.0;
        Offset = default;
        UpdateTransform();
    }

    public void ZoomIn()
    {
        var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
        var newZoom = Math.Min(Zoom * ZoomFactor, MaxZoom);
        var factor = newZoom / Zoom;
        var newOffset = new Point(
            center.X - (center.X - Offset.X) * factor,
            center.Y - (center.Y - Offset.Y) * factor
        );

        Zoom = newZoom;
        Offset = newOffset;
        UpdateTransform();
    }

    public void ZoomOut()
    {
        var center = new Point(Bounds.Width / 2, Bounds.Height / 2);
        var newZoom = Math.Max(Zoom / ZoomFactor, MinZoom);
        var factor = newZoom / Zoom;
        var newOffset = new Point(
            center.X - (center.X - Offset.X) * factor,
            center.Y - (center.Y - Offset.Y) * factor
        );

        Zoom = newZoom;
        Offset = newOffset;
        UpdateTransform();
    }

    public void DeleteSelected()
    {
        if (Graph == null) return;

        var selectedNode = Graph.SelectedNode;
        if (selectedNode != null)
        {
            Graph.RemoveNode(selectedNode);
            RefreshNodes();
            return;
        }

        var selectedConnection = Graph.SelectedConnection;
        if (selectedConnection != null)
        {
            Graph.RemoveConnection(selectedConnection);
            _connectionLayer?.InvalidateVisual();
        }
    }

    public void DeleteNode(Node node)
    {
        if (Graph == null) return;

        Graph.RemoveNode(node);
        RefreshNodes();
    }

    public Node? AddNode(NodeTypeInfo typeInfo, Point canvasPosition)
    {
        if (Graph == null) return null;

        var graphPosition = CanvasToGraph(canvasPosition);
        var node = Graph.AddNode(typeInfo, graphPosition);
        RefreshNodes();
        return node;
    }

    public async Task<GraphExecutionResult> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if (Graph == null)
        {
            return new GraphExecutionResult
            {
                IsSuccess = false,
                ErrorMessage = "No graph to execute"
            };
        }

        IsExecuting = true;

        try
        {
            _executor ??= new GraphExecutor(TypeCompatibility);
            var result = await _executor.ExecuteAsync(Graph, cancellationToken);
            ExecutionCompleted?.Invoke(this, result);
            return result;
        }
        finally
        {
            IsExecuting = false;
        }
    }

    public Point CanvasToGraph(Point canvasPoint)
    {
        return new Point(
            (canvasPoint.X - Offset.X) / Zoom,
            (canvasPoint.Y - Offset.Y) / Zoom
        );
    }

    public Point GraphToCanvas(Point graphPoint)
    {
        return new Point(
            graphPoint.X * Zoom + Offset.X,
            graphPoint.Y * Zoom + Offset.Y
        );
    }

    internal void SelectNode(Node node)
    {
        Graph?.SelectNode(node);
        UpdateNodeSelectionStates();
        NodeSelected?.Invoke(this, node);
    }

    internal void SelectConnection(Connection connection)
    {
        Graph?.SelectConnection(connection);
        ConnectionSelected?.Invoke(this, connection);
    }

    internal void InvalidateConnections()
    {
        _connectionLayer?.InvalidateVisual();
    }

    internal void StartConnectionDrag(Port sourcePort)
    {
        if (sourcePort.IsInput) return;

        _dragStartPort = sourcePort;
        _dragEndPoint = GraphToCanvas(sourcePort.Position);
        _isDraggingConnection = true;
    }

    internal void CompleteConnectionDrag(Port targetPort)
    {
        if (!_isDraggingConnection || _dragStartPort == null || Graph == null)
        {
            CancelConnectionDrag();
            return;
        }

        if (!targetPort.IsInput || targetPort.Owner == _dragStartPort.Owner)
        {
            CancelConnectionDrag();
            return;
        }

        // Check type compatibility
        if (!TypeCompatibility.AreTypesCompatible(_dragStartPort.DataType, targetPort.DataType))
        {
            CancelConnectionDrag();
            return;
        }

        // Create the connection
        Graph.AddConnection(_dragStartPort, targetPort);
        CancelConnectionDrag();
    }

    internal void CancelConnectionDrag()
    {
        _isDraggingConnection = false;
        _dragStartPort = null;
        _connectionLayer?.InvalidateVisual();
    }

    internal bool IsPortCompatibleWithDrag(Port port)
    {
        if (!_isDraggingConnection || _dragStartPort == null)
            return false;

        if (!port.IsInput)
            return false;

        if (port.Owner == _dragStartPort.Owner)
            return false;

        return TypeCompatibility.AreTypesCompatible(_dragStartPort.DataType, port.DataType);
    }

    /// <summary>
    /// Refreshes all node controls. Call this after modifying the Graph.Nodes collection.
    /// </summary>
    public void Refresh()
    {
        RefreshNodes();
    }

    void RefreshNodes()
    {
        if (_canvas == null || Graph == null) return;

        _canvas.Children.Clear();

        foreach (var node in Graph.Nodes)
        {
            var nodeControl = new MBlueprintNode
            {
                Node = node,
                Blueprint = this
            };

            Canvas.SetLeft(nodeControl, node.Position.X);
            Canvas.SetTop(nodeControl, node.Position.Y);

            _canvas.Children.Add(nodeControl);
        }

        UpdateTransform();
    }

    void UpdateTransform()
    {
        if (_transformContainer == null) return;

        // Scale first, then translate
        // This matches how Offset is calculated: canvas position = graph position * zoom + offset
        _transformContainer.RenderTransform = new TransformGroup
        {
            Children =
            {
                new ScaleTransform(Zoom, Zoom),
                new TranslateTransform(Offset.X, Offset.Y)
            }
        };

        // Update grid background to match zoom and offset
        if (_gridBackground != null)
        {
            _gridBackground.Zoom = Zoom;
            _gridBackground.Offset = Offset;
        }

        _connectionLayer?.InvalidateVisual();
    }
}
