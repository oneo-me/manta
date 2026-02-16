using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manta.NodeEditor.Controls;

public class MNodeEditorGrid : Control
{
    public static readonly StyledProperty<double> GridSizeProperty =
        AvaloniaProperty.Register<MNodeEditorGrid, double>(nameof(GridSize), 20.0);

    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<MNodeEditorGrid, double>(nameof(Zoom), 1.0);

    public static readonly StyledProperty<Point> OffsetProperty =
        AvaloniaProperty.Register<MNodeEditorGrid, Point>(nameof(Offset));

    public static readonly StyledProperty<IBrush?> GridBrushProperty =
        AvaloniaProperty.Register<MNodeEditorGrid, IBrush?>(nameof(GridBrush),
            new SolidColorBrush(Color.FromRgb(50, 50, 50)));

    public static readonly StyledProperty<IBrush?> BackgroundBrushProperty =
        AvaloniaProperty.Register<MNodeEditorGrid, IBrush?>(nameof(BackgroundBrush),
            new SolidColorBrush(Color.FromRgb(30, 30, 30)));

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

    public IBrush? GridBrush
    {
        get => GetValue(GridBrushProperty);
        set => SetValue(GridBrushProperty, value);
    }

    public IBrush? BackgroundBrush
    {
        get => GetValue(BackgroundBrushProperty);
        set => SetValue(BackgroundBrushProperty, value);
    }

    static MNodeEditorGrid()
    {
        AffectsRender<MNodeEditorGrid>(
            GridSizeProperty, ZoomProperty, OffsetProperty,
            GridBrushProperty, BackgroundBrushProperty);
    }

    public override void Render(DrawingContext context)
    {
        var bounds = Bounds;

        if (BackgroundBrush is not null)
            context.FillRectangle(BackgroundBrush, bounds);

        if (GridBrush is null) return;

        var scaledGridSize = GridSize * Zoom;
        if (scaledGridSize is < 4 or > 500) return;

        var pen = new Pen(GridBrush, 0.5);
        var offsetX = Offset.X % scaledGridSize;
        var offsetY = Offset.Y % scaledGridSize;

        for (var x = offsetX; x < bounds.Width; x += scaledGridSize)
            context.DrawLine(pen, new Point(x, 0), new Point(x, bounds.Height));

        for (var y = offsetY; y < bounds.Height; y += scaledGridSize)
            context.DrawLine(pen, new Point(0, y), new Point(bounds.Width, y));

        // Major grid lines
        var majorGridSize = scaledGridSize * 5;
        if (majorGridSize >= 20)
        {
            var majorPen = new Pen(GridBrush);
            var majorOffsetX = Offset.X % majorGridSize;
            var majorOffsetY = Offset.Y % majorGridSize;

            for (var x = majorOffsetX; x < bounds.Width; x += majorGridSize)
                context.DrawLine(majorPen, new Point(x, 0), new Point(x, bounds.Height));

            for (var y = majorOffsetY; y < bounds.Height; y += majorGridSize)
                context.DrawLine(majorPen, new Point(0, y), new Point(bounds.Width, y));
        }
    }
}
