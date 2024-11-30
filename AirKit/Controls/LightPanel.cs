using System.Collections.ObjectModel;
using AirKit.Controls.Panels;
using Avalonia;
using Avalonia.Controls;

namespace AirKit.Controls;

public class LightPanel : LightPanelBase
{
    public static readonly StyledProperty<LightPanelMode> ModeProperty =
        AvaloniaProperty.Register<LightPanel, LightPanelMode>(nameof(Mode));

    public LightPanelMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public static readonly StyledProperty<int> ColumnProperty =
        AvaloniaProperty.Register<LightPanel, int>(nameof(Column));

    public int Column
    {
        get => GetValue(ColumnProperty);
        set => SetValue(ColumnProperty, value);
    }

    public static readonly StyledProperty<double> ColumnSizeProperty =
        AvaloniaProperty.Register<LightPanel, double>(nameof(ColumnSize));

    public double ColumnSize
    {
        get => GetValue(ColumnSizeProperty);
        set => SetValue(ColumnSizeProperty, value);
    }

    static LightPanel()
    {
        AffectsMeasure<LightPanel>(ModeProperty, ColumnProperty, ColumnSizeProperty);
        AffectsArrange<LightPanel>(ModeProperty, ColumnProperty, ColumnSizeProperty);
    }

    protected override Size MeasureOverride(ReadOnlyCollection<Control> children, Size availableSize)
    {
        return Mode switch
        {
            LightPanelMode.Horizontal    => HorizontalLayout.Measure(this, children, availableSize),
            LightPanelMode.Vertical      => VerticalLayout.Measure(this, children, availableSize),
            LightPanelMode.Wrap          => WrapLayout.Measure(this, children, availableSize),
            LightPanelMode.WrapFull      => WrapLayout.Measure(this, children, availableSize),
            LightPanelMode.Column        => ColumnLayout.Measure(this, children, availableSize),
            LightPanelMode.Waterfall     => WaterfallLayout.Measure(this, children, availableSize, false),
            LightPanelMode.WaterfallBest => WaterfallLayout.Measure(this, children, availableSize, true),

            LightPanelMode.None => DefaultMeasure(availableSize),
            _                   => DefaultMeasure(availableSize)
        };
    }

    protected override void ArrangeOverride(ReadOnlyCollection<Control> children, Size finalSize)
    {
        switch (Mode)
        {
            case LightPanelMode.Horizontal:
                HorizontalLayout.Arrange(this, children, finalSize);
                break;

            case LightPanelMode.Vertical:
                VerticalLayout.Arrange(this, children, finalSize);
                break;

            case LightPanelMode.Wrap:
                WrapLayout.Arrange(this, children, finalSize);
                break;

            case LightPanelMode.WrapFull:
                WrapLayout.ArrangeFull(this, children, finalSize);
                break;

            case LightPanelMode.Column:
                ColumnLayout.Arrange(this, children, finalSize);
                break;

            case LightPanelMode.Waterfall:
                WaterfallLayout.Arrange(this, children, finalSize, false);
                break;

            case LightPanelMode.WaterfallBest:
                WaterfallLayout.Arrange(this, children, finalSize, true);
                break;

            case LightPanelMode.None:
                DefaultArrange(finalSize);
                break;

            default:
                DefaultArrange(finalSize);
                break;
        }
    }
}
