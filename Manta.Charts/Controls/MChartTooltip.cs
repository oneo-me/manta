using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Manta.Charts.Controls;

public class MChartTooltip : TemplatedControl
{
    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<MChartTooltip, string>(nameof(Header), string.Empty);

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly StyledProperty<ObservableCollection<ChartTooltipLine>> LinesProperty =
        AvaloniaProperty.Register<MChartTooltip, ObservableCollection<ChartTooltipLine>>(nameof(Lines));

    public ObservableCollection<ChartTooltipLine> Lines
    {
        get => GetValue(LinesProperty);
        set => SetValue(LinesProperty, value);
    }

    public MChartTooltip()
    {
        Lines = [];
    }
}

public class ChartTooltipLine
{
    public string Label { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
    public Color Color { get; init; }

    public IBrush ColorBrush => new SolidColorBrush(Color);
}

public class ChartTooltipAdorner : Control
{
    readonly MChartTooltip _tooltip;

    public ChartTooltipAdorner()
    {
        _tooltip = new();
        IsHitTestVisible = false;
    }

    public string Header
    {
        get => _tooltip.Header;
        set => _tooltip.Header = value;
    }

    public ObservableCollection<ChartTooltipLine> Lines
    {
        get => _tooltip.Lines;
        set => _tooltip.Lines = value;
    }

    public Point Position { get; set; }

    protected override Size MeasureOverride(Size availableSize)
    {
        _tooltip.Measure(availableSize);
        return _tooltip.DesiredSize;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var tooltipSize = _tooltip.DesiredSize;
        var pos = Position;

        var x = pos.X + 16;
        var y = pos.Y + 16;

        if (x + tooltipSize.Width > finalSize.Width - 8)
            x = pos.X - tooltipSize.Width - 8;
        if (y + tooltipSize.Height > finalSize.Height - 8)
            y = pos.Y - tooltipSize.Height - 8;
        if (x < 8)
            x = 8;
        if (y < 8)
            y = 8;

        _tooltip.Arrange(new(x, y, tooltipSize.Width, tooltipSize.Height));
        return finalSize;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        _tooltip.Render(context);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        ((ISetLogicalParent)_tooltip).SetParent(this);
        VisualChildren.Add(_tooltip);
        LogicalChildren.Add(_tooltip);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        VisualChildren.Remove(_tooltip);
        LogicalChildren.Remove(_tooltip);
        ((ISetLogicalParent)_tooltip).SetParent(null);
        base.OnDetachedFromVisualTree(e);
    }
}

public class MChartTooltipHost : ContentControl
{
    ChartTooltipAdorner? _adorner;
    AdornerLayer? _adornerLayer;

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<MChartTooltipHost, bool>(nameof(IsOpen));

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<MChartTooltipHost, string>(nameof(Header), string.Empty);

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public static readonly StyledProperty<ObservableCollection<ChartTooltipLine>> LinesProperty =
        AvaloniaProperty.Register<MChartTooltipHost, ObservableCollection<ChartTooltipLine>>(nameof(Lines));

    public ObservableCollection<ChartTooltipLine> Lines
    {
        get => GetValue(LinesProperty);
        set => SetValue(LinesProperty, value);
    }

    public static readonly StyledProperty<Point> PositionProperty =
        AvaloniaProperty.Register<MChartTooltipHost, Point>(nameof(Position));

    public Point Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    static MChartTooltipHost()
    {
        IsOpenProperty.Changed.AddClassHandler<MChartTooltipHost>((host, _) => host.UpdateAdorner());
        PositionProperty.Changed.AddClassHandler<MChartTooltipHost>((host, _) => host.UpdateAdorner());
        HeaderProperty.Changed.AddClassHandler<MChartTooltipHost>((host, _) => host.UpdateAdorner());
        LinesProperty.Changed.AddClassHandler<MChartTooltipHost>((host, _) => host.UpdateAdorner());
    }

    public MChartTooltipHost()
    {
        Lines = [];
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _adornerLayer = AdornerLayer.GetAdornerLayer(this);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        RemoveAdorner();
        _adornerLayer = null;
        base.OnDetachedFromVisualTree(e);
    }

    void UpdateAdorner()
    {
        if (_adornerLayer is null) return;

        if (IsOpen)
        {
            if (_adorner is null)
            {
                _adorner = new();
                AdornerLayer.SetAdornedElement(_adorner, this);
                _adornerLayer.Children.Add(_adorner);
            }

            _adorner.Header = Header;
            _adorner.Lines = Lines;
            _adorner.Position = Position;
            _adorner.InvalidateMeasure();
            _adorner.InvalidateArrange();
        }
        else
        {
            RemoveAdorner();
        }
    }

    void RemoveAdorner()
    {
        if (_adorner is not null && _adornerLayer is not null)
        {
            _adornerLayer.Children.Remove(_adorner);
            _adorner = null;
        }
    }
}
