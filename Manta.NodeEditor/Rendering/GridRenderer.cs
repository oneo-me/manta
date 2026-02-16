using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manta.NodeEditor.Rendering;

/// <summary>
/// Renders a zoomable/pannable grid background.
/// </summary>
public class GridRenderer : Control
{
    public static readonly StyledProperty<double> GridSizeProperty =
        AvaloniaProperty.Register<GridRenderer, double>(nameof(GridSize), 20.0);

    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<GridRenderer, double>(nameof(Zoom), 1.0);

    public static readonly StyledProperty<Point> OffsetProperty =
        AvaloniaProperty.Register<GridRenderer, Point>(nameof(Offset));

    public static readonly StyledProperty<IBrush?> BackgroundBrushProperty =
        AvaloniaProperty.Register<GridRenderer, IBrush?>(nameof(BackgroundBrush));

    public static readonly StyledProperty<IBrush?> MinorGridBrushProperty =
        AvaloniaProperty.Register<GridRenderer, IBrush?>(nameof(MinorGridBrush));

    public static readonly StyledProperty<IBrush?> MajorGridBrushProperty =
        AvaloniaProperty.Register<GridRenderer, IBrush?>(nameof(MajorGridBrush));

    public double GridSize
    {
        get => GetValue(GridSizeProperty);
        set => SetValue(GridSizeProperty, value);
    }

    public double Zoom
    {
        get => GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    public Point Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public IBrush? BackgroundBrush
    {
        get => GetValue(BackgroundBrushProperty);
        set => SetValue(BackgroundBrushProperty, value);
    }

    public IBrush? MinorGridBrush
    {
        get => GetValue(MinorGridBrushProperty);
        set => SetValue(MinorGridBrushProperty, value);
    }

    public IBrush? MajorGridBrush
    {
        get => GetValue(MajorGridBrushProperty);
        set => SetValue(MajorGridBrushProperty, value);
    }

    static GridRenderer()
    {
        AffectsRender<GridRenderer>(GridSizeProperty, ZoomProperty, OffsetProperty,
            BackgroundBrushProperty, MinorGridBrushProperty, MajorGridBrushProperty);
    }

    public override void Render(DrawingContext context)
    {
        var bounds = Bounds;
        var bg = BackgroundBrush ?? Brushes.Transparent;
        context.FillRectangle(bg, bounds);

        var scaled = GridSize * Zoom;
        if (scaled < 4) return;

        var minorBrush = MinorGridBrush ?? Brushes.Transparent;
        var pen = new Pen(minorBrush, 0.5);
        var ox = Offset.X % scaled;
        var oy = Offset.Y % scaled;

        for (var x = ox; x < bounds.Width; x += scaled)
            context.DrawLine(pen, new Point(x, 0), new Point(x, bounds.Height));
        for (var y = oy; y < bounds.Height; y += scaled)
            context.DrawLine(pen, new Point(0, y), new Point(bounds.Width, y));

        // Major grid (every 5 lines)
        var major = scaled * 5;
        if (major < 20) return;

        var majorBrush = MajorGridBrush ?? Brushes.Transparent;
        var majorPen = new Pen(majorBrush);
        var mox = Offset.X % major;
        var moy = Offset.Y % major;

        for (var x = mox; x < bounds.Width; x += major)
            context.DrawLine(majorPen, new Point(x, 0), new Point(x, bounds.Height));
        for (var y = moy; y < bounds.Height; y += major)
            context.DrawLine(majorPen, new Point(0, y), new Point(bounds.Width, y));
    }
}
