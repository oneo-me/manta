using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using Manta.NodeEditor.Models;

namespace Manta.NodeEditor.Controls;

public class MNodeEditorPort : TemplatedControl
{
    public static readonly StyledProperty<NodePort?> PortProperty =
        AvaloniaProperty.Register<MNodeEditorPort, NodePort?>(nameof(Port));

    public static readonly StyledProperty<bool> IsHoveredProperty =
        AvaloniaProperty.Register<MNodeEditorPort, bool>(nameof(IsHovered));

    public static readonly StyledProperty<bool> ShowInlineEditorProperty =
        AvaloniaProperty.Register<MNodeEditorPort, bool>(nameof(ShowInlineEditor));

    public static readonly StyledProperty<Control?> InlineEditorProperty =
        AvaloniaProperty.Register<MNodeEditorPort, Control?>(nameof(InlineEditor));

    Control? _inputConnector;
    Control? _outputConnector;

    public NodePort? Port
    {
        get => GetValue(PortProperty);
        set => SetValue(PortProperty, value);
    }

    public bool IsHovered
    {
        get => GetValue(IsHoveredProperty);
        set => SetValue(IsHoveredProperty, value);
    }

    public bool ShowInlineEditor
    {
        get => GetValue(ShowInlineEditorProperty);
        set => SetValue(ShowInlineEditorProperty, value);
    }

    public Control? InlineEditor
    {
        get => GetValue(InlineEditorProperty);
        set => SetValue(InlineEditorProperty, value);
    }

    internal MNodeEditor? NodeEditor { get; set; }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PortProperty)
        {
            var oldPort = change.GetOldValue<NodePort?>();
            var newPort = change.GetNewValue<NodePort?>();

            if (oldPort is not null)
                oldPort.PropertyChanged -= OnPortPropertyChanged;

            if (newPort is not null)
                newPort.PropertyChanged += OnPortPropertyChanged;

            CreateInlineEditor();
            UpdateShowInlineEditor();
        }
    }

    void OnPortPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(NodePort.IsConnected))
            UpdateShowInlineEditor();
    }

    void UpdateShowInlineEditor()
    {
        ShowInlineEditor = Port is { IsInput: true, IsConnected: false } && InlineEditor is not null;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _inputConnector = e.NameScope.Find<Control>("PART_InputConnector");
        _outputConnector = e.NameScope.Find<Control>("PART_OutputConnector");
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        LayoutUpdated += OnLayoutUpdated;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        LayoutUpdated -= OnLayoutUpdated;
    }

    void OnLayoutUpdated(object? sender, EventArgs e)
    {
        UpdatePortPosition();
    }

    internal void UpdatePortPosition()
    {
        if (Port is null) return;

        var connector = Port.IsInput ? _inputConnector : _outputConnector;
        if (connector is null) return;

        var editor = this.FindAncestorOfType<MNodeEditor>();
        if (editor is null) return;

        var center = new Point(connector.Bounds.Width / 2, connector.Bounds.Height / 2);
        var translated = connector.TranslatePoint(center, editor);

        if (!translated.HasValue) return;

        // Convert from canvas coordinates to graph coordinates
        var graphPoint = editor.CanvasToGraph(translated.Value);
        Port.PortCenterX = graphPoint.X;
        Port.PortCenterY = graphPoint.Y;
    }

    void CreateInlineEditor()
    {
        if (Port is null || Port.IsOutput)
        {
            InlineEditor = null;
            UpdateShowInlineEditor();
            return;
        }

        InlineEditor = Port.DataType switch
        {
            var t when t == typeof(double) => CreateDoubleEditor(),
            var t when t == typeof(float) => CreateFloatEditor(),
            var t when t == typeof(int) => CreateIntEditor(),
            var t when t == typeof(string) => CreateTextEditor(),
            var t when t == typeof(bool) => CreateBoolEditor(),
            _ => null
        };

        UpdateShowInlineEditor();
    }

    Control CreateDoubleEditor()
    {
        var editor = new NumericUpDown
        {
            Minimum = decimal.MinValue,
            Maximum = decimal.MaxValue,
            Value = Port?.Value is double d ? (decimal)d : 0m,
            Width = 80,
            FontSize = 11,
            FormatString = "F2"
        };
        editor.ValueChanged += (_, e) =>
        {
            if (Port is not null)
                Port.Value = (double)(e.NewValue ?? 0m);
        };
        return editor;
    }

    Control CreateFloatEditor()
    {
        var editor = new NumericUpDown
        {
            Minimum = decimal.MinValue,
            Maximum = decimal.MaxValue,
            Value = Port?.Value is float f ? (decimal)f : 0m,
            Width = 80,
            FontSize = 11,
            FormatString = "F2"
        };
        editor.ValueChanged += (_, e) =>
        {
            if (Port is not null)
                Port.Value = (float)(e.NewValue ?? 0m);
        };
        return editor;
    }

    Control CreateIntEditor()
    {
        var editor = new NumericUpDown
        {
            Minimum = int.MinValue,
            Maximum = int.MaxValue,
            Value = Port?.Value is int i ? i : 0,
            Increment = 1,
            Width = 80,
            FontSize = 11,
            FormatString = "F0"
        };
        editor.ValueChanged += (_, e) =>
        {
            if (Port is not null)
                Port.Value = (int)(e.NewValue ?? 0m);
        };
        return editor;
    }

    Control CreateTextEditor()
    {
        var editor = new TextBox
        {
            Text = Port?.Value as string ?? "",
            Width = 100,
            FontSize = 11
        };
        editor.TextChanged += (_, _) =>
        {
            if (Port is not null)
                Port.Value = editor.Text ?? "";
        };
        return editor;
    }

    Control CreateBoolEditor()
    {
        var editor = new CheckBox
        {
            IsChecked = Port?.Value is true,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };
        editor.IsCheckedChanged += (_, _) =>
        {
            if (Port is not null)
                Port.Value = editor.IsChecked ?? false;
        };
        return editor;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (Port is null || NodeEditor is null) return;

        var props = e.GetCurrentPoint(this).Properties;

        if (props.IsLeftButtonPressed && Port.IsOutput)
        {
            NodeEditor.StartConnectionDrag(Port);
            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (Port is { IsInput: true } && NodeEditor is { IsDraggingConnection: true })
        {
            NodeEditor.CompleteConnectionDrag(Port);
            e.Handled = true;
        }
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        IsHovered = true;

        if (NodeEditor is { IsDraggingConnection: true, DragSourcePort: not null } && Port is not null)
        {
            if (Port.IsInput && NodeEditor.IsConnectionCompatible(NodeEditor.DragSourcePort, Port))
                PseudoClasses.Set(":compatible", true);
            else if (Port.IsInput)
                PseudoClasses.Set(":incompatible", true);
        }
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        IsHovered = false;
        PseudoClasses.Set(":compatible", false);
        PseudoClasses.Set(":incompatible", false);
    }
}
