using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using Manta.Controls;
using Manta.NodeEditor.Core;

namespace Manta.NodeEditor.Rendering;

/// <summary>
/// Visual wrapper for a single node. Renders title bar, ports, content, and selection state.
/// </summary>
public class NodeFrame : TemplatedControl
{
    public static readonly StyledProperty<NodeBase?> NodeProperty =
        AvaloniaProperty.Register<NodeFrame, NodeBase?>(nameof(Node));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<NodeFrame, bool>(nameof(IsSelected));

    public static readonly StyledProperty<Control?> ContentProperty =
        AvaloniaProperty.Register<NodeFrame, Control?>(nameof(Content));

    MItems? _inputPorts;
    ContentPresenter? _outputPort;
    Button? _deleteBtn;
    bool _dragging;
    Point _dragStart;
    double _startX, _startY;

    public NodeBase? Node
    {
        get => GetValue(NodeProperty);
        set => SetValue(NodeProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public Control? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    internal NodeCanvas? Canvas { get; set; }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _inputPorts = e.NameScope.Find<MItems>("PART_Inputs");
        _outputPort = e.NameScope.Find<ContentPresenter>("PART_Output");
        _deleteBtn = e.NameScope.Find<Button>("PART_Delete");

        if (_deleteBtn is not null)
            _deleteBtn.Click += (_, ev) =>
            {
                if (Node is not null)
                    Canvas?.Graph?.Remove(Node);
                ev.Handled = true;
            };

        Rebuild();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == NodeProperty)
        {
            if (change.GetOldValue<NodeBase?>() is { } old)
                old.PropertyChanged -= OnNodeChanged;
            if (change.GetNewValue<NodeBase?>() is { } n)
                n.PropertyChanged += OnNodeChanged;
            Rebuild();
        }
    }

    void OnNodeChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NodeBase.IsSelected))
            IsSelected = Node?.IsSelected ?? false;
    }

    void Rebuild()
    {
        if (Node is null) return;

        IsSelected = Node.IsSelected;

        // Input ports
        if (_inputPorts is not null)
        {
            var views = new List<PortView>();
            foreach (var port in Node.Inputs)
                views.Add(new PortView { Port = port, Canvas = Canvas });
            _inputPorts.ItemsSource = views;
        }

        // Output port
        if (_outputPort is not null && Node.Output is not null)
            _outputPort.Content = new PortView { Port = Node.Output, Canvas = Canvas };

        // Custom content
        Content = Node.BuildContent();
    }

    // --- Node dragging ---

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (Canvas is null || Node is null) return;
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed) return;

        // Don't drag if clicking port connector
        if (IsPortArea(e.Source as Control)) return;

        Canvas.Graph?.ClearSelection();
        Node.IsSelected = true;
        IsSelected = true;

        _dragging = true;
        _dragStart = e.GetPosition(Canvas);
        _startX = Node.X;
        _startY = Node.Y;
        e.Pointer.Capture(this);
        e.Handled = true;
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (!_dragging || Node is null || Canvas is null) return;

        var pos = e.GetPosition(Canvas);
        var delta = (pos - _dragStart) / Canvas.Zoom;

        Node.X = _startX + delta.X;
        Node.Y = _startY + delta.Y;

        Avalonia.Controls.Canvas.SetLeft(this, Node.X);
        Avalonia.Controls.Canvas.SetTop(this, Node.Y);

        SyncAllPorts();
        Canvas.RefreshConnections();

        e.Handled = true;
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (_dragging)
        {
            _dragging = false;
            e.Pointer.Capture(null);
            e.Handled = true;
        }
    }

    internal void SyncAllPorts()
    {
        if (_inputPorts is not null)
            foreach (var pv in _inputPorts.GetVisualDescendants().OfType<PortView>())
                pv.SyncPosition();

        if (_outputPort?.Content is PortView opv)
            opv.SyncPosition();
    }

    static bool IsPortArea(Control? source)
    {
        var c = source;
        while (c is not null)
        {
            if (c is PortView) return true;
            c = c.Parent as Control;
        }

        return false;
    }
}
