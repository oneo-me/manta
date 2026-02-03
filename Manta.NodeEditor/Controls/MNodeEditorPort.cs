using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using Manta.NodeEditor.Models;

namespace Manta.NodeEditor.Controls;

public class MNodeEditorPort : TemplatedControl
{
    public static readonly StyledProperty<Port?> PortProperty =
        AvaloniaProperty.Register<MNodeEditorPort, Port?>(nameof(Port));

    public static readonly StyledProperty<bool> IsHoveredProperty =
        AvaloniaProperty.Register<MNodeEditorPort, bool>(nameof(IsHovered));

    public static readonly StyledProperty<bool> IsCompatibleProperty =
        AvaloniaProperty.Register<MNodeEditorPort, bool>(nameof(IsCompatible));

    public static readonly StyledProperty<bool> IsIncompatibleProperty =
        AvaloniaProperty.Register<MNodeEditorPort, bool>(nameof(IsIncompatible));

    public static readonly StyledProperty<bool> IsInputProperty =
        AvaloniaProperty.Register<MNodeEditorPort, bool>(nameof(IsInput));

    public static readonly StyledProperty<bool> IsOutputProperty =
        AvaloniaProperty.Register<MNodeEditorPort, bool>(nameof(IsOutput));

    Control? _inputConnector;
    Control? _outputConnector;

    public Port? Port
    {
        get => GetValue(PortProperty);
        set => SetValue(PortProperty, value);
    }

    public bool IsHovered
    {
        get => GetValue(IsHoveredProperty);
        set => SetValue(IsHoveredProperty, value);
    }

    public bool IsCompatible
    {
        get => GetValue(IsCompatibleProperty);
        set => SetValue(IsCompatibleProperty, value);
    }

    public bool IsIncompatible
    {
        get => GetValue(IsIncompatibleProperty);
        set => SetValue(IsIncompatibleProperty, value);
    }

    public MNodeEditor? NodeEditor { get; set; }

    public string DisplayName => Port?.DisplayName ?? "";

    public bool IsInput
    {
        get => GetValue(IsInputProperty);
        private set => SetValue(IsInputProperty, value);
    }

    public bool IsOutput
    {
        get => GetValue(IsOutputProperty);
        private set => SetValue(IsOutputProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PortProperty)
        {
            IsInput = Port?.IsInput ?? false;
            IsOutput = Port?.IsOutput ?? false;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _inputConnector = e.NameScope.Find<Control>("PART_Connector");
        _outputConnector = e.NameScope.Find<Control>("PART_OutputConnector");

        // Update position after layout is complete
        LayoutUpdated += OnLayoutUpdated;
    }

    void OnLayoutUpdated(object? sender, EventArgs e)
    {
        UpdatePosition();
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        IsHovered = true;

        // Update compatibility state during drag
        if (NodeEditor?.IsDraggingConnection == true && Port != null)
        {
            if (NodeEditor.IsPortCompatibleWithDrag(Port))
                IsCompatible = true;
            else if (Port.IsInput)
                IsIncompatible = true;
        }
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        IsHovered = false;
        IsCompatible = false;
        IsIncompatible = false;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (Port != null && NodeEditor != null)
        {
            var props = e.GetCurrentPoint(this).Properties;
            if (props.IsLeftButtonPressed && Port.IsOutput)
            {
                // Start connection drag from output port
                // Capture pointer on the NodeEditor so it receives move/release events
                NodeEditor.StartConnectionDrag(Port);
                e.Pointer.Capture(NodeEditor);
                e.Handled = true;
            }
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (Port != null && NodeEditor != null)
            if (Port.IsInput && NodeEditor.IsDraggingConnection)
            {
                // Complete connection to input port
                NodeEditor.CompleteConnectionDrag(Port);
                e.Handled = true;
            }
    }

    Control? ActiveConnector => IsInput ? _inputConnector : _outputConnector;

    internal void UpdatePosition()
    {
        if (Port == null) return;

        var connector = ActiveConnector;
        if (connector == null) return;

        // Find the parent MNodeEditorNode
        var nodeControl = this.FindAncestorOfType<MNodeEditorNode>();
        if (nodeControl?.Node == null) return;

        // Get connector center relative to the node control
        var center = new Point(connector.Bounds.Width / 2, connector.Bounds.Height / 2);
        var relativeToNode = connector.TranslatePoint(center, nodeControl);
        if (!relativeToNode.HasValue) return;

        // Port position in graph space = node position + offset within node
        Port.Position = new(
            nodeControl.Node.Position.X + relativeToNode.Value.X,
            nodeControl.Node.Position.Y + relativeToNode.Value.Y
        );
    }
}
