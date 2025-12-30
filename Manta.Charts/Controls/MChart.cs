using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.VisualTree;
using Manta.Charts.Models;
using Manta.Controls;

namespace Manta.Charts.Controls;

public enum ChartVariant
{
    Bar,
    Line,
    Pie
}

[TemplatePart(nameof(PART_Columns), typeof(MItems))]
public class MChart : TemplatedControl
{
    public static readonly StyledProperty<object?> TitleProperty = AvaloniaProperty.Register<MChart, object?>(
        nameof(Title));

    public object? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public static readonly StyledProperty<IDataTemplate?> TitleDataTemplateProperty = AvaloniaProperty.Register<MChart, IDataTemplate?>(
        nameof(TitleDataTemplate));

    public IDataTemplate? TitleDataTemplate
    {
        get => GetValue(TitleDataTemplateProperty);
        set => SetValue(TitleDataTemplateProperty, value);
    }

    public static readonly StyledProperty<bool> ShowLegendsProperty = AvaloniaProperty.Register<MChart, bool>(
        nameof(ShowLegends), true);

    public bool ShowLegends
    {
        get => GetValue(ShowLegendsProperty);
        set => SetValue(ShowLegendsProperty, value);
    }

    public static readonly StyledProperty<MChartData?> DataProperty = AvaloniaProperty.Register<MChart, MChartData?>(
        nameof(Data));

    public MChartData? Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    MItems? PART_Columns;
    MChartToolTip? _toolTip;
    OverlayLayer? _overlayLayer;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        PART_Columns = e.NameScope.Get<MItems>(nameof(PART_Columns));

        PART_Columns.PointerEntered += ColumnsOnPointerEntered;
        PART_Columns.PointerExited += ColumnsOnPointerExited;
        PART_Columns.PointerMoved += ColumnsOnPointerMoved;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        RemoveToolTip();
    }

    void EnsureToolTip()
    {
        if (_toolTip != null) return;

        _overlayLayer = OverlayLayer.GetOverlayLayer(this);
        if (_overlayLayer == null) return;

        _toolTip = new() { Opacity = 0 };
        _overlayLayer.Children.Add(_toolTip);
    }

    void RemoveToolTip()
    {
        if (_toolTip != null && _overlayLayer != null)
        {
            _overlayLayer.Children.Remove(_toolTip);
            _toolTip = null;
            _overlayLayer = null;
        }
    }

    void ColumnsOnPointerMoved(object? sender, PointerEventArgs e)
    {
        EnsureToolTip();

        if (_toolTip == null || _overlayLayer == null)
            return;

        var point = e.GetPosition(this);
        var hitTestResult = this.InputHitTest(point);
        var column = (hitTestResult as Visual)?.GetVisualAncestors()
            .Prepend(hitTestResult as Visual)
            .OfType<MChartColumn>()
            .FirstOrDefault();

        if (column is null)
        {
            _toolTip.Opacity = 0;
            return;
        }

        if (_toolTip.Data != column.Data)
            _toolTip.Data = column.Data;

        // Convert position to overlay layer coordinates
        var overlayPos = this.TranslatePoint(point, _overlayLayer);
        if (overlayPos.HasValue)
        {
            const double offset = 15;
            const double edgePadding = 20;
            var tooltipWidth = _toolTip.Bounds.Width;
            var tooltipHeight = _toolTip.Bounds.Height;
            var overlayWidth = _overlayLayer.Bounds.Width;
            var overlayHeight = _overlayLayer.Bounds.Height;

            var x = overlayPos.Value.X + offset;
            var y = overlayPos.Value.Y + offset;

            // Adjust position if tooltip would exceed right edge (with padding)
            if (x + tooltipWidth + edgePadding > overlayWidth)
                x = overlayPos.Value.X - tooltipWidth - offset;

            // Adjust position if tooltip would exceed bottom edge (with padding)
            if (y + tooltipHeight + edgePadding > overlayHeight)
                y = overlayPos.Value.Y - tooltipHeight - offset;

            // Ensure tooltip doesn't go beyond left/top edges (with padding)
            x = Math.Max(edgePadding, x);
            y = Math.Max(edgePadding, y);

            Canvas.SetLeft(_toolTip, x);
            Canvas.SetTop(_toolTip, y);
        }

        _toolTip.Opacity = 1;
    }

    void ColumnsOnPointerExited(object? sender, PointerEventArgs pointerEventArgs)
    {
        if (_toolTip != null)
            _toolTip.Opacity = 0;
    }

    void ColumnsOnPointerEntered(object? sender, PointerEventArgs pointerEventArgs)
    {
        EnsureToolTip();
    }
}