using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manta.NodeEditor.Controls;

public class MNodeEditorConnectionLayer : Control
{
    public MNodeEditor? NodeEditor { get; set; }

    public static readonly StyledProperty<IBrush?> ConnectionBrushProperty =
        AvaloniaProperty.Register<MNodeEditorConnectionLayer, IBrush?>(nameof(ConnectionBrush),
            new SolidColorBrush(Color.FromRgb(128, 128, 128)));

    public static readonly StyledProperty<IBrush?> SelectedConnectionBrushProperty =
        AvaloniaProperty.Register<MNodeEditorConnectionLayer, IBrush?>(nameof(SelectedConnectionBrush),
            new SolidColorBrush(Color.FromRgb(59, 130, 246)));

    public static readonly StyledProperty<IBrush?> DraggingConnectionBrushProperty =
        AvaloniaProperty.Register<MNodeEditorConnectionLayer, IBrush?>(nameof(DraggingConnectionBrush),
            new SolidColorBrush(Color.FromArgb(180, 59, 130, 246)));

    public static readonly StyledProperty<double> ConnectionThicknessProperty =
        AvaloniaProperty.Register<MNodeEditorConnectionLayer, double>(nameof(ConnectionThickness), 2.0);

    public IBrush? ConnectionBrush
    {
        get => GetValue(ConnectionBrushProperty);
        set => SetValue(ConnectionBrushProperty, value);
    }

    public IBrush? SelectedConnectionBrush
    {
        get => GetValue(SelectedConnectionBrushProperty);
        set => SetValue(SelectedConnectionBrushProperty, value);
    }

    public IBrush? DraggingConnectionBrush
    {
        get => GetValue(DraggingConnectionBrushProperty);
        set => SetValue(DraggingConnectionBrushProperty, value);
    }

    public double ConnectionThickness
    {
        get => GetValue(ConnectionThicknessProperty);
        set => SetValue(ConnectionThicknessProperty, value);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (NodeEditor?.Graph == null) return;

        var pen = new Pen(ConnectionBrush, ConnectionThickness);
        var selectedPen = new Pen(SelectedConnectionBrush, ConnectionThickness);
        var draggingPen = new Pen(DraggingConnectionBrush, ConnectionThickness);

        // Draw existing connections in graph coordinates
        // (the layer is now inside the transformed container)
        foreach (var connection in NodeEditor.Graph.Connections)
        {
            var startPoint = connection.SourcePort.Position;
            var endPoint = connection.TargetPort.Position;

            var currentPen = connection.IsSelected ? selectedPen : pen;
            DrawBezierConnection(context, startPoint, endPoint, currentPen);
        }

        // Draw dragging connection if any
        if (NodeEditor.IsDraggingConnection && NodeEditor.DragStartPort != null)
        {
            var startPoint = NodeEditor.DragStartPort.Position;
            // DragEndPoint is in canvas coordinates, convert to graph coordinates
            var endPoint = NodeEditor.CanvasToGraph(NodeEditor.DragEndPoint);

            DrawBezierConnection(context, startPoint, endPoint, draggingPen);
        }
    }

    static void DrawBezierConnection(DrawingContext context, Point start, Point end, Pen pen)
    {
        var dx = Math.Abs(end.X - start.X) * 0.5;
        dx = Math.Max(dx, 50);

        var control1 = new Point(start.X + dx, start.Y);
        var control2 = new Point(end.X - dx, end.Y);

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(start, false);
            ctx.CubicBezierTo(control1, control2, end);
        }

        context.DrawGeometry(null, pen, geometry);
    }
}
