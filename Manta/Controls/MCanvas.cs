using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Manta.Controls;

public class MCanvas : ContentControl
{
    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<MCanvas, double>(nameof(Zoom), 1.0,
            coerce: (o, v) => Math.Clamp(v, ((MCanvas)o).MinZoom, ((MCanvas)o).MaxZoom));

    public static readonly StyledProperty<Point> OffsetProperty =
        AvaloniaProperty.Register<MCanvas, Point>(nameof(Offset));

    public static readonly StyledProperty<double> GridSizeProperty =
        AvaloniaProperty.Register<MCanvas, double>(nameof(GridSize), 20.0);

    public static readonly StyledProperty<double> MinZoomProperty =
        AvaloniaProperty.Register<MCanvas, double>(nameof(MinZoom), 0.1);

    public static readonly StyledProperty<double> MaxZoomProperty =
        AvaloniaProperty.Register<MCanvas, double>(nameof(MaxZoom), 10.0);

    public static readonly StyledProperty<bool> ShowGridProperty =
        AvaloniaProperty.Register<MCanvas, bool>(nameof(ShowGrid), true);

    public static readonly StyledProperty<IBrush?> GridBrushProperty =
        AvaloniaProperty.Register<MCanvas, IBrush?>(nameof(GridBrush));

    /// <summary>
    ///     Base zoom factor per unit scroll delta. Actual factor = Math.Pow(ZoomBase, delta).
    /// </summary>
    const double ZoomBase = 1.1;

    const double PanSpeed = 50;

    MCanvasGrid? _grid;
    Panel? _transformContainer;
    bool _isPanning;
    Point _lastPanPosition;
    bool _isSpaceHeld;

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

    public double GridSize
    {
        get => GetValue(GridSizeProperty);
        set => SetValue(GridSizeProperty, value);
    }

    public double MinZoom
    {
        get => GetValue(MinZoomProperty);
        set => SetValue(MinZoomProperty, value);
    }

    public double MaxZoom
    {
        get => GetValue(MaxZoomProperty);
        set => SetValue(MaxZoomProperty, value);
    }

    public bool ShowGrid
    {
        get => GetValue(ShowGridProperty);
        set => SetValue(ShowGridProperty, value);
    }

    public IBrush? GridBrush
    {
        get => GetValue(GridBrushProperty);
        set => SetValue(GridBrushProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        // Measure children so they layout properly, but don't use content size
        // for this control's own size. An infinite canvas should fill available space,
        // not shrink to its content (e.g. Canvas reports 0x0).
        base.MeasureOverride(availableSize);
        return new Size(
            double.IsInfinity(availableSize.Width) ? 0 : availableSize.Width,
            double.IsInfinity(availableSize.Height) ? 0 : availableSize.Height);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _grid = e.NameScope.Find<MCanvasGrid>("PART_Grid");
        _transformContainer = e.NameScope.Find<Panel>("PART_TransformContainer");

        // Trackpad pinch-to-zoom
        AddHandler(Gestures.PointerTouchPadGestureMagnifyEvent, OnMagnify, RoutingStrategies.Bubble);

        UpdateTransform();
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        UpdateGrid();
    }

    // --- Scroll: pan / Ctrl+Scroll: zoom ---

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        var hasZoomModifier = e.KeyModifiers.HasFlag(KeyModifiers.Control) ||
                              e.KeyModifiers.HasFlag(KeyModifiers.Meta);

        if (hasZoomModifier)
        {
            ZoomAtPoint(e.GetPosition(this), Math.Pow(ZoomBase, e.Delta.Y));
        }
        else
        {
            Offset = new(
                Offset.X + e.Delta.X * PanSpeed,
                Offset.Y + e.Delta.Y * PanSpeed);
            UpdateTransform();
        }

        e.Handled = true;
    }

    // --- Trackpad pinch-to-zoom ---

    void OnMagnify(object? sender, PointerDeltaEventArgs e)
    {
        var zoomCenter = e.GetPosition(this);
        ZoomAtPoint(zoomCenter, 1 + e.Delta.X);
        e.Handled = true;
    }

    // --- Pan (drag) ---

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        Focus();

        var props = e.GetCurrentPoint(this).Properties;

        // Middle button always pans
        if (props.IsMiddleButtonPressed)
        {
            StartPan(e);
            return;
        }

        if (props.IsLeftButtonPressed)
            if (_isSpaceHeld || IsClickOnBackground(e.Source))
                StartPan(e);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        e.GetPosition(this);

        if (_isPanning)
        {
            var position = e.GetPosition(this);
            var delta = position - _lastPanPosition;
            Offset = new(Offset.X + delta.X, Offset.Y + delta.Y);
            _lastPanPosition = position;
            UpdateTransform();
            e.Handled = true;
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (_isPanning)
        {
            _isPanning = false;
            e.Pointer.Capture(null);
            e.Handled = true;
        }
    }

    // --- Keyboard ---

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch (e.Key)
        {
            case Key.Space:
                if (!_isSpaceHeld)
                {
                    _isSpaceHeld = true;
                    Cursor = new(StandardCursorType.Hand);
                }

                e.Handled = true;
                break;

            case Key.D1 when e.KeyModifiers.HasFlag(KeyModifiers.Meta):
                ResetView();
                e.Handled = true;
                break;

            case Key.D2 when e.KeyModifiers.HasFlag(KeyModifiers.Meta):
                FitAll();
                e.Handled = true;
                break;

            case Key.D3 when e.KeyModifiers.HasFlag(KeyModifiers.Meta):
                FitSelection();
                e.Handled = true;
                break;
        }
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        base.OnKeyUp(e);

        if (e.Key == Key.Space)
        {
            _isSpaceHeld = false;
            Cursor = Cursor.Default;
            e.Handled = true;
        }
    }

    /// <summary>
    ///     Resets zoom to 1x and offset to origin.
    /// </summary>
    public void ResetView()
    {
        Zoom = 1.0;
        Offset = default;
        UpdateTransform();
    }

    /// <summary>
    ///     Adjusts zoom and offset to fit all content within the viewport.
    /// </summary>
    public void FitAll()
    {
        if (_transformContainer == null || Content is not Control contentControl)
            return;

        contentControl.Measure(Size.Infinity);
        var contentBounds = contentControl.DesiredSize;

        if (contentBounds.Width <= 0 || contentBounds.Height <= 0)
            return;

        var viewWidth = Bounds.Width;
        var viewHeight = Bounds.Height;
        if (viewWidth <= 0 || viewHeight <= 0) return;

        const double padding = 40;
        var availableWidth = viewWidth - padding * 2;
        var availableHeight = viewHeight - padding * 2;

        var zoom = Math.Min(availableWidth / contentBounds.Width, availableHeight / contentBounds.Height);
        zoom = Math.Clamp(zoom, MinZoom, MaxZoom);

        var offsetX = (viewWidth - contentBounds.Width * zoom) / 2;
        var offsetY = (viewHeight - contentBounds.Height * zoom) / 2;

        Zoom = zoom;
        Offset = new(offsetX, offsetY);
        UpdateTransform();
    }

    /// <summary>
    ///     Adjusts zoom and offset to fit the current selection. Override in subclasses to provide selection-aware behavior.
    /// </summary>
    public virtual void FitSelection()
    {
        FitAll();
    }

    /// <summary>
    ///     Converts a point from canvas (screen) coordinates to world (content) coordinates.
    /// </summary>
    public Point CanvasToWorld(Point canvasPoint)
    {
        return new(
            (canvasPoint.X - Offset.X) / Zoom,
            (canvasPoint.Y - Offset.Y) / Zoom
        );
    }

    /// <summary>
    ///     Converts a point from world (content) coordinates to canvas (screen) coordinates.
    /// </summary>
    public Point WorldToCanvas(Point worldPoint)
    {
        return new(
            worldPoint.X * Zoom + Offset.X,
            worldPoint.Y * Zoom + Offset.Y
        );
    }

    void StartPan(PointerPressedEventArgs e)
    {
        _isPanning = true;
        _lastPanPosition = e.GetPosition(this);
        e.Pointer.Capture(this);
        e.Handled = true;
    }

    /// <summary>
    ///     Zoom toward/away from a specific screen point, keeping that point fixed.
    /// </summary>
    void ZoomAtPoint(Point screenPoint, double factor)
    {
        var worldBefore = CanvasToWorld(screenPoint);
        var newZoom = Math.Clamp(Zoom * factor, MinZoom, MaxZoom);

        if (Math.Abs(newZoom - Zoom) < 0.0001) return;

        Zoom = newZoom;
        Offset = new(
            screenPoint.X - worldBefore.X * Zoom,
            screenPoint.Y - worldBefore.Y * Zoom);
        UpdateTransform();
    }

    bool IsClickOnBackground(object? source)
    {
        var current = source as Control;
        while (current != null)
        {
            if (current == this) return true;
            if (current is MCanvasGrid) return true;
            if (current is ContentPresenter) return true;

            if (current.Parent is ContentPresenter)
                return false;

            current = current.Parent as Control;
        }

        return true;
    }

    void UpdateTransform()
    {
        if (_transformContainer == null) return;

        _transformContainer.RenderTransformOrigin = new(0, 0, RelativeUnit.Absolute);
        _transformContainer.RenderTransform = new TransformGroup
        {
            Children =
            {
                new ScaleTransform(Zoom, Zoom),
                new TranslateTransform(Offset.X, Offset.Y)
            }
        };

        UpdateGrid();
    }

    void UpdateGrid()
    {
        _grid?.UpdateViewport(Offset, Zoom);
    }
}
