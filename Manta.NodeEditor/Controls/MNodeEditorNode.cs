using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Manta.NodeEditor.Models;

namespace Manta.NodeEditor.Controls;

public class MNodeEditorNode : TemplatedControl
{
    public static readonly StyledProperty<Node?> NodeProperty =
        AvaloniaProperty.Register<MNodeEditorNode, Node?>(nameof(Node));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<MNodeEditorNode, bool>(nameof(IsSelected));

    ItemsControl? _inputPortsContainer;
    ItemsControl? _outputPortsContainer;
    Button? _deleteButton;
    bool _isDragging;
    Point _dragStartPosition;
    Point _nodeStartPosition;

    public Node? Node
    {
        get => GetValue(NodeProperty);
        set => SetValue(NodeProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public MNodeEditor? NodeEditor { get; set; }

    public string DisplayName => Node?.DisplayName ?? "Node";

    public IEnumerable<Port> InputPorts => Node?.InputPorts ?? [];
    public IEnumerable<Port> OutputPorts => Node?.OutputPorts ?? [];

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _inputPortsContainer = e.NameScope.Find<ItemsControl>("PART_InputPorts");
        _outputPortsContainer = e.NameScope.Find<ItemsControl>("PART_OutputPorts");
        _deleteButton = e.NameScope.Find<Button>("PART_DeleteButton");

        if (_deleteButton != null)
            _deleteButton.Click += OnDeleteButtonClick;

        UpdatePorts();
    }

    void OnDeleteButtonClick(object? sender, RoutedEventArgs e)
    {
        if (NodeEditor != null && Node != null)
            NodeEditor.DeleteNode(Node);
        e.Handled = true;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == NodeProperty)
        {
            UpdatePorts();
            if (Node != null)
                IsSelected = Node.IsSelected;
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        var props = e.GetCurrentPoint(this).Properties;

        if (props.IsLeftButtonPressed)
        {
            // Select this node
            if (NodeEditor != null && Node != null)
            {
                NodeEditor.SelectNode(Node);
                IsSelected = true;
            }

            // Start dragging
            _isDragging = true;
            _dragStartPosition = e.GetPosition(NodeEditor);
            _nodeStartPosition = Node?.Position ?? default;
            e.Pointer.Capture(this);
            e.Handled = true;
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (_isDragging && Node != null && NodeEditor != null)
        {
            var currentPosition = e.GetPosition(NodeEditor);
            var delta = currentPosition - _dragStartPosition;

            // Convert delta to graph coordinates
            var scaledDelta = new Point(delta.X / NodeEditor.Zoom, delta.Y / NodeEditor.Zoom);
            var newPosition = new Point(
                _nodeStartPosition.X + scaledDelta.X,
                _nodeStartPosition.Y + scaledDelta.Y
            );

            Node.Position = newPosition;
            Canvas.SetLeft(this, newPosition.X);
            Canvas.SetTop(this, newPosition.Y);

            // Update port positions and redraw connections
            UpdatePortPositions();
            NodeEditor.InvalidateConnections();

            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (_isDragging)
        {
            _isDragging = false;
            e.Pointer.Capture(null);
            e.Handled = true;
        }
    }

    internal Port? GetPortAtPosition(Point position)
    {
        // Check input ports
        if (_inputPortsContainer != null)
            foreach (var item in _inputPortsContainer.GetRealizedContainers())
                if (item is MNodeEditorPort { Port: not null } portControl)
                {
                    var bounds = portControl.Bounds;
                    var portPos = portControl.TranslatePoint(new(0, 0), this);
                    if (portPos.HasValue)
                    {
                        var rect = new Rect(portPos.Value, bounds.Size);
                        if (rect.Contains(position))
                            return portControl.Port;
                    }
                }

        // Check output ports
        if (_outputPortsContainer != null)
            foreach (var item in _outputPortsContainer.GetRealizedContainers())
                if (item is MNodeEditorPort { Port: not null } portControl)
                {
                    var bounds = portControl.Bounds;
                    var portPos = portControl.TranslatePoint(new(0, 0), this);
                    if (portPos.HasValue)
                    {
                        var rect = new Rect(portPos.Value, bounds.Size);
                        if (rect.Contains(position))
                            return portControl.Port;
                    }
                }

        return null;
    }

    internal void UpdatePortPositions()
    {
        if (NodeEditor == null) return;

        UpdatePortPositionsForContainer(_inputPortsContainer);
        UpdatePortPositionsForContainer(_outputPortsContainer);
    }

    void UpdatePortPositionsForContainer(ItemsControl? container)
    {
        if (container == null) return;

        foreach (var item in container.GetRealizedContainers())
            if (item is MNodeEditorPort portControl)
                portControl.UpdatePosition();
    }

    void UpdatePorts()
    {
        if (Node == null) return;

        // Create port controls and set them as ItemsSource
        // We need to create the controls ourselves to set the NodeEditor reference
        if (_inputPortsContainer != null)
            _inputPortsContainer.ItemsSource = Node.InputPorts.Select(p => new MNodeEditorPort
            {
                Port = p,
                NodeEditor = NodeEditor
            }).ToList();

        if (_outputPortsContainer != null)
            _outputPortsContainer.ItemsSource = Node.OutputPorts.Select(p => new MNodeEditorPort
            {
                Port = p,
                NodeEditor = NodeEditor
            }).ToList();

        // Schedule port position update after layout is complete
        UIThread.Post(() =>
        {
            UpdatePortPositions();
            NodeEditor?.InvalidateConnections();
        }, DispatcherPriority.Loaded);
    }
}
