using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using Manta.NodeEditor.Attributes;
using Manta.NodeEditor.Models;

namespace Manta.NodeEditor.Controls;

public class MNodeEditorNode : TemplatedControl
{
    public static readonly StyledProperty<NodeBase?> NodeDataProperty =
        AvaloniaProperty.Register<MNodeEditorNode, NodeBase?>(nameof(NodeData));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<MNodeEditorNode, bool>(nameof(IsSelected));

    public static readonly StyledProperty<Control?> ContentViewProperty =
        AvaloniaProperty.Register<MNodeEditorNode, Control?>(nameof(ContentView));

    ItemsControl? _inputPortsContainer;
    ContentPresenter? _outputPortPresenter;
    Button? _deleteButton;
    bool _isDragging;
    Point _dragStartPosition;
    double _nodeStartX;
    double _nodeStartY;

    public NodeBase? NodeData
    {
        get => GetValue(NodeDataProperty);
        set => SetValue(NodeDataProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    public Control? ContentView
    {
        get => GetValue(ContentViewProperty);
        set => SetValue(ContentViewProperty, value);
    }

    internal MNodeEditor? NodeEditor { get; set; }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _inputPortsContainer = e.NameScope.Find<ItemsControl>("PART_InputPorts");
        _outputPortPresenter = e.NameScope.Find<ContentPresenter>("PART_OutputPort");
        _deleteButton = e.NameScope.Find<Button>("PART_DeleteButton");

        if (_deleteButton is not null)
            _deleteButton.Click += OnDeleteClick;

        UpdateContent();
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == NodeDataProperty)
        {
            var oldNode = change.GetOldValue<NodeBase?>();
            var newNode = change.GetNewValue<NodeBase?>();

            if (oldNode is not null)
                oldNode.PropertyChanged -= OnNodePropertyChanged;
            if (newNode is not null)
                newNode.PropertyChanged += OnNodePropertyChanged;

            UpdateContent();
        }
    }

    void OnNodePropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NodeBase.IsSelected))
            IsSelected = NodeData?.IsSelected ?? false;
    }

    void UpdateContent()
    {
        if (NodeData is null) return;

        IsSelected = NodeData.IsSelected;

        // Setup input ports
        if (_inputPortsContainer is not null)
        {
            var portControls = new List<MNodeEditorPort>();
            foreach (var port in NodeData.Inputs)
            {
                portControls.Add(new MNodeEditorPort
                {
                    Port = port,
                    NodeEditor = NodeEditor
                });
            }

            _inputPortsContainer.ItemsSource = portControls;
        }

        // Setup output port
        if (_outputPortPresenter is not null && NodeData.Output is not null)
        {
            _outputPortPresenter.Content = new MNodeEditorPort
            {
                Port = NodeData.Output,
                NodeEditor = NodeEditor
            };
        }

        // Setup content view
        var nodeType = NodeData.GetType();
        var viewAttr = nodeType.GetCustomAttribute<NodeViewAttribute>();
        if (viewAttr is not null)
        {
            var view = viewAttr.CreateView();
            view.DataContext = NodeData;
            ContentView = view;
        }
        else
        {
            ContentView = NodeData.CreateContentView();
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (NodeEditor is null || NodeData is null) return;

        var props = e.GetCurrentPoint(this).Properties;
        if (!props.IsLeftButtonPressed) return;

        // Check if click is on a port connector - don't start drag
        if (e.Source is Control source && IsPortConnector(source))
            return;

        NodeEditor.SelectNode(NodeData);
        IsSelected = true;

        _isDragging = true;
        _dragStartPosition = e.GetPosition(NodeEditor);
        _nodeStartX = NodeData.X;
        _nodeStartY = NodeData.Y;
        e.Pointer.Capture(this);
        e.Handled = true;
    }

    static bool IsPortConnector(Control source)
    {
        var current = source;
        while (current is not null)
        {
            if (current is MNodeEditorPort)
                return true;
            current = current.Parent as Control;
        }

        return false;
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (!_isDragging || NodeData is null || NodeEditor is null) return;

        var pos = e.GetPosition(NodeEditor);
        var delta = pos - _dragStartPosition;
        var scaledDelta = new Point(delta.X / NodeEditor.Zoom, delta.Y / NodeEditor.Zoom);

        NodeData.X = _nodeStartX + scaledDelta.X;
        NodeData.Y = _nodeStartY + scaledDelta.Y;

        Canvas.SetLeft(this, NodeData.X);
        Canvas.SetTop(this, NodeData.Y);

        UpdateAllPortPositions();
        NodeEditor.InvalidateConnections();

        e.Handled = true;
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

    void OnDeleteClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (NodeEditor is not null && NodeData is not null)
            NodeEditor.Graph?.RemoveNode(NodeData);
        e.Handled = true;
    }

    internal void UpdateAllPortPositions()
    {
        if (_inputPortsContainer is not null)
        {
            foreach (var child in _inputPortsContainer.GetVisualDescendants())
            {
                if (child is MNodeEditorPort portControl)
                    portControl.UpdatePortPosition();
            }
        }

        if (_outputPortPresenter?.Content is MNodeEditorPort outputPort)
            outputPort.UpdatePortPosition();
    }
}
