using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Manta.NodeEditor.Core;

namespace Manta.NodeEditor.Rendering;

/// <summary>
/// Renders Bezier connection lines between ports and the active drag preview.
/// Draws in screen (canvas-local) coordinates — lives outside the zoom/pan transform.
/// </summary>
public class ConnectionRenderer : Control
{
    public static readonly StyledProperty<IBrush?> ConnectionBrushProperty =
        AvaloniaProperty.Register<ConnectionRenderer, IBrush?>(nameof(ConnectionBrush));

    public static readonly StyledProperty<IBrush?> SelectedBrushProperty =
        AvaloniaProperty.Register<ConnectionRenderer, IBrush?>(nameof(SelectedBrush));

    public static readonly StyledProperty<IBrush?> DragBrushProperty =
        AvaloniaProperty.Register<ConnectionRenderer, IBrush?>(nameof(DragBrush));

    internal NodeCanvas? Canvas { get; set; }

    public IBrush? ConnectionBrush
    {
        get => GetValue(ConnectionBrushProperty);
        set => SetValue(ConnectionBrushProperty, value);
    }

    public IBrush? SelectedBrush
    {
        get => GetValue(SelectedBrushProperty);
        set => SetValue(SelectedBrushProperty, value);
    }

    public IBrush? DragBrush
    {
        get => GetValue(DragBrushProperty);
        set => SetValue(DragBrushProperty, value);
    }

    static ConnectionRenderer()
    {
        AffectsRender<ConnectionRenderer>(ConnectionBrushProperty, SelectedBrushProperty, DragBrushProperty);
    }

    public override void Render(DrawingContext context)
    {
        if (Canvas?.Graph is not { } graph) return;

        var zoom = Canvas.Zoom;
        var offset = Canvas.Offset;
        var connBrush = ConnectionBrush ?? Brushes.Gray;
        var selBrush = SelectedBrush ?? Brushes.DodgerBlue;

        var pen = new Pen(connBrush, 2);
        var selectedPen = new Pen(selBrush, 3);

        foreach (var conn in graph.Connections)
        {
            var start = PortToScreen(conn.Source, zoom, offset);
            var end = PortToScreen(conn.Target, zoom, offset);
            DrawBezier(context, start, end, conn.IsSelected ? selectedPen : pen);
        }

        // Drag preview — DragEnd is already in screen coordinates
        if (Canvas is { IsDragging: true, DragSource: not null })
        {
            var dBrush = DragBrush ?? Brushes.Yellow;
            var dragPen = new Pen(dBrush, 2);
            var start = PortToScreen(Canvas.DragSource, zoom, offset);
            DrawBezier(context, start, Canvas.DragEnd, dragPen);
        }
    }

    /// <summary>
    /// Hit-test a screen-space point against all connections.
    /// </summary>
    internal Connection? HitTest(Point screenPoint, double tolerance = 8)
    {
        if (Canvas?.Graph is not { } graph) return null;

        var zoom = Canvas.Zoom;
        var offset = Canvas.Offset;

        foreach (var conn in graph.Connections)
        {
            var s = PortToScreen(conn.Source, zoom, offset);
            var e = PortToScreen(conn.Target, zoom, offset);

            if (IsNearBezier(screenPoint, s, e, tolerance))
                return conn;
        }

        return null;
    }

    static Point PortToScreen(Port p, double zoom, Point offset)
        => new(p.CenterX * zoom + offset.X, p.CenterY * zoom + offset.Y);

    static void DrawBezier(DrawingContext ctx, Point s, Point e, Pen pen)
    {
        var dx = Math.Max(Math.Abs(e.X - s.X) * 0.5, 50);
        var c1 = new Point(s.X + dx, s.Y);
        var c2 = new Point(e.X - dx, e.Y);

        var geo = new StreamGeometry();
        using (var g = geo.Open())
        {
            g.BeginFigure(s, false);
            g.CubicBezierTo(c1, c2, e);
        }

        ctx.DrawGeometry(null, pen, geo);
    }

    static bool IsNearBezier(Point p, Point s, Point e, double tol)
    {
        var dx = Math.Max(Math.Abs(e.X - s.X) * 0.5, 50);
        var c1 = new Point(s.X + dx, s.Y);
        var c2 = new Point(e.X - dx, e.Y);

        var prev = s;
        for (var i = 1; i <= 20; i++)
        {
            var t = i / 20.0;
            var mt = 1 - t;
            var cur = new Point(
                mt * mt * mt * s.X + 3 * mt * mt * t * c1.X + 3 * mt * t * t * c2.X + t * t * t * e.X,
                mt * mt * mt * s.Y + 3 * mt * mt * t * c1.Y + 3 * mt * t * t * c2.Y + t * t * t * e.Y);

            if (SegmentDist(p, prev, cur) < tol)
                return true;
            prev = cur;
        }

        return false;
    }

    static double SegmentDist(Point p, Point a, Point b)
    {
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;
        var len2 = dx * dx + dy * dy;
        if (len2 < 0.001)
            return Math.Sqrt((p.X - a.X) * (p.X - a.X) + (p.Y - a.Y) * (p.Y - a.Y));

        var t = Math.Clamp(((p.X - a.X) * dx + (p.Y - a.Y) * dy) / len2, 0, 1);
        var px = a.X + t * dx;
        var py = a.Y + t * dy;
        return Math.Sqrt((p.X - px) * (p.X - px) + (p.Y - py) * (p.Y - py));
    }
}
