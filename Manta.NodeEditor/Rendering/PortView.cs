using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using Manta.NodeEditor.Core;
using Manta.NodeEditor.TypeSystem;

namespace Manta.NodeEditor.Rendering;

/// <summary>
/// Renders a single port: connector dot + label + optional inline value editor.
/// </summary>
public class PortView : TemplatedControl
{
    public static readonly StyledProperty<Port?> PortProperty =
        AvaloniaProperty.Register<PortView, Port?>(nameof(Port));

    public static readonly StyledProperty<bool> ShowEditorProperty =
        AvaloniaProperty.Register<PortView, bool>(nameof(ShowEditor));

    public static readonly StyledProperty<Control?> EditorProperty =
        AvaloniaProperty.Register<PortView, Control?>(nameof(Editor));

    Control? _connector;
    Control? _outputConnector;

    public Port? Port
    {
        get => GetValue(PortProperty);
        set => SetValue(PortProperty, value);
    }

    public bool ShowEditor
    {
        get => GetValue(ShowEditorProperty);
        set => SetValue(ShowEditorProperty, value);
    }

    public Control? Editor
    {
        get => GetValue(EditorProperty);
        set => SetValue(EditorProperty, value);
    }

    internal NodeCanvas? Canvas { get; set; }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == PortProperty)
        {
            if (change.GetOldValue<Port?>() is { } old)
                old.PropertyChanged -= OnPortChanged;
            if (change.GetNewValue<Port?>() is { } p)
                p.PropertyChanged += OnPortChanged;

            RebuildEditor();
            ApplyTypeColor();
        }
    }

    void ApplyTypeColor()
    {
        if (Port is null) return;
        var brush = PortColors.GetBrush(Port.ValueType);
        var active = Port.IsOutput ? _outputConnector : _connector;
        if (active is Border border)
            border.Background = brush;
    }

    void OnPortChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Core.Port.IsConnected))
            ShowEditor = Port is { IsInput: true, IsConnected: false } && Editor is not null;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _connector = e.NameScope.Find<Control>("PART_Connector");
        _outputConnector = e.NameScope.Find<Control>("PART_OutputConnector");
        ApplyTypeColor();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        LayoutUpdated += OnLayout;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        LayoutUpdated -= OnLayout;
        base.OnDetachedFromVisualTree(e);
    }

    void OnLayout(object? sender, EventArgs e) => SyncPosition();

    internal void SyncPosition()
    {
        if (Port is null) return;

        var active = Port.IsOutput ? _outputConnector : _connector;
        if (active is null) return;

        var canvas = this.FindAncestorOfType<NodeCanvas>();
        if (canvas is null) return;

        var center = new Point(active.Bounds.Width / 2, active.Bounds.Height / 2);
        var mapped = active.TranslatePoint(center, canvas);
        if (!mapped.HasValue) return;

        var gp = canvas.ScreenToGraph(mapped.Value);
        Port.CenterX = gp.X;
        Port.CenterY = gp.Y;
    }

    void RebuildEditor()
    {
        if (Port is null or { IsOutput: true })
        {
            Editor = null;
            ShowEditor = false;
            return;
        }

        Editor = Port.ValueType switch
        {
            var t when t == typeof(double) => MakeNumeric("F2", v => Port.BoxedValue = (double)v, () => Port.BoxedValue is double d ? (decimal)d : 0m),
            var t when t == typeof(float)  => MakeNumeric("F2", v => Port.BoxedValue = (float)v, () => Port.BoxedValue is float f ? (decimal)f : 0m),
            var t when t == typeof(int)    => MakeNumeric("F0", v => Port.BoxedValue = (int)v, () => Port.BoxedValue is int i ? i : 0m),
            var t when t == typeof(string) => MakeText(),
            var t when t == typeof(bool)   => MakeBool(),
            _                              => null
        };

        ShowEditor = Port is { IsConnected: false } && Editor is not null;
    }

    Control MakeNumeric(string fmt, Action<decimal> onChanged, Func<decimal> getCurrent)
    {
        var n = new NumericUpDown
        {
            Value = getCurrent(),
            Width = 80,
            FontSize = 11,
            FormatString = fmt,
            Minimum = decimal.MinValue,
            Maximum = decimal.MaxValue
        };
        n.ValueChanged += (_, e) => onChanged(e.NewValue ?? 0m);
        return n;
    }

    Control MakeText()
    {
        var tb = new TextBox
        {
            Text = Port?.BoxedValue as string ?? "",
            Width = 100,
            FontSize = 11
        };
        tb.TextChanged += (_, _) =>
        {
            if (Port is not null)
                Port.BoxedValue = tb.Text ?? "";
        };
        return tb;
    }

    Control MakeBool()
    {
        var cb = new CheckBox
        {
            IsChecked = Port?.BoxedValue is true,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };
        cb.IsCheckedChanged += (_, _) =>
        {
            if (Port is not null)
                Port.BoxedValue = cb.IsChecked ?? false;
        };
        return cb;
    }

    // --- Interaction: connection dragging ---

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (Port is null or { IsInput: true } || Canvas is null) return;

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            Canvas.BeginConnectionDrag(Port, e.Pointer);
            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (Port is { IsInput: true } && Canvas is { IsDragging: true })
        {
            Canvas.EndConnectionDrag(Port);
            e.Handled = true;
        }
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        PseudoClasses.Set(":hovered", true);

        // Show compatibility feedback during connection drag
        if (Port is { IsInput: true } && Canvas is { IsDragging: true, DragSource: not null })
        {
            var compatible = Canvas.IsConnectionCompatible(Canvas.DragSource, Port);
            PseudoClasses.Set(":compatible", compatible);
            PseudoClasses.Set(":incompatible", !compatible);
            Canvas.SetDragHoverPort(Port);
        }
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        PseudoClasses.Set(":hovered", false);
        PseudoClasses.Set(":compatible", false);
        PseudoClasses.Set(":incompatible", false);

        Canvas?.SetDragHoverPort(null);
    }
}
