using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using Manta.NodeEditor.Core;
using Manta.NodeEditor.TypeSystem;

namespace Manta.NodeEditor.Rendering;

/// <summary>
/// Main node editor surface. Provides zoom, pan, node management,
/// connection drag, and keyboard shortcuts.
/// </summary>
public class NodeCanvas : TemplatedControl
{
    public static readonly StyledProperty<NodeGraph?> GraphProperty =
        AvaloniaProperty.Register<NodeCanvas, NodeGraph?>(nameof(Graph));

    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<NodeCanvas, double>(nameof(Zoom), 1.0);

    public static readonly StyledProperty<TypeCompatibility?> TypeCompatibilityProperty =
        AvaloniaProperty.Register<NodeCanvas, TypeCompatibility?>(nameof(TypeCompatibility));

    Panel? _transformContainer;
    Canvas? _nodeLayer;
    GridRenderer? _grid;
    ConnectionRenderer? _connections;
    Point _offset;
    bool _panning;
    Point _panStart;
    Point _panOffsetStart;
    Point _lastPointerPosition;

    // Connection drag state
    bool _isDragging;
    Port? _dragSource;
    Port? _dragHoverPort;
    Point _dragEnd; // in screen (canvas-local) coordinates

    // Copy buffer
    Type? _copyType;

    public NodeGraph? Graph
    {
        get => GetValue(GraphProperty);
        set => SetValue(GraphProperty, value);
    }

    public double Zoom
    {
        get => GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, Math.Clamp(value, 0.1, 5.0));
    }

    public TypeCompatibility? TypeCompatibility
    {
        get => GetValue(TypeCompatibilityProperty);
        set => SetValue(TypeCompatibilityProperty, value);
    }

    public Point Offset
    {
        get => _offset;
        private set
        {
            _offset = value;
            ApplyTransform();
            _grid?.SetValue(GridRenderer.OffsetProperty, value);
            _grid?.InvalidateVisual();
        }
    }

    internal bool IsDragging => _isDragging;
    internal Port? DragSource => _dragSource;
    internal Port? DragHoverPort => _dragHoverPort;
    internal Point DragEnd => _dragEnd;

    /// <summary>
    /// Check if two ports are type-compatible. Returns true if no TypeCompatibility is set.
    /// </summary>
    internal bool IsConnectionCompatible(Port source, Port target)
    {
        var tc = TypeCompatibility;
        if (tc is null) return true;
        return tc.AreTypesCompatible(source.ValueType, target.ValueType);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _transformContainer = e.NameScope.Find<Panel>("PART_Transform");
        _nodeLayer = e.NameScope.Find<Canvas>("PART_NodeLayer");
        _grid = e.NameScope.Find<GridRenderer>("PART_Grid");
        _connections = e.NameScope.Find<ConnectionRenderer>("PART_Connections");

        if (_connections is not null)
            _connections.Canvas = this;

        Focusable = true;
        ClipToBounds = true;

        AddHandler(Gestures.PointerTouchPadGestureMagnifyEvent, OnMagnify, RoutingStrategies.Bubble);

        ApplyTransform();
        RebuildNodes();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == GraphProperty)
        {
            if (change.GetOldValue<NodeGraph?>() is { } oldGraph)
            {
                oldGraph.Nodes.CollectionChanged -= OnNodesChanged;
                oldGraph.Connections.CollectionChanged -= OnConnectionsChanged;
            }

            if (change.GetNewValue<NodeGraph?>() is { } newGraph)
            {
                newGraph.Nodes.CollectionChanged += OnNodesChanged;
                newGraph.Connections.CollectionChanged += OnConnectionsChanged;
                newGraph.TypeCompatibility = TypeCompatibility;
            }

            RebuildNodes();
        }
        else if (change.Property == TypeCompatibilityProperty)
        {
            if (Graph is not null)
                Graph.TypeCompatibility = TypeCompatibility;
        }
        else if (change.Property == ZoomProperty)
        {
            ApplyTransform();
            _grid?.SetValue(GridRenderer.ZoomProperty, Zoom);
            _grid?.SetValue(GridRenderer.OffsetProperty, _offset);
            _grid?.InvalidateVisual();
        }
    }

    // --- Coordinate conversion ---

    /// <summary>
    /// Convert a point in canvas-local (screen) coordinates to graph coordinates.
    /// </summary>
    internal Point ScreenToGraph(Point canvasLocal)
    {
        return new Point(
            (canvasLocal.X - _offset.X) / Zoom,
            (canvasLocal.Y - _offset.Y) / Zoom);
    }

    /// <summary>
    /// Convert graph coordinates to canvas-local (screen) coordinates.
    /// </summary>
    internal Point GraphToScreen(double gx, double gy)
        => new(gx * Zoom + _offset.X, gy * Zoom + _offset.Y);

    // --- Connection drag ---

    /// <summary>
    /// Begin dragging a connection from an output port.
    /// Captures the pointer so all subsequent events come to this canvas.
    /// </summary>
    internal void BeginConnectionDrag(Port source, IPointer pointer)
    {
        _isDragging = true;
        _dragSource = source;
        _dragEnd = GraphToScreen(source.CenterX, source.CenterY);
        pointer.Capture(this);
        RefreshConnections();
    }

    /// <summary>
    /// Complete a connection drag to a target port.
    /// </summary>
    internal void EndConnectionDrag(Port target)
    {
        if (_isDragging && _dragSource is not null && Graph is not null)
            Graph.Connect(_dragSource, target);

        CancelDrag();
    }

    /// <summary>
    /// Cancel the current connection drag.
    /// </summary>
    void CancelDrag()
    {
        _isDragging = false;
        _dragSource = null;
        _dragHoverPort = null;
        RefreshConnections();
    }

    /// <summary>
    /// Set the port currently being hovered during a connection drag.
    /// </summary>
    internal void SetDragHoverPort(Port? port)
    {
        if (_dragHoverPort == port) return;
        _dragHoverPort = port;
        RefreshConnections();
    }

    /// <summary>
    /// Force redraw of connections overlay.
    /// </summary>
    internal void RefreshConnections()
    {
        _connections?.InvalidateVisual();
    }

    /// <summary>
    /// Find the nearest input port within tolerance of a screen-space point.
    /// </summary>
    Port? FindInputPortAt(Point screenPoint)
    {
        if (Graph is null) return null;

        const double tolerance = 20;

        foreach (var node in Graph.Nodes)
        {
            foreach (var port in node.Inputs)
            {
                var ps = GraphToScreen(port.CenterX, port.CenterY);
                if (Math.Abs(ps.X - screenPoint.X) < tolerance &&
                    Math.Abs(ps.Y - screenPoint.Y) < tolerance)
                    return port;
            }
        }

        return null;
    }

    // --- Transform ---

    void ApplyTransform()
    {
        if (_transformContainer is null) return;

        _transformContainer.RenderTransform = new TransformGroup
        {
            Children =
            {
                new ScaleTransform(Zoom, Zoom),
                new TranslateTransform(_offset.X, _offset.Y)
            }
        };
    }

    // --- Node management ---

    void RebuildNodes()
    {
        if (_nodeLayer is null) return;
        _nodeLayer.Children.Clear();

        if (Graph is null) return;

        foreach (var node in Graph.Nodes)
            AddNodeFrame(node);

        RefreshConnections();
    }

    void AddNodeFrame(NodeBase node)
    {
        if (_nodeLayer is null) return;

        var frame = new NodeFrame
        {
            Node = node,
            Canvas = this
        };

        Canvas.SetLeft(frame, node.X);
        Canvas.SetTop(frame, node.Y);

        _nodeLayer.Children.Add(frame);

        // Sync port positions after layout
        frame.LayoutUpdated += (_, _) =>
        {
            frame.SyncAllPorts();
            RefreshConnections();
        };
    }

    void RemoveNodeFrame(NodeBase node)
    {
        if (_nodeLayer is null) return;

        var frame = _nodeLayer.Children
            .OfType<NodeFrame>()
            .FirstOrDefault(f => f.Node == node);

        if (frame is not null)
            _nodeLayer.Children.Remove(frame);
    }

    void OnNodesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add:
                if (e.NewItems is not null)
                    foreach (NodeBase node in e.NewItems)
                        AddNodeFrame(node);
                break;

            case NotifyCollectionChangedAction.Remove:
                if (e.OldItems is not null)
                    foreach (NodeBase node in e.OldItems)
                        RemoveNodeFrame(node);
                break;

            case NotifyCollectionChangedAction.Reset:
                RebuildNodes();
                break;
        }

        RefreshConnections();
    }

    void OnConnectionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        RefreshConnections();
    }

    // --- Zoom ---

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        var hasZoomModifier = e.KeyModifiers.HasFlag(KeyModifiers.Control) ||
                              e.KeyModifiers.HasFlag(KeyModifiers.Meta);

        if (hasZoomModifier)
        {
            // Ctrl/Cmd + Scroll → Zoom
            var cursorPos = e.GetPosition(this);
            var graphBefore = ScreenToGraph(cursorPos);

            var delta = e.Delta.Y > 0 ? 1.1 : 1 / 1.1;
            var newZoom = Math.Clamp(Zoom * delta, 0.1, 5.0);

            // Set offset BEFORE Zoom so ApplyTransform uses consistent values
            _offset = new Point(
                cursorPos.X - graphBefore.X * newZoom,
                cursorPos.Y - graphBefore.Y * newZoom);
            Zoom = newZoom;
        }
        else
        {
            // Scroll → Pan
            const double panSpeed = 50;
            Offset = new Point(
                _offset.X + e.Delta.X * panSpeed,
                _offset.Y + e.Delta.Y * panSpeed);
        }

        SyncAllPortPositions();
        RefreshConnections();

        e.Handled = true;
    }

    // --- Pan ---

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var props = e.GetCurrentPoint(this).Properties;

        // Middle button for panning
        if (props.IsMiddleButtonPressed)
        {
            _panning = true;
            _panStart = e.GetPosition(this);
            _panOffsetStart = _offset;
            e.Pointer.Capture(this);
            e.Handled = true;
            return;
        }

        // Left click
        if (props.IsLeftButtonPressed)
        {
            // Check if clicked on a connection line (screen-space hit test)
            if (_connections is not null)
            {
                var screenPt = e.GetPosition(this);
                var hit = _connections.HitTest(screenPt);
                if (hit is not null)
                {
                    Graph?.ClearSelection();
                    hit.IsSelected = true;
                    RefreshConnections();
                    e.Handled = true;
                    return;
                }
            }

            // Click on empty area: clear selection and start panning
            if (e.Source == this || e.Source is GridRenderer || e.Source is Panel)
            {
                Graph?.ClearSelection();
                RefreshConnections();

                _panning = true;
                _panStart = e.GetPosition(this);
                _panOffsetStart = _offset;
                e.Pointer.Capture(this);
                e.Handled = true;
                return;
            }

            Focus();
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        _lastPointerPosition = e.GetPosition(this);

        if (_panning)
        {
            var pos = e.GetPosition(this);
            Offset = new Point(
                _panOffsetStart.X + (pos.X - _panStart.X),
                _panOffsetStart.Y + (pos.Y - _panStart.Y));

            SyncAllPortPositions();
            RefreshConnections();
            e.Handled = true;
            return;
        }

        if (_isDragging)
        {
            _dragEnd = e.GetPosition(this); // screen coordinates
            RefreshConnections();
            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (_panning)
        {
            _panning = false;
            e.Pointer.Capture(null);
            e.Handled = true;
            return;
        }

        if (_isDragging)
        {
            // Try to find an input port under the cursor
            var target = FindInputPortAt(e.GetPosition(this));
            if (target is not null)
                EndConnectionDrag(target);
            else
                CancelDrag();

            e.Pointer.Capture(null);
            e.Handled = true;
        }
    }

    // --- Trackpad pinch-to-zoom ---

    void OnMagnify(object? sender, PointerDeltaEventArgs e)
    {
        // Delta.X contains the magnification factor delta (e.g., 0.02 = slight zoom in)
        var zoomCenter = _lastPointerPosition;
        var graphBefore = ScreenToGraph(zoomCenter);

        var newZoom = Math.Clamp(Zoom * (1 + e.Delta.X), 0.1, 5.0);

        // Set offset BEFORE Zoom so ApplyTransform uses consistent values
        _offset = new Point(
            zoomCenter.X - graphBefore.X * newZoom,
            zoomCenter.Y - graphBefore.Y * newZoom);
        Zoom = newZoom;

        SyncAllPortPositions();
        RefreshConnections();

        e.Handled = true;
    }

    // --- Keyboard ---

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch (e.Key)
        {
            case Key.Delete or Key.Back:
                Graph?.DeleteSelected();
                RefreshConnections();
                e.Handled = true;
                break;

            case Key.Escape:
                if (_isDragging)
                    CancelDrag();
                else
                    Graph?.ClearSelection();
                RefreshConnections();
                e.Handled = true;
                break;

            case Key.C when e.KeyModifiers.HasFlag(KeyModifiers.Control):
                CopySelected();
                e.Handled = true;
                break;

            case Key.V when e.KeyModifiers.HasFlag(KeyModifiers.Control):
                PasteNode();
                e.Handled = true;
                break;
        }
    }

    // --- Copy / Paste ---

    void CopySelected()
    {
        if (Graph is null) return;
        var selected = Graph.Nodes.FirstOrDefault(n => n.IsSelected);
        if (selected is not null)
            _copyType = selected.GetType();
    }

    void PasteNode()
    {
        if (Graph is null || _copyType is null) return;

        if (Activator.CreateInstance(_copyType) is NodeBase newNode)
        {
            // Place slightly offset from center of view
            var center = ScreenToGraph(new Point(Bounds.Width / 2, Bounds.Height / 2));
            newNode.X = center.X + 20;
            newNode.Y = center.Y + 20;
            Graph.Add(newNode);
        }
    }

    // --- Helpers ---

    void SyncAllPortPositions()
    {
        if (_nodeLayer is null) return;

        foreach (var frame in _nodeLayer.Children.OfType<NodeFrame>())
            frame.SyncAllPorts();
    }
}
