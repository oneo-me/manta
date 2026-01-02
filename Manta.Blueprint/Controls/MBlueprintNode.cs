using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Manta.Blueprint.Models;

namespace Manta.Blueprint.Controls;

public class MBlueprintNode : TemplatedControl
{
    public static readonly StyledProperty<Node?> NodeProperty =
        AvaloniaProperty.Register<MBlueprintNode, Node?>(nameof(Node));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<MBlueprintNode, bool>(nameof(IsSelected));

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

    public MBlueprint? Blueprint { get; set; }

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
        if (Blueprint != null && Node != null)
            Blueprint.DeleteNode(Node);
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
            if (Blueprint != null && Node != null)
            {
                Blueprint.SelectNode(Node);
                IsSelected = true;
            }

            // Start dragging
            _isDragging = true;
            _dragStartPosition = e.GetPosition(Blueprint);
            _nodeStartPosition = Node?.Position ?? default;
            e.Pointer.Capture(this);
            e.Handled = true;
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (_isDragging && Node != null && Blueprint != null)
        {
            var currentPosition = e.GetPosition(Blueprint);
            var delta = currentPosition - _dragStartPosition;

            // Convert delta to graph coordinates
            var scaledDelta = new Point(delta.X / Blueprint.Zoom, delta.Y / Blueprint.Zoom);
            var newPosition = new Point(
                _nodeStartPosition.X + scaledDelta.X,
                _nodeStartPosition.Y + scaledDelta.Y
            );

            Node.Position = newPosition;
            Canvas.SetLeft(this, newPosition.X);
            Canvas.SetTop(this, newPosition.Y);

            // Update port positions and redraw connections
            UpdatePortPositions();
            Blueprint.InvalidateConnections();

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
                if (item is MBlueprintPort { Port: not null } portControl)
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
                if (item is MBlueprintPort { Port: not null } portControl)
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
        if (Blueprint == null) return;

        UpdatePortPositionsForContainer(_inputPortsContainer);
        UpdatePortPositionsForContainer(_outputPortsContainer);
    }

    void UpdatePortPositionsForContainer(ItemsControl? container)
    {
        if (container == null) return;

        foreach (var item in container.GetRealizedContainers())
            if (item is MBlueprintPort portControl)
                portControl.UpdatePosition();
    }

    void UpdatePorts()
    {
        if (Node == null) return;

        // Create port controls and set them as ItemsSource
        // We need to create the controls ourselves to set the Blueprint reference
        if (_inputPortsContainer != null)
            _inputPortsContainer.ItemsSource = Node.InputPorts.Select(p => new MBlueprintPort
            {
                Port = p,
                Blueprint = Blueprint
            }).ToList();

        if (_outputPortsContainer != null)
            _outputPortsContainer.ItemsSource = Node.OutputPorts.Select(p => new MBlueprintPort
            {
                Port = p,
                Blueprint = Blueprint
            }).ToList();

        // Schedule port position update after layout is complete
        Dispatcher.UIThread.Post(() =>
        {
            UpdatePortPositions();
            Blueprint?.InvalidateConnections();
        }, DispatcherPriority.Loaded);
    }
}
