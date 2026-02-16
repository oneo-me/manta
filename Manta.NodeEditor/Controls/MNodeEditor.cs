using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using Manta.NodeEditor.Models;
using Manta.NodeEditor.TypeSystem;

namespace Manta.NodeEditor.Controls;

public class MNodeEditor : TemplatedControl
{
    public static readonly StyledProperty<NodeGraph?> GraphProperty =
        AvaloniaProperty.Register<MNodeEditor, NodeGraph?>(nameof(Graph));

    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<MNodeEditor, double>(nameof(Zoom), 1.0,
            coerce: (_, v) => Math.Clamp(v, 0.1, 5.0));

    public static readonly StyledProperty<Point> OffsetProperty =
        AvaloniaProperty.Register<MNodeEditor, Point>(nameof(Offset));

    Canvas? _canvas;
    Panel? _transformContainer;
    MNodeEditorConnectionLayer? _connectionLayer;
    MNodeEditorGrid? _grid;

    bool _isPanning;
    Point _lastPanPosition;
    bool _isSpaceHeld;

    // Connection drag state
    public bool IsDraggingConnection { get; private set; }
    public NodePort? DragSourcePort { get; private set; }
    public Point DragEndPoint { get; private set; }

    public TypeCompatibility TypeCompatibility
    {
        get => field ??= TypeCompatibility.CreateDefault();
        set;
    }

    // Clipboard for copy/paste
    readonly List<NodeBase> _clipboard = [];

    public static readonly StyledProperty<NodeRegistry?> RegistryProperty =
        AvaloniaProperty.Register<MNodeEditor, NodeRegistry?>(nameof(Registry));

    public NodeRegistry? Registry
    {
        get => GetValue(RegistryProperty);
        set => SetValue(RegistryProperty, value);
    }

    public NodeGraph? Graph
    {
        get => GetValue(GraphProperty);
        set => SetValue(GraphProperty, value);
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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _canvas = e.NameScope.Find<Canvas>("PART_Canvas");
        _transformContainer = e.NameScope.Find<Panel>("PART_TransformContainer");
        _connectionLayer = e.NameScope.Find<MNodeEditorConnectionLayer>("PART_ConnectionLayer");
        _grid = e.NameScope.Find<MNodeEditorGrid>("PART_Grid");

        if (_connectionLayer is not null)
            _connectionLayer.NodeEditor = this;

        SubscribeToGraph();
        SyncAllNodes();
        UpdateTransform();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == GraphProperty)
        {
            UnsubscribeFromGraph(change.GetOldValue<NodeGraph?>());
            SubscribeToGraph();
            SyncAllNodes();
        }
        else if (change.Property == ZoomProperty || change.Property == OffsetProperty)
        {
            UpdateTransform();
        }
    }

    void SubscribeToGraph()
    {
        if (Graph is null) return;
        Graph.Nodes.CollectionChanged += OnNodesCollectionChanged;
        Graph.Connections.CollectionChanged += OnConnectionsCollectionChanged;
    }

    void UnsubscribeFromGraph(NodeGraph? oldGraph)
    {
        if (oldGraph is null) return;
        oldGraph.Nodes.CollectionChanged -= OnNodesCollectionChanged;
        oldGraph.Connections.CollectionChanged -= OnConnectionsCollectionChanged;
    }

    void OnNodesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems is not null)
                    foreach (NodeBase node in e.NewItems)
                        AddNodeControl(node);
                break;
            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems is not null)
                    foreach (NodeBase node in e.OldItems)
                        RemoveNodeControl(node);
                break;
            case NotifyCollectionChangedAction.Reset:
                SyncAllNodes();
                break;
        }
    }

    void OnConnectionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        _connectionLayer?.InvalidateVisual();
    }

    void SyncAllNodes()
    {
        if (_canvas is null) return;

        _canvas.Children.Clear();
        if (Graph is null) return;

        foreach (var node in Graph.Nodes)
            AddNodeControl(node);

        UpdateTransform();
    }

    void AddNodeControl(NodeBase node)
    {
        if (_canvas is null) return;

        var control = new MNodeEditorNode
        {
            NodeData = node,
            NodeEditor = this
        };
        Canvas.SetLeft(control, node.X);
        Canvas.SetTop(control, node.Y);

        // Sync position when model changes
        node.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(NodeBase.X))
                Canvas.SetLeft(control, node.X);
            else if (e.PropertyName == nameof(NodeBase.Y))
                Canvas.SetTop(control, node.Y);
        };

        _canvas.Children.Add(control);

        // Update port positions after layout
        control.Loaded += (_, _) =>
        {
            control.UpdateAllPortPositions();
            _connectionLayer?.InvalidateVisual();
        };
    }

    void RemoveNodeControl(NodeBase node)
    {
        if (_canvas is null) return;

        var control = _canvas.Children.OfType<MNodeEditorNode>()
            .FirstOrDefault(c => c.NodeData == node);

        if (control is not null)
            _canvas.Children.Remove(control);

        _connectionLayer?.InvalidateVisual();
    }

    void UpdateTransform()
    {
        if (_transformContainer is not null)
        {
            _transformContainer.RenderTransform = new TransformGroup
            {
                Children =
                {
                    new ScaleTransform(Zoom, Zoom),
                    new TranslateTransform(Offset.X, Offset.Y)
                }
            };
        }

        if (_grid is not null)
        {
            _grid.Zoom = Zoom;
            _grid.Offset = Offset;
        }
    }

    // --- Zoom / Pan ---

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        var position = e.GetPosition(this);
        var delta = e.Delta.Y > 0 ? 1.1 : 1 / 1.1;
        var newZoom = Math.Clamp(Zoom * delta, 0.1, 5.0);

        // Zoom towards mouse position
        var factor = newZoom / Zoom;
        var newOffsetX = position.X - (position.X - Offset.X) * factor;
        var newOffsetY = position.Y - (position.Y - Offset.Y) * factor;

        Zoom = newZoom;
        Offset = new Point(newOffsetX, newOffsetY);

        e.Handled = true;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        Focus();

        var props = e.GetCurrentPoint(this).Properties;

        // Middle mouse button: pan
        if (props.IsMiddleButtonPressed)
        {
            _isPanning = true;
            _lastPanPosition = e.GetPosition(this);
            e.Pointer.Capture(this);
            Cursor = new Cursor(StandardCursorType.Hand);
            e.Handled = true;
            return;
        }

        // Left button
        if (props.IsLeftButtonPressed)
        {
            // Space held: pan mode
            if (_isSpaceHeld)
            {
                _isPanning = true;
                _lastPanPosition = e.GetPosition(this);
                e.Pointer.Capture(this);
                e.Handled = true;
                return;
            }

            // Click on empty area
            if (!IsClickOnNodeOrPort(e.Source as Control))
            {
                // Check connection hit
                var graphPoint = CanvasToGraph(e.GetPosition(this));
                var hitConnection = _connectionLayer?.HitTestConnection(graphPoint);
                if (hitConnection is not null)
                {
                    Graph?.ClearSelection();
                    hitConnection.IsSelected = true;
                    e.Handled = true;
                    return;
                }

                // Clear selection
                Graph?.ClearSelection();
                e.Handled = true;
            }
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (_isPanning)
        {
            var pos = e.GetPosition(this);
            var delta = pos - _lastPanPosition;
            Offset = new Point(Offset.X + delta.X, Offset.Y + delta.Y);
            _lastPanPosition = pos;
            e.Handled = true;
            return;
        }

        if (IsDraggingConnection)
        {
            DragEndPoint = CanvasToGraph(e.GetPosition(this));
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
            Cursor = Cursor.Default;
            e.Handled = true;
        }

        if (IsDraggingConnection)
        {
            // Check if released on an input port
            var hitPort = FindPortAtPosition(e.GetPosition(this));
            if (hitPort is { IsInput: true } && DragSourcePort is not null)
                CompleteConnectionDrag(hitPort);
            else
                CancelConnectionDrag();

            e.Handled = true;
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch (e.Key)
        {
            case Key.Space:
                _isSpaceHeld = true;
                Cursor = new Cursor(StandardCursorType.Hand);
                e.Handled = true;
                break;

            case Key.Delete:
            case Key.Back:
                Graph?.DeleteSelected();
                e.Handled = true;
                break;

            case Key.Escape:
                if (IsDraggingConnection)
                    CancelConnectionDrag();
                else
                    Graph?.ClearSelection();
                e.Handled = true;
                break;

            case Key.C when e.KeyModifiers.HasFlag(KeyModifiers.Control):
                CopySelectedNodes();
                e.Handled = true;
                break;

            case Key.V when e.KeyModifiers.HasFlag(KeyModifiers.Control):
                PasteNodes();
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

    // --- Connection Drag API ---

    internal void StartConnectionDrag(NodePort sourcePort)
    {
        IsDraggingConnection = true;
        DragSourcePort = sourcePort;
        DragEndPoint = new Point(sourcePort.PortCenterX, sourcePort.PortCenterY);
    }

    internal void CompleteConnectionDrag(NodePort targetPort)
    {
        if (DragSourcePort is not null && targetPort.IsInput)
        {
            if (IsConnectionCompatible(DragSourcePort, targetPort))
                Graph?.Connect(DragSourcePort, targetPort);
        }

        CancelConnectionDrag();
    }

    internal void CancelConnectionDrag()
    {
        IsDraggingConnection = false;
        DragSourcePort = null;
        _connectionLayer?.InvalidateVisual();
    }

    internal bool IsConnectionCompatible(NodePort source, NodePort target)
    {
        if (source.Owner == target.Owner) return false;
        return TypeCompatibility.AreTypesCompatible(source.DataType, target.DataType);
    }

    // --- Selection ---

    internal void SelectNode(NodeBase node)
    {
        Graph?.ClearSelection();
        node.IsSelected = true;
    }

    // --- Coordinate Conversion ---

    public Point CanvasToGraph(Point canvasPoint) => new(
        (canvasPoint.X - Offset.X) / Zoom,
        (canvasPoint.Y - Offset.Y) / Zoom);

    public Point GraphToCanvas(Point graphPoint) => new(
        graphPoint.X * Zoom + Offset.X,
        graphPoint.Y * Zoom + Offset.Y);

    // --- Invalidation ---

    internal void InvalidateConnections()
    {
        _connectionLayer?.InvalidateVisual();
    }

    // --- Utilities ---

    static bool IsClickOnNodeOrPort(Control? source)
    {
        var current = source;
        while (current is not null)
        {
            if (current is MNodeEditorNode or MNodeEditorPort)
                return true;
            current = current.Parent as Control;
        }

        return false;
    }

    NodePort? FindPortAtPosition(Point position)
    {
        if (_canvas is null) return null;

        foreach (var child in _canvas.Children.OfType<MNodeEditorNode>())
        {
            var ports = child.GetVisualDescendants().OfType<MNodeEditorPort>();
            foreach (var portControl in ports)
            {
                if (portControl.Port is null) continue;

                var topLeft = portControl.TranslatePoint(new Point(0, 0), this);
                var bottomRight = portControl.TranslatePoint(
                    new Point(portControl.Bounds.Width, portControl.Bounds.Height), this);

                if (topLeft.HasValue && bottomRight.HasValue)
                {
                    var rect = new Rect(topLeft.Value, bottomRight.Value);
                    if (rect.Contains(position))
                        return portControl.Port;
                }
            }
        }

        return null;
    }

    // --- Copy / Paste ---

    void CopySelectedNodes()
    {
        if (Graph is null) return;
        _clipboard.Clear();
        _clipboard.AddRange(Graph.Nodes.Where(n => n.IsSelected));
    }

    void PasteNodes()
    {
        if (Graph is null || Registry is null || _clipboard.Count == 0) return;

        Graph.ClearSelection();

        foreach (var original in _clipboard)
        {
            var nodeType = original.GetType();
            if (!Registry.IsRegistered(nodeType)) continue;

            var copy = Registry.CreateNode(nodeType, original.X + 30, original.Y + 30);

            // Copy property values
            var properties = nodeType.GetProperties(
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (!prop.CanRead || !prop.CanWrite) continue;
                if (prop.DeclaringType == typeof(NodeBase) || prop.DeclaringType == typeof(Reactive.Views.Model))
                    continue;

                try
                {
                    prop.SetValue(copy, prop.GetValue(original));
                }
                catch
                {
                    // Ignore properties that can't be copied
                }
            }

            Graph.AddNode(copy);
            copy.IsSelected = true;
        }
    }

    // --- Public API ---

    public void ResetView()
    {
        Zoom = 1.0;
        Offset = default;
    }
}
