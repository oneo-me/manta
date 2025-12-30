using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Manta.Charts.Models;
using Manta.Charts.Renders;

namespace Manta.Charts.Controls;

public class ChartHoverEventArgs(RoutedEvent routedEvent, object? source) : RoutedEventArgs(routedEvent, source)
{
    public int GroupIndex { get; set; } = -1;
    public Point Position { get; set; }
    public string Header { get; set; } = string.Empty;
    public ObservableCollection<ChartTooltipLine> Lines { get; set; } = [];
}

public class MChartRender : TemplatedControl
{
    public static readonly RoutedEvent<ChartHoverEventArgs> HoverChangedEvent =
        RoutedEvent.Register<MChartRender, ChartHoverEventArgs>(nameof(HoverChanged), RoutingStrategies.Bubble);

    public event EventHandler<ChartHoverEventArgs>? HoverChanged
    {
        add => AddHandler(HoverChangedEvent, value);
        remove => RemoveHandler(HoverChangedEvent, value);
    }

    static MChartRender()
    {
        AffectsRender<MChartRender>(DataProperty);
    }

    public static readonly StyledProperty<ChartDataBase?> DataProperty =
        AvaloniaProperty.Register<MChartRender, ChartDataBase?>(nameof(Data));

    public ChartDataBase? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    int _hoverGroupIndex = -1;
    readonly ChartHitTestResult _hitResult = new();

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        var pos = e.GetPosition(this);

        var oldGroup = _hoverGroupIndex;
        _hoverGroupIndex = -1;

        // Check pie hit first
        if (_hitResult.PieHitInfo.HasValue)
        {
            var pie = _hitResult.PieHitInfo.Value;
            var dx = pos.X - pie.Center.X;
            var dy = pos.Y - pie.Center.Y;
            var dist = Math.Sqrt(dx * dx + dy * dy);

            if (dist <= pie.Radius)
            {
                var angle = Math.Atan2(dy, dx) * 180 / Math.PI;
                if (angle < -90) angle += 360;

                for (var i = 0; i < pie.Angles.Length - 1; i++)
                    if (angle >= pie.Angles[i] && angle < pie.Angles[i + 1])
                    {
                        _hoverGroupIndex = i;
                        break;
                    }
            }
        }
        else
        {
            // Check rect hits
            foreach (var rect in _hitResult.HitRects)
                if (rect.Bounds.Contains(pos))
                {
                    _hoverGroupIndex = rect.GroupIndex;
                    break;
                }
        }

        if (_hoverGroupIndex != oldGroup)
        {
            InvalidateVisual();
            RaiseHoverChanged(pos);
        }
        else if (_hoverGroupIndex >= 0)
        {
            RaiseHoverChanged(pos);
        }
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        if (_hoverGroupIndex != -1)
        {
            _hoverGroupIndex = -1;
            InvalidateVisual();
            RaiseHoverChanged(default);
        }
    }

    void RaiseHoverChanged(Point pos)
    {
        var data = Data;
        var args = new ChartHoverEventArgs(HoverChangedEvent, this)
        {
            GroupIndex = _hoverGroupIndex,
            Position = pos
        };

        if (_hoverGroupIndex >= 0 && data is not null)
        {
            var lines = new ObservableCollection<ChartTooltipLine>();

            switch (data)
            {
                case BarChartData barData when _hoverGroupIndex < barData.Items.Count:
                    var item = barData.Items.ElementAt(_hoverGroupIndex);
                    args.Header = item.Category;
                    var idx = 0;
                    foreach (var v in item.Values)
                    {
                        var color = ChartRenderContext.GetColor(idx, v.CustomColor);
                        lines.Add(new() { Label = v.Name, Value = v.Value.ToString("N0"), Color = color });
                        idx++;
                    }

                    break;

                case PieChartData pieData when _hoverGroupIndex < pieData.Values.Count:
                    var pieValue = pieData.Values.ElementAt(_hoverGroupIndex);
                    var total = pieData.Values.Sum(x => x.Value);
                    var percent = total > 0 ? pieValue.Value / total * 100 : 0;
                    var pieColor = ChartRenderContext.GetColor(_hoverGroupIndex, pieValue.CustomColor);
                    args.Header = pieValue.Name;
                    lines.Add(new() { Label = "", Value = $"{pieValue.Value:N0} ({percent:F1}%)", Color = pieColor });
                    break;
            }

            args.Lines = lines;
        }

        RaiseEvent(args);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var data = Data;
        if (data is null) return;

        _hitResult.Clear();

        context.DrawRectangle(new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)), null, Bounds);

        var contentRect = new Rect(0, 0, Bounds.Width, Bounds.Height);

        if (Bounds.Width <= 0 || Bounds.Height <= 0) return;

        var ctx = new ChartRenderContext
        {
            FontFamily = FontFamily,
            Background = Background,
            Foreground = Foreground,
            Bounds = Bounds
        };

        switch (data)
        {
            case BarChartData barData:
                switch (barData.Type)
                {
                    case BarChartType.Bar:
                        BarChartRenderer.Render(context, ctx, contentRect, barData, _hoverGroupIndex, _hitResult);
                        break;

                    case BarChartType.Line:
                        LineChartRenderer.Render(context, ctx, contentRect, barData, _hoverGroupIndex, _hitResult);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

                break;

            case PieChartData pieData:
                PieChartRenderer.Render(context, ctx, contentRect, pieData, _hoverGroupIndex, _hitResult);
                break;
        }
    }
}
