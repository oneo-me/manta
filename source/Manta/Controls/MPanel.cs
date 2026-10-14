using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Manta.Controls.Panels;

namespace Manta.Controls;

public enum MPanelMode
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

public class MPanel : MPanelBase
{
    public static readonly StyledProperty<MPanelMode> ModeProperty =
        AvaloniaProperty.Register<MPanel, MPanelMode>(nameof(Mode));

    public MPanelMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    public static readonly StyledProperty<int> ColumnProperty =
        AvaloniaProperty.Register<MPanel, int>(nameof(Column));

    public int Column
    {
        get => GetValue(ColumnProperty);
        set => SetValue(ColumnProperty, value);
    }

    public static readonly StyledProperty<double> ColumnSizeProperty =
        AvaloniaProperty.Register<MPanel, double>(nameof(ColumnSize));

    public double ColumnSize
    {
        get => GetValue(ColumnSizeProperty);
        set => SetValue(ColumnSizeProperty, value);
    }

    static MPanel()
    {
        AffectsMeasure<MPanel>(ModeProperty, ColumnProperty, ColumnSizeProperty);
        AffectsArrange<MPanel>(ModeProperty, ColumnProperty, ColumnSizeProperty);
    }

    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
    protected override Size MeasureOverride(ReadOnlyCollection<Control> children, Size availableSize)
    {
        return Mode switch
        {
            MPanelMode.Horizontal        => HorizontalLayout.Measure(this, children, availableSize),
            MPanelMode.Vertical          => VerticalLayout.Measure(this, children, availableSize),
            MPanelMode.Wrap              => WrapLayout.Measure(this, children, availableSize),
            MPanelMode.WrapFull          => WrapLayout.Measure(this, children, availableSize),
            MPanelMode.Column            => ColumnLayout.Measure(this, children, availableSize),
            MPanelMode.Waterfall         => WaterfallLayout.Measure(this, children, availableSize, false),
            MPanelMode.WaterfallBest     => WaterfallLayout.Measure(this, children, availableSize, true),
            MPanelMode.VerticalUniform   => VerticalUniformLayout.Measure(this, children, availableSize),
            MPanelMode.HorizontalUniform => HorizontalUniformLayout.Measure(this, children, availableSize),
            _                            => throw new ArgumentOutOfRangeException(nameof(Mode))
        };
    }

    [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly")]
    protected override void ArrangeOverride(ReadOnlyCollection<Control> children, Size finalSize)
    {
        switch (Mode)
        {
            case MPanelMode.Horizontal:
                HorizontalLayout.Arrange(this, children, finalSize);
                break;

            case MPanelMode.Vertical:
                VerticalLayout.Arrange(this, children, finalSize);
                break;

            case MPanelMode.Wrap:
                WrapLayout.Arrange(this, children, finalSize);
                break;

            case MPanelMode.WrapFull:
                WrapLayout.ArrangeFull(this, children, finalSize);
                break;

            case MPanelMode.Column:
                ColumnLayout.Arrange(this, children, finalSize);
                break;

            case MPanelMode.Waterfall:
                WaterfallLayout.Arrange(this, children, finalSize, false);
                break;

            case MPanelMode.WaterfallBest:
                WaterfallLayout.Arrange(this, children, finalSize, true);
                break;

            case MPanelMode.VerticalUniform:
                VerticalUniformLayout.Arrange(this, children, finalSize);
                break;

            case MPanelMode.HorizontalUniform:
                HorizontalUniformLayout.Arrange(this, children, finalSize);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(Mode));
        }
    }
}
