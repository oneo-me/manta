using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using LightKit.Controls.Panels;

namespace LightKit.Controls;

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
            PanelExtMode.Horizontal    => HorizontalLayout.Measure(this, children, availableSize),
            PanelExtMode.Vertical      => VerticalLayout.Measure(this, children, availableSize),
            PanelExtMode.Wrap          => WrapLayout.Measure(this, children, availableSize),
            PanelExtMode.WrapFull      => WrapLayout.Measure(this, children, availableSize),
            PanelExtMode.Column        => ColumnLayout.Measure(this, children, availableSize),
            PanelExtMode.Waterfall     => WaterfallLayout.Measure(this, children, availableSize, false),
            PanelExtMode.WaterfallBest => WaterfallLayout.Measure(this, children, availableSize, true),

            _ => throw new ArgumentOutOfRangeException(nameof(Mode))
        };
    }

    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
    protected override void ArrangeOverride(ReadOnlyCollection<Control> children, Size finalSize)
    {
        switch (Mode)
        {
            case PanelExtMode.Horizontal:
                HorizontalLayout.Arrange(this, children, finalSize);
                break;

            case PanelExtMode.Vertical:
                VerticalLayout.Arrange(this, children, finalSize);
                break;

            case PanelExtMode.Wrap:
                WrapLayout.Arrange(this, children, finalSize);
                break;

            case PanelExtMode.WrapFull:
                WrapLayout.ArrangeFull(this, children, finalSize);
                break;

            case PanelExtMode.Column:
                ColumnLayout.Arrange(this, children, finalSize);
                break;

            case PanelExtMode.Waterfall:
                WaterfallLayout.Arrange(this, children, finalSize, false);
                break;

            case PanelExtMode.WaterfallBest:
                WaterfallLayout.Arrange(this, children, finalSize, true);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(Mode));
        }
    }
}
