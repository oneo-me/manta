using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Manta.NodeEditor.Models;

namespace Manta.NodeEditor.Controls;

public class MNodeEditorConnectionLayer : Control
{
    public static readonly StyledProperty<IBrush?> ConnectionBrushProperty =
        AvaloniaProperty.Register<MNodeEditorConnectionLayer, IBrush?>(nameof(ConnectionBrush),
            new SolidColorBrush(Color.FromRgb(160, 160, 160)));

    public static readonly StyledProperty<IBrush?> SelectedConnectionBrushProperty =
        AvaloniaProperty.Register<MNodeEditorConnectionLayer, IBrush?>(nameof(SelectedConnectionBrush),
            new SolidColorBrush(Color.FromRgb(59, 130, 246)));

    public static readonly StyledProperty<IBrush?> DraggingConnectionBrushProperty =
        AvaloniaProperty.Register<MNodeEditorConnectionLayer, IBrush?>(nameof(DraggingConnectionBrush),
            new SolidColorBrush(Color.FromRgb(250, 204, 21)));

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

    internal MNodeEditor? NodeEditor { get; set; }

    public override void Render(DrawingContext context)
    {
        if (NodeEditor?.Graph is not { } graph) return;

        var pen = new Pen(ConnectionBrush, ConnectionThickness);
        var selectedPen = new Pen(SelectedConnectionBrush, ConnectionThickness + 1);
        var draggingPen = new Pen(DraggingConnectionBrush, ConnectionThickness);

        foreach (var connection in graph.Connections)
        {
            var start = new Point(connection.Source.PortCenterX, connection.Source.PortCenterY);
            var end = new Point(connection.Target.PortCenterX, connection.Target.PortCenterY);
            var currentPen = connection.IsSelected ? selectedPen : pen;
            DrawBezier(context, start, end, currentPen);
        }

        if (NodeEditor.IsDraggingConnection && NodeEditor.DragSourcePort is not null)
        {
            var start = new Point(
                NodeEditor.DragSourcePort.PortCenterX,
                NodeEditor.DragSourcePort.PortCenterY);
            var end = NodeEditor.DragEndPoint;
            DrawBezier(context, start, end, draggingPen);
        }
    }

    internal NodeConnection? HitTestConnection(Point graphPoint)
    {
        if (NodeEditor?.Graph is not { } graph) return null;

        foreach (var connection in graph.Connections)
        {
            var start = new Point(connection.Source.PortCenterX, connection.Source.PortCenterY);
            var end = new Point(connection.Target.PortCenterX, connection.Target.PortCenterY);

            if (IsPointNearBezier(graphPoint, start, end, 8))
                return connection;
        }

        return null;
    }

    static void DrawBezier(DrawingContext context, Point start, Point end, Pen pen)
    {
        var dx = Math.Max(Math.Abs(end.X - start.X) * 0.5, 50);
        var c1 = new Point(start.X + dx, start.Y);
        var c2 = new Point(end.X - dx, end.Y);

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(start, false);
            ctx.CubicBezierTo(c1, c2, end);
        }

        context.DrawGeometry(null, pen, geometry);
    }

    static bool IsPointNearBezier(Point point, Point start, Point end, double tolerance)
    {
        const int segments = 20;
        var dx = Math.Max(Math.Abs(end.X - start.X) * 0.5, 50);
        var c1 = new Point(start.X + dx, start.Y);
        var c2 = new Point(end.X - dx, end.Y);

        var prevPoint = start;

        for (var i = 1; i <= segments; i++)
        {
            var t = i / (double)segments;
            var mt = 1 - t;

            var x = mt * mt * mt * start.X + 3 * mt * mt * t * c1.X +
                    3 * mt * t * t * c2.X + t * t * t * end.X;
            var y = mt * mt * mt * start.Y + 3 * mt * mt * t * c1.Y +
                    3 * mt * t * t * c2.Y + t * t * t * end.Y;

            var curPoint = new Point(x, y);

            if (DistanceToSegment(point, prevPoint, curPoint) < tolerance)
                return true;

            prevPoint = curPoint;
        }

        return false;
    }

    static double DistanceToSegment(Point p, Point a, Point b)
    {
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;
        var lengthSquared = dx * dx + dy * dy;

        if (lengthSquared < 0.001)
            return Math.Sqrt((p.X - a.X) * (p.X - a.X) + (p.Y - a.Y) * (p.Y - a.Y));

        var t = Math.Clamp(((p.X - a.X) * dx + (p.Y - a.Y) * dy) / lengthSquared, 0, 1);
        var projX = a.X + t * dx;
        var projY = a.Y + t * dy;

        return Math.Sqrt((p.X - projX) * (p.X - projX) + (p.Y - projY) * (p.Y - projY));
    }
}
