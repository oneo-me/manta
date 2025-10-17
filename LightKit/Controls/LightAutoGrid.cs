using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using LightKit.Controls.Panels;

namespace LightKit.Controls;

public class LightAutoGrid : LightPanelBase
{
    public static readonly StyledProperty<ColumnDefinitions?> DefinitionsProperty =
        AvaloniaProperty.Register<LightAutoGrid, ColumnDefinitions?>(nameof(Definitions));

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
