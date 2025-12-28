using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Manta.Controls.Panels;

namespace Manta.Controls;

public enum MtPanelMode
{
    Vertical,
    Horizontal,
    Wrap,
    WrapFull,
    Column,

    Waterfall,
    WaterfallBest,

    VerticalUniform,
    HorizontalUniform
}

public class MtPanel : MtPanelBase
{
    public static readonly StyledProperty<MtPanelMode> ModeProperty =
        AvaloniaProperty.Register<MtPanel, MtPanelMode>(nameof(Mode));

    public MtPanelMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public static readonly StyledProperty<int> ColumnProperty =
        AvaloniaProperty.Register<MtPanel, int>(nameof(Column));

    public int Column
    {
        get => GetValue(ColumnProperty);
        set => SetValue(ColumnProperty, value);
    }

    public static readonly StyledProperty<double> ColumnSizeProperty =
        AvaloniaProperty.Register<MtPanel, double>(nameof(ColumnSize));

    public double ColumnSize
    {
        get => GetValue(ColumnSizeProperty);
        set => SetValue(ColumnSizeProperty, value);
    }

    static MtPanel()
    {
        AffectsMeasure<MtPanel>(ModeProperty, ColumnProperty, ColumnSizeProperty);
        AffectsArrange<MtPanel>(ModeProperty, ColumnProperty, ColumnSizeProperty);
    }

    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
    protected override Size MeasureOverride(ReadOnlyCollection<Control> children, Size availableSize)
    {
        return Mode switch
        {
            MtPanelMode.Horizontal        => HorizontalLayout.Measure(this, children, availableSize),
            MtPanelMode.Vertical          => VerticalLayout.Measure(this, children, availableSize),
            MtPanelMode.Wrap              => WrapLayout.Measure(this, children, availableSize),
            MtPanelMode.WrapFull          => WrapLayout.Measure(this, children, availableSize),
            MtPanelMode.Column            => ColumnLayout.Measure(this, children, availableSize),
            MtPanelMode.Waterfall         => WaterfallLayout.Measure(this, children, availableSize, false),
            MtPanelMode.WaterfallBest     => WaterfallLayout.Measure(this, children, availableSize, true),
            MtPanelMode.VerticalUniform   => VerticalUniformLayout.Measure(this, children, availableSize),
            MtPanelMode.HorizontalUniform => HorizontalUniformLayout.Measure(this, children, availableSize),
            _                             => throw new ArgumentOutOfRangeException(nameof(Mode))
        };
    }

    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
    protected override void ArrangeOverride(ReadOnlyCollection<Control> children, Size finalSize)
    {
        switch (Mode)
        {
            case MtPanelMode.Horizontal:
                HorizontalLayout.Arrange(this, children, finalSize);
                break;

            case MtPanelMode.Vertical:
                VerticalLayout.Arrange(this, children, finalSize);
                break;

            case MtPanelMode.Wrap:
                WrapLayout.Arrange(this, children, finalSize);
                break;

            case MtPanelMode.WrapFull:
                WrapLayout.ArrangeFull(this, children, finalSize);
                break;

            case MtPanelMode.Column:
                ColumnLayout.Arrange(this, children, finalSize);
                break;

            case MtPanelMode.Waterfall:
                WaterfallLayout.Arrange(this, children, finalSize, false);
                break;

            case MtPanelMode.WaterfallBest:
                WaterfallLayout.Arrange(this, children, finalSize, true);
                break;

            case MtPanelMode.VerticalUniform:
                VerticalUniformLayout.Arrange(this, children, finalSize);
                break;

            case MtPanelMode.HorizontalUniform:
                HorizontalUniformLayout.Arrange(this, children, finalSize);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(Mode));
        }
    }
}
