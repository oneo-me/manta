using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace Manta.Controls;

/// <summary>
/// Renders an infinite dot-grid background in screen coordinates.
/// Sits outside the transform container as a background layer.
/// Call <see cref="UpdateViewport"/> when zoom/offset changes.
/// </summary>
public class MCanvasGrid : Control
{
    public static readonly StyledProperty<double> GridSizeProperty =
        AvaloniaProperty.Register<MCanvasGrid, double>(nameof(GridSize), 20.0);

    public static readonly StyledProperty<IBrush?> GridBrushProperty =
        AvaloniaProperty.Register<MCanvasGrid, IBrush?>(nameof(GridBrush));

    public static readonly StyledProperty<IBrush?> BackgroundBrushProperty =
        AvaloniaProperty.Register<MCanvasGrid, IBrush?>(nameof(BackgroundBrush));

    public static readonly StyledProperty<bool> ShowGridProperty =
        AvaloniaProperty.Register<MCanvasGrid, bool>(nameof(ShowGrid), true);

    Point _offset;
    double _zoom = 1.0;

    public double GridSize
    {
        get => GetValue(GridSizeProperty);
        set => SetValue(GridSizeProperty, value);
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

    public bool ShowGrid
    {
        get => GetValue(ShowGridProperty);
        set => SetValue(ShowGridProperty, value);
    }

    static MCanvasGrid()
    {
        AffectsRender<MCanvasGrid>(
            GridSizeProperty,
            GridBrushProperty,
            BackgroundBrushProperty,
            ShowGridProperty);
    }

    /// <summary>
    /// Updates the current offset and zoom. Called by MCanvas on every transform change.
    /// </summary>
    public void UpdateViewport(Point offset, double zoom)
    {
        _offset = offset;
        _zoom = zoom;
        InvalidateVisual();
    }

    public override void Render(DrawingContext context)
    {
        var w = Bounds.Width;
        var h = Bounds.Height;
        if (w <= 0 || h <= 0) return;

        var screenRect = new Rect(0, 0, w, h);

        // Background fills the entire control
        if (BackgroundBrush != null)
            context.FillRectangle(BackgroundBrush, screenRect);

        if (!ShowGrid || GridBrush == null) return;

        var gridSize = GridSize;
        var screenGridSize = gridSize * _zoom;

        // Too small to see
        if (screenGridSize < 5) return;

        var minorPen = new Pen(GridBrush, 0.5);
        var majorPen = new Pen(GridBrush, 1.0);
        var majorGridSize = gridSize * 5;

        // Calculate world-space bounds of visible area
        var worldLeft = -_offset.X / _zoom;
        var worldTop = -_offset.Y / _zoom;
        var worldRight = worldLeft + w / _zoom;
        var worldBottom = worldTop + h / _zoom;

        // Minor grid lines
        var startX = Math.Floor(worldLeft / gridSize) * gridSize;
        var startY = Math.Floor(worldTop / gridSize) * gridSize;

        for (var wx = startX; wx <= worldRight; wx += gridSize)
        {
            var sx = wx * _zoom + _offset.X;
            context.DrawLine(minorPen, new(sx, 0), new(sx, h));
        }

        for (var wy = startY; wy <= worldBottom; wy += gridSize)
        {
            var sy = wy * _zoom + _offset.Y;
            context.DrawLine(minorPen, new(0, sy), new(w, sy));
        }

        // Major grid lines
        var majorStartX = Math.Floor(worldLeft / majorGridSize) * majorGridSize;
        var majorStartY = Math.Floor(worldTop / majorGridSize) * majorGridSize;

        for (var wx = majorStartX; wx <= worldRight; wx += majorGridSize)
        {
            var sx = wx * _zoom + _offset.X;
            context.DrawLine(majorPen, new(sx, 0), new(sx, h));
        }

        for (var wy = majorStartY; wy <= worldBottom; wy += majorGridSize)
        {
            var sy = wy * _zoom + _offset.Y;
            context.DrawLine(majorPen, new(0, sy), new(w, sy));
        }
    }
}
