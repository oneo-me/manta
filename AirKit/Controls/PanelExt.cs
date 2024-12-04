using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using AirKit.Controls.Panels;
using Avalonia;
using Avalonia.Controls;
using WrapPanel = AirKit.Controls.Panels.WrapPanel;

namespace AirKit.Controls;

public class PanelExt : PanelExtBase
{
    public static readonly StyledProperty<PanelExtMode> ModeProperty =
        AvaloniaProperty.Register<PanelExt, PanelExtMode>(nameof(Mode));

    public PanelExtMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public static readonly StyledProperty<int> ColumnProperty =
        AvaloniaProperty.Register<PanelExt, int>(nameof(Column));

    public int Column
    {
        get => GetValue(ColumnProperty);
        set => SetValue(ColumnProperty, value);
    }

    public static readonly StyledProperty<double> ColumnSizeProperty =
        AvaloniaProperty.Register<PanelExt, double>(nameof(ColumnSize));

    public double ColumnSize
    {
        get => GetValue(ColumnSizeProperty);
        set => SetValue(ColumnSizeProperty, value);
    }

    static PanelExt()
    {
        AffectsMeasure<PanelExt>(ModeProperty, ColumnProperty, ColumnSizeProperty);
        AffectsArrange<PanelExt>(ModeProperty, ColumnProperty, ColumnSizeProperty);
    }

    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
    protected override Size MeasureOverride(ReadOnlyCollection<Control> children, Size availableSize)
    {
        return Mode switch
        {
            PanelExtMode.Horizontal    => HorizontalPanel.Measure(this, children, availableSize),
            PanelExtMode.Vertical      => VerticalPanel.Measure(this, children, availableSize),
            PanelExtMode.Wrap          => WrapPanel.Measure(this, children, availableSize),
            PanelExtMode.WrapFull      => WrapPanel.Measure(this, children, availableSize),
            PanelExtMode.Column        => ColumnPanel.Measure(this, children, availableSize),
            PanelExtMode.Waterfall     => WaterfallPanel.Measure(this, children, availableSize, false),
            PanelExtMode.WaterfallBest => WaterfallPanel.Measure(this, children, availableSize, true),

            _ => throw new ArgumentOutOfRangeException(nameof(Mode))
        };
    }

    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
    protected override void ArrangeOverride(ReadOnlyCollection<Control> children, Size finalSize)
    {
        switch (Mode)
        {
            case PanelExtMode.Horizontal:
                HorizontalPanel.Arrange(this, children, finalSize);
                break;

            case PanelExtMode.Vertical:
                VerticalPanel.Arrange(this, children, finalSize);
                break;

            case PanelExtMode.Wrap:
                WrapPanel.Arrange(this, children, finalSize);
                break;

            case PanelExtMode.WrapFull:
                WrapPanel.ArrangeFull(this, children, finalSize);
                break;

            case PanelExtMode.Column:
                ColumnPanel.Arrange(this, children, finalSize);
                break;

            case PanelExtMode.Waterfall:
                WaterfallPanel.Arrange(this, children, finalSize, false);
                break;

            case PanelExtMode.WaterfallBest:
                WaterfallPanel.Arrange(this, children, finalSize, true);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(Mode));
        }
    }
}
