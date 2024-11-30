using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using AirKit.Controls.Panels;
using Avalonia;
using Avalonia.Controls;

namespace AirKit.Controls;

public class LightAutoGrid : LightPanelBase
{
    public static readonly StyledProperty<ColumnDefinitions?> DefinitionsProperty =
        AvaloniaProperty.Register<LightAutoGrid, ColumnDefinitions?>(nameof(Definitions));

    [SuppressMessage("Usage", "CA2227:集合属性应为只读")]
    public ColumnDefinitions? Definitions
    {
        get => GetValue(DefinitionsProperty);
        set => SetValue(DefinitionsProperty, value);
    }

    static LightAutoGrid()
    {
        AffectsMeasure<LightAutoGrid>(DefinitionsProperty);
        AffectsArrange<LightAutoGrid>(DefinitionsProperty);
    }

    protected override Size MeasureOverride(ReadOnlyCollection<Control> children, Size availableSize)
    {
        return AutoGridLayout.Measure(this, children, availableSize);
    }

    protected override void ArrangeOverride(ReadOnlyCollection<Control> children, Size finalSize)
    {
        AutoGridLayout.Arrange(this, children, finalSize);
    }
}
