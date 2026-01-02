using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manta.Blueprint.Controls;

public class MBlueprintGridBackground : Control
{
    public static readonly StyledProperty<double> GridSizeProperty =
        AvaloniaProperty.Register<MBlueprintGridBackground, double>(nameof(GridSize), 20.0);

    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<MBlueprintGridBackground, double>(nameof(Zoom), 1.0);

    public static readonly StyledProperty<Point> OffsetProperty =
        AvaloniaProperty.Register<MBlueprintGridBackground, Point>(nameof(Offset));

    public static readonly StyledProperty<IBrush?> GridBrushProperty =
        AvaloniaProperty.Register<MBlueprintGridBackground, IBrush?>(nameof(GridBrush),
            new SolidColorBrush(Color.FromRgb(42, 42, 42)));

    public static readonly StyledProperty<IBrush?> BackgroundBrushProperty =
        AvaloniaProperty.Register<MBlueprintGridBackground, IBrush?>(nameof(BackgroundBrush),
            new SolidColorBrush(Color.FromRgb(26, 26, 26)));

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

    static MBlueprintGridBackground()
    {
        AffectsRender<MBlueprintGridBackground>(
            GridSizeProperty,
            ZoomProperty,
            OffsetProperty,
            GridBrushProperty,
            BackgroundBrushProperty);
    }

    public override void Render(DrawingContext context)
    {
        var bounds = Bounds;

        // Draw background
        if (BackgroundBrush != null)
            context.FillRectangle(BackgroundBrush, new(0, 0, bounds.Width, bounds.Height));

        if (GridBrush == null) return;

        var scaledGridSize = GridSize * Zoom;

        // Skip drawing if grid is too small or too large
        if (scaledGridSize is < 5 or > 500) return;

        var pen = new Pen(GridBrush);

        // Calculate grid offset based on pan offset (ensure positive values)
        var offsetX = (Offset.X % scaledGridSize + scaledGridSize) % scaledGridSize;
        var offsetY = (Offset.Y % scaledGridSize + scaledGridSize) % scaledGridSize;

        // Draw vertical lines
        for (var x = offsetX; x < bounds.Width; x += scaledGridSize)
            context.DrawLine(pen, new(x, 0), new(x, bounds.Height));

        // Draw horizontal lines
        for (var y = offsetY; y < bounds.Height; y += scaledGridSize)
            context.DrawLine(pen, new(0, y), new(bounds.Width, y));
    }
}
